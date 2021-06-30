
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

Imports System.CodeDom.Compiler
Imports System.IO
Imports System.Reflection
Imports Microsoft.CSharp

Public Module ModParser

    ''' <summary>
    ''' Interface for mods
    ''' </summary>
    Public Interface IScript
        ''' <summary>
        ''' List of commands for mod
        ''' </summary>
        Property Commands As Dictionary(Of String, CommandInfo)
        ''' <summary>
        ''' Mod name
        ''' </summary>
        Property Name As String
        ''' <summary>
        ''' Name of part of mod
        ''' </summary>
        Property ModPart As String
        ''' <summary>
        ''' Mod version
        ''' </summary>
        Property Version As String
        ''' <summary>
        ''' Code executed when starting mod
        ''' </summary>
        Sub StartMod()
        ''' <summary>
        ''' Code executed when stopping mod
        ''' </summary>
        Sub StopMod()
        ''' <summary>
        ''' Code executed when performing command
        ''' </summary>
        ''' <param name="Command">A command.</param>
        ''' <param name="args">Arguments. Make sure to split your arguments if necessary.</param>
        Sub PerformCmd(ByVal Command As CommandInfo, Optional ByVal Args As String = "")
        ''' <summary>
        ''' Code executed when initializing events
        ''' </summary>
        ''' <param name="ev">Event name. Look it up on <see cref="Events"/></param>
        Sub InitEvents(ByVal ev As String)
        ''' <summary>
        ''' Code executed when initializing events
        ''' </summary>
        ''' <param name="ev">Event name. Look it up on <see cref="Events"/></param>
        ''' <param name="Args">Arguments.</param>
        Sub InitEvents(ByVal ev As String, ParamArray Args() As Object)
    End Interface

    ''' <summary>
    ''' Mods with their parts and scripts.
    ''' </summary>
    Public scripts As New Dictionary(Of String, ModInfo)
    Private ReadOnly modPath As String = paths("Mods")

    ''' <summary>
    ''' Compiles the script and returns the instance of script interface
    ''' </summary>
    ''' <param name="PLang">Specified programming language for scripts (C# or VB.NET)</param>
    ''' <param name="code">Code blocks from script</param>
    ''' <returns></returns>
    Private Function GenMod(ByVal PLang As String, ByVal code As String) As IScript

        'Check language
        Dim provider As CodeDomProvider
        Wdbg("I", $"Language detected: {PLang}")
        If PLang = "C#" Then
            provider = New CSharpCodeProvider
        ElseIf PLang = "VB.NET" Then
            provider = New VBCodeProvider
        Else
            Exit Function
        End If

        'Declare new compiler parameter object
        Dim prm As New CompilerParameters With {
            .GenerateExecutable = False,
            .GenerateInMemory = True
        }

        'Add referenced assemblies
        Wdbg("I", "Referenced assemblies will be added.")
        prm.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly.Location) 'It should reference itself
        prm.ReferencedAssemblies.Add("System.dll")
        prm.ReferencedAssemblies.Add("System.Core.dll")
        prm.ReferencedAssemblies.Add("System.Data.dll")
        prm.ReferencedAssemblies.Add("System.DirectoryServices.dll")
        prm.ReferencedAssemblies.Add("System.Xml.dll")
        prm.ReferencedAssemblies.Add("System.Xml.Linq.dll")
        Wdbg("I", "Referenced assemblies added.")

        'Detect referenced assemblies from comments that start with "Reference GAC: <ref>" or "Reference File: <path/to/ref>".
        Dim References() As String = code.SplitNewLines.Select(Function(x) x).Where(Function(x) x.ContainsAnyOf({"Reference GAC: ", "Reference File: "})).ToArray
        Wdbg("I", "Found {0} references (matches taken from searching for ""Reference GAC: "" or ""Reference File: "").", References.Length)
        For Each Reference As String In References
            Reference.RemoveNullsOrWhitespacesAtTheBeginning
            Wdbg("I", "Reference line: {0}", Reference)
            Dim LocationCheckRequired As Boolean = Reference.Contains("Reference File: ")
            If (Reference.StartsWith("//") And PLang = "C#") Or (Reference.StartsWith("'") And PLang = "VB.NET") Then
                'Remove comment mark
                If Reference.StartsWith("//") Then Reference = Reference.Remove(0, 2)
                If Reference.StartsWith("'") Then Reference = Reference.Remove(0, 1)

                'Remove "Reference GAC: " or "Reference File: " and remove all whitespaces or nulls in the beginning
                Reference = Reference.ReplaceAll({"Reference GAC: ", "Reference File: "}, "")
                Reference.RemoveNullsOrWhitespacesAtTheBeginning
                Wdbg("I", "Final reference line: {0}", Reference)

                'Add reference
                If LocationCheckRequired Then
                    'Check to see if the reference file exists
                    If Not File.Exists(Reference) Then
                        Wdbg("E", "File {0} not found to reference.", Reference)
                        W(DoTranslation("Referenced file {0} not found. This mod might not work properly without this file."), True, ColTypes.Warning, Reference)
                        GoTo NextEntry
                    End If
                End If
                prm.ReferencedAssemblies.Add(Reference)
            End If
