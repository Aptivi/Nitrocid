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

using Nitrocid.Base.Drivers;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Extras.Hashes.Drivers;
using Nitrocid.Base.Kernel.Extensions;

namespace Nitrocid.Extras.Hashes
{
    internal class HashesInit : IAddon
    {
        private readonly CRC32 singletonCrc32 = new();
        private readonly CRC32C singletonCrc32C = new();
        private readonly MD5 singletonMd5 = new();
        private readonly SHA1 singletonSha1 = new();
        private readonly SHA256Enhanced singletonSha256E = new();
        private readonly SHA384 singletonSha384 = new();
        private readonly SHA384Enhanced singletonSha384E = new();
        private readonly SHA512Enhanced singletonSha512E = new();

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasHashes);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasHashes);

        public ModLoadPriority AddonType => ModLoadPriority.Optional;

        public void FinalizeAddon()
        { }

        public void StartAddon()
        {
            DriverHandler.RegisterBaseDriver<IEncryptionDriver>(singletonCrc32);
            DriverHandler.RegisterBaseDriver<IEncryptionDriver>(singletonCrc32C);
            DriverHandler.RegisterBaseDriver<IEncryptionDriver>(singletonMd5);
            DriverHandler.RegisterBaseDriver<IEncryptionDriver>(singletonSha1);
            DriverHandler.RegisterBaseDriver<IEncryptionDriver>(singletonSha256E);
            DriverHandler.RegisterBaseDriver<IEncryptionDriver>(singletonSha384);
            DriverHandler.RegisterBaseDriver<IEncryptionDriver>(singletonSha384E);
            DriverHandler.RegisterBaseDriver<IEncryptionDriver>(singletonSha512E);
        }

        public void StopAddon()
        {
            DriverHandler.UnregisterBaseDriver<IEncryptionDriver>(singletonCrc32.DriverName);
            DriverHandler.UnregisterBaseDriver<IEncryptionDriver>(singletonCrc32C.DriverName);
            DriverHandler.UnregisterBaseDriver<IEncryptionDriver>(singletonMd5.DriverName);
            DriverHandler.UnregisterBaseDriver<IEncryptionDriver>(singletonSha1.DriverName);
            DriverHandler.UnregisterBaseDriver<IEncryptionDriver>(singletonSha256E.DriverName);
            DriverHandler.UnregisterBaseDriver<IEncryptionDriver>(singletonSha384.DriverName);
            DriverHandler.UnregisterBaseDriver<IEncryptionDriver>(singletonSha384E.DriverName);
            DriverHandler.UnregisterBaseDriver<IEncryptionDriver>(singletonSha512E.DriverName);
        }
    }
}
