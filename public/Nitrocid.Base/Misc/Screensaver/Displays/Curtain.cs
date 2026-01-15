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

using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Users.Login;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Misc.Screensaver.Displays
{
    /// <summary>
    /// Display code for Curtain
    /// </summary>
    public class CurtainDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Curtain";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Render the selected page of the login screen
            var canvases = ModernLogonScreen.GetLogonPages();
            int maxLogonScreens = canvases.Count + 1;

            // Check the page number
            int finalPage = Config.SaverConfig.CurtainPageNumber;
            if (finalPage > maxLogonScreens)
                finalPage = maxLogonScreens;

            // Now, render it to the screen
            string renderedScreen = ModernLogonScreen.PrintConfiguredLogonScreen(finalPage, canvases, true);
            TextWriterRaw.WriteRaw(renderedScreen);
            ScreensaverManager.Delay(Config.SaverConfig.CurtainDelay);
        }

    }
}
