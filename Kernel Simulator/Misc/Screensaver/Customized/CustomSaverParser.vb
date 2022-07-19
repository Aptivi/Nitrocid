
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
Imports KS.Files.Querying
Imports KS.Misc.Splash
Imports System.IO

Namespace Misc.Screensaver.Customized
    Public Module CustomSaverParser

        ''' <summary>
        ''' Compiles the custom screensaver file and configures it so it can be viewed
        ''' </summary>
        ''' <param name="file">File name with .ss.vb</param>
        Public Sub ParseCustomSaver(file As String)
            'Initialize path
            Dim ModPath As String = GetKernelPath(KernelPathType.Mods)
            Dim FinalScreensaverPath As String = NeutralizePath(file, ModPath)
            Dim SaverFileName As String = Path.GetFileName(FinalScreensaverPath)

            'Start parsing screensaver
            If FileExists(FinalScreensaverPath) Then
                Wdbg(DebugLevel.I, "Parsing {0}...", SaverFileName)
                Dim ScreensaverBase As BaseScreensaver
                If Path.GetExtension(FinalScreensaverPath) = ".dll" Then
                    'Try loading the screensaver
                    Try
                        Wdbg(DebugLevel.I, "{0} is probably a valid screensaver. Generating...", SaverFileName)
                        ScreensaverBase = GetScreensaverInstance(Assembly.LoadFrom(FinalScreensaverPath))
                        If ScreensaverBase IsNot Nothing Then
                            'This screensaver uses the modern BaseScreensaver and IScreensaver interfaces
                            Wdbg(DebugLevel.I, "{0} is a valid screensaver!", SaverFileName)
                            ReportProgress(DoTranslation("{0} has been initialized properly."), 0, ColTypes.Neutral, SaverFileName)
                            Dim SaverName As String = ScreensaverBase.ScreensaverName
                            Dim SaverInstance As CustomSaverInfo
                            SaverInstance = New CustomSaverInfo(SaverName, SaverFileName, FinalScreensaverPath, ScreensaverBase)
                            CustomSavers.Add(SaverName, SaverInstance)
                        Else
                            Wdbg(DebugLevel.E, "{0} is not a valid screensaver.", file)
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