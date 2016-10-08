//
// CheckForUpdatesPaketAction.cs
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
using MonoDevelop.Core;
using Paket;
using MonoDevelop.Ide;

namespace MonoDevelop.Paket
{
	public class CheckForUpdatesPaketAction : PaketAction
	{
		UpdatedPackagesProgressMonitorStatusMessage progressMessage;
		FilePath dependenciesFileName;
		readonly Action<IEnumerable<NuGetPackageUpdate>> onCompleted;

		public CheckForUpdatesPaketAction (
			UpdatedPackagesProgressMonitorStatusMessage progressMessage,
			FilePath dependenciesFileName,
			Action<IEnumerable<NuGetPackageUpdate>> onCompleted)
		{
			this.progressMessage = progressMessage;
			this.dependenciesFileName = dependenciesFileName;
			this.onCompleted = onCompleted;
		}

		public override void Run ()
		{
			List<NuGetPackageUpdate> updates = Dependencies.Locate (dependenciesFileName)
				.FindOutdated (false, false)
				.Select (update => new NuGetPackageUpdate (update.Item2, update.Item3))
				.ToList ();

			progressMessage.UpdatedPackagesFound = updates.Count;
			ReportUpdates (updates);

			Runtime.RunInMainThread (() => {
				PaketServices.UpdatedPackagesInSolution.RefreshUpdatedPackages (updates);
				onCompleted (updates);
			});
		}

		void ReportUpdates (IList<NuGetPackageUpdate> updates)
		{
			if (!updates.Any ()) {
				Monitor.Log.WriteLine ("No outdated packages found.");
				return;
			}

			Monitor.Log.WriteLine ("Outdated packages found:");

			foreach (NuGetPackageUpdate update in updates) {
				Monitor.Log.WriteLine ("  * {0}", update);
			}
		}
	}
}

