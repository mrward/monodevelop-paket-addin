//
// ProjectPaketReferencesFolderNode.cs
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
using MonoDevelop.Projects;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Tasks;

namespace MonoDevelop.Paket.NodeBuilders
{
	public class ProjectPaketReferencesFolderNode
	{
		readonly DotNetProject project;
		ProjectPackageInstallSettings installSettings;

		public ProjectPaketReferencesFolderNode (DotNetProject project)
		{
			this.project = project;
		}

		internal DotNetProject Project {
			get { return project; }
		}

		public IconId Icon {
			get { return Stock.OpenReferenceFolder; }
		}

		public IconId ClosedIcon {
			get { return Stock.ClosedReferenceFolder; }
		}

		public string GetLabel ()
		{
			return GettextCatalog.GetString ("Paket References");
		}

		public TaskSeverity? StatusSeverity {
			get {
				if (InstallSettings.HasError) {
					return TaskSeverity.Error;
				}
				return null;
			}
		}

		public string GetStatusMessage ()
		{
			if (InstallSettings.HasError) {
				return InstallSettings.ErrorMessage;
			}
			return string.Empty;
		}

		public void RefreshPackageReferences ()
		{
			installSettings = project.GetPackageInstallSettings ();
		}

		public ProjectPackageInstallSettings InstallSettings {
			get {
				if (installSettings == null) {
					RefreshPackageReferences ();
				}
				return installSettings;
			}
		}

		public IEnumerable<NuGetPackageReferenceNode> GetPackageReferences ()
		{
			return InstallSettings.InstallSettings
				.Select (installSettings => new NuGetPackageReferenceNode (project, installSettings));
		}

		public void OpenFile ()
		{
			project.OpenPaketReferencesFile ();
		}
	}
}

