
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using System;
using FluentFTP;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.FTP.Filesystem;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.FTP.Commands
{
    /// <summary>
    /// Gets remote files in directory checksum
    /// </summary>
    /// <remarks>
    /// If you want to get a remote files in directory checksum, use this command.
    /// </remarks>
    class FTP_SumFilesCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string RemoteDirectory = ListArgsOnly[0];
            string Hash = ListArgsOnly[1];

            // Check to see if hash is found
            if (Enum.IsDefined(typeof(FtpHashAlgorithm), Hash))
            {
                var HashResults = FTPHashing.FTPGetHashes(RemoteDirectory, (FtpHashAlgorithm)Convert.ToInt32(Enum.Parse(typeof(FtpHashAlgorithm), Hash)));
                foreach (string Filename in HashResults.Keys)
                {
                    TextWriterColor.Write("- " + Filename + ": ", false, KernelColorType.ListEntry);
                    TextWriterColor.Write(HashResults[Filename].Value, true, KernelColorType.ListValue);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Invalid encryption algorithm."), true, KernelColorType.Error);
            }
        }

    }
}