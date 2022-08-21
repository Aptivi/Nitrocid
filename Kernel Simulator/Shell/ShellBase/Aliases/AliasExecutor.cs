using Extensification.StringExts;
using KS.Misc.Writers.DebugWriters;

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

using KS.Network.RemoteDebug;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.ShellBase.Aliases
{
    static class AliasExecutor
    {

        /// <summary>
        /// Translates alias to actual command, preserving arguments
        /// </summary>
        /// <param name="aliascmd">Specifies the alias with arguments</param>
        public static void ExecuteAlias(string aliascmd, ShellType ShellType)
        {
            var AliasesList = AliasManager.GetAliasesListFromType(ShellType);
            string FirstWordCmd = aliascmd.SplitEncloseDoubleQuotes(" ")[0];
            string actualCmd = aliascmd.Replace(FirstWordCmd, AliasesList[FirstWordCmd]);
            DebugWriter.Wdbg(DebugLevel.I, "Actual command: {0}", actualCmd);
            var Params = new GetCommand.ExecuteCommandThreadParameters(actualCmd, ShellType, null);
            var StartCommandThread = ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].ShellCommandThread;
            StartCommandThread.Start(Params);
            StartCommandThread.Wait();
            StartCommandThread.Stop();
        }

        /// <summary>
        /// Executes the remote debugger alias
        /// </summary>
        /// <param name="aliascmd">Aliased command with arguments</param>
        /// <param name="SocketStream">A socket stream writer</param>
        /// <param name="Address">IP Address</param>
        public static void ExecuteRDAlias(string aliascmd, System.IO.StreamWriter SocketStream, string Address)
        {
            string FirstWordCmd = aliascmd.Split(' ')[0];
            string actualCmd = aliascmd.Replace(FirstWordCmd, AliasManager.RemoteDebugAliases[FirstWordCmd]);
            RemoteDebugCmd.ParseCmd(actualCmd, SocketStream, Address);
        }

    }
}