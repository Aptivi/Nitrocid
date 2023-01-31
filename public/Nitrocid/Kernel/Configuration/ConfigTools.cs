
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
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KS.Kernel.Events;
using static KS.Kernel.Configuration.Config;
using KS.ConsoleBase.Colors;
using ColorSeq;

namespace KS.Kernel.Configuration
{
    /// <summary>
    /// Configuration tools
    /// </summary>
    public static class ConfigTools
    {

        /// <summary>
        /// Reloads config
        /// </summary>
        public static void ReloadConfig()
        {
            EventsManager.FireEvent(EventType.PreReloadConfig);
            InitializeConfig();
            EventsManager.FireEvent(EventType.PostReloadConfig);
        }

        /// <summary>
        /// Reloads config
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryReloadConfig()
        {
            try
            {
                ReloadConfig();
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to reload config: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the config needs repair and repairs it as necessary.
        /// </summary>
        /// <returns>True if the config is repaired; False if no repairs done; Throws exceptions if unsuccessful.</returns>
        public static bool RepairConfig()
        {
            // Variables
            var FixesNeeded = false;

            // General sections
            int ExpectedSections = PristineConfigToken.Count;

            // Check for missing sections
            if (ConfigToken.Count != ExpectedSections)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Missing sections. Config fix needed set to true.");
                FixesNeeded = true;
            }

            // Go through sections
            try
            {
                foreach (KeyValuePair<string, JToken> Section in PristineConfigToken)
                {
                    if (ConfigToken[Section.Key] is not null)
                    {
                        // Check the normal keys count
                        if (ConfigToken[Section.Key].Count() != PristineConfigToken[Section.Key].Count())
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Missing sections and/or keys in {0}. Config fix needed set to true.", Section.Key);
                            FixesNeeded = true;
                        }
                        else
                        {
                            // Count is the same, but verify the names
                            for (int i = 0; i < PristineConfigToken[Section.Key].Count(); i++)
                            {
                                JProperty token = (JProperty)ConfigToken[Section.Key].ElementAt(i);
                                JProperty pristineToken = (JProperty)PristineConfigToken[Section.Key].ElementAt(i);
                                if (token.Name != pristineToken.Name)
                                {
                                    DebugWriter.WriteDebug(DebugLevel.W, "Name inconsistency. Expected: {0}, Actual: {1}. Config fix needed set to true.", pristineToken.Name, token.Name);
                                    FixesNeeded = true;
                                }
                            }
                        }

                        // Check the screensaver keys
                        if (Section.Key == "Screensaver")
                        {
                            foreach (JProperty ScreensaverSection in PristineConfigToken["Screensaver"].Where(x => x.First.Type == JTokenType.Object))
                            {
                                if (ConfigToken["Screensaver"][ScreensaverSection.Name].Count() != PristineConfigToken["Screensaver"][ScreensaverSection.Name].Count())
                                {
                                    DebugWriter.WriteDebug(DebugLevel.W, "Missing sections and/or keys in Screensaver > {0}. Config fix needed set to true.", ScreensaverSection.Name);
                                    FixesNeeded = true;
                                }
                                else
                                {
                                    // Count is the same, but verify the names
                                    for (int i = 0; i < PristineConfigToken["Screensaver"][ScreensaverSection.Name].Count(); i++)
                                    {
                                        JProperty token = (JProperty)ConfigToken["Screensaver"][ScreensaverSection.Name].ElementAt(i);
                                        JProperty pristineToken = (JProperty)PristineConfigToken["Screensaver"][ScreensaverSection.Name].ElementAt(i);
                                        if (token.Name != pristineToken.Name)
                                        {
                                            DebugWriter.WriteDebug(DebugLevel.W, "Name inconsistency. Expected: {0}, Actual: {1}. Config fix needed set to true.", pristineToken.Name, token.Name);
                                            FixesNeeded = true;
                                        }
                                    }
                                }
                            }
                        }

                        // Check the splash keys
                        if (Section.Key == "Splash")
                        {
                            foreach (JProperty SplashSection in PristineConfigToken["Splash"].Where(x => x.First.Type == JTokenType.Object))
                            {
                                if (ConfigToken["Splash"][SplashSection.Name].Count() != PristineConfigToken["Splash"][SplashSection.Name].Count())
                                {
                                    DebugWriter.WriteDebug(DebugLevel.W, "Missing sections and/or keys in Splash > {0}. Config fix needed set to true.", SplashSection.Name);
                                    FixesNeeded = true;
                                }
                                else
                                {
                                    // Count is the same, but verify the names
                                    for (int i = 0; i < PristineConfigToken["Splash"][SplashSection.Name].Count(); i++)
                                    {
                                        JProperty token = (JProperty)ConfigToken["Splash"][SplashSection.Name].ElementAt(i);
                                        JProperty pristineToken = (JProperty)PristineConfigToken["Splash"][SplashSection.Name].ElementAt(i);
                                        if (token.Name != pristineToken.Name)
                                        {
                                            DebugWriter.WriteDebug(DebugLevel.W, "Name inconsistency. Expected: {0}, Actual: {1}. Config fix needed set to true.", pristineToken.Name, token.Name);
                                            FixesNeeded = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Somehow, the config is corrupt or something. Fix it.
                DebugWriter.WriteDebug(DebugLevel.E, "Found a serious error in configuration: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                FixesNeeded = true;
            }

            // If the fixes are needed, try to remake config with parsed values
            if (FixesNeeded)
                CreateConfig();
            return FixesNeeded;
        }

        /// <summary>
        /// Gets the JSON token for specific configuration category with an optional sub-category
        /// </summary>
        /// <param name="ConfigCategory">Config category</param>
        /// <param name="ConfigSubCategoryName">Sub-category name. Should be an Object. Currently used for screensavers</param>
        public static JToken GetConfigCategory(ConfigCategory ConfigCategory, string ConfigSubCategoryName = "")
        {
            // Try to parse the config category
            DebugWriter.WriteDebug(DebugLevel.I, "Parsing config category {0}...", ConfigCategory);
            if (Enum.TryParse(((int)ConfigCategory).ToString(), out ConfigCategory))
            {
                // We got a valid category. Now, get the token for the specific category
                DebugWriter.WriteDebug(DebugLevel.I, "Category {0} found! Parsing sub-category {1} ({2})...", ConfigCategory, ConfigSubCategoryName, ConfigSubCategoryName.Length);
                var CategoryToken = ConfigToken[ConfigCategory.ToString()];

                // Try to get the sub-category token and check to see if it's found or not
                var SubCategoryToken = CategoryToken[ConfigSubCategoryName];
                if (!string.IsNullOrWhiteSpace(ConfigSubCategoryName) & SubCategoryToken is not null)
                {
                    // We got the subcategory! Check to see if it's really a sub-category (Object) or not
                    DebugWriter.WriteDebug(DebugLevel.I, "Sub-category {0} found! Is it really the sub-category? Type = {1}", ConfigSubCategoryName, SubCategoryToken.Type);
                    if (SubCategoryToken.Type == JTokenType.Object)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "It is really a sub-category!");
                        return SubCategoryToken;
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "It is not really a sub-category. Returning master category...");
                        return CategoryToken;
                    }
                }
                else
                {
                    // We only got the full category.
                    DebugWriter.WriteDebug(DebugLevel.I, "Returning master category...");
                    return CategoryToken;
                }
            }
            else
            {
                // We didn't get a category.
                DebugWriter.WriteDebug(DebugLevel.E, "Category {0} not found!", ConfigCategory);
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Config category {0} not found."), ConfigCategory);
            }
        }

        /// <summary>
        /// Sets the value of an entry in a category.
        /// </summary>
        /// <param name="ConfigCategory">Config category</param>
        /// <param name="ConfigEntryName">Config entry name.</param>
        /// <param name="ConfigValue">Config entry value to install</param>
        public static void SetConfigValue(ConfigCategory ConfigCategory, string ConfigEntryName, JToken ConfigValue) => SetConfigValue(ConfigCategory, GetConfigCategory(ConfigCategory), ConfigEntryName, ConfigValue);

        /// <summary>
        /// Sets the value of an entry in a category.
        /// </summary>
        /// <param name="ConfigCategory">Config category</param>
        /// <param name="ConfigSubCategoryName">Configuration subcategory name</param>
        /// <param name="ConfigEntryName">Config entry name.</param>
        /// <param name="ConfigValue">Config entry value to install</param>
        public static void SetConfigValue(ConfigCategory ConfigCategory, string ConfigSubCategoryName, string ConfigEntryName, JToken ConfigValue) => SetConfigValue(ConfigCategory, GetConfigCategory(ConfigCategory, ConfigSubCategoryName), ConfigEntryName, ConfigValue);

        /// <summary>
        /// Sets the value of an entry in a category.
        /// </summary>
        /// <param name="ConfigCategory">Config category</param>
        /// <param name="ConfigCategoryToken">Config category or sub-category token (You can get it from <see cref="GetConfigCategory(ConfigCategory, string)"/></param>
        /// <param name="ConfigEntryName">Config entry name.</param>
        /// <param name="ConfigValue">Config entry value to install</param>
        public static void SetConfigValue(ConfigCategory ConfigCategory, JToken ConfigCategoryToken, string ConfigEntryName, JToken ConfigValue)
        {
            // Try to parse the config category
            DebugWriter.WriteDebug(DebugLevel.I, "Parsing config category {0}...", ConfigCategory);
            if (Enum.TryParse(((int)ConfigCategory).ToString(), out ConfigCategory))
            {
                // We have a valid category. Now, find the config entry property in the token
                DebugWriter.WriteDebug(DebugLevel.I, "Parsing config entry {0}...", ConfigEntryName);
                var CategoryToken = ConfigToken[ConfigCategory.ToString()];
                if (ConfigCategoryToken[ConfigEntryName] is not null)
                {
                    // Assign the new value to it and write the changes to the token and the config file. Don't worry, debuggers, when you set the value like below,
                    // it will automatically save the changes to ConfigToken as in three lines above
                    DebugWriter.WriteDebug(DebugLevel.E, "Entry {0} found! Setting value...", ConfigEntryName);
                    ConfigCategoryToken[ConfigEntryName] = ConfigValue;

                    // Write the changes to the config file
                    File.WriteAllText(Paths.GetKernelPath(KernelPathType.Configuration), JsonConvert.SerializeObject(ConfigToken, Formatting.Indented));
                }
                else
                {
                    // We didn't get an entry.
                    DebugWriter.WriteDebug(DebugLevel.E, "Entry {0} not found!", ConfigEntryName);
                    throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Config entry {0} not found."), ConfigEntryName);
                }
            }
            else
            {
                // We didn't get a category.
                DebugWriter.WriteDebug(DebugLevel.E, "Category {0} not found!", ConfigCategory);
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Config category {0} not found."), ConfigCategory);
            }
        }

        /// <summary>
        /// Gets the value of an entry in a category.
        /// </summary>
        /// <param name="ConfigCategory">Config category</param>
        /// <param name="ConfigEntryName">Config entry name.</param>
        public static JToken GetConfigValue(ConfigCategory ConfigCategory, string ConfigEntryName) => GetConfigValue(ConfigCategory, GetConfigCategory(ConfigCategory), ConfigEntryName);

        /// <summary>
        /// Gets the value of an entry in a category.
        /// </summary>
        /// <param name="ConfigCategory">Config category</param>
        /// <param name="ConfigSubCategoryName">Configuration subcategory name</param>
        /// <param name="ConfigEntryName">Config entry name.</param>
        public static JToken GetConfigValue(ConfigCategory ConfigCategory, string ConfigSubCategoryName, string ConfigEntryName) => GetConfigValue(ConfigCategory, GetConfigCategory(ConfigCategory, ConfigSubCategoryName), ConfigEntryName);

        /// <summary>
        /// Gets the value of an entry in a category.
        /// </summary>
        /// <param name="ConfigCategory">Config category</param>
        /// <param name="ConfigCategoryToken">Config category or sub-category token (You can get it from <see cref="GetConfigCategory(ConfigCategory, string)"/></param>
        /// <param name="ConfigEntryName">Config entry name.</param>
        public static JToken GetConfigValue(ConfigCategory ConfigCategory, JToken ConfigCategoryToken, string ConfigEntryName)
        {
            // Try to parse the config category
            DebugWriter.WriteDebug(DebugLevel.I, "Parsing config category {0}...", ConfigCategory);
            if (Enum.TryParse(((int)ConfigCategory).ToString(), out ConfigCategory))
            {
                // We have a valid category. Now, find the config entry property in the token
                DebugWriter.WriteDebug(DebugLevel.I, "Parsing config entry {0}...", ConfigEntryName);
                var CategoryToken = ConfigToken[ConfigCategory.ToString()];
                if (ConfigCategoryToken[ConfigEntryName] is not null)
                {
                    // We got the appropriate value! Return it.
                    DebugWriter.WriteDebug(DebugLevel.E, "Entry {0} found! Getting value...", ConfigEntryName);
                    return ConfigCategoryToken[ConfigEntryName];
                }
                else
                {
                    // We didn't get an entry.
                    DebugWriter.WriteDebug(DebugLevel.E, "Entry {0} not found!", ConfigEntryName);
                    throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Config entry {0} not found."), ConfigEntryName);
                }
            }
            else
            {
                // We didn't get a category.
                DebugWriter.WriteDebug(DebugLevel.E, "Category {0} not found!", ConfigCategory);
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Config category {0} not found."), ConfigCategory);
            }
        }

        /// <summary>
        /// Finds a setting with the matching pattern
        /// </summary>
        public static List<InputChoiceInfo> FindSetting(string Pattern, JToken SettingsToken)
        {
            var Results = new List<InputChoiceInfo>();

            // Search the settings for the given pattern
            try
            {
                for (int SectionIndex = 0; SectionIndex <= SettingsToken.Count() - 1; SectionIndex++)
                {
                    var SectionToken = SettingsToken.ToList()[SectionIndex];
                    for (int SettingIndex = 0; SettingIndex <= SectionToken.Count() - 1; SettingIndex++)
                    {
                        var SettingToken = SectionToken.ToList()[SettingIndex]["Keys"];
                        for (int KeyIndex = 0; KeyIndex <= SettingToken.Count() - 1; KeyIndex++)
                        {
                            var Setting = SettingToken.ToList()[KeyIndex];
                            object CurrentValue = GetValueFromEntry(Setting);
                            string KeyName = Translate.DoTranslation((string)Setting["Name"]) + $" [{CurrentValue}]";
                            if (Regex.IsMatch(KeyName, Pattern, RegexOptions.IgnoreCase))
                            {
                                string desc = (string)Setting["Description"] ?? "";
                                InputChoiceInfo ici = new($"{SectionIndex + 1}/{KeyIndex + 1}", KeyName, desc);
                                Results.Add(ici);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to find setting {0}: {1}", Pattern, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }

            // Return the results
            return Results;
        }

        /// <summary>
        /// Checks all the config variables to see if they can be parsed
        /// </summary>
        public static Dictionary<string, bool> CheckConfigVariables()
        {
            var SettingsToken = JToken.Parse(Properties.Resources.Resources.SettingsEntries);
            var SaverSettingsToken = JToken.Parse(Properties.Resources.Resources.ScreensaverSettingsEntries);
            var SplashSettingsToken = JToken.Parse(Properties.Resources.Resources.SplashSettingsEntries);
            var Tokens = new[] { SettingsToken, SaverSettingsToken, SplashSettingsToken };
            var Results = new Dictionary<string, bool>();

            // Parse all the settings
            foreach (JToken Token in Tokens)
            {
                foreach (JProperty Section in Token)
                {
                    var SectionToken = Token[Section.Name];
                    foreach (JToken Key in SectionToken["Keys"])
                    {
                        string KeyName = (string)Key["Name"];
                        string KeyVariable = (string)Key["Variable"];
                        string KeyEnumeration = (string)Key["Enumeration"];
                        bool KeyEnumerationInternal = (bool)(Key["EnumerationInternal"] ?? false);
                        string KeyEnumerationAssembly = (string)Key["EnumerationAssembly"];
                        bool KeyIsInternal = (bool)(Key["IsInternal"] ?? false);
                        bool KeyFound;

                        // Check the variable
                        KeyFound = FieldManager.CheckField(KeyVariable, KeyIsInternal) | PropertyManager.CheckProperty(KeyVariable);
                        Results.Add($"{KeyName}, {KeyVariable}", KeyFound);

                        // Check the enumeration
                        if (KeyEnumeration is not null)
                        {
                            bool Result;
                            if (KeyEnumerationInternal)
                            {
                                // Apparently, we need to have a full assembly name for getting types.
                                Result = Type.GetType("KS." + KeyEnumeration + ", " + Assembly.GetExecutingAssembly().FullName) is not null;
                            }
                            else
                            {
                                Result = Type.GetType(KeyEnumeration + ", " + KeyEnumerationAssembly) is not null;
                            }
                            Results.Add($"{KeyName}, {KeyVariable}, {KeyEnumeration}", Result);
                        }
                    }
                }
            }

            // Return the results
            return Results;
        }

        /// <summary>
        /// Gets the value from a settings entry
        /// </summary>
        /// <param name="Setting">An entry to get the value from</param>
        /// <returns>The value</returns>
        public static object GetValueFromEntry(JToken Setting)
        {
            object CurrentValue = "Unknown";
            string Variable = (string)Setting["Variable"];
            bool VariableIsInternal = (bool)(Setting["IsInternal"] ?? false);
            bool VariableIsEnumerable = (bool)(Setting["IsEnumerable"] ?? false);
            int VariableEnumerableIndex = (int)(Setting["EnumerableIndex"] ?? 0);

            // Print the option by determining how to get the current value
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
            if (CurrentValue is KeyValuePair<KernelColorType, Color> color)
                CurrentValue = color.Value.PlainSequence;
            if (CurrentValue is Color color2)
                CurrentValue = color2.PlainSequence;

            // Get the language name
            if (CurrentValue is LanguageInfo lang)
                CurrentValue = lang.ThreeLetterLanguageName;

            return CurrentValue;
        }

    }
}
