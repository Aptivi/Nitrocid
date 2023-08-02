
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
using KS.Misc.Editors.TextEdit;
using KS.Shell.Shells.Text;
using NUnit.Framework;
using Shouldly;

namespace KSTests.Misc.Editors
{

    [TestFixture]
    public class TextEditorInitializationTests
    {

        /// <summary>
        /// Tests opening, saving, and closing text file
        /// </summary>
        [Test]
        [Description("Initialization")]
        public void TestOpenSaveCloseTextFile()
        {
            string PathToTestText = Path.GetFullPath("TestData/TestText.txt");
            TextEditTools.TextEdit_OpenTextFile(PathToTestText).ShouldBeTrue();
            TextEditShellCommon.TextEdit_FileLines.Add("Hello!");
            TextEditTools.TextEdit_SaveTextFile(false).ShouldBeTrue();
            TextEditShellCommon.TextEdit_FileLines.ShouldContain("Hello!");
            TextEditTools.TextEdit_CloseTextFile().ShouldBeTrue();
        }

    }
}
