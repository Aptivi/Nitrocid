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
using Terminaux.Shell.Scripting.Conditions;
using Nitrocid.Tests.Shell.ShellBase.Scripting.CustomConditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Nitrocid.Tests.Shell.ShellBase.Scripting
{

    [TestClass]
    public class UESHConditionTests
    {

        /// <summary>
        /// Tests querying available conditions
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestQueryAvailableConditions()
        {
            UESHConditional.AvailableConditions.ShouldNotBeNull();
            UESHConditional.AvailableConditions.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests querying available conditions
        /// </summary>
        [TestMethod]
        [DataRow(false, "1 eq 1", "", "", "", "", true)]
        [DataRow(false, "1 eq 2", "", "", "", "", false)]
        [DataRow(true, "$firstVar eq 1", "firstVar", "1", "", "", true)]
        [DataRow(true, "$firstVar eq 2", "firstVar", "1", "", "", false)]
        [DataRow(true, "$firstVar eq $secondVar", "firstVar", "1", "secondVar", "1", true)]
        [DataRow(true, "$firstVar eq $secondVar", "firstVar", "1", "secondVar", "2", false)]
        [DataRow(false, "ter.txt isfname", "", "", "", "", true)]
        [DataRow(false, "?><\"\0.zfu isfname", "", "", "", "", false)]
        [DataRow(true, "$firstVar isfname", "firstVar", "ter.txt", "", "", true)]
        [DataRow(true, "$firstVar isfname", "firstVar", "?><\"\0.zfu", "", "", false)]
        [Description("Action")]
        public void TestConditionSatisfied(bool varMode, string condition, string variable, string variableValue, string variable2, string variableValue2, bool expected)
        {
            if (varMode)
            {
                if (string.IsNullOrEmpty(variable))
                    Assert.Fail("First variable must be filled in");

                UESHVariables.InitializeVariable(variable);
                UESHVariables.SetVariable(variable, variableValue).ShouldBeTrue();
                UESHVariables.InitializeVariable(variable2);
                UESHVariables.SetVariable(variable2, variableValue2).ShouldBeTrue();
            }
            bool actual = UESHConditional.ConditionSatisfied(condition);
            actual.ShouldBe(expected);
        }

        /// <summary>
        /// Tests registering the condition and testing it
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestRegisterConditionAndTestSatisfaction()
        {
            Should.NotThrow(() => UESHConditional.RegisterCondition("haslen", new MyCondition()));
            Should.NotThrow(() => UESHConditional.ConditionSatisfied("Hello haslen")).ShouldBeTrue();
            Should.NotThrow(() => UESHConditional.ConditionSatisfied("\"\" haslen")).ShouldBeFalse();
        }

        /// <summary>
        /// Tests registering the condition and testing it
        /// </summary>
        [TestMethod]
        [Description("Action")]
        public void TestUnregisterConditionAndTestSatisfaction()
        {
            Should.NotThrow(() => UESHConditional.UnregisterCondition("haslen"));
            Should.Throw(() => UESHConditional.ConditionSatisfied("Hello haslen"), typeof(KernelException));
            Should.Throw(() => UESHConditional.ConditionSatisfied("\"\" haslen"), typeof(KernelException));
        }

    }
}
