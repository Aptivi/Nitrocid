
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

Imports KS.Shell.Shells.FTP

Namespace Network.FTP.Filesystem
    Public Module FTPHashing

        ''' <summary>
        ''' Gets a hash for file
        ''' </summary>
        ''' <param name="File">A file to be hashed</param>
        ''' <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="Exceptions.FTPFilesystemException"></exception>
        ''' <exception cref="InvalidOperationException"></exception>
        ''' <exception cref="ArgumentNullException"></exception>
        Public Function FTPGetHash(File As String, HashAlgorithm As FtpHashAlgorithm) As FtpHash
            If FtpConnected = True Then
                If File <> "" Then
                    If ClientFTP.FileExists(File) Then
                        Wdbg(DebugLevel.I, "Hashing {0} using {1}...", File, HashAlgorithm.ToString)
                        Return ClientFTP.GetChecksum(File, HashAlgorithm)
                    Else
                        Wdbg(DebugLevel.E, "{0} is not found.", File)
                        Throw New Exceptions.FTPFilesystemException(DoTranslation("{0} is not found in the server."), File)
                    End If
                Else
                    Throw New ArgumentNullException(File, DoTranslation("Enter a remote file to be hashed."))
                End If
            Else
                Throw New InvalidOperationException(DoTranslation("You must connect to a server before performing this operation."))
            End If
        End Function

        ''' <summary>
        ''' Gets a hash for files in a directory
        ''' </summary>
        ''' <param name="Directory">A directory for its contents to be hashed</param>
        ''' <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        ''' <exception cref="Exceptions.FTPFilesystemException"></exception>
        ''' <exception cref="InvalidOperationException"></exception>
        ''' <exception cref="ArgumentNullException"></exception>
        Public Function FTPGetHashes(Directory As String, HashAlgorithm As FtpHashAlgorithm) As Dictionary(Of String, FtpHash)
            Return FTPGetHashes(Directory, HashAlgorithm, FtpRecursiveHashing)
        End Function

        ''' <summary>
        ''' Gets a hash for files in a directory
        ''' </summary>
        ''' <param name="Directory">A directory for its contents to be hashed</param>
        ''' <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        ''' <param name="Recurse">Whether to hash the files within the subdirectories too.</param>
        ''' <exception cref="Exceptions.FTPFilesystemException"></exception>
        ''' <exception cref="InvalidOperationException"></exception>
        ''' <exception cref="ArgumentNullException"></exception>
        Public Function FTPGetHashes(Directory As String, HashAlgorithm As FtpHashAlgorithm, Recurse As Boolean) As Dictionary(Of String, FtpHash)
            If FtpConnected = True Then
                If Directory <> "" Then
                    If ClientFTP.DirectoryExists(Directory) Then
                        Dim Hashes As New Dictionary(Of String, FtpHash)
                        Dim Items As FtpListItem()
                        If Recurse Then
                            Items = ClientFTP.GetListing(Directory, FtpListOption.Recursive)
                        Else
                            Items = ClientFTP.GetListing(Directory)
                        End If
                        For Each Item As FtpListItem In Items
                            Wdbg(DebugLevel.I, "Hashing {0} using {1}...", Item.FullName, HashAlgorithm.ToString)
                            Hashes.Add(Item.FullName, FTPGetHash(Item.FullName, HashAlgorithm))
                        Next
                        Return Hashes
                    Else
                        Wdbg(DebugLevel.E, "{0} is not found.", Directory)
                        Throw New Exceptions.FTPFilesystemException(DoTranslation("{0} is not found in the server."), Directory)
                    End If
                Else
                    Throw New ArgumentNullException(Directory, DoTranslation("Enter a remote directory."))
                End If
            Else
                Throw New InvalidOperationException(DoTranslation("You must connect to a server before performing this operation."))
            End If
        End Function

    End Module
End Namespace
