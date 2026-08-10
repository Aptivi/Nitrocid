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

using System;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Time.Alarm;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Security.Permissions;
using Nitrocid.Base.Users;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Help;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Manages your alarms
    /// </summary>
    /// <remarks>
    /// You can manage all your alarms by this command. It allows you to list, start, and stop alarms.
    /// </remarks>
    class AlarmCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "alarm";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "start", new()
                    {
                        ExactWording = ["start"],
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_START_DESC"
                    }),
                    new CommandArgumentPart(true, "alarmname", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_MISC_INTERACTIVES_ALARMTUI_ALARMNAME"
                    }),
                    new CommandArgumentPart(true, "interval", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_START_INTERVAL_DESC"
                    }),
                ])
                {
                    ArgChecker = (cp) => AlarmCommand.CheckArgument(cp, "start")
                },
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "stop", new()
                    {
                        ExactWording = ["stop"],
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_STOP_DESC"
                    }),
                    new CommandArgumentPart(true, "alarmname", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_STOP_NAME_DESC"
                    }),
                ])
                {
                    ArgChecker = (cp) => AlarmCommand.CheckArgument(cp, "stop")
                },
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "list", new()
                    {
                        ExactWording = ["list"],
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_ARGUMENT_LIST_DESC"
                    }),
                ],
                [
                    new SwitchInfo("tui", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_ALARM_SWITCH_TUI_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    })
                ]),
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            if (!PermissionsTools.IsPermissionGranted(PermissionTypes.RunStrictCommands) &&
                !UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", vars: [parameters.CommandText]);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_NEEDSPERM"), true, ThemeColorType.Error, parameters.CommandText);
                return -4;
            }

            bool launchTui = parameters.ContainsSwitch("-tui");
            if (launchTui)
            {
                AlarmCli.OpenAlarmCli();
                return 0;
            }
            string commandMode = parameters.ArgumentsList[0].ToLower();

            // Now, the actual logic
            switch (commandMode)
            {
                case "start":
                    {
                        string name = parameters.ArgumentsList[1];
                        string interval = parameters.ArgumentsList[2];
                        TimeSpan span = TimeSpan.Parse(interval);
                        AlarmTools.StartAlarm(name, name, (int)span.TotalSeconds);
                        break;
                    }
                case "stop":
                    {
                        string name = parameters.ArgumentsList[1];
                        AlarmTools.StopAlarm(name);
                        break;
                    }
                case "list":
                    {
                        foreach (var alarm in AlarmTools.alarms)
                        {
                            SeparatorWriterColor.WriteSeparatorColor(alarm.Key, ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ALARM_NAME"), alarm.Value.Name);
                            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ALARM_DESC"), alarm.Value.Description);
                            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ALARM_DUE"), $"{alarm.Value.Length}");
                        }

                        break;
                    }

                default:
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_INVALIDCOMMAND_BRANCHED"), true, ThemeColorType.Error, commandMode);
                        HelpPrint.ShowHelp("alarm");
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                    }
            }
            return 0;
        }

        internal static int CheckArgument(CommandParameters parameters, string commandMode)
        {
            // These command modes require arguments to be passed, so re-check here and there.
            switch (commandMode.ToLower())
            {
                case "start":
                    {
                        if (parameters.ArgumentsList.Length > 2)
                        {
                            string name = parameters.ArgumentsList[1];
                            string interval = parameters.ArgumentsList[2];
                            if (AlarmTools.IsAlarmRegistered(name))
                            {
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ALARM_FOUND"), true, ThemeColorType.Error);
                                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                            }
                            if (!TimeSpan.TryParse(interval, out _))
                            {
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ALARM_INTERVALINVALID"), true, ThemeColorType.Error);
                                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                            }
                        }
                        else
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ALARM_NAMEINTERVALNEEDED"), true, ThemeColorType.Error);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                        }

                        break;
                    }
                case "stop":
                    {
                        if (parameters.ArgumentsList.Length > 1)
                        {
                            string name = parameters.ArgumentsList[1];
                            if (!AlarmTools.IsAlarmRegistered(name))
                            {
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ALARM_NOTFOUND"), true, ThemeColorType.Error);
                                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                            }
                        }
                        else
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_ALARMNAMENOTSPECCED"), true, ThemeColorType.Error);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                        }

                        break;
                    }
            }
            return 0;
        }
    }
}
