//
// PaketDocumentContext.cs
//
// Author:
//       Matt Ward <matt.ward@microsoft.com>
//
// Copyright (c) 2019 Microsoft
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
using System.Threading.Tasks;
using MonoDevelop.Core;
using MonoDevelop.Ide.Editor;
using MonoDevelop.Ide.Gui.Documents;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;

namespace MonoDevelop.Paket
{
	class PaketDocumentContext : DocumentContext
	{
		FileDocumentController controller;

		public PaketDocumentContext (FileDocumentController controller)
		{
			this.controller = controller;
		}

		public override string Name {
			get { return controller.DocumentTitle; }
		}

		public FilePath FileName {
			get { return controller.FilePath; }
		}

		public override Project Project {
			get { return controller.Owner as Project; }
		}

		[Obsolete]
		public override Microsoft.CodeAnalysis.Document AnalysisDocument { get; }

		[Obsolete]
		public override ParsedDocument ParsedDocument { get; }

		public override void AttachToProject (Project project)
		{
		}

		public override Microsoft.CodeAnalysis.Options.OptionSet GetOptionSet ()
		{
			return null;
		}

		[Obsolete]
		public override void ReparseDocument ()
		{
		}

		[Obsolete]
		public override Task<ParsedDocument> UpdateParseDocument ()
		{
			ParsedDocument document = null;
			return Task.FromResult (document);
		}

		public override T GetContent<T> ()
		{
			return controller.GetContent<T> ();
		}

		public override IEnumerable<T> GetContents<T> ()
		{
			return controller.GetContents<T> ();
		}
	}
}
