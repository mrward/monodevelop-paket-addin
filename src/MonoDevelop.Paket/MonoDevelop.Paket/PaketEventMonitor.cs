﻿//
// PaketEventsMonitor.cs
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
using Paket;
using MonoDevelop.Core;

namespace MonoDevelop.Paket
{
	public class PaketEventsMonitor : IDisposable
	{
		readonly ProgressMonitor monitor;

		public PaketEventsMonitor (ProgressMonitor monitor)
		{
			this.monitor = monitor;
			Logging.@event.Publish.AddHandler (OnTrace);
		}

		public void Dispose ()
		{
			Logging.@event.Publish.RemoveHandler (OnTrace);
		}

		void OnTrace (object sender, Logging.Trace trace)
		{
			monitor.Log.WriteLine (trace.Text);
		}

		public void ReportError (ProgressMonitorStatusMessage progressMessage, Exception ex)
		{
			LoggingService.LogError ("PaketAction error.", ex);
			monitor.Log.WriteLine (ex.Message);
			monitor.ReportError (progressMessage.Error, null);
			PaketConsolePad.Show (monitor);
		}

		public void ReportResult (ProgressMonitorStatusMessage progressMessage)
		{
			monitor.ReportSuccess (progressMessage.Success);
		}
	}
}

