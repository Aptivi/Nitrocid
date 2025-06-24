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
using System.Reflection;
using System.Text.RegularExpressions;
using Terminaux.Colors;
using Newtonsoft.Json;
using System.Linq;
using Nitrocid.Base.Files;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs.Styles;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Kernel.Configuration.Instances;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Events;
using Nitrocid.Base.Files.Paths;

namespace Nitrocid.Base.Kernel.Configuration
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
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to reload config: {0}", vars: [ex.Message]);
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

            // Check to see if we're dealing with Multivar
            if (Setting.Type == SettingsKeyType.SMultivar)
                return $"{Setting.Variables.Length} entries...";

            // This is not a multivar, so go ahead
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
                if (CurrentValue is KeyValuePair<ThemeColorType, Color> color)
                    CurrentValue = color.Value.PlainSequence;
                if (CurrentValue is Color color2)
                    CurrentValue = color2.PlainSequence;

                // Get the language name
                if (CurrentValue is LanguageInfo lang)
                    CurrentValue = lang.ThreeLetterLanguageName;

                DebugWriter.WriteDebug(DebugLevel.I, "Got current value! {0} [{1}], found under {2}...", vars: [CurrentValue, CurrentValue?.GetType().Name, Variable]);
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
                        string KeyName = LanguageTools.GetLocalized(Setting.Name) + $" [{CurrentValue}]";
                        if (Regex.IsMatch(KeyName, Pattern, RegexOptions.IgnoreCase))
                        {
                            string desc = LanguageTools.GetLocalized(Setting.Description);
                            InputChoiceInfo ici = new($"{SectionIndex + 1}/{SettingIndex + 1}", KeyName, desc);
                            DebugWriter.WriteDebug(DebugLevel.I, "Found setting {0} under section {1}, key {2}", vars: [KeyName, SectionIndex + 1, SettingIndex + 1]);
                            Results.Add(ici);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to find setting {0}: {1}", vars: [Pattern, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                throw;
            }

            // Return the results
            DebugWriter.WriteDebug(DebugLevel.I, "{0} results", vars: [Results.Count]);
            return Results;
        }

        /// <summary>
        /// Checks all the config variables to see if they can be parsed
        /// </summary>
        public static List<bool> CheckConfigVariables()
        {
            var Results = new List<bool>();
            var entries = new Dictionary<SettingsEntry[], BaseKernelConfig>();
            foreach (var config in Config.GetKernelConfigs())
                entries.Add(config.SettingsEntries ?? [], config);
            foreach (var entry in entries)
            {
                var variables = CheckConfigVariables(entry.Key, entry.Value);
                foreach (var variable in variables)
                    Results.Add(variable);
            }
            return Results;
        }

        /// <summary>
        /// Checks all the config variables to see if they can be parsed
        /// </summary>
        public static List<bool> CheckConfigVariables(string configTypeName)
        {
            if (string.IsNullOrEmpty(configTypeName))
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_NOTYPE"));
            var config = Config.GetKernelConfig(configTypeName) ??
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_NEEDSCONFIG"));
            return CheckConfigVariables(config);
        }

        /// <summary>
        /// Checks all the config variables to see if they can be parsed
        /// </summary>
        public static List<bool> CheckConfigVariables(BaseKernelConfig? entries)
        {
            if (entries is null)
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_NOBASEKERNELCONFIG"));
            return CheckConfigVariables(entries.SettingsEntries ?? [], entries);
        }

        /// <summary>
        /// Checks all the config variables to see if they can be parsed
        /// </summary>
        public static List<bool> CheckConfigVariables(SettingsEntry[]? entries, BaseKernelConfig? config)
        {
            if (config is null)
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_NOBASEKERNELCONFIG"));
            if (entries is null || entries.Length == 0)
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_NOENTRIES"));
            var Results = new List<bool>();

            // Parse all the settings
            for (int entryIdx = 0; entryIdx < entries.Length; entryIdx++)
            {
                SettingsEntry? entry = entries[entryIdx];
                var keys = entry.Keys;
                DebugWriter.WriteDebug(DebugLevel.I, "[Entry: {0}/{1}] Checking {2} settings keys on {3}...", vars: [entryIdx + 1, entries.Length, keys.Length, entry.Name]);
                Results.AddRange(CheckConfigVariables(keys, config));
            }

            // Return the results
            DebugWriter.WriteDebug(DebugLevel.I, "{0} results...", vars: [Results.Count]);
            return Results;
        }

        /// <summary>
        /// Checks all the config variables to see if they can be parsed
        /// </summary>
        public static List<bool> CheckConfigVariables(SettingsKey[]? keys, BaseKernelConfig? config)
        {
            if (config is null)
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_NOBASEKERNELCONFIG"));
            if (keys is null || keys.Length == 0)
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_NOKEYS"));
            var Results = new List<bool>();

            // Parse all the settings
            for (int keyIdx = 0; keyIdx < keys.Length; keyIdx++)
            {
                SettingsKey? key = keys[keyIdx];
                string KeyName = key.Name;
                string KeyVariable = key.Variable;
                string KeyEnumeration = key.Enumeration;
                bool KeyEnumerationInternal = key.EnumerationInternal;
                string KeyEnumerationAssembly = key.EnumerationAssembly;

                // Check for multivar
                DebugWriter.WriteDebug(DebugLevel.I, "[Key: {0}/{1}] Key name: {2}.", vars: [keyIdx + 1, keys.Length, KeyName]);
                if (key.Type == SettingsKeyType.SMultivar)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "[Key: {0}/{1}] Key is a multivar. Parsing {2} variables...", vars: [keyIdx + 1, keys.Length, key.Variables.Length]);
                    Results.AddRange(CheckConfigVariables(key.Variables, config));
                    continue;
                }

                // Check the variable
                DebugWriter.WriteDebug(DebugLevel.I, "[Key: {0}/{1}] Checking {2} for existence...", vars: [keyIdx + 1, keys.Length, KeyVariable]);
                bool KeyFound = PropertyManager.CheckProperty(KeyVariable, config.GetType());
                DebugWriter.WriteDebug(DebugLevel.I, "[Key: {0}/{1}] Result for {2}: {3}.", vars: [keyIdx + 1, keys.Length, KeyVariable, KeyFound]);
                DebugWriter.WriteDebugConditional(!KeyFound, DebugLevel.E, "[Key: {0}/{1}] {2} is not found!", vars: [keyIdx + 1, keys.Length, KeyVariable, KeyFound]);
                Results.Add(KeyFound);

                // Check the enumeration
                if (!string.IsNullOrEmpty(KeyEnumeration))
                {
                    string fullType;
                    DebugWriter.WriteDebug(DebugLevel.I, "[Key: {0}/{1}] Checking enumeration {2} used in {3} for existence...", vars: [keyIdx + 1, keys.Length, KeyEnumeration, KeyVariable]);
                    if (KeyEnumerationInternal)
                    {
                        // Apparently, we need to have a full assembly name for getting types.
                        fullType = $"{KernelReleaseInfo.rootNameSpace}.Base.{KeyEnumeration}, {typeof(Config).Assembly.FullName}";
                        DebugWriter.WriteDebug(DebugLevel.I, "[Key: {0}/{1}] Nitrocid enumeration {2} resolved to {3}.", vars: [keyIdx + 1, keys.Length, KeyEnumeration, fullType]);
                    }
                    else
                    {
                        // Add enumeration name and assembly info
                        fullType = $"{KeyEnumeration}, {KeyEnumerationAssembly}";
                        DebugWriter.WriteDebug(DebugLevel.I, "[Key: {0}/{1}] External enumeration {2} on {3} resolved to {4}.", vars: [keyIdx + 1, keys.Length, KeyEnumeration, KeyEnumerationAssembly, fullType]);
                    }

                    // Try to get the type
                    bool Result = Type.GetType(fullType) is not null;
                    DebugWriter.WriteDebug(DebugLevel.I, "[Key: {0}/{1}] Result: {2}.", vars: [keyIdx + 1, keys.Length, Result]);
                    DebugWriter.WriteDebugConditional(!Result, DebugLevel.E, "[Key: {0}/{1}] Enum {2} is not found!", vars: [keyIdx + 1, keys.Length, KeyVariable, KeyFound]);
                    Results.Add(Result);
                }
            }

            // Return the results
            DebugWriter.WriteDebug(DebugLevel.I, "{0} results...", vars: [Results.Count]);
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
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_CUSTOMSETTINGSNOCONTENT_REGISTER"));

            // Now, register!
            string name = kernelConfig.GetType().Name;
            if (!IsCustomSettingRegistered(name))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Custom settings type {0} not registered. Registering...", vars: [name]);
                Config.customConfigurations.Add(name, kernelConfig);
            }

            // Now, verify that we have a valid kernel config.
            var vars = CheckConfigVariables(kernelConfig);
            if (vars.Any((varFound) => !varFound))
            {
                Config.customConfigurations.Remove(name);
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_VALIDATIONFAILED_CUSTOM"));
            }

            // Make a configuration file
            string path = GetPathToCustomSettingsFile(kernelConfig);
            if (!FilesystemTools.FileExists(path))
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
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_CUSTOMSETTINGSNONAME_UNREGISTER"));

            // Now, register!
            if (IsCustomSettingRegistered(setting))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Custom settings type {0} registered. Unregistering...", vars: [setting]);
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
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_CUSTOMSETTINGSNONAME_UNREGISTER"));

            // Now, register!
            if (IsCustomSettingRegistered(setting))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Custom settings type {0} registered. Getting path...", vars: [setting]);
                var config = Config.GetKernelConfig(setting) ??
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_NEEDSCONFIG"));
                return GetPathToCustomSettingsFile(config);
            }
            else
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_CUSTOMSETTINGSNOTREGISTERED") + $" {setting}");
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
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_CUSTOMSETTINGSNEEDED"));

            // Now, do the job!
            string path = FilesystemTools.NeutralizePath($"{setting.GetType().Name}.json", PathsManagement.AppDataPath);
            DebugWriter.WriteDebug(DebugLevel.I, "Got path {0}...", vars: [path]);
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
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_TYPENOTFOUND"));
            var keys = GetSettingsKeys(settingsType);
            var key = keys.SingleOrDefault((sk) => sk.Variable == varName) ??
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_KEYNOTFOUNDMATCH"));
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
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_NOENTRIESJSONTEXT"));

            // Deserialize the settings entry array
            var entries = JsonConvert.DeserializeObject<SettingsEntry[]>(entriesText) ??
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_INVALIDENTRIES"));

            // Validate the contents
            foreach (var entry in entries)
            {
                // Name, description, and a non-empty key list is required.
                if (string.IsNullOrEmpty(entry.Name))
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_ENTRYNAMEEMPTY"));
                if (string.IsNullOrEmpty(entry.Desc))
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_ENTRYDESCEMPTY") + $" [{entry.Name}]");
                if (entry.Keys is null || entry.Keys.Length == 0)
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_ENTRYKEYLISTEMPTY") + $" [{entry.Name}]");

                // Validate the keys
                foreach (var key in entry.Keys)
                {
                    if (string.IsNullOrEmpty(key.Name))
                        throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_ENTRYKEYNAMEEMPTY") + $" [{entry.Name}]");
                    if (!Enum.IsDefined(key.Type))
                        throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_ENTRYKEYTYPEINVALID") + $" [{entry.Name} -> {key.Name}]");
                }
            }

            // Now, try to get the settings entry array.
            return entries;
        }

        /// <summary>
        /// Registers a base setting
        /// </summary>
        /// <param name="kernelConfig">Kernel configuration instance</param>
        internal static void RegisterBaseSetting(BaseKernelConfig kernelConfig)
        {
            // Check to see if the kernel config is null
            if (kernelConfig is null)
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_BASESETTINGSNOCONTENT_REGISTER"));

            // Now, register!
            string name = kernelConfig.GetType().Name;
            if (!IsCustomSettingBuiltin(name))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Base settings type {0} not registered. Registering...", vars: [name]);
                Config.baseConfigurations.Add(name, kernelConfig);
            }

            // Now, verify that we have a valid kernel config.
            var vars = CheckConfigVariables(kernelConfig);
            if (vars.Any((varFound) => !varFound))
            {
                Config.baseConfigurations.Remove(name);
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_VALIDATIONFAILED_BASE"));
            }

            // Make a configuration file
            string path = GetPathToCustomSettingsFile(kernelConfig);
            if (!FilesystemTools.FileExists(path))
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
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_BASESETTINGSNONAME_UNREGISTER"));

            // Now, register!
            if (IsCustomSettingBuiltin(setting))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Base settings type {0} registered. Unregistering...", vars: [setting]);
                Config.baseConfigurations.Remove(setting);
            }
        }

    }
}
