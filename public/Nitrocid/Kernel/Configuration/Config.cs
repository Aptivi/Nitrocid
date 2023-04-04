
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
using KS.Kernel.Configuration.Instances;
using Newtonsoft.Json.Schema;

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
            DebugCheck.AssertNull(serialized);
            DebugCheck.Assert(!string.IsNullOrEmpty(serialized));

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

            // Now, read the config.
            switch (type)
            {
                case ConfigType.Kernel:
                    // Validate the configuration file
                    schema = JSchema.Parse(Properties.Resources.Resources.KernelMainConfigSchema);
                    if (!configObj.IsValid(schema))
                        throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Configuration file is invalid."));

                    // Now, deserialize the config state.
                    mainConfig = (KernelMainConfig)JsonConvert.DeserializeObject(jsonContents, typeof(KernelMainConfig));
                    break;
                case ConfigType.Screensaver:
                    // Validate the configuration file
                    schema = JSchema.Parse(Properties.Resources.Resources.KernelSaverConfigSchema);
                    if (!configObj.IsValid(schema))
                        throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Configuration file is invalid."));

                    // Now, deserialize the config state.
                    saverConfig = (KernelSaverConfig)JsonConvert.DeserializeObject(jsonContents, typeof(KernelSaverConfig));
                    break;
                case ConfigType.Splash:
                    // Validate the configuration file
                    schema = JSchema.Parse(Properties.Resources.Resources.KernelSplashConfigSchema);
                    if (!configObj.IsValid(schema))
                        throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Configuration file is invalid."));

                    // Now, deserialize the config state.
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
                TextWriterColor.Write(Translate.DoTranslation("Trying to re-generate configuration..."), true, KernelColorType.Error);
                CreateConfig();
            }
        }

    }
}
