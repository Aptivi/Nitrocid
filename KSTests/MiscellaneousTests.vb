
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

    ''' <summary>
    ''' Tests sum calculation (integers)
    ''' </summary>
    <TestMethod()> Public Sub TestCalcSum()
        Dim ExpectedSum As Integer = 21
        Dim ActualSum As Integer = DoCalc("13 + 8").Keys(0)
        Assert.AreEqual(ExpectedSum, ActualSum, "DoCalc didn't calculate properly. Got {0}", ActualSum)
    End Sub

    ''' <summary>
    ''' Tests sub calculation (integers)
    ''' </summary>
    <TestMethod()> Public Sub TestCalcSub()
        Dim ExpectedSum As Integer = 17
        Dim ActualSum As Integer = DoCalc("42 - 25").Keys(0)
        Assert.AreEqual(ExpectedSum, ActualSum, "DoCalc didn't calculate properly. Got {0}", ActualSum)
    End Sub

    ''' <summary>
    ''' Tests mul calculation (integers)
    ''' </summary>
    <TestMethod()> Public Sub TestCalcMul()
        Dim ExpectedSum As Integer = 182
        Dim ActualSum As Integer = DoCalc("91 * 2").Keys(0)
        Assert.AreEqual(ExpectedSum, ActualSum, "DoCalc didn't calculate properly. Got {0}", ActualSum)
    End Sub

    ''' <summary>
    ''' Tests div calculation (integers)
    ''' </summary>
    <TestMethod()> Public Sub TestCalcDiv()
        Dim ExpectedSum As Integer = 5
        Dim ActualSum As Integer = DoCalc("250 / 50").Keys(0)
        Assert.AreEqual(ExpectedSum, ActualSum, "DoCalc didn't calculate properly. Got {0}", ActualSum)
    End Sub

    ''' <summary>
    ''' Tests sum calculation (doubles)
    ''' </summary>
    <TestMethod()> Public Sub TestCalcSumDbl()
        Dim ExpectedSum As Double = 64.9
        Dim ActualSum As Double = DoCalc("32.4 + 32.5").Keys(0)
        Assert.AreEqual(ExpectedSum, ActualSum, "DoCalc didn't calculate properly. Got {0}", ActualSum)
    End Sub

    ''' <summary>
    ''' Tests sub calculation (doubles)
    ''' </summary>
    <TestMethod()> Public Sub TestCalcSubDbl()
        Dim ExpectedSum As Double = 66.9
        Dim ActualSum As Double = DoCalc("69 - 2.1").Keys(0)
        Assert.AreEqual(ExpectedSum, ActualSum, "DoCalc didn't calculate properly. Got {0}", ActualSum)
    End Sub

    ''' <summary>
    ''' Tests mul calculation (doubles)
    ''' </summary>
    <TestMethod()> Public Sub TestCalcMulDbl()
        Dim ExpectedSum As Double = 4.8
        Dim ActualSum As Double = DoCalc("1.2 * 4").Keys(0)
        Assert.AreEqual(ExpectedSum, ActualSum, "DoCalc didn't calculate properly. Got {0}", ActualSum)
    End Sub

    ''' <summary>
    ''' Tests div calculation (doubles)
    ''' </summary>
    <TestMethod()> Public Sub TestCalcDivDbl()
        Dim ExpectedSum As Double = 2.5
        Dim ActualSum As Double = DoCalc("5 / 2").Keys(0)
        Assert.AreEqual(ExpectedSum, ActualSum, "DoCalc didn't calculate properly. Got {0}", ActualSum)
    End Sub

    ''' <summary>
    ''' Tests getting value
    ''' </summary>
    <TestMethod()> Public Sub TestGetValue()
        Dim Value As String = GetValue("HiddenFiles")
        Assert.IsNotNull(Value, "Value of variable HiddenFiles isn't get properly. Got null.")
    End Sub

    ''' <summary>
    ''' Tests setting value
    ''' </summary>
    <TestMethod()> Public Sub TestSetValue()
        SetValue("HiddenFiles", False)
        Dim Value As String = GetValue("HiddenFiles")
        Assert.AreEqual(Value, "False", "Value of variable HiddenFiles isn't set properly. Got {0}", Value)
    End Sub

    ''' <summary>
    ''' Tests getting variable
    ''' </summary>
    <TestMethod()> Public Sub TestGetField()
        Dim Field As FieldInfo = GetField("HiddenFiles")
        Assert.IsTrue(Field.Name = "HiddenFiles", "Field HiddenFiles isn't get properly. Name: {0}", Field.Name)
    End Sub

    ''' <summary>
    ''' Tests string encryption
    ''' </summary>
    <TestMethod()> Public Sub TestGetEncryptedString()
        Dim TextHash As String = "Test hashing."
        Dim TextHashMD5 As String = GetEncryptedString(TextHash, Algorithms.MD5)
        Dim TextHashSHA1 As String = GetEncryptedString(TextHash, Algorithms.SHA1)
        Dim TextHashSHA256 As String = GetEncryptedString(TextHash, Algorithms.SHA256)
        Assert.IsTrue(TextHashMD5 = "C4C1867580D6D25B11210F84F935359A" And
                      TextHashSHA1 = "CFF9FDA895B0B638957E17CF952457D81ADD622F" And
                      TextHashSHA256 = "525514740C93C5442DBCB8FB92FB1B17B6F8B94B3C98E6F07CA8AEB093C2E79F", "Encrypted string hash isn't get properly. Got:" + vbNewLine +
                                                                                                           TextHashMD5 + vbNewLine +
                                                                                                           TextHashSHA1 + vbNewLine +
                                                                                                           TextHashSHA256)
    End Sub

    ''' <summary>
    ''' Tests file encryption
    ''' </summary>
    <TestMethod()> Public Sub TestGetEncryptedFile()
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
                      FileHashSHA256 = "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855", "Encrypted file hash isn't get properly. Got:" + vbNewLine +
                                                                                                           FileHashMD5 + vbNewLine +
                                                                                                           FileHashSHA1 + vbNewLine +
                                                                                                           FileHashSHA256)
    End Sub

    ''' <summary>
    ''' Tests setting MOTD
    ''' </summary>
    <TestMethod()> Public Sub TestSetMOTD()
        InitPaths()
        SetMOTD(ProbePlaces("Hello, I am on <system>"), MessageType.MOTD)
        Dim MOTDFile As New StreamReader(paths("Home") + "/MOTD.txt")
        Assert.IsTrue(MOTDFile.ReadLine = ProbePlaces("Hello, I am on <system>"), "Setting MOTD failed.")
    End Sub

    ''' <summary>
    ''' Tests setting MAL
    ''' </summary>
    <TestMethod()> Public Sub TestSetMAL()
        InitPaths()
        SetMOTD(ProbePlaces("Hello, I am on <system>"), MessageType.MAL)
        Dim MALFile As New StreamReader(paths("Home") + "/MAL.txt")
        Assert.IsTrue(MALFile.ReadLine = ProbePlaces("Hello, I am on <system>"), "Setting MAL failed.")
    End Sub

    ''' <summary>
    ''' Tests reading MOTD from file
    ''' </summary>
    <TestMethod()> Public Sub TestReadMOTDFromFile()
        InitPaths()
        ReadMOTDFromFile(MessageType.MOTD)
        Dim MOTDLine As String = File.ReadAllText(paths("Home") + "/MOTD.txt")
        Assert.IsTrue(MOTDLine = MOTDMessage, "Reading MOTD failed. Got:" + vbNewLine + MOTDLine)
    End Sub

    ''' <summary>
    ''' Tests reading MAL from file
    ''' </summary>
    <TestMethod()> Public Sub TestReadMALFromFile()
        InitPaths()
        ReadMOTDFromFile(MessageType.MAL)
        Dim MALLine As String = File.ReadAllText(paths("Home") + "/MAL.txt")
        Assert.IsTrue(MALLine = MAL, "Reading MAL failed. Got:" + vbNewLine + MALLine)
    End Sub

    ''' <summary>
    ''' Tests parsing placeholders
    ''' </summary>
    <TestMethod()> Public Sub TestParsePlaceholders()
        Dim UnparsedStrings As New List(Of String)
        InitPaths() 'For some reason, ProbePlaces' event raise likes to use paths...
        signedinusrnm = "Test"
        Dim ParsedStrings As New List(Of String) From {
            ProbePlaces("Username is <user>"),
            ProbePlaces("Short date is <shortdate>"),
            ProbePlaces("Long date is <longdate>"),
            ProbePlaces("Short time is <shorttime>"),
            ProbePlaces("Long time is <longtime>"),
            ProbePlaces("Date is <date>"),
            ProbePlaces("Time is <time>"),
            ProbePlaces("Timezone is <timezone>"),
            ProbePlaces("Summer timezone is <summertimezone>"),
            ProbePlaces("Operating system is <system>")
        }
        For Each ParsedString As String In ParsedStrings
            If ParsedString.Contains("<") And ParsedString.Contains(">") Then
                UnparsedStrings.Add(ParsedString)
            End If
        Next
        Assert.IsTrue(UnparsedStrings.Count = 0, "Parsing placeholders failed. Below strings are affected:" + vbNewLine + vbNewLine +
                                                 "- " + String.Join(vbNewLine + "- ", UnparsedStrings.ToArray))
    End Sub

    ''' <summary>
    ''' Tests opening, saving, and closing text file
    ''' </summary>
    <TestMethod()> Public Sub TestOpenSaveCloseTextFile()
        Dim PathToTestText As String = Path.GetFullPath("TestText.txt")
        Assert.IsTrue(TextEdit_OpenTextFile(PathToTestText), "Opening text file failed. Returned False.")
        TextEdit_FileLines.Add("Hello!")
        Assert.IsTrue(TextEdit_SaveTextFile(False), "Saving text file failed. Returned False.")
        Assert.IsTrue(TextEdit_CloseTextFile(), "Closing text file failed. Returned False.")
    End Sub

    ''' <summary>
    ''' Tests reading all lines without roadblocks
    ''' </summary>
    <TestMethod()> Public Sub TestReadAllLinesNoBlock()
        Dim PathToTestText As String = Path.GetFullPath("TestText.txt")
        Dim LinesTestText As String() = ReadAllLinesNoBlock(PathToTestText)
        Assert.IsInstanceOfType(LinesTestText, GetType(String()), "Reading all lines failed.")
    End Sub

    ''' <summary>
    ''' Tests variable evaluation
    ''' </summary>
    <TestMethod()> Public Sub TestEvaluateString()
        Dim Evaluated As String = Evaluate("KS.Kernel.KernelVersion")
        Assert.IsFalse(Evaluated = "", "String evaluation failed. Got ""{0}"".", Evaluated)
    End Sub

    ''' <summary>
    ''' Tests replacing last occurrence
    ''' </summary>
    <TestMethod()> Public Sub TestReplaceLastOccurrence()
        Dim Source As String = "Kernel Simulation from Eofla Kernel"
        Dim Target As String = "Kernel"
        Source = Source.ReplaceLastOccurrence(Target, "OS")
        Assert.AreEqual(Source, "Kernel Simulation from Eofla OS", "Replacement failed. Got {0}", Source)
    End Sub

    ''' <summary>
    ''' Tests getting all indexes of a character
    ''' </summary>
    <TestMethod()> Public Sub TestAllIndexesOf()
        Dim Source As String = "Kernel Simulation from Eofla Kernel"
        Dim Target As String = "a"
        Dim Indexes As Integer = Source.AllIndexesOf(Target).Count
        Assert.AreEqual(Indexes, 2, "Getting all indexes of a character failed. Expected 2, got {0}", Indexes)
    End Sub

    ''' <summary>
    ''' Tests truncating...
    ''' </summary>
    <TestMethod()> Public Sub TestTruncate()
        Dim Source As String = "Kernel Simulation from Eofla Kernel"
        Dim Target As Integer = 20
        Source = Source.Truncate(Target)
        Assert.AreEqual(Source, "Kernel Simulation f...", "Truncation failed. Got {0}", Source)
    End Sub

    ''' <summary>
    ''' Tests string formatting
    ''' </summary>
    <TestMethod()> Public Sub TestFormatString()
        Dim Expected As String = "Kernel Simulator 0.0.1 first launched 2/22/2018."
        Dim Actual As String = "Kernel Simulator 0.0.1 first launched {0}/{1}/{2}."
        Dim Day As Integer = 22
        Dim Year As Integer = 2018
        Dim Month As Integer = 2
        Actual = Actual.FormatString(Month, Day, Year)
        Assert.AreEqual(Expected, Actual, "Format failed. Got {0}", Actual)
    End Sub

    ''' <summary>
    ''' Tests synth probing
    ''' </summary>
    <TestMethod()> Public Sub TestProbeSynth() 'If not working on AppVeyor, remove it.
        InitPaths()
        CurrDir = paths("Home")
        Dim PathToTestSynth As String = Path.GetFullPath("TestSynth.txt")
        Dim Successful As Boolean = ProbeSynth(PathToTestSynth)
        Assert.IsTrue(Successful, "Synth probing failed. Expected True, got {0}", Successful)
    End Sub

    ''' <summary>
    ''' Tests setting default screensaver
    ''' </summary>
    <TestMethod()> Public Sub TestSetDefaultScreensaver()
        InitPaths()
        SetDefaultScreensaver("matrix")
        Assert.IsTrue(ScrnSvrdb("matrix"), "Setting screensaver defaults failed. Expected True, got {0}", ScrnSvrdb("matrix"))
    End Sub

    ''' <summary>
    ''' Tests unsetting default screensaver
    ''' </summary>
    <TestMethod()> Public Sub TestUnsetDefaultScreensaver()
        InitPaths()
        SetDefaultScreensaver("matrix", False)
        Assert.IsFalse(ScrnSvrdb("matrix"), "Setting screensaver defaults failed. Expected False, got {0}", ScrnSvrdb("matrix"))
    End Sub

    ''' <summary>
    ''' Tests initializing, setting, and getting $variable
    ''' </summary>
    <TestMethod> Public Sub TestVariables()
        InitializeVariable("$test_var")
        Assert.IsTrue(ScriptVariables.Count > 0, "Initializing variable failed. Count is {0}", ScriptVariables.Count)
        Assert.IsTrue(SetVariable("$test_var", "test"), "Setting variable failed.")
        Dim ExpectedCommand As String = "echo test"
        Dim ActualCommand As String = GetVariable("$test_var", "echo $test_var")
        Assert.AreEqual(ExpectedCommand, ActualCommand, "Getting variable failed ({0})", ActualCommand)
    End Sub

    ''' <summary>
    ''' Tests removing spaces from the beginning of the string
    ''' </summary>
    <TestMethod> Public Sub TestRemoveSpacesFromBeginning()
        Dim ExpectedString As String = "Hello KS!"
        Dim TargetString As String = "     Hello KS!"
        TargetString = TargetString.RemoveSpacesFromBeginning
        Assert.AreEqual(ExpectedString, TargetString, "Removing space from beginning of string failed. Got ""{0}""", TargetString)
    End Sub

End Class