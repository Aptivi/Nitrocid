
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

Imports Newtonsoft.Json.Linq
Imports System.IO

Namespace Misc.Beautifiers
    Public Module JsonBeautifier

        ''' <summary>
        ''' Beautifies the JSON text contained in the file.
        ''' </summary>
        ''' <param name="JsonFile">Path to JSON file. It's automatically neutralized using <see cref="NeutralizePath(String, Boolean)"/>.</param>
        ''' <returns>Beautified JSON</returns>
        ''' <exception cref="FileNotFoundException"></exception>
        Public Function BeautifyJson(JsonFile As String) As String
            'Neutralize the file path
            Wdbg(DebugLevel.I, "Neutralizing json file {0}...", JsonFile)
            JsonFile = NeutralizePath(JsonFile, True)
            Wdbg(DebugLevel.I, "Got json file {0}...", JsonFile)

            'Try to beautify JSON
            Dim JsonFileContents As String = File.ReadAllText(JsonFile)
            Return BeautifyJsonText(JsonFileContents)
        End Function

        ''' <summary>
        ''' Beautifies the JSON text.
        ''' </summary>
        ''' <param name="JsonText">Contents of a minified JSON.</param>
        ''' <returns>Beautified JSON</returns>
        Public Function BeautifyJsonText(JsonText As String) As String
            'Make an instance of JToken with this text
            Dim JsonToken As JToken = JToken.Parse(JsonText)
            Wdbg(DebugLevel.I, "Created a token with text length of {0}", JsonText.Length)

            'Beautify JSON
            Dim BeautifiedJson As String = JsonConvert.SerializeObject(JsonToken, Formatting.Indented)
            Wdbg(DebugLevel.I, "Beautified the JSON text. Length: {0}", BeautifiedJson.Length)
            Return BeautifiedJson
        End Function

    End Module
End Namespace