
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Namespace ConsoleBase
    Public Module Input

        ''' <summary>
        ''' Current mask character
        ''' </summary>
        Public CurrentMask As String = "*"c

        ''' <summary>
        ''' Reads the line from the console (not implemented yet)
        ''' </summary>
        ''' <param name="InputText">Input text to write</param>
        ''' <param name="DefaultValue">Default value</param>
        Public Function ReadLine(InputText As String, DefaultValue As String) As String
            Throw New NotImplementedException("ReadLine.Reboot will be used, so not implemented at this time.")
        End Function

        ''' <summary>
        ''' Reads the next line of characters from the standard input stream without showing input being written by user.
        ''' </summary>
        Public Function ReadLineNoInput() As String
            If Not String.IsNullOrEmpty(CurrentMask) Then
                Return ReadLineNoInput(CurrentMask(0))
            Else
                Return ReadLineNoInput("")
            End If
        End Function

        ''' <summary>
        ''' Reads the next line of characters from the standard input stream without showing input being written by user.
        ''' </summary>
        ''' <param name="MaskChar">Specifies the password mask character</param>
        Public Function ReadLineNoInput(MaskChar As Char) As String
            'TODO: Use ReadLine.Reboot to do this job.
            Dim Final As String = ""
            While True
                Dim KeyInfo As ConsoleKeyInfo = Console.ReadKey(True)
                Dim KeyCharacter As Char = KeyInfo.KeyChar
                If KeyCharacter = vbCr Or KeyCharacter = vbLf Then
                    Exit While
                ElseIf KeyInfo.Key = ConsoleKey.Backspace Then
                    If Not Final.Length = 0 Then
                        Final = Final.Remove(Final.Length - 1)
                        If Not MaskChar = vbNullChar Then
                            Dim OldCursorLeft As Integer = Console.CursorLeft
                            Console.CursorLeft = OldCursorLeft - 1
                            Console.Write(" ")
                            Console.CursorLeft = OldCursorLeft - 1
                        End If
                    End If
                Else
                    Final += KeyCharacter
                    If Not MaskChar = vbNullChar Then Console.Write(MaskChar)
                End If
            End While
            Return Final
        End Function

        ''' <summary>
        ''' Reads the next key from the console input stream with the timeout
        ''' </summary>
        ''' <param name="Intercept"></param>
        ''' <param name="Timeout"></param>
        Public Function ReadKeyTimeout(Intercept As Boolean, Timeout As TimeSpan) As ConsoleKeyInfo
            Dim CurrentMilliseconds As Double
            While Not Console.KeyAvailable
                If Not CurrentMilliseconds = Timeout.TotalMilliseconds Then
                    CurrentMilliseconds += 1
                Else
                    Throw New Exceptions.ConsoleReadTimeoutException(DoTranslation("User didn't provide any input in a timely fashion."))
                End If
                Threading.Thread.Sleep(1)
            End While
            Return Console.ReadKey(Intercept)
        End Function

        ''' <summary>
        ''' Reads the next line of characters until the condition is met or the user pressed ENTER
        ''' </summary>
        ''' <param name="Condition">The condition to be met</param>
        Public Function ReadLineUntil(ByRef Condition As Boolean) As String
            Dim Final As String = ""
            Dim Finished As Boolean
            While Not Finished
                Dim KeyInfo As ConsoleKeyInfo
                Dim KeyCharacter As Char
                While Not Console.KeyAvailable
                    If Condition Then Finished = True
                    Threading.Thread.Sleep(1)
                    If Finished Then Exit While
                End While
                If Not Finished Then
                    KeyInfo = Console.ReadKey(True)
                    KeyCharacter = KeyInfo.KeyChar
                    If KeyCharacter = vbCr Or KeyCharacter = vbLf Then
                        Finished = True
                    ElseIf KeyInfo.Key = ConsoleKey.Backspace Then
                        If Not Final.Length = 0 Then
                            Final = Final.Remove(Final.Length - 1)
                            Console.Write(GetEsc() + "D") 'Cursor backwards by one character
                            Console.Write(GetEsc() + "[1X") 'Remove a character
                        End If
                    Else
                        Final += KeyCharacter
                        Console.Write(KeyCharacter)
                    End If
                End If
            End While
            Return Final
        End Function

        ''' <summary>
        ''' Reads the next line of characters that exceed the 256-character limit up to 65536 characters
        ''' </summary>
        Public Function ReadLineLong() As String
            Console.SetIn(New StreamReader(Console.OpenStandardInput(65536), Console.InputEncoding, False, 65536))
            Return Console.ReadLine()
        End Function

    End Module
End Namespace