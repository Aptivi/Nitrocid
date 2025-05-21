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

using Nitrocid.Extras.Calendar.Calendar.Events;
using Nitrocid.Extras.Calendar.Calendar.Reminders;
using System;
using System.Linq;
using System.Text;
using Terminaux.Sequences.Builder.Types;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Time.Calendars;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Base.Buffered;
using Nitrocid.Languages;
using Nitrocid.Kernel.Time.Converters;
using Nitrocid.Kernel.Time;
using Textify.General;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Nitrocid.Kernel.Time.Renderers;
using System.Collections.Generic;
using Terminaux.Inputs;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Nitrocid.Extras.Calendar.Calendar
{
    internal static class CalendarTui
    {
        private static string status = "";
        private static bool bail;
        private static (int Year, int Month, int Day, CalendarTypes calendar) state;
        private static readonly Keybinding[] bindings =
        [
            new Keybinding( /* Localizable */ "Exit", ConsoleKey.Escape),
            new Keybinding( /* Localizable */ "Keybindings", ConsoleKey.K),
            new Keybinding( /* Localizable */ "Events...", ConsoleKey.E),
            new Keybinding( /* Localizable */ "Reminders...", ConsoleKey.R),
        ];

        /// <summary>
        /// Opens an interactive calendar
        /// </summary>
        public static void OpenInteractive() =>
            OpenInteractive(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

        /// <summary>
        /// Opens an interactive calendar
        /// </summary>
        public static void OpenInteractive(CalendarTypes calendar) =>
            OpenInteractive(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, calendar);

        /// <summary>
        /// Opens an interactive calendar
        /// </summary>
        internal static void OpenInteractive(int Year, int Month, int Day, CalendarTypes calendar = CalendarTypes.Variant)
        {
            // Set status
            state = (Year, Month, Day, calendar);
            status = Translate.DoTranslation("Ready");
            bail = false;

            // Main loop
            var screen = new Screen();
            ScreenTools.SetCurrent(screen);
            ConsoleWrapper.CursorVisible = false;
            KernelColorTools.LoadBackground();
            try
            {
                while (!bail)
                {
                    // Now, render the keybindings
                    RenderKeybindings(ref screen);

                    // Render the box
                    RenderViewBox(ref screen);

                    // Now, render the calendar with the current selection
                    RenderCalendar(state, ref screen);

                    // Render the status
                    RenderStatus(ref screen);

                    // Wait for a keypress
                    ScreenTools.Render(screen);
                    var keypress = Input.ReadKey();
                    HandleKeypress(keypress, ref state);

                    // Reset, in case selection changed
                    screen.RemoveBufferedParts();
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Interactive calendar failed: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("The interactive calendar failed:") + $" {ex.Message}", new InfoBoxSettings()
                {
                    ForegroundColor = KernelColorTools.GetColor(KernelColorType.Error)
                });
            }
            bail = false;
            ScreenTools.UnsetCurrent(screen);

            // Close the file and clean up
            KernelColorTools.LoadBackground();
        }

        private static void RenderKeybindings(ref Screen screen)
        {
            // Make a screen part
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var keybindings = new Keybindings()
                {
                    KeybindingList = bindings,
                    Width = ConsoleWrapper.WindowWidth - 1,
                    BuiltinColor = KernelColorTools.GetColor(KernelColorType.TuiKeyBindingBuiltin),
                    BuiltinForegroundColor = KernelColorTools.GetColor(KernelColorType.TuiKeyBindingBuiltinForeground),
                    BuiltinBackgroundColor = KernelColorTools.GetColor(KernelColorType.TuiKeyBindingBuiltinBackground),
                    OptionColor = KernelColorTools.GetColor(KernelColorType.TuiKeyBindingOption),
                    OptionForegroundColor = KernelColorTools.GetColor(KernelColorType.TuiOptionForeground),
                    OptionBackgroundColor = KernelColorTools.GetColor(KernelColorType.TuiOptionBackground),
                };
                return RendererTools.RenderRenderable(keybindings, new(0, ConsoleWrapper.WindowHeight - 1));
            });
            screen.AddBufferedPart("Interactive calendar - Keybindings", part);
        }

        private static void RenderStatus(ref Screen screen)
        {
            // Make a screen part
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var builder = new StringBuilder();
                builder.Append(
                    $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.TuiForeground))}" +
                    $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.Background), true)}" +
                    $"{TextWriterWhereColor.RenderWhere(status + ConsoleClearing.GetClearLineToRightSequence(), 0, 0)}"
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Interactive calendar - Status", part);
        }

        private static void RenderViewBox(ref Screen screen)
        {
            // Make a screen part
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var builder = new StringBuilder();

                // Get the widths and heights
                int SeparatorConsoleWidthInterior = ConsoleWrapper.WindowWidth - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

                // Render the box
                var border = new Border()
                {
                    Left = 0,
                    Top = SeparatorMinimumHeight,
                    Width = SeparatorConsoleWidthInterior,
                    Height = SeparatorMaximumHeightInterior,
                    Color = KernelColorTools.GetColor(KernelColorType.TuiPaneSeparator),
                    BackgroundColor = KernelColorTools.GetColor(KernelColorType.Background),
                };
                builder.Append(border.Render());
                return builder.ToString();
            });
            screen.AddBufferedPart("Interactive calendar - View box", part);
        }

        private static void RenderCalendar((int Year, int Month, int Day, CalendarTypes calendar) state, ref Screen screen)
        {
            // First, update the status
            StatusNumInfo(state);

            // Then, render the contents with the selection indicator
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                // Populate some necessary variables
                var builder = new StringBuilder();
                var calendarInstance = CalendarTools.GetCalendar(state.calendar);
                var CalendarMonths = calendarInstance.Culture.DateTimeFormat.MonthNames;
                var maxDate = calendarInstance.Calendar.GetDaysInMonth(state.Year, state.Month);
                var selectedDate = new DateTime(state.Year, state.Month, TimeDateTools.KernelDateTime.Day > maxDate ? 1 : TimeDateTools.KernelDateTime.Day);
                var (year, month, _, _) = TimeDateConverters.GetDateFromCalendar(selectedDate, state.calendar);
                string CalendarTitle = CalendarMonths[month - 1] + " " + year;
                var boxForeground = KernelColorTools.GetColor(KernelColorType.NeutralText);
                var background = KernelColorTools.GetColor(KernelColorType.Background);
                var calendar = new FullCalendar()
                {
                    Left = 4,
                    Top = 3,
                    Year = state.Year,
                    Month = state.Month,
                    ForegroundColor = boxForeground,
                    BackgroundColor = background,
                    HighlightToday = false,
                    HighlightedDay = new(state.Year, state.Month, state.Day),
                    TodayColor = KernelColorTools.GetColor(KernelColorType.TodayDay),
                    WeekendColor = KernelColorTools.GetColor(KernelColorType.WeekendDay),
                    HeaderColor = KernelColorTools.GetColor(KernelColorType.TuiForeground),
                    Culture = calendarInstance.Culture,
                };
                builder.Append(calendar.Render());

                // Write the side box representing the list of events
                int eventBoxLeft = 4 + (6 * 7) + 2;
                int eventBoxTop = 3;
                int eventBoxWidth = ConsoleWrapper.WindowWidth - eventBoxLeft - 6;
                int eventBoxHeight = ConsoleWrapper.WindowHeight - 8;
                var eventBorder = new Border()
                {
                    Title = Translate.DoTranslation("Events and reminders for") + $" {CalendarTitle}",
                    Left = eventBoxLeft,
                    Top = eventBoxTop,
                    Width = eventBoxWidth,
                    Height = eventBoxHeight,
                    Color = boxForeground,
                    TextColor = boxForeground,
                    BackgroundColor = background,
                };
                builder.Append(eventBorder.Render());

                // List all events and reminders in a separate builder to wrap
                var eventsBuilder = new StringBuilder();
                eventsBuilder.AppendLine(RenderEvents(state));
                eventsBuilder.AppendLine(RenderReminders(state));
                var eventsLines = eventsBuilder.ToString().GetWrappedSentencesByWords(eventBoxWidth);
                for (int l = 0; l < eventsLines.Length && l < eventBoxHeight; l++)
                {
                    string eventsLine = eventsLines[l];
                    builder.Append(
                        CsiSequences.GenerateCsiCursorPosition(eventBoxLeft + 2, eventBoxTop + 2 + l) +
                        eventsLine
                    );
                }

                // Finalize everything
                builder.Append(
                    $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.TuiForeground))}" +
                    $"{ColorTools.RenderSetConsoleColor(background, true)}"
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Interactive calendar - Contents", part);
        }

        private static void StatusNumInfo((int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            // Change the status to reflect the selected day
            status =
                $"MM/DD/YYYY: {state.Month}/{state.Day}/{state.Year} | " +
                $"{Translate.DoTranslation("Long form")}: {TimeDateRenderers.RenderDate(new DateTime(state.Year, state.Month, state.Day), FormatType.Long)} | " +
                $"{Translate.DoTranslation("Calendar type")}: {state.calendar}";
        }

        private static void HandleKeypress(ConsoleKeyInfo key, ref (int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            // Check to see if we have this binding
            if (!bindings.Any((heb) => heb.BindingKeyName == key.Key && heb.BindingKeyModifiers == key.Modifiers))
            {
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        PreviousDay(ref state);
                        break;
                    case ConsoleKey.RightArrow:
                        NextDay(ref state);
                        break;
                    case ConsoleKey.UpArrow:
                        PreviousWeek(ref state);
                        break;
                    case ConsoleKey.DownArrow:
                        NextWeek(ref state);
                        break;
                    case ConsoleKey.PageUp:
                        PreviousMonth(ref state);
                        break;
                    case ConsoleKey.PageDown:
                        NextMonth(ref state);
                        break;
                    case ConsoleKey.Home:
                        PreviousYear(ref state);
                        break;
                    case ConsoleKey.End:
                        NextYear(ref state);
                        break;
                }
            }
            else
            {
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        bail = true;
                        break;
                    case ConsoleKey.K:
                        RenderKeybindingsBox();
                        break;
                    case ConsoleKey.E:
                        InfoBoxModalColor.WriteInfoBoxModal(RenderEventsDay(state));
                        break;
                    case ConsoleKey.R:
                        InfoBoxModalColor.WriteInfoBoxModal(RenderRemindersDay(state));
                        break;
                }
            }
        }

        private static void RenderKeybindingsBox()
        {
            // Show the available keys list
            if (bindings.Length == 0)
                return;
            InfoBoxModalColor.WriteInfoBoxModal(KeybindingTools.RenderKeybindingHelpText(bindings), new InfoBoxSettings()
            {
                ForegroundColor = KernelColorTools.GetColor(KernelColorType.TuiBoxForeground),
                BackgroundColor = KernelColorTools.GetColor(KernelColorType.TuiBoxBackground)
            });
            return;
        }

        private static void PreviousDay(ref (int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            state.Day--;
            if (state.Day == 0)
            {
                // First, decrement the month.
                var calendarInstance = CalendarTools.GetCalendar(state.calendar);
                state.Month--;
                if (state.Month == 0)
                {
                    state.Year--;
                    state.Month = calendarInstance.Calendar.GetMonthsInYear(state.Year);
                }
                state.Day = calendarInstance.Calendar.GetDaysInMonth(state.Year, state.Month);
            }
        }

        private static void NextDay(ref (int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            var calendarInstance = CalendarTools.GetCalendar(state.calendar);
            int maxDays = calendarInstance.Calendar.GetDaysInMonth(state.Year, state.Month);
            state.Day++;
            if (state.Day > maxDays)
            {
                // First, increment the month.
                int maxMonths = calendarInstance.Calendar.GetMonthsInYear(state.Year);
                state.Month++;
                if (state.Month > maxMonths)
                {
                    state.Year++;
                    state.Month = 1;
                }
                state.Day = 1;
            }
        }

        private static void PreviousWeek(ref (int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            state.Day -= 7;
            if (state.Day <= 0)
            {
                // Get absolute value of the negative day
                int offset = Math.Abs(state.Day);

                // First, decrement the month.
                var calendarInstance = CalendarTools.GetCalendar(state.calendar);
                state.Month--;
                if (state.Month == 0)
                {
                    state.Year--;
                    state.Month = calendarInstance.Calendar.GetMonthsInYear(state.Year);
                }
                state.Day = calendarInstance.Calendar.GetDaysInMonth(state.Year, state.Month) - offset;
            }
        }

        private static void NextWeek(ref (int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            var calendarInstance = CalendarTools.GetCalendar(state.calendar);
            int maxDays = calendarInstance.Calendar.GetDaysInMonth(state.Year, state.Month);
            state.Day += 7;
            if (state.Day > maxDays)
            {
                // Get day overflow count
                int offset = state.Day - maxDays;

                // First, increment the month.
                int maxMonths = calendarInstance.Calendar.GetMonthsInYear(state.Year);
                state.Month++;
                if (state.Month > maxMonths)
                {
                    state.Year++;
                    state.Month = 1;
                }
                state.Day = offset;
            }
        }

        private static void PreviousMonth(ref (int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            // First, decrement the month.
            var calendarInstance = CalendarTools.GetCalendar(state.calendar);
            state.Month--;
            if (state.Month == 0)
            {
                state.Year--;
                state.Month = calendarInstance.Calendar.GetMonthsInYear(state.Year);
            }
            state.Day = calendarInstance.Calendar.GetDaysInMonth(state.Year, state.Month);
        }

        private static void NextMonth(ref (int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            var calendarInstance = CalendarTools.GetCalendar(state.calendar);
            int maxMonths = calendarInstance.Calendar.GetMonthsInYear(state.Year);
            state.Month++;
            if (state.Month > maxMonths)
            {
                state.Year++;
                state.Month = 1;
            }
            state.Day = 1;
        }

        private static void PreviousYear(ref (int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            state.Year--;
            var calendarInstance = CalendarTools.GetCalendar(state.calendar);
            int maxDays = calendarInstance.Calendar.GetDaysInMonth(state.Year, state.Month);
            if (state.Day > maxDays)
                state.Day = maxDays;
        }

        private static void NextYear(ref (int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            state.Year++;
            var calendarInstance = CalendarTools.GetCalendar(state.calendar);
            int maxDays = calendarInstance.Calendar.GetDaysInMonth(state.Year, state.Month);
            if (state.Day > maxDays)
                state.Day = maxDays;
        }

        private static string RenderReminders((int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            var builder = new StringBuilder();
            var calendarInstance = CalendarTools.GetCalendar(state.calendar);
            var maxDate = calendarInstance.Calendar.GetDaysInMonth(state.Year, state.Month);
            var selectedDate = new DateTime(state.Year, state.Month, TimeDateTools.KernelDateTime.Day > maxDate ? 1 : TimeDateTools.KernelDateTime.Day);
            var (year, month, _, _) = TimeDateConverters.GetDateFromCalendar(selectedDate, state.calendar);
            var DateTo = new DateTime(year, month, calendarInstance.Calendar.GetDaysInMonth(year, month));
            bool found = false;
            Dictionary<string, List<string>> remindersString = [];
            for (int CurrentDay = 1; CurrentDay <= DateTo.Day; CurrentDay++)
            {
                // Populate some variables
                var CurrentDate = new DateTime(year, month, CurrentDay);

                // Render the reminders
                foreach (ReminderInfo Reminder in ReminderManager.Reminders)
                {
                    var rDate = Reminder.ReminderDate.Date;
                    var (rYear, rMonth, rDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(rDate.Year, rDate.Month, rDate.Day), state.calendar);
                    rDate = new(rYear, rMonth, rDay);
                    if (rDate == CurrentDate)
                    {
                        found = true;
                        string reminderDate = $"{month}/{CurrentDay}/{year}";
                        string reminderTitle = $"[{Reminder.ReminderImportance}] {Reminder.ReminderTitle}";
                        if (remindersString.TryGetValue(reminderDate, out List<string>? reminderList))
                            reminderList.Add(reminderTitle);
                        else
                            remindersString.Add(reminderDate, [reminderTitle]);
                    }
                }
            }
            if (!found)
                builder.AppendLine(Translate.DoTranslation("No reminders found for the selected month"));
            else
            {
                foreach (string date in remindersString.Keys)
                {
                    var reminders = remindersString[date];
                    builder.AppendLine(date);
                    foreach (string reminderName in reminders)
                        builder.AppendLine($"    {reminderName}");
                }
            }
            return builder.ToString();
        }

        private static string RenderEvents((int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            var builder = new StringBuilder();
            var calendarInstance = CalendarTools.GetCalendar(state.calendar);
            var maxDate = calendarInstance.Calendar.GetDaysInMonth(state.Year, state.Month);
            var selectedDate = new DateTime(state.Year, state.Month, TimeDateTools.KernelDateTime.Day > maxDate ? 1 : TimeDateTools.KernelDateTime.Day);
            var (year, month, _, _) = TimeDateConverters.GetDateFromCalendar(selectedDate, state.calendar);
            var DateTo = new DateTime(year, month, calendarInstance.Calendar.GetDaysInMonth(year, month));
            bool found = false;
            Dictionary<string, List<string>> eventsString = [];
            for (int CurrentDay = 1; CurrentDay <= DateTo.Day; CurrentDay++)
            {
                // Populate some variables
                var CurrentDate = new DateTime(year, month, CurrentDay);

                // Render the events
                foreach (EventInfo EventInstance in EventManager.CalendarEvents.Union(EventManager.baseEvents))
                {
                    EventInstance.UpdateEventInfo(new DateTime(state.Year, 1, 1));
                    var nDate = EventInstance.EventDate.Date;
                    var sDate = EventInstance.Start.Date;
                    var eDate = EventInstance.End.Date;
                    var (nYear, nMonth, nDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(nDate.Year, nDate.Month, nDate.Day), state.calendar);
                    var (sYear, sMonth, sDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(sDate.Year, sDate.Month, sDate.Day), state.calendar);
                    var (eYear, eMonth, eDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(eDate.Year, eDate.Month, eDate.Day), state.calendar);
                    nDate = new(nYear, nMonth, nDay);
                    sDate = new(sYear, sMonth, sDay);
                    eDate = new(eYear, eMonth, eDay);
                    if (EventInstance.IsYearly && CurrentDate >= sDate && CurrentDate <= eDate)
                    {
                        found = true;
                        string eventDate = $"{sDate.Month}/{sDate.Day}/{sDate.Year} -> {eDate.Month}/{eDate.Day}/{eDate.Year}";
                        string eventTitle = EventInstance.EventTitle;
                        if (!eventsString.ContainsKey(eventDate))
                            eventsString.Add(eventDate, [eventTitle]);
                    }
                    else if (!EventInstance.IsYearly && CurrentDate == nDate)
                    {
                        found = true;
                        string eventDate = $"{month}/{CurrentDay}/{year}";
                        string eventTitle = EventInstance.EventTitle;
                        if (eventsString.TryGetValue(eventDate, out List<string>? eventList))
                            eventList.Add(eventTitle);
                        else
                            eventsString.Add(eventDate, [eventTitle]);
                    }
                }
            }
            if (!found)
                builder.AppendLine(Translate.DoTranslation("No events found for the selected month"));
            else
            {
                foreach (string date in eventsString.Keys)
                {
                    var events = eventsString[date];
                    builder.AppendLine(date);
                    foreach (string eventName in events)
                        builder.AppendLine($"    {eventName}");
                }
            }
            return builder.ToString();
        }

        private static string RenderRemindersDay((int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            var builder = new StringBuilder();

            // Populate some variables
            bool found = false;
            var CurrentDate = new DateTime(state.Year, state.Month, state.Day);
            List<string> reminderNames = [];

            // Render the reminders
            foreach (ReminderInfo Reminder in ReminderManager.Reminders)
            {
                var rDate = Reminder.ReminderDate.Date;
                var (rYear, rMonth, rDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(rDate.Year, rDate.Month, rDate.Day), state.calendar);
                rDate = new(rYear, rMonth, rDay);
                if (rDate == CurrentDate)
                {
                    found = true;
                    reminderNames.Add($"[{Reminder.ReminderImportance}] {Reminder.ReminderTitle}");
                }
            }
            if (!found)
                builder.AppendLine(Translate.DoTranslation("No reminders found for the selected day"));
            else
            {
                builder.AppendLine($"{state.Month}/{state.Day}/{state.Year}");
                foreach (string reminderName in reminderNames)
                    builder.AppendLine($"    {reminderName}");
            }
            return builder.ToString();
        }

        private static string RenderEventsDay((int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            var builder = new StringBuilder();

            // Populate some variables
            bool found = false;
            var CurrentDate = new DateTime(state.Year, state.Month, state.Day);
            List<string> eventNames = [];

            // Render the events
            foreach (EventInfo EventInstance in EventManager.CalendarEvents.Union(EventManager.baseEvents))
            {
                EventInstance.UpdateEventInfo(new DateTime(state.Year, 1, 1));
                var nDate = EventInstance.EventDate.Date;
                var sDate = EventInstance.Start.Date;
                var eDate = EventInstance.End.Date;
                var (nYear, nMonth, nDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(nDate.Year, nDate.Month, nDate.Day), state.calendar);
                var (sYear, sMonth, sDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(sDate.Year, sDate.Month, sDate.Day), state.calendar);
                var (eYear, eMonth, eDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(eDate.Year, eDate.Month, eDate.Day), state.calendar);
                nDate = new(nYear, nMonth, nDay);
                sDate = new(sYear, sMonth, sDay);
                eDate = new(eYear, eMonth, eDay);
                if ((EventInstance.IsYearly && CurrentDate >= sDate && CurrentDate <= eDate) ||
                    (!EventInstance.IsYearly && CurrentDate == nDate))
                {
                    found = true;
                    eventNames.Add(EventInstance.EventTitle);
                }
            }
            if (!found)
                builder.AppendLine(Translate.DoTranslation("No events found for the selected day"));
            else
            {
                builder.AppendLine($"{state.Month}/{state.Day}/{state.Year}");
                foreach (string eventName in eventNames)
                    builder.AppendLine($"    {eventName}");
            }
            return builder.ToString();
        }
    }
}
