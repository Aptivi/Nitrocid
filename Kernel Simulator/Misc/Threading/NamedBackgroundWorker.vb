
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

Imports System.ComponentModel
Imports System.Threading

Namespace Misc.Threading
    'NOTE: OPEN THIS FILE WITH "Visual Basic Editor"!
    Public Class NamedBackgroundWorker
        Inherits BackgroundWorker

        Private _ThreadName As String
        Private IsNameSet As Boolean

        ''' <summary>
        ''' The thread name to identify the background worker
        ''' </summary>
        Public Property ThreadName As String
            Get
                Return _ThreadName
            End Get
            Private Set(value As String)
                If Not IsNameSet Then
                    _ThreadName = value
                    IsNameSet = True
                End If
            End Set
        End Property

        ''' <summary>
        ''' Makes a new background worker with the specified name
        ''' </summary>
        ''' <param name="ThreadName">The thread name</param>
        Public Sub New(ThreadName As String)
            Me.ThreadName = ThreadName
        End Sub

        ''' <summary>
        ''' Identifies, then raises the <see cref="DoWork"/> event.
        ''' </summary>
        Protected Overrides Sub OnDoWork(e As DoWorkEventArgs)
            If Thread.CurrentThread.Name Is Nothing Then Thread.CurrentThread.Name = ThreadName
            Wdbg(DebugLevel.I, "Made a background worker thread [{0}] {1}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name)
            MyBase.OnDoWork(e)
        End Sub

    End Class
End Namespace