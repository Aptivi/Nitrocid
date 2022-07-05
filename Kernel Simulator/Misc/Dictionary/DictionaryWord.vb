
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

Namespace Misc.Dictionary
    Public Class DictionaryWord

        Public Class Definition
            <JsonProperty("definition")>
            Public Property Definition() As String

            <JsonProperty("synonyms")>
            Public Property Synonyms() As List(Of String)

            <JsonProperty("antonyms")>
            Public Property Antonyms() As List(Of Object)

            <JsonProperty("example")>
            Public Property Example() As String
        End Class

        Public Class License
            <JsonProperty("name")>
            Public Property Name() As String

            <JsonProperty("url")>
            Public Property Url() As String
        End Class

        Public Class Meaning
            <JsonProperty("partOfSpeech")>
            Public Property PartOfSpeech() As String

            <JsonProperty("definitions")>
            Public Property Definitions() As List(Of Definition)

            <JsonProperty("synonyms")>
            Public Property Synonyms() As List(Of String)

            <JsonProperty("antonyms")>
            Public Property Antonyms() As List(Of String)
        End Class

        Public Class Phonetic
            <JsonProperty("text")>
            Public Property Text() As String

            <JsonProperty("audio")>
            Public Property Audio() As String

            <JsonProperty("sourceUrl")>
            Public Property SourceUrl() As String

            <JsonProperty("license")>
            Public Property License() As License
        End Class

        <JsonProperty("word")>
        Public Property Word() As String

        <JsonProperty("phonetic")>
        Public Property PhoneticWord() As String

        <JsonProperty("phonetics")>
        Public Property Phonetics() As List(Of Phonetic)

        <JsonProperty("meanings")>
        Public Property Meanings() As List(Of Meaning)

        <JsonProperty("license")>
        Public Property LicenseInfo() As License

        <JsonProperty("sourceUrls")>
        Public Property SourceUrls() As List(Of String)

    End Class
End Namespace