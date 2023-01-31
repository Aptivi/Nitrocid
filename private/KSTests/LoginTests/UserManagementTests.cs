
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

using KS.Users;
using NUnit.Framework;
using Shouldly;

namespace KSTests.LoginTests
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
        public void TestChangeUser() => UserManagement.TryChangeUsername("Account2", "Account3").ShouldBeTrue();

        /// <summary>
        /// Tests username change
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetUserProperty()
        {
            UserManagement.GetUserProperty("Account3", UserManagement.UserProperty.Username).ShouldBe("Account3");
            ((bool)UserManagement.GetUserProperty("Account3", UserManagement.UserProperty.Admin)).ShouldBeFalse();
        }

        /// <summary>
        /// Tests removing user
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestRemoveUser()
        {
            UserManagement.TryRemoveUser("Account1").ShouldBeTrue();
            UserManagement.TryRemoveUser("Account3").ShouldBeTrue();
        }

    }
}
