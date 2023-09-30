
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Screensaver;
using KS.Misc.Splash;
using KS.Misc.Text;
using KS.Resources;
using KS.Shell.Prompts;
using KS.Users.Permissions;
using Newtonsoft.Json;
using Terminaux.Colors;
using Terminaux.Colors.Wheel;
using Terminaux.Figlet;

namespace KS.Kernel.Configuration.Settings
{
    /// <summary>
    /// Settings application module
    /// </summary>
    public static class SettingsApp
    {

        // TODO: Even if we've introduced the SettingsKey and SettingsEntry classes, the whole settings entry implementation is still a convoluted mess. Please refactor.
        /// <summary>
        /// Main page
        /// </summary>
        public static void OpenMainPage(ConfigType SettingsType)
        {
            PermissionsTools.Demand(PermissionTypes.ManipulateSettings);
            bool PromptFinished = false;
            var SettingsEntries = ConfigTools.OpenSettingsResource(SettingsType);
            int MaxSections = SettingsEntries.Length;

            while (!PromptFinished)
            {
                // Populate sections
                var sections = new List<InputChoiceInfo>();
                for (int SectionIndex = 0; SectionIndex <= MaxSections - 1; SectionIndex++)
                {
                    SettingsEntry Section = SettingsEntries[SectionIndex];
                    if (SettingsType != ConfigType.Kernel)
                    {
                        var ici = new InputChoiceInfo(
                            $"{SectionIndex + 1}",
                            Translate.DoTranslation(Section.Name),
                            Translate.DoTranslation(Section.Desc)
                        );
                        sections.Add(ici);
                    }
                    else
                    {
                        var ici = new InputChoiceInfo(
                            $"{SectionIndex + 1}",
                            Translate.DoTranslation(Section.DisplayAs),
                            Translate.DoTranslation(Section.Desc)
                        );
                        sections.Add(ici);
                    }
                }

                // Alternative options
                var altSections = new List<InputChoiceInfo>()
                {
                    new InputChoiceInfo($"{MaxSections + 1}", Translate.DoTranslation("Find a Setting")),
                    new InputChoiceInfo($"{MaxSections + 2}", Translate.DoTranslation("Save Settings")),
                    new InputChoiceInfo($"{MaxSections + 3}", Translate.DoTranslation("Save Settings As")),
                    new InputChoiceInfo($"{MaxSections + 4}", Translate.DoTranslation("Load Settings From")),
                    new InputChoiceInfo($"{MaxSections + 5}", Translate.DoTranslation("Reload Settings")),
                    new InputChoiceInfo($"{MaxSections + 6}", Translate.DoTranslation("Exit")),
                };

                // Prompt for selection and check the answer
                string finalTitle = Translate.DoTranslation("Welcome to Settings!");
                int Answer = SelectionStyle.PromptSelection("\n  * " + finalTitle + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation("Select section:"),
                    sections, altSections);
                if (Answer >= 1 & Answer <= MaxSections)
                {
                    // The selected answer is a section
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Loading section..."), false);
                    SettingsEntry SelectedSection = SettingsEntries[Answer - 1];
                    DebugWriter.WriteDebug(DebugLevel.I, "Opening section {0}...", SelectedSection.Name);
                    OpenSection(SelectedSection.Name, SelectedSection, SettingsType);
                }
                else if (Answer == MaxSections + 1)
                {
                    // The selected answer is "Find a Setting"
                    VariableFinder(SettingsEntries, SettingsType);
                }
                else if (Answer == MaxSections + 2)
                {
                    // The selected answer is "Save Settings"
                    DebugWriter.WriteDebug(DebugLevel.I, "Saving settings...");
                    try
                    {
                        InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Saving settings..."), false);
                        Config.CreateConfig();
                    }
                    catch (Exception ex)
                    {
                        InfoBoxColor.WriteInfoBox(ex.Message, true, KernelColorType.Error);
                        DebugWriter.WriteDebugStackTrace(ex);
                    }
                }
                else if (Answer == MaxSections + 3)
                {
                    // The selected answer is "Save Settings As"
                    TextWriterColor.Write(Translate.DoTranslation("Where do you want to save the current kernel settings?"), true, KernelColorType.Question);
                    string Location = Filesystem.NeutralizePath(Input.ReadLine());
                    ConsoleWrapper.CursorVisible = false;
                    if (!Checking.FileExists(Location))
                    {
                        try
                        {
                            InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Saving settings..."), false);
                            Config.CreateConfig(Location);
                        }
                        catch (Exception ex)
                        {
                            InfoBoxColor.WriteInfoBox(ex.Message, true, KernelColorType.Error);
                            DebugWriter.WriteDebugStackTrace(ex);
                        }
                    }
                    else
                    {
                        InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Can't save kernel settings on top of existing file."), true, KernelColorType.Error);
                    }
                }
                else if (Answer == MaxSections + 4)
                {
                    // The selected answer is "Load Settings From"
                    TextWriterColor.Write(Translate.DoTranslation("Where do you want to load the current kernel settings from?"), true, KernelColorType.Question);
                    string Location = Filesystem.NeutralizePath(Input.ReadLine());
                    if (Checking.FileExists(Location))
                    {
                        try
                        {
                            InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Loading settings..."), false);
                            Config.ReadConfig(SettingsType, Location);
                            InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Saving settings..."), false);
                            Config.CreateConfig();
                        }
                        catch (Exception ex)
                        {
                            InfoBoxColor.WriteInfoBox(ex.Message, true, KernelColorType.Error);
                            DebugWriter.WriteDebugStackTrace(ex);
                        }
                    }
                    else
                    {
                        InfoBoxColor.WriteInfoBox(Translate.DoTranslation("File not found."), true, KernelColorType.Error);
                    }
                }
                else if (Answer == MaxSections + 5)
                {
                    // The selected answer is "Reload Settings"
                    DebugWriter.WriteDebug(DebugLevel.W, "Reloading...");
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Reloading settings..."), false);
                    ConfigTools.ReloadConfig();
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Configuration reloaded. You might need to reboot the kernel for some changes to take effect."));
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
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Specified option {0} is invalid.") + " " + Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error, Answer);
                }
            }
        }

