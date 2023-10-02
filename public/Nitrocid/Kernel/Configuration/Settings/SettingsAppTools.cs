
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

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel.Configuration.Instances;
using KS.Kernel.Debugging;
using KS.Languages;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return sections.ToArray();
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
                InfoBoxColor.WriteInfoBox(ex.Message, true, KernelColorType.Error);
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
                InfoBoxColor.WriteInfoBox(ex.Message, true, KernelColorType.Error);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        internal static void SaveSettingsAs()
        {
            string Location = InfoBoxColor.WriteInfoBoxInput(Translate.DoTranslation("Where do you want to save the current kernel settings?"), KernelColorType.Question);
            Location = Filesystem.NeutralizePath(Location);
            ConsoleWrapper.CursorVisible = false;
            if (!Checking.FileExists(Location))
                SaveSettings(Location);
            else
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Can't save kernel settings on top of existing file."), true, KernelColorType.Error);
        }

        internal static void LoadSettingsFrom(BaseKernelConfig config)
        {
            string Location = InfoBoxColor.WriteInfoBoxInput(Translate.DoTranslation("Where do you want to load the current kernel settings from?"), KernelColorType.Question);
            Location = Filesystem.NeutralizePath(Location);
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
                    InfoBoxColor.WriteInfoBox(ex.Message, true, KernelColorType.Error);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else
                InfoBoxColor.WriteInfoBox(Translate.DoTranslation("File not found."), true, KernelColorType.Error);
        }

        internal static void ReloadConfig()
        {
            DebugWriter.WriteDebug(DebugLevel.W, "Reloading...");
            InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Reloading settings..."), false);
            ConfigTools.ReloadConfig();
            InfoBoxColor.WriteInfoBox(Translate.DoTranslation("Configuration reloaded. You might need to reboot the kernel for some changes to take effect."));
        }

        internal static bool ValidatePlatformCompatibility(SettingsKey settings)
        {
            string[] keyUnsupportedPlatforms = settings.UnsupportedPlatforms.ToArray() ?? Array.Empty<string>();
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
    }
}
