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
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Users
{

    [TestClass]
    public class UserManagementTests
    {

        private static bool inited = false;

        /// <summary>
        /// Tests user addition
        /// </summary>
        [ClassInitialize]
        [Description("Management")]
#pragma warning disable IDE0060
        public static void TestAddUser(TestContext tc)
#pragma warning restore IDE0060
        {
            if (inited)
                return;
            inited = true;
            UserManagement.AddUser("Account1");
            UserManagement.AddUser("Account2", "password");
            UserManagement.UserExists("Account1").ShouldBeTrue();
            UserManagement.UserExists("Account2").ShouldBeTrue();
        }

        /// <summary>
        /// Tests username change
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestChangeUser() =>
            UserManagement.TryChangeUsername("Account2", "Account3").ShouldBeTrue();

        /// <summary>
        /// Tests listing all users
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestListAllUsers()
        {
            var list = UserManagement.ListAllUsers();
            list.ShouldNotBeNull();
            list.ShouldNotBeEmpty();
            list.ShouldContain("root");
            list.ShouldContain("Account1");
            list.ShouldContain("Account3");
        }

        /// <summary>
        /// Tests locking a user
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestLockUser()
        {
            UserManagement.LockUser("Account3");
            UserManagement.IsLocked("Account3").ShouldBeTrue();
            Should.Throw(() => UserManagement.RemoveUser("Account3"), typeof(KernelException));
            UserManagement.UnlockUser("Account3");
            UserManagement.IsLocked("Account3").ShouldBeFalse();
        }

        /// <summary>
        /// Tests removing user
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestRemoveUser()
        {
            UserManagement.TryRemoveUser("Account1").ShouldBeTrue();
        }

        /// <summary>
        /// Tests selecting user
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestSelectUser()
        {
            string user = UserManagement.SelectUser(1);
            user.ShouldBe("root");
        }

        /// <summary>
        /// Tests getting user
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetUser()
        {
            var user = UserManagement.GetUser("root");
            var hashRegex = DriverHandler.GetDriver<IEncryptionDriver>("SHA256").HashRegex;
            user.ShouldNotBeNull();
            user.Username.ShouldNotBeNullOrEmpty();
            user.Username.ShouldBe("root");
            user.FullName.ShouldNotBeNullOrEmpty();
            user.FullName.ShouldBe("System Account");
            user.Password.ShouldNotBeNullOrEmpty();
            user.Password.ShouldBe(Encryption.GetEncryptedString("", "SHA256"));
            hashRegex.IsMatch(user.Password).ShouldBeTrue();
            user.Permissions.ShouldNotBeNull();
            user.Permissions.ShouldBeEmpty();
            user.Groups.ShouldNotBeNull();
            user.Groups.ShouldBeEmpty();
            user.PreferredLanguage.ShouldBeNullOrEmpty();
            user.PreferredCulture.ShouldBeNullOrEmpty();
            user.Flags.HasFlag(UserFlags.Administrator).ShouldBeTrue();
            user.Flags.HasFlag(UserFlags.Anonymous).ShouldBeFalse();
            user.Flags.HasFlag(UserFlags.Disabled).ShouldBeFalse();
            user.CustomSettings.ShouldNotBeNull();
            user.CustomSettings.ShouldBeEmpty();
            UserManagement.GetUserDollarSign("root").ShouldBe("#");
            UserManagement.GetUserDollarSign("Account3").ShouldBe("$");
        }

        /// <summary>
        /// Tests getting index of user
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetUserIndex()
        {
            int userIdx = UserManagement.GetUserIndex("root");
            var userName = UserManagement.SelectUser(userIdx + 1);
            var user = UserManagement.GetUser(userName);
            var hashRegex = DriverHandler.GetDriver<IEncryptionDriver>("SHA256").HashRegex;
            user.ShouldNotBeNull();
            user.Username.ShouldNotBeNullOrEmpty();
            user.Username.ShouldBe("root");
            user.FullName.ShouldNotBeNullOrEmpty();
            user.FullName.ShouldBe("System Account");
            user.Password.ShouldNotBeNullOrEmpty();
            user.Password.ShouldBe(Encryption.GetEncryptedString("", "SHA256"));
            hashRegex.IsMatch(user.Password).ShouldBeTrue();
            user.Permissions.ShouldNotBeNull();
            user.Permissions.ShouldBeEmpty();
            user.Groups.ShouldNotBeNull();
            user.Groups.ShouldBeEmpty();
            user.PreferredLanguage.ShouldBeNullOrEmpty();
            user.PreferredCulture.ShouldBeNullOrEmpty();
            user.Flags.HasFlag(UserFlags.Administrator).ShouldBeTrue();
            user.Flags.HasFlag(UserFlags.Anonymous).ShouldBeFalse();
            user.Flags.HasFlag(UserFlags.Disabled).ShouldBeFalse();
            user.CustomSettings.ShouldNotBeNull();
            user.CustomSettings.ShouldBeEmpty();
            UserManagement.GetUserDollarSign("root").ShouldBe("#");
            UserManagement.GetUserDollarSign("Account3").ShouldBe("$");
        }

        /// <summary>
        /// Tests validating username
        /// </summary>
        [TestMethod]
        [DataRow("myvalidname", true)]
        [DataRow("MyValidName", true)]
        [DataRow("My!Valid?Name", false)]
        [DataRow("My Valid Name", false)]
        [DataRow("", false)]
        [Description("Management")]
        public void TestValidateUsername(string name, bool expected)
        {
            bool actual = UserManagement.ValidateUsername(name, false);
            actual.ShouldBe(expected);
        }
    }
}
