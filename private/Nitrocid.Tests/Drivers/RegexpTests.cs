//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using NUnit.Framework;
using Shouldly;
using KS.Drivers;
using System.Text.RegularExpressions;
using KS.Kernel.Exceptions;
using KS.Misc.Text.Probers.Regexp;

namespace Nitrocid.Tests.Drivers
{
    [TestFixture]
    public class RegexpTests
    {

        [Test]
        [TestCase(@"^[\w.-]+$", ExpectedResult = true)]
        [TestCase(@"", ExpectedResult = true)]
        [TestCase("^[\\w.-\"]+$", ExpectedResult = false)]
        [Description("Action")]
        public bool TestIsValidRegex(string pattern) =>
            RegexpTools.IsValidRegex(pattern);

        [Test]
        [TestCase("twitch", @"^[\w.-]+$", ExpectedResult = true)]
        [TestCase("twi?tch", @"^[\w.-]+$", ExpectedResult = false)]
        [TestCase("twitch", "", ExpectedResult = true)]
        [TestCase("", @"^[\w.-]+$", ExpectedResult = false)]
        [TestCase("", "", ExpectedResult = true)]
        [Description("Action")]
        public bool TestIsMatch(string text, string pattern) =>
            RegexpTools.IsMatch(text, pattern);

        [Test]
        [TestCase("twitch", @"^[\w.-]+$")]
        [TestCase("twi?tch", @"^[\w.-]+$")]
        [TestCase("twitch", "")]
        [TestCase("", @"^[\w.-]+$")]
        [TestCase("", "")]
        [Description("Action")]
        public void TestMatch(string text, string pattern)
        {
            Match match = default;
            Should.NotThrow(() => match = DriverHandler.CurrentRegexpDriverLocal.Match(text, pattern));
        }

        [Test]
        [TestCase("twitch", "^[\\w.-\"]+$")]
        [TestCase("twi?tch", "^[\\w.-\"]+$")]
        [TestCase("", "^[\\w.-\"]+$")]
        [Description("Action")]
        public void TestMatchInvalid(string text, string pattern)
        {
            Match match = default;
            Should.Throw(() => match = DriverHandler.CurrentRegexpDriverLocal.Match(text, pattern), typeof(KernelException));
        }

        [Test]
        [TestCase("twitch", @"^[\w.-]+$")]
        [TestCase("twi?tch", @"^[\w.-]+$")]
        [TestCase("twitch", "")]
        [TestCase("", @"^[\w.-]+$")]
        [TestCase("", "")]
        [Description("Action")]
        public void TestMatches(string text, string pattern)
        {
            MatchCollection match = default;
            Should.NotThrow(() => match = DriverHandler.CurrentRegexpDriverLocal.Matches(text, pattern));
        }

        [Test]
        [TestCase("twitch", "^[\\w.-\"]+$")]
        [TestCase("twi?tch", "^[\\w.-\"]+$")]
        [TestCase("", "^[\\w.-\"]+$")]
        [Description("Action")]
        public void TestMatchesInvalid(string text, string pattern)
        {
            MatchCollection match = default;
            Should.Throw(() => match = DriverHandler.CurrentRegexpDriverLocal.Matches(text, pattern), typeof(KernelException));
        }

        [Test]
        [TestCase("twitch", @"^[\w.-]+$", ExpectedResult = "")]
        [TestCase("twi?tch", @"^[\w.-]+$", ExpectedResult = "twi?tch")]
        [TestCase("twitch", "", ExpectedResult = "twitch")]
        [TestCase("", @"^[\w.-]+$", ExpectedResult = "")]
        [TestCase("", "", ExpectedResult = "")]
        [Description("Action")]
        public string TestFilter(string text, string pattern)
        {
            string filtered = default;
            Should.NotThrow(() => filtered = DriverHandler.CurrentRegexpDriverLocal.Filter(text, pattern));
            return filtered;
        }

        [Test]
        [TestCase("twitch", "^[\\w.-\"]+$")]
        [TestCase("twi?tch", "^[\\w.-\"]+$")]
        [TestCase("", "^[\\w.-\"]+$")]
        [Description("Action")]
        public void TestFilterInvalid(string text, string pattern)
        {
            string filtered = default;
            Should.Throw(() => filtered = DriverHandler.CurrentRegexpDriverLocal.Filter(text, pattern), typeof(KernelException));
        }

        [Test]
        [TestCase("twitch", @"^[\w.-]+$", "switch", ExpectedResult = "switch")]
        [TestCase("twi?tch", @"^[\w.-]+$", "switch", ExpectedResult = "twi?tch")]
        [TestCase("twitch", "", "switch", ExpectedResult = "switchtswitchwswitchiswitchtswitchcswitchhswitch")]
        [TestCase("", @"^[\w.-]+$", "switch", ExpectedResult = "")]
        [TestCase("", "", "switch", ExpectedResult = "switch")]
        [Description("Action")]
        public string TestFilter(string text, string pattern, string replaceWith)
        {
            string filtered = default;
            Should.NotThrow(() => filtered = DriverHandler.CurrentRegexpDriverLocal.Filter(text, pattern, replaceWith));
            return filtered;
        }

