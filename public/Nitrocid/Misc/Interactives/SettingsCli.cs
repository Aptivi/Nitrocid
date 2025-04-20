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
using System.Linq;
using System.Text;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Textify.General;
using Nitrocid.Kernel.Configuration.Instances;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Kernel.Configuration.Migration;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.Misc.Interactives
{
    /// <summary>
    /// Settings interactive TUI
    /// </summary>
    public class SettingsCli : BaseInteractiveTui<(string, int)>, IInteractiveTui<(string, int)>
    {
        internal BaseKernelConfig? config;
        internal int lastFirstPaneIdx = -1;
        internal List<(string, int)> entryNames = [];
        internal List<(string, int)> keyNames = [];

        /// <summary>
        /// Always true in the file manager as we want it to behave like Total Commander
        /// </summary>
        public override bool SecondPaneInteractable =>
            true;

        /// <inheritdoc/>
        public override IEnumerable<(string, int)> PrimaryDataSource
        {
            get
            {
                try
                {
                    if (lastFirstPaneIdx == FirstPaneCurrentSelection - 1)
                        return entryNames;
                    if (config is null)
                        return entryNames;
                    var configs = config.SettingsEntries ??
                        throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't get settings entries"));
                    var configNames = configs.Select((se, idx) =>
                        (Translate.DoTranslation(!string.IsNullOrEmpty(se.DisplayAs) ? se.DisplayAs : se.Name), idx)
                    ).ToArray();
                    var entry = configs[FirstPaneCurrentSelection - 1];
                    var keys = entry.Keys;
                    var finalkeyNames = keys.Select((key, idx) =>
                    {
                        object? currentValue = ConfigTools.GetValueFromEntry(key, config);
                        return ($"{Translate.DoTranslation(key.Name)} [{currentValue}]", idx);
                    }).ToArray();
                    entryNames.Clear();
                    entryNames.AddRange(configNames);
                    keyNames.Clear();
                    keyNames.AddRange(finalkeyNames);
                    lastFirstPaneIdx = FirstPaneCurrentSelection - 1;
                    return configNames;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get settings list: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return [];
                }
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<(string, int)> SecondaryDataSource
        {
            get
            {
                try
                {
                    return keyNames;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get settings key list: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return [];
                }
            }
        }

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetStatusFromItem((string, int) item)
        {
            string entryName = item.Item1;
            int entryIdx = item.Item2;
            if (config is null)
                return "";
            var configs = config.SettingsEntries ??
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't get settings entries"));
            string entryDesc = Translate.DoTranslation(configs[entryIdx].Desc);
            string status = $"E: {entryName} - {entryDesc}";
            return status;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem((string, int) item) =>
            item.Item1;

        /// <inheritdoc/>
        public override string GetInfoFromItem((string, int) item)
        {
            string entryName = item.Item1;
            int entryIdx = item.Item2;
            if (config is null)
                return "";
            var configs = config.SettingsEntries ??
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't get settings entries"));
            string entryDesc = Translate.DoTranslation(configs[entryIdx].Desc);
            string status =
                $"""
                {entryName}
                ====================================================
                    
                {entryDesc}
                """;
            return status;
        }

        /// <inheritdoc/>
        public override string GetStatusFromItemSecondary((string, int) item)
        {
            string keyName = item.Item1;
            int keyIdx = item.Item2;
            if (config is null)
                return "";
            var configs = config.SettingsEntries ??
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't get settings entries"));
            string keyDesc = Translate.DoTranslation(configs[FirstPaneCurrentSelection - 1].Keys[keyIdx].Description);
            string status = $"K: {keyName} - {keyDesc}";
            return status;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItemSecondary((string, int) item) =>
            item.Item1;

        /// <inheritdoc/>
        public override string GetInfoFromItemSecondary((string, int) item)
        {
            string keyName = item.Item1;
            int keyIdx = item.Item2;
            if (config is null)
                return "";
            var configs = config.SettingsEntries ??
                throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't get settings entries"));
            string keyDesc = Translate.DoTranslation(configs[FirstPaneCurrentSelection - 1].Keys[keyIdx].Description);
            string status =
                $"""
                {keyName}
                ====================================================
                    
                {keyDesc}
                """;
            return status;
        }

        internal void Set(int entryIdx, int keyIdx)
        {
            try
            {
                // Check the pane first
                if (CurrentPane != 2)
                    return;
                if (config is null)
                    return;

                // Get the key and try to set
                var configs = config.SettingsEntries ??
                    throw new KernelException(KernelExceptionType.Config, Translate.DoTranslation("Can't get settings entries"));
                var key = configs[entryIdx].Keys[keyIdx];
                var defaultValue = ConfigTools.GetValueFromEntry(key, config);
                var input = key.KeyInput.PromptForSet(key, defaultValue, out bool provided);
                if (provided)
                {
                    key.KeyInput.SetValue(key, input, config);
                    lastFirstPaneIdx = -1;
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't set settings entry") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void Save()
        {
            try
            {
                // Save the config
                SettingsAppTools.SaveSettings();
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't save settings") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void SaveAs()
        {
            try
            {
                // Save the config as...
                SettingsAppTools.SaveSettingsAs();
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't save settings") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void LoadFrom()
        {
            try
            {
                // Check the config first
                if (config is null)
                    return;

                // Load the config from...
                SettingsAppTools.LoadSettingsFrom(config);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't load settings") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void Reload()
        {
            try
            {
                // Reload the config
                SettingsAppTools.ReloadConfig();
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't reload settings") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void Migrate()
        {
            try
            {
                // Migrate the config
                if (!ConfigMigration.MigrateAllConfig())
                    InfoBoxModalColor.WriteInfoBoxModalColor(Translate.DoTranslation("Configuration migration may not have been completed successfully. If you're sure that your configuration files are valid, investigate the debug logs for more info."), KernelColorTools.GetColor(KernelColorType.Error));
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't migrate settings") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void CheckUpdates()
        {
            try
            {
                // Check for system updates
                SettingsAppTools.CheckForSystemUpdates();
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't check for updates") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }

        internal void SystemInfo()
        {
            try
            {
                // Show system information
                SettingsAppTools.SystemInformation();
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't get system information") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModalColorBack(finalInfoRendered.ToString(), Settings.BoxForegroundColor, Settings.BoxBackgroundColor);
            }
        }
    }
}
