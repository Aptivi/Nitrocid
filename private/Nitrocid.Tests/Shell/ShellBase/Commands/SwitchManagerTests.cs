//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Nitrocid.Shell.ShellBase.Switches;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Shell.ShellBase.Commands
{
    [TestClass]
    public class SwitchManagerTests
    {
        /// <summary>
        /// Tests getting switch values
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetSwitchValues()
        {
            string[] switches = ["-name=Harry", "-job=Programmer", "-company=\"Handworks Software Inc.\"", "-pruned"];
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
        [TestMethod]
        [Description("Management")]
        public void TestGetSwitchValuesWithNonValue()
        {
            string[] switches = ["-name=Harry", "-job=Programmer", "-company=\"Handworks Software Inc.\"", "-pruned"];
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
        [TestMethod]
        [Description("Management")]
        public void TestGetSwitchValuesOneNoValue()
        {
            string[] switches = ["-name=Harry", "-job=", "-company=\"Handworks Software Inc.\"", "-pruned"];
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
        [TestMethod]
        [Description("Management")]
        public void TestGetSwitchValuesOneNoValueWithNonValue()
        {
            string[] switches = ["-name=Harry", "-job=", "-company=\"Handworks Software Inc.\"", "-pruned"];
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
        [TestMethod]
        [Description("Management")]
        public void TestGetSwitchValuesJustNonValue()
        {
            string[] switches = ["-pruned"];
            var switchValues = SwitchManager.GetSwitchValues(switches);
            switchValues.Count.ShouldBe(0);
            switchValues.ShouldNotContain(("-pruned", ""));
            SwitchManager.ContainsSwitch(switches, "-pruned").ShouldBeTrue();
        }

        /// <summary>
        /// Tests getting switch values
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetSwitchValuesJustNonValueWithNonValue()
        {
            string[] switches = ["-pruned"];
            var switchValues = SwitchManager.GetSwitchValues(switches, true);
            switchValues.Count.ShouldBe(1);
            switchValues[0].Item1.ShouldBe("-pruned");
            switchValues[0].Item2.ShouldBe("");
            SwitchManager.ContainsSwitch(switches, "-pruned").ShouldBeTrue();
        }

        /// <summary>
        /// Tests getting switch values
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestGetSwitchValuesEmpty()
        {
            string[] switches = [];
            var switchValues = SwitchManager.GetSwitchValues(switches);
            switchValues.Count.ShouldBe(0);
        }

        /// <summary>
        /// Tests getting switch values
        /// </summary>
        [TestMethod]
        [Description("Management")]
        public void TestIsSwitchValueNumeric()
        {
            string[] switches = ["-name=Sarah", "-pcnum=3"];
            var switchValues = SwitchManager.GetSwitchValues(switches, true);
            switchValues.Count.ShouldBe(2);
            switchValues[0].Item1.ShouldBe("-name");
            switchValues[0].Item2.ShouldBe("Sarah");
            SwitchManager.GetSwitchValue(switches, "-name").ShouldBe("Sarah");
            SwitchManager.ContainsSwitch(switches, "-name").ShouldBeTrue();
            SwitchManager.IsSwitchValueNumeric(switches, "-name").ShouldBeFalse();
            switchValues[1].Item1.ShouldBe("-pcnum");
            switchValues[1].Item2.ShouldBe("3");
            SwitchManager.GetSwitchValue(switches, "-pcnum").ShouldBe("3");
            SwitchManager.ContainsSwitch(switches, "-pcnum").ShouldBeTrue();
            SwitchManager.IsSwitchValueNumeric(switches, "-pcnum").ShouldBeTrue();
        }

    }
}
