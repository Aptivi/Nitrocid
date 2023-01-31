
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

using System;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Drivers;
using KS.Drivers.Console.Consoles;
using KS.Kernel;
using KS.Misc.Writers.ConsoleWriters;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Writers.FancyWriters
{
    /// <summary>
    /// Separator writer
    /// </summary>
    public static class SeparatorWriterColor
    {

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, params object[] Vars)
        {
            bool canPosition = !DriverHandler.CurrentConsoleDriver.IsDumb;

            // Print the suffix and the text
            if (!string.IsNullOrWhiteSpace(Text))
            {
                if (PrintSuffix)
                    TextWriterColor.Write("- ", false, KernelColorType.Separator, Vars);
                if (!Text.EndsWith("-"))
                    Text += " ";

                // We need to set an appropriate color for the suffix in the text.
                if (Text.StartsWith("-"))
                {
                    for (int CharIndex = 0; CharIndex <= Text.Length - 1; CharIndex++)
                    {
                        if (Convert.ToString(Text[CharIndex]) == "-")
                        {
                            TextWriterColor.Write(Convert.ToString(Text[CharIndex]), false, KernelColorType.Separator);
                        }
                        else
                        {
                            // We're (mostly) done
                            Text = Text.Substring(CharIndex);
                            break;
                        }
                    }
                }

                // Render the text accordingly
                Text = canPosition ? Text.Truncate(ConsoleWrapper.WindowWidth - 6) : Text;
                TextWriterColor.Write(Text, false, KernelColorType.SeparatorText, Vars);
            }

            // See how many times to repeat the closing minus sign. We could be running this in the wrap command.
            int RepeatTimes = 0;
            if (canPosition)
                RepeatTimes = ConsoleWrapper.WindowWidth - (Text + " ").Length - 1;

            // Write the closing minus sign.
            int OldTop = ConsoleWrapper.CursorTop;
            TextWriterColor.Write("-".Repeat(RepeatTimes), true, KernelColorType.Separator);

            // Fix CursorTop value on Unix systems. Mono...
            if (KernelPlatform.IsOnUnix() && canPosition)
            {
                if (!(ConsoleWrapper.CursorTop == ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleWrapper.WindowHeight - 3)
                    ConsoleWrapper.CursorTop -= 1;
            }
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, KernelColorType ColTypes, params object[] Vars) =>
            WriteSeparator(Text, PrintSuffix, ColorTools.GetColor(ColTypes), ColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            WriteSeparator(Text, PrintSuffix, ColorTools.GetColor(colorTypeForeground), ColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, ConsoleColors Color, params object[] Vars) =>
            WriteSeparator(Text, PrintSuffix, new Color(Color), ColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteSeparator(Text, PrintSuffix, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, Color Color, params object[] Vars) =>
            WriteSeparator(Text, PrintSuffix, Color, ColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            bool canPosition = !DriverHandler.CurrentConsoleDriver.IsDumb;

            // Print the suffix and the text
            if (!string.IsNullOrWhiteSpace(Text))
            {
                if (PrintSuffix)
                    Text = "- " + Text;
                if (!Text.EndsWith("-"))
                    Text += " ";

                // Render the text accordingly
                Text = canPosition ? Text.Truncate(ConsoleWrapper.WindowWidth - 6) : Text;
                TextWriterColor.Write(Text, false, ForegroundColor, BackgroundColor, Vars);
            }

            // See how many times to repeat the closing minus sign. We could be running this in the wrap command.
            int RepeatTimes = 0;
            if (canPosition)
                RepeatTimes = ConsoleWrapper.WindowWidth - (Text + " ").Length + 1;

            // Write the closing minus sign.
            int OldTop = ConsoleWrapper.CursorTop;
            TextWriterColor.Write("-".Repeat(RepeatTimes), true, ForegroundColor, BackgroundColor);

            // Fix CursorTop value on Unix systems. Mono...
            if (KernelPlatform.IsOnUnix() && canPosition)
            {
                if (!(ConsoleWrapper.CursorTop == ConsoleWrapper.WindowHeight - 1) | OldTop == ConsoleWrapper.WindowHeight - 3)
                    ConsoleWrapper.CursorTop -= 1;
            }
        }

    }
}
