//
// PaketDependencyRulePart.cs
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

using System.Collections.Generic;
using MonoDevelop.Ide.Editor;

namespace MonoDevelop.Paket.Completion
{
	public class PaketDependencyRulePart
	{
		public PaketDependencyRulePart (IReadonlyTextDocument document, int start, int end)
		{
			Text = document.GetTextAt (start, end - start);
			Offset = start;
			EndOffset = end;
		}

		public PaketDependencyRulePart (string text, int start, int end)
		{
			Text = text;
			Offset = start;
			EndOffset = end;
		}

		public string Text { get; private set; }
		public int Offset { get; private set; }
		public int EndOffset { get; private set; }

		public IEnumerable<PaketDependencyRulePart> SplitByDelimiter (int delimiter)
		{
			string firstPart = Text.Substring (0, delimiter);
			string delimiterPart = Text.Substring (delimiter, 1);
			string lastPart = Text.Substring (delimiter + 1);

			yield return new PaketDependencyRulePart (firstPart, Offset, Offset + delimiter - 1);
			yield return new PaketDependencyRulePart (delimiterPart, Offset + delimiter, Offset + delimiter + 1);
			if (!string.IsNullOrEmpty (lastPart))
				yield return new PaketDependencyRulePart (lastPart, Offset + delimiter + 1, EndOffset);
		}
	}
}

