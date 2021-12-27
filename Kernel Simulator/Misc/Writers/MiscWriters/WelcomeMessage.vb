
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

Public Module WelcomeMessage

    ''' <summary>
    ''' The customized message banner to write. If none is specified, or if it only consists of whitespace, it uses the default message.
    ''' </summary>
    Public CustomBanner As String = ""

    ''' <summary>
    ''' Gets the custom banner actual text with placeholders parsed
    ''' </summary>
    Function GetCustomBanner() As String
        'The default message to write
        Dim MessageWrite As String = "      >> " + DoTranslation("Welcome to the kernel! - Version {0}") + " <<      "

        'Check to see if user specified custom message
        If Not String.IsNullOrWhiteSpace(CustomBanner) Then
            MessageWrite = CustomBanner
            MessageWrite = ProbePlaces(MessageWrite)
        End If

        'Just return the result
        Return MessageWrite
    End Function

    ''' <summary>
    ''' Writes the welcoming message to the console (welcome to kernel)
    ''' </summary>
    Sub WriteMessage()
        'The default message to write
        Dim MessageWrite As String = GetCustomBanner()

        'Finally, write the message
        If StartScroll Then
            WriteSlowly(MessageWrite, True, 10, ColTypes.Banner, KernelVersion)
        Else
            Write(MessageWrite, True, ColTypes.Banner, KernelVersion)
        End If

        If NewWelcomeStyle Then
            Write(vbNewLine + vbNewLine + Figgle.FiggleFonts.Banner.Render($"KS v{KernelVersion}"), True, ColTypes.Neutral)
        Else
            'Show license
            WriteLicense()
        End If
    End Sub

    ''' <summary>
    ''' Writes the license
    ''' </summary>
    Sub WriteLicense()
        Write(vbNewLine + "    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE" + vbNewLine +
                      "    This program comes with ABSOLUTELY NO WARRANTY, not even " + vbNewLine +
                      "    MERCHANTABILITY or FITNESS for particular purposes." + vbNewLine +
                      "    This is free software, and you are welcome to redistribute it" + vbNewLine +
                      "    under certain conditions; See COPYING file in source code." + vbNewLine, True, ColTypes.License)
        Write("* " + DoTranslation("For more information about the terms and conditions of using this software, visit") + " http://www.gnu.org/licenses/" + vbNewLine, True, ColTypes.License)
    End Sub

End Module
