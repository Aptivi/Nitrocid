
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
Imports System.Reflection
Imports Microsoft.CSharp
Imports KS.Misc.Splash
Imports KS.Misc.Writers.MiscWriters

Namespace Misc.Screensaver.Customized
    Public Module CustomSaverCompiler

        ''' <summary>
        ''' Compiles the custom screensaver file and configures it so it can be viewed
        ''' </summary>
        ''' <param name="file">File name with .ss.vb</param>
        Public Sub CompileCustom(file As String)
            'Initialize path
            Dim DoneFlag As Boolean
            Dim modPath As String = GetKernelPath(KernelPathType.Mods)
            file = file.Replace("\", "/").Replace(modPath, "")

            'Start parsing screensaver
            If FileExists(modPath + file) Then
                Wdbg(DebugLevel.I, "Parsing {0}...", file)
                If file.EndsWith(".ss.vb") Or file.EndsWith(".ss.cs") Or file.EndsWith(".dll") Then
                    Wdbg(DebugLevel.W, "{0} is a valid screensaver. Generating...", file)
                    If file.EndsWith(".ss.vb") Then
                        CustomSaver = GenSaver("VB.NET", IO.File.ReadAllText(modPath + file), DoneFlag)
                    ElseIf file.EndsWith(".ss.cs") Then
                        CustomSaver = GenSaver("C#", IO.File.ReadAllText(modPath + file), DoneFlag)
                    ElseIf file.EndsWith(".dll") Then
                        Try
                            CustomSaver = GetScreensaverInstance(Assembly.LoadFrom(modPath + file))
                            DoneFlag = True
                        Catch ex As ReflectionTypeLoadException
                            Wdbg(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", file, ex.Message)
                            WStkTrc(ex)
                            ReportProgress(DoTranslation("Screensaver can't be loaded because of the following: "), 0, ColTypes.Error)
                            For Each LoaderException As Exception In ex.LoaderExceptions
                                Wdbg(DebugLevel.E, "Loader exception: {0}", LoaderException.Message)
                                WStkTrc(LoaderException)
                                ReportProgress(LoaderException.Message, 0, ColTypes.Error)
                            Next
                        End Try
                    End If
                    If DoneFlag Then
                        Wdbg(DebugLevel.I, "{0} compiled correctly. Starting...", file)
                        CustomSaver.InitSaver()
                        Dim SaverName As String = CustomSaver.SaverName
                        Dim SaverInstance As CustomSaverInfo
                        If CustomSaver.Initialized = True Then
                            'Check to see if the screensaver is already found
                            Dim IsFound As Boolean
                            If Not SaverName = "" Then
                                IsFound = CustomSavers.ContainsKey(SaverName)
                            Else
                                IsFound = CustomSavers.ContainsKey(file)
                            End If
                            Wdbg(DebugLevel.I, "Is screensaver found? {0}", IsFound)
                            If Not IsFound Then
                                If Not SaverName = "" Then
                                    ReportProgress(DoTranslation("{0} has been initialized properly."), 0, ColTypes.Neutral, SaverName)
                                    Wdbg(DebugLevel.I, "{0} ({1}) compiled correctly. Starting...", SaverName, file)
                                    SaverInstance = New CustomSaverInfo(SaverName, file, NeutralizePath(file, modPath), CustomSaver)
                                    CustomSavers.Add(SaverName, SaverInstance)
                                Else
                                    ReportProgress(DoTranslation("{0} has been initialized properly."), 0, ColTypes.Neutral, file)
                                    Wdbg(DebugLevel.I, "{0} compiled correctly. Starting...", file)
                                    SaverInstance = New CustomSaverInfo(SaverName, file, NeutralizePath(file, modPath), CustomSaver)
                                    CustomSavers.Add(file, SaverInstance)
                                End If
                            Else
                                If Not SaverName = "" Then
                                    Wdbg(DebugLevel.W, "{0} ({1}) already exists. Recompiling...", SaverName, file)
                                    CustomSavers.Remove(SaverName)
                                    CompileCustom(file)
                                    Exit Sub
                                Else
                                    Wdbg(DebugLevel.W, "{0} already exists. Recompiling...", file)
                                    CustomSavers.Remove(file)
                                    CompileCustom(file)
                                    Exit Sub
                                End If
                            End If
                            InitializeCustomSaverSettings()
                            AddCustomSaverToSettings(If(SaverName = "", file, SaverName))
                        Else
                            If Not SaverName = "" Then
                                ReportProgress(DoTranslation("{0} did not initialize. The screensaver code might have experienced an error while initializing."), 0, ColTypes.Error, SaverName)
                                Wdbg(DebugLevel.W, "{0} ({1}) is compiled, but not initialized.", SaverName, file)
                            Else
                                ReportProgress(DoTranslation("{0} did not initialize. The screensaver code might have experienced an error while initializing."), 0, ColTypes.Error, file)
                                Wdbg(DebugLevel.W, "{0} is compiled, but not initialized.", file)
                            End If
                        End If
                    End If
                Else
                    Wdbg(DebugLevel.W, "{0} is not a screensaver. A screensaver code should have "".ss.vb"" or "".dll"" at the end.", file)
                End If
            Else
                ReportProgress(DoTranslation("Screensaver {0} does not exist."), 0, ColTypes.Error, file)
                Wdbg(DebugLevel.E, "The file {0} does not exist for compilation.", file)
            End If
        End Sub

        ''' <summary>
        ''' Compiles the screensaver and returns the instance of custom saver interface
        ''' </summary>
        ''' <param name="PLang">Specified programming language for scripts (C# or VB.NET)</param>
        ''' <param name="code">Screensaver code</param>
        ''' <returns>Interface of the compiled custom saver</returns>
        Function GenSaver(PLang As String, code As String, ByRef DoneFlag As Boolean) As ICustomSaver
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

            Using provider
                'Declare new compiler parameter object
                Dim prm As New CompilerParameters With {
                .GenerateExecutable = False,
                .GenerateInMemory = True
            }

                'Add referenced assemblies
                Wdbg(DebugLevel.I, "Referenced assemblies will be added.")
                prm.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly.Location)
                prm.ReferencedAssemblies.Add("System.dll")
                prm.ReferencedAssemblies.Add("System.Core.dll")
                prm.ReferencedAssemblies.Add("System.Data.dll")
                prm.ReferencedAssemblies.Add("System.DirectoryServices.dll")
                prm.ReferencedAssemblies.Add("System.Xml.dll")
                prm.ReferencedAssemblies.Add("System.Xml.Linq.dll")
                Wdbg(DebugLevel.I, "All referenced assemblies prepared.")

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
                Dim namespc As String = GetType(ICustomSaver).Namespace
                Dim modCode() As String = {}
                If PLang = "VB.NET" Then
                    modCode = {$"Imports {namespc}{vbNewLine}{code}"}
                ElseIf PLang = "C#" Then
                    modCode = {$"using {namespc};{vbNewLine}{code}"}
                End If
                Wdbg(DebugLevel.I, "Compiling...")
                Dim execCustomSaver As CompilerResults = provider.CompileAssemblyFromSource(prm, modCode)

                'Check to see if there are compilation errors
                Wdbg(DebugLevel.I, "Compilation results: Errors? {0}, Warnings? {1} | Total: {2}", execCustomSaver.Errors.HasErrors, execCustomSaver.Errors.HasWarnings, execCustomSaver.Errors.Count)
                If execCustomSaver.Errors.HasWarnings Then
                    ReportProgress(DoTranslation("Screensaver can be loaded, but these warnings may impact the way the screensaver works:"), 0, ColTypes.Warning)
                    Wdbg(DebugLevel.W, "Warnings when compiling:")
                    For Each errorName As CompilerError In execCustomSaver.Errors
                        If errorName.IsWarning Then
                            ReportProgress(errorName.ToString, 0, ColTypes.Warning)
                            PrintLineWithHandleConditional(KernelBooted Or (Not KernelBooted And (Not QuietKernel Or Not EnableSplash)), modCode(0).SplitNewLines, errorName.Line, errorName.Column, ColTypes.Warning)
                            Wdbg(DebugLevel.W, errorName.ToString)
                        End If
                    Next
                End If
                If execCustomSaver.Errors.HasErrors Then
                    ReportProgress(DoTranslation("Screensaver can't be loaded because of the following: "), 0, ColTypes.Error)
                    Wdbg(DebugLevel.E, "Errors when compiling:")
                    For Each errorName As CompilerError In execCustomSaver.Errors
                        If Not errorName.IsWarning Then
                            ReportProgress(errorName.ToString, 0, ColTypes.Error)
                            PrintLineWithHandleConditional(KernelBooted Or (Not KernelBooted And (Not QuietKernel Or Not EnableSplash)), modCode(0).SplitNewLines, errorName.Line, errorName.Column, ColTypes.Error)
                            Wdbg(DebugLevel.E, errorName.ToString)
                        End If
                    Next
                    Exit Function
                Else
                    DoneFlag = True
                End If

                'Make object type instance
                Wdbg(DebugLevel.I, "Creating instance of type...")
                Return GetScreensaverInstance(execCustomSaver.CompiledAssembly)
            End Using
        End Function

    End Module
End Namespace