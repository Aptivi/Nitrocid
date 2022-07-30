
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

Imports Figgle
Imports KS.Misc.Reflection

Namespace Misc.Writers.FancyWriters.Tools
    Public Module FigletTools

        ''' <summary>
        ''' The figlet fonts dictionary. It lists all the Figlet fonts supported by the Figgle library.
        ''' </summary>
        Public ReadOnly FigletFonts As Dictionary(Of String, Object) = GetProperties(GetType(FiggleFonts))

        ''' <summary>
        ''' Gets the figlet text height
        ''' </summary>
        ''' <param name="Text">Text</param>
        ''' <param name="FigletFont">Target figlet font</param>
        Public Function GetFigletHeight(Text As String, FigletFont As FiggleFont) As Integer
            Text = FigletFont.Render(Text)
            Dim TextLines As String() = Text.SplitNewLines
            Return TextLines.Length
        End Function

        ''' <summary>
        ''' Gets the figlet text width
        ''' </summary>
        ''' <param name="Text">Text</param>
        ''' <param name="FigletFont">Target figlet font</param>
        Public Function GetFigletWidth(Text As String, FigletFont As FiggleFont) As Integer
            Text = FigletFont.Render(Text)
            Dim TextLines As String() = Text.SplitNewLines
            Return TextLines(0).Length
        End Function

        ''' <summary>
        ''' Gets the figlet font from font name
        ''' </summary>
        ''' <param name="FontName">Font name that is supported by the Figgle library. Consult <see cref="FigletFonts"/> for more info.</param>
        ''' <returns>Figlet font instance of your font, or Small if not found</returns>
        Public Function GetFigletFont(FontName As String) As FiggleFont
            If FigletFonts.ContainsKey(FontName) Then
                Return FigletFonts(FontName)
            Else
                Return FiggleFonts.Small
            End If
        End Function

    End Module
End Namespace
