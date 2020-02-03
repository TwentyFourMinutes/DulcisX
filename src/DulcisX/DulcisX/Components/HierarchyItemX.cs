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

        public HierarchyItemX ParentItem { get; }

        public HierarchyItemTypeX ItemType { get; }

        protected HierarchyItemX(IVsHierarchy underlyingHierarchy, HierarchyItemX parentItem, uint itemId, HierarchyItemTypeX itemType) : base(underlyingHierarchy, itemId)
            => (ParentItem, ItemType) = (parentItem, itemType);

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

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
