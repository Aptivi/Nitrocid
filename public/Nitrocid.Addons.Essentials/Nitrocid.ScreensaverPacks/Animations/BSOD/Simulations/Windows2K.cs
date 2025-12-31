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
using Nitrocid.Base.Drivers.RNG;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Animations.BSOD.Simulations
{
    internal class Windows2K : BaseBSOD
    {
        public override void Simulate()
        {
            ColorTools.LoadBackDry(new Color(ConsoleColors.DarkBlue));
            ColorTools.SetConsoleColor(new Color(ConsoleColors.White));

            // Display technical information
            TextWriterRaw.WritePlain($"\n*** STOP: 0x0000007B (0x{RandomDriver.Random():X8}, 0x{RandomDriver.Random():X8}, 0x{RandomDriver.Random():X8}, 0x{RandomDriver.Random():X8})\n", true);

            // If this is the first time...
            TextWriterRaw.WritePlain("If this is the first time you've seen this Stop error screen,\n" +
                                       "restart your computer. If this screen appears again, follow\n" +
                                       "these steps:\n", true);

            // Display some steps
            TextWriterRaw.WritePlain("Check for viruses on your computer. Remove any newly installed.\n" +
                                       "hard drives or hard drive controllers. Check your hard drive\n" +
                                       "to make sure it is properly configured and terminated.\n" +
                                       "Run CHKDSK /F to check for hard drive corruption, and then\n" +
                                       "restart your computer.\n", true);

            // Getting Started manual reference
            TextWriterRaw.WritePlain("Refer to your Getting Started manual for more information on\n" +
                                       "troubleshooting Stop errors.", true);
        }
    }
}
