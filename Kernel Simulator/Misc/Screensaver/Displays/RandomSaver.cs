﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Terminaux.Base;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

namespace KS.Misc.Screensaver.Displays
{
    public class RandomSaverDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;

        public override string ScreensaverName { get; set; } = "RandomSaver";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;
        }

        public override void ScreensaverLogic()
        {
            int ScreensaverIndex = RandomDriver.Next(Misc.Screensaver.Screensaver.Screensavers.Count);
            string ScreensaverName = Misc.Screensaver.Screensaver.Screensavers.Keys.ElementAtOrDefault(ScreensaverIndex);
            var Screensaver = Misc.Screensaver.Screensaver.Screensavers[ScreensaverName];

            // We don't want another "random" screensaver showing up, so keep selecting until it's no longer "random"
            while (ScreensaverName == "random")
            {
                ScreensaverIndex = RandomDriver.Next(Misc.Screensaver.Screensaver.Screensavers.Count);
                ScreensaverName = Misc.Screensaver.Screensaver.Screensavers.Keys.ElementAtOrDefault(ScreensaverIndex);
                Screensaver = Misc.Screensaver.Screensaver.Screensavers[ScreensaverName];
            }

            // Run the screensaver
            ScreensaverDisplayer.DisplayScreensaver(Screensaver);
        }

    }
}