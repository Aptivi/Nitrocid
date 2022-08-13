
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

Imports KS.Kernel.Exceptions

Namespace Misc.Probers.Motd
    Public Module MotdParse

        'Variables
        Public MotdFilePath As String = GetKernelPath(KernelPathType.MOTD)

        ''' <summary>
        ''' Sets the Message of the Day
        ''' </summary>
        ''' <param name="Message">A message of the day</param>
        Public Sub SetMotd(Message As String)
            Try
                Dim MOTDStreamW As IO.StreamWriter

                'Get the MOTD and MAL file path
                MotdFilePath = NeutralizePath(MotdFilePath)
                Wdbg(DebugLevel.I, "Path: {0}", MotdFilePath)

                'Set the message according to message type
                MOTDStreamW = New IO.StreamWriter(MotdFilePath) With {.AutoFlush = True}
                Wdbg(DebugLevel.I, "Opened stream to MOTD path")
                MOTDStreamW.WriteLine(Message)
                MOTDMessage = Message

                'Close the message stream
                MOTDStreamW.Close()
                Wdbg(DebugLevel.I, "Stream closed")
            Catch ex As Exception
                Throw New MOTDException(DoTranslation("Error when trying to set MOTD: {0}"), ex.Message)
                WStkTrc(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Reads the message of the day
        ''' </summary>
        Public Sub ReadMotd()
            Try
                Dim MOTDStreamR As IO.StreamReader
                Dim MOTDBuilder As New System.Text.StringBuilder

                'Get the MOTD and MAL file path
                MotdFilePath = NeutralizePath(MotdFilePath)
                Wdbg(DebugLevel.I, "Path: {0}", MotdFilePath)

                'Read the message according to message type
                MOTDStreamR = New IO.StreamReader(MotdFilePath)
                Wdbg(DebugLevel.I, "Opened stream to MOTD path")
                MOTDBuilder.Append(MOTDStreamR.ReadToEnd)
                MOTDMessage = MOTDBuilder.ToString
                MOTDStreamR.Close()
                Wdbg(DebugLevel.I, "Stream closed")
            Catch ex As Exception
                Throw New MOTDException(DoTranslation("Error when trying to get MOTD: {0}"), ex.Message)
                WStkTrc(ex)
            End Try
        End Sub

    End Module
End Namespace
