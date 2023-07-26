
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
using System.IO;
using System.Linq;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Splash;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KS.Kernel.Events;
using KS.ConsoleBase.Colors;
using KS.Kernel.Configuration.Instances;
using Newtonsoft.Json.Schema;
using KS.ConsoleBase.Writers.ConsoleWriters;

namespace KS.Kernel.Configuration
{
    /// <summary>
    /// Configuration module
    /// </summary>
    public static class Config
    {

        internal static KernelMainConfig mainConfig = new();
        internal static KernelSaverConfig saverConfig = new();
        internal static KernelSplashConfig splashConfig = new();

        /// <summary>
        /// Main configuration entry for the kernel
        /// </summary>
        public static KernelMainConfig MainConfig { get => mainConfig; }
        /// <summary>
        /// Screensaver configuration entry for the kernel
        /// </summary>
        public static KernelSaverConfig SaverConfig { get => saverConfig; }
        /// <summary>
        /// Splash configuration entry for the kernel
        /// </summary>
        public static KernelSplashConfig SplashConfig { get => splashConfig; }

        /// <summary>
        /// Creates the kernel configuration file
        /// </summary>
        public static void CreateConfig()
        {
            CreateConfig(ConfigType.Kernel, Paths.GetKernelPath(KernelPathType.Configuration));
            CreateConfig(ConfigType.Screensaver, Paths.GetKernelPath(KernelPathType.SaverConfiguration));
            CreateConfig(ConfigType.Splash, Paths.GetKernelPath(KernelPathType.SplashConfiguration));
        }

        /// <summary>
        /// Creates the kernel configuration file with custom path
        /// </summary>
        public static void CreateConfig(string ConfigFolder)
        {
            if (Flags.SafeMode)
                return;

            Filesystem.ThrowOnInvalidPath(ConfigFolder);
            if (!Checking.FolderExists(ConfigFolder))
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Specify an existent folder to store the three configuration files on."));

            // Save all three configuration types
            CreateConfig(ConfigType.Kernel, ConfigFolder + "/" + Path.GetFileName(Paths.GetKernelPath(KernelPathType.Configuration)));
            CreateConfig(ConfigType.Screensaver, ConfigFolder + "/" + Path.GetFileName(Paths.GetKernelPath(KernelPathType.SaverConfiguration)));
            CreateConfig(ConfigType.Splash, ConfigFolder + "/" + Path.GetFileName(Paths.GetKernelPath(KernelPathType.SplashConfiguration)));
        }

