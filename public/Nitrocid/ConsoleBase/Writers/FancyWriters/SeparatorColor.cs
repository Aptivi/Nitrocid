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

using System;
using System.Text;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using Terminaux.Colors;

namespace KS.ConsoleBase.Writers.FancyWriters
{
    /// <summary>
    /// Separator writer
    /// </summary>
    public static class SeparatorWriterColor
    {

        /// <summary>
        /// Draw a separator with text plainly
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorPlain(string Text, params object[] Vars) =>
            WriteSeparatorPlain(Text, true, Vars);

        /// <summary>
        /// Draw a separator with text plainly
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorPlain(string Text, bool PrintSuffix, params object[] Vars)
        {
            try
            {
                TextWriterColor.WritePlain(RenderSeparatorPlain(Text, PrintSuffix, Vars));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, params object[] Vars) =>
            WriteSeparator(Text, true, Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorKernelColor(string Text, KernelColorType ColTypes, params object[] Vars) =>
            WriteSeparatorColorBack(Text, true, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorKernelColor(string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            WriteSeparatorColorBack(Text, true, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColor(string Text, ConsoleColors Color, params object[] Vars) =>
            WriteSeparatorColorBack(Text, true, new Color(Color), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColorBack(string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteSeparatorColorBack(Text, true, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColor(string Text, Color Color, params object[] Vars) =>
            WriteSeparatorColorBack(Text, true, Color, KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColorBack(string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars) =>
            WriteSeparatorColorBack(Text, true, ForegroundColor, BackgroundColor, Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, bool PrintSuffix, params object[] Vars)
        {
            try
            {
                TextWriterColor.WritePlain(RenderSeparator(Text, PrintSuffix, Vars));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorKernelColor(string Text, bool PrintSuffix, KernelColorType ColTypes, params object[] Vars) =>
            WriteSeparatorColorBack(Text, PrintSuffix, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorKernelColor(string Text, bool PrintSuffix, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            WriteSeparatorColorBack(Text, PrintSuffix, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColor(string Text, bool PrintSuffix, ConsoleColors Color, params object[] Vars) =>
            WriteSeparatorColorBack(Text, PrintSuffix, new Color(Color), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColorBack(string Text, bool PrintSuffix, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteSeparatorColorBack(Text, PrintSuffix, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColor(string Text, bool PrintSuffix, Color Color, params object[] Vars) =>
            WriteSeparatorColorBack(Text, PrintSuffix, Color, KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you have suffix on your text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparatorColorBack(string Text, bool PrintSuffix, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                TextWriterColor.WritePlain(RenderSeparator(Text, PrintSuffix, ForegroundColor, BackgroundColor, Vars));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Renders a separator with text plainly
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparatorPlain(string Text, params object[] Vars) =>
            RenderSeparatorPlain(Text, true, Vars);

        /// <summary>
        /// Renders a separator with text plainly
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparatorPlain(string Text, bool PrintSuffix, params object[] Vars)
        {
            try
            {
                var separator = new StringBuilder();
                bool canPosition = !DriverHandler.CurrentConsoleDriverLocal.IsDumb;
                Text = TextTools.FormatString(Text, Vars);

                // Print the suffix and the text
                if (!string.IsNullOrWhiteSpace(Text))
                {
                    if (PrintSuffix)
                        separator.Append("- ");
                    if (!Text.EndsWith('-'))
                        Text += " ";

                    // We need to set an appropriate color for the suffix in the text.
                    if (Text.StartsWith('-'))
                    {
                        for (int CharIndex = 0; CharIndex <= Text.Length - 1; CharIndex++)
                        {
                            if (Text[CharIndex] == '-')
                            {
                                separator.Append(Text[CharIndex]);
                            }
                            else
                            {
                                // We're (mostly) done
                                Text = Text[CharIndex..];
                                break;
                            }
                        }
                    }

                    // Render the text accordingly
                    Text = canPosition ? Text.Truncate(ConsoleWrapper.WindowWidth - 6) : Text;
                    separator.Append(Text);
                }

                // See how many times to repeat the closing minus sign. We could be running this in the wrap command.
                int RepeatTimes = 0;
                if (canPosition)
                    RepeatTimes = ConsoleWrapper.WindowWidth - (Text + " ").Length - 1;

                // Write the closing minus sign.
                separator.Append(new string('-', RepeatTimes));
                return separator.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
            return "";
        }

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, params object[] Vars) =>
            RenderSeparator(Text, true, Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, bool PrintSuffix, params object[] Vars)
        {
            try
            {
                var separator = new StringBuilder();
                bool canPosition = !DriverHandler.CurrentConsoleDriverLocal.IsDumb;
                Text = TextTools.FormatString(Text, Vars);

                // Print the suffix and the text
                if (!string.IsNullOrWhiteSpace(Text))
                {
                    if (PrintSuffix)
                        separator.Append(
                            KernelColorTools.GetColor(KernelColorType.Separator).VTSequenceForeground +
                            "- "
                        );
                    if (!Text.EndsWith('-'))
                        Text += " ";

                    // We need to set an appropriate color for the suffix in the text.
                    if (Text.StartsWith('-'))
                    {
                        for (int CharIndex = 0; CharIndex <= Text.Length - 1; CharIndex++)
                        {
                            if (Text[CharIndex] == '-')
                            {
                                separator.Append(
                                    KernelColorTools.GetColor(KernelColorType.Separator).VTSequenceForeground +
                                    Text[CharIndex]
                                );
                            }
                            else
                            {
                                // We're (mostly) done
                                Text = Text[CharIndex..];
                                break;
                            }
                        }
                    }

                    // Render the text accordingly
                    Text = canPosition ? Text.Truncate(ConsoleWrapper.WindowWidth - 6) : Text;
                    separator.Append(
                        KernelColorTools.GetColor(KernelColorType.SeparatorText).VTSequenceForeground +
                        Text
                    );
                }

                // See how many times to repeat the closing minus sign. We could be running this in the wrap command.
                int RepeatTimes = 0;
                if (canPosition)
                    RepeatTimes = ConsoleWrapper.WindowWidth - (Text + " ").Length - 1;

                // Write the closing minus sign.
                separator.Append(
                    KernelColorTools.GetColor(KernelColorType.Separator).VTSequenceForeground +
                    new string('-', RepeatTimes)
                );
                return separator.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
            return "";
        }

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars) =>
            RenderSeparator(Text, true, ForegroundColor, BackgroundColor, Vars);

        /// <summary>
        /// Renders a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="PrintSuffix">Whether or not to print the leading suffix. Only use if you don't have suffix on your text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderSeparator(string Text, bool PrintSuffix, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                var separator = new StringBuilder();
                bool canPosition = !DriverHandler.CurrentConsoleDriverLocal.IsDumb;
                Text = TextTools.FormatString(Text, Vars);

                // Print the suffix and the text
                if (!string.IsNullOrWhiteSpace(Text))
                {
                    if (PrintSuffix)
                        separator.Append(
                            ForegroundColor.VTSequenceForeground +
                            BackgroundColor.VTSequenceBackground +
                            "- "
                        );
                    if (!Text.EndsWith('-'))
                        Text += " ";

                    // We need to set an appropriate color for the suffix in the text.
                    if (Text.StartsWith('-'))
                    {
                        for (int CharIndex = 0; CharIndex <= Text.Length - 1; CharIndex++)
                        {
                            if (Text[CharIndex] == '-')
                            {
                                separator.Append(
                                    ForegroundColor.VTSequenceForeground +
                                    BackgroundColor.VTSequenceBackground +
                                    Text[CharIndex]
                                );
                            }
                            else
                            {
                                // We're (mostly) done
                                Text = Text[CharIndex..];
                                break;
                            }
                        }
                    }

                    // Render the text accordingly
                    Text = canPosition ? Text.Truncate(ConsoleWrapper.WindowWidth - 6) : Text;
                    separator.Append(
                        ForegroundColor.VTSequenceForeground +
                        BackgroundColor.VTSequenceBackground +
                        Text
                    );
                }

                // See how many times to repeat the closing minus sign. We could be running this in the wrap command.
                int RepeatTimes = 0;
                if (canPosition)
                    RepeatTimes = ConsoleWrapper.WindowWidth - (Text + " ").Length - 1;

                // Write the closing minus sign.
                separator.Append(
                    ForegroundColor.VTSequenceForeground +
                    BackgroundColor.VTSequenceBackground +
                    new string('-', RepeatTimes) +
                    KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground +
                    KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground
                );
                return separator.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
            return "";
        }

    }
}
