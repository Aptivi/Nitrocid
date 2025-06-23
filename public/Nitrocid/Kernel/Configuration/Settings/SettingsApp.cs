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

using System;
using System.Collections.Generic;
using System.Linq;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Security.Permissions;
using Textify.General;
using Terminaux.Base;
using Terminaux.Inputs;
using Nitrocid.Kernel.Configuration.Migration;
using Terminaux.Inputs.Interactive;
using Nitrocid.Misc.Interactives;
using Terminaux.Inputs.Styles;
using Nitrocid.Kernel.Exceptions;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Nitrocid.Kernel.Configuration.Settings
{
    /// <summary>
    /// Settings application module
    /// </summary>
    public static class SettingsApp
    {

        /// <summary>
        /// Opens the main page for settings, listing all the sections that are configurable
        /// </summary>
        /// <param name="settingsType">Type of settings</param>
        /// <param name="useSelection">Whether to use the selection style or the interactive TUI</param>
        public static void OpenMainPage(string settingsType, bool useSelection = false) =>
            OpenMainPage(Config.GetKernelConfig(settingsType), useSelection);

        /// <summary>
        /// Opens the main page for settings, listing all the sections that are configurable
        /// </summary>
        /// <param name="settingsType">Type of settings</param>
        /// <param name="useSelection">Whether to use the selection style or the interactive TUI</param>
        public static void OpenMainPage(BaseKernelConfig? settingsType, bool useSelection = false)
        {
            // Verify that we actually have the type
            if (settingsType is null)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_EXCEPTION_TYPENOTFOUND"), true, ThemeColorType.Error);
                return;
            }

            // Verify the user permission
            PermissionsTools.Demand(PermissionTypes.ManipulateSettings);

            // Decide whether to use the selection style
            if (!useSelection)
            {
                var tui = new SettingsCli
                {
                    config = settingsType,
                    lastFirstPaneIdx = -1,
                };
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_SET"), ConsoleKey.Enter, (_, entryIdx, _, keyIdx) => tui.Set(entryIdx, keyIdx)));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_SAVE"), ConsoleKey.F1, (_, _, _, _) => tui.Save()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_SAVEAS"), ConsoleKey.F2, (_, _, _, _) => tui.SaveAs()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_LOADFROM"), ConsoleKey.F3, (_, _, _, _) => tui.LoadFrom()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_RELOAD"), ConsoleKey.F4, (_, _, _, _) => tui.Reload()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_MIGRATE"), ConsoleKey.F5, (_, _, _, _) => tui.Migrate()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_CHECKUPDATES"), ConsoleKey.F6, (_, _, _, _) => tui.CheckUpdates()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SYSINFO"), ConsoleKey.F7, (_, _, _, _) => tui.SystemInfo()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_RESETALL"), ConsoleKey.F8, (_, _, _, _) => tui.ResetAll()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_KEYBINDING_RESETENTRY"), ConsoleKey.R, ConsoleModifiers.Shift, (_, entryIdx, _, keyIdx) => tui.ResetEntry(entryIdx, keyIdx)));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SELECTCONFIG"), ConsoleKey.F9, (_, _, _, _) => tui.SelectConfig()));
                InteractiveTuiTools.OpenInteractiveTui(tui);
                return;
            }

            // Now, the main loop
            bool PromptFinished = false;
            SettingsEntry[]? SettingsEntries = settingsType.SettingsEntries;
            if (SettingsEntries is null || SettingsEntries.Length == 0)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_ENTRIESNOTFOUND"), true, ThemeColorType.Error);
                return;
            }
            int MaxSections = SettingsEntries.Length;

            // TODO: We need to remove the legacy settings app in v0.2.1.
            InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_LEGACYSHUTDOWN"));
            while (!PromptFinished)
            {
                // Populate sections and alt options
                var sections = SettingsAppTools.GetSectionChoices(SettingsEntries);
                var altSections = new InputChoiceInfo[]
                {
                    new($"{MaxSections + 1}", LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_FINDANOPTION"), LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_FINDANOPTION_DESC")),
                    new($"{MaxSections + 2}", LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SAVESETTINGS"), LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SAVESETTINGS_DESC")),
                    new($"{MaxSections + 3}", LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SAVEAS"), LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SAVEAS_DESC")),
                    new($"{MaxSections + 4}", LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_LOADFROM"), LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_LOADFROM_DESC")),
                    new($"{MaxSections + 5}", LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_RELOAD"), LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_RELOAD_DESC")),
                    new($"{MaxSections + 6}", LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_CHECKUPDATES"), LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_CHECKUPDATES_DESC")),
                    new($"{MaxSections + 7}", LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SYSINFO"), LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SYSINFO_DESC")),
                    new($"{MaxSections + 8}", LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_MIGRATEOLDCONFIG"), LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_MIGRATEOLDCONFIG_DESC")),
                    new($"{MaxSections + 9}", LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SELECTCONFIG"), LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SELECTCONFIG_DESC")),
                    new($"{MaxSections + 10}", LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_EXIT")),
                };

                // Prompt for selection and check the answer
                string finalTitle = LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_HEADER");
                int Answer = SelectionStyle.PromptSelection(RenderHeader(finalTitle, TextTools.FormatString(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_HEADER_LANDING"), settingsType.GetType().Name)),
                    sections, altSections);
                if (Answer >= 1 & Answer <= MaxSections)
                {
                    // The selected answer is a section
                    InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_LOADINGSECTION"));
                    SettingsEntry SelectedSection = SettingsEntries[Answer - 1];
                    DebugWriter.WriteDebug(DebugLevel.I, "Opening section {0}...", vars: [SelectedSection.Name]);
                    OpenSection(SelectedSection.Name, SelectedSection, settingsType);
                }
                else if (Answer == MaxSections + 1)
                {
                    // The selected answer is "Find an option"
                    VariableFinder(settingsType);
                }
                else if (Answer == MaxSections + 2)
                {
                    // The selected answer is "Save settings"
                    SettingsAppTools.SaveSettings();
                }
                else if (Answer == MaxSections + 3)
                {
                    // The selected answer is "Save settings as"
                    SettingsAppTools.SaveSettingsAs();
                }
                else if (Answer == MaxSections + 4)
                {
                    // The selected answer is "Load settings from"
                    SettingsAppTools.LoadSettingsFrom(settingsType);
                }
                else if (Answer == MaxSections + 5)
                {
                    // The selected answer is "Reload settings"
                    SettingsAppTools.ReloadConfig();
                }
                else if (Answer == MaxSections + 6)
                {
                    // The selected answer is "Check for system updates"
                    SettingsAppTools.CheckForSystemUpdates();
                }
                else if (Answer == MaxSections + 7)
                {
                    // The selected answer is "System information"
                    SettingsAppTools.SystemInformation();
                }
                else if (Answer == MaxSections + 8)
                {
                    // The selected answer is "Select configuration"
                    if (!ConfigMigration.MigrateAllConfig())
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_MIGRATEOLDCONFIG_SUCCESS") + " " +
                            LanguageTools.GetLocalized("NKS_COMMON_GOBACK"), new InfoBoxSettings()
                            {
                                ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                            });
                }
                else if (Answer == MaxSections + 9)
                {
                    // The selected answer is "Migrate old configuration"
                    var selectedConfig = SettingsAppTools.SelectConfig();
                    if (selectedConfig is not null && selectedConfig.SettingsEntries is not null)
                    {
                        settingsType = selectedConfig;
                        SettingsEntries = settingsType.SettingsEntries;
                        MaxSections = SettingsEntries.Length;
                    }
                }
                else if (Answer == MaxSections + 10 || Answer == -1)
                {
                    // The selected answer is "Exit"
                    DebugWriter.WriteDebug(DebugLevel.W, "Exiting...");
                    PromptFinished = true;
                    ConsoleWrapper.Clear();
                }
                else
                {
                    // Invalid selection
                    DebugWriter.WriteDebug(DebugLevel.W, "Option is not valid. Returning...");
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_OPTIONINVALID") + " " + LanguageTools.GetLocalized("NKS_COMMON_GOBACK"), new InfoBoxSettings()
                    {
                        ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                    }, Answer);
                }
            }
        }

        /// <summary>
        /// Open section
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="SettingsSection">Settings section entry</param>
        /// <param name="settingsType">Type of settings</param>
        public static void OpenSection(string Section, SettingsEntry SettingsSection, BaseKernelConfig settingsType)
        {
            PermissionsTools.Demand(PermissionTypes.ManipulateSettings);
            try
            {
                // General variables
                bool SectionFinished = false;
                var SectionToken = SettingsSection.Keys;
                var SectionDescription = SettingsSection.Desc;
                var SectionDisplayName = SettingsSection.DisplayAs ?? Section;
                int MaxOptions = SectionToken.Length;

                while (!SectionFinished)
                {
                    // Populate sections
                    var sections = new List<InputChoiceInfo>();

                    // Check for platform compatibility
                    string Notes = "";
                    var unsupportedConfigs = SectionToken.Where((sk) => sk.Unsupported).ToArray();
                    var unsupportedConfigNames = unsupportedConfigs.Select((sk) => LanguageTools.GetLocalized(sk.Name)).ToArray();
                    bool hasUnsupportedConfigs = unsupportedConfigs.Length > 0;
                    if (hasUnsupportedConfigs)
                        Notes = LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_UNSUPPORTEDCONFIG") + $" {string.Join(", ", unsupportedConfigNames)}";

                    // Populate sections
                    for (int SectionIndex = 0; SectionIndex <= MaxOptions - 1; SectionIndex++)
                    {
                        var Setting = SectionToken[SectionIndex];
                        if (Setting.Unsupported)
                            continue;

                        // Now, populate the input choice info
                        object? CurrentValue =
                            Setting.Masked ? "***" : ConfigTools.GetValueFromEntry(Setting, settingsType);
                        string choiceName = $"{SectionIndex + 1}";
                        string choiceTitle = $"{LanguageTools.GetLocalized(Setting.Name)} [{CurrentValue}]";
                        string choiceDesc = LanguageTools.GetLocalized(Setting.Description);
                        var ici = new InputChoiceInfo(
                            choiceName,
                            choiceTitle,
                            choiceDesc
                        );
                        sections.Add(ici);
                    }
                    DebugWriter.WriteDebug(DebugLevel.W, "Section {0} has {1} selections.", vars: [Section, MaxOptions]);

                    // Populate the alt sections correctly
                    var altSections = new List<InputChoiceInfo>()
                    {
                        new($"{MaxOptions + 1}", LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_GOBACK"))
                    };

                    // Prompt user and check for input
                    string finalSection = LanguageTools.GetLocalized(SectionDisplayName);
                    int Answer = SelectionStyle.PromptSelection(RenderHeader(finalSection, LanguageTools.GetLocalized(SectionDescription), Notes),
                        [.. sections], [.. altSections]);

                    // We need to check for exit early
                    if (Answer == -1)
                    {
                        // Go Back...
                        DebugWriter.WriteDebug(DebugLevel.I, "User requested exit. Returning...");
                        break;
                    }

                    // Check the answer
                    var allSections = sections.Union(altSections).ToArray();
                    string answerChoice = allSections[Answer - 1].ChoiceName;
                    int finalAnswer = Answer < 0 ? 0 : Convert.ToInt32(answerChoice);
                    DebugWriter.WriteDebug(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...");
                    if (finalAnswer >= 1 & finalAnswer <= MaxOptions)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Opening key {0} from section {1}...", vars: [finalAnswer, Section]);
                        OpenKey(finalAnswer, SettingsSection, settingsType);
                    }
                    else if (finalAnswer == MaxOptions + 1)
                    {
                        // Go Back...
                        DebugWriter.WriteDebug(DebugLevel.I, "User requested exit. Returning...");
                        SectionFinished = true;
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Option is not valid. Returning...");
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_OPTIONINVALID"), true, ThemeColorType.Error, Answer);
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_COMMON_GOBACK"), true, ThemeColorType.Error);
                        Input.ReadKey();
                    }
                }
            }
            catch (Exception ex)
            {
                SettingsAppTools.HandleError(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_INVALIDSECTION"), ex);
            }
        }

        /// <summary>
        /// Open a key.
        /// </summary>
        /// <param name="settingsType">Settings type</param>
        /// <param name="KeyNumber">Key number</param>
        /// <param name="SettingsSection">Settings token</param>
        public static void OpenKey(int KeyNumber, SettingsEntry SettingsSection, BaseKernelConfig settingsType)
        {
            PermissionsTools.Demand(PermissionTypes.ManipulateSettings);
            try
            {
                // Section and key
                var SectionToken = SettingsSection.Keys;
                var KeyToken = SectionToken[KeyNumber - 1];

                // Key properties
                SettingsKeyType KeyType = KeyToken.Type;
                object? KeyDefaultValue = "";
                bool KeyFinished = false;

                // Preset properties
                string ShellType = KeyToken.ShellType;

                // Inputs
                while (!KeyFinished)
                {
                    if (KeyType == SettingsKeyType.SUnknown)
                        break;

                    // Determine how to get key default value
                    KeyDefaultValue = ConfigTools.GetValueFromEntry(KeyToken, settingsType);

                    // Prompt for input
                    var keyInput = KeyToken.KeyInput;
                    var keyInputUser = keyInput.PromptForSet(KeyToken, KeyDefaultValue, settingsType, out KeyFinished);

                    // Now, set the value if input is provided correctly
                    if (KeyFinished)
                    {
                        keyInput.SetValue(KeyToken, keyInputUser, settingsType);
                        SettingsAppTools.SaveSettings();
                    }
                }
            }
            catch (Exception ex)
            {
                SettingsAppTools.HandleError(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_INVALIDKEY"), ex);
            }
        }

        /// <summary>
        /// A sub for variable finding prompt
        /// </summary>
        public static void VariableFinder(BaseKernelConfig configType)
        {
            try
            {
                List<InputChoiceInfo> Results;
                List<InputChoiceInfo> Back =
                [
                    new InputChoiceInfo("<---", LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_GOBACK"))
                ];

                // Prompt the user
                DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for searching...");
                string SearchFor = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SEARCHPROMPT"));

                // Search for the setting
                ConsoleWrapper.CursorVisible = false;
                InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SEARCHING"));
                Results = ConfigTools.FindSetting(SearchFor, configType);
                InputChoiceInfo[] finalResults = Results.Union(Back).ToArray();

                // Write the settings
                if (Results.Count > 0)
                {
                    int sel = 0;
                    while (sel != Results.Count)
                    {
                        // Prompt for setting
                        sel = InfoBoxSelectionColor.WriteInfoBoxSelection([.. finalResults], LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SEARCHFOUND"));

                        // If pressed back, bail
                        if (sel == Results.Count || sel == -1)
                            break;

                        // Go to setting
                        var ChosenSetting = Results[sel];
                        int SectionIndex = Convert.ToInt32(ChosenSetting.ChoiceName.Split('/')[0]) - 1;
                        int KeyNumber = Convert.ToInt32(ChosenSetting.ChoiceName.Split('/')[1]);
                        var Section = configType.SettingsEntries?[SectionIndex] ??
                            throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_EXCEPTION_CANTGETSECTION"));
                        OpenKey(KeyNumber, Section, configType);
                        Results = ConfigTools.FindSetting(SearchFor, configType);
                        finalResults = Results.Union(Back).ToArray();
                    }
                }
                else
                {
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_NORESULTS"), new InfoBoxSettings()
                    {
                        ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                    });
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SEARCHFAILURE") + $" {ex.Message}", new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                });
            }
        }

        internal static string RenderHeader(string title, string description, string notes = "")
        {
            string classicTitle = "- " + title + " ";
            if (Config.MainConfig.ClassicSettingsHeaderStyle)
                // User prefers the classic style
                return
                    classicTitle +
                    new string('-', ConsoleWrapper.WindowWidth - classicTitle.Length) + CharManager.NewLine + CharManager.NewLine +
                    description +
                    (!string.IsNullOrEmpty(notes) ? CharManager.NewLine + notes : "");
            else
                // User prefers the modern style
                return title + ": " + description +
                    (!string.IsNullOrEmpty(notes) ? CharManager.NewLine + notes : "");
        }

    }
}
