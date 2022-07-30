
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

Imports System.Reflection
Imports System.Threading
Imports Newtonsoft.Json.Linq
Imports KS.Misc.Configuration
Imports KS.Misc.Screensaver.Customized
Imports KS.Misc.Screensaver.Displays

Namespace Misc.Screensaver
    Public Module Screensaver

        'Public Variables
        Public LockMode As Boolean
        Public InSaver As Boolean
        Public ScreensaverDebug As Boolean
        Public DefSaverName As String = "matrix"
        Public ScrnTimeout As Integer = 300000
        Public PasswordLock As Boolean = True
        Public ReadOnly colors() As ConsoleColor = CType([Enum].GetValues(GetType(ConsoleColor)), ConsoleColor())        '15 Console Colors
        Public ReadOnly colors255() As ConsoleColors = CType([Enum].GetValues(GetType(ConsoleColors)), ConsoleColors())  '255 Console Colors

        'Private variables
        Friend Screensavers As New Dictionary(Of String, BaseScreensaver) From {
            {"barrot", New BarRotDisplay()},
            {"beatfader", New BeatFaderDisplay()},
            {"beatpulse", New BeatPulseDisplay()},
            {"beatedgepulse", New BeatEdgePulseDisplay()},
            {"bouncingblock", New BouncingBlockDisplay()},
            {"bouncingtext", New BouncingTextDisplay()},
            {"colormix", New ColorMixDisplay()},
            {"dateandtime", New DateAndTimeDisplay()},
            {"disco", New DiscoDisplay()},
            {"dissolve", New DissolveDisplay()},
            {"edgepulse", New EdgePulseDisplay()},
            {"fader", New FaderDisplay()},
            {"faderback", New FaderBackDisplay()},
            {"fallingline", New FallingLineDisplay()},
            {"figlet", New FigletDisplay()},
            {"fireworks", New FireworksDisplay()},
            {"flashcolor", New FlashColorDisplay()},
            {"flashtext", New FlashTextDisplay()},
            {"glitch", New GlitchDisplay()},
            {"glittercolor", New GlitterColorDisplay()},
            {"glittermatrix", New GlitterMatrixDisplay()},
            {"indeterminate", New IndeterminateDisplay()},
            {"lighter", New LighterDisplay()},
            {"lines", New LinesDisplay()},
            {"linotypo", New LinotypoDisplay()},
            {"marquee", New MarqueeDisplay()},
            {"matrix", New MatrixDisplay()},
            {"noise", New NoiseDisplay()},
            {"personlookup", New PersonLookupDisplay()},
            {"plain", New PlainDisplay()},
            {"progressclock", New ProgressClockDisplay()},
            {"pulse", New PulseDisplay()},
            {"ramp", New RampDisplay()},
            {"random", New RandomSaverDisplay()},
            {"snaker", New SnakerDisplay()},
            {"spotwrite", New SpotWriteDisplay()},
            {"stackbox", New StackBoxDisplay()},
            {"typewriter", New TypewriterDisplay()},
            {"typo", New TypoDisplay()},
            {"windowslogo", New WindowsLogoDisplay()},
            {"wipe", New WipeDisplay()}
        }
        Friend SaverAutoReset As New AutoResetEvent(False)
        Friend Timeout As New KernelThread("Screensaver timeout thread", False, AddressOf HandleTimeout)

        ''' <summary>
        ''' Handles the screensaver time so that when it reaches the time threshold, the screensaver launches
        ''' </summary>
        Sub HandleTimeout()
            Dim CountedTime As Integer
            Dim OldCursorLeft As Integer = Console.CursorLeft
            While Not KernelShutdown
                If Not ScrnTimeReached Then
                    For CountedTime = 0 To ScrnTimeout
                        Thread.Sleep(1)
                        If Console.KeyAvailable Or OldCursorLeft <> Console.CursorLeft Then
                            CountedTime = 0
                        End If
                        OldCursorLeft = Console.CursorLeft
                        If CountedTime > ScrnTimeout Then
                            'This shouldn't happen, but the counted time is bigger than the screen timeout. Just bail.
                            Exit For
                        End If

                        'Poll to see if there is a kernel shutdown signal
                        If KernelShutdown Then Exit While
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
                KernelEventManager.RaisePreShowScreensaver(saver)
                Wdbg(DebugLevel.I, "Requested screensaver: {0}", saver)
                If Screensavers.ContainsKey(saver.ToLower()) Then
                    saver = saver.ToLower()
                    Dim BaseSaver As BaseScreensaver = Screensavers(saver)
                    ScreensaverDisplayerThread.Start(BaseSaver)
                    Wdbg(DebugLevel.I, "{0} started", saver)
                    DetectKeypress()
                    ScreensaverDisplayerThread.Stop()
                    SaverAutoReset.WaitOne()
                ElseIf CustomSavers.ContainsKey(saver) Then
                    'Only one custom screensaver can be used.
                    ScreensaverDisplayerThread.Start(New CustomDisplay(CustomSavers(saver).ScreensaverBase))
                    Wdbg(DebugLevel.I, "Custom screensaver {0} started", saver)
                    DetectKeypress()
                    ScreensaverDisplayerThread.Stop()
                    SaverAutoReset.WaitOne()
                Else
                    Write(DoTranslation("The requested screensaver {0} is not found."), True, ColTypes.Error, saver)
                    Wdbg(DebugLevel.I, "Screensaver {0} not found in the dictionary.", saver)
                End If

                'Raise event
                Wdbg(DebugLevel.I, "Screensaver really stopped.")
                KernelEventManager.RaisePostShowScreensaver(saver)
            Catch ex As InvalidOperationException
                Write(DoTranslation("Error when trying to start screensaver, because of an invalid operation."), True, ColTypes.Error)
                WStkTrc(ex)
            Catch ex As Exception
                Write(DoTranslation("Error when trying to start screensaver:") + " {0}", True, ColTypes.Error, ex.Message)
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
            KernelEventManager.RaisePreUnlock(DefSaverName)
            If PasswordLock Then
                ShowPasswordPrompt(CurrentUser.Username)
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
        Public Function GetScreensaverInstance(Assembly As Assembly) As BaseScreensaver
            For Each t As Type In Assembly.GetTypes()
                If t.GetInterface(GetType(IScreensaver).Name) IsNot Nothing Then Return CType(Assembly.CreateInstance(t.FullName), BaseScreensaver)
            Next
        End Function

        ''' <summary>
        ''' Screensaver error handler
        ''' </summary>
        Friend Sub HandleSaverError(Exception As Exception)
            If Exception IsNot Nothing Then
                Wdbg(DebugLevel.W, "Screensaver experienced an error: {0}.", Exception.Message)
                WStkTrc(Exception)
                HandleSaverCancel()
                Write(DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), True, ColTypes.Error, Exception.Message)
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
End Namespace
