//
// PaketSearchDataSource.cs
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
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Components.MainToolbar;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Paket.Commands;
using Xwt.Drawing;

namespace MonoDevelop.Paket
{
	public class PaketSearchDataSource : ISearchDataSource
	{
		List<PaketSearchCommand> commands;

		public PaketSearchDataSource (SearchPopupSearchPattern searchPattern)
		{
			commands = PaketSearchCommands.FilterCommands (searchPattern.Pattern).ToList ();
		}

		public Image GetIcon (int item)
		{
			return null;
		}

		public string GetMarkup (int item, bool isSelected)
		{
			return commands [item].GetMarkup ();
		}

		public string GetDescriptionMarkup (int item, bool isSelected)
		{
			return commands [item].GetDescriptionMarkup ();
		}

		public TooltipInformation GetTooltip (int item)
		{
			return null;
		}

		public double GetWeight (int item)
		{
			return 0;
		}

		public DomRegion GetRegion (int item)
		{
			return DomRegion.Empty;
		}

		public bool CanActivate (int item)
		{
			return true;
		}

		public void Activate (int item)
		{
			commands [item].Run ();
		}

		public int ItemCount {
			get {
				return commands.Count;
			}
		}
	}
}

