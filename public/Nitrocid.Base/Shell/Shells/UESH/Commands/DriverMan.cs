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
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using System;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Security.Permissions;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Manages your drivers
    /// </summary>
    /// <remarks>
    /// You can manage all your drivers installed in Nitrocid KS by this command. It allows you to set, get info, and list all your drivers.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class DriverManCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!PermissionsTools.IsPermissionGranted(PermissionTypes.RunStrictCommands) &&
                !UserManagement.CurrentUser.Flags.HasFlag(UserFlags.Administrator))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Cmd exec {0} failed: adminList(signedinusrnm) is False, strictCmds.Contains({0}) is True", vars: [parameters.CommandText]);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_NEEDSPERM"), true, ThemeColorType.Error, parameters.CommandText);
                return -4;
            }

            if (!KernelEntry.SafeMode)
            {
                PermissionsTools.Demand(PermissionTypes.ManageDrivers);
                string CommandDriver = parameters.ArgumentsList[0].ToLower();
                DriverTypes typeTerm = DriverTypes.RNG;
                string driverValue = "";

                // These command drivers require arguments to be passed, so re-check here and there. Optional arguments also lie there.
                switch (CommandDriver)
                {
                    case "change":
                        {
                            if (parameters.ArgumentsList.Length > 2)
                            {
                                typeTerm = Enum.Parse<DriverTypes>(parameters.ArgumentsList[1]);
                                driverValue = parameters.ArgumentsList[2];
                            }

                            break;
                        }
                    case "list":
                        {
                            if (parameters.ArgumentsList.Length > 1)
                                typeTerm = Enum.Parse<DriverTypes>(parameters.ArgumentsList[1]);

                            break;
                        }
                }

                // Now, the actual logic
                switch (CommandDriver)
                {
                    case "change":
                        {
                            if (DriverHandler.IsRegistered(typeTerm, driverValue))
                                DriverHandler.SetDriverSafe(typeTerm, driverValue);
                            else
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_DRIVERMAN_DRIVERNOTFOUND"), true, ThemeColorType.Error);
                            break;
                        }
                    case "list":
                        {
                            SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_DRIVERMAN_DRIVERSFOR") + $" {typeTerm}", ThemeColorsTools.GetColor(ThemeColorType.Separator));
                            foreach (var driver in DriverHandler.GetDrivers(typeTerm))
                            {
                                if (!driver.DriverInternal)
                                {
                                    TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_DRIVERMAN_NAME") + ": ", false, ThemeColorType.ListEntry);
                                    TextWriterColor.Write(driver.DriverName, true, ThemeColorType.ListValue);
                                }
                            }
                            break;
                        }
                    case "types":
                        {
                            var types = DriverHandler.knownTypes;
                            foreach (var type in types)
                            {
                                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_DRIVERMAN_TYPENAME") + ": ", false, ThemeColorType.ListEntry);
                                TextWriterColor.Write($"{type.Key.Name} [{type.Key.FullName}]", true, ThemeColorType.ListValue);
                                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_DRIVERMAN_TYPE") + ": ", false, ThemeColorType.ListEntry);
                                TextWriterColor.Write(type.Value.ToString(), true, ThemeColorType.ListValue);
                            }
                            break;
                        }

                    default:
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_INVALIDCOMMAND_BRANCHED"), true, ThemeColorType.Error, CommandDriver);
                            HelpPrint.ShowHelp("driverman");
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.DriverManagement);
                        }
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_DRIVERMAN_SAFEMODE"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.DriverManagement);
            }
            return 0;
        }

    }
}
