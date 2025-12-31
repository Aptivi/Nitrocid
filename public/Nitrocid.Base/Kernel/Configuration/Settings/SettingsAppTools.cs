//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Base.Files;
using System;
using System.Linq;
using Terminaux.Base;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Kernel.Configuration.Instances;
using Nitrocid.Base.Kernel.Exceptions;

#if SPECIFIERREL
using Nitrocid.Base.Kernel.Updates;
#endif

namespace Nitrocid.Base.Kernel.Configuration.Settings
{
    internal static class SettingsAppTools
    {
        internal static void SaveSettings()
        {
            // Just a wrapper for CreateConfig() that SettingsApp uses
            DebugWriter.WriteDebug(DebugLevel.I, "Saving settings...");
            try
            {
                InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SAVINGSETTINGS"));
                Config.CreateConfig();
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(ex.Message, new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                });
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        internal static void SaveSettings(string location)
        {
            // Just a wrapper for CreateConfig() that SettingsApp uses
            DebugWriter.WriteDebug(DebugLevel.I, "Saving settings...");
            try
            {
                InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SAVINGTO") + $" {location}...");
                Config.CreateConfig(location);
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(ex.Message, new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                });
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        internal static void SaveSettingsAs()
        {
            string Location = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SAVEPROMPT"), new InfoBoxSettings()
            {
                ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Question)
            });
            Location = FilesystemTools.NeutralizePath(Location);
            ConsoleWrapper.CursorVisible = false;
            if (!FilesystemTools.FileExists(Location))
                SaveSettings(Location);
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_CANTOVERWRITE"), new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                });
        }

