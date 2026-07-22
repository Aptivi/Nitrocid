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

using Nitrocid.Drivers;
using Nitrocid.Drivers.Encryption;
using Nitrocid.Kernel.Extensions;

namespace Nitrocid.Extras.Crc32
{
    internal class Crc32Init : IAddon
    {
        private readonly CRC32 singleton = new();

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasCrc32);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasCrc32);

        public void FinalizeAddon()
        { }

        public void StartAddon() =>
            DriverHandler.RegisterBaseDriver<IEncryptionDriver>(singleton);

        public void StopAddon() =>
            DriverHandler.UnregisterBaseDriver<IEncryptionDriver>(singleton.DriverName);
    }
}
