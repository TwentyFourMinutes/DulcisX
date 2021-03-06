﻿using DulcisX.Core.Enums;
using Microsoft.VisualStudio.Shell.Interop;
using System.Diagnostics;

namespace DulcisX.Hierarchy
{
    /// <summary>
    /// Represents a Solution Folder within a <see cref="SolutionNode"/>.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class SolutionFolderNode : SolutionItemNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionFolderNode"/> class.
        /// </summary>
        /// <param name="solution">The Solution in which the <see cref="SolutionFolderNode"/> sits in.</param>
        /// <param name="hierarchy">The Hierarchy of the <see cref="SolutionFolderNode"/> itself.</param>
        public SolutionFolderNode(SolutionNode solution, IVsHierarchy hierarchy) : base(solution, hierarchy, CommonNodeIds.SolutionFolder)
        {
        }
    }
}
