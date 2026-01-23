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

using System;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Files;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Nitrocid.Network;
using Nitrocid.Network.Transfer;
using Nitrocid.Kernel.Configuration;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Download a file
    /// </summary>
    /// <remarks>
    /// This command downloads a file from the website to a file, preserving the file name. This is currently very basic, but it will be expanded in future releases.
    /// </remarks>
    class GetCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            int RetryCount = 1;
            string URL = parameters.ArgumentsList[0];
            string outputPath = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-outputpath");
            int failCode = 0;
            DebugWriter.WriteDebug(DebugLevel.I, "URL: {0}", vars: [URL]);
            while (RetryCount <= Config.MainConfig.DownloadRetries)
            {
                try
                {
                    if (!URL.StartsWith("ftp://") || !URL.StartsWith("ftps://") || !URL.StartsWith("ftpes://"))
                    {
                        if (!string.IsNullOrEmpty(URL))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Downloading from {0}..."), URL);
                            if (string.IsNullOrEmpty(outputPath))
                            {
                                // Use the current output path
                                if (NetworkTransfer.DownloadFile(parameters.ArgumentsList[0]))
                                    TextWriterColor.Write(Translate.DoTranslation("Download has completed."));
                            }
                            else
                            {
                                // Use the custom path
                                outputPath = FilesystemTools.NeutralizePath(outputPath);
                                if (NetworkTransfer.DownloadFile(parameters.ArgumentsList[0], outputPath))
                                    TextWriterColor.Write(Translate.DoTranslation("Download has completed."));
                            }
                            return 0;
                        }
                        else
                        {
                            TextWriters.Write(Translate.DoTranslation("Specify the address"), true, KernelColorType.Error);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.HTTPNetwork);
                        }
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("Please use \"ftp\" if you are going to download files from the FTP server."), true, KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.HTTPNetwork);
                    }
                }
                catch (Exception ex)
                {
                    NetworkTools.TransferFinished = false;
                    TextWriters.Write(Translate.DoTranslation("Download failed in try {0}: {1}"), true, KernelColorType.Error, RetryCount, ex.Message);
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
