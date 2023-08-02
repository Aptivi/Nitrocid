
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

using KS.Shell.ShellBase.Commands;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSTests.Shell.ShellBase.Commands
{
    [TestFixture]
    public class SwitchManagerTests
    {
        /// <summary>
        /// Tests getting switch values
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetSwitchValues()
        {
            string[] switches = new string[] { "-name=Harry", "-job=Programmer", "-company=\"Handworks Software Inc.\"", "-pruned" };
            var switchValues = SwitchManager.GetSwitchValues(switches);
            switchValues.Count.ShouldBe(3);
            switchValues[0].Item1.ShouldBe("-name");
            switchValues[0].Item2.ShouldBe("Harry");
            SwitchManager.GetSwitchValue(switches, "-name").ShouldBe("Harry");
            SwitchManager.ContainsSwitch(switches, "-name").ShouldBeTrue();
            switchValues[1].Item1.ShouldBe("-job");
            switchValues[1].Item2.ShouldBe("Programmer");
            SwitchManager.GetSwitchValue(switches, "-job").ShouldBe("Programmer");
            SwitchManager.ContainsSwitch(switches, "-job").ShouldBeTrue();
            switchValues[2].Item1.ShouldBe("-company");
            switchValues[2].Item2.ShouldBe("\"Handworks Software Inc.\"");
            SwitchManager.GetSwitchValue(switches, "-company").ShouldBe("\"Handworks Software Inc.\"");
            SwitchManager.ContainsSwitch(switches, "-company").ShouldBeTrue();
            switchValues.ShouldNotContain(("-pruned", ""));
            SwitchManager.ContainsSwitch(switches, "-pruned").ShouldBeTrue();
        }

        /// <summary>
        /// Tests getting switch values
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetSwitchValuesWithNonValue()
        {
            string[] switches = new string[] { "-name=Harry", "-job=Programmer", "-company=\"Handworks Software Inc.\"", "-pruned" };
            var switchValues = SwitchManager.GetSwitchValues(switches, true);
            switchValues.Count.ShouldBe(4);
            switchValues[0].Item1.ShouldBe("-name");
            switchValues[0].Item2.ShouldBe("Harry");
            SwitchManager.GetSwitchValue(switches, "-name").ShouldBe("Harry");
            SwitchManager.ContainsSwitch(switches, "-name").ShouldBeTrue();
            switchValues[1].Item1.ShouldBe("-job");
            switchValues[1].Item2.ShouldBe("Programmer");
            SwitchManager.GetSwitchValue(switches, "-job").ShouldBe("Programmer");
            SwitchManager.ContainsSwitch(switches, "-job").ShouldBeTrue();
            switchValues[2].Item1.ShouldBe("-company");
            switchValues[2].Item2.ShouldBe("\"Handworks Software Inc.\"");
            SwitchManager.GetSwitchValue(switches, "-company").ShouldBe("\"Handworks Software Inc.\"");
            SwitchManager.ContainsSwitch(switches, "-company").ShouldBeTrue();
            switchValues[3].Item1.ShouldBe("-pruned");
            switchValues[3].Item2.ShouldBe("");
            SwitchManager.ContainsSwitch(switches, "-pruned").ShouldBeTrue();
        }

        /// <summary>
        /// Tests getting switch values
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetSwitchValuesOneNoValue()
        {
            string[] switches = new string[] { "-name=Harry", "-job=", "-company=\"Handworks Software Inc.\"", "-pruned" };
            var switchValues = SwitchManager.GetSwitchValues(switches);
            switchValues.Count.ShouldBe(3);
            switchValues[0].Item1.ShouldBe("-name");
            switchValues[0].Item2.ShouldBe("Harry");
            SwitchManager.GetSwitchValue(switches, "-name").ShouldBe("Harry");
            SwitchManager.ContainsSwitch(switches, "-name").ShouldBeTrue();
            switchValues[1].Item1.ShouldBe("-job");
            switchValues[1].Item2.ShouldBe("");
            SwitchManager.GetSwitchValue(switches, "-job").ShouldBe("");
            SwitchManager.ContainsSwitch(switches, "-job").ShouldBeTrue();
            switchValues[2].Item1.ShouldBe("-company");
            switchValues[2].Item2.ShouldBe("\"Handworks Software Inc.\"");
            SwitchManager.GetSwitchValue(switches, "-company").ShouldBe("\"Handworks Software Inc.\"");
            SwitchManager.ContainsSwitch(switches, "-company").ShouldBeTrue();
            switchValues.ShouldNotContain(("-pruned", ""));
            SwitchManager.ContainsSwitch(switches, "-pruned").ShouldBeTrue();
        }

        /// <summary>
        /// Tests getting switch values
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetSwitchValuesOneNoValueWithNonValue()
        {
            string[] switches = new string[] { "-name=Harry", "-job=", "-company=\"Handworks Software Inc.\"", "-pruned" };
            var switchValues = SwitchManager.GetSwitchValues(switches, true);
            switchValues.Count.ShouldBe(4);
            switchValues[0].Item1.ShouldBe("-name");
            switchValues[0].Item2.ShouldBe("Harry");
            SwitchManager.GetSwitchValue(switches, "-name").ShouldBe("Harry");
            SwitchManager.ContainsSwitch(switches, "-name").ShouldBeTrue();
            switchValues[1].Item1.ShouldBe("-job");
            switchValues[1].Item2.ShouldBe("");
            SwitchManager.GetSwitchValue(switches, "-job").ShouldBe("");
            SwitchManager.ContainsSwitch(switches, "-job").ShouldBeTrue();
            switchValues[2].Item1.ShouldBe("-company");
            switchValues[2].Item2.ShouldBe("\"Handworks Software Inc.\"");
            SwitchManager.GetSwitchValue(switches, "-company").ShouldBe("\"Handworks Software Inc.\"");
            SwitchManager.ContainsSwitch(switches, "-company").ShouldBeTrue();
            switchValues[3].Item1.ShouldBe("-pruned");
            switchValues[3].Item2.ShouldBe("");
            SwitchManager.ContainsSwitch(switches, "-pruned").ShouldBeTrue();
        }

        /// <summary>
        /// Tests getting switch values
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetSwitchValuesJustNonValue()
        {
            string[] switches = new string[] { "-pruned" };
            var switchValues = SwitchManager.GetSwitchValues(switches);
            switchValues.Count.ShouldBe(0);
            switchValues.ShouldNotContain(("-pruned", ""));
            SwitchManager.ContainsSwitch(switches, "-pruned").ShouldBeTrue();
        }

        /// <summary>
        /// Tests getting switch values
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetSwitchValuesJustNonValueWithNonValue()
        {
            string[] switches = new string[] { "-pruned" };
            var switchValues = SwitchManager.GetSwitchValues(switches, true);
            switchValues.Count.ShouldBe(1);
            switchValues[0].Item1.ShouldBe("-pruned");
            switchValues[0].Item2.ShouldBe("");
            SwitchManager.ContainsSwitch(switches, "-pruned").ShouldBeTrue();
        }

        /// <summary>
        /// Tests getting switch values
        /// </summary>
        [Test]
        [Description("Management")]
        public void TestGetSwitchValuesEmpty()
        {
            string[] switches = Array.Empty<string>();
            var switchValues = SwitchManager.GetSwitchValues(switches);
            switchValues.Count.ShouldBe(0);
        }

    }
}
