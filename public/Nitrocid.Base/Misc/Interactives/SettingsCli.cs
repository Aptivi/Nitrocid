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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Textify.General;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Configuration.Instances;
using Nitrocid.Base.Kernel.Configuration.Migration;
using Nitrocid.Base.Kernel.Exceptions;
using Magico.Enumeration;

namespace Nitrocid.Base.Misc.Interactives
{
    /// <summary>
    /// Settings interactive TUI
    /// </summary>
    public class SettingsCli : BaseInteractiveTui<(string, int), (string, string)>, IInteractiveTui<(string, int), (string, string)>
    {
        internal BaseKernelConfig? config;
        internal int lastFirstPaneIdx = -1;
        internal bool legacyMultivarProcessing = false;
        internal List<(string, int)> entryNames = [];
        internal List<(string, string)> keyNames = [];
        internal List<(string, int)> toExpand = [];

        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                HelpTitle = /* Localizable */ "NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_HELP01_BODY") + "\n\n" +
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/advanced-and-power-users/inner-workings/kernel-settings",
            }
        ];

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
                        throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_SETTINGSENTRIES"));
                    int finalIdx = FirstPaneCurrentSelection - 1 < configs.Length ? FirstPaneCurrentSelection - 1 : 0;
                    var configNames = configs.Select((se, idx) =>
                        (!string.IsNullOrEmpty(se.DisplayAs) ? LanguageTools.GetLocalized(se.DisplayAs) : se.Name, idx)
                    ).ToArray();
                    entryNames.Clear();
                    entryNames.AddRange(configNames);
                    lastFirstPaneIdx = finalIdx;
                    return configNames;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get settings list: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return [];
                }
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<(string, string)> SecondaryDataSource
        {
            get
            {
                try
                {
                    if (config is null)
                        return keyNames;
                    var configs = config.SettingsEntries ??
                        throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_SETTINGSENTRIES"));
                    int entryIdx = FirstPaneCurrentSelection - 1;
                    int finalIdx = entryIdx < configs.Length ? entryIdx : 0;
                    var entry = configs[finalIdx];

                    // Flatten settings keys, in case we need to process expansion
                    var keys = FlattenSettingsKeys(entryIdx, entry.Keys);
                    var finalkeyNames = keys.Select((kvp) =>
                    {
                        var key = kvp.Value;
                        object? currentValue = key.Masked ? "***" : ConfigTools.GetValueFromEntry(key, config);
                        string finalKeyName = key.Type == SettingsKeyType.SMultivar ? $"[{(toExpand.Contains((kvp.Key, entryIdx)) ? "-" : "+")}] {LanguageTools.GetLocalized(key.Name)}" : $"[ ] {LanguageTools.GetLocalized(key.Name)} [{currentValue}]";
                        return (finalKeyName, kvp.Key);
                    }).ToArray();
                    keyNames.Clear();
                    keyNames.AddRange(finalkeyNames);
                    return keyNames;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get settings key list: {0}", vars: [ex.Message]);
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
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_SETTINGSENTRIES"));
            string entryDesc = LanguageTools.GetLocalized(configs[entryIdx].Desc);
            string status = $"{entryName} - {entryDesc}";
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
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_SETTINGSENTRIES"));
            string entryDesc = LanguageTools.GetLocalized(configs[entryIdx].Desc);
            string status =
                $"""
                {LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ENTRYNAME").FormatString(entryName)}
                {LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_DESCRIPTION")} {entryDesc}
                """;
            return status;
        }

        /// <inheritdoc/>
        public override string GetStatusFromItemSecondary((string, string) item)
        {
            int entryIdx = FirstPaneCurrentSelection - 1;
            string keyIdx = item.Item2;
            if (config is null)
                return "";
            var configs = config.SettingsEntries ??
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_SETTINGSENTRIES"));
            var keys = FlattenSettingsKeys(entryIdx, configs[entryIdx].Keys);
            var key = keys[keyIdx];
            string entryName = entryNames[entryIdx].Item1;
            string keyName = LanguageTools.GetLocalized(key.Name);
            string keyDesc = LanguageTools.GetLocalized(key.Description);
            string status = $"{entryName} > {keyName} - {keyDesc}";
            return status;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItemSecondary((string, string) item)
        {
            int level = item.Item2.Split('|').Length - 1;
            return $"{new string(' ', level * 2)}{item.Item1}";
        }

        /// <inheritdoc/>
        public override string GetInfoFromItemSecondary((string, string) item)
        {
            int entryIdx = FirstPaneCurrentSelection - 1;
            string keyName = item.Item1;
            string keyIdx = item.Item2;
            if (config is null)
                return "";
            var configs = config.SettingsEntries ??
                throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_SETTINGSENTRIES"));
            var keys = FlattenSettingsKeys(entryIdx, configs[entryIdx].Keys);
            string entryName = entryNames[entryIdx].Item1;
            string keyDesc = LanguageTools.GetLocalized(keys[keyIdx].Description);
            string status =
                $"""
                {LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ENTRYNAME").FormatString(entryName)} > {keyName}
                {LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_DESCRIPTION")} {keyDesc}
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

                // Get the key
                var configs = config.SettingsEntries ??
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_SETTINGSENTRIES"));
                var keyTuple = ((string, string)?)SecondaryDataSource.GetElementFromIndex(keyIdx) ??
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_SETTINGSENTRIES"));
                var keys = FlattenSettingsKeys(entryIdx, configs[entryIdx].Keys);
                var key = keys[keyTuple.Item2];

                // Check for multivar
                if (key.Type == SettingsKeyType.SMultivar && !legacyMultivarProcessing)
                {
                    // Add this multivar to the expansion list
                    if (!toExpand.Remove((keyTuple.Item2, entryIdx)))
                        toExpand.Add((keyTuple.Item2, entryIdx));
                    return;
                }

                // Try to set
                var defaultValue = ConfigTools.GetValueFromEntry(key, config);
                var input = key.KeyInput.PromptForSet(key, defaultValue, config, out bool provided);
                if (provided)
                {
                    key.KeyInput.SetValue(key, input, config);
                    lastFirstPaneIdx = -1;
                    SettingsAppTools.SaveSettings();
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_CANTSETENTRY") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
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
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_CANTSAVE") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
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
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_CANTSAVE") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
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
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_CANTLOAD") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
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
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_CANTRELOAD") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void Migrate()
        {
            try
            {
                // Migrate the config
                if (!ConfigMigration.MigrateAllConfig())
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_MIGRATEOLDCONFIG_SUCCESS"), new InfoBoxSettings()
                    {
                        ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                    });
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_CANTMIGRATE") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
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
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_CANTCHECK") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
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
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_CANTSYSINFO") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void ResetAll()
        {
            try
            {
                if (config is null)
                    return;

                // Get the config entries
                var configs = config.SettingsEntries ??
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_SETTINGSENTRIES"));
                var fallbackConfig = Config.GetFallbackKernelConfig(config.GetType().Name) ??
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_EXCEPTION_FALLBACK"));

                // Helper function
                void ResetKeys(SettingsKey[] keys)
                {
                    if (config is null)
                        return;

                    foreach (var configKey in keys)
                    {
                        if (configKey.Type == SettingsKeyType.SMultivar)
                            ResetKeys(configKey.Variables);
                        else
                        {
                            var fallbackValue = ConfigTools.GetValueFromEntry(configKey, fallbackConfig);
                            configKey.KeyInput.SetValue(configKey, fallbackValue, config);
                            lastFirstPaneIdx = -1;
                        }
                    }
                }

                // Now, reset
                foreach (var configEntry in configs)
                    ResetKeys(configEntry.Keys);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_RESETALLFAILED") + TextTools.FormatString("{0}: {1}", config?.GetType().Name ?? "<null>", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
            finally
            {
                SettingsAppTools.SaveSettings();
            }
        }

        internal void ResetEntry(int entryIdx, int keyIdx)
        {
            try
            {
                // Check the pane first
                if (CurrentPane != 2)
                    return;
                if (config is null)
                    return;

                // Get the config entries
                var configs = config.SettingsEntries ??
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_SETTINGSENTRIES"));
                var fallbackConfig = Config.GetFallbackKernelConfig(config.GetType().Name) ??
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_EXCEPTION_FALLBACK"));
                var keyTuple = ((string, string)?)SecondaryDataSource.GetElementFromIndex(keyIdx) ??
                    throw new KernelException(KernelExceptionType.Config, LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_EXCEPTION_SETTINGSENTRIES"));
                var keys = FlattenSettingsKeys(entryIdx, configs[entryIdx].Keys);
                var key = keys[keyTuple.Item2];

                // Helper function
                void ResetKey(SettingsKey key)
                {
                    if (config is null)
                        return;

                    if (key.Type == SettingsKeyType.SMultivar)
                    {
                        // Get the settings keys from this key and prompt for it
                        var keys = key.Variables;
                        bool bailLoop = false;
                        while (!bailLoop)
                        {
                            // Prompt for it
                            var keysChoices = keys.Select((sk, idx) => new InputChoiceInfo($"{idx + 1}", $"{sk.Name} [{(sk.Masked ? "***" : ConfigTools.GetValueFromEntry(sk, config))}]: {sk.Description}")).ToList();
                            keysChoices.Add(new($"{keysChoices.Count + 1}", LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_EXIT")));
                            int choiceIdx = InfoBoxSelectionColor.WriteInfoBoxSelection([.. keysChoices], LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_MULTIVAR_CHOOSE") + $" \"{key.Name}\"");

                            // Check to see if exit is requested
                            if (choiceIdx < 0 || choiceIdx == keysChoices.Count - 1)
                            {
                                bailLoop = true;
                                continue;
                            }

                            // Now, get the settings key from the master key and set the value as-is
                            var selectedKey = keys[choiceIdx];
                            var fallbackValue = ConfigTools.GetValueFromEntry(selectedKey, fallbackConfig);
                            selectedKey.KeyInput.SetValue(selectedKey, fallbackValue, config);
                        }
                    }
                    else
                    {
                        var fallbackValue = ConfigTools.GetValueFromEntry(key, fallbackConfig);
                        key.KeyInput.SetValue(key, fallbackValue, config);
                        lastFirstPaneIdx = -1;
                    }
                }
                ResetKey(key);
                SettingsAppTools.SaveSettings();
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_TUI_RESETFAILED") + TextTools.FormatString("{0}: {1}", config?.GetType().Name ?? "<null>", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void SelectConfig()
        {
            try
            {
                // Select configuration
                var selectedConfig = SettingsAppTools.SelectConfig();
                if (selectedConfig is not null)
                {
                    toExpand.Clear();
                    config = selectedConfig;
                    lastFirstPaneIdx = -1;
                    InteractiveTuiTools.SelectionMovement(this, 0, 1);
                    InteractiveTuiTools.SelectionMovement(this, 0, 2);
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SELECTCONFIG_FAILED") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void EnableLegacyMultivarProcessing()
        {
            legacyMultivarProcessing = !legacyMultivarProcessing;
            if (legacyMultivarProcessing)
                InfoBoxModalColor.WriteInfoBoxModal("Please note that the legacy multivar processing support for the settings CLI will be removed in the final version of Nitrocid 0.2.0. Use this only when modern multivar processing is buggy.", Settings.InfoBoxSettings);
        }

        private Dictionary<string, SettingsKey> FlattenSettingsKeys(int entryIdx, SettingsKey[] keys, int level = 0, int[]? originalIndexes = null)
        {
            var allKeys = new Dictionary<string, SettingsKey>();
            for (int i = 0; i < keys.Length; i++)
            {
                SettingsKey? key = keys[i];
                string origKeyStr = originalIndexes is not null ? $"{string.Join("|", originalIndexes)}|{i}" : $"{i}";
                allKeys.Add(origKeyStr, key);
                if (key.Type == SettingsKeyType.SMultivar)
                {
                    int[] newOriginalIndexes = originalIndexes is not null ? [.. originalIndexes, i] : [i];
                    var moreKeys = FlattenSettingsKeys(entryIdx, key.Variables, level + 1, newOriginalIndexes);
                    foreach (var keyToAdd in moreKeys)
                    {
                        if (toExpand.Contains((origKeyStr, entryIdx)))
                            allKeys.Add(keyToAdd.Key, keyToAdd.Value);
                    }
                }
            }
            return allKeys;
        }
    }
}
