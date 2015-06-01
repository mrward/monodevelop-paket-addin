//
// UpdatedPackagesProgressMonitorStatusMessage.cs
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
using MonoDevelop.Core;

namespace MonoDevelop.Paket
{
	public class UpdatedPackagesProgressMonitorStatusMessage : ProgressMonitorStatusMessage
	{
		public UpdatedPackagesProgressMonitorStatusMessage (string status, string success, string error, string warning)
			: base (status, success, error, warning)
		{
		}

		public int UpdatedPackagesFound { get; set; }

		protected override string GetSuccessMessage ()
		{
			if (UpdatedPackagesFound == 0) {
				return GettextCatalog.GetString ("Paket dependencies are up to date.");
			} else if (UpdatedPackagesFound == 1) {
				return GettextCatalog.GetString ("1 update found.", UpdatedPackagesFound);
			}

			return GettextCatalog.GetString ("{0} updates found.", UpdatedPackagesFound);
		}
	}
}

