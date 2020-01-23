
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Imports NAudio.Wave

Public Module VoiceManagement

    Public Sub Speak(ByVal Text As String)
        Dim SpeakReq As New WebClient
        SpeakReq.Headers.Add("Content-Type", "audio/mpeg")
        SpeakReq.Headers.Add("User-Agent", "KS on (" + EnvironmentOSType + ")")
        Wdbg("I", "Headers required: {0}", SpeakReq.Headers.Count)
        SpeakReq.DownloadFile("http://translate.google.com/translate_tts?tl=en&q=" + Text + "&client=gtx", paths("Temp") + "/tts.mpeg")
        Dim AudioRead As New AudioFileReader(paths("Temp") + "/tts.mpeg")
        Wdbg("I", "AudioRead: {0}", AudioRead.TotalTime)
        Dim WaveEvent As New WaveOutEvent()
        WaveEvent.Init(AudioRead)
        Wdbg("I", "Initialized Wave Out using {0}", WaveEvent.DeviceNumber)
        WaveEvent.Play()
        While WaveEvent.PlaybackState = PlaybackState.Playing
        End While
        WaveEvent.Dispose()
        AudioRead.Close()
        IO.File.Delete(paths("Temp") + "/tts.mpeg")
    End Sub

End Module
