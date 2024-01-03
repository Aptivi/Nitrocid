//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Nitrocid.Shell.ShellBase.Scripting;
using Nitrocid.Shell.ShellBase.Scripting.Conditions;
using Nitrocid.Tests.Shell.ShellBase.Scripting.CustomConditions;
using NUnit.Framework;
using Shouldly;

namespace Nitrocid.Tests.Shell.ShellBase.Scripting
{

    [TestFixture]
    public class UESHConditionTests
    {

        /// <summary>
        /// Tests querying available conditions
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestQueryAvailableConditions()
        {
            UESHConditional.AvailableConditions.ShouldNotBeNull();
            UESHConditional.AvailableConditions.ShouldNotBeEmpty();
        }

        /// <summary>
        /// Tests querying available conditions
        /// </summary>
        [Test]
        [TestCase(false, "1 eq 1", "", "", "", "", ExpectedResult = true)]
        [TestCase(false, "1 eq 2", "", "", "", "", ExpectedResult = false)]
        [TestCase(true, "$firstVar eq 1", "firstVar", "1", "", "", ExpectedResult = true)]
        [TestCase(true, "$firstVar eq 2", "firstVar", "1", "", "", ExpectedResult = false)]
        [TestCase(true, "$firstVar eq $secondVar", "firstVar", "1", "secondVar", "1", ExpectedResult = true)]
        [TestCase(true, "$firstVar eq $secondVar", "firstVar", "1", "secondVar", "2", ExpectedResult = false)]
        [TestCase(false, "ter.txt isfname", "", "", "", "", ExpectedResult = true)]
        [TestCase(false, "?><\"\0.zfu isfname", "", "", "", "", ExpectedResult = false)]
        [TestCase(true, "$firstVar isfname", "firstVar", "ter.txt", "", "", ExpectedResult = true)]
        [TestCase(true, "$firstVar isfname", "firstVar", "?><\"\0.zfu", "", "", ExpectedResult = false)]
        [Description("Action")]
        public bool TestConditionSatisfied(bool varMode, string condition, string variable, string variableValue, string variable2, string variableValue2)
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
            return UESHConditional.ConditionSatisfied(condition);
        }

        /// <summary>
        /// Tests registering the condition and testing it
        /// </summary>
        [Test]
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
        [Test]
        [Description("Action")]
        public void TestUnregisterConditionAndTestSatisfaction()
        {
            Should.NotThrow(() => UESHConditional.UnregisterCondition("haslen"));
            Should.Throw(() => UESHConditional.ConditionSatisfied("Hello haslen"), typeof(KernelException));
            Should.Throw(() => UESHConditional.ConditionSatisfied("\"\" haslen"), typeof(KernelException));
        }

    }
}
