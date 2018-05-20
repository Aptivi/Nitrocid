
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Module PanicSim

    Sub panicPrompt()

        W("Write a message: ", "input")
        Dim kpmsg As String = System.Console.ReadLine()
        If (kpmsg = "") Then
            Wln("Blank message.", "neutralText")
        ElseIf (kpmsg = "q") Then
            Wln("Text printing has been cancelled.", "neutralText")
        Else
            W("Write error type: ", "input")
            Dim kpet = System.Console.ReadKey.KeyChar
            System.Console.WriteLine()
            If (kpet = "") Then
                Wln("Blank error type", "neutralText")
            ElseIf (kpet = "q") Then
                Wln("Text printing has been cancelled.", "neutralText")
            ElseIf (kpet = "S" Or kpet = "U" Or kpet = "D" Or kpet = "F" Or kpet = "C") Then
                W("Restart time in seconds: ", "input")
                Dim kptime = System.Console.ReadLine()
                If (kptime = "") Then
                    Wln("Blank time", "neutralText")
                ElseIf (CDbl(kptime) <= 3600 And (kpet <> "C" Or kpet <> "D")) Then
                    KernelError(kpet, True, CLng(kptime), kpmsg)
                ElseIf ((CDbl(kptime) <= 3600 And kpet = "C") Or (CDbl(kptime) <= 0 And kpet = "C") Or (CDbl(kptime) <= 3600 And kpet = "D") Or (CDbl(kptime) <= 0 And kpet = "D")) Then
                    KernelError(kpet, False, 0, kpmsg)
                ElseIf (CDbl(kptime) <= 0 And kpet <> "C") Then
                    Wln("Invalid time.", "neutralText")
                ElseIf (kptime = "q") Then
                    Wln("Text printing has been cancelled.", "neutralText")
                End If
            Else
                Wln("Invalid error type", "neutralText")
            End If
        End If

    End Sub

End Module
