
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files.Folders;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;

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
            Kernel.Kernel.KernelEventManager.RaiseThemeStudioStarted();
            DebugWriter.WriteDebug(DebugLevel.I, "Starting theme studio with theme name {0}", ThemeName);
            string Response;
            int MaximumOptions = 38 + 9; // Colors + options
            var StudioExiting = default(bool);

            while (!StudioExiting)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Studio not exiting yet. Populating {0} options...", MaximumOptions);
                ConsoleWrapper.Clear();
                TextWriterColor.Write(Translate.DoTranslation("Making a new theme \"{0}\".") + Kernel.Kernel.NewLine, true, ColorTools.ColTypes.Neutral, ThemeName);

                // List options
                TextWriterColor.Write("1) " + Translate.DoTranslation("Input color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedInputColor.PlainSequence);
                TextWriterColor.Write("2) " + Translate.DoTranslation("License color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedLicenseColor.PlainSequence);
                TextWriterColor.Write("3) " + Translate.DoTranslation("Continuable kernel error color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedContKernelErrorColor.PlainSequence);
                TextWriterColor.Write("4) " + Translate.DoTranslation("Uncontinuable kernel error color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedUncontKernelErrorColor.PlainSequence);
                TextWriterColor.Write("5) " + Translate.DoTranslation("Host name color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedHostNameShellColor.PlainSequence);
                TextWriterColor.Write("6) " + Translate.DoTranslation("User name color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedUserNameShellColor.PlainSequence);
                TextWriterColor.Write("7) " + Translate.DoTranslation("Background color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedBackgroundColor.PlainSequence);
                TextWriterColor.Write("8) " + Translate.DoTranslation("Neutral text color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedNeutralTextColor.PlainSequence);
                TextWriterColor.Write("9) " + Translate.DoTranslation("List entry color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedListEntryColor.PlainSequence);
                TextWriterColor.Write("10) " + Translate.DoTranslation("List value color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedListValueColor.PlainSequence);
                TextWriterColor.Write("11) " + Translate.DoTranslation("Stage color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedStageColor.PlainSequence);
                TextWriterColor.Write("12) " + Translate.DoTranslation("Error color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedErrorColor.PlainSequence);
                TextWriterColor.Write("13) " + Translate.DoTranslation("Warning color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedWarningColor.PlainSequence);
                TextWriterColor.Write("14) " + Translate.DoTranslation("Option color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools._SelectedOptionColor.PlainSequence);
                TextWriterColor.Write("15) " + Translate.DoTranslation("Banner color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedBannerColor.PlainSequence);
                TextWriterColor.Write("16) " + Translate.DoTranslation("Notification title color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedNotificationTitleColor.PlainSequence);
                TextWriterColor.Write("17) " + Translate.DoTranslation("Notification description color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedNotificationDescriptionColor.PlainSequence);
                TextWriterColor.Write("18) " + Translate.DoTranslation("Notification progress color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedNotificationProgressColor.PlainSequence);
                TextWriterColor.Write("19) " + Translate.DoTranslation("Notification failure color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedNotificationFailureColor.PlainSequence);
                TextWriterColor.Write("20) " + Translate.DoTranslation("Question color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedQuestionColor.PlainSequence);
                TextWriterColor.Write("21) " + Translate.DoTranslation("Success color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedSuccessColor.PlainSequence);
                TextWriterColor.Write("22) " + Translate.DoTranslation("User dollar color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedUserDollarColor.PlainSequence);
                TextWriterColor.Write("23) " + Translate.DoTranslation("Tip color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedTipColor.PlainSequence);
                TextWriterColor.Write("24) " + Translate.DoTranslation("Separator text color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedSeparatorTextColor.PlainSequence);
                TextWriterColor.Write("25) " + Translate.DoTranslation("Separator color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedSeparatorColor.PlainSequence);
                TextWriterColor.Write("26) " + Translate.DoTranslation("List title color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedListTitleColor.PlainSequence);
                TextWriterColor.Write("27) " + Translate.DoTranslation("Development warning color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedDevelopmentWarningColor.PlainSequence);
                TextWriterColor.Write("28) " + Translate.DoTranslation("Stage time color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedStageTimeColor.PlainSequence);
                TextWriterColor.Write("29) " + Translate.DoTranslation("Progress color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedProgressColor.PlainSequence);
                TextWriterColor.Write("30) " + Translate.DoTranslation("Back option color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedBackOptionColor.PlainSequence);
                TextWriterColor.Write("31) " + Translate.DoTranslation("Low priority border color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedLowPriorityBorderColor.PlainSequence);
                TextWriterColor.Write("32) " + Translate.DoTranslation("Medium priority border color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedMediumPriorityBorderColor.PlainSequence);
                TextWriterColor.Write("33) " + Translate.DoTranslation("High priority border color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedHighPriorityBorderColor.PlainSequence);
                TextWriterColor.Write("34) " + Translate.DoTranslation("Table separator color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedTableSeparatorColor.PlainSequence);
                TextWriterColor.Write("35) " + Translate.DoTranslation("Table header color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedTableHeaderColor.PlainSequence);
                TextWriterColor.Write("36) " + Translate.DoTranslation("Table value color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedTableValueColor.PlainSequence);
                TextWriterColor.Write("37) " + Translate.DoTranslation("Selected option color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedSelectedOptionColor.PlainSequence);
                TextWriterColor.Write("38) " + Translate.DoTranslation("Alternative option color") + ": [{0}] ", true, ColorTools.ColTypes.Option, ThemeStudioTools.SelectedAlternativeOptionColor.PlainSequence);
                TextWriterColor.Write();

                // List saving and loading options
                TextWriterColor.Write("39) " + Translate.DoTranslation("Save Theme to Current Directory"), true, ColorTools.ColTypes.AlternativeOption);
                TextWriterColor.Write("40) " + Translate.DoTranslation("Save Theme to Another Directory..."), true, ColorTools.ColTypes.AlternativeOption);
                TextWriterColor.Write("41) " + Translate.DoTranslation("Save Theme to Current Directory as..."), true, ColorTools.ColTypes.AlternativeOption);
                TextWriterColor.Write("42) " + Translate.DoTranslation("Save Theme to Another Directory as..."), true, ColorTools.ColTypes.AlternativeOption);
                TextWriterColor.Write("43) " + Translate.DoTranslation("Load Theme From File..."), true, ColorTools.ColTypes.AlternativeOption);
                TextWriterColor.Write("44) " + Translate.DoTranslation("Load Theme From Prebuilt Themes..."), true, ColorTools.ColTypes.AlternativeOption);
                TextWriterColor.Write("45) " + Translate.DoTranslation("Load Current Colors"), true, ColorTools.ColTypes.AlternativeOption);
                TextWriterColor.Write("46) " + Translate.DoTranslation("Preview..."), true, ColorTools.ColTypes.AlternativeOption);
                TextWriterColor.Write("47) " + Translate.DoTranslation("Exit"), true, ColorTools.ColTypes.AlternativeOption);
                TextWriterColor.Write();

                // Prompt user
                DebugWriter.WriteDebug(DebugLevel.I, "Waiting for user input...");
                TextWriterColor.Write("> ", false, ColorTools.ColTypes.Input);
                Response = Input.ReadLine();
                DebugWriter.WriteDebug(DebugLevel.I, "Got response: {0}", Response);

                // Check for response integrity
                if (StringQuery.IsStringNumeric(Response))
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
                            case 1: // Input color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedInputColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedInputColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 2: // License color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedLicenseColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedLicenseColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 3: // Continuable kernel error color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedContKernelErrorColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedContKernelErrorColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 4: // Uncontinuable kernel error color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedUncontKernelErrorColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedUncontKernelErrorColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 5: // Host name color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedHostNameShellColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedHostNameShellColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 6: // User name color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedUserNameShellColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedUserNameShellColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 7: // Background color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedBackgroundColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedBackgroundColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 8: // Neutral text color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedNeutralTextColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedNeutralTextColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 9: // list entry color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedListEntryColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedListEntryColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 10: // list value color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedListValueColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedListValueColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 11: // Stage color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedStageColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedStageColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 12: // Error color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedErrorColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedErrorColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 13: // Warning color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedWarningColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedWarningColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 14: // Option color
                                {
                                    SelectedColorInstance = ThemeStudioTools._SelectedOptionColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools._SelectedOptionColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 15: // Banner color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedBannerColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedBannerColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 16: // Notification title color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedNotificationTitleColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedNotificationTitleColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 17: // Notification description color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedNotificationDescriptionColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedNotificationDescriptionColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 18: // Notification progress color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedNotificationProgressColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedNotificationProgressColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 19: // Notification failure color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedNotificationFailureColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedNotificationFailureColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 20: // Question color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedQuestionColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedQuestionColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 21: // Success color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedSuccessColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedSuccessColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 22: // User dollar color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedUserDollarColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedUserDollarColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 23: // Tip color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedTipColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedTipColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 24: // Separator text color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedSeparatorTextColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedSeparatorTextColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 25: // Separator color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedSeparatorColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedSeparatorColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 26: // List title color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedListTitleColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedListTitleColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 27: // Development warning color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedDevelopmentWarningColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedDevelopmentWarningColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 28: // Stage time color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedStageTimeColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedStageTimeColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 29: // Progress color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedProgressColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedProgressColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 30: // Back option color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedBackOptionColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedBackOptionColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 31: // Low priority border color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedLowPriorityBorderColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedLowPriorityBorderColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 32: // Medium priority border color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedMediumPriorityBorderColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedMediumPriorityBorderColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 33: // High priority border color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedHighPriorityBorderColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedHighPriorityBorderColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 34: // Table separator color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedTableSeparatorColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedTableSeparatorColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 35: // Table header color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedTableHeaderColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedTableHeaderColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 36: // Table value color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedTableValueColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedTableValueColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 37: // Selected option color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedSelectedOptionColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedSelectedOptionColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 38: // Selected option color
                                {
                                    SelectedColorInstance = ThemeStudioTools.SelectedAlternativeOptionColor;
                                    string ColorWheelReturn = ColorWheelOpen.ColorWheel(SelectedColorInstance.Type == ColorType.TrueColor, (ConsoleColors)Convert.ToInt32(SelectedColorInstance.Type == ColorType._255Color ? SelectedColorInstance.PlainSequence : global::ColorSeq.ConsoleColors.White), SelectedColorInstance.R, SelectedColorInstance.G, SelectedColorInstance.B);
                                    ThemeStudioTools.SelectedAlternativeOptionColor = new Color(ColorWheelReturn);
                                    break;
                                }
                            case 39: // Save theme to current directory
                                {
                                    ThemeStudioTools.SaveThemeToCurrentDirectory(ThemeName);
                                    break;
                                }
                            case 40: // Save theme to another directory...
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for directory name...");
                                    TextWriterColor.Write(Translate.DoTranslation("Specify directory to save theme to:") + " [{0}] ", false, ColorTools.ColTypes.Input, CurrentDirectory.CurrentDir);
                                    string DirectoryName = Input.ReadLine(false);
                                    DirectoryName = string.IsNullOrWhiteSpace(DirectoryName) ? CurrentDirectory.CurrentDir : DirectoryName;
                                    DebugWriter.WriteDebug(DebugLevel.I, "Got directory name {0}.", DirectoryName);
                                    ThemeStudioTools.SaveThemeToAnotherDirectory(ThemeName, DirectoryName);
                                    break;
                                }
                            case 41: // Save theme to current directory as...
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                                    TextWriterColor.Write(Translate.DoTranslation("Specify theme name:") + " [{0}] ", false, ColorTools.ColTypes.Input, ThemeName);
                                    string AltThemeName = Input.ReadLine(false);
                                    AltThemeName = string.IsNullOrWhiteSpace(AltThemeName) ? ThemeName : AltThemeName;
                                    DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
                                    ThemeStudioTools.SaveThemeToCurrentDirectory(AltThemeName);
                                    break;
                                }
                            case 42: // Save theme to another directory as...
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme and directory name...");
                                    TextWriterColor.Write(Translate.DoTranslation("Specify directory to save theme to:") + " [{0}] ", false, ColorTools.ColTypes.Input, CurrentDirectory.CurrentDir);
                                    string DirectoryName = Input.ReadLine(false);
                                    DirectoryName = string.IsNullOrWhiteSpace(DirectoryName) ? CurrentDirectory.CurrentDir : DirectoryName;
                                    DebugWriter.WriteDebug(DebugLevel.I, "Got directory name {0}.", DirectoryName);
                                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                                    TextWriterColor.Write(Translate.DoTranslation("Specify theme name:") + " [{0}] ", false, ColorTools.ColTypes.Input, ThemeName);
                                    string AltThemeName = Input.ReadLine(false);
                                    AltThemeName = string.IsNullOrWhiteSpace(AltThemeName) ? ThemeName : AltThemeName;
                                    DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
                                    ThemeStudioTools.SaveThemeToAnotherDirectory(AltThemeName, DirectoryName);
                                    break;
                                }
                            case 43: // Load Theme From File...
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                                    TextWriterColor.Write(Translate.DoTranslation("Specify theme file name wihout the .json extension:") + " ", false, ColorTools.ColTypes.Input);
                                    string AltThemeName = Input.ReadLine(false) + ".json";
                                    DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
                                    ThemeStudioTools.LoadThemeFromFile(AltThemeName);
                                    break;
                                }
                            case 44: // Load Theme From Prebuilt Themes...
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                                    TextWriterColor.Write(Translate.DoTranslation("Specify theme name:") + " ", false, ColorTools.ColTypes.Input);
                                    string AltThemeName = Input.ReadLine(false);
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
                                    ThemeStudioTools.PreparePreview();
                                    break;
                                }
                            case 47: // Exit
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Exiting studio...");
                                    StudioExiting = true;
                                    break;
                                }
                        }
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Option is not valid. Returning...");
                        TextWriterColor.Write(Translate.DoTranslation("Specified option {0} is invalid."), true, ColorTools.ColTypes.Error, NumericResponse);
                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                        ConsoleWrapper.ReadKey();
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Answer is not numeric.");
                    TextWriterColor.Write(Translate.DoTranslation("The answer must be numeric."), true, ColorTools.ColTypes.Error);
                    TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                    ConsoleWrapper.ReadKey();
                }
            }

            // Raise event
            Kernel.Kernel.KernelEventManager.RaiseThemeStudioExit();
        }

    }
}
