//
// PaketFileChangedNotifier.cs
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

using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace MonoDevelop.Paket
{
	public class PaketFileChangedNotifier
	{
		public void NotifyPaketDependenciesFileChanged ()
		{
			Solution solution = IdeApp.ProjectOperations.CurrentSelectedSolution;
			if (solution != null) {
				NotifyPaketDependenciesFileChanged (solution);
			}
		}

		public void NotifyPaketLockFileChanged ()
		{
			Solution solution = IdeApp.ProjectOperations.CurrentSelectedSolution;
			if (solution != null) {
				NotifyPaketLockFileChanged (solution);
			}
		}

		public void NotifyPaketDependenciesFileChanged (Solution solution)
		{
			string fileName = Path.Combine (solution.BaseDirectory, "paket.dependencies");
			NotifyFileChangedIfExists (fileName);
		}

		public void NotifyPaketLockFileChanged (Solution solution)
		{
			string fileName = Path.Combine (solution.BaseDirectory, "paket.lock");
			NotifyFileChangedIfExists (fileName);
		}

		void NotifyFileChangedIfExists (string fileName)
		{
			if (File.Exists (fileName))
				FileService.NotifyFileChanged (fileName);
		}

		public void NotifyPaketReferencesFileChanged (Project project)
		{
			string fileName = Path.Combine (project.BaseDirectory, "paket.references");
			NotifyFileChangedIfExists (fileName);
		}

		public void NotifyAllPaketFilesChangedInSolution ()
		{
			Solution solution = IdeApp.ProjectOperations.CurrentSelectedSolution;
			if (solution != null) {
				NotifyPaketDependenciesFileChanged (solution);
				NotifyPaketLockFileChanged (solution);
				NotifyPaketReferencesFilesChanged (solution);
			}
		}

		public void NotifyAllPaketAndProjectFilesChangedInSolution ()
		{
			Solution solution = IdeApp.ProjectOperations.CurrentSelectedSolution;
			if (solution != null) {
				NotifyPaketDependenciesFileChanged (solution);
				NotifyPaketLockFileChanged (solution);
				NotifyPaketReferencesFilesChanged (solution);
				NotifyProjectFilesChanged (solution);
			}
		}

		public void NotifyPaketReferencesFilesChanged (Solution solution)
		{
			foreach (Project project in solution.GetAllProjects ()) {
				NotifyPaketReferencesFileChanged (project);
			}
		}

		public void NotifyAllProjectFilesChangedInSolution ()
		{
			Solution solution = IdeApp.ProjectOperations.CurrentSelectedSolution;
			if (solution != null)
				NotifyProjectFilesChanged (solution);
		}

		void NotifyProjectFilesChanged (Solution solution)
		{
			foreach (Project project in solution.GetAllProjects ()) {
				FileService.NotifyFileChanged (project.FileName);
			}
		}
	}
}

