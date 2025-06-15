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

using System;
using FluentFTP;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ShellPacks.Tools.Filesystem;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.ShellPacks.Shells.FTP.Commands
{
    /// <summary>
    /// Gets remote file checksum
    /// </summary>
    /// <remarks>
    /// If you want to get a remote file checksum, use this command.
    /// </remarks>
    class SumFileCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string RemoteFile = parameters.ArgumentsList[0];
            string Hash = parameters.ArgumentsList[1];

            // Check to see if hash is found
            if (Enum.IsDefined(typeof(FtpHashAlgorithm), Hash))
            {
                var HashResult = FTPHashing.FTPGetHash(RemoteFile, (FtpHashAlgorithm)Convert.ToInt32(Enum.Parse(typeof(FtpHashAlgorithm), Hash)));
                TextWriterColor.Write(HashResult.Value);
                return 0;
            }
            else
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_INVALIDENCRYPTION"), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.FTPFilesystem);
            }
        }

    }
}
