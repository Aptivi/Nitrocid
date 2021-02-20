
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

Public Module ScreensaverSettings

    '-> Color Settings
    ''' <summary>
    ''' [ColorMix] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public ColorMix255Colors As Boolean
    ''' <summary>
    ''' [ColorMix] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public ColorMixTrueColor As Boolean
    ''' <summary>
    ''' [Disco] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public Disco255Colors As Boolean
    ''' <summary>
    ''' [Disco] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public DiscoTrueColor As Boolean
    ''' <summary>
    ''' [GlitterColor] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public GlitterColor255Colors As Boolean
    ''' <summary>
    ''' [GlitterColor] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public GlitterColorTrueColor As Boolean
    ''' <summary>
    ''' [Lines] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public Lines255Colors As Boolean
    ''' <summary>
    ''' [Lines] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public LinesTrueColor As Boolean
    ''' <summary>
    ''' [Dissolve] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public Dissolve255Colors As Boolean
    ''' <summary>
    ''' [Dissolve] Enable truecolor support. Has a higher priority than 255 color support.
    ''' </summary>
    Public DissolveTrueColor As Boolean
    ''' <summary>
    ''' [BouncingBlock] Enable 255 color support. Has a higher priority than 16 color support.
    ''' </summary>
    Public BouncingBlock255Colors As Boolean
    ''' <summary>
    ''' [BouncingBlock] Enable truecolor support. Has a higher priority than 16 color support.
    ''' </summary>
    Public BouncingBlockTrueColor As Boolean
    ''' <summary>
    ''' [Disco] Enable color cycling
    ''' </summary>
    Public DiscoCycleColors As Boolean

    '-> Delays
    ''' <summary>
    ''' [BouncingBlock] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public BouncingBlockDelay As Integer = 10
    ''' <summary>
    ''' [BouncingText] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public BouncingTextDelay As Integer = 10
    ''' <summary>
    ''' [ColorMix] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public ColorMixDelay As Integer = 1
    ''' <summary>
    ''' [Disco] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public DiscoDelay As Integer = 100
    ''' <summary>
    ''' [GlitterColor] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public GlitterColorDelay As Integer = 1
    ''' <summary>
    ''' [GlitterMatrix] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public GlitterMatrixDelay As Integer = 1
    ''' <summary>
    ''' [Lines] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public LinesDelay As Integer = 500
    ''' <summary>
    ''' [Matrix] How many milliseconds to wait before making the next write?
    ''' </summary>
    Public MatrixDelay As Integer = 1

    '-> Texts
    ''' <summary>
    ''' [BouncingText] Text for Bouncing Text
    ''' </summary>
    Public BouncingTextWrite As String = "Kernel Simulator"

End Module
