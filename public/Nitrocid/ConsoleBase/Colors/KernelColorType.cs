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

namespace Nitrocid.ConsoleBase.Colors
{
    /// <summary>
    /// Enumeration for color types
    /// </summary>
    public enum KernelColorType
    {
        /// <summary>
        /// Input text
        /// </summary>
        Input,
        /// <summary>
        /// License color
        /// </summary>
        License,
        /// <summary>
        /// Continuable kernel panic text (usually sync'd with Warning)
        /// </summary>
        ContKernelError,
        /// <summary>
        /// Uncontinuable kernel panic text (usually sync'd with Error)
        /// </summary>
        UncontKernelError,
        /// <summary>
        /// Host name color
        /// </summary>
        HostNameShell,
        /// <summary>
        /// User name color
        /// </summary>
        UserNameShell,
        /// <summary>
        /// Background color
        /// </summary>
        Background,
        /// <summary>
        /// Neutral text (for general purposes)
        /// </summary>
        NeutralText,
        /// <summary>
        /// List entry text
        /// </summary>
        ListEntry,
        /// <summary>
        /// List value text
        /// </summary>
        ListValue,
        /// <summary>
        /// Stage text
        /// </summary>
        Stage,
        /// <summary>
        /// Error text
        /// </summary>
        Error,
        /// <summary>
        /// Warning text
        /// </summary>
        Warning,
        /// <summary>
        /// Option text
        /// </summary>
        Option,
        /// <summary>
        /// Banner text
        /// </summary>
        Banner,
        /// <summary>
        /// Notification title text
        /// </summary>
        NotificationTitle,
        /// <summary>
        /// Notification description text
        /// </summary>
        NotificationDescription,
        /// <summary>
        /// Notification progress text
        /// </summary>
        NotificationProgress,
        /// <summary>
        /// Notification failure text
        /// </summary>
        NotificationFailure,
        /// <summary>
        /// Question text
        /// </summary>
        Question,
        /// <summary>
        /// Success text
        /// </summary>
        Success,
        /// <summary>
        /// User dollar sign on shell text
        /// </summary>
        UserDollar,
        /// <summary>
        /// Tip text
        /// </summary>
        Tip,
        /// <summary>
        /// Separator text
        /// </summary>
        SeparatorText,
        /// <summary>
        /// Separator color
        /// </summary>
        Separator,
        /// <summary>
        /// List title text
        /// </summary>
        ListTitle,
        /// <summary>
        /// Development warning text
        /// </summary>
        DevelopmentWarning,
        /// <summary>
        /// Stage time text
        /// </summary>
        StageTime,
        /// <summary>
        /// General progress text
        /// </summary>
        Progress,
        /// <summary>
        /// Back option text
        /// </summary>
        BackOption,
        /// <summary>
        /// Low priority notification border color
        /// </summary>
        LowPriorityBorder,
        /// <summary>
        /// Medium priority notification border color
        /// </summary>
        MediumPriorityBorder,
        /// <summary>
        /// High priority notification border color
        /// </summary>
        HighPriorityBorder,
        /// <summary>
        /// Table separator
        /// </summary>
        TableSeparator,
        /// <summary>
        /// Table header
        /// </summary>
        TableHeader,
        /// <summary>
        /// Table value
        /// </summary>
        TableValue,
        /// <summary>
        /// Selected option
        /// </summary>
        SelectedOption,
        /// <summary>
        /// Alternative option
        /// </summary>
        AlternativeOption,
        /// <summary>
        /// Weekend day
        /// </summary>
        WeekendDay,
        /// <summary>
        /// Event day
        /// </summary>
        EventDay,
        /// <summary>
        /// Table title
        /// </summary>
        TableTitle,
        /// <summary>
        /// Today
        /// </summary>
        TodayDay,
        /// <summary>
        /// Interactive TUI background color
        /// </summary>
        TuiBackground,
        /// <summary>
        /// Interactive TUI foreground color
        /// </summary>
        TuiForeground,
        /// <summary>
        /// Interactive TUI pane background color
        /// </summary>
        TuiPaneBackground,
        /// <summary>
        /// Interactive TUI pane separator color
        /// </summary>
        TuiPaneSeparator,
        /// <summary>
        /// Interactive TUI pane selected separator color
        /// </summary>
        TuiPaneSelectedSeparator,
        /// <summary>
        /// Interactive TUI pane selected item color (foreground)
        /// </summary>
        TuiPaneSelectedItemFore,
        /// <summary>
        /// Interactive TUI pane selected item color (background)
        /// </summary>
        TuiPaneSelectedItemBack,
        /// <summary>
        /// Interactive TUI pane item color (foreground)
        /// </summary>
        TuiPaneItemFore,
        /// <summary>
        /// Interactive TUI pane item color (background)
        /// </summary>
        TuiPaneItemBack,
        /// <summary>
        /// Interactive TUI option background color
        /// </summary>
        TuiOptionBackground,
        /// <summary>
        /// Interactive TUI key binding in option color
        /// </summary>
        TuiKeyBindingOption,
        /// <summary>
        /// Interactive TUI option foreground color
        /// </summary>
        TuiOptionForeground,
        /// <summary>
        /// Interactive TUI box background color
        /// </summary>
        TuiBoxBackground,
        /// <summary>
        /// Interactive TUI box foreground color
        /// </summary>
        TuiBoxForeground,
        /// <summary>
        /// Disabled option
        /// </summary>
        DisabledOption,
        /// <summary>
        /// Interactive TUI builtin key binding background color
        /// </summary>
        TuiKeyBindingBuiltinBackground,
        /// <summary>
        /// Interactive TUI builtin key binding color
        /// </summary>
        TuiKeyBindingBuiltin,
        /// <summary>
        /// Interactive TUI builtin key binding foreground color
        /// </summary>
        TuiKeyBindingBuiltinForeground,
    }
}
