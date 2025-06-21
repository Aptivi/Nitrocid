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

extern alias TextifyDep;

using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Files;
using Nitrocid.Misc.Splash;
using Nitrocid.Misc.Notifications;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Configuration.Instances;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Kernel.Events;
using Terminaux.Colors.Themes.Colors;
using TextifyDep::Textify.Tools;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Nitrocid.Kernel.Configuration
{
    /// <summary>
    /// Configuration module
    /// </summary>
    public static class Config
    {

        internal static Dictionary<string, BaseKernelConfig> customConfigurations = [];
        internal static Dictionary<string, BaseKernelConfig> baseConfigurations = new()
        {
            { nameof(KernelMainConfig),    new KernelMainConfig() },
            { nameof(KernelSaverConfig),   new KernelSaverConfig() },
            { nameof(KernelSplashConfig),  new KernelSplashConfig() },
            { nameof(KernelDriverConfig),  new KernelDriverConfig() },
            { nameof(KernelWidgetsConfig), new KernelWidgetsConfig() },
        };

        /// <summary>
        /// Main configuration entry for the kernel
        /// </summary>
        public static KernelMainConfig MainConfig =>
            baseConfigurations is not null ? (KernelMainConfig)baseConfigurations[nameof(KernelMainConfig)] : GetFallbackKernelConfig<KernelMainConfig>();
        /// <summary>
        /// Screensaver configuration entry for the kernel
        /// </summary>
        public static KernelSaverConfig SaverConfig =>
            baseConfigurations is not null ? (KernelSaverConfig)baseConfigurations[nameof(KernelSaverConfig)] : GetFallbackKernelConfig<KernelSaverConfig>();
        /// <summary>
        /// Splash configuration entry for the kernel
        /// </summary>
        public static KernelSplashConfig SplashConfig =>
            baseConfigurations is not null ? (KernelSplashConfig)baseConfigurations[nameof(KernelSplashConfig)] : GetFallbackKernelConfig<KernelSplashConfig>();
        /// <summary>
        /// Driver configuration entry for the kernel
        /// </summary>
        public static KernelDriverConfig DriverConfig =>
            baseConfigurations is not null ? (KernelDriverConfig)baseConfigurations[nameof(KernelDriverConfig)] : GetFallbackKernelConfig<KernelDriverConfig>();
        /// <summary>
        /// Widget configuration entry for the kernel
        /// </summary>
        public static KernelWidgetsConfig WidgetConfig =>
            baseConfigurations is not null ? (KernelWidgetsConfig)baseConfigurations[nameof(KernelWidgetsConfig)] : GetFallbackKernelConfig<KernelWidgetsConfig>();

        /// <summary>
        /// Gets the kernel configuration
        /// </summary>
        /// <param name="name">Custom config type name to query</param>
        /// <returns>An instance of <see cref="BaseKernelConfig"/> if found. Otherwise, null.</returns>
        public static BaseKernelConfig? GetKernelConfig(string name)
        {
            if (ConfigTools.IsCustomSettingBuiltin(name))
                return baseConfigurations[name];
            if (ConfigTools.IsCustomSettingRegistered(name))
                return customConfigurations[name];
            return null;
        }

        /// <summary>
        /// Gets the fallback kernel configuration
        /// </summary>
        /// <param name="name">Custom config type name to query</param>
        /// <returns>An instance of <see cref="BaseKernelConfig"/> if found. Otherwise, null.</returns>
        public static BaseKernelConfig? GetFallbackKernelConfig(string name)
        {
            Type? configType = null;
            if (ConfigTools.IsCustomSettingBuiltin(name))
                configType = baseConfigurations[name].GetType();
            else if (ConfigTools.IsCustomSettingRegistered(name))
                configType = customConfigurations[name].GetType();
            if (configType is null)
                return null;
            else
                return (BaseKernelConfig?)Activator.CreateInstance(configType);
        }

        /// <summary>
        /// Gets the fallback kernel configuration
        /// </summary>
        /// <returns>An instance of <see cref="BaseKernelConfig"/> if found. Otherwise, null.</returns>
        public static TConfig GetFallbackKernelConfig<TConfig>()
            where TConfig : BaseKernelConfig =>
            Activator.CreateInstance<TConfig>();

        /// <summary>
        /// Gets the kernel configuration instances
        /// </summary>
        /// <returns>Kernel configuration instances</returns>
        public static BaseKernelConfig[] GetKernelConfigs() =>
            baseConfigurations.Values.Union(customConfigurations.Values).ToArray();

        /// <summary>
        /// Creates the kernel configuration file
        /// </summary>
        public static void CreateConfig()
        {
            var configs = GetKernelConfigs();
            foreach (var config in configs)
                CreateConfig(config);
        }

        /// <summary>
        /// Creates the kernel configuration file with custom path
        /// </summary>
        public static void CreateConfig(string ConfigFolder)
        {
            if (KernelEntry.SafeMode)
                return;

            if (!FilesystemTools.FolderExists(ConfigFolder))
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_NEEDSFOLDER"));
            DebugWriter.WriteDebug(DebugLevel.I, "Config folder {0} exists, so saving...", vars: [ConfigFolder]);

            // Save all configuration types
            var configs = GetKernelConfigs();
            foreach (var config in configs)
                CreateConfig(config, ConfigFolder + "/" + config.GetType().Name + ".json");
        }

        /// <summary>
        /// Creates the kernel configuration file with custom path and custom type
        /// </summary>
        public static void CreateConfig(BaseKernelConfig type) =>
            CreateConfig(type, ConfigTools.GetPathToCustomSettingsFile(type));

        /// <summary>
        /// Creates the kernel configuration file with custom path and custom type
        /// </summary>
        public static void CreateConfig(BaseKernelConfig type, string ConfigPath)
        {
            if (KernelEntry.SafeMode)
                return;

            // Serialize the config object
            string serialized = GetSerializedConfig(type);
            DebugWriter.WriteDebug(DebugLevel.I, "Got serialized config object of length {0}...", vars: [serialized.Length]);

            // Save Config
            FilesystemTools.WriteContentsText(ConfigPath, serialized);
            EventsManager.FireEvent(EventType.ConfigSaved);
        }

        /// <summary>
        /// Creates the kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryCreateConfig()
        {
            try
            {
                CreateConfig();
                return true;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ConfigSaveError, ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Config saving error: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Creates the kernel configuration file with custom folder
        /// </summary>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryCreateConfig(string ConfigFolder)
        {
            try
            {
                CreateConfig(ConfigFolder);
                return true;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ConfigSaveError, ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Config saving error: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Creates the kernel configuration file with custom folder
        /// </summary>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryCreateConfig(BaseKernelConfig type)
        {
            try
            {
                CreateConfig(type);
                return true;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ConfigSaveError, ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Config saving error: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Creates the kernel configuration file with custom folder
        /// </summary>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryCreateConfig(BaseKernelConfig type, string ConfigPath)
        {
            try
            {
                CreateConfig(type, ConfigPath);
                return true;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ConfigSaveError, ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Config saving error: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Configures the kernel according to the kernel configuration file
        /// </summary>
        public static void ReadConfig()
        {
            var configs = GetKernelConfigs();
            foreach (var config in configs)
                ReadConfig(config);
        }

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration type
        /// </summary>
        public static void ReadConfig(BaseKernelConfig type) =>
            ReadConfig(type, ConfigTools.GetPathToCustomSettingsFile(type));

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration type and file
        /// </summary>
        public static void ReadConfig<TConfig>(TConfig type, string ConfigPath)
        {
            // Open the config JSON file
            if (!FilesystemTools.FileExists(ConfigPath))
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_NEEDSFILE"));

            // Fix up and read!
            DebugWriter.WriteDebug(DebugLevel.I, "Config path {0} exists, so fixing and reading...", vars: [ConfigPath]);
            if (type is not BaseKernelConfig baseType)
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_INVALIDCLASS"));
            try
            {
                // First, fix the configuration file up
                RepairConfig(baseType);
                string jsonContents = FilesystemTools.ReadContentsText(ConfigPath);

                // Now, deserialize the config state.
                string typeName = type.GetType().Name;
                if (ConfigTools.IsCustomSettingBuiltin(typeName))
                    baseConfigurations[typeName] = (BaseKernelConfig?)JsonConvert.DeserializeObject(jsonContents, type.GetType()) ??
                        throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_BASECONFIGDESERIALIZE"), typeName);
                else
                    customConfigurations[typeName] = (BaseKernelConfig?)JsonConvert.DeserializeObject(jsonContents, type.GetType()) ??
                        throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_CUSTOMCONFIGDESERIALIZE"), typeName);
            }
            catch (Exception e)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Fatal error trying to parse and validate config file! {0}", vars: [e.Message]);
                DebugWriter.WriteDebugStackTrace(e);
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_INVALID"), e);
            }
        }

        /// <summary>
        /// Configures the kernel according to the kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryReadConfig()
        {
            try
            {
                ReadConfig();
                return true;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ConfigReadError, ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to read config: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                if (!SplashReport.KernelBooted)
                    NotificationManager.NotifySend(new Notification(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_NOTIFICATION_LOADERROR_TITLE"), LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_NOTIFICATION_LOADERROR_DESC"), NotificationPriority.Medium, NotificationType.Normal));
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_READFAILED"), ex, ex.Message);
            }
        }

        /// <summary>
        /// Main loader for configuration file
        /// </summary>
        public static void InitializeConfig()
        {
            // Make a config file if not found
            foreach (var baseConfig in baseConfigurations)
            {
                string finalPath = ConfigTools.GetPathToCustomSettingsFile(baseConfig.Value);
                if (!FilesystemTools.FileExists(finalPath))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "No {0} config file found. Creating at {1}...", vars: [baseConfig.Key, finalPath]);
                    CreateConfig(baseConfig.Value);
                }
            }

            // Validate config
            try
            {
                var vars = ConfigTools.CheckConfigVariables();
                if (vars.Any((varFound) => !varFound))
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_VALIDATIONFAILED"));
            }
            catch (KernelException cex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_VALIDATIONFAILED") + $" {cex.Message}", new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                });
                DebugWriter.WriteDebug(DebugLevel.E, "Config validation error! {0}", vars: [cex.Message]);
                DebugWriter.WriteDebugStackTrace(cex);
            }

            // Load and read config
            try
            {
                TryReadConfig();
            }
            catch (KernelException cex) when (cex.ExceptionType == KernelExceptionType.Config)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_READINGFAILED") + $" {cex.Message}", new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                });
                DebugWriter.WriteDebug(DebugLevel.E, "Config read error! {0}", vars: [cex.Message]);
                DebugWriter.WriteDebugStackTrace(cex);

                // Set to notify the user about config error
                ConfigTools.NotifyConfigError = true;

                // Fix anyways, for compatibility...
                InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_TRYINGTOFIX"), new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                });
                RepairConfig();
            }
        }

        private static string GetSerializedConfig(BaseKernelConfig type)
        {
            // Serialize the config object
            string serialized = JsonConvert.SerializeObject(type, Formatting.Indented);
            DebugCheck.AssertNull(serialized, "serialized config object is null!");
            DebugCheck.Assert(!string.IsNullOrEmpty(serialized), "serialized config object is empty or whitespace!");
            return serialized;
        }

        private static void RepairConfig()
        {
            var configs = GetKernelConfigs();
            foreach (var config in configs)
                RepairConfig(config);
        }

        private static void RepairConfig(BaseKernelConfig type)
        {
            // Get the current kernel config JSON file vs the serialized config JSON string
            string path = ConfigTools.GetPathToCustomSettingsFile(type);
            string serialized = GetSerializedConfig(type);
            string current = FilesystemTools.ReadContentsText(path);

            // Compare the two config JSON files
            try
            {
                var serializedObj = JObject.Parse(serialized);
                var currentObj = JObject.Parse(current);
                var diffObj = JsonTools.FindDifferences(serializedObj, currentObj);

                // Skim through the difference object
                foreach (var diff in diffObj)
                {
                    // Check to see if we have a difference token
                    if (diff.Value is null)
                        continue;

                    // Get the key and the diff type
                    string modifiedKey = diff.Key[1..];
                    string modifiedType = string.Join("", diff.Value.Select((diffToken) => ((JProperty)diffToken).Name));

                    // Now, work on how to add or remove the key to the current object, but ignore all the modifications since they're usually valid.
                    DebugCheck.Assert(modifiedType == "-" || modifiedType == "+" || modifiedType == "*", $"modified type is garbage. {modifiedType}");
                    if (modifiedType == "-")
                    {
                        // Missing key from current config. Most likely, we've added a new config entry.
                        DebugWriter.WriteDebug(DebugLevel.I, "Adding missing key: {0}", vars: [modifiedKey]);
                        var newValue = serializedObj[modifiedKey];
                        currentObj.Add(modifiedKey, newValue);
                    }
                    else if (modifiedType == "+")
                    {
                        // Extra key from current config. Most likely, we've removed a new config entry.
                        DebugWriter.WriteDebug(DebugLevel.I, "Removing extraneous key: {0}", vars: [modifiedKey]);
                        currentObj.Remove(modifiedKey);
                    }
                }

                // Save the config
                if (diffObj.HasValues)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Saving updated config...");
                    string modified = JsonConvert.SerializeObject(currentObj, Formatting.Indented);
                    FilesystemTools.WriteContentsText(path, modified);
                }
            }
            catch (Exception ex)
            {
                // OK. We're seriously screwed. Let's just write the factory default settings.
                DebugWriter.WriteDebug(DebugLevel.F, "Failed to fix configuration! {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                RepairConfigLastResort(type);
            }
        }

        private static void RepairConfigLastResort(BaseKernelConfig type)
        {
            try
            {
                // Get the factory settings
                string serialized = GetSerializedConfig(type);

                // Re-create config
                string path = ConfigTools.GetPathToCustomSettingsFile(type);
                CreateConfig(type, path);
                DebugWriter.WriteDebug(DebugLevel.F, "Last Resort: Bailed!");
            }
            catch (Exception ex)
            {
                // In this case, give up.
                DebugWriter.WriteDebug(DebugLevel.F, "Last Resort: Failed to fix configuration! {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

    }
}
