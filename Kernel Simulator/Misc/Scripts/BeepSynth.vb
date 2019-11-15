
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

Module BeepSynth

    Sub ProbeSynth(ByVal file As String)
        file = CurrDir + "/" + file.Replace("\", "/")
        If file.Contains(CurrDir.Replace("\", "/")) And file.AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
#Disable Warning IDE0059
            file = ReplaceLastOccurrence(file, CurrDir, "")
#Enable Warning IDE0059
        End If
        If IO.File.Exists(file) Then
            Dim FStream As New IO.StreamReader(file)
            If FStream.ReadLine = "KS-BSynth" Then
                'Comments are ignored in the file. Comment format: - <message>
                While Not FStream.EndOfStream
                    Dim line As String = FStream.ReadLine
                    If Not line.StartsWith("-") And Not line = "" Then
                        Try
                            Dim freq As Integer = line.Remove(line.IndexOf(","))
                            Dim ms As Integer = line.Substring(line.IndexOf(",") + 1)
                            Console.Beep(freq, ms)
                        Catch ex As Exception
                            W(DoTranslation("Failed to probe a synth line.", currentLang), True, ColTypes.Neutral)
                        End Try
                    End If
                End While
            Else
                W(DoTranslation("The file isn't a scripted synth file.", currentLang), True, ColTypes.Neutral)
            End If
        Else
            W(DoTranslation("Scripted file {0} does not exist.", currentLang), True, ColTypes.Neutral, file)
        End If
    End Sub

End Module
