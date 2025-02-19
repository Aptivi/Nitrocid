﻿//
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

using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Threading;
using Terminaux.Base;
using System.Text;
using Nitrocid.Kernel.Debugging;
using Terminaux.Colors;
using System;
using Textify.General;
using Terminaux.Base.Buffered;
using Terminaux.Sequences.Builder;
using Nitrocid.Languages;
using Terminaux.Writer.FancyWriters;
using Terminaux.Base.Extensions;
using System.Text.RegularExpressions;
using Terminaux.Sequences;
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters;

namespace Nitrocid.ConsoleBase
{
    /// <summary>
    /// Additional routines for the console
    /// </summary>
    public static class ConsoleTools
    {
        internal static bool UseAltBuffer = true;

        /// <summary>
        /// Resets the console colors without clearing screen
        /// </summary>
        /// <param name="useKernelColors">Whether to use the kernel colors or to use the default terminal colors</param>
        public static void ResetColors(bool useKernelColors = false)
        {
            ResetBackground(useKernelColors);
            ResetForeground(useKernelColors);
        }

        /// <summary>
        /// Resets the background console color without clearing screen
        /// </summary>
        /// <param name="useKernelColors">Whether to use the kernel colors or to use the default terminal colors</param>
        public static void ResetBackground(bool useKernelColors = false)
        {
            if (useKernelColors)
                KernelColorTools.SetConsoleColor(KernelColorType.Background, Background: true);
            else
                ColorTools.ResetBackground();
        }

        /// <summary>
        /// Resets the foreground console color without clearing screen
        /// </summary>
        /// <param name="useKernelColors">Whether to use the kernel colors or to use the default terminal colors</param>
        public static void ResetForeground(bool useKernelColors = false)
        {
            if (useKernelColors)
                KernelColorTools.SetConsoleColor(KernelColorType.NeutralText);
            else
                ColorTools.ResetForeground();
        }

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
                    Translate.DoTranslation("You must be running either ConEmu or a Windows 10 command prompt with VT processing enabled.") + "\n" :
                    Translate.DoTranslation("Your terminal is {0} on {1}.") + "\n";
                return TextWriterWhereColor.RenderWhere(TextTools.FormatString(message, KernelPlatform.GetTerminalType(), KernelPlatform.GetTerminalEmulator()), 3, 1, KernelColorTools.GetColor(KernelColorType.Warning), KernelColorTools.GetColor(KernelColorType.Background));
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
                    InteriorWidth = times + 1,
                    InteriorHeight = 3,
                };
                var hueBand = new BoxFrame()
                {
                    Left = 3,
                    Top = 9,
                    InteriorWidth = times + 1,
                    InteriorHeight = 1,
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
                    TextWriterWhereColor.RenderWhereColorBack(Translate.DoTranslation("Do these ramps look right to you? They should transition smoothly.") + " <y/n>", 3, ConsoleWrapper.WindowHeight - 2, KernelColorTools.GetColor(KernelColorType.Question), KernelColorTools.GetColor(KernelColorType.Background)) +
                    KernelColorTools.GetColor(KernelColorType.Input).VTSequenceForeground;
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
            KernelColorTools.LoadBackground();
        }

        internal static string BufferChar(string text, (VtSequenceType type, Match[] sequences)[] sequencesCollections, ref int i, ref int vtSeqIdx, out bool isVtSequence)
        {
            // Before buffering the character, check to see if we're surrounded by the VT sequence. This is to work around
            // the problem in .NET 6.0 Linux that prevents it from actually parsing the VT sequences like it's supposed to
            // do in Windows.
            //
            // Windows 10, Windows 11, and higher contain cmd.exe that checks to see if we passed it the escape character
            // alone, and it tries to parse each sequence passed to it.
            //
            // Linux, on the other hand, the terminal emulator has a completely different behavior, because it just omits
            // the escape character, which results in the entire sequence being printed except the Escape \u001b key, which
            // is not the way that it's supposed to work.
            //
            // To overcome this limitation, we need to print the whole sequence to the console found by the virtual terminal
            // control sequence matcher to match how it works on Windows.
            char ch = text[i];
            string seq = "";
            bool vtSeq = false;
            foreach ((var _, var sequences) in sequencesCollections)
            {
                if (sequences.Length > 0 && sequences[vtSeqIdx].Index == i)
                {
                    // We're at an index which is the same as the captured VT sequence. Get the sequence
                    seq = sequences[vtSeqIdx].Value;
                    vtSeq = true;

                    // Raise the index in case we have the next sequence, but only if we're sure that we have another
                    if (vtSeqIdx + 1 < sequences.Length)
                        vtSeqIdx++;

                    // Raise the paragraph index by the length of the sequence
                    i += seq.Length - 1;
                }
            }
            isVtSequence = vtSeq;
            return !string.IsNullOrEmpty(seq) ? seq : ch.ToString();
        }
    }
}
