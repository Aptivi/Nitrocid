
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
using System.IO;
using System.Linq;
using System.Reflection;
using ColorSeq;
using Extensification.StringExts;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Reflection;
using KS.Misc.Settings;
using KS.Misc.Splash;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.Prompts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KS.Kernel.Events;
using KS.ConsoleBase.Colors;

namespace KS.Kernel.Configuration
{
    /// <summary>
    /// Configuration module
    /// </summary>
    public static class Config
    {

        /// <summary>
        /// Base config token to be loaded each kernel startup.
        /// </summary>
        internal static JObject ConfigToken;
        /// <summary>
        /// Fallback configuration
        /// </summary>
        internal readonly static JObject PristineConfigToken = GetNewConfigObject();

        /// <summary>
        /// Creates a new JSON object containing the kernel settings of all kinds
        /// </summary>
        /// <returns>A pristine config object</returns>
        public static JObject GetNewConfigObject()
        {
            JToken ConfigMetadata = JToken.Parse(Properties.Resources.Resources.SettingsEntries);
            JToken ScreensaverConfigMetadata = JToken.Parse(Properties.Resources.Resources.ScreensaverSettingsEntries);
            JToken SplashConfigMetadata = JToken.Parse(Properties.Resources.Resources.SplashSettingsEntries);

            // Populate the kernel configuration objects
            JObject KernelConfigObject = GetNewConfigObject(ConfigMetadata);
            JObject ScreensaverConfigObject = GetNewConfigObject(ScreensaverConfigMetadata);
            JObject SplashConfigObject = GetNewConfigObject(SplashConfigMetadata);

            // Add screensaver and splash objects to their own sections
            foreach (var saver in ScreensaverConfigObject)
                ((JObject)KernelConfigObject["Screensaver"]).Add(saver.Key, saver.Value);
            KernelConfigObject.Add("Splash", SplashConfigObject);

            // Return the final result
            return KernelConfigObject;
        }

