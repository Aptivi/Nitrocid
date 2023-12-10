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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Network.Base;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can change your host name to another name
    /// </summary>
    /// <remarks>
    /// If you're planning to change your host name to another name, this command is for you.
    /// <br></br>
    /// This command used to change your host name and resets it everytime you reboot the kernel, but now it stores it in the config file as soon as you change your host name.
    /// <br></br>
    /// This version of the kernel finally allows hostnames that is less than 4 characters.
    /// <br></br>
    /// This command is also useful if you're identifying multiple computers/servers, so you won't forget them.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class ChHostNameCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (string.IsNullOrEmpty(parameters.ArgumentsList[0]))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Blank host name."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Network;
            }
            else if (parameters.ArgumentsList[0].IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray()) != -1)
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Special characters are not allowed."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.Network;
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Changing from: {0} to {1}..."), NetworkTools.HostName, parameters.ArgumentsList[0]);
                NetworkTools.ChangeHostname(parameters.ArgumentsList[0]);
                return 0;
            }
        }

    }
}
