
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

Public Module MOTDParse

    'Variables
    Public MOTDFilePath, MALFilePath As String

    'TODO: Document this by Milestone 2
    ''' <summary>
    ''' Types of message
    ''' </summary>
    Public Enum MessageType As Integer
        MOTD = 1
        MAL = 2
    End Enum

    ''' <summary>
    ''' Sets the Message of the Day or MAL
    ''' </summary>
    ''' <param name="Message">A message of the day before/after login</param>
    ''' <param name="MType">Message type</param>
    Public Sub SetMOTD(Message As String, MType As MessageType)
        Try
            Dim MOTDStreamW As IO.StreamWriter

            'Get the MOTD and MAL file path
            MOTDFilePath = GetKernelPath(KernelPathType.MOTD)
            MALFilePath = GetKernelPath(KernelPathType.MAL)
            Wdbg(DebugLevel.I, "Paths: {0}, {1}", MOTDFilePath, MALFilePath)
            Wdbg(DebugLevel.I, "Message type: {0}", MType)

            'Set the message according to message type
            If MType = MessageType.MOTD Then
                MOTDStreamW = New IO.StreamWriter(MOTDFilePath) With {.AutoFlush = True}
                Wdbg(DebugLevel.I, "Opened stream to MOTD path")
                MOTDStreamW.WriteLine(Message)
                MOTDMessage = Message
            ElseIf MType = MessageType.MAL Then
                MOTDStreamW = New IO.StreamWriter(MALFilePath) With {.AutoFlush = True}
                Wdbg(DebugLevel.I, "Opened stream to MAL path")
                MOTDStreamW.Write(Message)
                MAL = Message
            Else
                W(DoTranslation("MOTD/MAL is valid, but the message type is not valid. Assuming MOTD..."), True, ColTypes.Error)
                MOTDStreamW = New IO.StreamWriter(MOTDFilePath) With {.AutoFlush = True}
                Wdbg(DebugLevel.I, "Opened stream to MOTD path")
                MOTDStreamW.WriteLine(Message)
                MOTDMessage = Message
            End If

            'Close the message stream
            MOTDStreamW.Close()
            Wdbg(DebugLevel.I, "Stream closed")
        Catch ex As Exception
            W(DoTranslation("Error when trying to set MOTD/MAL: {0}"), True, ColTypes.Error, ex.Message)
            WStkTrc(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Reads the message of the day before/after login
    ''' </summary>
    ''' <param name="MType">Message type</param>
    Public Sub ReadMOTD(MType As MessageType)
        Try
            Dim MOTDStreamR As IO.StreamReader
            Dim MOTDBuilder As New Text.StringBuilder

            'Get the MOTD and MAL file path
            MOTDFilePath = GetKernelPath(KernelPathType.MOTD)
            MALFilePath = GetKernelPath(KernelPathType.MAL)
            Wdbg(DebugLevel.I, "Paths: {0}, {1}", MOTDFilePath, MALFilePath)
            Wdbg(DebugLevel.I, "Message type: {0}", MType)

            'Read the message according to message type
            If MType = MessageType.MOTD Then
                MOTDStreamR = New IO.StreamReader(MOTDFilePath)
                Wdbg(DebugLevel.I, "Opened stream to MOTD path")
                MOTDBuilder.Append(MOTDStreamR.ReadToEnd)
                MOTDMessage = MOTDBuilder.ToString
                MOTDStreamR.Close()
                Wdbg(DebugLevel.I, "Stream closed")
            ElseIf MType = MessageType.MAL Then
                MOTDStreamR = New IO.StreamReader(MALFilePath)
                Wdbg(DebugLevel.I, "Opened stream to MAL path")
                MOTDBuilder.Append(MOTDStreamR.ReadToEnd)
                MAL = MOTDBuilder.ToString
                MOTDStreamR.Close()
                Wdbg(DebugLevel.I, "Stream closed")
            Else
                W(DoTranslation("Tried to read MOTD/MAL that is of the invalid message type."), True, ColTypes.Error)
            End If
        Catch ex As Exception
            W(DoTranslation("Error when trying to get MOTD/MAL: {0}"), True, ColTypes.Error, ex.Message)
            WStkTrc(ex)
        End Try
    End Sub
End Module
