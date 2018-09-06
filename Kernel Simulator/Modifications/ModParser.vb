
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Imports Microsoft.VisualBasic
Imports System.CodeDom.Compiler
Imports System.Reflection

Public Module ModParser

    Private DoneFlag As Boolean = False
    Public Interface IScript
        Sub StartMod()
        Sub StopMod()
        Property Cmd As String
        Property Def As String
        Sub PerformCmd(Optional ByVal args As String = "")
    End Interface
    Public scripts As New Dictionary(Of String, IScript)

    Private Function GenMod(ByVal code As String) As IScript
        Using provider As New VBCodeProvider()
            Dim prm As New CompilerParameters()
            prm.GenerateExecutable = False
            prm.GenerateInMemory = True
            prm.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly.Location)
            prm.ReferencedAssemblies.Add("System.dll")
            prm.ReferencedAssemblies.Add("System.Core.dll")
            prm.ReferencedAssemblies.Add("System.Data.dll")
            prm.ReferencedAssemblies.Add("System.DirectoryServices.dll")
            prm.ReferencedAssemblies.Add("System.Xml.dll")
            prm.ReferencedAssemblies.Add("System.Xml.Linq.dll")
            Dim namespc As String = GetType(IScript).Namespace
            Dim modCode() As String = New String() {"Imports " & namespc & vbNewLine & code}
            Dim res As CompilerResults = provider.CompileAssemblyFromSource(prm, modCode)
            If (res.Errors.HasErrors) And (Quiet = False) Then
                Wln("Mod can't be loaded because of the following: ", "neutralText")
                For Each errorName In res.Errors
                    Wln(errorName.ToString, "neutralText") : Wdbg(errorName.ToString, True)
                Next
                Exit Function
            Else
                DoneFlag = True
            End If
            For Each t As Type In res.CompiledAssembly.GetTypes()
                If t.GetInterface(GetType(IScript).Name) IsNot Nothing Then Return CType(res.CompiledAssembly.CreateInstance(t.Name), IScript)
            Next
        End Using
    End Function

    Sub ParseMods()

        Dim modPath As String = Environ("USERPROFILE") + "\KSMods\"
        If Not FileIO.FileSystem.DirectoryExists(modPath) Then FileIO.FileSystem.CreateDirectory(modPath)
        If (Quiet = False) And (FileIO.FileSystem.GetFiles(modPath).Count <> 0) Then Wln("mod: Loading mods...", "neutralText")
        For Each modFile As String In FileIO.FileSystem.GetFiles(modPath)
            modFile = modFile.Replace(modPath, "")
            If Not modFile.EndsWith(".m") Then
                'Ignore all mods who doesn't end with .m
                Wdbg("Unsupported file type for mod file {0}.", True, modFile)
            ElseIf modFile.EndsWith("SS.m") Then
                'Ignore all mods who ends with SS.m
                Wdbg("Mod file {0} is a screensaver and is ignored.", True, modFile)
            Else
                Dim script As IScript = GenMod(IO.File.ReadAllText(modPath + modFile))
                If (DoneFlag = True) Then script.StartMod()
                If (script.Cmd <> "") Then
                    scripts.Add(script.Cmd, script)
                    modcmnds.Add(script.Cmd)
                    If (script.Def = "") Then
                        Wln("No definition for command {0}.", "neutralText", script.Cmd)
                        Wdbg("{0}.Def = (""{1}"" = """"), {0}.Def = ""Command defined by mod""", True, script.Cmd, script.Def)
                        script.Def = "Command defined by mod"
                    End If
                    moddefs.Add(script.Cmd, script.Def)
                End If
            End If
        Next

    End Sub

End Module
