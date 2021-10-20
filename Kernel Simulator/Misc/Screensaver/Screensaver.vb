
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

Imports System.ComponentModel
Imports System.Reflection
Imports System.Threading
Imports Newtonsoft.Json.Linq

Public Module Screensaver

    'Public Variables
    Public LockMode As Boolean
    Public InSaver As Boolean
    Public ScreensaverDebug As Boolean
    Public DefSaverName As String = "matrix"
    Public WithEvents Timeout As New BackgroundWorker
    Public ScrnTimeout As Integer = 300000
    Public PasswordLock As Boolean = True
    Public ReadOnly colors() As ConsoleColor = CType([Enum].GetValues(GetType(ConsoleColor)), ConsoleColor())        '15 Console Colors
    Public ReadOnly colors255() As ConsoleColors = CType([Enum].GetValues(GetType(ConsoleColors)), ConsoleColors())  '255 Console Colors
    Public ReadOnly Screensavers As New Dictionary(Of String, BackgroundWorker) From {{"beatfader", BeatFader},
                                                                                      {"bouncingblock", BouncingBlock},
                                                                                      {"bouncingtext", BouncingText},
                                                                                      {"colormix", ColorMix},
                                                                                      {"disco", Disco},
                                                                                      {"dissolve", Dissolve},
                                                                                      {"fader", Fader},
                                                                                      {"faderback", FaderBack},
                                                                                      {"flashcolor", FlashColor},
                                                                                      {"glittercolor", GlitterColor},
                                                                                      {"glittermatrix", GlitterMatrix},
                                                                                      {"lighter", Lighter},
                                                                                      {"lines", Lines},
                                                                                      {"linotypo", Linotypo},
                                                                                      {"marquee", Marquee},
                                                                                      {"matrix", Matrix},
                                                                                      {"plain", Plain},
                                                                                      {"progressclock", ProgressClock},
                                                                                      {"ramp", Ramp},
                                                                                      {"spotwrite", SpotWrite},
                                                                                      {"typewriter", Typewriter},
                                                                                      {"typo", Typo},
                                                                                      {"windowslogo", WindowsLogo},
                                                                                      {"wipe", Wipe}}

    'Private variables
    Friend SaverAutoReset As New AutoResetEvent(False)

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
                    LockScreen()
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
            saver = saver.ToLower()
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
                CustomSaver = CustomSavers(saver).Screensaver
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
    ''' Locks the screen. The password will be required when unlocking, depending on the kernel settings.
    ''' </summary>
    Public Sub LockScreen()
        LockMode = True
        ShowSavers(DefSaverName)
        EventManager.RaisePreUnlock(DefSaverName)
        If PasswordLock Then
            ShowPasswordPrompt(CurrentUser)
        Else
            LockMode = False
        End If
    End Sub

    ''' <summary>
    ''' Sets the default screensaver
    ''' </summary>
    ''' <param name="saver">Specified screensaver</param>
    Public Sub SetDefaultScreensaver(saver As String)
        saver = saver.ToLower
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
    ''' Gets a screensaver instance from loaded assembly
    ''' </summary>
    ''' <param name="Assembly">An assembly</param>
    Public Function GetScreensaverInstance(Assembly As Assembly) As ICustomSaver
        For Each t As Type In Assembly.GetTypes()
            If t.GetInterface(GetType(ICustomSaver).Name) IsNot Nothing Then Return CType(Assembly.CreateInstance(t.FullName), ICustomSaver)
        Next
    End Function

    ''' <summary>
    ''' Screensaver error handler
    ''' </summary>
    Friend Sub HandleSaverError(Exception As Exception)
        If Exception IsNot Nothing Then
            Wdbg(DebugLevel.W, "Screensaver experienced an error: {0}.", Exception.Message)
            HandleSaverCancel()
            W(DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), True, ColTypes.Error, Exception.Message)
        End If
    End Sub

    ''' <summary>
    ''' Screensaver cancellation handler
    ''' </summary>
    Friend Sub HandleSaverCancel()
        Wdbg(DebugLevel.W, "Cancellation is pending. Cleaning everything up...")
        SetInputColor()
        LoadBack()
        Console.CursorVisible = True
        Wdbg(DebugLevel.I, "All clean. Screensaver stopped.")
        SaverAutoReset.Set()
    End Sub

End Module
