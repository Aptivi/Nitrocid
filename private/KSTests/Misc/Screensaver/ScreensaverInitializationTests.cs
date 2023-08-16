
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

using KS.Misc.Screensaver.Customized;
using NUnit.Framework;
using Shouldly;
using Saver = KS.Misc.Screensaver.Screensaver;

namespace KSTests.Misc.Screensaver
{

    [TestFixture]
    public class ScreensaverInitializationTests
    {

        /// <summary>
        /// Tests registering a custom screensaver
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestRegisterCustomSaver()
        {
            Saver.IsScreensaverRegistered("CustomSaver").ShouldBeFalse();
            CustomSaverTools.RegisterCustomScreensaver("CustomSaver", new CustomSaverTest());
            Saver.IsScreensaverRegistered("CustomSaver").ShouldBeTrue();
        }

        /// <summary>
        /// Tests unregistering a custom screensaver
        /// </summary>
        [Test]
        [Description("Setting")]
        public void TestUnregisterCustomSaver()
        {
            Saver.IsScreensaverRegistered("CustomSaver").ShouldBeTrue();
            CustomSaverTools.UnregisterCustomScreensaver("CustomSaver");
            Saver.IsScreensaverRegistered("CustomSaver").ShouldBeFalse();
        }

    }
}
