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

using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Globalization;
using Nitrocid.Tests.Misc.Reflection.Data;

namespace Nitrocid.Tests.Misc.Reflection
{

    [TestClass]
    public class MethodManagementTests
    {

        /// <summary>
        /// Tests getting a method (static)
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetMethodStatic()
        {
            var value = MethodManager.GetMethod(nameof(CultureManager.GetCultures));
            value.ShouldNotBeNull();
        }

        /// <summary>
        /// Tests getting a method
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetMethod()
        {
            var instance = new ReflectedCommand();
            var value = MethodManager.GetMethod(nameof(instance.Execute), instance.GetType());
            value.ShouldNotBeNull();
            value.DeclaringType.ShouldBe(instance.GetType());
        }

        /// <summary>
        /// Tests invoking a method (static)
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestInvokeMethodStatic()
        {
            var value = MethodManager.InvokeMethodStatic(nameof(CultureManager.GetCultures));
            value.ShouldNotBeNull();
            value.ShouldBeOfType(typeof(CultureInfo[]));
        }

        /// <summary>
        /// Tests invoking a method (non-static)
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestInvokeMethod()
        {
            var instance = new ReflectedCommand();
            var value = MethodManager.InvokeMethod(nameof(instance.HelpHelper), instance, []);
            instance.doSet.ShouldBe("yes");
        }

    }
}
