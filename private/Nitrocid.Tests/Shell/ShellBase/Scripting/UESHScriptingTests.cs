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

using Nitrocid.Kernel.Exceptions;
using Terminaux.Shell.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.IO;

namespace Nitrocid.Tests.Shell.ShellBase.Scripting
{

    [TestClass]
    public class UESHScriptingTests
    {

        /// <summary>
        /// Tests linting a valid UESH script
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestLintUESHScriptValid() =>
            Should.NotThrow(() => UESHParse.Execute(Path.GetFullPath("TestData/ScriptValid.uesh"), "", true));

        /// <summary>
        /// Tests linting a invalid UESH script
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestLintUESHScriptInvalid() =>
            Should.Throw(() => UESHParse.Execute(Path.GetFullPath("TestData/ScriptInvalid.uesh"), "", true), typeof(KernelException));

        /// <summary>
        /// Tests linting an empty UESH script
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestLintUESHScriptEmpty() =>
            Should.NotThrow(() => UESHParse.Execute(Path.GetFullPath("TestData/ScriptEmpty.uesh"), "", true));

    }
}
