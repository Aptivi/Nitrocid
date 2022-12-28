
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using System.IO;
using KS.Misc.Beautifiers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Shouldly;

namespace KSTests.MiscTests
{

    [TestFixture]
    public class JsonActionTests
    {

        /// <summary>
        /// Tests beautifying the JSON text
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestBeautifyJsonText()
        {
            string Beautified = JsonBeautifier.BeautifyJsonText(JsonConvert.SerializeObject(ColorSeq.Color255.ColorDataJson));
            Beautified.ShouldNotBeEmpty();
            Beautified.ShouldBe(JsonConvert.SerializeObject(ColorSeq.Color255.ColorDataJson, Formatting.Indented));
        }

        /// <summary>
        /// Tests beautifying the JSON text
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestBeautifyJsonFile()
        {
            string SourcePath = Path.GetFullPath("TestData/Hacker.json");
            string Beautified = JsonBeautifier.BeautifyJson(SourcePath);
            Beautified.ShouldNotBeEmpty();
            Beautified.ShouldBe(JsonConvert.SerializeObject(JToken.Parse(Beautified), Formatting.Indented));
        }

        /// <summary>
        /// Tests minifying the JSON text
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestMinifyJsonText()
        {
            string Minified = JsonMinifier.MinifyJsonText(JsonConvert.SerializeObject(ColorSeq.Color255.ColorDataJson));
            Minified.ShouldNotBeEmpty();
            Minified.ShouldBe(JsonConvert.SerializeObject(ColorSeq.Color255.ColorDataJson, Formatting.None));
        }

        /// <summary>
        /// Tests minifying the JSON text
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestMinifyJsonFile()
        {
            string SourcePath = Path.GetFullPath("TestData/Hacker.json");
            string Minified = JsonMinifier.MinifyJson(SourcePath);
            Minified.ShouldNotBeEmpty();
            Minified.ShouldBe(JsonConvert.SerializeObject(JToken.Parse(Minified), Formatting.None));
        }

    }
}
