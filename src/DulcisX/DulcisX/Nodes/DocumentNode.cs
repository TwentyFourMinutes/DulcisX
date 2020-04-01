using DulcisX.Core.Enums;
using DulcisX.Exceptions;
using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using StringyEnums;
using System.IO;

namespace DulcisX.Nodes
{
    /// <summary>
    /// Represents a document within a <see cref="ProjectNode"/>.
    /// </summary>
    public class DocumentNode : ProjectItemNode, IPhysicalNode
    {
        /// <inheritdoc/>
        public override NodeTypes NodeType => NodeTypes.Document;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentNode"/> class.
        /// </summary>
        /// <param name="solution">The Solution in which the <see cref="DocumentNode"/> sits in.</param>
        /// <param name="project">The Project in which the <see cref="DocumentNode"/> sits in.</param>
        /// <param name="itemId">The Unique Identifier for the <see cref="DocumentNode"/> in the <paramref name="project"/>.</param>
        public DocumentNode(SolutionNode solution, ProjectNode project, uint itemId) : base(solution, project, itemId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentNode"/> class.
        /// </summary>
        /// <param name="solution">The Solution in which the <see cref="DocumentNode"/> sits in.</param>
        /// <param name="hierarchy">The Hierarchy of the Project in which the <see cref="DocumentNode"/> sits in.</param>
        /// <param name="itemId">The Unique Identifier for the <see cref="DocumentNode"/> in the <paramref name="hierarchy"/>.</param>
        public DocumentNode(SolutionNode solution, IVsHierarchy hierarchy, uint itemId) : base(solution, hierarchy, itemId)
        {
        }

        /// <inheritdoc/>
        public string GetFileName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return HierarchyUtilities.GetHierarchyProperty<string>(UnderlyingHierarchy, ItemId, (int)__VSHPROPID.VSHPROPID_Name);
        }

        /// <inheritdoc/>
        public string GetFullName()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = GetParentProject().UnderlyingProject.GetMkDocument(ItemId, out var fullName);

            ErrorHandler.ThrowOnFailure(result);

            return fullName;
        }

        public string GetDirectoryName()
            => Path.GetDirectoryName(GetFullName());

        /// <summary>
        /// Returns the Build Action for the current <see cref="DocumentNode"/>.
        /// </summary>
        /// <returns>A string containing the Build Action value.</returns>
        public string GetBuildAction()
            => HierarchyUtilities.GetHierarchyProperty<string>(UnderlyingHierarchy, ItemId, (int)__VSHPROPID4.VSHPROPID_BuildAction);

        /// <summary>
        /// Returns the <see cref="CopyToOutputDirectory"/> for the current <see cref="DocumentNode"/>.
        /// </summary>
        /// <returns>An <see cref="CopyToOutputDirectory"/> enumeration with the current value.</returns>
        public CopyToOutputDirectory GetCopyToOutputDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var val = GetParentProject().GetItemProperty(ItemId, DocumentProperty.CopyToOutputDirectory);

            return val.GetEnumFromRepresentation<CopyToOutputDirectory>();
        }

        /// <summary>
        /// Sets the current Output Directory settings.
        /// </summary>
        /// <param name="copyToOutputDirectory">The <see cref="CopyToOutputDirectory"/> settings.</param>
        public void SetCopyToOutputDirectory(CopyToOutputDirectory copyToOutputDirectory)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            GetParentProject().SetItemProperty(ItemId, DocumentProperty.CopyToOutputDirectory, copyToOutputDirectory.GetRepresentation());
        }

        /// <summary>
        /// Renames the physical file of the current <see cref="DocumentNode"/> and updates the name in the Solution Explorer.
        /// </summary>
        /// <param name="newName">A string containg the new name of the file. Can include extension of the file, but not necessarily.</param>
        /// <returns><see langword="true"/> if the operation suceeded a result; otherwise <see langword="false"/>.</returns>
        public DocumentNode Rename(string newName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var fullName = GetFullName();

            if (!Path.HasExtension(newName))
            {
                newName += Path.GetExtension(fullName);
            }

            var newFullName = Path.Combine(Path.GetDirectoryName(fullName), newName);

            return GetParentProject().MoveNodeInsideProject(this, newFullName);
        }

        /// <summary>
        /// Changes the extension of the physical file of the current <see cref="DocumentNode"/> and updates the name in the Solution Explorer.
        /// </summary>
        /// <param name="extension">A string containg the new extension of the file.</param>
        /// <returns>A new instance of a <see cref="DocumentNode"/> representing the renamed Document.</returns>
        public DocumentNode ChangeExtension(string extension)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            return GetParentProject().MoveNodeInsideProject(this, Path.ChangeExtension(GetFullName(), extension));
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="DocumentNode"/> was renamed since the last save in Visual Studio.
        /// </summary>
        /// <returns><see langword="true"/> if the <see cref="DocumentNode"/> was renamed; otherwise <see langword="false"/>.</returns>
        public bool ChangedSinceLastUserSave()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var docCookie = GetParentProject().GetDocumentCookie(this);

            return ((IVsRunningDocumentTable3)ParentSolution.RunningDocumentTable).IsDocumentDirty(docCookie);
        }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="DocumentNode"/> is read-only.
        /// </summary>
        /// <returns><see langword="true"/> if the <see cref="DocumentNode"/> is read-only; otherwise <see langword="false"/>.</returns>
        public bool IsReadonly()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var docCookie = GetParentProject().GetDocumentCookie(this);

            return ((IVsRunningDocumentTable3)ParentSolution.RunningDocumentTable).IsDocumentReadOnly(docCookie);
        }

        /// <summary>
        /// Saves any changes to the current <see cref="DocumentNode"/> content of the file.
        /// </summary>
        /// <param name="forceSave">Determines whether to force the file save operation or not.</param>
        public void Save(bool forceSave = false)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var result = ParentSolution.UnderlyingSolution.SaveSolutionElement(forceSave ? (uint)__VSSLNSAVEOPTIONS.SLNSAVEOPT_ForceSave : (uint)__VSSLNSAVEOPTIONS.SLNSAVEOPT_SaveIfDirty, UnderlyingHierarchy, GetParentProject().GetDocumentCookie(this));

            ErrorHandler.ThrowOnFailure(result);
        }

        /// <summary>
        /// Returns the type of the current <see cref="DocumentNode"/>, determined by the extension of the file.
        /// </summary>
        /// <returns>A <see cref="DocumentType"/> enumeration.</returns>
        public DocumentType GetDocumentType()
        {
            switch (Path.GetExtension(GetFileName()).ToLower())
            {
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".ico":
                case ".svg":
                case ".webp":
                case ".gif":
                case ".tif":
                case ".tiff":
                case ".bmp":
                case ".psd":
                case ".ai":
                    return DocumentType.Image;
                default:
                    return DocumentType.Text;
            }
        }
    }
}
