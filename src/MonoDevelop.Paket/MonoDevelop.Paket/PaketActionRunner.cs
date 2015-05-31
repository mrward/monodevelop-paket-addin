//
// PaketActionRunner.cs
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
using MonoDevelop.Core;

namespace MonoDevelop.Paket
{
	public class PaketActionRunner
	{
		public void Run (
			ProgressMonitorStatusMessage progressMessage,
			PaketAction action)
		{
			Run (progressMessage, new [] { action });
		}

		public void Run (
			ProgressMonitorStatusMessage progressMessage,
			IEnumerable<PaketAction> actions)
		{
			RunWithProgressMonitor (progressMessage, actions.ToList ());
		}

		void RunWithProgressMonitor (
			ProgressMonitorStatusMessage progressMessage,
			IList<PaketAction> actions)
		{
			using (IProgressMonitor monitor = CreateProgressMonitor (progressMessage.Status)) {
				using (PaketEventsMonitor eventMonitor = CreateEventMonitor (monitor)) {
					try {
						monitor.BeginTask (null, actions.Count);
						RunActionsWithProgressMonitor (monitor, actions);
						eventMonitor.ReportResult (progressMessage);
					} catch (Exception ex) {
						eventMonitor.ReportError (progressMessage, ex);
					} finally {
						monitor.EndTask ();
					}
				}
			}
		}

		void RunActionsWithProgressMonitor (IProgressMonitor monitor, IList<PaketAction> actions)
		{
			foreach (PaketAction action in actions) {
				action.Run ();
				monitor.Step (1);
			}
		}

		IProgressMonitor CreateProgressMonitor (string status)
		{
			var factory = new ProgressMonitorFactory ();
			return factory.CreateProgressMonitor (status);
		}

		PaketEventsMonitor CreateEventMonitor (IProgressMonitor monitor)
		{
			return new PaketEventsMonitor (monitor);
		}
	}
}

