
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
using System.Collections.Generic;
using System.Linq;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Kernel.Configuration.Instances;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Users.Permissions;

namespace KS.Kernel.Configuration.Settings
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
        public static void OpenMainPage(string settingsType) =>
            OpenMainPage(Config.GetKernelConfig(settingsType));

        /// <summary>
        /// Opens the main page for settings, listing all the sections that are configurable
        /// </summary>
        /// <param name="settingsType">Type of settings</param>
        public static void OpenMainPage(BaseKernelConfig settingsType)
        {
            // Verify that we actually have the type
            if (settingsType is null)
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Settings type is not found."), true, KernelColorType.Error);
                return;
            }

            // Now, the main loop
            PermissionsTools.Demand(PermissionTypes.ManipulateSettings);
            bool PromptFinished = false;
            SettingsEntry[] SettingsEntries = settingsType.SettingsEntries;
            int MaxSections = SettingsEntries.Length;

            while (!PromptFinished)
            {
                // Populate sections and alt options
                var sections = SettingsAppTools.GetSectionChoices(SettingsEntries);
                var altSections = new InputChoiceInfo[]
                {
                    new($"{MaxSections + 1}", Translate.DoTranslation("Find a Setting")),
                    new($"{MaxSections + 2}", Translate.DoTranslation("Save Settings")),
                    new($"{MaxSections + 3}", Translate.DoTranslation("Save Settings As")),
                    new($"{MaxSections + 4}", Translate.DoTranslation("Load Settings From")),
                    new($"{MaxSections + 5}", Translate.DoTranslation("Reload Settings")),
                    new($"{MaxSections + 6}", Translate.DoTranslation("Exit")),
                };

                // Prompt for selection and check the answer
                string finalTitle = Translate.DoTranslation("Welcome to Settings!");
                int Answer = SelectionStyle.PromptSelection(RenderHeader(finalTitle, Translate.DoTranslation("Select section:")),
                    sections, altSections);
                if (Answer >= 1 & Answer <= MaxSections)
                {
                    // The selected answer is a section
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Loading section..."), false);
                    SettingsEntry SelectedSection = SettingsEntries[Answer - 1];
                    DebugWriter.WriteDebug(DebugLevel.I, "Opening section {0}...", SelectedSection.Name);
                    OpenSection(SelectedSection.Name, SelectedSection, settingsType);
                }
                else if (Answer == MaxSections + 1)
                {
                    // The selected answer is "Find a Setting"
                    VariableFinder(settingsType);
                }
                else if (Answer == MaxSections + 2)
                {
                    // The selected answer is "Save Settings"
                    SettingsAppTools.SaveSettings();
                }
                else if (Answer == MaxSections + 3)
                {
                    // The selected answer is "Save Settings As"
                    SettingsAppTools.SaveSettingsAs();
                }
                else if (Answer == MaxSections + 4)
                {
                    // The selected answer is "Load Settings From"
                    SettingsAppTools.LoadSettingsFrom(settingsType);
                }
                else if (Answer == MaxSections + 5)
                {
                    // The selected answer is "Reload Settings"
                    SettingsAppTools.ReloadConfig();
                }
                else if (Answer == MaxSections + 6 || Answer == -1)
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
                    InfoBoxColor.WriteInfoBoxKernelColor(Translate.DoTranslation("Specified option {0} is invalid.") + " " + Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error, Answer);
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
                    var altSections = new List<InputChoiceInfo>()
                    {
                        new InputChoiceInfo($"{MaxOptions + 1}", Translate.DoTranslation("Go Back..."))
                    };
                    var displayUnsupportedConfigs = new List<string>();

                    string Notes = "";
                    for (int SectionIndex = 0; SectionIndex <= MaxOptions - 1; SectionIndex++)
                    {
                        var Setting = SectionToken[SectionIndex];

                        // Check to see if the host platform is supported
                        bool platformUnsupported = SettingsAppTools.ValidatePlatformCompatibility(Setting);
                        if (platformUnsupported)
                        {
                            displayUnsupportedConfigs.Add(Translate.DoTranslation(Setting.Name));
                            Notes = Translate.DoTranslation("One or more of the following settings found in this section are unsupported in your platform:") + $" {string.Join(", ", displayUnsupportedConfigs)}";
                            continue;
                        }

                        // Now, populate the input choice info
                        object CurrentValue = ConfigTools.GetValueFromEntry(Setting, settingsType);
                        var ici = new InputChoiceInfo(
                            $"{SectionIndex + 1}",
                            $"{Translate.DoTranslation(Setting.Name)} [{CurrentValue}]",
                            Translate.DoTranslation(Setting.Description)
                        );
                        sections.Add(ici);
                    }
                    DebugWriter.WriteDebug(DebugLevel.W, "Section {0} has {1} selections.", Section, MaxOptions);

                    // Prompt user and check for input
                    string finalSection = SectionTranslateName ? Translate.DoTranslation(SectionDisplayName) : SectionDisplayName;
                    int Answer = SelectionStyle.PromptSelection(RenderHeader(finalSection, Translate.DoTranslation(SectionDescription), Notes),
                        sections, altSections);

                    // Check the answer
                    var allSections = sections.Union(altSections).ToArray();
                    int finalAnswer = Answer < 0 ? 0 : Convert.ToInt32(allSections[Answer - 1].ChoiceName);
                    DebugWriter.WriteDebug(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...");
                    if (finalAnswer >= 1 & finalAnswer <= MaxOptions)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Opening key {0} from section {1}...", finalAnswer, Section);
                        OpenKey(finalAnswer, SettingsSection, settingsType);
                    }
                    else if (finalAnswer == MaxOptions + 1 | Answer == -1)
                    {
                        // Go Back...
                        DebugWriter.WriteDebug(DebugLevel.I, "User requested exit. Returning...");
                        SectionFinished = true;
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Option is not valid. Returning...");
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Specified option {0} is invalid."), true, KernelColorType.Error, Answer);
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                        Input.DetectKeypress();
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
                object KeyDefaultValue = "";
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
                    var keyInputUser = keyInput.PromptForSet(KeyToken, KeyDefaultValue, out KeyFinished);

                    // Now, set the value if input is provided correctly
                    if (KeyFinished)
                        keyInput.SetValue(KeyToken, keyInputUser, settingsType);
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
                List<InputChoiceInfo> Back = new()
                {
                    new InputChoiceInfo("<---", Translate.DoTranslation("Go Back..."))
                };

                // Prompt the user
                TextWriterColor.Write(Translate.DoTranslation("Write what do you want to search for."));
                DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for searching...");
                TextWriterColor.WriteKernelColor(">> ", false, KernelColorType.Input);
                string SearchFor = Input.ReadLine();

                // Search for the setting
                ConsoleWrapper.CursorVisible = false;
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Searching for settings..."), false);
                Results = ConfigTools.FindSetting(SearchFor, configType);

                // Write the settings
                if (Results.Count > 0)
                {
                    int sel = 0;
                    while (sel != Results.Count + 1)
                    {
                        // Prompt for setting
                        sel = SelectionStyle.PromptSelection(Translate.DoTranslation("These settings are found. Please select one."), Results, Back);

                        // If pressed back, bail
                        if (sel == Results.Count + 1 || sel == -1)
                            break;

                        // Go to setting
                        var ChosenSetting = Results[sel - 1];
                        int SectionIndex = Convert.ToInt32(ChosenSetting.ChoiceName.Split('/')[0]) - 1;
                        int KeyNumber = Convert.ToInt32(ChosenSetting.ChoiceName.Split('/')[1]);
                        var Section = configType.SettingsEntries[SectionIndex];
                        OpenKey(KeyNumber, Section, configType);
                        Results = ConfigTools.FindSetting(SearchFor, configType);
                    }
                }
                else
                {
                    InfoBoxColor.WriteInfoBoxKernelColor(Translate.DoTranslation("Nothing is found. Make sure that you've written the setting correctly."), true, KernelColorType.Error);
                }
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBoxKernelColor(Translate.DoTranslation("Failed to find your requested setting.") + $" {ex.Message}", true, KernelColorType.Error);
            }
        }

        internal static string RenderHeader(string title, string description, string notes = "")
        {
            string classicTitle = "- " + title + " ";
            if (Config.MainConfig.ClassicSettingsHeaderStyle)
                // User prefers the classic style
                return
                    classicTitle +
                    new string('-', ConsoleWrapper.WindowWidth - (classicTitle).Length) + CharManager.NewLine + CharManager.NewLine +
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
