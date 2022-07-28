
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

Imports SharpCompress.Archives.Zip
Imports System.IO
Imports System.Threading
Imports KS.Files.Folders
Imports KS.Misc.ZipFile
Imports KS.Shell.Prompts

Namespace Shell.Shells
    Public Class ZipShell
        Inherits ShellExecutor
        Implements IShell

        Public Overrides ReadOnly Property ShellType As ShellType Implements IShell.ShellType
            Get
                Return ShellType.ZIPShell
            End Get
        End Property

        Public Overrides Property Bail As Boolean Implements IShell.Bail

        Public Overrides Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
            'Set current directory for ZIP shell
            ZipShell_CurrentDirectory = CurrentDir

            'Get file path
            Dim ZipFile As String = ""
            If ShellArgs.Length > 0 Then
                ZipFile = ShellArgs(0)
            Else
                Write(DoTranslation("File not specified. Exiting shell..."), True, ColTypes.Error)
                Bail = True
            End If

            While Not Bail
                Try
                    'Open file if not open
                    If ZipShell_FileStream Is Nothing Then ZipShell_FileStream = New FileStream(ZipFile, FileMode.Open)
                    If ZipShell_ZipArchive Is Nothing Then ZipShell_ZipArchive = ZipArchive.Open(ZipShell_FileStream)

                    'See UESHShell.vb for more info
                    SyncLock GetCancelSyncLock(ShellType)
                        'Prepare for prompt
                        If DefConsoleOut IsNot Nothing Then
                            Console.SetOut(DefConsoleOut)
                        End If
                        WriteShellPrompt(ShellType)

                        'Raise the event
                        KernelEventManager.RaiseZipShellInitialized()
                    End SyncLock

                    'Prompt for the command
                    Dim WrittenCommand As String = ReadLine()
                    If Not (WrittenCommand = Nothing Or WrittenCommand?.StartsWithAnyOf({" ", "#"})) Then
                        KernelEventManager.RaiseZipPreExecuteCommand(WrittenCommand)
                        GetLine(WrittenCommand, False, "", ShellType.ZIPShell)
                        KernelEventManager.RaiseZipPostExecuteCommand(WrittenCommand)
                    End If
                Catch taex As ThreadInterruptedException
                    CancelRequested = False
                    Bail = True
                Catch ex As Exception
                    WStkTrc(ex)
                    Write(DoTranslation("There was an error in the shell.") + NewLine + "Error {0}: {1}", True, ColTypes.Error, ex.GetType.FullName, ex.Message)
                    Continue While
                End Try
            End While

            'Close file stream
            ZipShell_ZipArchive.Dispose()
            ZipShell_CurrentDirectory = ""
            ZipShell_CurrentArchiveDirectory = ""
            ZipShell_ZipArchive = Nothing
            ZipShell_FileStream = Nothing
        End Sub

    End Class
End Namespace
