
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

Imports KS.Shell.Prompts.Presets.UESH
Imports KS.Shell.Prompts.Presets.Test
Imports KS.Shell.Prompts.Presets.ZIP
Imports KS.Shell.Prompts.Presets.Text
Imports KS.Shell.Prompts.Presets.SFTP
Imports KS.Shell.Prompts.Presets.RSS
Imports KS.Shell.Prompts.Presets.Mail
Imports KS.Shell.Prompts.Presets.Json
Imports KS.Shell.Prompts.Presets.HTTP
Imports KS.Shell.Prompts.Presets.Hex
Imports KS.Shell.Prompts.Presets.FTP
Imports KS.Shell.Prompts.Presets.RAR
Imports KS.Kernel.Exceptions

Namespace Shell.Prompts
    Public Module PromptPresetManager

        'Shell presets
        Friend ReadOnly UESHShellPresets As New Dictionary(Of String, PromptPresetBase) From {
            {"Default", New DefaultPreset()},
            {"PowerLine1", New PowerLine1Preset()}
        }
        Friend ReadOnly TestShellPresets As New Dictionary(Of String, PromptPresetBase) From {
            {"Default", New TestDefaultPreset()}
        }
        Friend ReadOnly ZipShellPresets As New Dictionary(Of String, PromptPresetBase) From {
            {"Default", New ZipDefaultPreset()}
        }
        Friend ReadOnly TextShellPresets As New Dictionary(Of String, PromptPresetBase) From {
            {"Default", New TextDefaultPreset()}
        }
        Friend ReadOnly SFTPShellPresets As New Dictionary(Of String, PromptPresetBase) From {
            {"Default", New SFTPDefaultPreset()}
        }
        Friend ReadOnly RSSShellPresets As New Dictionary(Of String, PromptPresetBase) From {
            {"Default", New RSSDefaultPreset()}
        }
        Friend ReadOnly MailShellPresets As New Dictionary(Of String, PromptPresetBase) From {
            {"Default", New MailDefaultPreset()}
        }
        Friend ReadOnly JsonShellPresets As New Dictionary(Of String, PromptPresetBase) From {
            {"Default", New JsonDefaultPreset()}
        }
        Friend ReadOnly HTTPShellPresets As New Dictionary(Of String, PromptPresetBase) From {
            {"Default", New HTTPDefaultPreset()}
        }
        Friend ReadOnly HexShellPresets As New Dictionary(Of String, PromptPresetBase) From {
            {"Default", New HexDefaultPreset()}
        }
        Friend ReadOnly FTPShellPresets As New Dictionary(Of String, PromptPresetBase) From {
            {"Default", New FTPDefaultPreset()}
        }
        Friend ReadOnly RARShellPresets As New Dictionary(Of String, PromptPresetBase) From {
            {"Default", New RarDefaultPreset()}
        }

        'Custom shell presets used by mods
        Friend ReadOnly UESHCustomShellPresets As New Dictionary(Of String, PromptPresetBase)
        Friend ReadOnly TestCustomShellPresets As New Dictionary(Of String, PromptPresetBase)
        Friend ReadOnly ZipCustomShellPresets As New Dictionary(Of String, PromptPresetBase)
        Friend ReadOnly TextCustomShellPresets As New Dictionary(Of String, PromptPresetBase)
        Friend ReadOnly SFTPCustomShellPresets As New Dictionary(Of String, PromptPresetBase)
        Friend ReadOnly RSSCustomShellPresets As New Dictionary(Of String, PromptPresetBase)
        Friend ReadOnly MailCustomShellPresets As New Dictionary(Of String, PromptPresetBase)
        Friend ReadOnly JsonCustomShellPresets As New Dictionary(Of String, PromptPresetBase)
        Friend ReadOnly HTTPCustomShellPresets As New Dictionary(Of String, PromptPresetBase)
        Friend ReadOnly HexCustomShellPresets As New Dictionary(Of String, PromptPresetBase)
        Friend ReadOnly FTPCustomShellPresets As New Dictionary(Of String, PromptPresetBase)
        Friend ReadOnly RARCustomShellPresets As New Dictionary(Of String, PromptPresetBase)

        'Current presets
        Friend UESHShellCurrentPreset As PromptPresetBase = UESHShellPresets("Default")
        Friend TestShellCurrentPreset As PromptPresetBase = TestShellPresets("Default")
        Friend ZipShellCurrentPreset As PromptPresetBase = ZipShellPresets("Default")
        Friend TextShellCurrentPreset As PromptPresetBase = TextShellPresets("Default")
        Friend SFTPShellCurrentPreset As PromptPresetBase = SFTPShellPresets("Default")
        Friend RSSShellCurrentPreset As PromptPresetBase = RSSShellPresets("Default")
        Friend MailShellCurrentPreset As PromptPresetBase = MailShellPresets("Default")
        Friend JsonShellCurrentPreset As PromptPresetBase = JsonShellPresets("Default")
        Friend HTTPShellCurrentPreset As PromptPresetBase = HTTPShellPresets("Default")
        Friend HexShellCurrentPreset As PromptPresetBase = HexShellPresets("Default")
        Friend FTPShellCurrentPreset As PromptPresetBase = FTPShellPresets("Default")
        Friend RARShellCurrentPreset As PromptPresetBase = FTPShellPresets("Default")

        ''' <summary>
        ''' Sets the shell preset
        ''' </summary>
        ''' <param name="PresetName">The preset name</param>
        Public Sub SetPreset(PresetName As String, ShellType As ShellType, Optional ThrowOnNotFound As Boolean = True)
            Dim Presets As Dictionary(Of String, PromptPresetBase) = GetPresetsFromShell(ShellType)
            Dim CustomPresets As Dictionary(Of String, PromptPresetBase) = GetCustomPresetsFromShell(ShellType)

            'Check to see if we have the preset
            If Presets.ContainsKey(PresetName) Then
                SetPresetInternal(PresetName, ShellType, Presets)
            ElseIf CustomPresets.ContainsKey(Presetname) Then
                SetPresetInternal(PresetName, ShellType, CustomPresets)
            Else
                If ThrowOnNotFound Then
                    Wdbg(DebugLevel.I, "Preset {0} for {1} doesn't exist. Throwing...", PresetName, ShellType.ToString())
                    Throw New NoSuchShellPresetException(DoTranslation("The specified preset {0} is not found."), PresetName)
                Else
                    SetPresetInternal("Default", ShellType, Presets)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Sets the preset
        ''' </summary>
        ''' <param name="PresetName">The preset name</param>
        ''' <param name="ShellType">The shell type</param>
        Friend Sub SetPresetInternal(PresetName As String, ShellType As ShellType, Presets As Dictionary(Of String, PromptPresetBase))
            Select Case ShellType
                Case ShellType.Shell
                    UESHShellCurrentPreset = Presets(PresetName)
                Case ShellType.TestShell
                    TestShellCurrentPreset = Presets(PresetName)
                Case ShellType.ZIPShell
                    ZipShellCurrentPreset = Presets(PresetName)
                Case ShellType.TextShell
                    TextShellCurrentPreset = Presets(PresetName)
                Case ShellType.SFTPShell
                    SFTPShellCurrentPreset = Presets(PresetName)
                Case ShellType.RSSShell
                    RSSShellCurrentPreset = Presets(PresetName)
                Case ShellType.MailShell
                    MailShellCurrentPreset = Presets(PresetName)
                Case ShellType.JsonShell
                    JsonShellCurrentPreset = Presets(PresetName)
                Case ShellType.HTTPShell
                    HTTPShellCurrentPreset = Presets(PresetName)
                Case ShellType.HexShell
                    HexShellCurrentPreset = Presets(PresetName)
                Case ShellType.FTPShell
                    FTPShellCurrentPreset = Presets(PresetName)
                Case ShellType.RARShell
                    RARShellCurrentPreset = Presets(PresetName)
            End Select
        End Sub

        ''' <summary>
        ''' Gets the current preset base from the shell
        ''' </summary>
        ''' <param name="ShellType">The shell type</param>
        Public Function GetCurrentPresetBaseFromShell(ShellType As ShellType) As PromptPresetBase
            Select Case ShellType
                Case ShellType.Shell
                    Return UESHShellCurrentPreset
                Case ShellType.TestShell
                    Return TestShellCurrentPreset
                Case ShellType.ZIPShell
                    Return ZipShellCurrentPreset
                Case ShellType.TextShell
                    Return TextShellCurrentPreset
                Case ShellType.SFTPShell
                    Return SFTPShellCurrentPreset
                Case ShellType.RSSShell
                    Return RSSShellCurrentPreset
                Case ShellType.MailShell
                    Return MailShellCurrentPreset
                Case ShellType.JsonShell
                    Return JsonShellCurrentPreset
                Case ShellType.HTTPShell
                    Return HTTPShellCurrentPreset
                Case ShellType.HexShell
                    Return HexShellCurrentPreset
                Case ShellType.FTPShell
                    Return FTPShellCurrentPreset
                Case ShellType.RARShell
                    Return RARShellCurrentPreset
            End Select
        End Function

        ''' <summary>
        ''' Gets the predefined presets from the shell
        ''' </summary>
        ''' <param name="ShellType">The shell type</param>
        Public Function GetPresetsFromShell(ShellType As ShellType) As Dictionary(Of String, PromptPresetBase)
            Select Case ShellType
                Case ShellType.Shell
                    Return UESHShellPresets
                Case ShellType.TestShell
                    Return TestShellPresets
                Case ShellType.ZIPShell
                    Return ZipShellPresets
                Case ShellType.TextShell
                    Return TextShellPresets
                Case ShellType.SFTPShell
                    Return SFTPShellPresets
                Case ShellType.RSSShell
                    Return RSSShellPresets
                Case ShellType.MailShell
                    Return MailShellPresets
                Case ShellType.JsonShell
                    Return JsonShellPresets
                Case ShellType.HTTPShell
                    Return HTTPShellPresets
                Case ShellType.HexShell
                    Return HexShellPresets
                Case ShellType.FTPShell
                    Return FTPShellPresets
                Case ShellType.RARShell
                    Return RARShellPresets
            End Select
        End Function

        ''' <summary>
        ''' Gets the custom presets (defined by mods) from the shell
        ''' </summary>
        ''' <param name="ShellType">The shell type</param>
        Public Function GetCustomPresetsFromShell(ShellType As ShellType) As Dictionary(Of String, PromptPresetBase)
            Select Case ShellType
                Case ShellType.Shell
                    Return UESHCustomShellPresets
                Case ShellType.TestShell
                    Return TestCustomShellPresets
                Case ShellType.ZIPShell
                    Return ZipCustomShellPresets
                Case ShellType.TextShell
                    Return TextCustomShellPresets
                Case ShellType.SFTPShell
                    Return SFTPCustomShellPresets
                Case ShellType.RSSShell
                    Return RSSCustomShellPresets
                Case ShellType.MailShell
                    Return MailCustomShellPresets
                Case ShellType.JsonShell
                    Return JsonCustomShellPresets
                Case ShellType.HTTPShell
                    Return HTTPCustomShellPresets
                Case ShellType.HexShell
                    Return HexCustomShellPresets
                Case ShellType.FTPShell
                    Return FTPCustomShellPresets
                Case ShellType.RARShell
                    Return RARCustomShellPresets
            End Select
        End Function

        ''' <summary>
        ''' Writes the shell prompt
        ''' </summary>
        ''' <param name="ShellType">Shell type</param>
        Public Sub WriteShellPrompt(ShellType As ShellType)
            Dim ShellPromptStyle As String = GetCustomShellPromptStyle(ShellType)
            If Not String.IsNullOrWhiteSpace(ShellPromptStyle) And Not Maintenance Then
                'Parse the shell prompt style
                Dim ParsedPromptStyle As String = ProbePlaces(ShellPromptStyle)
                ParsedPromptStyle.ConvertVTSequences()
                Write(ParsedPromptStyle, False, ColTypes.Input)
            Else
                Dim CurrentPresetBase As PromptPresetBase = GetCurrentPresetBaseFromShell(ShellType)
                Write(CurrentPresetBase.PresetPrompt, False, ColTypes.Input)
            End If

            'Set input color in case custom preset or custom shell prompt style didn't set the input color as instructed
            SetInputColor()
        End Sub

    End Module
End Namespace
