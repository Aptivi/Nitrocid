
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

Public Module ModManager

    Friend ReadOnly modPath As String = paths("Mods")

    ''' <summary>
    ''' Loads all mods in KSMods
    ''' </summary>
    Sub StartMods()
        Wdbg("I", "Safe mode: {0}", SafeMode)
        If Not SafeMode Then
            If Not Directory.Exists(modPath) Then Directory.CreateDirectory(modPath)
            Dim count As Integer = Directory.EnumerateFiles(modPath).Count
            Wdbg("I", "Files count: {0}", count)
            If count <> 0 Then
                W(DoTranslation("mod: Loading mods..."), True, ColTypes.Neutral)
                Wdbg("I", "Mods are being loaded. Total mods with screensavers = {0}", count)
                For Each modFile As String In Directory.EnumerateFiles(modPath)
                    W(DoTranslation("Starting mod") + " {0}...", True, ColTypes.Neutral, Path.GetFileName(modFile))
                    ParseMod(modFile.Replace("\", "/"))
                Next
            Else
                W(DoTranslation("mod: No mods detected."), True, ColTypes.Neutral)
            End If
        Else
            W(DoTranslation("Parsing mods not allowed on safe mode."), True, ColTypes.Error)
        End If
    End Sub

    ''' <summary>
    ''' Starts a specified mod
    ''' </summary>
    ''' <param name="ModFilename">Mod filename found in KSMods</param>
    Public Sub StartMod(ByVal ModFilename As String)
        Throw New NotImplementedException()
    End Sub

    ''' <summary>
    ''' Stops all mods in KSMods
    ''' </summary>
    Sub StopMods()
        Wdbg("I", "Safe mode: {0}", SafeMode)
        If Not SafeMode Then
            If Not Directory.Exists(modPath) Then Directory.CreateDirectory(modPath)
            Dim count As Integer = Directory.EnumerateFiles(modPath).Count
            Wdbg("I", "Files count: {0}", count)
            If count <> 0 Then
                W(DoTranslation("mod: Stopping mods..."), True, ColTypes.Neutral)
                Wdbg("I", "Mods are being stopped. Total mods with screensavers = {0}", count)
                For Each script As String In scripts.Keys
                    Wdbg("I", "Stopping... Mod name: {0}", script)
                    Dim ScriptParts As Dictionary(Of String, IScript) = scripts(script).ModParts
                    For Each ScriptPart As String In ScriptParts.Keys
                        Wdbg("I", "Stopping part {0} v{1}", ScriptParts(ScriptPart).ModPart, ScriptParts(ScriptPart).Version)
                        ScriptParts(ScriptPart).StopMod()
                        If Not String.IsNullOrWhiteSpace(ScriptParts(ScriptPart).Name) And Not String.IsNullOrWhiteSpace(ScriptParts(ScriptPart).Version) Then
                            W(DoTranslation("{0} v{1} stopped"), True, ColTypes.Neutral, ScriptParts(ScriptPart).ModPart, ScriptParts(ScriptPart).Version)
                        End If
                    Next
                    W(DoTranslation("Mod {0} stopped"), True, ColTypes.Neutral, script)
                Next
                CSvrdb.Clear()
            Else
                W(DoTranslation("mod: No mods detected."), True, ColTypes.Neutral)
            End If
        Else
            W(DoTranslation("Parsing mods not allowed on safe mode."), True, ColTypes.Error)
        End If
    End Sub

    ''' <summary>
    ''' Stops a specified mod
    ''' </summary>
    ''' <param name="ModFilename">Mod filename found in KSMods</param>
    Public Sub StopMod(ByVal ModFilename As String)
        Throw New NotImplementedException()
    End Sub

    ''' <summary>
    ''' Reloads all mods
    ''' </summary>
    Sub ReloadMods()
        'Clear all scripts, commands, and defs
        modcmnds.Clear()
        ModDefs.Clear()
        Wdbg("I", "Mod commands for main shell cleared.")
        FTPModCommands.Clear()
        FTPModDefs.Clear()
        Wdbg("I", "Mod commands for FTP shell cleared.")
        MailModCommands.Clear()
        MailModDefs.Clear()
        Wdbg("I", "Mod commands for mail shell cleared.")
        SFTPModCommands.Clear()
        SFTPModDefs.Clear()
        Wdbg("I", "Mod commands for SFTP shell cleared.")
        TextEdit_ModCommands.Clear()
        TextEdit_ModHelpEntries.Clear()
        Wdbg("I", "Mod commands for text editor shell cleared.")
        Test_ModCommands.Clear()
        TestModDefs.Clear()
        Wdbg("I", "Mod commands for test shell cleared.")
        DebugModCmds.Clear()
        RDebugModDefs.Clear()
        Wdbg("I", "Mod commands for remote debug shell cleared.")
        ZipShell_ModCommands.Clear()
        ZipShell_ModHelpEntries.Clear()
        Wdbg("I", "Mod commands for ZIP shell cleared.")
        RSSModCommands.Clear()
        RSSModDefs.Clear()
        Wdbg("I", "Mod commands for RSS shell cleared.")
        scripts.Clear()
        Wdbg("I", "Mod scripts cleared.")

        'Stop all mods
        StopMods()
        Wdbg("I", "All mods stopped.")

        'Start all mods
        StartMods()
        Wdbg("I", "All mods restarted.")
    End Sub

    ''' <summary>
    ''' Reloads a specified mod
    ''' </summary>
    ''' <param name="ModFilename">Mod filename found in KSMods</param>
    Public Sub ReloadMod(ByVal ModFilename As String)
        Throw New NotImplementedException()
    End Sub

    ''' <summary>
    ''' Reloads all generic definitions so it can be updated with language change
    ''' </summary>
    ''' <param name="OldModDesc">Old mod command description</param>
    Sub ReloadGenericDefs(ByVal OldModDesc As String)
        For i As Integer = 0 To ModDefs.Keys.Count - 1
            Wdbg("I", "Replacing ""{0}""...", OldModDesc)
            Dim Cmd As String = ModDefs.Keys(i)
            If ModDefs(Cmd).Contains(OldModDesc) Then
                Wdbg("I", "Old Definition: {0}", ModDefs(Cmd))
                ModDefs(Cmd) = ModDefs(Cmd).Replace(OldModDesc, DoTranslation("Command defined by "))
                Wdbg("I", "New Definition: {0}", ModDefs(Cmd))
            End If
        Next
    End Sub

End Module
