
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

Imports System.Text
Imports KS.Network.FTP

Namespace Shell.Prompts.Presets.FTP
    Public Class FtpPowerLineBG2Preset
        Inherits PromptPresetBase
        Implements IPromptPreset

        Public Overrides ReadOnly Property PresetName As String = "PowerLineBG2" Implements IPromptPreset.PresetName

        Public Overrides ReadOnly Property PresetPrompt As String Implements IPromptPreset.PresetPrompt
            Get
                Return PresetPromptBuilder()
            End Get
        End Property

        Public Overrides ReadOnly Property PresetShellType As ShellType = ShellType.FTPShell Implements IPromptPreset.PresetShellType

        Friend Overrides Function PresetPromptBuilder() As String Implements IPromptPreset.PresetPromptBuilder
            'PowerLine glyphs
            Dim TransitionChar As Char = Convert.ToChar(&HE0B0)
            Dim TransitionPartChar As Char = Convert.ToChar(&HE0B1)
            Dim PadlockChar As Char = Convert.ToChar(&HE0A2)

            'PowerLine preset colors
            Dim FirstColorSegmentForeground As New Color(255, 85, 255)
            Dim FirstColorSegmentBackground As New Color(25, 25, 25)
            Dim SecondColorSegmentForeground As New Color(255, 85, 255)
            Dim SecondColorSegmentBackground As New Color(25, 25, 25)
            Dim ThirdColorSegmentForeground As New Color(255, 85, 255)
            Dim ThirdColorSegmentBackground As New Color(25, 25, 25)
            Dim LastTransitionForeground As New Color(25, 25, 25)

            'Builder
            Dim PresetStringBuilder As New StringBuilder

            'Build the preset
            If FtpConnected Then
                'Current username
                PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground)
                PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground)
                PresetStringBuilder.AppendFormat(" {0} ", FtpUser)

                'Transition
                PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground)
                PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground)
                PresetStringBuilder.AppendFormat("{0}", TransitionPartChar)

                'Current hostname
                PresetStringBuilder.Append(SecondColorSegmentForeground.VTSequenceForeground)
                PresetStringBuilder.Append(SecondColorSegmentBackground.VTSequenceBackground)
                PresetStringBuilder.AppendFormat(" {0} {1} ", PadlockChar, FtpSite)

                'Transition
                PresetStringBuilder.Append(SecondColorSegmentForeground.VTSequenceForeground)
                PresetStringBuilder.Append(ThirdColorSegmentBackground.VTSequenceBackground)
                PresetStringBuilder.AppendFormat("{0}", TransitionPartChar)

                'Current directory
                PresetStringBuilder.Append(ThirdColorSegmentForeground.VTSequenceForeground)
                PresetStringBuilder.Append(ThirdColorSegmentBackground.VTSequenceBackground)
                PresetStringBuilder.AppendFormat(" {0} ", FtpCurrentRemoteDir)

                'Transition
                PresetStringBuilder.Append(LastTransitionForeground.VTSequenceForeground)
                PresetStringBuilder.Append(BackgroundColor.VTSequenceBackground)
                PresetStringBuilder.AppendFormat("{0} ", TransitionChar)
            Else
                'FTP current directory
                PresetStringBuilder.Append(FirstColorSegmentForeground.VTSequenceForeground)
                PresetStringBuilder.Append(FirstColorSegmentBackground.VTSequenceBackground)
                PresetStringBuilder.AppendFormat(" {0} ", FtpCurrentDirectory)

                'Transition
                PresetStringBuilder.Append(LastTransitionForeground.VTSequenceForeground)
                PresetStringBuilder.Append(BackgroundColor.VTSequenceBackground)
                PresetStringBuilder.AppendFormat("{0} ", TransitionChar)
            End If

            'Present final string
            Return PresetStringBuilder.ToString()
        End Function

    End Class
End Namespace
