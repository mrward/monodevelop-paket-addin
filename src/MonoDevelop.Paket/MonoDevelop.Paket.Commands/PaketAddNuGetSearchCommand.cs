//
// PaketAddNuGetSearchCommandQuery.cs
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

namespace MonoDevelop.Paket.Commands
{
	public class PaketAddNuGetSearchCommand : PaketSearchCommand
	{
		PaketAddNuGetSearchCommandQuery query;

		public PaketAddNuGetSearchCommand (string search)
			: base ("add nuget")
		{
			query = new PaketAddNuGetSearchCommandQuery (search);
			query.Parse ();
		}

		public override void Run ()
		{
			var commandLine = PaketCommandLine.CreateCommandLine (GenerateCommandLine ());
			var message = ProgressMonitorStatusMessageFactory.CreateAddNuGetPackageMessage (GetPackageIdToDisplay ());
			PaketServices.CommandRunner.Run (commandLine, message);
		}

		string GetPackageIdToDisplay ()
		{
			if (string.IsNullOrEmpty (query.PackageId))
				return "NuGet package";

			return query.PackageId;
		}

		string GenerateCommandLine ()
		{
			return string.Format ("add nuget {0} {1}", query.PackageId, GetPackageVersionCommandLineArgument ());
		}

		string GetPackageVersionCommandLineArgument ()
		{
			if (query.HasVersion ())
				return "version " + query.PackageVersion;
			
			return string.Empty;
		}

		public override string GetMarkup ()
		{
			return GettextCatalog.GetString (
				"paket add nuget <b>{0} {1}</b>",
				GetPackageIdMarkup (),
				GetPackageVersionMarkup ());
		}

		string GetPackageIdMarkup ()
		{
			if (query.HasPackageId ())
				return query.PackageId;

			return "PackageId";
		}

		string GetPackageVersionMarkup ()
		{
			if (query.HasPackageId ())
				return query.PackageVersion;

			return "[Version]";
		}

		public override string GetDescriptionMarkup ()
		{
			return GettextCatalog.GetString ("Adds a NuGet package to your paket.dependencies file");
		}
	}
}

