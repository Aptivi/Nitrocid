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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Text.RegularExpressions;
using Nitrocid.Drivers;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Misc.Text.Probers.Regexp;

namespace Nitrocid.Tests.Drivers
{
    [TestClass]
    public class RegexpTests
    {

        [TestMethod]
        [DataRow(@"^[\w.-]+$", true)]
        [DataRow(@"", true)]
        [DataRow("^[\\w.-\"]+$", false)]
        [Description("Action")]
        public void TestIsValidRegex(string pattern, bool expected)
        {
            bool actual = RegexpTools.IsValidRegex(pattern);
            actual.ShouldBe(expected);
        }

        [TestMethod]
        [DataRow("twitch", @"^[\w.-]+$", true)]
        [DataRow("twi?tch", @"^[\w.-]+$", false)]
        [DataRow("twitch", "", true)]
        [DataRow("", @"^[\w.-]+$", false)]
        [DataRow("", "", true)]
        [Description("Action")]
        public void TestIsMatch(string text, string pattern, bool expected)
        {
            bool actual = RegexpTools.IsMatch(text, pattern);
            actual.ShouldBe(expected);
        }

        [TestMethod]
        [DataRow("twitch", @"^[\w.-]+$")]
        [DataRow("twi?tch", @"^[\w.-]+$")]
        [DataRow("twitch", "")]
        [DataRow("", @"^[\w.-]+$")]
        [DataRow("", "")]
        [Description("Action")]
        public void TestMatch(string text, string pattern)
        {
            Match match = default;
            Should.NotThrow(() => match = DriverHandler.CurrentRegexpDriverLocal.Match(text, pattern));
        }

        [TestMethod]
        [DataRow("twitch", "^[\\w.-\"]+$")]
        [DataRow("twi?tch", "^[\\w.-\"]+$")]
        [DataRow("", "^[\\w.-\"]+$")]
        [Description("Action")]
        public void TestMatchInvalid(string text, string pattern)
        {
            Match match = default;
            Should.Throw(() => match = DriverHandler.CurrentRegexpDriverLocal.Match(text, pattern), typeof(KernelException));
        }

        [TestMethod]
        [DataRow("twitch", @"^[\w.-]+$")]
        [DataRow("twi?tch", @"^[\w.-]+$")]
        [DataRow("twitch", "")]
        [DataRow("", @"^[\w.-]+$")]
        [DataRow("", "")]
        [Description("Action")]
        public void TestMatches(string text, string pattern)
        {
            MatchCollection match = default;
            Should.NotThrow(() => match = DriverHandler.CurrentRegexpDriverLocal.Matches(text, pattern));
        }

        [TestMethod]
        [DataRow("twitch", "^[\\w.-\"]+$")]
        [DataRow("twi?tch", "^[\\w.-\"]+$")]
        [DataRow("", "^[\\w.-\"]+$")]
        [Description("Action")]
        public void TestMatchesInvalid(string text, string pattern)
        {
            MatchCollection match = default;
            Should.Throw(() => match = DriverHandler.CurrentRegexpDriverLocal.Matches(text, pattern), typeof(KernelException));
        }

        [TestMethod]
        [DataRow("twitch", @"^[\w.-]+$", "")]
        [DataRow("twi?tch", @"^[\w.-]+$", "twi?tch")]
        [DataRow("twitch", "", "twitch")]
        [DataRow("", @"^[\w.-]+$", "")]
        [DataRow("", "", "")]
        [Description("Action")]
        public void TestFilter(string text, string pattern, string expected)
        {
            string filtered = default;
            Should.NotThrow(() => filtered = DriverHandler.CurrentRegexpDriverLocal.Filter(text, pattern));
            filtered.ShouldBe(expected);
        }

        [TestMethod]
        [DataRow("twitch", "^[\\w.-\"]+$")]
        [DataRow("twi?tch", "^[\\w.-\"]+$")]
        [DataRow("", "^[\\w.-\"]+$")]
        [Description("Action")]
        public void TestFilterInvalid(string text, string pattern)
        {
            string filtered = default;
            Should.Throw(() => filtered = DriverHandler.CurrentRegexpDriverLocal.Filter(text, pattern), typeof(KernelException));
        }

        [TestMethod]
        [DataRow("twitch", @"^[\w.-]+$", "switch", "switch")]
        [DataRow("twi?tch", @"^[\w.-]+$", "switch", "twi?tch")]
        [DataRow("twitch", "", "switch", "switchtswitchwswitchiswitchtswitchcswitchhswitch")]
        [DataRow("", @"^[\w.-]+$", "switch", "")]
        [DataRow("", "", "switch", "switch")]
        [Description("Action")]
        public void TestFilter(string text, string pattern, string replaceWith, string expected)
        {
            string filtered = default;
            Should.NotThrow(() => filtered = DriverHandler.CurrentRegexpDriverLocal.Filter(text, pattern, replaceWith));
            filtered.ShouldBe(expected);
        }

