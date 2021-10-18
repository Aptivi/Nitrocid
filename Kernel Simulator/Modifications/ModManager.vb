
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
Imports System.Reflection
Imports Newtonsoft.Json.Linq

Public Module ModManager

    ''' <summary>
    ''' Mods with their parts and scripts.
    ''' </summary>
    Public scripts As New Dictionary(Of String, ModInfo)
    Public BlacklistedModsString As String = ""
    Friend ReadOnly ModPath As String = GetKernelPath(KernelPathType.Mods)

    ''' <summary>
    ''' Loads all mods in KSMods
    ''' </summary>
    Public Sub StartMods()
        Wdbg(DebugLevel.I, "Safe mode: {0}", SafeMode)
        If Not SafeMode Then
            'We're not in safe mode. We're good now.
            If Not Directory.Exists(ModPath) Then Directory.CreateDirectory(ModPath)
            Dim count As Integer = Directory.EnumerateFiles(ModPath).Count
            Wdbg(DebugLevel.I, "Files count: {0}", count)

            'Check to see if we have mods
            If count <> 0 Then
                W(DoTranslation("mod: Loading mods..."), True, ColTypes.Neutral)
                Wdbg(DebugLevel.I, "Mods are being loaded. Total mods with screensavers = {0}", count)
                Dim CurrentCount As Integer = 1
                For Each modFile As String In Directory.EnumerateFiles(ModPath)
                    If Not GetBlacklistedMods.Contains(modFile) Then
                        Wdbg(DebugLevel.I, "Mod {0} is not blacklisted.", Path.GetFileName(modFile))
                        W("[{1}/{2}] " + DoTranslation("Starting mod") + " {0}...", True, ColTypes.Progress, Path.GetFileName(modFile), CurrentCount, count)
                        ParseMod(modFile)
                    Else
                        Wdbg(DebugLevel.W, "Trying to start blacklisted mod {0}. Ignoring...", Path.GetFileName(modFile))
                        W("[{1}/{2}] " + DoTranslation("Mod {0} is blacklisted."), True, ColTypes.Warning, Path.GetFileName(modFile), CurrentCount, count)
                    End If
                    CurrentCount += 1
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
    Public Sub StartMod(ModFilename As String)
        Wdbg(DebugLevel.I, "Safe mode: {0}", SafeMode)
        ModFilename = Path.Combine(ModPath, ModFilename)
        Wdbg(DebugLevel.I, "Mod file path: {0}", ModFilename)

        If Not SafeMode Then
            If File.Exists(ModFilename) Then
                Wdbg(DebugLevel.I, "Mod file exists! Starting...")
                If Not HasModStarted(ModFilename) Then
                    If Not GetBlacklistedMods.Contains(ModFilename) Then
                        Wdbg(DebugLevel.I, "Mod {0} is not blacklisted.", Path.GetFileName(ModFilename))
                        W(DoTranslation("Starting mod") + " {0}...", True, ColTypes.Neutral, Path.GetFileName(ModFilename))
                        ParseMod(ModFilename)
                    Else
                        Wdbg(DebugLevel.W, "Trying to start blacklisted mod {0}. Ignoring...", Path.GetFileName(ModFilename))
                        W(DoTranslation("Mod {0} is blacklisted."), True, ColTypes.Warning, Path.GetFileName(ModFilename))
                    End If
                Else
                    W(DoTranslation("Mod has already been started!"), True, ColTypes.Error)
                End If
            Else
                W(DoTranslation("Mod {0} not found."), True, ColTypes.Neutral, Path.GetFileName(ModFilename))
            End If
        Else
            W(DoTranslation("Parsing mods not allowed on safe mode."), True, ColTypes.Error)
        End If
    End Sub

    ''' <summary>
    ''' Stops all mods in KSMods
    ''' </summary>
    Public Sub StopMods()
        Wdbg(DebugLevel.I, "Safe mode: {0}", SafeMode)
        If Not SafeMode Then
            'We're not in safe mode. We're good now.
            If Not Directory.Exists(ModPath) Then Directory.CreateDirectory(ModPath)
            Dim count As Integer = Directory.EnumerateFiles(ModPath).Count
            Wdbg(DebugLevel.I, "Files count: {0}", count)

            'Check to see if we have mods
            If count <> 0 Then
                W(DoTranslation("mod: Stopping mods..."), True, ColTypes.Neutral)
                Wdbg(DebugLevel.I, "Mods are being stopped. Total mods with screensavers = {0}", count)

                'Enumerate and delete the script as soon as the stopping is complete
                For ScriptIndex As Integer = scripts.Count - 1 To 0 Step -1
                    Dim TargetMod As ModInfo = scripts.Values(ScriptIndex)
                    Dim ScriptParts As Dictionary(Of String, PartInfo) = TargetMod.ModParts

                    'Try to stop the mod and all associated parts
                    Wdbg(DebugLevel.I, "Stopping... Mod name: {0}", TargetMod.ModName)
                    For PartIndex As Integer = ScriptParts.Count - 1 To 0 Step -1
                        Dim ScriptPartInfo As PartInfo = ScriptParts.Values(PartIndex)
                        Wdbg(DebugLevel.I, "Stopping part {0} v{1}", ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version)

                        'Stop the associated part
                        ScriptPartInfo.PartScript.StopMod()
                        If Not String.IsNullOrWhiteSpace(ScriptPartInfo.PartName) And Not String.IsNullOrWhiteSpace(ScriptPartInfo.PartScript.Version) Then
                            W(DoTranslation("{0} v{1} stopped"), True, ColTypes.Neutral, ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version)
                        End If

                        'Remove the part from the list
                        ScriptParts.Remove(ScriptParts.Keys(PartIndex))
                    Next

                    'Remove the mod from the list
                    W(DoTranslation("Mod {0} stopped"), True, ColTypes.Neutral, TargetMod.ModName)
                    scripts.Remove(scripts.Keys(ScriptIndex))
                Next

                'Clear all mod commands list, since we've stopped all mods.
                ModCommands.Clear()
                ModDefs.Clear()
                Wdbg(DebugLevel.I, "Mod commands for main shell cleared.")
                FTPModCommands.Clear()
                FTPModDefs.Clear()
                Wdbg(DebugLevel.I, "Mod commands for FTP shell cleared.")
                MailModCommands.Clear()
                MailModDefs.Clear()
                Wdbg(DebugLevel.I, "Mod commands for mail shell cleared.")
                SFTPModCommands.Clear()
                SFTPModDefs.Clear()
                Wdbg(DebugLevel.I, "Mod commands for SFTP shell cleared.")
                TextEdit_ModCommands.Clear()
                TextEdit_ModHelpEntries.Clear()
                Wdbg(DebugLevel.I, "Mod commands for text editor shell cleared.")
                Test_ModCommands.Clear()
                TestModDefs.Clear()
                Wdbg(DebugLevel.I, "Mod commands for test shell cleared.")
                DebugModCmds.Clear()
                RDebugModDefs.Clear()
                Wdbg(DebugLevel.I, "Mod commands for remote debug shell cleared.")
                ZipShell_ModCommands.Clear()
                ZipShell_ModHelpEntries.Clear()
                Wdbg(DebugLevel.I, "Mod commands for ZIP shell cleared.")
                RSSModCommands.Clear()
                RSSModDefs.Clear()
                Wdbg(DebugLevel.I, "Mod commands for RSS shell cleared.")
                JsonShell_ModCommands.Clear()
                JsonShell_ModDefs.Clear()
                Wdbg(DebugLevel.I, "Mod commands for JSON shell cleared.")
                HTTPModCommands.Clear()
                HTTPModDefs.Clear()
                Wdbg(DebugLevel.I, "Mod commands for HTTP shell cleared.")

                'Clear the custom screensavers
                CustomSavers.Clear()
            Else
                W(DoTranslation("mod: No mods detected."), True, ColTypes.Neutral)
            End If
        Else
            W(DoTranslation("Stopping mods not allowed on safe mode."), True, ColTypes.Error)
        End If
    End Sub

    ''' <summary>
    ''' Stops a specified mod
    ''' </summary>
    ''' <param name="ModFilename">Mod filename found in KSMods</param>
    Public Sub StopMod(ModFilename As String)
        Wdbg(DebugLevel.I, "Safe mode: {0}", SafeMode)
        ModFilename = Path.Combine(ModPath, ModFilename)
        Wdbg(DebugLevel.I, "Mod file path: {0}", ModFilename)

        If Not SafeMode Then
            If File.Exists(ModFilename) Then
                'Determine if we're dealing with screensaver
                If Path.GetExtension(ModFilename) = ".ss." Then
                    Wdbg(DebugLevel.I, "Target mod is a screensaver.")

                    'Iterate through all the screensavers
                    For SaverIndex As Integer = CustomSavers.Count - 1 To 0 Step -1
                        Dim TargetScreensaver As ScreensaverInfo = CustomSavers.Values(SaverIndex)
                        Wdbg(DebugLevel.I, "Checking screensaver {0}", TargetScreensaver.SaverName)

                        'Check to see if we're dealign with the same screensaver
                        If TargetScreensaver.FileName = ModFilename Then
                            CustomSavers.Remove(CustomSavers.Keys(SaverIndex))
                        End If
                    Next
                Else
                    If HasModStarted(ModFilename) Then
                        W(DoTranslation("mod: Stopping mod {0}..."), True, ColTypes.Neutral, Path.GetFileName(ModFilename))
                        Wdbg(DebugLevel.I, "Mod {0} is being stopped.", Path.GetFileName(ModFilename))

                        'Iterate through all the mods
                        For ScriptIndex As Integer = scripts.Count - 1 To 0 Step -1
                            Dim TargetMod As ModInfo = scripts.Values(ScriptIndex)
                            Dim ScriptParts As Dictionary(Of String, PartInfo) = TargetMod.ModParts

                            'Try to stop the mod and all associated parts
                            Wdbg(DebugLevel.I, "Checking mod {0}...", TargetMod.ModName)
                            If TargetMod.ModFileName = Path.GetFileName(ModFilename) Then
                                Wdbg(DebugLevel.I, "Found mod to be stopped. Stopping...")

                                'Iterate through all the parts
                                For PartIndex As Integer = ScriptParts.Count - 1 To 0 Step -1
                                    Dim ScriptPartInfo As PartInfo = ScriptParts.Values(PartIndex)
                                    Wdbg(DebugLevel.I, "Stopping part {0} v{1}", ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version)

                                    'Remove all the commands associated with the part
                                    If ScriptPartInfo.PartScript.Commands IsNot Nothing Then
                                        For Each CommandInfo As CommandInfo In ScriptPartInfo.PartScript.Commands.Values
                                            Select Case CommandInfo.Type
                                                Case ShellCommandType.Shell
                                                    Wdbg(DebugLevel.I, "Removing command {0} from main shell...", CommandInfo.Command)
                                                    ModCommands.Remove(CommandInfo.Command)
                                                    ModDefs.Remove(CommandInfo.Command)
                                                Case ShellCommandType.FTPShell
                                                    Wdbg(DebugLevel.I, "Removing command {0} from FTP shell...", CommandInfo.Command)
                                                    FTPModCommands.Remove(CommandInfo.Command)
                                                    FTPModDefs.Remove(CommandInfo.Command)
                                                Case ShellCommandType.MailShell
                                                    Wdbg(DebugLevel.I, "Removing command {0} from mail shell...", CommandInfo.Command)
                                                    MailModCommands.Remove(CommandInfo.Command)
                                                    MailModDefs.Remove(CommandInfo.Command)
                                                Case ShellCommandType.SFTPShell
                                                    Wdbg(DebugLevel.I, "Removing command {0} from SFTP shell...", CommandInfo.Command)
                                                    SFTPModCommands.Remove(CommandInfo.Command)
                                                    SFTPModDefs.Remove(CommandInfo.Command)
                                                Case ShellCommandType.TextShell
                                                    Wdbg(DebugLevel.I, "Removing command {0} from text editor shell...", CommandInfo.Command)
                                                    TextEdit_ModCommands.Remove(CommandInfo.Command)
                                                    TextEdit_ModHelpEntries.Remove(CommandInfo.Command)
                                                Case ShellCommandType.TestShell
                                                    Wdbg(DebugLevel.I, "Removing command {0} from test shell...", CommandInfo.Command)
                                                    Test_ModCommands.Remove(CommandInfo.Command)
                                                    TestModDefs.Remove(CommandInfo.Command)
                                                Case ShellCommandType.RemoteDebugShell
                                                    Wdbg(DebugLevel.I, "Removing command {0} from remote debug shell...", CommandInfo.Command)
                                                    DebugModCmds.Remove(CommandInfo.Command)
                                                    RDebugModDefs.Remove(CommandInfo.Command)
                                                Case ShellCommandType.ZIPShell
                                                    Wdbg(DebugLevel.I, "Removing command {0} from ZIP shell...", CommandInfo.Command)
                                                    ZipShell_ModCommands.Remove(CommandInfo.Command)
                                                    ZipShell_ModHelpEntries.Remove(CommandInfo.Command)
                                                Case ShellCommandType.RSSShell
                                                    Wdbg(DebugLevel.I, "Removing command {0} from RSS shell...", CommandInfo.Command)
                                                    RSSModCommands.Remove(CommandInfo.Command)
                                                    RSSModDefs.Remove(CommandInfo.Command)
                                                Case ShellCommandType.JsonShell
                                                    Wdbg(DebugLevel.I, "Removing command {0} from JSON shell...", CommandInfo.Command)
                                                    JsonShell_ModCommands.Remove(CommandInfo.Command)
                                                    JsonShell_ModDefs.Remove(CommandInfo.Command)
                                                Case ShellCommandType.HTTPShell
                                                    Wdbg(DebugLevel.I, "Removing command {0} from HTTP shell...", CommandInfo.Command)
                                                    HTTPModCommands.Remove(CommandInfo.Command)
                                                    HTTPModDefs.Remove(CommandInfo.Command)
                                            End Select
                                        Next
                                    End If

                                    'Stop the associated part
                                    ScriptPartInfo.PartScript.StopMod()
                                    If Not String.IsNullOrWhiteSpace(ScriptPartInfo.PartName) And Not String.IsNullOrWhiteSpace(ScriptPartInfo.PartScript.Version) Then
                                        W(DoTranslation("{0} v{1} stopped"), True, ColTypes.Neutral, ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version)
                                    End If

                                    'Remove the part from the list
                                    ScriptParts.Remove(ScriptParts.Keys(PartIndex))
                                Next

                                'Remove the mod from the list
                                W(DoTranslation("Mod {0} stopped"), True, ColTypes.Neutral, TargetMod.ModName)
                                scripts.Remove(scripts.Keys(ScriptIndex))
                            End If
                        Next
                    Else
                        W(DoTranslation("Mod hasn't started yet!"), True, ColTypes.Error)
                    End If
                End If
            Else
                W(DoTranslation("Mod {0} not found."), True, ColTypes.Neutral, Path.GetFileName(ModFilename))
            End If
        Else
            W(DoTranslation("Stopping mods not allowed on safe mode."), True, ColTypes.Error)
        End If
    End Sub

    ''' <summary>
    ''' Reloads all mods
    ''' </summary>
    Sub ReloadMods()
        'Stop all mods
        StopMods()
        Wdbg(DebugLevel.I, "All mods stopped.")

        'Start all mods
        StartMods()
        Wdbg(DebugLevel.I, "All mods restarted.")
    End Sub

    ''' <summary>
    ''' Reloads a specified mod
    ''' </summary>
    ''' <param name="ModFilename">Mod filename found in KSMods</param>
    Public Sub ReloadMod(ModFilename As String)
        StopMod(ModFilename)
        StartMod(ModFilename)
    End Sub

    ''' <summary>
    ''' Checks to see if the mod has started
    ''' </summary>
    ''' <param name="ModFilename">Mod filename found in KSMods</param>
    Public Function HasModStarted(ModFilename As String) As Boolean
        'Iterate through each mod and mod part
        For Each ModName As String In scripts.Keys
            Wdbg(DebugLevel.I, "Checking mod {0}...", ModName)
            For Each PartName As String In scripts(ModName).ModParts.Keys
                Wdbg(DebugLevel.I, "Checking part {0}...", PartName)
                If scripts(ModName).ModParts(PartName).PartFilePath = ModFilename Then
                    Wdbg(DebugLevel.I, "Found part {0} ({1}). Returning True...", PartName, ModFilename)
                    Return True
                End If
            Next
        Next

        'If not found, exit with mod not started yet
        Return False
    End Function

    ''' <summary>
    ''' Adds the mod to the blacklist (specified mod will not start on the next boot)
    ''' </summary>
    ''' <param name="ModFilename">Mod filename found in KSMods</param>
    Public Sub AddModToBlacklist(ModFilename As String)
        ModFilename = NeutralizePath(ModFilename, GetKernelPath(KernelPathType.Mods))
        Wdbg(DebugLevel.I, "Adding {0} to the mod blacklist...", ModFilename)
        Dim BlacklistedMods As List(Of String) = GetBlacklistedMods()
        If Not BlacklistedMods.Contains(ModFilename) Then
            Wdbg(DebugLevel.I, "Mod {0} not on the blacklist. Adding...", ModFilename)
            BlacklistedMods.Add(ModFilename)
        End If
        BlacklistedModsString = String.Join(";", BlacklistedMods)
        Dim Token As JToken = GetConfigCategory(ConfigCategory.Misc)
        SetConfigValue(ConfigCategory.Misc, Token, "Blacklisted mods", BlacklistedModsString)
    End Sub

    ''' <summary>
    ''' Removes the mod from the blacklist (specified mod will start on the next boot)
    ''' </summary>
    ''' <param name="ModFilename">Mod filename found in KSMods</param>
    Public Sub RemoveModFromBlacklist(ModFilename As String)
        ModFilename = NeutralizePath(ModFilename, GetKernelPath(KernelPathType.Mods))
        Wdbg(DebugLevel.I, "Removing {0} from the mod blacklist...", ModFilename)
        Dim BlacklistedMods As List(Of String) = GetBlacklistedMods()
        If BlacklistedMods.Contains(ModFilename) Then
            Wdbg(DebugLevel.I, "Mod {0} on the blacklist. Removing...", ModFilename)
            BlacklistedMods.Remove(ModFilename)
        End If
        BlacklistedModsString = String.Join(";", BlacklistedMods)
        Dim Token As JToken = GetConfigCategory(ConfigCategory.Misc)
        SetConfigValue(ConfigCategory.Misc, Token, "Blacklisted mods", BlacklistedModsString)
    End Sub

    ''' <summary>
    ''' Gets the blacklisted mods list
    ''' </summary>
    Public Function GetBlacklistedMods() As List(Of String)
        Return BlacklistedModsString.Split(";").ToList
    End Function

    ''' <summary>
    ''' Installs the mod DLL or single code file to the mod directory
    ''' </summary>
    ''' <param name="ModPath">Target mod path</param>
    Public Sub InstallMod(ModPath As String)
#Disable Warning BC42104
        Dim TargetModPath As String = NeutralizePath(Path.GetFileName(ModPath), GetKernelPath(KernelPathType.Mods))
        Dim Script As IScript
        ModPath = NeutralizePath(ModPath, True)
        Wdbg(DebugLevel.I, "Installing mod {0} to {1}...", ModPath, TargetModPath)

        'Check for upgrade
        If File.Exists(TargetModPath) Then
            W(DoTranslation("Trying to install an already-installed mod. Updating mod..."), True, ColTypes.Warning)
            StopMod(Path.GetFileName(TargetModPath))
        End If

        Try
            'First, parse the mod file
            If Path.GetExtension(ModPath) = ".cs" Then
                'Mod has a language of C#
                Wdbg(DebugLevel.I, "Mod language is C# from extension "".cs""")
                Script = GenMod("C#", File.ReadAllText(ModPath))
            ElseIf Path.GetExtension(ModPath) = ".vb" Then
                'Mod has a language of VB.NET
                Wdbg(DebugLevel.I, "Mod language is VB.NET from extension "".vb""")
                Script = GenMod("VB.NET", File.ReadAllText(ModPath))
            ElseIf Path.GetExtension(ModPath) = ".dll" Then
                'Mod is a dynamic DLL
                Try
                    Script = GetModInstance(Assembly.LoadFrom(ModPath))
                Catch ex As ReflectionTypeLoadException
                    Wdbg(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", ModPath, ex.Message)
                    WStkTrc(ex)
                    W(DoTranslation("Mod can't be loaded because of the following: "), True, ColTypes.Error)
                    For Each LoaderException As Exception In ex.LoaderExceptions
                        Wdbg(DebugLevel.E, "Loader exception: {0}", LoaderException.Message)
                        WStkTrc(LoaderException)
                        W(LoaderException.Message, True, ColTypes.Error)
                    Next
                    W(DoTranslation("Contact the vendor of the mod to upgrade the mod to the compatible version."), True, ColTypes.Error)
                    Throw ex
                End Try
            End If

            'Second, check the script
            If Script Is Nothing Then Throw New Exceptions.ModInstallException(DoTranslation("The mod file provided is incompatible."))

            'Then, install the file.
            File.Copy(ModPath, TargetModPath, True)

            'Check for the manual pages
            If Directory.Exists(ModPath + ".manual") Then
                Wdbg(DebugLevel.I, "Found manual page directory. {0}.manual exists. Installing manual pages...", ModPath)
                Directory.CreateDirectory(TargetModPath + ".manual")
                For Each ModManualFile As String In Directory.EnumerateFiles(ModPath + ".manual", "*.man", SearchOption.AllDirectories)
                    Dim ManualFileName As String = Path.GetFileNameWithoutExtension(ModManualFile)
                    Dim ManualInstance As New Manual(ModManualFile)
                    If Not ManualInstance.ValidManpage Then Throw New Exceptions.ModInstallException(DoTranslation("The manual page {0} is invalid.").FormatString(ManualFileName))
                    CopyFileOrDir(ModManualFile, TargetModPath + ".manual/" + ModManualFile)
                Next
            End If

            'Finally, start the mod
            W(DoTranslation("Starting mod") + " {0}...", True, ColTypes.Neutral, Path.GetFileNameWithoutExtension(TargetModPath))
            StartMod(TargetModPath)
        Catch ex As Exception
            Wdbg(DebugLevel.E, "Installation failed for {0}: {1}", ModPath, ex.Message)
            WStkTrc(ex)
            W(DoTranslation("Installation failed for") + " {0}: {1}", True, ColTypes.Error, ModPath, ex.Message)
        End Try
#Enable Warning BC42104
    End Sub

    ''' <summary>
    ''' Uninstalls the mod from the mod directory
    ''' </summary>
    ''' <param name="ModPath">Target mod path found in KSMods</param>
    Public Sub UninstallMod(ModPath As String)
        Dim TargetModPath As String = NeutralizePath(ModPath, GetKernelPath(KernelPathType.Mods), True)
        Wdbg(DebugLevel.I, "Uninstalling mod {0}...", TargetModPath)
        Try
            'First, stop all mods related to it
            StopMod(TargetModPath)

            'Then, remove the file.
            File.Delete(TargetModPath)

            'Finally, check for the manual pages and remove them
            If Directory.Exists(ModPath + ".manual") Then
                Wdbg(DebugLevel.I, "Found manual page directory. {0}.manual exists. Removing manual pages...", ModPath)
                For Each ModManualFile As String In Directory.EnumerateFiles(ModPath + ".manual", "*.man", SearchOption.AllDirectories)
                    Dim ManualFileName As String = Path.GetFileNameWithoutExtension(ModManualFile)
                    Dim ManualInstance As New Manual(ModManualFile)
                    If ManualInstance.ValidManpage Then
                        Pages.Remove(ManualInstance.Title)
                    Else
                        Throw New Exceptions.ModUninstallException(DoTranslation("The manual page {0} is invalid.").FormatString(ManualFileName))
                    End If
                Next
                Directory.Delete(ModPath + ".manual", True)
            End If
        Catch ex As Exception
            Wdbg(DebugLevel.E, "Uninstallation failed for {0}: {1}", ModPath, ex.Message)
            WStkTrc(ex)
            W(DoTranslation("Uninstallation failed for") + " {0}: {1}", True, ColTypes.Error, ModPath, ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Reloads all generic definitions so it can be updated with language change
    ''' </summary>
    ''' <param name="OldModDesc">Old mod command description</param>
    Sub ReloadGenericDefs(OldModDesc As String)
        For i As Integer = 0 To ModDefs.Keys.Count - 1
            Wdbg(DebugLevel.I, "Replacing ""{0}""...", OldModDesc)
            Dim Cmd As String = ModDefs.Keys(i)
            If ModDefs(Cmd).Contains(OldModDesc) Then
                Wdbg(DebugLevel.I, "Old Definition: {0}", ModDefs(Cmd))
                ModDefs(Cmd) = ModDefs(Cmd).Replace(OldModDesc, DoTranslation("Command defined by "))
                Wdbg(DebugLevel.I, "New Definition: {0}", ModDefs(Cmd))
            End If
        Next
    End Sub

End Module
