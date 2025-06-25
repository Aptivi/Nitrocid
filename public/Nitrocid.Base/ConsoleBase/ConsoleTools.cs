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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base;
using System.Text;
using Terminaux.Colors;
using System;
using Textify.General;
using Terminaux.Base.Buffered;
using Terminaux.Sequences.Builder;
using Terminaux.Base.Extensions;
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters.Graphical;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Threading;

namespace Nitrocid.Base.ConsoleBase
{
    internal static class ConsoleTools
    {
        internal static bool UseAltBuffer = true;

        internal static void PreviewMainBuffer()
        {
            if (KernelPlatform.IsOnWindows())
                return;
            if (!(ConsoleMisc.IsOnAltBuffer && UseAltBuffer))
                return;

            // Show the main buffer
            ConsoleMisc.ShowMainBuffer();

            // Sleep for five seconds
            ThreadManager.SleepNoBlock(5000);

            // Show the alternative buffer
            ConsoleMisc.ShowAltBuffer();
        }

        internal static void ShowColorRampAndSet()
        {
            var screen = new Screen();
            var rampPart = new ScreenPart();
            ScreenTools.SetCurrent(screen);

            // Show a tip
            rampPart.AddDynamicText(() =>
            {
                string message =
                    KernelPlatform.IsOnWindows() ?
                    LanguageTools.GetLocalized("NKS_CONSOLEBASE_TOOLS_COLORTEST_WARNING") + "\n" :
                    LanguageTools.GetLocalized("NKS_CONSOLEBASE_TOOLS_COLORTEST_INFO") + "\n";
                return TextWriterWhereColor.RenderWhere(TextTools.FormatString(message, KernelPlatform.GetTerminalType(), KernelPlatform.GetTerminalEmulator()), 3, 1, ThemeColorsTools.GetColor(ThemeColorType.Warning), ThemeColorsTools.GetColor(ThemeColorType.Background));
            });

            // Show three color bands
            rampPart.AddDynamicText(() =>
            {
                var band = new StringBuilder();

                // First, render a box
                int times = ConsoleWrapper.WindowWidth - 10;
                DebugWriter.WriteDebug(DebugLevel.I, "Band length: {0} cells", vars: [times]);
                var rgbBand = new BoxFrame()
                {
                    Left = 3,
                    Top = 3,
                    Width = times + 1,
                    Height = 3,
                };
                var hueBand = new BoxFrame()
                {
                    Left = 3,
                    Top = 9,
                    Width = times + 1,
                    Height = 1,
                };
                band.Append(
                    rgbBand.Render() +
                    hueBand.Render()
                );
                band.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCursorPosition, 5, 5));

                // Then, render the three bands, starting from the red color
                double threshold = 255 / (double)times;
                for (double i = 0; i <= times; i++)
                    band.Append($"{new Color(Convert.ToInt32(i * threshold), 0, 0).VTSequenceBackground} ");
                band.Append(ColorTools.RenderResetBackground());
                band.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCursorPosition, 5, 6));

                // The green color
                for (double i = 0; i <= times; i++)
                    band.Append($"{new Color(0, Convert.ToInt32(i * threshold), 0).VTSequenceBackground} ");
                band.Append(ColorTools.RenderResetBackground());
                band.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCursorPosition, 5, 7));

                // The blue color
                for (double i = 0; i <= times; i++)
                    band.Append($"{new Color(0, 0, Convert.ToInt32(i * threshold)).VTSequenceBackground} ");
                band.Append(ColorTools.RenderResetBackground());
                band.Append(VtSequenceBuilderTools.BuildVtSequence(VtSequenceSpecificTypes.CsiCursorPosition, 5, 11));

                // Now, show the hue band
                double hueThreshold = 360 / (double)times;
                for (double h = 0; h <= times; h++)
                    band.Append($"{new Color($"hsl:{Convert.ToInt32(h * hueThreshold)};100;50").VTSequenceBackground} ");
                band.AppendLine();
                band.Append(ColorTools.RenderResetBackground());
                return TextWriterWhereColor.RenderWhere(TextTools.FormatString(band.ToString(), KernelPlatform.GetTerminalType(), KernelPlatform.GetTerminalEmulator()), 3, 3);
            });

            // Tell the user to select either Y or N
            rampPart.AddDynamicText(() =>
            {
                return
                    TextWriterWhereColor.RenderWhereColorBack(LanguageTools.GetLocalized("NKS_CONSOLEBASE_TOOLS_COLORTEST_QUESTION") + " <y/n>", 3, ConsoleWrapper.WindowHeight - 2, ThemeColorsTools.GetColor(ThemeColorType.Question), ThemeColorsTools.GetColor(ThemeColorType.Background)) +
                    ThemeColorsTools.GetColor(ThemeColorType.Input).VTSequenceForeground;
            });
            screen.AddBufferedPart("Ramp screen part", rampPart);
            ConsoleKey answer = ConsoleKey.None;
            ScreenTools.Render();
            while (answer != ConsoleKey.N && answer != ConsoleKey.Y)
                answer = Input.ReadKey().Key;

            // Set the appropriate config
            bool supports256Color = answer == ConsoleKey.Y;
            Config.MainConfig.ConsoleSupportsTrueColor = supports256Color;

            // Clear the screen and remove the screen
            ScreenTools.UnsetCurrent(screen);
            ThemeColorsTools.LoadBackground();
        }
    }
}
