﻿//
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

using Nitrocid.Shell.ShellBase.Help;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.ConsoleBase.Colors;
using System;
using Nitrocid.Kernel.Time.Alarm;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Misc.Interactives;
using Terminaux.Writer.ConsoleWriters;

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
                            SeparatorWriterColor.WriteSeparatorColor(alarm.Key, KernelColorTools.GetColor(KernelColorType.ListTitle));
                            TextWriters.Write("- " + Translate.DoTranslation("Alarm name:") + " ", false, KernelColorType.ListEntry);
                            TextWriters.Write(alarm.Value.Name, true, KernelColorType.ListValue);
                            TextWriters.Write("- " + Translate.DoTranslation("Alarm description:") + " ", false, KernelColorType.ListEntry);
                            TextWriters.Write(alarm.Value.Description, true, KernelColorType.ListValue);
                            TextWriters.Write("- " + Translate.DoTranslation("Alarm due date:") + " ", false, KernelColorType.ListEntry);
                            TextWriters.Write($"{alarm.Value.Length}", true, KernelColorType.ListValue);
                        }

                        break;
                    }

                default:
                    {
                        TextWriters.Write(Translate.DoTranslation("Invalid command {0}. Check the usage below:"), true, KernelColorType.Error, CommandMode);
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
                                TextWriters.Write(Translate.DoTranslation("Alarm already exists."), true, KernelColorType.Error);
                                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                            }
                            if (!TimeSpan.TryParse(interval, out _))
                            {
                                TextWriters.Write(Translate.DoTranslation("Alarm interval is invalid."), true, KernelColorType.Error);
                                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                            }
                        }
                        else
                        {
                            TextWriters.Write(Translate.DoTranslation("Alarm name and interval is not specified."), true, KernelColorType.Error);
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
                                TextWriters.Write(Translate.DoTranslation("Alarm doesn't exist."), true, KernelColorType.Error);
                                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                            }
                        }
                        else
                        {
                            TextWriters.Write(Translate.DoTranslation("Alarm name is not specified."), true, KernelColorType.Error);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                        }

                        break;
                    }
            }
            return 0;
        }
    }
}