        internal static void LoadSettingsFrom(BaseKernelConfig config)
        {
            string Location = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_LOADPROMPT"), new InfoBoxSettings()
            {
                ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Question)
            });
            Location = FilesystemTools.NeutralizePath(Location);
            if (FilesystemTools.FileExists(Location))
            {
                try
                {
                    InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_LOADING"));
                    Config.ReadConfig(config, Location);
                    InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SAVINGSETTINGS"));
                    Config.CreateConfig();
                }
                catch (Exception ex)
                {
                    InfoBoxModalColor.WriteInfoBoxModal(ex.Message, new InfoBoxSettings()
                    {
                        ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                    });
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHATTR_FILENOTFOUND"), new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                });
        }

        internal static void ReloadConfig()
        {
            DebugWriter.WriteDebug(DebugLevel.W, "Reloading...");
            InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_RELOADING"));
            ConfigTools.ReloadConfig();
            InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_RELOADED"));
        }

        internal static void CheckForSystemUpdates()
        {
            // The LocaleClean analyzer-based cleaner reports false positives for extra strings that happen to be
            // translated in the compiler pre-processor directives, so we need to move all translations here to
            // avoid this happening again and for the locale tools to actually see them.
            string devVersionWarning = LanguageTools.GetLocalized("NKS_KERNEL_UPDATES_CHECKDISABLED");
            string checkFailed = LanguageTools.GetLocalized("NKS_KERNEL_UPDATES_CHECKFAILED");
            string checking = LanguageTools.GetLocalized("NKS_KERNEL_UPDATES_CHECKING");
            string newVersion = LanguageTools.GetLocalized("NKS_KERNEL_UPDATES_FOUND");
            string upToDate = LanguageTools.GetLocalized("NKS_KERNEL_UPDATES_UPTODATE");

#if SPECIFIERREL
            // Check for updates now
            InfoBoxNonModalColor.WriteInfoBox(checking);
            var AvailableUpdate = UpdateManager.FetchBinaryArchive();
            if (AvailableUpdate is not null)
            {
                if (!AvailableUpdate.Updated)
                    InfoBoxModalColor.WriteInfoBoxModal(newVersion + $"{AvailableUpdate.UpdateVersion}");
                else
                    InfoBoxModalColor.WriteInfoBoxModal(upToDate);
            }
            else if (AvailableUpdate is null)
                InfoBoxModalColor.WriteInfoBoxModal(checkFailed);
#else
            InfoBoxModalColor.WriteInfoBoxModal(devVersionWarning);
#endif
        }

        internal static void SystemInformation()
        {
            InfoBoxModalColor.WriteInfoBoxModal(
                $"{LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_KERNELVERSION")}: {KernelReleaseInfo.VersionFullStr}\n" +
                $"{LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_KERNELAPI")}: {KernelReleaseInfo.ApiVersion}\n" +
                $"{LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_HOSTID")}: {KernelPlatform.GetCurrentRid()}\n" +
                $"{LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_HOSTGENERICID")}: {KernelPlatform.GetCurrentGenericRid()}"
            );
        }

        internal static BaseKernelConfig? SelectConfig()
        {
            var configs = Config.GetKernelConfigs();
            var choices = configs.Select((bkc, idx) => new InputChoiceInfo(bkc.GetType().Name, bkc.Name)).ToArray();

            // Prompt user to provide the base kernel config
            int selected = InfoBoxSelectionColor.WriteInfoBoxSelection(choices, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SELECTCONFIG_SELECT"));

            // Check the index
            if (selected == -1)
                return null;
            else
            {
                // Check for settings entries
                var selectedConfig = configs[selected];
                SettingsEntry[]? settingsEntries = selectedConfig.SettingsEntries;
                if (settingsEntries is null || settingsEntries.Length == 0)
                {
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_ENTRIESNOTFOUND"));
                    return null;
                }
                return configs[selected];
            }
        }

        internal static bool IsUnsupported(SettingsKey settings)
        {
            string[] keyUnsupportedPlatforms = settings.UnsupportedPlatforms.ToArray() ?? [];
            bool platformUnsupported = false;
            foreach (string platform in keyUnsupportedPlatforms)
            {
                switch (platform.ToLower())
                {
                    case "windows":
                        if (KernelPlatform.IsOnWindows())
                            platformUnsupported = true;
                        break;
                    case "unix":
                        if (KernelPlatform.IsOnUnix())
                            platformUnsupported = true;
                        break;
                    case "macos":
                        if (KernelPlatform.IsOnMacOS())
                            platformUnsupported = true;
                        break;
                }
            }
            return platformUnsupported;
        }

        internal static void SetPropertyValue(string KeyVar, object? Value, BaseKernelConfig configType)
        {
            // Consult a comment in ConfigTools about "as dynamic" for more info.
            var configTypeInstance = configType.GetType();
            string configTypeName = configTypeInstance.Name;

            if (PropertyManager.CheckProperty(KeyVar, configTypeInstance))
            {
                if (ConfigTools.IsCustomSettingBuiltin(configTypeName))
                    PropertyManager.SetPropertyValueInstance(configType as dynamic, KeyVar, Value, configTypeInstance);
                else if (ConfigTools.IsCustomSettingRegistered(configTypeName))
                    PropertyManager.SetPropertyValueInstanceExplicit(configType, KeyVar, Value, configTypeInstance);
            }
        }

        internal static object? GetPropertyValue(string KeyVar, BaseKernelConfig configType)
        {
            var configTypeInstance = configType.GetType();
            string configTypeName = configTypeInstance.Name;

            if (PropertyManager.CheckProperty(KeyVar, configTypeInstance))
            {
                if (ConfigTools.IsCustomSettingBuiltin(configTypeName))
                    return PropertyManager.GetPropertyValueInstance(configType as dynamic, KeyVar, configTypeInstance);
                else if (ConfigTools.IsCustomSettingRegistered(configTypeName))
                    return PropertyManager.GetPropertyValueInstanceExplicit(configType, KeyVar, configTypeInstance);
            }
            return null;
        }

        internal static object[] ParseParameters(SettingsKey key)
        {
            // Don't do anything if we don't have arguments
            if (key.SelectionFunctionArgs is null || key.SelectionFunctionArgs.Length == 0)
                return [];

            // Check the parameters and convert them
            object[] objects = new object[key.SelectionFunctionArgs.Length];
            for (int i = 0; i < key.SelectionFunctionArgs.Length; i++)
            {
                SettingsFunctionArgs? arg = key.SelectionFunctionArgs[i];

                // Try to get the type
                Type type = Type.GetType(arg.ArgType) ??
                    throw new KernelException(KernelExceptionType.Reflection, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_EXCEPTION_ARGTYPE") + $": {arg.ArgType}");

                // Use this type to convert the string value to that type
                var converted = Convert.ChangeType(arg.ArgValue, type);
                objects[i] = converted;
            }
            return objects;
        }
    }
}
