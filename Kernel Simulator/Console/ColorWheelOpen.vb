
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

Public Module ColorWheelOpen

    Public WheelUpperLeftCornerChar As String = "╔"
    Public WheelUpperRightCornerChar As String = "╗"
    Public WheelLowerLeftCornerChar As String = "╚"
    Public WheelLowerRightCornerChar As String = "╝"
    Public WheelUpperFrameChar As String = "═"
    Public WheelLowerFrameChar As String = "═"
    Public WheelLeftFrameChar As String = "║"
    Public WheelRightFrameChar As String = "║"

    ''' <summary>
    ''' Initializes color wheel
    ''' </summary>
    Public Function ColorWheel() As String
        Return ColorWheel(False, ConsoleColors.White, 0, 0, 0)
    End Function

    ''' <summary>
    ''' Initializes color wheel
    ''' </summary>
    ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
    Public Function ColorWheel(TrueColor As Boolean) As String
        Return ColorWheel(TrueColor, ConsoleColors.White, 0, 0, 0)
    End Function

    ''' <summary>
    ''' Initializes color wheel
    ''' </summary>
    ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
    ''' <param name="DefaultColor">The default 255-color to use</param>
    Public Function ColorWheel(TrueColor As Boolean, DefaultColor As ConsoleColors) As String
        Return ColorWheel(TrueColor, DefaultColor, 0, 0, 0)
    End Function

    ''' <summary>
    ''' Initializes color wheel
    ''' </summary>
    ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
    ''' <param name="DefaultColorR">The default red color range of 0-255 to use</param>
    ''' <param name="DefaultColorG">The default green color range of 0-255 to use</param>
    ''' <param name="DefaultColorB">The default blue color range of 0-255 to use</param>
    Public Function ColorWheel(TrueColor As Boolean, DefaultColorR As Integer, DefaultColorG As Integer, DefaultColorB As Integer) As String
        Return ColorWheel(TrueColor, ConsoleColors.White, DefaultColorR, DefaultColorG, DefaultColorB)
    End Function

    ''' <summary>
    ''' Initializes color wheel
    ''' </summary>
    ''' <param name="TrueColor">Whether or not to use true color. It can be changed dynamically during runtime.</param>
    ''' <param name="DefaultColor">The default 255-color to use</param>
    ''' <param name="DefaultColorR">The default red color range of 0-255 to use</param>
    ''' <param name="DefaultColorG">The default green color range of 0-255 to use</param>
    ''' <param name="DefaultColorB">The default blue color range of 0-255 to use</param>
    Public Function ColorWheel(TrueColor As Boolean, DefaultColor As ConsoleColors, DefaultColorR As Integer, DefaultColorG As Integer, DefaultColorB As Integer) As String
        Dim CurrentColor As ConsoleColors = DefaultColor
        Dim CurrentColorR As Integer = DefaultColorR
        Dim CurrentColorG As Integer = DefaultColorG
        Dim CurrentColorB As Integer = DefaultColorB
        Dim CurrentRange As Char = "R"
        Dim ColorWheelExiting As Boolean

        Console.CursorVisible = False
        While Not ColorWheelExiting
            Console.Clear()
            If TrueColor Then
                Write(vbNewLine + DoTranslation("Select color using ""<-"" and ""->"" keys. Press ENTER to quit. Press ""i"" to insert color number manually."), True, ColTypes.Tip)
                Write(DoTranslation("Press ""t"" to switch to 255 color mode."), True, ColTypes.Tip)
                Write(DoTranslation("Press ""c"" to write full color code."), True, ColTypes.Tip)

                'The red color level
                Dim RedForeground As Color = If(CurrentRange = "R", New Color(ConsoleColors.Black), New Color("255;0;0"))
                Dim RedBackground As Color = If(CurrentRange = "R", New Color("255;0;0"), New Color(ConsoleColors.Black))
                Write(vbNewLine + "  ", False, ColTypes.Neutral)
                Write(" < ", False, RedForeground, RedBackground)
                WriteWhere("R: {0}", (Console.CursorLeft + 35 - $"R: {CurrentColorR}".Length) / 2, Console.CursorTop, True, New Color($"{CurrentColorR};0;0"), CurrentColorR)
                WriteWhere(" > " + vbNewLine, Console.CursorLeft + 32, Console.CursorTop, False, RedForeground, RedBackground)

                'The green color level
                Dim GreenForeground As Color = If(CurrentRange = "G", New Color(ConsoleColors.Black), New Color("0;255;0"))
                Dim GreenBackground As Color = If(CurrentRange = "G", New Color("0;255;0"), New Color(ConsoleColors.Black))
                Write(vbNewLine + "  ", False, ColTypes.Neutral)
                Write(" < ", False, GreenForeground, GreenBackground)
                WriteWhere("G: {0}", (Console.CursorLeft + 35 - $"G: {CurrentColorG}".Length) / 2, Console.CursorTop, True, New Color($"0;{CurrentColorG};0"), CurrentColorG)
                WriteWhere(" > " + vbNewLine, Console.CursorLeft + 32, Console.CursorTop, False, GreenForeground, GreenBackground)

                'The blue color level
                Dim BlueForeground As Color = If(CurrentRange = "B", New Color(ConsoleColors.Black), New Color("0;0;255"))
                Dim BlueBackground As Color = If(CurrentRange = "B", New Color("0;0;255"), New Color(ConsoleColors.Black))
                Write(vbNewLine + "  ", False, ColTypes.Neutral)
                Write(" < ", False, BlueForeground, BlueBackground)
                WriteWhere("B: {0}", (Console.CursorLeft + 35 - $"B: {CurrentColorB}".Length) / 2, Console.CursorTop, True, New Color($"0;0;{CurrentColorB}"), CurrentColorB)
                WriteWhere(" > " + vbNewLine, Console.CursorLeft + 32, Console.CursorTop, False, BlueForeground, BlueBackground)

                'Draw the RGB ramp
                WriteWhere(WheelUpperLeftCornerChar + WheelUpperFrameChar.Repeat(Console.WindowWidth - 6) + WheelUpperRightCornerChar, 2, Console.WindowHeight - 6, True, ColTypes.Gray)
                WriteWhere(WheelLeftFrameChar + " ".Repeat(Console.WindowWidth - 6) + WheelRightFrameChar, 2, Console.WindowHeight - 5, True, ColTypes.Gray)
                WriteWhere(WheelLeftFrameChar + " ".Repeat(Console.WindowWidth - 6) + WheelRightFrameChar, 2, Console.WindowHeight - 4, True, ColTypes.Gray)
                WriteWhere(WheelLeftFrameChar + " ".Repeat(Console.WindowWidth - 6) + WheelRightFrameChar, 2, Console.WindowHeight - 3, True, ColTypes.Gray)
                WriteWhere(WheelLowerLeftCornerChar + WheelLowerFrameChar.Repeat(Console.WindowWidth - 6) + WheelLowerRightCornerChar, 2, Console.WindowHeight - 2, True, ColTypes.Gray)
                WriteWhere(" ".Repeat(PercentRepeat(CurrentColorR, 255, 6)), 3, Console.WindowHeight - 5, True, New Color(ConsoleColors.Black), New Color(255, 0, 0))
                WriteWhere(" ".Repeat(PercentRepeat(CurrentColorG, 255, 6)), 3, Console.WindowHeight - 4, True, New Color(ConsoleColors.Black), New Color(0, 255, 0))
                WriteWhere(" ".Repeat(PercentRepeat(CurrentColorB, 255, 6)), 3, Console.WindowHeight - 3, True, New Color(ConsoleColors.Black), New Color(0, 0, 255))

                'Show example
                Write(vbNewLine + "- Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, New Color($"{CurrentColorR};{CurrentColorG};{CurrentColorB}"))

                'Read and get response
                Dim ConsoleResponse As ConsoleKeyInfo = Console.ReadKey(True)
                If ConsoleResponse.Key = ConsoleKey.LeftArrow Then
                    Select Case CurrentRange
                        Case "R"
                            If CurrentColorR = 0 Then
                                CurrentColorR = 255
                            Else
                                CurrentColorR -= 1
                            End If
                        Case "G"
                            If CurrentColorG = 0 Then
                                CurrentColorG = 255
                            Else
                                CurrentColorG -= 1
                            End If
                        Case "B"
                            If CurrentColorB = 0 Then
                                CurrentColorB = 255
                            Else
                                CurrentColorB -= 1
                            End If
                    End Select
                ElseIf ConsoleResponse.Key = ConsoleKey.RightArrow Then
                    Select Case CurrentRange
                        Case "R"
                            If CurrentColorR = 255 Then
                                CurrentColorR = 0
                            Else
                                CurrentColorR += 1
                            End If
                        Case "G"
                            If CurrentColorG = 255 Then
                                CurrentColorG = 0
                            Else
                                CurrentColorG += 1
                            End If
                        Case "B"
                            If CurrentColorB = 255 Then
                                CurrentColorB = 0
                            Else
                                CurrentColorB += 1
                            End If
                    End Select
                ElseIf ConsoleResponse.Key = ConsoleKey.UpArrow Then
                    Select Case CurrentRange
                        Case "R"
                            CurrentRange = "B"
                        Case "G"
                            CurrentRange = "R"
                        Case "B"
                            CurrentRange = "G"
                    End Select
                ElseIf ConsoleResponse.Key = ConsoleKey.DownArrow Then
                    Select Case CurrentRange
                        Case "R"
                            CurrentRange = "G"
                        Case "G"
                            CurrentRange = "B"
                        Case "B"
                            CurrentRange = "R"
                    End Select
                ElseIf ConsoleResponse.Key = ConsoleKey.I Then
                    Dim _DefaultColor As Integer
                    Select Case CurrentRange
                        Case "R"
                            _DefaultColor = CurrentColorR
                        Case "G"
                            _DefaultColor = CurrentColorG
                        Case "B"
                            _DefaultColor = CurrentColorB
                    End Select
                    WriteWhere(DoTranslation("Enter color number from 0 to 255:") + " [{0}] ", 0, Console.WindowHeight - 1, False, ColTypes.Input, _DefaultColor)
                    Console.CursorVisible = True
                    Dim ColorNum As String = Console.ReadLine
                    Console.CursorVisible = False
                    If IsNumeric(ColorNum) Then
                        If ColorNum >= 0 And ColorNum <= 255 Then
                            Select Case CurrentRange
                                Case "R"
                                    CurrentColorR = ColorNum
                                Case "G"
                                    CurrentColorG = ColorNum
                                Case "B"
                                    CurrentColorB = ColorNum
                            End Select
                        End If
                    End If
                ElseIf ConsoleResponse.Key = ConsoleKey.C Then
                    WriteWhere(DoTranslation("Enter color code that satisfies these formats:") + " ""RRR;GGG;BBB"" / 0-255 [{0}] ", 0, Console.WindowHeight - 1, False, ColTypes.Input, $"{CurrentColorR};{CurrentColorG};{CurrentColorB}")
                    Console.CursorVisible = True
                    Dim ColorSequence As String = Console.ReadLine
                    Console.CursorVisible = False
                    Try
                        Dim ParsedColor As New Color(ColorSequence)
                        CurrentColorR = ParsedColor.R
                        CurrentColorG = ParsedColor.G
                        CurrentColorB = ParsedColor.B
                    Catch ex As Exception
                        WStkTrc(ex)
                        Wdbg(DebugLevel.E, "Possible input error: {0} ({1})", ColorSequence, ex.Message)
                    End Try
                ElseIf ConsoleResponse.Key = ConsoleKey.T Then
                    TrueColor = False
                ElseIf ConsoleResponse.Key = ConsoleKey.Enter Then
                    ColorWheelExiting = True
                End If
            Else
                Write(vbNewLine + DoTranslation("Select color using ""<-"" and ""->"" keys. Use arrow up and arrow down keys to select between color ranges. Press ENTER to quit. Press ""i"" to insert color number manually."), True, ColTypes.Tip)
                Write(DoTranslation("Press ""t"" to switch to true color mode."), True, ColTypes.Tip)

                'The color selection
                Write(vbNewLine + "   < ", False, ColTypes.Gray)
                WriteWhere($"{CurrentColor} [{Convert.ToInt32(CurrentColor)}]", (Console.CursorLeft + 38 - $"{CurrentColor} [{Convert.ToInt32(CurrentColor)}]".Length) / 2, Console.CursorTop, True, New Color(CurrentColor))
                WriteWhere(" >", Console.CursorLeft + 32, Console.CursorTop, False, ColTypes.Gray)

                'Show prompt
                Write(vbNewLine + vbNewLine + "- Lorem ipsum dolor sit amet, consectetur adipiscing elit.", True, New Color(CurrentColor))

                'Read and get response
                Dim ConsoleResponse As ConsoleKeyInfo = Console.ReadKey(True)
                If ConsoleResponse.Key = ConsoleKey.LeftArrow Then
                    If CurrentColor = 0 Then
                        CurrentColor = 255
                    Else
                        CurrentColor -= 1
                    End If
                ElseIf ConsoleResponse.Key = ConsoleKey.RightArrow Then
                    If CurrentColor = 255 Then
                        CurrentColor = 0
                    Else
                        CurrentColor += 1
                    End If
                ElseIf ConsoleResponse.Key = ConsoleKey.I Then
                    WriteWhere(DoTranslation("Enter color number from 0 to 255:") + " [{0}] ", 0, Console.WindowHeight - 1, False, ColTypes.Input, CInt(CurrentColor))
                    Console.CursorVisible = True
                    Dim ColorNum As String = Console.ReadLine
                    Console.CursorVisible = False
                    If IsNumeric(ColorNum) Then
                        If ColorNum >= 0 And ColorNum <= 255 Then
                            CurrentColor = ColorNum
                        End If
                    End If
                ElseIf ConsoleResponse.Key = ConsoleKey.T Then
                    TrueColor = True
                ElseIf ConsoleResponse.Key = ConsoleKey.Enter Then
                    ColorWheelExiting = True
                End If
            End If
        End While

        Console.CursorVisible = True
        If TrueColor Then
            Return $"{CurrentColorR};{CurrentColorG};{CurrentColorB}"
        Else
            Return CurrentColor
        End If
    End Function

End Module