        [Test]
        [TestCase("twitch", "^[\\w.-\"]+$", "switch")]
        [TestCase("twi?tch", "^[\\w.-\"]+$", "switch")]
        [TestCase("", "^[\\w.-\"]+$", "switch")]
        [Description("Action")]
        public void TestFilterInvalid(string text, string pattern, string replaceWith)
        {
            string filtered = default;
            Should.Throw(() => filtered = DriverHandler.CurrentRegexpDriverLocal.Filter(text, pattern, replaceWith), typeof(KernelException));
        }

        [Test]
        [TestCase("twitch-switch", @"[st]", ExpectedResult = new string[] { "", "wi", "ch-", "wi", "ch" })]
        [TestCase("twi?tch-switch", @"[st]", ExpectedResult = new string[] { "", "wi?", "ch-", "wi", "ch" })]
        [TestCase("twitch", "", ExpectedResult = new string[] { "", "t", "w", "i", "t", "c", "h", "" })]
        [TestCase("", @"[st]", ExpectedResult = new string[] { "" })]
        [TestCase("", "", ExpectedResult = new string[] { "", "" })]
        [Description("Action")]
        public string[] TestSplit(string text, string pattern)
        {
            string[] split = default;
            Should.NotThrow(() => split = DriverHandler.CurrentRegexpDriverLocal.Split(text, pattern));
            return split;
        }

        [Test]
        [TestCase("twitch", "^[\\w.-\"]+$")]
        [TestCase("twi?tch", "^[\\w.-\"]+$")]
        [TestCase("", "^[\\w.-\"]+$")]
        [Description("Action")]
        public void TestSplitInvalid(string text, string pattern)
        {
            string[] split = default;
            Should.Throw(() => split = DriverHandler.CurrentRegexpDriverLocal.Split(text, pattern), typeof(KernelException));
        }

        [Test]
        [TestCase(@"Twi\tch", ExpectedResult = @"Twi\\tch")]
        [TestCase(@"Twi*tch", ExpectedResult = @"Twi\*tch")]
        [TestCase(@"Twi+tch", ExpectedResult = @"Twi\+tch")]
        [TestCase(@"Twi?tch", ExpectedResult = @"Twi\?tch")]
        [TestCase(@"Twi|tch", ExpectedResult = @"Twi\|tch")]
        [TestCase(@"Twi(tch", ExpectedResult = @"Twi\(tch")]
        [TestCase(@"Twi{tch", ExpectedResult = @"Twi\{tch")]
        [TestCase(@"Twi[tch", ExpectedResult = @"Twi\[tch")]
        [TestCase(@"Twi)tch", ExpectedResult = @"Twi\)tch")]
        [TestCase(@"Twi^tch", ExpectedResult = @"Twi\^tch")]
        [TestCase(@"Twi$tch", ExpectedResult = @"Twi\$tch")]
        [TestCase(@"Twi.tch", ExpectedResult = @"Twi\.tch")]
        [TestCase(@"Twi#tch", ExpectedResult = @"Twi\#tch")]
        [TestCase(@"Twi tch", ExpectedResult = @"Twi\ tch")]
        [Description("Action")]
        public string TestEscape(string text)
        {
            string final = default;
            Should.NotThrow(() => final = DriverHandler.CurrentRegexpDriverLocal.Escape(text));
            return final;
        }

        [Test]
        [TestCase(@"Twi\\tch", ExpectedResult = @"Twi\tch")]
        [TestCase(@"Twi\*tch", ExpectedResult = @"Twi*tch")]
        [TestCase(@"Twi\+tch", ExpectedResult = @"Twi+tch")]
        [TestCase(@"Twi\?tch", ExpectedResult = @"Twi?tch")]
        [TestCase(@"Twi\|tch", ExpectedResult = @"Twi|tch")]
        [TestCase(@"Twi\(tch", ExpectedResult = @"Twi(tch")]
        [TestCase(@"Twi\{tch", ExpectedResult = @"Twi{tch")]
        [TestCase(@"Twi\[tch", ExpectedResult = @"Twi[tch")]
        [TestCase(@"Twi\)tch", ExpectedResult = @"Twi)tch")]
        [TestCase(@"Twi\^tch", ExpectedResult = @"Twi^tch")]
        [TestCase(@"Twi\$tch", ExpectedResult = @"Twi$tch")]
        [TestCase(@"Twi\.tch", ExpectedResult = @"Twi.tch")]
        [TestCase(@"Twi\#tch", ExpectedResult = @"Twi#tch")]
        [TestCase(@"Twi\ tch", ExpectedResult = @"Twi tch")]
        [Description("Action")]
        public string TestUnescape(string text)
        {
            string final = default;
            Should.NotThrow(() => final = DriverHandler.CurrentRegexpDriverLocal.Unescape(text));
            return final;
        }
    }
}
