//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Languages;
using System;
using System.Text;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Reader;
using Terminaux.Writer.FancyWriters;

namespace Nitrocid.Shell.Homepage
{
    /// <summary>
    /// The Nitrocid Homepage tools
    /// </summary>
    public static class HomepageTools
    {
        private static bool isOnHomepage = false;

        /// <summary>
        /// Opens The Nitrocid Homepage
        /// </summary>
        public static void OpenHomepage()
        {
            if (isOnHomepage)
                return;
            isOnHomepage = true;
            var homeScreen = new Screen();

            try
            {
                // Create a screen for the homepage
                var homeScreenBuffer = new ScreenPart();
                ScreenTools.SetCurrent(homeScreen);
                ColorTools.LoadBack();

                // Now, render the homepage
                homeScreenBuffer.AddDynamicText(() =>
                {
                    // TODO: Currently, it's a placeholder.
                    var builder = new StringBuilder();

                    // Make a master border
                    builder.Append(BorderColor.RenderBorder(0, 1, ConsoleWrapper.WindowWidth - 2, ConsoleWrapper.WindowHeight - 4, KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator)));
                    
                    // Currently, we don't need to do anything.
                    builder.Append(CenteredTextColor.RenderCentered("The Nitrocid Homepage is currently under construction. Press any key to go back to the shell. Please wait while we finish constructing this homepage for you...", KernelColorTools.GetColor(KernelColorType.Warning), 1, 1));

                    // Return the resulting homepage
                    return builder.ToString();
                });
                homeScreen.AddBufferedPart("The Nitrocid Homepage", homeScreenBuffer);

                // Render the thing and wait for a keypress
                // TODO: Currently doesn't handle any key; every key = exit.
                ScreenTools.Render();
                var key = TermReader.ReadKey();
            }
            catch (Exception ex)
            {
                ColorTools.LoadBack();
                InfoBoxColor.WriteInfoBoxColor(Translate.DoTranslation("The Nitrocid Homepage has crashed and needs to revert back to the shell.") + $": {ex.Message}", true, KernelColorTools.GetColor(KernelColorType.Error));
            }
            finally
            {
                isOnHomepage = false;
                ScreenTools.UnsetCurrent(homeScreen);
                ColorTools.LoadBack();
            }
        }
    }
}