        /// <summary>
        /// Creates a new JSON object containing the kernel settings of all kinds
        /// </summary>
        /// <returns>A pristine config object</returns>
        internal static JObject GetNewConfigObject(JToken metadata)
        {
            JObject ConfigObject = new();

            // Get the max sections
            int MaxSections = metadata.Count();
            DebugWriter.WriteDebug(DebugLevel.I, "Max sections from metadata: {0}", MaxSections);
            for (int SectionIndex = 0; SectionIndex <= MaxSections - 1; SectionIndex++)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Section index: {0}", SectionIndex);
                JObject ConfigSectionOptionsObject = new();

                // Get the section property and fetch metadata information from section
                JProperty Section = (JProperty)metadata.ToList()[SectionIndex];
                DebugWriter.WriteDebug(DebugLevel.I, "Section name: {0}", Section.Name);
                var SectionTokenGeneral = metadata[Section.Name];
                var SectionToken = SectionTokenGeneral["Keys"];

                // Count the options
                int MaxOptions = SectionToken.Count();
                DebugWriter.WriteDebug(DebugLevel.I, "Number of options: {0}", MaxOptions);
                for (int OptionIndex = 0; OptionIndex <= MaxOptions - 1; OptionIndex++)
                {
                    // Get the setting token and fetch information
                    DebugWriter.WriteDebug(DebugLevel.I, "Option index: {0}", OptionIndex);
                    var Setting = SectionToken[OptionIndex];
                    string VariableKeyName = (string)Setting["Name"];
                    string Variable = (string)Setting["Variable"];
                    bool VariableIsInternal = (bool)(Setting["IsInternal"] ?? false);
                    bool VariableIsEnumerable = (bool)(Setting["IsEnumerable"] ?? false);
                    int VariableEnumerableIndex = (int)(Setting["EnumerableIndex"] ?? 0);
                    DebugWriter.WriteDebug(DebugLevel.I, "Variable key name: {0} [reflecting {1}] with int: {2}, enum: {3}, enumidx: {4}", VariableKeyName, Variable, VariableIsInternal, VariableIsEnumerable, VariableEnumerableIndex);

                    // Get variable value and type
                    SettingsKeyType VariableType = (SettingsKeyType)Convert.ToInt32(Enum.Parse(typeof(SettingsKeyType), (string)Setting["Type"]));
                    DebugWriter.WriteDebug(DebugLevel.I, "Got variable type: {0}", VariableType);
                    object VariableValue = null;

                    // Check the value if we're dealing with an enumerable
                    if (FieldManager.CheckField(Variable, VariableIsInternal) && VariableIsEnumerable)
                        VariableValue = FieldManager.GetValueFromEnumerable(Variable, VariableEnumerableIndex, VariableIsInternal);

                    // Check the variable type, and deal with it as appropriate
                    if (VariableType == SettingsKeyType.SColor)
                    {
                        // Get the plain sequence from the color
                        if (VariableValue is KeyValuePair<KernelColorType, Color> color)
                            VariableValue = color.Value.PlainSequence;
                        else
                        {
                            if (FieldManager.CheckField(Variable, VariableIsInternal))
                            {
                                var finalVal = FieldManager.GetValue(Variable, VariableIsInternal);
                                if (finalVal.GetType() == typeof(Color))
                                    VariableValue = ((Color)finalVal).PlainSequence;
                                else
                                    VariableValue = new Color(((string)finalVal).ReleaseDoubleQuotes()).PlainSequence;
                            }
                            else
                            {
                                var finalVal = PropertyManager.GetPropertyValue(Variable);
                                if (finalVal.GetType() == typeof(Color))
                                    VariableValue = ((Color)finalVal).PlainSequence;
                                else
                                    VariableValue = new Color(((string)finalVal).ReleaseDoubleQuotes()).PlainSequence;
                            }
                        }

                        // Setting entry is color, but the variable could be either String or Color.
                        if ((FieldManager.CheckField(Variable, VariableIsInternal) && FieldManager.GetField(Variable, VariableIsInternal).FieldType == typeof(string)) ||
                            (PropertyManager.CheckProperty(Variable) && PropertyManager.GetProperty(Variable).PropertyType == typeof(string)))
                        {
                            // We're dealing with the field or the property which takes color but is a string containing plain sequence
                            VariableValue = new Color((string)VariableValue).PlainSequence;
                        }
                        DebugWriter.WriteDebug(DebugLevel.I, "Got color var value: {0}", VariableValue);
                    }
                    else if (VariableType == SettingsKeyType.SPreset)
                    {
                        if (VariableValue is KeyValuePair<string, PromptPresetBase> preset)
                            VariableValue = preset.Value.PresetName;
                        DebugWriter.WriteDebug(DebugLevel.I, "Got var value: {0}", VariableValue);
                    }
                    else
                    {
                        if (FieldManager.CheckField(Variable, VariableIsInternal))
                            VariableValue = FieldManager.GetValue(Variable, VariableIsInternal);
                        else
                            VariableValue = PropertyManager.GetPropertyValue(Variable);
                        DebugWriter.WriteDebug(DebugLevel.I, "Got var value: {0}", VariableValue);
                    }

                    // Check to see if the value is numeric
                    if (VariableValue is int or long)
                    {
                        if (Convert.ToInt64(VariableValue) <= int.MaxValue)
                            VariableValue = int.Parse(Convert.ToString(VariableValue));
                        else if (Convert.ToInt64(VariableValue) <= long.MaxValue)
                            VariableValue = long.Parse(Convert.ToString(VariableValue));
                        DebugWriter.WriteDebug(DebugLevel.I, "Made necessary conversion for value: {0} [{1}]", VariableValue, VariableValue.GetType());
                    }

                    // Now, add the key to the options object
                    DebugWriter.WriteDebug(DebugLevel.I, "Adding {0} to final options object", VariableKeyName);
                    ConfigSectionOptionsObject.Add(VariableKeyName, VariableValue != null ? JToken.FromObject(VariableValue) : null);
                }

                // Now, add the key to the options object
                DebugWriter.WriteDebug(DebugLevel.I, "Adding {0} to final object", Section.Name);
                ConfigObject.Add(Section.Name, ConfigSectionOptionsObject);
            }

            return ConfigObject;
        }

