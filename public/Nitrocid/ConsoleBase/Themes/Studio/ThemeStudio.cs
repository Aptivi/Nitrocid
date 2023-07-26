
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
using System.Linq;
using ColorPrint.Core.Wheel;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files.Folders;
using KS.Kernel.Debugging;
using KS.Kernel.Events;
using KS.Languages;
using KS.Misc.Text;

namespace KS.ConsoleBase.Themes.Studio
{
    static class ThemeStudio
    {

        /// <summary>
        /// Starts the theme studio
        /// </summary>
        /// <param name="ThemeName">Theme name</param>
        public static void StartThemeStudio(string ThemeName)
        {
            // Inform user that we're on the studio
            EventsManager.FireEvent(EventType.ThemeStudioStarted);
            DebugWriter.WriteDebug(DebugLevel.I, "Starting theme studio with theme name {0}", ThemeName);
            ThemeStudioTools.SelectedThemeName = ThemeName;
            string Response;
            int MaximumOptions = ThemeStudioTools.SelectedColors.Count + 9; // Colors + options
            var StudioExiting = false;

            while (!StudioExiting)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Studio not exiting yet. Populating {0} options...", MaximumOptions);
                ConsoleWrapper.Clear();
                TextWriterColor.Write(Translate.DoTranslation("Making a new theme \"{0}\".") + CharManager.NewLine, ThemeName);

                // List options
                for (int key = 0; key < ThemeStudioTools.SelectedColors.Count; key++)
                    TextWriterColor.Write("{0}) " + ThemeStudioTools.SelectedColors.Keys.ElementAt(key) + ": [{1}] ", true, KernelColorType.Option, key + 1, ThemeStudioTools.SelectedColors.Values.ElementAt(key).PlainSequence);
                TextWriterColor.Write();

                // List saving and loading options
                TextWriterColor.Write("39) " + Translate.DoTranslation("Save Theme to Current Directory"), true, KernelColorType.AlternativeOption);
                TextWriterColor.Write("40) " + Translate.DoTranslation("Save Theme to Another Directory..."), true, KernelColorType.AlternativeOption);
                TextWriterColor.Write("41) " + Translate.DoTranslation("Save Theme to Current Directory as..."), true, KernelColorType.AlternativeOption);
                TextWriterColor.Write("42) " + Translate.DoTranslation("Save Theme to Another Directory as..."), true, KernelColorType.AlternativeOption);
                TextWriterColor.Write("43) " + Translate.DoTranslation("Load Theme From File..."), true, KernelColorType.AlternativeOption);
                TextWriterColor.Write("44) " + Translate.DoTranslation("Load Theme From Prebuilt Themes..."), true, KernelColorType.AlternativeOption);
                TextWriterColor.Write("45) " + Translate.DoTranslation("Load Current Colors"), true, KernelColorType.AlternativeOption);
                TextWriterColor.Write("46) " + Translate.DoTranslation("Preview..."), true, KernelColorType.AlternativeOption);
                TextWriterColor.Write("47) " + Translate.DoTranslation("Exit"), true, KernelColorType.AlternativeOption);
                TextWriterColor.Write();

                // Prompt user
                DebugWriter.WriteDebug(DebugLevel.I, "Waiting for user input...");
                TextWriterColor.Write("> ", false, KernelColorType.Input);
                Response = Input.ReadLine();
                DebugWriter.WriteDebug(DebugLevel.I, "Got response: {0}", Response);

                // Check for response integrity
                if (TextTools.IsStringNumeric(Response))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Response is numeric.");
                    int NumericResponse = Convert.ToInt32(Response);
                    DebugWriter.WriteDebug(DebugLevel.I, "Checking response...");
                    if (NumericResponse >= 1 & NumericResponse <= MaximumOptions)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Numeric response {0} is >= 1 and <= {0}.", NumericResponse, MaximumOptions);
                        Color SelectedColorInstance;
                        switch (NumericResponse)
                        {
                            case 39: // Save theme to current directory
                                {
                                    ThemeStudioTools.SaveThemeToCurrentDirectory(ThemeName);
                                    break;
                                }
                            case 40: // Save theme to another directory...
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for directory name...");
                                    TextWriterColor.Write(Translate.DoTranslation("Specify directory to save theme to:") + " [{0}] ", false, KernelColorType.Input, CurrentDirectory.CurrentDir);
                                    string DirectoryName = Input.ReadLine();
                                    DirectoryName = string.IsNullOrWhiteSpace(DirectoryName) ? CurrentDirectory.CurrentDir : DirectoryName;
                                    DebugWriter.WriteDebug(DebugLevel.I, "Got directory name {0}.", DirectoryName);
                                    ThemeStudioTools.SaveThemeToAnotherDirectory(ThemeName, DirectoryName);
                                    break;
                                }
                            case 41: // Save theme to current directory as...
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                                    TextWriterColor.Write(Translate.DoTranslation("Specify theme name:") + " [{0}] ", false, KernelColorType.Input, ThemeName);
                                    string AltThemeName = Input.ReadLine();
                                    AltThemeName = string.IsNullOrWhiteSpace(AltThemeName) ? ThemeName : AltThemeName;
                                    DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
                                    ThemeStudioTools.SaveThemeToCurrentDirectory(AltThemeName);
                                    break;
                                }
                            case 42: // Save theme to another directory as...
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme and directory name...");
                                    TextWriterColor.Write(Translate.DoTranslation("Specify directory to save theme to:") + " [{0}] ", false, KernelColorType.Input, CurrentDirectory.CurrentDir);
                                    string DirectoryName = Input.ReadLine();
                                    DirectoryName = string.IsNullOrWhiteSpace(DirectoryName) ? CurrentDirectory.CurrentDir : DirectoryName;
                                    DebugWriter.WriteDebug(DebugLevel.I, "Got directory name {0}.", DirectoryName);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                                    TextWriterColor.Write(Translate.DoTranslation("Specify theme name:") + " [{0}] ", false, KernelColorType.Input, ThemeName);
                                    string AltThemeName = Input.ReadLine();
                                    AltThemeName = string.IsNullOrWhiteSpace(AltThemeName) ? ThemeName : AltThemeName;
                                    DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
                                    ThemeStudioTools.SaveThemeToAnotherDirectory(AltThemeName, DirectoryName);
                                    break;
                                }
                            case 43: // Load Theme From File...
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                                    TextWriterColor.Write(Translate.DoTranslation("Specify theme file name wihout the .json extension:") + " ", false, KernelColorType.Input);
                                    string AltThemeName = Input.ReadLine() + ".json";
                                    DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
                                    ThemeStudioTools.LoadThemeFromFile(AltThemeName);
                                    break;
                                }
                            case 44: // Load Theme From Prebuilt Themes...
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                                    TextWriterColor.Write(Translate.DoTranslation("Specify theme name:") + " ", false, KernelColorType.Input);
                                    string AltThemeName = Input.ReadLine();
                                    DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
                                    ThemeStudioTools.LoadThemeFromResource(AltThemeName);
                                    break;
                                }
                            case 45: // Load Current Colors
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Loading current colors...");
                                    ThemeStudioTools.LoadThemeFromCurrentColors();
                                    break;
                                }
                            case 46: // Preview...
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Printing text with colors of theme...");
                                    ThemeTools.PreviewTheme(ThemeStudioTools.SelectedColors);

                                    // Pause until a key is pressed
                                    TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation("Press any key to go back."));
                                    Input.DetectKeypress();
                                    break;
                                }
                            case 47: // Exit
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Exiting studio...");
                                    StudioExiting = true;
                                    break;
                                }
                            default:
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedColors[ThemeStudioTools.SelectedColors.Keys.ElementAt(NumericResponse - 1)];
                                    string ColorWheelReturn = ColorWheel.InputForColor(SelectedColorInstance).PlainSequence;
                                    ThemeStudioTools.SelectedColors[ThemeStudioTools.SelectedColors.Keys.ElementAt(NumericResponse - 1)] = new Color(ColorWheelReturn);
                                    break;
                                }
                        }
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Option is not valid. Returning...");
                        TextWriterColor.Write(Translate.DoTranslation("Specified option {0} is invalid."), true, KernelColorType.Error, NumericResponse);
                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                        Input.DetectKeypress();
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Answer is not numeric.");
                    TextWriterColor.Write(Translate.DoTranslation("The answer must be numeric."), true, KernelColorType.Error);
                    TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                    Input.DetectKeypress();
                }
            }

            // Raise event
            EventsManager.FireEvent(EventType.ThemeStudioExit);
        }

    }
}
