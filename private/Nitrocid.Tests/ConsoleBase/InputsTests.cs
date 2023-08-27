
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

using KS.ConsoleBase.Inputs;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;

namespace Nitrocid.Tests.ConsoleBase
{

    [TestFixture]
    public class InputsTests
    {

        private static IEnumerable<TestCaseData> ExpectedChoicesString
        {
            get
            {
                return new[] {
                    //               ---------- Actual ----------                       ---------- Expected ----------
                    new TestCaseData("y/n", Array.Empty<string>(),                      new object[] { new string[] { "y", "n" }, new string[] { null, null }, 2 }),
                    new TestCaseData("y/n", new string[] { "Yes", "No" },               new object[] { new string[] { "y", "n" }, new string[] { "Yes", "No" }, 2 }),
                    new TestCaseData("y/n/c", Array.Empty<string>(),                    new object[] { new string[] { "y", "n", "c" }, new string[] { null, null, null }, 3 }),
                    new TestCaseData("y/n/c", new string[] { "Yes", "No", "Cancel" },   new object[] { new string[] { "y", "n", "c" }, new string[] { "Yes", "No", "Cancel" }, 3 }),
                    new TestCaseData("", Array.Empty<string>(),                         new object[] { Array.Empty<string>(), Array.Empty<string>(), 0 }),
                };
            }
        }

        private static IEnumerable<TestCaseData> ExpectedChoicesArray
        {
            get
            {
                return new[] {
                    //               ---------- Actual ----------                                               ---------- Expected ----------
                    new TestCaseData(new string[] { "y", "n" }, Array.Empty<string>(),                          new object[] { new string[] { "y", "n" }, new string[] { null, null }, 2 }),
                    new TestCaseData(new string[] { "y", "n" }, new string[] { "Yes", "No" },                   new object[] { new string[] { "y", "n" }, new string[] { "Yes", "No" }, 2 }),
                    new TestCaseData(new string[] { "y", "n", "c" }, Array.Empty<string>(),                     new object[] { new string[] { "y", "n", "c" }, new string[] { null, null, null }, 3 }),
                    new TestCaseData(new string[] { "y", "n", "c" }, new string[] { "Yes", "No", "Cancel" },    new object[] { new string[] { "y", "n", "c" }, new string[] { "Yes", "No", "Cancel" }, 3 }),
                    new TestCaseData(Array.Empty<string>(), Array.Empty<string>(),                              new object[] { Array.Empty<string>(), Array.Empty<string>(), 0 }),
                };
            }
        }

        /// <summary>
        /// Tests filtering the VT sequences that matches the regex
        /// </summary>
        [Test]
        [TestCaseSource(nameof(ExpectedChoicesString))]
        [Description("Querying")]
        public void TestGetInputChoices(string choices, string[] titles, object[] expectedData)
        {
            var inputChoices = InputChoiceTools.GetInputChoices(choices, titles);
            inputChoices.Count.ShouldBe(expectedData[2]);
            for (int i = 0; i < inputChoices.Count; i++)
            {
                InputChoiceInfo choice = inputChoices[i];
                string[] expectedChoices = (string[])expectedData[0];
                string[] expectedChoiceTitles = (string[])expectedData[1];
                choice.ChoiceName.ShouldBe(expectedChoices[i]);
                choice.ChoiceTitle.ShouldBe(expectedChoiceTitles[i]);
            }
        }

        /// <summary>
        /// Tests filtering the VT sequences that matches the regex
        /// </summary>
        [Test]
        [TestCaseSource(nameof(ExpectedChoicesArray))]
        [Description("Querying")]
        public void TestGetInputChoices(string[] choices, string[] titles, object[] expectedData)
        {
            var inputChoices = InputChoiceTools.GetInputChoices(choices, titles);
            inputChoices.Count.ShouldBe(expectedData[2]);
            for (int i = 0; i < inputChoices.Count; i++)
            {
                InputChoiceInfo choice = inputChoices[i];
                string[] expectedChoices = (string[])expectedData[0];
                string[] expectedChoiceTitles = (string[])expectedData[1];
                choice.ChoiceName.ShouldBe(expectedChoices[i]);
                choice.ChoiceTitle.ShouldBe(expectedChoiceTitles[i]);
            }
        }

    }
}
