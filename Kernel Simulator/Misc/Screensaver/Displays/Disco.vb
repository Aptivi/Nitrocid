
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

Imports System.ComponentModel
Imports System.Threading

Module DiscoDisplay

    Public WithEvents Disco As New BackgroundWorker

    ''' <summary>
    ''' Handles the code of Disco
    ''' </summary>
    Sub Disco_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles Disco.DoWork
        Console.CursorVisible = False
        Dim MaximumColors As Integer = 15
        Dim MaximumColorsR As Integer = 255
        Dim MaximumColorsG As Integer = 255
        Dim MaximumColorsB As Integer = 255
        Dim CurrentColor As Integer = 0
        Dim CurrentColorR, CurrentColorG, CurrentColorB As Integer
        Dim random As New Random()
        Do While True
            SleepNoBlock(DiscoDelay, Disco)
            If Disco.CancellationPending = True Then
                Wdbg("W", "Cancellation is pending. Cleaning everything up...")
                e.Cancel = True
                Console.Clear()
                Dim esc As Char = GetEsc()
                Console.Write(esc + "[38;5;" + CStr(inputColor) + "m")
                Console.Write(esc + "[48;5;" + CStr(backgroundColor) + "m")
                LoadBack()
                Console.CursorVisible = True
                Wdbg("I", "All clean. Disco screensaver stopped.")
                SaverAutoReset.Set()
                Exit Do
            Else
                If DiscoTrueColor Then
                    Dim esc As Char = GetEsc()
                    If Not DiscoCycleColors Then
                        Dim RedColorNum As Integer = random.Next(255)
                        Dim GreenColorNum As Integer = random.Next(255)
                        Dim BlueColorNum As Integer = random.Next(255)
                        Dim ColorStorage As New RGB(RedColorNum, GreenColorNum, BlueColorNum)
                        Console.Write(esc + "[48;2;" + ColorStorage.ToString + "m")
                    Else
                        Dim ColorStorage As New RGB(CurrentColorR, CurrentColorG, CurrentColorB)
                        Console.Write(esc + "[48;2;" + ColorStorage.ToString + "m")
                    End If
                ElseIf Disco255Colors Then
                    Dim esc As Char = GetEsc()
                    If Not DiscoCycleColors Then
                        Dim color As Integer = random.Next(255)
                        Console.Write(esc + "[48;5;" + CStr(color) + "m")
                    Else
                        MaximumColors = 255
                        Console.Write(esc + "[48;5;" + CStr(CurrentColor) + "m")
                    End If
                Else
                    If Not DiscoCycleColors Then
                        Console.BackgroundColor = colors(random.Next(colors.Length - 1))
                    Else
                        Console.BackgroundColor = colors(CurrentColor)
                    End If
                End If
                Console.Clear()
                If DiscoTrueColor Then
                    If CurrentColorR >= MaximumColorsR Then
                        CurrentColorR = 0
                    Else
                        CurrentColorR += 1
                    End If
                    If CurrentColorG >= MaximumColorsG Then
                        CurrentColorG = 0
                    ElseIf CurrentColorR = 0 Then
                        CurrentColorG += 1
                    End If
                    If CurrentColorB >= MaximumColorsB Then
                        CurrentColorB = 0
                    ElseIf CurrentColorG = 0 And CurrentColorR = 0 Then
                        CurrentColorB += 1
                    End If
                    If CurrentColorB = 0 And CurrentColorG = 0 And CurrentColorR = 0 Then
                        CurrentColorB = 0
                        CurrentColorG = 0
                        CurrentColorR = 0
                    End If
                Else
                    If CurrentColor >= MaximumColors Then
                        CurrentColor = 0
                    Else
                        CurrentColor += 1
                    End If
                End If
            End If
        Loop
    End Sub

End Module
