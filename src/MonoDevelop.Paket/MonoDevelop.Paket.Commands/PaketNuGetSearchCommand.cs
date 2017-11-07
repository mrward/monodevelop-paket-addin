//
// PaketNuGetSearchCommand.cs
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
	public abstract class PaketNuGetSearchCommand : PaketSearchCommand
	{
		PaketNuGetSearchCommandQuery query;

		protected PaketNuGetSearchCommand (string commandName, string search, Project project = null)
			: base (string.Format ("{0} nuget", commandName))
		{
			query = new PaketNuGetSearchCommandQuery (search);
			query.Parse ();
			Project = project;
			PaketNuGetCommandName = commandName;
			SupportsVersion = true;
		}

		public override void Run ()
		{
			var commandLine = PaketCommandLine.CreateCommandLine (GenerateCommandLine ());
			ProgressMonitorStatusMessage message = GetProgressMessage (GetPackageIdToDisplay ());
			PaketServices.CommandRunner.Run (commandLine, message, NotifyPaketFilesChanged);
		}

		protected abstract ProgressMonitorStatusMessage GetProgressMessage (string packageId);

		/// <summary>
		/// The 'to', 'from' part of the message shown in the unified search window.
		/// (to project foo)
		/// (from project foo)
		/// </summary>
		/// <value>The project name markup start.</value>
		protected abstract string ProjectNameMarkupStart { get; }

		protected bool SupportsVersion { get; set; }

		protected Project Project { get; set; }

		string GetPackageIdToDisplay ()
		{
			if (string.IsNullOrEmpty (query.PackageId))
				return "NuGet package";

			return query.PackageId;
		}

		string PaketNuGetCommandName { get; set; }

		string GenerateCommandLine ()
		{
			return string.Format (
				"{0} {1}{2}{3}",
				PaketNuGetCommandName,
				query.PackageId,
				GetPackageVersionCommandLineArgument (),
				GetProjectCommandLineArgument ());
		}

		string GetPackageVersionCommandLineArgument ()
		{
			if (SupportsVersion && query.HasVersion ())
				return " --version " + query.PackageVersion;

			return string.Empty;
		}

		string GetProjectCommandLineArgument ()
		{
			if (Project != null)
				return string.Format (" --project \"{0}\"", Project.FileName);

			return string.Empty;
		}

		public override string GetMarkup ()
		{
			return GettextCatalog.GetString (
				"paket {0} <b>{1}</b>{2}",
				PaketNuGetCommandName,
				GetPackageIdAndVersionMarkup (),
				GetProjectNameMarkup ());
		}

		string GetPackageIdAndVersionMarkup ()
		{
			return string.Format ("{0} {1}",
				GetPackageIdMarkup (),
				GetPackageVersionMarkup ())
					.TrimEnd ();
		}

		string GetPackageIdMarkup ()
		{
			if (query.HasPackageId ())
				return query.PackageId;

			return "PackageId";
		}

		string GetPackageVersionMarkup ()
		{
			if (!SupportsVersion)
				return String.Empty;

			if (query.HasPackageId ())
				return query.PackageVersion;

			return "[Version]";
		}

		string GetProjectNameMarkup ()
		{
			if (Project != null)
				return string.Format (" ({0} project {1})", ProjectNameMarkupStart, Project.Name);

			return string.Empty;
		}

		protected virtual void NotifyPaketFilesChanged ()
		{
			if (Project != null) {
				NotifyPaketFilesChanged (Project);
				FileService.NotifyFileChanged (Project.FileName);
			}

			PaketServices.FileChangedNotifier.NotifyPaketDependenciesFileChanged ();
			PaketServices.FileChangedNotifier.NotifyPaketLockFileChanged ();
		}
	}
}

