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

using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Themes.Colors;

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Main kernel configuration instance
    /// </summary>
    public partial class KernelMainConfig : BaseKernelConfig
    {
        /// <summary>
        /// Whether to use accent colors for themes that support accents
        /// </summary>
        public bool UseAccentColors
        {
            get => ThemeColorsTools.UseAccentColors;
            set => ThemeColorsTools.UseAccentColors = value;
        }
        /// <summary>
        /// Accent color (foreground)
        /// </summary>
        public string AccentForegroundColor
        {
            get => ThemeColorsTools.AccentForegroundColor;
            set => ThemeColorsTools.AccentForegroundColor = value;
        }
        /// <summary>
        /// Accent color (background)
        /// </summary>
        public string AccentBackgroundColor
        {
            get => ThemeColorsTools.AccentBackgroundColor;
            set => ThemeColorsTools.AccentBackgroundColor = value;
        }
        /// <summary>
        /// Whether to use accent colors for themes that support accents
        /// </summary>
        public bool UseConsoleColorPalette
        {
            get => ColorTools.GlobalSettings.UseTerminalPalette;
            set => ColorTools.GlobalSettings.UseTerminalPalette = value;
        }
        /// <summary>
        /// Whether to allow background color
        /// </summary>
        public bool AllowBackgroundColor
        {
            get => ConsoleColoring.AllowBackground;
            set => ConsoleColoring.AllowBackground = value;
        }
        /// <summary>
        /// User Name Shell Color
        /// </summary>
        public string UserNameShellColor
        {
            get => ThemeColorsTools.GetColor("UserNameShellColor").PlainSequence;
            set => ThemeColorsTools.SetColor("UserNameShellColor", new Color(value));
        }
        /// <summary>
        /// Host Name Shell Color
        /// </summary>
        public string HostNameShellColor
        {
            get => ThemeColorsTools.GetColor("HostNameShellColor").PlainSequence;
            set => ThemeColorsTools.SetColor("HostNameShellColor", new Color(value));
        }
        /// <summary>
        /// Continuable Kernel Error Color
        /// </summary>
        public string ContinuableKernelErrorColor
        {
            get => ThemeColorsTools.GetColor("ContKernelErrorColor").PlainSequence;
            set => ThemeColorsTools.SetColor("ContKernelErrorColor", new Color(value));
        }
        /// <summary>
        /// Uncontinuable Kernel Error Color
        /// </summary>
        public string UncontinuableKernelErrorColor
        {
            get => ThemeColorsTools.GetColor("UncontKernelErrorColor").PlainSequence;
            set => ThemeColorsTools.SetColor("UncontKernelErrorColor", new Color(value));
        }
        /// <summary>
        /// Text Color
        /// </summary>
        public string TextColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.NeutralText).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.NeutralText, new Color(value));
        }
        /// <summary>
        /// License Color
        /// </summary>
        public string LicenseColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.License).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.License, new Color(value));
        }
        /// <summary>
        /// Background Color
        /// </summary>
        public string BackgroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Background).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Background, new Color(value));
        }
        /// <summary>
        /// Input Color
        /// </summary>
        public string InputColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Input).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Input, new Color(value));
        }
        /// <summary>
        /// List Entry Color
        /// </summary>
        public string ListEntryColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.ListEntry).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.ListEntry, new Color(value));
        }
        /// <summary>
        /// List Value Color
        /// </summary>
        public string ListValueColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.ListValue).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.ListValue, new Color(value));
        }
        /// <summary>
        /// Kernel Stage Color
        /// </summary>
        public string KernelStageColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Stage).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Stage, new Color(value));
        }
        /// <summary>
        /// Error Text Color
        /// </summary>
        public string ErrorTextColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Error).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Error, new Color(value));
        }
        /// <summary>
        /// Warning Text Color
        /// </summary>
        public string WarningTextColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Warning).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Warning, new Color(value));
        }
        /// <summary>
        /// Option Color
        /// </summary>
        public string OptionColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Option).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Option, new Color(value));
        }
        /// <summary>
        /// Banner Color
        /// </summary>
        public string BannerColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Banner).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Banner, new Color(value));
        }
        /// <summary>
        /// Notification Title Color
        /// </summary>
        public string NotificationTitleColor
        {
            get => ThemeColorsTools.GetColor("NotificationTitleColor").PlainSequence;
            set => ThemeColorsTools.SetColor("NotificationTitleColor", new Color(value));
        }
        /// <summary>
        /// Notification Description Color
        /// </summary>
        public string NotificationDescriptionColor
        {
            get => ThemeColorsTools.GetColor("NotificationDescriptionColor").PlainSequence;
            set => ThemeColorsTools.SetColor("NotificationDescriptionColor", new Color(value));
        }
        /// <summary>
        /// Notification Progress Color
        /// </summary>
        public string NotificationProgressColor
        {
            get => ThemeColorsTools.GetColor("NotificationProgressColor").PlainSequence;
            set => ThemeColorsTools.SetColor("NotificationProgressColor", new Color(value));
        }
        /// <summary>
        /// Notification Failure Color
        /// </summary>
        public string NotificationFailureColor
        {
            get => ThemeColorsTools.GetColor("NotificationFailureColor").PlainSequence;
            set => ThemeColorsTools.SetColor("NotificationFailureColor", new Color(value));
        }
        /// <summary>
        /// Question Color
        /// </summary>
        public string QuestionColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Question).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Question, new Color(value));
        }
        /// <summary>
        /// Success Color
        /// </summary>
        public string SuccessColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Success).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Success, new Color(value));
        }
        /// <summary>
        /// User Dollar Color
        /// </summary>
        public string UserDollarColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.UserDollar).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.UserDollar, new Color(value));
        }
        /// <summary>
        /// Tip Color
        /// </summary>
        public string TipColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Tip).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Tip, new Color(value));
        }
        /// <summary>
        /// Separator Text Color
        /// </summary>
        public string SeparatorTextColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.SeparatorText).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.SeparatorText, new Color(value));
        }
        /// <summary>
        /// Separator Color
        /// </summary>
        public string SeparatorColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Separator).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Separator, new Color(value));
        }
        /// <summary>
        /// List Title Color
        /// </summary>
        public string ListTitleColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.ListTitle).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.ListTitle, new Color(value));
        }
        /// <summary>
        /// Development Warning Color
        /// </summary>
        public string DevelopmentWarningColor
        {
            get => ThemeColorsTools.GetColor("DevelopmentWarningColor").PlainSequence;
            set => ThemeColorsTools.SetColor("DevelopmentWarningColor", new Color(value));
        }
        /// <summary>
        /// Stage Time Color
        /// </summary>
        public string StageTimeColor
        {
            get => ThemeColorsTools.GetColor("StageTimeColor").PlainSequence;
            set => ThemeColorsTools.SetColor("StageTimeColor", new Color(value));
        }
        /// <summary>
        /// Progress Color
        /// </summary>
        public string ProgressColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.Progress).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.Progress, new Color(value));
        }
        /// <summary>
        /// Back Option Color
        /// </summary>
        public string BackOptionColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.BackOption).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.BackOption, new Color(value));
        }
        /// <summary>
        /// Low Priority Border Color
        /// </summary>
        public string LowPriorityBorderColor
        {
            get => ThemeColorsTools.GetColor("LowPriorityBorderColor").PlainSequence;
            set => ThemeColorsTools.SetColor("LowPriorityBorderColor", new Color(value));
        }
        /// <summary>
        /// Medium Priority Border Color
        /// </summary>
        public string MediumPriorityBorderColor
        {
            get => ThemeColorsTools.GetColor("MediumPriorityBorderColor").PlainSequence;
            set => ThemeColorsTools.SetColor("MediumPriorityBorderColor", new Color(value));
        }
        /// <summary>
        /// High Priority Border Color
        /// </summary>
        public string HighPriorityBorderColor
        {
            get => ThemeColorsTools.GetColor("HighPriorityBorderColor").PlainSequence;
            set => ThemeColorsTools.SetColor("HighPriorityBorderColor", new Color(value));
        }
        /// <summary>
        /// Table Separator Color
        /// </summary>
        public string TableSeparatorColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TableSeparator).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TableSeparator, new Color(value));
        }
        /// <summary>
        /// Table Header Color
        /// </summary>
        public string TableHeaderColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TableHeader).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TableHeader, new Color(value));
        }
        /// <summary>
        /// Table Value Color
        /// </summary>
        public string TableValueColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TableValue).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TableValue, new Color(value));
        }
        /// <summary>
        /// Selected Option Color
        /// </summary>
        public string SelectedOptionColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.SelectedOption).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.SelectedOption, new Color(value));
        }
        /// <summary>
        /// Alternative Option Color
        /// </summary>
        public string AlternativeOptionColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.AlternativeOption).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.AlternativeOption, new Color(value));
        }
        /// <summary>
        /// Weekend Day Color
        /// </summary>
        public string WeekendDayColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.WeekendDay).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.WeekendDay, new Color(value));
        }
        /// <summary>
        /// Event Day Color
        /// </summary>
        public string EventDayColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.EventDay).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.EventDay, new Color(value));
        }
        /// <summary>
        /// Table Title Color
        /// </summary>
        public string TableTitleColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TableTitle).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TableTitle, new Color(value));
        }
        /// <summary>
        /// Today Day Color
        /// </summary>
        public string TodayDayColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TodayDay).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TodayDay, new Color(value));
        }
        /// <summary>
        /// Interactive TUI background color
        /// </summary>
        public string TuiBackgroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiBackground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiBackground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI foreground color
        /// </summary>
        public string TuiForegroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiForeground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiForeground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI pane background color
        /// </summary>
        public string TuiPaneBackgroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiPaneBackground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiPaneBackground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI pane separator color
        /// </summary>
        public string TuiPaneSeparatorColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSeparator).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiPaneSeparator, new Color(value));
        }
        /// <summary>
        /// Interactive TUI selected pane separator color
        /// </summary>
        public string TuiPaneSelectedSeparatorColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiPaneSelectedSeparator, new Color(value));
        }
        /// <summary>
        /// Interactive TUI selected pane item foreground color
        /// </summary>
        public string TuiPaneSelectedItemForeColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedItemFore).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiPaneSelectedItemFore, new Color(value));
        }
        /// <summary>
        /// Interactive TUI selected pane item background color
        /// </summary>
        public string TuiPaneSelectedItemBackColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedItemBack).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiPaneSelectedItemBack, new Color(value));
        }
        /// <summary>
        /// Interactive TUI pane item foreground color
        /// </summary>
        public string TuiPaneItemForeColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiPaneItemFore).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiPaneItemFore, new Color(value));
        }
        /// <summary>
        /// Interactive TUI pane item background color
        /// </summary>
        public string TuiPaneItemBackColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiPaneItemBack).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiPaneItemBack, new Color(value));
        }
        /// <summary>
        /// Interactive TUI option background color
        /// </summary>
        public string TuiOptionBackgroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiOptionBackground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiOptionBackground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI option foreground color
        /// </summary>
        public string TuiOptionForegroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiOptionForeground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiOptionForeground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI option binding name color
        /// </summary>
        public string TuiKeyBindingOptionColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingOption).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiKeyBindingOption, new Color(value));
        }
        /// <summary>
        /// Interactive TUI box background color
        /// </summary>
        public string TuiBoxBackgroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiBoxBackground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiBoxBackground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI box foreground color
        /// </summary>
        public string TuiBoxForegroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiBoxForeground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiBoxForeground, new Color(value));
        }
        /// <summary>
        /// Disabled option color
        /// </summary>
        public string DisabledOptionColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.DisabledOption).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.DisabledOption, new Color(value));
        }
        /// <summary>
        /// Interactive TUI builtin key binding background color
        /// </summary>
        public string TuiKeyBindingBuiltinBackgroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltinBackground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiKeyBindingBuiltinBackground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI builtin key binding foreground color
        /// </summary>
        public string TuiKeyBindingBuiltinForegroundColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltinForeground).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiKeyBindingBuiltinForeground, new Color(value));
        }
        /// <summary>
        /// Interactive TUI builtin key binding color
        /// </summary>
        public string TuiKeyBindingBuiltinColor
        {
            get => ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltin).PlainSequence;
            set => ThemeColorsTools.SetColor(ThemeColorType.TuiKeyBindingBuiltin, new Color(value));
        }
    }
}
