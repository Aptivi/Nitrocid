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

using Nitrocid.Drivers;
using Nitrocid.Drivers.Encryption;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Modifications;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Nitrocid.Extras.Crc32Castagnoli
{
    internal class Crc32CInit : IAddon
    {
        private readonly CRC32C singleton = new();

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasCrc32C);

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon() =>
            DriverHandler.RegisterBaseDriver<IEncryptionDriver>(singleton);

        void IAddon.StopAddon() =>
            DriverHandler.UnregisterBaseDriver<IEncryptionDriver>(singleton.DriverName);
    }
}
