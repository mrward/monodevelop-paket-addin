//
// CreatePaketDependenciesFileIfNotExistPaketAction.cs
//
// Author:
//       Matt Ward <matt.ward@xamarin.com>
//
// Copyright (c) 2015 Xamarin Inc. (http://xamarin.com)
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
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Projects;

namespace MonoDevelop.Paket
{
	public class CreatePaketDependenciesFileIfNotExistPaketAction : PaketAction
	{
		FilePath dependenciesFileName;

		public CreatePaketDependenciesFileIfNotExistPaketAction (DotNetProject project)
		{
			dependenciesFileName = project.ParentSolution.BuildPaketDependenciesFileName ();
		}

		public override void Run ()
		{
			if (!File.Exists (dependenciesFileName)) {
				// Workaround Paket bug - Cannot call Dependencies.Init since this will
				// fail if the current working directory does not match the directory for
				// the paket.dependencies file.
				// https://github.com/fsprojects/Paket/issues/2886

				var commandLine = PaketCommandLine.CreateCommandLine ("init");
				Run (commandLine);
			}
		}

		void Run (PaketCommandLine commandLine)
		{
			var operation = Runtime.ProcessService.StartConsoleProcess (
				commandLine.Command,
				commandLine.Arguments,
				commandLine.WorkingDirectory,
				GetConsole ()
			);

			operation.Task.Wait ();
		}

		OperationConsole GetConsole ()
		{
			var aggregatedMonitor = Monitor as AggregatedProgressMonitor;
			if (aggregatedMonitor == null) {
				return null;
			}

			var outputMonitor = aggregatedMonitor.LeaderMonitor as OutputProgressMonitor;
			if (outputMonitor?.Console == null) {
				return null;
			}

			return new OperationConsoleWrapper (outputMonitor.Console);
		}
	}
}

