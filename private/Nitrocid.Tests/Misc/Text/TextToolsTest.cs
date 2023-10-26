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

using KS.Misc.Text;
using NUnit.Framework;
using Shouldly;
using System.Linq;

namespace Nitrocid.Tests.Misc.Text
{

    [TestFixture]
    public class TextToolsTest
    {

        /// <summary>
        /// Tests getting wrapped sentences
        /// </summary>
        [Test]
        [Description("Querying")]
        public static void TestGetWrappedSentences()
        {
            var sentences = TextTools.GetWrappedSentences("Nitrocid", 4);
            sentences.ShouldNotBeNull();
            sentences.ShouldNotBeEmpty();
            sentences.Length.ShouldBe(2);
            sentences[0].ShouldBe("Nitr");
            sentences[1].ShouldBe("ocid");
        }

        /// <summary>
        /// Tests getting wrapped sentences
        /// </summary>
        [Test]
        [Description("Querying")]
        public static void TestGetWrappedSentencesIndented()
        {
            var sentences = TextTools.GetWrappedSentences("Nitrocid", 4, 2);
            sentences.ShouldNotBeNull();
            sentences.ShouldNotBeEmpty();
            sentences.Length.ShouldBe(3);
            sentences[0].ShouldBe("Ni");
            sentences[1].ShouldBe("troc");
            sentences[2].ShouldBe("id");
        }

        /// <summary>
        /// Tests replacing last occurrence of a string
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestReplaceLastOccurrence()
        {
            string expected = "Nitrocid is awesome and its features are great!";
            string Source = "Nitrocid is awesome and is great!";
            string Target = "is";
            Source = Source.ReplaceLastOccurrence(Target, "its features are");
            Source.ShouldBe(expected);
        }

        /// <summary>
        /// Tests replacing all specified occurrences of strings with a single string
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestReplaceAll()
        {
            string ExpectedString = "Please test Nitrocid. This sub is a unit test.";
            string TargetString = "Please <replace> Nitrocid. This sub is a unit <replace2>.";
            TargetString = TargetString.ReplaceAll(new[] { "<replace>", "<replace2>" }, "test");
            TargetString.ShouldBe(ExpectedString);
        }

        /// <summary>
        /// Tests replacing all specified occurrences of strings with multiple strings
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestReplaceAllRange()
        {
            string ExpectedString = "Please test the integrity of Nitrocid. This sub is a unit test.";
            string TargetString = "Please <replace> Nitrocid. This sub is a unit <replace2>.";
            TargetString = TargetString.ReplaceAllRange(new[] { "<replace>", "<replace2>" }, new[] { "test the integrity of", "test" });
            TargetString.ShouldBe(ExpectedString);
        }

        /// <summary>
        /// Tests truncating...
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestTruncate()
        {
            string expected = "Nitrocid is awesome...";
            string Source = "Nitrocid is awesome and is great!";
            int Target = 20;
            Source = Source.Truncate(Target);
            Source.ShouldBe(expected);
        }

        /// <summary>
        /// Tests string formatting
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestFormatString()
        {
            string Expected = "Kernel Simulator 0.0.1 first launched 2/22/2018.";
            string Actual = "Kernel Simulator 0.0.1 first launched {0}/{1}/{2}.";
            int Day = 22;
            int Year = 2018;
            int Month = 2;
            Actual = TextTools.FormatString(Actual, Month, Day, Year);
            Actual.ShouldBe(Expected);
        }

        /// <summary>
        /// Tests string formatting with reference to null
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestFormatStringNullReference()
        {
            string Expected = "Nothing is ";
            string Actual = "{0} is {1}";
            Actual = TextTools.FormatString(Actual, "Nothing", null);
            Actual.ShouldBe(Expected);
        }

        /// <summary>
        /// Tests reserving orders of characters in a string
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestReverse()
        {
            string expected = "Hello";
            string TargetString = "olleH";
            TargetString.Reverse().ShouldBe(expected);
        }

        /// <summary>
        /// Tests releasing a string from double quotes
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestReleaseDoubleQuotes()
        {
            string expected = "Hi!";
            string TargetString = "\"Hi!\"";
            TargetString.ReleaseDoubleQuotes().ShouldBe(expected);
        }

        /// <summary>
        /// Tests getting all indexes of a character
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestAllIndexesOf()
        {
            int expected = 3;
            string Source = "Nitrocid is awesome and is great!";
            string Target = "a";
            int Indexes = Source.AllIndexesOf(Target).Count();
            Indexes.ShouldBe(expected);
        }

        /// <summary>
        /// Tests checking if the string contains any of the target strings.
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestContainsAnyOf()
        {
            string TargetString = "Hello, Nitrocid users!";
            TargetString.ContainsAnyOf(new[] { "Nitrocid", "users" }).ShouldBeTrue();
            TargetString.ContainsAnyOf(new[] { "Awesome", "developers" }).ShouldBeFalse();
        }

        /// <summary>
        /// Tests checking to see if the string starts with any of the values
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestStartsWithAnyOf()
        {
            string TargetString = "dotnet-hostfxr-3.1 dotnet-hostfxr-5.0 runtime-3.1 runtime-5.0 sdk-3.1 sdk-5.0";
            TargetString.StartsWithAnyOf(new[] { "dotnet", "sdk" }).ShouldBeTrue();
        }

        /// <summary>
        /// Tests splitting a string with new lines (vbCrLf)
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestSplitNewLinesCrLf()
        {
            string TargetString = "First line\r\nSecond line\r\nThird line";
            var TargetArray = TargetString.SplitNewLines();
            TargetArray.Length.ShouldBe(3);
        }

        /// <summary>
        /// Tests splitting a string with new lines (vbLf)
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestSplitNewLinesLf()
        {
            string TargetString = "First line\nSecond line\nThird line";
            var TargetArray = TargetString.SplitNewLines();
            TargetArray.Length.ShouldBe(3);
        }

        /// <summary>
        /// Tests splitting a string with double quotes enclosed
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestSplitEncloseDoubleQuotes()
        {
            string TargetString = "First \"Second Third\" Fourth";
            var TargetArray = TargetString.SplitEncloseDoubleQuotes();
            TargetArray.Length.ShouldBe(3);
        }

        /// <summary>
        /// Tests checking to see if the string is numeric
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestIsStringNumeric()
        {
            string TargetString = "1";
            TextTools.IsStringNumeric(TargetString).ShouldBeTrue();
        }

        /// <summary>
        /// [Counterexample] Tests checking to see if the string is numeric
        /// </summary>
        [Test]
        [Description("Querying")]
        public void TestIsStringNumericCounterexample()
        {
            string TargetString = "a";
            TextTools.IsStringNumeric(TargetString).ShouldBeFalse();
        }

    }

}
