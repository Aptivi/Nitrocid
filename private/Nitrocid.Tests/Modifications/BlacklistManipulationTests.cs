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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Nitrocid.Base.Files;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Extensions;
using System.Collections.Generic;

namespace Nitrocid.Tests.Modifications
{

    [TestClass]
    public class BlacklistManipulationTests
    {

        /// <summary>
        /// Tests adding a mod to the blacklist
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestAddModToBlacklist()
        {
            var modManagerType = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasMods, "Nitrocid.Extras.Mods.Modifications.ModManager");
            InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasMods, "AddModToBlacklist", modManagerType, "MaliciousMod.dll");
            var blacklistedMods = (IEnumerable<string>?)InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasMods, "GetBlacklistedMods", modManagerType) ?? [];
            blacklistedMods.ShouldContain(FilesystemTools.NeutralizePath("MaliciousMod.dll", PathsManagement.GetKernelPath(KernelPathType.Mods)));
        }

        /// <summary>
        /// Tests removing a mod from the blacklist
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestRemoveModFromBlacklist()
        {
            var modManagerType = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasMods, "Nitrocid.Extras.Mods.Modifications.ModManager");
            InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasMods, "RemoveModFromBlacklist", modManagerType, "MaliciousMod.dll");
            var blacklistedMods = (IEnumerable<string>?)InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasMods, "GetBlacklistedMods", modManagerType) ?? [];
            blacklistedMods.ShouldNotContain(FilesystemTools.NeutralizePath("MaliciousMod.dll", PathsManagement.GetKernelPath(KernelPathType.Mods)));
        }

    }
}
