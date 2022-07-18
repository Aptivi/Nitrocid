
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

Namespace Misc.Screensaver.Displays
    Public Module TypoSettings

        Private _typoDelay As Integer = 50
        Private _typoWriteAgainDelay As Integer = 3000
        Private _typoWrite As String = "Kernel Simulator"
        Private _typoWritingSpeedMin As Integer = 50
        Private _typoWritingSpeedMax As Integer = 80
        Private _typoMissStrikePossibility As Integer = 20
        Private _typoMissPossibility As Integer = 10
        Private _typoTextColor As String = New Color(ConsoleColor.White).PlainSequence

        ''' <summary>
        ''' [Typo] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property TypoDelay As Integer
            Get
                Return _typoDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 50
                _typoDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Typo] How many milliseconds to wait before writing the text again?
        ''' </summary>
        Public Property TypoWriteAgainDelay As Integer
            Get
                Return _typoWriteAgainDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 3000
                _typoWriteAgainDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Typo] Text for Typo. Longer is better.
        ''' </summary>
        Public Property TypoWrite As String
            Get
                Return _typoWrite
            End Get
            Set(value As String)
                If String.IsNullOrEmpty(value) Then value = "Kernel Simulator"
                _typoWrite = value
            End Set
        End Property
        ''' <summary>
        ''' [Typo] Minimum writing speed in WPM
        ''' </summary>
        Public Property TypoWritingSpeedMin As Integer
            Get
                Return _typoWritingSpeedMin
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 50
                _typoWritingSpeedMin = value
            End Set
        End Property
        ''' <summary>
        ''' [Typo] Maximum writing speed in WPM
        ''' </summary>
        Public Property TypoWritingSpeedMax As Integer
            Get
                Return _typoWritingSpeedMax
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 80
                _typoWritingSpeedMax = value
            End Set
        End Property
        ''' <summary>
        ''' [Typo] Possibility that the writer made a typo in percent
        ''' </summary>
        Public Property TypoMissStrikePossibility As Integer
            Get
                Return _typoMissStrikePossibility
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 20
                _typoMissStrikePossibility = value
            End Set
        End Property
        ''' <summary>
        ''' [Typo] Possibility that the writer missed a character in percent
        ''' </summary>
        Public Property TypoMissPossibility As Integer
            Get
                Return _typoMissPossibility
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 10
                _typoMissPossibility = value
            End Set
        End Property
        ''' <summary>
        ''' [Typo] Text color
        ''' </summary>
        Public Property TypoTextColor As String
            Get
                Return _typoTextColor
            End Get
            Set(value As String)
                _typoTextColor = New Color(value).PlainSequence
            End Set
        End Property

    End Module
    Public Class TypoDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random
        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean

        Public Overrides Property ScreensaverName As String = "Typo" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SetConsoleColor(New Color(TypoTextColor))
            Console.Clear()
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Dim CpmSpeedMin As Integer = TypoWritingSpeedMin * 5
            Dim CpmSpeedMax As Integer = TypoWritingSpeedMax * 5
            Dim Strikes As New List(Of String) From {"q`12wsa", "r43edfgt5", "u76yhjki8", "p09ol;'[-=]\", "/';. ", "m,lkjn ", "vbhgfc ", "zxdsa "}
            Dim CapStrikes As New List(Of String) From {"Q~!@WSA", "R$#EDFGT%", "U&^YHJKI*", "P)(OL:""{_+}|", "?"":> ", "M<LKJN ", "VBHGFC ", "ZXDSA "}
            Dim CapSymbols As String = "~!@$#%&^*)(:""{_+}|?><"

            Console.CursorVisible = False

            'Prepare display (make a paragraph indentation)
            Console.WriteLine()
            Console.Write("    ")
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", Console.CursorLeft, Console.CursorTop)

            'Get struck character and write it
            For Each StruckChar As Char In TypoWrite
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If ResizeSyncing Then Exit For

                'Calculate needed milliseconds from two WPM speeds (minimum and maximum)
                Dim SelectedCpm As Integer = RandomDriver.Next(CpmSpeedMin, CpmSpeedMax)
                Dim WriteMs As Integer = (60 / SelectedCpm) * 1000
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Delay for {0} CPM: {1} ms", SelectedCpm, WriteMs)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Struck character: {0}", StruckChar)

                'See if the typo is guaranteed
                Dim Probability As Double = If(TypoMissStrikePossibility >= 80, 80, TypoMissStrikePossibility) / 100
                Dim TypoGuaranteed As Boolean = RandomDriver.NextDouble < Probability
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Probability: {0} Guarantee: {1}", Probability, TypoGuaranteed)
                If TypoGuaranteed Then
                    'Sometimes, a typo is generated by missing a character.
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Made a typo!")
                    Dim MissProbability As Double = If(TypoMissPossibility >= 10, 10, TypoMissPossibility) / 100
                    Dim MissGuaranteed As Boolean = RandomDriver.NextDouble < MissProbability
                    If MissGuaranteed Then
                        'Miss is guaranteed. Simulate the missed character
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Missed a character!")
                        StruckChar = ""
                    Else
                        'Typo is guaranteed. Select a strike string randomly until the struck key is found in between the characters
                        Dim StrikeCharsIndex As Integer
                        Dim StruckFound As Boolean = False
                        Dim CappedStrike As Boolean = False
                        Dim StrikesString As String = ""
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Bruteforcing...")
                        Do Until StruckFound
                            StrikeCharsIndex = RandomDriver.Next(0, Strikes.Count - 1)
                            CappedStrike = Char.IsUpper(StruckChar) Or CapSymbols.Contains(StruckChar)
                            StrikesString = If(CappedStrike, CapStrikes(StrikeCharsIndex), Strikes(StrikeCharsIndex))
                            StruckFound = Not String.IsNullOrEmpty(StrikesString) AndAlso StrikesString.Contains(StruckChar)
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Strike chars index: {0}", StrikeCharsIndex)
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Capped strike: {0}", CappedStrike)
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Strikes pattern: {0}", StrikesString)
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Found? {0}", StruckFound)
                        Loop
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Found!")

                        'Select a random character that is a typo from the selected strike index
                        Dim RandomStrikeIndex As Integer = RandomDriver.Next(0, StrikesString.Length - 1)
                        Dim MistypedChar As Char = StrikesString(RandomStrikeIndex)
                        If "`-=\][';/.,".Contains(MistypedChar) And CappedStrike Then
                            'The mistyped character is a symbol and the strike is capped. Select a symbol from CapStrikes.
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Mistyped character is a symbol and the strike is capped.")
                            MistypedChar = CapStrikes(StrikeCharsIndex)(RandomStrikeIndex)
                        End If
                        StruckChar = MistypedChar
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Struck character: {0}", StruckChar)
                    End If
                End If

                'Write the final character to the console and wait
                If Not StruckChar = vbNullChar Then Console.Write(StruckChar)
                SleepNoBlock(WriteMs, ScreensaverDisplayerThread)
            Next

            'Wait until retry
            Console.WriteLine()
            If Not ResizeSyncing Then SleepNoBlock(TypoWriteAgainDelay, ScreensaverDisplayerThread)

            'Reset resize sync
            ResizeSyncing = False
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            SleepNoBlock(TypoDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace
