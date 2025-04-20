﻿//
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
using System.Reflection;
using System.Text.RegularExpressions;
using Terminaux.Colors;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Files;
using Nitrocid.Misc.Reflection;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Events;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Files.Operations.Querying;
using Terminaux.Inputs.Styles;
using Nitrocid.Misc.Reflection.Internal;

namespace Nitrocid.Kernel.Configuration
{
    /// <summary>
    /// Configuration tools
    /// </summary>
    public static class ConfigTools
    {

        internal static bool NotifyConfigError;

        /// <summary>
        /// Reloads config
        /// </summary>
        public static void ReloadConfig()
        {
            EventsManager.FireEvent(EventType.PreReloadConfig);
            Config.InitializeConfig();
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
        /// <param name="Setting">An entry to get the value from</param>
        /// <param name="configType">Settings type</param>
        /// <returns>The value</returns>
        public static object? GetValueFromEntry(SettingsKey Setting, BaseKernelConfig configType)
        {
            object? CurrentValue = "Unknown";
            string Variable = Setting.Variable;
            var configTypeInstance = configType.GetType();
            string configTypeName = configTypeInstance.Name;

            // Print the option by determining how to get the current value. "as dynamic" will need to stay as we're passing the
            // configuration type instance to the generic version of the below PropertyManager function, because if we're passing
            // it as it is, we're passing the base class of it, not the one that we need.
            if (PropertyManager.CheckProperty(Variable, configTypeInstance))
            {
                if (IsCustomSettingBuiltin(configTypeName))
                    CurrentValue = PropertyManager.GetPropertyValueInstance(configType as dynamic, Variable, configTypeInstance);
                else if (IsCustomSettingRegistered(configTypeName))
                    CurrentValue = PropertyManager.GetPropertyValueInstanceExplicit(configType, Variable, configTypeInstance);

                // Get the plain sequence from the color
                if (CurrentValue is KeyValuePair<KernelColorType, Color> color)
                    CurrentValue = color.Value.PlainSequence;
                if (CurrentValue is Color color2)
                    CurrentValue = color2.PlainSequence;

                // Get the language name
                if (CurrentValue is LanguageInfo lang)
                    CurrentValue = lang.ThreeLetterLanguageName;

                DebugWriter.WriteDebug(DebugLevel.I, "Got current value! {0} [{1}], found under {2}...", CurrentValue, CurrentValue?.GetType().Name, Variable);
            }
            return CurrentValue;
        }

        /// <summary>
        /// Finds a setting with the matching pattern
        /// </summary>
        public static List<InputChoiceInfo> FindSetting(string Pattern, BaseKernelConfig configType)
        {
            var Results = new List<InputChoiceInfo>();

            // Search the settings for the given pattern
            try
            {
                var settingsEntries = configType.SettingsEntries ?? [];
                for (int SectionIndex = 0; SectionIndex <= settingsEntries.Length - 1; SectionIndex++)
                {
                    var SectionToken = settingsEntries[SectionIndex];
                    var keys = SectionToken.Keys;
                    for (int SettingIndex = 0; SettingIndex <= keys.Length - 1; SettingIndex++)
                    {
                        var Setting = keys[SettingIndex];
                        object? CurrentValue = GetValueFromEntry(Setting, configType);
                        string KeyName = Translate.DoTranslation(Setting.Name) + $" [{CurrentValue}]";
                        if (Regex.IsMatch(KeyName, Pattern, RegexOptions.IgnoreCase))
                        {
                            string desc = Translate.DoTranslation(Setting.Description);
                            InputChoiceInfo ici = new($"{SectionIndex + 1}/{SettingIndex + 1}", KeyName, desc);
                            DebugWriter.WriteDebug(DebugLevel.I, "Found setting {0} under section {1}, key {2}", KeyName, SectionIndex + 1, SettingIndex + 1);
                            Results.Add(ici);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to find setting {0}: {1}", Pattern, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                throw;
            }

            // Return the results
            DebugWriter.WriteDebug(DebugLevel.I, "{0} results", Results.Count);
            return Results;
        }

        /// <summary>
        /// Checks all the config variables to see if they can be parsed
        /// </summary>
        public static Dictionary<string, bool> CheckConfigVariables()
        {
            var Results = new Dictionary<string, bool>();
            var entries = new Dictionary<SettingsEntry[], BaseKernelConfig>();
            foreach (var config in Config.GetKernelConfigs())
                entries.Add(config.SettingsEntries ?? [], config);
            foreach (var entry in entries)
            {
                var variables = CheckConfigVariables(entry.Key, entry.Value);
                foreach (var variable in variables)
                    Results.Add(variable.Key, variable.Value);
            }
            return Results;
        }

        /// <summary>
        /// Checks all the config variables to see if they can be parsed
        /// </summary>
        public static Dictionary<string, bool> CheckConfigVariables(string configTypeName)
        {
            if (string.IsNullOrEmpty(configTypeName))
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't check configuration variables when no type specified."));
            var config = Config.GetKernelConfig(configTypeName) ??
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't check configuration variables when the kernel config is not specified."));
            return CheckConfigVariables(config);
        }

        /// <summary>
        /// Checks all the config variables to see if they can be parsed
        /// </summary>
        public static Dictionary<string, bool> CheckConfigVariables(BaseKernelConfig? entries)
        {
            if (entries is null)
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't check configuration variables when no entries are specified."));
            return CheckConfigVariables(entries.SettingsEntries ?? [], entries);
        }

        /// <summary>
        /// Checks all the config variables to see if they can be parsed
        /// </summary>
        public static Dictionary<string, bool> CheckConfigVariables(SettingsEntry[]? entries, BaseKernelConfig? config)
        {
            if (config is null)
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't check configuration variables when no entries are specified."));
            if (entries is null || entries.Length == 0)
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't check configuration variables when no entries are specified."));
            var Results = new Dictionary<string, bool>();

            // Parse all the settings
            foreach (var entry in entries)
            {
                var keys = entry.Keys;
                foreach (var key in keys)
                {
                    string KeyName = key.Name;
                    string KeyVariable = key.Variable;
                    string KeyEnumeration = key.Enumeration;
                    bool KeyEnumerationInternal = key.EnumerationInternal;
                    string KeyEnumerationAssembly = key.EnumerationAssembly;
                    bool KeyFound;

                    // Check the variable
                    DebugWriter.WriteDebug(DebugLevel.I, "Checking {0} for existence...", KeyVariable);
                    KeyFound = PropertyManager.CheckProperty(KeyVariable, config.GetType());
                    Results.Add($"{KeyName}, {KeyVariable}", KeyFound);

                    // Check the enumeration
                    if (!string.IsNullOrEmpty(KeyEnumeration))
                    {
                        bool Result;
                        if (KeyEnumerationInternal)
                        {
                            // Apparently, we need to have a full assembly name for getting types.
                            Result = Type.GetType($"{KernelMain.rootNameSpace}.{KeyEnumeration}, {Assembly.GetExecutingAssembly().FullName}") is not null;
                        }
                        else
                        {
                            Result = Type.GetType($"{KeyEnumeration}, {KeyEnumerationAssembly}") is not null;
                        }
                        Results.Add($"{KeyName}, {KeyVariable}, {KeyEnumeration}", Result);
                    }
                }
            }

            // Return the results
            DebugWriter.WriteDebug(DebugLevel.I, "{0} results...", Results.Count);
            return Results;
        }

        /// <summary>
        /// Registers a custom setting
        /// </summary>
        /// <param name="kernelConfig">Kernel configuration instance</param>
        public static void RegisterCustomSetting(BaseKernelConfig kernelConfig)
        {
            // Check to see if the kernel config is null
            if (kernelConfig is null)
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Trying to register a custom setting with no content."));

            // Now, register!
            string name = kernelConfig.GetType().Name;
            if (!IsCustomSettingRegistered(name))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Custom settings type {0} not registered. Registering...", name);
                Config.customConfigurations.Add(name, kernelConfig);
            }

            // Now, verify that we have a valid kernel config.
            var vars = CheckConfigVariables(kernelConfig);
            if (vars.Values.Any((varFound) => !varFound))
            {
                var invalidKeys = vars
                    .Where((kvp) => !kvp.Value)
                    .Select((kvp) => kvp.Key)
                    .ToArray();
                Config.customConfigurations.Remove(name);
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Trying to register a custom setting with invalid content. The invalid keys are") + ":\n\n  - " + string.Join("\n  - ", invalidKeys));
            }

            // Make a configuration file
            string path = GetPathToCustomSettingsFile(kernelConfig);
            if (!Checking.FileExists(path))
                Config.CreateConfig(kernelConfig);
            Config.ReadConfig(kernelConfig, path);
        }

        /// <summary>
        /// Unregisters a custom setting
        /// </summary>
        public static void UnregisterCustomSetting(string setting)
        {
            // Check to see if the kernel config is null
            if (string.IsNullOrEmpty(setting))
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Trying to unregister a custom setting with no name."));

            // Now, register!
            if (IsCustomSettingRegistered(setting))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Custom settings type {0} registered. Unregistering...", setting);
                Config.customConfigurations.Remove(setting);
            }
        }

        /// <summary>
        /// Checks to see whether the custom setting is registered
        /// </summary>
        /// <param name="setting">Settings type to query</param>
        /// <returns>True if found. False otherwise.</returns>
        public static bool IsCustomSettingRegistered(string setting) =>
            IsCustomSettingBuiltin(setting) || Config.customConfigurations.ContainsKey(setting);

        /// <summary>
        /// Checks to see whether the custom setting is built-in
        /// </summary>
        /// <param name="setting">Settings type to query</param>
        /// <returns>True if found. False otherwise.</returns>
        public static bool IsCustomSettingBuiltin(string setting) =>
            Config.baseConfigurations.ContainsKey(setting);

        /// <summary>
        /// Gets a path to the custom settings JSON file
        /// </summary>
        /// <param name="setting">Settings type to query</param>
        /// <returns>Path to the custom settings JSON file to read from or write to.</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetPathToCustomSettingsFile(string setting)
        {
            // Check to see if the kernel config is null
            if (string.IsNullOrEmpty(setting))
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Trying to unregister a custom setting with no name."));

            // Now, register!
            if (IsCustomSettingRegistered(setting))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Custom settings type {0} registered. Getting path...", setting);
                var config = Config.GetKernelConfig(setting) ??
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't check configuration variables when the kernel config is not specified."));
                return GetPathToCustomSettingsFile(config);
            }
            else
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("The custom setting is not registered.") + $" {setting}");
        }

