
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

Imports Newtonsoft.Json.Linq

Namespace ConsoleBase.Colors
    Public Class ConsoleColorsInfo

        ''' <summary>
        ''' The color ID
        ''' </summary>
        Public ReadOnly Property ColorID As Integer
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
        ''' Is the color bright?
        ''' </summary>
        Public ReadOnly Property IsBright As Boolean
        ''' <summary>
        ''' Is the color dark?
        ''' </summary>
        Public ReadOnly Property IsDark As Boolean

        ''' <summary>
        ''' Makes a new instance of 255-color console color information
        ''' </summary>
        ''' <param name="ColorValue">A 255-color console color</param>
        Public Sub New(ColorValue As ConsoleColors)
            If Not (ColorValue < 0 Or ColorValue > 255) Then
                Dim ColorData As JObject = ColorDataJson(CInt(ColorValue))
                ColorID = ColorData("colorId")
                R = ColorData("rgb")("r")
                G = ColorData("rgb")("g")
                B = ColorData("rgb")("b")
                IsBright = R + 0.2126 + G + 0.7152 + B + 0.0722 > 255 / 2
                IsDark = R + 0.2126 + G + 0.7152 + B + 0.0722 < 255 / 2
            Else
                Throw New ArgumentOutOfRangeException(NameOf(ColorValue), ColorValue, DoTranslation("The color value is outside the range of 0-255."))
            End If
        End Sub

    End Class
End Namespace