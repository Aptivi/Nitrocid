//
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
using Terminaux.Base.Extensions;

namespace Nitrocid.Misc.Interactives
{
    /// <summary>
    /// Settings interactive TUI
    /// </summary>
    public class SettingsCli : BaseInteractiveTui<(string, int)>, IInteractiveTui<(string, int)>
    {
        internal static BaseKernelConfig config;

        /// <summary>
        /// File manager bindings
        /// </summary>
        public override InteractiveTuiBinding[] Bindings { get; } =
        [
            // Operations
            new InteractiveTuiBinding("Set", ConsoleKey.Enter,
                (_, _) => Set(InteractiveTuiStatus.FirstPaneCurrentSelection - 1, InteractiveTuiStatus.SecondPaneCurrentSelection - 1)),
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
                    var configs = config.SettingsEntries;
                    var configNames = configs.Select((se, idx) => (se.Name, idx)).ToArray();
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
                    var entry = config.SettingsEntries[InteractiveTuiStatus.FirstPaneCurrentSelection - 1];
                    var keys = entry.Keys;
                    var keyNames = keys.Select((key, idx) =>
                    {
                        object currentValue = ConfigTools.GetValueFromEntry(key, config);
                        return ($"{key.Name} [{currentValue}]", idx);
                    }).ToArray();
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
            string status;
            if (InteractiveTuiStatus.CurrentPane == 2)
            {
                string keyName = item.Item1;
                int keyIdx = item.Item2;
                string keyDesc = config.SettingsEntries[InteractiveTuiStatus.FirstPaneCurrentSelection - 1].Keys[keyIdx].Description;
                status = $"K: {keyName} - {keyDesc}";
            }
            else
            {
                string entryName = item.Item1;
                int entryIdx = item.Item2;
                string entryDesc = config.SettingsEntries[entryIdx].Desc;
                status = $"E: {entryName} - {entryDesc}";
            }
            return status;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem((string, int) item) =>
            item.Item1;

        /// <inheritdoc/>
        public override string GetInfoFromItem((string, int) item)
        {
            string status;
            if (InteractiveTuiStatus.CurrentPane == 2)
            {
                string keyName = item.Item1;
                int keyIdx = item.Item2;
                string keyDesc = config.SettingsEntries[InteractiveTuiStatus.FirstPaneCurrentSelection - 1].Keys[keyIdx].Description;
                status =
                    $"""
                    {keyName}
                    ====================================================
                    
                    {keyDesc}
                    """;
            }
            else
            {
                string entryName = item.Item1;
                int entryIdx = item.Item2;
                string entryDesc = config.SettingsEntries[entryIdx].Desc;
                status =
                    $"""
                    {entryName}
                    ====================================================
                    
                    {entryDesc}
                    """;
            }
            return status;
        }

        private static void Set(int entryIdx, int keyIdx)
        {
            try
            {
                // Get the key and try to set
                var key = config.SettingsEntries[entryIdx].Keys[keyIdx];
                var defaultValue = ConfigTools.GetValueFromEntry(key, config);
                var input = key.KeyInput.PromptForSet(key, defaultValue, out bool provided);
                if (provided)
                    key.KeyInput.SetValue(key, input, config);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(Translate.DoTranslation("Can't open file or folder") + TextTools.FormatString(": {0}", ex.Message));
                finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));
                InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            }
        }
    }
}
