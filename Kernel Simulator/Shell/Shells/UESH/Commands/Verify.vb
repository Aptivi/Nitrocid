
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

Imports System.IO
Imports KS.Files.Querying
Imports KS.Misc.Encryption

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Verifies the file
    ''' </summary>
    ''' <remarks>
    ''' If you've previously calculated the hash sum of a file using the supported algorithms, and you have the expected hash or file that contains the hashes list, you can use this command to verify the sanity of the file.
    ''' <br></br>
    ''' It can handle three types:
    ''' <br></br>
    ''' <list type="bullet">
    ''' <item>
    ''' <term>First</term>
    ''' <description>It can verify files by comparing expected hash with the actual hash.</description>
    ''' </item>
    ''' <item>
    ''' <term>Second</term>
    ''' <description>It can verify files by opening the hashes file that sumfiles generated and finding the hash of the file.</description>
    ''' </item>
    ''' <item>
    ''' <term>Third</term>
    ''' <description>It can verify files by opening the hashes file that some other tool generated and finding the hash of the file, assuming that it's in this format: <c>&lt;expected hash&gt; &lt;file name&gt;</c></description>
    ''' </item>
    ''' </list>
    ''' <br></br>
    ''' If the hashes match, that means that the file is not corrupted. However, if they don't match, the file is corrupted and must be redownloaded.
    ''' <br></br>
    ''' If you run across a hash file that verify can't parse, feel free to post an issue or make a PR.
    ''' </remarks>
    Class VerifyCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Try
                Dim HashFile As String = NeutralizePath(ListArgsOnly(2))
                If FileExists(HashFile) Then
                    If VerifyHashFromHashesFile(ListArgsOnly(3), [Enum].Parse(GetType(Algorithms), ListArgsOnly(0)), ListArgsOnly(2), ListArgsOnly(1)) Then
                        Write(DoTranslation("Hashes match."), True, ColTypes.Neutral)
                    Else
                        Write(DoTranslation("Hashes don't match."), True, ColTypes.Warning)
                    End If
                Else
                    If VerifyHashFromHash(ListArgsOnly(3), [Enum].Parse(GetType(Algorithms), ListArgsOnly(0)), ListArgsOnly(2), ListArgsOnly(1)) Then
                        Write(DoTranslation("Hashes match."), True, ColTypes.Neutral)
                    Else
                        Write(DoTranslation("Hashes don't match."), True, ColTypes.Warning)
                    End If
                End If
            Catch ihae As Exceptions.InvalidHashAlgorithmException
                WStkTrc(ihae)
                Write(DoTranslation("Invalid encryption algorithm."), True, ColTypes.Error)
            Catch ihe As Exceptions.InvalidHashException
                WStkTrc(ihe)
                Write(DoTranslation("Hashes are malformed."), True, ColTypes.Error)
            Catch fnfe As FileNotFoundException
                WStkTrc(fnfe)
                Write(DoTranslation("{0} is not found."), True, ColTypes.Error, ListArgsOnly(3))
            End Try
        End Sub

    End Class
End Namespace
