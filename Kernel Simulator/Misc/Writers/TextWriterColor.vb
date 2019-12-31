
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

Imports System.Console
Imports System.IO
Imports System.Threading

'This module is very important to reduce line numbers when there is color.
Public Module TextWriterColor

    Public dbgWriter As StreamWriter
    Public DebugQuota As Double = 1073741824 '1073741824 bytes = 1 GiB (1 GB for Windows)
    Public RDebugDNP As String = "KSUser" 'Appended with random ID when new session arrives
    Public dbgStackTraces As New List(Of String)

    Public Enum ColTypes As Integer
        Neutral = 1
        Input = 2
        Continuable = 3
        Uncontinuable = 4
        HostName = 5
        UserName = 6
        License = 7
        Gray = 8
        HelpDef = 9
        HelpCmd = 10
        Stage = 11
    End Enum

    ''' <summary>
    ''' Outputs the text into the debugger file, and sets the time stamp.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the the debugger file. Supports {0}, {1}, ...</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    ''' <remarks></remarks>
    Public Sub Wdbg(ByVal text As String, ByVal ParamArray vars() As Object)
        If DebugMode Then
            Dim STrace As New StackTrace(True)
            Dim Source As String = Path.GetFileName(STrace.GetFrame(1).GetFileName)
            Dim LineNum As String = STrace.GetFrame(1).GetFileLineNumber
            Dim Func As String = STrace.GetFrame(1).GetMethod.Name
            Dim OffendingIndex As New List(Of String)

            'Check for debug quota
            CheckForExceed()

            'For contributors who are testing new code: Uncomment the two Debug.WriteLine lines for immediate debugging (Immediate Window)
            If Not Source Is Nothing And Not LineNum = 0 Then
                'Debug to file and all connected debug devices (raw mode)
                dbgWriter.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString} ({Func} - {Source}:{LineNum}): {text}", vars)
                For i As Integer = 0 To dbgConns.Count - 1
                    Try
                        dbgConns.Keys(i).WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString} ({Func} - {Source}:{LineNum}): {text}", vars)
                    Catch ex As Exception
                        OffendingIndex.Add(GetSWIndex(dbgConns.Keys(i)))
                        WStkTrc(ex)
                    End Try
                Next
                'Debug.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString} ({Func} - {Source}:{LineNum}): {text}", vars)
            Else 'Rare case, unless debug symbol is not found on archives.
                dbgWriter.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString}: {text}", vars)
                For i As Integer = 0 To dbgConns.Count - 1
                    Try
                        dbgConns.Keys(i).WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString}: {text}", vars)
                    Catch ex As Exception
                        OffendingIndex.Add(GetSWIndex(dbgConns.Keys(i)))
                        WStkTrc(ex)
                    End Try
                Next
                'Debug.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString}: {text}", vars)
            End If

            'Disconnect offending clients who are disconnected
            For Each i As Integer In OffendingIndex
                If i <> -1 Then
                    DebugDevices.Keys(i).Disconnect(True)
                    Wdbg("Debug device {0} ({1}) disconnected.", dbgConns.Values(i), DebugDevices.Values(i))
                    dbgConns.Remove(dbgConns.Keys(i))
                    DebugDevices.Remove(DebugDevices.Keys(i))
                End If
            Next
            OffendingIndex.Clear()
        End If
    End Sub

    Public Sub WStkTrc(ByVal Ex As Exception)
        If DebugMode Then
            Dim OffendingIndex As New List(Of String)

            'Check for quota
            CheckForExceed()

            'These two vbNewLines are padding for accurate stack tracing.
            dbgStackTraces.Add($"{vbNewLine}{Ex.ToString.Substring(0, Ex.ToString.IndexOf(":"))}: {Ex.Message}{vbNewLine}{Ex.StackTrace}{vbNewLine}")
            dbgWriter.WriteLine(dbgStackTraces(0))
            For i As Integer = 0 To dbgConns.Count - 1
                Try
                    dbgConns.Keys(i).WriteLine(dbgStackTraces(0))
                Catch exc As Exception
                    OffendingIndex.Add(GetSWIndex(dbgConns.Keys(i)))
                End Try
            Next

            'Disconnect offending clients who are disconnected
            For Each i As Integer In OffendingIndex
                If i <> -1 Then
                    DebugDevices.Keys(i).Disconnect(True)
                    Wdbg("Debug device {0} ({1}) disconnected.", dbgConns.Values(i), DebugDevices.Values(i))
                    dbgConns.Remove(dbgConns.Keys(i))
                    DebugDevices.Remove(DebugDevices.Keys(i))
                End If
            Next
            OffendingIndex.Clear()
        End If
    End Sub

    Public Sub CheckForExceed()
        Try
            Dim FInfo As New FileInfo(paths("Debugging"))
            Dim OldSize As Double = FInfo.Length
            Dim Lines() As String = ReadAllLinesNoBlock(paths("Debugging"))
            If OldSize > DebugQuota Then
                dbgWriter.Close()
                dbgWriter = New StreamWriter(paths("Debugging")) With {.AutoFlush = True}
                For l As Integer = 5 To Lines.Length - 2 'Remove the first 5 lines from stream.
                    dbgWriter.WriteLine(Lines(l))
                Next
                Wdbg("Max debug quota size exceeded, was {0} MB.", FormatNumber(OldSize / 1024 / 1024, 1))
            End If
        Catch ex As Exception
            WStkTrc(ex)
            Exit Sub
        End Try
    End Sub

    Private Function ReadAllLinesNoBlock(ByVal path As String) As String()
        Dim AllLnList As New List(Of String)
        Dim FOpen As New StreamReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        While Not FOpen.EndOfStream
            AllLnList.Add(FOpen.ReadLine)
        End While
        Return AllLnList.ToArray
    End Function

    ''' <summary>
    ''' Outputs the text into the terminal prompt, and sets colors as needed.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="colorType">A type of colors that will be changed.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    ''' <remarks>This is used to reduce number of lines containing "System.Console.ForegroundColor = " and "System.Console.ResetColor()" text.</remarks>
    Public Sub W(ByVal text As Object, ByVal Line As Boolean, ByVal colorType As ColTypes, ByVal ParamArray vars() As Object)

        Dim esc As Char = GetEsc()
        Try
            If colorType = ColTypes.Neutral Or colorType = ColTypes.Input Then
                Write(esc + "[38;5;" + CStr(neutralTextColor) + "m")
            ElseIf colorType = ColTypes.Continuable Then
                Write(esc + "[38;5;" + CStr(contKernelErrorColor) + "m")
            ElseIf colorType = ColTypes.Uncontinuable Then
                Write(esc + "[38;5;" + CStr(uncontKernelErrorColor) + "m")
            ElseIf colorType = ColTypes.HostName Then
                Write(esc + "[38;5;" + CStr(hostNameShellColor) + "m")
            ElseIf colorType = ColTypes.UserName Then
                Write(esc + "[38;5;" + CStr(userNameShellColor) + "m")
            ElseIf colorType = ColTypes.License Then
                Write(esc + "[38;5;" + CStr(licenseColor) + "m")
            ElseIf colorType = ColTypes.Gray Then
                If backgroundColor = ConsoleColors.DarkYellow Or backgroundColor = ConsoleColors.Yellow Or backgroundColor = ConsoleColors.White Then
                    Write(esc + "[38;5;" + CStr(neutralTextColor) + "m")
                Else
                    Write(esc + "[38;5;" + CStr(ConsoleColors.Gray) + "m")
                End If
            ElseIf colorType = ColTypes.HelpDef Then
                Write(esc + "[38;5;" + CStr(cmdDefColor) + "m")
            ElseIf colorType = ColTypes.HelpCmd Then
                Write(esc + "[38;5;" + CStr(cmdListColor) + "m")
            ElseIf colorType = ColTypes.Stage Then
                Write(esc + "[38;5;" + CStr(stageColor) + "m")
            Else
                Exit Sub
            End If

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            For v As Integer = 0 To vars.Length - 1
                text = text.Replace("{" + CStr(v) + "}", vars(v).ToString)
            Next

            If Line Then WriteLine(text) Else Write(text)
            If backgroundColor = ConsoleColors.Black Then ResetColor()
            If colorType = ColTypes.Input And ColoredShell = True Then Write(esc + "[38;5;" + CStr(inputColor) + "m")
        Catch ex As Exception
            WStkTrc(ex)
            KernelError("C", False, 0, DoTranslation("There is a serious error when printing text.", currentLang), ex)
        End Try

    End Sub

    Public Sub WriteSlowly(ByVal msg As String, ByVal Line As Boolean, ByVal MsEachLetter As Double, ParamArray ByVal vars() As Object)
        'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
        For v As Integer = 0 To vars.Length - 1
            msg = msg.Replace("{" + CStr(v) + "}", vars(v).ToString)
        Next

        'Write text slowly
        Dim chars As List(Of Char) = msg.ToCharArray.ToList
        For Each ch As Char In chars
            Thread.Sleep(MsEachLetter)
            Write(ch)
        Next
        If Line Then
            WriteLine()
        End If
    End Sub

    Public Sub WriteSlowlyC(ByVal msg As String, ByVal Line As Boolean, ByVal MsEachLetter As Double, ByVal colorType As ColTypes, ParamArray ByVal vars() As Object)
        Dim esc As Char = GetEsc()
        If colorType = ColTypes.Neutral Or colorType = ColTypes.Input Then
            Write(esc + "[38;5;" + CStr(neutralTextColor) + "m")
        ElseIf colorType = ColTypes.Continuable Then
            Write(esc + "[38;5;" + CStr(contKernelErrorColor) + "m")
        ElseIf colorType = ColTypes.Uncontinuable Then
            Write(esc + "[38;5;" + CStr(uncontKernelErrorColor) + "m")
        ElseIf colorType = ColTypes.HostName Then
            Write(esc + "[38;5;" + CStr(hostNameShellColor) + "m")
        ElseIf colorType = ColTypes.UserName Then
            Write(esc + "[38;5;" + CStr(userNameShellColor) + "m")
        ElseIf colorType = ColTypes.License Then
            Write(esc + "[38;5;" + CStr(licenseColor) + "m")
        ElseIf colorType = ColTypes.Gray Then
            If backgroundColor = ConsoleColors.DarkYellow Or backgroundColor = ConsoleColors.Yellow Or backgroundColor = ConsoleColors.White Then
                Write(esc + "[38;5;" + CStr(neutralTextColor) + "m")
            Else
                Write(esc + "[38;5;" + CStr(ConsoleColors.Gray) + "m")
            End If
        ElseIf colorType = ColTypes.HelpDef Then
            Write(esc + "[38;5;" + CStr(cmdDefColor) + "m")
        ElseIf colorType = ColTypes.HelpCmd Then
            Write(esc + "[38;5;" + CStr(cmdListColor) + "m")
        ElseIf colorType = ColTypes.Stage Then
            Write(esc + "[38;5;" + CStr(stageColor) + "m")
        Else
            Exit Sub
        End If

        'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
        For v As Integer = 0 To vars.Length - 1
            msg = msg.Replace("{" + CStr(v) + "}", vars(v).ToString)
        Next

        'Write text slowly
        Dim chars As List(Of Char) = msg.ToCharArray.ToList
        For Each ch As Char In chars
            Thread.Sleep(MsEachLetter)
            Write(ch)
        Next
        If Line Then
            WriteLine()
        End If
        If backgroundColor = ConsoleColors.Black Then ResetColor()
        If colorType = ColTypes.Input And ColoredShell = True Then Write(esc + "[38;5;" + CStr(inputColor) + "m")
    End Sub
    Public Sub WriteWhere(ByVal msg As String, ByVal Left As Integer, ByVal Top As Integer, ByVal colorType As ColTypes, ByVal ParamArray vars() As Object)
        Dim esc As Char = GetEsc()
        If colorType = ColTypes.Neutral Or colorType = ColTypes.Input Then
            Write(esc + "[38;5;" + CStr(neutralTextColor) + "m")
        ElseIf colorType = ColTypes.Continuable Then
            Write(esc + "[38;5;" + CStr(contKernelErrorColor) + "m")
        ElseIf colorType = ColTypes.Uncontinuable Then
            Write(esc + "[38;5;" + CStr(uncontKernelErrorColor) + "m")
        ElseIf colorType = ColTypes.HostName Then
            Write(esc + "[38;5;" + CStr(hostNameShellColor) + "m")
        ElseIf colorType = ColTypes.UserName Then
            Write(esc + "[38;5;" + CStr(userNameShellColor) + "m")
        ElseIf colorType = ColTypes.License Then
            Write(esc + "[38;5;" + CStr(licenseColor) + "m")
        ElseIf colorType = ColTypes.Gray Then
            If backgroundColor = ConsoleColors.DarkYellow Or backgroundColor = ConsoleColors.Yellow Or backgroundColor = ConsoleColors.White Then
                Write(esc + "[38;5;" + CStr(neutralTextColor) + "m")
            Else
                Write(esc + "[38;5;" + CStr(ConsoleColors.Gray) + "m")
            End If
        ElseIf colorType = ColTypes.HelpDef Then
            Write(esc + "[38;5;" + CStr(cmdDefColor) + "m")
        ElseIf colorType = ColTypes.HelpCmd Then
            Write(esc + "[38;5;" + CStr(cmdListColor) + "m")
        ElseIf colorType = ColTypes.Stage Then
            Write(esc + "[38;5;" + CStr(stageColor) + "m")
        Else
            Exit Sub
        End If

        'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
        For v As Integer = 0 To vars.Length - 1
            msg = msg.Replace("{" + CStr(v) + "}", vars(v).ToString)
        Next

        'Write text in another place
        Dim OldLeft As Integer = CursorLeft
        Dim OldTop As Integer = CursorTop
        SetCursorPosition(Left, Top)
        Write(msg)
        SetCursorPosition(OldLeft, OldTop)
    End Sub

    Public Sub WriteC(ByVal text As Object, ByVal Line As Boolean, ByVal color As ConsoleColors, ByVal ParamArray vars() As Object)

        Dim esc As Char = GetEsc()
        Try
            'Try to write to console
            Write(esc + "[38;5;" + CStr(color) + "m")

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            For v As Integer = 0 To vars.Length - 1
                text = text.Replace("{" + CStr(v) + "}", vars(v).ToString)
            Next

            If Line Then WriteLine(text) Else Write(text)
            If backgroundColor = ConsoleColors.Black Then ResetColor()
        Catch ex As Exception
            WStkTrc(ex)
            KernelError("C", False, 0, DoTranslation("There is a serious error when printing text.", currentLang), ex)
        End Try

    End Sub

End Module