NextEntry:
        Next

        'Try to compile
        Dim namespc As String = GetType(IScript).Namespace
        Dim modCode() As String
        If PLang = "VB.NET" Then
            modCode = {$"Imports {namespc}{vbNewLine}{code}"}
        ElseIf PLang = "C#" Then
            modCode = {$"using {namespc};{vbNewLine}{code}"}
        End If
#Disable Warning BC42104
        Wdbg("I", "Compiling...")
        Dim res As CompilerResults = provider.CompileAssemblyFromSource(prm, modCode)
#Enable Warning BC42104

        'Check to see if there are compilation errors
        Wdbg("I", "Has errors: {0}", res.Errors.HasErrors)
        Wdbg("I", "Has warnings: {0}", res.Errors.HasWarnings)
        If res.Errors.HasErrors Then
            W(DoTranslation("Mod can't be loaded because of the following: "), True, ColTypes.Error)
            For Each errorName In res.Errors
                W(errorName.ToString, True, ColTypes.Error)
                Wdbg("E", errorName.ToString)
            Next
            Exit Function
        End If

        'Make object type instance
        Wdbg("I", "Creating instance of type...")
        Return GetModInstance(res.CompiledAssembly)
    End Function

    ''' <summary>
    ''' Gets the mod instance from compiled assembly
    ''' </summary>
    ''' <param name="Assembly">An assembly</param>
    Public Function GetModInstance(ByVal Assembly As Assembly) As IScript
        For Each t As Type In Assembly.GetTypes()
            If t.GetInterface(GetType(IScript).Name) IsNot Nothing Then Return CType(Assembly.CreateInstance(t.FullName), IScript)
        Next
    End Function

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
    ''' Starts to parse the mod, and configures it so it can be used
    ''' </summary>
    ''' <param name="modFile">Mod file name with extension. It should end with .vb or .cs</param>
    Sub ParseMod(ByVal modFile As String)
        modFile = modFile.Replace(modPath, "")
        If modFile.EndsWith(".ss.vb") Then
            'Mod is a screensaver that has a language of VB.NET
            Wdbg("W", "Mod file {0} is a screensaver. Language: VB.NET", modFile)
            CompileCustom(modFile)
        ElseIf modFile.EndsWith(".ss.cs") Then
            'Mod is a screensaver that has a language of C#
            Wdbg("W", "Mod file {0} is a screensaver. Language: C#", modFile)
            CompileCustom(modFile)
        ElseIf modFile.EndsWith(".cs") Then
            'Mod has a language of C#
            Wdbg("I", "Mod language is C# from extension "".cs""")
            Dim script As IScript = GenMod("C#", IO.File.ReadAllText(modPath + modFile))
            FinalizeMods(script, modFile)
        ElseIf modFile.EndsWith(".vb") Then
            'Mod has a language of VB.NET
            Wdbg("I", "Mod language is VB.NET from extension "".vb""")
            Dim script As IScript = GenMod("VB.NET", IO.File.ReadAllText(modPath + modFile))
            FinalizeMods(script, modFile)
        ElseIf modFile.EndsWith(".dll") Then
            'Mod is a dynamic DLL
            Try
                Dim script As IScript = GetModInstance(Assembly.LoadFrom(modPath + modFile))
                If script Is Nothing Then CompileCustom(modPath + modFile)
                FinalizeMods(script, modFile)
            Catch ex As ReflectionTypeLoadException
                Wdbg("E", "Error trying to load dynamic mod {0}: {1}", modFile, ex.Message)
                WStkTrc(ex)
                W(DoTranslation("Mod can't be loaded because of the following: "), True, ColTypes.Error)
                For Each LoaderException As Exception In ex.LoaderExceptions
                    Wdbg("E", "Loader exception: {0}", LoaderException.Message)
                    WStkTrc(LoaderException)
                    W(LoaderException.Message, True, ColTypes.Error)
                Next
            End Try
        Else
            'Ignore all mods that its file name doesn't end with .vb
            Wdbg("W", "Unsupported file type for mod file {0}.", modFile)
        End If
    End Sub

    ''' <summary>
    ''' Configures the mod so it can be used
    ''' </summary>
    ''' <param name="script">Instance of script</param>
    ''' <param name="modFile">Mod file name with extension. It should end with .vb, .ss.vb, .ss.cs, or .cs</param>
    Sub FinalizeMods(ByVal script As IScript, ByVal modFile As String)
        Dim ModParts As New Dictionary(Of String, IScript)
        Dim ModInstance As ModInfo

        'Try to finalize mod
        If script IsNot Nothing Then
            EventManager.RaiseModParsed(modFile)
            Try
                'Start the mod
                script.StartMod()
                Wdbg("I", "script.StartMod() initialized. Mod name: {0} | Mod part: {1} | Version: {2}", script.Name, script.ModPart, script.Version)

                'See if the mod has part name
                If String.IsNullOrWhiteSpace(script.ModPart) Then
                    Wdbg("W", "No part name for {0}", modFile)
                    W(DoTranslation("Mod {0} does not have the part name. Mod parsing failed. Review the source code."), True, ColTypes.Error, modFile)
                    Exit Sub
                End If

                'See if the commands in a mod are valid
                If script.Commands IsNot Nothing Then
                    For Each Command As String In script.Commands.Keys
                        If String.IsNullOrWhiteSpace(Command) Then
                            Wdbg("W", "No command for {0}", modFile)
                            W(DoTranslation("Mod {0} has invalid command. Mod parsing failed. Review the source code."), True, ColTypes.Error, modFile)
                            Exit Sub
                        End If
                    Next
                End If

                'See if the mod has name
                Dim ModName As String = script.Name
                If String.IsNullOrWhiteSpace(ModName) Then
                    ModName = modFile
                    Wdbg("W", "No name for {0}", modFile)
                    W(DoTranslation("Mod {0} does not have the name. Review the source code."), True, ColTypes.Warning, modFile)
                Else
                    Wdbg("I", "There is a name for {0}", modFile)
                End If
                Wdbg("I", "Mod name: {0}", ModName)

                'See if the mod part conflicts with existing parts
                Wdbg("I", "Checking to see if {0} exists in scripts...", ModName)
                If scripts.ContainsKey(ModName) Then
                    Wdbg("I", "Exists. Adding mod part {0}...", script.ModPart)
                    If Not scripts(ModName).ModParts.ContainsKey(script.ModPart) Then
                        Wdbg("I", "No conflict with {0}. Adding as is...", script.ModPart)
                        scripts(ModName).ModParts.Add(script.ModPart, script)
                    Else
                        Wdbg("W", "There is a conflict with {0}. Appending item number...", script.ModPart)
                        script.ModPart += CStr(scripts(ModName).ModParts.Count)
                        scripts(ModName).ModParts.Add(script.ModPart, script)
                    End If
                Else
                    Wdbg("I", "Adding mod with mod part {0}...", script.ModPart)
                    If Not ModParts.ContainsKey(script.ModPart) Then
                        Wdbg("I", "No conflict with {0}. Adding as is...", script.ModPart)
                        ModParts.Add(script.ModPart, script)
                    Else
                        Wdbg("W", "There is a conflict with {0}. Appending item number...", script.ModPart)
                        script.ModPart += CStr(scripts.Count)
                        ModParts.Add(script.ModPart, script)
                    End If
                    ModInstance = New ModInfo(ModName, modFile, NeutralizePath(modFile, modPath), ModParts, script.Version)
                    scripts.Add(ModName, ModInstance)
                End If

                'See if the mod has version
                If script.Version = "" And script.Name <> "" Then
                    Wdbg("I", "{0}.Version = """" | {0}.Name = {1}", modFile, script.Name)
                    W(DoTranslation("Mod {0} does not have the version."), True, ColTypes.Warning, script.Name)
                ElseIf script.Name <> "" And script.Version <> "" Then
                    Wdbg("I", "{0}.Version = {2} | {0}.Name = {1}", modFile, script.Name, script.Version)
                    W(DoTranslation("{0} v{1} started") + " ({2})", True, ColTypes.Neutral, script.Name, script.Version, script.ModPart)
                End If

                'Process the commands that are defined in a mod
                If script.Commands IsNot Nothing Then
                    For i As Integer = 0 To script.Commands.Keys.Count - 1
                        'See if the command conflicts with pre-existing shell commands
                        Dim Command As String = script.Commands.Keys(i)
                        Dim ActualCommand As String = Command
                        If script.Commands(Command).Type = ShellCommandType.Shell Then
                            If Commands.ContainsKey(Command) Or modcmnds.Contains(Command) Then
                                Wdbg("W", "Command {0} conflicts with available shell commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                Command += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                            End If
                        ElseIf script.Commands(Command).Type = ShellCommandType.FTPShell Then
                            If FTPCommands.ContainsKey(Command) Or FTPModCommands.Contains(Command) Then
                                Wdbg("W", "Command {0} conflicts with available FTP shell commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                Command += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                            End If
                        ElseIf script.Commands(Command).Type = ShellCommandType.MailShell Then
                            If MailCommands.ContainsKey(Command) Or MailModCommands.Contains(Command) Then
                                Wdbg("W", "Command {0} conflicts with available mail shell commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                Command += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                            End If
                        ElseIf script.Commands(Command).Type = ShellCommandType.SFTPShell Then
                            If SFTPCommands.ContainsKey(Command) Or SFTPModCommands.Contains(Command) Then
                                Wdbg("W", "Command {0} conflicts with available SFTP shell commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                Command += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                            End If
                        ElseIf script.Commands(Command).Type = ShellCommandType.TextShell Then
                            If TextEdit_Commands.ContainsKey(Command) Or TextEdit_ModCommands.Contains(Command) Then
                                Wdbg("W", "Command {0} conflicts with available text shell commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                Command += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                            End If
                        ElseIf script.Commands(Command).Type = ShellCommandType.TestShell Then
                            If Test_Commands.ContainsKey(Command) Or Test_ModCommands.Contains(Command) Then
                                Wdbg("W", "Command {0} conflicts with available text shell commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                Command += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                            End If
                        ElseIf script.Commands(Command).Type = ShellCommandType.RemoteDebugShell Then
                            If DebugCommands.ContainsKey(Command) Or DebugModCmds.Contains(Command) Then
                                Wdbg("W", "Command {0} conflicts with available remote debug shell commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                Command += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                            End If
                        ElseIf script.Commands(Command).Type = ShellCommandType.ZIPShell Then
                            If ZipShell_Commands.ContainsKey(Command) Or ZipShell_ModCommands.Contains(Command) Then
                                Wdbg("W", "Command {0} conflicts with available ZIP shell commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                Command += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                            End If
                        ElseIf script.Commands(Command).Type = ShellCommandType.RSSShell Then
                            If RSSCommands.ContainsKey(Command) Or RSSModCommands.Contains(Command) Then
                                Wdbg("W", "Command {0} conflicts with available RSS shell commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                Command += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                            End If
                        End If

                        'See if mod can be added to command list
                        If Command <> "" Then
                            If script.Commands(ActualCommand).HelpDefinition = "" Then
                                W(DoTranslation("No definition for command {0}."), True, ColTypes.Warning, Command)
                                Wdbg("W", "{0}.Def = Nothing, {0}.Def = ""Command defined by {1} ({2})""", Command, script.Name, script.ModPart)
                                script.Commands(ActualCommand).HelpDefinition = DoTranslation("Command defined by ") + script.Name + " (" + script.ModPart + ")"
                            End If

                            Wdbg("I", "Command type: {0}", script.Commands(ActualCommand).Type)
                            Select Case script.Commands(ActualCommand).Type
                                Case ShellCommandType.Shell
                                    Wdbg("I", "Adding command {0} for main shell...", Command)
                                    If Not modcmnds.Contains(Command) Then modcmnds.Add(Command)
                                    script.Commands.RenameKey(ActualCommand, Command)
                                    moddefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                Case ShellCommandType.FTPShell
                                    Wdbg("I", "Adding command {0} for FTP shell...", Command)
                                    If Not FTPModCommands.Contains(Command) Then FTPModCommands.Add(Command)
                                    script.Commands.RenameKey(ActualCommand, Command)
                                    FTPModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                Case ShellCommandType.MailShell
                                    Wdbg("I", "Adding command {0} for mail shell...", Command)
                                    If Not MailModCommands.Contains(Command) Then MailModCommands.Add(Command)
                                    script.Commands.RenameKey(ActualCommand, Command)
                                    MailModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                Case ShellCommandType.SFTPShell
                                    Wdbg("I", "Adding command {0} for SFTP shell...", Command)
                                    If Not SFTPModCommands.Contains(Command) Then SFTPModCommands.Add(Command)
                                    script.Commands.RenameKey(ActualCommand, Command)
                                    SFTPModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                Case ShellCommandType.TextShell
                                    Wdbg("I", "Adding command {0} for text editor shell...", Command)
                                    If Not TextEdit_ModCommands.Contains(Command) Then TextEdit_ModCommands.Add(Command)
                                    script.Commands.RenameKey(ActualCommand, Command)
                                    TextEdit_ModHelpEntries.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                Case ShellCommandType.TestShell
                                    Wdbg("I", "Adding command {0} for test shell...", Command)
                                    If Not Test_ModCommands.Contains(Command) Then Test_ModCommands.Add(Command)
                                    script.Commands.RenameKey(ActualCommand, Command)
                                    TestModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                Case ShellCommandType.RemoteDebugShell
                                    Wdbg("I", "Adding command {0} for remote debug shell...", Command)
                                    If Not DebugModCmds.Contains(Command) Then DebugModCmds.Add(Command)
                                    script.Commands.RenameKey(ActualCommand, Command)
                                    RDebugModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                Case ShellCommandType.ZIPShell
                                    Wdbg("I", "Adding command {0} for ZIP shell...", Command)
                                    If Not ZipShell_ModCommands.Contains(Command) Then ZipShell_ModCommands.Add(Command)
                                    script.Commands.RenameKey(ActualCommand, Command)
                                    ZipShell_ModHelpEntries.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                Case ShellCommandType.RSSShell
                                    Wdbg("I", "Adding command {0} for RSS shell...", Command)
                                    If Not RSSModCommands.Contains(Command) Then RSSModCommands.Add(Command)
                                    script.Commands.RenameKey(ActualCommand, Command)
                                    RSSModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                            End Select
                        End If
                    Next
                End If

                'Create instance of mod info

                'Raise event
                EventManager.RaiseModFinalized(modFile)
            Catch ex As Exception
                EventManager.RaiseModFinalizationFailed(modFile, ex.Message)
                WStkTrc(ex)
                W(DoTranslation("Failed to finalize mod {0}: {1}"), True, ColTypes.Error, modFile, ex.Message)
            End Try
        Else
            EventManager.RaiseModParseError(modFile)
        End If
    End Sub

    '------------------------------------------- Reloader -------------------------------------------
    ''' <summary>
    ''' Reloads all mods
    ''' </summary>
    Sub ReloadMods()
        'Clear all scripts, commands, and defs
        modcmnds.Clear()
        moddefs.Clear()
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
    ''' Reloads all generic definitions so it can be updated with language change
    ''' </summary>
    ''' <param name="OldModDesc">Old mod command description</param>
    Sub ReloadGenericDefs(ByVal OldModDesc As String)
        For i As Integer = 0 To moddefs.Keys.Count - 1
            Wdbg("I", "Replacing ""{0}""...", OldModDesc)
            Dim Cmd As String = moddefs.Keys(i)
            If moddefs(Cmd).Contains(OldModDesc) Then
                Wdbg("I", "Old Definition: {0}", moddefs(Cmd))
                moddefs(Cmd) = moddefs(Cmd).Replace(OldModDesc, DoTranslation("Command defined by "))
                Wdbg("I", "New Definition: {0}", moddefs(Cmd))
            End If
        Next
    End Sub

End Module