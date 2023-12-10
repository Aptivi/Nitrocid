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

using KS.ConsoleBase;
using KS.ConsoleBase.Buffered;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles.Infobox;
using KS.Languages;
using System;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class TestScreen : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the screen feature for the console");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override void Run(params string[] args)
        {
            // Show the screen measurement sticks
            var stickScreen = new Screen();
            try
            {
                var stickScreenPart = new ScreenPart();
                stickScreenPart.Position(0, 1);
                stickScreenPart.BackgroundColor(new Color(ConsoleColors.Gray));
                stickScreenPart.AddDynamicText(GenerateWidthStick);
                stickScreenPart.AddDynamicText(GenerateHeightStick);
                stickScreenPart.AddDynamicText(() => KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground);
                stickScreenPart.AddDynamicText(() => KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground);
                stickScreen.AddBufferedPart("Test", stickScreenPart);
                ScreenTools.SetCurrent(stickScreen);
                ScreenTools.Render();
                Input.DetectKeypress();
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBoxKernelColor(Translate.DoTranslation("Screen failed to render:") + $" {ex.Message}", KernelColorType.Error);
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
