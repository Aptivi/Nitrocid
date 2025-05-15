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
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Selection;
using Nitrocid.ConsoleBase.Writers;
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
                TextWriters.Write(Translate.DoTranslation("Settings type is not found."), true, KernelColorType.Error);
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
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(Translate.DoTranslation("Set"), ConsoleKey.Enter, (_, entryIdx, _, keyIdx) => tui.Set(entryIdx, keyIdx)));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(Translate.DoTranslation("Save"), ConsoleKey.F1, (_, _, _, _) => tui.Save()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(Translate.DoTranslation("Save as"), ConsoleKey.F2, (_, _, _, _) => tui.SaveAs()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(Translate.DoTranslation("Load from"), ConsoleKey.F3, (_, _, _, _) => tui.LoadFrom()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(Translate.DoTranslation("Reload"), ConsoleKey.F4, (_, _, _, _) => tui.Reload()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(Translate.DoTranslation("Migrate"), ConsoleKey.F5, (_, _, _, _) => tui.Migrate()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(Translate.DoTranslation("Check for system updates"), ConsoleKey.F6, (_, _, _, _) => tui.CheckUpdates()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(Translate.DoTranslation("System information"), ConsoleKey.F7, (_, _, _, _) => tui.SystemInfo()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(Translate.DoTranslation("Reset all"), ConsoleKey.F8, (_, _, _, _) => tui.ResetAll()));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(Translate.DoTranslation("Reset entry"), ConsoleKey.R, ConsoleModifiers.Shift, (_, entryIdx, _, keyIdx) => tui.ResetEntry(entryIdx, keyIdx)));
                tui.Bindings.Add(new InteractiveTuiBinding<(string, int)>(Translate.DoTranslation("Select configuration"), ConsoleKey.F9, (_, _, _, _) => tui.SelectConfig()));
                InteractiveTuiTools.OpenInteractiveTui(tui);
                return;
            }

            // Now, the main loop
            bool PromptFinished = false;
            SettingsEntry[]? SettingsEntries = settingsType.SettingsEntries;
            if (SettingsEntries is null || SettingsEntries.Length == 0)
            {
                TextWriters.Write(Translate.DoTranslation("Settings entries are not found."), true, KernelColorType.Error);
                return;
            }
            int MaxSections = SettingsEntries.Length;

            while (!PromptFinished)
            {
                // Populate sections and alt options
                var sections = SettingsAppTools.GetSectionChoices(SettingsEntries);
                var altSections = new InputChoiceInfo[]
                {
                    new($"{MaxSections + 1}", Translate.DoTranslation("Find an option"), Translate.DoTranslation("Allows you to easily search for a settings entry using either its title or its value.")),
                    new($"{MaxSections + 2}", Translate.DoTranslation("Save settings"), Translate.DoTranslation("Saves your kernel configuration changes to the config file found in the application data folder.")),
                    new($"{MaxSections + 3}", Translate.DoTranslation("Save settings as"), Translate.DoTranslation("Saves your kernel configuration changes to the config file found in the specified folder.")),
                    new($"{MaxSections + 4}", Translate.DoTranslation("Load settings from"), Translate.DoTranslation("Loads the kernel configuration from the specified config file.")),
                    new($"{MaxSections + 5}", Translate.DoTranslation("Reload settings"), Translate.DoTranslation("Reloads the kernel configuration for any external changes")),
                    new($"{MaxSections + 6}", Translate.DoTranslation("Check for system updates"), Translate.DoTranslation("Checks for system updates (requires an active Internet connection).")),
                    new($"{MaxSections + 7}", Translate.DoTranslation("System information"), Translate.DoTranslation("Shows you basic system information (more info available in the 'sysinfo' command).")),
                    new($"{MaxSections + 8}", Translate.DoTranslation("Migrate old configuration"), Translate.DoTranslation("Migrates some of your old configuration from older versions of the kernel")),
                    new($"{MaxSections + 9}", Translate.DoTranslation("Select configuration"), Translate.DoTranslation("Prompts you to select a configuration instance")),
                    new($"{MaxSections + 10}", Translate.DoTranslation("Exit")),
                };

                // Prompt for selection and check the answer
                string finalTitle = Translate.DoTranslation("Welcome to Settings!");
                int Answer = SelectionStyle.PromptSelection(RenderHeader(finalTitle, TextTools.FormatString(Translate.DoTranslation("You're on the landing page of the {0} settings. Select a section or an option to get started. Depending on which settings you've changed, you might need to restart the kernel."), settingsType.GetType().Name)),
                    sections, altSections);
                if (Answer >= 1 & Answer <= MaxSections)
                {
                    // The selected answer is a section
                    InfoBoxNonModalColor.WriteInfoBox(Translate.DoTranslation("Loading section..."));
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
                        InfoBoxModalColor.WriteInfoBoxModalColor(Translate.DoTranslation("Configuration migration may not have been completed successfully. If you're sure that your configuration files are valid, investigate the debug logs for more info.") + " " +
                            Translate.DoTranslation("Press any key to go back."), KernelColorTools.GetColor(KernelColorType.Error));
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
                    InfoBoxModalColor.WriteInfoBoxModalColor(Translate.DoTranslation("Specified option {0} is invalid.") + " " + Translate.DoTranslation("Press any key to go back."), KernelColorTools.GetColor(KernelColorType.Error), Answer);
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
                bool SectionTranslateName = SettingsSection.DisplayAs != null;
                int MaxOptions = SectionToken.Length;

                while (!SectionFinished)
                {
                    // Populate sections
                    var sections = new List<InputChoiceInfo>();

                    // Check for platform compatibility
                    string Notes = "";
                    var unsupportedConfigs = SectionToken.Where((sk) => sk.Unsupported).ToArray();
                    var unsupportedConfigNames = unsupportedConfigs.Select((sk) => sk.Name.Localized).ToArray();
                    bool hasUnsupportedConfigs = unsupportedConfigs.Length > 0;
                    if (hasUnsupportedConfigs)
                        Notes = Translate.DoTranslation("One or more of the following settings found in this section are unsupported in your platform:") + $" {string.Join(", ", unsupportedConfigNames)}";

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
                        string choiceTitle = $"{Setting.Name} [{CurrentValue}]";
                        string choiceDesc = Setting.Description;
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
                        new($"{MaxOptions + 1}", Translate.DoTranslation("Go Back..."))
                    };

                    // Prompt user and check for input
                    string finalSection = SectionTranslateName ? Translate.DoTranslation(SectionDisplayName) : SectionDisplayName;
                    int Answer = SelectionStyle.PromptSelection(RenderHeader(finalSection, Translate.DoTranslation(SectionDescription), Notes),
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
                        TextWriters.Write(Translate.DoTranslation("Specified option {0} is invalid."), true, KernelColorType.Error, Answer);
                        TextWriters.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                        Input.ReadKey();
                    }
                }
            }
            catch (Exception ex)
            {
                SettingsAppTools.HandleError(Translate.DoTranslation("Invalid section. Please go back."), ex);
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
                SettingsAppTools.HandleError(Translate.DoTranslation("Invalid key. Please go back."), ex);
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
                    new InputChoiceInfo("<---", Translate.DoTranslation("Go Back..."))
                ];

                // Prompt the user
                DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for searching...");
                string SearchFor = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Write what do you want to search for."));

                // Search for the setting
                ConsoleWrapper.CursorVisible = false;
                InfoBoxNonModalColor.WriteInfoBox(Translate.DoTranslation("Searching for settings..."));
                Results = ConfigTools.FindSetting(SearchFor, configType);
                InputChoiceInfo[] finalResults = Results.Union(Back).ToArray();

                // Write the settings
                if (Results.Count > 0)
                {
                    int sel = 0;
                    while (sel != Results.Count)
                    {
                        // Prompt for setting
                        sel = InfoBoxSelectionColor.WriteInfoBoxSelection([.. finalResults], Translate.DoTranslation("These settings are found. Please select one."));

                        // If pressed back, bail
                        if (sel == Results.Count || sel == -1)
                            break;

                        // Go to setting
                        var ChosenSetting = Results[sel];
                        int SectionIndex = Convert.ToInt32(ChosenSetting.ChoiceName.Split('/')[0]) - 1;
                        int KeyNumber = Convert.ToInt32(ChosenSetting.ChoiceName.Split('/')[1]);
                        var Section = configType.SettingsEntries?[SectionIndex] ??
                            throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't get section"));
                        OpenKey(KeyNumber, Section, configType);
                        Results = ConfigTools.FindSetting(SearchFor, configType);
                        finalResults = Results.Union(Back).ToArray();
                    }
                }
                else
                {
                    InfoBoxModalColor.WriteInfoBoxModalColor(Translate.DoTranslation("Nothing is found. Make sure that you've written the setting correctly."), KernelColorTools.GetColor(KernelColorType.Error));
                }
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModalColor(Translate.DoTranslation("Failed to find your requested setting.") + $" {ex.Message}", KernelColorTools.GetColor(KernelColorType.Error));
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
                return "\n  * " + title + CharManager.NewLine + CharManager.NewLine +
                    description +
                    (!string.IsNullOrEmpty(notes) ? CharManager.NewLine + notes : "");
        }

    }
}
