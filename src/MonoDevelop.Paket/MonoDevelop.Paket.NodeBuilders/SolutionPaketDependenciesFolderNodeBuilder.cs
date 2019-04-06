﻿//
// SolutionPaketDependenciesFolderNodeBuilder.cs
//
// Author:
//       Matt Ward <ward.matt@gmail.com>
//
// Copyright (c) 2015 Matthew Ward
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using MonoDevelop.Ide.Gui.Components;

namespace MonoDevelop.Paket.NodeBuilders
{
	public class SolutionPaketDependenciesFolderNodeBuilder : TypeNodeBuilder
	{
		public override Type NodeDataType {
			get { return typeof(SolutionPaketDependenciesFolderNode); }
		}

		public override string GetNodeName (ITreeNavigator thisNode, object dataObject)
		{
			return "PaketDependencies";
		}

		public override Type CommandHandlerType {
			get { return typeof(SolutionPaketDependenciesFolderNodeCommandHandler); }
		}

		public override string ContextMenuAddinPath {
			get { return "/MonoDevelop/Paket/ContextMenu/ProjectPad/PaketDependencies"; }
		}

		public override void BuildNode (ITreeBuilder treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			var node = (SolutionPaketDependenciesFolderNode)dataObject;
			node.RefreshPackageRequirements ();

			nodeInfo.Label = node.GetLabel ();
			nodeInfo.Icon = Context.GetIcon (node.Icon);
			nodeInfo.ClosedIcon = Context.GetIcon (node.ClosedIcon);
			nodeInfo.StatusSeverity = node.StatusSeverity;
			nodeInfo.StatusMessage = node.GetStatusMessage ();
		}

		public override int GetSortIndex (ITreeNavigator node)
		{
			return -490;
		}

		public override bool HasChildNodes (ITreeBuilder builder, object dataObject)
		{
			return true;
		}

		public override void BuildChildNodes (ITreeBuilder treeBuilder, object dataObject)
		{
			var node = (SolutionPaketDependenciesFolderNode)dataObject;
			foreach (NuGetPackageDependencyNode dependency in node.GetPackageDependencies ()) {
				treeBuilder.AddChild (dependency);
			}
		}
	}
}

