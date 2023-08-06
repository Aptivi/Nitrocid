
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Drivers;
using KS.Drivers.Encryption;
using KS.Users;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Users
{

    [TestFixture]
    public class UserManagementTests
    {

        /// <summary>
        /// Tests user addition
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestAddUser()
        {
            UserManagement.AddUser("Account1");
            UserManagement.AddUser("Account2", "password");
            UserManagement.UserExists("Account1").ShouldBeTrue();
            UserManagement.UserExists("Account2").ShouldBeTrue();
        }

        /// <summary>
        /// Tests username change
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestChangeUser() =>
            UserManagement.TryChangeUsername("Account2", "Account3").ShouldBeTrue();

        /// <summary>
        /// Tests listing all users
        /// </summary>
        [Test]
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
        /// Tests removing user
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestRemoveUser()
        {
            UserManagement.TryRemoveUser("Account1").ShouldBeTrue();
        }

        /// <summary>
        /// Tests selecting user
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestSelectUser()
        {
            string user = UserManagement.SelectUser(1);
            user.ShouldBe("root");
        }

        /// <summary>
        /// Tests getting user
        /// </summary>
        [Test]
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
            user.Admin.ShouldBeTrue();
            user.Anonymous.ShouldBeFalse();
            user.Disabled.ShouldBeFalse();
            user.CustomSettings.ShouldNotBeNull();
            user.CustomSettings.ShouldBeEmpty();
            UserManagement.GetUserDollarSign("root").ShouldBe("#");
            UserManagement.GetUserDollarSign("Account3").ShouldBe("$");
        }

        /// <summary>
        /// Tests getting index of user
        /// </summary>
        [Test]
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
            user.Admin.ShouldBeTrue();
            user.Anonymous.ShouldBeFalse();
            user.Disabled.ShouldBeFalse();
            user.CustomSettings.ShouldNotBeNull();
            user.CustomSettings.ShouldBeEmpty();
            UserManagement.GetUserDollarSign("root").ShouldBe("#");
            UserManagement.GetUserDollarSign("Account3").ShouldBe("$");
        }

        /// <summary>
        /// Tests validating username
        /// </summary>
        [Test]
        [TestCase("myvalidname", ExpectedResult = true)]
        [TestCase("MyValidName", ExpectedResult = true)]
        [TestCase("My!Valid?Name", ExpectedResult = false)]
        [TestCase("My Valid Name", ExpectedResult = false)]
        [TestCase("", ExpectedResult = false)]
        [Description("Management")]
        public bool TestValidateUsername(string name) =>
           UserManagement.ValidateUsername(name, false);

    }
}
