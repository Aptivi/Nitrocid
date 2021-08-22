
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

Module TestGetCommand

    Public TStartCommandThread As New Thread(AddressOf TParseCommand) With {.Name = "Test Shell Command Thread"}

    ''' <summary>
    ''' Parses the specified command and executes it
    ''' </summary>
    ''' <param name="requestedCommand">A command to be executed. It may come with arguments</param>
    Sub TParseCommand(ByVal requestedCommand As String)
        Try
            'Variables
            Dim Command As String
            Dim RequiredArgumentsProvided As Boolean = True

            '1. Get the index of the first space (Used for step 3)
            Dim index As Integer = requestedCommand.IndexOf(" ")
            If index = -1 Then index = requestedCommand.Length
            Wdbg("I", "Index: {0}", index)

            '2. Split the requested command string into words
            Dim words() As String = requestedCommand.Split({" "c})
            For i As Integer = 0 To words.Length - 1
                Wdbg("I", "Word {0}: {1}", i + 1, words(i))
            Next
            Command = words(0)

            '3. Get the string of arguments
            Dim strArgs As String = requestedCommand.Substring(index)
            Wdbg("I", "Prototype strArgs: {0}", strArgs)
            If Not index = requestedCommand.Length Then strArgs = strArgs.Substring(1)
            Wdbg("I", "Finished strArgs: {0}", strArgs)

            '4. Split the arguments with enclosed quotes and set the required boolean variable
            Dim eqargs() As String = strArgs.SplitEncloseDoubleQuotes(" ")
            If eqargs IsNot Nothing Then
                RequiredArgumentsProvided = eqargs?.Length >= Test_Commands(Command).MinimumArguments
            ElseIf Test_Commands(Command).ArgumentsRequired And eqargs Is Nothing Then
                RequiredArgumentsProvided = False
            End If

            '4a. Debug: get all arguments from eqargs()
            If eqargs IsNot Nothing Then Wdbg("I", "Arguments parsed from eqargs(): " + String.Join(", ", eqargs))

            '5. Check to see if a requested command is obsolete
            If Test_Commands(Command).Obsolete Then
                Wdbg("I", "The command requested {0} is obsolete", Command)
                W(DoTranslation("This command is obsolete and will be removed in a future release."), True, ColTypes.Neutral)
            End If

            '6. Execute a command
            Select Case Command
                Case "print"
                    If RequiredArgumentsProvided Then
                        Dim Color As ColTypes = eqargs(0)
                        Dim Line As Boolean = eqargs(1)
                        Dim Text As String = eqargs(2)
                        W(Text, Line, Color)
                    End If
                Case "printf"
                    If RequiredArgumentsProvided Then
                        Dim Parts As New List(Of String)(eqargs)
                        Dim Color As ColTypes = eqargs(0)
                        Dim Line As Boolean = eqargs(1)
                        Dim Vars As Object() = eqargs(2).Split(";")
                        Dim Text As String = eqargs(3)
                        For i As Integer = 0 To Vars.Length - 1
                            Vars(i) = Evaluate(Vars(i)).ToString
                        Next
                        W(Text, Line, Color, Vars)
                    End If
                Case "printd"
                    If RequiredArgumentsProvided Then
                        Wdbg("I", String.Join(" ", eqargs))
                    End If
                Case "printdf"
                    If RequiredArgumentsProvided Then
                        Dim Vars As Object() = eqargs(0).Split(";")
                        For i As Integer = 0 To Vars.Length - 1
                            Vars(i) = Evaluate(Vars(i)).ToString
                        Next
                        Wdbg("I", eqargs(1), Vars)
                    End If
                Case "printsep"
                    If RequiredArgumentsProvided Then
                        WriteSeparator(eqargs(0), True, ColTypes.Neutral)
                    End If
                Case "printsepf"
                    If RequiredArgumentsProvided Then
                        Dim Vars As Object() = eqargs(0).Split(";")
                        For i As Integer = 0 To Vars.Length - 1
                            Vars(i) = Evaluate(Vars(i)).ToString
                        Next
                        WriteSeparator(eqargs(1), True, ColTypes.Neutral, Vars)
                    End If
                Case "printsepcolor"
                    If RequiredArgumentsProvided Then
                        WriteSeparatorC(eqargs(1), True, New Color(eqargs(0)))
                    End If
                Case "printsepcolorf"
                    If RequiredArgumentsProvided Then
                        Dim Vars As Object() = eqargs(1).Split(";")
                        For i As Integer = 0 To Vars.Length - 1
                            Vars(i) = Evaluate(Vars(i)).ToString
                        Next
                        WriteSeparatorC(eqargs(2), True, New Color(eqargs(0)), Vars)
                    End If
                Case "testevent"
                    If RequiredArgumentsProvided Then
                        Try
                            Dim SubName As String = "Raise" + eqargs(0)
                            CallByName(New Events, SubName, CallType.Method)
                        Catch ex As Exception
                            W(DoTranslation("Failure to raise event {0}: {1}"), True, ColTypes.Error, eqargs(0))
                        End Try
                    End If
                Case "probehw"
                    StartProbing()
                Case "garbage"
                    DisposeAll()
                Case "panic"
                    If RequiredArgumentsProvided Then
                        Dim EType As Char = eqargs(0)
                        Dim Reboot As Boolean = eqargs(1)
                        Dim RTime As Long = eqargs(2)
                        Dim Exc As New Exception
                        Dim Message As String = eqargs(3)
                        KernelError(EType, Reboot, RTime, Message, Exc)
                    End If
                Case "panicf"
                    If RequiredArgumentsProvided Then
                        Dim EType As Char = eqargs(0)
                        Dim Reboot As Boolean = eqargs(1)
                        Dim RTime As Long = eqargs(2)
                        Dim Args As String = eqargs(3)
                        Dim Exc As New Exception
                        Dim Message As String = eqargs(4)
                        KernelError(EType, Reboot, RTime, Message, Exc, Args)
                    End If
                Case "translate"
                    If RequiredArgumentsProvided Then
                        Dim Lang As String = eqargs(0)
                        Dim Message As String = eqargs(1)
                        W(DoTranslation(Message, Lang), True, ColTypes.Neutral)
                    End If
                Case "places"
                    If RequiredArgumentsProvided Then
                        W(ProbePlaces(eqargs(0)), True, ColTypes.Neutral)
                    End If
                Case "testsha512"
                    If RequiredArgumentsProvided Then
                        Dim spent As New Stopwatch
                        spent.Start() 'Time when you're on a breakpoint is counted
                        W(GetEncryptedString(eqargs(0), Algorithms.SHA512), True, ColTypes.Neutral)
                        W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                        spent.Stop()
                    End If
                Case "testsha256"
                    If RequiredArgumentsProvided Then
                        Dim spent As New Stopwatch
                        spent.Start() 'Time when you're on a breakpoint is counted
                        W(GetEncryptedString(eqargs(0), Algorithms.SHA256), True, ColTypes.Neutral)
                        W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                        spent.Stop()
                    End If
                Case "testsha1"
                    If RequiredArgumentsProvided Then
                        Dim spent As New Stopwatch
                        spent.Start() 'Time when you're on a breakpoint is counted
                        W(GetEncryptedString(eqargs(0), Algorithms.SHA1), True, ColTypes.Neutral)
                        W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                        spent.Stop()
                    End If
                Case "testmd5"
                    If RequiredArgumentsProvided Then
                        Dim spent As New Stopwatch
                        spent.Start() 'Time when you're on a breakpoint is counted
                        W(GetEncryptedString(eqargs(0), Algorithms.MD5), True, ColTypes.Neutral)
                        W(DoTranslation("Time spent: {0} milliseconds"), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
                        spent.Stop()
                    End If
                Case "testregexp"
                    If RequiredArgumentsProvided Then
                        Dim Exp As String = eqargs(0)
                        Dim Reg As New Regex(Exp)
                        Dim Matches As MatchCollection = Reg.Matches(eqargs(1))
                        Dim MatchNum As Integer = 1
                        For Each Mat As Match In Matches
                            W(DoTranslation("Match {0} ({1}): {2}"), True, ColTypes.Neutral, MatchNum, Exp, Mat)
                            MatchNum += 1
                        Next
                    End If
                Case "loadmods"
                    StartMods()
                Case "stopmods"
                    StopMods()
                Case "debug"
                    If RequiredArgumentsProvided Then
                        If eqargs(0) = True Then
                            DebugMode = True
                        Else
                            RebootRequested = True 'Abort remote debugger
                            DebugMode = False
                            RebootRequested = False
                        End If
                    End If
                Case "rdebug"
                    If RequiredArgumentsProvided Then
                        If eqargs(0) = True Then
                            StartRDebugThread()
                        Else
                            StopRDebugThread()
                        End If
                    End If
                Case "colortest"
                    If RequiredArgumentsProvided Then
                        Dim esc As Char = GetEsc()
                        Console.WriteLine(esc + "[38;5;" + eqargs(0) + "mIndex " + eqargs(0))
                    End If
                Case "colortruetest"
                    If RequiredArgumentsProvided Then
                        Dim esc As Char = GetEsc()
                        Console.WriteLine(esc + "[38;2;" + eqargs(0) + "mIndex " + eqargs(0))
                    End If
                Case "sendnot"
                    If RequiredArgumentsProvided Then
                        Dim Notif As New Notification With {.Priority = eqargs(0),
                                                            .Title = eqargs(1),
                                                            .Desc = eqargs(2)}
                        NotifySend(Notif)
                    End If
                Case "dcalend"
                    If RequiredArgumentsProvided Then
                        If eqargs(0) = "Gregorian" Then
                            W(RenderDate(New CultureInfo("en-US")), True, ColTypes.Neutral)
                        ElseIf eqargs(0) = "Hijri" Then
                            Dim Cult As New CultureInfo("ar") : Cult.DateTimeFormat.Calendar = New HijriCalendar
                            W(RenderDate(Cult), True, ColTypes.Neutral)
                        ElseIf eqargs(0) = "Persian" Then
                            W(RenderDate(New CultureInfo("fa")), True, ColTypes.Neutral)
                        ElseIf eqargs(0) = "Saudi-Hijri" Then
                            W(RenderDate(New CultureInfo("ar-SA")), True, ColTypes.Neutral)
                        ElseIf eqargs(0) = "Thai-Buddhist" Then
                            W(RenderDate(New CultureInfo("th-TH")), True, ColTypes.Neutral)
                        End If
                    End If
                Case "listcodepages"
                    Dim Encodings() As EncodingInfo = Encoding.GetEncodings
                    For Each Encoding As EncodingInfo In Encodings
                        W("{0}: {1} ({2})", True, ColTypes.Neutral, Encoding.CodePage, Encoding.Name, Encoding.DisplayName)
                    Next
                Case "lscompilervars"
                    For Each CompilerVar As String In GetCompilerVars()
                        W("- {0}", True, ColTypes.ListEntry, CompilerVar)
                    Next
                Case "testdictwriterstr"
                    Dim NormalStringDict As New Dictionary(Of String, String) From {{"One", "String 1"}, {"Two", "String 2"}, {"Three", "String 3"}}
                    Dim ArrayStringDict As New Dictionary(Of String, String()) From {{"One", {"String 1", "String 2", "String 3"}}, {"Two", {"String 1", "String 2", "String 3"}}, {"Three", {"String 1", "String 2", "String 3"}}}
                    W(DoTranslation("Normal string dictionary:"), True, ColTypes.Neutral)
                    WriteList(NormalStringDict)
                    W(DoTranslation("Array string dictionary:"), True, ColTypes.Neutral)
                    WriteList(ArrayStringDict)
                Case "testdictwriterint"
                    Dim NormalIntegerDict As New Dictionary(Of String, Integer) From {{"One", 1}, {"Two", 2}, {"Three", 3}}
                    Dim ArrayIntegerDict As New Dictionary(Of String, Integer()) From {{"One", {1, 2, 3}}, {"Two", {1, 2, 3}}, {"Three", {1, 2, 3}}}
                    W(DoTranslation("Normal integer dictionary:"), True, ColTypes.Neutral)
                    WriteList(NormalIntegerDict)
                    W(DoTranslation("Array integer dictionary:"), True, ColTypes.Neutral)
                    WriteList(ArrayIntegerDict)
                Case "testdictwriterchar"
                    Dim NormalCharDict As New Dictionary(Of String, Char) From {{"One", "1"c}, {"Two", "2"c}, {"Three", "3"c}}
                    Dim ArrayCharDict As New Dictionary(Of String, Char()) From {{"One", {"1"c, "2"c, "3"c}}, {"Two", {"1"c, "2"c, "3"c}}, {"Three", {"1"c, "2"c, "3"c}}}
                    W(DoTranslation("Normal char dictionary:"), True, ColTypes.Neutral)
                    WriteList(NormalCharDict)
                    W(DoTranslation("Array char dictionary:"), True, ColTypes.Neutral)
                    WriteList(ArrayCharDict)
                Case "testlistwriterstr"
                    Dim NormalStringList As New List(Of String) From {"String 1", "String 2", "String 3"}
                    Dim ArrayStringList As New List(Of String()) From {{{"String 1", "String 2", "String 3"}}, {{"String 1", "String 2", "String 3"}}, {{"String 1", "String 2", "String 3"}}}
                    W(DoTranslation("Normal string list:"), True, ColTypes.Neutral)
                    WriteList(NormalStringList)
                    W(DoTranslation("Array string list:"), True, ColTypes.Neutral)
                    WriteList(ArrayStringList)
                Case "testlistwriterint"
                    Dim NormalIntegerList As New List(Of Integer) From {1, 2, 3}
                    Dim ArrayIntegerList As New List(Of Integer()) From {{{1, 2, 3}}, {{1, 2, 3}}, {{1, 2, 3}}}
                    W(DoTranslation("Normal integer list:"), True, ColTypes.Neutral)
                    WriteList(NormalIntegerList)
                    W(DoTranslation("Array integer list:"), True, ColTypes.Neutral)
                    WriteList(ArrayIntegerList)
                Case "testlistwriterchar"
                    Dim NormalCharList As New List(Of Char) From {"1"c, "2"c, "3"c}
                    Dim ArrayCharList As New List(Of Char()) From {{{"1"c, "2"c, "3"c}}, {{"1"c, "2"c, "3"c}}, {{"1"c, "2"c, "3"c}}}
                    W(DoTranslation("Normal char list:"), True, ColTypes.Neutral)
                    WriteList(NormalCharList)
                    W(DoTranslation("Array char list:"), True, ColTypes.Neutral)
                    WriteList(ArrayCharList)
                Case "lscultures"
                    Dim Cults As CultureInfo() = CultureInfo.GetCultures(CultureTypes.AllCultures)
                    For Each Cult As CultureInfo In Cults
                        If eqargs?.Length > 0 Or eqargs IsNot Nothing Then
                            If Cult.Name.ToLower.Contains(eqargs(0).ToLower) Or Cult.EnglishName.ToLower.Contains(eqargs(0).ToLower) Then
                                W("{0}: {1}", True, ColTypes.Neutral, Cult.Name, Cult.EnglishName)
                            End If
                        Else
                            W("{0}: {1}", True, ColTypes.Neutral, Cult.Name, Cult.EnglishName)
                        End If
                    Next
                Case "getcustomsaversetting"
                    If RequiredArgumentsProvided Then
                        If CSvrdb.ContainsKey(eqargs(0)) Then
                            W("- {0} -> {1}: ", False, ColTypes.ListEntry, eqargs(0), eqargs(1))
                            W(GetCustomSaverSettings(eqargs(0), eqargs(1)), True, ColTypes.ListValue)
                        Else
                            W(DoTranslation("Screensaver {0} not found."), True, ColTypes.Error, eqargs(0))
                        End If
                    End If
                Case "setcustomsaversetting"
                    If RequiredArgumentsProvided Then
                        If CSvrdb.ContainsKey(eqargs(0)) Then
                            If SetCustomSaverSettings(eqargs(0), eqargs(1), eqargs(2)) Then
                                W(DoTranslation("Settings set successfully for screensaver") + " {0}.", True, ColTypes.Neutral, eqargs(0))
                            Else
                                W(DoTranslation("Failed to set a setting for screensaver") + " {0}.", True, ColTypes.Error, eqargs(0))
                            End If
                        Else
                            W(DoTranslation("Screensaver {0} not found."), True, ColTypes.Error, eqargs(0))
                        End If
                    End If
                Case "help"
                    If eqargs?.Length = 0 Or eqargs Is Nothing Then
                        TestShowHelp()
                    Else
                        TestShowHelp(eqargs(0))
                    End If
                Case "exit"
                    Test_ExitFlag = True
                Case "shutdown"
                    Test_ShutdownFlag = True
                    Test_ExitFlag = True
            End Select

            'If not enough arguments, show help entry
            If Test_Commands(Command).ArgumentsRequired And Not RequiredArgumentsProvided Then
                Wdbg("W", "User hasn't provided enough arguments for {0}", Command)
                W(DoTranslation("There was not enough arguments. See below for usage:"), True, ColTypes.Neutral)
                TestShowHelp(Command)
            End If
        Catch taex As ThreadAbortException
            Exit Sub
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("Error trying to execute command") + " {3}." + vbNewLine + DoTranslation("Error {0}: {1}") + vbNewLine + "{2}", True, ColTypes.Error,
                  ex.GetType.FullName, ex.Message, ex.StackTrace, requestedCommand)
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to execute command") + " {2}." + vbNewLine + DoTranslation("Error {0}: {1}"), True, ColTypes.Error, ex.GetType.FullName, ex.Message, requestedCommand)
            End If
        End Try
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
