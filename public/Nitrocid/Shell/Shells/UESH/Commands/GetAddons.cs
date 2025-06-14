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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Kernel.Updates;
using Nitrocid.Languages;
using Nitrocid.Network.Transfer;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
using System;
using System.IO.Compression;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Gets the addons
    /// </summary>
    /// <remarks>
    /// This command will download the addons pack from the official project site and install them to the Nitrocid directory.
    /// </remarks>
    class GetAddonsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Bail if there are addons already installed
            if (AddonTools.ListAddons().Count > 0 && !SwitchManager.ContainsSwitch(parameters.SwitchesList, "-reinstall"))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETADDONS_ALREADYINSTALLED"), KernelColorType.Progress);
                return 0;
            }

            // First, try to fetch the addons package
            KernelUpdate? addonsPackage;
            try
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETADDONS_FETCHING"), KernelColorType.Progress);
                addonsPackage = UpdateManager.FetchAddonPack();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Error trying to fetch the addon package: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETADDONS_FETCHFAILED") + $": {ex.Message}", KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.AddonManagement);
            }

            // Now, try to download the addons package
            try
            {
                if (addonsPackage is null)
                    throw new KernelException(KernelExceptionType.Unknown, LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETADDONS_OBTAINFAILED"));
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETADDONS_DOWNLOADING"), KernelColorType.Progress);
                NetworkTransfer.DownloadFile(addonsPackage.UpdateURL.ToString(), PathsManagement.AppDataPath + "/addons.zip");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Error trying to download the addon package: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETADDONS_DOWNLOADFAILED") + $": {ex.Message}", KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.AddonManagement);
            }

            // Finally, try to install the addons package
            try
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETADDONS_INSTALLING"), KernelColorType.Progress);
                ZipFile.ExtractToDirectory(PathsManagement.AppDataPath + "/addons.zip", PathsManagement.AddonsPath, true);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Error trying to install the addon package: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_GETADDONS_INSTALLFAILED") + $": {ex.Message}", KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.AddonManagement);
            }
            return 0;
        }
    }
}
