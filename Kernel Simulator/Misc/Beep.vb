
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

Module Beep

    Sub BeepFreq()

        W("Beep Frequency in Hz that has the limit of 37-32767 Hz: ", "input")
        answerbeep = System.Console.ReadLine()
        If (CDbl(answerbeep) > Int32.MaxValue) Then
            Wln("Integer overflow on frequency.", "neutralText")
        ElseIf (answerbeep = "q") Then
            Wln("Computer beeping has been cancelled.", "neutralText")
        Else
            If Integer.TryParse(answerbeep, CInt(key)) Then
                If (CDbl(answerbeep) <= 36 Or CDbl(answerbeep) >= 32768) Then
                    Wln("Invalid value for beep frequency.", "neutralText")
                ElseIf (CDbl(answerbeep) > 2048) Then
                    W("WARNING: Beep may be loud, depending on speaker. Setting values higher than 2048 might cause your ears to damage, " + _
                      "and more importantly, your motherboard speaker might deafen, or malfunction." + vbNewLine + _
                      "Are you sure that you want to beep at this frequency, {0}? (y/N) ", "input", answerbeep)
                    Dim answerrape = System.Console.ReadKey.KeyChar
                    If (answerrape = "n" Or answerrape = "N" Or answerrape = "" Or answerrape = "q") Then
                        Wln(vbNewLine + "High frequency. Please read documentation for more info why high frequency shouldn't be used.", "neutralText")
                    ElseIf (answerrape = "y" Or answerrape = "Y") Then
                        System.Console.WriteLine()
                        BeepSystem()
                    End If
                Else
                    BeepSystem()
                End If
            End If
        End If

    End Sub

    Sub BeepSystem()

        W("Beep Time in seconds that has the limit of 1-3600: ", "input")
        answerbeepms = System.Console.ReadLine()
        If Double.TryParse(answerbeepms, key) Then
            If (CDbl(answerbeepms) <= 0 Or CDbl(answerbeepms) >= 3601) Then
                Wln("Invalid value for beep time.", "neutralText")
            ElseIf (answerbeepms = "q") Then
                Wln("Computer beeping has been cancelled.", "neutralText")
            Else
                Beep(CInt(answerbeep), CDbl(answerbeepms))
            End If
        End If

    End Sub

    Sub Beep(ByVal freq As Integer, ByVal s As Double)

        If (freq <= 36 Or freq >= 32768) Then
            Wln("Invalid value for beep frequency.", "neutralText")
        ElseIf (freq > 2048) Then
            W("WARNING: Beep may be loud, depending on speaker. Setting values higher than 2048 might cause your ears to damage, " + _
              "and more importantly, your motherboard speaker might deafen, or malfunction." + vbNewLine + _
              "Are you sure that you want to beep at this frequency, {0}? (y/N) ", "input", answerbeep)
            Dim answerrape = System.Console.ReadKey.KeyChar
            If (answerrape = "n" Or answerrape = "N" Or answerrape = "" Or answerrape = "q") Then
                Wln(vbNewLine + "High frequency. Please read documentation for more info why high frequency shouldn't be used.", "neutralText")
            ElseIf (answerrape = "y" Or answerrape = "Y") Then
                Wln(vbNewLine + "Beeping in {0} seconds in {1} Hz...", "neutralText", s, freq)
                System.Console.Beep(freq, CInt(s * 1000))
                Wln("Beep complete.", "neutralText")
            End If
        Else
            Wln("Beeping in {0} seconds in {1} Hz...", "neutralText", s, freq)
            System.Console.Beep(freq, CInt(s * 1000))
            Wln("Beep complete.", "neutralText")
        End If

    End Sub

End Module
