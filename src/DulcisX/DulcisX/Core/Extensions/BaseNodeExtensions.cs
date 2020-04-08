using DulcisX.Core.Enums;
using DulcisX.Exceptions;
using DulcisX.Nodes;
using EnvDTE80;

namespace DulcisX.Core.Extensions
{
    internal static class BaseNodeExtensions
    {
        internal static bool IsTypeMatching(this IBaseNode node, NodeTypes nodeType)
        {
            if (nodeType.ContainsMultipleFlags())
            {
                throw new NoFlagsAllowedException(nameof(NodeTypes));
            }

            switch (nodeType)
            {
                case NodeTypes.Unknown:
                    return node is UnknownNode;
                case NodeTypes.Document:
                    return node is DocumentNode;
                case NodeTypes.Folder:
                    return node is FolderNode;
                case NodeTypes.Project:
                    return node is ProjectNode project && project.ProjectNodeType == ProjectNodeType.Project;
                case NodeTypes.MiscellaneousFilesProject:
                    return node is ProjectNode miscProject && miscProject.ProjectNodeType == ProjectNodeType.MiscellaneousFilesProject;
                case NodeTypes.SolutionItemsProject:
                    return node is ProjectNode solutionItemsProject && solutionItemsProject.ProjectNodeType == ProjectNodeType.SolutionItemsProject;
                case NodeTypes.VirtualProject:
                    return node is ProjectNode virtualProject && virtualProject.ProjectNodeType == ProjectNodeType.VirtualProject;
                case NodeTypes.SolutionFolder:
                    return node is SolutionFolderNode;
                case NodeTypes.Solution:
                    return node is SolutionNode;
                case NodeTypes.All:
                    return true;
            }

            return false;
        }

        internal static NodeTypes GetNodeType(this IBaseNode node)
        {
            if (node is ProjectNode project)
            {
                switch (project.ProjectNodeType)
                {
                    case ProjectNodeType.Project:
                        return NodeTypes.Project;
                    case ProjectNodeType.MiscellaneousFilesProject:
                        return NodeTypes.MiscellaneousFilesProject;
                    case ProjectNodeType.SolutionItemsProject:
                        return NodeTypes.SolutionItemsProject;
                    case ProjectNodeType.VirtualProject:
                        return NodeTypes.VirtualProject;
                }
            }
            else
            {
                switch (node)
                {
                    case UnknownNode _:
                        return NodeTypes.Unknown;
                    case DocumentNode _:
                        return NodeTypes.Document;
                    case FolderNode _:
                        return NodeTypes.Folder;
                    case SolutionFolder _:
                        return NodeTypes.SolutionFolder;
                    case SolutionNode _:
                        return NodeTypes.Solution;
                }
            }

            return NodeTypes.Unknown;
        }
    }
}
