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

using Nitrocid.Extras.Calendar.Calendar.Events;
using Nitrocid.Extras.Calendar.Calendar.Reminders;
using System;
using System.Linq;
using System.Text;
using Textify.Sequences.Builder.Types;
using Textify.Sequences.Tools;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Time.Calendars;
using Nitrocid.ConsoleBase;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Base.Buffered;
using Terminaux.Inputs;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Interactive;
using Nitrocid.ConsoleBase.Writers.FancyWriters;
using Nitrocid.Kernel.Time.Converters;
using Nitrocid.Kernel.Time;
using Textify.General;
using Terminaux.Colors;

namespace Nitrocid.Extras.Calendar.Calendar
{
    internal static class CalendarTui
    {
        private static string status;
        private static bool bail;
        private static (int Year, int Month, int Day, CalendarTypes calendar) state;
        private static readonly CalendarTuiBinding[] bindings =
        [
            new CalendarTuiBinding( /* Localizable */ "Exit", ConsoleKey.Escape, default, (b) => { bail = true; return b; }, true),
            new CalendarTuiBinding( /* Localizable */ "Keybindings", ConsoleKey.K, default, RenderKeybindingsBox, true),
            new CalendarTuiBinding( /* Localizable */ "Events...", ConsoleKey.E, default, ListEvents, true),
            new CalendarTuiBinding( /* Localizable */ "Reminders...", ConsoleKey.R, default, ListReminders, true),
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
        internal static void OpenInteractive(int Year, int Month, int Day, CalendarTypes calendar = CalendarTypes.Gregorian)
        {
            // Set status
            state = (Year, Month, Day, calendar);
            status = Translate.DoTranslation("Ready");
            bail = false;

            // Main loop
            var screen = new Screen();
            ScreenTools.SetCurrent(screen);
            ConsoleWrapper.CursorVisible = false;
            ColorTools.LoadBack();
            try
            {
                while (!bail)
                {
                    // Now, render the keybindings
                    RenderKeybindings(ref screen);

                    // Render the box
                    RenderHexViewBox(ref screen);

                    // Now, render the visual hex with the current selection
                    RenderCalendar(state, ref screen);

                    // Render the status
                    RenderStatus(ref screen);

                    // Wait for a keypress
                    ScreenTools.Render(screen);
                    var keypress = Input.DetectKeypress();
                    HandleKeypress(keypress, ref state);

                    // Reset, in case selection changed
                    screen.RemoveBufferedParts();
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Interactive calendar failed: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                InfoBoxColor.WriteInfoBoxColor(Translate.DoTranslation("The interactive calendar failed:") + $" {ex.Message}", KernelColorTools.GetColor(KernelColorType.Error));
            }
            bail = false;
            ScreenTools.UnsetCurrent(screen);

            // Close the file and clean up
            ColorTools.LoadBack();
        }

        private static void RenderKeybindings(ref Screen screen)
        {
            // Make a screen part
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var bindingsBuilder = new StringBuilder(CsiSequences.GenerateCsiCursorPosition(1, ConsoleWrapper.WindowHeight));
                foreach (CalendarTuiBinding binding in bindings)
                {
                    // First, check to see if the rendered binding info is going to exceed the console window width
                    string renderedBinding = $"{GetBindingKeyShortcut(binding, false)} {(binding._localizable ? Translate.DoTranslation(binding.Name) : binding.Name)}  ";
                    int actualLength = VtSequenceTools.FilterVTSequences(bindingsBuilder.ToString()).Length;
                    bool canDraw = renderedBinding.Length + actualLength < ConsoleWrapper.WindowWidth - 3;
                    if (canDraw)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Drawing binding {0} with description {1}...", GetBindingKeyShortcut(binding, false), binding.Name);
                        bindingsBuilder.Append(
                            $"{BaseInteractiveTui.KeyBindingOptionColor.VTSequenceForeground}" +
                            $"{BaseInteractiveTui.OptionBackgroundColor.VTSequenceBackground}" +
                            GetBindingKeyShortcut(binding, false) +
                            $"{BaseInteractiveTui.OptionForegroundColor.VTSequenceForeground}" +
                            $"{BaseInteractiveTui.BackgroundColor.VTSequenceBackground}" +
                            $" {(binding._localizable ? Translate.DoTranslation(binding.Name) : binding.Name)}  "
                        );
                    }
                    else
                    {
                        // We can't render anymore, so just break and write a binding to show more
                        DebugWriter.WriteDebug(DebugLevel.I, "Bailing because of no space...");
                        bindingsBuilder.Append(
                            $"{CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth - 2, ConsoleWrapper.WindowHeight)}" +
                            $"{BaseInteractiveTui.KeyBindingOptionColor.VTSequenceForeground}" +
                            $"{BaseInteractiveTui.OptionBackgroundColor.VTSequenceBackground}" +
                            " K "
                        );
                        break;
                    }
                }
                return bindingsBuilder.ToString();
            });
            screen.AddBufferedPart("Interactive calendar interactive - Keybindings", part);
        }

