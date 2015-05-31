//
// SolutionPaketDependenciesNodeBuilderExtension.cs
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
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Projects;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace MonoDevelop.Paket.NodeBuilders
{
	public class SolutionPaketDependenciesNodeBuilderExtension : NodeBuilderExtension
	{
		public SolutionPaketDependenciesNodeBuilderExtension ()
		{
			FileService.FileChanged += FileChanged;
		}

		public override void Dispose ()
		{
			FileService.FileChanged -= FileChanged;
		}

		public override bool CanBuildNode (Type dataType)
		{
			return typeof(Solution).IsAssignableFrom (dataType);
		}

		public override bool HasChildNodes (ITreeBuilder builder, object dataObject)
		{
			return SolutionHasDependencies (dataObject);
		}

		bool SolutionHasDependencies (object dataObject)
		{
			var solution = (Solution) dataObject;
			return solution.HasPaketDependencies ();
		}

		public override void BuildChildNodes (ITreeBuilder treeBuilder, object dataObject)
		{
			var solution = (Solution)dataObject;
			if (SolutionHasDependencies (solution)) {
				treeBuilder.AddChild (new SolutionPaketDependenciesFolderNode (solution));
			}
		}

		void FileChanged (object sender, FileEventArgs e)
		{
			List<FilePath> paketDependencyFiles = GetPaketDependencyFilesChanged (e).ToList ();
			if (paketDependencyFiles.Any ()) {
				RefreshAllChildNodes (paketDependencyFiles);
			}
		}

		IEnumerable<FilePath> GetPaketDependencyFilesChanged (FileEventArgs fileEventArgs)
		{
			return fileEventArgs
				.Where (file => file.FileName.IsPaketDependenciesFileName ())
				.Select (file => file.FileName);
		}

		void RefreshAllChildNodes (ICollection<FilePath> dependencyFiles)
		{
			foreach (Solution solution in IdeApp.Workspace.GetAllSolutions ()) {
				FilePath solutionPaketDependenciesFile = solution.GetPaketDependenciesFile ();
				if (dependencyFiles.Any (file => file.Equals (solutionPaketDependenciesFile))) {
					Context.UpdateChildrenFor (solution);
				}
			}
		}
	}
}

