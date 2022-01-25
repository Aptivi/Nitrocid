
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

Imports System.CodeDom.Compiler
Imports System.IO
Imports System.Reflection
Imports Microsoft.CSharp
Imports KS.ManPages
Imports KS.Misc.JsonShell
Imports KS.Misc.Screensaver.Customized
Imports KS.Misc.Splash
Imports KS.Misc.TextEdit
Imports KS.Misc.Writers.MiscWriters
Imports KS.Misc.ZipFile
Imports KS.Network.FTP
Imports KS.Network.HTTP
Imports KS.Network.Mail
Imports KS.Network.RemoteDebug
Imports KS.Network.RSS
Imports KS.Network.SFTP
Imports KS.TestShell

Namespace Modifications
    Public Module ModParser

        ''' <summary>
        ''' Compiles the script and returns the instance of script interface
        ''' </summary>
        ''' <param name="PLang">Specified programming language for scripts (C# or VB.NET)</param>
        ''' <param name="code">Code blocks from script</param>
        Friend Function GenMod(PLang As String, code As String) As IScript
            'Check language
            Dim provider As CodeDomProvider
            Wdbg(DebugLevel.I, $"Language detected: {PLang}")
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
            Wdbg(DebugLevel.I, "Referenced assemblies will be added.")
            prm.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly.Location) 'It should reference itself
            prm.ReferencedAssemblies.Add("System.dll")
            prm.ReferencedAssemblies.Add("System.Core.dll")
            prm.ReferencedAssemblies.Add("System.Data.dll")
            prm.ReferencedAssemblies.Add("System.DirectoryServices.dll")
            prm.ReferencedAssemblies.Add("System.Xml.dll")
            prm.ReferencedAssemblies.Add("System.Xml.Linq.dll")
            Wdbg(DebugLevel.I, "Referenced assemblies added.")

            'Detect referenced assemblies from comments that start with "Reference GAC: <ref>" or "Reference File: <path/to/ref>".
            Dim References() As String = code.SplitNewLines.Select(Function(x) x).Where(Function(x) x.ContainsAnyOf({"Reference GAC: ", "Reference File: "})).ToArray
            Wdbg(DebugLevel.I, "Found {0} references (matches taken from searching for ""Reference GAC: "" or ""Reference File: "").", References.Length)
            For Each Reference As String In References
                Reference.RemoveNullsOrWhitespacesAtTheBeginning
                Wdbg(DebugLevel.I, "Reference line: {0}", Reference)
                Dim LocationCheckRequired As Boolean = Reference.Contains("Reference File: ")
                If (Reference.StartsWith("//") And PLang = "C#") Or (Reference.StartsWith("'") And PLang = "VB.NET") Then
                    'Remove comment mark
                    If Reference.StartsWith("//") Then Reference = Reference.Remove(0, 2)
                    If Reference.StartsWith("'") Then Reference = Reference.Remove(0, 1)

                    'Remove "Reference GAC: " or "Reference File: " and remove all whitespaces or nulls in the beginning
                    Reference = Reference.ReplaceAll({"Reference GAC: ", "Reference File: "}, "")
                    Reference.RemoveNullsOrWhitespacesAtTheBeginning
                    Wdbg(DebugLevel.I, "Final reference line: {0}", Reference)

                    'Add reference
                    If LocationCheckRequired Then
                        'Check to see if the reference file exists
                        If Not FileExists(Reference) Then
                            Wdbg(DebugLevel.E, "File {0} not found to reference.", Reference)
                            ReportProgress(DoTranslation("Referenced file {0} not found. This mod might not work properly without this file."), 0, ColTypes.Warning, Reference)
                        Else
                            prm.ReferencedAssemblies.Add(Reference)
                        End If
                    Else
                        prm.ReferencedAssemblies.Add(Reference)
                    End If
                End If
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
            Wdbg(DebugLevel.I, "Compiling...")
            Dim res As CompilerResults = provider.CompileAssemblyFromSource(prm, modCode)
#Enable Warning BC42104

            'Check to see if there are compilation errors
            Wdbg(DebugLevel.I, "Has errors: {0}", res.Errors.HasErrors)
            Wdbg(DebugLevel.I, "Has warnings: {0}", res.Errors.HasWarnings)
            If res.Errors.HasWarnings Then
                ReportProgress(DoTranslation("Mod can be loaded, but these warnings may impact the way the mod works:"), 0, ColTypes.Warning)
                Wdbg(DebugLevel.W, "Warnings when compiling:")
                For Each errorName As CompilerError In res.Errors
                    If errorName.IsWarning Then
                        ReportProgress(errorName.ToString, 0, ColTypes.Warning)
                        PrintLineWithHandleConditional(KernelBooted Or (Not KernelBooted And Not QuietKernel And Not EnableSplash), modCode(0).SplitNewLines, errorName.Line, errorName.Column, ColTypes.Warning)
                        Wdbg(DebugLevel.W, errorName.ToString)
                    End If
                Next
            End If
            If res.Errors.HasErrors Then
                ReportProgress(DoTranslation("Mod can't be loaded because of the following: "), 0, ColTypes.Error)
                Wdbg(DebugLevel.E, "Errors when compiling:")
                For Each errorName As CompilerError In res.Errors
                    If Not errorName.IsWarning Then
                        ReportProgress(errorName.ToString, 0, ColTypes.Error)
                        PrintLineWithHandleConditional(KernelBooted Or (Not KernelBooted And Not QuietKernel And Not EnableSplash), modCode(0).SplitNewLines, errorName.Line, errorName.Column, ColTypes.Error)
                        Wdbg(DebugLevel.E, errorName.ToString)
                    End If
                Next
                Exit Function
            End If

            'Make object type instance
            Wdbg(DebugLevel.I, "Creating instance of type...")
            Return GetModInstance(res.CompiledAssembly)
        End Function

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
        ''' <param name="modFile">Mod file name with extension. It should end with .vb or .cs</param>
        Sub ParseMod(modFile As String)
            modFile = Path.GetFileName(modFile)
            If modFile.EndsWith(".ss.vb") Then
                'Mod is a screensaver that has a language of VB.NET
                Wdbg(DebugLevel.W, "Mod file {0} is a screensaver. Language: VB.NET", modFile)
                CompileCustom(modFile)
            ElseIf modFile.EndsWith(".ss.cs") Then
                'Mod is a screensaver that has a language of C#
                Wdbg(DebugLevel.W, "Mod file {0} is a screensaver. Language: C#", modFile)
                CompileCustom(modFile)
            ElseIf modFile.EndsWith(".cs") Then
                'Mod has a language of C#
                Wdbg(DebugLevel.I, "Mod language is C# from extension "".cs""")
                Dim script As IScript = GenMod("C#", File.ReadAllText(ModPath + modFile))
                FinalizeMods(script, modFile)
            ElseIf modFile.EndsWith(".vb") Then
                'Mod has a language of VB.NET
                Wdbg(DebugLevel.I, "Mod language is VB.NET from extension "".vb""")
                Dim script As IScript = GenMod("VB.NET", File.ReadAllText(ModPath + modFile))
                FinalizeMods(script, modFile)
            ElseIf modFile.EndsWith(".dll") Then
                'Mod is a dynamic DLL
                Try
                    Dim script As IScript = GetModInstance(Assembly.LoadFrom(ModPath + modFile))
                    If script Is Nothing Then CompileCustom(ModPath + modFile)
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
                End Try
            Else
                'Ignore all mods that its file name doesn't end with .vb
                Wdbg(DebugLevel.W, "Unsupported file type for mod file {0}.", modFile)
            End If
        End Sub

        ''' <summary>
        ''' Configures the mod so it can be used
        ''' </summary>
        ''' <param name="script">Instance of script</param>
        ''' <param name="modFile">Mod file name with extension. It should end with .vb, .ss.vb, .ss.cs, or .cs</param>
        Sub FinalizeMods(script As IScript, modFile As String)
            Dim ModParts As New Dictionary(Of String, PartInfo)
            Dim ModInstance As ModInfo
            Dim PartInstance As PartInfo

            'Try to finalize mod
            If script IsNot Nothing Then
                Kernel.KernelEventManager.RaiseModParsed(modFile)
                Try
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

                    'See if the mod part conflicts with existing parts
                    Wdbg(DebugLevel.I, "Checking to see if {0} exists in scripts...", ModName)
                    If Mods.ContainsKey(ModName) Then
                        'The mod already exists. Add mod part to existing mod.
                        Wdbg(DebugLevel.I, "Exists. Adding mod part {0}...", script.ModPart)
                        If Not Mods(ModName).ModParts.ContainsKey(script.ModPart) Then
                            Wdbg(DebugLevel.I, "No conflict with {0}. Adding as is...", script.ModPart)
                            PartInstance = New PartInfo(ModName, script.ModPart, modFile, NeutralizePath(modFile, ModPath), script)
                            Mods(ModName).ModParts.Add(script.ModPart, PartInstance)
                        Else
                            Wdbg(DebugLevel.W, "There is a conflict with {0}. Appending item number...", script.ModPart)
                            script.ModPart += CStr(Mods(ModName).ModParts.Count)
                            PartInstance = New PartInfo(ModName, script.ModPart, modFile, NeutralizePath(modFile, ModPath), script)
                            Mods(ModName).ModParts.Add(script.ModPart, PartInstance)
                        End If
                    Else
                        'The mod wasn't existent. Add mod part to new entry of mod.
                        Wdbg(DebugLevel.I, "Adding mod with mod part {0}...", script.ModPart)
                        If Not ModParts.ContainsKey(script.ModPart) Then
                            Wdbg(DebugLevel.I, "No conflict with {0}. Adding as is...", script.ModPart)
                            PartInstance = New PartInfo(ModName, script.ModPart, modFile, NeutralizePath(modFile, ModPath), script)
                            ModParts.Add(script.ModPart, PartInstance)
                        Else
                            Wdbg(DebugLevel.W, "There is a conflict with {0}. Appending item number...", script.ModPart)
                            script.ModPart += CStr(Mods.Count)
                            PartInstance = New PartInfo(ModName, script.ModPart, modFile, NeutralizePath(modFile, ModPath), script)
                            ModParts.Add(script.ModPart, PartInstance)
                        End If
                        ModInstance = New ModInfo(ModName, modFile, NeutralizePath(modFile, ModPath), ModParts, script.Version)
                        Mods.Add(ModName, ModInstance)
                    End If

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
                            Select Case script.Commands(Command).Type
                                Case ShellType.Shell
                                    If Shell.Shell.Commands.ContainsKey(Command) Or ModCommands.Contains(Command) Then
                                        Wdbg(DebugLevel.W, "Command {0} conflicts with available shell commands or mod commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                        Command += $"-{script.Name}-{script.ModPart}"
                                    End If
                                Case ShellType.FTPShell
                                    If FTPCommands.ContainsKey(Command) Or FTPModCommands.Contains(Command) Then
                                        Wdbg(DebugLevel.W, "Command {0} conflicts with available FTP shell commands or mod commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                        Command += $"-{script.Name}-{script.ModPart}"
                                    End If
                                Case ShellType.MailShell
                                    If MailCommands.ContainsKey(Command) Or MailModCommands.Contains(Command) Then
                                        Wdbg(DebugLevel.W, "Command {0} conflicts with available mail shell commands or mod commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                        Command += $"-{script.Name}-{script.ModPart}"
                                    End If
                                Case ShellType.SFTPShell
                                    If SFTPCommands.ContainsKey(Command) Or SFTPModCommands.Contains(Command) Then
                                        Wdbg(DebugLevel.W, "Command {0} conflicts with available SFTP shell commands or mod commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                        Command += $"-{script.Name}-{script.ModPart}"
                                    End If
                                Case ShellType.TextShell
                                    If TextEdit_Commands.ContainsKey(Command) Or TextEdit_ModCommands.Contains(Command) Then
                                        Wdbg(DebugLevel.W, "Command {0} conflicts with available text shell commands or mod commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                        Command += $"-{script.Name}-{script.ModPart}"
                                    End If
                                Case ShellType.TestShell
                                    If Test_Commands.ContainsKey(Command) Or Test_ModCommands.Contains(Command) Then
                                        Wdbg(DebugLevel.W, "Command {0} conflicts with available text shell commands or mod commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                        Command += $"-{script.Name}-{script.ModPart}"
                                    End If
                                Case ShellType.RemoteDebugShell
                                    If DebugCommands.ContainsKey(Command) Or DebugModCmds.Contains(Command) Then
                                        Wdbg(DebugLevel.W, "Command {0} conflicts with available remote debug shell commands or mod commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                        Command += $"-{script.Name}-{script.ModPart}"
                                    End If
                                Case ShellType.ZIPShell
                                    If ZipShell_Commands.ContainsKey(Command) Or ZipShell_ModCommands.Contains(Command) Then
                                        Wdbg(DebugLevel.W, "Command {0} conflicts with available ZIP shell commands or mod commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                        Command += $"-{script.Name}-{script.ModPart}"
                                    End If
                                Case ShellType.RSSShell
                                    If RSSCommands.ContainsKey(Command) Or RSSModCommands.Contains(Command) Then
                                        Wdbg(DebugLevel.W, "Command {0} conflicts with available RSS shell commands or mod commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                        Command += $"-{script.Name}-{script.ModPart}"
                                    End If
                                Case ShellType.JsonShell
                                    If JsonShell_Commands.ContainsKey(Command) Or JsonShell_ModCommands.Contains(Command) Then
                                        Wdbg(DebugLevel.W, "Command {0} conflicts with available JSON shell commands or mod commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                        Command += $"-{script.Name}-{script.ModPart}"
                                    End If
                                Case ShellType.HTTPShell
                                    If HTTPCommands.ContainsKey(Command) Or HTTPModCommands.Contains(Command) Then
                                        Wdbg(DebugLevel.W, "Command {0} conflicts with available HTTP shell commands or mod commands. Appending ""-{1}-{2}"" to end of command...", Command, script.Name, script.ModPart)
                                        Command += $"-{script.Name}-{script.ModPart}"
                                    End If
                            End Select

                            'See if mod can be added to command list
                            If Command <> "" Then
                                If script.Commands(ActualCommand).HelpDefinition = "" Then
                                    ReportProgress(DoTranslation("No definition for command {0}."), 0, ColTypes.Warning, Command)
                                    Wdbg(DebugLevel.W, "{0}.Def = Nothing, {0}.Def = ""Command defined by {1} ({2})""", Command, script.Name, script.ModPart)
                                    script.Commands(ActualCommand).HelpDefinition = DoTranslation("Command defined by ") + script.Name + " (" + script.ModPart + ")"
                                End If

                                Wdbg(DebugLevel.I, "Command type: {0}", script.Commands(ActualCommand).Type)
                                Select Case script.Commands(ActualCommand).Type
                                    Case ShellType.Shell
                                        Wdbg(DebugLevel.I, "Adding command {0} for main shell...", Command)
                                        If Not ModCommands.Contains(Command) Then ModCommands.Add(Command)
                                        script.Commands.RenameKey(ActualCommand, Command)
                                        ModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                    Case ShellType.FTPShell
                                        Wdbg(DebugLevel.I, "Adding command {0} for FTP shell...", Command)
                                        If Not FTPModCommands.Contains(Command) Then FTPModCommands.Add(Command)
                                        script.Commands.RenameKey(ActualCommand, Command)
                                        FTPModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                    Case ShellType.MailShell
                                        Wdbg(DebugLevel.I, "Adding command {0} for mail shell...", Command)
                                        If Not MailModCommands.Contains(Command) Then MailModCommands.Add(Command)
                                        script.Commands.RenameKey(ActualCommand, Command)
                                        MailModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                    Case ShellType.SFTPShell
                                        Wdbg(DebugLevel.I, "Adding command {0} for SFTP shell...", Command)
                                        If Not SFTPModCommands.Contains(Command) Then SFTPModCommands.Add(Command)
                                        script.Commands.RenameKey(ActualCommand, Command)
                                        SFTPModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                    Case ShellType.TextShell
                                        Wdbg(DebugLevel.I, "Adding command {0} for text editor shell...", Command)
                                        If Not TextEdit_ModCommands.Contains(Command) Then TextEdit_ModCommands.Add(Command)
                                        script.Commands.RenameKey(ActualCommand, Command)
                                        TextEdit_ModHelpEntries.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                    Case ShellType.TestShell
                                        Wdbg(DebugLevel.I, "Adding command {0} for test shell...", Command)
                                        If Not Test_ModCommands.Contains(Command) Then Test_ModCommands.Add(Command)
                                        script.Commands.RenameKey(ActualCommand, Command)
                                        TestModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                    Case ShellType.RemoteDebugShell
                                        Wdbg(DebugLevel.I, "Adding command {0} for remote debug shell...", Command)
                                        If Not DebugModCmds.Contains(Command) Then DebugModCmds.Add(Command)
                                        script.Commands.RenameKey(ActualCommand, Command)
                                        RDebugModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                    Case ShellType.ZIPShell
                                        Wdbg(DebugLevel.I, "Adding command {0} for ZIP shell...", Command)
                                        If Not ZipShell_ModCommands.Contains(Command) Then ZipShell_ModCommands.Add(Command)
                                        script.Commands.RenameKey(ActualCommand, Command)
                                        ZipShell_ModHelpEntries.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                    Case ShellType.RSSShell
                                        Wdbg(DebugLevel.I, "Adding command {0} for RSS shell...", Command)
                                        If Not RSSModCommands.Contains(Command) Then RSSModCommands.Add(Command)
                                        script.Commands.RenameKey(ActualCommand, Command)
                                        RSSModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                    Case ShellType.JsonShell
                                        Wdbg(DebugLevel.I, "Adding command {0} for JSON shell...", Command)
                                        If Not JsonShell_ModCommands.Contains(Command) Then JsonShell_ModCommands.Add(Command)
                                        script.Commands.RenameKey(ActualCommand, Command)
                                        JsonShell_ModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                    Case ShellType.HTTPShell
                                        Wdbg(DebugLevel.I, "Adding command {0} for HTTP shell...", Command)
                                        If Not HTTPModCommands.Contains(Command) Then HTTPModCommands.Add(Command)
                                        script.Commands.RenameKey(ActualCommand, Command)
                                        HTTPModDefs.AddIfNotFound(Command, script.Commands(Command).HelpDefinition)
                                End Select
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
                    Kernel.KernelEventManager.RaiseModFinalized(modFile)
                Catch ex As Exception
                    Kernel.KernelEventManager.RaiseModFinalizationFailed(modFile, ex.Message)
                    WStkTrc(ex)
                    ReportProgress(DoTranslation("Failed to finalize mod {0}: {1}"), 0, ColTypes.Error, modFile, ex.Message)
                End Try
            Else
                Kernel.KernelEventManager.RaiseModParseError(modFile)
            End If
        End Sub

    End Module
End Namespace