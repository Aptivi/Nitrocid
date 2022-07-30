
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

Namespace Misc.Splash
    Public Module SplashSettings

        '-> Simple
        ''' <summary>
        ''' [Simple] The progress text location
        ''' </summary>
        Public SimpleProgressTextLocation As TextLocation = TextLocation.Top

        '-> Progress
        ''' <summary>
        ''' [Progress] The progress color
        ''' </summary>
        Public ProgressProgressColor As String = ColorTools.ProgressColor.PlainSequence
        ''' <summary>
        ''' [Progress] The progress text location
        ''' </summary>
        Public ProgressProgressTextLocation As TextLocation = TextLocation.Top

    End Module
End Namespace
