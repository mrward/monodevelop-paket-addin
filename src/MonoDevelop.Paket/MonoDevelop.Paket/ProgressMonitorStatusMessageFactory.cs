//
// ProgressMonitorStatusMessageFactory.cs
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

using MonoDevelop.Core;

namespace MonoDevelop.Paket
{
	public static class ProgressMonitorStatusMessageFactory
	{
		public static ProgressMonitorStatusMessage CreateAddNuGetPackageMessage (string packageId)
		{
			return new ProgressMonitorStatusMessage (
				GetString ("Adding {0} ...", packageId),
				GetString ("{0} added successfully.", packageId),
				GetString ("Could not add {0}.", packageId),
				GetString ("{0} added with warnings.", packageId)
			);
		}

		public static ProgressMonitorStatusMessage CreateAutoRestoreOnMessage ()
		{
			return new ProgressMonitorStatusMessage (
				GetString ("Enabling automatic paket restore..."),
				GetString ("Automatic paket restore enabled successfully."),
				GetString ("Could not enable automatic paket restore."),
				GetString ("Automatic paket restore enabled with warnings.")
			);
		}

		public static ProgressMonitorStatusMessage CreateAutoRestoreOffMessage ()
		{
			return new ProgressMonitorStatusMessage (
				GetString ("Disabling automatic paket restore..."),
				GetString ("Automatic paket restore disabled successfully."),
				GetString ("Could not disable automatic paket restore."),
				GetString ("Automatic paket restore disabled with warnings.")
			);
		}

		public static ProgressMonitorStatusMessage CreateConvertFromNuGetMessage ()
		{
			return new ProgressMonitorStatusMessage (
				GetString ("Converting from NuGet to Paket..."),
				GetString ("Conversion to Paket completed successfully."),
				GetString ("Could not convert from NuGet to Paket."),
				GetString ("Conversion to Paket completed with warnings.")
			);
		}

		public static ProgressMonitorStatusMessage CreateInitMessage ()
		{
			return new ProgressMonitorStatusMessage (
				GetString ("Creating paket.dependencies file..."),
				GetString ("paket.dependencies created successfully."),
				GetString ("Could not create paket.dependencies file."),
				GetString ("paket.dependencies created with warnings.")
			);
		}

		public static ProgressMonitorStatusMessage CreateInstallMessage ()
		{
			return new ProgressMonitorStatusMessage (
				GetString ("Install paket dependencies..."),
				GetString ("Paket dependencies installed successfully."),
				GetString ("Could not install paket dependencies."),
				GetString ("Paket dependencies installed with warnings.")
			);
		}

		public static ProgressMonitorStatusMessage CreateOutdatedMessage ()
		{
			return new ProgressMonitorStatusMessage (
				GetString ("Checking for outdated paket dependencies..."),
				GetString ("Outdated paket dependencies checked successfully."),
				GetString ("Could not check for outdated paket dependencies."),
				GetString ("Outdated paket dependencies checked with warnings.")
			);
		}

		public static ProgressMonitorStatusMessage CreateRemoveNuGetPackageMessage (string packageId)
		{
			return new ProgressMonitorStatusMessage (
				GetString ("Removing {0} ...", packageId),
				GetString ("{0} removed successfully.", packageId),
				GetString ("Could not remove {0}.", packageId),
				GetString ("{0} removed with warnings.", packageId)
			);
		}

		public static ProgressMonitorStatusMessage CreateUpdateMessage ()
		{
			return new ProgressMonitorStatusMessage (
				GetString ("Updating paket dependencies..."),
				GetString ("Paket dependencies updated successfully."),
				GetString ("Could not update paket dependencies."),
				GetString ("Paket dependencies updated with warnings.")
			);
		}

		public static ProgressMonitorStatusMessage CreateRestoreMessage ()
		{
			return new ProgressMonitorStatusMessage (
				GetString ("Restoring paket dependencies..."),
				GetString ("Paket dependencies restored successfully."),
				GetString ("Could not restore paket dependencies."),
				GetString ("Paket dependencies restored with warnings.")
			);
		}

		public static ProgressMonitorStatusMessage CreateSimplifyMessage ()
		{
			return new ProgressMonitorStatusMessage (
				GetString ("Simplifying paket dependencies..."),
				GetString ("Paket dependencies simplified successfully."),
				GetString ("Could not simplify paket dependencies."),
				GetString ("Paket dependencies simplified with warnings.")
			);
		}

		static string GetString (string phrase)
		{
			return GettextCatalog.GetString (phrase);
		}

		static string GetString (string phrase, object arg0)
		{
			return GettextCatalog.GetString (phrase, arg0);
		}
	}
}

