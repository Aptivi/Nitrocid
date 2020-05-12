
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

Imports System.Security.Cryptography
Imports System.Text

Public Module Encryption
    Public Enum Algorithms
        MD5
        SHA1
        SHA256
    End Enum

    Function GetArrayEnc(ByVal encrypted As Byte()) As String
        Dim hash As String = ""
        For i As Integer = 0 To encrypted.Length - 1
            Wdbg("I", "Appending {0} to hash", encrypted(i))
            hash += $"{encrypted(i):X2}"
        Next
        Wdbg("I", "Final hash: {0}", hash)
        Return hash
    End Function

    Public Function GetEncryptedString(ByVal str As String, ByVal algorithm As Algorithms) As String
        Wdbg("I", "Selected algorithm: {0}", algorithm.ToString)
        Wdbg("I", "String length: {0}", str.Length)
        Select Case algorithm
            Case 0
                Dim hashbyte As Byte() = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(str))
                Return GetArrayEnc(hashbyte)
            Case 1
                Dim hashbyte As Byte() = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(str))
                Return GetArrayEnc(hashbyte)
            Case 2
                Dim hashbyte As Byte() = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(str))
                Return GetArrayEnc(hashbyte)
        End Select
        Return ""
    End Function
End Module
