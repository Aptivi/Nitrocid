
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using ColorSeq;
using ColorSeq.Accessibility;
using Extensification.StringExts;
using FluentFTP;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Files.Folders;
using KS.Files.Querying;
using KS.Kernel.Debugging.RemoteDebug;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Calendar;
using KS.Misc.Games;
using KS.Misc.Notifications;
using KS.Misc.Screensaver;
using KS.Misc.Timers;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.Misc.Writers.MiscWriters;
using KS.Network.Base;
using KS.Network.RPC;
using KS.Shell.Prompts;
using KS.Shell.Shells.FTP;
using KS.Shell.Shells.Hex;
using KS.Shell.Shells.Mail;
using KS.Shell.Shells.RSS;
using KS.Shell.Shells.Text;
using KS.Shell.ShellBase.Shells;
using ManagedWeatherMap.Core;
using MimeKit.Text;
using Newtonsoft.Json;
using System;
using TermRead.Reader;
using static KS.ConsoleBase.Inputs.Styles.ChoiceStyle;
using static KS.Misc.Games.SpeedPress;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Kernel.Configuration.Instances
{
    /// <summary>
    /// Main kernel configuration instance
    /// </summary>
    public class KernelMainConfig
    {
        /// <summary>
        /// Triggers maintenance mode. This disables multiple accounts.
        /// </summary>
        public bool Maintenance { get; set; }
        /// <summary>
        /// Each startup, it will check for updates.
        /// </summary>
        public bool CheckUpdateStart { get; set; } = true;
        /// <summary>
        /// If specified, it will display customized startup banner with placeholder support. You can use {0} for kernel version.
        /// </summary>
        public string CustomBanner
        {
            get => WelcomeMessage.GetCustomBanner();
            set => WelcomeMessage.customBanner = value;
        }
        /// <summary>
        /// When switching languages, change the month names, calendar, etc.
        /// </summary>
        public bool LangChangeCulture { get; set; }
        /// <summary>
        /// Specifies the kernel language.
        /// </summary>
        public string CurrentLanguage
        {
            get => LanguageManager.currentLanguage is not null ? LanguageManager.currentLanguage.ThreeLetterLanguageName : "eng";
            set => LanguageManager.SetLangDry(value);
        }
        /// <summary>
        /// Which variant of the current language is being used to change the month names, calendar, etc.?
        /// </summary>
        public string CurrentCultStr { get; set; } = "en-US";
        /// <summary>
        /// Shows brief information about the application on boot.
        /// </summary>
        public bool ShowAppInfoOnBoot { get; set; } = true;
        /// <summary>
        /// Shows how much time did the kernel take to finish a stage.
        /// </summary>
        public bool ShowStageFinishTimes { get; set; }
        /// <summary>
        /// Automatically start the kernel modifications on boot.
        /// </summary>
        public bool StartKernelMods { get; set; } = true;
        /// <summary>
        /// Shows the current time, time zone, and date before logging in.
        /// </summary>
        public bool ShowCurrentTimeBeforeLogin { get; set; } = true;
        /// <summary>
        /// If there is a minor fault during kernel boot, notifies the user about it.
        /// </summary>
        public bool NotifyFaultsBoot { get; set; } = true;
        /// <summary>
        /// If there is any kernel error, choose whether or not to print the stack trace to the console.
        /// </summary>
        public bool ShowStackTraceOnKernelError { get; set; }
        /// <summary>
        /// If there is any update, the kernel will automatically download it.
        /// </summary>
        public bool AutoDownloadUpdate { get; set; } = true;
        /// <summary>
        /// Enables debugging for the kernel event system
        /// </summary>
        public bool EventDebug { get; set; }
        /// <summary>
        /// Enables the stylish splash screen on startup. Please note that it will disable argument prompt and test shell pre-boot.
        /// </summary>
        public bool EnableSplash { get; set; } = true;
        /// <summary>
        /// Splash name from the available splashes implemented in the kernel.
        /// </summary>
        public string SplashName { get; set; } = "Dots";
        /// <summary>
        /// Write a figlet font that is supported by the Figgle library. Consult the library documentation for more information
        /// </summary>
        public string BannerFigletFont
        {
            get => KernelTools.bannerFigletFont;
            set => KernelTools.bannerFigletFont = FigletTools.FigletFonts.ContainsKey(value) ? value : "Banner";
        }
        /// <summary>
        /// Whether to simulate a situation where there is no APM available. If enabled, it informs the user that it's now safe to turn off the computer upon shutdown.
        /// </summary>
        public bool SimulateNoAPM { get; set; }
        /// <summary>
        /// If you want to set the background color to your favorite terminal emulator color, set it to false. Otherwise, Nitrocid KS will set its own background colors.
        /// </summary>
        public bool SetBackground { get; set; } = true;
        /// <summary>
        /// If you are color blind or if you want to simulate color blindness, then you can enable it.
        /// </summary>
        public bool ColorBlind
        {
            get => ColorSeq.ColorTools.EnableColorTransformation;
            set => ColorSeq.ColorTools.EnableColorTransformation = value;
        }
        /// <summary>
        /// The type of color blindness, whether it's protan, deuter, or tritan.
        /// </summary>
        public int BlindnessDeficiency
        {
            get => (int)ColorSeq.ColorTools.ColorDeficiency;
            set => ColorSeq.ColorTools.ColorDeficiency = (Deficiency)value;
        }
        /// <summary>
        /// How severe is the color blindness?
        /// </summary>
        public double BlindnessSeverity
        {
            get => ColorSeq.ColorTools.ColorDeficiencySeverity;
            set => ColorSeq.ColorTools.ColorDeficiencySeverity = value;
        }
        /// <summary>
        /// Enables beeping upon shutting down the kernel.
        /// </summary>
        public bool BeepOnShutdown { get; set; }
        /// <summary>
        /// Enables delaying upon shutting down the kernel.
        /// </summary>
        public bool DelayOnShutdown { get; set; }
        /// <summary>
        /// If you want the simulation of color blindness to be simple, enable this. Please note that the formula used in this simulation method may not be accurate.
        /// </summary>
        public bool ColorBlindSimple
        {
            get => ColorSeq.ColorTools.EnableSimpleColorTransformation;
            set => ColorSeq.ColorTools.EnableSimpleColorTransformation = value;
        }
        /// <summary>
        /// If you are sure that the console supports true color, or if you want to change your terminal to a terminal that supports true color, change this value.
        /// </summary>
        public bool ConsoleSupportsTrueColor { get; set; } = true;
        /// <summary>
        /// The type of alternative calendar.
        /// </summary>
        public int AltCalendar { get; set; } = (int)CalendarTypes.Hijri;
        /// <summary>
        /// If you want the logon screen or the date and time viewer to also show the alternative calendar, enable it.
        /// </summary>
        public bool EnableAltCalendar { get; set; }
        /// <summary>
        /// Automatically load the custom screensavers on boot.
        /// </summary>
        public bool StartCustomScreensavers { get; set; } = true;
        /// <summary>
        /// User Name Shell Color
        /// </summary>
        public string UserNameShellColor
        {
            get => ColorTools.GetColor(KernelColorType.UserNameShell).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.UserNameShell, new Color(value));
        }
        /// <summary>
        /// Host Name Shell Color
        /// </summary>
        public string HostNameShellColor
        {
            get => ColorTools.GetColor(KernelColorType.HostNameShell).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.HostNameShell, new Color(value));
        }
        /// <summary>
        /// Continuable Kernel Error Color
        /// </summary>
        public string ContinuableKernelErrorColor
        {
            get => ColorTools.GetColor(KernelColorType.ContKernelError).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.ContKernelError, new Color(value));
        }
        /// <summary>
        /// Uncontinuable Kernel Error Color
        /// </summary>
        public string UncontinuableKernelErrorColor
        {
            get => ColorTools.GetColor(KernelColorType.UncontKernelError).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.UncontKernelError, new Color(value));
        }
        /// <summary>
        /// Text Color
        /// </summary>
        public string TextColor
        {
            get => ColorTools.GetColor(KernelColorType.NeutralText).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.NeutralText, new Color(value));
        }
        /// <summary>
        /// License Color
        /// </summary>
        public string LicenseColor
        {
            get => ColorTools.GetColor(KernelColorType.License).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.License, new Color(value));
        }
        /// <summary>
        /// Background Color
        /// </summary>
        public string BackgroundColor
        {
            get => ColorTools.GetColor(KernelColorType.Background).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Background, new Color(value));
        }
        /// <summary>
        /// Input Color
        /// </summary>
        public string InputColor
        {
            get => ColorTools.GetColor(KernelColorType.Input).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Input, new Color(value));
        }
        /// <summary>
        /// List Entry Color
        /// </summary>
        public string ListEntryColor
        {
            get => ColorTools.GetColor(KernelColorType.ListEntry).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.ListEntry, new Color(value));
        }
        /// <summary>
        /// List Value Color
        /// </summary>
        public string ListValueColor
        {
            get => ColorTools.GetColor(KernelColorType.ListValue).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.ListValue, new Color(value));
        }
        /// <summary>
        /// Kernel Stage Color
        /// </summary>
        public string KernelStageColor
        {
            get => ColorTools.GetColor(KernelColorType.Stage).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Stage, new Color(value));
        }
        /// <summary>
        /// Error Text Color
        /// </summary>
        public string ErrorTextColor
        {
            get => ColorTools.GetColor(KernelColorType.Error).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Error, new Color(value));
        }
        /// <summary>
        /// Warning Text Color
        /// </summary>
        public string WarningTextColor
        {
            get => ColorTools.GetColor(KernelColorType.Warning).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Warning, new Color(value));
        }
        /// <summary>
        /// Option Color
        /// </summary>
        public string OptionColor
        {
            get => ColorTools.GetColor(KernelColorType.Option).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Option, new Color(value));
        }
        /// <summary>
        /// Banner Color
        /// </summary>
        public string BannerColor
        {
            get => ColorTools.GetColor(KernelColorType.Banner).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Banner, new Color(value));
        }
        /// <summary>
        /// Notification Title Color
        /// </summary>
        public string NotificationTitleColor
        {
            get => ColorTools.GetColor(KernelColorType.NotificationTitle).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.NotificationTitle, new Color(value));
        }
        /// <summary>
        /// Notification Description Color
        /// </summary>
        public string NotificationDescriptionColor
        {
            get => ColorTools.GetColor(KernelColorType.NotificationDescription).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.NotificationDescription, new Color(value));
        }
        /// <summary>
        /// Notification Progress Color
        /// </summary>
        public string NotificationProgressColor
        {
            get => ColorTools.GetColor(KernelColorType.NotificationProgress).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.NotificationProgress, new Color(value));
        }
        /// <summary>
        /// Notification Failure Color
        /// </summary>
        public string NotificationFailureColor
        {
            get => ColorTools.GetColor(KernelColorType.NotificationFailure).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.NotificationFailure, new Color(value));
        }
        /// <summary>
        /// Question Color
        /// </summary>
        public string QuestionColor
        {
            get => ColorTools.GetColor(KernelColorType.Question).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Question, new Color(value));
        }
        /// <summary>
        /// Success Color
        /// </summary>
        public string SuccessColor
        {
            get => ColorTools.GetColor(KernelColorType.Success).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Success, new Color(value));
        }
        /// <summary>
        /// User Dollar Color
        /// </summary>
        public string UserDollarColor
        {
            get => ColorTools.GetColor(KernelColorType.UserDollar).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.UserDollar, new Color(value));
        }
        /// <summary>
        /// Tip Color
        /// </summary>
        public string TipColor
        {
            get => ColorTools.GetColor(KernelColorType.Tip).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Tip, new Color(value));
        }
        /// <summary>
        /// Separator Text Color
        /// </summary>
        public string SeparatorTextColor
        {
            get => ColorTools.GetColor(KernelColorType.SeparatorText).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.SeparatorText, new Color(value));
        }
        /// <summary>
        /// Separator Color
        /// </summary>
        public string SeparatorColor
        {
            get => ColorTools.GetColor(KernelColorType.Separator).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Separator, new Color(value));
        }
        /// <summary>
        /// List Title Color
        /// </summary>
        public string ListTitleColor
        {
            get => ColorTools.GetColor(KernelColorType.ListTitle).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.ListTitle, new Color(value));
        }
        /// <summary>
        /// Development Warning Color
        /// </summary>
        public string DevelopmentWarningColor
        {
            get => ColorTools.GetColor(KernelColorType.DevelopmentWarning).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.DevelopmentWarning, new Color(value));
        }
        /// <summary>
        /// Stage Time Color
        /// </summary>
        public string StageTimeColor
        {
            get => ColorTools.GetColor(KernelColorType.StageTime).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.StageTime, new Color(value));
        }
        /// <summary>
        /// Progress Color
        /// </summary>
        public string ProgressColor
        {
            get => ColorTools.GetColor(KernelColorType.Progress).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.Progress, new Color(value));
        }
        /// <summary>
        /// Back Option Color
        /// </summary>
        public string BackOptionColor
        {
            get => ColorTools.GetColor(KernelColorType.BackOption).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.BackOption, new Color(value));
        }
        /// <summary>
        /// Low Priority Border Color
        /// </summary>
        public string LowPriorityBorderColor
        {
            get => ColorTools.GetColor(KernelColorType.LowPriorityBorder).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.LowPriorityBorder, new Color(value));
        }
        /// <summary>
        /// Medium Priority Border Color
        /// </summary>
        public string MediumPriorityBorderColor
        {
            get => ColorTools.GetColor(KernelColorType.MediumPriorityBorder).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.MediumPriorityBorder, new Color(value));
        }
        /// <summary>
        /// High Priority Border Color
        /// </summary>
        public string HighPriorityBorderColor
        {
            get => ColorTools.GetColor(KernelColorType.HighPriorityBorder).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.HighPriorityBorder, new Color(value));
        }
        /// <summary>
        /// Table Separator Color
        /// </summary>
        public string TableSeparatorColor
        {
            get => ColorTools.GetColor(KernelColorType.TableSeparator).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.TableSeparator, new Color(value));
        }
        /// <summary>
        /// Table Header Color
        /// </summary>
        public string TableHeaderColor
        {
            get => ColorTools.GetColor(KernelColorType.TableHeader).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.TableHeader, new Color(value));
        }
        /// <summary>
        /// Table Value Color
        /// </summary>
        public string TableValueColor
        {
            get => ColorTools.GetColor(KernelColorType.TableValue).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.TableValue, new Color(value));
        }
        /// <summary>
        /// Selected Option Color
        /// </summary>
        public string SelectedOptionColor
        {
            get => ColorTools.GetColor(KernelColorType.SelectedOption).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.SelectedOption, new Color(value));
        }
        /// <summary>
        /// Alternative Option Color
        /// </summary>
        public string AlternativeOptionColor
        {
            get => ColorTools.GetColor(KernelColorType.AlternativeOption).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.AlternativeOption, new Color(value));
        }
        /// <summary>
        /// Weekend Day Color
        /// </summary>
        public string WeekendDayColor
        {
            get => ColorTools.GetColor(KernelColorType.WeekendDay).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.WeekendDay, new Color(value));
        }
        /// <summary>
        /// Event Day Color
        /// </summary>
        public string EventDayColor
        {
            get => ColorTools.GetColor(KernelColorType.EventDay).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.EventDay, new Color(value));
        }
        /// <summary>
        /// Table Title Color
        /// </summary>
        public string TableTitleColor
        {
            get => ColorTools.GetColor(KernelColorType.TableTitle).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.TableTitle, new Color(value));
        }
        /// <summary>
        /// Today Day Color
        /// </summary>
        public string TodayDayColor
        {
            get => ColorTools.GetColor(KernelColorType.TodayDay).PlainSequence;
            set => ColorTools.SetColor(KernelColorType.TodayDay, new Color(value));
        }
        /// <summary>
        /// Keep hardware probing messages silent.
        /// </summary>
        public bool QuietHardwareProbe { get; set; }
        /// <summary>
        /// If true, probes all the hardware; else, will only probe the needed hardware.
        /// </summary>
        public bool FullHardwareProbe { get; set; }
        /// <summary>
        /// Make hardware probing messages a bit talkative.
        /// </summary>
        public bool VerboseHardwareProbe { get; set; }
        /// <summary>
        /// Show Message of the Day before displaying login screen.
        /// </summary>
        public bool ShowMOTD { get; set; } = true;
        /// <summary>
        /// Clear screen before displaying login screen.
        /// </summary>
        public bool ClearOnLogin { get; set; }
        /// <summary>
        /// The kernel host name to communicate with the rest of the computers
        /// </summary>
        public string HostName { get; set; } = "kernel";
        /// <summary>
        /// Shows available users if enabled
        /// </summary>
        public bool ShowAvailableUsers { get; set; } = true;
        /// <summary>
        /// Which file is the MOTD text file? Write an absolute path to the text file
        /// </summary>
        public string MotdFilePath { get; set; } = Paths.GetKernelPath(KernelPathType.MOTD);
        /// <summary>
        /// Which file is the MAL text file? Write an absolute path to the text file
        /// </summary>
        public string MalFilePath { get; set; } = Paths.GetKernelPath(KernelPathType.MAL);
        /// <summary>
        /// Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string UsernamePrompt { get; set; } = "";
        /// <summary>
        /// Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string PasswordPrompt { get; set; } = "";
        /// <summary>
        /// Shows Message of the Day after displaying login screen
        /// </summary>
        public bool ShowMAL { get; set; } = true;
        /// <summary>
        /// Includes the anonymous users in the list
        /// </summary>
        public bool IncludeAnonymous { get; set; }
        /// <summary>
        /// Includes the disabled users in the list
        /// </summary>
        public bool IncludeDisabled { get; set; }
        /// <summary>
        /// Whether to use the modern way to present log-on screen or the classic way (write your username)
        /// </summary>
        public bool ModernLogon { get; set; } = true;
        /// <summary>
        /// Simplified help command for all the shells
        /// </summary>
        public bool SimHelp { get; set; }
        /// <summary>
        /// Sets the shell's current directory. Write an absolute path to any existing directory
        /// </summary>
        public string CurrentDir
        {
            get
            {
                return CurrentDirectory._CurrentDirectory;
            }
            set
            {
                Filesystem.ThrowOnInvalidPath(value);
                value = Filesystem.NeutralizePath(value);
                if (Checking.FolderExists(value))
                {
                    CurrentDirectory._CurrentDirectory = value;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.Filesystem, Translate.DoTranslation("Directory {0} not found").FormatString(value));
                }
            }
        }
        /// <summary>
        /// Group of paths separated by the colon. It works the same as PATH. Write a full path to a folder or a folder name. When you're finished, write \"q\". Write a minus sign next to the path to remove an existing directory.
        /// </summary>
        public string PathsToLookup { get; set; } = Environment.GetEnvironmentVariable("PATH");
        /// <summary>
        /// Prompt Preset
        /// </summary>
        public string PromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.Shell).PresetName;
            set => PromptPresetManager.SetPresetDry(value, ShellType.Shell, false);
        }
        /// <summary>
        /// FTP Prompt Preset
        /// </summary>
        public string FTPPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.FTPShell).PresetName;
            set => PromptPresetManager.SetPresetDry(value, ShellType.FTPShell, false);
        }
        /// <summary>
        /// Mail Prompt Preset
        /// </summary>
        public string MailPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.MailShell).PresetName;
            set => PromptPresetManager.SetPresetDry(value, ShellType.MailShell, false);
        }
        /// <summary>
        /// SFTP Prompt Preset
        /// </summary>
        public string SFTPPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.SFTPShell).PresetName;
            set => PromptPresetManager.SetPresetDry(value, ShellType.SFTPShell, false);
        }
        /// <summary>
        /// RSS Prompt Preset
        /// </summary>
        public string RSSPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.RSSShell).PresetName;
            set => PromptPresetManager.SetPresetDry(value, ShellType.RSSShell, false);
        }
        /// <summary>
        /// Text Edit Prompt Preset
        /// </summary>
        public string TextEditPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.TextShell).PresetName;
            set => PromptPresetManager.SetPresetDry(value, ShellType.TextShell, false);
        }
        /// <summary>
        /// JSON Shell Prompt Preset
        /// </summary>
        public string JSONShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.JsonShell).PresetName;
            set => PromptPresetManager.SetPresetDry(value, ShellType.JsonShell, false);
        }
        /// <summary>
        /// Hex Edit Prompt Preset
        /// </summary>
        public string HexEditPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.HexShell).PresetName;
            set => PromptPresetManager.SetPresetDry(value, ShellType.HexShell, false);
        }
        /// <summary>
        /// HTTP Shell Prompt Preset
        /// </summary>
        public string HTTPShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.HTTPShell).PresetName;
            set => PromptPresetManager.SetPresetDry(value, ShellType.HTTPShell, false);
        }
        /// <summary>
        /// Archive Shell Prompt Preset
        /// </summary>
        public string ArchiveShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.ArchiveShell).PresetName;
            set => PromptPresetManager.SetPresetDry(value, ShellType.ArchiveShell, false);
        }
        /// <summary>
        /// Admin Shell Prompt Preset
        /// </summary>
        public string AdminShellPromptPreset
        {
            get => PromptPresetManager.GetCurrentPresetBaseFromShell(ShellType.AdminShell).PresetName;
            set => PromptPresetManager.SetPresetDry(value, ShellType.AdminShell, false);
        }
        /// <summary>
        /// Default choice output type
        /// </summary>
        public int DefaultChoiceOutputType { get; set; } = (int)ChoiceOutputType.Modern;
        /// <summary>
        /// Controls how the files will be sorted
        /// </summary>
        public int SortMode { get; set; } = (int)FilesystemSortOptions.FullName;
        /// <summary>
        /// Controls the direction of filesystem sorting whether it's ascending or descending
        /// </summary>
        public int SortDirection { get; set; } = (int)FilesystemSortDirection.Ascending;
        /// <summary>
        /// Shows hidden files.
        /// </summary>
        public bool HiddenFiles { get; set; }
        /// <summary>
        /// If enabled, the kernel will parse the whole folder for its total size. Else, will only parse the surface.
        /// </summary>
        public bool FullParseMode { get; set; }
        /// <summary>
        /// Shows what file is being processed during the filesystem operations
        /// </summary>
        public bool ShowFilesystemProgress { get; set; } = true;
        /// <summary>
        /// Shows the brief file details while listing files
        /// </summary>
        public bool ShowFileDetailsList { get; set; } = true;
        /// <summary>
        /// Hides the annoying message if the listing function tries to open an unauthorized folder
        /// </summary>
        public bool SuppressUnauthorizedMessages { get; set; } = true;
        /// <summary>
        /// Makes the "cat" command print the file's line numbers
        /// </summary>
        public bool PrintLineNumbers { get; set; }
        /// <summary>
        /// Sorts the filesystem list professionally
        /// </summary>
        public bool SortList { get; set; } = true;
        /// <summary>
        /// If enabled, shows the total folder size in list, depending on how to calculate the folder sizes according to the configuration.
        /// </summary>
        public bool ShowTotalSizeInList { get; set; }
        /// <summary>
        /// Write a remote debugger port. It must be numeric, and must not be already used. Otherwise, remote debugger will fail to open the port
        /// </summary>
        public int DebugPort
        {
            get => RemoteDebugger.debugPort;
            set => RemoteDebugger.debugPort = value < 0 ? 3014 : value;
        }
        /// <summary>
        /// Write how many times the "get" command should retry failed downloads. It must be numeric.
        /// </summary>
        public int DownloadRetries
        {
            get => NetworkTools.downloadRetries;
            set => NetworkTools.downloadRetries = value < 0 ? 3 : value;
        }
        /// <summary>
        /// Write how many times the "put" command should retry failed uploads. It must be numeric.
        /// </summary>
        public int UploadRetries
        {
            get => NetworkTools.uploadRetries;
            set => NetworkTools.uploadRetries = value < 0 ? 3 : value;
        }
        /// <summary>
        /// If true, it makes "get" or "put" show the progress bar while downloading or uploading.
        /// </summary>
        public bool ShowProgress { get; set; } = true;
        /// <summary>
        /// Whether or not to log FTP username
        /// </summary>
        public bool FTPLoggerUsername { get; set; }
        /// <summary>
        /// Whether or not to log FTP IP address
        /// </summary>
        public bool FTPLoggerIP { get; set; }
        /// <summary>
        /// Pick the first profile only when connecting
        /// </summary>
        public bool FTPFirstProfileOnly { get; set; }
        /// <summary>
        /// When listing mail messages, show body preview
        /// </summary>
        public bool ShowPreview { get; set; }
        /// <summary>
        /// Records remote debug chat to debug log
        /// </summary>
        public bool RecordChatToDebugLog { get; set; } = true;
        /// <summary>
        /// Shows the SSH server banner on connection
        /// </summary>
        public bool SSHBanner { get; set; }
        /// <summary>
        /// Whether or not to enable RPC
        /// </summary>
        public bool RPCEnabled { get; set; } = true;
        /// <summary>
        /// Write an RPC port. It must be numeric, and must not be already used. Otherwise, RPC will fail to open the port.
        /// </summary>
        public int RPCPort
        {
            get => RemoteProcedure.rpcPort;
            set => RemoteProcedure.rpcPort = value < 0 ? 12345 : value;
        }
        /// <summary>
        /// Shows the FTP file details while listing remote directories
        /// </summary>
        public bool FtpShowDetailsInList { get; set; } = true;
        /// <summary>
        /// Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string FtpUserPromptStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string FtpPassPromptStyle { get; set; } = "";
        /// <summary>
        /// Uses the first FTP profile to connect to FTP
        /// </summary>
        public bool FtpUseFirstProfile { get; set; }
        /// <summary>
        /// If enabled, adds a new connection to the FTP speed dial
        /// </summary>
        public bool FtpNewConnectionsToSpeedDial { get; set; } = true;
        /// <summary>
        /// Tries to validate the FTP certificates. Turning it off is not recommended
        /// </summary>
        public bool FtpTryToValidateCertificate { get; set; } = true;
        /// <summary>
        /// Shows the FTP message of the day on login
        /// </summary>
        public bool FtpShowMotd { get; set; } = true;
        /// <summary>
        /// Always accept invalid FTP certificates. Turning it on is not recommended as it may pose security risks
        /// </summary>
        public bool FtpAlwaysAcceptInvalidCerts { get; set; }
        /// <summary>
        /// Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string Mail_UserPromptStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string Mail_PassPromptStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your IMAP server prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string Mail_IMAPPromptStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your SMTP server prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string Mail_SMTPPromptStyle { get; set; } = "";
        /// <summary>
        /// Automatically detect the mail server based on the given address
        /// </summary>
        public bool Mail_AutoDetectServer { get; set; } = true;
        /// <summary>
        /// Enables mail server debug
        /// </summary>
        public bool Mail_Debug { get; set; }
        /// <summary>
        /// Notifies you for any new mail messages
        /// </summary>
        public bool Mail_NotifyNewMail { get; set; } = true;
        /// <summary>
        /// Write how you want your GPG password prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string Mail_GPGPromptStyle { get; set; } = "";
        /// <summary>
        /// How many milliseconds to send the IMAP ping?
        /// </summary>
        public int Mail_ImapPingInterval
        {
            get => MailShellCommon.imapPingInterval;
            set => MailShellCommon.imapPingInterval = value < 0 ? 30000 : value;
        }
        /// <summary>
        /// How many milliseconds to send the SMTP ping?
        /// </summary>
        public int Mail_SmtpPingInterval
        {
            get => MailShellCommon.smtpPingInterval;
            set => MailShellCommon.smtpPingInterval = value < 0 ? 30000 : value;
        }
        /// <summary>
        /// Controls how the mail text will be shown
        /// </summary>
        public int Mail_TextFormat { get; set; } = (int)TextFormat.Plain;
        /// <summary>
        /// If you want remote debug to start on boot, enable this
        /// </summary>
        public bool RDebugAutoStart { get; set; } = true;
        /// <summary>
        /// Specifies the remote debug message format. {0} for name, {1} for message
        /// </summary>
        public string RDebugMessageFormat { get; set; } = "";
        /// <summary>
        /// Write how you want your RSS feed server prompt to be. Leave blank to use default style. Placeholders are parsed.
        /// </summary>
        public string RSSFeedUrlPromptStyle { get; set; } = "";
        /// <summary>
        /// Auto refresh RSS feed
        /// </summary>
        public bool RSSRefreshFeeds { get; set; } = true;
        /// <summary>
        /// How many milliseconds to refresh the RSS feed?
        /// </summary>
        public int RSSRefreshInterval
        {
            get => RSSShellCommon.refreshInterval;
            set => RSSShellCommon.refreshInterval = value < 0 ? 60000 : value;
        }
        /// <summary>
        /// Shows the SFTP file details while listing remote directories
        /// </summary>
        public bool SFTPShowDetailsInList { get; set; } = true;
        /// <summary>
        /// Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string SFTPUserPromptStyle { get; set; } = "";
        /// <summary>
        /// If enabled, adds a new connection to the SFTP speed dial
        /// </summary>
        public bool SFTPNewConnectionsToSpeedDial { get; set; } = true;
        /// <summary>
        /// How many milliseconds to wait before declaring timeout?
        /// </summary>
        public int PingTimeout
        {
            get => NetworkTools.pingTimeout;
            set => NetworkTools.pingTimeout = value < 0 ? 60000 : value;
        }
        /// <summary>
        /// Write how you want your download percentage text to be. Leave blank to use default style. Placeholders are parsed. {0} for downloaded size, {1} for target size, {2} for percentage.
        /// </summary>
        public string DownloadPercentagePrint { get; set; } = "";
        /// <summary>
        /// Write how you want your upload percentage text to be. Leave blank to use default style. Placeholders are parsed. {0} for uploaded size, {1} for target size, {2} for percentage.
        /// </summary>
        public string UploadPercentagePrint { get; set; } = "";
        /// <summary>
        /// Whether to recursively hash a directory. Please note that not all the FTP servers support that
        /// </summary>
        public bool FtpRecursiveHashing { get; set; }
        /// <summary>
        /// How many e-mail messages to display in one page?
        /// </summary>
        public int Mail_MaxMessagesInPage
        {
            get => MailShellCommon.maxMessagesInPage;
            set => MailShellCommon.maxMessagesInPage = value < 0 ? 10 : value;
        }
        /// <summary>
        /// If enabled, the mail shell will show how many bytes transmitted when downloading mail.
        /// </summary>
        public bool Mail_ShowProgress { get; set; } = true;
        /// <summary>
        /// Write how you want your mail transfer progress style to be. Leave blank to use default style. Placeholders are parsed. {0} for transferred size and {1} for total size.
        /// </summary>
        public string Mail_ProgressStyle { get; set; } = "";
        /// <summary>
        /// Write how you want your mail transfer progress style to be. Leave blank to use default style. Placeholders are parsed. {0} for transferred size.
        /// </summary>
        public string Mail_ProgressStyleSingle { get; set; } = "";
        /// <summary>
        /// Shows the notification showing the download progress
        /// </summary>
        public bool DownloadNotificationProvoke { get; set; }
        /// <summary>
        /// Shows the notification showing the upload progress
        /// </summary>
        public bool UploadNotificationProvoke { get; set; }
        /// <summary>
        /// How many milliseconds to wait before RSS feed fetch timeout?
        /// </summary>
        public int RSSFetchTimeout
        {
            get => RSSShellCommon.fetchTimeout;
            set => RSSShellCommon.fetchTimeout = value < 0 ? 60000 : value;
        }
        /// <summary>
        /// How many times to verify the upload and download and retry if the verification fails before the download fails as a whole?
        /// </summary>
        public int FtpVerifyRetryAttempts
        {
            get => FTPShellCommon.verifyRetryAttempts;
            set => FTPShellCommon.verifyRetryAttempts = value < 0 ? 3 : value;
        }
        /// <summary>
        /// How many milliseconds to wait before the FTP connection timeout?
        /// </summary>
        public int FtpConnectTimeout
        {
            get => FTPShellCommon.connectTimeout;
            set => FTPShellCommon.connectTimeout = value < 0 ? 15000 : value;
        }
        /// <summary>
        /// How many milliseconds to wait before the FTP data connection timeout?
        /// </summary>
        public int FtpDataConnectTimeout
        {
            get => FTPShellCommon.dataConnectTimeout;
            set => FTPShellCommon.dataConnectTimeout = value < 0 ? 15000 : value;
        }
        /// <summary>
        /// Choose the version of Internet Protocol that the FTP server supports and that the FTP client uses
        /// </summary>
        public int FtpProtocolVersions { get; set; } = (int)FtpIpVersion.ANY;
        /// <summary>
        /// If enabled, will use the notification system to notify the host of remote debug connection error. Otherwise, will use the default console writing.
        /// </summary>
        public bool NotifyOnRemoteDebugConnectionError { get; set; } = true;
        /// <summary>
        /// Which screensaver do you want to lock your screen with?
        /// </summary>
        public string DefaultSaverName
        {
            get => Screensaver.defSaverName;
            set => Screensaver.defSaverName = Screensaver.Screensavers.ContainsKey(value) ? value : "plain";
        }
        /// <summary>
        /// Write when to launch screensaver after specified milliseconds. It must be numeric
        /// </summary>
        public int ScreenTimeout
        {
            get => Screensaver.scrnTimeout;
            set => Screensaver.scrnTimeout = value < 0 ? 300000 : value;
        }
        /// <summary>
        /// Enables debugging for screensavers. Please note that it may quickly fill the debug log and slightly slow the screensaver down, depending on the screensaver used. Only works if kernel debugging is enabled for diagnostic purposes.
        /// </summary>
        public bool ScreensaverDebug { get; set; }
        /// <summary>
        /// After locking the screen, ask for password
        /// </summary>
        public bool PasswordLock { get; set; } = true;
        /// <summary>
        /// The time and date will be shown in the upper right corner of the screen
        /// </summary>
        public bool CornerTimeDate { get; set; }
        /// <summary>
        /// Enables eyecandy on startup
        /// </summary>
        public bool StartScroll { get; set; } = true;
        /// <summary>
        /// The time and date will be longer, showing full month names, etc.
        /// </summary>
        public bool LongTimeDate { get; set; } = true;
        /// <summary>
        /// Select your preferred unit for temperature (this only applies to the "weather" command)
        /// </summary>
        public int PreferredUnit { get; set; } = (int)UnitMeasurement.Metric;
        /// <summary>
        /// Turns on or off the text editor autosave feature
        /// </summary>
        public bool TextEdit_AutoSaveFlag { get; set; } = true;
        /// <summary>
        /// If autosave is enabled, the text file will be saved for each "n" seconds
        /// </summary>
        public int TextEdit_AutoSaveInterval
        {
            get => TextEditShellCommon.autoSaveInterval;
            set => TextEditShellCommon.autoSaveInterval = value < 0 ? 60 : value;
        }
        /// <summary>
        /// Turns on or off the hex editor autosave feature
        /// </summary>
        public bool HexEdit_AutoSaveFlag { get; set; } = true;
        /// <summary>
        /// If autosave is enabled, the binary file will be saved for each \"n\" seconds
        /// </summary>
        public int HexEdit_AutoSaveInterval
        {
            get => HexEditShellCommon.autoSaveInterval;
            set => HexEditShellCommon.autoSaveInterval = value < 0 ? 60 : value;
        }
        /// <summary>
        /// Wraps the list outputs if it seems too long for the current console geometry
        /// </summary>
        public bool WrapListOutputs { get; set; }
        /// <summary>
        /// Covers the notification with the border
        /// </summary>
        public bool DrawBorderNotification { get; set; }
        /// <summary>
        /// Write the filenames of the mods that will not run on startup. When you're finished, write "q". Write a minus sign next to the path to remove an existing mod.
        /// </summary>
        public string BlacklistedModsString { get; set; } = "";
        /// <summary>
        /// What is the minimum number to choose?
        /// </summary>
        public int SolverMinimumNumber
        {
            get => Solver.minimumNumber;
            set => Solver.minimumNumber = value < 0 ? 0 : value;
        }
        /// <summary>
        /// What is the maximum number to choose?
        /// </summary>
        public int SolverMaximumNumber
        {
            get => Solver.maximumNumber;
            set => Solver.maximumNumber = value < 0 ? 1000 : value;
        }
        /// <summary>
        /// Whether to show what's written in the input prompt.
        /// </summary>
        public bool SolverShowInput { get; set; }
        /// <summary>
        /// A character that resembles the upper left corner. Be sure to only input one character
        /// </summary>
        public string NotifyUpperLeftCornerChar
        {
            get => NotificationManager.notifyUpperLeftCornerChar;
            set => NotificationManager.notifyUpperLeftCornerChar = string.IsNullOrEmpty(value) ? "╔" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the upper right corner. Be sure to only input one character
        /// </summary>
        public string NotifyUpperRightCornerChar
        {
            get => NotificationManager.notifyUpperRightCornerChar;
            set => NotificationManager.notifyUpperRightCornerChar = string.IsNullOrEmpty(value) ? "╗" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the lower left corner. Be sure to only input one character
        /// </summary>
        public string NotifyLowerLeftCornerChar
        {
            get => NotificationManager.notifyLowerLeftCornerChar;
            set => NotificationManager.notifyLowerLeftCornerChar = string.IsNullOrEmpty(value) ? "╚" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the lower right corner. Be sure to only input one character
        /// </summary>
        public string NotifyLowerRightCornerChar
        {
            get => NotificationManager.notifyLowerRightCornerChar;
            set => NotificationManager.notifyLowerRightCornerChar = string.IsNullOrEmpty(value) ? "╝" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the upper frame. Be sure to only input one character
        /// </summary>
        public string NotifyUpperFrameChar
        {
            get => NotificationManager.notifyUpperFrameChar;
            set => NotificationManager.notifyUpperFrameChar = string.IsNullOrEmpty(value) ? "═" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the lower frame. Be sure to only input one character
        /// </summary>
        public string NotifyLowerFrameChar
        {
            get => NotificationManager.notifyLowerFrameChar;
            set => NotificationManager.notifyLowerFrameChar = string.IsNullOrEmpty(value) ? "═" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the left frame. Be sure to only input one character
        /// </summary>
        public string NotifyLeftFrameChar
        {
            get => NotificationManager.notifyLeftFrameChar;
            set => NotificationManager.notifyLeftFrameChar = string.IsNullOrEmpty(value) ? "║" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the right frame. Be sure to only input one character
        /// </summary>
        public string NotifyRightFrameChar
        {
            get => NotificationManager.notifyRightFrameChar;
            set => NotificationManager.notifyRightFrameChar = string.IsNullOrEmpty(value) ? "║" : value[0].ToString();
        }
        /// <summary>
        /// Select your preferred difficulty
        /// </summary>
        public int SpeedPressCurrentDifficulty { get; set; } = (int)SpeedPressDifficulty.Medium;
        /// <summary>
        /// How many milliseconds to wait for the keypress before the timeout? (In custom difficulty)
        /// </summary>
        public int SpeedPressTimeout
        {
            get => speedPressTimeout;
            set => speedPressTimeout = value < 0 ? 3000 : value;
        }
        /// <summary>
        /// Each login, it will show the latest RSS headline from the selected headline URL
        /// </summary>
        public bool ShowHeadlineOnLogin { get; set; }
        /// <summary>
        /// RSS headline URL to be used when showing the latest headline. This is usually your favorite feed
        /// </summary>
        public string RssHeadlineUrl { get; set; } = "https://www.techrepublic.com/rssfeeds/articles/";
        /// <summary>
        /// If enabled, deletes all events and/or reminders before saving all of them using the calendar command
        /// </summary>
        public bool SaveEventsRemindersDestructively { get; set; }
        /// <summary>
        /// Selects the default JSON formatting (beautified or minified) for the JSON shell to save
        /// </summary>
        public int JsonShell_Formatting { get; set; } = (int)Formatting.Indented;
        /// <summary>
        /// If enabled, will use figlet for timer. Please note that it needs a big console screen in order to render the time properly with Figlet enabled.
        /// </summary>
        public bool EnableFigletTimer { get; set; }
        /// <summary>
        /// Write a figlet font that is supported by the Figgle library. Consult the library documentation for more information
        /// </summary>
        public string TimerFigletFont
        {
            get => TimerScreen.timerFigletFont;
            set => TimerScreen.timerFigletFont = FigletTools.FigletFonts.ContainsKey(value) ? value : "Small";
        }
        /// <summary>
        /// Shows the commands count in the command list, controlled by the three count show switches for different kinds of commands.
        /// </summary>
        public bool ShowCommandsCount { get; set; }
        /// <summary>
        /// Show the shell commands count on help
        /// </summary>
        public bool ShowShellCommandsCount { get; set; } = true;
        /// <summary>
        /// Show the mod commands count on help
        /// </summary>
        public bool ShowModCommandsCount { get; set; } = true;
        /// <summary>
        /// Show the aliases count on help
        /// </summary>
        public bool ShowShellAliasesCount { get; set; } = true;
        /// <summary>
        /// A character that masks the password. Leave blank for more security
        /// </summary>
        public string CurrentMask
        {
            get => Input.currentMask;
            set => Input.currentMask = string.IsNullOrEmpty(value) ? "*" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the upper left corner. Be sure to only input one character
        /// </summary>
        public string ProgressUpperLeftCornerChar
        {
            get => ProgressTools.progressUpperLeftCornerChar;
            set => ProgressTools.progressUpperLeftCornerChar = string.IsNullOrEmpty(value) ? "╔" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the upper right corner. Be sure to only input one character
        /// </summary>
        public string ProgressUpperRightCornerChar
        {
            get => ProgressTools.progressUpperRightCornerChar;
            set => ProgressTools.progressUpperRightCornerChar = string.IsNullOrEmpty(value) ? "╗" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the lower left corner. Be sure to only input one character
        /// </summary>
        public string ProgressLowerLeftCornerChar
        {
            get => ProgressTools.progressLowerLeftCornerChar;
            set => ProgressTools.progressLowerLeftCornerChar = string.IsNullOrEmpty(value) ? "╚" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the lower right corner. Be sure to only input one character
        /// </summary>
        public string ProgressLowerRightCornerChar
        {
            get => ProgressTools.progressLowerRightCornerChar;
            set => ProgressTools.progressLowerRightCornerChar = string.IsNullOrEmpty(value) ? "╝" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the upper frame. Be sure to only input one character
        /// </summary>
        public string ProgressUpperFrameChar
        {
            get => ProgressTools.progressUpperFrameChar;
            set => ProgressTools.progressUpperFrameChar = string.IsNullOrEmpty(value) ? "═" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the lower frame. Be sure to only input one character
        /// </summary>
        public string ProgressLowerFrameChar
        {
            get => ProgressTools.progressLowerFrameChar;
            set => ProgressTools.progressLowerFrameChar = string.IsNullOrEmpty(value) ? "═" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the left frame. Be sure to only input one character
        /// </summary>
        public string ProgressLeftFrameChar
        {
            get => ProgressTools.progressLeftFrameChar;
            set => ProgressTools.progressLeftFrameChar = string.IsNullOrEmpty(value) ? "║" : value[0].ToString();
        }
        /// <summary>
        /// A character that resembles the right frame. Be sure to only input one character
        /// </summary>
        public string ProgressRightFrameChar
        {
            get => ProgressTools.progressRightFrameChar;
            set => ProgressTools.progressRightFrameChar = string.IsNullOrEmpty(value) ? "║" : value[0].ToString();
        }
        /// <summary>
        /// Whether the input history is enabled or not. If enabled, you can access recently typed commands using the up or down arrow keys.
        /// </summary>
        public bool InputHistoryEnabled
        {
            get
            {
                return TermReaderSettings.HistoryEnabled;
            }
            set
            {
                TermReaderSettings.HistoryEnabled = value;
            }
        }
        /// <summary>
        /// Whether to use PowerLine to render the spaceship or to use the standard greater than character. If you want to use PowerLine with Meteor, you need to install an appropriate font with PowerLine support.
        /// </summary>
        public bool MeteorUsePowerLine { get; set; } = true;
        /// <summary>
        /// Specifies the game speed in milliseconds.
        /// </summary>
        public int MeteorSpeed
        {
            get => MeteorShooter.meteorSpeed;
            set => MeteorShooter.meteorSpeed = value < 0 ? 10 : value;
        }
        /// <summary>
        /// Enables the scroll bar in selection screens
        /// </summary>
        public bool EnableScrollBarInSelection { get; set; } = true;
        /// <summary>
        /// If Do Not Disturb is enabled, all notifications received will be supressed from the display. This means that you won't be able to see any notification to help you focus.
        /// </summary>
        public bool DoNotDisturb
        {
            get => Flags.DoNotDisturb;
            set => Flags.DoNotDisturb = value;
        }
        /// <summary>
        /// A character that resembles the upper left corner. Be sure to only input one character.
        /// </summary>
        public string BorderUpperLeftCornerChar
        {
            get
            {
                return BorderTools._borderUpperLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╔";
                BorderTools._borderUpperLeftCornerChar = value;
            }
        }
        /// <summary>
        /// A character that resembles the upper right corner. Be sure to only input one character.
        /// </summary>
        public string BorderUpperRightCornerChar
        {
            get
            {
                return BorderTools._borderUpperRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╗";
                BorderTools._borderUpperRightCornerChar = value;
            }
        }
        /// <summary>
        /// A character that resembles the lower left corner. Be sure to only input one character.
        /// </summary>
        public string BorderLowerLeftCornerChar
        {
            get
            {
                return BorderTools._borderLowerLeftCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╚";
                BorderTools._borderLowerLeftCornerChar = value;
            }
        }
        /// <summary>
        /// A character that resembles the lower right corner. Be sure to only input one character.
        /// </summary>
        public string BorderLowerRightCornerChar
        {
            get
            {
                return BorderTools._borderLowerRightCornerChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "╝";
                BorderTools._borderLowerRightCornerChar = value;
            }
        }
        /// <summary>
        /// A character that resembles the upper frame. Be sure to only input one character.
        /// </summary>
        public string BorderUpperFrameChar
        {
            get
            {
                return BorderTools._borderUpperFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                BorderTools._borderUpperFrameChar = value;
            }
        }
        /// <summary>
        /// A character that resembles the lower frame. Be sure to only input one character.
        /// </summary>
        public string BorderLowerFrameChar
        {
            get
            {
                return BorderTools._borderLowerFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "═";
                BorderTools._borderLowerFrameChar = value;
            }
        }
        /// <summary>
        /// A character that resembles the left frame. Be sure to only input one character.
        /// </summary>
        public string BorderLeftFrameChar
        {
            get
            {
                return BorderTools._borderLeftFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                BorderTools._borderLeftFrameChar = value;
            }
        }
        /// <summary>
        /// A character that resembles the right frame. Be sure to only input one character.
        /// </summary>
        public string BorderRightFrameChar
        {
            get
            {
                return BorderTools._borderRightFrameChar;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "║";
                BorderTools._borderRightFrameChar = value;
            }
        }
        /// <summary>
        /// File manager background color
        /// </summary>
        public string FileManagerBackgroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkBlue)).PlainSequence;
        /// <summary>
        /// File manager foreground color
        /// </summary>
        public string FileManagerForegroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Yellow)).PlainSequence;
        /// <summary>
        /// File manager pane background color
        /// </summary>
        public string FileManagerPaneBackgroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Blue3)).PlainSequence;
        /// <summary>
        /// File manager pane separator color
        /// </summary>
        public string FileManagerPaneSeparatorColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkGreen_005f00)).PlainSequence;
        /// <summary>
        /// File manager selected pane separator color
        /// </summary>
        public string FileManagerPaneSelectedSeparatorColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Green3_00d700)).PlainSequence;
        /// <summary>
        /// File manager selected pane file foreground color
        /// </summary>
        public string FileManagerPaneSelectedFileForeColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Yellow)).PlainSequence;
        /// <summary>
        /// File manager selected pane file background color
        /// </summary>
        public string FileManagerPaneSelectedFileBackColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkBlue)).PlainSequence;
        /// <summary>
        /// File manager pane file foreground color
        /// </summary>
        public string FileManagerPaneFileForeColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkYellow)).PlainSequence;
        /// <summary>
        /// File manager pane file background color
        /// </summary>
        public string FileManagerPaneFileBackColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Blue3)).PlainSequence;
        /// <summary>
        /// File manager option background color
        /// </summary>
        public string FileManagerOptionBackgroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkCyan)).PlainSequence;
        /// <summary>
        /// File manager option foreground color
        /// </summary>
        public string FileManagerOptionForegroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Black)).PlainSequence;
        /// <summary>
        /// File manager option binding name color
        /// </summary>
        public string FileManagerKeyBindingOptionColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Cyan)).PlainSequence;
        /// <summary>
        /// File manager box background color
        /// </summary>
        public string FileManagerBoxBackgroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Red)).PlainSequence;
        /// <summary>
        /// File manager box foreground color
        /// </summary>
        public string FileManagerBoxForegroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.White)).PlainSequence;
        /// <summary>
        /// Task manager background color
        /// </summary>
        public string TaskManagerBackgroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkBlue)).PlainSequence;
        /// <summary>
        /// Task manager foreground color
        /// </summary>
        public string TaskManagerForegroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Yellow)).PlainSequence;
        /// <summary>
        /// Task manager pane background color
        /// </summary>
        public string TaskManagerPaneBackgroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Blue3)).PlainSequence;
        /// <summary>
        /// Task manager pane separator color
        /// </summary>
        public string TaskManagerPaneSeparatorColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkGreen_005f00)).PlainSequence;
        /// <summary>
        /// Task manager selected pane task foreground color
        /// </summary>
        public string TaskManagerPaneSelectedTaskForeColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Yellow)).PlainSequence;
        /// <summary>
        /// Task manager selected pane task background color
        /// </summary>
        public string TaskManagerPaneSelectedTaskBackColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkBlue)).PlainSequence;
        /// <summary>
        /// Task manager pane task foreground color
        /// </summary>
        public string TaskManagerPaneTaskForeColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkYellow)).PlainSequence;
        /// <summary>
        /// Task manager pane task background color
        /// </summary>
        public string TaskManagerPaneTaskBackColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Blue3)).PlainSequence;
        /// <summary>
        /// Task manager option background color
        /// </summary>
        public string TaskManagerOptionBackgroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.DarkCyan)).PlainSequence;
        /// <summary>
        /// Task manager option foreground color
        /// </summary>
        public string TaskManagerOptionForegroundColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Black)).PlainSequence;
        /// <summary>
        /// Task manager option binding name color
        /// </summary>
        public string TaskManagerKeyBindingOptionColor { get; set; } = new Color(Convert.ToInt32(ConsoleColors.Cyan)).PlainSequence;
    }
}
