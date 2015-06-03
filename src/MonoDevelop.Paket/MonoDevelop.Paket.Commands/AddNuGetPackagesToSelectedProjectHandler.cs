//
// AddNuGetPackagesToSelectedProjectHandler.cs
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
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.PackageManagement;
using MonoDevelop.Projects;

namespace MonoDevelop.Paket.Commands
{
	public class AddNuGetPackagesToSelectedProjectHandler : CommandHandler
	{
		protected override void Update (CommandInfo info)
		{
			if (IsEnabled ()) {
				info.Bypass = false;
			} else {
				info.Enabled = false;
				info.Bypass = true;
			}
		}

		protected virtual bool IsEnabled ()
		{
			return IsDotNetProjectSelected ();
		}

		protected bool IsDotNetProjectSelected ()
		{
			return GetSelectedDotNetProject () != null;
		}

		protected DotNetProject GetSelectedDotNetProject ()
		{
			return IdeApp.ProjectOperations.CurrentSelectedProject as DotNetProject;
		}

		protected override void Run ()
		{
			DotNetProject project = GetSelectedDotNetProject ();
			if (project == null)
				return;
			
			var runner = new AddPackagesDialogRunner ();
			runner.RunToAddPackageReferences ();
			if (runner.PackagesToAdd.Any ()) {
				AddPackagesToProject (project, runner.PackagesToAdd.ToList ());
			}
		}

		void AddPackagesToProject (DotNetProject project, IList<NuGetPackageToAdd> packagesToAdd)
		{
			var message = ProgressMonitorStatusMessageFactory.CreateAddNuGetPackagesMessage (packagesToAdd);

			List<PaketAction> actions = packagesToAdd
				.Select (package => new AddNuGetToProjectPaketAction (package, project))
				.OfType<PaketAction> ()
				.ToList ();

			actions.Insert (0, new CreatePaketDependenciesFileIfNotExistPaketAction (project));
			PaketServices.ActionRunner.Run (message, actions);
		}
	}
}

