
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
        Property CmdType As ModType
        ''' <summary>
        ''' Command definition for mod
        ''' </summary>
        Property Def As String
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
        ''' <param name="ev">Event name. Look it up on <see cref="EventsAndExceptions"/></param>
        Sub InitEvents(ByVal ev As String)
        ''' <summary>
        ''' Code executed when initializing events
        ''' </summary>
        ''' <param name="ev">Event name. Look it up on <see cref="EventsAndExceptions"/></param>
        ''' <param name="Args">Arguments.</param>
        Sub InitEvents(ByVal ev As String, ParamArray Args() As Object)
    End Interface

    'Variables
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
            W(DoTranslation("Mod can't be loaded because of the following: ", currentLang), True, ColTypes.Err)
            For Each errorName In res.Errors
                W(errorName.ToString, True, ColTypes.Err)
                Wdbg("E", errorName.ToString)
            Next
            Exit Function
        End If

        'Make object type instance
        For Each t As Type In res.CompiledAssembly.GetTypes()
            If t.GetInterface(GetType(IScript).Name) IsNot Nothing Then Return CType(res.CompiledAssembly.CreateInstance(t.Name), IScript)
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
            If (count <> 0) And StartStop = True Then
                W(DoTranslation("mod: Loading mods...", currentLang), True, ColTypes.Neutral)
                Wdbg("I", "Mods are being loaded. Total mods with screensavers = {0}", count)
            ElseIf (count <> 0) And StartStop = False Then
                W(DoTranslation("mod: Stopping mods...", currentLang), True, ColTypes.Neutral)
                Wdbg("I", "Mods are being stopped. Total mods with screensavers = {0}", count)
            End If
            If StartStop = False Then
                For Each script As Dictionary(Of String, IScript) In scripts.Values
                    Wdbg("I", "Stopping... Mod name: {0}", scripts.GetKeyFromValue(script))
                    For Each ScriptPart As String In script.Keys
                        Wdbg("I", "Stopping part {0} v{1}", script(ScriptPart).ModPart, script(ScriptPart).Version)
                        script(ScriptPart).StopMod()
                        If script(ScriptPart).Name <> "" And script(ScriptPart).Version <> "" Then
                            W(DoTranslation("{0} v{1} stopped", currentLang), True, ColTypes.Neutral, script(ScriptPart).ModPart, script(ScriptPart).Version)
                        End If
                    Next
                    W(DoTranslation("Mod {0} stopped", currentLang), True, ColTypes.Neutral, scripts.GetKeyFromValue(script))
                Next
            Else
                For Each modFile As String In FileIO.FileSystem.GetFiles(modPath)
                    StartParse(modFile.Replace("\", "/"), StartStop)
                Next
            End If
        Else
            W(DoTranslation("Parsing mods not allowed on safe mode.", currentLang), True, ColTypes.Err)
        End If
    End Sub

    ''' <summary>
    ''' Starts to parse the mod, and configures it so it can be used
    ''' </summary>
    ''' <param name="modFile">Mod file name with extension. It should end with .m or CS.m</param>
    ''' <param name="StartStop">Whether to start or stop mods</param>
    Sub StartParse(ByVal modFile As String, Optional ByVal StartStop As Boolean = True)
        modFile = modFile.Replace(modPath, "")
        If Not modFile.EndsWith(".m") Then
            'Ignore all mods that its file name doesn't end with .m
            Wdbg("W", "Unsupported file type for mod file {0}.", modFile)
        ElseIf modFile.EndsWith("SS.m") Then
            'Ignore all mods that its file name ends with SS.m
            Wdbg("W", "Mod file {0} is a screensaver and is ignored.", modFile)
        ElseIf modFile.EndsWith("CS.m") Then
            'Mod has a language of C#
            Dim script As IScript = GenMod("C#", IO.File.ReadAllText(modPath + modFile))
            FinalizeMods(script, modFile, StartStop)
        Else
            Dim script As IScript = GenMod("VB.NET", IO.File.ReadAllText(modPath + modFile))
            FinalizeMods(script, modFile, StartStop)
        End If
    End Sub

    '------------------------------------------- Finalizer -------------------------------------------
    ''' <summary>
    ''' Configures the mod so it can be used
    ''' </summary>
    ''' <param name="script">Instance of script</param>
    ''' <param name="modFile">Mod file name with extension. It should end with .m, SS.m, or CS.m</param>
    ''' <param name="StartStop">Whether to start or stop mods</param>
    Sub FinalizeMods(ByVal script As IScript, ByVal modFile As String, Optional ByVal StartStop As Boolean = True)
        Dim ModParts As New Dictionary(Of String, IScript)
        If Not IsNothing(script) Then
            script.StartMod()
            Wdbg("I", "script.StartMod() initialized. Mod name: {0} | Mod part: {1} | Version: {2}", script.Name, script.ModPart, script.Version)
            If script.ModPart = "" Then
                Wdbg("W", "No part name for {0}", modFile)
                W(DoTranslation("Mod {0} does not have the part name. Mod parsing failed. Review the source code.", currentLang), True, ColTypes.Err, modFile)
                Exit Sub
            End If
            If script.Cmd = "" Then
                Wdbg("W", "No command for {0}", modFile)
                W(DoTranslation("Mod {0} does not have the command. Mod parsing failed. Review the source code.", currentLang), True, ColTypes.Err, modFile)
                Exit Sub
            End If
            If script.Name = "" Then
                Wdbg("W", "No name for {0}", modFile)
                W(DoTranslation("Mod {0} does not have the name. Review the source code.", currentLang), True, ColTypes.Neutral, modFile)
                Wdbg("I", "Checking to see if {0} exists in scripts...", script.Cmd)
                If scripts.ContainsKey(script.Cmd) Then
                    Wdbg("I", "Exists. Adding mod part {0}...", script.ModPart)
                    scripts(script.Cmd).Add(script.ModPart, script)
                Else
                    Wdbg("I", "Adding mod with mod part {0}...", script.ModPart)
                    ModParts.Add(script.ModPart, script)
                    scripts.Add(script.Cmd, ModParts)
                End If
            Else
                Wdbg("I", "There is a name for {0}", modFile)
                Wdbg("I", "Checking to see if {0} exists in scripts...", script.Name)
                If scripts.ContainsKey(script.Name) Then
                    Wdbg("I", "Exists. Adding mod part {0}...", script.ModPart)
                    scripts(script.Name).Add(script.ModPart, script)
                Else
                    Wdbg("I", "Adding mod with mod part {0}...", script.ModPart)
                    ModParts.Add(script.ModPart, script)
                    scripts.Add(script.Name, ModParts)
                End If
            End If
            If script.Version = "" And script.Name <> "" Then
                Wdbg("I", "{0}.Version = """" | {0}.Name = {1}", modFile, script.Name)
                W(DoTranslation("Mod {0} does not have the version.", currentLang), True, ColTypes.Neutral, script.Name)
            ElseIf script.Name <> "" And script.Version <> "" Then
                Wdbg("I", "{0}.Version = {2} | {0}.Name = {1}", modFile, script.Name, script.Version)
                W(DoTranslation("{0} v{1} started", currentLang) + " ({2})", True, ColTypes.Neutral, script.Name, script.Version, script.ModPart)
            End If
            If script.Cmd <> "" And StartStop = True Then
                If script.Def = "" Then
                    W(DoTranslation("No definition for command {0}.", currentLang), True, ColTypes.Neutral, script.Cmd)
                    Wdbg("W", "{0}.Def = Nothing, {0}.Def = ""Command defined by {1} ({2})""", script.Cmd, script.Name, script.ModPart)
                    script.Def = DoTranslation("Command defined by ", currentLang) + script.Name + " (" + script.ModPart + ")"
                End If
                If script.CmdType = ModType.Shell Then
                    If Not modcmnds.Contains(script.Cmd) Then modcmnds.Add(script.Cmd)
                    moddefs.AddIfNotFound(script.Cmd, script.Def)
                ElseIf script.CmdType = ModType.FTPShell Then
                    If Not FTPModCommands.Contains(script.Cmd) Then FTPModCommands.Add(script.Cmd)
                    FTPModDefs.AddIfNotFound(script.Cmd, script.Def)
                ElseIf script.CmdType = ModType.MailShell Then
                    If Not MailModCommands.Contains(script.Cmd) Then MailModCommands.Add(script.Cmd)
                    MailModDefs.AddIfNotFound(script.Cmd, script.Def)
                ElseIf script.CmdType = ModType.SFTPShell Then
                    If Not SFTPModCommands.Contains(script.Cmd) Then SFTPModCommands.Add(script.Cmd)
                    SFTPModDefs.AddIfNotFound(script.Cmd, script.Def)
                ElseIf script.CmdType = ModType.TextShell Then
                    If Not TextEdit_ModCommands.Contains(script.Cmd) Then TextEdit_ModCommands.Add(script.Cmd)
                    TextEdit_ModHelpEntries.AddIfNotFound(script.Cmd, script.Def)
                End If
            End If
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
        FTPModCommands.Clear()
        FTPModDefs.Clear()
        MailModCommands.Clear()
        MailModDefs.Clear()
        TextEdit_ModCommands.Clear()
        TextEdit_ModHelpEntries.Clear()
        scripts.Clear()

        'Stop all mods
        ParseMods(False)

        'Start all mods
        ParseMods(True)
        Dim modPath As String = paths("Mods")
        For Each modFile As String In FileIO.FileSystem.GetFiles(modPath)
            Wdbg("I", "Reloading mod {0}", modFile.Replace(modPath, ""))
            CompileCustom(modFile.Replace(modPath, ""))
        Next
    End Sub

    ''' <summary>
    ''' Reloads all generic definitions so it can be updated with language change
    ''' </summary>
    ''' <param name="OldModDesc">Old mod command description</param>
    Sub ReloadGenericDefs(ByVal OldModDesc As String)
        For i As Integer = 0 To moddefs.Keys.Count - 1
            Dim Cmd As String = moddefs.Keys(i)
            If moddefs(Cmd).Contains(OldModDesc) Then
                moddefs(Cmd) = moddefs(Cmd).Replace(OldModDesc, DoTranslation("Command defined by ", currentLang))
            End If
        Next
    End Sub

End Module