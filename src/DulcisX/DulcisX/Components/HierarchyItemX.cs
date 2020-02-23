using DulcisX.Core.Extensions;
using DulcisX.Core.Models;
using DulcisX.Core.Models.Enums;
using DulcisX.Helpers;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DulcisX.Components
{
    public class HierarchyItemX : HierarchyPropertiesX, IEnumerable<HierarchyItemX>, IEquatable<HierarchyItemX>
    {
        public string FullName => GetFullName();

        public bool ContainsItems => IsContainer && this.Any();

        public bool IsContainer => UnderlyingHierarchy.IsContainer(ItemId);

        #region Parents

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
                    _parentProject = new ProjectX(UnderlyingHierarchy, VSConstants.VSITEMID_ROOT, ParentSolution);
                }

                return _parentProject;
            }
            internal set => _parentProject = value;
        }

        public SolutionX ParentSolution { get; }

        #endregion

        public HierarchyItemTypeX ItemType { get; }

        internal HierarchyItemX(IVsHierarchy underlyingHierarchy, uint itemId, HierarchyItemTypeX itemType, ConstructorInstance<SolutionX> solutionInstance, ConstructorInstance<ProjectX> projectInstance, HierarchyItemX parentItem = default) : base(underlyingHierarchy, itemId)
        {
            ItemType = itemType;
            ParentItem = parentItem;
            ParentSolution = solutionInstance.GetInstance(this);
            ParentProject = projectInstance.GetInstance(this);
        }

        #region Helper Methods

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

        public HierarchyItemX GetParent()
        {
            if (ItemType == HierarchyItemTypeX.Solution)
            {
                return null;
            }

            var parentItemId = GetProperty((int)__VSHPROPID.VSHPROPID_Parent);

            if (parentItemId > VSConstants.VSITEMID_ROOT)
                parentItemId = VSConstants.VSITEMID_ROOT;

            IVsHierarchy tempHierarchy;

            if (parentItemId == VSConstants.VSITEMID_ROOT &&
               ItemId == VSConstants.VSITEMID_ROOT)
            {
                tempHierarchy = UnderlyingHierarchy.GetProperty<IVsHierarchy>(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ParentHierarchy);
            }
            else
            {
                tempHierarchy = UnderlyingHierarchy;
            }

            return tempHierarchy.ConstructHierarchyItem(parentItemId, ParentSolution);
        }

        public HierarchyItemX GetFirstParent(HierarchyItemTypeX itemType)
        {
            HierarchyItemX previousParent = ParentItem;

            if (itemType == HierarchyItemTypeX.Project)
            {
                return ParentProject;
            }
            else if (itemType == HierarchyItemTypeX.Solution)
            {
                return ParentSolution;
            }

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

        #endregion

        #region Enumerable

        public IEnumerator<HierarchyItemX> GetEnumerator()
        {
            if (ItemType == HierarchyItemTypeX.Document)
                yield break;

            ThreadHelper.ThrowIfNotOnUIThread();

            var node = GetProperty((int)__VSHPROPID.VSHPROPID_FirstVisibleChild);

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
                        yield return hierarchy.ConstructHierarchyItem(VSConstants.VSITEMID_ROOT, ParentSolution, null, ParentProject, this);
                    }
                }
                else
                {
                    yield return UnderlyingHierarchy.ConstructHierarchyItem(node, ParentSolution, null, ParentProject, this);
                }

                node = UnderlyingHierarchy.GetProperty(node, (int)__VSHPROPID.VSHPROPID_NextVisibleSibling);
            }
            while (true);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        #endregion

        #region Equality Comparison

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        public bool Equals(HierarchyItemX other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.FullName == other.FullName;
        }

        public static bool operator ==(HierarchyItemX hierarchyItem1, HierarchyItemX hierarchyItem2)
        {
            if (hierarchyItem1 is null && hierarchyItem2 is null)
            {
                return true;
            }

            if ((hierarchyItem1 is null && !(hierarchyItem2 is null)) ||
                (hierarchyItem2 is null && !(hierarchyItem1 is null)))
                return false;

            if (ReferenceEquals(hierarchyItem1, hierarchyItem2))
            {
                return true;
            }

            return hierarchyItem1.FullName == hierarchyItem2.FullName;
        }

        public static bool operator !=(HierarchyItemX hierarchyItem1, HierarchyItemX hierarchyItem2)
        {
            return !(hierarchyItem1 == hierarchyItem2);
        }

        #endregion
    }
}
