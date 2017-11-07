//
// PaketCommandRunner.cs
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
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.ProgressMonitoring;

namespace MonoDevelop.Paket
{
	public class PaketCommandRunner
	{
		static Action defaultAfterRun = () => { };

		public void Run (PaketCommandLine command, ProgressMonitorStatusMessage progressMessage)
		{
			Run (command, progressMessage, defaultAfterRun);
		}

		public void Run (
			PaketCommandLine command,
			ProgressMonitorStatusMessage progressMessage,
			Action afterRun)
		{
			AggregatedProgressMonitor progressMonitor = CreateProgressMonitor (progressMessage);
			Run (command, progressMessage, progressMonitor, afterRun);
		}

		public void Run (
			PaketCommandLine command,
			ProgressMonitorStatusMessage progressMessage,
			AggregatedProgressMonitor progressMonitor)
		{
			Run (command, progressMessage, progressMonitor, defaultAfterRun);
		}

		public void Run (
			PaketCommandLine command,
			ProgressMonitorStatusMessage progressMessage,
			AggregatedProgressMonitor progressMonitor,
			Action afterRun)
		{
			try {
				Run (command, progressMonitor, progressMessage, afterRun);
			} catch (Exception ex) {
				LoggingService.LogError ("Error running paket command", ex);
				progressMonitor.Log.WriteLine (ex.Message);
				progressMonitor.ReportError (progressMessage.Error, null);
				PaketConsolePad.Show (progressMonitor);
				progressMonitor.Dispose ();
			}
		}

		AggregatedProgressMonitor CreateProgressMonitor (ProgressMonitorStatusMessage progressMessage)
		{
			var factory = new ProgressMonitorFactory ();
			return (AggregatedProgressMonitor)factory.CreateProgressMonitor (progressMessage.Status);
		}

		void Run (
			PaketCommandLine commandLine,
			AggregatedProgressMonitor progressMonitor,
			ProgressMonitorStatusMessage progressMessage,
			Action afterRun)
		{
			progressMonitor.Log.WriteLine (commandLine);

			var outputProgressMonitor = progressMonitor.LeaderMonitor as OutputProgressMonitor;
			var operationConsole = new OperationConsoleWrapper (outputProgressMonitor.Console);
			operationConsole.DisposeWrappedOperationConsole = true;

			Runtime.ProcessService.StartConsoleProcess (
				commandLine.Command,
				commandLine.Arguments,
				commandLine.WorkingDirectory,
				operationConsole,
				null,
				(sender, e) => {
					using (progressMonitor) {
						OnCommandCompleted ((ProcessAsyncOperation)sender, operationConsole, progressMonitor, progressMessage);
						afterRun ();
					}
				}
			);
		}

		void OnCommandCompleted (
			ProcessAsyncOperation operation,
			OperationConsoleWrapper operationConsole,
			AggregatedProgressMonitor progressMonitor,
			ProgressMonitorStatusMessage progressMessage)
		{
			ReportOutcome (operation, operationConsole, progressMonitor, progressMessage);
		}

		void ReportOutcome (
			ProcessAsyncOperation operation,
			OperationConsoleWrapper operationConsole,
			AggregatedProgressMonitor progressMonitor,
			ProgressMonitorStatusMessage progressMessage)
		{
			if (!operation.Task.IsFaulted && operation.ExitCode == 0 && !operationConsole.HasWrittenErrors) {
				progressMonitor.ReportSuccess (progressMessage.Success);
			} else {
				progressMonitor.ReportError (progressMessage.Error, null);
				PaketConsolePad.Show (progressMonitor);
			}
		}
	}
}

