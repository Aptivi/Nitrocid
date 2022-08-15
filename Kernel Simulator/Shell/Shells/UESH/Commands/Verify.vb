
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
    Class VerifyCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Try
                Dim HashFile As String = NeutralizePath(ListArgs(2))
                If FileExists(HashFile) Then
                    If VerifyHashFromHashesFile(ListArgs(3), [Enum].Parse(GetType(Algorithms), ListArgs(0)), ListArgs(2), ListArgs(1)) Then
                        Write(DoTranslation("Hashes match."), True, ColTypes.Neutral)
                    Else
                        Write(DoTranslation("Hashes don't match."), True, ColTypes.Warning)
                    End If
                Else
                    If VerifyHashFromHash(ListArgs(3), [Enum].Parse(GetType(Algorithms), ListArgs(0)), ListArgs(2), ListArgs(1)) Then
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
                Write(DoTranslation("{0} is not found."), True, ColTypes.Error, ListArgs(3))
            End Try
        End Sub

    End Class
End Namespace
