
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports Microsoft.VisualBasic.FileIO

Module TestGetCommand

    Public TStartCommandThread As New Thread(AddressOf TParseCommand)

    ''' <summary>
    ''' Parses the specified command and executes it
    ''' </summary>
    ''' <param name="FullCmd">A command to be executed. It may come with arguments</param>
    Sub TParseCommand(ByVal FullCmd As String)
        'Variables
        Dim RequiredArgumentsProvided As Boolean = True

        'Get command and arguments
        Dim index As Integer = FullCmd.IndexOf(" ")
        If index = -1 Then index = FullCmd.Length
        Dim words = FullCmd.Split({" "c})
        Dim strArgs As String = FullCmd.Substring(index)
        If Not index = FullCmd.Length Then strArgs = strArgs.Substring(1)
        Dim Cmd As String = words(0)

        'Parse arguments
        Dim FullArgsQ() As String
        Dim TStream As New MemoryStream(Encoding.Default.GetBytes(strArgs))
        Dim Parser As New TextFieldParser(TStream) With {
            .Delimiters = {" "},
            .HasFieldsEnclosedInQuotes = True,
            .TrimWhiteSpace = False
        }
        FullArgsQ = Parser.ReadFields
        If FullArgsQ IsNot Nothing Then
            For i As Integer = 0 To FullArgsQ.Length - 1
                FullArgsQ(i).Replace("""", "")
            Next
            RequiredArgumentsProvided = FullArgsQ?.Length >= Test_Commands(words(0)).MinimumArguments
        ElseIf Test_Commands(words(0)).ArgumentsRequired And FullArgsQ Is Nothing Then
            RequiredArgumentsProvided = False
        End If

        'Command code
        If Cmd = "print" Then 'Usage: print <Color> <Line> <Message>
            If RequiredArgumentsProvided Then
                Dim Color As ColTypes = FullArgsQ(0)
                Dim Line As Boolean = FullArgsQ(1)
                Dim Text As String = FullArgsQ(2)
                W(Text, Line, Color)
            End If
        ElseIf Cmd = "printf" Then 'Usage: printf <Color> <Line> <Variable1;Variable2;Variable3;...> <Message>
            If RequiredArgumentsProvided Then
                Dim Parts As New List(Of String)(FullArgsQ)
                Dim Color As ColTypes = FullArgsQ(0)
                Dim Line As Boolean = FullArgsQ(1)
                Dim Vars As Object() = FullArgsQ(2).Split(";")
                Dim Text As String = FullArgsQ(3)
                For i As Integer = 0 To Vars.Count - 1
                    Vars(i) = Evaluate(Vars(i)).ToString
                Next
                W(Text, Line, Color, Vars)
            End If
        ElseIf Cmd = "printd" Then 'Usage: printd <Message>
            If RequiredArgumentsProvided Then
                Wdbg("I", String.Join(" ", FullArgsQ))
            End If
        ElseIf Cmd = "printdf" Then 'Usage: printdf <Variable1;Variable2;Variable3;...> <Message>
            If RequiredArgumentsProvided Then
                Dim Vars As Object() = FullArgsQ(0).Split(";")
                For i As Integer = 0 To Vars.Count - 1
                    Vars(i) = Evaluate(Vars(i)).ToString
                Next
                Wdbg("I", FullArgsQ(1), Vars)
            End If
        ElseIf Cmd = "testevent" Then 'Usage: testevent <Event>
            If RequiredArgumentsProvided Then
                Try
                    Dim SubName As String = "Raise" + FullArgsQ(0)
                    CallByName(New Events, SubName, CallType.Method)
                Catch ex As Exception
                    W(DoTranslation("Failure to raise event {0}: {1}"), True, ColTypes.Err, FullArgsQ(0))
                End Try
            End If
        ElseIf Cmd = "probehw" Then
            StartProbing()
        ElseIf Cmd = "garbage" Then
            DisposeAll()
        ElseIf Cmd = "panic" Then 'Usage: panic <ErrorType> <Reboot> <RebootTime> <Description>
            If RequiredArgumentsProvided Then
                Dim EType As Char = FullArgsQ(0)
                Dim Reboot As Boolean = FullArgsQ(1)
                Dim RTime As Long = FullArgsQ(2)
                Dim Exc As New Exception
                Dim Message As String = FullArgsQ(3)
                KernelError(EType, Reboot, RTime, Message, Exc)
            End If
        ElseIf Cmd = "panicf" Then 'Usage: panicf <ErrorType> <Reboot> <RebootTime> <Variable1;Variable2;Variable3;...> <Description>
            If RequiredArgumentsProvided Then
                Dim EType As Char = FullArgsQ(0)
                Dim Reboot As Boolean = FullArgsQ(1)
                Dim RTime As Long = FullArgsQ(2)
                Dim Args As String = FullArgsQ(3)
                Dim Exc As New Exception
                Dim Message As String = FullArgsQ(4)
                KernelError(EType, Reboot, RTime, Message, Exc, Args)
            End If
        ElseIf Cmd = "translate" Then 'Usage: translate <Lang> <Message> | Message: A message that is found on KS lang files
            If RequiredArgumentsProvided Then
                Dim Lang As String = FullArgsQ(0)
                Dim Message As String = FullArgsQ(1)
                W(DoTranslation(Message, Lang), True, ColTypes.Neutral)
            End If
        ElseIf Cmd = "places" Then 'Usage: places <Message> | Same as print, but with no option to change colors, etc. Only message with placeholder support
            If RequiredArgumentsProvided Then
                W(ProbePlaces(FullArgsQ(0)), True, ColTypes.Neutral)
            End If
        ElseIf Cmd = "testsha512" Then
            If RequiredArgumentsProvided Then
                Dim spent As New Stopwatch
                spent.Start() 'Time when you're on a breakpoint is counted
                W(GetEncryptedString(FullArgsQ(0), Algorithms.SHA512), True, ColTypes.Neutral)
                W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                spent.Stop()
            End If
        ElseIf Cmd = "testsha256" Then
            If RequiredArgumentsProvided Then
                Dim spent As New Stopwatch
                spent.Start() 'Time when you're on a breakpoint is counted
                W(GetEncryptedString(FullArgsQ(0), Algorithms.SHA256), True, ColTypes.Neutral)
                W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                spent.Stop()
            End If
        ElseIf Cmd = "testsha1" Then
            If RequiredArgumentsProvided Then
                Dim spent As New Stopwatch
                spent.Start() 'Time when you're on a breakpoint is counted
                W(GetEncryptedString(FullArgsQ(0), Algorithms.SHA1), True, ColTypes.Neutral)
                W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                spent.Stop()
            End If
        ElseIf Cmd = "testmd5" Then
            If RequiredArgumentsProvided Then
                Dim spent As New Stopwatch
                spent.Start() 'Time when you're on a breakpoint is counted
                W(GetEncryptedString(FullArgsQ(0), Algorithms.MD5), True, ColTypes.Neutral)
                W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                spent.Stop()
            End If
        ElseIf Cmd = "testregexp" Then 'Usage: testregexp <pattern> <string>
            If RequiredArgumentsProvided Then
                Dim Exp As String = FullArgsQ(0)
                Dim Reg As New Regex(Exp)
                Dim Matches As MatchCollection = Reg.Matches(FullArgsQ(1))
                Dim MatchNum As Integer = 1
                For Each Mat As Match In Matches
                    W(DoTranslation("Match {0} ({1}): {2}"), True, ColTypes.Neutral, MatchNum, Exp, Mat)
                    MatchNum += 1
                Next
            End If
        ElseIf Cmd = "loadmods" Then 'Usage: loadmods <Enable>
            If RequiredArgumentsProvided Then ParseMods(FullArgsQ(0))
        ElseIf Cmd = "debug" Then 'Usage: debug <Enable>
            If RequiredArgumentsProvided Then
                If FullArgsQ(0) = True Then
                    DebugMode = True
                Else
                    RebootRequested = True 'Abort remote debugger
                    DebugMode = False
                    RebootRequested = False
                End If
            End If
        ElseIf Cmd = "rdebug" Then 'Usage: rdebug <Enable>
            If RequiredArgumentsProvided Then
                If FullArgsQ(0) = True Then
                    StartRDebugThread(True)
                Else
                    StartRDebugThread(False)
                End If
            End If
        ElseIf Cmd = "colortest" Then 'Usage: colortest <index>
            If RequiredArgumentsProvided Then
                Dim esc As Char = GetEsc()
                Console.WriteLine(esc + "[38;5;" + FullArgsQ(0) + "mIndex " + FullArgsQ(0))
            End If
        ElseIf Cmd = "colortruetest" Then 'Usage: colortruetest <R;G;B>
            If RequiredArgumentsProvided Then
                Dim esc As Char = GetEsc()
                Console.WriteLine(esc + "[38;2;" + FullArgsQ(0) + "mIndex " + FullArgsQ(0))
            End If
        ElseIf Cmd = "sendnot" Then 'Usage: sendnot <Priority> <title> <desc>
            If RequiredArgumentsProvided Then
                Dim Notif As New Notification With {.Priority = FullArgsQ(0),
                                                    .Title = FullArgsQ(1),
                                                    .Desc = FullArgsQ(2)}
                NotifySend(Notif)
            End If
        ElseIf Cmd = "dcalend" Then 'Usage: dcalend <CalendType>
            If RequiredArgumentsProvided Then
                If FullArgsQ(0) = "Gregorian" Then
                    W(RenderDate(New CultureInfo("en-US")), True, ColTypes.Neutral)
                ElseIf FullArgsQ(0) = "Hijri" Then
                    Dim Cult As New CultureInfo("ar") : Cult.DateTimeFormat.Calendar = New HijriCalendar
                    W(RenderDate(Cult), True, ColTypes.Neutral)
                ElseIf FullArgsQ(0) = "Persian" Then
                    W(RenderDate(New CultureInfo("fa")), True, ColTypes.Neutral)
                ElseIf FullArgsQ(0) = "Saudi-Hijri" Then
                    W(RenderDate(New CultureInfo("ar-SA")), True, ColTypes.Neutral)
                ElseIf FullArgsQ(0) = "Thai-Buddhist" Then
                    W(RenderDate(New CultureInfo("th-TH")), True, ColTypes.Neutral)
                End If
            End If
        ElseIf Cmd = "listcodepages" Then
            Dim Encodings() As EncodingInfo = Encoding.GetEncodings
            For Each Encoding As EncodingInfo In Encodings
                W("{0}: {1} ({2})", True, ColTypes.Neutral, Encoding.CodePage, Encoding.Name, Encoding.DisplayName)
            Next
        ElseIf Cmd = "lscompilervars" Then
#If NTFSCorruptionFix Then
            W("- NTFSCorruptionFix", True, ColTypes.Neutral)
#End If
#If NOWRITELOCK Then
            W("- NOWRITELOCK", True, ColTypes.Neutral)
#End If
#If SPECIFIER = "DEV" Then
            W("- SPECIFIER = ""DEV""", True, ColTypes.Neutral)