        /// <summary>
        /// Open section
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="SettingsSection">Settings section entry</param>
        /// <param name="SettingsType">Settings type</param>
        public static void OpenSection(string Section, SettingsEntry SettingsSection, ConfigType SettingsType)
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
                        string[] keyUnsupportedPlatforms = Setting.UnsupportedPlatforms.ToArray() ?? Array.Empty<string>();
                        bool platformUnsupported = false;
                        foreach (string platform in keyUnsupportedPlatforms)
                        {
                            switch (platform.ToLower())
                            {
                                case "windows":
                                    if (KernelPlatform.IsOnWindows())
                                        platformUnsupported = true;
                                    break;
                                case "unix":
                                    if (KernelPlatform.IsOnUnix())
                                        platformUnsupported = true;
                                    break;
                                case "macos":
                                    if (KernelPlatform.IsOnMacOS())
                                        platformUnsupported = true;
                                    break;
                            }
                        }
                        if (platformUnsupported)
                        {
                            displayUnsupportedConfigs.Add(Translate.DoTranslation(Setting.Name));
                            Notes = Translate.DoTranslation("One or more of the following settings found in this section are unsupported in your platform:") + $" {string.Join(", ", displayUnsupportedConfigs)}";
                            continue;
                        }

                        // Now, populate the input choice info
                        object CurrentValue = ConfigTools.GetValueFromEntry(Setting, SettingsType);
                        var ici = new InputChoiceInfo(
                            $"{SectionIndex + 1}",
                            $"{Translate.DoTranslation(Setting.Name)} [{CurrentValue}]",
                            Translate.DoTranslation(Setting.Description)
                        );
                        sections.Add(ici);
                    }

                    if (SettingsType == ConfigType.Screensaver)
                    {
                        var ici = new InputChoiceInfo($"{MaxOptions + 2}", Translate.DoTranslation("Preview screensaver"));
                        altSections.Add(ici);
                    }
                    else if (SettingsType == ConfigType.Splash)
                    {
                        var ici = new InputChoiceInfo($"{MaxOptions + 2}", Translate.DoTranslation("Preview splash"));
                        altSections.Add(ici);
                    }
                    DebugWriter.WriteDebug(DebugLevel.W, "Section {0} has {1} selections.", Section, MaxOptions);

