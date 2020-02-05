using DulcisX.Core.Extensions;
using DulcisX.Core.Models;
using DulcisX.Core.Models.Enums;
using DulcisX.Helpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DulcisX.Components
{
    public class HierarchyItemX : HierarchyPropertiesX, IEnumerable<HierarchyItemX>
    {
        public string FullName => GetFullName();

        public bool ContainsItems => IsContainer && this.Any();

        public bool IsContainer => UnderlyingHierarchy.IsContainer(ItemId);

        public bool HasParent => ParentItem != null;

        private HierarchyItemX _parentItem;

        public HierarchyItemX ParentItem
        {
            get
            {
                if (_parentItem is null && ItemType != HierarchyItemTypeX.Solution)
                {
                    _parentItem = GetParent();
                }

                return _parentItem;
            }
            internal set => _parentItem = value;
        }

        private ProjectX _parentProject;

        public ProjectX ParentProject
        {
            get
            {
                if (_parentProject is null &&
                    (ItemType == HierarchyItemTypeX.Document ||
                    ItemType == HierarchyItemTypeX.Folder))
                {
                    _parentProject = GetFirstParent(HierarchyItemTypeX.Project).AsProject();
                }

                return _parentProject;
            }
            internal set => _parentProject = value;
        }

        public SolutionX ParentSolution { get; }

        public HierarchyItemTypeX ItemType { get; }

        internal HierarchyItemX(IVsHierarchy underlyingHierarchy, uint itemId, HierarchyItemTypeX itemType, ConstructorInstance<SolutionX> solutionInstance, ConstructorInstance<ProjectX> projectInstance, HierarchyItemX parentItem = default) : base(underlyingHierarchy, itemId)
        {
            ItemType = itemType;
            ParentItem = parentItem;
            ParentSolution = solutionInstance.GetInstance(this);
            ParentProject = projectInstance.GetInstance(this);
        }

        public SolutionX AsSolution()
        {
            VsHelper.ValidateHierarchyType(ItemType, HierarchyItemTypeX.Solution);

            return (SolutionX)this;
        }

        public ProjectX AsProject()
        {
            VsHelper.ValidateHierarchyType(ItemType, HierarchyItemTypeX.Project);

            return new ProjectX(this.UnderlyingHierarchy, ItemId, ParentSolution);
        }

        public DocumentX AsDocument()
        {
            VsHelper.ValidateHierarchyType(ItemType, HierarchyItemTypeX.Document);

            return (DocumentX)this;
        }

        public IEnumerator<HierarchyItemX> GetEnumerator()
        {
            if (ItemType == HierarchyItemTypeX.Document)
                yield break;

            ThreadHelper.ThrowIfNotOnUIThread();

            var node = UnderlyingHierarchy.GetProperty(ItemId, (int)__VSHPROPID.VSHPROPID_FirstVisibleChild);

            do
            {
                if (VsHelper.IsItemIdNil(node))
                {
                    break;
                }

                if (ItemType == HierarchyItemTypeX.Solution ||
                    ItemType == HierarchyItemTypeX.VirtualFolder)
                {
                    if (UnderlyingHierarchy.TryGetNestedHierarchy(node, out var hierarchy))
                    {
                        var type = hierarchy.GetHierarchyItemType(VSConstants.VSITEMID_ROOT);

                        yield return new HierarchyItemX(hierarchy, VSConstants.VSITEMID_ROOT, type, ConstructorInstance.FromValue(ParentSolution), ConstructorInstance.FromValue(ParentProject), this);
                    }
                }
                else
                {
                    var type = UnderlyingHierarchy.GetHierarchyItemType(node);

                    yield return new HierarchyItemX(UnderlyingHierarchy, node, type, ConstructorInstance.FromValue(ParentSolution), ConstructorInstance.FromValue(ParentProject), this);
                }

                node = UnderlyingHierarchy.GetProperty(node, (int)__VSHPROPID.VSHPROPID_NextVisibleSibling);
            }
            while (true);
        }

        public HierarchyItemX GetParent()
        {
            if (ItemType == HierarchyItemTypeX.Solution)
            {
                return null;
            }

            var parentItemId = UnderlyingHierarchy.GetProperty(ItemId, (int)__VSHPROPID.VSHPROPID_Parent);

            if (parentItemId > VSConstants.VSITEMID_ROOT)
                parentItemId = VSConstants.VSITEMID_ROOT;

            HierarchyItemTypeX itemType;

            IVsHierarchy tempHierarchy = null;

            if (parentItemId == VSConstants.VSITEMID_ROOT &&
               ItemId == VSConstants.VSITEMID_ROOT)
            {
                tempHierarchy = UnderlyingHierarchy.GetProperty<IVsHierarchy>(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ParentHierarchy);

                itemType = tempHierarchy.GetHierarchyItemType(parentItemId);
            }
            else
            {
                itemType = UnderlyingHierarchy.GetHierarchyItemType(parentItemId);
            }

            tempHierarchy = tempHierarchy ?? UnderlyingHierarchy;

            return new HierarchyItemX(tempHierarchy, parentItemId, itemType, ConstructorInstance.FromValue(ParentSolution), ConstructorInstance.Empty<ProjectX>());
        }

        public HierarchyItemX GetFirstParent(HierarchyItemTypeX itemType)
        {
            HierarchyItemX previousParent = ParentItem;

            while (true)
            {
                if (previousParent is null)
                {
                    return null;
                }
                else if (previousParent.ItemType == itemType)
                {
                    return previousParent;
                }

                previousParent = previousParent.ParentItem;
            }
        }

        private string GetFullName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            string fullName = null;
            int result = 0;

            switch (ItemType)
            {
                case HierarchyItemTypeX.Document:
                case HierarchyItemTypeX.Folder:
                    result = ParentProject.UnderlyingProject.GetMkDocument(ItemId, out fullName);
                    break;
                case HierarchyItemTypeX.Project:
                    result = AsProject().UnderlyingProject.GetMkDocument(ItemId, out fullName);
                    break;
                case HierarchyItemTypeX.Solution:
                    result = AsSolution().UnderlyingSolution.GetProperty((int)__VSPROPID.VSPROPID_SolutionFileName, out var tempPath);
                    fullName = (string)tempPath;
                    break;
                case HierarchyItemTypeX.VirtualFolder:
                    fullName = null;
                    break;
            }

            VsHelper.ValidateSuccessStatusCode(result);

            return fullName;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
