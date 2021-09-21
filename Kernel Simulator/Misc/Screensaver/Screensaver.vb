
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
Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Threading
Imports Microsoft.CSharp
Imports Newtonsoft.Json.Linq

Public Module Screensaver

    'Public Variables
    Public LockMode As Boolean
    Public InSaver As Boolean
    Public ScreensaverDebug As Boolean
    Public DefSaverName As String = "matrix"
    Public CustomSavers As New Dictionary(Of String, ScreensaverInfo)
    Public WithEvents Timeout As New BackgroundWorker
    Public FinalSaver As ICustomSaver
    Public CustomSaverSettingsToken As JObject
    Public ScrnTimeout As Integer = 300000
    Public ReadOnly colors() As ConsoleColor = CType([Enum].GetValues(GetType(ConsoleColor)), ConsoleColor())        '15 Console Colors
    Public ReadOnly colors255() As ConsoleColors = CType([Enum].GetValues(GetType(ConsoleColors)), ConsoleColors())  '255 Console Colors
    Public ReadOnly Screensavers As New Dictionary(Of String, BackgroundWorker) From {{"aptErrorSim", AptErrorSim},
                                                                                      {"beatFader", BeatFader},
                                                                                      {"bouncingBlock", BouncingBlock},
                                                                                      {"bouncingText", BouncingText},
                                                                                      {"colorMix", ColorMix},
                                                                                      {"disco", Disco},
                                                                                      {"dissolve", Dissolve},
                                                                                      {"fader", Fader},
                                                                                      {"faderBack", FaderBack},
                                                                                      {"flashColor", FlashColor},
                                                                                      {"glitterColor", GlitterColor},
                                                                                      {"glitterMatrix", GlitterMatrix},
                                                                                      {"hackUserFromAD", HackUserFromAD},
                                                                                      {"lighter", Lighter},
                                                                                      {"lines", Lines},
                                                                                      {"linotypo", Linotypo},
                                                                                      {"marquee", Marquee},
                                                                                      {"matrix", Matrix},
                                                                                      {"plain", Plain},
                                                                                      {"progressClock", ProgressClock},
                                                                                      {"ramp", Ramp},
                                                                                      {"spotWrite", SpotWrite},
                                                                                      {"typewriter", Typewriter},
                                                                                      {"typo", Typo},
                                                                                      {"wipe", Wipe}}

    'Private variables
    Friend SaverAutoReset As New AutoResetEvent(False)
    Private execCustomSaver As CompilerResults
    Private DoneFlag As Boolean = False

    ''' <summary>
    ''' Handles the screensaver time so that when it reaches the time threshold, the screensaver launches
    ''' </summary>
    Sub HandleTimeout(sender As Object, e As DoWorkEventArgs) Handles Timeout.DoWork
        Dim count As Integer
        Dim oldcursor As Integer = Console.CursorLeft
        While True
            If Not ScrnTimeReached Then
                For count = 0 To ScrnTimeout
                    Thread.Sleep(1)
                    If oldcursor <> Console.CursorLeft Then
                        count = 0
                    End If
                    oldcursor = Console.CursorLeft
                Next
                If Not RebootRequested Then
                    Wdbg(DebugLevel.W, "Screen time has reached.")
                    ShowSavers(DefSaverName)
                End If
            End If
        End While
    End Sub

    ''' <summary>
    ''' Shows the screensaver
    ''' </summary>
    ''' <param name="saver">A specified screensaver</param>
    Sub ShowSavers(saver As String)
        Try
            InSaver = True
            ScrnTimeReached = True
            EventManager.RaisePreShowScreensaver(saver)
            Wdbg(DebugLevel.I, "Requested screensaver: {0}", saver)
            If Screensavers.ContainsKey(saver) Then
                Screensavers(saver).RunWorkerAsync()
                Wdbg(DebugLevel.I, "{0} started", saver)
                Console.ReadKey()
                Screensavers(saver).CancelAsync()
                SaverAutoReset.WaitOne()
            ElseIf CustomSavers.ContainsKey(saver) Then
                'Only one custom screensaver can be used.
                FinalSaver = CustomSavers(saver).Screensaver
                Custom.RunWorkerAsync()
                Wdbg(DebugLevel.I, "Custom screensaver {0} started", saver)
                Console.ReadKey()
                Custom.CancelAsync()
                SaverAutoReset.WaitOne()
            Else
                W(DoTranslation("The requested screensaver {0} is not found."), True, ColTypes.Error, saver)
                Wdbg(DebugLevel.I, "Screensaver {0} not found in the dictionary.", saver)
            End If

            'Raise event
            Wdbg(DebugLevel.I, "Screensaver really stopped.")
            EventManager.RaisePostShowScreensaver(saver)
        Catch ex As InvalidOperationException
            W(DoTranslation("Error when trying to start screensaver, because of an invalid operation."), True, ColTypes.Error)
            WStkTrc(ex)
        Catch ex As Exception
            W(DoTranslation("Error when trying to start screensaver:") + " {0}", True, ColTypes.Error, ex.Message)
            WStkTrc(ex)
        Finally
            InSaver = False
            ScrnTimeReached = False
        End Try
    End Sub

    ''' <summary>
    ''' Locks the screen. The password will be required when unlocking.
    ''' </summary>
    Public Sub LockScreen()
        LockMode = True
        ShowSavers(DefSaverName)
        EventManager.RaisePreUnlock(DefSaverName)
        ShowPasswordPrompt(CurrentUser)
    End Sub

    ''' <summary>
    ''' Sets the default screensaver
    ''' </summary>
    ''' <param name="saver">Specified screensaver</param>
    Public Sub SetDefaultScreensaver(saver As String)
        If Screensavers.ContainsKey(saver) Or CustomSavers.ContainsKey(saver) Then
            Wdbg(DebugLevel.I, "{0} is found. Setting it to default...", saver)
            DefSaverName = saver
            Dim Token As JToken = GetConfigCategory(ConfigCategory.Screensaver)
            SetConfigValue(ConfigCategory.Screensaver, Token, "Screensaver", saver)
        Else
            Wdbg(DebugLevel.W, "{0} is not found.", saver)
            Throw New Exceptions.NoSuchScreensaverException(DoTranslation("Screensaver {0} not found in database. Check the name and try again."), saver)
        End If
    End Sub

    ''' <summary>
    ''' Compiles the custom screensaver file and configures it so it can be viewed
    ''' </summary>
    ''' <param name="file">File name with .ss.vb</param>
    Public Sub CompileCustom(file As String)
        'Initialize path
        Dim modPath As String = GetKernelPath(KernelPathType.Mods)
        file = file.Replace("\", "/").Replace(modPath, "")

        'Start parsing screensaver
        If IO.File.Exists(modPath + file) Then
            Wdbg(DebugLevel.I, "Parsing {0}...", file)
            If file.EndsWith(".ss.vb") Or file.EndsWith(".ss.cs") Or file.EndsWith(".dll") Then
                Wdbg(DebugLevel.W, "{0} is a valid screensaver. Generating...", file)
                If file.EndsWith(".ss.vb") Then
                    FinalSaver = GenSaver("VB.NET", IO.File.ReadAllText(modPath + file))
                ElseIf file.EndsWith(".ss.cs") Then
                    FinalSaver = GenSaver("C#", IO.File.ReadAllText(modPath + file))
                ElseIf file.EndsWith(".dll") Then
                    Try
                        FinalSaver = GetScreensaverInstance(Assembly.LoadFrom(modPath + file))
                        DoneFlag = True
                    Catch ex As ReflectionTypeLoadException
                        Wdbg(DebugLevel.E, "Error trying to load dynamic mod {0}: {1}", file, ex.Message)
                        WStkTrc(ex)
                        W(DoTranslation("Screensaver can't be loaded because of the following: "), True, ColTypes.Error)
                        For Each LoaderException As Exception In ex.LoaderExceptions
                            Wdbg(DebugLevel.E, "Loader exception: {0}", LoaderException.Message)
                            WStkTrc(LoaderException)
                            W(LoaderException.Message, True, ColTypes.Error)
                        Next
                    End Try
                End If
                If DoneFlag = True Then
                    Wdbg(DebugLevel.I, "{0} compiled correctly. Starting...", file)
                    FinalSaver.InitSaver()
                    Dim SaverName As String = FinalSaver.SaverName
                    Dim SaverInstance As ScreensaverInfo
                    If FinalSaver.Initialized = True Then
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
                                W(DoTranslation("{0} has been initialized properly."), True, ColTypes.Neutral, SaverName)
                                Wdbg(DebugLevel.I, "{0} ({1}) compiled correctly. Starting...", SaverName, file)
                                SaverInstance = New ScreensaverInfo(SaverName, file, NeutralizePath(file, modPath), FinalSaver)
                                CustomSavers.Add(SaverName, SaverInstance)
                            Else
                                W(DoTranslation("{0} has been initialized properly."), True, ColTypes.Neutral, file)
                                Wdbg(DebugLevel.I, "{0} compiled correctly. Starting...", file)
                                SaverInstance = New ScreensaverInfo(SaverName, file, NeutralizePath(file, modPath), FinalSaver)
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
                            W(DoTranslation("{0} did not initialize. The screensaver code might have experienced an error while initializing."), True, ColTypes.Error, SaverName)
                            Wdbg(DebugLevel.W, "{0} ({1}) is compiled, but not initialized.", SaverName, file)
                        Else
                            W(DoTranslation("{0} did not initialize. The screensaver code might have experienced an error while initializing."), True, ColTypes.Error, file)
                            Wdbg(DebugLevel.W, "{0} is compiled, but not initialized.", file)
                        End If
                    End If
                End If
            Else
                Wdbg(DebugLevel.W, "{0} is not a screensaver. A screensaver code should have "".ss.vb"" or "".dll"" at the end.", file)
            End If
        Else
            W(DoTranslation("Screensaver {0} does not exist."), True, ColTypes.Error, file)
            Wdbg(DebugLevel.E, "The file {0} does not exist for compilation.", file)
        End If
    End Sub

    ''' <summary>
    ''' Compiles the screensaver and returns the instance of custom saver interface
    ''' </summary>
    ''' <param name="PLang">Specified programming language for scripts (C# or VB.NET)</param>
    ''' <param name="code">Screensaver code</param>
    ''' <returns>Interface of the compiled custom saver</returns>
    Function GenSaver(PLang As String, code As String) As ICustomSaver
        DoneFlag = False

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

            'Try to compile
            Dim namespc As String = GetType(ICustomSaver).Namespace
            Dim modCode() As String = {}
            If PLang = "VB.NET" Then
                modCode = {$"Imports {namespc}{vbNewLine}{code}"}
            ElseIf PLang = "C#" Then
                modCode = {$"using {namespc};{vbNewLine}{code}"}
            End If
            Wdbg(DebugLevel.I, "Compiling...")
            execCustomSaver = provider.CompileAssemblyFromSource(prm, modCode)

            'Check to see if there are compilation errors
            Wdbg(DebugLevel.I, "Compilation results: Errors? {0}, Warnings? {1} | Total: {2}", execCustomSaver.Errors.HasErrors, execCustomSaver.Errors.HasWarnings, execCustomSaver.Errors.Count)
            If execCustomSaver.Errors.HasErrors Then
                W(DoTranslation("Screensaver can't be loaded because of the following: "), True, ColTypes.Error)
                Wdbg(DebugLevel.E, "Errors when compiling:")
                For Each errorName In execCustomSaver.Errors
                    W(errorName.ToString, True, ColTypes.Error) : Wdbg(DebugLevel.E, errorName.ToString, True)
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

    ''' <summary>
    ''' Gets a screensaver instance from loaded assembly
    ''' </summary>
    ''' <param name="Assembly">An assembly</param>
    Public Function GetScreensaverInstance(Assembly As Assembly) As ICustomSaver
        For Each t As Type In Assembly.GetTypes()
            If t.GetInterface(GetType(ICustomSaver).Name) IsNot Nothing Then Return CType(Assembly.CreateInstance(t.FullName), ICustomSaver)
        Next
    End Function

    ''' <summary>
    ''' Initializes and reads the custom saver settings
    ''' </summary>
    Public Sub InitializeCustomSaverSettings()
        If Not File.Exists(GetKernelPath(KernelPathType.CustomSaverSettings)) Then MakeFile(GetKernelPath(KernelPathType.CustomSaverSettings))
        Dim CustomSaverJsonContent As String = File.ReadAllText(GetKernelPath(KernelPathType.CustomSaverSettings))
        Dim CustomSaverToken As JObject = JObject.Parse(If(Not String.IsNullOrEmpty(CustomSaverJsonContent), CustomSaverJsonContent, "{}"))
        For Each Saver As String In CustomSavers.Keys
            Dim CustomSaverSettings As JObject = TryCast(CustomSaverToken(Saver), JObject)
            If CustomSaverSettings IsNot Nothing Then
                For Each Setting In CustomSaverSettings
                    CustomSavers(Saver).Screensaver.SaverSettings(Setting.Key) = Setting.Value.ToString
                Next
            End If
        Next
        CustomSaverSettingsToken = CustomSaverToken
    End Sub

    ''' <summary>
    ''' Saves the custom saver settings
    ''' </summary>
    Public Sub SaveCustomSaverSettings()
        For Each Saver As String In CustomSavers.Keys
            If CustomSavers(Saver).Screensaver.SaverSettings IsNot Nothing Then
                For Each Setting As String In CustomSavers(Saver).Screensaver.SaverSettings.Keys
                    If Not TryCast(CustomSaverSettingsToken(Saver), JObject).ContainsKey(Setting) Then
                        TryCast(CustomSaverSettingsToken(Saver), JObject).Add(Setting, CustomSavers(Saver).Screensaver.SaverSettings(Setting).ToString)
                    Else
                        CustomSaverSettingsToken(Saver)(Setting) = CustomSavers(Saver).Screensaver.SaverSettings(Setting).ToString
                    End If
                Next
            End If
        Next
        If CustomSaverSettingsToken IsNot Nothing Then File.WriteAllText(GetKernelPath(KernelPathType.CustomSaverSettings), JsonConvert.SerializeObject(CustomSaverSettingsToken, Formatting.Indented))
    End Sub

    ''' <summary>
    ''' Adds a custom screensaver to settings
    ''' </summary>
    ''' <param name="CustomSaver">A custom saver</param>
    ''' <exception cref="Exceptions.NoSuchScreensaverException"></exception>
    Public Sub AddCustomSaverToSettings(CustomSaver As String)
        If Not CustomSavers.ContainsKey(CustomSaver) Then Throw New Exceptions.NoSuchScreensaverException(DoTranslation("Screensaver {0} not found."), CustomSaver)
        If Not CustomSaverSettingsToken.ContainsKey(CustomSaver) Then
            Dim NewCustomSaver As New JObject
            If CustomSavers(CustomSaver).Screensaver.SaverSettings IsNot Nothing Then
                For Each Setting As String In CustomSavers(CustomSaver).Screensaver.SaverSettings.Keys
                    NewCustomSaver.Add(Setting, CustomSavers(CustomSaver).Screensaver.SaverSettings(Setting).ToString)
                Next
                CustomSaverSettingsToken.Add(CustomSaver, NewCustomSaver)
                If CustomSaverSettingsToken IsNot Nothing Then File.WriteAllText(GetKernelPath(KernelPathType.CustomSaverSettings), JsonConvert.SerializeObject(CustomSaverSettingsToken, Formatting.Indented))
            End If
        End If
    End Sub

    ''' <summary>
    ''' Removes a custom screensaver from settings
    ''' </summary>
    ''' <param name="CustomSaver">A custom saver</param>
    ''' <exception cref="Exceptions.NoSuchScreensaverException"></exception>
    ''' <exception cref="Exceptions.ScreensaverManagementException"></exception>
    Public Sub RemoveCustomSaverFromSettings(CustomSaver As String)
        If Not CustomSavers.ContainsKey(CustomSaver) Then Throw New Exceptions.NoSuchScreensaverException(DoTranslation("Screensaver {0} not found."), CustomSaver)
        If Not CustomSaverSettingsToken.Remove(CustomSaver) Then Throw New Exceptions.ScreensaverManagementException(DoTranslation("Failed to remove screensaver {0} from config."), CustomSaver)
        If CustomSaverSettingsToken IsNot Nothing Then File.WriteAllText(GetKernelPath(KernelPathType.CustomSaverSettings), JsonConvert.SerializeObject(CustomSaverSettingsToken, Formatting.Indented))
    End Sub

    ''' <summary>
    ''' Gets custom saver settings
    ''' </summary>
    ''' <param name="CustomSaver">A custom saver</param>
    ''' <param name="SaverSetting">A saver setting</param>
    ''' <returns>Saver setting value if successful; nothing if unsuccessful.</returns>
    ''' <exception cref="Exceptions.NoSuchScreensaverException"></exception>
    Public Function GetCustomSaverSettings(CustomSaver As String, SaverSetting As String) As Object
        If Not CustomSaverSettingsToken.ContainsKey(CustomSaver) Then Throw New Exceptions.NoSuchScreensaverException(DoTranslation("Screensaver {0} not found."), CustomSaver)
        For Each Setting As JProperty In CustomSaverSettingsToken(CustomSaver)
            If Setting.Name = SaverSetting Then
                Return Setting.Value.ToObject(GetType(Object))
            End If
        Next
    End Function

    ''' <summary>
    ''' Sets custom saver settings
    ''' </summary>
    ''' <param name="CustomSaver">A custom saver</param>
    ''' <param name="SaverSetting">A saver setting</param>
    ''' <param name="Value">Value</param>
    ''' <returns>True if successful; False if unsuccessful.</returns>
    ''' <exception cref="Exceptions.NoSuchScreensaverException"></exception>
    Public Function SetCustomSaverSettings(CustomSaver As String, SaverSetting As String, Value As Object) As Boolean
        If Not CustomSaverSettingsToken.ContainsKey(CustomSaver) Then Throw New Exceptions.NoSuchScreensaverException(DoTranslation("Screensaver {0} not found."), CustomSaver)
        Dim SettingFound As Boolean
        For Each Setting As JProperty In CustomSaverSettingsToken(CustomSaver)
            If Setting.Name = SaverSetting Then
                SettingFound = True
                CustomSaverSettingsToken(CustomSaver)(SaverSetting) = Value.ToString
            End If
        Next
        If CustomSaverSettingsToken IsNot Nothing Then File.WriteAllText(GetKernelPath(KernelPathType.CustomSaverSettings), JsonConvert.SerializeObject(CustomSaverSettingsToken, Formatting.Indented))
        Return SettingFound
    End Function

End Module
