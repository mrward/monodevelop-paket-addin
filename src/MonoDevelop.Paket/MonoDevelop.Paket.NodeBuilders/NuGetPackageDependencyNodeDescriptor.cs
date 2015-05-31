//
// NuGetPackageDependencyNodeDescriptor.cs
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

using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.DesignerSupport;
using Paket;

namespace MonoDevelop.Paket.NodeBuilders
{
	public class NuGetPackageDependencyNodeDescriptor : CustomDescriptor
	{
		readonly NuGetPackageDependencyNode packageDependencyNode;
		readonly Requirements.PackageRequirement packageRequirement;
		string frameworkRestrictions;

		public NuGetPackageDependencyNodeDescriptor (NuGetPackageDependencyNode packageDependencyNode)
		{
			this.packageDependencyNode = packageDependencyNode;
			packageRequirement = packageDependencyNode.PackageRequirement;
		}

		[LocalizedCategory ("Package")]
		[LocalizedDisplayName ("Id")]
		[LocalizedDescription ("Package Id.")]
		public string Id {
			get { return packageDependencyNode.Id; }
		}

		[LocalizedCategory ("Package")]
		[LocalizedDisplayName ("Version")]
		[LocalizedDescription ("Version.")]
		public string Version {
			get {
				if (packageRequirement.VersionRequirement != null)
					return packageRequirement.VersionRequirement.ToString ();
				return string.Empty;
			}
		}

		[LocalizedCategory ("Package")]
		[LocalizedDisplayName ("Copy Local")]
		[LocalizedDescription ("Copy Local.")]
		public string CopyLocal {
			get {
				if (packageRequirement.Settings.ImportTargets != null)
					return packageRequirement.Settings.CopyLocal.Value.ToString ();
				return string.Empty;
			}
		}

		[LocalizedCategory ("Package")]
		[LocalizedDisplayName ("Import Targets")]
		[LocalizedDescription ("Import targets.")]
		public string ImportTargets {
			get {
				if (packageRequirement.Settings.ImportTargets != null)
					return packageRequirement.Settings.ImportTargets.Value.ToString ();
				return string.Empty;
			}
		}

		[LocalizedCategory ("Package")]
		[LocalizedDisplayName ("Omit Content")]
		[LocalizedDescription ("Omit content.")]
		public string OmitContent {
			get {
				if (packageRequirement.Settings.OmitContent != null)
					return packageRequirement.Settings.OmitContent.Value.ToString ();
				return string.Empty;
			}
		}

		[LocalizedCategory ("Package")]
		[LocalizedDisplayName ("Frameworks")]
		[LocalizedDescription ("Framework restrictions.")]
		public string Frameworks {
			get {
				if (frameworkRestrictions == null) {
					frameworkRestrictions = GetFrameworkRestrictions ();
				}
				return frameworkRestrictions;
			}
		}

		string GetFrameworkRestrictions ()
		{
			if (!packageRequirement.Settings.FrameworkRestrictions.Any ()) {
				return string.Empty;
			}

			return string.Join (", ", packageRequirement.Settings.FrameworkRestrictions);
		}
	}
}

