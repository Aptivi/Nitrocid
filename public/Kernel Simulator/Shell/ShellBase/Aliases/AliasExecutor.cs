
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Extensification.StringExts;
using KS.Kernel.Debugging;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using System.IO;

namespace KS.Shell.ShellBase.Aliases
{
    static class AliasExecutor
    {

        /// <summary>
        /// Translates alias to actual command, preserving arguments
        /// </summary>
        /// <param name="aliascmd">Specifies the alias with arguments</param>
        /// <param name="ShellType">Type of shell</param>
        /// <param name="SocketStream">A socket stream writer</param>
        /// <param name="Address">IP Address</param>
        public static void ExecuteAlias(string aliascmd, ShellType ShellType, StreamWriter SocketStream = null, string Address = "") =>
            ExecuteAlias(aliascmd, Shell.GetShellTypeName(ShellType), SocketStream, Address);

        /// <summary>
        /// Translates alias to actual command, preserving arguments
        /// </summary>
        /// <param name="aliascmd">Specifies the alias with arguments</param>
        /// <param name="ShellType">Type of shell</param>
        /// <param name="SocketStream">A socket stream writer</param>
        /// <param name="Address">IP Address</param>
        public static void ExecuteAlias(string aliascmd, string ShellType, StreamWriter SocketStream = null, string Address = "")
        {
            var AliasesList = AliasManager.GetAliasesListFromType(ShellType);

            // Get the actual command from the alias
            string FirstWordCmd = aliascmd.SplitEncloseDoubleQuotes(" ")[0];
            string actualCmd = aliascmd.Replace(FirstWordCmd, AliasesList[FirstWordCmd]);
            DebugWriter.WriteDebug(DebugLevel.I, "Actual command: {0}", actualCmd);

            // Make thread parameters.
            var Params = new GetCommand.ExecuteCommandParameters(actualCmd, ShellType, SocketStream, Address);

            // Check to see if we're on the shell or on the remote debug
            if (ShellType == "RemoteDebugShell")
            {
                // Handle the remote debug case specially
                GetCommand.ExecuteCommand(Params);
            }
            else
            {
                // Start the command thread
                var StartCommandThread = ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].ShellCommandThread;
                StartCommandThread.Start(Params);
                StartCommandThread.Wait();
                StartCommandThread.Stop();
            }
        }

    }
}
