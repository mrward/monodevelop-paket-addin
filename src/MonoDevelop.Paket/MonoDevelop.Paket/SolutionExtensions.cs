﻿//
// SolutionExtensions.cs
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
	public static class SolutionExtensions
	{
		public static FilePath GetPaketDependenciesFile (this Solution solution)
		{
			FilePath path = solution.BuildPaketDependenciesFileName ();
			if (File.Exists (path))
				return path;

			return FilePath.Null;
		}

		public static FilePath BuildPaketDependenciesFileName (this Solution solution)
		{
			return Path.Combine (solution.BaseDirectory, "paket.dependencies");
		}

		public static bool HasPaketDependencies (this Solution solution)
		{
			return !solution.GetPaketDependenciesFile ().IsNull;
		}

		public static IEnumerable<Requirements.PackageRequirement> GetPackageRequirements (this Solution solution)
		{
			try {
				var dependenciesFileName = solution.GetPaketDependenciesFile ();
				if (dependenciesFileName.IsNotNull) {
					return DependenciesFile
						.ReadFromFile (dependenciesFileName)
						.Groups
						.FirstOrDefault ()
						.Value
						.Packages;
				}
			} catch (Exception ex) {
				LoggingService.LogError ("GetPaketDependencies error.", ex);
			}

			return Enumerable.Empty <Requirements.PackageRequirement> ();
		}

		public static void OpenPaketDependenciesFile (this Solution solution)
		{
			var dependenciesFileName = solution.GetPaketDependenciesFile ();
			if (dependenciesFileName.IsNotNull) {
				IdeApp.Workbench.OpenDocument (dependenciesFileName, null, true);
			}
		}
	}
}

