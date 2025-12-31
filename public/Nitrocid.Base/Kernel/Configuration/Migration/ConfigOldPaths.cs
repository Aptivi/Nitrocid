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

using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Files;
using System;
using System.Collections.Generic;

namespace Nitrocid.Base.Kernel.Configuration.Migration
{
    internal class ConfigOldPaths
    {
        // Variables
        internal static Dictionary<string, string> oldPaths = [];

        internal static string GetOldKernelPath(ConfigOldPathType PathType)
        {
            if (Enum.IsDefined(typeof(ConfigOldPathType), PathType))
                return FilesystemTools.NeutralizePath(oldPaths[PathType.ToString()]);
            else
                throw new KernelException(KernelExceptionType.InvalidKernelPath, LanguageTools.GetLocalized("NKS_FILES_PATHS_PATHTYPEINVALID"));
        }

        private static void InitOldPaths()
        {
            static void AddPath(string config, string fileName)
            {
                if (!oldPaths.ContainsKey(config))
                    oldPaths.Add(config, PathsManagement.HomePath + $"/{fileName}");
            }

            AddPath(nameof(ConfigOldPathType.Configuration), "KernelConfig.json");
            AddPath(nameof(ConfigOldPathType.Aliases), "Aliases.json");
            AddPath(nameof(ConfigOldPathType.Users), "Users.json");
            AddPath(nameof(ConfigOldPathType.FTPSpeedDial), "FTP_SpeedDial.json");
            AddPath(nameof(ConfigOldPathType.SFTPSpeedDial), "SFTP_SpeedDial.json");
            AddPath(nameof(ConfigOldPathType.DebugDevNames), "DebugDeviceNames.json");
            AddPath(nameof(ConfigOldPathType.MOTD), "MOTD.txt");
            AddPath(nameof(ConfigOldPathType.MAL), "MAL.txt");
        }

        static ConfigOldPaths() =>
            InitOldPaths();
    }
}
