
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Namespace Misc.Encryption
    Public Module HashVerifier

        ''' <summary>
        ''' Verifies the hash sum of a file from hashes file
        ''' </summary>
        ''' <param name="FileName">Target file</param>
        ''' <param name="HashType">Hash algorithm</param>
        ''' <param name="HashesFile">Hashes file that contains the target file</param>
        ''' <param name="ActualHash">Actual hash calculated from hash tool</param>
        ''' <returns>True if they match; else, false.</returns>
        ''' <exception cref="Exceptions.InvalidHashException"></exception>
        ''' <exception cref="Exceptions.InvalidHashAlgorithmException"></exception>
        ''' <exception cref="FileNotFoundException"></exception>
        Public Function VerifyHashFromHashesFile(FileName As String, HashType As Algorithms, HashesFile As String, ActualHash As String) As Boolean
            Dim ExpectedHashLength As Integer
            Dim ExpectedHash As String = ""

            FileName = NeutralizePath(FileName)
            HashesFile = NeutralizePath(HashesFile)
            Wdbg(DebugLevel.I, "File name: {0}", FileName)
            Wdbg(DebugLevel.I, "Hashes file name: {0}", HashesFile)
            If FileExists(FileName) Then
                Wdbg(DebugLevel.I, "Hash type: {0} ({1})", HashType, HashType.ToString)
                ExpectedHashLength = GetExpectedHashLength(HashType)

                'Verify the hash
                If FileExists(HashesFile) Then
                    Dim HashStream As New StreamReader(HashesFile)
                    Wdbg(DebugLevel.I, "Stream length: {0}", HashStream.BaseStream.Length)
                    Do While Not HashStream.EndOfStream
                        'Check if made from KS, and take it from before-last split space. If not, take it from the beginning
                        Dim StringLine As String = HashStream.ReadLine
                        If StringLine.StartsWith("- ") Then
                            Wdbg(DebugLevel.I, "Hashes file is of KS format")
                            If (StringLine.StartsWith("- " + FileName) Or StringLine.StartsWith("- " + Path.GetFileName(FileName))) And StringLine.EndsWith($"({HashType})") Then
                                Dim HashSplit() As String = StringLine.Split(" "c)
                                ExpectedHash = HashSplit(HashSplit.Length - 2).ToUpper
                                ActualHash = ActualHash.ToUpper
                            End If
                        Else
                            Wdbg(DebugLevel.I, "Hashes file is of standard format")
                            If StringLine.EndsWith(Path.GetFileName(FileName)) Then
                                Dim HashSplit() As String = StringLine.Split(" "c)
                                ExpectedHash = HashSplit(0).ToUpper
                                ActualHash = ActualHash.ToUpper
                            End If
                        End If
                    Loop
                    HashStream.Close()
                Else
                    Throw New FileNotFoundException("Hashes file {0} not found.".FormatString(HashesFile))
                End If

                If ActualHash.Length = ExpectedHashLength And ExpectedHash.Length = ExpectedHashLength Then
                    Wdbg(DebugLevel.I, "Hashes are consistent.")
                    Wdbg(DebugLevel.I, "Hashes {0} and {1}", ActualHash, ExpectedHash)
                    If ActualHash = ExpectedHash Then
                        Wdbg(DebugLevel.I, "Hashes match.")
                        Return True
                    Else
                        Wdbg(DebugLevel.W, "Hashes don't match.")
                        Return False
                    End If
                Else
                    Wdbg(DebugLevel.E, "{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength)
                    Throw New Exceptions.InvalidHashException("{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength)
                End If
            Else
                Throw New FileNotFoundException("File {0} not found.".FormatString(FileName))
            End If
        End Function

        ''' <summary>
        ''' Verifies the hash sum of a file from expected hash
        ''' </summary>
        ''' <param name="FileName">Target file</param>
        ''' <param name="HashType">Hash algorithm</param>
        ''' <param name="ExpectedHash">Expected hash of a target file</param>
        ''' <param name="ActualHash">Actual hash calculated from hash tool</param>
        ''' <returns>True if they match; else, false.</returns>
        ''' <exception cref="Exceptions.InvalidHashException"></exception>
        ''' <exception cref="Exceptions.InvalidHashAlgorithmException"></exception>
        ''' <exception cref="FileNotFoundException"></exception>
        Public Function VerifyHashFromHash(FileName As String, HashType As Algorithms, ExpectedHash As String, ActualHash As String) As Boolean
            Dim ExpectedHashLength As Integer

            FileName = NeutralizePath(FileName)
            ExpectedHash = ExpectedHash.ToUpper
            ActualHash = ActualHash.ToUpper
            Wdbg(DebugLevel.I, "File name: {0}", FileName)
            If FileExists(FileName) Then
                Wdbg(DebugLevel.I, "Hash type: {0} ({1})", HashType, HashType.ToString)
                ExpectedHashLength = GetExpectedHashLength(HashType)

                'Verify the hash
                If ActualHash.Length = ExpectedHashLength And ExpectedHash.Length = ExpectedHashLength Then
                    Wdbg(DebugLevel.I, "Hashes are consistent.")
                    Wdbg(DebugLevel.I, "Hashes {0} and {1}", ActualHash, ExpectedHash)
                    If ActualHash = ExpectedHash Then
                        Wdbg(DebugLevel.I, "Hashes match.")
                        Return True
                    Else
                        Wdbg(DebugLevel.W, "Hashes don't match.")
                        Return False
                    End If
                Else
                    Wdbg(DebugLevel.E, "{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength)
                    Throw New Exceptions.InvalidHashException("{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength)
                End If
            Else
                Throw New FileNotFoundException("File {0} not found.".FormatString(FileName))
            End If
        End Function

        ''' <summary>
        ''' Verifies the hash sum of a file from hashes file once the file's hash is calculated.
        ''' </summary>
        ''' <param name="FileName">Target file</param>
        ''' <param name="HashType">Hash algorithm</param>
        ''' <param name="HashesFile">Hashes file that contains the target file</param>
        ''' <returns>True if they match; else, false.</returns>
        ''' <exception cref="Exceptions.InvalidHashException"></exception>
        ''' <exception cref="Exceptions.InvalidHashAlgorithmException"></exception>
        ''' <exception cref="FileNotFoundException"></exception>
        Public Function VerifyUncalculatedHashFromHashesFile(FileName As String, HashType As Algorithms, HashesFile As String) As Boolean
            Dim ExpectedHashLength As Integer
            Dim ExpectedHash As String = ""
            Dim ActualHash As String = ""

            FileName = NeutralizePath(FileName)
            HashesFile = NeutralizePath(HashesFile)
            Wdbg(DebugLevel.I, "File name: {0}", FileName)
            Wdbg(DebugLevel.I, "Hashes file name: {0}", HashesFile)
            If FileExists(FileName) Then
                Wdbg(DebugLevel.I, "Hash type: {0} ({1})", HashType, HashType.ToString)
                ExpectedHashLength = GetExpectedHashLength(HashType)

                'Verify the hash
                If FileExists(HashesFile) Then
                    Dim HashStream As New StreamReader(HashesFile)
                    Wdbg(DebugLevel.I, "Stream length: {0}", HashStream.BaseStream.Length)
                    Do While Not HashStream.EndOfStream
                        'Check if made from KS, and take it from before-last split space. If not, take it from the beginning
                        Dim StringLine As String = HashStream.ReadLine
                        If StringLine.StartsWith("- ") Then
                            Wdbg(DebugLevel.I, "Hashes file is of KS format")
                            If (StringLine.StartsWith("- " + FileName) Or StringLine.StartsWith("- " + Path.GetFileName(FileName))) And StringLine.EndsWith($"({HashType})") Then
                                Dim HashSplit() As String = StringLine.Split(" "c)
                                ExpectedHash = HashSplit(HashSplit.Length - 2).ToUpper
                                ActualHash = GetEncryptedFile(FileName, HashType).ToUpper
                            End If
                        Else
                            Wdbg(DebugLevel.I, "Hashes file is of standard format")
                            If StringLine.EndsWith(Path.GetFileName(FileName)) Then
                                Dim HashSplit() As String = StringLine.Split(" "c)
                                ExpectedHash = HashSplit(0).ToUpper
                                ActualHash = GetEncryptedFile(FileName, HashType).ToUpper
                            End If
                        End If
                    Loop
                    HashStream.Close()
                Else
                    Throw New FileNotFoundException("Hashes file {0} not found.".FormatString(HashesFile))
                End If

                If ActualHash.Length = ExpectedHashLength And ExpectedHash.Length = ExpectedHashLength Then
                    Wdbg(DebugLevel.I, "Hashes are consistent.")
                    Wdbg(DebugLevel.I, "Hashes {0} and {1}", ActualHash, ExpectedHash)
                    If ActualHash = ExpectedHash Then
                        Wdbg(DebugLevel.I, "Hashes match.")
                        Return True
                    Else
                        Wdbg(DebugLevel.W, "Hashes don't match.")
                        Return False
                    End If
                Else
                    Wdbg(DebugLevel.E, "{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength)
                    Throw New Exceptions.InvalidHashException("{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength)
                End If
            Else
                Throw New FileNotFoundException("File {0} not found.".FormatString(FileName))
            End If
        End Function

        ''' <summary>
        ''' Verifies the hash sum of a file from expected hash once the file's hash is calculated.
        ''' </summary>
        ''' <param name="FileName">Target file</param>
        ''' <param name="HashType">Hash algorithm</param>
        ''' <param name="ExpectedHash">Expected hash of a target file</param>
        ''' <returns>True if they match; else, false.</returns>
        ''' <exception cref="Exceptions.InvalidHashException"></exception>
        ''' <exception cref="Exceptions.InvalidHashAlgorithmException"></exception>
        ''' <exception cref="FileNotFoundException"></exception>
        Public Function VerifyUncalculatedHashFromHash(FileName As String, HashType As Algorithms, ExpectedHash As String) As Boolean
            Dim ExpectedHashLength As Integer
            Dim ActualHash As String
            FileName = NeutralizePath(FileName)
            ExpectedHash = ExpectedHash.ToUpper
            Wdbg(DebugLevel.I, "File name: {0}", FileName)
            If FileExists(FileName) Then
                Wdbg(DebugLevel.I, "Hash type: {0} ({1})", HashType, HashType.ToString)
                ExpectedHashLength = GetExpectedHashLength(HashType)

                'Calculate the file hash
                ActualHash = GetEncryptedFile(FileName, HashType).ToUpper

                'Verify the hash
                If ActualHash.Length = ExpectedHashLength And ExpectedHash.Length = ExpectedHashLength Then
                    Wdbg(DebugLevel.I, "Hashes are consistent.")
                    Wdbg(DebugLevel.I, "Hashes {0} and {1}", ActualHash, ExpectedHash)
                    If ActualHash = ExpectedHash Then
                        Wdbg(DebugLevel.I, "Hashes match.")
                        Return True
                    Else
                        Wdbg(DebugLevel.W, "Hashes don't match.")
                        Return False
                    End If
                Else
                    Wdbg(DebugLevel.E, "{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength)
                    Throw New Exceptions.InvalidHashException("{0} ({1}) or {2} ({3}) is malformed. Check the algorithm ({4}). Expected length: {5}", ActualHash, ActualHash.Length, ExpectedHash, ExpectedHash.Length, HashType, ExpectedHashLength)
                End If
            Else
                Throw New FileNotFoundException("File {0} not found.".FormatString(FileName))
            End If
        End Function

        ''' <summary>
        ''' Gets the expected hash length
        ''' </summary>
        ''' <param name="HashType">An encryption algorithm</param>
        ''' <returns>The expected hash length</returns>
        Public Function GetExpectedHashLength(HashType As Algorithms) As Integer
            Select Case HashType
                Case Algorithms.SHA512
                    Return 128
                Case Algorithms.SHA384
                    Return 96
                Case Algorithms.SHA256
                    Return 64
                Case Algorithms.SHA1
                    Return 40
                Case Algorithms.MD5
                    Return 32
                Case Algorithms.CRC32
                    Return 8
                Case Else
                    Throw New Exceptions.InvalidHashAlgorithmException("Invalid encryption algorithm.")
            End Select
        End Function

    End Module
End Namespace