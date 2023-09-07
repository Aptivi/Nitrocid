
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

using KS.Shell.ShellBase.Scripting;
using NUnit.Framework;
using Shouldly;
using System;
using System.Linq.Expressions;

namespace Nitrocid.Tests.Shell.ShellBase.Scripting
{

    [TestFixture]
    public class UESHVariableTests
    {

        /// <summary>
        /// Tests sanitizing variable name
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestSanitizeVariableNamesWithDollarSign()
        {
            string expected = "$my_var";
            string sanitized = UESHVariables.SanitizeVariableName("$my_var");
            sanitized.ShouldBe(expected);
        }

        /// <summary>
        /// Tests sanitizing variable name
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestSanitizeVariableNamesWithoutDollarSign()
        {
            string expected = "$my_var";
            string sanitized = UESHVariables.SanitizeVariableName("my_var");
            sanitized.ShouldBe(expected);
        }

        /// <summary>
        /// Tests initializing, setting, and getting $variable
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestVariables()
        {
            UESHVariables.InitializeVariable("$test_var");
            UESHVariables.Variables.ShouldNotBeEmpty();
            UESHVariables.SetVariable("$test_var", "test").ShouldBeTrue();
            UESHVariables.GetVariable("$test_var").ShouldBe("test");
            UESHVariables.SetVariables("$test_var_arr", new[] { "Nitrocid", "KS" }).ShouldBeTrue();
            UESHVariables.GetVariable("$test_var_arr[0]").ShouldBe("Nitrocid");
            UESHVariables.GetVariable("$test_var_arr[1]").ShouldBe("KS");
            string ExpectedCommand = "echo test";
            string ActualCommand = UESHVariables.GetVariableCommand("$test_var", "echo $test_var");
            ActualCommand.ShouldBe(ExpectedCommand);
        }

        /// <summary>
        /// Tests converting the environment variables to UESH's declaration
        /// </summary>
        [Test]
        [Description("Action")]
        public void TestConvertEnvVariables()
        {
            UESHVariables.ConvertSystemEnvironmentVariables();
            UESHVariables.Variables.ShouldNotBeNull();
            UESHVariables.Variables.ShouldNotBeEmpty();
            UESHVariables.Variables.Count.ShouldBeGreaterThan(1);
        }

        /// <summary>
        /// Tests initializing the variables from...
        /// </summary>
        [Test]
        [TestCase("", "", "")]
        [TestCase("$test=bar", "$test", "bar")]
        [TestCase("$test2=bars $test3=\"Nitrocid KS\"", "$test2|$test3", "bars|Nitrocid KS")]
        [Description("Action")]
        public void TestInitializeVariablesFrom(string expression, string expectedKeys, string expectedValues)
        {
            UESHVariables.InitializeVariablesFrom(expression);
            string[] expectedKeysArray = string.IsNullOrEmpty(expectedKeys) ? Array.Empty<string>() : expectedKeys.Split('|');
            string[] expectedValuesArray = string.IsNullOrEmpty(expectedValues) ? Array.Empty<string>() : expectedValues.Split('|');
            for (int i = 0; i < expectedKeysArray.Length; i++)
            {
                string key = expectedKeysArray[i];
                string value = expectedValuesArray[i];
                UESHVariables.Variables.ShouldContainKeyAndValue(key, value);
            }
        }

        /// <summary>
        /// Tests getting the variables from...
        /// </summary>
        [Test]
        [TestCase("", "", "")]
        [TestCase("$test=bar", "$test", "bar")]
        [TestCase("$test2=bars $test3=\"Nitrocid KS\"", "$test2|$test3", "bars|\"Nitrocid KS\"")]
        [Description("Action")]
        public void TestGetVariablesFrom(string expression, string expectedKeys, string expectedValues)
        {
            var (varStoreKeys, varStoreValues) = UESHVariables.GetVariablesFrom(expression);
            string[] expectedKeysArray = string.IsNullOrEmpty(expectedKeys) ? Array.Empty<string>() : expectedKeys.Split('|');
            string[] expectedValuesArray = string.IsNullOrEmpty(expectedValues) ? Array.Empty<string>() : expectedValues.Split('|');
            string[] actualKeysArray = varStoreKeys;
            string[] actualValuesArray = varStoreValues;
            actualKeysArray.ShouldBe(expectedKeysArray);
            actualValuesArray.ShouldBe(expectedValuesArray);
        }

    }
}
