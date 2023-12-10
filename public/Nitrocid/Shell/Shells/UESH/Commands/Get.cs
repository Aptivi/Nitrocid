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
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Network.Base;
using KS.Network.Base.Transfer;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Switches;

namespace KS.Shell.Shells.UESH.Commands
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
            DebugWriter.WriteDebug(DebugLevel.I, "URL: {0}", URL);
            while (RetryCount <= NetworkTools.DownloadRetries)
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
                            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Specify the address"), true, KernelColorType.Error);
                            return 10000 + (int)KernelExceptionType.HTTPNetwork;
                        }
                    }
                    else
                    {
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Please use \"ftp\" if you are going to download files from the FTP server."), true, KernelColorType.Error);
                        return 10000 + (int)KernelExceptionType.HTTPNetwork;
                    }
                }
                catch (Exception ex)
                {
                    NetworkTools.TransferFinished = false;
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Download failed in try {0}: {1}"), true, KernelColorType.Error, RetryCount, ex.Message);
                    RetryCount += 1;
                    DebugWriter.WriteDebug(DebugLevel.I, "Try count: {0}", RetryCount);
                    DebugWriter.WriteDebugStackTrace(ex);
                    failCode = ex.GetHashCode();
                }
            }
            return failCode;
        }

    }
}
