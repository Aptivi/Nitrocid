
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

using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Network.Base;
using KS.Network.Base.Transfer;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Download a file
    /// </summary>
    /// <remarks>
    /// This command downloads a file from the website to a file, preserving the file name. This is currently very basic, but it will be expanded in future releases.
    /// </remarks>
    class Get_Command : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            int RetryCount = 1;
            string URL = ListArgsOnly[0];
            string outputPath = SwitchManager.GetSwitchValue(ListSwitchesOnly, "-outputpath");
            DebugWriter.WriteDebug(DebugLevel.I, "URL: {0}", URL);
            while (!(RetryCount > NetworkTools.DownloadRetries))
            {
                try
                {
                    if (!(URL.StartsWith("ftp://") | URL.StartsWith("ftps://") | URL.StartsWith("ftpes://")))
                    {
                        if (!string.IsNullOrEmpty(URL))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Downloading from {0}..."), URL);
                            if (string.IsNullOrEmpty(outputPath))
                            {
                                // Use the current output path
                                if (NetworkTransfer.DownloadFile(ListArgsOnly[0]))
                                    TextWriterColor.Write(Translate.DoTranslation("Download has completed."));
                            }
                            else
                            {
                                // Use the custom path
                                outputPath = Filesystem.NeutralizePath(outputPath);
                                if (NetworkTransfer.DownloadFile(ListArgsOnly[0], outputPath))
                                    TextWriterColor.Write(Translate.DoTranslation("Download has completed."));
                            }
                        }
                        else
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Specify the address"), true, KernelColorType.Error);
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Please use \"ftp\" if you are going to download files from the FTP server."), true, KernelColorType.Error);
                    }
                    return;
                }
                catch (Exception ex)
                {
                    NetworkTools.TransferFinished = false;
                    TextWriterColor.Write(Translate.DoTranslation("Download failed in try {0}: {1}"), true, KernelColorType.Error, RetryCount, ex.Message);
                    RetryCount += 1;
                    DebugWriter.WriteDebug(DebugLevel.I, "Try count: {0}", RetryCount);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }

    }
}
