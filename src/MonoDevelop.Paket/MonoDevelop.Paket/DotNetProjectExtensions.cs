﻿//
// DotNetProjectExtensions.cs
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
using System.IO;
using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.Projects;
using Paket;
using MonoDevelop.Ide;

namespace MonoDevelop.Paket
{
	public static class DotNetProjectExtensions
	{
		public static FilePath GetPaketReferencesFile (this DotNetProject project)
		{
			FilePath path = project.BuildPaketReferencesFileName ();
			if (File.Exists (path))
				return path;

			return FilePath.Null;
		}

		public static FilePath BuildPaketReferencesFileName (this DotNetProject project)
		{
			return Path.Combine (project.BaseDirectory, "paket.references");
		}

		public static bool HasPaketReferences (this DotNetProject project)
		{
			return !project.GetPaketReferencesFile ().IsNull;
		}

		public static ProjectPackageInstallSettings GetPackageInstallSettings (this DotNetProject project)
		{
			try {
				var referencesFileName = project.GetPaketReferencesFile ();
				if (referencesFileName.IsNotNull) {
					IEnumerable<PackageInstallSettings> installSettings = ReferencesFile
						.FromFile (referencesFileName)
						.Groups
						.FirstOrDefault ()
						.Value
						.NugetPackages;
					return new ProjectPackageInstallSettings (installSettings);
				}
			} catch (Exception ex) {
				LoggingService.LogError ("GetPaketReferences error.", ex);
				return new ProjectPackageInstallSettings (ex);
			}

			return new ProjectPackageInstallSettings ();
		}

		public static void OpenPaketReferencesFile (this DotNetProject project)
		{
			var referencesFileName = project.GetPaketReferencesFile ();
			if (referencesFileName.IsNotNull) {
				IdeApp.Workbench.OpenDocument (referencesFileName, null, true);
			}
		}
	}
}

