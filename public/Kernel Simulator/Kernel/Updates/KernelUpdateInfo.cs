
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using KS.Kernel.Debugging;

namespace KS.Kernel.Updates
{
    /// <summary>
    /// Kernel update information class
    /// </summary>
    public class KernelUpdateInfo
    {

        /// <summary>
        /// Updated kernel version
        /// </summary>
        public Version UpdateVersion { get; private set; }
        /// <summary>
        /// Update file URL
        /// </summary>
        public Uri UpdateURL { get; private set; }

        /// <summary>
        /// Installs a new instance of class KernelUpdateInfo
        /// </summary>
        /// <param name="UpdateVer">The kernel version fetched from the update token</param>
        /// <param name="UpdateUrl">The kernel URL fetched from the update token</param>
        protected internal KernelUpdateInfo(Version UpdateVer, string UpdateUrl)
        {
            try
            {
                UpdateVersion = UpdateVer;
                UpdateURL = new Uri(UpdateUrl);
                DebugWriter.WriteDebug(DebugLevel.I, "Added new update {0} with {1} as URI", UpdateVer.ToString(), UpdateUrl);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to create new instance of update class with update {0} with {1} as URI: {2}", UpdateVer.ToString(), UpdateUrl, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

    }
}
