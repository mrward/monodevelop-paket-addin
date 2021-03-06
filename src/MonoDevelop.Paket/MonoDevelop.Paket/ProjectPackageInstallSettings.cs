﻿//
// ProjectPackageInstallSettings.cs
//
// Author:
//       Matt Ward <matt.ward@microsoft.com>
//
// Copyright (c) 2018 Microsoft
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

using System;
using System.Collections.Generic;
using System.Linq;
using Paket;

namespace MonoDevelop.Paket
{
	public class ProjectPackageInstallSettings
	{
		public ProjectPackageInstallSettings (Exception ex)
		: this (Enumerable.Empty<PackageInstallSettings> ())
		{
			Error = ex;
		}

		public ProjectPackageInstallSettings (
			IEnumerable<PackageInstallSettings> installSettings)
		{
			InstallSettings = installSettings;
		}

		public ProjectPackageInstallSettings ()
			: this (Enumerable.Empty<PackageInstallSettings> ())
		{
		}

		public IEnumerable<PackageInstallSettings> InstallSettings { get; private set; }

		public bool HasError {
			get { return Error != null; }
		}

		public string ErrorMessage {
			get {
				if (Error != null) {
					return Error.Message;
				}
				return string.Empty;
			}
		}

		public Exception Error { get; private set; }
	}
}
