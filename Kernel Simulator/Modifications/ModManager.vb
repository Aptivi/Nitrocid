
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

Imports System.IO
Imports System.Reflection
Imports Newtonsoft.Json.Linq
Imports KS.Files.Operations
Imports KS.Files.Querying
Imports KS.ManPages
Imports KS.Misc.Configuration
Imports KS.Shell.Shells.Json
Imports KS.Misc.Screensaver.Customized
Imports KS.Misc.Splash
Imports KS.Shell.Shells.Text
Imports KS.Shell.Shells.Hex
Imports KS.Shell.Shells.Rar
Imports KS.Shell.Shells.Zip
Imports KS.Shell.Shells.FTP
Imports KS.Shell.Shells.HTTP
Imports KS.Network.Mail
Imports KS.Shell.Shells.Mail
Imports KS.Network.RemoteDebug
Imports KS.Shell.Shells.RSS
Imports KS.Shell.Shells.SFTP
Imports KS.Shell.Shells.Test
Imports KS.Shell.Shells.UESH

Namespace Modifications
    Public Module ModManager

        Public BlacklistedModsString As String = ""
        Friend Mods As New Dictionary(Of String, ModInfo)

        ''' <summary>
        ''' Loads all mods in KSMods
        ''' </summary>
        Public Sub StartMods()
            Dim ModPath As String = GetKernelPath(KernelPathType.Mods)
            Wdbg(DebugLevel.I, "Safe mode: {0}", SafeMode)
            If Not SafeMode Then
                'We're not in safe mode. We're good now.
                If Not FolderExists(ModPath) Then Directory.CreateDirectory(ModPath)
                Dim count As Integer = Directory.EnumerateFiles(ModPath).Count
                Wdbg(DebugLevel.I, "Files count: {0}", count)

                'Check to see if we have mods
                If count <> 0 Then
                    ReportProgress(DoTranslation("mod: Loading mods..."), 0, ColTypes.Neutral)
                    Wdbg(DebugLevel.I, "Mods are being loaded. Total mods with screensavers = {0}", count)
                    Dim CurrentCount As Integer = 1
                    For Each modFile As String In Directory.EnumerateFiles(ModPath)
                        If Not GetBlacklistedMods.Contains(modFile) Then
                            Wdbg(DebugLevel.I, "Mod {0} is not blacklisted.", Path.GetFileName(modFile))
                            ReportProgress("[{1}/{2}] " + DoTranslation("Starting mod") + " {0}...", 0, ColTypes.Progress, Path.GetFileName(modFile), CurrentCount, count)
                            modFile = Path.GetFileName(modFile)
                            ParseMod(modFile)
                        Else
                            Wdbg(DebugLevel.W, "Trying to start blacklisted mod {0}. Ignoring...", Path.GetFileName(modFile))
                            ReportProgress("[{1}/{2}] " + DoTranslation("Mod {0} is blacklisted."), 0, ColTypes.Warning, Path.GetFileName(modFile), CurrentCount, count)
                        End If
                        CurrentCount += 1
                    Next
                Else
                    ReportProgress(DoTranslation("mod: No mods detected."), 0, ColTypes.Neutral)
                End If
            Else
                ReportProgress(DoTranslation("Parsing mods not allowed on safe mode."), 0, ColTypes.Error)
            End If
        End Sub

        ''' <summary>
        ''' Starts a specified mod
        ''' </summary>
        ''' <param name="ModFilename">Mod filename found in KSMods</param>
        Public Sub StartMod(ModFilename As String)
            Dim ModPath As String = GetKernelPath(KernelPathType.Mods)
            Dim PathToMod As String = Path.Combine(ModPath, ModFilename)
            Wdbg(DebugLevel.I, "Safe mode: {0}", SafeMode)
            Wdbg(DebugLevel.I, "Mod file path: {0}", PathToMod)

            If Not SafeMode Then
                If FileExists(PathToMod) Then
                    Wdbg(DebugLevel.I, "Mod file exists! Starting...")
                    If Not HasModStarted(PathToMod) Then
                        If Not GetBlacklistedMods.Contains(PathToMod) Then
                            Wdbg(DebugLevel.I, "Mod {0} is not blacklisted.", ModFilename)
                            ReportProgress(DoTranslation("Starting mod") + " {0}...", 0, ColTypes.Neutral, ModFilename)
                            ParseMod(ModFilename)
                        Else
                            Wdbg(DebugLevel.W, "Trying to start blacklisted mod {0}. Ignoring...", ModFilename)
                            ReportProgress(DoTranslation("Mod {0} is blacklisted."), 0, ColTypes.Warning, ModFilename)
                        End If
                    Else
                        ReportProgress(DoTranslation("Mod has already been started!"), 0, ColTypes.Error)
                    End If
                Else
                    ReportProgress(DoTranslation("Mod {0} not found."), 0, ColTypes.Neutral, ModFilename)
                End If
            Else
                ReportProgress(DoTranslation("Parsing mods not allowed on safe mode."), 0, ColTypes.Error)
            End If
        End Sub

        ''' <summary>
        ''' Stops all mods in KSMods
        ''' </summary>
        Public Sub StopMods()
            Dim ModPath As String = GetKernelPath(KernelPathType.Mods)
            Wdbg(DebugLevel.I, "Safe mode: {0}", SafeMode)
            If Not SafeMode Then
                'We're not in safe mode. We're good now.
                If Not FolderExists(ModPath) Then Directory.CreateDirectory(ModPath)
                Dim count As Integer = Directory.EnumerateFiles(ModPath).Count
                Wdbg(DebugLevel.I, "Files count: {0}", count)

                'Check to see if we have mods
                If count <> 0 Then
                    Write(DoTranslation("mod: Stopping mods..."), True, ColTypes.Neutral)
                    Wdbg(DebugLevel.I, "Mods are being stopped. Total mods with screensavers = {0}", count)

                    'Enumerate and delete the script as soon as the stopping is complete
                    For ScriptIndex As Integer = Mods.Count - 1 To 0 Step -1
                        Dim TargetMod As ModInfo = Mods.Values(ScriptIndex)
                        Dim ScriptParts As Dictionary(Of String, PartInfo) = TargetMod.ModParts

                        'Try to stop the mod and all associated parts
                        Wdbg(DebugLevel.I, "Stopping... Mod name: {0}", TargetMod.ModName)
                        For PartIndex As Integer = ScriptParts.Count - 1 To 0 Step -1
                            Dim ScriptPartInfo As PartInfo = ScriptParts.Values(PartIndex)
                            Wdbg(DebugLevel.I, "Stopping part {0} v{1}", ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version)

                            'Stop the associated part
                            ScriptPartInfo.PartScript.StopMod()
                            If Not String.IsNullOrWhiteSpace(ScriptPartInfo.PartName) And Not String.IsNullOrWhiteSpace(ScriptPartInfo.PartScript.Version) Then
                                Write(DoTranslation("{0} v{1} stopped"), True, ColTypes.Neutral, ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version)
                            End If

                            'Remove the part from the list
                            ScriptParts.Remove(ScriptParts.Keys(PartIndex))
                        Next

                        'Remove the mod from the list
                        Write(DoTranslation("Mod {0} stopped"), True, ColTypes.Neutral, TargetMod.ModName)
                        Mods.Remove(Mods.Keys(ScriptIndex))
                    Next

                    'Clear all mod commands list, since we've stopped all mods.
                    For Each ShellTypeName As String In [Enum].GetNames(GetType(ShellType))
                        Dim ShellTypeEnum As ShellType = [Enum].Parse(GetType(ShellType), ShellTypeName)
                        ListModCommands(ShellTypeEnum).Clear()
                        Wdbg(DebugLevel.I, "Mod commands for {0} cleared.", ShellTypeEnum.ToString)
                    Next

                    'Clear the custom screensavers
                    CustomSavers.Clear()
                Else
                    Write(DoTranslation("mod: No mods detected."), True, ColTypes.Neutral)
                End If
            Else
                Write(DoTranslation("Stopping mods not allowed on safe mode."), True, ColTypes.Error)
            End If
        End Sub

        ''' <summary>
        ''' Stops a specified mod
        ''' </summary>
        ''' <param name="ModFilename">Mod filename found in KSMods</param>
        Public Sub StopMod(ModFilename As String)
            Dim ModPath As String = GetKernelPath(KernelPathType.Mods)
            Dim PathToMod As String = Path.Combine(ModPath, ModFilename)
            Wdbg(DebugLevel.I, "Safe mode: {0}", SafeMode)
            Wdbg(DebugLevel.I, "Mod file path: {0}", PathToMod)

            If Not SafeMode Then
                If FileExists(PathToMod) Then
                    If HasModStarted(PathToMod) Then
                        Write(DoTranslation("mod: Stopping mod {0}..."), True, ColTypes.Neutral, ModFilename)
                        Wdbg(DebugLevel.I, "Mod {0} is being stopped.", ModFilename)

                        'Iterate through all the mods
                        For ScriptIndex As Integer = Mods.Count - 1 To 0 Step -1
                            Dim TargetMod As ModInfo = Mods.Values(ScriptIndex)
                            Dim ScriptParts As Dictionary(Of String, PartInfo) = TargetMod.ModParts

                            'Try to stop the mod and all associated parts
                            Wdbg(DebugLevel.I, "Checking mod {0}...", TargetMod.ModName)
                            If TargetMod.ModFileName = ModFilename Then
                                Wdbg(DebugLevel.I, "Found mod to be stopped. Stopping...")

                                'Iterate through all the parts
                                For PartIndex As Integer = ScriptParts.Count - 1 To 0 Step -1
                                    Dim ScriptPartInfo As PartInfo = ScriptParts.Values(PartIndex)
                                    Wdbg(DebugLevel.I, "Stopping part {0} v{1}", ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version)

                                    'Remove all the commands associated with the part
                                    If ScriptPartInfo.PartScript.Commands IsNot Nothing Then
                                        For Each CommandInfo As CommandInfo In ScriptPartInfo.PartScript.Commands.Values
                                            Wdbg(DebugLevel.I, "Removing command {0} from {1}...", CommandInfo.Command, CommandInfo.Type)
                                            ListModCommands(CommandInfo.Type).Remove(CommandInfo.Command)
                                        Next
                                    End If

                                    'Stop the associated part
                                    ScriptPartInfo.PartScript.StopMod()
                                    If Not String.IsNullOrWhiteSpace(ScriptPartInfo.PartName) And Not String.IsNullOrWhiteSpace(ScriptPartInfo.PartScript.Version) Then
                                        Write(DoTranslation("{0} v{1} stopped"), True, ColTypes.Neutral, ScriptPartInfo.PartName, ScriptPartInfo.PartScript.Version)
                                    End If

                                    'Remove the part from the list
                                    ScriptParts.Remove(ScriptParts.Keys(PartIndex))
                                Next

                                'Remove the mod from the list
                                Write(DoTranslation("Mod {0} stopped"), True, ColTypes.Neutral, TargetMod.ModName)
                                Mods.Remove(Mods.Keys(ScriptIndex))
                            End If
                        Next
                    Else
                        Write(DoTranslation("Mod hasn't started yet!"), True, ColTypes.Error)
                    End If
                Else
                    Write(DoTranslation("Mod {0} not found."), True, ColTypes.Neutral, ModFilename)
                End If
            Else
                Write(DoTranslation("Stopping mods not allowed on safe mode."), True, ColTypes.Error)
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
            For Each ModName As String In Mods.Keys
                Wdbg(DebugLevel.I, "Checking mod {0}...", ModName)
                For Each PartName As String In Mods(ModName).ModParts.Keys
                    Wdbg(DebugLevel.I, "Checking part {0}...", PartName)
                    If Mods(ModName).ModParts(PartName).PartFilePath = ModFilename Then
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
            Dim TargetModPath As String = NeutralizePath(Path.GetFileName(ModPath), GetKernelPath(KernelPathType.Mods))
            Dim Script As IScript
            ModPath = NeutralizePath(ModPath, True)
            Wdbg(DebugLevel.I, "Installing mod {0} to {1}...", ModPath, TargetModPath)

            'Check for upgrade
            If FileExists(TargetModPath) Then
                Write(DoTranslation("Trying to install an already-installed mod. Updating mod..."), True, ColTypes.Warning)
                StopMod(Path.GetFileName(TargetModPath))
            End If

            Try
                'First, parse the mod file
                If Path.GetExtension(ModPath) = ".dll" Then
                    'Mod is a dynamic DLL
                    Try
                        Script = GetModInstance(Assembly.LoadFrom(ModPath))
                        If Script Is Nothing Then Throw New Exceptions.ModInstallException(DoTranslation("The mod file provided is incompatible."))
                    Catch ex As ReflectionTypeLoadException
                        Wdbg(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", ModPath, ex.Message)
                        WStkTrc(ex)
                        Write(DoTranslation("Mod can't be loaded because of the following: "), True, ColTypes.Error)
                        For Each LoaderException As Exception In ex.LoaderExceptions
                            Wdbg(DebugLevel.E, "Loader exception: {0}", LoaderException.Message)
                            WStkTrc(LoaderException)
                            Write(LoaderException.Message, True, ColTypes.Error)
                        Next
                        Write(DoTranslation("Contact the vendor of the mod to upgrade the mod to the compatible version."), True, ColTypes.Error)
                        Throw
                    Catch ex As Exceptions.ModInstallException
                        Throw
                    End Try
                End If

                'Then, install the file.
                File.Copy(ModPath, TargetModPath, True)

                'Check for the manual pages
                If FolderExists(ModPath + ".manual") Then
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
                Write(DoTranslation("Starting mod") + " {0}...", True, ColTypes.Neutral, Path.GetFileNameWithoutExtension(TargetModPath))
                StartMod(Path.GetFileName(TargetModPath))
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Installation failed for {0}: {1}", ModPath, ex.Message)
                WStkTrc(ex)
                Write(DoTranslation("Installation failed for") + " {0}: {1}", True, ColTypes.Error, ModPath, ex.Message)
            End Try
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
                If FolderExists(ModPath + ".manual") Then
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
                Write(DoTranslation("Uninstallation failed for") + " {0}: {1}", True, ColTypes.Error, ModPath, ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Lists the mods
        ''' </summary>
        Public Function ListMods() As Dictionary(Of String, ModInfo)
            Return ListMods("")
        End Function

        ''' <summary>
        ''' Lists the mods
        ''' </summary>
        ''' <param name="SearchTerm">Search term</param>
        Public Function ListMods(SearchTerm As String) As Dictionary(Of String, ModInfo)
            Dim ListedMods As New Dictionary(Of String, ModInfo)

            'List the mods using the search term
            For Each ModName As String In Mods.Keys
                If ModName.Contains(SearchTerm) Then
                    ListedMods.Add(ModName, Mods(ModName))
                End If
            Next
            Return ListedMods
        End Function

        ''' <summary>
        ''' Lists the mod commands based on the shell
        ''' </summary>
        ''' <param name="ShellType">Selected shell type</param>
        Public Function ListModCommands(ShellType As ShellType) As Dictionary(Of String, CommandInfo)
            Select Case ShellType
                Case ShellType.Shell
                    Return ModCommands
                Case ShellType.RemoteDebugShell
                    Return DebugModCmds
                Case ShellType.FTPShell
                    Return FTPModCommands
                Case ShellType.SFTPShell
                    Return SFTPModCommands
                Case ShellType.MailShell
                    Return MailModCommands
                Case ShellType.TextShell
                    Return TextEdit_ModCommands
                Case ShellType.TestShell
                    Return Test_ModCommands
                Case ShellType.ZIPShell
                    Return ZipShell_ModCommands
                Case ShellType.RSSShell
                    Return RSSModCommands
                Case ShellType.JsonShell
                    Return JsonShell_ModCommands
                Case ShellType.HTTPShell
                    Return HTTPModCommands
                Case ShellType.HexShell
                    Return HexEdit_ModCommands
                Case ShellType.RARShell
                    Return RARShell_ModCommands
            End Select
        End Function

    End Module
End Namespace
