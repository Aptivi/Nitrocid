
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using KS.ConsoleBase.Colors;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Shouldly;

namespace KSTests.ConsoleTests
{

    [TestFixture]
    public class Color255QueryingTests
    {

        /// <summary>
        /// Tests querying 255-color data from JSON (parses only needed data by KS)
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestQueryColorDataFromJson()
        {
            for (int ColorIndex = 0; ColorIndex <= 255; ColorIndex++)
            {
                JObject ColorData = (JObject)ColorSeq.Color255.ColorDataJson[ColorIndex];
                ColorData["colorId"].ToString().ShouldBe(ColorIndex.ToString());
                int.TryParse(ColorData["rgb"]["r"].ToString(), out int argresult).ShouldBeTrue();
                int.TryParse(ColorData["rgb"]["g"].ToString(), out int argresult1).ShouldBeTrue();
                int.TryParse(ColorData["rgb"]["b"].ToString(), out int argresult2).ShouldBeTrue();
            }
        }

        /// <summary>
        /// Tests getting an escape character
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestGetEsc()
        {
            Color255.GetEsc().ShouldBe(Convert.ToChar(0x1B));
        }

    }
}
