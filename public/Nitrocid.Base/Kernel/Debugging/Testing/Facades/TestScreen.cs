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

using Terminaux.Base.Buffered;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs.Styles.Infobox;
using System;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Inputs;
using Terminaux.Base.Extensions;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Kernel.Debugging.Testing.Facades
{
    internal class TestScreen : TestFacade
    {
        public override string TestName => LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_TESTSCREEN_DESC");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override void Run()
        {
            // Show the screen measurement sticks
            var stickScreen = new Screen();
            try
            {
                var stickScreenPart = new ScreenPart();
                stickScreenPart.AddDynamicText(() =>
                {
                    var builder = new StringBuilder();
                    builder.Append(
                        ConsolePositioning.RenderChangePosition(0, 1) +
                        ConsoleColoring.RenderSetConsoleColor(new Color(ConsoleColors.Silver), true) +
                        GenerateWidthStick() + GenerateHeightStick() +
                        ConsoleColoring.RenderResetColors()
                    );
                    return builder.ToString();
                });
                stickScreen.AddBufferedPart("Test", stickScreenPart);
                ScreenTools.SetCurrent(stickScreen);
                ScreenTools.Render();
                Input.ReadKey();
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_TESTSCREEN_FAILED") + $" {ex.Message}", new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                });
            }
            finally
            {
                ScreenTools.UnsetCurrent(stickScreen);
            }
        }

        private static string GenerateWidthStick() =>
            new(' ', ConsoleWrapper.WindowWidth);

        private static string GenerateHeightStick()
        {
            var stick = new StringBuilder();
            for (int i = 0; i < ConsoleWrapper.WindowHeight; i++)
            {
                stick.Append(CsiSequences.GenerateCsiCursorPosition(2, i));
                stick.Append(' ');
            }
            return stick.ToString();
        }
    }
}
