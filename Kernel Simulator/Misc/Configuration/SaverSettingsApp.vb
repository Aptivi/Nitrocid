
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

Public Module SaverSettingsApp

    ''' <summary>
    ''' Main page
    ''' </summary>
    Sub OpenMainPageSaver()
        Dim PromptFinished As Boolean
        Dim AnswerString As String
        Dim AnswerInt As Integer
        Dim MaxSections As Integer = ConfigToken("Screensaver").Count - 4 'Screensaver - Keys
        Dim ConfigurableScreensavers As New List(Of String)

        While Not PromptFinished
            Console.Clear()
            'List sections
            WriteSeparator(DoTranslation("Welcome to Screensaver Settings!"), True)
            W(vbNewLine + DoTranslation("Select screensaver to configure:") + vbNewLine, True, ColTypes.Neutral)
            W(" 1) ColorMix...", True, ColTypes.Option)
            W(" 2) Matrix...", True, ColTypes.Option)
            W(" 3) GlitterMatrix...", True, ColTypes.Option)
            W(" 4) Disco...", True, ColTypes.Option)
            W(" 5) Lines...", True, ColTypes.Option)
            W(" 6) GlitterColor...", True, ColTypes.Option)
            W(" 7) BouncingText...", True, ColTypes.Option)
            W(" 8) Dissolve...", True, ColTypes.Option)
            W(" 9) BouncingBlock...", True, ColTypes.Option)
            W(" 10) ProgressClock...", True, ColTypes.Option)
            W(" 11) Lighter...", True, ColTypes.Option)
            W(" 12) Fader...", True, ColTypes.Option)
            W(" 13) Typo...", True, ColTypes.Option)
            W(" 14) Wipe...", True, ColTypes.Option)
            W(" 15) Marquee...", True, ColTypes.Option)
            W(" 16) FaderBack...", True, ColTypes.Option)
            W(" 17) BeatFader...", True, ColTypes.Option)
            W(" 18) Linotypo...", True, ColTypes.Option)
            W(" 19) Typewriter...", True, ColTypes.Option)
            W(" 20) FlashColor...", True, ColTypes.Option)
            W(" 21) SpotWrite...", True, ColTypes.Option)
            W(" 22) Ramp...", True, ColTypes.Option)

            'Populate custom screensavers
            For Each CustomSaver As String In CustomSavers.Keys
                If CustomSavers(CustomSaver).Screensaver.SaverSettings?.Count >= 1 Then
                    ConfigurableScreensavers.Add(CustomSaver)
                    W(" {0}) {1}...", True, ColTypes.Option, MaxSections, CustomSaver)
                    MaxSections += 1
                End If
            Next

            Console.WriteLine()
            W(" {0}) " + DoTranslation("Save Settings"), True, ColTypes.BackOption, MaxSections + 1)
            W(" {0}) " + DoTranslation("Exit"), True, ColTypes.BackOption, MaxSections + 2)

            'Prompt user and check for input
            Console.WriteLine()
            W("> ", False, ColTypes.Input)
            AnswerString = Console.ReadLine
            Wdbg(DebugLevel.I, "User answered {0}", AnswerString)
            Console.WriteLine()

            Wdbg(DebugLevel.I, "Is the answer numeric? {0}", IsNumeric(AnswerString))
            If Integer.TryParse(AnswerString, AnswerInt) Then
                Wdbg(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...")
                If AnswerInt >= 1 And AnswerInt <= MaxSections Then
                    Wdbg(DebugLevel.I, "Opening section {0}...", AnswerInt)
                    OpenSectionSaver(AnswerString)
                ElseIf AnswerInt = MaxSections + 1 Then 'Save Settings
                    Wdbg(DebugLevel.I, "Saving settings...")
                    Try
                        CreateConfig()
                        SaveCustomSaverSettings()
                    Catch ex As Exception
                        W(ex.Message, True, ColTypes.Error)
                        WStkTrc(ex)
                        Console.ReadKey()
                    End Try
                ElseIf AnswerInt = MaxSections + 2 Then 'Exit
                    Wdbg(DebugLevel.W, "Exiting...")
                    PromptFinished = True
                    Console.Clear()
                Else
                    Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            Else
                Wdbg(DebugLevel.W, "Answer is not numeric.")
                W(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                Console.ReadKey()
            End If
        End While
    End Sub

    ''' <summary>
    ''' Open section
    ''' </summary>
    ''' <param name="SectionNum">Section number</param>
    Sub OpenSectionSaver(SectionNum As String, ParamArray SectionParameters() As Object)
        'General variables
        Dim MaxOptions As Integer = 0
        Dim SectionFinished As Boolean
        Dim AnswerString As String
        Dim AnswerInt As Integer
        Dim BuiltinSavers As Integer = ConfigToken("Screensaver").Count - 4 'Screensaver - Keys

        'Section-specific variables
        Dim ConfigurableScreensavers As New List(Of String)

        While Not SectionFinished
            Console.Clear()

            'List options
            Select Case SectionNum
                Case "1" 'ColorMix
                    MaxOptions = 12
                    WriteSeparator("ColorMix", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " ColorMix." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMix255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixDelay)))
                    W(" 4) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixBackgroundColor)))
                    W(" 5) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMinimumRedColorLevel)))
                    W(" 6) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMinimumGreenColorLevel)))
                    W(" 7) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMinimumBlueColorLevel)))
                    W(" 8) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMinimumColorLevel)))
                    W(" 9) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMaximumRedColorLevel)))
                    W(" 10) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMaximumGreenColorLevel)))
                    W(" 11) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMaximumBlueColorLevel)))
                    W(" 12) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ColorMixMaximumColorLevel)))
                Case "2" 'Matrix
                    MaxOptions = 1
                    WriteSeparator("Matrix", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Matrix." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MatrixDelay)))
                Case "3" 'GlitterMatrix
                    MaxOptions = 3
                    WriteSeparator("GlitterMatrix", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " GlitterMatrix." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterMatrixDelay)))
                    W(" 2) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterMatrixBackgroundColor)))
                    W(" 3) " + DoTranslation("Foreground color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterMatrixForegroundColor)))
                Case "4" 'Disco
                    MaxOptions = 13
                    WriteSeparator("Disco", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Disco." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Disco255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoTrueColor)))
                    W(" 3) " + DoTranslation("Cycle colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoCycleColors)))
                    W(" 4) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoDelay)))
                    W(" 5) " + DoTranslation("Use Beats Per Minute") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoUseBeatsPerMinute)))
                    W(" 6) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMinimumRedColorLevel)))
                    W(" 7) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMinimumGreenColorLevel)))
                    W(" 8) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMinimumBlueColorLevel)))
                    W(" 9) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMinimumColorLevel)))
                    W(" 10) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMaximumRedColorLevel)))
                    W(" 11) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMaximumGreenColorLevel)))
                    W(" 12) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMaximumBlueColorLevel)))
                    W(" 13) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DiscoMaximumColorLevel)))
                Case "5" 'Lines
                    MaxOptions = 13
                    WriteSeparator("Lines", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Lines." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Lines255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesDelay)))
                    W(" 4) " + DoTranslation("Line character") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesLineChar)))
                    W(" 5) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesBackgroundColor)))
                    W(" 6) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMinimumRedColorLevel)))
                    W(" 7) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMinimumGreenColorLevel)))
                    W(" 8) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMinimumBlueColorLevel)))
                    W(" 9) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMinimumColorLevel)))
                    W(" 10) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMaximumRedColorLevel)))
                    W(" 11) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMaximumGreenColorLevel)))
                    W(" 12) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMaximumBlueColorLevel)))
                    W(" 13) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinesMaximumColorLevel)))
                Case "6" 'GlitterColor
                    MaxOptions = 11
                    WriteSeparator("GlitterColor", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " GlitterColor." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColor255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorDelay)))
                    W(" 4) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMinimumRedColorLevel)))
                    W(" 5) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMinimumGreenColorLevel)))
                    W(" 6) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMinimumBlueColorLevel)))
                    W(" 7) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMinimumColorLevel)))
                    W(" 8) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMaximumRedColorLevel)))
                    W(" 9) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMaximumGreenColorLevel)))
                    W(" 10) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMaximumBlueColorLevel)))
                    W(" 11) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(GlitterColorMaximumColorLevel)))
                Case "7" 'BouncingText
                    MaxOptions = 14
                    WriteSeparator("BouncingText", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " BouncingText." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingText255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextDelay)))
                    W(" 4) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextWrite)))
                    W(" 5) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextBackgroundColor)))
                    W(" 6) " + DoTranslation("Foreground color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextForegroundColor)))
                    W(" 7) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMinimumRedColorLevel)))
                    W(" 8) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMinimumGreenColorLevel)))
                    W(" 9) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMinimumBlueColorLevel)))
                    W(" 10) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMinimumColorLevel)))
                    W(" 11) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMaximumRedColorLevel)))
                    W(" 12) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMaximumGreenColorLevel)))
                    W(" 13) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMaximumBlueColorLevel)))
                    W(" 14) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingTextMaximumColorLevel)))
                Case "8" 'Dissolve
                    MaxOptions = 11
                    WriteSeparator("Dissolve", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Dissolve." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Dissolve255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveTrueColor)))
                    W(" 3) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveBackgroundColor)))
                    W(" 4) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMinimumRedColorLevel)))
                    W(" 5) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMinimumGreenColorLevel)))
                    W(" 6) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMinimumBlueColorLevel)))
                    W(" 7) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMinimumColorLevel)))
                    W(" 8) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMaximumRedColorLevel)))
                    W(" 9) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMaximumGreenColorLevel)))
                    W(" 10) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMaximumBlueColorLevel)))
                    W(" 11) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(DissolveMaximumColorLevel)))
                Case "9" 'BouncingBlock
                    MaxOptions = 13
                    WriteSeparator("BouncingBlock", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " BouncingBlock." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlock255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockDelay)))
                    W(" 4) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockBackgroundColor)))
                    W(" 5) " + DoTranslation("Foreground color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockForegroundColor)))
                    W(" 6) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMinimumRedColorLevel)))
                    W(" 7) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMinimumGreenColorLevel)))
                    W(" 8) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMinimumBlueColorLevel)))
                    W(" 9) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMinimumColorLevel)))
                    W(" 10) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMaximumRedColorLevel)))
                    W(" 11) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMaximumGreenColorLevel)))
                    W(" 12) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMaximumBlueColorLevel)))
                    W(" 13) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BouncingBlockMaximumColorLevel)))
                Case "10" 'ProgressClock
                    MaxOptions = 68
                    WriteSeparator("ProgressClock", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " ProgressClock." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClock255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockTrueColor)))
                    W(" 3) " + DoTranslation("Cycle colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockCycleColors)))
                    W(" 4) " + DoTranslation("Color of Seconds Bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockSecondsProgressColor)))
                    W(" 5) " + DoTranslation("Color of Minutes Bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinutesProgressColor)))
                    W(" 6) " + DoTranslation("Color of Hours Bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockHoursProgressColor)))
                    W(" 7) " + DoTranslation("Color of Information") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockProgressColor)))
                    W(" 8) " + DoTranslation("Ticks to change color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockCycleColorsTicks)))
                    W(" 9) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockDelay)))
                    W(" 10) " + DoTranslation("Upper left corner character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperLeftCornerCharHours)))
                    W(" 11) " + DoTranslation("Upper left corner character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperLeftCornerCharMinutes)))
                    W(" 12) " + DoTranslation("Upper left corner character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperLeftCornerCharSeconds)))
                    W(" 13) " + DoTranslation("Lower left corner character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerLeftCornerCharHours)))
                    W(" 14) " + DoTranslation("Lower left corner character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerLeftCornerCharMinutes)))
                    W(" 15) " + DoTranslation("Lower left corner character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerLeftCornerCharSeconds)))
                    W(" 16) " + DoTranslation("Upper right corner character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperRightCornerCharHours)))
                    W(" 17) " + DoTranslation("Upper right corner character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperRightCornerCharMinutes)))
                    W(" 18) " + DoTranslation("Upper right corner character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperRightCornerCharSeconds)))
                    W(" 19) " + DoTranslation("Lower right corner character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerRightCornerCharHours)))
                    W(" 20) " + DoTranslation("Lower right corner character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerRightCornerCharMinutes)))
                    W(" 21) " + DoTranslation("Lower right corner character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerRightCornerCharSeconds)))
                    W(" 22) " + DoTranslation("Upper frame character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperFrameCharHours)))
                    W(" 23) " + DoTranslation("Upper frame character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperFrameCharMinutes)))
                    W(" 24) " + DoTranslation("Upper frame character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockUpperFrameCharSeconds)))
                    W(" 25) " + DoTranslation("Lower frame character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerFrameCharHours)))
                    W(" 26) " + DoTranslation("Lower frame character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerFrameCharMinutes)))
                    W(" 27) " + DoTranslation("Lower frame character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLowerFrameCharSeconds)))
                    W(" 28) " + DoTranslation("Left frame character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLeftFrameCharHours)))
                    W(" 29) " + DoTranslation("Left frame character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLeftFrameCharMinutes)))
                    W(" 30) " + DoTranslation("Left frame character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockLeftFrameCharSeconds)))
                    W(" 31) " + DoTranslation("Right frame character for hours bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockRightFrameCharHours)))
                    W(" 32) " + DoTranslation("Right frame character for minutes bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockRightFrameCharMinutes)))
                    W(" 33) " + DoTranslation("Right frame character for seconds bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockRightFrameCharSeconds)))
                    W(" 34) " + DoTranslation("Information text for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockInfoTextHours)))
                    W(" 35) " + DoTranslation("Information text for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockInfoTextMinutes)))
                    W(" 36) " + DoTranslation("Information text for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockInfoTextSeconds)))
                    W(" 37) " + DoTranslation("Minimum red color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumRedColorLevelHours)))
                    W(" 38) " + DoTranslation("Minimum green color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumGreenColorLevelHours)))
                    W(" 39) " + DoTranslation("Minimum blue color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumBlueColorLevelHours)))
                    W(" 40) " + DoTranslation("Minimum color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumColorLevelHours)))
                    W(" 41) " + DoTranslation("Maximum red color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumRedColorLevelHours)))
                    W(" 42) " + DoTranslation("Maximum green color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumGreenColorLevelHours)))
                    W(" 43) " + DoTranslation("Maximum blue color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumBlueColorLevelHours)))
                    W(" 44) " + DoTranslation("Maximum color level for hours") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumColorLevelHours)))
                    W(" 45) " + DoTranslation("Minimum red color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumRedColorLevelMinutes)))
                    W(" 46) " + DoTranslation("Minimum green color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumGreenColorLevelMinutes)))
                    W(" 47) " + DoTranslation("Minimum blue color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumBlueColorLevelMinutes)))
                    W(" 48) " + DoTranslation("Minimum color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumColorLevelMinutes)))
                    W(" 49) " + DoTranslation("Maximum red color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumRedColorLevelMinutes)))
                    W(" 50) " + DoTranslation("Maximum green color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumGreenColorLevelMinutes)))
                    W(" 51) " + DoTranslation("Maximum blue color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumBlueColorLevelMinutes)))
                    W(" 52) " + DoTranslation("Maximum color level for minutes") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumColorLevelMinutes)))
                    W(" 53) " + DoTranslation("Minimum red color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumRedColorLevelSeconds)))
                    W(" 54) " + DoTranslation("Minimum green color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumGreenColorLevelSeconds)))
                    W(" 55) " + DoTranslation("Minimum blue color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumBlueColorLevelSeconds)))
                    W(" 56) " + DoTranslation("Minimum color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumColorLevelSeconds)))
                    W(" 57) " + DoTranslation("Maximum red color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumRedColorLevelSeconds)))
                    W(" 58) " + DoTranslation("Maximum green color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumGreenColorLevelSeconds)))
                    W(" 59) " + DoTranslation("Maximum blue color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumBlueColorLevelSeconds)))
                    W(" 60) " + DoTranslation("Maximum color level for seconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumColorLevelSeconds)))
                    W(" 61) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumRedColorLevel)))
                    W(" 62) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumGreenColorLevel)))
                    W(" 63) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumBlueColorLevel)))
                    W(" 64) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMinimumColorLevel)))
                    W(" 65) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumRedColorLevel)))
                    W(" 66) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumGreenColorLevel)))
                    W(" 67) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumBlueColorLevel)))
                    W(" 68) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(ProgressClockMaximumColorLevel)))
                Case "11" 'Lighter
                    MaxOptions = 13
                    WriteSeparator("Lighter", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Lighter." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Lighter255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterDelay)))
                    W(" 4) " + DoTranslation("Max Positions Count") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMaxPositions)))
                    W(" 5) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterBackgroundColor)))
                    W(" 6) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMinimumRedColorLevel)))
                    W(" 7) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMinimumGreenColorLevel)))
                    W(" 8) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMinimumBlueColorLevel)))
                    W(" 9) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMinimumColorLevel)))
                    W(" 10) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMaximumRedColorLevel)))
                    W(" 11) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMaximumGreenColorLevel)))
                    W(" 12) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMaximumBlueColorLevel)))
                    W(" 13) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LighterMaximumColorLevel)))
                Case "12" 'Fader
                    MaxOptions = 11
                    WriteSeparator("Fader", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Fader." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderDelay)))
                    W(" 2) " + DoTranslation("Fade Out Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderFadeOutDelay)))
                    W(" 3) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderWrite)))
                    W(" 4) " + DoTranslation("Max Fade Steps") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderMaxSteps)))
                    W(" 5) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackgroundColor)))
                    W(" 6) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderMinimumRedColorLevel)))
                    W(" 7) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderMinimumGreenColorLevel)))
                    W(" 8) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderMinimumBlueColorLevel)))
                    W(" 9) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderMaximumRedColorLevel)))
                    W(" 10) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderMaximumGreenColorLevel)))
                    W(" 11) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderMaximumBlueColorLevel)))
                Case "13" 'Typo
                    MaxOptions = 8
                    WriteSeparator("Typo", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Typo." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoDelay)))
                    W(" 2) " + DoTranslation("Write Again Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoWriteAgainDelay)))
                    W(" 3) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoWrite)))
                    W(" 4) " + DoTranslation("Minimum writing speed in WPM") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoWritingSpeedMin)))
                    W(" 5) " + DoTranslation("Maximum writing speed in WPM") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoWritingSpeedMax)))
                    W(" 6) " + DoTranslation("Probability of typo in percent") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoMissStrikePossibility)))
                    W(" 7) " + DoTranslation("Probability of miss in percent") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoMissPossibility)))
                    W(" 8) " + DoTranslation("Text color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypoTextColor)))
                Case "14" 'Wipe
                    MaxOptions = 13
                    WriteSeparator("Wipe", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Wipe." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Wipe255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeDelay)))
                    W(" 4) " + DoTranslation("Wipes to change direction") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeWipesNeededToChangeDirection)))
                    W(" 5) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeBackgroundColor)))
                    W(" 6) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMinimumRedColorLevel)))
                    W(" 7) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMinimumGreenColorLevel)))
                    W(" 8) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMinimumBlueColorLevel)))
                    W(" 9) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMinimumColorLevel)))
                    W(" 10) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMaximumRedColorLevel)))
                    W(" 11) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMaximumGreenColorLevel)))
                    W(" 12) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMaximumBlueColorLevel)))
                    W(" 13) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(WipeMaximumColorLevel)))
                Case "15" 'Marquee
                    MaxOptions = 6
                    WriteSeparator("Marquee", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Marquee." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Marquee255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeDelay)))
                    W(" 4) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeWrite)))
                    W(" 5) " + DoTranslation("Always centered") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeAlwaysCentered)))
                    W(" 6) " + DoTranslation("Use Console API") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeUseConsoleAPI)))
                    W(" 7) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeBackgroundColor)))
                    W(" 8) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMinimumRedColorLevel)))
                    W(" 9) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMinimumGreenColorLevel)))
                    W(" 10) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMinimumBlueColorLevel)))
                    W(" 11) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMinimumColorLevel)))
                    W(" 12) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMaximumRedColorLevel)))
                    W(" 13) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMaximumGreenColorLevel)))
                    W(" 14) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMaximumBlueColorLevel)))
                    W(" 15) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(MarqueeMaximumColorLevel)))
                Case "16" 'FaderBack
                    MaxOptions = 9
                    WriteSeparator("FaderBack", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " FaderBack." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackDelay)))
                    W(" 2) " + DoTranslation("Fade Out Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackFadeOutDelay)))
                    W(" 3) " + DoTranslation("Max Fade Steps") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackMaxSteps)))
                    W(" 4) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackMinimumRedColorLevel)))
                    W(" 5) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackMinimumGreenColorLevel)))
                    W(" 6) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackMinimumBlueColorLevel)))
                    W(" 7) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackMaximumRedColorLevel)))
                    W(" 8) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackMaximumGreenColorLevel)))
                    W(" 9) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FaderBackMaximumBlueColorLevel)))
                Case "17" 'BeatFader
                    MaxOptions = 14
                    WriteSeparator("BeatFader", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " BeatFader." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFader255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Beats Per Minute") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderDelay)))
                    W(" 4) " + DoTranslation("Cycle colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderCycleColors)))
                    W(" 5) " + DoTranslation("Beat Color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderBeatColor)))
                    W(" 6) " + DoTranslation("Max Fade Steps") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMaxSteps)))
                    W(" 7) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMinimumRedColorLevel)))
                    W(" 8) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMinimumGreenColorLevel)))
                    W(" 9) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMinimumBlueColorLevel)))
                    W(" 10) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMinimumColorLevel)))
                    W(" 11) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMaximumRedColorLevel)))
                    W(" 12) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMaximumGreenColorLevel)))
                    W(" 13) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMaximumBlueColorLevel)))
                    W(" 14) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(BeatFaderMaximumColorLevel)))
                Case "18" 'Linotypo
                    MaxOptions = 12
                    WriteSeparator("Linotypo", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Linotypo." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoDelay)))
                    W(" 2) " + DoTranslation("New Screen Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoNewScreenDelay)))
                    W(" 3) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoWrite)))
                    W(" 4) " + DoTranslation("Minimum writing speed in WPM") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoWritingSpeedMin)))
                    W(" 5) " + DoTranslation("Maximum writing speed in WPM") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoWritingSpeedMax)))
                    W(" 6) " + DoTranslation("Probability of typo in percent") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoMissStrikePossibility)))
                    W(" 7) " + DoTranslation("Column Count") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoTextColumns)))
                    W(" 8) " + DoTranslation("Line Fill Threshold") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoEtaoinThreshold)))
                    W(" 9) " + DoTranslation("Line Fill Capping Probability in percent") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoEtaoinCappingPossibility)))
                    W(" 10) " + DoTranslation("Line Fill Type") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoEtaoinType)))
                    W(" 11) " + DoTranslation("Probability of miss in percent") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoMissPossibility)))
                    W(" 12) " + DoTranslation("Text color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(LinotypoTextColor)))
                Case "19" 'Typewriter
                    MaxOptions = 6
                    WriteSeparator("Typewriter", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Typewriter." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypewriterDelay)))
                    W(" 2) " + DoTranslation("New Screen Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypewriterNewScreenDelay)))
                    W(" 3) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypewriterWrite)))
                    W(" 4) " + DoTranslation("Minimum writing speed in WPM") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypewriterWritingSpeedMin)))
                    W(" 5) " + DoTranslation("Maximum writing speed in WPM") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypewriterWritingSpeedMax)))
                    W(" 6) " + DoTranslation("Text color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(TypewriterTextColor)))
                Case "20" 'FlashColor
                    MaxOptions = 12
                    WriteSeparator("FlashColor", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " FlashColor." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColor255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColorTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColorDelay)))
                    W(" 4) " + DoTranslation("Background color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColorBackgroundColor)))
                    W(" 5) " + DoTranslation("Minimum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColorMinimumRedColorLevel)))
                    W(" 6) " + DoTranslation("Minimum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColorMinimumGreenColorLevel)))
                    W(" 7) " + DoTranslation("Minimum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColorMinimumBlueColorLevel)))
                    W(" 8) " + DoTranslation("Minimum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColorMinimumColorLevel)))
                    W(" 9) " + DoTranslation("Maximum red color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColorMaximumRedColorLevel)))
                    W(" 10) " + DoTranslation("Maximum green color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColorMaximumGreenColorLevel)))
                    W(" 11) " + DoTranslation("Maximum blue color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColorMaximumBlueColorLevel)))
                    W(" 12) " + DoTranslation("Maximum color level") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(FlashColorMaximumColorLevel)))
                Case "21" 'SpotWrite
                    MaxOptions = 4
                    WriteSeparator("SpotWrite", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " SpotWrite." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SpotWriteDelay)))
                    W(" 2) " + DoTranslation("New Screen Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SpotWriteNewScreenDelay)))
                    W(" 3) " + DoTranslation("Text shown") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SpotWriteWrite)))
                    W(" 4) " + DoTranslation("Text color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(SpotWriteTextColor)))
                Case "22" 'Ramp
                    MaxOptions = 28
                    WriteSeparator("Ramp", True)
                    W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " Ramp." + vbNewLine, True, ColTypes.Neutral)
                    W(" 1) " + DoTranslation("Activate 255 colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(Ramp255Colors)))
                    W(" 2) " + DoTranslation("Activate true colors") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampTrueColor)))
                    W(" 3) " + DoTranslation("Delay in Milliseconds") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampDelay)))
                    W(" 4) " + DoTranslation("Next ramp interval") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampNextRampDelay)))
                    W(" 5) " + DoTranslation("Upper left corner character for ramp bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampUpperLeftCornerChar)))
                    W(" 6) " + DoTranslation("Lower left corner character for ramp bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampUpperRightCornerChar)))
                    W(" 7) " + DoTranslation("Upper right corner character for ramp bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampLowerLeftCornerChar)))
                    W(" 8) " + DoTranslation("Lower right corner character for ramp bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampLowerRightCornerChar)))
                    W(" 9) " + DoTranslation("Upper frame character for ramp bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampUpperFrameChar)))
                    W(" 10) " + DoTranslation("Lower frame character for ramp bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampLowerFrameChar)))
                    W(" 11) " + DoTranslation("Left frame character for ramp bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampLeftFrameChar)))
                    W(" 12) " + DoTranslation("Right frame character for ramp bar") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampRightFrameChar)))
                    W(" 13) " + DoTranslation("Minimum red color level for start color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMinimumRedColorLevelStart)))
                    W(" 14) " + DoTranslation("Minimum green color level for start color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMinimumGreenColorLevelStart)))
                    W(" 15) " + DoTranslation("Minimum blue color level for start color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMinimumBlueColorLevelStart)))
                    W(" 16) " + DoTranslation("Minimum color level for start color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMinimumColorLevelStart)))
                    W(" 17) " + DoTranslation("Maximum red color level for start color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMaximumRedColorLevelStart)))
                    W(" 18) " + DoTranslation("Maximum green color level for start color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMaximumGreenColorLevelStart)))
                    W(" 19) " + DoTranslation("Maximum blue color level for start color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMaximumBlueColorLevelStart)))
                    W(" 20) " + DoTranslation("Maximum color level for start color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMaximumColorLevelStart)))
                    W(" 21) " + DoTranslation("Minimum red color level for end color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMinimumRedColorLevelEnd)))
                    W(" 22) " + DoTranslation("Minimum green color level for end color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMinimumGreenColorLevelEnd)))
                    W(" 23) " + DoTranslation("Minimum blue color level for end color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMinimumBlueColorLevelEnd)))
                    W(" 24) " + DoTranslation("Minimum color level for end color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMinimumColorLevelEnd)))
                    W(" 25) " + DoTranslation("Maximum red color level for end color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMaximumRedColorLevelEnd)))
                    W(" 26) " + DoTranslation("Maximum green color level for end color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMaximumGreenColorLevelEnd)))
                    W(" 27) " + DoTranslation("Maximum blue color level for end color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMaximumBlueColorLevelEnd)))
                    W(" 28) " + DoTranslation("Maximum color level for end color") + " [{0}]", True, ColTypes.Option, GetConfigValueField(NameOf(RampMaximumColorLevelEnd)))
                Case $"{If(SectionParameters.Length <> 0, SectionParameters(0), $"{BuiltinSavers + 1}")}" 'a custom saver
                    Dim SaverIndex As Integer = SectionParameters(0) - BuiltinSavers - 1
                    Dim Configurables As List(Of String) = SectionParameters(1)
                    Dim OptionNumber As Integer = 1
                    If CustomSavers(Configurables(SaverIndex)).Screensaver.SaverSettings IsNot Nothing Then
                        MaxOptions = CustomSavers(Configurables(SaverIndex)).Screensaver.SaverSettings.Count
                        WriteSeparator("{0}" + vbNewLine, True, Configurables(SaverIndex))
                        W(vbNewLine + DoTranslation("This section lists screensaver settings for") + " {0}." + vbNewLine, True, ColTypes.Neutral, Configurables(SaverIndex))
                        For Each Setting As String In CustomSavers(Configurables(SaverIndex)).Screensaver.SaverSettings.Keys
                            W(" {0}) {1} [{2}]", True, ColTypes.Option, OptionNumber, Setting, CustomSavers(Configurables(SaverIndex)).Screensaver.SaverSettings(Setting))
                            OptionNumber += 1
                        Next
                    End If
                Case Else 'Invalid section
                    WriteSeparator("*) ???", True)
                    W(vbNewLine + "X) " + DoTranslation("Invalid section entered. Please go back."), True, ColTypes.Error)
            End Select
            Console.WriteLine()
            W(" {0}) " + DoTranslation("Go Back...") + vbNewLine, True, ColTypes.BackOption, MaxOptions + 1)
            Wdbg(DebugLevel.W, "Section {0} has {1} selections.", SectionNum, MaxOptions)

            'Prompt user and check for input
            W("> ", False, ColTypes.Input)
            AnswerString = Console.ReadLine
            Wdbg(DebugLevel.I, "User answered {0}", AnswerString)
            Console.WriteLine()

            Wdbg(DebugLevel.I, "Is the answer numeric? {0}", IsNumeric(AnswerString))
            If Integer.TryParse(AnswerString, AnswerInt) Then
                Wdbg(DebugLevel.I, "Succeeded. Checking the answer if it points to the right direction...")
                If AnswerInt >= 1 And AnswerInt <= MaxOptions Then
                    Wdbg(DebugLevel.I, "Opening key {0} from section {1}...", AnswerInt, SectionNum)
                    OpenKeySaver(SectionNum, AnswerInt)
                ElseIf AnswerInt = MaxOptions + 1 Then 'Go Back...
                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                    SectionFinished = True
                Else
                    Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            Else
                Wdbg(DebugLevel.W, "Answer is not numeric.")
                W(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                Console.ReadKey()
            End If
        End While
    End Sub

    ''' <summary>
    ''' Open a key.
    ''' </summary>
    ''' <param name="Section">Section number</param>
    ''' <param name="KeyNumber">Key number</param>
    Sub OpenKeySaver(Section As String, KeyNumber As Integer)
        Dim MaxKeyOptions As Integer = 0
        Dim KeyFinished As Boolean
        Dim KeyType As SettingsKeyType = SettingsKeyType.SUnknown
        Dim KeyVar As String = ""
        Dim KeyValue As Object = ""
        Dim VariantValue As Object = ""
        Dim VariantValueFromExternalPrompt As Boolean
        Dim AnswerString As String = ""
        Dim AnswerInt As Integer
        Dim SectionParts() As String = Section.Split(".")
        Dim ListJoinString As String = ""
        Dim TargetList As IEnumerable(Of Object)
        Dim SelectFrom As IEnumerable(Of Object)
        Dim SelectionEnumZeroBased As Boolean
        Dim NeutralizePaths As Boolean
        Dim NeutralizeRootPath As String = CurrDir
        Dim BuiltinSavers As Integer = ConfigToken("Screensaver").Count - 4 'Screensaver - Keys

        While Not KeyFinished
            Console.Clear()
            'List Keys for specified section
            Select Case Section
                Case "1" 'ColorMix
                    Select Case KeyNumber
                        Case 1 'ColorMix: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ColorMix255Colors)
                            WriteSeparator("ColorMix > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for ColorMix."), True, ColTypes.Neutral)
                        Case 2 'ColorMix: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ColorMixTrueColor)
                            WriteSeparator("ColorMix > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for ColorMix."), True, ColTypes.Neutral)
                        Case 3 'ColorMix: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixDelay)
                            WriteSeparator("ColorMix > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'ColorMix: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ColorMixBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ColorMixBackgroundColor).Type = ColorType.TrueColor, If(New Color(ColorMixBackgroundColor).Type = ColorType._255Color, New Color(ColorMixBackgroundColor).PlainSequence, ConsoleColors.Black), New Color(ColorMixBackgroundColor).R, New Color(ColorMixBackgroundColor).G, New Color(ColorMixBackgroundColor).B)
                        Case 5 'ColorMix: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMinimumRedColorLevel)
                            WriteSeparator("ColorMix > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 6 'ColorMix: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMinimumGreenColorLevel)
                            WriteSeparator("ColorMix > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'ColorMix: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMinimumBlueColorLevel)
                            WriteSeparator("ColorMix > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'ColorMix: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMinimumColorLevel)
                            WriteSeparator("ColorMix > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 9 'ColorMix: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMaximumRedColorLevel)
                            WriteSeparator("ColorMix > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 10 'ColorMix: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMaximumGreenColorLevel)
                            WriteSeparator("ColorMix > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'ColorMix: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMaximumBlueColorLevel)
                            WriteSeparator("ColorMix > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'ColorMix: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ColorMixMaximumColorLevel)
                            WriteSeparator("ColorMix > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("ColorMix > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "2" 'Matrix
                    Select Case KeyNumber
                        Case 1 'Matrix: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MatrixDelay)
                            WriteSeparator("Matrix > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("Matrix > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "3" 'GlitterMatrix
                    Select Case KeyNumber
                        Case 1 'GlitterMatrix: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterMatrixDelay)
                            WriteSeparator("GlitterMatrix > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'GlitterMatrix: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(GlitterMatrixBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(GlitterMatrixBackgroundColor).Type = ColorType.TrueColor, If(New Color(GlitterMatrixBackgroundColor).Type = ColorType._255Color, New Color(GlitterMatrixBackgroundColor).PlainSequence, ConsoleColors.Black), New Color(GlitterMatrixBackgroundColor).R, New Color(GlitterMatrixBackgroundColor).G, New Color(GlitterMatrixBackgroundColor).B)
                        Case 3 'GlitterMatrix: Foreground color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(GlitterMatrixForegroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(GlitterMatrixForegroundColor).Type = ColorType.TrueColor, If(New Color(GlitterMatrixForegroundColor).Type = ColorType._255Color, New Color(GlitterMatrixForegroundColor).PlainSequence, ConsoleColors.Green), New Color(GlitterMatrixForegroundColor).R, New Color(GlitterMatrixForegroundColor).G, New Color(GlitterMatrixForegroundColor).B)
                        Case Else
                            WriteSeparator("GlitterMatrix > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "4" 'Disco
                    Select Case KeyNumber
                        Case 1 'Disco: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Disco255Colors)
                            WriteSeparator("Disco > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for Disco."), True, ColTypes.Neutral)
                        Case 2 'Disco: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DiscoTrueColor)
                            WriteSeparator("Disco > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for Disco."), True, ColTypes.Neutral)
                        Case 3 'Disco: Cycle colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DiscoCycleColors)
                            WriteSeparator("Disco > " + DoTranslation("Cycle colors"), True)
                            W(vbNewLine + DoTranslation("Disco will cycle colors when enabled. Otherwise, select random colors."), True, ColTypes.Neutral)
                        Case 4 'Disco: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoDelay)
                            WriteSeparator("Disco > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 5 'Disco: Use Beats Per Second
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DiscoUseBeatsPerMinute)
                            WriteSeparator("Disco > " + DoTranslation("Use Beats Per Minute"), True)
                            W(vbNewLine + DoTranslation("Whether to use the Beats Per Minute unit to write the next color."), True, ColTypes.Neutral)
                        Case 6 'Disco: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMinimumRedColorLevel)
                            WriteSeparator("Disco > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'Disco: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMinimumGreenColorLevel)
                            WriteSeparator("Disco > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'Disco: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMinimumBlueColorLevel)
                            WriteSeparator("Disco > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'Disco: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMinimumColorLevel)
                            WriteSeparator("Disco > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 10 'Disco: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMaximumRedColorLevel)
                            WriteSeparator("Disco > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'Disco: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMaximumGreenColorLevel)
                            WriteSeparator("Disco > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'Disco: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMaximumBlueColorLevel)
                            WriteSeparator("Disco > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'Disco: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DiscoMaximumColorLevel)
                            WriteSeparator("Disco > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("Disco > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "5" 'Lines
                    Select Case KeyNumber
                        Case 1 'Lines: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Lines255Colors)
                            WriteSeparator("Lines > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for Lines."), True, ColTypes.Neutral)
                        Case 2 'Lines: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(LinesTrueColor)
                            WriteSeparator("Lines > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for Lines."), True, ColTypes.Neutral)
                        Case 3 'Lines: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesDelay)
                            WriteSeparator("Lines > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'Lines: Line character
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(LinesLineChar)
                            WriteSeparator("Lines > " + DoTranslation("Line character"), True)
                            W(vbNewLine + DoTranslation("A character to form a line. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 5 'Lines: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(LinesBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(LinesBackgroundColor).Type = ColorType.TrueColor, If(New Color(LinesBackgroundColor).Type = ColorType._255Color, New Color(LinesBackgroundColor).PlainSequence, ConsoleColors.Black), New Color(LinesBackgroundColor).R, New Color(LinesBackgroundColor).G, New Color(LinesBackgroundColor).B)
                        Case 6 'Lines: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMinimumRedColorLevel)
                            WriteSeparator("Lines > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'Lines: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMinimumGreenColorLevel)
                            WriteSeparator("Lines > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'Lines: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMinimumBlueColorLevel)
                            WriteSeparator("Lines > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'Lines: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMinimumColorLevel)
                            WriteSeparator("Lines > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 10 'Lines: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMaximumRedColorLevel)
                            WriteSeparator("Lines > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'Lines: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMaximumGreenColorLevel)
                            WriteSeparator("Lines > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'Lines: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMaximumBlueColorLevel)
                            WriteSeparator("Lines > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'Lines: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinesMaximumColorLevel)
                            WriteSeparator("Lines > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("Lines > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "6" 'GlitterColor
                    Select Case KeyNumber
                        Case 1 'GlitterColor: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(GlitterColor255Colors)
                            WriteSeparator("GlitterColor > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for GlitterColor."), True, ColTypes.Neutral)
                        Case 2 'GlitterColor: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(GlitterColorTrueColor)
                            WriteSeparator("GlitterColor > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for GlitterColor."), True, ColTypes.Neutral)
                        Case 3 'GlitterColor: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorDelay)
                            WriteSeparator("GlitterColor > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'GlitterColor: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMinimumRedColorLevel)
                            WriteSeparator("GlitterColor > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 5 'GlitterColor: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMinimumGreenColorLevel)
                            WriteSeparator("GlitterColor > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 6 'GlitterColor: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMinimumBlueColorLevel)
                            WriteSeparator("GlitterColor > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'GlitterColor: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMinimumColorLevel)
                            WriteSeparator("GlitterColor > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 8 'GlitterColor: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMaximumRedColorLevel)
                            WriteSeparator("GlitterColor > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'GlitterColor: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMaximumGreenColorLevel)
                            WriteSeparator("GlitterColor > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 10 'GlitterColor: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMaximumBlueColorLevel)
                            WriteSeparator("GlitterColor > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'GlitterColor: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(GlitterColorMaximumColorLevel)
                            WriteSeparator("GlitterColor > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("GlitterColor > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "7" 'BouncingText
                    Select Case KeyNumber
                        Case 1 'BouncingText: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BouncingText255Colors)
                            WriteSeparator("BouncingText > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for BouncingText."), True, ColTypes.Neutral)
                        Case 2 'BouncingText: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BouncingTextTrueColor)
                            WriteSeparator("BouncingText > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for BouncingText."), True, ColTypes.Neutral)
                        Case 3 'BouncingText: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextDelay)
                            WriteSeparator("BouncingText > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'BouncingText: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(BouncingTextWrite)
                            WriteSeparator("BouncingText > " + DoTranslation("Text shown"), True)
                            W(vbNewLine + DoTranslation("Write any text you want shown. Shorter is better."), True, ColTypes.Neutral)
                        Case 5 'BouncingText: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BouncingTextBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BouncingTextBackgroundColor).Type = ColorType.TrueColor, If(New Color(BouncingTextBackgroundColor).Type = ColorType._255Color, New Color(BouncingTextBackgroundColor).PlainSequence, ConsoleColors.Black), New Color(BouncingTextBackgroundColor).R, New Color(BouncingTextBackgroundColor).G, New Color(BouncingTextBackgroundColor).B)
                        Case 6 'BouncingText: Foreground color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BouncingTextForegroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BouncingTextForegroundColor).Type = ColorType.TrueColor, If(New Color(BouncingTextForegroundColor).Type = ColorType._255Color, New Color(BouncingTextForegroundColor).PlainSequence, ConsoleColors.White), New Color(BouncingTextForegroundColor).R, New Color(BouncingTextForegroundColor).G, New Color(BouncingTextForegroundColor).B)
                        Case 7 'BouncingText: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMinimumRedColorLevel)
                            WriteSeparator("BouncingText > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'BouncingText: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMinimumGreenColorLevel)
                            WriteSeparator("BouncingText > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'BouncingText: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMinimumBlueColorLevel)
                            WriteSeparator("BouncingText > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 10 'BouncingText: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMinimumColorLevel)
                            WriteSeparator("BouncingText > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 11 'BouncingText: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMaximumRedColorLevel)
                            WriteSeparator("BouncingText > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'BouncingText: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMaximumGreenColorLevel)
                            WriteSeparator("BouncingText > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'BouncingText: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMaximumBlueColorLevel)
                            WriteSeparator("BouncingText > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 14 'BouncingText: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingTextMaximumColorLevel)
                            WriteSeparator("BouncingText > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("BouncingText > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "8" 'Dissolve
                    Select Case KeyNumber
                        Case 1 'Dissolve: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Dissolve255Colors)
                            WriteSeparator("Dissolve > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for Dissolve."), True, ColTypes.Neutral)
                        Case 2 'Dissolve: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(DissolveTrueColor)
                            WriteSeparator("Dissolve > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for Dissolve."), True, ColTypes.Neutral)
                        Case 5 'Dissolve: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(DissolveBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(DissolveBackgroundColor).Type = ColorType.TrueColor, If(New Color(DissolveBackgroundColor).Type = ColorType._255Color, New Color(DissolveBackgroundColor).PlainSequence, ConsoleColors.Black), New Color(DissolveBackgroundColor).R, New Color(DissolveBackgroundColor).G, New Color(DissolveBackgroundColor).B)
                        Case 6 'Dissolve: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMinimumRedColorLevel)
                            WriteSeparator("Dissolve > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'Dissolve: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMinimumGreenColorLevel)
                            WriteSeparator("Dissolve > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'Dissolve: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMinimumBlueColorLevel)
                            WriteSeparator("Dissolve > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'Dissolve: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMinimumColorLevel)
                            WriteSeparator("Dissolve > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 10 'Dissolve: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMaximumRedColorLevel)
                            WriteSeparator("Dissolve > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'Dissolve: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMaximumGreenColorLevel)
                            WriteSeparator("Dissolve > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'Dissolve: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMaximumBlueColorLevel)
                            WriteSeparator("Dissolve > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'Dissolve: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(DissolveMaximumColorLevel)
                            WriteSeparator("Dissolve > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("Dissolve > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "9" 'BouncingBlock
                    Select Case KeyNumber
                        Case 1 'BouncingBlock: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BouncingBlock255Colors)
                            WriteSeparator("BouncingBlock > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for BouncingBlock."), True, ColTypes.Neutral)
                        Case 2 'BouncingBlock: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BouncingBlockTrueColor)
                            WriteSeparator("BouncingBlock > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for BouncingBlock."), True, ColTypes.Neutral)
                        Case 3 'BouncingBlock: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockDelay)
                            WriteSeparator("BouncingBlock > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'BouncingBlock: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BouncingBlockBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BouncingBlockBackgroundColor).Type = ColorType.TrueColor, If(New Color(BouncingBlockBackgroundColor).Type = ColorType._255Color, New Color(BouncingBlockBackgroundColor).PlainSequence, ConsoleColors.Black), New Color(BouncingBlockBackgroundColor).R, New Color(BouncingBlockBackgroundColor).G, New Color(BouncingBlockBackgroundColor).B)
                        Case 5 'BouncingBlock: Foreground color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BouncingBlockForegroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BouncingBlockForegroundColor).Type = ColorType.TrueColor, If(New Color(BouncingBlockForegroundColor).Type = ColorType._255Color, New Color(BouncingBlockForegroundColor).PlainSequence, ConsoleColors.White), New Color(BouncingBlockForegroundColor).R, New Color(BouncingBlockForegroundColor).G, New Color(BouncingBlockForegroundColor).B)
                        Case 6 'BouncingBlock: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMinimumRedColorLevel)
                            WriteSeparator("BouncingBlock > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'BouncingBlock: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMinimumGreenColorLevel)
                            WriteSeparator("BouncingBlock > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'BouncingBlock: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMinimumBlueColorLevel)
                            WriteSeparator("BouncingBlock > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'BouncingBlock: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMinimumColorLevel)
                            WriteSeparator("BouncingBlock > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 10 'BouncingBlock: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMaximumRedColorLevel)
                            WriteSeparator("BouncingBlock > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'BouncingBlock: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMaximumGreenColorLevel)
                            WriteSeparator("BouncingBlock > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'BouncingBlock: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMaximumBlueColorLevel)
                            WriteSeparator("BouncingBlock > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'BouncingBlock: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BouncingBlockMaximumColorLevel)
                            WriteSeparator("BouncingBlock > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("BouncingBlock > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "10" 'ProgressClock
                    Select Case KeyNumber
                        Case 1 'ProgressClock: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ProgressClock255Colors)
                            WriteSeparator("ProgressClock > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for ProgressClock."), True, ColTypes.Neutral)
                        Case 2 'ProgressClock: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ProgressClockTrueColor)
                            WriteSeparator("ProgressClock > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for ProgressClock."), True, ColTypes.Neutral)
                        Case 3 'ProgressClock: Cycle colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(ProgressClockCycleColors)
                            WriteSeparator("ProgressClock > " + DoTranslation("Cycle colors"), True)
                            W(vbNewLine + DoTranslation("ProgressClock will select random colors if it's enabled. Otherwise, use colors from config."), True, ColTypes.Neutral)
                        Case 4 'ProgressClock: Color of Seconds Bar
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ProgressClockSecondsProgressColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ProgressClockSecondsProgressColor).Type = ColorType.TrueColor, If(New Color(ProgressClockSecondsProgressColor).Type = ColorType._255Color, New Color(ProgressClockSecondsProgressColor).PlainSequence, ConsoleColors.DarkBlue), New Color(ProgressClockSecondsProgressColor).R, New Color(ProgressClockSecondsProgressColor).G, New Color(ProgressClockSecondsProgressColor).B)
                        Case 5 'ProgressClock: Color of Minutes Bar
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ProgressClockMinutesProgressColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ProgressClockMinutesProgressColor).Type = ColorType.TrueColor, If(New Color(ProgressClockMinutesProgressColor).Type = ColorType._255Color, New Color(ProgressClockMinutesProgressColor).PlainSequence, ConsoleColors.DarkMagenta), New Color(ProgressClockMinutesProgressColor).R, New Color(ProgressClockMinutesProgressColor).G, New Color(ProgressClockMinutesProgressColor).B)
                        Case 6 'ProgressClock: Color of Hours Bar
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ProgressClockHoursProgressColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ProgressClockHoursProgressColor).Type = ColorType.TrueColor, If(New Color(ProgressClockHoursProgressColor).Type = ColorType._255Color, New Color(ProgressClockHoursProgressColor).PlainSequence, ConsoleColors.DarkCyan), New Color(ProgressClockHoursProgressColor).R, New Color(ProgressClockHoursProgressColor).G, New Color(ProgressClockHoursProgressColor).B)
                        Case 7 'ProgressClock: Color of Information
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(ProgressClockProgressColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(ProgressClockProgressColor).Type = ColorType.TrueColor, If(New Color(ProgressClockProgressColor).Type = ColorType._255Color, New Color(ProgressClockProgressColor).PlainSequence, ConsoleColors.Gray), New Color(ProgressClockProgressColor).R, New Color(ProgressClockProgressColor).G, New Color(ProgressClockProgressColor).B)
                        Case 8 'ProgressClock: Ticks to change color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockCycleColorsTicks)
                            WriteSeparator("ProgressClock > " + DoTranslation("Ticks to change color"), True)
                            W(vbNewLine + DoTranslation("If color cycling is enabled, how many ticks before changing colors in ProgressClock? 1 tick = 0.5 seconds"), True, ColTypes.Neutral)
                        Case 9 'ProgressClock: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockDelay)
                            WriteSeparator("ProgressClock > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 10 'ProgressClock: Upper left corner character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperLeftCornerCharHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Upper left corner character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 11 'ProgressClock: Upper left corner character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperLeftCornerCharMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Upper left corner character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 12 'ProgressClock: Upper left corner character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperLeftCornerCharSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Upper left corner character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 13 'ProgressClock: Lower left corner character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerLeftCornerCharHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Lower left corner character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 14 'ProgressClock: Lower left corner character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerLeftCornerCharMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Lower left corner character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 15 'ProgressClock: Lower left corner character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerLeftCornerCharSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Lower left corner character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 16 'ProgressClock: Upper right corner character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperRightCornerCharHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Upper right corner character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 17 'ProgressClock: Upper right corner character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperRightCornerCharMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Upper right corner character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 18 'ProgressClock: Upper right corner character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperRightCornerCharSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Upper right corner character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 19 'ProgressClock: Lower right corner character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerRightCornerCharHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Lower right corner character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 20 'ProgressClock: Lower right corner character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerRightCornerCharMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Lower right corner character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 21 'ProgressClock: Lower right corner character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerRightCornerCharSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Lower right corner character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 22 'ProgressClock: Upper frame character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperFrameCharHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Upper frame character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 23 'ProgressClock: Upper frame character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperFrameCharMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Upper frame character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 24 'ProgressClock: Upper frame character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockUpperFrameCharSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Upper frame character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 25 'ProgressClock: Lower frame character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerFrameCharHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Lower frame character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 26 'ProgressClock: Lower frame character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerFrameCharMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Lower frame character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 27 'ProgressClock: Lower frame character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLowerFrameCharSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Lower frame character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 28 'ProgressClock: Left frame character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLeftFrameCharHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Left frame character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the left frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 29 'ProgressClock: Left frame character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLeftFrameCharMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Left frame character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the left frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 30 'ProgressClock: Left frame character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockLeftFrameCharSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Left frame character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the left frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 31 'ProgressClock: Right frame character for hours bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockRightFrameCharHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Right frame character for hours bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the right frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 32 'ProgressClock: Right frame character for minutes bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockRightFrameCharMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Right frame character for minutes bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the right frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 33 'ProgressClock: Right frame character for seconds bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockRightFrameCharSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Right frame character for seconds bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the right frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 34 'ProgressClock: Information text for hours
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockInfoTextHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Information text for hours"), True)
                            W(vbNewLine + DoTranslation("Write how your information text for the current hour shows. {0} for current hour out of 24 hours."), True, ColTypes.Neutral)
                        Case 35 'ProgressClock: Information text for minutes
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockInfoTextMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Information text for minutes"), True)
                            W(vbNewLine + DoTranslation("Write how your information text for the current minute shows. {0} for current minute out of 60 minutes."), True, ColTypes.Neutral)
                        Case 36 'ProgressClock: Information text for seconds
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(ProgressClockInfoTextSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Information text for seconds"), True)
                            W(vbNewLine + DoTranslation("Write how your information text for the current second shows. {0} for current second out of 60 seconds."), True, ColTypes.Neutral)
                        Case 37 'ProgressClock: Minimum red color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumRedColorLevelHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum red color level for hours"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 38 'ProgressClock: Minimum green color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumGreenColorLevelHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum green color level for hours"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 39 'ProgressClock: Minimum blue color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumBlueColorLevelHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum blue color level for hours"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 44 'ProgressClock: Minimum color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumColorLevelHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum color level for hours"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 41 'ProgressClock: Maximum red color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumRedColorLevelHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum red color level for hours"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 42 'ProgressClock: Maximum green color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumGreenColorLevelHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum green color level for hours"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 43 'ProgressClock: Maximum blue color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumBlueColorLevelHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum blue color level for hours"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 44 'ProgressClock: Maximum color level for hours
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumColorLevelHours)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum color level for hours"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 45 'ProgressClock: Minimum red color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumRedColorLevelMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum red color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 46 'ProgressClock: Minimum green color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumGreenColorLevelMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum green color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 47 'ProgressClock: Minimum blue color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumBlueColorLevelMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum blue color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 48 'ProgressClock: Minimum color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumColorLevelMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 49 'ProgressClock: Maximum red color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumRedColorLevelMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum red color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 50 'ProgressClock: Maximum green color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumGreenColorLevelMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum green color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 51 'ProgressClock: Maximum blue color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumBlueColorLevelMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum blue color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 52 'ProgressClock: Maximum color level for minutes
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumColorLevelMinutes)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum color level for minutes"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 53 'ProgressClock: Minimum red color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumRedColorLevelSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum red color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 54 'ProgressClock: Minimum green color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumGreenColorLevelSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum green color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 55 'ProgressClock: Minimum blue color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumBlueColorLevelSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum blue color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 56 'ProgressClock: Minimum color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumColorLevelSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 57 'ProgressClock: Maximum red color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumRedColorLevelSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum red color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 58 'ProgressClock: Maximum green color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumGreenColorLevelSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum green color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 59 'ProgressClock: Maximum blue color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumBlueColorLevelSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum blue color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 60 'ProgressClock: Maximum color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumColorLevelSeconds)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum color level for seconds"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 61 'ProgressClock: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumRedColorLevel)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 62 'ProgressClock: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumGreenColorLevel)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 63 'ProgressClock: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumBlueColorLevel)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 64 'ProgressClock: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMinimumColorLevel)
                            WriteSeparator("ProgressClock > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 65 'ProgressClock: Maximum red color level for seconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumRedColorLevel)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 66 'ProgressClock: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumGreenColorLevel)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 67 'ProgressClock: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumBlueColorLevel)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 68 'ProgressClock: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(ProgressClockMaximumColorLevel)
                            WriteSeparator("ProgressClock > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("ProgressClock > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "11" 'Lighter
                    Select Case KeyNumber
                        Case 1 'Lighter: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Lighter255Colors)
                            WriteSeparator("Lighter > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for Lighter."), True, ColTypes.Neutral)
                        Case 2 'Lighter: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(LighterTrueColor)
                            WriteSeparator("Lighter > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for Lighter."), True, ColTypes.Neutral)
                        Case 3 'Lighter: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterDelay)
                            WriteSeparator("Lighter > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'Lighter: Max Positions Count
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMaxPositions)
                            WriteSeparator("Lighter > " + DoTranslation("Max Positions Count"), True)
                            W(vbNewLine + DoTranslation("How many positions are lit before dimming?"), True, ColTypes.Neutral)
                        Case 5 'Lighter: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(LighterBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(LighterBackgroundColor).Type = ColorType.TrueColor, If(New Color(LighterBackgroundColor).Type = ColorType._255Color, New Color(LighterBackgroundColor).PlainSequence, ConsoleColors.White), New Color(LighterBackgroundColor).R, New Color(LighterBackgroundColor).G, New Color(LighterBackgroundColor).B)
                        Case 6 'Lighter: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMinimumRedColorLevel)
                            WriteSeparator("Lighter > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'Lighter: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMinimumGreenColorLevel)
                            WriteSeparator("Lighter > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'Lighter: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMinimumBlueColorLevel)
                            WriteSeparator("Lighter > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'Lighter: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMinimumColorLevel)
                            WriteSeparator("Lighter > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 10 'Lighter: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMaximumRedColorLevel)
                            WriteSeparator("Lighter > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'Lighter: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMaximumGreenColorLevel)
                            WriteSeparator("Lighter > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'Lighter: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMaximumBlueColorLevel)
                            WriteSeparator("Lighter > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'Lighter: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LighterMaximumColorLevel)
                            WriteSeparator("Lighter > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("Lighter > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "12" 'Fader
                    Select Case KeyNumber
                        Case 1 'Fader: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderDelay)
                            WriteSeparator("Fader > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'Fader: Fade Out Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderFadeOutDelay)
                            WriteSeparator("Fader > " + DoTranslation("Fade Out Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before fading out text?"), True, ColTypes.Neutral)
                        Case 3 'Fader: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(FaderWrite)
                            WriteSeparator("Fader > " + DoTranslation("Text shown"), True)
                            W(vbNewLine + DoTranslation("Write any text you want shown. Shorter is better."), True, ColTypes.Neutral)
                        Case 4 'Fader: Max Fade Steps
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMaxSteps)
                            WriteSeparator("Fader > " + DoTranslation("Max Fade Steps"), True)
                            W(vbNewLine + DoTranslation("How many fade steps to do?"), True, ColTypes.Neutral)
                        Case 5 'Fader: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(FaderBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(FaderBackgroundColor).Type = ColorType.TrueColor, If(New Color(FaderBackgroundColor).Type = ColorType._255Color, New Color(FaderBackgroundColor).PlainSequence, ConsoleColors.White), New Color(FaderBackgroundColor).R, New Color(FaderBackgroundColor).G, New Color(FaderBackgroundColor).B)
                        Case 6 'Fader: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMinimumRedColorLevel)
                            WriteSeparator("Fader > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'Fader: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMinimumGreenColorLevel)
                            WriteSeparator("Fader > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'Fader: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMinimumBlueColorLevel)
                            WriteSeparator("Fader > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'Fader: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMaximumRedColorLevel)
                            WriteSeparator("Fader > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 10 'Fader: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMaximumGreenColorLevel)
                            WriteSeparator("Fader > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'Fader: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderMaximumBlueColorLevel)
                            WriteSeparator("Fader > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("Fader > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "13" 'Typo
                    Select Case KeyNumber
                        Case 1 'Typo: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoDelay)
                            WriteSeparator("Typo > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'Typo: Write Again Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoWriteAgainDelay)
                            WriteSeparator("Typo > " + DoTranslation("Write Again Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before writing text again?"), True, ColTypes.Neutral)
                        Case 3 'Typo: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(TypoWrite)
                            WriteSeparator("Typo > " + DoTranslation("Text shown"), True)
                            W(vbNewLine + DoTranslation("Write any text you want shown. Longer is better."), True, ColTypes.Neutral)
                        Case 4 'Typo: Minimum writing speed in WPM
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoWritingSpeedMin)
                            WriteSeparator("Typo > " + DoTranslation("Minimum writing speed in WPM"), True)
                            W(vbNewLine + DoTranslation("Minimum writing speed in WPM"), True, ColTypes.Neutral)
                        Case 5 'Typo: Maximum writing speed in WPM
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoWritingSpeedMax)
                            WriteSeparator("Typo > " + DoTranslation("Maximum writing speed in WPM"), True)
                            W(vbNewLine + DoTranslation("Maximum writing speed in WPM"), True, ColTypes.Neutral)
                        Case 6 'Typo: Probability of typo in percent
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoMissStrikePossibility)
                            WriteSeparator("Typo > " + DoTranslation("Probability of typo in percent"), True)
                            W(vbNewLine + DoTranslation("Probability of typo in percent"), True, ColTypes.Neutral)
                        Case 7 'Typo: Probability of miss in percent
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypoMissPossibility)
                            WriteSeparator("Typo > " + DoTranslation("Probability of miss in percent"), True)
                            W(vbNewLine + DoTranslation("Probability of miss in percent"), True, ColTypes.Neutral)
                        Case 8 'Typo: Text color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(TypoTextColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(TypoTextColor).Type = ColorType.TrueColor, If(New Color(TypoTextColor).Type = ColorType._255Color, New Color(TypoTextColor).PlainSequence, ConsoleColors.White), New Color(TypoTextColor).R, New Color(TypoTextColor).G, New Color(TypoTextColor).B)
                        Case Else
                            WriteSeparator("Typo > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "14" 'Wipe
                    Select Case KeyNumber
                        Case 1 'Wipe: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Wipe255Colors)
                            WriteSeparator("Wipe > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for Wipe."), True, ColTypes.Neutral)
                        Case 2 'Wipe: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(WipeTrueColor)
                            WriteSeparator("Wipe > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for Wipe."), True, ColTypes.Neutral)
                        Case 3 'Wipe: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeDelay)
                            WriteSeparator("Wipe > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'Wipe: Wipes to change direction
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeWipesNeededToChangeDirection)
                            WriteSeparator("Wipe > " + DoTranslation("Wipes to change direction"), True)
                            W(vbNewLine + DoTranslation("How many wipes to do before changing direction randomly?"), True, ColTypes.Neutral)
                        Case 5 'Wipe: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(WipeBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(WipeBackgroundColor).Type = ColorType.TrueColor, If(New Color(WipeBackgroundColor).Type = ColorType._255Color, New Color(WipeBackgroundColor).PlainSequence, ConsoleColors.White), New Color(WipeBackgroundColor).R, New Color(WipeBackgroundColor).G, New Color(WipeBackgroundColor).B)
                        Case 6 'Wipe: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMinimumRedColorLevel)
                            WriteSeparator("Wipe > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'Wipe: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMinimumGreenColorLevel)
                            WriteSeparator("Wipe > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'Wipe: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMinimumBlueColorLevel)
                            WriteSeparator("Wipe > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'Wipe: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMinimumColorLevel)
                            WriteSeparator("Wipe > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 10 'Wipe: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMaximumRedColorLevel)
                            WriteSeparator("Wipe > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'Wipe: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMaximumGreenColorLevel)
                            WriteSeparator("Wipe > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'Wipe: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMaximumBlueColorLevel)
                            WriteSeparator("Wipe > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'Wipe: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(WipeMaximumColorLevel)
                            WriteSeparator("Wipe > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("Wipe > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "15" 'Marquee
                    Select Case KeyNumber
                        Case 1 'Marquee: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Marquee255Colors)
                            WriteSeparator("Marquee > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for Marquee."), True, ColTypes.Neutral)
                        Case 2 'Marquee: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(MarqueeTrueColor)
                            WriteSeparator("Marquee > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for Marquee."), True, ColTypes.Neutral)
                        Case 3 'Marquee: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeDelay)
                            WriteSeparator("Marquee > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'Marquee: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(MarqueeWrite)
                            WriteSeparator("Marquee > " + DoTranslation("Text shown"), True)
                            W(vbNewLine + DoTranslation("Write any text you want shown."), True, ColTypes.Neutral)
                        Case 5 'Marquee: Always centered
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(MarqueeAlwaysCentered)
                            WriteSeparator("Marquee > " + DoTranslation("Always centered"), True)
                            W(vbNewLine + DoTranslation("Whether the text shown on the marquee is always centered."), True, ColTypes.Neutral)
                        Case 6 'Marquee: Use Console API
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(MarqueeUseConsoleAPI)
                            WriteSeparator("Marquee > " + DoTranslation("Use Console API"), True)
                            W(vbNewLine + DoTranslation("Whether to use the Console API to clear text or to use the faster line clearing VT sequence. If False, Marquee will use the appropriate VT sequence. Otherwise, it will use the probably slower Console API."), True, ColTypes.Neutral)
                        Case 7 'Marquee: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(MarqueeBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(MarqueeBackgroundColor).Type = ColorType.TrueColor, If(New Color(MarqueeBackgroundColor).Type = ColorType._255Color, New Color(MarqueeBackgroundColor).PlainSequence, ConsoleColors.White), New Color(MarqueeBackgroundColor).R, New Color(MarqueeBackgroundColor).G, New Color(MarqueeBackgroundColor).B)
                        Case 8 'Marquee: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMinimumRedColorLevel)
                            WriteSeparator("Marquee > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'Marquee: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMinimumGreenColorLevel)
                            WriteSeparator("Marquee > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 10 'Marquee: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMinimumBlueColorLevel)
                            WriteSeparator("Marquee > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'Marquee: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMinimumColorLevel)
                            WriteSeparator("Marquee > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 12 'Marquee: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMaximumRedColorLevel)
                            WriteSeparator("Marquee > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 13 'Marquee: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMaximumGreenColorLevel)
                            WriteSeparator("Marquee > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 14 'Marquee: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMaximumBlueColorLevel)
                            WriteSeparator("Marquee > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 15 'Marquee: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(MarqueeMaximumColorLevel)
                            WriteSeparator("Marquee > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("Marquee > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "16" 'FaderBack
                    Select Case KeyNumber
                        Case 1 'FaderBack: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackDelay)
                            WriteSeparator("FaderBack > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'FaderBack: Fade Out Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackFadeOutDelay)
                            WriteSeparator("FaderBack > " + DoTranslation("Fade Out Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before fading out text?"), True, ColTypes.Neutral)
                        Case 3 'FaderBack: Max Fade Steps
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackMaxSteps)
                            WriteSeparator("FaderBack > " + DoTranslation("Max Fade Steps"), True)
                            W(vbNewLine + DoTranslation("How many fade steps to do?"), True, ColTypes.Neutral)
                        Case 4 'FaderBack: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackMinimumRedColorLevel)
                            WriteSeparator("FaderBack > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 5 'FaderBack: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackMinimumGreenColorLevel)
                            WriteSeparator("FaderBack > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 6 'FaderBack: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackMinimumBlueColorLevel)
                            WriteSeparator("FaderBack > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'FaderBack: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackMaximumRedColorLevel)
                            WriteSeparator("FaderBack > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'FaderBack: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackMaximumGreenColorLevel)
                            WriteSeparator("FaderBack > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'FaderBack: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FaderBackMaximumBlueColorLevel)
                            WriteSeparator("FaderBack > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("FaderBack > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "17" 'BeatFader
                    Select Case KeyNumber
                        Case 1 'BeatFader: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BeatFader255Colors)
                            WriteSeparator("BeatFader > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for BeatFader."), True, ColTypes.Neutral)
                        Case 2 'BeatFader: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BeatFaderTrueColor)
                            WriteSeparator("BeatFader > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for BeatFader."), True, ColTypes.Neutral)
                        Case 3 'BeatFader: Delay in Beats Per Minute
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderDelay)
                            WriteSeparator("BeatFader > " + DoTranslation("Delay in Beats Per Minute"), True)
                            W(vbNewLine + DoTranslation("How many beats per minute to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'BeatFader: Cycle colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(BeatFaderCycleColors)
                            WriteSeparator("BeatFader > " + DoTranslation("Cycle colors"), True)
                            W(vbNewLine + DoTranslation("BeatFader will select random colors if it's enabled. Otherwise, use colors from config."), True, ColTypes.Neutral)
                        Case 5 'BeatFader: Beat Color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(BeatFaderBeatColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(BeatFaderBeatColor).Type = ColorType.TrueColor, If(New Color(BeatFaderBeatColor).Type = ColorType._255Color, New Color(BeatFaderBeatColor).PlainSequence, ConsoleColors.NavyBlue), New Color(BeatFaderBeatColor).R, New Color(BeatFaderBeatColor).G, New Color(BeatFaderBeatColor).B)
                        Case 6 'BeatFader: Max Fade Steps
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMaxSteps)
                            WriteSeparator("BeatFader > " + DoTranslation("Max Fade Steps"), True)
                            W(vbNewLine + DoTranslation("How many fade steps to do?"), True, ColTypes.Neutral)
                        Case 7 'BeatFader: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMinimumRedColorLevel)
                            WriteSeparator("BeatFader > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'BeatFader: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMinimumGreenColorLevel)
                            WriteSeparator("BeatFader > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 9 'BeatFader: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMinimumBlueColorLevel)
                            WriteSeparator("BeatFader > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 10 'BeatFader: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMinimumColorLevel)
                            WriteSeparator("BeatFader > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 11 'BeatFader: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMaximumRedColorLevel)
                            WriteSeparator("BeatFader > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'BeatFader: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMaximumGreenColorLevel)
                            WriteSeparator("BeatFader > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'BeatFader: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMaximumBlueColorLevel)
                            WriteSeparator("BeatFader > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 14 'BeatFader: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(BeatFaderMaximumColorLevel)
                            WriteSeparator("BeatFader > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("BeatFader > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "18" 'Linotypo
                    Select Case KeyNumber
                        Case 1 'Linotypo: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoDelay)
                            WriteSeparator("Linotypo > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'Linotypo: New Screen Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoNewScreenDelay)
                            WriteSeparator("Linotypo > " + DoTranslation("New Screen Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before writing the text in the new screen again?"), True, ColTypes.Neutral)
                        Case 3 'Linotypo: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(LinotypoWrite)
                            WriteSeparator("Linotypo > " + DoTranslation("Text shown"), True)
                            W(vbNewLine + DoTranslation("Write any text you want shown. Longer is better."), True, ColTypes.Neutral)
                            W(DoTranslation("This screensaver supports written text on file. Pass the complete file path to this field, and the screensaver will display the contents of the file appropriately."), True, ColTypes.Neutral)
                        Case 4 'Linotypo: Minimum writing speed in WPM
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoWritingSpeedMin)
                            WriteSeparator("Linotypo > " + DoTranslation("Minimum writing speed in WPM"), True)
                            W(vbNewLine + DoTranslation("Minimum writing speed in WPM"), True, ColTypes.Neutral)
                        Case 5 'Linotypo: Maximum writing speed in WPM
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoWritingSpeedMax)
                            WriteSeparator("Linotypo > " + DoTranslation("Maximum writing speed in WPM"), True)
                            W(vbNewLine + DoTranslation("Maximum writing speed in WPM"), True, ColTypes.Neutral)
                        Case 6 'Linotypo: Probability of typo in percent
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoMissStrikePossibility)
                            WriteSeparator("Linotypo > " + DoTranslation("Probability of typo in percent"), True)
                            W(vbNewLine + DoTranslation("Probability of typo in percent"), True, ColTypes.Neutral)
                        Case 7 'Linotypo: Column Count
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoTextColumns)
                            WriteSeparator("Linotypo > " + DoTranslation("Column Count"), True)
                            W(vbNewLine + DoTranslation("The text columns to be printed."), True, ColTypes.Neutral)
                        Case 8 'Linotypo: Line Fill Threshold
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoEtaoinThreshold)
                            WriteSeparator("Linotypo > " + DoTranslation("Line Fill Threshold"), True)
                            W(vbNewLine + DoTranslation("How many characters to write before triggering the ""line fill""?"), True, ColTypes.Neutral)
                        Case 9 'Linotypo: Line Fill Capping Probability in percent
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoEtaoinCappingPossibility)
                            WriteSeparator("Linotypo > " + DoTranslation("Line Fill Capping Probability in percent"), True)
                            W(vbNewLine + DoTranslation("Possibility that the line fill pattern will be printed in all caps in percent"), True, ColTypes.Neutral)
                        Case 10 'Linotypo: Line Fill Type
                            MaxKeyOptions = 3
                            KeyType = SettingsKeyType.SSelection
                            KeyVar = NameOf(LinotypoEtaoinType)
                            SelectionEnumZeroBased = True
                            WriteSeparator("Linotypo > " + DoTranslation("Line Fill Type"), True)
                            W(vbNewLine + DoTranslation("Line fill pattern type"), True, ColTypes.Neutral)
                            W(" 1) " + DoTranslation("Common Pattern"), True, ColTypes.Option)
                            W(" 2) " + DoTranslation("Complete Pattern"), True, ColTypes.Option)
                            W(" 3) " + DoTranslation("Random Pattern"), True, ColTypes.Option)
                        Case 11 'Linotypo: Probability of miss in percent
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(LinotypoMissPossibility)
                            WriteSeparator("Linotypo > " + DoTranslation("Probability of miss in percent"), True)
                            W(vbNewLine + DoTranslation("Probability of miss in percent"), True, ColTypes.Neutral)
                        Case 12 'Linotypo: Text color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(LinotypoTextColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(LinotypoTextColor).Type = ColorType.TrueColor, If(New Color(LinotypoTextColor).Type = ColorType._255Color, New Color(LinotypoTextColor).PlainSequence, ConsoleColors.White), New Color(LinotypoTextColor).R, New Color(LinotypoTextColor).G, New Color(LinotypoTextColor).B)
                        Case Else
                            WriteSeparator("Linotypo > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "19" 'Typewriter
                    Select Case KeyNumber
                        Case 1 'Typewriter: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypewriterDelay)
                            WriteSeparator("Typewriter > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'Typewriter: New Screen Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypewriterNewScreenDelay)
                            WriteSeparator("Typewriter > " + DoTranslation("New Screen Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before writing the text in the new screen again?"), True, ColTypes.Neutral)
                        Case 3 'Typewriter: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(TypewriterWrite)
                            WriteSeparator("Typewriter > " + DoTranslation("Text shown"), True)
                            W(vbNewLine + DoTranslation("Write any text you want shown. Longer is better."), True, ColTypes.Neutral)
                            W(DoTranslation("This screensaver supports written text on file. Pass the complete file path to this field, and the screensaver will display the contents of the file appropriately."), True, ColTypes.Neutral)
                        Case 4 'Typewriter: Minimum writing speed in WPM
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypewriterWritingSpeedMin)
                            WriteSeparator("Typewriter > " + DoTranslation("Minimum writing speed in WPM"), True)
                            W(vbNewLine + DoTranslation("Minimum writing speed in WPM"), True, ColTypes.Neutral)
                        Case 5 'Typewriter: Maximum writing speed in WPM
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(TypewriterWritingSpeedMax)
                            WriteSeparator("Typewriter > " + DoTranslation("Maximum writing speed in WPM"), True)
                            W(vbNewLine + DoTranslation("Maximum writing speed in WPM"), True, ColTypes.Neutral)
                        Case 6 'Typewriter: Text color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(TypewriterTextColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(TypewriterTextColor).Type = ColorType.TrueColor, If(New Color(TypewriterTextColor).Type = ColorType._255Color, New Color(TypewriterTextColor).PlainSequence, ConsoleColors.White), New Color(TypewriterTextColor).R, New Color(TypewriterTextColor).G, New Color(TypewriterTextColor).B)
                        Case Else
                            WriteSeparator("Typewriter > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "20" 'FlashColor
                    Select Case KeyNumber
                        Case 1 'FlashColor: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FlashColor255Colors)
                            WriteSeparator("FlashColor > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for FlashColor."), True, ColTypes.Neutral)
                        Case 2 'FlashColor: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(FlashColorTrueColor)
                            WriteSeparator("FlashColor > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for FlashColor."), True, ColTypes.Neutral)
                        Case 3 'FlashColor: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FlashColorDelay)
                            WriteSeparator("FlashColor > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'FlashColor: Background color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(FlashColorBackgroundColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(FlashColorBackgroundColor).Type = ColorType.TrueColor, If(New Color(FlashColorBackgroundColor).Type = ColorType._255Color, New Color(FlashColorBackgroundColor).PlainSequence, ConsoleColors.White), New Color(FlashColorBackgroundColor).R, New Color(FlashColorBackgroundColor).G, New Color(FlashColorBackgroundColor).B)
                        Case 5 'FlashColor: Minimum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FlashColorMinimumRedColorLevel)
                            WriteSeparator("FlashColor > " + DoTranslation("Minimum red color level"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 6 'FlashColor: Minimum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FlashColorMinimumGreenColorLevel)
                            WriteSeparator("FlashColor > " + DoTranslation("Minimum green color level"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 7 'FlashColor: Minimum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FlashColorMinimumBlueColorLevel)
                            WriteSeparator("FlashColor > " + DoTranslation("Minimum blue color level"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 8 'FlashColor: Minimum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FlashColorMinimumColorLevel)
                            WriteSeparator("FlashColor > " + DoTranslation("Minimum color level"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 9 'FlashColor: Maximum red color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FlashColorMaximumRedColorLevel)
                            WriteSeparator("FlashColor > " + DoTranslation("Maximum red color level"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 10 'FlashColor: Maximum green color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FlashColorMaximumGreenColorLevel)
                            WriteSeparator("FlashColor > " + DoTranslation("Maximum green color level"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 11 'FlashColor: Maximum blue color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FlashColorMaximumBlueColorLevel)
                            WriteSeparator("FlashColor > " + DoTranslation("Maximum blue color level"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 12 'FlashColor: Maximum color level
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(FlashColorMaximumColorLevel)
                            WriteSeparator("FlashColor > " + DoTranslation("Maximum color level"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("FlashColor > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "21" 'SpotWrite
                    Select Case KeyNumber
                        Case 1 'SpotWrite: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(SpotWriteDelay)
                            WriteSeparator("SpotWrite > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 2 'SpotWrite: New Screen Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(SpotWriteNewScreenDelay)
                            WriteSeparator("SpotWrite > " + DoTranslation("New Screen Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before writing the text in the new screen again?"), True, ColTypes.Neutral)
                        Case 3 'SpotWrite: Text shown
                            KeyType = SettingsKeyType.SLongString
                            KeyVar = NameOf(SpotWriteWrite)
                            WriteSeparator("SpotWrite > " + DoTranslation("Text shown"), True)
                            W(vbNewLine + DoTranslation("Write any text you want shown. Longer is better."), True, ColTypes.Neutral)
                            W(DoTranslation("This screensaver supports written text on file. Pass the complete file path to this field, and the screensaver will display the contents of the file appropriately."), True, ColTypes.Neutral)
                        Case 4 'SpotWrite: Text color
                            KeyType = SettingsKeyType.SVariant
                            KeyVar = NameOf(SpotWriteTextColor)
                            VariantValueFromExternalPrompt = True
                            VariantValue = ColorWheel(New Color(SpotWriteTextColor).Type = ColorType.TrueColor, If(New Color(SpotWriteTextColor).Type = ColorType._255Color, New Color(SpotWriteTextColor).PlainSequence, ConsoleColors.White), New Color(SpotWriteTextColor).R, New Color(SpotWriteTextColor).G, New Color(SpotWriteTextColor).B)
                        Case Else
                            WriteSeparator("SpotWrite > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case "22" 'Ramp
                    Select Case KeyNumber
                        Case 1 'Ramp: Activate 255 colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(Ramp255Colors)
                            WriteSeparator("Ramp > " + DoTranslation("Activate 255 colors"), True)
                            W(vbNewLine + DoTranslation("Activates 255 color support for Ramp."), True, ColTypes.Neutral)
                        Case 2 'Ramp: Activate true colors
                            KeyType = SettingsKeyType.SBoolean
                            KeyVar = NameOf(RampTrueColor)
                            WriteSeparator("Ramp > " + DoTranslation("Activate true colors"), True)
                            W(vbNewLine + DoTranslation("Activates true color support for Ramp."), True, ColTypes.Neutral)
                        Case 3 'Ramp: Delay in Milliseconds
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampDelay)
                            WriteSeparator("Ramp > " + DoTranslation("Delay in Milliseconds"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before making the next write?"), True, ColTypes.Neutral)
                        Case 4 'Ramp: Next ramp interval
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampNextRampDelay)
                            WriteSeparator("Ramp > " + DoTranslation("Next ramp interval"), True)
                            W(vbNewLine + DoTranslation("How many milliseconds to wait before filling in the next ramp?"), True, ColTypes.Neutral)
                        Case 5 'Ramp: Upper left corner character for ramp bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(RampUpperLeftCornerChar)
                            WriteSeparator("Ramp > " + DoTranslation("Upper left corner character for ramp bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 6 'Ramp: Lower left corner character for ramp bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(RampLowerLeftCornerChar)
                            WriteSeparator("Ramp > " + DoTranslation("Lower left corner character for ramp bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 7 'Ramp: Upper right corner character for ramp bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(RampUpperRightCornerChar)
                            WriteSeparator("Ramp > " + DoTranslation("Upper right corner character for ramp bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 8 'Ramp: Lower right corner character for ramp bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(RampLowerRightCornerChar)
                            WriteSeparator("Ramp > " + DoTranslation("Lower right corner character for ramp bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower left corner. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 9 'Ramp: Upper frame character for ramp bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(RampUpperFrameChar)
                            WriteSeparator("Ramp > " + DoTranslation("Upper frame character for ramp bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the upper frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 10 'Ramp: Lower frame character for ramp bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(RampLowerFrameChar)
                            WriteSeparator("Ramp > " + DoTranslation("Lower frame character for ramp bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the lower frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 11 'Ramp: Left frame character for ramp bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(RampLeftFrameChar)
                            WriteSeparator("Ramp > " + DoTranslation("Left frame character for ramp bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the left frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 12 'Ramp: Right frame character for ramp bar
                            KeyType = SettingsKeyType.SString
                            KeyVar = NameOf(RampRightFrameChar)
                            WriteSeparator("Ramp > " + DoTranslation("Right frame character for ramp bar"), True)
                            W(vbNewLine + DoTranslation("A character that resembles the right frame. Be sure to only input one character."), True, ColTypes.Neutral)
                        Case 13 'Ramp: Minimum red color level for start color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMinimumRedColorLevelStart)
                            WriteSeparator("Ramp > " + DoTranslation("Minimum red color level for start color"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 14 'Ramp: Minimum green color level for start color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMinimumGreenColorLevelStart)
                            WriteSeparator("Ramp > " + DoTranslation("Minimum green color level for start color"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 15 'Ramp: Minimum blue color level for start color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMinimumBlueColorLevelStart)
                            WriteSeparator("Ramp > " + DoTranslation("Minimum blue color level for start color"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 16 'Ramp: Minimum color level for start color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMinimumColorLevelStart)
                            WriteSeparator("Ramp > " + DoTranslation("Minimum color level for start color"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 17 'Ramp: Maximum red color level for start color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMaximumRedColorLevelStart)
                            WriteSeparator("Ramp > " + DoTranslation("Maximum red color level for start color"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 18 'Ramp: Maximum green color level for start color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMaximumGreenColorLevelStart)
                            WriteSeparator("Ramp > " + DoTranslation("Maximum green color level for start color"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 19 'Ramp: Maximum blue color level for start color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMaximumBlueColorLevelStart)
                            WriteSeparator("Ramp > " + DoTranslation("Maximum blue color level for start color"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 20 'Ramp: Maximum color level for start color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMaximumColorLevelStart)
                            WriteSeparator("Ramp > " + DoTranslation("Maximum color level for start color"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 21 'Ramp: Minimum red color level for end color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMinimumRedColorLevelEnd)
                            WriteSeparator("Ramp > " + DoTranslation("Minimum red color level for end color"), True)
                            W(vbNewLine + DoTranslation("Minimum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 22 'Ramp: Minimum green color level for end color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMinimumGreenColorLevelEnd)
                            WriteSeparator("Ramp > " + DoTranslation("Minimum green color level for end color"), True)
                            W(vbNewLine + DoTranslation("Minimum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 23 'Ramp: Minimum blue color level for end color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMinimumBlueColorLevelEnd)
                            WriteSeparator("Ramp > " + DoTranslation("Minimum blue color level for end color"), True)
                            W(vbNewLine + DoTranslation("Minimum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 24 'Ramp: Minimum color level for end color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMinimumColorLevelEnd)
                            WriteSeparator("Ramp > " + DoTranslation("Minimum color level for end color"), True)
                            W(vbNewLine + DoTranslation("Minimum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case 25 'Ramp: Maximum red color level for end color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMaximumRedColorLevelEnd)
                            WriteSeparator("Ramp > " + DoTranslation("Maximum red color level for end color"), True)
                            W(vbNewLine + DoTranslation("Maximum red color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 26 'Ramp: Maximum green color level for end color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMaximumGreenColorLevelEnd)
                            WriteSeparator("Ramp > " + DoTranslation("Maximum green color level for end color"), True)
                            W(vbNewLine + DoTranslation("Maximum green color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 27 'Ramp: Maximum blue color level for end color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMaximumBlueColorLevelEnd)
                            WriteSeparator("Ramp > " + DoTranslation("Maximum blue color level for end color"), True)
                            W(vbNewLine + DoTranslation("Maximum blue color level. The minimum accepted value is 0 and the maximum accepted value is 255."), True, ColTypes.Neutral)
                        Case 28 'Ramp: Maximum color level for end color
                            KeyType = SettingsKeyType.SInt
                            KeyVar = NameOf(RampMaximumColorLevelEnd)
                            WriteSeparator("Ramp > " + DoTranslation("Maximum color level for end color"), True)
                            W(vbNewLine + DoTranslation("Maximum color level. The minimum accepted value is 0 and the maximum accepted value is 255 for 255 colors or 16 for 16 colors."), True, ColTypes.Neutral)
                        Case Else
                            WriteSeparator("Ramp > ???", True)
                            W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End Select
                Case $"{If(SectionParts.Length > 1, SectionParts(1), $"{BuiltinSavers + 1}")}" 'Custom saver
                    Dim SaverIndex As Integer = SectionParts(1) - BuiltinSavers - 1
                    Dim SaverSettings As Dictionary(Of String, Object) = CustomSavers.Values(SaverIndex).Screensaver.SaverSettings
                    Dim KeyIndex As Integer = KeyNumber - 1
                    If KeyIndex <= SaverSettings.Count - 1 Then
                        KeyType = SettingsKeyType.SVariant
                        KeyVar = CustomSavers.Values(SaverIndex).Screensaver.SaverSettings.Keys(KeyIndex)
                        WriteSeparator("{0} > {1}" + vbNewLine, True, CustomSavers.Keys(SaverIndex), SaverSettings.Keys(KeyIndex))
                        W(vbNewLine + DoTranslation("Consult the screensaver manual or source code for information."), True, ColTypes.Neutral)
                    Else
                        WriteSeparator("{0} > ???" + vbNewLine, True, CustomSavers.Keys(SaverIndex))
                        W(vbNewLine + "X) " + DoTranslation("Invalid key number entered. Please go back."), True, ColTypes.Error)
                    End If
                Case Else
                    WriteSeparator("*) ???", True)
                    W(vbNewLine + "X) " + DoTranslation("Invalid section entered. Please go back."), True, ColTypes.Error)
            End Select

#Disable Warning BC42104
            'If the type is boolean, write the two options
            If KeyType = SettingsKeyType.SBoolean Then
                Console.WriteLine()
                MaxKeyOptions = 2
                W(" 1) " + DoTranslation("Enable"), True, ColTypes.Option)
                W(" 2) " + DoTranslation("Disable"), True, ColTypes.Option)
            End If
            Console.WriteLine()

            'Add an option to go back.
            If Not KeyType = SettingsKeyType.SVariant And Not KeyType = SettingsKeyType.SInt And Not KeyType = SettingsKeyType.SLongString And Not KeyType = SettingsKeyType.SString And Not KeyType = SettingsKeyType.SList Then
                W(" {0}) " + DoTranslation("Go Back...") + vbNewLine, True, ColTypes.BackOption, MaxKeyOptions + 1)
            ElseIf KeyType = SettingsKeyType.SList Then
                W(DoTranslation("Current items:"), True, ColTypes.ListTitle)
                WriteList(TargetList)
                W(vbNewLine + " q) " + DoTranslation("Save Changes...") + vbNewLine, True, ColTypes.Option, MaxKeyOptions + 1)
            End If

            'Get key value
            If Not KeyType = SettingsKeyType.SUnknown Then KeyValue = GetConfigValueField(KeyVar)

            'Print debugging info
            Wdbg(DebugLevel.W, "Key {0} in section {1} has {2} selections.", KeyNumber, Section, MaxKeyOptions)
            Wdbg(DebugLevel.W, "Target variable: {0}, Key Type: {1}, Key value: {2}, Variant Value: {3}", KeyVar, KeyType, KeyValue, VariantValue)

            'Prompt user
            If KeyType = SettingsKeyType.SVariant And Not VariantValueFromExternalPrompt Then
                W("> ", False, ColTypes.Input)
                VariantValue = Console.ReadLine
                If NeutralizePaths Then AnswerString = NeutralizePath(AnswerString, NeutralizeRootPath)
                Wdbg(DebugLevel.I, "User answered {0}", VariantValue)
            ElseIf Not KeyType = SettingsKeyType.SVariant Then
                If KeyType = SettingsKeyType.SList Then
                    W("> ", False, ColTypes.Input)
                    Do Until AnswerString = "q"
                        AnswerString = Console.ReadLine
                        If Not AnswerString = "q" Then
                            If NeutralizePaths Then AnswerString = NeutralizePath(AnswerString, NeutralizeRootPath)
                            If Not AnswerString.StartsWith("-") Then
                                'We're not removing an item!
                                TargetList = Enumerable.Append(TargetList, AnswerString)
                            Else
                                'We're removing an item.
                                Dim DeletedItems As IEnumerable(Of Object) = Enumerable.Empty(Of Object)
                                DeletedItems = Enumerable.Append(DeletedItems, AnswerString.Substring(1))
                                TargetList = Enumerable.Except(TargetList, DeletedItems)
                            End If
                            Wdbg(DebugLevel.I, "Added answer {0} to list.", AnswerString)
                            W("> ", False, ColTypes.Input)
                        End If
                    Loop
                Else
                    W(If(KeyType = SettingsKeyType.SUnknown, "> ", "[{0}] > "), False, ColTypes.Input, KeyValue)
                    If KeyType = SettingsKeyType.SLongString Then
                        AnswerString = ReadLineLong()
                    Else
                        AnswerString = Console.ReadLine
                    End If
                    If NeutralizePaths Then AnswerString = NeutralizePath(AnswerString, NeutralizeRootPath)
                    Wdbg(DebugLevel.I, "User answered {0}", AnswerString)
                End If
            End If

            'Check for input
            Wdbg(DebugLevel.I, "Is the answer numeric? {0}", IsNumeric(AnswerString))
            If Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SBoolean Then
                Wdbg(DebugLevel.I, "Answer is numeric and key is of the Boolean type.")
                If AnswerInt >= 1 And AnswerInt <= MaxKeyOptions Then
                    Wdbg(DebugLevel.I, "Translating {0} to the boolean equivalent...", AnswerInt)
                    KeyFinished = True
                    Select Case AnswerInt
                        Case 1 'True
                            Wdbg(DebugLevel.I, "Setting to True...")
                            SetConfigValueField(KeyVar, True)
                        Case 2 'False
                            Wdbg(DebugLevel.I, "Setting to False...")
                            SetConfigValueField(KeyVar, False)
                    End Select
                ElseIf AnswerInt = MaxKeyOptions + 1 Then 'Go Back...
                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                    KeyFinished = True
                Else
                    Wdbg(DebugLevel.W, "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Error, AnswerInt)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            ElseIf (Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SInt) Or
                   (Integer.TryParse(AnswerString, AnswerInt) And KeyType = SettingsKeyType.SSelection) Then
                Wdbg(DebugLevel.I, "Answer is numeric and key is of the {0} type.", KeyType)
                If AnswerInt = MaxKeyOptions + 1 And KeyType = SettingsKeyType.SSelection Then 'Go Back...
                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                    KeyFinished = True
                ElseIf KeyType = SettingsKeyType.SSelection And AnswerInt > 0 And SelectFrom IsNot Nothing Then
                    Wdbg(DebugLevel.I, "Setting variable {0} to item index {1}...", KeyVar, AnswerInt - 1)
                    KeyFinished = True
                    SetConfigValueField(KeyVar, SelectFrom(AnswerInt - 1))
                ElseIf (KeyType = SettingsKeyType.SSelection And AnswerInt > 0) Or
                       (KeyType = SettingsKeyType.SInt And AnswerInt >= 0) Then
                    If (KeyType = SettingsKeyType.SSelection And Not AnswerInt > MaxKeyOptions) Or KeyType = SettingsKeyType.SInt Then
                        If SelectionEnumZeroBased Then AnswerInt -= 1
                        Wdbg(DebugLevel.I, "Setting variable {0} to {1}...", KeyVar, AnswerInt)
                        KeyFinished = True
                        SetConfigValueField(KeyVar, AnswerInt)
                    ElseIf KeyType = SettingsKeyType.SSelection Then
                        Wdbg(DebugLevel.W, "Answer is not valid.")
                        W(DoTranslation("The answer may not exceed the entries shown."), True, ColTypes.Error)
                        W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                        Console.ReadKey()
                    End If
                ElseIf AnswerInt = 0 Then
                    Wdbg(DebugLevel.W, "Zero is not allowed.")
                    W(DoTranslation("The answer may not be zero."), True, ColTypes.Error)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                Else
                    Wdbg(DebugLevel.W, "Negative values are disallowed.")
                    W(DoTranslation("The answer may not be negative."), True, ColTypes.Error)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                    Console.ReadKey()
                End If
            ElseIf KeyType = SettingsKeyType.SUnknown Then
                Wdbg(DebugLevel.I, "User requested exit. Returning...")
                KeyFinished = True
            ElseIf KeyType = SettingsKeyType.SString Or KeyType = SettingsKeyType.SLongString Then
                Wdbg(DebugLevel.I, "Answer is not numeric and key is of the String type. Setting variable...")

                'Check to see if written answer is empty
                If String.IsNullOrWhiteSpace(AnswerString) Then
                    Wdbg(DebugLevel.I, "Answer is nothing. Setting to {0}...", KeyValue)
                    AnswerString = KeyValue
                End If

                'Check to see if the user intended to clear the variable to make it consist of nothing
                If AnswerString.ToLower = "/clear" Then
                    Wdbg(DebugLevel.I, "User requested clear.")
                    AnswerString = ""
                End If

                'Set the value
                KeyFinished = True
                SetConfigValueField(KeyVar, AnswerString)
            ElseIf KeyType = SettingsKeyType.SList Then
                Wdbg(DebugLevel.I, "Answer is not numeric and key is of the List type. Adding answers to the list...")
                KeyFinished = True
                SetConfigValueField(KeyVar, String.Join(ListJoinString, TargetList))
            ElseIf SectionParts.Length > 1 Then
                If Section = SectionParts(1) And SectionParts(1) > BuiltinSavers And KeyType = SettingsKeyType.SVariant Then
                    Dim SaverIndex As Integer = SectionParts(1) - BuiltinSavers - 1
                    Dim SaverSettings As Dictionary(Of String, Object) = CustomSavers.Values(SaverIndex).Screensaver.SaverSettings
                    SaverSettings(KeyVar) = VariantValue
                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                    KeyFinished = True
                ElseIf KeyType = SettingsKeyType.SVariant Then
                    SetConfigValueField(KeyVar, VariantValue)
                    Wdbg(DebugLevel.I, "User requested exit. Returning...")
                    KeyFinished = True
                End If
            ElseIf KeyType = SettingsKeyType.SVariant Then
                SetConfigValueField(KeyVar, VariantValue)
                Wdbg(DebugLevel.I, "User requested exit. Returning...")
                KeyFinished = True
            Else
                Wdbg(DebugLevel.W, "Answer is not valid.")
                W(DoTranslation("The answer is invalid. Check to make sure that the answer is numeric for config entries that need numbers as answers."), True, ColTypes.Error)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Error)
                Console.ReadKey()
            End If
#Enable Warning BC42104
        End While
    End Sub

End Module
