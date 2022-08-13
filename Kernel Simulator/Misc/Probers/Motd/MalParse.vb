
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
    Public Module MalParse

        'Variables
        Public MalFilePath As String = GetKernelPath(KernelPathType.MAL)

        ''' <summary>
        ''' Sets the MAL
        ''' </summary>
        ''' <param name="Message">A message of the day after login</param>
        Public Sub SetMal(Message As String)
            Try
                Dim MALStreamW As IO.StreamWriter

                'Get the MOTD and MAL file path
                MalFilePath = NeutralizePath(MalFilePath)
                Wdbg(DebugLevel.I, "Path: {0}", MalFilePath)

                'Set the message according to message type
                MALStreamW = New IO.StreamWriter(MalFilePath) With {.AutoFlush = True}
                Wdbg(DebugLevel.I, "Opened stream to MAL path")
                MALStreamW.Write(Message)
                MAL = Message

                'Close the message stream
                MALStreamW.Close()
                Wdbg(DebugLevel.I, "Stream closed")
            Catch ex As Exception
                Throw New MOTDException(DoTranslation("Error when trying to set MAL: {0}"), ex.Message)
                WStkTrc(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Reads the message of the day before/after login
        ''' </summary>
        Public Sub ReadMal()
            Try
                Dim MALStreamR As IO.StreamReader
                Dim MALBuilder As New System.Text.StringBuilder

                'Get the MOTD and MAL file path
                MalFilePath = NeutralizePath(MalFilePath)
                Wdbg(DebugLevel.I, "Path: {0}", MalFilePath)

                'Read the message according to message type
                MALStreamR = New IO.StreamReader(MalFilePath)
                Wdbg(DebugLevel.I, "Opened stream to MAL path")
                MALBuilder.Append(MALStreamR.ReadToEnd)
                MAL = MALBuilder.ToString
                MALStreamR.Close()
                Wdbg(DebugLevel.I, "Stream closed")
            Catch ex As Exception
                Throw New MOTDException(DoTranslation("Error when trying to get MAL: {0}"), ex.Message)
                WStkTrc(ex)
            End Try
        End Sub

    End Module
End Namespace
