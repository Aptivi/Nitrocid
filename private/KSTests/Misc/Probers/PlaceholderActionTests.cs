
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

using KS.Misc.Probers.Placeholder;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;

namespace KSTests.Misc.Probers
{

    [TestFixture]
    public class PlaceholderActionTests
    {

        /// <summary>
        /// Tests parsing placeholders
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestParsePlaceholders()
        {
            var UnparsedStrings = new List<string>();
            var ParsedStrings = new List<string>()
            {
                PlaceParse.ProbePlaces("Hostname is <host>"),
                PlaceParse.ProbePlaces("Short date is <shortdate>"),
                PlaceParse.ProbePlaces("Long date is <longdate>"),
                PlaceParse.ProbePlaces("Short time is <shorttime>"),
                PlaceParse.ProbePlaces("Long time is <longtime>"),
                PlaceParse.ProbePlaces("Date is <date>"),
                PlaceParse.ProbePlaces("Time is <time>"),
                PlaceParse.ProbePlaces("Timezone is <timezone>"),
                PlaceParse.ProbePlaces("Summer timezone is <summertimezone>"),
                PlaceParse.ProbePlaces("Operating system is <system>"),
                PlaceParse.ProbePlaces("Newline is <newline>"),
                PlaceParse.ProbePlaces("Foreground reset is <f:reset>here"),
                PlaceParse.ProbePlaces("Background reset is <b:reset>here"),
                PlaceParse.ProbePlaces("Foreground color is <f:4>0-15"),
                PlaceParse.ProbePlaces("Foreground color is <b:4>0-15"),
                PlaceParse.ProbePlaces("Foreground color is <f:254>0-255"),
                PlaceParse.ProbePlaces("Foreground color is <b:254>0-255"),
                PlaceParse.ProbePlaces("Foreground color is <f:255;255;255>truecolor"),
                PlaceParse.ProbePlaces("Foreground color is <b:255;255;255>truecolor")
            };
            foreach (string ParsedString in ParsedStrings)
            {
                if (ParsedString.Contains("<") & ParsedString.Contains(">"))
                {
                    UnparsedStrings.Add(ParsedString);
                }
            }
            UnparsedStrings.ShouldBeEmpty();
        }

    }
}