        /// <summary>
        /// Creates the kernel configuration file
        /// </summary>
        public static void CreateConfig() =>
            CreateConfig(Paths.GetKernelPath(KernelPathType.Configuration));

        /// <summary>
        /// Creates the kernel configuration file with custom path
        /// </summary>
        public static void CreateConfig(string ConfigPath)
        {
            if (Flags.SafeMode)
                return;

            Filesystem.ThrowOnInvalidPath(ConfigPath);
            object ConfigurationObject = GetNewConfigObject();

            // Save Config
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(ConfigurationObject, Formatting.Indented));
            EventsManager.FireEvent(EventType.ConfigSaved);
        }

        /// <summary>
        /// Creates the kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryCreateConfig() =>
            TryCreateConfig(Paths.GetKernelPath(KernelPathType.Configuration));

        /// <summary>
        /// Creates the kernel configuration file with custom path
        /// </summary>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryCreateConfig(string ConfigPath) =>
            TryCreateConfig(JObject.Parse(File.ReadAllText(ConfigPath)));

        /// <summary>
        /// Creates the kernel configuration file with custom path
        /// </summary>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryCreateConfig(JToken ConfigToken)
        {
            try
            {
                CreateConfig((string)ConfigToken);
                return true;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ConfigSaveError, ex);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Configures the kernel according to the kernel failsafe configuration
        /// </summary>
        public static void ReadFailsafeConfig() =>
            ReadConfig(PristineConfigToken, true);

        /// <summary>
        /// Configures the kernel according to the kernel configuration file
        /// </summary>
        public static void ReadConfig() =>
            ReadConfig(Paths.GetKernelPath(KernelPathType.Configuration));

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration file
        /// </summary>
        public static void ReadConfig(string ConfigPath)
        {
            Filesystem.ThrowOnInvalidPath(ConfigPath);
            ReadConfig(JObject.Parse(File.ReadAllText(ConfigPath)));
        }

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration file (new)
        /// </summary>
        public static void ReadConfig(JToken ConfigToken, bool Force = false)
        {
            if (Flags.SafeMode & !Force)
                return;

            // Load config token
            Config.ConfigToken = (JObject)ConfigToken;
            DebugWriter.WriteDebug(DebugLevel.I, "Config loaded with {0} sections", ConfigToken.Count());

            // Parse config metadata
            JToken ConfigMetadata = JToken.Parse(Properties.Resources.Resources.SettingsEntries);
            JToken ScreensaverConfigMetadata = JToken.Parse(Properties.Resources.Resources.ScreensaverSettingsEntries);
            JToken SplashConfigMetadata = JToken.Parse(Properties.Resources.Resources.SplashSettingsEntries);
            JToken[] Metadatas = new[] { ConfigMetadata, ScreensaverConfigMetadata, SplashConfigMetadata };

            // Load configuration values
            foreach (JToken metadata in Metadatas)
            {
                // Get the max sections
                int MaxSections = metadata.Count();
                DebugWriter.WriteDebug(DebugLevel.I, "Max sections from metadata: {0}", MaxSections);
                for (int SectionIndex = 0; SectionIndex <= MaxSections - 1; SectionIndex++)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Section index: {0}", SectionIndex);

                    // Get the section property and fetch metadata information from section
                    JProperty Section = (JProperty)metadata.ToList()[SectionIndex];
                    DebugWriter.WriteDebug(DebugLevel.I, "Section name: {0}", Section.Name);
                    var SectionTokenGeneral = metadata[Section.Name];
                    var SectionToken = SectionTokenGeneral["Keys"];

                    // Get config token from path
                    var SectionTokenPath = SectionTokenGeneral["Path"];
                    var ConfigTokenFromPath = ConfigToken.SelectToken((string)SectionTokenPath);

                    // Count the options
                    int MaxOptions = SectionToken.Count();
                    bool repairRequired = false;
                    DebugWriter.WriteDebug(DebugLevel.I, "Number of options: {0}", MaxOptions);
                    for (int OptionIndex = 0; OptionIndex <= MaxOptions - 1; OptionIndex++)
                    {
                        // Get the setting token and fetch information
                        DebugWriter.WriteDebug(DebugLevel.I, "Option index: {0}", OptionIndex);
                        var Setting = SectionToken[OptionIndex];
                        string VariableKeyName = (string)Setting["Name"];
                        string Variable = (string)Setting["Variable"];
                        bool VariableIsInternal = (bool)(Setting["IsInternal"] ?? false);
                        bool VariableIsEnumerable = (bool)(Setting["IsEnumerable"] ?? false);
                        int VariableEnumerableIndex = (int)(Setting["EnumerableIndex"] ?? 0);
                        DebugWriter.WriteDebug(DebugLevel.I, "Variable key name: {0} [reflecting {1}] with int: {2}, enum: {3}, enumidx: {4}", VariableKeyName, Variable, VariableIsInternal, VariableIsEnumerable, VariableEnumerableIndex);

                        // Get variable value and type
                        SettingsKeyType VariableType = (SettingsKeyType)Convert.ToInt32(Enum.Parse(typeof(SettingsKeyType), (string)Setting["Type"]));
                        DebugWriter.WriteDebug(DebugLevel.I, "Got variable type: {0}", VariableType);
                        object VariableValue = null;

                        // Check the value if we're dealing with an enumerable
                        if (FieldManager.CheckField(Variable, VariableIsInternal) && VariableIsEnumerable)
                            VariableValue = FieldManager.GetValueFromEnumerable(Variable, VariableEnumerableIndex, VariableIsInternal);

                        // Check the variable type, and deal with it as appropriate
                        if (VariableType == SettingsKeyType.SColor)
                        {
                            // Get the plain sequence from the color
                            if (VariableValue is KeyValuePair<KernelColorType, Color> color)
                                VariableValue = color.Value.PlainSequence;
                            else
                                VariableValue = new Color(((string)ConfigTokenFromPath[VariableKeyName]).ReleaseDoubleQuotes());

                            // Setting entry is color, but the variable could be either String or Color.
                            if ((FieldManager.CheckField(Variable, VariableIsInternal) && FieldManager.GetField(Variable, VariableIsInternal).FieldType == typeof(string)) ||
                                (PropertyManager.CheckProperty(Variable) && PropertyManager.GetProperty(Variable).PropertyType == typeof(string)))
                            {
                                // We're dealing with the field or the property which takes color but is a string containing plain sequence
                                VariableValue = ((Color)VariableValue).PlainSequence;
                            }
                            DebugWriter.WriteDebug(DebugLevel.I, "Got color var value: {0}", VariableValue);
                        }
                        else if (VariableType == SettingsKeyType.SSelection)
                        {
                            bool SelectionEnum = (bool)(Setting["IsEnumeration"] ?? false);
                            string SelectionEnumAssembly = (string)Setting["EnumerationAssembly"];
                            bool SelectionEnumInternal = (bool)(Setting["EnumerationInternal"] ?? false);
                            if (SelectionEnum)
                            {
                                if (SelectionEnumInternal)
                                {
                                    // Apparently, we need to have a full assembly name for getting types.
                                    Type enumType = Type.GetType("KS." + Setting["Enumeration"].ToString() + ", " + Assembly.GetExecutingAssembly().FullName);
                                    VariableValue = Enum.Parse(enumType, ((string)ConfigTokenFromPath[VariableKeyName]).ReleaseDoubleQuotes());
                                }
                                else
                                {
                                    Type enumType = Type.GetType(Setting["Enumeration"].ToString() + ", " + SelectionEnumAssembly);
                                    VariableValue = Enum.Parse(enumType, ((string)ConfigTokenFromPath[VariableKeyName]).ReleaseDoubleQuotes());
                                }
                            }
                            else
                            {
                                VariableValue = ConfigTokenFromPath[VariableKeyName].ToObject<dynamic>();
                            }
                            DebugWriter.WriteDebug(DebugLevel.I, "Got var value: {0}", VariableValue);
                        }
                        else if (VariableType == SettingsKeyType.SPreset)
                        {
                            if (VariableValue is KeyValuePair<string, PromptPresetBase> preset)
                            {
                                // Set the preset and bail
                                PromptPresetManager.SetPreset((string)ConfigTokenFromPath[VariableKeyName], preset.Key);
                                continue;
                            }
                        }
                        else if (VariableType == SettingsKeyType.SLang)
                        {
                            // Set the language
                            string lang = (string)ConfigTokenFromPath[VariableKeyName]["ThreeLetterLanguageName"];
                            LanguageManager.SetLang(lang);
                            continue;
                        }
                        else if (ConfigTokenFromPath is not null && 
                                 ConfigTokenFromPath[VariableKeyName] is not null)
                        {
                            VariableValue = ConfigTokenFromPath[VariableKeyName].ToObject<dynamic>();
                            DebugWriter.WriteDebug(DebugLevel.I, "Got var value: {0}", VariableValue);
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Might be a new config entry: [{0}] {1}", Variable, VariableKeyName);
                            DebugWriter.WriteDebug(DebugLevel.W, "Setting dirty config flag...");
                            repairRequired = true;
                            continue;
                        }

                        // Check to see if the value is numeric
                        if (VariableValue is int or long)
                        {
                            if (Convert.ToInt64(VariableValue) <= int.MaxValue)
                                VariableValue = int.Parse(Convert.ToString(VariableValue));
                            else if (Convert.ToInt64(VariableValue) <= long.MaxValue)
                                VariableValue = long.Parse(Convert.ToString(VariableValue));
                            DebugWriter.WriteDebug(DebugLevel.I, "Made necessary conversion for value: {0} [{1}]", VariableValue, VariableValue.GetType());
                        }

                        // Now, set the value
                        if (FieldManager.CheckField(Variable))
                        {
                            // We're dealing with the field
                            DebugWriter.WriteDebug(DebugLevel.I, "Setting variable {0}...", Variable);
                            FieldManager.SetValue(Variable, VariableValue, true);
                        }
                        else if (PropertyManager.CheckProperty(Variable))
                        {
                            // We're dealing with the property
                            DebugWriter.WriteDebug(DebugLevel.I, "Setting property {0}...", Variable);
                            PropertyManager.SetPropertyValue(Variable, VariableValue);
                        }
                    }

                    // If the config needs repair, just fix it!
                    if (repairRequired)
                        ConfigTools.RepairConfig();
                }
            }
        }

        /// <summary>
        /// Configures the kernel according to the kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryReadConfig() => TryReadConfig(Paths.GetKernelPath(KernelPathType.Configuration));

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryReadConfig(string ConfigPath)
        {
            Filesystem.ThrowOnInvalidPath(ConfigPath);
            return TryReadConfig(JObject.Parse(File.ReadAllText(ConfigPath)));
        }

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryReadConfig(JToken ConfigToken)
        {
            try
            {
                ReadConfig(ConfigToken);
                return true;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ConfigReadError, ex);
                DebugWriter.WriteDebugStackTrace(ex);
                if (!SplashReport.KernelBooted)
                {
                    NotificationManager.NotifySend(new Notification(Translate.DoTranslation("Error loading settings"), Translate.DoTranslation("There is an error while loading settings. You may need to check the settings file."), NotificationManager.NotifPriority.Medium, NotificationManager.NotifType.Normal));
                }
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to read config: {0}", ex.Message);
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("There is an error trying to read configuration: {0}."), ex, ex.Message);
            }
        }

        /// <summary>
        /// Main loader for configuration file
        /// </summary>
        public static void InitializeConfig()
        {
            // Make a config file if not found
            if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.Configuration)))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "No config file found. Creating...");
                CreateConfig();
            }

            // Load and read config
            try
            {
                TryReadConfig();
            }
            catch (KernelException cex) when (cex.ExceptionType == KernelExceptionType.Config)
            {
                TextWriterColor.Write(cex.Message, true, KernelColorType.Error);
                DebugWriter.WriteDebugStackTrace(cex);
                TextWriterColor.Write(Translate.DoTranslation("Trying to fix configuration..."), true, KernelColorType.Error);
                ConfigTools.RepairConfig();
            }
        }

    }
}
