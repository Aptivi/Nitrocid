//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Nitrocid.ConsoleBase.Inputs;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using System.Globalization;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class ListCultures : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Lists supported cultures");
        public override TestSection TestSection => TestSection.Languages;
        public override void Run(params string[] args)
        {
            string Text = InputTools.ReadLine(Translate.DoTranslation("Write a search term:") + " ");
            var Cults = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (CultureInfo Cult in Cults)
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    if (Cult.Name.ToLower().Contains(Text.ToLower()) | Cult.EnglishName.ToLower().Contains(Text.ToLower()))
                    {
                        TextWriterColor.Write("{0}: {1}", Cult.Name, Cult.EnglishName);
                    }
                }
                else
                {
                    TextWriterColor.Write("{0}: {1}", Cult.Name, Cult.EnglishName);
                }
            }
        }
    }
}
