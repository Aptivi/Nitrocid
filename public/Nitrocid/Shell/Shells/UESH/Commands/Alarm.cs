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

using Terminaux.Shell.Help;
using Terminaux.Shell.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Terminaux.Colors.Themes.Colors;
using System;
using Nitrocid.Kernel.Time.Alarm;
using Terminaux.Shell.Switches;
using Nitrocid.Misc.Interactives;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Security.Permissions;
using Nitrocid.Users;
using Nitrocid.Kernel.Debugging;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Manages your alarms
    /// </summary>
    /// <remarks>
    /// You can manage all your alarms by this command. It allows you to list, start, and stop alarms.
    /// </remarks>
    class AlarmCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!PermissionsTools.IsPermissionGranted(PermissionTypes.RunStrictCommands) &&
                !UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", vars: [parameters.CommandText]);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_NEEDSPERM"), true, ThemeColorType.Error, parameters.CommandText);
                return -4;
            }

            bool launchTui = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-tui");
            if (launchTui)
            {
                AlarmCli.OpenAlarmCli();
                return 0;
            }
            string CommandMode = parameters.ArgumentsList[0].ToLower();
            string name = "";
            string interval = "";

            // Now, the actual logic
            switch (CommandMode)
            {
                case "start":
                    {
                        name = parameters.ArgumentsList[1];
                        interval = parameters.ArgumentsList[2];
                        TimeSpan span = TimeSpan.Parse(interval);
                        AlarmTools.StartAlarm(name, name, (int)span.TotalSeconds);
                        break;
                    }
                case "stop":
                    {
                        name = parameters.ArgumentsList[1];
                        AlarmTools.StopAlarm(name);
                        break;
                    }
                case "list":
                    {
                        foreach (var alarm in AlarmTools.alarms)
                        {
                            SeparatorWriterColor.WriteSeparatorColor(alarm.Key, ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                            TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ALARM_NAME") + " ", false, ThemeColorType.ListEntry);
                            TextWriters.Write(alarm.Value.Name, true, ThemeColorType.ListValue);
                            TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ALARM_DESC") + " ", false, ThemeColorType.ListEntry);
                            TextWriters.Write(alarm.Value.Description, true, ThemeColorType.ListValue);
                            TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ALARM_DUE") + " ", false, ThemeColorType.ListEntry);
                            TextWriters.Write($"{alarm.Value.Length}", true, ThemeColorType.ListValue);
                        }

                        break;
                    }

                default:
                    {
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_INVALIDCOMMAND_BRANCHED"), true, ThemeColorType.Error, CommandMode);
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
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ALARM_FOUND"), true, ThemeColorType.Error);
                                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                            }
                            if (!TimeSpan.TryParse(interval, out _))
                            {
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ALARM_INTERVALINVALID"), true, ThemeColorType.Error);
                                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                            }
                        }
                        else
                        {
                            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ALARM_NAMEINTERVALNEEDED"), true, ThemeColorType.Error);
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
                                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_ALARM_NOTFOUND"), true, ThemeColorType.Error);
                                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                            }
                        }
                        else
                        {
                            TextWriters.Write(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_ALARMNAMENOTSPECCED"), true, ThemeColorType.Error);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                        }

                        break;
                    }
            }
            return 0;
        }
    }
}