        [TestMethod]
        [DataRow("twitch", "^[\\w.-\"]+$", "switch")]
        [DataRow("twi?tch", "^[\\w.-\"]+$", "switch")]
        [DataRow("", "^[\\w.-\"]+$", "switch")]
        [Description("Action")]
        public void TestFilterInvalid(string text, string pattern, string replaceWith)
        {
            string filtered = default;
            Should.Throw(() => filtered = DriverHandler.CurrentRegexpDriverLocal.Filter(text, pattern, replaceWith), typeof(KernelException));
        }

        [TestMethod]
        [DataRow("twitch-switch", @"[st]", new string[] { "", "wi", "ch-", "wi", "ch" })]
        [DataRow("twi?tch-switch", @"[st]", new string[] { "", "wi?", "ch-", "wi", "ch" })]
        [DataRow("twitch", "", new string[] { "", "t", "w", "i", "t", "c", "h", "" })]
        [DataRow("", @"[st]", new string[] { "" })]
        [DataRow("", "", new string[] { "", "" })]
        [Description("Action")]
        public void TestSplit(string text, string pattern, string[] expected)
        {
            string[] split = default;
            Should.NotThrow(() => split = DriverHandler.CurrentRegexpDriverLocal.Split(text, pattern));
            split.ShouldBe(expected);
        }

        [TestMethod]
        [DataRow("twitch", "^[\\w.-\"]+$")]
        [DataRow("twi?tch", "^[\\w.-\"]+$")]
        [DataRow("", "^[\\w.-\"]+$")]
        [Description("Action")]
        public void TestSplitInvalid(string text, string pattern)
        {
            string[] split = default;
            Should.Throw(() => split = DriverHandler.CurrentRegexpDriverLocal.Split(text, pattern), typeof(KernelException));
        }

        [TestMethod]
        [DataRow(@"Twi\tch", @"Twi\\tch")]
        [DataRow(@"Twi*tch", @"Twi\*tch")]
        [DataRow(@"Twi+tch", @"Twi\+tch")]
        [DataRow(@"Twi?tch", @"Twi\?tch")]
        [DataRow(@"Twi|tch", @"Twi\|tch")]
        [DataRow(@"Twi(tch", @"Twi\(tch")]
        [DataRow(@"Twi{tch", @"Twi\{tch")]
        [DataRow(@"Twi[tch", @"Twi\[tch")]
        [DataRow(@"Twi)tch", @"Twi\)tch")]
        [DataRow(@"Twi^tch", @"Twi\^tch")]
        [DataRow(@"Twi$tch", @"Twi\$tch")]
        [DataRow(@"Twi.tch", @"Twi\.tch")]
        [DataRow(@"Twi#tch", @"Twi\#tch")]
        [DataRow(@"Twi tch", @"Twi\ tch")]
        [Description("Action")]
        public void TestEscape(string text, string expected)
        {
            string final = default;
            Should.NotThrow(() => final = DriverHandler.CurrentRegexpDriverLocal.Escape(text));
            final.ShouldBe(expected);
        }

        [TestMethod]
        [DataRow(@"Twi\\tch", @"Twi\tch")]
        [DataRow(@"Twi\*tch", @"Twi*tch")]
        [DataRow(@"Twi\+tch", @"Twi+tch")]
        [DataRow(@"Twi\?tch", @"Twi?tch")]
        [DataRow(@"Twi\|tch", @"Twi|tch")]
        [DataRow(@"Twi\(tch", @"Twi(tch")]
        [DataRow(@"Twi\{tch", @"Twi{tch")]
        [DataRow(@"Twi\[tch", @"Twi[tch")]
        [DataRow(@"Twi\)tch", @"Twi)tch")]
        [DataRow(@"Twi\^tch", @"Twi^tch")]
        [DataRow(@"Twi\$tch", @"Twi$tch")]
        [DataRow(@"Twi\.tch", @"Twi.tch")]
        [DataRow(@"Twi\#tch", @"Twi#tch")]
        [DataRow(@"Twi\ tch", @"Twi tch")]
        [Description("Action")]
        public void TestUnescape(string text, string expected)
        {
            string final = default;
            Should.NotThrow(() => final = DriverHandler.CurrentRegexpDriverLocal.Unescape(text));
            final.ShouldBe(expected);
        }
    }
}
