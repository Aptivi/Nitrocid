
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
using ColorPrint.Core.Wheel;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Screensaver;
using KS.Misc.Screensaver.Customized;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Shell.Prompts;
using Newtonsoft.Json.Linq;

namespace KS.Misc.Settings
{
    /// <summary>
    /// Settings application module
    /// </summary>
    public static class SettingsApp
    {

        /// <summary>
        /// Main page
        /// </summary>
        public static void OpenMainPage(ConfigType SettingsType)
        {
            bool PromptFinished = false;
            var SettingsToken = OpenSettingsResource(SettingsType);
            int MaxSections = SettingsToken.Count();

            while (!PromptFinished)
            {
                // Populate sections
                var sections = new List<InputChoiceInfo>();
                for (int SectionIndex = 0; SectionIndex <= MaxSections - 1; SectionIndex++)
                {
                    JProperty Section = (JProperty)SettingsToken.ToList()[SectionIndex];
                    if (SettingsType != ConfigType.Kernel)
                    {
                        var ici = new InputChoiceInfo(
                            $"{SectionIndex + 1}",
                            Translate.DoTranslation(Section.Name),
                            Translate.DoTranslation(Section.First["Desc"].ToString())
                        );
                        sections.Add(ici);
                    }
                    else
                    {
                        var ici = new InputChoiceInfo(
                            $"{SectionIndex + 1}",
                            Translate.DoTranslation(Section.First["DisplayAs"].ToString()),
                            Translate.DoTranslation(Section.First["Desc"].ToString())
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
                    new InputChoiceInfo($"{MaxSections + 5}", Translate.DoTranslation("Exit")),
                };

                // Prompt for selection and check the answer
                string finalTitle = Translate.DoTranslation("Welcome to Settings!");
                int Answer = SelectionStyle.PromptSelection(finalTitle + CharManager.NewLine + "=".Repeat(finalTitle.Length) + CharManager.NewLine + Translate.DoTranslation("Select section:"), 
                    sections, altSections);
                if (Answer >= 1 & Answer <= MaxSections)
                {
                    // The selected answer is a section
                    JProperty SelectedSection = (JProperty)SettingsToken.ToList()[Answer - 1];
                    DebugWriter.WriteDebug(DebugLevel.I, "Opening section {0}...", SelectedSection.Name);
                    OpenSection(SelectedSection.Name, SettingsToken, SettingsType);
                }
                else if (Answer == MaxSections + 1)
                {
                    // The selected answer is "Find a Setting"
                    VariableFinder(SettingsToken, SettingsType);
                }
                else if (Answer == MaxSections + 2)
                {
                    // The selected answer is "Save Settings"
                    DebugWriter.WriteDebug(DebugLevel.I, "Saving settings...");
                    try
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Saving settings..."), true, KernelColorType.Progress);
                        Config.CreateConfig();
                        CustomSaverTools.SaveCustomSaverSettings();
                    }
                    catch (Exception ex)
                    {
                        TextWriterColor.Write(ex.Message, true, KernelColorType.Error);
                        DebugWriter.WriteDebugStackTrace(ex);
                        Input.DetectKeypress();
                    }
                }
                else if (Answer == MaxSections + 3)
                {
                    // The selected answer is "Save Settings As"
                    TextWriterColor.Write(Translate.DoTranslation("Where do you want to save the current kernel settings?"), true, KernelColorType.Question);
                    string Location = Filesystem.NeutralizePath(Input.ReadLine());
                    if (!Checking.FileExists(Location))
                    {
                        try
                        {
                            Config.CreateConfig(Location);
                        }
                        catch (Exception ex)
                        {
                            TextWriterColor.Write(ex.Message, true, KernelColorType.Error);
                            DebugWriter.WriteDebugStackTrace(ex);
                            Input.DetectKeypress();
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Can't save kernel settings on top of existing file."), true, KernelColorType.Error);
                        Input.DetectKeypress();
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
                            Config.ReadConfig((ConfigType)SettingsType, Location);
                            Config.CreateConfig();
                        }
                        catch (Exception ex)
                        {
                            TextWriterColor.Write(ex.Message, true, KernelColorType.Error);
                            DebugWriter.WriteDebugStackTrace(ex);
                            Input.DetectKeypress();
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("File not found."), true, KernelColorType.Error);
                        Input.DetectKeypress();
                    }
                }
                else if (Answer == MaxSections + 5 || Answer == -1)
                {
                    // The selected answer is "Exit"
                    DebugWriter.WriteDebug(DebugLevel.W, "Exiting...");
                    PromptFinished = true;
                    ConsoleBase.ConsoleWrapper.Clear();
                }
                else
                {
                    // Invalid selection
                    DebugWriter.WriteDebug(DebugLevel.W, "Option is not valid. Returning...");
                    TextWriterColor.Write(Translate.DoTranslation("Specified option {0} is invalid."), true, KernelColorType.Error, Answer);
                    TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, KernelColorType.Error);
                    Input.DetectKeypress();
                }
            }
        }

