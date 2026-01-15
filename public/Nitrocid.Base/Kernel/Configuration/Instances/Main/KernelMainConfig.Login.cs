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
using Nitrocid.Base.Users.Login.Handlers;

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Main kernel configuration instance
    /// </summary>
    public partial class KernelMainConfig : BaseKernelConfig
    {
        /// <summary>
        /// Show Message of the Day before displaying login screen.
        /// </summary>
        public bool ShowMOTD { get; set; } = true;
        /// <summary>
        /// Clear screen before displaying login screen.
        /// </summary>
        public bool ClearOnLogin { get; set; }
        /// <summary>
        /// The kernel host name to communicate with the rest of the computers
        /// </summary>
        public string HostName { get; set; } = "kernel";
        /// <summary>
        /// Shows available users if enabled
        /// </summary>
        public bool ShowAvailableUsers { get; set; } = true;
        /// <summary>
        /// Which file is the MOTD text file? Write an absolute path to the text file
        /// </summary>
        public string MotdFilePath { get; set; } = PathsManagement.GetKernelPath(KernelPathType.MOTD);
        /// <summary>
        /// Which file is the MAL text file? Write an absolute path to the text file
        /// </summary>
        public string MalFilePath { get; set; } = PathsManagement.GetKernelPath(KernelPathType.MAL);
        /// <summary>
        /// Write how you want your login prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string UsernamePrompt { get; set; } = "";
        /// <summary>
        /// Write how you want your password prompt to be. Leave blank to use default style. Placeholders are parsed
        /// </summary>
        public string PasswordPrompt { get; set; } = "";
        /// <summary>
        /// Shows Message of the Day after displaying login screen
        /// </summary>
        public bool ShowMAL { get; set; } = true;
        /// <summary>
        /// Includes the anonymous users in the list
        /// </summary>
        public bool IncludeAnonymous { get; set; }
        /// <summary>
        /// Includes the disabled users in the list
        /// </summary>
        public bool IncludeDisabled { get; set; }
        /// <summary>
        /// Current login handler.
        /// </summary>
        public string CurrentLoginHandler
        {
            get => LoginHandlerTools.CurrentHandlerName;
            set => LoginHandlerTools.CurrentHandlerName = value;
        }
    }
}