        /// <summary>
        /// Gets a path to the custom settings JSON file
        /// </summary>
        /// <param name="setting">Settings type to query</param>
        /// <returns>Path to the custom settings JSON file to read from or write to.</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetPathToCustomSettingsFile(BaseKernelConfig setting)
        {
            // Sanity checks...
            if (setting is null)
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Trying to query an empty custom setting."));

            // Now, do the job!
            string path = FilesystemTools.NeutralizePath($"{setting.GetType().Name}.json", PathsManagement.AppDataPath);
            DebugWriter.WriteDebug(DebugLevel.I, "Got path {0}...", path);
            return path;
        }

        /// <summary>
        /// Gets the settings keys from the base kernel config
        /// </summary>
        /// <param name="settings">Configuration instance</param>
        /// <returns>An array of <see cref="SettingsKey"/> instances</returns>
        /// <exception cref="KernelException"></exception>
        public static SettingsKey[] GetSettingsKeys(BaseKernelConfig settings)
        {
            var entries = settings.SettingsEntries ?? [];
            List<SettingsKey> keys = [];
            foreach (var entry in entries)
                keys.AddRange(entry.Keys);
            return [.. keys];
        }

        /// <summary>
        /// Gets the settings entries
        /// </summary>
        /// <param name="settingsType">Settings type name</param>
        /// <returns>An array of <see cref="SettingsEntry"/> instances or an empty array if the specified type is not found</returns>
        /// <exception cref="KernelException"></exception>
        public static SettingsKey[] GetSettingsKeys(string settingsType)
        {
            if (!IsCustomSettingRegistered(settingsType))
                return [];
            var config = Config.GetKernelConfigs().Single((bkc) => bkc.GetType().Name == settingsType);
            return GetSettingsKeys(config);
        }

