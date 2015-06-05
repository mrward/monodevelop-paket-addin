﻿//
// PaketCompletionTextEditorExtension.cs
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

using Mono.TextEditor;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Ide.Gui.Content;

namespace MonoDevelop.Paket.Completion
{
	public class PaketCompletionTextEditorExtension : CompletionTextEditorExtension
	{
		public override ICompletionDataList HandleCodeCompletion (CodeCompletionContext completionContext, char completionChar, ref int triggerWordLength)
		{
			PaketCompletionContext context = GetCompletionContext (completionContext, completionChar);
			if (context.CompletionType == PaketCompletionType.Keyword) {
				triggerWordLength = context.TriggerWordLength;
				var provider = new PaketKeywordCompletionItemProvider();
				return provider.GenerateCompletionItems ();
			}
			return null;
		}

		PaketCompletionContext GetCompletionContext (CodeCompletionContext completionContext, char completionChar)
		{
			if (completionChar == ' ')
				return PaketCompletionContext.None;
			
			DocumentLine line = Editor.GetLineByOffset (completionContext.TriggerOffset);
			if (line == null)
				return PaketCompletionContext.None;

			if (!IsFirstWordOnLine (line, completionContext.TriggerOffset))
				return PaketCompletionContext.None;

			int previousWordOffset = Editor.FindPrevWordOffset (completionContext.TriggerOffset);
			return new PaketCompletionContext {
				CompletionType = PaketCompletionType.Keyword,
				TriggerWordLength = completionContext.TriggerOffset - previousWordOffset
			};
		}

		bool IsFirstWordOnLine (DocumentLine line, int triggerOffset)
		{
			int currentWordLength = 0;
			int currentOffset = line.Offset;

			while (currentOffset < triggerOffset) {
				char currentChar = Editor.GetCharAt (currentOffset);
				if (currentChar == ' ') {
					if (currentWordLength > 0) {
						return false;
					}
				} else if (char.IsLetter (currentChar) || currentChar == '_') {
					currentWordLength++;
				} else {
					return false;
				}
				currentOffset++;
			}

			return true;
		}

		public override ICompletionDataList CodeCompletionCommand (CodeCompletionContext completionContext)
		{
			PaketCompletionContext context = GetCompletionContext (completionContext, '\n');
			if (context.CompletionType == PaketCompletionType.Keyword) {
				var provider = new PaketKeywordCompletionItemProvider();
				return provider.GenerateCompletionItems ();
			}
			return null;
		}
	}
}

