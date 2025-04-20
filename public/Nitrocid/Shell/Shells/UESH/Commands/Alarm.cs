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

using Nitrocid.Shell.ShellBase.Help;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Terminaux.Writer.FancyWriters;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.ConsoleBase.Colors;
using System;
using Nitrocid.Kernel.Time.Alarm;
using Nitrocid.Shell.ShellBase.Switches;
using Terminaux.Inputs.Interactive;
using Nitrocid.Misc.Interactives;

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
                var tui = new AlarmCli();
                tui.Bindings.Add(new InteractiveTuiBinding<string>(Translate.DoTranslation("Add"), ConsoleKey.A, (_, _, _, _) => tui.Start(), true));
                tui.Bindings.Add(new InteractiveTuiBinding<string>(Translate.DoTranslation("Remove"), ConsoleKey.Delete, (alarm, _, _, _) => tui.Stop(alarm)));
                InteractiveTuiTools.OpenInteractiveTui(tui);
                return 0;
            }
            string CommandMode = parameters.ArgumentsList[0].ToLower();
            string name = "";
            string interval = "";
            TimeSpan span = new();

            // These command modes require arguments to be passed, so re-check here and there.
            // TODO: Make UESH natively support this command setup.
            switch (CommandMode)
            {
                case "start":
                    {
                        if (parameters.ArgumentsList.Length > 2)
                        {
                            name = parameters.ArgumentsList[1];
                            interval = parameters.ArgumentsList[2];
                            if (AlarmTools.IsAlarmRegistered(name))
                            {
                                TextWriters.Write(Translate.DoTranslation("Alarm already exists."), true, KernelColorType.Error);
                                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Alarm);
                            }
                            if (!TimeSpan.TryParse(interval, out span))
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
                            name = parameters.ArgumentsList[1];
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

            // Now, the actual logic
            switch (CommandMode)
            {
                case "start":
                    {
                        AlarmTools.StartAlarm(name, name, (int)span.TotalSeconds);
                        break;
                    }
                case "stop":
                    {
                        AlarmTools.StopAlarm(name);
                        break;
                    }
                case "list":
                    {
                        foreach (var alarm in AlarmTools.alarms)
                        {
                            SeparatorWriterColor.WriteSeparator(alarm.Key, true);
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

    }
}
