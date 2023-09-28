
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can set an alternative shortcut to the command if you want to use shorter words for long commands.
    /// </summary>
    /// <remarks>
    /// Some commands in this kernel are long, and some people doesn't write fast on computers. The alias command fixes this problem by providing the shorter terms for long commands.
    /// <br></br>
    /// You can also use this command if you plan to make scripts if the real file system will be added in the future, or if you are rushing for something and you don't have time to execute the long command.
    /// <br></br>
    /// You can add or remove the alias to the long command.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class AliasCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length > 3)
            {
                if (parameters.ArgumentsList[0] == "add" & ShellManager.AvailableShells.ContainsKey(parameters.ArgumentsList[1]))
                {
                    AliasManager.ManageAlias(parameters.ArgumentsList[0], parameters.ArgumentsList[1], parameters.ArgumentsList[2], parameters.ArgumentsList[3]);
                    return 0;
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Invalid type {0}."), true, KernelColorType.Error, parameters.ArgumentsList[1]);
                    return 10000 + (int)KernelExceptionType.AliasNoSuchType;
                }
            }
            else if (parameters.ArgumentsList.Length == 3)
            {
                if (parameters.ArgumentsList[0] == "rem" & ShellManager.AvailableShells.ContainsKey(parameters.ArgumentsList[1]))
                {
                    AliasManager.ManageAlias(parameters.ArgumentsList[0], parameters.ArgumentsList[1], parameters.ArgumentsList[2]);
                    return 0;
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Invalid type {0}."), true, KernelColorType.Error, parameters.ArgumentsList[1]);
                    return 10000 + (int)KernelExceptionType.AliasNoSuchType;
                }
            }
            return 0;
        }

    }
}
