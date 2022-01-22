
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

Imports KS.Misc.Writers.FancyWriters.Tools

Namespace Misc.Writers.FancyWriters
    Public Module ProgressColor

        ''' <summary>
        ''' Writes the progress bar
        ''' </summary>
        ''' <param name="Progress">The progress percentage</param>
        ''' <param name="Left">The progress position from the upper left corner</param>
        ''' <param name="Top">The progress position from the top</param>
        Public Sub WriteProgress(Progress As Double, Left As Integer, Top As Integer)
            WriteWhere(ProgressUpperLeftCornerChar + ProgressUpperFrameChar.Repeat(Console.WindowWidth - 10) + ProgressUpperRightCornerChar, Left, Top, True, ColTypes.Gray)
            WriteWhere(ProgressLeftFrameChar + " ".Repeat(Console.WindowWidth - 10) + ProgressRightFrameChar, Left, Top + 1, True, ColTypes.Gray)
            WriteWhere(ProgressLowerLeftCornerChar + ProgressLowerFrameChar.Repeat(Console.WindowWidth - 10) + ProgressLowerRightCornerChar, Left, Top + 2, True, ColTypes.Gray)
            WriteWhere(" ".Repeat(PercentRepeat(Progress, 100, 10)), Left + 1, Top + 1, True, ColTypes.Neutral, ColTypes.Progress)
        End Sub

        ''' <summary>
        ''' Writes the progress bar
        ''' </summary>
        ''' <param name="Progress">The progress percentage</param>
        ''' <param name="Left">The progress position from the upper left corner</param>
        ''' <param name="Top">The progress position from the top</param>
        ''' <param name="ProgressColor">The progress bar color</param>
        Public Sub WriteProgress(Progress As Double, Left As Integer, Top As Integer, ProgressColor As ColTypes)
            WriteWhere(ProgressUpperLeftCornerChar + ProgressUpperFrameChar.Repeat(Console.WindowWidth - 10) + ProgressUpperRightCornerChar, Left, Top, True, ColTypes.Gray)
            WriteWhere(ProgressLeftFrameChar + " ".Repeat(Console.WindowWidth - 10) + ProgressRightFrameChar, Left, Top + 1, True, ColTypes.Gray)
            WriteWhere(ProgressLowerLeftCornerChar + ProgressLowerFrameChar.Repeat(Console.WindowWidth - 10) + ProgressLowerRightCornerChar, Left, Top + 2, True, ColTypes.Gray)
            WriteWhere(" ".Repeat(PercentRepeat(Progress, 100, 10)), Left + 1, Top + 1, True, ColTypes.Neutral, ProgressColor)
        End Sub

        ''' <summary>
        ''' Writes the progress bar
        ''' </summary>
        ''' <param name="Progress">The progress percentage</param>
        ''' <param name="Left">The progress position from the upper left corner</param>
        ''' <param name="Top">The progress position from the top</param>
        ''' <param name="ProgressColor">The progress bar color</param>
        ''' <param name="FrameColor">The progress bar frame color</param>
        Public Sub WriteProgress(Progress As Double, Left As Integer, Top As Integer, ProgressColor As ColTypes, FrameColor As ColTypes)
            WriteWhere(ProgressUpperLeftCornerChar + ProgressUpperFrameChar.Repeat(Console.WindowWidth - 10) + ProgressUpperRightCornerChar, Left, Top, True, FrameColor)
            WriteWhere(ProgressLeftFrameChar + " ".Repeat(Console.WindowWidth - 10) + ProgressRightFrameChar, Left, Top + 1, True, FrameColor)
            WriteWhere(ProgressLowerLeftCornerChar + ProgressLowerFrameChar.Repeat(Console.WindowWidth - 10) + ProgressLowerRightCornerChar, Left, Top + 2, True, FrameColor)
            WriteWhere(" ".Repeat(PercentRepeat(Progress, 100, 10)), Left + 1, Top + 1, True, ColTypes.Neutral, ProgressColor)
        End Sub

        ''' <summary>
        ''' Writes the progress bar
        ''' </summary>
        ''' <param name="Progress">The progress percentage</param>
        ''' <param name="Left">The progress position from the upper left corner</param>
        ''' <param name="Top">The progress position from the top</param>
        ''' <param name="ProgressColor">The progress bar color</param>
        Public Sub WriteProgress(Progress As Double, Left As Integer, Top As Integer, ProgressColor As Color)
            WriteWhere(ProgressUpperLeftCornerChar + ProgressUpperFrameChar.Repeat(Console.WindowWidth - 10) + ProgressUpperRightCornerChar, Left, Top, True, ColTypes.Gray)
            WriteWhere(ProgressLeftFrameChar + " ".Repeat(Console.WindowWidth - 10) + ProgressRightFrameChar, Left, Top + 1, True, ColTypes.Gray)
            WriteWhere(ProgressLowerLeftCornerChar + ProgressLowerFrameChar.Repeat(Console.WindowWidth - 10) + ProgressLowerRightCornerChar, Left, Top + 2, True, ColTypes.Gray)
            WriteWhere(" ".Repeat(PercentRepeat(Progress, 100, 10)), Left + 1, Top + 1, True, NeutralTextColor, ProgressColor)
        End Sub

        ''' <summary>
        ''' Writes the progress bar
        ''' </summary>
        ''' <param name="Progress">The progress percentage</param>
        ''' <param name="Left">The progress position from the upper left corner</param>
        ''' <param name="Top">The progress position from the top</param>
        ''' <param name="ProgressColor">The progress bar color</param>
        ''' <param name="FrameColor">The progress bar frame color</param>
        Public Sub WriteProgress(Progress As Double, Left As Integer, Top As Integer, ProgressColor As Color, FrameColor As Color)
            WriteWhere(ProgressUpperLeftCornerChar + ProgressUpperFrameChar.Repeat(Console.WindowWidth - 10) + ProgressUpperRightCornerChar, Left, Top, True, FrameColor)
            WriteWhere(ProgressLeftFrameChar + " ".Repeat(Console.WindowWidth - 10) + ProgressRightFrameChar, Left, Top + 1, True, FrameColor)
            WriteWhere(ProgressLowerLeftCornerChar + ProgressLowerFrameChar.Repeat(Console.WindowWidth - 10) + ProgressLowerRightCornerChar, Left, Top + 2, True, FrameColor)
            WriteWhere(" ".Repeat(PercentRepeat(Progress, 100, 10)), Left + 1, Top + 1, True, NeutralTextColor, ProgressColor)
        End Sub

    End Module
End Namespace
