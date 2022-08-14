
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Imports KS.Files.Folders
Imports KS.Files.Operations
Imports KS.Files.Querying
Imports KS.Misc.Splash.Splashes
Imports KS.Misc.Reflection
Imports System.IO
Imports System.Reflection

Namespace Misc.Splash
    Public Module SplashManager

        Public SplashName As String = "Simple"
        Friend SplashThread As New KernelThread("Kernel Splash Thread", False, Sub() CurrentSplash.Display())
        Private ReadOnly InstalledSplashes As New Dictionary(Of String, SplashInfo) From {
            {"Simple", New SplashInfo("Simple", New SplashSimple)},
            {"Progress", New SplashInfo("Progress", New SplashProgress)},
            {"Blank", New SplashInfo("Blank", New SplashBlank, False)},
            {"Fader", New SplashInfo("Fader", New SplashFader)},
            {"FaderBack", New SplashInfo("FaderBack", New SplashFaderBack)},
            {"BeatFader", New SplashInfo("BeatFader", New SplashBeatFader)},
            {"systemd", New SplashInfo("systemd", New SplashSystemd)},
            {"sysvinit", New SplashInfo("sysvinit", New SplashSysvinit)},
            {"openrc", New SplashInfo("openrc", New SplashOpenRC)},
            {"Pulse", New SplashInfo("Pulse", New SplashPulse)},
            {"BeatPulse", New SplashInfo("BeatPulse", New SplashBeatPulse)},
            {"EdgePulse", New SplashInfo("EdgePulse", New SplashEdgePulse)},
            {"BeatEdgePulse", New SplashInfo("BeatEdgePulse", New SplashBeatEdgePulse)},
            {"PowerLine", New SplashInfo("PowerLine", New SplashPowerLine)}
        }

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
            Dim SplashPath As String = GetKernelPath(KernelPathType.CustomSplashes)
            If Not FolderExists(SplashPath) Then MakeDirectory(SplashPath)
            Dim SplashFiles As List(Of FileSystemInfo) = CreateList(SplashPath)
            For Each SplashFileInfo As FileSystemInfo In SplashFiles
                Dim FilePath As String = SplashFileInfo.FullName
                Dim FileName As String = SplashFileInfo.Name

                'Try to parse the splash file
                If SplashFileInfo.Extension = ".dll" Then
                    'We got a .dll file that may or may not contain splash file. Parse that to verify.
                    Try
                        'Add splash dependencies folder (if any) to the private appdomain lookup folder
                        Dim SplashDepPath As String = SplashPath + "Deps/" + Path.GetFileNameWithoutExtension(FileName) + "-" + FileVersionInfo.GetVersionInfo(FilePath).FileVersion + "/"
                        AddPathToAssemblySearchPath(SplashDepPath)

                        'Now, actually parse that.
                        Wdbg(DebugLevel.I, "Parsing splash file {0}...", FilePath)
                        Dim SplashAssembly As Assembly = Assembly.LoadFrom(FilePath)
                        Dim SplashInstance As ISplash = GetSplashInstance(SplashAssembly)
                        If SplashInstance IsNot Nothing Then
                            Wdbg(DebugLevel.I, "Found valid splash! Getting information...")
                            Dim Name As String = SplashInstance.SplashName
                            Dim DisplaysProgress As Boolean = SplashInstance.SplashDisplaysProgress

                            'Install the values to the new instance
                            Wdbg(DebugLevel.I, "- Name: {0}", Name)
                            Wdbg(DebugLevel.I, "- Displays Progress: {0}", DisplaysProgress)
                            Wdbg(DebugLevel.I, "Installing splash...")
                            Dim InstalledSplash As New SplashInfo(Name, SplashInstance, DisplaysProgress)
                            InstalledSplashes.AddOrModify(Name, InstalledSplash)
                        Else
                            Wdbg(DebugLevel.W, "Skipping incompatible splash file {0}...", FilePath)
                        End If
                    Catch ex As ReflectionTypeLoadException
                        Wdbg(DebugLevel.W, "Could not handle splash file {0}! {1}", FilePath, ex.Message)
                        WStkTrc(ex)
                    End Try
                Else
                    Wdbg(DebugLevel.W, "Skipping incompatible splash file {0} because file extension is not .dll ({1})...", FilePath, SplashFileInfo.Extension)
                End If
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
