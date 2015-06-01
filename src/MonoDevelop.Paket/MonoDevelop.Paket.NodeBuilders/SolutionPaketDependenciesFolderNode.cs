//
// SolutionPaketDependenciesFolderNode.cs
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
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;
using Paket;

namespace MonoDevelop.Paket.NodeBuilders
{
	public class SolutionPaketDependenciesFolderNode
	{
		readonly Solution solution;
		List<NuGetPackageUpdate> updates = new List<NuGetPackageUpdate> ();

		public SolutionPaketDependenciesFolderNode (Solution solution)
		{
			this.solution = solution;
		}

		internal Solution Solution {
			get { return solution; }
		}

		public IconId Icon {
			get { return Stock.OpenReferenceFolder; }
		}

		public IconId ClosedIcon {
			get { return Stock.ClosedReferenceFolder; }
		}

		public string GetLabel ()
		{
			return GettextCatalog.GetString ("Paket Dependencies") + GetUpdatedCountLabel ();
		}

		string GetUpdatedCountLabel ()
		{
			int count = PaketServices.UpdatedPackagesInSolution.UpdatedPackagesCount;
			if (count == 0) {
				return string.Empty;
			}

			return " <span color='grey'>" + GetUpdatedPackagesCountLabel (count) + "</span>";
		}

		string GetUpdatedPackagesCountLabel (int count)
		{
			return string.Format ("({0} {1})", count, GetUpdateText (count));
		}

		string GetUpdateText (int count)
		{
			if (count > 1) {
				return GettextCatalog.GetString ("updates");
			}
			return GettextCatalog.GetString ("update");
		}

		public IEnumerable<NuGetPackageDependencyNode> GetPackageDependencies ()
		{
			return solution.GetPackageRequirements ()
				.Select (packageReference => CreateDependencyNode (packageReference));
		}

		NuGetPackageDependencyNode CreateDependencyNode (Requirements.PackageRequirement packageReference)
		{
			var node = new NuGetPackageDependencyNode (solution, packageReference);
			node.UpdatedPackage = PaketServices.UpdatedPackagesInSolution.FindUpdatedPackage (node.Id);
			return node;
		}

		public void OpenFile ()
		{
			solution.OpenPaketDependenciesFile ();
		}

		public FilePath GetPaketDependenciesFile ()
		{
			return solution.GetPaketDependenciesFile ();
		}
	}
}

