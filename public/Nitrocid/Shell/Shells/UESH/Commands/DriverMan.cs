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

using System.IO;
using Nitrocid.Kernel;
using Nitrocid.Shell.ShellBase.Help;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Files;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Writers.FancyWriters;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Security.Permissions;
using Nitrocid.Files.Paths;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Nitrocid.Drivers;
using System;

namespace Nitrocid.Shell.Shells.UESH.Commands
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
            if (!KernelEntry.SafeMode)
            {
                PermissionsTools.Demand(PermissionTypes.ManageDrivers);
                string CommandDriver = parameters.ArgumentsList[0].ToLower();
                DriverTypes typeTerm = DriverTypes.RNG;

                // These command drivers require arguments to be passed, so re-check here and there. Optional arguments also lie there.
                switch (CommandDriver)
                {
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
                    case "list":
                        {
                            foreach (string Driver in DriverHandler.GetDrivers(typeTerm).Keys)
                            {
                                var driver = DriverHandler.GetDriver(typeTerm, Driver);
                                if (!driver.DriverInternal)
                                {
                                    SeparatorWriterColor.WriteSeparator(Driver, true);
                                    TextWriters.Write("- " + Translate.DoTranslation("Driver name") + ": ", false, KernelColorType.ListEntry);
                                    TextWriters.Write(driver.DriverName, true, KernelColorType.ListValue);
                                }
                            }
                            break;
                        }
                    case "types":
                        {
                            var types = DriverHandler.knownTypes;
                            foreach (var type in types)
                            {
                                TextWriters.Write("- " + Translate.DoTranslation("Driver type name") + ": ", false, KernelColorType.ListEntry);
                                TextWriters.Write($"{type.Key.Name} [{type.Key.FullName}]", true, KernelColorType.ListValue);
                                TextWriters.Write("- " + Translate.DoTranslation("Driver type") + ": ", false, KernelColorType.ListEntry);
                                TextWriters.Write(type.Value.ToString(), true, KernelColorType.ListValue);
                            }
                            break;
                        }

                    default:
                        {
                            TextWriters.Write(Translate.DoTranslation("Invalid command {0}. Check the usage below:"), true, KernelColorType.Error, CommandDriver);
                            HelpPrint.ShowHelp("driverman");
                            return 10000 + (int)KernelExceptionType.DriverManagement;
                        }
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Driver management is disabled in safe mode."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.DriverManagement;
            }
            return 0;
        }

    }
}
