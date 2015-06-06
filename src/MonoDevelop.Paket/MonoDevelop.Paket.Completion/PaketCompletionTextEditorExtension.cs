//
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
using System.Text;
using System;

namespace MonoDevelop.Paket.Completion
{
	public class PaketCompletionTextEditorExtension : CompletionTextEditorExtension
	{
		PaketDependencyFileLineParser parser = new PaketDependencyFileLineParser ();

		public override ICompletionDataList HandleCodeCompletion (CodeCompletionContext completionContext, char completionChar, ref int triggerWordLength)
		{
			PaketCompletionContext context = GetCompletionContext (completionContext);
			triggerWordLength = context.TriggerWordLength;
			if (context.CompletionType == PaketCompletionType.Keyword) {
				var provider = new PaketKeywordCompletionItemProvider ();
				return provider.GenerateCompletionItems ();
			} else if (context.CompletionType == PaketCompletionType.NuGetPackageSource) {
				var provider = new NuGetPackageSourceCompletionItemProvider ();
				return provider.GenerateCompletionItems (Editor.FileName);
			} else if (context.CompletionType == PaketCompletionType.KeywordValue) {
				var provider = new PaketKeywordValueCompletionItemProvider ();
				return provider.GenerateCompletionItems (context.Keyword);
			}
			return null;
		}

		PaketCompletionContext GetCompletionContext (CodeCompletionContext completionContext)
		{
			PaketDependencyFileLineParseResult result = ParseLine (completionContext);
			if (result == null)
				return PaketCompletionContext.None;

			if (result.IsComment)
				return PaketCompletionContext.None;

			if (result.IsCurrentItemFirstKeywordValue) {
				if (result.IsSourceRule ()) {
					return new PaketCompletionContext {
						CompletionType = PaketCompletionType.NuGetPackageSource
					};
				} else {
					var paketContext = CreatePaketCompletionContext (completionContext, result, PaketCompletionType.KeywordValue);
					if (result.TotalItems < result.CurrentItem) {
						paketContext.TriggerWordLength = 0;
					}
					return paketContext;
				}
			} else if (result.CurrentItem > 1) {
				return PaketCompletionContext.None;
			}

			return CreatePaketCompletionContext (completionContext, result, PaketCompletionType.Keyword);
		}

		PaketCompletionContext CreatePaketCompletionContext (
			CodeCompletionContext completionContext,
			PaketDependencyFileLineParseResult result,
			PaketCompletionType completionType)
		{
			int previousWordOffset = Editor.FindPrevWordOffset (completionContext.TriggerOffset);
			return new PaketCompletionContext {
				CompletionType = completionType,
				Keyword = result.RuleName,
				TriggerWordLength = completionContext.TriggerOffset - previousWordOffset
			};
		}

		PaketDependencyFileLineParseResult ParseLine (CodeCompletionContext completionContext)
		{
			DocumentLine line = Editor.GetLineByOffset (completionContext.TriggerOffset);
			if (line == null)
				return null;

			return parser.Parse (Editor.Document, line.Offset, completionContext.TriggerOffset);
		}

		public override ICompletionDataList CodeCompletionCommand (CodeCompletionContext completionContext)
		{
			int triggerWordLength = 0;
			return HandleCodeCompletion (completionContext, '\n', ref triggerWordLength);
		}
	}
}

