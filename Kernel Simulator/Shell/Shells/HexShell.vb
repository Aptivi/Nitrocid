
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

Imports System.Threading
Imports KS.Misc.Editors.HexEdit
Imports KS.Shell.Prompts

Namespace Shell.Shells
    Public Class HexShell
        Inherits ShellExecutor
        Implements IShell

        Public Overrides ReadOnly Property ShellType As ShellType Implements IShell.ShellType
            Get
                Return ShellType.HexShell
            End Get
        End Property

        Public Overrides Property Bail As Boolean Implements IShell.Bail

        Public Overrides Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
            'Get file path
            Dim FilePath As String = ""
            If ShellArgs.Length > 0 Then
                FilePath = ShellArgs(0)
            Else
                Write(DoTranslation("File not specified. Exiting shell..."), True, ColTypes.Error)
                Bail = True
            End If
            Write(DoTranslation("Please note that editing binary files using this shell is experimental and may lead to data corruption or data loss if not used properly.") + NewLine +
                  DoTranslation("DON'T LAUNCH THE SHELL UNLESS YOU KNOW WHAT YOU'RE DOING!"), True, ColTypes.Warning)

            'Actual shell logic
            While Not Bail
                Try
                    'Open file if not open
                    If HexEdit_FileStream Is Nothing Then
                        Wdbg(DebugLevel.W, "File not open yet. Trying to open {0}...", FilePath)
                        If Not HexEdit_OpenBinaryFile(FilePath) Then
                            Write(DoTranslation("Failed to open file. Exiting shell..."), True, ColTypes.Error)
                            Bail = True
                            Exit While
                        End If
                        HexEdit_AutoSave.Start()
                    End If

                    'See UESHShell.vb for more info
                    SyncLock GetCancelSyncLock(ShellType)
                        'Restore the console state
                        If DefConsoleOut IsNot Nothing Then
                            Console.SetOut(DefConsoleOut)
                        End If

                        'Prepare for prompt
                        Wdbg(DebugLevel.I, "HexEdit_PromptStyle = {0}", HexEdit_PromptStyle)
                        WriteShellPrompt(ShellType)

                        'Raise the event
                        KernelEventManager.RaiseHexShellInitialized()
                    End SyncLock

                    'Prompt for command
                    Dim WrittenCommand As String = ReadLine()
                    If Not (WrittenCommand = Nothing Or WrittenCommand?.StartsWithAnyOf({" ", "#"})) Then
                        KernelEventManager.RaiseHexPreExecuteCommand(WrittenCommand)
                        GetLine(WrittenCommand, False, "", ShellType.HexShell)
                        KernelEventManager.RaiseHexPostExecuteCommand(WrittenCommand)
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

            'Close file
            HexEdit_CloseBinaryFile()
            HexEdit_AutoSave.Stop()
        End Sub

    End Class
End Namespace
