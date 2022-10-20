
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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using ColorSeq;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Screensaver.Customized;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
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
        public static void OpenMainPage(SettingsType SettingsType)
        {
            bool PromptFinished = false;
            var SettingsToken = OpenSettingsResource(SettingsType);
            int MaxSections = SettingsToken.Count();

            while (!PromptFinished)
            {
                // Populate sections
                var sections = new List<string>();
                var sectionNums = new List<int>();
                var altSections = new List<string>()
                {
                    Translate.DoTranslation("Find a Setting"),
                    Translate.DoTranslation("Save Settings"),
                    Translate.DoTranslation("Save Settings As"),
                    Translate.DoTranslation("Load Settings From"),
                    Translate.DoTranslation("Exit"),
                };
                var altSectionNums = new List<int>()
                {
                    MaxSections + 1,
                    MaxSections + 2,
                    MaxSections + 3,
                    MaxSections + 4,
                    MaxSections + 5
                };
                for (int SectionIndex = 0; SectionIndex <= MaxSections - 1; SectionIndex++)
                {
                    JProperty Section = (JProperty)SettingsToken.ToList()[SectionIndex];
                    if (SettingsType != SettingsType.Normal)
                    {
                        sections.Add(Section.Name);
                        sectionNums.Add(SectionIndex + 1);
                    }
                    else
                    {
                        sections.Add(Section.First["DisplayAs"].ToString());
                        sectionNums.Add(SectionIndex + 1);
                    }
                }

                // Prompt for selection and check the answer
                string finalTitle = Translate.DoTranslation("Welcome to Settings!");
                int Answer = SelectionStyle.PromptSelection(finalTitle + CharManager.NewLine + "=".Repeat(finalTitle.Length) + CharManager.NewLine + Translate.DoTranslation("Select section:"), 
                    string.Join("/", sectionNums), sections.ToArray(), 
                    string.Join("/", altSectionNums), altSections.ToArray());
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
                    VariableFinder(SettingsToken);
                }
                else if (Answer == MaxSections + 2)
                {
                    // The selected answer is "Save Settings"
                    DebugWriter.WriteDebug(DebugLevel.I, "Saving settings...");
                    try
                    {
                        Config.CreateConfig();
                        CustomSaverTools.SaveCustomSaverSettings();
                    }
                    catch (Exception ex)
                    {
                        TextWriterColor.Write(ex.Message, true, ColorTools.ColTypes.Error);
                        DebugWriter.WriteDebugStackTrace(ex);
                        ConsoleBase.ConsoleWrapper.ReadKey();
                    }
                }
                else if (Answer == MaxSections + 3)
                {
                    // The selected answer is "Save Settings As"
                    TextWriterColor.Write(Translate.DoTranslation("Where do you want to save the current kernel settings?"), true, ColorTools.ColTypes.Question);
                    string Location = Filesystem.NeutralizePath(Input.ReadLine());
                    if (!Checking.FileExists(Location))
                    {
                        try
                        {
                            Config.CreateConfig(Location);
                        }
                        catch (Exception ex)
                        {
                            TextWriterColor.Write(ex.Message, true, ColorTools.ColTypes.Error);
                            DebugWriter.WriteDebugStackTrace(ex);
                            ConsoleBase.ConsoleWrapper.ReadKey();
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Can't save kernel settings on top of existing file."), true, ColorTools.ColTypes.Error);
                        ConsoleBase.ConsoleWrapper.ReadKey();
                    }
                }
                else if (Answer == MaxSections + 4)
                {
                    // The selected answer is "Load Settings From"
                    TextWriterColor.Write(Translate.DoTranslation("Where do you want to load the current kernel settings from?"), true, ColorTools.ColTypes.Question);
                    string Location = Filesystem.NeutralizePath(Input.ReadLine());
                    if (Checking.FileExists(Location))
                    {
                        try
                        {
                            Config.ReadConfig(Location);
                            Config.CreateConfig();
                        }
                        catch (Exception ex)
                        {
                            TextWriterColor.Write(ex.Message, true, ColorTools.ColTypes.Error);
                            DebugWriter.WriteDebugStackTrace(ex);
                            ConsoleBase.ConsoleWrapper.ReadKey();
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("File not found."), true, ColorTools.ColTypes.Error);
                        ConsoleBase.ConsoleWrapper.ReadKey();
                    }
                }
                else if (Answer == MaxSections + 5)
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
                    TextWriterColor.Write(Translate.DoTranslation("Specified option {0} is invalid."), true, ColorTools.ColTypes.Error, Answer);
                    TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                    ConsoleBase.ConsoleWrapper.ReadKey();
                }
            }
        }

        /// <summary>
        /// Open section
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="SettingsToken">Settings token</param>
        /// <param name="SettingsType">Settings type</param>
        public static void OpenSection(string Section, JToken SettingsToken, SettingsType SettingsType)
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
                    var sections = new List<string>();
                    var sectionNums = new List<int>();
                    var altSections = new List<string>()
                    {
                        Translate.DoTranslation("Go Back...")
                    };
                    var altSectionNums = new List<int>()
                    {
                        MaxOptions + 1
                    };
                    for (int SectionIndex = 0; SectionIndex <= MaxOptions - 1; SectionIndex++)
                    {
                        var Setting = SectionToken[SectionIndex];
                        object CurrentValue = "Unknown";
                        string Variable = (string)Setting["Variable"];
                        string VariableProperty = (string)Setting["VariableProperty"];
                        bool VariableIsInternal = (bool)(Setting["IsInternal"] ?? false);
                        bool VariableIsEnumerable = (bool)(Setting["IsEnumerable"] ?? false);
                        int VariableEnumerableIndex = (int)(Setting["EnumerableIndex"] ?? 0);
                        SettingsKeyType VariableType = (SettingsKeyType)Convert.ToInt32(Enum.Parse(typeof(SettingsKeyType), (string)Setting["Type"]));

                        // Print the option by determining how to get the current value
                        if (VariableProperty is null)
                        {
                            if (FieldManager.CheckField(Variable, VariableIsInternal))
                            {
                                // We're dealing with the field, get the value from it. However, check to see if that field is an enumerable
                                if (VariableIsEnumerable)
                                    CurrentValue = FieldManager.GetValueFromEnumerable(Variable, VariableEnumerableIndex, VariableIsInternal);
                                else
                                    CurrentValue = FieldManager.GetValue(Variable, VariableIsInternal);
                            }
                            else if (PropertyManager.CheckProperty(Variable))
                            {
                                // We're dealing with the property, get the value from it
                                CurrentValue = PropertyManager.GetPropertyValue(Variable);
                            }

                            // Get the plain sequence from the color
                            if (CurrentValue is KeyValuePair<ColorTools.ColTypes, Color> color)
                            {
                                CurrentValue = color.Value.PlainSequence;
                            }
                        }
                        else
                        {
                            // Get the property value from variable
                            CurrentValue = PropertyManager.GetPropertyValueInVariable(Variable, VariableProperty);
                        }
                        sections.Add($"{Setting["Name"]} [{CurrentValue}]");
                        sectionNums.Add(SectionIndex + 1);
                    }
                    if (SettingsType == SettingsType.Screensaver)
                    {
                        altSections.Add(Translate.DoTranslation("Preview screensaver"));
                        altSectionNums.Add(MaxOptions + 2);
                    }
                    else if (SettingsType == SettingsType.Splash)
                    {
                        altSections.Add(Translate.DoTranslation("Preview splash"));
                        altSectionNums.Add(MaxOptions + 2);
                    }
                    DebugWriter.WriteDebug(DebugLevel.W, "Section {0} has {1} selections.", Section, MaxOptions);

                    // Prompt user and check for input
                    string finalSection = SectionTranslateName ? Translate.DoTranslation((string)SectionDisplayName) : (string)SectionDisplayName;
                    int Answer = SelectionStyle.PromptSelection(finalSection + CharManager.NewLine + "=".Repeat(finalSection.Length) + CharManager.NewLine + Translate.DoTranslation((string)SectionDescription),
                        string.Join("/", sectionNums), sections.ToArray(),
                        string.Join("/", altSectionNums), altSections.ToArray());
                    DebugWriter.WriteDebug(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...");
                    if (Answer >= 1 & Answer <= MaxOptions)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Opening key {0} from section {1}...", Answer, Section);
                        OpenKey(Section, Answer, SettingsToken);
                    }
                    else if (Answer == MaxOptions + 1 & SettingsType == SettingsType.Screensaver)
                    {
                        // Preview screensaver
                        DebugWriter.WriteDebug(DebugLevel.I, "User requested screensaver preview.");
                        Screensaver.Screensaver.ShowSavers(Section);
                    }
                    else if (Answer == MaxOptions + 1 & SettingsType == SettingsType.Splash)
                    {
                        // Preview splash
                        DebugWriter.WriteDebug(DebugLevel.I, "User requested splash preview.");
                        Splash.SplashManager.PreviewSplash(Section);
                    }
                    else if (Answer == MaxOptions + 1 | Answer == MaxOptions + 2 & (SettingsType == SettingsType.Screensaver || SettingsType == SettingsType.Splash))
                    {
                        // Go Back...
                        DebugWriter.WriteDebug(DebugLevel.I, "User requested exit. Returning...");
                        SectionFinished = true;
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Option is not valid. Returning...");
                        TextWriterColor.Write(Translate.DoTranslation("Specified option {0} is invalid."), true, ColorTools.ColTypes.Error, Answer);
                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                        ConsoleBase.ConsoleWrapper.ReadKey();
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleBase.ConsoleWrapper.Clear();
                DebugWriter.WriteDebug(DebugLevel.I, "Error trying to open section: {0}", ex.Message);
                SeparatorWriterColor.WriteSeparator("???", true);
                TextWriterColor.Write(CharManager.NewLine + "X) " + Translate.DoTranslation("Invalid section entered. Please go back."), true, ColorTools.ColTypes.Error);
                TextWriterColor.Write("X) " + Translate.DoTranslation("If you're sure that you've opened the right section, check this message out:"), true, ColorTools.ColTypes.Error);
                TextWriterColor.Write("X) " + ex.Message, true, ColorTools.ColTypes.Error);
                ConsoleBase.ConsoleWrapper.ReadKey();
            }
        }

        /// <summary>
        /// Open a key.
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="KeyNumber">Key number</param>
        /// <param name="SettingsToken">Settings token</param>
        public static void OpenKey(string Section, int KeyNumber, JToken SettingsToken)
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
                string KeyVarProperty = (string)KeyToken["VariableProperty"];
                bool KeyIsInternal = (bool)(KeyToken["IsInternal"] ?? false);
                bool KeyIsEnumerable = (bool)(KeyToken["IsEnumerable"] ?? false);
                int KeyEnumerableIndex = (int)(KeyToken["EnumerableIndex"] ?? 0);
                object KeyValue = "";
                object KeyDefaultValue = "";
                bool KeyFinished = false;

                // Integer slider properties
                int IntSliderMinimumValue = (int)(KeyToken["MinimumValue"] ?? 0);
                int IntSliderMaximumValue = (int)(KeyToken["MaximumValue"] ?? 100);

                // Selection properties
                bool SelectionEnum = (bool)(KeyToken["IsEnumeration"] ?? false);
                string SelectionEnumAssembly = (string)KeyToken["EnumerationAssembly"];
                bool SelectionEnumInternal = (bool)(KeyToken["EnumerationInternal"] ?? false);
                bool SelectionEnumZeroBased = (bool)(KeyToken["EnumerationZeroBased"] ?? false);

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

                // Inputs
                string AnswerString = "";

                while (!KeyFinished)
                {
                    if (KeyType == SettingsKeyType.SUnknown)
                        break;

                    // Determine how to get key default value
                    if (KeyVarProperty is null)
                    {
                        if (FieldManager.CheckField(KeyVar, KeyIsInternal))
                        {
                            // We're dealing with the field, get the value from it. However, check to see if that field is an enumerable
                            if (KeyIsEnumerable)
                                KeyDefaultValue = FieldManager.GetValueFromEnumerable(KeyVar, KeyEnumerableIndex, KeyIsInternal);
                            else
                                KeyDefaultValue = FieldManager.GetValue(KeyVar, KeyIsInternal);
                        }
                        else if (PropertyManager.CheckProperty(KeyVar))
                        {
                            // We're dealing with the property, get the value from it
                            KeyDefaultValue = PropertyManager.GetPropertyValue(KeyVar);
                        }

                        // Get the plain sequence from the color
                        if (KeyDefaultValue is Color color)
                        {
                            KeyDefaultValue = color.PlainSequence;
                        }
                    }
                    else
                    {
                        // Get the property value from variable
                        KeyDefaultValue = PropertyManager.GetPropertyValueInVariable(KeyVar, KeyVarProperty);
                    }

                    // How the settings app displays the options and parses the output varies by the keytype
                    switch (KeyType)
                    {
                        case SettingsKeyType.SChar:
                        case SettingsKeyType.SInt:
                        case SettingsKeyType.SList:
                        case SettingsKeyType.SString:
                            ConsoleBase.ConsoleWrapper.Clear();

                            // Make an introductory banner
                            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation(Section + " Settings...") + " > " + Translate.DoTranslation(KeyName), true);
                            TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation(KeyDescription), true, ColorTools.ColTypes.NeutralText);

                            // Write the prompt
                            if (!(KeyType == SettingsKeyType.SIntSlider))
                                TextWriterColor.Write("[{0}] > ", false, ColorTools.ColTypes.Input, KeyDefaultValue);

                            // Get the target list from the method defined in the manifest (SelectionFunctionName)
                            if (KeyType == SettingsKeyType.SList)
                            {
                                TargetList = (IEnumerable<object>)MethodManager.GetMethod(ListFunctionName).Invoke(ListFunctionType, null);
                                TextWriterColor.Write(Translate.DoTranslation("Current items:"), true, ColorTools.ColTypes.ListTitle);
                                ListWriterColor.WriteList(TargetList);
                                TextWriterColor.Write();
                                TextWriterColor.Write(CharManager.NewLine + " q) " + Translate.DoTranslation("Save Changes...") + CharManager.NewLine, true, ColorTools.ColTypes.Option);

                                // Prompt the user and parse the answer
                                TextWriterColor.Write("> ", false, ColorTools.ColTypes.Input);
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
                                        TextWriterColor.Write("> ", false, ColorTools.ColTypes.Input);
                                    }
                                }
                            }
                            else if (KeyType == SettingsKeyType.SChar)
                                AnswerString = Convert.ToString(ConsoleBase.ConsoleWrapper.ReadKey().KeyChar);
                            else
                                AnswerString = Input.ReadLine();

                            // Neutralize path if required with the assumption that the keytype is not list
                            if (NeutralizePaths)
                                AnswerString = Filesystem.NeutralizePath(AnswerString, NeutralizeRootPath);
                            DebugWriter.WriteDebug(DebugLevel.I, "User answered {0}", AnswerString);

                            break;
                        case SettingsKeyType.SSelection:
                            // TEMP
                            ConsoleBase.ConsoleWrapper.Clear();

                            // Make an introductory banner
                            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation(Section + " Settings...") + " > " + Translate.DoTranslation(KeyName), true);
                            TextWriterColor.Write(CharManager.NewLine + Translate.DoTranslation(KeyDescription), true, ColorTools.ColTypes.NeutralText);
                            // TEMP END

                            // Determine which list we're going to select
                            if (SelectionEnum)
                            {
                                if (SelectionEnumInternal)
                                {
                                    // Apparently, we need to have a full assembly name for getting types.
                                    SelectFrom = Type.GetType("KS." + KeyToken["Enumeration"].ToString() + ", " + Assembly.GetExecutingAssembly().FullName).GetEnumNames();
                                    Selections = Type.GetType("KS." + KeyToken["Enumeration"].ToString() + ", " + Assembly.GetExecutingAssembly().FullName).GetEnumValues();
                                    MaxKeyOptions = SelectFrom.Count();
                                }
                                else
                                {
                                    SelectFrom = Type.GetType(KeyToken["Enumeration"].ToString() + ", " + SelectionEnumAssembly).GetEnumNames();
                                    Selections = Type.GetType(KeyToken["Enumeration"].ToString() + ", " + SelectionEnumAssembly).GetEnumValues();
                                    MaxKeyOptions = SelectFrom.Count();
                                }
                            }
                            else
                            {
                                SelectFrom = (IEnumerable<object>)MethodManager.GetMethod((string)KeyToken["SelectionFunctionName"]).Invoke(KeyToken["SelectionFunctionType"], null);
                                MaxKeyOptions = SelectFrom.Count();
                            }

                            TextWriterColor.Write(Translate.DoTranslation("Current items:"), true, ColorTools.ColTypes.ListTitle);
                            ListWriterColor.WriteList(SelectFrom);
                            TextWriterColor.Write();
                            TextWriterColor.Write(" {0}) " + Translate.DoTranslation("Go Back...") + CharManager.NewLine, true, ColorTools.ColTypes.BackOption, MaxKeyOptions + 1);
                            AnswerString = Input.ReadLine();

                            break;
                        case SettingsKeyType.SColor:
                            Color keyColorValue = Color.Empty;

                            // Check to see if the color is contained in the dictionary
                            if (KeyDefaultValue is KeyValuePair<ColorTools.ColTypes, Color> keyColorValuePair)
                                keyColorValue = keyColorValuePair.Value;
                            else if (KeyDefaultValue is string keyColorString)
                                keyColorValue = new Color(keyColorString);

                            // Get the color value from the color wheel
                            ColorValue = ColorWheelOpen.ColorWheel(
                                // Determine if the color is true color
                                keyColorValue.Type == ColorType.TrueColor,

                                // Get the ConsoleColors number from the current color value
                                (ConsoleColors)Convert.ToInt32(
                                    keyColorValue.Type == ColorType._255Color || keyColorValue.Type == ColorType._16Color ?
                                    keyColorValue.PlainSequence :
                                    ConsoleColors.White
                                ),

                                // Now, get the RGB from the color class
                                keyColorValue.R, keyColorValue.G, keyColorValue.B
                            );

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
                                TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Current value:") + " {0} / {1} - {2}" + Convert.ToString(CharManager.GetEsc()) + "[0K", 5, ConsoleBase.ConsoleWrapper.WindowHeight - 5, false, ColorTools.ColTypes.NeutralText, CurrentValue, IntSliderMinimumValue, IntSliderMaximumValue);

                                // Parse the user input
                                PressedKey = ConsoleBase.ConsoleWrapper.ReadKey().Key;
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
                                    if (FieldManager.CheckField(KeyVar))
                                    {
                                        // We're dealing with the field
                                        FieldManager.SetValue(KeyVar, (object)FinalBool, true);
                                    }
                                    else if (PropertyManager.CheckProperty(KeyVar))
                                    {
                                        // We're dealing with the property
                                        PropertyManager.SetPropertyValue(KeyVar, (object)FinalBool);
                                    }
                                    break;
                                }
                            case SettingsKeyType.SSelection:
                                {
                                    // We're dealing with selection
                                    DebugWriter.WriteDebug(DebugLevel.I, "Answer is numeric and key is of the selection type.");
                                    int AnswerIndex = AnswerInt - 1;
                                    if (AnswerInt == MaxKeyOptions + 1) // Go Back...
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
                                            if (FieldManager.CheckField(KeyVar))
                                            {
                                                // We're dealing with the field
                                                FieldManager.SetValue(KeyVar, selectionsArray.ToArray()[AnswerIndex], true);
                                            }
                                            else if (PropertyManager.CheckProperty(KeyVar))
                                            {
                                                // We're dealing with the property
                                                PropertyManager.SetPropertyValue(KeyVar, selectionsArray.ToArray()[AnswerIndex]);
                                            }
                                        }
                                        else if (!(AnswerInt > MaxKeyOptions))
                                        {
                                            object FinalValue;
                                            if (!SelectionEnum)
                                            {
                                                DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt);
                                                KeyFinished = true;
                                                FinalValue = SelectFrom.ElementAtOrDefault(AnswerInt - 1);
                                            }
                                            else
                                            {
                                                DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt);
                                                KeyFinished = true;
                                                FinalValue = AnswerInt;
                                            }

                                            // Now, set the value
                                            if (FieldManager.CheckField(KeyVar))
                                            {
                                                // We're dealing with the field
                                                FieldManager.SetValue(KeyVar, FinalValue, true);
                                            }
                                            else if (PropertyManager.CheckProperty(KeyVar))
                                            {
                                                // We're dealing with the property
                                                PropertyManager.SetPropertyValue(KeyVar, FinalValue);
                                            }
                                        }
                                        else
                                        {
                                            DebugWriter.WriteDebug(DebugLevel.W, "Answer is not valid.");
                                            TextWriterColor.Write(Translate.DoTranslation("The answer may not exceed the entries shown."), true, ColorTools.ColTypes.Error);
                                            TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                                            ConsoleBase.ConsoleWrapper.ReadKey();
                                        }
                                    }
                                    else if (AnswerInt == 0 & !SelectionEnumZeroBased)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.W, "Zero is not allowed.");
                                        TextWriterColor.Write(Translate.DoTranslation("The answer may not be zero."), true, ColorTools.ColTypes.Error);
                                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                                        ConsoleBase.ConsoleWrapper.ReadKey();
                                    }
                                    else
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.W, "Negative values are disallowed.");
                                        TextWriterColor.Write(Translate.DoTranslation("The answer may not be negative."), true, ColorTools.ColTypes.Error);
                                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                                        ConsoleBase.ConsoleWrapper.ReadKey();
                                    }

                                    break;
                                }
                            case SettingsKeyType.SInt:
                                {
                                    // We're dealing with integers
                                    DebugWriter.WriteDebug(DebugLevel.I, "Answer is numeric and key is of the integer type.");
                                    int AnswerIndex = AnswerInt - 1;
                                    if (AnswerInt >= 0)
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt);
                                        KeyFinished = true;

                                        // Now, set the value
                                        if (FieldManager.CheckField(KeyVar))
                                        {
                                            // We're dealing with the field
                                            FieldManager.SetValue(KeyVar, AnswerInt, true);
                                        }
                                        else if (PropertyManager.CheckProperty(KeyVar))
                                        {
                                            // We're dealing with the property
                                            PropertyManager.SetPropertyValue(KeyVar, AnswerInt);
                                        }
                                    }
                                    else
                                    {
                                        DebugWriter.WriteDebug(DebugLevel.W, "Negative values are disallowed.");
                                        TextWriterColor.Write(Translate.DoTranslation("The answer may not be negative."), true, ColorTools.ColTypes.Error);
                                        TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                                        ConsoleBase.ConsoleWrapper.ReadKey();
                                    }

                                    break;
                                }
                            case SettingsKeyType.SIntSlider:
                                {
                                    // We're dealing with integers with limits
                                    DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt);
                                    KeyFinished = true;

                                    // Now, set the value
                                    if (FieldManager.CheckField(KeyVar))
                                    {
                                        // We're dealing with the field
                                        FieldManager.SetValue(KeyVar, AnswerInt, true);
                                    }
                                    else if (PropertyManager.CheckProperty(KeyVar))
                                    {
                                        // We're dealing with the property
                                        PropertyManager.SetPropertyValue(KeyVar, AnswerInt);
                                    }

                                    break;
                                }
                        }
                    }
                    else if (ReadLineReboot.ReadLine.ReadRanToCompletion)
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
                                    if (FieldManager.CheckField(KeyVar, true))
                                    {
                                        // We're dealing with the field
                                        FieldManager.SetValue(KeyVar, AnswerString, true);
                                    }
                                    else if (PropertyManager.CheckProperty(KeyVar))
                                    {
                                        // We're dealing with the property
                                        PropertyManager.SetPropertyValue(KeyVar, AnswerString);
                                    }

                                    break;
                                }
                            case SettingsKeyType.SList:
                                {
                                    string FinalDelimiter;
                                    DebugWriter.WriteDebug(DebugLevel.I, "Answer is not numeric and key is of the List type. Adding answers to the list...");
                                    KeyFinished = true;

                                    // Get the delimiter
                                    if (ListJoinString is null)
                                    {
                                        FinalDelimiter = Convert.ToString(FieldManager.GetValue(ListJoinStringVariable));
                                    }
                                    else
                                    {
                                        FinalDelimiter = ListJoinString;
                                    }

                                    // Now, set the value
                                    string JoinedString = string.Join(FinalDelimiter, TargetList);
                                    if (FieldManager.CheckField(KeyVar))
                                    {
                                        // We're dealing with the field
                                        FieldManager.SetValue(KeyVar, JoinedString, true);
                                    }
                                    else if (PropertyManager.CheckProperty(KeyVar))
                                    {
                                        // We're dealing with the property
                                        PropertyManager.SetPropertyValue(KeyVar, JoinedString);
                                    }

                                    break;
                                }
                            case SettingsKeyType.SColor:
                                {
                                    object FinalColor;

                                    // KeyVar is not always KernelColors, which is a dictionary. This applies to standard settings. Everything else should
                                    // be either the Color type or a String type.
                                    if (FieldManager.CheckField(KeyVar, KeyIsInternal) &&
                                        FieldManager.GetValue(KeyVar, KeyIsInternal) is Dictionary<ColorTools.ColTypes, Color> colors)
                                    {
                                        var colorTypeOnDict = colors.ElementAt(KeyEnumerableIndex).Key;
                                        colors[colorTypeOnDict] = new Color(ColorValue.ToString());
                                        FinalColor = colors;
                                    }
                                    else if (FieldManager.CheckField(KeyVar, KeyIsInternal) &&
                                             FieldManager.GetField(KeyVar, KeyIsInternal).FieldType == typeof(Color))
                                    {
                                        FinalColor = new Color(ColorValue.ToString());
                                    }
                                    else
                                    {
                                        FinalColor = ColorValue.ToString();
                                    }

                                    // Now, set the value
                                    if (FieldManager.CheckField(KeyVar, KeyIsInternal))
                                    {
                                        // We're dealing with the field
                                        FieldManager.SetValue(KeyVar, FinalColor, KeyIsInternal);
                                    }
                                    else if (PropertyManager.CheckProperty(KeyVar))
                                    {
                                        // We're dealing with the property
                                        PropertyManager.SetPropertyValue(KeyVar, FinalColor);
                                    }
                                    KeyFinished = true;
                                    break;
                                }

                            default:
                                {
                                    DebugWriter.WriteDebug(DebugLevel.W, "Answer is not valid.");
                                    TextWriterColor.Write(Translate.DoTranslation("The answer is invalid. Check to make sure that the answer is numeric for config entries that need numbers as answers."), true, ColorTools.ColTypes.Error);
                                    TextWriterColor.Write(Translate.DoTranslation("Press any key to go back."), true, ColorTools.ColTypes.Error);
                                    ConsoleBase.ConsoleWrapper.ReadKey();
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
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation(Section + " Settings...") + " > ???", true);
                TextWriterColor.Write(CharManager.NewLine + "X) " + Translate.DoTranslation("Invalid section entered. Please go back."), true, ColorTools.ColTypes.Error);
                TextWriterColor.Write("X) " + Translate.DoTranslation("If you're sure that you've opened the right section, check this message out:"), true, ColorTools.ColTypes.Error);
                TextWriterColor.Write("X) " + ex.Message, true, ColorTools.ColTypes.Error);
                ConsoleBase.ConsoleWrapper.ReadKey();
            }
        }

        /// <summary>
        /// A sub for variable finding prompt
        /// </summary>
        public static void VariableFinder(JToken SettingsToken)
        {
            string SearchFor;
            string SettingsNumber;
            List<string> Results;

            // Prompt the user
            TextWriterColor.Write(Translate.DoTranslation("Write what do you want to search for."), true, ColorTools.ColTypes.NeutralText);
            DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for searching...");
            TextWriterColor.Write(">> ", false, ColorTools.ColTypes.Input);
            SearchFor = Input.ReadLine();

            // Search for the setting
            Results = ConfigTools.FindSetting(SearchFor, SettingsToken);

            // Write the settings
            if (!(Results.Count == 0))
            {
                ListWriterColor.WriteList(Results);

                // Prompt for the number of setting to go to
                TextWriterColor.Write(Translate.DoTranslation("Write the number of the setting to go to. Any other character means go back."), true, ColorTools.ColTypes.NeutralText);
                DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for writing...");
                TextWriterColor.Write(">> ", false, ColorTools.ColTypes.Input);
                SettingsNumber = Input.ReadLine();

                // Parse the input and go to setting
                if (StringQuery.IsStringNumeric(SettingsNumber))
                {
                    int ChosenSettingIndex = Convert.ToInt32(SettingsNumber) - 1;
                    string ChosenSetting = Results[ChosenSettingIndex];
                    int SectionIndex = Convert.ToInt32(ChosenSetting.AsSpan().Slice(1, ChosenSetting.IndexOf("/") - 1).ToString()) - 1;
                    int KeyNumber = Convert.ToInt32(ChosenSetting.AsSpan().Slice(ChosenSetting.IndexOf("/") + 1, ChosenSetting.IndexOf("]") - (ChosenSetting.IndexOf("/") + 1)).ToString());
                    JProperty Section = (JProperty)SettingsToken.ToList()[SectionIndex];
                    string SectionName = Section.Name;
                    OpenKey(SectionName, KeyNumber, SettingsToken);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Nothing is found. Make sure that you've written the setting correctly."), true, ColorTools.ColTypes.Error);
                ConsoleBase.ConsoleWrapper.ReadKey();
            }
        }

        /// <summary>
        /// Open the settings resource
        /// </summary>
        /// <param name="SettingsType">The settings type</param>
        private static JToken OpenSettingsResource(SettingsType SettingsType)
        {
            switch (SettingsType)
            {
                case SettingsType.Normal:
                    {
                        return JToken.Parse(Properties.Resources.Resources.SettingsEntries);
                    }
                case SettingsType.Screensaver:
                    {
                        return JToken.Parse(Properties.Resources.Resources.ScreensaverSettingsEntries);
                    }
                case SettingsType.Splash:
                    {
                        return JToken.Parse(Properties.Resources.Resources.SplashSettingsEntries);
                    }
                default:
                    {
                        return JToken.Parse(Properties.Resources.Resources.SettingsEntries);
                    }
            }
        }

    }
}
