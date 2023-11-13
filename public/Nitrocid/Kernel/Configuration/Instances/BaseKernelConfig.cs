﻿//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Kernel.Configuration.Settings;
using Newtonsoft.Json;

namespace KS.Kernel.Configuration.Instances
{
    /// <summary>
    /// Base kernel configuration. Left blank to allow flexibility.
    /// </summary>
    public class BaseKernelConfig : IKernelConfig
    {
        /// <summary>
        /// List of settings entries
        /// </summary>
        [JsonIgnore]
        public virtual SettingsEntry[] SettingsEntries { get; }
    }
}