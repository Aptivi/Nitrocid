
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
Imports LibVLCSharp.Shared

Public Module VoiceManagement

    Public Sub Speak(ByVal Text As String)
        'Download required file
        Dim SpeakReq As New WebClient
        SpeakReq.Headers.Add("Content-Type", "audio/mpeg")
        SpeakReq.Headers.Add("User-Agent", "KS on (" + EnvironmentOSType + ")")
        Wdbg("I", "Headers required: {0}", SpeakReq.Headers.Count)
        SpeakReq.DownloadFile("http://translate.google.com/translate_tts?tl=en&q=" + Text + "&client=gtx", paths("Temp") + "/tts.mpeg")

        'Platform checks, because NAudio is Windows only (mfplat.dll, Windows APIs, etc.). See https://github.com/naudio/NAudio/issues/184
        'TODO: Remove NAudio in favor of VLC.
        If EnvironmentOSType.Contains("Windows") Then
            'Read the file
            Dim AudioRead As New AudioFileReader(paths("Temp") + "/tts.mpeg")
            Wdbg("I", "AudioRead: {0}", AudioRead.TotalTime)

            'Initialize Wave Out
            Dim WaveEvent As New WaveOutEvent()
            WaveEvent.Init(AudioRead)
            Wdbg("I", "Initialized Wave Out using {0}", WaveEvent.DeviceNumber)

            'Play the speech
            WaveEvent.Play()
            While WaveEvent.PlaybackState = PlaybackState.Playing
            End While

            'Dispose and close objects
            Wdbg("I", "Stopped.")
            WaveEvent.Dispose()
            AudioRead.Close()
        Else
            'Use VLC for playback. Requires installing packages as specified in https://github.com/videolan/libvlcsharp/blob/3.x/docs/linux-setup.md
            Core.Initialize()
            Dim MLib As New LibVLC
            Dim MP As New MediaPlayer(MLib)
            AddHandler MLib.Log, Sub(sender, e) W($"{e.Level}@{e.Module}: {e.Message}", True, ColTypes.Neutral)
            Dim MFile As New Media(MLib, paths("Temp") + "/tts.mpeg")
            MP.Media = MFile
            MP.Play()
            While MP.State = VLCState.Playing
            End While

            'Dispose and close objects
            Wdbg("I", "Stopped.")
            MFile.Dispose()
            MP.Dispose()
        End If

        'Remove the file
        IO.File.Delete(paths("Temp") + "/tts.mpeg")
    End Sub

End Module
