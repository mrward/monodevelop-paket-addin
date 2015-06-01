//
// NuGetPackageDependencyNode.cs
//
// Author:
//       Matt Ward <matt.ward@xamarin.com>
//
// Copyright (c) 2015 Xamarin Inc. (http://xamarin.com)
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
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;
using Paket;

namespace MonoDevelop.Paket.NodeBuilders
{
	public class NuGetPackageDependencyNode
	{
		readonly Solution solution;
		readonly Requirements.PackageRequirement packageRequirement;

		public NuGetPackageDependencyNode (
			Solution solution,
			Requirements.PackageRequirement packageRequirement)
		{
			this.solution = solution;
			this.packageRequirement = packageRequirement;
		}

		internal Requirements.PackageRequirement PackageRequirement {
			get { return packageRequirement; }
		}

		public string Name {
			get { return packageRequirement.Name.ToString (); }
		}

		public string Id {
			get { return Name; }
		}

		public string GetLabel ()
		{
			return Name + GetUpdatedPackageVersionLabel ();
		}

		public IconId GetIconId ()
		{
			return Stock.Reference;
		}

		public FilePath GetPackageDependencyFile ()
		{
			return solution.GetPaketDependenciesFile ();
		}

		public NuGetPackageUpdate UpdatedPackage { get; set; }

		string GetUpdatedPackageVersionLabel ()
		{
			if (UpdatedPackage == null)
				return string.Empty;

			return string.Format (" <span color='grey'>({0} {1})</span>",
				UpdatedPackage.Version,
				GettextCatalog.GetString ("available"));
		}
	}
}

