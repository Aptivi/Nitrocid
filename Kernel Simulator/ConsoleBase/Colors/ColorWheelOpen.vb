
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Imports KS.Misc.Reflection

Namespace ConsoleBase.Colors
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
            Wdbg(DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", CurrentColorR, CurrentColorG, CurrentColorB)
            Wdbg(DebugLevel.I, "Got color ({0})", CurrentColor)

            Console.CursorVisible = False
            While Not ColorWheelExiting
                Console.Clear()
                If TrueColor Then
                    Write(NewLine + DoTranslation("Select color using ""<-"" and ""->"" keys. Press ENTER to quit. Press ""i"" to insert color number manually."), True, ColTypes.Tip)
                    Write(DoTranslation("Press ""t"" to switch to 255 color mode."), True, ColTypes.Tip)
                    Write(DoTranslation("Press ""c"" to write full color code."), True, ColTypes.Tip)
                    Wdbg(DebugLevel.I, "Current Range: {0}", CurrentRange)

                    'The red color level
                    Dim RedForeground As Color = If(CurrentRange = "R", New Color(ConsoleColors.Black), New Color("255;0;0"))
                    Dim RedBackground As Color = If(CurrentRange = "R", New Color("255;0;0"), New Color(ConsoleColors.Black))
                    Wdbg(DebugLevel.I, "Red foreground: {0} | Red background: {1}", RedForeground.PlainSequence, RedBackground.PlainSequence)
                    Write(NewLine + "  ", False, ColTypes.Neutral)
                    Write(" < ", False, RedForeground, RedBackground)
                    WriteWhere("R: {0}", (Console.CursorLeft + 35 - $"R: {CurrentColorR}".Length) / 2, Console.CursorTop, New Color($"{CurrentColorR};0;0"), CurrentColorR)
                    WriteWhere(" > " + NewLine, Console.CursorLeft + 32, Console.CursorTop, RedForeground, RedBackground)

                    'The green color level
                    Dim GreenForeground As Color = If(CurrentRange = "G", New Color(ConsoleColors.Black), New Color("0;255;0"))
                    Dim GreenBackground As Color = If(CurrentRange = "G", New Color("0;255;0"), New Color(ConsoleColors.Black))
                    Wdbg(DebugLevel.I, "Green foreground: {0} | Green background: {1}", GreenForeground.PlainSequence, GreenBackground.PlainSequence)
                    Write(NewLine + "  ", False, ColTypes.Neutral)
                    Write(" < ", False, GreenForeground, GreenBackground)
                    WriteWhere("G: {0}", (Console.CursorLeft + 35 - $"G: {CurrentColorG}".Length) / 2, Console.CursorTop, New Color($"0;{CurrentColorG};0"), CurrentColorG)
                    WriteWhere(" > " + NewLine, Console.CursorLeft + 32, Console.CursorTop, GreenForeground, GreenBackground)

                    'The blue color level
                    Dim BlueForeground As Color = If(CurrentRange = "B", New Color(ConsoleColors.Black), New Color("0;0;255"))
                    Dim BlueBackground As Color = If(CurrentRange = "B", New Color("0;0;255"), New Color(ConsoleColors.Black))
                    Wdbg(DebugLevel.I, "Blue foreground: {0} | Blue background: {1}", BlueForeground.PlainSequence, BlueBackground.PlainSequence)
                    Write(NewLine + "  ", False, ColTypes.Neutral)
                    Write(" < ", False, BlueForeground, BlueBackground)
                    WriteWhere("B: {0}", (Console.CursorLeft + 35 - $"B: {CurrentColorB}".Length) / 2, Console.CursorTop, New Color($"0;0;{CurrentColorB}"), CurrentColorB)
                    WriteWhere(" > " + NewLine, Console.CursorLeft + 32, Console.CursorTop, BlueForeground, BlueBackground)

                    'Draw the RGB ramp
                    WriteWhere(WheelUpperLeftCornerChar + WheelUpperFrameChar.Repeat(Console.WindowWidth - 6) + WheelUpperRightCornerChar, 2, Console.WindowHeight - 6, ColTypes.Gray)
                    WriteWhere(WheelLeftFrameChar + " ".Repeat(Console.WindowWidth - 6) + WheelRightFrameChar, 2, Console.WindowHeight - 5, ColTypes.Gray)
                    WriteWhere(WheelLeftFrameChar + " ".Repeat(Console.WindowWidth - 6) + WheelRightFrameChar, 2, Console.WindowHeight - 4, ColTypes.Gray)
                    WriteWhere(WheelLeftFrameChar + " ".Repeat(Console.WindowWidth - 6) + WheelRightFrameChar, 2, Console.WindowHeight - 3, ColTypes.Gray)
                    WriteWhere(WheelLowerLeftCornerChar + WheelLowerFrameChar.Repeat(Console.WindowWidth - 6) + WheelLowerRightCornerChar, 2, Console.WindowHeight - 2, ColTypes.Gray)
                    WriteWhere(" ".Repeat(PercentRepeat(CurrentColorR, 255, 6)), 3, Console.WindowHeight - 5, New Color(ConsoleColors.Black), New Color(255, 0, 0))
                    WriteWhere(" ".Repeat(PercentRepeat(CurrentColorG, 255, 6)), 3, Console.WindowHeight - 4, New Color(ConsoleColors.Black), New Color(0, 255, 0))
                    WriteWhere(" ".Repeat(PercentRepeat(CurrentColorB, 255, 6)), 3, Console.WindowHeight - 3, New Color(ConsoleColors.Black), New Color(0, 0, 255))

                    'Show example
                    Dim PreviewColor As New Color($"{CurrentColorR};{CurrentColorG};{CurrentColorB}")
                    Write(NewLine + "- Lorem ipsum dolor sit amet, consectetur adipiscing elit. ({0})", True, PreviewColor, PreviewColor.Hex)

                    'Read and get response
                    Dim ConsoleResponse As ConsoleKeyInfo = Console.ReadKey(True)
                    Wdbg(DebugLevel.I, "Keypress: {0}", ConsoleResponse.Key.ToString)
                    If ConsoleResponse.Key = ConsoleKey.LeftArrow Then
                        Wdbg(DebugLevel.I, "Decrementing number...")
                        Select Case CurrentRange
                            Case "R"
                                If CurrentColorR = 0 Then
                                    Wdbg(DebugLevel.I, "Reached zero! Back to 255.")
                                    CurrentColorR = 255
                                Else
                                    CurrentColorR -= 1
                                    Wdbg(DebugLevel.I, "Decremented to {0}", CurrentColorR)
                                End If
                            Case "G"
                                If CurrentColorG = 0 Then
                                    Wdbg(DebugLevel.I, "Reached zero! Back to 255.")
                                    CurrentColorG = 255
                                Else
                                    CurrentColorG -= 1
                                    Wdbg(DebugLevel.I, "Decremented to {0}", CurrentColorG)
                                End If
                            Case "B"
                                If CurrentColorB = 0 Then
                                    Wdbg(DebugLevel.I, "Reached zero! Back to 255.")
                                    CurrentColorB = 255
                                Else
                                    CurrentColorB -= 1
                                    Wdbg(DebugLevel.I, "Decremented to {0}", CurrentColorB)
                                End If
                        End Select
                    ElseIf ConsoleResponse.Key = ConsoleKey.RightArrow Then
                        Wdbg(DebugLevel.I, "Incrementing number...")
                        Select Case CurrentRange
                            Case "R"
                                If CurrentColorR = 255 Then
                                    Wdbg(DebugLevel.I, "Reached 255! Back to zero.")
                                    CurrentColorR = 0
                                Else
                                    CurrentColorR += 1
                                    Wdbg(DebugLevel.I, "Incremented to {0}", CurrentColorR)
                                End If
                            Case "G"
                                If CurrentColorG = 255 Then
                                    Wdbg(DebugLevel.I, "Reached 255! Back to zero.")
                                    CurrentColorG = 0
                                Else
                                    CurrentColorG += 1
                                    Wdbg(DebugLevel.I, "Incremented to {0}", CurrentColorG)
                                End If
                            Case "B"
                                If CurrentColorB = 255 Then
                                    Wdbg(DebugLevel.I, "Reached 255! Back to zero.")
                                    CurrentColorB = 0
                                Else
                                    CurrentColorB += 1
                                    Wdbg(DebugLevel.I, "Incremented to {0}", CurrentColorB)
                                End If
                        End Select
                    ElseIf ConsoleResponse.Key = ConsoleKey.UpArrow Then
                        Wdbg(DebugLevel.I, "Changing range...")
                        Select Case CurrentRange
                            Case "R"
                                CurrentRange = "B"
                            Case "G"
                                CurrentRange = "R"
                            Case "B"
                                CurrentRange = "G"
                        End Select
                    ElseIf ConsoleResponse.Key = ConsoleKey.DownArrow Then
                        Wdbg(DebugLevel.I, "Changing range...")
                        Select Case CurrentRange
                            Case "R"
                                CurrentRange = "G"
                            Case "G"
                                CurrentRange = "B"
                            Case "B"
                                CurrentRange = "R"
                        End Select
                    ElseIf ConsoleResponse.Key = ConsoleKey.I Then
                        Wdbg(DebugLevel.I, "Prompting for color number...")
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
                        Dim ColorNum As String = ReadLine()
                        Console.CursorVisible = False
                        Wdbg(DebugLevel.I, "Got response: {0}", ColorNum)
                        If IsStringNumeric(ColorNum) Then
                            Wdbg(DebugLevel.I, "Numeric! Checking range...")
                            If ColorNum >= 0 And ColorNum <= 255 Then
                                Wdbg(DebugLevel.I, "In range!")
                                Select Case CurrentRange
                                    Case "R"
                                        Wdbg(DebugLevel.I, "Changing red color level to {0}...", ColorNum)
                                        CurrentColorR = ColorNum
                                    Case "G"
                                        Wdbg(DebugLevel.I, "Changing green color level to {0}...", ColorNum)
                                        CurrentColorG = ColorNum
                                    Case "B"
                                        Wdbg(DebugLevel.I, "Changing blue color level to {0}...", ColorNum)
                                        CurrentColorB = ColorNum
                                End Select
                            End If
                        End If
                    ElseIf ConsoleResponse.Key = ConsoleKey.C Then
                        WriteWhere(DoTranslation("Enter color code that satisfies these formats:") + " ""RRR;GGG;BBB"" / 0-255 [{0}] ", 0, Console.WindowHeight - 1, False, ColTypes.Input, $"{CurrentColorR};{CurrentColorG};{CurrentColorB}")
                        Console.CursorVisible = True
                        Dim ColorSequence As String = ReadLine()
                        Console.CursorVisible = False
                        Try
                            Wdbg(DebugLevel.I, "Parsing {0}...", ColorSequence)
                            Dim ParsedColor As New Color(ColorSequence)
                            CurrentColorR = ParsedColor.R
                            CurrentColorG = ParsedColor.G
                            CurrentColorB = ParsedColor.B
                        Catch ex As Exception
                            WStkTrc(ex)
                            Wdbg(DebugLevel.E, "Possible input error: {0} ({1})", ColorSequence, ex.Message)
                        End Try
                    ElseIf ConsoleResponse.Key = ConsoleKey.T Then
                        Wdbg(DebugLevel.I, "Switching back to 255 color...")
                        TrueColor = False
                    ElseIf ConsoleResponse.Key = ConsoleKey.Enter Then
                        Wdbg(DebugLevel.I, "Exiting...")
                        ColorWheelExiting = True
                    End If
                Else
                    Write(NewLine + DoTranslation("Select color using ""<-"" and ""->"" keys. Use arrow up and arrow down keys to select between color ranges. Press ENTER to quit. Press ""i"" to insert color number manually."), True, ColTypes.Tip)
                    Write(DoTranslation("Press ""t"" to switch to true color mode."), True, ColTypes.Tip)

                    'The color selection
                    Write(NewLine + "   < ", False, ColTypes.Gray)
                    WriteWhere($"{CurrentColor} [{Convert.ToInt32(CurrentColor)}]", (Console.CursorLeft + 38 - $"{CurrentColor} [{Convert.ToInt32(CurrentColor)}]".Length) / 2, Console.CursorTop, New Color(CurrentColor))
                    WriteWhere(" >", Console.CursorLeft + 32, Console.CursorTop, ColTypes.Gray)

                    'Show prompt
                    Dim PreviewColor As New Color(CurrentColor)
                    Write(NewLine + NewLine + "- Lorem ipsum dolor sit amet, consectetur adipiscing elit. ({0})", True, PreviewColor, PreviewColor.Hex)

                    'Read and get response
                    Dim ConsoleResponse As ConsoleKeyInfo = Console.ReadKey(True)
                    Wdbg(DebugLevel.I, "Keypress: {0}", ConsoleResponse.Key.ToString)
                    If ConsoleResponse.Key = ConsoleKey.LeftArrow Then
                        Wdbg(DebugLevel.I, "Decrementing number...")
                        If CurrentColor = 0 Then
                            Wdbg(DebugLevel.I, "Reached zero! Back to 255.")
                            CurrentColor = 255
                        Else
                            CurrentColor -= 1
                            Wdbg(DebugLevel.I, "Decremented to {0}", CurrentColor)
                        End If
                    ElseIf ConsoleResponse.Key = ConsoleKey.RightArrow Then
                        If CurrentColor = 255 Then
                            Wdbg(DebugLevel.I, "Reached 255! Back to zero.")
                            CurrentColor = 0
                        Else
                            CurrentColor += 1
                            Wdbg(DebugLevel.I, "Incremented to {0}", CurrentColor)
                        End If
                    ElseIf ConsoleResponse.Key = ConsoleKey.I Then
                        Wdbg(DebugLevel.I, "Prompting for color number...")
                        WriteWhere(DoTranslation("Enter color number from 0 to 255:") + " [{0}] ", 0, Console.WindowHeight - 1, ColTypes.Input, CInt(CurrentColor))
                        Console.CursorVisible = True
                        Dim ColorNum As String = ReadLine()
                        Console.CursorVisible = False
                        Wdbg(DebugLevel.I, "Got response: {0}", ColorNum)
                        If IsStringNumeric(ColorNum) Then
                            Wdbg(DebugLevel.I, "Numeric! Checking range...")
                            If ColorNum >= 0 And ColorNum <= 255 Then
                                Wdbg(DebugLevel.I, "In range! Changing color level to {0}...", ColorNum)
                                CurrentColor = ColorNum
                            End If
                        End If
                    ElseIf ConsoleResponse.Key = ConsoleKey.T Then
                        Wdbg(DebugLevel.I, "Switching back to 255 color...")
                        TrueColor = True
                    ElseIf ConsoleResponse.Key = ConsoleKey.Enter Then
                        Wdbg(DebugLevel.I, "Exiting...")
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
End Namespace
