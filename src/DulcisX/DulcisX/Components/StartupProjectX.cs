using DulcisX.Core.Models.Enums;

namespace DulcisX.Components
{
    public class StartupProjectX
    {
        public StartupOptions StartupOptions { get; }

        public ProjectX Project { get; set; }

        internal StartupProjectX(StartupOptions startupOptions, ProjectX project)
        {
            StartupOptions = startupOptions;
            Project = project;
        }
    }
}
