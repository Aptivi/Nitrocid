
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Imports System.Speech.Synthesis
Imports System.Collections.ObjectModel

Public Module VoiceManagement

    Public CurrentVoice As InstalledVoice
    Public VoiceSynth As New SpeechSynthesizer
    Public Function GetVoices() As List(Of InstalledVoice)
        Dim Voices As ReadOnlyCollection(Of InstalledVoice) = VoiceSynth.GetInstalledVoices
        Wdbg("{0} voices", Voices.Count)
        Return Voices.ToList
    End Function
    Public Sub SetVoice(ByVal Voice As String)
        Dim Done As Boolean
        For Each IVoice As InstalledVoice In GetVoices()
            Wdbg("Parsing voice {0}, expecting {1}", IVoice.VoiceInfo.Name, Voice)
            If IVoice.VoiceInfo.Name = Voice Then
                Done = True
                VoiceSynth.SelectVoice(Voice)
                Wdbg("Voice selected")
                W(DoTranslation("Voice set", currentLang), True, ColTypes.Neutral)
                Exit For
            End If
        Next
        If Not Done Then
            Wdbg("Voice not found")
            W(DoTranslation("Invalid voice.", currentLang), True, ColTypes.Neutral)
        End If
    End Sub

End Module
