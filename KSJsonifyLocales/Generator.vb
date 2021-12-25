
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
Imports KS.TextWriterColor
Imports KS.ColorTools
Imports KS.PlatformDetector
Imports KS.KernelTools

Module LocaleGenerator

    ''' <summary>
    ''' Entry point
    ''' </summary>
    Sub Main(Args As String())
        'Check for terminal (macOS only). Go to Kernel.vb on Kernel Simulator for more info.
#If STOCKTERMINALMACOS = False Then
        If IsOnMacOS() Then
            If GetTerminalEmulator() = "Apple_Terminal" Then
                Console.WriteLine("Kernel Simulator makes use of VT escape sequences, but Terminal.app has broken support for 255 and true colors. This program can't continue.")
                Environment.Exit(5)
            End If
        End If
#End If

        'Parse for arguments
        Dim Custom As Boolean = True
        Dim Normal As Boolean = True
        Dim CopyToResources As Boolean
        If Args.Length > 0 Then
            'Change the values of custom and normal to match the switches provided
            Custom = Args.Contains("--CustomOnly") Or Args.Contains("--All")
            Normal = Args.Contains("--NormalOnly") Or Args.Contains("--All")
            CopyToResources = Args.Contains("--CopyToResources")
        End If

        Dim Total As New Stopwatch
        Total.Start()
        Try
            'Enumerate the translations folder
            Dim ToParse As New List(Of TargetLanguage)
            Dim EnglishFile As String = Path.GetFullPath("Translations/eng.txt")
            Dim Files() As String = Directory.EnumerateFiles("Translations").ToArray
            Dim CustomFiles() As String = Directory.EnumerateFiles("CustomLanguages").ToArray
            Dim MetadataPath As String = Path.GetFullPath("Translations/Metadata.json")
            Dim CustomMetadataPath As String = Path.GetFullPath("CustomLanguages/Metadata.json")
            Dim Metadata As JToken = JObject.Parse(File.ReadAllText(MetadataPath))
            Dim CustomMetadata As JToken = JObject.Parse(File.ReadAllText(CustomMetadataPath))

            'Add languages to parse list
            If Normal Then
                For Each File As String In Files
                    Dim FileName As String = Path.GetFileNameWithoutExtension(File)
                    Dim FileExtension As String = Path.GetExtension(File)

                    'Check the file and add if the localization file is a text file
                    If FileExtension = ".txt" Then
                        Dim LanguageInstance As New TargetLanguage(File, FileName, False)
                        Debug.WriteLine(File)
                        ToParse.Add(LanguageInstance)
                    End If
                Next
            End If

            'Add the custom languages
            If Custom Then
                For Each File As String In CustomFiles
                    Dim FileName As String = Path.GetFileNameWithoutExtension(File)
                    Dim FileExtension As String = Path.GetExtension(File)

                    'Check the file and add if not in KS resources, not a Readme, and is a text file
                    If FileExtension = ".txt" And Not FileName.ToLower = "readme" And Not KS.Languages.ContainsKey(FileName) Then
                        Dim LanguageInstance As New TargetLanguage(File, FileName, True)
                        Debug.WriteLine(File)
                        ToParse.Add(LanguageInstance)
                    End If
                Next
            End If

            'Make a localized JSON file for target languages
            Dim FileNumber As Integer = 1
            Dim FileLinesEng() As String = File.ReadAllLines(EnglishFile)
            For Each Language As TargetLanguage In ToParse
                'Initialize the stopwatch and the counter
                Dim GenerationInterval As New Stopwatch
                GenerationInterval.Start()

                'Initialize two arrays for localization
                Dim File As String = Language.FileName
                Dim FileName As String = Language.LanguageName
                Dim FileLines() As String = IO.File.ReadAllLines(File)

                'Show the generation message
                Debug.WriteLine("Lines for {0} (Eng: {1}, Loc: {2})", FileName, FileLinesEng.Length, FileLines.Length)
                Write($"[{FileNumber}/{ToParse.Count}] " + "Generating locale JSON for " + $"{FileName}...", True, ColTypes.Progress)

                'Make a JSON object for each language entry
                Dim LocalizedJson As New JObject
                Dim LocalizationDataJson As New JObject
                For i As Integer = 0 To FileLines.Length - 1
                    If Not String.IsNullOrWhiteSpace(FileLines(i)) And Not String.IsNullOrWhiteSpace(FileLinesEng(i)) Then
                        Try
                            Debug.WriteLine("Adding ""{0}, {1}""...", FileLinesEng(i), FileLines(i))
                            LocalizationDataJson.Add(FileLinesEng(i), FileLines(i))
                        Catch ex As Exception
                            Write($"[{FileNumber}/{ToParse.Count}] " + "Malformed line" + $" {i + 1}: {FileLinesEng(i)} -> {FileLines(i)}", True, ColTypes.Error)
                            Write($"[{FileNumber}/{ToParse.Count}] " + "Error trying to parse above line:" + $" {ex.Message}", True, ColTypes.Error)
                        End Try
                    End If
                Next

                'Fetch the metadata and put their values
                Dim LanguageMetadata As JToken = If(Language.CustomLanguage, CustomMetadata, Metadata).SelectToken(FileName)
                Dim LanguageName As JToken = LanguageMetadata.SelectToken("name")
                Dim LanguageTransliterable As JToken = LanguageMetadata.SelectToken("transliterable")
                LocalizedJson.Add("Name", LanguageName)
                LocalizedJson.Add("Transliterable", LanguageTransliterable)
                LocalizedJson.Add("Localizations", LocalizationDataJson)

                'Serialize the JSON object
                Dim SerializedLocale As String = JsonConvert.SerializeObject(LocalizedJson, Formatting.Indented)

                'Save changes
                Debug.WriteLine("Saving as {0}...", FileName + ".json")
                If Language.CustomLanguage Then
                    Directory.CreateDirectory(KS.HomePath + "/KSLanguages/")
                    IO.File.WriteAllText(KS.HomePath + "/KSLanguages/" + FileName + ".json", SerializedLocale)
                Else
                    If Args.Length > 0 AndAlso Args(0) = "--CopyToResources" Then
                        IO.File.WriteAllText("../Resources/" + FileName + ".json", SerializedLocale)
                    Else
                        Directory.CreateDirectory("Translations/Output")
                        IO.File.WriteAllText("Translations/Output/" + FileName + ".json", SerializedLocale)
                    End If
                End If
                Write($"[{FileNumber}/{ToParse.Count}] " + "Saved new language JSON file to" + $" {FileName}.json!", True, ColTypes.Success)

                'Show elapsed time and reset
                Write($"[{FileNumber}/{ToParse.Count}] " + "Time elapsed:" + $" {GenerationInterval.Elapsed}", True, ColTypes.StageTime)
                FileNumber += 1
                GenerationInterval.Restart()
            Next
        Catch ex As Exception
            Write("Unexpected error in converter:" + $" {ex.Message}", True, ColTypes.Error)
            Write(ex.StackTrace, True, ColTypes.Error)
        End Try

        'Finish the program
        Write("Finished in " + $"{Total.Elapsed}", True, ColTypes.Neutral)
        Write("Press any key to continue...", True, ColTypes.Neutral)
        Total.Reset()
        Console.ReadKey()
    End Sub

End Module
