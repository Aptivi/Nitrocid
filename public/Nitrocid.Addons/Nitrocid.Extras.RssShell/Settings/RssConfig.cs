﻿//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using KS.Kernel.Configuration;
using KS.Kernel.Configuration.Instances;
using KS.Kernel.Configuration.Settings;
using KS.Shell.Prompts;
using Newtonsoft.Json;
using Nitrocid.Extras.RssShell.RSS;

namespace Nitrocid.Extras.RssShell.Settings
{
    /// <summary>
    /// Configuration instance for RSS
    /// </summary>
    public class RssConfig : BaseKernelConfig, IKernelConfig
    {
        /// <inheritdoc/>
        [JsonIgnore]
        public override SettingsEntry[] SettingsEntries =>
            ConfigTools.GetSettingsEntries(Resources.AddonResources.RssSettings);

        /// <summary>
        /// RSS Prompt Preset
        /// </summary>
        public string RSSPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell("RSSShell").PresetName;
            set => PromptPresetManager.SetPreset(value, "RSSShell", false);
        }
        /// <summary>
        /// Write how you want your RSS feed server prompt to be. Leave blank to use default style. Placeholders are parsed.
        /// </summary>
        public string RSSFeedUrlPromptStyle { get; set; } = "";
        /// <summary>
        /// Auto refresh RSS feed
        /// </summary>
        public bool RSSRefreshFeeds { get; set; } = true;
        /// <summary>
        /// How many milliseconds to refresh the RSS feed?
        /// </summary>
        public int RSSRefreshInterval
        {
            get => RSSShellCommon.refreshInterval;
            set => RSSShellCommon.refreshInterval = value < 0 ? 60000 : value;
        }
        /// <summary>
        /// How many milliseconds to wait before RSS feed fetch timeout?
        /// </summary>
        public int RSSFetchTimeout
        {
            get => RSSShellCommon.fetchTimeout;
            set => RSSShellCommon.fetchTimeout = value < 0 ? 60000 : value;
        }
    }
}