//
// PaketAddRemoveSearchCommand.cs
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
using MonoDevelop.Core;
using MonoDevelop.Projects;

namespace MonoDevelop.Paket.Commands
{
	public class PaketRemoveNuGetSearchCommand : PaketSearchCommand
	{
		PaketNuGetSearchCommandQuery query;

		public PaketRemoveNuGetSearchCommand (string search, Project project = null)
			: base ("remove nuget")
		{
			query = new PaketNuGetSearchCommandQuery (search);
			query.Parse ();
			Project = project;
		}

		public override void Run ()
		{
			var commandLine = PaketCommandLine.CreateCommandLine (GenerateCommandLine ());
			var message = ProgressMonitorStatusMessageFactory.CreateRemoveNuGetPackageMessage (GetPackageIdToDisplay ());
			PaketServices.CommandRunner.Run (commandLine, message, OnPaketRunCompleted);
		}

		protected Project Project { get; set; }

		string GetPackageIdToDisplay ()
		{
			if (string.IsNullOrEmpty (query.PackageId))
				return "NuGet package";

			return query.PackageId;
		}

		string GenerateCommandLine ()
		{
			return string.Format (
				"remove nuget {0}{1}{2}",
				query.PackageId,
				GetPackageVersionCommandLineArgument (),
				GetProjectCommandLineArgument ());
		}

		string GetPackageVersionCommandLineArgument ()
		{
			if (query.HasVersion ())
				return " version " + query.PackageVersion;

			return string.Empty;
		}

		string GetProjectCommandLineArgument ()
		{
			if (Project != null)
				return string.Format (" project \"{0}\"", Project.FileName);

			return string.Empty;
		}

		public override string GetMarkup ()
		{
			return GettextCatalog.GetString (
				"paket remove nuget <b>{0} {1}</b> {2}",
				GetPackageIdMarkup (),
				GetPackageVersionMarkup (),
				GetProjectNameMarkup ());
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

		string GetProjectNameMarkup ()
		{
			if (Project != null)
				return string.Format ("(from project {0})", Project.Name);

			return string.Empty;
		}

		public override string GetDescriptionMarkup ()
		{
			if (Project != null) {
				return GettextCatalog.GetString ("Removes a NuGet package from the current project.");
			}

			return GettextCatalog.GetString ("Removes a NuGet package from your paket.dependencies file");
		}

		void OnPaketRunCompleted ()
		{
			try {
				FileService.NotifyFileChanged (Project.FileName);
			} catch (Exception ex) {
				LoggingService.LogError ("Notify file changed error.", ex);
			}
		}
	}
}

