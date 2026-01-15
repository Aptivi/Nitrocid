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
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Base.Extensions;
using Terminaux.Inputs.Styles.Choice;

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Main kernel configuration instance
    /// </summary>
    public partial class KernelMainConfig : BaseKernelConfig
    {
        private string pathsToLookup = Environment.GetEnvironmentVariable("PATH") ?? "";

        /// <summary>
        /// Simplified help command for all the shells
        /// </summary>
        public bool SimHelp { get; set; }
        /// <summary>
        /// Sets the shell's current directory. Write an absolute path to any existing directory
        /// </summary>
        public string CurrentDir
        {
            get => FilesystemTools._CurrentDirectory;
            set
            {
                value = FilesystemTools.NeutralizePath(value);
                if (FilesystemTools.FolderExists(value))
                {
                    FilesystemTools._CurrentDirectory = value;
                    ConsoleFilesystem.CurrentDir = value;
                }
                else
                    throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_FILES_EXCEPTION_DIRECTORYNOTFOUND2"), value);
            }
        }
        /// <summary>
        /// Group of paths separated by the colon. It works the same as PATH. Write a full path to a folder or a folder name. When you're finished, write \"q\". Write a minus sign next to the path to remove an existing directory.
        /// </summary>
        public string PathsToLookup
        {
            get => pathsToLookup;
            set
            {
                pathsToLookup = value;
                ConsoleFilesystem.LookupPaths = value;
            }
        }
        /// <summary>
        /// Default choice output type
        /// </summary>
        public int DefaultChoiceOutputType { get; set; } = (int)ChoiceOutputType.Modern;
        /// <summary>
        /// Sets console title on command execution
        /// </summary>
        public bool SetTitleOnCommandExecution { get; set; } = true;
        /// <summary>
        /// Shows the shell count in the normal UESH shell (depending on the preset)
        /// </summary>
        public bool ShowShellCount { get; set; }
    }
}
