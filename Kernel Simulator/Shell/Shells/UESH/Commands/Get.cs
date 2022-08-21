
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

using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Network;
using KS.Network.Transfer;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Download a file
    /// </summary>
    /// <remarks>
    /// This command downloads a file from the website to a file, preserving the file name. This is currently very basic, but it will be expanded in future releases.
    /// </remarks>
    class Get_Command : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            int RetryCount = 1;
            string URL = ListArgsOnly[0];
            DebugWriter.Wdbg(DebugLevel.I, "URL: {0}", URL);
            while (!(RetryCount > NetworkTools.DownloadRetries))
            {
                try
                {
                    if (!(URL.StartsWith("ftp://") | URL.StartsWith("ftps://") | URL.StartsWith("ftpes://")))
                    {
                        if (!URL.StartsWith(" "))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Downloading from {0}..."), true, ColorTools.ColTypes.Neutral, URL);
                            if (NetworkTransfer.DownloadFile(ListArgsOnly[0]))
                            {
                                TextWriterColor.Write(Translate.DoTranslation("Download has completed."), true, ColorTools.ColTypes.Neutral);
                            }
                        }
                        else
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Specify the address"), true, ColorTools.ColTypes.Error);
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Please use \"ftp\" if you are going to download files from the FTP server."), true, ColorTools.ColTypes.Error);
                    }
                    return;
                }
                catch (Exception ex)
                {
                    NetworkTools.TransferFinished = false;
                    TextWriterColor.Write(Translate.DoTranslation("Download failed in try {0}: {1}"), true, ColorTools.ColTypes.Error, RetryCount, ex.Message);
                    RetryCount += 1;
                    DebugWriter.Wdbg(DebugLevel.I, "Try count: {0}", RetryCount);
                    DebugWriter.WStkTrc(ex);
                }
            }
        }

    }
}