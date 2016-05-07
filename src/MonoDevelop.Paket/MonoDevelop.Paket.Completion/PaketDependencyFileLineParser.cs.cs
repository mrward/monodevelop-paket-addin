//
// PaketDependencyFileLineParser.cs
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
using MonoDevelop.Ide.Editor;

namespace MonoDevelop.Paket.Completion
{
	public class PaketDependencyFileLineParser
	{
		List<PaketDependencyRulePart> parts;

		public PaketDependencyFileLineParseResult Parse (IReadonlyTextDocument document, int offset, int lastOffset)
		{
			parts = new List<PaketDependencyRulePart> ();

			int currentOffset = offset;
			while (currentOffset < lastOffset) {
				char currentChar = document.GetCharAt (currentOffset);
				if (currentChar == ' ') {
					currentOffset++;
				} else if (currentChar == '"') {
					currentOffset = ParseString (document, currentOffset, lastOffset);
				} else if (IsCommentCharacter (currentChar) && !parts.Any ()) {
					return PaketDependencyFileLineParseResult.CommentLine;
				} else {
					currentOffset = ParsePart (document, currentOffset, lastOffset);
				}
			}

			return new PaketDependencyFileLineParseResult (parts, lastOffset);
		}

		bool IsCommentCharacter (char currentChar)
		{
			return (currentChar == '#') || (currentChar == '/');
		}

		int ParsePart (IReadonlyTextDocument document, int currentOffset, int lastOffset)
		{
			return ParsePart (document, currentOffset, lastOffset, ' ');
		}

		int ParsePart (IReadonlyTextDocument document, int currentOffset, int lastOffset, char delimiter)
		{
			int index = document.Text.IndexOf (delimiter, currentOffset + 1, lastOffset - currentOffset - 1);
			if (index >= 0) {
				parts.Add (new PaketDependencyRulePart (document, currentOffset, index));
				EnsurePartAddedForSettingsDelimiter ();
				return index + 1;
			}

			parts.Add (new PaketDependencyRulePart (document, currentOffset, lastOffset));
			EnsurePartAddedForSettingsDelimiter ();
			return lastOffset;
		}

		int ParseString (IReadonlyTextDocument document, int currentOffset, int lastOffset)
		{
			return ParsePart (document, currentOffset, lastOffset, '"');
		}

		void EnsurePartAddedForSettingsDelimiter ()
		{
			if (parts.Count != 1)
				return;

			PaketDependencyRulePart keywordPart = parts[0];
			int delimiter = keywordPart.Text.IndexOf (':');
			if (delimiter > 0) {
				parts.Clear ();
				parts.AddRange (keywordPart.SplitByDelimiter (delimiter));
			}
		}
	}
}

