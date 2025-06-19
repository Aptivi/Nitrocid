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

using Textify.Tools.Placeholder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Misc.Probers
{

    [TestClass]
    public class PlaceholderActionTests
    {

        /// <summary>
        /// Tests registering the placeholder
        /// </summary>
        [ClassInitialize]
#pragma warning disable IDE0060
        public static void TestRegisterPlaceholder(TestContext tc)
#pragma warning restore IDE0060
        {
            PlaceParse.RegisterCustomPlaceholder("greeting", (_) => "Hello!");
            PlaceParse.IsPlaceholderRegistered("<greeting>").ShouldBeTrue();
            PlaceParse.IsPlaceholderBuiltin("<greeting>").ShouldBeFalse();
        }

        /// <summary>
        /// Tests parsing placeholders
        /// </summary>
        [TestMethod]
        [DataRow("Hostname is <host>")]
        [DataRow("Short date is <shortdate>")]
        [DataRow("Long date is <longdate>")]
        [DataRow("Short time is <shorttime>")]
        [DataRow("Long time is <longtime>")]
        [DataRow("Date is <date>")]
        [DataRow("Time is <time>")]
        [DataRow("Timezone is <timezone>")]
        [DataRow("Summer timezone is <summertimezone>")]
        [DataRow("Operating system is <system>")]
        [DataRow("Newline is <newline>")]
        [DataRow("User is <user>")]
        [DataRow("Random File is <randomfile>")]
        [DataRow("Random Folder is <randomfolder>")]
        [DataRow("RID is <rid>")]
        [DataRow("RID is <ridgeneric>")]
        [DataRow("Terminal emulator is <termemu>")]
        [DataRow("Terminal type is <termtype>")]
        [DataRow("Foreground reset is <fgreset>here")]
        [DataRow("Background reset is <bgreset>here")]
        [DataRow("Foreground color is <f:4>0-15")]
        [DataRow("Foreground color is <b:4>0-15")]
        [DataRow("Foreground color is <f:254>0-255")]
        [DataRow("Foreground color is <b:254>0-255")]
        [DataRow("Foreground color is <f:255;255;255>truecolor")]
        [DataRow("Foreground color is <b:255;255;255>truecolor")]
        [Description("Action")]
        public void TestParsePlaceholders(string stringToProbe)
        {
            string probed = PlaceParse.ProbePlaces(stringToProbe);
            bool dirty = probed.Contains('<') && probed.Contains('>');
            dirty.ShouldBeFalse();
        }

        /// <summary>
        /// Tests parsing placeholders
        /// </summary>
        [TestMethod]
        [DataRow("<greeting>", "Hello!")]
        [DataRow("Nitrocid, <greeting>", "Nitrocid, Hello!")]
        [DataRow("<greeting> How are you?", "Hello! How are you?")]
        [DataRow("Nitrocid, <greeting> How are you?", "Nitrocid, Hello! How are you?")]
        [Description("Action")]
        public void TestParseCustomPlaceholder(string stringToProbe, string expectedString)
        {
            string probed = PlaceParse.ProbePlaces(stringToProbe);
            probed.ShouldBe(expectedString);
        }

        /// <summary>
        /// Tests checking to see if the placeholders are built-in
        /// </summary>
        [TestMethod]
        [DataRow("<system>", true)]
        [DataRow("<newline>", true)]
        [DataRow("<greeting>", false)]
        [DataRow("<nonexistent>", false)]
        [Description("Action")]
        public void TestIsBuiltin(string placeholder, bool expectedBuiltin)
        {
            bool actualBuiltin = PlaceParse.IsPlaceholderBuiltin(placeholder);
            actualBuiltin.ShouldBe(expectedBuiltin);
        }

        /// <summary>
        /// Tests checking to see if the placeholders are registered
        /// </summary>
        [TestMethod]
        [DataRow("<system>", true)]
        [DataRow("<newline>", true)]
        [DataRow("<greeting>", true)]
        [DataRow("<nonexistent>", false)]
        [Description("Action")]
        public void TestIsRegistered(string placeholder, bool expectedBuiltin)
        {
            bool actualBuiltin = PlaceParse.IsPlaceholderRegistered(placeholder);
            actualBuiltin.ShouldBe(expectedBuiltin);
        }

        /// <summary>
        /// Tests unregistering the placeholder
        /// </summary>
        [ClassCleanup]
        public static void TestUnregisterPlaceholder()
        {
            PlaceParse.UnregisterCustomPlaceholder("<greeting>");
            PlaceParse.IsPlaceholderRegistered("<greeting>").ShouldBeFalse();
            PlaceParse.IsPlaceholderBuiltin("<greeting>").ShouldBeFalse();
        }

    }
}
