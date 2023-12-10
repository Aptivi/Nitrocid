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

using System;
using FluentFTP;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using Nitrocid.Extras.FtpShell.Tools.Filesystem;

namespace Nitrocid.Extras.FtpShell.FTP.Commands
{
    /// <summary>
    /// Gets remote files in directory checksum
    /// </summary>
    /// <remarks>
    /// If you want to get a remote files in directory checksum, use this command.
    /// </remarks>
    class SumFilesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string RemoteDirectory = parameters.ArgumentsList[0];
            string Hash = parameters.ArgumentsList[1];

            // Check to see if hash is found
            if (Enum.IsDefined(typeof(FtpHashAlgorithm), Hash))
            {
                var HashResults = FTPHashing.FTPGetHashes(RemoteDirectory, (FtpHashAlgorithm)Convert.ToInt32(Enum.Parse(typeof(FtpHashAlgorithm), Hash)));
                foreach (string Filename in HashResults.Keys)
                {
                    TextWriterColor.WriteKernelColor("- " + Filename + ": ", false, KernelColorType.ListEntry);
                    TextWriterColor.WriteKernelColor(HashResults[Filename].Value, true, KernelColorType.ListValue);
                }
                return 0;
            }
            else
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Invalid encryption algorithm."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.FTPFilesystem;
            }
        }

    }
}
