//
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
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class Windows98 : BaseBSOD
    {
        public override void Simulate()
        {
            ColorTools.LoadBackDry(new Color(ConsoleColors.DarkBlue));
            ColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // Windows header
            string headerText = " Windows ";
            int headerStartLeft = ConsoleWrapper.WindowWidth / 2 - headerText.Length / 2;
            int headerStartTop = ConsoleWrapper.WindowHeight / 2 - 5;
            TextWriterWhereColor.WriteWhereColorBack(headerText, headerStartLeft, headerStartTop, new Color(ConsoleColors.DarkBlue), new Color(ConsoleColors.White));

            // Write error
            int errorLeft = ConsoleWrapper.WindowWidth / 2 - 35;
            TextWriterWhereColor.WriteWhereColorBack("A fatal exception 0E has occurred at 0028:C0011E36 in VXD VMM(01) +\n" +
                                            "00010E36. The current application will be terminated.\n\n" +
                                            "*  Press any key to terminate the current application.\n" +
                                            "*  Press CTRL+ALT+DEL again to restart your computer. You will\n" +
                                            "   lose any unsaved information in all applications.\n\n" +
                                            "                      Press any key to continue",
                                            errorLeft, headerStartTop + 2, new Color(ConsoleColors.White), new Color(ConsoleColors.DarkBlue));
        }
    }
}