        /// <summary>
        /// Open section
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="SettingsToken">Settings token</param>
        /// <param name="SettingsType">Settings type</param>
        public static void OpenSection(string Section, JToken SettingsToken, ConfigType SettingsType)
        {
            try
            {
                // General variables
                bool SectionFinished = false;
                var SectionTokenGeneral = SettingsToken[Section];
                var SectionToken = SectionTokenGeneral["Keys"];
                var SectionDescription = SectionTokenGeneral["Desc"];
                var SectionDisplayName = SectionTokenGeneral["DisplayAs"] ?? Section;
                bool SectionTranslateName = SectionTokenGeneral["DisplayAs"] != null;
                int MaxOptions = SectionToken.Count();

                while (!SectionFinished)
                {
                    // Populate sections
                    var sections = new List<InputChoiceInfo>();
                    var altSections = new List<InputChoiceInfo>()
                    {
                        new InputChoiceInfo($"{MaxOptions + 1}", Translate.DoTranslation("Go Back..."))
                    };

                    string Notes = "";
                    for (int SectionIndex = 0; SectionIndex <= MaxOptions - 1; SectionIndex++)
                    {
                        var Setting = SectionToken[SectionIndex];

                        // Check to see if the host platform is supported
                        string[] keyUnsupportedPlatforms = ((JArray)Setting["UnsupportedPlatforms"])?.Values<string>().ToArray() ?? Array.Empty<string>();
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
                            Notes = Translate.DoTranslation("One or more of the settings found in this section are unsupported in your platform.");
                            continue;
                        }

                        // Now, populate the input choice info
                        object CurrentValue = ConfigTools.GetValueFromEntry(Setting, SettingsType);
                        var ici = new InputChoiceInfo(
                            $"{SectionIndex + 1}",
                            $"{Translate.DoTranslation(Setting["Name"].ToString())} [{CurrentValue}]",
                            Translate.DoTranslation(Setting["Description"]?.ToString() ?? "")
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
                    int Answer = SelectionStyle.PromptSelection(finalSection + CharManager.NewLine + "=".Repeat(finalSection.Length) + CharManager.NewLine + Translate.DoTranslation((string)SectionDescription) + (!string.IsNullOrEmpty(Notes) ? CharManager.NewLine + Notes : ""),
                        sections, altSections);

                    // Check the answer
                    var allSections = sections.Union(altSections).ToArray();
                    int finalAnswer = Answer < 0 ? 0 : Convert.ToInt32(allSections[Answer - 1].ChoiceName);
                    DebugWriter.WriteDebug(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...");
                    if (finalAnswer >= 1 & finalAnswer <= MaxOptions)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Opening key {0} from section {1}...", finalAnswer, Section);
                        OpenKey(Section, finalAnswer, SettingsToken, SettingsType);
                    }
                    else if (finalAnswer == MaxOptions + 2 & SettingsType == ConfigType.Screensaver)
                    {
                        // Preview screensaver
                        DebugWriter.WriteDebug(DebugLevel.I, "User requested screensaver preview.");
                        Screensaver.Screensaver.ShowSavers(Section);
                        if (Screensaver.Screensaver.inSaver)
                        {
                            Input.DetectKeypress();
                            ScreensaverDisplayer.BailFromScreensaver();
                        }
                    }
                    else if (finalAnswer == MaxOptions + 2 & SettingsType == ConfigType.Splash)
                    {
                        // Preview splash
                        DebugWriter.WriteDebug(DebugLevel.I, "User requested splash preview.");
                        Splash.SplashManager.PreviewSplash(Section);
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
                ConsoleBase.ConsoleWrapper.Clear();
                DebugWriter.WriteDebug(DebugLevel.I, "Error trying to open section: {0}", ex.Message);
                string finalSection = Translate.DoTranslation("You're Lost!");
                TextWriterColor.Write(finalSection + CharManager.NewLine + "=".Repeat(finalSection.Length) + CharManager.NewLine + Translate.DoTranslation("Invalid section entered. Please go back."), true, KernelColorType.Error);
                TextWriterColor.Write(Translate.DoTranslation("If you're sure that you've opened the right section, check this message out:"), true, KernelColorType.Error);
                TextWriterColor.Write(ex.Message, true, KernelColorType.Error);
                Input.DetectKeypress();
            }
        }

        /// <summary>
        /// Open a key.
        /// </summary>
        /// <param name="SettingsType">Settings type</param>
        /// <param name="Section">Section</param>
        /// <param name="KeyNumber">Key number</param>
        /// <param name="SettingsToken">Settings token</param>
        public static void OpenKey(string Section, int KeyNumber, JToken SettingsToken, ConfigType SettingsType)
        {
            try
            {
                // Section and key
                var SectionTokenGeneral = SettingsToken[Section];
                var SectionToken = SectionTokenGeneral["Keys"];
                var KeyToken = SectionToken.ToList()[KeyNumber - 1];
                int MaxKeyOptions = 0;

                // Key properties
                string KeyName = (string)KeyToken["Name"];
                string KeyDescription = (string)KeyToken["Description"];
                SettingsKeyType KeyType = (SettingsKeyType)Convert.ToInt32(Enum.Parse(typeof(SettingsKeyType), (string)KeyToken["Type"]));
                string KeyVar = (string)KeyToken["Variable"];
                int KeyEnumerableIndex = (int)(KeyToken["EnumerableIndex"] ?? 0);
                object KeyValue = "";
                object KeyDefaultValue = "";
                bool KeyFinished = false;
                string finalSection = Translate.DoTranslation(KeyName);

                // Integer slider properties
                int IntSliderMinimumValue = (int)(KeyToken["MinimumValue"] ?? 0);
                int IntSliderMaximumValue = (int)(KeyToken["MaximumValue"] ?? 100);

                // Selection properties
                bool SelectionEnum = (bool)(KeyToken["IsEnumeration"] ?? false);
                string SelectionEnumAssembly = (string)KeyToken["EnumerationAssembly"];
                bool SelectionEnumInternal = (bool)(KeyToken["EnumerationInternal"] ?? false);
                bool SelectionEnumZeroBased = (bool)(KeyToken["EnumerationZeroBased"] ?? false);
                bool SelectionFunctionDict = (bool)(KeyToken["IsSelectionFunctionDict"] ?? false);
                Type SelectionEnumType = default;

                // Color properties
                object ColorValue = "";

                // List properties
                string ListJoinString = (string)KeyToken["Delimiter"];
                string ListJoinStringVariable = (string)KeyToken["DelimiterVariable"];
                string ListFunctionName = (string)KeyToken["SelectionFunctionName"];
                string ListFunctionType = (string)KeyToken["SelectionFunctionType"];
                bool ListIsPathCurrentPath = (bool)(KeyToken["IsPathCurrentPath"] ?? false);
                KernelPathType ListValuePathType = (KernelPathType)Convert.ToInt32(KeyToken["ValuePathType"] is not null ? Enum.Parse(typeof(KernelPathType), (string)KeyToken["ValuePathType"]) : KernelPathType.Mods);
                var TargetList = default(IEnumerable<object>);
                var SelectFrom = default(IEnumerable<object>);
                var Selections = default(object);
                bool NeutralizePaths = (bool)(KeyToken["IsValuePath"] ?? false);
                string NeutralizeRootPath = ListIsPathCurrentPath ? CurrentDirectory.CurrentDir : Paths.GetKernelPath(ListValuePathType);

                // Preset properties
                string ShellType = (string)KeyToken["ShellType"];

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
                            ConsoleBase.ConsoleWrapper.Clear();

                            // Make an introductory banner
                            TextWriterColor.Write(finalSection + CharManager.NewLine + "=".Repeat(finalSection.Length) + CharManager.NewLine + Translate.DoTranslation(KeyDescription), true, KernelColorType.Question);

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
                                            DeletedItems = DeletedItems.Append(AnswerString.Substring(1));
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
                                if (SelectionEnumInternal)
                                {
                                    // Apparently, we need to have a full assembly name for getting types.
                                    SelectionEnumType = Type.GetType("KS." + KeyToken["Enumeration"].ToString() + ", " + Assembly.GetExecutingAssembly().FullName);
                                    SelectFrom = SelectionEnumType.GetEnumNames();
                                    Selections = SelectionEnumType.GetEnumValues();
                                }
                                else
                                {
                                    SelectionEnumType = Type.GetType(KeyToken["Enumeration"].ToString() + ", " + SelectionEnumAssembly);
                                    SelectFrom = SelectionEnumType.GetEnumNames();
                                    Selections = SelectionEnumType.GetEnumValues();
                                }
                            }
                            else
                            {
                                if (SelectionFunctionDict)
                                    SelectFrom = (IEnumerable<object>)((IDictionary)MethodManager.GetMethod((string)KeyToken["SelectionFunctionName"]).Invoke(KeyToken["SelectionFunctionType"], null)).Keys;
                                else
                                    SelectFrom = (IEnumerable<object>)MethodManager.GetMethod((string)KeyToken["SelectionFunctionName"]).Invoke(KeyToken["SelectionFunctionType"], null);
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
                            int Answer = SelectionStyle.PromptSelection(finalSection + CharManager.NewLine + "=".Repeat(finalSection.Length) + CharManager.NewLine + Translate.DoTranslation(KeyDescription),
                                string.Join("/", itemNums), items.ToArray(),
                                string.Join("/", altSectionNums), altSections.ToArray());
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
                            ColorValue = ColorWheel.InputForColor(keyColorValue).PlainSequence;

                            break;
                        case SettingsKeyType.SIntSlider:
                            var PressedKey = default(ConsoleKey);
                            int CurrentValue = Convert.ToInt32(KeyDefaultValue);
                            ConsoleBase.ConsoleWrapper.CursorVisible = false;
                            while (PressedKey != ConsoleKey.Enter)
                            {
                                // Draw the progress bar
                                ProgressBarColor.WriteProgress(100d * (CurrentValue / (double)IntSliderMaximumValue), 4, ConsoleBase.ConsoleWrapper.WindowHeight - 4);

                                // Show the current value
                                TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Current value:") + " {0} / {1} - {2}" + Convert.ToString(CharManager.GetEsc()) + "[0K", 5, ConsoleBase.ConsoleWrapper.WindowHeight - 5, false, KernelColorType.NeutralText, CurrentValue, IntSliderMinimumValue, IntSliderMaximumValue);

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
                                            ConsoleBase.ConsoleWrapper.CursorVisible = true;
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
                    }

                    // Check for input
                    DebugWriter.WriteDebug(DebugLevel.I, "Is the answer numeric? {0}", StringQuery.IsStringNumeric(AnswerString));
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
                                        FinalDelimiter = Convert.ToString(FieldManager.GetValue(ListJoinStringVariable, null, true));
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
                ConsoleBase.ConsoleWrapper.Clear();
                DebugWriter.WriteDebug(DebugLevel.I, "Error trying to open section: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                string finalSection = Translate.DoTranslation("You're Lost!");
                TextWriterColor.Write(finalSection + CharManager.NewLine + "=".Repeat(finalSection.Length) + CharManager.NewLine + Translate.DoTranslation("Invalid section entered. Please go back."), true, KernelColorType.Error);
                TextWriterColor.Write(Translate.DoTranslation("If you're sure that you've opened the right section, check this message out:"), true, KernelColorType.Error);
                TextWriterColor.Write(ex.Message, true, KernelColorType.Error);
                Input.DetectKeypress();
            }
        }

        /// <summary>
        /// A sub for variable finding prompt
        /// </summary>
        public static void VariableFinder(JToken SettingsToken, ConfigType SettingsType)
        {
            List<InputChoiceInfo> Results;
            List<InputChoiceInfo> Back = new() { new InputChoiceInfo("<---", Translate.DoTranslation("Go Back...")) };

            // Prompt the user
            TextWriterColor.Write(Translate.DoTranslation("Write what do you want to search for."));
            DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for searching...");
            TextWriterColor.Write(">> ", false, KernelColorType.Input);
            string SearchFor = Input.ReadLine();

            // Search for the setting
            Results = ConfigTools.FindSetting(SearchFor, SettingsToken, SettingsType);

            // Write the settings
            if (!(Results.Count == 0))
            {
                // Prompt for setting
                int sel = SelectionStyle.PromptSelection(Translate.DoTranslation("These settings are found. Please select one."), Results, Back);

                // If pressed back, bail
                if (sel == Results.Count + 1)
                    return;

                // Go to setting
                var ChosenSetting = Results[sel - 1];
                int SectionIndex = Convert.ToInt32(ChosenSetting.ChoiceName.Split('/')[0]) - 1;
                int KeyNumber = Convert.ToInt32(ChosenSetting.ChoiceName.Split('/')[1]);
                JProperty Section = (JProperty)SettingsToken.ToList()[SectionIndex];
                string SectionName = Section.Name;
                OpenKey(SectionName, KeyNumber, SettingsToken, SettingsType);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Nothing is found. Make sure that you've written the setting correctly."), true, KernelColorType.Error);
                Input.DetectKeypress();
            }
        }

        /// <summary>
        /// Open the settings resource
        /// </summary>
        /// <param name="SettingsType">The settings type</param>
        private static JToken OpenSettingsResource(ConfigType SettingsType)
        {
            switch (SettingsType)
            {
                case ConfigType.Kernel:
                    {
                        return JToken.Parse(Properties.Resources.Resources.SettingsEntries);
                    }
                case ConfigType.Screensaver:
                    {
                        return JToken.Parse(Properties.Resources.Resources.ScreensaverSettingsEntries);
                    }
                case ConfigType.Splash:
                    {
                        return JToken.Parse(Properties.Resources.Resources.SplashSettingsEntries);
                    }
                default:
                    {
                        return JToken.Parse(Properties.Resources.Resources.SettingsEntries);
                    }
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
                        DebugCheck.Assert(false);
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
                    ConfigType.Kernel       => PropertyManager.GetPropertyValueInstance(Config.MainConfig, KeyVar),
                    ConfigType.Screensaver  => PropertyManager.GetPropertyValueInstance(Config.SaverConfig, KeyVar),
                    ConfigType.Splash       => PropertyManager.GetPropertyValueInstance(Config.SplashConfig, KeyVar),
                    _                       => null,
                };
            }
            return null;
        }

    }
}
