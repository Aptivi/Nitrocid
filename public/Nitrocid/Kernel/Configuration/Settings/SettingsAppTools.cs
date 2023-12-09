//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Inputs.Styles.Infobox;
using KS.Files;
using KS.Files.Operations.Querying;
using KS.Kernel.Configuration.Instances;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

#if SPECIFIERREL
#if !PACKAGEMANAGERBUILD
using KS.Kernel.Updates;
#endif
#endif

namespace KS.Kernel.Configuration.Settings
{
    internal static class SettingsAppTools
    {
        internal static InputChoiceInfo[] GetSectionChoices(SettingsEntry[] SettingsEntries)
        {
            // Verify that the section choices are not empty
            DebugCheck.Assert(SettingsEntries.Length > 0, "populating empty section choices makes settings app useless.");

            // Populate sections
            var sections = new List<InputChoiceInfo>();
            int MaxSections = SettingsEntries.Length;
            for (int SectionIndex = 0; SectionIndex <= MaxSections - 1; SectionIndex++)
            {
                // Get a section and check to see if the display name is empty
                SettingsEntry Section = SettingsEntries[SectionIndex];
                string displayAs =
                    !string.IsNullOrEmpty(Section.DisplayAs) ?
                    Translate.DoTranslation(Section.DisplayAs) :
                    Translate.DoTranslation(Section.Name);
                string description = Translate.DoTranslation(Section.Desc);

                // Populate the choice information
                var choice = new InputChoiceInfo(
                    $"{SectionIndex + 1}",
                    displayAs,
                    description
                );
                sections.Add(choice);
            }
            return [.. sections];
        }

