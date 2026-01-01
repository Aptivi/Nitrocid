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

using Nitrocid.Drivers.Encryption;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Security.Permissions;
using Nitrocid.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;

namespace Nitrocid.Tests.Security.Permissions
{
    [TestClass]
    public class PermissionManagerTests
    {

        private static readonly UserInfo rootUser = new("root", Encryption.GetEncryptedString("", "SHA256"), [], "System account", "", [], UserFlags.Administrator, []);

        /// <summary>
        /// Add necessary user for testing
        /// </summary>
        [ClassInitialize]
#pragma warning disable IDE0060
        public static void AddNecessaryUser(TestContext tc)
#pragma warning restore IDE0060
        {
            UserManagement.AddUser("account");
            UserManagement.UserExists("account").ShouldBeTrue();
        }

        /// <summary>
        /// Tests querying permission grant for root
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestIsPermissionGrantedForRoot() =>
            PermissionsTools.IsPermissionGranted("root", PermissionTypes.ManageUsers).ShouldBeTrue();

        /// <summary>
        /// Tests querying permission grant for normal user
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestIsPermissionGrantedForNormalUser() =>
            PermissionsTools.IsPermissionGranted("account", PermissionTypes.ManageUsers).ShouldBeFalse();

        /// <summary>
        /// Tests demand for root
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestDemandForRoot() =>
            Should.NotThrow(() => PermissionsTools.Demand(PermissionTypes.ManageUsers));

        /// <summary>
        /// Tests demand for normal user
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestDemandForNormalUser()
        {
            UserManagement.CurrentUserInfo = UserManagement.Users.Single((ui) => ui.Username == "account");
            Should.Throw(() => PermissionsTools.Demand(PermissionTypes.ManageUsers), typeof(KernelException));
            UserManagement.CurrentUserInfo = rootUser;
        }

        /// <summary>
        /// Remove necessary user for testing
        /// </summary>
        [ClassCleanup]
        public static void RemoveNecessaryUser() =>
            UserManagement.TryRemoveUser("account").ShouldBeTrue();

    }
}
