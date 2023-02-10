
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using ColorSeq;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using System;
using KS.ConsoleBase.Colors;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class PowerLineTest : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests your console for PowerLine support");
        public override void Run()
        {
            char TransitionChar = Convert.ToChar(0xE0B0);
            char PadlockChar = Convert.ToChar(0xE0A2);
            char GitBranchChar = Convert.ToChar(0xE0A0);
            TextWriterColor.Write(Translate.DoTranslation("Be sure to use a console font supporting PowerLine glyphs, or the output may not render properly. We recommend") + " Cascadia Code/Mono PL", true, KernelColorType.Warning);
            TextWriterColor.Write(" One ", false, new Color(ConsoleColors.Black), new Color(85, 255, 255));
            TextWriterColor.Write(Convert.ToString(TransitionChar), false, new Color(85, 255, 255), new Color(255, 85, 255));
            TextWriterColor.Write(" Two ", false, new Color(ConsoleColors.Black), new Color(255, 85, 255));
            TextWriterColor.Write(Convert.ToString(TransitionChar), false, new Color(255, 85, 255), new Color(255, 255, 85));
            TextWriterColor.Write($" {PadlockChar} Secure ", false, new Color(ConsoleColors.Black), new Color(255, 255, 85));
            TextWriterColor.Write(Convert.ToString(TransitionChar), false, new Color(255, 255, 85), new Color(255, 255, 255));
            TextWriterColor.Write($" {GitBranchChar} master ", false, new Color(ConsoleColors.Black), new Color(255, 255, 255));
            TextWriterColor.Write(Convert.ToString(TransitionChar), true, new Color(255, 255, 255));
        }
    }
}
