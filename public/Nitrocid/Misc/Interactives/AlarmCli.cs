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
using Nitrocid.Kernel.Time.Alarm;
using Nitrocid.Languages;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Textify.General;

namespace Nitrocid.Misc.Interactives
{
    internal class AlarmCli : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            AlarmTools.alarms.Keys;

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override int RefreshInterval =>
            1000;

        /// <inheritdoc/>
        public override string GetInfoFromItem(string item)
        {
            // Get an instance of the alarm to grab its info from
            var alarm = AlarmTools.GetAlarmFromId(item).Value;

            // Generate the rendered text
            string name = alarm.Name;
            string description = alarm.Description;
            var due = alarm.Length;

            // Render them to the second pane
            return
                Translate.DoTranslation("Alarm name") + $": {name}" + CharManager.NewLine +
                Translate.DoTranslation("Alarm description") + $": {description}" + CharManager.NewLine +
                Translate.DoTranslation("Alarm due date") + $": {due}";
            ;
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem(string item)
        {
            // Get an instance of the alarm to grab its info from
            var alarm = AlarmTools.GetAlarmFromId(item).Value;

            // Generate the rendered text
            string name = alarm.Name;
            return $"{item} - {name}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item) =>
            item;

        internal void Start()
        {
            string name = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Write the alarm name"));
            if (string.IsNullOrWhiteSpace(name))
            {
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Alarm name is not specified."));
                return;
            }
            string interval = InfoBoxInputColor.WriteInfoBoxInput(Translate.DoTranslation("Write the alarm interval in this format") + ": HH:MM:SS");
            if (!TimeSpan.TryParse(interval, out TimeSpan span))
            {
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Alarm interval is either not specified or is invalid."));
                return;
            }
            AlarmTools.StartAlarm(name, name, (int)span.TotalSeconds);
        }

        internal void Stop(string? alarm)
        {
            if (alarm is not null)
                AlarmTools.StopAlarm(alarm);
        }
    }
}
