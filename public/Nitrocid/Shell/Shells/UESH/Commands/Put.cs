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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Files;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Nitrocid.Network;
using Nitrocid.Network.Transfer;
using Nitrocid.Kernel.Configuration;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Upload a file
    /// </summary>
    /// <remarks>
    /// This command uploads a file from the website to a file, preserving the file name. This is currently very basic, but it will be expanded in future releases.
    /// </remarks>
    class PutCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            int RetryCount = 1;
            string FileName = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            string URL = parameters.ArgumentsList[1];
            int failCode = 0;
            DebugWriter.WriteDebug(DebugLevel.I, "URL: {0}", vars: [URL]);
            while (RetryCount <= Config.MainConfig.UploadRetries)
            {
                try
                {
                    if (!URL.StartsWith("ftp://") || !URL.StartsWith("ftps://") || !URL.StartsWith("ftpes://"))
                    {
                        if (!URL.StartsWith(" "))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Uploading {0} to {1}..."), FileName, URL);
                            if (NetworkTransfer.UploadFile(FileName, URL))
                            {
                                TextWriterColor.Write(Translate.DoTranslation("Upload has completed."));
                                return 0;
                            }
                        }
                        else
                        {
                            TextWriters.Write(Translate.DoTranslation("Specify the address"), true, KernelColorType.Error);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.Network);
                        }
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("Please use \"ftp\" if you are going to upload files to the FTP server."), true, KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.Network);
                    }
                }
                catch (Exception ex)
                {
                    NetworkTools.TransferFinished = false;
                    TextWriters.Write(Translate.DoTranslation("Upload failed in try {0}: {1}"), true, KernelColorType.Error, RetryCount, ex.Message);
                    RetryCount += 1;
                    DebugWriter.WriteDebug(DebugLevel.I, "Try count: {0}", vars: [RetryCount]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    failCode = ex.GetHashCode();
                }
            }
            return failCode;
        }

    }
}
