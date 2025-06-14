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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Extras.Calendar.Calendar.Commands;
using Nitrocid.Extras.Calendar.Calendar.Events;
using Nitrocid.Extras.Calendar.Calendar.Reminders;
using Nitrocid.Extras.Calendar.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;
using Nitrocid.Extras.Calendar.Calendar;
using Nitrocid.Shell.Homepage;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Extras.Calendar.Calendar.Screensavers;
using Nitrocid.Extras.Calendar.Localized;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Calendar
{
    internal class CalendarInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("altdate", LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_ALTDATE_DESC", "Nitrocid.Extras.Calendar"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "culture", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_ALTDATE_ARGUMENT_CULTURE_DESC", "Nitrocid.Extras.Calendar")
                        })
                    ],
                    [
                        new SwitchInfo("date", LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_ALTDATE_SWITCH_DATE_DESC", "Nitrocid.Extras.Calendar"), new SwitchOptions()
                        {
                            ConflictsWith = ["full", "time"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("time", LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_ALTDATE_SWITCH_TIME_DESC", "Nitrocid.Extras.Calendar"), new SwitchOptions()
                        {
                            ConflictsWith = ["date", "full"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("full", LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_ALTDATE_DESC", "Nitrocid.Extras.Calendar"), new SwitchOptions()
                        {
                            ConflictsWith = ["date", "time"],
                            AcceptsValues = false
                        }),
                        new SwitchInfo("utc", LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_ALTDATE_SWITCH_UTC_DESC", "Nitrocid.Extras.Calendar"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ], true)
                ], new AltDateCommand(), CommandFlags.RedirectionSupported),

            new CommandInfo("calendar", LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_DESC", "Nitrocid.Extras.Calendar"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "tui", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["tui"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_TUI_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(false, "year", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_YEAR_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(false, "month", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_MONTH_DESC", "Nitrocid.Extras.Calendar")
                        })
                    ],
                    [
                        new SwitchInfo("calendar", LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_SWITCH_CALENDAR_DESC", "Nitrocid.Extras.Calendar")),
                        new SwitchInfo("legacy", LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_SWITCH_LEGACY_DESC", "Nitrocid.Extras.Calendar"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        }),
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "event", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["event"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_EVENT_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(true, "add", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["add"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_EVENT_ADD_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(true, "date", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_EVENT_TARGETDATE_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(true, "title", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_TITLE_DESC", "Nitrocid.Extras.Calendar")
                        })
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "event", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["event"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_EVENT_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(true, "remove", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["remove"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_EVENT_REMOVE_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(true, "eventId", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_EVENT_EVENTID_DESC", "Nitrocid.Extras.Calendar")
                        })
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "event", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["event"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_EVENT_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(true, "list", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["list"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_EVENT_LIST_DESC", "Nitrocid.Extras.Calendar")
                        })
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "event", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["event"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_EVENT_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(true, "saveall", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["saveall"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_EVENT_SAVEALL_DESC", "Nitrocid.Extras.Calendar")
                        })
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "reminder", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["reminder"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_REMINDER_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(true, "add", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["add"]
                        }),
                        new CommandArgumentPart(true, "dateandtime", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_REMINDER_TARGET_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(true, "title", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_TITLE_DESC", "Nitrocid.Extras.Calendar")
                        })
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "reminder", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["reminder"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_REMINDER_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(true, "remove", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["remove"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_REMINDER_REMOVE_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(true, "reminderid", new CommandArgumentPartOptions()
                        {
                            IsNumeric = true,
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_REMINDER_REMINDERID_DESC", "Nitrocid.Extras.Calendar")
                        })
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "reminder", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["reminder"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_REMINDER_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(true, "list", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["list"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_REMINDER_LIST_DESC", "Nitrocid.Extras.Calendar")
                        })
                    ]),
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "reminder", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["reminder"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_REMINDER_DESC", "Nitrocid.Extras.Calendar")
                        }),
                        new CommandArgumentPart(true, "saveall", new CommandArgumentPartOptions()
                        {
                            ExactWording = ["saveall"],
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_CALENDAR_COMMAND_CALENDAR_ARGUMENT_REMINDER_SAVEALL_DESC", "Nitrocid.Extras.Calendar")
                        })
                    ]),
                ], new CalendarCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasCalendar);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        internal static CalendarConfig CalendarConfig =>
            (CalendarConfig)Config.baseConfigurations[nameof(CalendarConfig)];

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
            HomepageTools.RegisterBuiltinAction("Calendar", CalendarTui.OpenInteractive);
        }

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.Calendar", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            var config = new CalendarConfig();
            ConfigTools.RegisterBaseSetting(config);
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
            ScreensaverManager.AddonSavers.Add("calendar", new CalendarDisplay());
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.Calendar");
            ReminderManager.Reminders.Clear();
            EventManager.CalendarEvents.Clear();
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(CalendarConfig));
            HomepageTools.UnregisterBuiltinAction("Calendar");
            ScreensaverManager.AddonSavers.Remove("calendar");
        }
    }
}
