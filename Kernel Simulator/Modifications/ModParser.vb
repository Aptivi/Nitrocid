
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

Imports System.IO
Imports System.Reflection
Imports KS.ManPages
Imports KS.Misc.Reflection
Imports KS.Misc.Screensaver.Customized
Imports KS.Misc.Screensaver
Imports KS.Misc.Splash
Imports KS.Network.Mail
Imports KS.Kernel.Exceptions

Namespace Modifications
    Public Module ModParser

        ''' <summary>
        ''' Gets the mod instance from compiled assembly
        ''' </summary>
        ''' <param name="Assembly">An assembly</param>
        Public Function GetModInstance(Assembly As Assembly) As IScript
            For Each t As Type In Assembly.GetTypes()
                If t.GetInterface(GetType(IScript).Name) IsNot Nothing Then Return CType(Assembly.CreateInstance(t.FullName), IScript)
            Next
        End Function

        ''' <summary>
        ''' Starts to parse the mod, and configures it so it can be used
        ''' </summary>
        ''' <param name="modFile">Mod file name with extension. It should end with .dll</param>
        Sub ParseMod(modFile As String)
            modFile = Path.GetFileName(modFile)
            If modFile.EndsWith(".dll") Then
                'Mod is a dynamic DLL
                Try
                    'Check to see if the DLL is actually a mod
                    Dim script As IScript = GetModInstance(Assembly.LoadFrom(ModPath + modFile))

                    'Check to see if the DLL is actually a screensaver
                    If script Is Nothing Then CompileCustom(ModPath + modFile)
                    Dim CheckSaver As ICustomSaver = GetScreensaverInstance(Assembly.LoadFrom(ModPath + modFile))

                    'If we didn't find anything, abort
                    If script Is Nothing AndAlso CheckSaver Is Nothing Then Throw New InvalidModException(DoTranslation("The modfile is invalid."))

                    'Finalize the mod
                    FinalizeMods(script, modFile)
                Catch ex As ReflectionTypeLoadException
                    Wdbg(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", modFile, ex.Message)
                    WStkTrc(ex)
                    ReportProgress(DoTranslation("Mod can't be loaded because of the following: "), 0, ColTypes.Error)
                    For Each LoaderException As Exception In ex.LoaderExceptions
                        Wdbg(DebugLevel.E, "Loader exception: {0}", LoaderException.Message)
                        WStkTrc(LoaderException)
                        ReportProgress(LoaderException.Message, 0, ColTypes.Error)
                    Next
                    ReportProgress(DoTranslation("Contact the vendor of the mod to upgrade the mod to the compatible version."), 0, ColTypes.Error)
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", modFile, ex.Message)
                    WStkTrc(ex)
                    ReportProgress(DoTranslation("Mod can't be loaded because of the following: ") + ex.Message, 0, ColTypes.Error)
                End Try
            Else
                'Ignore unsupported files
                Wdbg(DebugLevel.W, "Unsupported file type for mod file {0}.", modFile)
            End If
        End Sub

        ''' <summary>
        ''' Configures the mod so it can be used
        ''' </summary>
        ''' <param name="script">Instance of script</param>
        ''' <param name="modFile">Mod file name with extension. It should end with .dll</param>
        Sub FinalizeMods(script As IScript, modFile As String)
            Dim ModParts As New Dictionary(Of String, PartInfo)
            Dim ModInstance As ModInfo
            Dim PartInstance As PartInfo

            'Try to finalize mod
            If script IsNot Nothing Then
                KernelEventManager.RaiseModParsed(modFile)
                Try
                    'Add mod dependencies folder (if any) to the private appdomain lookup folder
                    Dim ModDepPath As String = ModPath + "Deps/" + Path.GetFileNameWithoutExtension(modFile) + "-" + FileVersionInfo.GetVersionInfo(ModPath + modFile).FileVersion + "/"
                    AddPathToAssemblySearchPath(ModDepPath)

                    'Start the mod
                    script.StartMod()
                    Wdbg(DebugLevel.I, "script.StartMod() initialized. Mod name: {0} | Mod part: {1} | Version: {2}", script.Name, script.ModPart, script.Version)

                    'See if the mod has part name
                    If String.IsNullOrWhiteSpace(script.ModPart) Then
                        Wdbg(DebugLevel.W, "No part name for {0}", modFile)
                        ReportProgress(DoTranslation("Mod {0} does not have the part name. Mod parsing failed. Review the source code."), 0, ColTypes.Error, modFile)
                        Exit Sub
                    End If

                    'See if the commands in a mod are valid
                    If script.Commands IsNot Nothing Then
                        For Each Command As String In script.Commands.Keys
                            If String.IsNullOrWhiteSpace(Command) Then
                                Wdbg(DebugLevel.W, "No command for {0}", modFile)
                                ReportProgress(DoTranslation("Mod {0} has invalid command. Mod parsing failed. Review the source code."), 0, ColTypes.Error, modFile)
                                Exit Sub
                            End If
                        Next
                    End If

                    'See if the mod has name
                    Dim ModName As String = script.Name
                    If String.IsNullOrWhiteSpace(ModName) Then
                        'Mod has no name! Give it a file name.
                        ModName = modFile
                        Wdbg(DebugLevel.W, "No name for {0}", modFile)
                        ReportProgress(DoTranslation("Mod {0} does not have the name. Review the source code."), 0, ColTypes.Warning, modFile)
                    Else
                        Wdbg(DebugLevel.I, "There is a name for {0}", modFile)
                    End If
                    Wdbg(DebugLevel.I, "Mod name: {0}", ModName)

                    'Check to see if there is a part under the same name.
                    Dim Parts As Dictionary(Of String, PartInfo) = If(Mods.ContainsKey(ModName), Mods(ModName).ModParts, ModParts)
                    Wdbg(DebugLevel.I, "Adding mod part {0}...", script.ModPart)
                    If Parts.ContainsKey(script.ModPart) Then
                        'Append the number to the end of the name
                        Wdbg(DebugLevel.W, "There is a conflict with {0}. Appending item number...", script.ModPart)
                        script.ModPart = $"{script.ModPart} [{Parts.Count}]"
                    End If

                    'Now, add the part
                    PartInstance = New PartInfo(ModName, script.ModPart, modFile, NeutralizePath(modFile, ModPath), script)
                    Parts.Add(script.ModPart, PartInstance)
                    ModInstance = New ModInfo(ModName, modFile, NeutralizePath(modFile, ModPath), Parts, script.Version)
                    Mods.AddIfNotFound(ModName, ModInstance)

                    'See if the mod has version
                    If String.IsNullOrWhiteSpace(script.Version) And Not String.IsNullOrWhiteSpace(script.Name) Then
                        Wdbg(DebugLevel.I, "{0}.Version = """" | {0}.Name = {1}", modFile, script.Name)
                        ReportProgress(DoTranslation("Mod {0} does not have the version."), 0, ColTypes.Warning, script.Name)
                    ElseIf Not String.IsNullOrWhiteSpace(script.Name) And Not String.IsNullOrWhiteSpace(script.Version) Then
                        Wdbg(DebugLevel.I, "{0}.Version = {2} | {0}.Name = {1}", modFile, script.Name, script.Version)
                        ReportProgress(DoTranslation("{0} v{1} started") + " ({2})", 0, ColTypes.Success, script.Name, script.Version, script.ModPart)
                    End If

                    'Process the commands that are defined in a mod
                    If script.Commands IsNot Nothing Then
                        For i As Integer = 0 To script.Commands.Keys.Count - 1
                            'See if the command conflicts with pre-existing shell commands
                            Dim Command As String = script.Commands.Keys(i)
                            Dim ActualCommand As String = Command
                            Dim CommandType As CommandType = script.Commands.Values(i).Type
                            Wdbg(DebugLevel.I, "Command type: {0}", CommandType)
                            If IsCommandFound(Command, CommandType) Or ListModCommands(CommandType).ContainsKey(Command) Then
                                Wdbg(DebugLevel.W, "Command {0} conflicts with available shell commands or mod commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                Command += $"-{script.Name}-{script.ModPart}"
                            End If

                            'See if mod can be added to command list
                            If Command <> "" Then
                                If script.Commands(ActualCommand).HelpDefinition = "" Then
                                    ReportProgress(DoTranslation("No definition for command {0}."), 0, ColTypes.Warning, Command)
                                    Wdbg(DebugLevel.W, "{0}.Def = Nothing, {0}.Def = ""Command defined by {1} ({2})""", Command, script.Name, script.ModPart)
                                    script.Commands(ActualCommand).HelpDefinition = DoTranslation("Command defined by ") + script.Name + " (" + script.ModPart + ")"
                                End If

                                'Now, add the command to the mod list
                                Wdbg(DebugLevel.I, "Adding command {0} for {1}...", Command, CommandType.ToString)
                                If Not ListModCommands(CommandType).ContainsKey(Command) Then ListModCommands(CommandType).Add(Command, script.Commands(ActualCommand))
                                script.Commands.RenameKey(ActualCommand, Command)
                            End If
                        Next
                    End If

                    'Check for accompanying manual pages for mods
                    Dim ModManualPath As String = NeutralizePath(modFile + ".manual", ModPath)
                    If FolderExists(ModManualPath) Then
                        Wdbg(DebugLevel.I, "Found manual page collection in {0}", ModManualPath)
                        For Each ModManualFile As String In Directory.EnumerateFiles(ModManualPath, "*.man", SearchOption.AllDirectories)
                            InitMan(ModManualFile)
                        Next
                    End If

                    'Raise event
                    KernelEventManager.RaiseModFinalized(modFile)
                Catch ex As Exception
                    KernelEventManager.RaiseModFinalizationFailed(modFile, ex.Message)
                    WStkTrc(ex)
                    ReportProgress(DoTranslation("Failed to finalize mod {0}: {1}"), 0, ColTypes.Error, modFile, ex.Message)
                End Try
            Else
                KernelEventManager.RaiseModParseError(modFile)
            End If
        End Sub

    End Module
End Namespace