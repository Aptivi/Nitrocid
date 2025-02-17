﻿//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using System.Linq;
using System.Text;
using Terminaux.Writer.CyclicWriters;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class ListCodepages : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Lists all supported codepages");
        public override TestSection TestSection => TestSection.Languages;
        public override void Run(params string[] args)
        {
            string[] Encodings = Encoding.GetEncodings().Select((ei) => $"[{ei.CodePage}] {ei.Name}: {ei.DisplayName}").ToArray();
            var listing = new Listing()
            {
                Objects = Encodings,
                KeyColor = KernelColorTools.GetColor(KernelColorType.ListEntry),
                ValueColor = KernelColorTools.GetColor(KernelColorType.ListValue),
            };
            TextWriterRaw.WriteRaw(listing.Render());
        }
    }
}