                    // Prompt user and check for input
                    string finalSection = SectionTranslateName ? Translate.DoTranslation((string)SectionDisplayName) : (string)SectionDisplayName;
                    int Answer = SelectionStyle.PromptSelection("\n  * " + finalSection + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation((string)SectionDescription) + (!string.IsNullOrEmpty(Notes) ? CharManager.NewLine + Notes : ""),
                        sections, altSections);

                    // Check the answer
                    var allSections = sections.Union(altSections).ToArray();
                    int finalAnswer = Answer < 0 ? 0 : Convert.ToInt32(allSections[Answer - 1].ChoiceName);
                    DebugWriter.WriteDebug(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...");
                    if (finalAnswer >= 1 & finalAnswer <= MaxOptions)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Opening key {0} from section {1}...", finalAnswer, Section);
                        OpenKey(finalAnswer, SettingsSection, SettingsType);
                    }
                    else if (finalAnswer == MaxOptions + 2 & SettingsType == ConfigType.Screensaver)
                    {
                        // Preview screensaver
                        DebugWriter.WriteDebug(DebugLevel.I, "User requested screensaver preview.");
                        ScreensaverManager.ShowSavers(Section);
                        if (ScreensaverManager.inSaver)
                        {
                            Input.DetectKeypress();
                            ScreensaverDisplayer.BailFromScreensaver();
                        }
                    }
                    else if (finalAnswer == MaxOptions + 2 & SettingsType == ConfigType.Splash)
                    {
                        // Preview splash
                        DebugWriter.WriteDebug(DebugLevel.I, "User requested splash preview.");
                        SplashManager.PreviewSplash(Section);
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
                        TextWriterColor.Write(Translate.DoTranslation("Specified option {0} is invalid."), true, KernelColorType.Error, Answer);
                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                        Input.DetectKeypress();
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleWrapper.Clear();
                DebugWriter.WriteDebug(DebugLevel.I, "Error trying to open section: {0}", ex.Message);
                string finalSection = Translate.DoTranslation("You're Lost!");
                TextWriterColor.Write("\n  * " + finalSection + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation("Invalid section entered. Please go back."), true, KernelColorType.Error);
                TextWriterColor.Write(Translate.DoTranslation("If you're sure that you've opened the right section, check this message out:"), true, KernelColorType.Error);
                TextWriterColor.Write(ex.Message, true, KernelColorType.Error);
                Input.DetectKeypress();
            }
        }

        /// <summary>
        /// Open a key.
        /// </summary>
        /// <param name="SettingsType">Settings type</param>
        /// <param name="KeyNumber">Key number</param>
        /// <param name="SettingsSection">Settings token</param>
        public static void OpenKey(int KeyNumber, SettingsEntry SettingsSection, ConfigType SettingsType)
        {
            PermissionsTools.Demand(PermissionTypes.ManipulateSettings);
            try
            {
                // Section and key
                var SectionToken = SettingsSection.Keys;
                var KeyToken = SectionToken[KeyNumber - 1];
                int MaxKeyOptions = 0;

                // Key properties
                string KeyName = KeyToken.Name;
                string KeyDescription = KeyToken.Description;
                SettingsKeyType KeyType = KeyToken.Type;
                string KeyVar = KeyToken.Variable;
                int KeyEnumerableIndex = KeyToken.EnumerableIndex;
                object KeyValue = "";
                object KeyDefaultValue = "";
                bool KeyFinished = false;
                string finalSection = Translate.DoTranslation(KeyName);

                // Integer slider properties
                int IntSliderMinimumValue = KeyToken.MinimumValue;
                int IntSliderMaximumValue = KeyToken.MaximumValue;

                // Selection properties
                bool SelectionEnum = KeyToken.IsEnumeration;
                string SelectionEnumAssembly = KeyToken.EnumerationAssembly;
                bool SelectionEnumInternal = KeyToken.EnumerationInternal;
                bool SelectionEnumZeroBased = KeyToken.EnumerationZeroBased;
                bool SelectionFunctionDict = KeyToken.IsSelectionFunctionDict;
                Type SelectionEnumType = default;

                // Color properties
                object ColorValue = "";

                // List properties
                string ListJoinString = KeyToken.Delimiter;
                string ListJoinStringVariable = KeyToken.DelimiterVariable;
                string ListFunctionName = KeyToken.SelectionFunctionName;
                string ListFunctionType = KeyToken.SelectionFunctionType;
                bool ListIsPathCurrentPath = KeyToken.IsPathCurrentPath;
                KernelPathType ListValuePathType = KeyToken.ValuePathType;
                var TargetList = default(IEnumerable<object>);
                var SelectFrom = default(IEnumerable<object>);
                var Selections = default(object);
                bool NeutralizePaths = KeyToken.IsValuePath;
                string NeutralizeRootPath = ListIsPathCurrentPath ? CurrentDirectory.CurrentDir : Paths.GetKernelPath(ListValuePathType);

                // Preset properties
                string ShellType = KeyToken.ShellType;

                // Inputs
                string AnswerString = "";
                while (!KeyFinished)
                {
                    if (KeyType == SettingsKeyType.SUnknown)
                        break;

                    // Determine how to get key default value
                    KeyDefaultValue = ConfigTools.GetValueFromEntry(KeyToken, SettingsType);

                    // How the settings app displays the options and parses the output varies by the keytype
                    switch (KeyType)
                    {
                        case SettingsKeyType.SChar:
                        case SettingsKeyType.SInt:
                        case SettingsKeyType.SDouble:
                        case SettingsKeyType.SList:
                        case SettingsKeyType.SString:
                            ConsoleWrapper.Clear();

                            // Make an introductory banner
                            TextWriterColor.Write("\n  * " + finalSection + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation(KeyDescription), true, KernelColorType.Question);

                            // Write the prompt
                            TextWriterColor.Write("[{0}] > ", false, KernelColorType.Input, KeyDefaultValue);

                            // Get the target list from the method defined in the manifest (SelectionFunctionName)
                            if (KeyType == SettingsKeyType.SList)
                            {
                                TargetList = (IEnumerable<object>)MethodManager.GetMethod(ListFunctionName).Invoke(ListFunctionType, null);
                                TextWriterColor.Write(Translate.DoTranslation("Current items:"), true, KernelColorType.ListTitle);
                                ListWriterColor.WriteList(TargetList);
                                TextWriterColor.Write();
                                TextWriterColor.Write(CharManager.NewLine + " q) " + Translate.DoTranslation("Save Changes...") + CharManager.NewLine, true, KernelColorType.Option);

                                // Prompt the user and parse the answer
                                TextWriterColor.Write("> ", false, KernelColorType.Input);
                                while (AnswerString != "q")
                                {
                                    AnswerString = Input.ReadLine();
                                    if (!(AnswerString == "q"))
                                    {
                                        if (NeutralizePaths)
                                            AnswerString = Filesystem.NeutralizePath(AnswerString, NeutralizeRootPath);

                                        // Check to see if we're removing an item
                                        if (!AnswerString.StartsWith("-"))
                                        {
                                            // We're not removing an item!
                                            TargetList = TargetList.Append(AnswerString);
                                        }
                                        else
                                        {
                                            // We're removing an item.
                                            var DeletedItems = Enumerable.Empty<object>();
                                            DeletedItems = DeletedItems.Append(AnswerString[1..]);
                                            TargetList = TargetList.Except(DeletedItems);
                                        }
                                        DebugWriter.WriteDebug(DebugLevel.I, "Added answer {0} to list.", AnswerString);
                                        TextWriterColor.Write("> ", false, KernelColorType.Input);
                                    }
                                }
                            }
                            else if (KeyType == SettingsKeyType.SChar)
                                AnswerString = Convert.ToString(Input.DetectKeypress().KeyChar);
                            else
                                AnswerString = Input.ReadLine();

                            // Neutralize path if required with the assumption that the keytype is not list
                            if (NeutralizePaths)
                                AnswerString = Filesystem.NeutralizePath(AnswerString, NeutralizeRootPath);
                            DebugWriter.WriteDebug(DebugLevel.I, "User answered {0}", AnswerString);

                            break;
                        case SettingsKeyType.SSelection:
                            // Determine which list we're going to select
                            if (SelectionEnum)
                            {
                                var enumeration = KeyToken.Enumeration;
                                if (SelectionEnumInternal)
                                {
                                    // Apparently, we need to have a full assembly name for getting types.
                                    SelectionEnumType = Type.GetType("KS." + enumeration + ", " + Assembly.GetExecutingAssembly().FullName);
                                    SelectFrom = SelectionEnumType.GetEnumNames();
                                    Selections = SelectionEnumType.GetEnumValues();
                                }
                                else
                                {
                                    SelectionEnumType = Type.GetType(enumeration + ", " + SelectionEnumAssembly);
                                    SelectFrom = SelectionEnumType.GetEnumNames();
                                    Selections = SelectionEnumType.GetEnumValues();
                                }
                            }
                            else
                            {
                                if (SelectionFunctionDict)
                                    SelectFrom = (IEnumerable<object>)((IDictionary)MethodManager.GetMethod(ListFunctionName).Invoke(ListFunctionType, null)).Keys;
                                else
                                    SelectFrom = (IEnumerable<object>)MethodManager.GetMethod(ListFunctionName).Invoke(ListFunctionType, null);
                            }
                            MaxKeyOptions = SelectFrom.Count();

                            // Populate items
                            var items = new List<string>();
                            var itemNums = new List<int>();
                            var altSections = new List<string>()
                            {
                                Translate.DoTranslation("Go Back...")
                            };
                            var altSectionNums = new List<int>()
                            {
                                MaxKeyOptions + 1
                            };

                            // Since there is no way to index the SelectFrom enumerable, we have to manually initialize a counter. Ugly!
                            int itemCount = 1;
                            foreach (var item in SelectFrom)
                            {
                                items.Add(item.ToString());
                                itemNums.Add(itemCount);
                                itemCount++;
                            }

                            // Prompt user and check for input
                            int Answer = SelectionStyle.PromptSelection("\n  * " + finalSection + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation(KeyDescription),
                                string.Join("/", itemNums), items.ToArray(),
                                string.Join("/", altSectionNums), altSections.ToArray());
                            if (Answer == -1)
                                break;
                            AnswerString = Answer.ToString();

                            break;
                        case SettingsKeyType.SColor:
                            Color keyColorValue = Color.Empty;

                            // Check to see if the color is contained in the dictionary
                            if (KeyDefaultValue is KeyValuePair<KernelColorType, Color> keyColorValuePair)
                                keyColorValue = keyColorValuePair.Value;
                            else if (KeyDefaultValue is string keyColorString)
                                keyColorValue = new Color(keyColorString);

                            // Get the color value from the color wheel
                            KernelColorTools.LoadBack(new Color(ConsoleColors.Black));
                            ColorValue = ColorWheel.InputForColor(keyColorValue).PlainSequence;

                            break;
                        case SettingsKeyType.SIntSlider:
                            var PressedKey = default(ConsoleKey);
                            int CurrentValue = Convert.ToInt32(KeyDefaultValue);
                            ConsoleWrapper.CursorVisible = false;
                            ConsoleWrapper.Clear();
                            while (PressedKey != ConsoleKey.Enter)
                            {
                                // Draw the progress bar
                                ProgressBarColor.WriteProgress(100d * (CurrentValue / (double)IntSliderMaximumValue), 4, ConsoleWrapper.WindowHeight - 4);

                                // Show the current value
                                TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Current value:") + " {0} / {1} - {2}" + $"{ConsoleExtensions.GetClearLineToRightSequence()}", 5, ConsoleWrapper.WindowHeight - 5, false, KernelColorType.NeutralText, CurrentValue, IntSliderMinimumValue, IntSliderMaximumValue);

                                // Parse the user input
                                PressedKey = Input.DetectKeypress().Key;
                                switch (PressedKey)
                                {
                                    case ConsoleKey.LeftArrow:
                                        {
                                            if (CurrentValue > IntSliderMinimumValue)
                                                CurrentValue -= 1;
                                            break;
                                        }
                                    case ConsoleKey.RightArrow:
                                        {
                                            if (CurrentValue < IntSliderMaximumValue)
                                                CurrentValue += 1;
                                            break;
                                        }
                                    case ConsoleKey.Enter:
                                        {
                                            AnswerString = CurrentValue.ToString();
                                            ConsoleWrapper.CursorVisible = true;
                                            break;
                                        }
                                }
                            }

                            break;
                        case SettingsKeyType.SBoolean:
                            AnswerString = Convert.ToString(Convert.ToInt32(!(bool)KeyDefaultValue));

                            break;
                        case SettingsKeyType.SPreset:
                            PromptPresetManager.PromptForPresets(ShellType);

                            break;
                        case SettingsKeyType.SFiglet:
                            AnswerString = FigletSelector.PromptForFiglet();

                            break;
                    }

                    // Check for input
                    DebugWriter.WriteDebug(DebugLevel.I, "Is the answer numeric? {0}", TextTools.IsStringNumeric(AnswerString));
                    if (int.TryParse(AnswerString, out int AnswerInt))
                    {
                        // The answer is numeric! Now, check for types
                        switch (KeyType)
                        {
                            case SettingsKeyType.SBoolean:
                                {
                                    // We're dealing with boolean
                                    DebugWriter.WriteDebug(DebugLevel.I, "Answer is numeric and key is of the Boolean type.");
                                    var FinalBool = true;
                                    DebugWriter.WriteDebug(DebugLevel.I, "Translating {0} to the boolean equivalent...", AnswerInt);
                                    KeyFinished = true;

                                    // Set boolean
                                    switch (AnswerInt)
                                    {
                                        case 0: // False
                                            {
                                                DebugWriter.WriteDebug(DebugLevel.I, "Setting to False...");
                                                FinalBool = false;
                                                break;
                                            }
                                        case 1: // True
                                            {
                                                DebugWriter.WriteDebug(DebugLevel.I, "Setting to True...");
                                                FinalBool = true;
                                                break;
                                            }
                                    }

                                    // Now, set the value
                                    SetPropertyValue(KeyVar, FinalBool, SettingsType);
                                    break;
                                }
                            case SettingsKeyType.SSelection:
                                {
                                    // We're dealing with selection
                                    DebugWriter.WriteDebug(DebugLevel.I, "Answer is numeric and key is of the selection type.");
                                    int AnswerIndex = AnswerInt - 1;
                                    if (AnswerInt == MaxKeyOptions + 1 || AnswerInt == -1) // Go Back...
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "User requested exit. Returning...");
                                        KeyFinished = true;
                                    }
                                    else if (AnswerInt > 0)
                                    {
                                        if (Selections is IEnumerable<object> selectionsArray)
                                        {
                                            DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to item index {1}...", KeyVar, AnswerInt);
                                            KeyFinished = true;

                                            // Now, set the value
                                            SetPropertyValue(KeyVar, selectionsArray.ToArray()[AnswerIndex], SettingsType);
                                        }
                                        else if (!(AnswerInt > MaxKeyOptions))
                                        {
                                            object FinalValue;
                                            DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt);
                                            KeyFinished = true;
                                            FinalValue = SelectFrom.ElementAtOrDefault(AnswerInt - 1);
                                            if (SelectionEnum)
                                                FinalValue = Enum.Parse(SelectionEnumType, FinalValue.ToString());

                                            // Now, set the value
                                            SetPropertyValue(KeyVar, FinalValue, SettingsType);
                                        }
                                        else
                                        {
                                            DebugWriter.WriteDebug(DebugLevel.W, "Answer is not valid.");
                                            TextWriterColor.Write(Translate.DoTranslation("The answer may not exceed the entries shown."), true, KernelColorType.Error);
                                            TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                                            Input.DetectKeypress();
                                        }
                                    }
                                    else if (AnswerInt == 0 & !SelectionEnumZeroBased)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.W, "Zero is not allowed.");
                                        TextWriterColor.Write(Translate.DoTranslation("The answer may not be zero."), true, KernelColorType.Error);
                                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                                        Input.DetectKeypress();
                                    }
                                    else
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.W, "Negative values are disallowed.");
                                        TextWriterColor.Write(Translate.DoTranslation("The answer may not be negative."), true, KernelColorType.Error);
                                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                                        Input.DetectKeypress();
                                    }

                                    break;
                                }
                            case SettingsKeyType.SInt:
                                {
                                    // We're dealing with integers
                                    DebugWriter.WriteDebug(DebugLevel.I, "Answer is numeric and key is of the integer type.");
                                    if (AnswerInt >= 0)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt);
                                        KeyFinished = true;

                                        // Now, set the value
                                        SetPropertyValue(KeyVar, AnswerInt, SettingsType);
                                    }
                                    else
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.W, "Negative values are disallowed.");
                                        TextWriterColor.Write(Translate.DoTranslation("The answer may not be negative."), true, KernelColorType.Error);
                                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                                        Input.DetectKeypress();
                                    }

                                    break;
                                }
                            case SettingsKeyType.SIntSlider:
                                {
                                    // We're dealing with integers with limits
                                    DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt);
                                    KeyFinished = true;

                                    // Now, set the value
                                    SetPropertyValue(KeyVar, AnswerInt, SettingsType);

                                    break;
                                }
                        }
                    }
                    else if (double.TryParse(AnswerString, out double AnswerDbl))
                    {
                        switch (KeyType)
                        {
                            case SettingsKeyType.SDouble:
                                {
                                    // We're dealing with integers
                                    DebugWriter.WriteDebug(DebugLevel.I, "Answer is numeric and key is of the integer type.");
                                    if (AnswerDbl >= 0.0d)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerDbl);
                                        KeyFinished = true;

                                        // Now, set the value
                                        SetPropertyValue(KeyVar, AnswerDbl, SettingsType);
                                    }
                                    else
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.W, "Negative values are disallowed.");
                                        TextWriterColor.Write(Translate.DoTranslation("The answer may not be negative."), true, KernelColorType.Error);
                                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                                        Input.DetectKeypress();
                                    }

                                    break;
                                }
                        }
                    }
                    else
                    {
                        switch (KeyType)
                        {
                            case SettingsKeyType.SString:
                            case SettingsKeyType.SChar:
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Answer is not numeric and key is of the String or Char (inferred from keytype {0}) type. Setting variable...", KeyType.ToString());

                                    // Check to see if written answer is empty
                                    if (string.IsNullOrWhiteSpace(AnswerString))
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Answer is nothing. Setting to {0}...", KeyValue);
                                        AnswerString = Convert.ToString(KeyValue);
                                    }

                                    // Check to see if the user intended to clear the variable to make it consist of nothing
                                    if (AnswerString.ToLower() == "/clear")
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "User requested clear.");
                                        AnswerString = "";
                                    }

                                    // Set the value
                                    KeyFinished = true;
                                    SetPropertyValue(KeyVar, AnswerString, SettingsType);

                                    break;
                                }
                            case SettingsKeyType.SList:
                                {
                                    string FinalDelimiter;
                                    DebugWriter.WriteDebug(DebugLevel.I, "Answer is not numeric and key is of the List type. Adding answers to the list...");
                                    KeyFinished = true;

                                    // Get the delimiter
                                    if (ListJoinString is null)
                                        FinalDelimiter = Convert.ToString(PropertyManager.GetPropertyValue(ListJoinStringVariable, null, true));
                                    else
                                        FinalDelimiter = ListJoinString;

                                    // Now, set the value
                                    string JoinedString = string.Join(FinalDelimiter, TargetList);
                                    SetPropertyValue(KeyVar, JoinedString, SettingsType);

                                    break;
                                }
                            case SettingsKeyType.SColor:
                                {
                                    object FinalColor;

                                    // KeyVar is not always KernelColors, which is a dictionary. This applies to standard settings. Everything else should
                                    // be either the Color type or a String type.
                                    if (PropertyManager.CheckProperty(KeyVar) &&
                                        GetPropertyValue(KeyVar, SettingsType) is Dictionary<KernelColorType, Color> colors2)
                                    {
                                        var colorTypeOnDict = colors2.ElementAt(KeyEnumerableIndex).Key;
                                        colors2[colorTypeOnDict] = new Color(ColorValue.ToString());
                                        FinalColor = colors2;
                                    }
                                    else if (PropertyManager.CheckProperty(KeyVar) &&
                                             PropertyManager.GetProperty(KeyVar).PropertyType == typeof(Color))
                                    {
                                        FinalColor = new Color(ColorValue.ToString());
                                    }
                                    else
                                    {
                                        FinalColor = ColorValue.ToString();
                                    }

                                    // Now, set the value
                                    SetPropertyValue(KeyVar, FinalColor, SettingsType);
                                    KeyFinished = true;
                                    break;
                                }
                            case SettingsKeyType.SPreset:
                                {
                                    // Already set by SetPresetInternal
                                    KeyFinished = true;
                                    break;
                                }
                            case SettingsKeyType.SFiglet:
                                {
                                    KeyFinished = true;
                                    SetPropertyValue(KeyVar, AnswerString, SettingsType);
                                    break;
                                }
                            default:
                                {
                                    DebugWriter.WriteDebug(DebugLevel.W, "Answer is not valid.");
                                    TextWriterColor.Write(Translate.DoTranslation("The answer is invalid. Check to make sure that the answer is numeric for config entries that need numbers as answers."), true, KernelColorType.Error);
                                    TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                                    Input.DetectKeypress();
                                    break;
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleWrapper.Clear();
                DebugWriter.WriteDebug(DebugLevel.I, "Error trying to open section: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                string finalSection = Translate.DoTranslation("You're Lost!");
                TextWriterColor.Write("\n  * " + finalSection + CharManager.NewLine + CharManager.NewLine + Translate.DoTranslation("Invalid section entered. Please go back."), true, KernelColorType.Error);
                TextWriterColor.Write(Translate.DoTranslation("If you're sure that you've opened the right section, check this message out:"), true, KernelColorType.Error);
                TextWriterColor.Write(ex.Message, true, KernelColorType.Error);
                Input.DetectKeypress();
            }
        }

        /// <summary>
        /// A sub for variable finding prompt
        /// </summary>
        public static void VariableFinder(SettingsEntry[] SettingsEntries, ConfigType SettingsType)
        {
            List<InputChoiceInfo> Results;
            List<InputChoiceInfo> Back = new() { new InputChoiceInfo("<---", Translate.DoTranslation("Go Back...")) };

            // Prompt the user
            TextWriterColor.Write(Translate.DoTranslation("Write what do you want to search for."));
            DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for searching...");
            TextWriterColor.Write(">> ", false, KernelColorType.Input);
            string SearchFor = Input.ReadLine();

            // Search for the setting
            ConsoleWrapper.CursorVisible = false;
            InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Searching for settings..."), false);
            Results = ConfigTools.FindSetting(SearchFor, SettingsEntries, SettingsType);

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
                    var Section = SettingsEntries[SectionIndex];
                    OpenKey(KeyNumber, Section, SettingsType);
                    Results = ConfigTools.FindSetting(SearchFor, SettingsEntries, SettingsType);
                }
            }
            else
            {
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Nothing is found. Make sure that you've written the setting correctly."), true, KernelColorType.Error);
            }
        }

        private static void SetPropertyValue(string KeyVar, object Value, ConfigType SettingsType)
        {
            if (PropertyManager.CheckProperty(KeyVar))
            {
                // We're dealing with the property
                switch (SettingsType)
                {
                    case ConfigType.Kernel:
                        PropertyManager.SetPropertyValueInstance(Config.MainConfig, KeyVar, Value);
                        break;
                    case ConfigType.Screensaver:
                        PropertyManager.SetPropertyValueInstance(Config.SaverConfig, KeyVar, Value);
                        break;
                    case ConfigType.Splash:
                        PropertyManager.SetPropertyValueInstance(Config.SplashConfig, KeyVar, Value);
                        break;
                    default:
                        DebugCheck.AssertFail($"dealing with settings type other than kernel, screensaver, and splash. {SettingsType}");
                        break;
                }
            }
        }

        private static object GetPropertyValue(string KeyVar, ConfigType SettingsType)
        {
            if (PropertyManager.CheckProperty(KeyVar))
            {
                // We're dealing with the property
                return SettingsType switch
                {
                    ConfigType.Kernel => PropertyManager.GetPropertyValueInstance(Config.MainConfig, KeyVar),
                    ConfigType.Screensaver => PropertyManager.GetPropertyValueInstance(Config.SaverConfig, KeyVar),
                    ConfigType.Splash => PropertyManager.GetPropertyValueInstance(Config.SplashConfig, KeyVar),
                    _ => null,
                };
            }
            return null;
        }

    }
}
