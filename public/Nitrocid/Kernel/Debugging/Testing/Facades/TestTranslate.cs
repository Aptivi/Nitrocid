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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class TestTranslate : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests translating a string that exists in resources to specific language");
        public override TestSection TestSection => TestSection.Languages;
        public override int TestOptionalParameters => 2;
        public override void Run(params string[] args)
        {
            string lang = args.Length > 1 ? args[1] : "spa";
            string str = args.Length > 0 ? Translate.DoTranslation(args[0], lang) : Translate.DoTranslation("Welcome to Kernel!", lang);
            TextWriterColor.Write(str);
        }
    }
}
