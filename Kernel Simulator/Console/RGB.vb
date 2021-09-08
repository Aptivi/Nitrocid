
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

Public Class RGB

    Private _Red As Short
    Private _Green As Short
    Private _Blue As Short

    ''' <summary>
    ''' Red level
    ''' </summary>
    Public Property Red As Short
        Get
            Return _Red
        End Get
        Set
            If Value >= 0 And Value <= 255 Then
                _Red = Value
            Else
                Throw New InvalidOperationException(DoTranslation("Red color level exceeded. It was {0}.").FormatString(Value))
            End If
        End Set
    End Property
    ''' <summary>
    ''' Green level
    ''' </summary>
    Public Property Green As Short
        Get
            Return _Green
        End Get
        Set
            If Value >= 0 And Value <= 255 Then
                _Green = Value
            Else
                Throw New InvalidOperationException(DoTranslation("Green color level exceeded. It was {0}.").FormatString(Value))
            End If
        End Set
    End Property
    ''' <summary>
    ''' Blue level
    ''' </summary>
    Public Property Blue As Short
        Get
            Return _Blue
        End Get
        Set
            If Value >= 0 And Value <= 255 Then
                _Blue = Value
            Else
                Throw New InvalidOperationException(DoTranslation("Blue color level exceeded. It was {0}.").FormatString(Value))
            End If
        End Set
    End Property

    ''' <summary>
    ''' Makes a new instance of the RGB color storage
    ''' </summary>
    ''' <param name="RedLevel">Red color level of 0-255</param>
    ''' <param name="GreenLevel">Green color level of 0-255</param>
    ''' <param name="BlueLevel">Blue color level of 0-255</param>
    Public Sub New(RedLevel As Short, GreenLevel As Short, BlueLevel As Short)
        Red = RedLevel
        Green = GreenLevel
        Blue = BlueLevel
    End Sub

    ''' <summary>
    ''' Makes a new instance of the RGB color storage
    ''' </summary>
    ''' <param name="RGBString">A VT-sequence-compatible RGB color pattern. For example, RRR;GGG;BBB, which:
    ''' <br>RRR: a red color level of 0-255</br>
    ''' <br>GGG: a green color level of 0-255</br>
    ''' <br>BBB: a blue color level of 0-255</br></param>
    Public Sub New(RGBString As String)
        Dim RGBStringArray() As String = RGBString.Split(";")
        If RGBStringArray.Length = 3 Then
            If IsNumeric(RGBStringArray(0)) Then
                Red = RGBStringArray(0)
            End If
            If IsNumeric(RGBStringArray(1)) Then
                Green = RGBStringArray(1)
            End If
            If IsNumeric(RGBStringArray(2)) Then
                Blue = RGBStringArray(2)
            End If
        Else
            Throw New ArgumentException(DoTranslation("Invalid VT sequence for RGB color."))
        End If
    End Sub

    ''' <summary>
    ''' Returns a compatible VT escape color syntax for the set color levels. Put this in your VT escape syntax so it looks like this:
    ''' <br></br><see cref="GetEsc()"/>[38;2;<see cref="RGB.ToString()"/>mText (for foreground)
    ''' <br></br><see cref="GetEsc()"/>[48;2;<see cref="RGB.ToString()"/>mText (for background)
    ''' </summary>
    ''' <returns>A compatible VT escape color syntax</returns>
    Public Overrides Function ToString() As String
        Return $"{Red};{Green};{Blue}"
    End Function

End Class
