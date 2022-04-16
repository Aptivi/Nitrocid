
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

Imports Figgle
Imports KS.Misc.Reflection

Namespace Misc.Writers.FancyWriters
    Public Module FigletColor

        ''' <summary>
        ''' Writes the figlet text
        ''' </summary>
        ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        ''' <param name="FigletFont">Figlet font to use in the text.</param>
        ''' <param name="Vars">Variables to format the message before it's written.</param>
        Public Sub WriteFigletPlain(Text As String, FigletFont As FiggleFont, ParamArray Vars() As Object)
            Try
                'Format string as needed
                If Not Vars.Length = 0 Then Text = FormatString(Text, Vars)

                'Write the font
                Text = FigletFont.Render(Text)
                WritePlain(Text, True, Vars)
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
        End Sub

        ''' <summary>
        ''' Writes the figlet text
        ''' </summary>
        ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        ''' <param name="FigletFont">Figlet font to use in the text.</param>
        ''' <param name="ColTypes">A type of colors that will be changed.</param>
        ''' <param name="Vars">Variables to format the message before it's written.</param>
        Public Sub WriteFiglet(Text As String, FigletFont As FiggleFont, ColTypes As ColTypes, ParamArray Vars() As Object)
            Try
                'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                SetConsoleColor(ColTypes)

                'Actually write
                WriteFigletPlain(Text, FigletFont, Vars)
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
        End Sub

        ''' <summary>
        ''' Writes the figlet text
        ''' </summary>
        ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        ''' <param name="FigletFont">Figlet font to use in the text.</param>
        ''' <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        ''' <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        ''' <param name="Vars">Variables to format the message before it's written.</param>
        Public Sub WriteFiglet(Text As String, FigletFont As FiggleFont, colorTypeForeground As ColTypes, colorTypeBackground As ColTypes, ParamArray Vars() As Object)
            Try
                'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                SetConsoleColor(colorTypeForeground)
                SetConsoleColor(colorTypeBackground, True)

                'Actually write
                WriteFigletPlain(Text, FigletFont, Vars)
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
        End Sub

        ''' <summary>
        ''' Writes the figlet text
        ''' </summary>
        ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        ''' <param name="FigletFont">Figlet font to use in the text.</param>
        ''' <param name="color">A color that will be changed to.</param>
        ''' <param name="Vars">Variables to format the message before it's written.</param>
        Public Sub WriteFiglet(Text As String, FigletFont As FiggleFont, Color As ConsoleColor, ParamArray Vars() As Object)
            Try
                'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                SetConsoleColor(New Color(Color))

                'Actually write
                WriteFigletPlain(Text, FigletFont, Vars)
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
        End Sub

        ''' <summary>
        ''' Writes the figlet text
        ''' </summary>
        ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        ''' <param name="FigletFont">Figlet font to use in the text.</param>
        ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
        ''' <param name="BackgroundColor">A background color that will be changed to.</param>
        ''' <param name="Vars">Variables to format the message before it's written.</param>
        Public Sub WriteFiglet(Text As String, FigletFont As FiggleFont, ForegroundColor As ConsoleColor, BackgroundColor As ConsoleColor, ParamArray Vars() As Object)
            Try
                'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                SetConsoleColor(New Color(ForegroundColor))
                SetConsoleColor(New Color(BackgroundColor), True)

                'Actually write
                WriteFigletPlain(Text, FigletFont, Vars)
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
        End Sub

        ''' <summary>
        ''' Writes the figlet text
        ''' </summary>
        ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        ''' <param name="FigletFont">Figlet font to use in the text.</param>
        ''' <param name="Color">A color that will be changed to.</param>
        ''' <param name="Vars">Variables to format the message before it's written.</param>
        Public Sub WriteFiglet(Text As String, FigletFont As FiggleFont, Color As Color, ParamArray Vars() As Object)
            Try
                'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                SetConsoleColor(Color)

                'Actually write
                WriteFigletPlain(Text, FigletFont, Vars)
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
        End Sub

        ''' <summary>
        ''' Writes the figlet text
        ''' </summary>
        ''' <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        ''' <param name="FigletFont">Figlet font to use in the text.</param>
        ''' <param name="ForegroundColor">A foreground color that will be changed to.</param>
        ''' <param name="BackgroundColor">A background color that will be changed to.</param>
        ''' <param name="Vars">Variables to format the message before it's written.</param>
        Public Sub WriteFiglet(Text As String, FigletFont As FiggleFont, ForegroundColor As Color, BackgroundColor As Color, ParamArray Vars() As Object)
            Try
                'Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                SetConsoleColor(ForegroundColor)
                SetConsoleColor(BackgroundColor, True)

                'Actually write
                WriteFigletPlain(Text, FigletFont, Vars)
            Catch ex As Exception When Not ex.GetType.Name = "ThreadAbortException"
                WStkTrc(ex)
                KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
        End Sub

    End Module
End Namespace
