//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using KS.Misc.Text.Probers.Placeholder;
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Misc.Probers
{

    [TestFixture]
    public class PlaceholderActionTests
    {

        /// <summary>
        /// Tests registering the placeholder
        /// </summary>
        [OneTimeSetUp]
        public void TestRegisterPlaceholder()
        {
            PlaceParse.RegisterCustomPlaceholder("greeting", (_) => "Hello!");
            PlaceParse.IsPlaceholderRegistered("<greeting>").ShouldBeTrue();
            PlaceParse.IsPlaceholderBuiltin("<greeting>").ShouldBeFalse();
        }

        /// <summary>
        /// Tests parsing placeholders
        /// </summary>
        [Test]
        [TestCase("Hostname is <host>")]
        [TestCase("Short date is <shortdate>")]
        [TestCase("Long date is <longdate>")]
        [TestCase("Short time is <shorttime>")]
        [TestCase("Long time is <longtime>")]
        [TestCase("Date is <date>")]
        [TestCase("Time is <time>")]
        [TestCase("Timezone is <timezone>")]
        [TestCase("Summer timezone is <summertimezone>")]
        [TestCase("Operating system is <system>")]
        [TestCase("Newline is <newline>")]
        [TestCase("User is <user>")]
        [TestCase("Random File is <randomfile>")]
        [TestCase("Random Folder is <randomfolder>")]
        [TestCase("RID is <rid>")]
        [TestCase("RID is <ridgeneric>")]
        [TestCase("Terminal emulator is <termemu>")]
        [TestCase("Terminal type is <termtype>")]
        [TestCase("Foreground reset is <fgreset>here")]
        [TestCase("Background reset is <bgreset>here")]
        [TestCase("Foreground color is <f:4>0-15")]
        [TestCase("Foreground color is <b:4>0-15")]
        [TestCase("Foreground color is <f:254>0-255")]
        [TestCase("Foreground color is <b:254>0-255")]
        [TestCase("Foreground color is <f:255;255;255>truecolor")]
        [TestCase("Foreground color is <b:255;255;255>truecolor")]
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
        [Test]
        [TestCase("<greeting>", "Hello!")]
        [TestCase("Nitrocid, <greeting>", "Nitrocid, Hello!")]
        [TestCase("<greeting> How are you?", "Hello! How are you?")]
        [TestCase("Nitrocid, <greeting> How are you?", "Nitrocid, Hello! How are you?")]
        [Description("Action")]
        public void TestParseCustomPlaceholder(string stringToProbe, string expectedString)
        {
            string probed = PlaceParse.ProbePlaces(stringToProbe);
            probed.ShouldBe(expectedString);
        }

        /// <summary>
        /// Tests checking to see if the placeholders are built-in
        /// </summary>
        [Test]
        [TestCase("<host>", true)]
        [TestCase("<shortdate>", true)]
        [TestCase("<longdate>", true)]
        [TestCase("<shorttime>", true)]
        [TestCase("<longtime>", true)]
        [TestCase("<date>", true)]
        [TestCase("<time>", true)]
        [TestCase("<timezone>", true)]
        [TestCase("<summertimezone>", true)]
        [TestCase("<system>", true)]
        [TestCase("<newline>", true)]
        [TestCase("<user>", true)]
        [TestCase("<randomfile>", true)]
        [TestCase("<randomfolder>", true)]
        [TestCase("<rid>", true)]
        [TestCase("<ridgeneric>", true)]
        [TestCase("<termemu>", true)]
        [TestCase("<termtype>", true)]
        [TestCase("<f:reset>", true)]
        [TestCase("<b:reset>", true)]
        [TestCase("<greeting>", false)]
        [Description("Action")]
        public void TestIsBuiltin(string placeholder, bool expectedBuiltin)
        {
            bool actualBuiltin = PlaceParse.IsPlaceholderBuiltin(placeholder);
            actualBuiltin.ShouldBe(expectedBuiltin);
        }

        /// <summary>
        /// Tests checking to see if the placeholders are registered
        /// </summary>
        [Test]
        [TestCase("<host>", true)]
        [TestCase("<shortdate>", true)]
        [TestCase("<longdate>", true)]
        [TestCase("<shorttime>", true)]
        [TestCase("<longtime>", true)]
        [TestCase("<date>", true)]
        [TestCase("<time>", true)]
        [TestCase("<timezone>", true)]
        [TestCase("<summertimezone>", true)]
        [TestCase("<system>", true)]
        [TestCase("<newline>", true)]
        [TestCase("<user>", true)]
        [TestCase("<randomfile>", true)]
        [TestCase("<randomfolder>", true)]
        [TestCase("<rid>", true)]
        [TestCase("<ridgeneric>", true)]
        [TestCase("<termemu>", true)]
        [TestCase("<termtype>", true)]
        [TestCase("<f:reset>", true)]
        [TestCase("<b:reset>", true)]
        [TestCase("<greeting>", true)]
        [TestCase("<nonexistent>", false)]
        [Description("Action")]
        public void TestIsRegistered(string placeholder, bool expectedBuiltin)
        {
            bool actualBuiltin = PlaceParse.IsPlaceholderRegistered(placeholder);
            actualBuiltin.ShouldBe(expectedBuiltin);
        }

        /// <summary>
        /// Tests unregistering the placeholder
        /// </summary>
        [OneTimeTearDown]
        public void TestUnregisterPlaceholder()
        {
            PlaceParse.UnregisterCustomPlaceholder("<greeting>");
            PlaceParse.IsPlaceholderRegistered("<greeting>").ShouldBeFalse();
            PlaceParse.IsPlaceholderBuiltin("<greeting>").ShouldBeFalse();
        }

    }
}
