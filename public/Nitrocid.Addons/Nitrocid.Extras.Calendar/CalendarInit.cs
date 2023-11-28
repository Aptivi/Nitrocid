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

using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Extensions;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.ShellBase.Switches;
using Nitrocid.Extras.Calendar.Calendar.Commands;
using Nitrocid.Extras.Calendar.Calendar.Events;
using Nitrocid.Extras.Calendar.Calendar.Reminders;
using Nitrocid.Extras.Calendar.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Nitrocid.Extras.Calendar
{
    internal class CalendarInit : IAddon
    {
        private readonly Dictionary<string, CommandInfo> addonCommands = new()
        {
            { "altdate",
                new CommandInfo("altdate", /* Localizable */ "Shows date and time",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "culture")
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
                    ], new AltDateCommand(), CommandFlags.RedirectionSupported)
            },

            { "calendar",
                new CommandInfo("calendar", /* Localizable */ "Calendar, event, and reminder manager",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "show", new CommandArgumentPartOptions()
                            {
                                ExactWording = "show"
                            }),
                            new CommandArgumentPart(false, "year", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            }),
                            new CommandArgumentPart(false, "month", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        ],
                        [
                            new SwitchInfo("calendar", /* Localizable */ "Calendar type to work on")
                        ]),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "event", new CommandArgumentPartOptions()
                            {
                                ExactWording = "event"
                            }),
                            new CommandArgumentPart(true, "add", new CommandArgumentPartOptions()
                            {
                                ExactWording = "add"
                            }),
                            new CommandArgumentPart(true, "date"),
                            new CommandArgumentPart(true, "title")
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "event", new CommandArgumentPartOptions()
                            {
                                ExactWording = "event"
                            }),
                            new CommandArgumentPart(true, "remove", new CommandArgumentPartOptions()
                            {
                                ExactWording = "remove"
                            }),
                            new CommandArgumentPart(true, "eventId", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "event", new CommandArgumentPartOptions()
                            {
                                ExactWording = "event"
                            }),
                            new CommandArgumentPart(true, "list", new CommandArgumentPartOptions()
                            {
                                ExactWording = "list"
                            })
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "event", new CommandArgumentPartOptions()
                            {
                                ExactWording = "event"
                            }),
                            new CommandArgumentPart(true, "saveall", new CommandArgumentPartOptions()
                            {
                                ExactWording = "saveall"
                            })
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "reminder", new CommandArgumentPartOptions()
                            {
                                ExactWording = "reminder"
                            }),
                            new CommandArgumentPart(true, "add", new CommandArgumentPartOptions()
                            {
                                ExactWording = "add"
                            }),
                            new CommandArgumentPart(true, "dateandtime"),
                            new CommandArgumentPart(true, "title")
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "reminder", new CommandArgumentPartOptions()
                            {
                                ExactWording = "reminder"
                            }),
                            new CommandArgumentPart(true, "remove", new CommandArgumentPartOptions()
                            {
                                ExactWording = "remove"
                            }),
                            new CommandArgumentPart(true, "reminderid", new CommandArgumentPartOptions()
                            {
                                IsNumeric = true
                            })
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "reminder", new CommandArgumentPartOptions()
                            {
                                ExactWording = "reminder"
                            }),
                            new CommandArgumentPart(true, "list", new CommandArgumentPartOptions()
                            {
                                ExactWording = "list"
                            })
                        }),
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "reminder", new CommandArgumentPartOptions()
                            {
                                ExactWording = "reminder"
                            }),
                            new CommandArgumentPart(true, "saveall", new CommandArgumentPartOptions()
                            {
                                ExactWording = "saveall"
                            })
                        }),
                    ], new CalendarCommand())
            },
        };

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasCalendar);

        AddonType IAddon.AddonType => AddonType.Optional;

        internal static CalendarConfig CalendarConfig =>
            (CalendarConfig)Config.baseConfigurations[nameof(CalendarConfig)];

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

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
        }

        void IAddon.StartAddon()
        {
            var config = new CalendarConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands.Values]);
        }

        void IAddon.StopAddon()
        {
            ReminderManager.Reminders.Clear();
            EventManager.CalendarEvents.Clear();
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Keys]);
            ConfigTools.UnregisterBaseSetting(nameof(CalendarConfig));
        }
    }
}