        private static void RenderStatus(ref Screen screen)
        {
            // Make a screen part
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var builder = new StringBuilder();
                builder.Append(
                    $"{BaseInteractiveTui.ForegroundColor.VTSequenceForeground}" +
                    $"{KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground}" +
                    $"{TextWriterWhereColor.RenderWherePlain(status + ConsoleExtensions.GetClearLineToRightSequence(), 0, 0)}"
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Interactive calendar interactive - Status", part);
        }

        private static void RenderHexViewBox(ref Screen screen)
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
                builder.Append(
                    $"{BaseInteractiveTui.PaneSeparatorColor.VTSequenceForeground}" +
                    $"{KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground}" +
                    $"{BorderColor.RenderBorderPlain(0, SeparatorMinimumHeight, SeparatorConsoleWidthInterior, SeparatorMaximumHeightInterior)}"
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Interactive calendar interactive - Hex view box", part);
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
                var CalendarDays = calendarInstance.Culture.DateTimeFormat.DayNames;
                var CalendarMonths = calendarInstance.Culture.DateTimeFormat.MonthNames;
                var maxDate = calendarInstance.Calendar.GetDaysInMonth(state.Year, state.Month);
                var selectedDate = new DateTime(state.Year, state.Month, TimeDateTools.KernelDateTime.Day > maxDate ? 1 : TimeDateTools.KernelDateTime.Day);
                var (year, month, _, _) = TimeDateConverters.GetDateFromCalendar(selectedDate, state.calendar);
                var DateTo = new DateTime(year, month, calendarInstance.Calendar.GetDaysInMonth(year, month));
                int CurrentWeek = 1;
                string CalendarTitle = CalendarMonths[month - 1] + " " + year;

                // Write the calendar title in a box
                var boxForeground = KernelColorTools.GetColor(KernelColorType.NeutralText);
                var background = KernelColorTools.GetColor(KernelColorType.Background);
                int boxLeft = 4;
                int boxTop = 3;
                int boxWidth = 4 + (6 * 6);
                int boxHeight = 11;
                builder.Append(
                    BorderTextColor.RenderBorderText(CalendarTitle, boxLeft, boxTop, boxWidth, boxHeight, boxForeground, background)
                );

                // Make a calendar
                int dayPosX = boxLeft + 1;
                int dayPosY = 4;
                for (int CurrentDay = 1; CurrentDay <= DateTo.Day; CurrentDay++)
                {
                    // Populate some variables
                    var CurrentDate = new DateTime(year, month, CurrentDay);
                    if (CurrentDate.DayOfWeek == 0)
                    {
                        CurrentWeek += 1;
                        dayPosY += 2;
                    }
                    int CurrentWeekIndex = CurrentWeek - 1;
                    int currentDay = (int)CurrentDate.DayOfWeek + 1;
                    dayPosX = boxLeft + 1 + (6 * (currentDay - 1));
                    string CurrentDayMark;

                    // Some flags
                    bool ReminderMarked = false;
                    bool EventMarked = false;
                    bool IsWeekend = CurrentDate.DayOfWeek == DayOfWeek.Friday || CurrentDate.DayOfWeek == DayOfWeek.Saturday;
                    bool IsToday = CurrentDate == new DateTime(state.Year, state.Month, state.Day);
                    var foreground =
                        IsToday ? KernelColorTools.GetColor(KernelColorType.TodayDay) :
                        IsWeekend ? KernelColorTools.GetColor(KernelColorType.WeekendDay) :
                        BaseInteractiveTui.ForegroundColor;

                    // Know where and how to put the day number
                    foreach (ReminderInfo Reminder in ReminderManager.Reminders)
                    {
                        var rDate = Reminder.ReminderDate.Date;
                        var (rYear, rMonth, rDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(rDate.Year, rDate.Month, rDate.Day), state.calendar);
                        rDate = new(rYear, rMonth, rDay);
                        if (rDate == CurrentDate & !ReminderMarked)
                            ReminderMarked = true;
                    }
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
                        if (((EventInstance.IsYearly && CurrentDate >= sDate && CurrentDate <= eDate) ||
                             (!EventInstance.IsYearly && CurrentDate == nDate)) && !EventMarked)
                        {
                            foreground = KernelColorTools.GetColor(KernelColorType.EventDay);
                            EventMarked = true;
                        }
                    }
                    string markStart = ReminderMarked && EventMarked ? "[" : ReminderMarked ? "(" : EventMarked ? "<" : " ";
                    string markEnd = ReminderMarked && EventMarked ? "]" : ReminderMarked ? ")" : EventMarked ? ">" : " ";
                    CurrentDayMark = $"{markStart}{CurrentDay}{markEnd}";
                    builder.Append(
                        CsiSequences.GenerateCsiCursorPosition(dayPosX + 1, dayPosY + 1) +
                        $"{foreground.VTSequenceForeground}" +
                        $"{background.VTSequenceBackground}" +
                        CurrentDayMark
                    );
                }

                // Write the side box representing the list of events
                int eventBoxLeft = 4 + (6 * 7) + 2;
                int eventBoxTop = 3;
                int eventBoxWidth = ConsoleWrapper.WindowWidth - eventBoxLeft - 6;
                int eventBoxHeight = ConsoleWrapper.WindowHeight - 8;
                builder.Append(
                    BorderTextColor.RenderBorderText(Translate.DoTranslation("Events and reminders for") + $" {CalendarTitle}", eventBoxLeft, eventBoxTop, eventBoxWidth, eventBoxHeight, boxForeground, background)
                );

                // List all the events if they don't overflow
                int eventEntryTop = 4;
                int eventEntryLeft = eventBoxLeft + 1;
                for (int CurrentDay = 1; CurrentDay <= DateTo.Day; CurrentDay++)
                {
                    eventEntryLeft = eventBoxLeft + 1;
                    if (eventEntryTop - 1 >= eventBoxHeight + eventBoxTop)
                        break;

                    // Populate some variables
                    var CurrentDate = new DateTime(year, month, CurrentDay);
                    int CurrentWeekIndex = CurrentWeek - 1;
                    builder.Append(
                        TextWriterWhereColor.RenderWhere($"{month}/{CurrentDay}/{year}", eventEntryLeft, eventEntryTop, boxForeground, background)
                    );
                    eventEntryLeft += 2;
                    eventEntryTop++;

                    // Some flags
                    bool ReminderMarked = false;
                    bool EventMarked = false;
                    bool IsWeekend = CurrentDate.DayOfWeek == DayOfWeek.Friday || CurrentDate.DayOfWeek == DayOfWeek.Saturday;
                    bool IsToday = CurrentDate == new DateTime(state.Year, state.Month, state.Day);
                    var foreground =
                        IsToday ? KernelColorTools.GetColor(KernelColorType.TodayDay) :
                        IsWeekend ? KernelColorTools.GetColor(KernelColorType.WeekendDay) :
                        BaseInteractiveTui.ForegroundColor;

                    // Know where and how to put the reminders and events
                    foreach (ReminderInfo Reminder in ReminderManager.Reminders)
                    {
                        var rDate = Reminder.ReminderDate.Date;
                        var (rYear, rMonth, rDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(rDate.Year, rDate.Month, rDate.Day), state.calendar);
                        rDate = new(rYear, rMonth, rDay);
                        if (rDate == CurrentDate & !ReminderMarked)
                        {
                            ReminderMarked = true;
                            eventEntryTop++;
                            if (eventEntryTop - 1 > eventBoxHeight + eventBoxTop)
                                break;
                            builder.Append(
                                TextWriterWhereColor.RenderWhere(Reminder.ReminderTitle.Truncate(eventBoxWidth - 5), eventEntryLeft, eventEntryTop - 1, boxForeground, background)
                            );
                        }
                    }
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
                        if (((EventInstance.IsYearly && CurrentDate >= sDate && CurrentDate <= eDate) ||
                             (!EventInstance.IsYearly && CurrentDate == nDate)) && !EventMarked)
                        {
                            foreground = KernelColorTools.GetColor(KernelColorType.EventDay);
                            EventMarked = true;
                            eventEntryTop++;
                            if (eventEntryTop - 1 > eventBoxHeight + eventBoxTop)
                                break;
                            builder.Append(
                                TextWriterWhereColor.RenderWhere(EventInstance.EventTitle.Truncate(eventBoxWidth - 5), eventEntryLeft, eventEntryTop - 1, boxForeground, background)
                            );
                        }
                    }
                }

