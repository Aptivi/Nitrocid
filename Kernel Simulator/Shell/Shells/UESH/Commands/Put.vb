
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

Imports KS.Network
Imports KS.Network.Transfer

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Upload a file
    ''' </summary>
    ''' <remarks>
    ''' This command uploads a file from the website to a file, preserving the file name. This is currently very basic, but it will be expanded in future releases.
    ''' </remarks>
    Class PutCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim RetryCount As Integer = 1
            Dim FileName As String = NeutralizePath(ListArgs(0))
            Dim URL As String = ListArgs(1)
            Wdbg(DebugLevel.I, "URL: {0}", URL)
            While Not RetryCount > UploadRetries
                Try
                    If Not (URL.StartsWith("ftp://") Or URL.StartsWith("ftps://") Or URL.StartsWith("ftpes://")) Then
                        If Not URL.StartsWith(" ") Then
                            Write(DoTranslation("Uploading {0} to {1}..."), True, ColTypes.Neutral, FileName, URL)
                            If UploadFile(FileName, URL) Then
                                Write(DoTranslation("Upload has completed."), True, ColTypes.Neutral)
                            End If
                        Else
                            Write(DoTranslation("Specify the address"), True, ColTypes.Error)
                        End If
                    Else
                        Write(DoTranslation("Please use ""ftp"" if you are going to upload files to the FTP server."), True, ColTypes.Error)
                    End If
                    Exit Sub
                Catch ex As Exception
                    TransferFinished = False
                    Write(DoTranslation("Upload failed in try {0}: {1}"), True, ColTypes.Error, RetryCount, ex.Message)
                    RetryCount += 1
                    Wdbg(DebugLevel.I, "Try count: {0}", RetryCount)
                    WStkTrc(ex)
                End Try
            End While
        End Sub

    End Class
End Namespace
