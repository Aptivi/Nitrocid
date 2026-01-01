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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Aliases;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using System;

namespace Nitrocid.Shell.Shells.UESH.Commands
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
            string mode = parameters.ArgumentsList[0];
            string type = parameters.ArgumentsList[1];
            string aliasCmd = parameters.ArgumentsList[2];
            bool shouldSave = false;
            if (parameters.ArgumentsList.Length > 3)
            {
                string destCmd = parameters.ArgumentsList[3];
                if (mode == "add" & ShellManager.AvailableShells.ContainsKey(type))
                {
                    // User tries to add an alias.
                    try
                    {
                        AliasManager.AddAlias(destCmd, aliasCmd, type);
                        shouldSave = true;
                        TextWriters.Write(Translate.DoTranslation("You can now run '{0}' as '{1}'!"), KernelColorType.Success, aliasCmd, destCmd);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to add alias. {0}", ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                        TextWriters.Write(ex.Message, KernelColorType.Error);
                    }
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Invalid type {0}."), KernelColorType.Error, type);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.AliasNoSuchType);
                }
            }
            else if (parameters.ArgumentsList.Length == 3)
            {
                if (parameters.ArgumentsList[0] == "rem" & ShellManager.AvailableShells.ContainsKey(type))
                {
                    // User tries to remove an alias
                    try
                    {
                        AliasManager.RemoveAlias(aliasCmd, type);
                        shouldSave = true;
                        TextWriters.Write(Translate.DoTranslation("Removed alias {0} successfully."), KernelColorType.Success, aliasCmd);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to remove alias. Stack trace written using WStkTrc(). {0}", ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                        TextWriters.Write(ex.Message, KernelColorType.Error);
                    }
                }
                else
                {
                    TextWriters.Write(Translate.DoTranslation("Invalid type {0}."), KernelColorType.Error, type);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.AliasNoSuchType);
                }
            }

            // Save all aliases if the addition or the removal is successful
            if (shouldSave)
                AliasManager.SaveAliases();
            return 0;
        }

    }
}
