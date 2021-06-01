
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
        ''' Command name for mod
        ''' </summary>
        Property Cmd As String
        ''' <summary>
        ''' Command shell type for mod
        ''' </summary>
        Property CmdType As ShellCommandType
        ''' <summary>
        ''' Command definition for mod
        ''' </summary>
        Property Def As String
        ''' <summary>
        ''' Indicates whether only admins can use this command
        ''' </summary>
        Property CmdRestricted As Boolean
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
        ''' <param name="args">Arguments. Make sure to split your arguments if necessary.</param>
        Sub PerformCmd(Optional ByVal args As String = "")
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

    'Variables
    ''' <summary>
    ''' Mods with their parts and scripts.
    ''' </summary>
    Public scripts As New Dictionary(Of String, Dictionary(Of String, IScript))
    Private ReadOnly modPath As String = paths("Mods")

    '------------------------------------------- Generators -------------------------------------------
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
        Dim References() As String = code.Replace(Chr(13), "").Split(Chr(10)).Select(Function(x) x).Where(Function(x) x.ContainsAnyOf({"Reference GAC: ", "Reference File: "})).ToArray
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

    '------------------------------------------- Parsers -------------------------------------------
    ''' <summary>
    ''' Loads or stops all mods in KSMods
    ''' </summary>
    ''' <param name="StartStop">If true, the mods start, otherwise, the mod stops.</param>
    Sub ParseMods(ByVal StartStop As Boolean)
        Wdbg("I", "Safe mode: {0}", SafeMode)
        If Not SafeMode Then
            If Not FileIO.FileSystem.DirectoryExists(modPath) Then FileIO.FileSystem.CreateDirectory(modPath)
            Dim count As Integer = FileIO.FileSystem.GetFiles(modPath).Count
            Wdbg("I", "Files count: {0}", count)
            If count <> 0 Then
                If StartStop Then
                    W(DoTranslation("mod: Loading mods..."), True, ColTypes.Neutral)
                    Wdbg("I", "Mods are being loaded. Total mods with screensavers = {0}", count)
                ElseIf Not StartStop Then
                    W(DoTranslation("mod: Stopping mods..."), True, ColTypes.Neutral)
                    Wdbg("I", "Mods are being stopped. Total mods with screensavers = {0}", count)
                End If
                If Not StartStop Then
                    For Each script As Dictionary(Of String, IScript) In scripts.Values
                        Wdbg("I", "Stopping... Mod name: {0}", scripts.GetKeyFromValue(script))
                        For Each ScriptPart As String In script.Keys
                            Wdbg("I", "Stopping part {0} v{1}", script(ScriptPart).ModPart, script(ScriptPart).Version)
                            script(ScriptPart).StopMod()
                            If script(ScriptPart).Name <> "" And script(ScriptPart).Version <> "" Then
                                W(DoTranslation("{0} v{1} stopped"), True, ColTypes.Neutral, script(ScriptPart).ModPart, script(ScriptPart).Version)
                            End If
                        Next
                        W(DoTranslation("Mod {0} stopped"), True, ColTypes.Neutral, scripts.GetKeyFromValue(script))
                    Next
                    For Each Screensaver As String In CSvrdb.Keys
                        ScrnSvrdb.Remove(Screensaver)
                    Next
                    CSvrdb.Clear()
                Else
                    For Each modFile As String In FileIO.FileSystem.GetFiles(modPath)
                        W(DoTranslation("Starting mod") + " {0}...", True, ColTypes.Neutral, Path.GetFileName(modFile))
                        StartParse(modFile.Replace("\", "/"), StartStop)
                    Next
                End If
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
    ''' <param name="StartStop">Whether to start or stop mods</param>
    Sub StartParse(ByVal modFile As String, Optional ByVal StartStop As Boolean = True)
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
            FinalizeMods(script, modFile, StartStop)
        ElseIf modFile.EndsWith(".vb") Then
            'Mod has a language of VB.NET
            Wdbg("I", "Mod language is VB.NET from extension "".vb""")
            Dim script As IScript = GenMod("VB.NET", IO.File.ReadAllText(modPath + modFile))
            FinalizeMods(script, modFile, StartStop)
        ElseIf modFile.EndsWith(".dll") Then
            'Mod is a dynamic DLL
            Try
                Dim script As IScript = GetModInstance(Assembly.LoadFrom(modPath + modFile))
                FinalizeMods(script, modFile, StartStop)
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

    '------------------------------------------- Finalizer -------------------------------------------
    ''' <summary>
    ''' Configures the mod so it can be used
    ''' </summary>
    ''' <param name="script">Instance of script</param>
    ''' <param name="modFile">Mod file name with extension. It should end with .vb, .ss.vb, .ss.cs, or .cs</param>
    ''' <param name="StartStop">Whether to start or stop mods</param>
    Sub FinalizeMods(ByVal script As IScript, ByVal modFile As String, Optional ByVal StartStop As Boolean = True)
        Dim ModParts As New Dictionary(Of String, IScript)
        If script IsNot Nothing Then
            EventManager.RaiseModParsed(StartStop, modFile)
            Try
                script.StartMod()
                Wdbg("I", "script.StartMod() initialized. Mod name: {0} | Mod part: {1} | Version: {2}", script.Name, script.ModPart, script.Version)

                'See if the mod has part name
                If script.ModPart = "" Then
                    Wdbg("W", "No part name for {0}", modFile)
                    W(DoTranslation("Mod {0} does not have the part name. Mod parsing failed. Review the source code."), True, ColTypes.Error, modFile)
                    Exit Sub
                End If

                'See if the mod has command
                If script.Cmd = "" Then
                    Wdbg("W", "No command for {0}", modFile)
                    W(DoTranslation("Mod {0} does not have the command. Mod parsing failed. Review the source code."), True, ColTypes.Error, modFile)
                    Exit Sub
                End If

                'See if the mod has name
                Dim ModName As String = script.Name
                If ModName = "" Then
                    ModName = script.Cmd
                    Wdbg("W", "No name for {0}", modFile)
                    W(DoTranslation("Mod {0} does not have the name. Review the source code."), True, ColTypes.Neutral, modFile)
                Else
                    Wdbg("I", "There is a name for {0}", modFile)
                End If
                Wdbg("I", "Mod name: {0}", ModName)

                'See if the mod part conflicts with existing parts
                Wdbg("I", "Checking to see if {0} exists in scripts...", ModName)
                If scripts.ContainsKey(ModName) Then
                    Wdbg("I", "Exists. Adding mod part {0}...", script.ModPart)
                    If Not scripts(ModName).ContainsKey(script.ModPart) Then
                        Wdbg("I", "No conflict with {0}. Adding as is...", script.ModPart)
                        scripts(ModName).Add(script.ModPart, script)
                    Else
                        Wdbg("W", "There is a conflict with {0}. Appending item number...", script.ModPart)
                        script.ModPart += CStr(scripts(ModName).Count)
                        scripts(ModName).Add(script.ModPart, script)
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
                    scripts.Add(ModName, ModParts)
                End If

                'See if the mod has version
                If script.Version = "" And script.Name <> "" Then
                    Wdbg("I", "{0}.Version = """" | {0}.Name = {1}", modFile, script.Name)
                    W(DoTranslation("Mod {0} does not have the version."), True, ColTypes.Neutral, script.Name)
                ElseIf script.Name <> "" And script.Version <> "" Then
                    Wdbg("I", "{0}.Version = {2} | {0}.Name = {1}", modFile, script.Name, script.Version)
                    W(DoTranslation("{0} v{1} started") + " ({2})", True, ColTypes.Neutral, script.Name, script.Version, script.ModPart)
                End If

                'See if the command conflicts with pre-existing shell commands
                If script.CmdType = ShellCommandType.Shell Then
                    If Commands.ContainsKey(script.Cmd) Then
                        Wdbg("W", "Command {0} conflicts with available shell commands. Appending ""-{1}-{2}"" to end of command...", script.Cmd, script.Name, script.ModPart)
                        script.Cmd += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                    End If
                ElseIf script.CmdType = ShellCommandType.FTPShell Then
                    If FTPCommands.ContainsKey(script.Cmd) Then
                        Wdbg("W", "Command {0} conflicts with available FTP shell commands. Appending ""-{1}-{2}"" to end of command...", script.Cmd, script.Name, script.ModPart)
                        script.Cmd += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                    End If
                ElseIf script.CmdType = ShellCommandType.MailShell Then
                    If MailCommands.ContainsKey(script.Cmd) Then
                        Wdbg("W", "Command {0} conflicts with available mail shell commands. Appending ""-{1}-{2}"" to end of command...", script.Cmd, script.Name, script.ModPart)
                        script.Cmd += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                    End If
                ElseIf script.CmdType = ShellCommandType.SFTPShell Then
                    If SFTPCommands.ContainsKey(script.Cmd) Then
                        Wdbg("W", "Command {0} conflicts with available SFTP shell commands. Appending ""-{1}-{2}"" to end of command...", script.Cmd, script.Name, script.ModPart)
                        script.Cmd += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                    End If
                ElseIf script.CmdType = ShellCommandType.TextShell Then
                    If TextEdit_Commands.ContainsKey(script.Cmd) Then
                        Wdbg("W", "Command {0} conflicts with available text shell commands. Appending ""-{1}-{2}"" to end of command...", script.Cmd, script.Name, script.ModPart)
                        script.Cmd += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                    End If
                ElseIf script.CmdType = ShellCommandType.TestShell Then
                    If Test_Commands.ContainsKey(script.Cmd) Then
                        Wdbg("W", "Command {0} conflicts with available text shell commands. Appending ""-{1}-{2}"" to end of command...", script.Cmd, script.Name, script.ModPart)
                        script.Cmd += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                    End If
                ElseIf script.CmdType = ShellCommandType.RemoteDebugShell Then
                    If DebugCommands.ContainsKey(script.Cmd) Then
                        Wdbg("W", "Command {0} conflicts with available remote debug shell commands. Appending ""-{1}-{2}"" to end of command...", script.Cmd, script.Name, script.ModPart)
                        script.Cmd += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                    End If
                ElseIf script.CmdType = ShellCommandType.ZIPShell Then
                    If ZipShell_Commands.ContainsKey(script.Cmd) Then
                        Wdbg("W", "Command {0} conflicts with available ZIP shell commands. Appending ""-{1}-{2}"" to end of command...", script.Cmd, script.Name, script.ModPart)
                        script.Cmd += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                    End If
                ElseIf script.CmdType = ShellCommandType.RSSShell Then
                    If RSSCommands.ContainsKey(script.Cmd) Then
                        Wdbg("W", "Command {0} conflicts with available RSS shell commands. Appending ""-{1}-{2}"" to end of command...", script.Cmd, script.Name, script.ModPart)
                        script.Cmd += "-{0}-{1}".FormatString(script.Name, script.ModPart)
                    End If
                End If

                'See if mod can be added to command list
                If script.Cmd <> "" And StartStop = True Then
                    If script.Def = "" Then
                        W(DoTranslation("No definition for command {0}."), True, ColTypes.Neutral, script.Cmd)
                        Wdbg("W", "{0}.Def = Nothing, {0}.Def = ""Command defined by {1} ({2})""", script.Cmd, script.Name, script.ModPart)
                        script.Def = DoTranslation("Command defined by ") + script.Name + " (" + script.ModPart + ")"
                    End If
                    Wdbg("I", "Command type: {0}", script.CmdType)
                    If script.CmdType = ShellCommandType.Shell Then
                        Wdbg("I", "Adding command {0} for main shell...", script.Cmd)
                        If Not modcmnds.Contains(script.Cmd) Then modcmnds.Add(script.Cmd)
                        moddefs.AddIfNotFound(script.Cmd, script.Def)
                    ElseIf script.CmdType = ShellCommandType.FTPShell Then
                        Wdbg("I", "Adding command {0} for FTP shell...", script.Cmd)
                        If Not FTPModCommands.Contains(script.Cmd) Then FTPModCommands.Add(script.Cmd)
                        FTPModDefs.AddIfNotFound(script.Cmd, script.Def)
                    ElseIf script.CmdType = ShellCommandType.MailShell Then
                        Wdbg("I", "Adding command {0} for mail shell...", script.Cmd)
                        If Not MailModCommands.Contains(script.Cmd) Then MailModCommands.Add(script.Cmd)
                        MailModDefs.AddIfNotFound(script.Cmd, script.Def)
                    ElseIf script.CmdType = ShellCommandType.SFTPShell Then
                        Wdbg("I", "Adding command {0} for SFTP shell...", script.Cmd)
                        If Not SFTPModCommands.Contains(script.Cmd) Then SFTPModCommands.Add(script.Cmd)
                        SFTPModDefs.AddIfNotFound(script.Cmd, script.Def)
                    ElseIf script.CmdType = ShellCommandType.TextShell Then
                        Wdbg("I", "Adding command {0} for text editor shell...", script.Cmd)
                        If Not TextEdit_ModCommands.Contains(script.Cmd) Then TextEdit_ModCommands.Add(script.Cmd)
                        TextEdit_ModHelpEntries.AddIfNotFound(script.Cmd, script.Def)
                    ElseIf script.CmdType = ShellCommandType.TestShell Then
                        Wdbg("I", "Adding command {0} for test shell...", script.Cmd)
                        If Not Test_ModCommands.Contains(script.Cmd) Then Test_ModCommands.Add(script.Cmd)
                        TestModDefs.AddIfNotFound(script.Cmd, script.Def)
                    ElseIf script.CmdType = ShellCommandType.RemoteDebugShell Then
                        Wdbg("I", "Adding command {0} for remote debug shell...", script.Cmd)
                        If Not DebugModCmds.Contains(script.Cmd) Then DebugModCmds.Add(script.Cmd)
                        RDebugModDefs.AddIfNotFound(script.Cmd, script.Def)
                    ElseIf script.CmdType = ShellCommandType.ZIPShell Then
                        Wdbg("I", "Adding command {0} for ZIP shell...", script.Cmd)
                        If Not ZipShell_ModCommands.Contains(script.Cmd) Then ZipShell_ModCommands.Add(script.Cmd)
                        ZipShell_ModHelpEntries.AddIfNotFound(script.Cmd, script.Def)
                    ElseIf script.CmdType = ShellCommandType.RSSShell Then
                        Wdbg("I", "Adding command {0} for RSS shell...", script.Cmd)
                        If Not RSSModCommands.Contains(script.Cmd) Then RSSModCommands.Add(script.Cmd)
                        RSSModDefs.AddIfNotFound(script.Cmd, script.Def)
                    End If
                End If

                'Raise event
                EventManager.RaiseModFinalized(StartStop, modFile)
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
        ParseMods(False)
        Wdbg("I", "All mods stopped.")

        'Start all mods
        ParseMods(True)
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