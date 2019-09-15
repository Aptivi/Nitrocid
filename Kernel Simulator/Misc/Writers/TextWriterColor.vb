
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

Imports System.Console
Imports System.IO

'This module is very important to reduce line numbers when there is color.
Public Module TextWriterColor

    Public dbgWriter As New StreamWriter(paths("Debugging"), True) With {.AutoFlush = True}

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
            Dim OffendingIndex As New List(Of Integer)

            'For contributors who are testing new code: Uncomment the two Debug.WriteLine lines for immediate debugging (Immediate Window)
            If Not Source Is Nothing And Not LineNum = 0 Then
                'Debug to file and all connected debug devices (raw mode)
                dbgWriter.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString} ({Source}:{LineNum}): {text}", vars)
                For i As Integer = 0 To dbgConns.Count - 1
                    Try
                        dbgConns(i).WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString} ({Source}:{LineNum}): {text}", vars)
                    Catch ex As Exception
                        OffendingIndex.Add(GetSWIndex(dbgConns(i)))
                    End Try
                Next
                'Debug.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString} ({Source}:{LineNum}): {text}", vars)
            Else 'Rare case, unless debug symbol is not found on archives.
                dbgWriter.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString}: {text}", vars)
                For i As Integer = 0 To dbgConns.Count - 1
                    Try
                        dbgConns(i).WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString}: {text}", vars)
                    Catch ex As Exception
                        OffendingIndex.Add(GetSWIndex(dbgConns(i)))
                    End Try
                Next
                'Debug.WriteLine($"{KernelDateTime.ToShortDateString} {KernelDateTime.ToShortTimeString}: {text}", vars)
            End If

            'Disconnect offending clients who are disconnected
            For Each i As Integer In OffendingIndex
                If i <> -1 Then
                    DebugDevices.Keys(i).Disconnect(True)
                    dbgConns.RemoveAt(i)
                    Wdbg("Debug device {0} disconnected.", DebugDevices.Values(i))
                    DebugDevices.Remove(DebugDevices.Keys(i))
                End If
            Next
            OffendingIndex.Clear()
        End If
    End Sub

    Public Sub WStkTrc(ByVal Ex As Exception)
        If DebugMode Then
            'These two vbNewLines are padding for accurate stack tracing.
            dbgWriter.WriteLine($"{vbNewLine}{Ex.ToString.Substring(0, Ex.ToString.IndexOf(":"))}: {Ex.Message}{vbNewLine}{Ex.StackTrace}{vbNewLine}")
        End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt, and sets colors as needed.
    ''' </summary>
    ''' <param name="text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
    ''' <param name="Line">Whether to print a new line or not</param>
    ''' <param name="colorType">A type of colors that will be changed.</param>
    ''' <param name="vars">Endless amounts of any variables that is separated by commas.</param>
    ''' <remarks>This is used to reduce number of lines containing "System.Console.ForegroundColor = " and "System.Console.ResetColor()" text.</remarks>
    Public Sub W(ByVal text As Object, ByVal Line As Boolean, ByVal colorType As ColTypes, ByVal ParamArray vars() As Object)

        Try
            If colorType = ColTypes.Neutral Or colorType = ColTypes.Input Then
                ForegroundColor = neutralTextColor
            ElseIf colorType = ColTypes.Continuable Then
                ForegroundColor = contKernelErrorColor
            ElseIf colorType = ColTypes.Uncontinuable Then
                ForegroundColor = uncontKernelErrorColor
            ElseIf colorType = ColTypes.HostName Then
                ForegroundColor = hostNameShellColor
            ElseIf colorType = ColTypes.UserName Then
                ForegroundColor = userNameShellColor
            ElseIf colorType = ColTypes.License Then
                ForegroundColor = licenseColor
            ElseIf colorType = ColTypes.Gray Then
                ForegroundColor = ConsoleColor.Gray
            ElseIf colorType = ColTypes.HelpDef Then
                ForegroundColor = cmdDefColor
            ElseIf colorType = ColTypes.HelpCmd Then
                ForegroundColor = cmdListColor
            Else
                Exit Sub
            End If

            'Parse variables ({0}, {1}, ...) in the "text" string variable. (Used as a workaround for Linux)
            For v As Integer = 0 To vars.Length - 1
                text = text.Replace("{" + CStr(v) + "}", vars(v).ToString)
            Next

            If Line Then WriteLine(text) Else Write(text)
            If Console.BackgroundColor = ConsoleColor.Black Then ResetColor()
            If colorType = ColTypes.Input And ColoredShell = True Then ForegroundColor = inputColor
        Catch ex As Exception
            KernelError("C", False, 0, DoTranslation("There is a serious error when printing text.", currentLang), ex)
        End Try

    End Sub

End Module
