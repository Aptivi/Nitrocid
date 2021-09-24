
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

Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module LocaleGenerator

    ''' <summary>
    ''' Entry point
    ''' </summary>
    Sub Main(Args As String())
        Try
            'Enumerate the translations folder
            Dim ToParse As New List(Of String)
            Dim EnglishFile As String = "eng.txt"
            Dim Files = Directory.EnumerateFiles("Translations")

            'Add languages to parse list
            For Each File As String In Files
                If File.EndsWith(".txt") Then
                    Debug.WriteLine(File)
                    ToParse.Add(File)
                End If
                If File.Contains("eng.txt") Then
                    Debug.WriteLine("English file: " + File)
                    EnglishFile = File
                End If
            Next
            ToParse.Sort()

            'Make a localized JSON file for target languages
            For Each File As String In ToParse
                'Initialize two arrays for localization
                Dim FileLines() As String = IO.File.ReadAllLines(File)
                Dim FileLinesEng() As String = IO.File.ReadAllLines(EnglishFile)
                Debug.WriteLine("Lines (Eng: {0}, Loc: {1})", FileLinesEng.Length, FileLines.Length)

                'Make a JSON object for each language entry
                Dim LocalizedJson As New JObject
                For i As Integer = 0 To FileLines.Length - 1
                    If Not String.IsNullOrWhiteSpace(FileLines(i)) And Not String.IsNullOrWhiteSpace(FileLinesEng(i)) Then
                        Try
                            Debug.WriteLine("Adding ""{0}, {1}""...", FileLinesEng(i), FileLines(i))
                            LocalizedJson.Add(FileLinesEng(i), FileLines(i))
                        Catch ex As Exception
                            Console.WriteLine(ex.Message)
                            Console.WriteLine($"Malformed line {i + 1}: {FileLinesEng(i)} -> {FileLines(i)}")
                        End Try
                    End If
                Next

                'Save changes
                If Args.Length > 0 AndAlso Args(0) = "--CopyToResources" Then
                    IO.File.WriteAllText("../Resources/" + Path.GetFileNameWithoutExtension(File) + ".json", JsonConvert.SerializeObject(LocalizedJson, Formatting.Indented))
                    Console.WriteLine($"Saved to ../Resources/{Path.GetFileNameWithoutExtension(File)}.json!")
                Else
                    Directory.CreateDirectory("Translations/Output")
                    IO.File.WriteAllText("Translations/Output/" + Path.GetFileNameWithoutExtension(File) + ".json", JsonConvert.SerializeObject(LocalizedJson, Formatting.Indented))
                    Console.WriteLine($"Saved to Translations/Output/{Path.GetFileNameWithoutExtension(File)}.json!")
                End If
            Next
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Console.WriteLine(ex.StackTrace)
        End Try

        'Finish the program
        Console.WriteLine("Press any key to continue...")
        Console.ReadKey()
    End Sub

End Module
