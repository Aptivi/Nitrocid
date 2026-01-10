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
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Files;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Network;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Network.Transfer;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
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
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GET_DOWNLOADING"), URL);
                            if (string.IsNullOrEmpty(outputPath))
                            {
                                // Use the current output path
                                if (NetworkTransfer.DownloadFile(parameters.ArgumentsList[0]))
                                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GET_COMPLETE"));
                            }
                            else
                            {
                                // Use the custom path
                                outputPath = FilesystemTools.NeutralizePath(outputPath);
                                if (NetworkTransfer.DownloadFile(parameters.ArgumentsList[0], outputPath))
                                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GET_COMPLETE"));
                            }
                            return 0;
                        }
                        else
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GET_NEEDSADDRESS"), true, ThemeColorType.Error);
                            return KernelExceptionTools.GetErrorCode(KernelExceptionType.HTTPNetwork);
                        }
                    }
                    else
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GET_NEEDSFTP"), true, ThemeColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.HTTPNetwork);
                    }
                }
                catch (Exception ex)
                {
                    NetworkTools.TransferFinished = false;
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GET_FAILEDTRY"), true, ThemeColorType.Error, RetryCount, ex.Message);
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
