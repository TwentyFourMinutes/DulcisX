using DulcisX.Core.Enums;
using DulcisX.Nodes.Events;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Threading;

namespace DulcisX.Nodes
{
    internal class ModifyHierarchyEvents : NodeEventSink, IVsHierarchyEvents
    {
        internal SemaphoreSlim Semaphore { get; }
        internal bool OperationSuccessful { get; private set; }

        private readonly ProjectNode _project;
        private readonly string _fullName;
        private readonly ModifyHierarchyType _modifyType;
        private readonly uint _itemId;

        private readonly Timer _cancelTimer;
        private bool _isDisposed;

        private ModifyHierarchyEvents(SemaphoreSlim semaphore, SolutionNode solution, ProjectNode project, TimeSpan duration) : base(solution)
        {
            Semaphore = semaphore;
            _project = project;
            _cancelTimer = new Timer(_ =>
            {
                _ = System.Threading.Tasks.Task.Run(async () =>
                  {
                      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                      OperationSuccessful = false;
                      this.Dispose();
                  });
            }, null, (int)duration.TotalMilliseconds, (int)duration.TotalMilliseconds);
        }

        private ModifyHierarchyEvents(SemaphoreSlim semaphore, SolutionNode solution, ProjectNode project, string fullName, TimeSpan duration) : this(semaphore, solution, project, duration)
        {
            _fullName = fullName;
            _modifyType = ModifyHierarchyType.FullName;
        }

        private ModifyHierarchyEvents(SemaphoreSlim semaphore, SolutionNode solution, ProjectNode project, uint itemId, TimeSpan duration) : this(semaphore, solution, project, duration)
        {
            _itemId = itemId;
            _modifyType = ModifyHierarchyType.ItemId;
        }

        public int OnItemAdded(uint itemidParent, uint itemidSiblingPrev, uint itemidAdded)
        {
            HandleHierarchyChange(itemidAdded);

            return CommonStatusCodes.Success;
        }

        public int OnItemsAppended(uint itemidParent)
        {
            return CommonStatusCodes.NotImplemented;
        }

        public int OnItemDeleted(uint itemid)
        {
            HandleHierarchyChange(itemid);

            return CommonStatusCodes.Success;
        }

        public int OnPropertyChanged(uint itemid, int propid, uint flags)
        {
            HandleHierarchyChange(itemid);

            return CommonStatusCodes.Success;
        }

        public int OnInvalidateItems(uint itemidParent)
        {
            return CommonStatusCodes.NotImplemented;
        }

        public int OnInvalidateIcon(IntPtr hicon)
        {
            return CommonStatusCodes.NotImplemented;
        }

        private void HandleHierarchyChange(uint itemId)
        {
            if (_modifyType == ModifyHierarchyType.FullName)
            {
                var result = _project.UnderlyingProject.GetMkDocument(itemId, out var fullName);

                ErrorHandler.ThrowOnFailure(result);

                if (_fullName == fullName.TrimEnd('\\'))
                {
                    OperationSuccessful = true;

                    this.Dispose();
                }
            }
            else
            {
                if (itemId == _itemId)
                {
                    OperationSuccessful = true;

                    this.Dispose();
                }
            }
        }

        internal static ModifyHierarchyEvents Create(IPhysicalProjectItemNode projectItem, TimeSpan timeout)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var parentProject = projectItem.GetParentProject();

            var semaphore = new SemaphoreSlim(0, 1);

            var modifyHierarchyEvents = new ModifyHierarchyEvents(semaphore, projectItem.ParentSolution, parentProject, projectItem.ItemId, timeout);

            return BaseCreate(modifyHierarchyEvents, parentProject.UnderlyingHierarchy);
        }

        internal static ModifyHierarchyEvents Create(ProjectNode project, string fullName, TimeSpan timeout)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var semaphore = new SemaphoreSlim(0, 1);

            var modifyHierarchyEvents = new ModifyHierarchyEvents(semaphore, project.ParentSolution, project, fullName, timeout);

            return BaseCreate(modifyHierarchyEvents, project.UnderlyingHierarchy);
        }

        private static ModifyHierarchyEvents BaseCreate(ModifyHierarchyEvents hierarchyEvents, IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = hierarchy.AdviseHierarchyEvents(hierarchyEvents, out var cookieUID);

            ErrorHandler.ThrowOnFailure(result);

            hierarchyEvents.SetCookie(cookieUID);

            return hierarchyEvents;
        }

        public override void Dispose()
        {
            if (!_isDisposed)
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                _cancelTimer.Dispose();

                var result = _project.UnderlyingHierarchy.UnadviseHierarchyEvents(Cookie);

                Semaphore.Release();
                Semaphore.Dispose();

                ErrorHandler.ThrowOnFailure(result);

                _isDisposed = true;
            }
        }
    }
}