        internal static void SaveSettings()
        {
            // Just a wrapper for CreateConfig() that SettingsApp uses
            DebugWriter.WriteDebug(DebugLevel.I, "Saving settings...");
            try
            {
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Saving settings..."), false);
                Config.CreateConfig();
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBoxKernelColor(ex.Message, true, KernelColorType.Error);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        internal static void SaveSettings(string location)
        {
            // Just a wrapper for CreateConfig() that SettingsApp uses
            DebugWriter.WriteDebug(DebugLevel.I, "Saving settings...");
            try
            {
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Saving settings to") + $" {location}...", false);
                Config.CreateConfig(location);
            }
            catch (Exception ex)
            {
                InfoBoxColor.WriteInfoBoxKernelColor(ex.Message, true, KernelColorType.Error);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        internal static void SaveSettingsAs()
        {
            string Location = InfoBoxInputColor.WriteInfoBoxInputKernelColor(Translate.DoTranslation("Where do you want to save the current kernel settings?"), KernelColorType.Question);
            Location = FilesystemTools.NeutralizePath(Location);
            ConsoleWrapper.CursorVisible = false;
            if (!Checking.FileExists(Location))
                SaveSettings(Location);
            else
                InfoBoxColor.WriteInfoBoxKernelColor(Translate.DoTranslation("Can't save kernel settings on top of existing file."), true, KernelColorType.Error);
        }

        internal static void LoadSettingsFrom(BaseKernelConfig config)
        {
            string Location = InfoBoxInputColor.WriteInfoBoxInputKernelColor(Translate.DoTranslation("Where do you want to load the current kernel settings from?"), KernelColorType.Question);
            Location = FilesystemTools.NeutralizePath(Location);
            if (Checking.FileExists(Location))
            {
                try
                {
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Loading settings..."), false);
                    Config.ReadConfig(config, Location);
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Saving settings..."), false);
                    Config.CreateConfig();
                }
                catch (Exception ex)
                {
                    InfoBoxColor.WriteInfoBoxKernelColor(ex.Message, true, KernelColorType.Error);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else
                InfoBoxColor.WriteInfoBoxKernelColor(Translate.DoTranslation("File not found."), true, KernelColorType.Error);
        }

        internal static void ReloadConfig()
        {
            DebugWriter.WriteDebug(DebugLevel.W, "Reloading...");
            InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Reloading settings..."), false);
            ConfigTools.ReloadConfig();
            InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Configuration reloaded. You might need to reboot the kernel for some changes to take effect."));
        }

        internal static void CheckForSystemUpdates()
        {
#if SPECIFIERREL && !PACKAGEMANAGERBUILD
            // Check for updates now
            InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Checking for system updates..."), false);
            var AvailableUpdate = UpdateManager.FetchBinaryArchive();
            if (AvailableUpdate is not null)
            {
                if (!AvailableUpdate.Updated)
                {
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Found new version: ") + $"{AvailableUpdate.UpdateVersion}");
                }
                else
                {
                    InfoBoxColor.WriteInfoBox(Translate.DoTranslation("You're up to date!"));
                }
            }
            else if (AvailableUpdate is null)
            {
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Failed to check for updates."));
            }
#elif PACKAGEMANAGERBUILD
            InfoBoxColor.WriteInfoBox(Translate.DoTranslation("You've installed Nitrocid KS using your package manager. Please use it to upgrade your kernel instead."));
#else
            InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Checking for updates is disabled on development versions."));
#endif
        }

        internal static void SystemInformation()
        {
            InfoBoxColor.WriteInfoBox(
                $"{Translate.DoTranslation("Kernel version")}: {KernelMain.VersionFullStr}\n" +
                $"{Translate.DoTranslation("Kernel API version")}: {KernelMain.ApiVersion}\n" +
                $"{Translate.DoTranslation("Host ID")}: {KernelPlatform.GetCurrentRid()}\n" +
                $"{Translate.DoTranslation("Host Generic ID")}: {KernelPlatform.GetCurrentGenericRid()}"
            );
        }

        internal static bool ValidatePlatformCompatibility(SettingsKey settings)
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

        internal static void SetPropertyValue(string KeyVar, object Value, BaseKernelConfig configType)
        {
            // Consult a comment in ConfigTools about "as dynamic" for more info.
            var configTypeInstance = configType.GetType();
            string configTypeName = configTypeInstance.Name;

            if (ConfigTools.IsCustomSettingBuiltin(configTypeName) && PropertyManager.CheckProperty(KeyVar))
                PropertyManager.SetPropertyValueInstance(configType as dynamic, KeyVar, Value);
            else if (ConfigTools.IsCustomSettingRegistered(configTypeName) && PropertyManager.CheckProperty(KeyVar, configTypeInstance))
                PropertyManager.SetPropertyValueInstanceExplicit(configType, KeyVar, Value, configTypeInstance);
        }

        internal static object GetPropertyValue(string KeyVar, BaseKernelConfig configType)
        {
            var configTypeInstance = configType.GetType();
            string configTypeName = configTypeInstance.Name;

            if (ConfigTools.IsCustomSettingBuiltin(configTypeName) && PropertyManager.CheckProperty(KeyVar))
                return PropertyManager.GetPropertyValueInstance(configType as dynamic, KeyVar);
            else if (ConfigTools.IsCustomSettingRegistered(configTypeName) && PropertyManager.CheckProperty(KeyVar, configTypeInstance))
                return PropertyManager.GetPropertyValueInstanceExplicit(configType, KeyVar, configTypeInstance);
            return null;
        }

        internal static void HandleError(string message, Exception ex = null)
        {
            if (ex is null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Error trying to open section.");
                string finalSection = Translate.DoTranslation("You're Lost!");
                InfoBoxColor.WriteInfoBoxKernelColor(
                    $"  * {finalSection}\n\n" +
                    $"{message}\n\n" +
                    $"{Translate.DoTranslation("If you're sure that you've opened the right section, turn on the kernel debugger, reproduce, and try to investigate the logs.")}",
                    KernelColorType.Error
                );
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Error trying to open section: {0}", ex.Message);
                string finalSection = Translate.DoTranslation("You're Lost!");
                InfoBoxColor.WriteInfoBoxKernelColor(
                    $"  * {finalSection}\n\n" +
                    $"{message}\n\n" +
                    $"{Translate.DoTranslation("If you're sure that you've opened the right section, check this message out:")}\n" +
                    $"  - {ex.Message}\n\n" +
                    $"{Translate.DoTranslation("If you don't understand the above message, turn on the kernel debugger, reproduce, and try to investigate the logs.")}",
                    KernelColorType.Error
                );
            }
        }
    }
}
