//
// PaketDependenciesSyntaxMode.cs
//
// Author:
//       Matt Ward <ward.matt@gmail.com>
//
// Copyright (c) 2015 Matthew ward
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
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;

namespace MonoDevelop.Paket
{
	public class PaketDependenciesSyntaxMode : SyntaxMode
	{
		public PaketDependenciesSyntaxMode ()
		{
			var provider = new ResourceStreamProvider (typeof(PaketDependenciesSyntaxMode).Assembly, "PaketDependenciesSyntaxMode.xml");
			using (var reader = provider.Open ()) {
				SyntaxMode mode = SyntaxMode.Read (reader);
				rules = new List<Rule> (mode.Rules);
				keywords = new List<Keywords> (mode.Keywords);
				spans = mode.Spans;
				matches = mode.Matches;
				prevMarker = mode.PrevMarker;
				SemanticRules = new List<SemanticRule> (mode.SemanticRules);
				SemanticRules.Add (new HighlightUrlSemanticRule ("String"));
				keywordTable = mode.keywordTable;
				keywordTableIgnoreCase = mode.keywordTableIgnoreCase;
			}
		}
	}
}

