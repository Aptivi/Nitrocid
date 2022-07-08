
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

Namespace Misc.Screensaver
    ''' <summary>
    ''' Custom screensaver interface with groups of subs and properties
    ''' </summary>
    <Obsolete("This custom screensaver interface is obsolete. Use IScreensaver and BaseScreensaver instead.")>
    Public Interface ICustomSaver
        ''' <summary>
        ''' Initializes screensaver
        ''' </summary>
        Sub InitSaver()
        ''' <summary>
        ''' Do anything before displaying screensaver
        ''' </summary>
        Sub PreDisplay()
        ''' <summary>
        ''' Do anything after displaying screensaver
        ''' </summary>
        Sub PostDisplay()
        ''' <summary>
        ''' Display a screensaver
        ''' </summary>
        Sub ScrnSaver()
        ''' <summary>
        ''' Indicate whether or not the screensaver is initialized
        ''' </summary>
        ''' <returns>true if initialized, false if uninitialized</returns>
        Property Initialized As Boolean
        ''' <summary>
        ''' How many milliseconds to delay for each call to ScrnSaver
        ''' </summary>
        ''' <returns>A millisecond value</returns>
        Property DelayForEachWrite As Integer
        ''' <summary>
        ''' The name of screensaver
        ''' </summary>
        ''' <returns>The name</returns>
        Property SaverName As String
        ''' <summary>
        ''' Settings for custom screensaver
        ''' </summary>
        ''' <returns>A set of keys and values holding settings for the screensaver</returns>
        Property SaverSettings As Dictionary(Of String, Object)
    End Interface
End Namespace