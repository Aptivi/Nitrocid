
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

Imports KS.TimeDate

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Shows time information
    ''' </summary>
    ''' <remarks>
    ''' This shows you the detailed time information, including the time analysis, binary representation, and even the Unix time.
    ''' </remarks>
    Class GetTimeInfoCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim DateTimeInfo As Date
            If Date.TryParse(ListArgsOnly(0), DateTimeInfo) Then
                Write("-- " + DoTranslation("Information for") + " {0} --" + NewLine, True, ColTypes.Neutral, Render(DateTimeInfo))
                Write(DoTranslation("Milliseconds:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Millisecond)
                Write(DoTranslation("Seconds:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Second)
                Write(DoTranslation("Minutes:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Minute)
                Write(DoTranslation("Hours:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Hour)
                Write(DoTranslation("Days:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Day)
                Write(DoTranslation("Months:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.Month)
                Write(DoTranslation("Year:") + " {0}" + NewLine, True, ColTypes.Neutral, DateTimeInfo.Year)
                Write(DoTranslation("Date:") + " {0}", True, ColTypes.Neutral, RenderDate(DateTimeInfo))
                Write(DoTranslation("Time:") + " {0}" + NewLine, True, ColTypes.Neutral, RenderTime(DateTimeInfo))
                Write(DoTranslation("Day of Year:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.DayOfYear)
                Write(DoTranslation("Day of Week:") + " {0}" + NewLine, True, ColTypes.Neutral, DateTimeInfo.DayOfWeek.ToString)
                Write(DoTranslation("Binary:") + " {0}", True, ColTypes.Neutral, DateTimeInfo.ToBinary)
                Write(DoTranslation("Local Time:") + " {0}", True, ColTypes.Neutral, Render(DateTimeInfo.ToLocalTime))
                Write(DoTranslation("Universal Time:") + " {0}", True, ColTypes.Neutral, Render(DateTimeInfo.ToUniversalTime))
                Write(DoTranslation("Unix Time:") + " {0}", True, ColTypes.Neutral, DateToUnix(DateTimeInfo))
            Else
                Write(DoTranslation("Failed to parse date information for") + " {0}. " + DoTranslation("Ensure that the format is correct."), True, ColTypes.Error, ListArgsOnly(0))
            End If
        End Sub

    End Class
End Namespace
