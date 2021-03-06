using DulcisX.Core.Enums;
using DulcisX.Core.Extensions;
using DulcisX.Exceptions;
using DulcisX.Helpers;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DulcisX.Hierarchy
{
    /// <summary>
    /// Represents a project within a <see cref="SolutionNode"/>.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ProjectNode : SolutionItemNode, IPhysicalNode
    {
        /// <summary>
        /// Gets the projects <see cref="IVsBuildPropertyStorage"/>.
        /// </summary>
        public IVsBuildPropertyStorage VsBuildPropertyStorage
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (!(UnderlyingHierarchy is IVsBuildPropertyStorage vsBuildPropertyStorage))
                {
                    throw new InvalidHierarchyItemException($"This item does not support the '{nameof(IVsBuildPropertyStorage)}' interface.");
                }

                return vsBuildPropertyStorage;
            }
        }

        private IVsProject _underlyingProject;

        /// <summary>
        /// Gets the native <see cref="IVsProject"/> for the current <see cref="ProjectNode"/>.
        /// </summary>
        public IVsProject UnderlyingProject
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                if (_underlyingProject is null)
                {
                    _underlyingProject = (IVsProject)UnderlyingHierarchy;
                }

                return _underlyingProject;
            }
        }

        /// <inheritdoc/>
        public ProjectNodeType ProjectNodeType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectNode"/> class.
        /// </summary>
        /// <param name="solution">The Solution in which the <see cref="ProjectNode"/> sits in.</param>
        /// <param name="hierarchy">The Hierarchy of the Project.</param>
        /// <param name="projectType">The type of the current Project.</param>
        public ProjectNode(SolutionNode solution, IVsHierarchy hierarchy, ProjectNodeType projectType = ProjectNodeType.Project) : base(solution, hierarchy, CommonNodeIds.Project)
        {
            ProjectNodeType = projectType;
        }

        /// <summary>
        /// Returns the Unique Identifier for the current <see cref="ProjectNode"/> in the <see cref="SolutionNode"/>.
        /// </summary>
        /// <returns>The Unique Identifier.</returns>
        public Guid GetGuid()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = ParentSolution.UnderlyingSolution.GetGuidOfProject(UnderlyingHierarchy, out var underlyingGuid);

            ErrorHandler.ThrowOnFailure(result);

            return underlyingGuid;
        }

        /// <inheritdoc/>
        public string GetFileName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return HierarchyUtilities.GetHierarchyProperty<string>(UnderlyingHierarchy, CommonNodeIds.Project, (int)__VSHPROPID.VSHPROPID_Name);
        }

        /// <inheritdoc/>
        public string GetFullName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingProject.GetMkDocument(ItemId, out var fullName);

            ErrorHandler.ThrowOnFailure(result);

            return fullName;
        }

        public string GetDirectoryName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return HierarchyUtilities.GetHierarchyProperty<string>(UnderlyingHierarchy, CommonNodeIds.Project, (int)__VSHPROPID.VSHPROPID_ProjectDir);
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="ProjectNode"/> is loaded.
        /// </summary>
        /// <returns><see langword="true"/> if the <see cref="ProjectNode"/> is loaded; otherwise <see langword="false"/>.</returns>
        public bool IsLoaded()
        {
            return !HierarchyUtilities.IsStubHierarchy(UnderlyingHierarchy);
        }

        /// <summary>
        /// Returns the Project output type for the current <see cref="ProjectNode"/>.
        /// </summary>
        /// <returns>An <see cref="__VSPROJOUTPUTTYPE"/> enumeration with the current value.</returns>
        public __VSPROJOUTPUTTYPE GetOutputTypeAction()
            => HierarchyUtilities.GetHierarchyProperty(UnderlyingHierarchy, ItemId, (int)__VSHPROPID5.VSHPROPID_OutputType, obj => (__VSPROJOUTPUTTYPE)Unbox.AsInt32(obj));

        /// <summary>
        /// Sets the current Project output type.
        /// </summary>
        /// <param name="outputType">The <see cref="__VSPROJOUTPUTTYPE"/> settings.</param>
        public void SetOutputTypeAction(__VSPROJOUTPUTTYPE outputType)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = UnderlyingHierarchy.SetProperty(ItemId, (int)__VSHPROPID5.VSHPROPID_OutputType, (int)outputType);
            ErrorHandler.ThrowOnFailure(result);
        }

        /// <summary>
        /// Returns the value of the given <see cref="DocumentProperty"/> on a <see cref="ProjectItemNode"/>.
        /// </summary>
        /// <param name="itemId">The Unique Identifier of the <see cref="ProjectItemNode"/>.</param>
        /// <param name="documentProperty">The property of which its value should be retrieved.</param>
        /// <returns>The value of the <see cref="DocumentProperty"></see>.</returns>
        public string GetItemProperty(uint itemId, DocumentProperty documentProperty)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = VsBuildPropertyStorage.GetItemAttribute(itemId, documentProperty.ToString(), out var val);

            ErrorHandler.ThrowOnFailure(result);

            return val;
        }

        /// <summary>
        /// Sets the value of the given <see cref="DocumentProperty"/> on a <see cref="ProjectItemNode"/>.
        /// </summary>
        /// <param name="itemId">The Unique Identifier of the <see cref="ProjectItemNode"/>.</param>
        /// <param name="documentProperty">The property of which its value should be retrieved.</param>
        /// <param name="value">The string value to which the specified <paramref name="documentProperty"/> should change to.</param>
        public void SetItemProperty(uint itemId, DocumentProperty documentProperty, string value)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = VsBuildPropertyStorage.SetItemAttribute(itemId, documentProperty.ToString(), value);

            ErrorHandler.ThrowOnFailure(result);
        }

        /// <inheritdoc/>
        public override IEnumerable<BaseNode> GetChildren()
        {
            var node = HierarchyUtilities.GetFirstChild(UnderlyingHierarchy, ItemId, true);

            while (!VsHelper.IsItemIdNil(node))
            {
                var child = NodeFactory.GetProjectItemNode(ParentSolution, this, UnderlyingHierarchy, node);

                if (!(child is UnknownNode))
                {
                    yield return child;
                }

                node = HierarchyUtilities.GetNextSibling(UnderlyingHierarchy, node, true);
            }
        }

        /// <summary>
        /// Creates a new <see cref="DocumentNode"/> in the current <see cref="ProjectNode"/>-
        /// </summary>
        /// <param name="name">The name of the new <see cref="DocumentNode"/>.</param>
        /// <returns>An <see cref="VSADDRESULT"/> enumeration which indicates if the operation succeeded.</returns>
        public VSADDRESULT CreateDocument(string name)
            => CreateDocument(this.ItemId, name);

        /// <summary>
        /// Creates a new <see cref="DocumentNode"/> in the current <see cref="ProjectNode"/>-
        /// </summary>
        /// <param name="parentNode">The <see cref="FolderNode"/> in which the new <see cref="DocumentNode"/> should be placed in.</param>
        /// <param name="name">The name of the new <see cref="DocumentNode"/>.</param>
        /// <returns>An <see cref="VSADDRESULT"/> enumeration which indicates if the operation succeeded.</returns>
        public VSADDRESULT CreateDocument(FolderNode parentNode, string name)
            => CreateDocument(parentNode.ItemId, name);

        private VSADDRESULT CreateDocument(uint parentId, string name)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var addResult = new VSADDRESULT[1];

            var result = UnderlyingProject.AddItem(parentId, VSADDITEMOPERATION.VSADDITEMOP_CLONEFILE, name, 1, new string[] { Path.GetTempFileName() }, IntPtr.Zero, addResult);

            ErrorHandler.ThrowOnFailure(result);

            return addResult[0];
        }

        /// <summary>
        /// Adds a file which exists on disk to the current <see cref="ProjectNode"/>-
        /// </summary>
        /// <param name="fullName">The full name of the file on disk.</param>
        /// <returns>An <see cref="VSADDRESULT"/> enumeration which indicates if the operation succeeded.</returns>
        public VSADDRESULT AddExistingDocument(string fullName)
            => AddExistingDocument(this.ItemId, fullName);

        /// <summary>
        /// Adds a file which exists on disk to the current <see cref="ProjectNode"/>-
        /// </summary>
        /// <param name="parentNode">The <see cref="FolderNode"/> in which the file should be placed in.</param>
        /// <param name="fullName">The full name of the file on disk.</param>
        /// <returns>An <see cref="VSADDRESULT"/> enumeration which indicates if the operation succeeded.</returns>
        public VSADDRESULT AddExistingDocument(FolderNode parentNode, string fullName)
            => AddExistingDocument(parentNode.ItemId, fullName);

        internal VSADDRESULT AddExistingDocument(uint parentId, string fullName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!File.Exists(fullName))
            {
                throw new FileNotFoundException("The method couldn't find the specified file.", fullName);
            }

            VSADDRESULT[] addResult = new VSADDRESULT[1];

            var result = UnderlyingProject.AddItem(parentId, VSADDITEMOPERATION.VSADDITEMOP_CLONEFILE, Path.GetFileName(fullName), 1, new string[] { fullName }, IntPtr.Zero, addResult);

            ErrorHandler.ThrowOnFailure(result);

            return addResult[0];
        }

        public ValueTask<bool> ModifyAndWaitAsync(Action<ModifyProjectDocument> modifier, TimeSpan? timeout = null)
            => ModifyBase(modifier).SaveChangesAsync(timeout);

        public void Modify(Action<ModifyProjectDocument> modifier)
            => ModifyBase(modifier).SaveChanges();

        private ModifyProjectDocument ModifyBase(Action<ModifyProjectDocument> modifier)
        {
            if (modifier is null)
            {
                throw new ArgumentNullException(nameof(modifier));
            }

            var modifyingProjectNode = new ModifyProjectDocument(this);

            modifier.Invoke(modifyingProjectNode);

            return modifyingProjectNode;
        }

        /// <summary>
        /// Removes a <see cref="ProjectItemNode"/> from the current <see cref="ProjectNode"/> and removes it physically from the disk. A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="node">The <see cref="ProjectItemNode"/> which should be removed.</param>
        /// <param name="errorCode">The error code, if the operation did not succeed, otherwise 0.</param>
        /// <returns>A return value indicates whether the operation succeeded.</returns>
        public bool TryRemoveChildren(ProjectItemNode node, out int errorCode)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var project = (IVsProject2)UnderlyingProject;

            errorCode = project.RemoveItem(0, node.ItemId, out var success);

            return VsConverter.AsBoolean(success) && ErrorHandler.Succeeded(errorCode);
        }

        /// <summary>
        /// Removes a <see cref="ProjectItemNode"/> from the current <see cref="ProjectNode"/> and removes it physically from the disk. A return value indicates whether the operation succeeded.
        /// </summary>
        /// <typeparam name="TNode">The physical node which represents the <paramref name="fullName"/>.</typeparam>
        /// <param name="fullName">A string containg the full name aka. the document moniker.</param>
        /// <param name="node">The <see cref="IPhysicalNode"/> with the given <paramref name="fullName"/>.</param>
        /// <returns>A return value indicates whether the operation succeeded.</returns>
        public bool TryGetPhysicalNode<TNode>(string fullName, out TNode node) where TNode : class, IPhysicalProjectItemNode
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var priority = new VSDOCUMENTPRIORITY[1];

            var result = UnderlyingProject.IsDocumentInProject(fullName, out var found, priority, out var item);

            ErrorHandler.ThrowOnFailure(result);

            if (!VsConverter.AsBoolean(found))
            {
                node = null;
                return false;
            }

            node = NodeFactory.GetProjectItemNode(ParentSolution, this, this.UnderlyingHierarchy, item) as TNode;

            return node is object;
        }

        public bool ContainsPhysicalNode(string fullName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var priority = new VSDOCUMENTPRIORITY[1];

            var result = UnderlyingProject.IsDocumentInProject(fullName, out var found, priority, out _);

            ErrorHandler.ThrowOnFailure(result);

            return VsConverter.AsBoolean(found);
        }

        public string GetRelativePath(IPhysicalProjectItemNode node)
        {
            var nodePath = node.GetFullName();

            return GetRelativePath(nodePath);
        }

        internal string GetRelativePath(string fullName)
        {
            var rootPath = this.GetDirectoryName();

            if (fullName.StartsWith(rootPath, StringComparison.Ordinal))
            {
                return fullName.Substring(rootPath.Length).TrimStart('\\');
            }
            else
            {
                throw new ArgumentException("The provided node is not a child of the Project.", nameof(fullName));
            }
        }

        public DocumentNode MoveNodeInsideProject(FolderNode destination, DocumentNode node)
            => MoveNodeInsideProject(node, Path.Combine(destination.GetFullName(), node.GetFileName()));
        internal DocumentNode MoveNodeInsideProject(DocumentNode node, string newFullName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var fullName = node.GetFullName();

            File.Move(fullName, newFullName);

            if (!this.TryRemoveChildren(node, out var code))
            {
                ErrorHandler.ThrowOnFailure(code);
            }

            var parentId = this.GetParentNodeId();

            var success = this.AddExistingDocument(parentId, newFullName);

            if (success != VSADDRESULT.ADDRESULT_Success)
            {
                throw new OperationNotSuccessfulException($"Couldn't re-add the file to the project. AddResult: '{success}'.");
            }

            this.TryGetPhysicalNode<DocumentNode>(newFullName, out var document);

            return document;
        }

        public ProjectNodeReferences GetReferences()
            => new ProjectNodeReferences(this);

        public string GetTargetFramework()
        {
            return UnderlyingHierarchy.GetProperty<string>(CommonNodeIds.Project, (int)__VSHPROPID4.VSHPROPID_TargetFrameworkMoniker);
        }

        public string GetTargetPlatform()
        {
            return UnderlyingHierarchy.GetProperty<string>(CommonNodeIds.Project, (int)__VSHPROPID5.VSHPROPID_TargetPlatformIdentifier);
        }

        public __VSPROJTARGETRUNTIME GetTargetRuntime()
        {
            return (__VSPROJTARGETRUNTIME)UnderlyingHierarchy.GetProperty(CommonNodeIds.Project, (int)__VSHPROPID5.VSHPROPID_TargetRuntime);
        }
    }
}
