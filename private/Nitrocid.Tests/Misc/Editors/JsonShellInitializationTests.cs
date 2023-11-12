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

using System.IO;
using KS.Files.Editors.JsonShell;
using KS.Shell.Shells.Json;
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Misc.Editors
{

    [TestFixture]
    public class JsonShellInitializationTests
    {

        /// <summary>
        /// Tests opening, saving, and closing a JSON file
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestOpenSaveCloseJsonFile()
        {
            string PathToTestJson = Path.GetFullPath("TestData/TestJson.json");
            JsonTools.OpenJsonFile(PathToTestJson).ShouldBeTrue();
            JsonTools.Add("$", "property", "HowText", "How are you today?");
            JsonShellCommon.FileToken["HowText"].ShouldNotBeNull();
            JsonTools.GetToken("HelloText").ShouldNotBeNull();
            JsonTools.SerializeToString("HelloText").ShouldNotBeNullOrEmpty();
            JsonTools.Remove("HowText");
            JsonShellCommon.FileToken["HowText"].ShouldBeNull();
            JsonTools.SaveFile(false).ShouldBeTrue();
            JsonTools.CloseTextFile().ShouldBeTrue();
        }

    }
}
