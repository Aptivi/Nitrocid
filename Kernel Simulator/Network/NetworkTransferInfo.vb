
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

Imports System.IO

Namespace Network
    Public Class NetworkTransferInfo

        Private _MessageSuppressed As Boolean

        ''' <summary>
        ''' How many bytes downloaded/uploaded
        ''' </summary>
        Public ReadOnly Property DoneSize As Long
        ''' <summary>
        ''' File size
        ''' </summary>
        Public ReadOnly Property FileSize As Long
        ''' <summary>
        ''' The transfer type
        ''' </summary>
        Public ReadOnly Property TransferType As NetworkTransferType
        ''' <summary>
        ''' Whether the message is suppressed. Once set, it can't be unset.
        ''' </summary>
        ''' <returns></returns>
        Public Property MessageSuppressed As Boolean
            Get
                Return _MessageSuppressed
            End Get
            Set
                If Not _MessageSuppressed Then _MessageSuppressed = Value
            End Set
        End Property

        Protected Friend Sub New(DoneSize As Long, FileSize As Long, TransferType As NetworkTransferType)
            Me.DoneSize = DoneSize
            Me.FileSize = FileSize
            Me.TransferType = TransferType
        End Sub

    End Class
End Namespace
