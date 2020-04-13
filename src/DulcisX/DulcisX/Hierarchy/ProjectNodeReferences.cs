using DulcisX.Core.Enums;
using DulcisX.Core.Extensions;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;
using System.Linq;

namespace DulcisX.Hierarchy
{
    public class ProjectNodeReferences
    {
        private IEnumerable<IVsReferenceProviderContext> _referenceContextProviders;

        public IEnumerable<IVsReferenceProviderContext> ReferenceContextProviders
        {
            get
            {
                if (_referenceContextProviders is null)
                {
                    _referenceContextProviders = ReferenceManager.GetProviderContexts()
                                                                 .OfType<IVsReferenceProviderContext>()
                                                                 .ToCachingEnumerable();
                }

                return _referenceContextProviders;
            }
        }

        private IVsReferenceManagerUser _referenceManager;

        public IVsReferenceManagerUser ReferenceManager
        {
            get
            {
                if (_referenceManager is null)
                {
                    _referenceManager = _project.UnderlyingHierarchy.GetProperty<IVsReferenceManagerUser>(CommonNodeIds.Project, (int)__VSHPROPID5.VSHPROPID_ReferenceManagerUser);
                }

                return _referenceManager;
            }
        }

        private IEnumerable<IVsAssemblyReference> _assmeblyReferences;

        public IEnumerable<IVsAssemblyReference> AssmeblyReferences
        {
            get
            {
                if (_assmeblyReferences is null)
                {
                    _assmeblyReferences = GetReferencesOfContextProvider<IVsAssemblyReferenceProviderContext, IVsAssemblyReference>();
                }

                return _assmeblyReferences;
            }
        }

        private IEnumerable<IVsComReference> _comReferences;

        public IEnumerable<IVsComReference> ComReferences
        {
            get
            {
                if (_comReferences is null)
                {
                    _comReferences = GetReferencesOfContextProvider<IVsComReferenceProviderContext, IVsComReference>();
                }

                return _comReferences;
            }
        }

        private IEnumerable<IVsConnectedServiceInstanceReference> _connectedServiceReferences;

        public IEnumerable<IVsConnectedServiceInstanceReference> ConnectedServiceReferences
        {
            get
            {
                if (_connectedServiceReferences is null)
                {
                    _connectedServiceReferences = GetReferencesOfContextProvider<IVsConnectedServiceInstanceReferenceProviderContext, IVsConnectedServiceInstanceReference>();
                }

                return _connectedServiceReferences;
            }
        }

        private IEnumerable<IVsFileReference> _fileReferences;

        public IEnumerable<IVsFileReference> FileReferences
        {
            get
            {
                if (_fileReferences is null)
                {
                    _fileReferences = GetReferencesOfContextProvider<IVsFileReferenceProviderContext, IVsFileReference>();
                }

                return _fileReferences;
            }
        }

        private IEnumerable<IVsPlatformReference> _platformReferences;

        public IEnumerable<IVsPlatformReference> PlatformReferences
        {
            get
            {
                if (_platformReferences is null)
                {
                    _platformReferences = GetReferencesOfContextProvider<IVsPlatformReferenceProviderContext, IVsPlatformReference>();
                }

                return _platformReferences;
            }
        }

        private IEnumerable<IVsProjectReference> _projectReferences;

        public IEnumerable<IVsProjectReference> ProjectReferences
        {
            get
            {
                if (_projectReferences is null)
                {
                    _projectReferences = GetReferencesOfContextProvider<IVsProjectReferenceProviderContext, IVsProjectReference>();
                }

                return _projectReferences;
            }
        }

        private IEnumerable<IVsSharedProjectReference> _sharedProjectReferences;

        public IEnumerable<IVsSharedProjectReference> SharedProjectReferences
        {
            get
            {
                if (_sharedProjectReferences is null)
                {
                    _sharedProjectReferences = GetReferencesOfContextProvider<IVsSharedProjectReferenceProviderContext, IVsSharedProjectReference>();
                }

                return _sharedProjectReferences;
            }
        }

        private string _primaryTargetFramework;

        public string PrimaryTargetFramework
        {
            get
            {
                if (_primaryTargetFramework is null)
                {
                    _primaryTargetFramework = ReferenceContextProviders.OfType<IVsAssemblyReferenceProviderContext>().First().TargetFrameworkMoniker;
                }

                return _primaryTargetFramework;
            }
            set
            {
                _primaryTargetFramework = value;

                ReferenceContextProviders.OfType<IVsAssemblyReferenceProviderContext>().First().TargetFrameworkMoniker = value;
            }

        }

        private readonly ProjectNode _project;

        internal ProjectNodeReferences(ProjectNode project)
        {
            _project = project;
        }

        private IEnumerable<TReference> GetReferencesOfContextProvider<TContextProvider, TReference>() where TContextProvider : IVsReferenceProviderContext
                                                                                                       where TReference : IVsReference
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return ReferenceContextProviders.OfType<TContextProvider>()
                                            .First()
                                            .References
                                            .OfType<TReference>()
                                            .ToCachingEnumerable();
        }
    }
}
