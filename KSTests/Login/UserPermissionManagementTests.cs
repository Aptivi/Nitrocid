using KS.Login;
using NUnit.Framework;
using Shouldly;

namespace KSTests
{

    // Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

    [TestFixture]
    public class UserPermissionManagementTests
    {

        [SetUp]
        public static void AddNecessaryUser()
        {
            UserManagement.AddUser("Account");
        }

        /// <summary>
    /// Tests adding permissions to user
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestAddUserPerm()
        {
            PermissionManagement.TryAddPermission(PermissionManagement.PermissionType.Administrator, "Account").ShouldBeTrue();
            PermissionManagement.TryAddPermission(PermissionManagement.PermissionType.Disabled, "Account").ShouldBeTrue();
        }

        /// <summary>
    /// Tests removing permissions from user
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestRemoveUserPerm()
        {
            PermissionManagement.TryRemovePermission(PermissionManagement.PermissionType.Administrator, "Account").ShouldBeTrue();
            PermissionManagement.TryRemovePermission(PermissionManagement.PermissionType.Disabled, "Account").ShouldBeTrue();
        }

        /// <summary>
    /// Tests loading permissions
    /// </summary>
        [Test]
        [Description("Management")]
        public void TestLoadPermissions()
        {
            PermissionManagement.TryLoadPermissions().ShouldBeTrue();
        }

        [TearDown]
        public static void RemoveNecessaryUser()
        {
            UserManagement.RemoveUser("Account");
        }

    }
}