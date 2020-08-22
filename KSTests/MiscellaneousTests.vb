
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports System.IO
Imports System.Reflection
Imports KS

<TestClass()> Public Class MiscellaneousTests

    <TestMethod()> Public Sub TestCalcSum()
        Try
            Dim ExpectedSum As Integer = 21
            Dim ActualSum As Integer = DoCalc("13 + 8").Keys(0)
            Assert.AreEqual(ExpectedSum, ActualSum)
        Catch afex As AssertFailedException
            Assert.Fail("DoCalc didn't calculate properly.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestCalcSub()
        Try
            Dim ExpectedSum As Integer = 17
            Dim ActualSum As Integer = DoCalc("42 - 25").Keys(0)
            Assert.AreEqual(ExpectedSum, ActualSum)
        Catch afex As AssertFailedException
            Assert.Fail("DoCalc didn't calculate properly.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestCalcMul()
        Try
            Dim ExpectedSum As Integer = 182
            Dim ActualSum As Integer = DoCalc("91 * 2").Keys(0)
            Assert.AreEqual(ExpectedSum, ActualSum)
        Catch afex As AssertFailedException
            Assert.Fail("DoCalc didn't calculate properly.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestCalcDiv()
        Try
            Dim ExpectedSum As Integer = 5
            Dim ActualSum As Integer = DoCalc("250 / 50").Keys(0)
            Assert.AreEqual(ExpectedSum, ActualSum)
        Catch afex As AssertFailedException
            Assert.Fail("DoCalc didn't calculate properly.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestCalcSumDbl()
        Try
            Dim ExpectedSum As Double = 64.9
            Dim ActualSum As Double = DoCalc("32.4 + 32.5").Keys(0)
            Assert.AreEqual(ExpectedSum, ActualSum)
        Catch afex As AssertFailedException
            Assert.Fail("DoCalc didn't calculate properly.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestCalcSubDbl()
        Try
            Dim ExpectedSum As Double = 66.9
            Dim ActualSum As Double = DoCalc("69 - 2.1").Keys(0)
            Assert.AreEqual(ExpectedSum, ActualSum)
        Catch afex As AssertFailedException
            Assert.Fail("DoCalc didn't calculate properly.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestCalcMulDbl()
        Try
            Dim ExpectedSum As Double = 4.8
            Dim ActualSum As Double = DoCalc("1.2 * 4").Keys(0)
            Assert.AreEqual(ExpectedSum, ActualSum)
        Catch afex As AssertFailedException
            Assert.Fail("DoCalc didn't calculate properly.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestCalcDivDbl()
        Try
            Dim ExpectedSum As Double = 2.5
            Dim ActualSum As Double = DoCalc("5 / 2").Keys(0)
            Assert.AreEqual(ExpectedSum, ActualSum)
        Catch afex As AssertFailedException
            Assert.Fail("DoCalc didn't calculate properly.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestGetValue()
        Try
            Dim Value As String = GetValue("HiddenFiles")
            Assert.IsNotNull(Value)
        Catch afex As AssertFailedException
            Assert.Fail("Value of variable HiddenFiles isn't get properly.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestSetValue()
        Try
            SetValue("HiddenFiles", False)
            Dim Value As String = GetValue("HiddenFiles")
            Assert.AreEqual(Value, "False")
        Catch afex As AssertFailedException
            Assert.Fail("Value of variable HiddenFiles isn't set properly.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestGetField()
        Try
            Dim Field As FieldInfo = GetField("HiddenFiles")
            Assert.IsTrue(Field.Name = "HiddenFiles")
        Catch afex As AssertFailedException
            Assert.Fail("Field HiddenFiles isn't get properly.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestGetEncryptedString()
        Try
            Dim TextHash As String = "Test hashing."
            Dim TextHashMD5 As String = GetEncryptedString(TextHash, Algorithms.MD5)
            Dim TextHashSHA1 As String = GetEncryptedString(TextHash, Algorithms.SHA1)
            Dim TextHashSHA256 As String = GetEncryptedString(TextHash, Algorithms.SHA256)
            Assert.IsTrue(TextHashMD5 = "C4C1867580D6D25B11210F84F935359A" And
                          TextHashSHA1 = "CFF9FDA895B0B638957E17CF952457D81ADD622F" And
                          TextHashSHA256 = "525514740C93C5442DBCB8FB92FB1B17B6F8B94B3C98E6F07CA8AEB093C2E79F")
        Catch afex As AssertFailedException
            Assert.Fail("Encrypted string hash isn't get properly")
        End Try
    End Sub

    <TestMethod()> Public Sub TestGetEncryptedFile()
        Try
            InitPaths()
            Dim FileStreamHash As FileStream = File.Create(paths("Home") + "/TestSum.txt")
            FileStreamHash.Write(Text.Encoding.Default.GetBytes("Test hashing."), 0, 13)
            FileStreamHash.Flush()
            Dim FileHashMD5 As String = GetEncryptedFile(FileStreamHash, Algorithms.MD5)
            Dim FileHashSHA1 As String = GetEncryptedFile(FileStreamHash, Algorithms.SHA1)
            Dim FileHashSHA256 As String = GetEncryptedFile(FileStreamHash, Algorithms.SHA256)
            FileStreamHash.Close()
            File.Delete(paths("Home") + "/TestSum.txt")
            Assert.IsTrue(FileHashMD5 = "D41D8CD98F00B204E9800998ECF8427E" And
                          FileHashSHA1 = "DA39A3EE5E6B4B0D3255BFEF95601890AFD80709" And
                          FileHashSHA256 = "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855")
        Catch afex As AssertFailedException
            Assert.Fail("Encrypted file hash isn't get properly")
        End Try
    End Sub

    <TestMethod()> Public Sub TestSetMOTD()
        Try
            InitPaths()
            SetMOTD(ProbePlaces("Hello, I am on <system>"), MessageType.MOTD)
            Dim MOTDFile As New StreamReader(paths("Home") + "/MOTD.txt")
            Assert.IsTrue(MOTDFile.ReadLine = ProbePlaces("Hello, I am on <system>"))
        Catch afex As AssertFailedException
            Assert.Fail("Setting MOTD failed.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestSetMAL()
        Try
            InitPaths()
            SetMOTD(ProbePlaces("Hello, I am on <system>"), MessageType.MAL)
            Dim MALFile As New StreamReader(paths("Home") + "/MAL.txt")
            Assert.IsTrue(MALFile.ReadLine = ProbePlaces("Hello, I am on <system>"))
        Catch afex As AssertFailedException
            Assert.Fail("Setting MAL failed.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestReadMOTDFromFile()
        Try
            InitPaths()
            ReadMOTDFromFile(MessageType.MOTD)
            Dim MOTDFile As New StreamReader(paths("Home") + "/MOTD.txt")
            Assert.IsTrue(MOTDFile.ReadLine = MOTDMessage.ReplaceLastOccurrence(Environment.NewLine, ""))
        Catch afex As AssertFailedException
            Assert.Fail("Reading MOTD failed.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestReadMALFromFile()
        Try
            InitPaths()
            ReadMOTDFromFile(MessageType.MAL)
            Dim MALFile As New StreamReader(paths("Home") + "/MAL.txt")
            Assert.IsTrue(MALFile.ReadLine = MAL)
        Catch afex As AssertFailedException
            Assert.Fail("Reading MAL failed.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestParsePlaceholders()
        Dim UnparsedStrings As New List(Of String)
        Try
            InitPaths() 'For some reason, ProbePlaces' event raise likes to use paths...
            signedinusrnm = "Test"
            Dim ParsedStrings As New List(Of String)
            ParsedStrings.Add(ProbePlaces("Username is <user>"))
            ParsedStrings.Add(ProbePlaces("Short date is <shortdate>"))
            ParsedStrings.Add(ProbePlaces("Long date is <longdate>"))
            ParsedStrings.Add(ProbePlaces("Short time is <shorttime>"))
            ParsedStrings.Add(ProbePlaces("Long time is <longtime>"))
            ParsedStrings.Add(ProbePlaces("Date is <date>"))
            ParsedStrings.Add(ProbePlaces("Time is <time>"))
            ParsedStrings.Add(ProbePlaces("Timezone is <timezone>"))
            ParsedStrings.Add(ProbePlaces("Summer timezone is <summertimezone>"))
            ParsedStrings.Add(ProbePlaces("Operating system is <system>"))
            For Each ParsedString As String In ParsedStrings
                If ParsedString.Contains("<") And ParsedString.Contains(">") Then
                    UnparsedStrings.Add(ParsedString)
                End If
            Next
            Assert.IsTrue(UnparsedStrings.Count = 0)
        Catch afex As AssertFailedException
            Assert.Fail("Parsing placeholders failed. Below strings are affected:" + vbNewLine + vbNewLine +
                        "- " + String.Join(vbNewLine + "- ", UnparsedStrings.ToArray))
        End Try
    End Sub

    <TestMethod()> Public Sub TestOpenSaveCloseTextFile()
        Dim CurrentState As String = "Opening"
        Try
            Dim PathToTestText As String = Path.GetFullPath("TestText.txt")
            Assert.IsTrue(TextEdit_OpenTextFile(PathToTestText))
            CurrentState = "Saving"
            TextEdit_FileLines.Add("Hello!")
            Assert.IsTrue(TextEdit_SaveTextFile())
            CurrentState = "Closing"
            Assert.IsTrue(TextEdit_CloseTextFile())
        Catch afex As AssertFailedException
            Assert.Fail("{0} text file failed.", CurrentState)
        End Try
    End Sub

    <TestMethod()> Public Sub TestReadAllLinesNoBlock()
        Try
            Dim PathToTestText As String = Path.GetFullPath("TestText.txt")
            Dim LinesTestText As String() = ReadAllLinesNoBlock(PathToTestText)
            Assert.IsInstanceOfType(LinesTestText, GetType(String()))
        Catch afex As AssertFailedException
            Assert.Fail("Reading all lines failed.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestEvaluateString()
        Try
            Dim Evaluated As String = Evaluate("KS.Kernel.KernelVersion")
            Assert.IsFalse(Evaluated = "")
        Catch afex As AssertFailedException
            Assert.Fail("String evaluation failed.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestReplaceLastOccurrence()
        Try
            Dim Source As String = "Kernel Simulation from Eofla Kernel"
            Dim Target As String = "Kernel"
            Assert.AreEqual(Source.ReplaceLastOccurrence(Target, "OS"), "Kernel Simulation from Eofla OS")
        Catch afex As AssertFailedException
            Assert.Fail("Replacement failed.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestAllIndexesOf()
        Try
            Dim Source As String = "Kernel Simulation from Eofla Kernel"
            Dim Target As String = "a"
            Assert.AreEqual(Source.AllIndexesOf(Target).Count, 2)
        Catch afex As AssertFailedException
            Assert.Fail("Getting all indexes of a character failed.")
        End Try
    End Sub

    <TestMethod()> Public Sub TestTruncate()
        Try
            Dim Source As String = "Kernel Simulation from Eofla Kernel"
            Dim Target As Integer = 20
            Assert.AreEqual(Source.Truncate(Target), "Kernel Simulation f...")
        Catch afex As AssertFailedException
            Assert.Fail("Truncation failed.")
        End Try
    End Sub

End Class