
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using System;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase.Inputs;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.ConsoleBase.Colors
{
    /// <summary>
    /// Color wheel module
    /// </summary>
    public static class ColorWheelOpen
    {

        /// <summary>
        /// Upper left corner character for color wheel
        /// </summary>
        public static string WheelUpperLeftCornerChar = "╔";
        /// <summary>
        /// Upper right corner character for color wheel
        /// </summary>
        public static string WheelUpperRightCornerChar = "╗";
        /// <summary>
        /// Lower left corner character for color wheel
        /// </summary>
        public static string WheelLowerLeftCornerChar = "╚";
        /// <summary>
        /// Lower right corner character for color wheel
        /// </summary>
        public static string WheelLowerRightCornerChar = "╝";
        /// <summary>
        /// Upper frame character for color wheel
        /// </summary>
        public static string WheelUpperFrameChar = "═";
        /// <summary>
        /// Lower frame character for color wheel
        /// </summary>
        public static string WheelLowerFrameChar = "═";
        /// <summary>
        /// Left frame character for color wheel
        /// </summary>
        public static string WheelLeftFrameChar = "║";
        /// <summary>
        /// Right frame character for color wheel
        /// </summary>
        public static string WheelRightFrameChar = "║";

        /// <summary>
        /// Initializes color wheel
        /// </summary>
        public static string ColorWheel() => ColorWheel(false, ConsoleColors.White, 0, 0, 0);

        /// <summary>
        /// Initializes color wheel
        /// </summary>
        /// <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
        public static string ColorWheel(bool TrueColor) => ColorWheel(TrueColor, ConsoleColors.White, 0, 0, 0);

        /// <summary>
        /// Initializes color wheel
        /// </summary>
        /// <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
        /// <param name="DefaultColor">The default 255-color to use</param>
        public static string ColorWheel(bool TrueColor, ConsoleColors DefaultColor) => ColorWheel(TrueColor, DefaultColor, 0, 0, 0);

        /// <summary>
        /// Initializes color wheel
        /// </summary>
        /// <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
        /// <param name="DefaultColorR">The default red color range of 0-255 to use</param>
        /// <param name="DefaultColorG">The default green color range of 0-255 to use</param>
        /// <param name="DefaultColorB">The default blue color range of 0-255 to use</param>
        public static string ColorWheel(bool TrueColor, int DefaultColorR, int DefaultColorG, int DefaultColorB) => ColorWheel(TrueColor, ConsoleColors.White, DefaultColorR, DefaultColorG, DefaultColorB);

        /// <summary>
        /// Initializes color wheel
        /// </summary>
        /// <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
        /// <param name="DefaultColor">The default 255-color to use</param>
        /// <param name="DefaultColorR">The default red color range of 0-255 to use</param>
        /// <param name="DefaultColorG">The default green color range of 0-255 to use</param>
        /// <param name="DefaultColorB">The default blue color range of 0-255 to use</param>
        public static string ColorWheel(bool TrueColor, ConsoleColors DefaultColor, int DefaultColorR, int DefaultColorG, int DefaultColorB)
        {
            var CurrentColor = DefaultColor;
            int CurrentColorR = DefaultColorR;
            int CurrentColorG = DefaultColorG;
            int CurrentColorB = DefaultColorB;
            char CurrentRange = 'R';
            var ColorWheelExiting = default(bool);
            DebugWriter.WriteDebug(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", CurrentColorR, CurrentColorG, CurrentColorB);
            DebugWriter.WriteDebug(DebugLevel.I, "Got color ({0})", CurrentColor);

            ConsoleWrapper.CursorVisible = false;
            var _DefaultColor = default(int);
            while (!ColorWheelExiting)
            {
                ConsoleWrapper.Clear();
                if (TrueColor)
                {
                    TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Select color using \"<-\" and \"->\" keys. Press ENTER to quit. Press \"i\" to insert color number manually."), true, KernelColorType.Tip);
                    TextWriterColor.Write(Translate.DoTranslation("Press \"t\" to switch to 255 color mode."), true, KernelColorType.Tip);
                    TextWriterColor.Write(Translate.DoTranslation("Press \"c\" to write full color code."), true, KernelColorType.Tip);
                    DebugWriter.WriteDebug(DebugLevel.I, "Current Range: {0}", CurrentRange);

                    // The red color level
                    var RedForeground = Convert.ToString(CurrentRange) == "R" ? new Color((int)ConsoleColors.Black) : new Color("255;0;0");
                    var RedBackground = Convert.ToString(CurrentRange) == "R" ? new Color("255;0;0") : new Color((int)ConsoleColors.Black);
                    DebugWriter.WriteDebug(DebugLevel.I, "Red foreground: {0} | Red background: {1}", RedForeground.PlainSequence, RedBackground.PlainSequence);
                    TextWriterColor.Write(CharManager.NewLine + "  ", false, KernelColorType.NeutralText);
                    TextWriterColor.Write(" < ", false, RedForeground, RedBackground);
                    TextWriterWhereColor.WriteWhere("R: {0}", (int)Math.Round((ConsoleWrapper.CursorLeft + 35 - $"R: {CurrentColorR}".Length) / 2d), ConsoleWrapper.CursorTop, true, new Color($"{CurrentColorR};0;0"), CurrentColorR);
                    TextWriterWhereColor.WriteWhere(" > ", ConsoleWrapper.CursorLeft + 32, ConsoleWrapper.CursorTop, RedForeground, RedBackground);
                    TextWriterColor.Write();

                    // The green color level
                    var GreenForeground = Convert.ToString(CurrentRange) == "G" ? new Color((int)ConsoleColors.Black) : new Color("0;255;0");
                    var GreenBackground = Convert.ToString(CurrentRange) == "G" ? new Color("0;255;0") : new Color((int)ConsoleColors.Black);
                    DebugWriter.WriteDebug(DebugLevel.I, "Green foreground: {0} | Green background: {1}", GreenForeground.PlainSequence, GreenBackground.PlainSequence);
                    TextWriterColor.Write(CharManager.NewLine + "  ", false, KernelColorType.NeutralText);
                    TextWriterColor.Write(" < ", false, GreenForeground, GreenBackground);
                    TextWriterWhereColor.WriteWhere("G: {0}", (int)Math.Round((ConsoleWrapper.CursorLeft + 35 - $"G: {CurrentColorG}".Length) / 2d), ConsoleWrapper.CursorTop, true, new Color($"0;{CurrentColorG};0"), CurrentColorG);
                    TextWriterWhereColor.WriteWhere(" > ", ConsoleWrapper.CursorLeft + 32, ConsoleWrapper.CursorTop, GreenForeground, GreenBackground);
                    TextWriterColor.Write();

                    // The blue color level
                    var BlueForeground = Convert.ToString(CurrentRange) == "B" ? new Color((int)ConsoleColors.Black) : new Color("0;0;255");
                    var BlueBackground = Convert.ToString(CurrentRange) == "B" ? new Color("0;0;255") : new Color((int)ConsoleColors.Black);
                    DebugWriter.WriteDebug(DebugLevel.I, "Blue foreground: {0} | Blue background: {1}", BlueForeground.PlainSequence, BlueBackground.PlainSequence);
                    TextWriterColor.Write(CharManager.NewLine + "  ", false, KernelColorType.NeutralText);
                    TextWriterColor.Write(" < ", false, BlueForeground, BlueBackground);
                    TextWriterWhereColor.WriteWhere("B: {0}", (int)Math.Round((ConsoleWrapper.CursorLeft + 35 - $"B: {CurrentColorB}".Length) / 2d), ConsoleWrapper.CursorTop, true, new Color($"0;0;{CurrentColorB}"), CurrentColorB);
                    TextWriterWhereColor.WriteWhere(" > ", ConsoleWrapper.CursorLeft + 32, ConsoleWrapper.CursorTop, BlueForeground, BlueBackground);
                    TextWriterColor.Write();

                    // Draw the RGB ramp
                    TextWriterWhereColor.WriteWhere(WheelUpperLeftCornerChar + WheelUpperFrameChar.Repeat(ConsoleWrapper.WindowWidth - 6) + WheelUpperRightCornerChar, 2, ConsoleWrapper.WindowHeight - 6, true, KernelColorType.Gray);
                    TextWriterWhereColor.WriteWhere(WheelLeftFrameChar + " ".Repeat(ConsoleWrapper.WindowWidth - 6) + WheelRightFrameChar, 2, ConsoleWrapper.WindowHeight - 5, true, KernelColorType.Gray);
                    TextWriterWhereColor.WriteWhere(WheelLeftFrameChar + " ".Repeat(ConsoleWrapper.WindowWidth - 6) + WheelRightFrameChar, 2, ConsoleWrapper.WindowHeight - 4, true, KernelColorType.Gray);
                    TextWriterWhereColor.WriteWhere(WheelLeftFrameChar + " ".Repeat(ConsoleWrapper.WindowWidth - 6) + WheelRightFrameChar, 2, ConsoleWrapper.WindowHeight - 3, true, KernelColorType.Gray);
                    TextWriterWhereColor.WriteWhere(WheelLowerLeftCornerChar + WheelLowerFrameChar.Repeat(ConsoleWrapper.WindowWidth - 6) + WheelLowerRightCornerChar, 2, ConsoleWrapper.WindowHeight - 2, true, KernelColorType.Gray);
                    TextWriterWhereColor.WriteWhere(" ".Repeat(ConsoleExtensions.PercentRepeat(CurrentColorR, 255, 6)), 3, ConsoleWrapper.WindowHeight - 5, true, new Color((int)ConsoleColors.Black), new Color(255, 0, 0));
                    TextWriterWhereColor.WriteWhere(" ".Repeat(ConsoleExtensions.PercentRepeat(CurrentColorG, 255, 6)), 3, ConsoleWrapper.WindowHeight - 4, true, new Color((int)ConsoleColors.Black), new Color(0, 255, 0));
                    TextWriterWhereColor.WriteWhere(" ".Repeat(ConsoleExtensions.PercentRepeat(CurrentColorB, 255, 6)), 3, ConsoleWrapper.WindowHeight - 3, true, new Color((int)ConsoleColors.Black), new Color(0, 0, 255));

                    // Show example
                    var PreviewColor = new Color($"{CurrentColorR};{CurrentColorG};{CurrentColorB}");
                    TextWriterColor.Write(CharManager.NewLine + "- Lorem ipsum dolor sit amet, consectetur adipiscing elit. ({0})", true, PreviewColor, PreviewColor.Hex);

                    // Read and get response
                    var ConsoleResponse = Input.DetectKeypress();
                    DebugWriter.WriteDebug(DebugLevel.I, "Keypress: {0}", ConsoleResponse.Key.ToString());
                    if (ConsoleResponse.Key == ConsoleKey.LeftArrow)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Decrementing number...");
                        switch (CurrentRange)
                        {
                            case 'R':
                                {
                                    if (CurrentColorR == 0)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Reached zero! Back to 255.");
                                        CurrentColorR = 255;
                                    }
                                    else
                                    {
                                        CurrentColorR -= 1;
                                        DebugWriter.WriteDebug(DebugLevel.I, "Decremented to {0}", CurrentColorR);
                                    }

                                    break;
                                }
                            case 'G':
                                {
                                    if (CurrentColorG == 0)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Reached zero! Back to 255.");
                                        CurrentColorG = 255;
                                    }
                                    else
                                    {
                                        CurrentColorG -= 1;
                                        DebugWriter.WriteDebug(DebugLevel.I, "Decremented to {0}", CurrentColorG);
                                    }

                                    break;
                                }
                            case 'B':
                                {
                                    if (CurrentColorB == 0)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Reached zero! Back to 255.");
                                        CurrentColorB = 255;
                                    }
                                    else
                                    {
                                        CurrentColorB -= 1;
                                        DebugWriter.WriteDebug(DebugLevel.I, "Decremented to {0}", CurrentColorB);
                                    }

                                    break;
                                }
                        }
                    }
                    else if (ConsoleResponse.Key == ConsoleKey.RightArrow)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Incrementing number...");
                        switch (CurrentRange)
                        {
                            case 'R':
                                {
                                    if (CurrentColorR == 255)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Reached 255! Back to zero.");
                                        CurrentColorR = 0;
                                    }
                                    else
                                    {
                                        CurrentColorR += 1;
                                        DebugWriter.WriteDebug(DebugLevel.I, "Incremented to {0}", CurrentColorR);
                                    }

                                    break;
                                }
                            case 'G':
                                {
                                    if (CurrentColorG == 255)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Reached 255! Back to zero.");
                                        CurrentColorG = 0;
                                    }
                                    else
                                    {
                                        CurrentColorG += 1;
                                        DebugWriter.WriteDebug(DebugLevel.I, "Incremented to {0}", CurrentColorG);
                                    }

                                    break;
                                }
                            case 'B':
                                {
                                    if (CurrentColorB == 255)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Reached 255! Back to zero.");
                                        CurrentColorB = 0;
                                    }
                                    else
                                    {
                                        CurrentColorB += 1;
                                        DebugWriter.WriteDebug(DebugLevel.I, "Incremented to {0}", CurrentColorB);
                                    }

                                    break;
                                }
                        }
                    }
                    else if (ConsoleResponse.Key == ConsoleKey.UpArrow)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Changing range...");
                        switch (CurrentRange)
                        {
                            case 'R':
                                {
                                    CurrentRange = 'B';
                                    break;
                                }
                            case 'G':
                                {
                                    CurrentRange = 'R';
                                    break;
                                }
                            case 'B':
                                {
                                    CurrentRange = 'G';
                                    break;
                                }
                        }
                    }
                    else if (ConsoleResponse.Key == ConsoleKey.DownArrow)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Changing range...");
                        switch (CurrentRange)
                        {
                            case 'R':
                                {
                                    CurrentRange = 'G';
                                    break;
                                }
                            case 'G':
                                {
                                    CurrentRange = 'B';
                                    break;
                                }
                            case 'B':
                                {
                                    CurrentRange = 'R';
                                    break;
                                }
                        }
                    }
                    else if (ConsoleResponse.Key == ConsoleKey.I)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Prompting for color number...");
                        switch (CurrentRange)
                        {
                            case 'R':
                                {
                                    _DefaultColor = CurrentColorR;
                                    break;
                                }
                            case 'G':
                                {
                                    _DefaultColor = CurrentColorG;
                                    break;
                                }
                            case 'B':
                                {
                                    _DefaultColor = CurrentColorB;
                                    break;
                                }
                        }
                        TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Enter color number from 0 to 255:") + " [{0}] ", 0, ConsoleWrapper.WindowHeight - 1, false, KernelColorType.Input, _DefaultColor);
                        ConsoleWrapper.CursorVisible = true;
                        string ColorNum = Input.ReadLine();
                        ConsoleWrapper.CursorVisible = false;
                        DebugWriter.WriteDebug(DebugLevel.I, "Got response: {0}", ColorNum);
                        if (StringQuery.IsStringNumeric(ColorNum))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Numeric! Checking range...");
                            if (Convert.ToDouble(ColorNum) >= 0d & Convert.ToDouble(ColorNum) <= 255d)
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "In range!");
                                switch (CurrentRange)
                                {
                                    case 'R':
                                        {
                                            DebugWriter.WriteDebug(DebugLevel.I, "Changing red color level to {0}...", ColorNum);
                                            CurrentColorR = Convert.ToInt32(ColorNum);
                                            break;
                                        }
                                    case 'G':
                                        {
                                            DebugWriter.WriteDebug(DebugLevel.I, "Changing green color level to {0}...", ColorNum);
                                            CurrentColorG = Convert.ToInt32(ColorNum);
                                            break;
                                        }
                                    case 'B':
                                        {
                                            DebugWriter.WriteDebug(DebugLevel.I, "Changing blue color level to {0}...", ColorNum);
                                            CurrentColorB = Convert.ToInt32(ColorNum);
                                            break;
                                        }
                                }
                            }
                        }
                    }
                    else if (ConsoleResponse.Key == ConsoleKey.C)
                    {
                        TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Enter color code that satisfies these formats:") + " \"RRR;GGG;BBB\" / 0-255 [{0}] ", 0, ConsoleWrapper.WindowHeight - 1, false, KernelColorType.Input, $"{CurrentColorR};{CurrentColorG};{CurrentColorB}");
                        ConsoleWrapper.CursorVisible = true;
                        string ColorSequence = Input.ReadLine();
                        ConsoleWrapper.CursorVisible = false;
                        try
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Parsing {0}...", ColorSequence);
                            var ParsedColor = new Color(ColorSequence);
                            CurrentColorR = ParsedColor.R;
                            CurrentColorG = ParsedColor.G;
                            CurrentColorB = ParsedColor.B;
                        }
                        catch (Exception ex)
                        {
                            DebugWriter.WriteDebugStackTrace(ex);
                            DebugWriter.WriteDebug(DebugLevel.E, "Possible input error: {0} ({1})", ColorSequence, ex.Message);
                        }
                    }
                    else if (ConsoleResponse.Key == ConsoleKey.T)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Switching back to 255 color...");
                        TrueColor = false;
                    }
                    else if (ConsoleResponse.Key == ConsoleKey.Enter)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Exiting...");
                        ColorWheelExiting = true;
                    }
                }
                else
                {
                    TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Select color using \"<-\" and \"->\" keys. Use arrow up and arrow down keys to select between color ranges. Press ENTER to quit. Press \"i\" to insert color number manually."), true, KernelColorType.Tip);
                    TextWriterColor.Write(Translate.DoTranslation("Press \"t\" to switch to true color mode."), true, KernelColorType.Tip);

                    // The color selection
                    TextWriterColor.Write(CharManager.NewLine + "   < ", false, KernelColorType.Gray);
                    TextWriterWhereColor.WriteWhere($"{CurrentColor} [{Convert.ToInt32((int)CurrentColor)}]", (int)Math.Round((ConsoleWrapper.CursorLeft + 38 - $"{CurrentColor} [{Convert.ToInt32((int)CurrentColor)}]".Length) / 2d), ConsoleWrapper.CursorTop, true, new Color((int)CurrentColor));
                    TextWriterWhereColor.WriteWhere(" >", ConsoleWrapper.CursorLeft + 32, ConsoleWrapper.CursorTop, KernelColorType.Gray);

                    // Show prompt
                    var PreviewColor = new Color((int)CurrentColor);
                    TextWriterColor.Write(CharManager.NewLine + CharManager.NewLine + "- Lorem ipsum dolor sit amet, consectetur adipiscing elit. ({0})", true, PreviewColor, PreviewColor.Hex);

                    // Read and get response
                    var ConsoleResponse = Input.DetectKeypress();
                    DebugWriter.WriteDebug(DebugLevel.I, "Keypress: {0}", ConsoleResponse.Key.ToString());
                    if (ConsoleResponse.Key == ConsoleKey.LeftArrow)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Decrementing number...");
                        if (CurrentColor == 0)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Reached zero! Back to 255.");
                            CurrentColor = (ConsoleColors)255;
                        }
                        else
                        {
                            CurrentColor--;
                            DebugWriter.WriteDebug(DebugLevel.I, "Decremented to {0}", CurrentColor);
                        }
                    }
                    else if (ConsoleResponse.Key == ConsoleKey.RightArrow)
                    {
                        if ((int)CurrentColor == 255)
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Reached 255! Back to zero.");
                            CurrentColor = 0;
                        }
                        else
                        {
                            CurrentColor++;
                            DebugWriter.WriteDebug(DebugLevel.I, "Incremented to {0}", CurrentColor);
                        }
                    }
                    else if (ConsoleResponse.Key == ConsoleKey.I)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Prompting for color number...");
                        TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Enter color number from 0 to 255:") + " [{0}] ", 0, ConsoleWrapper.WindowHeight - 1, KernelColorType.Input, (int)CurrentColor);
                        ConsoleWrapper.CursorVisible = true;
                        string ColorNum = Input.ReadLine();
                        ConsoleWrapper.CursorVisible = false;
                        DebugWriter.WriteDebug(DebugLevel.I, "Got response: {0}", ColorNum);
                        if (StringQuery.IsStringNumeric(ColorNum))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Numeric! Checking range...");
                            if (Convert.ToDouble(ColorNum) >= 0d & Convert.ToDouble(ColorNum) <= 255d)
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "In range! Changing color level to {0}...", ColorNum);
                                CurrentColor = (ConsoleColors)Convert.ToInt32(ColorNum);
                            }
                        }
                    }
                    else if (ConsoleResponse.Key == ConsoleKey.T)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Switching back to 255 color...");
                        TrueColor = true;
                    }
                    else if (ConsoleResponse.Key == ConsoleKey.Enter)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Exiting...");
                        ColorWheelExiting = true;
                    }
                }
            }

            ConsoleWrapper.CursorVisible = true;
            if (TrueColor)
            {
                return $"{CurrentColorR};{CurrentColorG};{CurrentColorB}";
            }
            else
            {
                return ((int)CurrentColor).ToString();
            }
        }

    }
}
