//
// PaketCommandLine.cs
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

using System;
using MonoDevelop.Core;
using System.IO;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Ide;

namespace MonoDevelop.Paket
{
	public class PaketCommandLine
	{
		MonoRuntimeInfo monoRuntime;
		bool isMonoRuntime;

		public PaketCommandLine (string rootPath, string arguments)
			: this (
				rootPath,
				arguments,
				MonoRuntimeInfo.FromCurrentRuntime (),
				!Platform.IsWindows)
		{
		}

		public PaketCommandLine (
			string rootPath,
			string arguments,
			MonoRuntimeInfo monoRuntime,
			bool isMonoRuntime)
		{
			this.monoRuntime = monoRuntime;
			this.isMonoRuntime = isMonoRuntime;
			RootPath = rootPath;

			GenerateCommandLine (arguments);
			GenerateWorkingDirectory ();
		}

		public static PaketCommandLine CreateInitCommandLine ()
		{
			return CreateCommandLine ("init");
		}

		public static PaketCommandLine CreateCommandLine (string arguments)
		{
			FilePath solutionDirectory = IdeApp.ProjectOperations.CurrentSelectedSolution.BaseDirectory;
			return new PaketCommandLine (solutionDirectory, arguments);
		}

		public string RootPath { get; private set; }
		public string Command { get; set; }
		public string Arguments { get; private set; }
		public string WorkingDirectory { get; private set; }

		void GenerateCommandLine (string arguments)
		{
			if (isMonoRuntime) {
				GenerateMonoCommandLine (arguments);
			} else {
				GenerateWindowsCommandLine (arguments);
			}
		}

		void GenerateMonoCommandLine (string arguments)
		{
			Arguments = String.Format(
				"--runtime=v4.0 \"{0}\" {1}",
				PaketApplicationPath.GetPath (),
				arguments);

			Command = Path.Combine (monoRuntime.Prefix, "bin", "mono");
		}

		void GenerateWindowsCommandLine (string arguments)
		{
			Arguments = arguments;
			Command = PaketApplicationPath.GetPath ().ToString ();
		}

		void GenerateWorkingDirectory ()
		{
			WorkingDirectory = RootPath;
		}

		public override string ToString()
		{
			return String.Format("{0} {1}", GetQuotedCommand (), Arguments);
		}

		string GetQuotedCommand ()
		{
			if (Command.Contains (" ")) {
				return String.Format ("\"{0}\"", Command);
			}
			return Command;
		}
	}
}

