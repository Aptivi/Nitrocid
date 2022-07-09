
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

Imports System.Reflection
Imports KS.Misc.Splash
Imports System.IO

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
                Dim ScreensaverBase As BaseScreensaver
                If Path.GetExtension(file) = ".dll" Then
                    'Try loading the screensaver
                    Try
                        Wdbg(DebugLevel.I, "{0} is probably a valid screensaver. Generating...", file)
                        ScreensaverBase = GetScreensaverInstance(Assembly.LoadFrom(modPath + file))
                        If ScreensaverBase IsNot Nothing Then
                            'This screensaver uses the modern BaseScreensaver and IScreensaver interfaces
                            Wdbg(DebugLevel.I, "{0} is a valid screensaver!", file)
                            ReportProgress(DoTranslation("{0} has been initialized properly."), 0, ColTypes.Neutral, file)
                            Dim SaverName As String = ScreensaverBase.ScreensaverName
                            Dim SaverInstance As CustomSaverInfo
                            SaverInstance = New CustomSaverInfo(SaverName, file, NeutralizePath(file, modPath), Nothing, ScreensaverBase)
                            CustomSavers.Add(file, SaverInstance)
                        Else
                            CustomSaver = GetScreensaverInstanceLegacy(Assembly.LoadFrom(modPath + file))
                            Wdbg(DebugLevel.I, "Is {0} actually a valid screensaver? {1}", file, CustomSaver IsNot Nothing)
                            If CustomSaver IsNot Nothing Then DoneFlag = True
                        End If
                    Catch ex As ReflectionTypeLoadException
                        Wdbg(DebugLevel.E, "Error trying to load dynamic screensaver {0} because of reflection failure: {1}", file, ex.Message)
                        WStkTrc(ex)
                        ReportProgress(DoTranslation("Screensaver can't be loaded because of the following: "), 0, ColTypes.Error)
                        For Each LoaderException As Exception In ex.LoaderExceptions
                            Wdbg(DebugLevel.E, "Loader exception: {0}", LoaderException.Message)
                            WStkTrc(LoaderException)
                            ReportProgress(LoaderException.Message, 0, ColTypes.Error)
                        Next
                    Catch ex As Exception
                        Wdbg(DebugLevel.E, "Error trying to load dynamic screensaver {0}: {1}", file, ex.Message)
                        WStkTrc(ex)
                    End Try

                    'Now, initialize the screensaver
                    If DoneFlag Then
                        'This screensaver uses the legacy ICustomSaver, which will be removed in 0.0.24.0
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
                                    SaverInstance = New CustomSaverInfo(SaverName, file, NeutralizePath(file, modPath), CustomSaver, Nothing)
                                    CustomSavers.Add(SaverName, SaverInstance)
                                Else
                                    ReportProgress(DoTranslation("{0} has been initialized properly."), 0, ColTypes.Neutral, file)
                                    Wdbg(DebugLevel.I, "{0} compiled correctly. Starting...", file)
                                    SaverInstance = New CustomSaverInfo(SaverName, file, NeutralizePath(file, modPath), CustomSaver, Nothing)
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

    End Module
End Namespace