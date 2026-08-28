//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Extras.Calendar.Calendar.Commands;
using Nitrocid.Extras.Calendar.Calendar.Events;
using Nitrocid.Extras.Calendar.Calendar.Reminders;
using Nitrocid.Extras.Calendar.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Terminaux.Shell.Commands;
using Nitrocid.Kernel.Extensions;
using System.Linq;
using Nitrocid.Extras.Calendar.Calendar;
using Nitrocid.Shell.Homepage;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Extras.Calendar.Calendar.Screensavers;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Calendar
{
    internal class CalendarInit : IAddon
    {
        private readonly BaseCommand[] addonCommands =
        [
            new AltDateCommand(),
            new CalendarCommand(),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasCalendar);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasCalendar);

        internal static CalendarConfig CalendarConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(CalendarConfig)) ? (CalendarConfig)Config.baseConfigurations[nameof(CalendarConfig)] : Config.GetFallbackKernelConfig<CalendarConfig>();

        public void FinalizeAddon()
        {
            // Initialize events and reminders
            if (!ReminderManager.ReminderThread.IsAlive)
                ReminderManager.ReminderThread.Start();
            if (!EventManager.EventThread.IsAlive)
                EventManager.EventThread.Start();
            EventManager.LoadEvents();
            ReminderManager.LoadReminders();
            DebugWriter.WriteDebug(DebugLevel.I, "Loaded events & reminders.");

            // Add the calendar option to the homepage
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "NKS_CALENDAR_HOMEPAGE_CALENDAR", CalendarTui.OpenInteractive);
        }

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Calendar.Resources.Languages.Output.Localizations", typeof(CalendarInit).Assembly));
            var config = new CalendarConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
            ScreensaverManager.AddonSavers.Add("calendar", new CalendarDisplay());
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            ReminderManager.Reminders.Clear();
            EventManager.CalendarEvents.Clear();
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(CalendarConfig));
            HomepageTools.UnregisterBuiltinAction(/* Localizable */ "NKS_CALENDAR_HOMEPAGE_CALENDAR");
            ScreensaverManager.AddonSavers.Remove("calendar");
        }
    }
}
