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

#if NKS_EXTENSIONS
using System.Diagnostics;

namespace Nitrocid.Base.Kernel.Extensions
{
    [DebuggerDisplay("Addon name = {AddonName}")]
    internal class AddonInfo
    {
        internal string AddonName =>
            Addon is not null ? Addon.AddonName : "";

        internal string AddonDisplayName =>
            Addon is not null ? Addon.AddonTranslatedName : "";

        internal IAddon Addon { get; }

        internal AddonInfo(IAddon addon)
        {
            Addon = addon;
        }
    }
}
#endif
