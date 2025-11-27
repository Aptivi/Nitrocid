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
using Nitrocid.Base.Kernel.Time.Alarm;
using Nitrocid.Base.Languages;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Textify.General;

namespace Nitrocid.Base.Misc.Interactives
{
    internal class AlarmCli : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                HelpTitle = /* Localizable */ "NKS_MISC_INTERACTIVES_ALARMTUI_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_MISC_INTERACTIVES_ALARMTUI_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_HELP01_BODY") + "\n\n" +
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/advanced-and-power-users/inner-workings/inner-essentials/date-and-time",
            }
        ];

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
                LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_ALARMNAME") + $": {name}" + CharManager.NewLine +
                LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_ALARMDESC") + $": {description}" + CharManager.NewLine +
                LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_ALARMDUE") + $": {due}";
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
            string name = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_ALARMNAMEPROMPT"));
            if (string.IsNullOrWhiteSpace(name))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_ALARMNAMENOTSPECCED"));
                return;
            }
            string interval = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_ALARMNAMEINTERVALPROMPT") + ": HH:MM:SS");
            if (!TimeSpan.TryParse(interval, out TimeSpan span))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_ALARMNAMEINTERVALINVALID"));
                return;
            }
            AlarmTools.StartAlarm(name, name, (int)span.TotalSeconds);
        }

        internal void Stop(string? alarm)
        {
            if (alarm is not null)
                AlarmTools.StopAlarm(alarm);
        }

        internal static void OpenAlarmCli()
        {
            var tui = new AlarmCli();
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_KEYBINDING_ADD"), ConsoleKey.A, (_, _, _, _) => tui.Start(), true));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_KEYBINDING_REMOVE"), ConsoleKey.Delete, (alarm, _, _, _) => tui.Stop(alarm)));
            InteractiveTuiTools.OpenInteractiveTui(tui);
        }
    }
}