                // Finalize everything
                builder.Append(
                    $"{BaseInteractiveTui.ForegroundColor.VTSequenceForeground}" +
                    $"{background.VTSequenceBackground}"
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Interactive calendar interactive - Contents", part);
        }

        private static void StatusNumInfo((int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            // Change the status to reflect the selected day
            status =
                $"MM/DD/YYYY: {state.Month}/{state.Day}/{state.Year} | {state.calendar}";
        }

        private static void HandleKeypress(ConsoleKeyInfo key, ref (int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            // Check to see if we have this binding
            if (!bindings.Any((heb) => heb.Key == key.Key && heb.KeyModifiers == key.Modifiers))
            {
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        PreviousDay(ref state);
                        return;
                    case ConsoleKey.RightArrow:
                        NextDay(ref state);
                        return;
                    case ConsoleKey.UpArrow:
                        PreviousWeek(ref state);
                        return;
                    case ConsoleKey.DownArrow:
                        NextWeek(ref state);
                        return;
                    case ConsoleKey.PageUp:
                        PreviousMonth(ref state);
                        return;
                    case ConsoleKey.PageDown:
                        NextMonth(ref state);
                        return;
                    case ConsoleKey.Home:
                        PreviousYear(ref state);
                        return;
                    case ConsoleKey.End:
                        NextYear(ref state);
                        return;
                }
                return;
            }

            // Now, get the first binding and execute it.
            var bind = bindings
                .First((heb) => heb.Key == key.Key && heb.KeyModifiers == key.Modifiers);
            state = bind.Action(state);
        }

        private static (int Year, int Month, int Day, CalendarTypes calendar) RenderKeybindingsBox((int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            // Show the available keys list
            if (bindings.Length == 0)
                return state;

            // User needs an infobox that shows all available keys
            string section = Translate.DoTranslation("Available keys");
            int maxBindingLength = bindings
                .Max((heb) => GetBindingKeyShortcut(heb).Length);
            string[] bindingRepresentations = bindings
                .Select((heb) => $"{GetBindingKeyShortcut(heb) + new string(' ', maxBindingLength - GetBindingKeyShortcut(heb).Length) + $" | {(heb._localizable ? Translate.DoTranslation(heb.Name) : heb.Name)}"}")
                .ToArray();
            InfoBoxColor.WriteInfoBoxColorBack(
                $"{section}{CharManager.NewLine}" +
                $"{new string('=', section.Length)}{CharManager.NewLine}{CharManager.NewLine}" +
                $"{string.Join('\n', bindingRepresentations)}"
            , BaseInteractiveTui.BoxForegroundColor, BaseInteractiveTui.BoxBackgroundColor);
            return state;
        }

        private static string GetBindingKeyShortcut(CalendarTuiBinding bind, bool mark = true)
        {
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(bind.KeyModifiers != 0 ? $"{bind.KeyModifiers} + " : "")}{bind.Key}{markEnd}";
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

        private static (int Year, int Month, int Day, CalendarTypes calendar) ListReminders((int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            var builder = new StringBuilder();
            var calendarInstance = CalendarTools.GetCalendar(state.calendar);
            var maxDate = calendarInstance.Calendar.GetDaysInMonth(state.Year, state.Month);
            var selectedDate = new DateTime(state.Year, state.Month, TimeDateTools.KernelDateTime.Day > maxDate ? 1 : TimeDateTools.KernelDateTime.Day);
            var (year, month, _, _) = TimeDateConverters.GetDateFromCalendar(selectedDate, state.calendar);
            var DateTo = new DateTime(year, month, calendarInstance.Calendar.GetDaysInMonth(year, month));
            for (int CurrentDay = 1; CurrentDay <= DateTo.Day; CurrentDay++)
            {
                // Populate some variables
                var CurrentDate = new DateTime(year, month, CurrentDay);
                builder.AppendLine($"{month}/{CurrentDay}/{year}");

                // Render the reminders
                foreach (ReminderInfo Reminder in ReminderManager.Reminders)
                {
                    var rDate = Reminder.ReminderDate.Date;
                    var (rYear, rMonth, rDay, _) = TimeDateConverters.GetDateFromCalendar(new DateTime(rDate.Year, rDate.Month, rDate.Day), state.calendar);
                    rDate = new(rYear, rMonth, rDay);
                    if (rDate == CurrentDate)
                        builder.AppendLine($"  {Reminder.ReminderTitle}");
                }
            }
            InfoBoxColor.WriteInfoBox(builder.ToString());
            return state;
        }

        private static (int Year, int Month, int Day, CalendarTypes calendar) ListEvents((int Year, int Month, int Day, CalendarTypes calendar) state)
        {
            var builder = new StringBuilder();
            var calendarInstance = CalendarTools.GetCalendar(state.calendar);
            var maxDate = calendarInstance.Calendar.GetDaysInMonth(state.Year, state.Month);
            var selectedDate = new DateTime(state.Year, state.Month, TimeDateTools.KernelDateTime.Day > maxDate ? 1 : TimeDateTools.KernelDateTime.Day);
            var (year, month, _, _) = TimeDateConverters.GetDateFromCalendar(selectedDate, state.calendar);
            var DateTo = new DateTime(year, month, calendarInstance.Calendar.GetDaysInMonth(year, month));
            for (int CurrentDay = 1; CurrentDay <= DateTo.Day; CurrentDay++)
            {
                // Populate some variables
                var CurrentDate = new DateTime(year, month, CurrentDay);
                builder.AppendLine($"{month}/{CurrentDay}/{year}");

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
                    if (((EventInstance.IsYearly && CurrentDate >= sDate && CurrentDate <= eDate) ||
                         (!EventInstance.IsYearly && CurrentDate == nDate)))
                        builder.AppendLine($"  {EventInstance.EventTitle}");
                }
            }
            InfoBoxColor.WriteInfoBox(builder.ToString());
            return state;
        }
    }
}
