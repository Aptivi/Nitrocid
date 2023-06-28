
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
using System.Reflection;
using System.Text.RegularExpressions;
using KS.ConsoleBase.Inputs;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
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
        /// Gets the value from a settings entry
        /// </summary>
        /// <param name="SettingsType">Settings type</param>
        /// <param name="Setting">An entry to get the value from</param>
        /// <returns>The value</returns>
        public static object GetValueFromEntry(JToken Setting, ConfigType SettingsType)
        {
            object CurrentValue = "Unknown";
            string Variable = (string)Setting["Variable"];

            // Print the option by determining how to get the current value
            if (PropertyManager.CheckProperty(Variable))
            {
                // We're dealing with the property, get the value from it
                switch (SettingsType)
                {
                    case ConfigType.Kernel:
                        CurrentValue = PropertyManager.GetPropertyValueInstance(MainConfig, Variable);
                        break;
                    case ConfigType.Screensaver:
                        CurrentValue = PropertyManager.GetPropertyValueInstance(SaverConfig, Variable);
                        break;
                    case ConfigType.Splash:
                        CurrentValue = PropertyManager.GetPropertyValueInstance(SplashConfig, Variable);
                        break;
                    default:
                        DebugCheck.Assert(false, $"dealing with settings type other than kernel, screensaver, and splash. {SettingsType}");
                        break;
                }
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

        /// <summary>
        /// Finds a setting with the matching pattern
        /// </summary>
        public static List<InputChoiceInfo> FindSetting(string Pattern, JToken SettingsToken, ConfigType SettingsType)
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
                            object CurrentValue = GetValueFromEntry(Setting, SettingsType);
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
                        bool KeyFound;

                        // Check the variable
                        KeyFound = FieldManager.CheckField(KeyVariable) | PropertyManager.CheckProperty(KeyVariable);
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

    }
}