        /// <summary>
        /// Gets a settings entry from the variable name
        /// </summary>
        /// <param name="settings">Configuration instance</param>
        /// <param name="varName">Variable name to look for</param>
        /// <returns>A <see cref="SettingsKey"/> instance</returns>
        /// <exception cref="KernelException"></exception>
        public static SettingsKey GetSettingsKey(BaseKernelConfig settings, string varName) =>
            GetSettingsKey(settings.GetType().Name, varName);

        /// <summary>
        /// Gets a settings entry from the variable name
        /// </summary>
        /// <param name="settingsType">Settings type name</param>
        /// <param name="varName">Variable name to look for</param>
        /// <returns>A <see cref="SettingsEntry"/> instance</returns>
        /// <exception cref="KernelException"></exception>
        public static SettingsKey GetSettingsKey(string settingsType, string varName)
        {
            if (!IsCustomSettingRegistered(settingsType))
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Settings type not found."));
            var keys = GetSettingsKeys(settingsType);
            var key = keys.SingleOrDefault((sk) => sk.Variable == varName) ??
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Settings key not found to match the specified variable."));
            return key;
        }

        /// <summary>
        /// Gets the settings entries
        /// </summary>
        /// <param name="entriesText">Settings entries JSON contents</param>
        /// <returns>An array of <see cref="SettingsEntry"/> instances</returns>
        /// <exception cref="KernelException"></exception>
        public static SettingsEntry[] GetSettingsEntries(string entriesText)
        {
            // Some sanity checks
            if (string.IsNullOrEmpty(entriesText))
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("The settings entries JSON value is empty."));

