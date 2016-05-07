//
// PaketSearchCategory.cs
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
using System.Threading;
using System.Threading.Tasks;
using MonoDevelop.Components.MainToolbar;
using MonoDevelop.Ide;

namespace MonoDevelop.Paket
{
	public class PaketSearchCategory : SearchCategory
	{
		string[] tags = new string[0];

		public PaketSearchCategory ()
			: base ("Paket")
		{
		}

		public override Task GetResults (
			ISearchResultCallback searchResultCallback,
			SearchPopupSearchPattern pattern,
			CancellationToken token)
		{
			if (!CanSearch (pattern.Tag))
				return Task.FromResult (0);

			return Task.Factory.StartNew (() => {
				var dataSource = new PaketSearchDataSource (pattern);
				foreach (PaketSearchResult result in dataSource.GetResults ()) {
					searchResultCallback.ReportResult (result);
				}
			});
		}

		bool CanSearch (string tag)
		{
			return IsValidTag (tag) && IsSolutionOpen ();
		}

		bool IsSolutionOpen ()
		{
			return IdeApp.ProjectOperations.CurrentSelectedSolution != null;
		}

		public override bool IsValidTag (string tag)
		{
			return tag == null;
		}

		public override string[] Tags {
			get { return tags; }
		}
	}
}

