//
// ProjectPaketReferencesFolderNodeCommandHandler.cs
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

using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.PackageManagement;
using MonoDevelop.Paket.Commands;

namespace MonoDevelop.Paket.NodeBuilders
{
	public class ProjectPaketReferencesFolderNodeCommandHandler : NodeCommandHandler
	{
		public override void ActivateItem ()
		{
			FolderNode.OpenFile ();
		}

		ProjectPaketReferencesFolderNode FolderNode {
			get { return (ProjectPaketReferencesFolderNode)CurrentNode.DataItem; }
		}

		[CommandHandler (PaketCommands.AddPackage)]
		public void AddPackage ()
		{
			var runner = new AddPackagesDialogRunner ();
			runner.RunToAddPackageReferences ();
			AddPackages (runner.PackagesToAdd.ToList ());
		}

		void AddPackages (IList<NuGetPackageToAdd> packagesToAdd)
		{
			if (!packagesToAdd.Any ())
				return;

			var message = ProgressMonitorStatusMessageFactory.CreateAddNuGetPackagesMessage (packagesToAdd);

			List<AddNuGetToProjectPaketAction> actions = packagesToAdd
				.Select (package => new AddNuGetToProjectPaketAction (package, FolderNode.Project))
				.ToList ();
			PaketServices.ActionRunner.Run (message, actions);
		}
	}
}

