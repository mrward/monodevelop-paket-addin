//
// UpdatedPackagesInSolution.cs
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
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace MonoDevelop.Paket
{
	public class UpdatedPackagesInSolution
	{
		public UpdatedPackagesInSolution ()
		{
			IdeApp.Workspace.SolutionUnloaded += SolutionUnloaded;
		}

		void SolutionUnloaded (object sender, SolutionEventArgs e)
		{
			Clear ();
		}

		public void Clear ()
		{
			updates = new List<NuGetPackageUpdate> ();
		}

		List<NuGetPackageUpdate> updates = new List<NuGetPackageUpdate> ();

		public void RefreshUpdatedPackages (IEnumerable<NuGetPackageUpdate> updates)
		{
			this.updates = updates.ToList ();
		}

		public int UpdatedPackagesCount {
			get { return updates.Count; }
		}

		public NuGetPackageUpdate FindUpdatedPackage (string id)
		{
			return updates.FirstOrDefault (update => update.IsMatch (id));
		}

		public void Remove (string packageId)
		{
			updates.RemoveAll (update => update.IsMatch (packageId));
		}
	}
}

