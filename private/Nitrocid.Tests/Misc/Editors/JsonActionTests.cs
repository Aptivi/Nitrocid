
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

using System.IO;
using KS.Files.Editors.JsonShell;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Misc.Editors
{

    [TestFixture]
    public class JsonActionTests
    {

        private static string demoJson =
            """
            {
              "LyricsPath": "C:/Users/MyUser/Music/"
            }
            """;
        private static string minifiedJson =
            """
            {"LyricsPath":"C:/Users/MyUser/Music/"}
            """;

        /// <summary>
        /// Tests beautifying the JSON text
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestBeautifyJsonText()
        {
            string Beautified = JsonTools.BeautifyJsonText(minifiedJson);
            Beautified.ShouldNotBeEmpty();
            Beautified.ShouldBe(demoJson);
        }

        /// <summary>
        /// Tests beautifying the JSON text
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestBeautifyJsonFile()
        {
            string SourcePath = Path.GetFullPath("TestData/Hacker.json");
            string Beautified = JsonTools.BeautifyJson(SourcePath);
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
            string Minified = JsonTools.MinifyJsonText(demoJson);
            Minified.ShouldNotBeEmpty();
            Minified.ShouldBe(minifiedJson);
        }

        /// <summary>
        /// Tests minifying the JSON text
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestMinifyJsonFile()
        {
            string SourcePath = Path.GetFullPath("TestData/Hacker.json");
            string Minified = JsonTools.MinifyJson(SourcePath);
            Minified.ShouldNotBeEmpty();
            Minified.ShouldBe(JsonConvert.SerializeObject(JToken.Parse(Minified), Formatting.None));
        }

    }
}
