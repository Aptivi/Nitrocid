
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

Imports Dictify.Manager.DictionaryManager
Imports Dictify.Models

Namespace Shell.Shells.UESH.Commands
    Class DictCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Words As DictionaryWord() = GetWordInfo(ListArgsOnly(0))

            'Iterate for each word
            For Each Word As DictionaryWord In Words
                'First, print the license out
                WriteSeparator(DoTranslation("License information"), True)
                Write("dictionaryapi.dev " + DoTranslation("API is licensed under") + $" {Word.LicenseInfo.Name}: {Word.LicenseInfo.Url}", True, ColTypes.Neutral)

                'Now, we can write the word information
                WriteSeparator(DoTranslation("Word information for") + $" {ListArgs(0)}", True)
                Write(DoTranslation("Word:"), False, ColTypes.ListEntry) : Write($" {Word.Word}", True, ColTypes.ListValue)

                'Meanings...
                WriteSeparator(DoTranslation("Word meanings for") + $" {ListArgs(0)}", True)
                For Each MeaningBase As DictionaryWord.Meaning In Word.Meanings
                    'Base part of speech
                    Write(DoTranslation("Part of Speech:"), False, ColTypes.ListEntry) : Write($" {MeaningBase.PartOfSpeech}", True, ColTypes.ListValue)

                    'Get the definitions
                    For Each DefinitionBase As DictionaryWord.DefinitionType In MeaningBase.Definitions
                        'Write definition and, if applicable, example
                        Write("  - " + DoTranslation("Definition:"), False, ColTypes.ListEntry) : Write($" {DefinitionBase.Definition}", True, ColTypes.ListValue)
                        Write("  - " + DoTranslation("Example in Sentence:"), False, ColTypes.ListEntry) : Write($" {DefinitionBase.Example}", True, ColTypes.ListValue)

                        'Now, write the specific synonyms (usually blank)
                        If DefinitionBase.Synonyms.Any Then
                            Write("  - " + DoTranslation("Synonyms:"), True, ColTypes.ListEntry)
                            WriteList(DefinitionBase.Synonyms)
                        End If

                        '...and the specific antonyms (usually blank)
                        If DefinitionBase.Antonyms.Any Then
                            Write("  - " + DoTranslation("Antonyms:"), True, ColTypes.ListEntry)
                            WriteList(DefinitionBase.Antonyms)
                        End If
                    Next

                    'Now, write the base synonyms (usually blank)
                    If MeaningBase.Synonyms.Any Then
                        Write("  - " + DoTranslation("Synonyms:"), True, ColTypes.ListEntry)
                        WriteList(MeaningBase.Synonyms)
                    End If

                    '...and the base antonyms (usually blank)
                    If MeaningBase.Antonyms.Any Then
                        Write("  - " + DoTranslation("Antonyms:"), True, ColTypes.ListEntry)
                        WriteList(MeaningBase.Antonyms)
                    End If
                Next

                'Sources...
                WriteSeparator(DoTranslation("Sources used to define") + $" {ListArgs(0)}", True)
                WriteList(Word.SourceUrls)
            Next
        End Sub

    End Class
End Namespace
