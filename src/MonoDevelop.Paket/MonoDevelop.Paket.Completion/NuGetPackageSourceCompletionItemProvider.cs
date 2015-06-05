//
// NuGetPackageSourceCompletionItemProvider.cs
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
using System.Linq;
using Microsoft.FSharp.Collections;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Projects;
using Paket;

namespace MonoDevelop.Paket.Completion
{
	public class NuGetPackageSourceCompletionItemProvider
	{
		public ICompletionDataList GenerateCompletionItems (FilePath paketDependenciesFileName)
		{
			try {
				Solution solution = IdeApp.ProjectOperations.CurrentSelectedSolution;
				if (solution == null)
					return null;

				FSharpList<string> feeds = Dependencies.Locate (paketDependenciesFileName.ParentDirectory)
					.GetDefinedNuGetFeeds ();

				var completionData = new CompletionDataList ();
				completionData.AddRange (feeds);
				return completionData;
			} catch (Exception ex) {
				LoggingService.LogError ("Unable to provide NuGet package source completion items.", ex);
			}

			return null;
		}
	}
}

