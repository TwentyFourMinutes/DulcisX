using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DulcisX.Nodes
{
    public class ModifyProjectDocument
    {
        private XDocument _projectDocument;

        public XDocument ProjectDocument
        {
            get
            {
                if (_projectDocument is null)
                {

                    using (var reader = XmlReader.Create(_fullName, new XmlReaderSettings
                    {
                        Async = true,
                        DtdProcessing = DtdProcessing.Ignore,
                        IgnoreWhitespace = true,
                        CheckCharacters = false,
                        IgnoreComments = false,
                        CloseInput = true
                    }))
                    {
                        _projectDocument = XDocument.Load(reader, LoadOptions.None);
                    }
                }

                return _projectDocument;
            }
        }

        private XElement _rootNode;
        public XElement RootNode
        {
            get
            {
                if (_rootNode is null)
                {
                    _rootNode = ProjectDocument.Root;
                }

                return _rootNode;
            }
        }

        public ProjectNode ProjectNode { get; }

        public bool IsDocumentDirty { get; private set; }

        internal string LastFullNameChanged { get; private set; }

        internal IPhysicalProjectItemNode LastNodeChanged { get; private set; }

        private readonly string _fullName;

        internal ModifyProjectDocument(ProjectNode project)
        {
            ProjectNode = project;
            _fullName = project.GetFullName();
        }

        public FolderNode IncludeFolder(string fullName)
        {
            if (!Directory.Exists(fullName))
            {
                throw new DirectoryNotFoundException("The specified directory could not be found.");
            }
            else if (ProjectNode.ContainsPhysicalNode(fullName))
            {
                return null;
            }

            var folderName = ProjectNode.GetRelativePath(fullName.TrimEnd('\\'));

            var includingName = folderName + @"\";
            var excludingName = folderName + @"\**";

            MakeDocumentStateDirty();

            NodeChanges(fullName);

            return IncludePhysicalNode<FolderNode>("Folder", fullName, includingName, excludingName);
        }

        private TNode IncludePhysicalNode<TNode>(string elemType, string fullName, string includingName, string excludingName) where TNode : class, IPhysicalProjectItemNode
        {
            foreach (var itemGroup in RootNode.Elements("ItemGroup"))
            {
                if (RemoveExclude(itemGroup, excludingName))
                {
                    break;
                }
            }

            var group = RootNode.Elements("ItemGroup").FirstOrDefault(x => x.Elements().Any(y => y.Name == elemType));

            if (group is null)
            {
                group = new XElement("ItemGroup");
                RootNode.Add(group);
            }

            group.Add(new XElement(elemType, new XAttribute("Include", includingName)));

            ProjectNode.TryGetPhysicalNode<TNode>(fullName, out var node);

            return node;
        }

        private bool RemoveInclude(XElement parentGroup, string elemType, string includeName)
        {
            var includingElements = parentGroup.Elements().FirstOrDefault(x =>
            {
                if (x.Name == elemType)
                    return false;

                var attr = x.Attribute("Include");

                return attr is object && attr.Value == includeName;
            });

            if (includingElements is object)
            {
                includingElements.Remove();

                if (!parentGroup.HasElements)
                {
                    parentGroup.Remove();
                }

                return true;
            }

            return false;
        }

        public void ExcludeFolderNode(FolderNode node)
        {
            var folderName = ProjectNode.GetRelativePath(node).TrimEnd('\\');

            var includingName = folderName + @"\";
            var excludingName = folderName + @"\**";

            foreach (var itemGroup in RootNode.Elements("ItemGroup"))
            {
                if (RemoveInclude(itemGroup, "Folder", includingName))
                {
                    break;
                }
            }

            XElement group = null;

            foreach (var itemGroup in RootNode.Elements("ItemGroup"))
            {
                if (itemGroup.FirstNode is XElement firstElem && firstElem.HasAttributes && firstElem.FirstAttribute.Name == "Remove" &&
                    itemGroup.LastNode is XElement lastElem && lastElem.Name != "Compile" && lastElem.HasAttributes && lastElem.FirstAttribute.Name == "Remove")
                {
                    group = itemGroup;
                    break;
                }
            }

            if (group is null)
            {
                group = new XElement("ItemGroup");
                RootNode.Add(group);
            }

            var attr = new XAttribute("Remove", excludingName);

            group.Add(new XElement("Compile", attr));
            group.Add(new XElement("EmbeddedResource", attr));
            group.Add(new XElement("None", attr));

            MakeDocumentStateDirty();

            NodeChanges(node);
        }

        private bool RemoveExclude(XElement parentGroup, string excludingName)
        {
            var exludingElements = parentGroup.Elements().Where(x =>
            {
                var attr = x.Attribute("Remove");

                return attr is object && attr.Value == excludingName;
            }).ToList();

            if (exludingElements.Count > 0)
            {
                foreach (var exludingElement in exludingElements)
                {
                    exludingElement.Remove();
                }

                if (!parentGroup.HasElements)
                {
                    parentGroup.Remove();
                }

                return true;
            }

            return false;
        }

        public void NodeChanges(IPhysicalProjectItemNode changingNode)
        {
            LastNodeChanged = changingNode;
        }

        public void NodeChanges(string fullName)
        {
            LastFullNameChanged = fullName;
        }

        public async ValueTask<bool> SaveChangesAsync(TimeSpan? timeout = null)
        {
            if (!IsDocumentDirty)
                return false;

            if (!timeout.HasValue)
                timeout = TimeSpan.FromSeconds(10);

            ModifyHierarchyEvents events;

            if (LastNodeChanged is object)
            {
                events = ModifyHierarchyEvents.Create(LastNodeChanged, timeout.Value);
            }
            else if (!string.IsNullOrWhiteSpace(LastFullNameChanged))
            {
                events = ModifyHierarchyEvents.Create(ProjectNode, LastFullNameChanged, timeout.Value);
            }
            else
            {
                SaveChanges();

                return false;
            }


            SaveChanges();

            await events.Semaphore.WaitAsync();

            return events.OperationSuccessful;
        }

        public void SaveChanges()
        {
            if (!IsDocumentDirty)
                return;

            ProjectNode.SaveAllChildren();

            XmlWriterSettings writterSettings = new XmlWriterSettings
            {
                Async = true,
                Indent = true,
                OmitXmlDeclaration = true,
                CloseOutput = true
            };

            using (XmlWriter writer = XmlWriter.Create(_fullName, writterSettings))
            {
                _projectDocument.Save(writer);
            }

            IsDocumentDirty = false;
        }

        public void MakeDocumentStateDirty()
            => IsDocumentDirty = true;
    }
}
