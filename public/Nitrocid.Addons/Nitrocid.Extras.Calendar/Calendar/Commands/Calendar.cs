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
using System.Linq;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Extras.Calendar.Calendar.Events;
using Nitrocid.Extras.Calendar.Calendar.Reminders;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Time.Calendars;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;

namespace Nitrocid.Extras.Calendar.Calendar.Commands
{
    /// <summary>
    /// Manages your calendar
    /// </summary>
    /// <remarks>
    /// This is a master application for the calendar that not only it shows you the calendar, but also shows and manages the events and reminders.
    /// </remarks>
    class CalendarCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string Action = parameters.ArgumentsList[0];

            // Enumerate based on action
            int ActionMinimumArguments = 1;
            var ActionArguments = parameters.ArgumentsList.Skip(1).ToArray();
            switch (Action)
            {
                case "tui":
                    {
                        // User chose to show the calendar TUI
                        var calendar = CalendarTypes.Variant;
                        bool useLegacy = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-legacy");
                        if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-calendar"))
                            calendar = Enum.Parse<CalendarTypes>(SwitchManager.GetSwitchValue(parameters.SwitchesList, "-calendar"));
                        if (ActionArguments.Length != 0)
                        {
                            try
                            {
                                string StringYear = ActionArguments[0];
                                string StringMonth = DateTime.Today.Month.ToString();
                                if (ActionArguments.Length >= 2)
                                    StringMonth = ActionArguments[1];

                                // Show the calendar using the provided year and month
                                int yearInt = Convert.ToInt32(StringYear);
                                int monthInt = Convert.ToInt32(StringMonth);
                                if (useLegacy)
                                    CalendarPrint.PrintCalendar(yearInt, monthInt, calendar);
                                else
                                    CalendarTui.OpenInteractive(yearInt, monthInt, DateTime.Today.Day, calendar);
                            }
                            catch (Exception ex)
                            {
                                DebugWriter.WriteDebugStackTrace(ex);
                                TextWriters.Write(Translate.DoTranslation("Failed to show the calendar.") + " {0}", true, KernelColorType.Error, ex.Message);
                                return ex.GetHashCode();
                            }
                        }
                        else
                        {
                            if (useLegacy)
                                CalendarPrint.PrintCalendar(calendar);
                            else
                                CalendarTui.OpenInteractive(calendar);
                        }

                        return 0;
                    }
                case "event":
                    {
                        // User chose to manipulate with the day events
                        if (ActionArguments.Length >= ActionMinimumArguments)
                        {
                            // User provided any of add, remove, and list. However, the first two arguments need minimum arguments of three parameters, so check.
                            string ActionType = ActionArguments[0];
                            switch (ActionType)
                            {
                                case "add":
                                    {
                                        // Parse the arguments to check to see if enough arguments are passed to those parameters
                                        ActionMinimumArguments = 3;
                                        if (ActionArguments.Length >= ActionMinimumArguments)
                                        {
                                            // Enough arguments provided.
                                            try
                                            {
                                                string StringDate = ActionArguments[1];
                                                string EventTitle = ActionArguments[2];
                                                var ParsedDate = DateTime.Parse(StringDate);
                                                EventManager.AddEvent(ParsedDate, EventTitle);
                                            }
                                            catch (Exception ex)
                                            {
                                                DebugWriter.WriteDebugStackTrace(ex);
                                                TextWriters.Write(Translate.DoTranslation("Failed to add an event.") + " {0}", true, KernelColorType.Error, ex.Message);
                                                return ex.GetHashCode();
                                            }
                                        }
                                        else
                                        {
                                            TextWriters.Write(Translate.DoTranslation("Not enough arguments provided to add an event."), true, KernelColorType.Error);
                                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Calendar);
                                        }

                                        return 0;
                                    }
                                case "remove":
                                    {
                                        // Parse the arguments to check to see if enough arguments are passed to those parameters
                                        ActionMinimumArguments = 2;
                                        if (ActionArguments.Length >= ActionMinimumArguments)
                                        {
                                            // Enough arguments provided.
                                            try
                                            {
                                                int EventId = Convert.ToInt32(ActionArguments[1]);
                                                var EventInstance = EventManager.CalendarEvents[EventId - 1];
                                                EventManager.RemoveEvent(EventInstance.EventDate, EventId);
                                            }
                                            catch (Exception ex)
                                            {
                                                DebugWriter.WriteDebugStackTrace(ex);
                                                TextWriters.Write(Translate.DoTranslation("Failed to remove an event.") + " {0}", true, KernelColorType.Error, ex.Message);
                                                return ex.GetHashCode();
                                            }
                                        }
                                        else
                                        {
                                            TextWriters.Write(Translate.DoTranslation("Not enough arguments provided to remove an event."), true, KernelColorType.Error);
                                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Calendar);
                                        }

                                        return 0;
                                    }
                                case "list":
                                    {
                                        // User chose to list. No parse needed as we're only FilesystemTools.
                                        EventManager.ListEvents();
                                        return 0;
                                    }
                                case "saveall":
                                    {
                                        // User chose to save all.
                                        EventManager.SaveEvents();
                                        return 0;
                                    }
                                default:
                                    {
                                        // Invalid action.
                                        TextWriters.Write(Translate.DoTranslation("Invalid action."), true, KernelColorType.Error);
                                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.Calendar);
                                    }
                            }
                        }
                        else
                        {
                            TextWriters.Write(Translate.DoTranslation("Not enough arguments provided for event manipulation."), true, KernelColorType.Error);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Calendar);
                        }
                    }
                case "reminder":
                    {
                        // User chose to manipulate with the day reminders
                        if (ActionArguments.Length >= ActionMinimumArguments)
                        {
                            // User provided any of add, remove, and list. However, the first two arguments need minimum arguments of three parameters, so check.
                            string ActionType = ActionArguments[0];
                            switch (ActionType)
                            {
                                case "add":
                                    {
                                        // Parse the arguments to check to see if enough arguments are passed to those parameters
                                        ActionMinimumArguments = 3;
                                        if (ActionArguments.Length >= ActionMinimumArguments)
                                        {
                                            // Enough arguments provided.
                                            try
                                            {
                                                string StringDate = ActionArguments[1];
                                                string ReminderTitle = ActionArguments[2];
                                                var ParsedDate = DateTime.Parse(StringDate);
                                                ReminderManager.AddReminder(ParsedDate, ReminderTitle);
                                            }
                                            catch (Exception ex)
                                            {
                                                DebugWriter.WriteDebugStackTrace(ex);
                                                TextWriters.Write(Translate.DoTranslation("Failed to add a reminder.") + " {0}", true, KernelColorType.Error, ex.Message);
                                                return ex.GetHashCode();
                                            }
                                        }
                                        else
                                        {
                                            TextWriters.Write(Translate.DoTranslation("Not enough arguments provided to add a reminder."), true, KernelColorType.Error);
                                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Calendar);
                                        }

                                        return 0;
                                    }
                                case "remove":
                                    {
                                        // Parse the arguments to check to see if enough arguments are passed to those parameters
                                        ActionMinimumArguments = 2;
                                        if (ActionArguments.Length >= ActionMinimumArguments)
                                        {
                                            // Enough arguments provided.
                                            try
                                            {
                                                int ReminderId = Convert.ToInt32(ActionArguments[1]);
                                                var ReminderInstance = ReminderManager.Reminders[ReminderId - 1];
                                                ReminderManager.RemoveReminder(ReminderInstance.ReminderDate, ReminderId);
                                            }
                                            catch (Exception ex)
                                            {
                                                DebugWriter.WriteDebugStackTrace(ex);
                                                TextWriters.Write(Translate.DoTranslation("Failed to remove a reminder.") + " {0}", true, KernelColorType.Error, ex.Message);
                                                return ex.GetHashCode();
                                            }
                                        }
                                        else
                                        {
                                            TextWriters.Write(Translate.DoTranslation("Not enough arguments provided to remove a reminder."), true, KernelColorType.Error);
                                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Calendar);
                                        }

                                        return 0;
                                    }
                                case "list":
                                    {
                                        // User chose to list. No parse needed as we're only FilesystemTools.
                                        ReminderManager.ListReminders();
                                        return 0;
                                    }
                                case "saveall":
                                    {
                                        // User chose to save all.
                                        ReminderManager.SaveReminders();
                                        return 0;
                                    }
                                default:
                                    {
                                        // Invalid action.
                                        TextWriters.Write(Translate.DoTranslation("Invalid action."), true, KernelColorType.Error);
                                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.Calendar);
                                    }
                            }
                        }
                        else
                        {
                            TextWriters.Write(Translate.DoTranslation("Not enough arguments provided for reminder manipulation."), true, KernelColorType.Error);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Calendar);
                        }
                    }
                default:
                    {
                        // Invalid action.
                        TextWriters.Write(Translate.DoTranslation("Invalid action."), true, KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.Calendar);
                    }
            }
        }

    }
}