        /// <summary>
        /// Creates the kernel configuration file with custom path and custom type
        /// </summary>
        public static void CreateConfig(ConfigType type, string ConfigPath)
        {
            if (Flags.SafeMode)
                return;

            Filesystem.ThrowOnInvalidPath(ConfigPath);

            // Serialize the config object
            string serialized = GetSerializedConfig(type);

            // Save Config
            File.WriteAllText(ConfigPath, serialized);
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
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Creates the kernel configuration file with custom folder
        /// </summary>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryCreateConfig(ConfigType type, string ConfigPath)
        {
            try
            {
                CreateConfig(type, ConfigPath);
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
        /// Configures the kernel according to the kernel configuration file
        /// </summary>
        public static void ReadConfig()
        {
            ReadConfig(ConfigType.Kernel, Paths.GetKernelPath(KernelPathType.Configuration));
            ReadConfig(ConfigType.Screensaver, Paths.GetKernelPath(KernelPathType.SaverConfiguration));
            ReadConfig(ConfigType.Splash, Paths.GetKernelPath(KernelPathType.SplashConfiguration));
        }

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration type
        /// </summary>
        public static void ReadConfig(ConfigType type)
        {
            switch (type)
            {
                case ConfigType.Kernel:
                    ReadConfig(type, Paths.GetKernelPath(KernelPathType.Configuration));
                    break;
                case ConfigType.Screensaver:
                    ReadConfig(type, Paths.GetKernelPath(KernelPathType.SaverConfiguration));
                    break;
                case ConfigType.Splash:
                    ReadConfig(type, Paths.GetKernelPath(KernelPathType.SplashConfiguration));
                    break;
            }
        }

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration type and file
        /// </summary>
        public static void ReadConfig(ConfigType type, string ConfigPath)
        {
            // Open the config JSON file
            Filesystem.ThrowOnInvalidPath(ConfigPath);
            if (!Checking.FileExists(ConfigPath))
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Specify an existent path to a configuration file"));
            string jsonContents = File.ReadAllText(ConfigPath);
            JObject configObj = JObject.Parse(jsonContents);
            JSchema schema;

            // Validate the configuration file
            try
            {
                switch (type)
                {
                    case ConfigType.Kernel:
                        schema = JSchema.Parse(Properties.Resources.Resources.KernelMainConfigSchema);
                        configObj.Validate(schema);
                        break;
                    case ConfigType.Screensaver:
                        // Validate the configuration file
                        schema = JSchema.Parse(Properties.Resources.Resources.KernelSaverConfigSchema);
                        configObj.Validate(schema);
                        break;
                    case ConfigType.Splash:
                        // Validate the configuration file
                        schema = JSchema.Parse(Properties.Resources.Resources.KernelSplashConfigSchema);
                        configObj.Validate(schema);
                        break;
                }
            }
            catch (Exception e)
            {
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Configuration file is invalid."), e);
            }

            // Now, deserialize the config state.
            switch (type)
            {
                case ConfigType.Kernel:
                    RepairConfig(ConfigType.Kernel);
                    mainConfig = (KernelMainConfig)JsonConvert.DeserializeObject(jsonContents, typeof(KernelMainConfig));
                    break;
                case ConfigType.Screensaver:
                    RepairConfig(ConfigType.Screensaver);
                    saverConfig = (KernelSaverConfig)JsonConvert.DeserializeObject(jsonContents, typeof(KernelSaverConfig));
                    break;
                case ConfigType.Splash:
                    RepairConfig(ConfigType.Splash);
                    splashConfig = (KernelSplashConfig)JsonConvert.DeserializeObject(jsonContents, typeof(KernelSplashConfig));
                    break;
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
                DebugWriter.WriteDebugStackTrace(ex);
                if (!SplashReport.KernelBooted)
                    NotificationManager.NotifySend(new Notification(Translate.DoTranslation("Error loading settings"), Translate.DoTranslation("There is an error while loading settings. You may need to check the settings file."), NotificationManager.NotifPriority.Medium, NotificationManager.NotifType.Normal));
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to read config: {0}", ex.Message);
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("There is an error trying to read configuration: {0}."), ex, ex.Message);
            }
        }

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryReadConfig(ConfigType type)
        {
            try
            {
                ReadConfig(type);
                return true;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ConfigReadError, ex);
                DebugWriter.WriteDebugStackTrace(ex);
                if (!SplashReport.KernelBooted)
                    NotificationManager.NotifySend(new Notification(Translate.DoTranslation("Error loading settings"), Translate.DoTranslation("There is an error while loading settings. You may need to check the settings file."), NotificationManager.NotifPriority.Medium, NotificationManager.NotifType.Normal));
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to read config: {0}", ex.Message);
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("There is an error trying to read configuration: {0}."), ex, ex.Message);
            }
        }