            // Verify that the configuration is of a valid format
            string? schemaStr = ResourcesManager.GetData("SettingsSchema.json", ResourcesType.Schemas) ??
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't get base settings schema."));
            var schema = JSchema.Parse(schemaStr);
            var configObj = JArray.Parse(entriesText);
            configObj.Validate(schema);

            // Now, try to get the settings entry array.
            return JsonConvert.DeserializeObject<SettingsEntry[]>(entriesText) ??
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't get settings entries."));
        }

        /// <summary>
        /// Registers a base setting
        /// </summary>
        /// <param name="kernelConfig">Kernel configuration instance</param>
        internal static void RegisterBaseSetting(BaseKernelConfig kernelConfig)
        {
            // Check to see if the kernel config is null
            if (kernelConfig is null)
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Trying to register a base setting with no content."));

            // Now, register!
            string name = kernelConfig.GetType().Name;
            if (!IsCustomSettingBuiltin(name))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Base settings type {0} not registered. Registering...", name);
                Config.baseConfigurations.Add(name, kernelConfig);
            }

            // Now, verify that we have a valid kernel config.
            var vars = CheckConfigVariables(kernelConfig);
            if (vars.Values.Any((varFound) => !varFound))
            {
                var invalidKeys = vars
                    .Where((kvp) => !kvp.Value)
                    .Select((kvp) => kvp.Key)
                    .ToArray();
                Config.baseConfigurations.Remove(name);
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Trying to register a base setting with invalid content. The invalid keys are") + ":\n\n  - " + string.Join("\n  - ", invalidKeys));
            }

            // Make a configuration file
            string path = GetPathToCustomSettingsFile(kernelConfig);
            if (!Checking.FileExists(path))
                Config.CreateConfig(kernelConfig);
            Config.ReadConfig(kernelConfig, path);
        }

        /// <summary>
        /// Unregisters a base setting
        /// </summary>
        internal static void UnregisterBaseSetting(string setting)
        {
            // Check to see if the kernel config is null
            if (string.IsNullOrEmpty(setting))
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Trying to unregister a base setting with no name."));

            // Now, register!
            if (IsCustomSettingBuiltin(setting))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Base settings type {0} registered. Unregistering...", setting);
                Config.baseConfigurations.Remove(setting);
            }
        }

    }
}
