//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Transfer;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

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
        public override string Command =>
            "get";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_COMMAND_GET_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
             [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "url", new()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GET_ARGUMENT_URL_DESC"
                    })
                ],
                [
                    new SwitchInfo("outputpath", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_GET_SWITCH_OUTPUTPATH_DESC", new SwitchOptions()
                    {
                        ArgumentsRequired = true
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            int retryCount = 1;
            string url = parameters.ArgumentsList[0];
            string outputPath = SwitchManager.GetSwitchValue(parameters.SwitchesList, "-outputpath");
            int failCode = 0;
            DebugWriter.WriteDebug(DebugLevel.I, "URL: {0}", vars: [url]);
            while (retryCount <= Config.MainConfig.DownloadRetries)
            {
                try
                {
                    if (!url.StartsWith("ftp://") || !url.StartsWith("ftps://") || !url.StartsWith("ftpes://"))
                    {
                        if (!string.IsNullOrEmpty(url))
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GET_DOWNLOADING"), url);
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
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GET_FAILEDTRY"), true, ThemeColorType.Error, retryCount, ex.Message);
                    retryCount += 1;
                    DebugWriter.WriteDebug(DebugLevel.I, "Try count: {0}", vars: [retryCount]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    failCode = ex.GetHashCode();
                }
            }
            return failCode;
        }

    }
}
