
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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
Imports System.Security.Cryptography
Imports System.Text

Module TGetCommand

    Sub TParseCommand(ByVal FullCmd As String)
        Dim FullArgs As String
        Dim FullArgsL As List(Of String)
        Dim Cmd As String
        FullArgsL = FullCmd.Split(" ").ToList
        FullArgsL.RemoveAt(0)
        FullArgs = String.Join(" ", FullArgsL)
        Cmd = FullCmd.Split(" ").ToList(0)
        If Cmd = "print" Then 'Usage: print <Color> <Line> <Message>
            If FullArgsL.Count - 1 >= 2 Then
                Dim Parts As New List(Of String)(FullArgsL)
                Dim Color As ColTypes = Parts(0)
                Dim Line As Boolean = Parts(1)
                Dim Text As String
                Parts.RemoveAt(0) : Parts.RemoveAt(0)
                Text = String.Join(" ", Parts)
                W(Text, Line, Color)
            End If
        ElseIf Cmd = "printf" Then 'Usage: printf <Color> <Line> <Variable1;Variable2;Variable3;...> <Message>
            If FullArgsL.Count - 1 >= 3 Then
                Dim Parts As New List(Of String)(FullArgsL)
                Dim Color As ColTypes = Parts(0)
                Dim Line As Boolean = Parts(1)
                Dim Vars As Object() = Parts(2).Split(";")
                Dim Text As String
                Parts.RemoveAt(0) : Parts.RemoveAt(0) : Parts.RemoveAt(0)
                Text = String.Join(" ", Parts)
                W(Text, Line, Color, Vars)
            End If
        ElseIf Cmd = "printd" Then 'Usage: printd <Message>
            If FullArgsL.Count - 1 >= 0 Then
                Wdbg(String.Join(" ", FullArgsL))
            End If
        ElseIf Cmd = "printdf" Then 'Usage: printdf <Variable1;Variable2;Variable3;...> <Message>
            If FullArgsL.Count - 1 >= 1 Then
                Dim Vars As Object() = FullArgsL(0).Split(";")
                FullArgsL.RemoveAt(0)
                Wdbg(String.Join(" ", FullArgsL), Vars)
            End If
        ElseIf Cmd = "testevent" Then 'Usage: testevent <Event>
            If FullArgsL.Count - 1 = 0 Then
                Try
                    Dim SubName As String = "Raise" + FullArgsL(0)
                    CallByName(New EventsAndExceptions, SubName, CallType.Method)
                Catch ex As Exception
                    W(DoTranslation("Failure to raise event {0}: {1}", currentLang), True, ColTypes.Neutral, FullArgsL(0))
                End Try
            End If
        ElseIf Cmd = "probehw" Then
            HDDList.Clear()
            CPUList.Clear()
            RAMList.Clear()
            ProbeHW()
        ElseIf Cmd = "garbage" Then
            DisposeAll()
        ElseIf Cmd = "panic" Then 'Usage: panic <ErrorType> <Reboot> <RebootTime> <Description>
            Dim EType As Char = FullArgsL(0)
            Dim Reboot As Boolean = FullArgsL(1)
            Dim RTime As Long = FullArgsL(2)
            Dim Exc As New Exception
            FullArgsL.RemoveRange(0, 3)
            Dim Message As String = String.Join(" ", FullArgsL)
            KernelError(EType, Reboot, RTime, Message, Exc)
        ElseIf Cmd = "panicf" Then 'Usage: panicf <ErrorType> <Reboot> <RebootTime> <Variable1;Variable2;Variable3;...> <Description>
            Dim EType As Char = FullArgsL(0)
            Dim Reboot As Boolean = FullArgsL(1)
            Dim RTime As Long = FullArgsL(2)
            Dim Args As String = FullArgsL(3)
            Dim Exc As New Exception
            FullArgsL.RemoveRange(0, 3)
            Dim Message As String = String.Join(" ", FullArgsL)
            KernelError(EType, Reboot, RTime, Message, Exc, Args)
        ElseIf Cmd = "translate" Then 'Usage: translate <Lang> <Message> | Message: A message that is found on KS lang files
            Dim Lang As String = FullArgsL(0)
            FullArgsL.RemoveAt(0)
            Dim Message As String = String.Join(" ", FullArgsL)
            W(DoTranslation(Message, Lang), True, ColTypes.Neutral)
        ElseIf Cmd = "places" Then 'Usage: places <Message> | Same as print, but with no option to change colors, etc. Only message with placeholder support
            W(ProbePlaces(FullArgs), True, ColTypes.Neutral)
        ElseIf Cmd = "testsha256" Then
            Dim spent As New Stopwatch
            spent.Start() 'Time when you're on a breakpoint is counted
            Dim hashbyte As Byte() = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(FullArgs))
            W(GetArraySHA256(hashbyte), True, ColTypes.Neutral)
            W(DoTranslation("Time spent: {0} milliseconds", currentLang), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
            spent.Stop()
        ElseIf Cmd = "testmd5" Then
            Dim spent As New Stopwatch
            spent.Start() 'Time when you're on a breakpoint is counted
            Dim hashbyte As Byte() = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(FullArgs))
            W(GetArrayMD5(hashbyte), True, ColTypes.Neutral)
            W(DoTranslation("Time spent: {0} milliseconds", currentLang), True, ColTypes.Neutral, spent.ElapsedMilliseconds)
            spent.Stop()
        ElseIf Cmd = "loadmods" Then 'Usage: loadmods <Enable>
            If FullArgsL.Count - 1 = 0 Then ParseMods(FullArgsL(0))
        ElseIf Cmd = "debug" Then 'Usage: debug <Enable>
            If FullArgsL.Count - 1 = 0 Then
                If FullArgsL(0) = True Then
                    DebugMode = True
                Else
                    RebootRequested = True 'Abort remote debugger
                    DebugMode = False
                    RebootRequested = False
                End If
            End If
        ElseIf Cmd = "rdebug" Then 'Usage: rdebug <Enable>
            If FullArgsL.Count - 1 = 0 Then
                If FullArgsL(0) = True Then
                    StartRDebugThread()
                Else
                    RebootRequested = True 'Abort remote debugger
                    RebootRequested = False
                End If
            End If
        ElseIf Cmd = "help" Then
            W("- print <Color> <Line> <Message>" + vbNewLine +
              "- printf <Color> <Line> <Variable1;Variable2;Variable3;...> <Message>" + vbNewLine +
              "- printd <Message>" + vbNewLine +
              "- printdf <Variable1;Variable2;Variable3;...> <Message>" + vbNewLine +
              "- panic <ErrorType> <Reboot> <RebootTime> <Description>" + vbNewLine +
              "- panicf <ErrorType> <Reboot> <RebootTime> <Variable1;Variable2;Variable3;...> <Description>" + vbNewLine +
              "- translate <Lang> <Message>" + vbNewLine +
              "- testsha256 <Message>" + vbNewLine +
              "- testmd5 <Message>" + vbNewLine +
              "- debug <Enable>" + vbNewLine +
              "- rdebug <Enable>" + vbNewLine +
              "- testevent <Event>" + vbNewLine +
              "- probehw" + vbNewLine +
              "- garbage" + vbNewLine +
              "- exit", True, ColTypes.Neutral)
        ElseIf Cmd = "exit" Then
            Environment.Exit(0)
        End If
    End Sub

End Module
