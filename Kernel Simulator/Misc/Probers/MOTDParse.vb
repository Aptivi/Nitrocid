
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Module MOTDParse

    'Variables (Streams)
    Public MOTDStreamR As IO.StreamReader
    Public MOTDStreamW As IO.StreamWriter
    Public MOTDFilePath, MALFilePath As String

    'Enumerators (Type of message)
    Public Enum MessageType As Integer
        MOTD = 1
        MAL = 2
    End Enum

    Public Sub SetMOTD(ByVal MOTD As String, ByVal MType As MessageType)
        MOTDFilePath = paths("Home") + "/MOTD.txt"
        MALFilePath = paths("Home") + "/MAL.txt"
        If MType = 1 Then
            MOTDStreamW = New IO.StreamWriter(MOTDFilePath) With {.AutoFlush = True}
            MOTDStreamW.WriteLine(MOTD)
            MOTDMessage = MOTD
        ElseIf MType = 2 Then
            MOTDStreamW = New IO.StreamWriter(MALFilePath) With {.AutoFlush = True}
            MOTDStreamW.Write(MOTD)
            MAL = MOTD
        Else
            W(DoTranslation("MOTD/MAL is valid, but the message type is not valid. Assuming MOTD...", currentLang), True, "neutralText")
            MOTDStreamW = New IO.StreamWriter(MOTDFilePath) With {.AutoFlush = True}
            MOTDStreamW.WriteLine(MOTD)
            MOTDMessage = MOTD
        End If
        MOTDStreamW.Close()
    End Sub
    Public Sub ReadMOTDFromFile(ByVal MType As MessageType)
        MOTDFilePath = paths("Home") + "/MOTD.txt"
        MALFilePath = paths("Home") + "/MAL.txt"
        Dim MOTDBuilder As New Text.StringBuilder
        If MType = 1 Then
            MOTDStreamR = New IO.StreamReader(MOTDFilePath)
            MOTDBuilder.Append(MOTDStreamR.ReadToEnd)
            MOTDMessage = MOTDBuilder.ToString
        ElseIf MType = 2 Then
            MOTDStreamR = New IO.StreamReader(MALFilePath)
            MOTDBuilder.Append(MOTDStreamR.ReadToEnd)
            MAL = MOTDBuilder.ToString
        Else
            W(DoTranslation("Tried to read MOTD/MAL that is of the invalid message type.", currentLang), True, "neutralText")
        End If
        MOTDStreamR.Close()
    End Sub
End Module
