
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

Imports KS.Misc.Splash.Splashes
Imports System.IO
Imports System.Reflection

Namespace Misc.Splash
    Public Module SplashManager

        Public SplashName As String = "Simple"
        Friend SplashThread As New KernelThread("Kernel Splash Thread", False, Sub() CurrentSplash.Display())
        Private InstalledSplashes As New Dictionary(Of String, SplashInfo) From {{"Simple", New SplashInfo("Simple", True, 3, 1, 9, 1, New SplashSimple)},
                                                                                 {"Progress", New SplashInfo("Progress", True, 3, 1, 9, 1, New SplashProgress)},
                                                                                 {"Blank", New SplashInfo("Blank", False, 0, 0, 0, 0, New SplashBlank)}}

        ''' <summary>
        ''' Current splash screen
        ''' </summary>
        Public ReadOnly Property CurrentSplash As ISplash
            Get
                If Splashes.ContainsKey(SplashName) Then
                    Return Splashes(SplashName).EntryPoint
                Else
                    Return Splashes("Simple").EntryPoint
                End If
            End Get
        End Property

        ''' <summary>
        ''' Current splash screen info instance
        ''' </summary>
        Public ReadOnly Property CurrentSplashInfo As SplashInfo
            Get
                If Splashes.ContainsKey(SplashName) Then
                    Return Splashes(SplashName)
                Else
                    Return Splashes("Simple")
                End If
            End Get
        End Property

        ''' <summary>
        ''' All the installed splashes either normal or custom
        ''' </summary>
        Public ReadOnly Property Splashes As Dictionary(Of String, SplashInfo)
            Get
                Return InstalledSplashes
            End Get
        End Property

        ''' <summary>
        ''' Loads all the splashes from the KSSplashes folder
        ''' </summary>
        Public Sub LoadSplashes()
            If Not FolderExists(GetKernelPath(KernelPathType.CustomSplashes)) Then MakeDirectory(GetKernelPath(KernelPathType.CustomSplashes))
            Dim SplashFiles As List(Of FileSystemInfo) = CreateList(GetKernelPath(KernelPathType.CustomSplashes))
            For Each SplashFileInfo As FileSystemInfo In SplashFiles
                Dim FilePath As String = SplashFileInfo.FullName

                'Try to parse the splash file
                Try
                    Wdbg(DebugLevel.I, "Parsing splash file {0}...", FilePath)
                    Dim SplashAssembly As Assembly = Assembly.LoadFrom(FilePath)
                    Dim SplashInstance As ISplash = GetSplashInstance(SplashAssembly)
                    If SplashInstance IsNot Nothing Then
                        Wdbg(DebugLevel.I, "Found valid splash! Getting information...")
                        Dim Name As String = SplashInstance.SplashName
                        Dim DisplaysProgress As Boolean = SplashInstance.SplashDisplaysProgress
                        Dim ProgressWritePositionX As Integer = SplashInstance.ProgressWritePositionX
                        Dim ProgressWritePositionY As Integer = SplashInstance.ProgressWritePositionY
                        Dim ProgressReportWritePositionX As Integer = SplashInstance.ProgressReportWritePositionX
                        Dim ProgressReportWritePositionY As Integer = SplashInstance.ProgressReportWritePositionY

                        'Install the values to the new instance
                        Wdbg(DebugLevel.I, "- Name: {0}", Name)
                        Wdbg(DebugLevel.I, "- Displays Progress: {0}", DisplaysProgress)
                        Wdbg(DebugLevel.I, "- Progress Write Position X: {0}", ProgressWritePositionX)
                        Wdbg(DebugLevel.I, "- Progress Write Position Y: {0}", ProgressWritePositionY)
                        Wdbg(DebugLevel.I, "- Progress Report Write Position X: {0}", ProgressReportWritePositionX)
                        Wdbg(DebugLevel.I, "- Progress Report Write Position Y: {0}", ProgressReportWritePositionY)
                        Wdbg(DebugLevel.I, "Installing splash...")
                        Dim InstalledSplash As New SplashInfo(Name, DisplaysProgress, ProgressWritePositionX, ProgressWritePositionY, ProgressReportWritePositionX, ProgressReportWritePositionY, SplashInstance)
                        InstalledSplashes.AddOrModify(Name, InstalledSplash)
                    Else
                        Wdbg(DebugLevel.W, "Skipping incompatible splash file {0}...", FilePath)
                    End If
                Catch ex As ReflectionTypeLoadException
                    Wdbg(DebugLevel.W, "Could not handle splash file {0}! {1}", FilePath, ex.Message)
                    WStkTrc(ex)
                End Try
            Next
        End Sub

        ''' <summary>
        ''' Unloads all the splashes from the KSSplashes folder
        ''' </summary>
        Public Sub UnloadSplashes()
            Dim SplashFiles As List(Of FileSystemInfo) = CreateList(GetKernelPath(KernelPathType.CustomSplashes))
            For Each SplashFileInfo As FileSystemInfo In SplashFiles
                Dim FilePath As String = SplashFileInfo.FullName

                'Try to parse the splash file
                Try
                    Wdbg(DebugLevel.I, "Parsing splash file {0}...", FilePath)
                    Dim SplashAssembly As Assembly = Assembly.LoadFrom(FilePath)
                    Dim SplashInstance As ISplash = GetSplashInstance(SplashAssembly)
                    If SplashInstance IsNot Nothing Then
                        Wdbg(DebugLevel.I, "Found valid splash! Getting information...")
                        Dim Name As String = SplashInstance.SplashName

                        'Uninstall the splash
                        Wdbg(DebugLevel.I, "- Name: {0}", Name)
                        Wdbg(DebugLevel.I, "Uninstalling splash...")
                        InstalledSplashes.Remove(Name)
                    Else
                        Wdbg(DebugLevel.W, "Skipping incompatible splash file {0}...", FilePath)
                    End If
                Catch ex As ReflectionTypeLoadException
                    Wdbg(DebugLevel.W, "Could not handle splash file {0}! {1}", FilePath, ex.Message)
                    WStkTrc(ex)
                End Try
            Next
        End Sub

        ''' <summary>
        ''' Gets the splash instance from compiled assembly
        ''' </summary>
        ''' <param name="Assembly">An assembly</param>
        Public Function GetSplashInstance(Assembly As Assembly) As ISplash
            For Each t As Type In Assembly.GetTypes()
                If t.GetInterface(GetType(ISplash).Name) IsNot Nothing Then Return CType(Assembly.CreateInstance(t.FullName), ISplash)
            Next
        End Function

        ''' <summary>
        ''' Opens the splash screen
        ''' </summary>
        Sub OpenSplash()
            If EnableSplash Then
                Console.CursorVisible = False
                CurrentSplash.Opening()
                SplashThread.Stop()
                SplashThread.Start()
            End If
        End Sub

        ''' <summary>
        ''' Closes the splash screen
        ''' </summary>
        Sub CloseSplash()
            If EnableSplash Then
                CurrentSplash.Closing()
                SplashThread.Stop()
                Console.CursorVisible = True
            End If
            _KernelBooted = True
        End Sub

    End Module
End Namespace
