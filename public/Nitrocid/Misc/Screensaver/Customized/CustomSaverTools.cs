
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

using System.Collections.Generic;
using System.IO;
using KS.Files;
using KS.Files.Operations;
using KS.Kernel.Exceptions;
using KS.Languages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Misc.Screensaver.Customized
{
    /// <summary>
    /// Custom screensaver tools
    /// </summary>
    public static class CustomSaverTools
    {

        internal static Dictionary<string, CustomSaverInfo> CustomSavers = new();
        internal static JObject CustomSaverSettingsToken;

        /// <summary>
        /// Initializes and reads the custom saver settings
        /// </summary>
        public static void InitializeCustomSaverSettings()
        {
            Making.MakeFile(Paths.GetKernelPath(KernelPathType.CustomSaverSettings), false);
            string CustomSaverJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.CustomSaverSettings));
            var CustomSaverToken = JObject.Parse(!string.IsNullOrEmpty(CustomSaverJsonContent) ? CustomSaverJsonContent : "{}");
            foreach (string Saver in CustomSavers.Keys)
            {
                JObject CustomSaverSettings = CustomSaverToken[Saver] as JObject;
                if (CustomSaverSettings is not null)
                {
                    foreach (KeyValuePair<string, JToken> Setting in CustomSaverSettings)
                        CustomSavers[Saver].ScreensaverBase.ScreensaverSettings[Setting.Key] = Setting.Value.ToString();
                }
            }
            CustomSaverSettingsToken = CustomSaverToken;
        }

        /// <summary>
        /// Saves the custom saver settings
        /// </summary>
        public static void SaveCustomSaverSettings()
        {
            foreach (string Saver in CustomSavers.Keys)
            {
                if (CustomSavers[Saver].ScreensaverBase is not null)
                {
                    if (CustomSavers[Saver].ScreensaverBase.ScreensaverSettings is not null)
                    {
                        foreach (string Setting in CustomSavers[Saver].ScreensaverBase.ScreensaverSettings.Keys)
                        {
                            if (!(CustomSaverSettingsToken[Saver] as JObject).ContainsKey(Setting))
                            {
                                (CustomSaverSettingsToken[Saver] as JObject).Add(Setting, CustomSavers[Saver].ScreensaverBase.ScreensaverSettings[Setting].ToString());
                            }
                            else
                            {
                                CustomSaverSettingsToken[Saver][Setting] = CustomSavers[Saver].ScreensaverBase.ScreensaverSettings[Setting].ToString();
                            }
                        }
                    }
                }
            }
            if (CustomSaverSettingsToken is not null)
                File.WriteAllText(Paths.GetKernelPath(KernelPathType.CustomSaverSettings), JsonConvert.SerializeObject(CustomSaverSettingsToken, Formatting.Indented));
        }

        /// <summary>
        /// Adds a custom screensaver to settings
        /// </summary>
        /// <param name="CustomSaver">A custom saver</param>
        public static void AddCustomSaverToSettings(string CustomSaver)
        {
            if (!CustomSavers.ContainsKey(CustomSaver))
                throw new KernelException(KernelExceptionType.NoSuchScreensaver, Translate.DoTranslation("Screensaver {0} not found."), CustomSaver);
            if (!CustomSaverSettingsToken.ContainsKey(CustomSaver))
            {
                var NewCustomSaver = new JObject();
                if (CustomSavers[CustomSaver].ScreensaverBase is not null)
                {
                    if (CustomSavers[CustomSaver].ScreensaverBase.ScreensaverSettings is not null)
                    {
                        foreach (string Setting in CustomSavers[CustomSaver].ScreensaverBase.ScreensaverSettings.Keys)
                            NewCustomSaver.Add(Setting, CustomSavers[CustomSaver].ScreensaverBase.ScreensaverSettings[Setting].ToString());
                        CustomSaverSettingsToken.Add(CustomSaver, NewCustomSaver);
                        if (CustomSaverSettingsToken is not null)
                            File.WriteAllText(Paths.GetKernelPath(KernelPathType.CustomSaverSettings), JsonConvert.SerializeObject(CustomSaverSettingsToken, Formatting.Indented));
                    }
                }
            }
        }

        /// <summary>
        /// Removes a custom screensaver from settings
        /// </summary>
        /// <param name="CustomSaver">A custom saver</param>
        public static void RemoveCustomSaverFromSettings(string CustomSaver)
        {
            if (!CustomSavers.ContainsKey(CustomSaver))
                throw new KernelException(KernelExceptionType.NoSuchScreensaver, Translate.DoTranslation("Screensaver {0} not found."), CustomSaver);
            if (!CustomSaverSettingsToken.Remove(CustomSaver))
                throw new KernelException(KernelExceptionType.ScreensaverManagement, Translate.DoTranslation("Failed to remove screensaver {0} from config."), CustomSaver);
            if (CustomSaverSettingsToken is not null)
                File.WriteAllText(Paths.GetKernelPath(KernelPathType.CustomSaverSettings), JsonConvert.SerializeObject(CustomSaverSettingsToken, Formatting.Indented));
        }

        /// <summary>
        /// Gets custom saver settings
        /// </summary>
        /// <param name="CustomSaver">A custom saver</param>
        /// <param name="SaverSetting">A saver setting</param>
        /// <returns>Saver setting value if successful; nothing if unsuccessful.</returns>
        public static object GetCustomSaverSettings(string CustomSaver, string SaverSetting)
        {
            if (!CustomSaverSettingsToken.ContainsKey(CustomSaver))
                throw new KernelException(KernelExceptionType.NoSuchScreensaver, Translate.DoTranslation("Screensaver {0} not found."), CustomSaver);
            foreach (JProperty Setting in CustomSaverSettingsToken[CustomSaver])
            {
                if (Setting.Name == SaverSetting)
                {
                    return Setting.Value.ToObject(typeof(object));
                }
            }
            return null;
        }

        /// <summary>
        /// Sets custom saver settings
        /// </summary>
        /// <param name="CustomSaver">A custom saver</param>
        /// <param name="SaverSetting">A saver setting</param>
        /// <param name="Value">Value</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool SetCustomSaverSettings(string CustomSaver, string SaverSetting, object Value)
        {
            if (!CustomSaverSettingsToken.ContainsKey(CustomSaver))
                throw new KernelException(KernelExceptionType.NoSuchScreensaver, Translate.DoTranslation("Screensaver {0} not found."), CustomSaver);
            var SettingFound = false;
            foreach (JProperty Setting in CustomSaverSettingsToken[CustomSaver])
            {
                if (Setting.Name == SaverSetting)
                {
                    SettingFound = true;
                    CustomSaverSettingsToken[CustomSaver][SaverSetting] = Value.ToString();
                }
            }
            if (CustomSaverSettingsToken is not null)
                File.WriteAllText(Paths.GetKernelPath(KernelPathType.CustomSaverSettings), JsonConvert.SerializeObject(CustomSaverSettingsToken, Formatting.Indented));
            return SettingFound;
        }

    }
}
