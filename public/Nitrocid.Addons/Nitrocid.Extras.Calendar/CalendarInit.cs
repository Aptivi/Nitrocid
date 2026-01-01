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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Extras.Calendar.Calendar.Commands;
using Nitrocid.Extras.Calendar.Calendar.Events;
using Nitrocid.Extras.Calendar.Calendar.Reminders;
using Nitrocid.Extras.Calendar.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Modifications;
using System.Linq;
using Nitrocid.Extras.Calendar.Calendar;
using Nitrocid.Kernel.Time.Calendars;
using EventInfo = Nitrocid.Extras.Calendar.Calendar.Events.EventInfo;
using Nitrocid.Shell.Homepage;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Extras.Calendar.Calendar.Screensavers;

namespace Nitrocid.Extras.Calendar
{
    internal class CalendarInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("altdate", /* Localizable */ "Shows date and time",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "culture", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Culture name"
                        })
                    ],
                    [
                        new SwitchInfo("date", /* Localizable */ "Shows just the date", new SwitchOptions()
                        {
                            ConflictsWith = ["full", "time"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("time", /* Localizable */ "Shows just the time", new SwitchOptions()
                        {
                            ConflictsWith = ["date", "full"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("full", /* Localizable */ "Shows date and time", new SwitchOptions()
                        {
                            ConflictsWith = ["date", "time"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("utc", /* Localizable */ "Uses UTC instead of local", new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ], true)
                ], new AltDateCommand(), CommandFlags.RedirectionSupported),

            new CommandInfo("calendar", /* Localizable */ "Calendar, event, and reminder manager",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "tui", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["tui"],
                            ArgumentDescription = /* Localizable */ "Opens the calendar interactive TUI"
                        }),
                        new CommandArgumentPart(false, "year", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Target year"
                        }),
                        new CommandArgumentPart(false, "month", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Target month number"
                        })
                    ],
                    [
                        new SwitchInfo("calendar", /* Localizable */ "Calendar type to work on"),
                        new SwitchInfo("legacy", /* Localizable */ "Use the table-based calendar viewer", new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ]),
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "event", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["event"],
                            ArgumentDescription = /* Localizable */ "Interaction with the events"
                        }),
                        new CommandArgumentPart(true, "add", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["add"],
                            ArgumentDescription = /* Localizable */ "Adds an event"
                        }),
                        new CommandArgumentPart(true, "date", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Target date"
                        }),
                        new CommandArgumentPart(true, "title", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Display name"
                        })
                    }),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "event", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["event"],
                            ArgumentDescription = /* Localizable */ "Interaction with the events"
                        }),
                        new CommandArgumentPart(true, "remove", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["remove"],
                            ArgumentDescription = /* Localizable */ "Removes an event"
                        }),
                        new CommandArgumentPart(true, "eventId", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Event ID"
                        })
                    ]),
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "event", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["event"],
                            ArgumentDescription = /* Localizable */ "Interaction with the events"
                        }),
                        new CommandArgumentPart(true, "list", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["list"],
                            ArgumentDescription = /* Localizable */ "Lists all events"
                        })
                    }),
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "event", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["event"],
                            ArgumentDescription = /* Localizable */ "Interaction with the events"
                        }),
                        new CommandArgumentPart(true, "saveall", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["saveall"],
                            ArgumentDescription = /* Localizable */ "Saves all the events"
                        })
                    }),
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "reminder", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["reminder"],
                            ArgumentDescription = /* Localizable */ "Interaction with the reminders"
                        }),
                        new CommandArgumentPart(true, "add", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["add"]
                        }),
                        new CommandArgumentPart(true, "dateandtime", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Target date and time"
                        }),
                        new CommandArgumentPart(true, "title", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Display name"
                        })
                    }),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "reminder", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["reminder"],
                            ArgumentDescription = /* Localizable */ "Interaction with the reminders"
                        }),
                        new CommandArgumentPart(true, "remove", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["remove"],
                            ArgumentDescription = /* Localizable */ "Removes a reminder"
                        }),
                        new CommandArgumentPart(true, "reminderid", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = /* Localizable */ "Reminder ID"
                        })
                    ]),
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "reminder", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["reminder"],
                            ArgumentDescription = /* Localizable */ "Interaction with the reminders"
                        }),
                        new CommandArgumentPart(true, "list", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["list"],
                            ArgumentDescription = /* Localizable */ "Lists all reminders"
                        })
                    }),
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "reminder", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["reminder"],
                            ArgumentDescription = /* Localizable */ "Interaction with the reminders"
                        }),
                        new CommandArgumentPart(true, "saveall", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["saveall"],
                            ArgumentDescription = /* Localizable */ "Saves all reminders"
                        })
                    }),
                ], new CalendarCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasCalendar);

        internal static CalendarConfig CalendarConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(CalendarConfig)) ? (CalendarConfig)Config.baseConfigurations[nameof(CalendarConfig)] : new CalendarConfig();

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => new(new Dictionary<string, Delegate>()
        {
            { nameof(CalendarPrint.PrintCalendar), new Action(CalendarPrint.PrintCalendar) },
            { nameof(CalendarPrint.PrintCalendar) + "2", new Action<CalendarTypes>(CalendarPrint.PrintCalendar) },
            { nameof(CalendarPrint.PrintCalendar) + "3", new Action<int, int, CalendarTypes>(CalendarPrint.PrintCalendar) },
            { nameof(EventManager.AddEvent), new Action<DateTime, string>(EventManager.AddEvent) },
            { nameof(EventManager.RemoveEvent), new Action<DateTime, int>(EventManager.RemoveEvent) },
            { nameof(EventManager.ListEvents), new Action(EventManager.ListEvents) },
            { nameof(EventManager.LoadEvents), new Action(EventManager.LoadEvents) },
            { nameof(EventManager.LoadEvent), new Func<string, EventInfo?>(EventManager.LoadEvent) },
            { nameof(EventManager.SaveEvents), new Action(EventManager.SaveEvents) },
            { nameof(EventManager.SaveEvents) + "2", new Action<string, bool>(EventManager.SaveEvents) },
            { nameof(EventManager.SaveEvent), new Action<EventInfo>(EventManager.SaveEvent) },
            { nameof(EventManager.SaveEvent) + "2", new Action<EventInfo, string>(EventManager.SaveEvent) },
            { nameof(ReminderManager.AddReminder), new Action<DateTime, string>(ReminderManager.AddReminder) },
            { nameof(ReminderManager.RemoveReminder), new Action<DateTime, int>(ReminderManager.RemoveReminder) },
            { nameof(ReminderManager.ListReminders), new Action(ReminderManager.ListReminders) },
            { nameof(ReminderManager.LoadReminders), new Action(ReminderManager.LoadReminders) },
            { nameof(ReminderManager.LoadReminder), new Func<string, ReminderInfo?>(ReminderManager.LoadReminder) },
            { nameof(ReminderManager.SaveReminders), new Action(ReminderManager.SaveReminders) },
            { nameof(ReminderManager.SaveReminders) + "2", new Action<string, bool>(ReminderManager.SaveReminders) },
            { nameof(ReminderManager.SaveReminder), new Action<ReminderInfo>(ReminderManager.SaveReminder) },
            { nameof(ReminderManager.SaveReminder) + "2", new Action<ReminderInfo, string>(ReminderManager.SaveReminder) },
        });

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
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
            HomepageTools.RegisterBuiltinAction(/* Localizable */ "Calendar", CalendarTui.OpenInteractive);
        }

        void IAddon.StartAddon()
        {
            var config = new CalendarConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
            ScreensaverManager.AddonSavers.Add("calendar", new CalendarDisplay());
        }

        void IAddon.StopAddon()
        {
            ReminderManager.Reminders.Clear();
            EventManager.CalendarEvents.Clear();
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(CalendarConfig));
            HomepageTools.UnregisterBuiltinAction("Calendar");
            ScreensaverManager.AddonSavers.Remove("calendar");
        }
    }
}
