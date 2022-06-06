
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

Namespace Misc.Splash
    ''' <summary>
    ''' Splash screen interface
    ''' </summary>
    Public Interface ISplash

        ''' <summary>
        ''' Whether the splash is closing. If true, the thread of which handles the display should close itself. <see cref="Closing()"/> should set this property to True.
        ''' </summary>
        Property SplashClosing As Boolean
        ''' <summary>
        ''' Progress write left console position
        ''' </summary>
        ReadOnly Property ProgressWritePositionX As Integer
        ''' <summary>
        ''' Progress write top console position
        ''' </summary>
        ReadOnly Property ProgressWritePositionY As Integer
        ''' <summary>
        ''' Progress report write left console position
        ''' </summary>
        ReadOnly Property ProgressReportWritePositionX As Integer
        ''' <summary>
        ''' Progress report write top console position
        ''' </summary>
        ReadOnly Property ProgressReportWritePositionY As Integer
        ''' <summary>
        ''' Splash name
        ''' </summary>
        ReadOnly Property SplashName As String
        ''' <summary>
        ''' Splash displays progress
        ''' </summary>
        ReadOnly Property SplashDisplaysProgress As Boolean
        ''' <summary>
        ''' The opening screen. Should be synchronous.
        ''' </summary>
        Sub Opening()
        ''' <summary>
        ''' The screen which is meant to be looped. You can set it to do nothing. Should be async. It should also handle <see cref="System.Threading.ThreadInterruptedException"/> to avoid kernel exiting on startup.
        ''' </summary>
        Sub Display()
        ''' <summary>
        ''' The closing screen. Should be synchronous.
        ''' </summary>
        Sub Closing()
        ''' <summary>
        ''' Report the progress
        ''' </summary>
        ''' <param name="ProgressReport">The progress text to indicate how did the kernel progress</param>
        ''' <param name="Progress">The progress indicator of the kernel</param>
        ''' <param name="ProgressWritePositionX"></param>
        ''' <param name="ProgressWritePositionY"></param>
        ''' <param name="ProgressReportWritePositionX"></param>
        ''' <param name="ProgressReportWritePositionY"></param>
        Sub Report(Progress As Integer, ProgressReport As String, ProgressWritePositionX As Integer, ProgressWritePositionY As Integer, ProgressReportWritePositionX As Integer, ProgressReportWritePositionY As Integer, ParamArray Vars() As Object)

    End Interface
End Namespace