        /// <summary>
        /// Configures the kernel according to the custom kernel configuration file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool TryReadConfig(ConfigType type, string ConfigPath)
        {
            try
            {
                ReadConfig(type, ConfigPath);
                return true;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.ConfigReadError, ex);
                DebugWriter.WriteDebugStackTrace(ex);
                if (!SplashReport.KernelBooted)
                    NotificationManager.NotifySend(new Notification(Translate.DoTranslation("Error loading settings"), Translate.DoTranslation("There is an error while loading settings. You may need to check the settings file."), NotificationManager.NotifPriority.Medium, NotificationManager.NotifType.Normal));
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
                CreateConfig(ConfigType.Kernel, Paths.GetKernelPath(KernelPathType.Configuration));
            }
            if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.SaverConfiguration)))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "No saver config file found. Creating...");
                CreateConfig(ConfigType.Screensaver, Paths.GetKernelPath(KernelPathType.SaverConfiguration));
            }
            if (!Checking.FileExists(Paths.GetKernelPath(KernelPathType.SplashConfiguration)))
            {
                DebugWriter.WriteDebug(DebugLevel.E, "No splash config file found. Creating...");
                CreateConfig(ConfigType.Splash, Paths.GetKernelPath(KernelPathType.SplashConfiguration));
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
                RepairConfig();
            }
        }

        private static string GetSerializedConfig(ConfigType type)
        {
            // Serialize the config object
            string serialized = "";
            switch (type)
            {
                case ConfigType.Kernel:
                    serialized = JsonConvert.SerializeObject(mainConfig, Formatting.Indented);
                    break;
                case ConfigType.Screensaver:
                    serialized = JsonConvert.SerializeObject(saverConfig, Formatting.Indented);
                    break;
                case ConfigType.Splash:
                    serialized = JsonConvert.SerializeObject(splashConfig, Formatting.Indented);
                    break;
            }
            DebugCheck.AssertNull(serialized, "serialized config object is null!");
            DebugCheck.Assert(!string.IsNullOrEmpty(serialized), "serialized config object is empty or whitespace!");
            return serialized;
        }

        private static void RepairConfig()
        {
            RepairConfig(ConfigType.Kernel);
            RepairConfig(ConfigType.Screensaver);
            RepairConfig(ConfigType.Splash);
        }

        private static void RepairConfig(ConfigType type)
        {
            // Get the current kernel config JSON file vs the serialized config JSON string
            string serialized = GetSerializedConfig(type);
            string current = "";
            switch (type)
            {
                case ConfigType.Kernel:
                    current = File.ReadAllText(Paths.GetKernelPath(KernelPathType.Configuration));
                    break;
                case ConfigType.Screensaver:
                    current = File.ReadAllText(Paths.GetKernelPath(KernelPathType.SaverConfiguration));
                    break;
                case ConfigType.Splash:
                    current = File.ReadAllText(Paths.GetKernelPath(KernelPathType.SplashConfiguration));
                    break;
            }

            // Config difference function. Credits to https://stackoverflow.com/a/53654867
            JObject FindConfigDifferences(JToken serializedObj, JToken currentObj)
            {
                var diff = new JObject();
                if (!JToken.DeepEquals(currentObj, serializedObj))
                {
                    switch (currentObj.Type)
                    {
                        case JTokenType.Object:
                            {
                                var addedKeys = ((JObject)currentObj).Properties().Select(c => c.Name).Except(((JObject)serializedObj).Properties().Select(c => c.Name));
                                var removedKeys = ((JObject)serializedObj).Properties().Select(c => c.Name).Except(((JObject)currentObj).Properties().Select(c => c.Name));
                                var unchangedKeys = ((JObject)currentObj).Properties().Where(c => JToken.DeepEquals(c.Value, serializedObj[c.Name])).Select(c => c.Name);
                                foreach (var k in addedKeys)
                                {
                                    diff[k] = new JObject
                                    {
                                        ["+"] = currentObj[k].Path
                                    };
                                }
                                foreach (var k in removedKeys)
                                {
                                    diff[k] = new JObject
                                    {
                                        ["-"] = serializedObj[k].Path
                                    };
                                }
                            }
                            break;
                        case JTokenType.Array:
                            {
                                diff["+"] = new JArray(((JArray)currentObj).Except(serializedObj));
                                diff["-"] = new JArray(((JArray)serializedObj).Except(currentObj));
                            }
                            break;
                        default:
                            diff["+"] = currentObj;
                            diff["-"] = serializedObj;
                            break;
                    }
                }
                return diff;
            }

            // Use the above function to compare the two config JSON files
            var serializedObj = JObject.Parse(serialized);
            var currentObj = JObject.Parse(current);
            var diffObj = FindConfigDifferences(serializedObj, currentObj);

            // Skim through the difference object
            foreach (var diff in diffObj)
            {
                // Get the key and the diff type
                string modifiedKey = diff.Key;
                string modifiedType = string.Join("", diff.Value.Select((diffToken) => ((JProperty)diffToken).Name));

                // Now, work on how to add or remove the key to the current object
                DebugCheck.Assert(modifiedType == "-" || modifiedType == "+", $"modified type is garbage. {modifiedType}");
                if (modifiedType == "-")
                {
                    // Missing key from current config. Most likely, we've added a new config entry.
                    var newValue = serializedObj[modifiedKey];
                    currentObj.Add(modifiedKey, newValue);
                }
                else
                {
                    // Extra key from current config. Most likely, we've removed a new config entry.
                    currentObj.Remove(modifiedKey);
                }
            }

            // Save the config
            if (diffObj.HasValues)
            {
                string modified = JsonConvert.SerializeObject(currentObj, Formatting.Indented);
                switch (type)
                {
                    case ConfigType.Kernel:
                        File.WriteAllText(Paths.GetKernelPath(KernelPathType.Configuration), modified);
                        break;
                    case ConfigType.Screensaver:
                        File.WriteAllText(Paths.GetKernelPath(KernelPathType.SaverConfiguration), modified);
                        break;
                    case ConfigType.Splash:
                        File.WriteAllText(Paths.GetKernelPath(KernelPathType.SplashConfiguration), modified);
                        break;
                }
            }
        }

    }
}
