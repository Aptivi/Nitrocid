
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

Public Module BeepSynth

    ''' <summary>
    ''' Probes the synth file and plays it back using the console PC speaker
    ''' </summary>
    ''' <param name="file">A file name in current shell path</param>
    Public Sub ProbeSynth(ByVal file As String)
        file = NeutralizePath(file)
        Wdbg("I", "Probing {0}...", file)
        If IO.File.Exists(file) Then
            Dim FStream As New IO.StreamReader(file)
            Wdbg("I", "Opened StreamReader(file) with the length of {0}", FStream.BaseStream.Length)
            If FStream.ReadLine = "KS-BSynth" Then
                'Comments are ignored in the file. Comment format: - <message>
                Wdbg("I", "File is scripted")
                While Not FStream.EndOfStream
                    Dim line As String = FStream.ReadLine
                    Wdbg("I", "Line: {0}", line)
                    If Not line.StartsWith("-") And Not line = "" Then
                        Try
                            Wdbg("I", "Not a comment. Getting frequency and time...")
                            Dim freq As Integer = line.Remove(line.IndexOf(","))
                            Dim ms As Integer = line.Substring(line.IndexOf(",") + 1)
                            Wdbg("I", "Got frequency {0} Hz and time {1} ms", freq, ms)
                            Console.Beep(freq, ms)
                        Catch ex As Exception
                            Wdbg("E", "Not a comment and not a synth line. ({0})", line)
                            W(DoTranslation("Failed to probe a synth line.", currentLang), True, ColTypes.Err)
                        End Try
                    End If
                End While
            Else
                Wdbg("E", "File is not scripted")
                W(DoTranslation("The file isn't a scripted synth file.", currentLang), True, ColTypes.Err)
            End If
        Else
            Wdbg("E", "File doesn't exist")
            W(DoTranslation("Scripted file {0} does not exist.", currentLang), True, ColTypes.Err, file)
        End If
    End Sub

End Module
