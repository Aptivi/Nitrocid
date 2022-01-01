
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

Public Class Color

    ''' <summary>
    ''' Either 0-255, or &lt;R&gt;;&lt;G&gt;;&lt;B&gt;
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property PlainSequence As String
    ''' <summary>
    ''' Either 0-255, or &lt;R&gt;;&lt;G&gt;;&lt;B&gt; enclosed in quotes if necessary
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property PlainSequenceEnclosed As String
    ''' <summary>
    ''' Parsable VT sequence (Foreground)
    ''' </summary>
    Public ReadOnly Property VTSequenceForeground As String
    ''' <summary>
    ''' Parsable VT sequence (Background)
    ''' </summary>
    Public ReadOnly Property VTSequenceBackground As String
    ''' <summary>
    ''' The red color value
    ''' </summary>
    Public ReadOnly Property R As Integer
    ''' <summary>
    ''' The green color value
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property G As Integer
    ''' <summary>
    ''' The blue color value
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property B As Integer
    ''' <summary>
    ''' Color type
    ''' </summary>
    Public ReadOnly Property Type As ColorType
    ''' <summary>
    ''' Is the color bright?
    ''' </summary>
    Public ReadOnly Property IsBright As Boolean
    ''' <summary>
    ''' Is the color dark?
    ''' </summary>
    Public ReadOnly Property IsDark As Boolean

    ''' <summary>
    ''' Makes a new instance of color class from specifier.
    ''' </summary>
    ''' <param name="ColorSpecifier">A color specifier. It must be a valid number from 0-255 if using 255-colors, or a VT sequence if using true color as follows: &lt;R&gt;;&lt;G&gt;;&lt;B&gt;</param>
    ''' <exception cref="Exceptions.ColorException"></exception>
    Public Sub New(ColorSpecifier As String)
        If ColorSpecifier.Contains(";") Then
            ColorSpecifier = ColorSpecifier.Replace("""", "")
            Dim ColorSpecifierArray() As String = ColorSpecifier.Split(";")
            If ColorSpecifierArray.Length = 3 Then
                PlainSequence = $"{ColorSpecifierArray(0)};{ColorSpecifierArray(1)};{ColorSpecifierArray(2)}"
                PlainSequenceEnclosed = $"{ColorSpecifierArray(0)};{ColorSpecifierArray(1)};{ColorSpecifierArray(2)}".EncloseByDoubleQuotes
                VTSequenceForeground = GetEsc() + $"[38;2;{PlainSequence}m"
                VTSequenceBackground = GetEsc() + $"[48;2;{PlainSequence}m"
                Type = ColorType.TrueColor
                IsBright = ColorSpecifierArray(0) + 0.2126 + ColorSpecifierArray(1) + 0.7152 + ColorSpecifierArray(2) + 0.0722 > 255 / 2
                IsDark = ColorSpecifierArray(0) + 0.2126 + ColorSpecifierArray(1) + 0.7152 + ColorSpecifierArray(2) + 0.0722 < 255 / 2
                R = ColorSpecifierArray(0)
                G = ColorSpecifierArray(1)
                B = ColorSpecifierArray(2)
            Else
                Throw New Exceptions.ColorException(DoTranslation("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows:") + " <R>;<G>;<B>")
            End If
        ElseIf IsNumeric(ColorSpecifier) Then
            ColorSpecifier = ColorSpecifier.Replace("""", "")
            Dim ColorsInfo As New ConsoleColorsInfo(ColorSpecifier)
            PlainSequence = ColorSpecifier
            PlainSequenceEnclosed = ColorSpecifier
            VTSequenceForeground = GetEsc() + $"[38;5;{PlainSequence}m"
            VTSequenceBackground = GetEsc() + $"[48;5;{PlainSequence}m"
            Type = ColorType._255Color
            IsBright = ColorsInfo.IsBright
            IsDark = ColorsInfo.IsDark
            R = ColorsInfo.R
            G = ColorsInfo.G
            B = ColorsInfo.B
        Else
            Throw New Exceptions.ColorException(DoTranslation("Invalid color specifier. Ensure that it's on the correct format, which means a number from 0-255 if using 255 colors or a VT sequence if using true color as follows:") + " <R>;<G>;<B>")
        End If
    End Sub

    ''' <summary>
    ''' Makes a new instance of color class from specifier.
    ''' </summary>
    ''' <param name="R">The red level</param>
    ''' <param name="G">The green level</param>
    ''' <param name="B">The blue level</param>
    ''' <exception cref="Exceptions.ColorException"></exception>
    Public Sub New(R As Integer, G As Integer, B As Integer)
        PlainSequence = $"{R};{G};{B}"
        PlainSequenceEnclosed = $"{R};{G};{B}".EncloseByDoubleQuotes
        VTSequenceForeground = GetEsc() + $"[38;2;{PlainSequence}m"
        VTSequenceBackground = GetEsc() + $"[48;2;{PlainSequence}m"
        Type = ColorType.TrueColor
        IsBright = R + 0.2126 + G + 0.7152 + B + 0.0722 > 255 / 2
        IsDark = R + 0.2126 + G + 0.7152 + B + 0.0722 < 255 / 2
        Me.R = R
        Me.G = G
        Me.B = B
    End Sub

    ''' <summary>
    ''' Makes a new instance of color class from specifier.
    ''' </summary>
    ''' <param name="ColorNum">The color number</param>
    ''' <exception cref="Exceptions.ColorException"></exception>
    Public Sub New(ColorNum As Integer)
        Dim ColorsInfo As New ConsoleColorsInfo(ColorNum)
        PlainSequence = ColorNum
        PlainSequenceEnclosed = ColorNum
        VTSequenceForeground = GetEsc() + $"[38;5;{PlainSequence}m"
        VTSequenceBackground = GetEsc() + $"[48;5;{PlainSequence}m"
        Type = ColorType._255Color
        IsBright = ColorsInfo.IsBright
        IsDark = ColorsInfo.IsDark
        R = ColorsInfo.R
        G = ColorsInfo.G
        B = ColorsInfo.B
    End Sub

End Class
