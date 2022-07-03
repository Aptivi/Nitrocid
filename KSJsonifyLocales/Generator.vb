
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

Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports KS.ConsoleBase
Imports KS.Files
Imports KS.Languages
Imports KS.Misc.Writers.ConsoleWriters

Module LocaleGenerator

    ''' <summary>
    ''' Entry point
    ''' </summary>
    Sub Main(Args As String())
        'Check console
        CheckConsole()

        'Parse for arguments
        Dim Arguments As New List(Of String)
        Dim Switches As New List(Of String)
        Dim Custom As Boolean = True
        Dim Normal As Boolean = True
        Dim CopyToResources As Boolean
        Dim Singular As Boolean
        Dim Quiet As Boolean
        Dim ToSearch As String = ""
        If Args.Length > 0 Then
            'Separate between switches and arguments
            For Each Arg As String In Args
                If Arg.StartsWith("--") Then
                    'It's a switch.
                    Switches.Add(Arg)
                Else
                    'It's an argument.
                    Arguments.Add(Arg)
                End If
            Next

            'Change the values of custom and normal to match the switches provided
            Custom = Switches.Contains("--CustomOnly") Or Switches.Contains("--All")
            Normal = Switches.Contains("--NormalOnly") Or Switches.Contains("--All")
            CopyToResources = Switches.Contains("--CopyToResources")
            Quiet = Switches.Contains("--Quiet")

            'Check to see if we're going to parse one language
            Singular = Switches.Contains("--Singular")
            If Singular And Arguments.Count > 0 Then
                'Select the language to be searched
                ToSearch = Arguments(0)
            ElseIf Singular Then
                'We can't be singular without providing the language!
                Console.WriteLine("Provide a language to generate.")
                Environment.Exit(1)
            End If

            'Check to see if we're going to show help
            If Switches.Contains("--Help") Then
                Console.WriteLine("{0} [--CustomOnly|--NormalOnly|--All|--Singular|--CopyToResources|--Help]", Path.GetFileName(Environment.GetCommandLineArgs(0)))
                Environment.Exit(1)
            End If
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
                        If Not Singular Or (Singular And FileName = ToSearch) Then
                            Dim LanguageInstance As New TargetLanguage(File, FileName, False)
                            Debug.WriteLine(File)
                            ToParse.Add(LanguageInstance)
                        End If
                    End If
                Next
            End If

            'Add the custom languages
            If Custom Then
                For Each File As String In CustomFiles
                    Dim FileName As String = Path.GetFileNameWithoutExtension(File)
                    Dim FileExtension As String = Path.GetExtension(File)

                    'Check the file and add if not in KS resources, not a Readme, and is a text file
                    If FileExtension = ".txt" And Not FileName.ToLower = "readme" And Not Languages.ContainsKey(FileName) Then
                        If Not Singular Or (Singular And FileName = ToSearch) Then
                            Dim LanguageInstance As New TargetLanguage(File, FileName, True)
                            Debug.WriteLine(File)
                            ToParse.Add(LanguageInstance)
                        End If
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
                If Not Quiet Then Write($"[{FileNumber}/{ToParse.Count}] " + "Generating locale JSON for " + $"{FileName}...", True, ColTypes.Progress)

                'Make a JSON object for each language entry
                Dim LocalizedJson As New JObject
                Dim LocalizationDataJson As New JObject
                For i As Integer = 0 To FileLines.Length - 1
                    If Not String.IsNullOrWhiteSpace(FileLines(i)) And Not String.IsNullOrWhiteSpace(FileLinesEng(i)) Then
                        Try
                            Debug.WriteLine("Adding ""{0}, {1}""...", FileLinesEng(i), FileLines(i))
                            LocalizationDataJson.Add(FileLinesEng(i), FileLines(i))
                        Catch ex As Exception
                            If Not Quiet Then Write($"[{FileNumber}/{ToParse.Count}] " + "Malformed line" + $" {i + 1}: {FileLinesEng(i)} -> {FileLines(i)}", True, ColTypes.Error)
                            If Not Quiet Then Write($"[{FileNumber}/{ToParse.Count}] " + "Error trying to parse above line:" + $" {ex.Message}", True, ColTypes.Error)
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
                    Directory.CreateDirectory(HomePath + "/KSLanguages/")
                    IO.File.WriteAllText(HomePath + "/KSLanguages/" + FileName + ".json", SerializedLocale)
                Else
                    If CopyToResources Then
#If EXECUTABLEINFRAMEWORKFOLDER Then
                        IO.File.WriteAllText("../../Resources/" + FileName + ".json", SerializedLocale)
#Else
                        IO.File.WriteAllText("../Resources/" + FileName + ".json", SerializedLocale)
#End If
                    Else
                        Directory.CreateDirectory("Translations/Output")
                        IO.File.WriteAllText("Translations/Output/" + FileName + ".json", SerializedLocale)
                    End If
                End If
                If Not Quiet Then Write($"[{FileNumber}/{ToParse.Count}] " + "Saved new language JSON file to" + $" {FileName}.json!", True, ColTypes.Success)

                'Show elapsed time and reset
                If Not Quiet Then Write($"[{FileNumber}/{ToParse.Count}] " + "Time elapsed:" + $" {GenerationInterval.Elapsed}", True, ColTypes.StageTime)
                FileNumber += 1
                GenerationInterval.Restart()
            Next
        Catch ex As Exception
            If Not Quiet Then Write("Unexpected error in converter:" + $" {ex.Message}", True, ColTypes.Error)
            If Not Quiet Then Write(ex.StackTrace, True, ColTypes.Error)
        End Try

        'Finish the program
        If Not Quiet Then Write("Finished in " + $"{Total.Elapsed}", True, ColTypes.Neutral)
        Total.Reset()
    End Sub

End Module
