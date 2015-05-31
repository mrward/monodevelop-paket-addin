//
// NuGetPackageReferenceNodeDescriptor.cs
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
	public class NuGetPackageReferenceNodeDescriptor : CustomDescriptor
	{
		readonly NuGetPackageReferenceNode packageReferenceNode;
		readonly Requirements.InstallSettings installSettings;
		string frameworkRestrictions;

		public NuGetPackageReferenceNodeDescriptor (NuGetPackageReferenceNode packageReferenceNode)
		{
			this.packageReferenceNode = packageReferenceNode;
			installSettings = packageReferenceNode.InstallSettings.Settings;
		}

		[LocalizedCategory ("Package")]
		[LocalizedDisplayName ("Id")]
		[LocalizedDescription ("Package Id.")]
		public string Id {
			get { return packageReferenceNode.Id; }
		}

		[LocalizedCategory ("Package")]
		[LocalizedDisplayName ("Copy Local")]
		[LocalizedDescription ("Copy Local.")]
		public string CopyLocal {
			get {
				if (installSettings.CopyLocal != null)
					return installSettings.CopyLocal.Value.ToString ();
				return string.Empty;
			}
		}

		[LocalizedCategory ("Package")]
		[LocalizedDisplayName ("Import Targets")]
		[LocalizedDescription ("Import targets.")]
		public string ImportTargets {
			get {
				if (installSettings.ImportTargets != null)
					return installSettings.ImportTargets.Value.ToString ();
				return string.Empty;
			}
		}

		[LocalizedCategory ("Package")]
		[LocalizedDisplayName ("Omit Content")]
		[LocalizedDescription ("Omit content.")]
		public string OmitContent {
			get {
				if (installSettings.OmitContent != null)
					return installSettings.OmitContent.Value.ToString ();
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
			if (!installSettings.FrameworkRestrictions.Any ()) {
				return string.Empty;
			}

			return string.Join (", ", installSettings.FrameworkRestrictions);
		}
	}
}

