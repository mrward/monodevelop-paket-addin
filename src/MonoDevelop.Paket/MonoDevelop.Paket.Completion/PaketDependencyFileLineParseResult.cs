//
// PaketDependencyFileLineParseResult.cs
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

namespace MonoDevelop.Paket.Completion
{
	public class PaketDependencyFileLineParseResult
	{
		readonly List<PaketDependencyRulePart> parts;

		public PaketDependencyFileLineParseResult (IEnumerable<PaketDependencyRulePart> parts, int triggerOffset)
		{
			this.parts = parts.ToList ();
			CurrentItem = this.parts.Count;

			PaketDependencyRulePart lastPart = parts.LastOrDefault ();
			if (lastPart != null) {
				if (triggerOffset > lastPart.EndOffset) {
					CurrentItem++;
				}
			}

			IsCurrentItemFirstKeywordValue = CheckCurrentItemIsFirstKeywordValue ();
		}

		bool CheckCurrentItemIsFirstKeywordValue ()
		{
			return (CurrentItem == 2) ||
				((CurrentItem == 3) && (parts[1].Text == ":"));
		}

		public static readonly PaketDependencyFileLineParseResult CommentLine = new PaketDependencyFileLineParseResult {
			IsComment = true
		};

		PaketDependencyFileLineParseResult ()
		{
		}

		public bool IsComment { get; private set; }

		public int TotalItems {
			get { return parts.Count; }
		}

		public int CurrentItem { get; private set; }

		public string RuleName {
			get {
				if (parts.Count > 0)
					return parts[0].Text;
				return string.Empty;
			}
		}

		public bool IsSourceRule ()
		{
			return string.Equals (RuleName, "source", StringComparison.OrdinalIgnoreCase);
		}

		public bool IsCurrentItemFirstKeywordValue { get; private set; }
	}
}

