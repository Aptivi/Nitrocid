
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Module DebugLogPrint

    Sub PrintLog()
        Dim line As String
        Try
            Using dbglog = File.Open(paths("Debugging"), FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite), reader As New StreamReader(dbglog)
                line = reader.ReadLine()
                Do While reader.EndOfStream <> True
                    W(line, True, ColTypes.Neutral)
                    line = reader.ReadLine
                Loop
            End Using
        Catch ex As Exception
            W(DoTranslation("Debug log not found", currentLang), True, ColTypes.Neutral)
            WStkTrc(ex)
        End Try
    End Sub

End Module
