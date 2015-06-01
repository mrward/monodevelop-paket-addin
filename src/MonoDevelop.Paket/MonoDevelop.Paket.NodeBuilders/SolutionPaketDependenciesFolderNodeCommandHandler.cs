//
// SolutionPaketDependenciesFolderNodeCommandHandler.cs
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

using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Components.Commands;
using MonoDevelop.Paket.Commands;

namespace MonoDevelop.Paket.NodeBuilders
{
	public class SolutionPaketDependenciesFolderNodeCommandHandler : NodeCommandHandler
	{
		public override void ActivateItem ()
		{
			FolderNode.OpenFile ();
		}

		[CommandHandler (PaketCommands.Restore)]
		public void Restore ()
		{
			var message = ProgressMonitorStatusMessageFactory.CreateRestoreMessage ();
			var action = new RestorePaketAction (FolderNode.GetPaketDependenciesFile ());
			PaketServices.ActionRunner.Run (message, action);
		}

		SolutionPaketDependenciesFolderNode FolderNode {
			get {
				return (SolutionPaketDependenciesFolderNode)CurrentNode.DataItem;
			}
		}

		[CommandHandler (PaketCommands.Install)]
		public void Install ()
		{
			var message = ProgressMonitorStatusMessageFactory.CreateInstallMessage ();
			var action = new InstallPaketAction (FolderNode.GetPaketDependenciesFile ());
			PaketServices.ActionRunner.Run (message, action);
		}

		[CommandHandler (PaketCommands.Simplify)]
		public void Simplify ()
		{
			var message = ProgressMonitorStatusMessageFactory.CreateSimplifyMessage ();
			var action = new SimplifyPaketAction (FolderNode.GetPaketDependenciesFile ());
			PaketServices.ActionRunner.Run (message, action);
		}
	}
}

