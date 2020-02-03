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
        public bool ContainsItems => ItemType != HierarchyItemTypeX.Document && this.Any();

        public bool HasParent => ParentItem != null;

        private HierarchyItemX _parentItem;

        public HierarchyItemX ParentItem
        {
            get
            {
                if (_parentItem is null)
                {
                    _parentItem = GetParent();
                }

                return _parentItem;
            }
            internal set => _parentItem = value;
        }

        public HierarchyItemTypeX ItemType { get; }

        protected HierarchyItemX(IVsHierarchy underlyingHierarchy, uint itemId, HierarchyItemTypeX itemType) : base(underlyingHierarchy, itemId)
            => ItemType = itemType;

        protected HierarchyItemX(IVsHierarchy underlyingHierarchy, HierarchyItemX parentItem, uint itemId, HierarchyItemTypeX itemType) : this(underlyingHierarchy, itemId, itemType)
            => _parentItem = parentItem;

        public SolutionX AsSolution()
        {
            VsHelper.ValidateHierarchyType(ItemType, HierarchyItemTypeX.Solution);

            return (SolutionX)this;
        }

        public ProjectX AsProject()
        {
            VsHelper.ValidateHierarchyType(ItemType, HierarchyItemTypeX.Project);

            return (ProjectX)this;
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
                if (VsHelper.IsNil(node))
                {
                    break;
                }

                if (ItemType == HierarchyItemTypeX.Solution ||
                    ItemType == HierarchyItemTypeX.VirtualFolder)
                {
                    if (UnderlyingHierarchy.TryGetNestedHierarchy(node, out var hierarchy))
                    {
                        var type = hierarchy.IsProject(VSConstants.VSITEMID_ROOT) ? HierarchyItemTypeX.Project : HierarchyItemTypeX.VirtualFolder;

                        yield return new HierarchyItemX(hierarchy, this, VSConstants.VSITEMID_ROOT, type);
                    }
                }
                else
                {
                    var isFolder = UnderlyingHierarchy.IsFolder(node);

                    yield return new HierarchyItemX(UnderlyingHierarchy, this, node, isFolder ? HierarchyItemTypeX.Folder : HierarchyItemTypeX.Document);
                }

                node = UnderlyingHierarchy.GetProperty(node, (int)__VSHPROPID.VSHPROPID_NextVisibleSibling);
            }
            while (true);
        }

        public HierarchyItemX GetParent(IVsHierarchy parentHierarchy = null)
        {
            if (ItemType == HierarchyItemTypeX.Solution)
            {
                return null;
            }

            var parentItemId = UnderlyingHierarchy.GetProperty(ItemId, (int)__VSHPROPID.VSHPROPID_Parent);

            HierarchyItemTypeX itemType;

            if (ItemType == HierarchyItemTypeX.VirtualFolder ||
                ItemType == HierarchyItemTypeX.Project)
            {
                itemType = HierarchyItemTypeX.VirtualFolder;
            }
            else if (parentHierarchy is null && !UnderlyingHierarchy.TryGetProperty(ItemId, (int)__VSHPROPID.VSHPROPID_ParentHierarchy, out parentHierarchy))
            {
                itemType = HierarchyItemTypeX.Solution;
            }
            else if (parentHierarchy.IsProject(parentItemId))
            {
                itemType = HierarchyItemTypeX.Project;
            }
            else
            {
                var isFolder = UnderlyingHierarchy.IsFolder(parentItemId);
                itemType = isFolder ? HierarchyItemTypeX.Folder : HierarchyItemTypeX.Document;
            }

            return new HierarchyItemX(parentHierarchy, parentItemId, itemType);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