#ElseIf SPECIFIER = "RC" Then
            W("- SPECIFIER = ""RC""", True, ColTypes.Neutral)
#ElseIf SPECIFIER = "NEARING" Then
            W("- SPECIFIER = ""NEARING""", True, ColTypes.Neutral)
#ElseIf SPECIFIER = "REL" Then
            W("- SPECIFIER = ""REL""", True, ColTypes.Neutral)
#End If
#If ENABLEIMMEDIATEWINDOWDEBUG Then
            W("- ENABLEIMMEDIATEWINDOWDEBUG", True, ColTypes.Neutral)
#End If
        ElseIf Cmd = "help" Then
            If FullArgsQ?.Length = 0 Or FullArgsQ Is Nothing Then
                TestShowHelp()
            Else
                TestShowHelp(FullArgsQ(0))
            End If
        ElseIf Cmd = "exit" Then
            TEST_ExitFlag = True
        ElseIf Cmd = "shutdown" Then
            TEST_ShutdownFlag = True
            TEST_ExitFlag = True
        End If

        'If not enough arguments, show help entry
        If Test_Commands(Cmd).ArgumentsRequired And Not RequiredArgumentsProvided Then
            Wdbg("W", "User hasn't provided enough arguments for {0}", Cmd)
            W(DoTranslation("There was not enough arguments. See below for usage:"), True, ColTypes.Neutral)
            TestShowHelp(Cmd)
        End If
    End Sub

    Sub TCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            Console.WriteLine()
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            TStartCommandThread.Abort()
        End If
    End Sub

End Module
