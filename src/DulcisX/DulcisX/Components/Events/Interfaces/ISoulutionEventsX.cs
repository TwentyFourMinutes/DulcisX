using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace DulcisX.Components.Events
{
    public delegate void QueryProjectClose(ProjectX hierarchy, bool isRemoving, ref bool shouldCancel);
    public delegate void QueryProjectUnload(ProjectX hierarchy, ref bool shouldCancel);
    public delegate void QuerySolutionClose(ref bool shouldCancel);

    public interface ISolutionEventsX
    {
        event Action<ProjectX, bool> OnAfterProjectOpen;

        event QueryProjectClose OnQueryProjectClose;

        event Action<ProjectX, bool> OnBeforeProjectClose;

        event Action<IVsHierarchy, ProjectX> OnAfterProjectLoad;

        event QueryProjectUnload OnQueryProjectUnload;

        event Action<ProjectX, IVsHierarchy> OnBeforeProjectUnload;

        event Action<bool> OnAfterSolutionOpen;

        event QuerySolutionClose OnQuerySolutionClose;

        event Action OnBeforeSolutionClose;

        event Action OnAfterSolutionClose;
    }
}
