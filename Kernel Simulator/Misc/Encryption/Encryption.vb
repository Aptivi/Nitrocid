
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
Imports System.Security.Cryptography
Imports System.Text
Imports Force.Crc32

Namespace Misc.Encryption
    Public Module Encryption

        ''' <summary>
        ''' Encryption algorithms supported by KS
        ''' </summary>
        Public Enum Algorithms
            ''' <summary>
            ''' The MD5 Algorithm
            ''' </summary>
            MD5
            ''' <summary>
            ''' The SHA1 Algorithm
            ''' </summary>
            SHA1
            ''' <summary>
            ''' The SHA256 Algorithm
            ''' </summary>
            SHA256
            ''' <summary>
            ''' The SHA384 Algorithm
            ''' </summary>
            SHA384
            ''' <summary>
            ''' The SHA512 Algorithm
            ''' </summary>
            SHA512
            ''' <summary>
            ''' The CRC32 Algorithm
            ''' </summary>
            CRC32
        End Enum

        ''' <summary>
        ''' Translates the array of encrypted bytes to string
        ''' </summary>
        ''' <param name="encrypted">Array of encrypted bytes</param>
        ''' <returns>A string representation of hash sum</returns>
        Function GetArrayEnc(encrypted As Byte()) As String
            Dim hash As String = ""
            For i As Integer = 0 To encrypted.Length - 1
                Wdbg(DebugLevel.I, "Appending {0} to hash", encrypted(i))
                hash += $"{encrypted(i):X2}"
            Next
            Wdbg(DebugLevel.I, "Final hash: {0}", hash)
            Return hash
        End Function

        ''' <summary>
        ''' Encrypts a string
        ''' </summary>
        ''' <param name="str">Source string</param>
        ''' <param name="algorithm">Algorithm</param>
        ''' <returns>Encrypted hash sum</returns>
        Public Function GetEncryptedString(str As String, algorithm As Algorithms) As String
            Wdbg(DebugLevel.I, "Selected algorithm: {0}", algorithm.ToString)
            Wdbg(DebugLevel.I, "String length: {0}", str.Length)
            Select Case algorithm
                Case Algorithms.MD5
                    Dim hashbyte As Byte() = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(str))
                    Return GetArrayEnc(hashbyte)
                Case Algorithms.SHA1
                    Dim hashbyte As Byte() = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(str))
                    Return GetArrayEnc(hashbyte)
                Case Algorithms.SHA256
                    Dim hashbyte As Byte() = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(str))
                    Return GetArrayEnc(hashbyte)
                Case Algorithms.SHA384
                    Dim hashbyte As Byte() = SHA384.Create().ComputeHash(Encoding.UTF8.GetBytes(str))
                    Return GetArrayEnc(hashbyte)
                Case Algorithms.SHA512
                    Dim hashbyte As Byte() = SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(str))
                    Return GetArrayEnc(hashbyte)
                Case Algorithms.CRC32
                    Dim hashbyte As Byte() = New Crc32Algorithm().ComputeHash(Encoding.UTF8.GetBytes(str))
                    Return GetArrayEnc(hashbyte)
            End Select
            Return ""
        End Function

        ''' <summary>
        ''' Encrypts a file
        ''' </summary>
        ''' <param name="str">Source stream</param>
        ''' <param name="algorithm">Algorithm</param>
        ''' <returns>Encrypted hash sum</returns>
        Public Function GetEncryptedFile(str As Stream, algorithm As Algorithms) As String
            Wdbg(DebugLevel.I, "Selected algorithm: {0}", algorithm.ToString)
            Wdbg(DebugLevel.I, "Stream length: {0}", str.Length)
            Select Case algorithm
                Case Algorithms.MD5
                    Dim hashbyte As Byte() = MD5.Create().ComputeHash(str)
                    Return GetArrayEnc(hashbyte)
                Case Algorithms.SHA1
                    Dim hashbyte As Byte() = SHA1.Create().ComputeHash(str)
                    Return GetArrayEnc(hashbyte)
                Case Algorithms.SHA256
                    Dim hashbyte As Byte() = SHA256.Create().ComputeHash(str)
                    Return GetArrayEnc(hashbyte)
                Case Algorithms.SHA384
                    Dim hashbyte As Byte() = SHA384.Create().ComputeHash(str)
                    Return GetArrayEnc(hashbyte)
                Case Algorithms.SHA512
                    Dim hashbyte As Byte() = SHA512.Create().ComputeHash(str)
                    Return GetArrayEnc(hashbyte)
                Case Algorithms.CRC32
                    Dim hashbyte As Byte() = New Crc32Algorithm().ComputeHash(str)
                    Return GetArrayEnc(hashbyte)
            End Select
            Return ""
        End Function

        ''' <summary>
        ''' Encrypts a file
        ''' </summary>
        ''' <param name="Path">Relative path</param>
        ''' <param name="algorithm">Algorithm</param>
        ''' <returns>Encrypted hash sum</returns>
        Public Function GetEncryptedFile(Path As String, algorithm As Algorithms) As String
            Path = NeutralizePath(Path)
            Dim Str As New FileStream(Path, FileMode.Open)
            Dim Encrypted As String = GetEncryptedFile(Str, algorithm)
            Str.Close()
            Return Encrypted
        End Function

        ''' <summary>
        ''' Gets empty hash
        ''' </summary>
        ''' <param name="Algorithm">Algorithm</param>
        ''' <returns>Empty hash</returns>
        Public Function GetEmptyHash(Algorithm As Algorithms) As String
            Return GetEncryptedString("", Algorithm)
        End Function

    End Module
End Namespace
