﻿
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
Imports System.Threading
Imports KS.Misc.TextEdit

Namespace Shell.Shells
    Public Class TextShell
        Inherits ShellExecutor
        Implements IShell

        Public Overrides ReadOnly Property ShellType As ShellType Implements IShell.ShellType
            Get
                Return ShellType.TextShell
            End Get
        End Property

        Public Overrides Property Bail As Boolean Implements IShell.Bail

        Public Overrides Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
            'Add handler for text editor shell
            SwitchCancellationHandler(ShellType.TextShell)

            'Get file path
            Dim FilePath As String = ""
            If ShellArgs.Length > 0 Then
                FilePath = ShellArgs(0)
            Else
                TextWriterColor.Write(DoTranslation("File not specified. Exiting shell..."), True, ColTypes.Error)
                Bail = True
            End If

            'Actual shell logic
            While Not Bail
                SyncLock EditorCancelSync
                    'Open file if not open
                    If TextEdit_FileStream Is Nothing Then
                        Wdbg(DebugLevel.W, "File not open yet. Trying to open {0}...", FilePath)
                        If Not TextEdit_OpenTextFile(FilePath) Then
                            TextWriterColor.Write(DoTranslation("Failed to open file. Exiting shell..."), True, ColTypes.Error)
                            Exit While
                        End If
                        TextEdit_AutoSave.Start()
                    End If

                    'Prepare for prompt
                    If DefConsoleOut IsNot Nothing Then
                        Console.SetOut(DefConsoleOut)
                    End If
                    Wdbg(DebugLevel.I, "TextEdit_PromptStyle = {0}", TextEdit_PromptStyle)
                    If TextEdit_PromptStyle = "" Then
                        TextWriterColor.Write("[", False, ColTypes.Gray) : TextWriterColor.Write("{0}{1}", False, ColTypes.UserName, Path.GetFileName(FilePath), If(TextEdit_WasTextEdited(), "*", "")) : TextWriterColor.Write("] > ", False, ColTypes.Gray) : TextWriterColor.Write("", False, InputColor)
                    Else
                        Dim ParsedPromptStyle As String = ProbePlaces(TextEdit_PromptStyle)
                        ParsedPromptStyle.ConvertVTSequences
                        TextWriterColor.Write(ParsedPromptStyle, False, ColTypes.Gray) : TextWriterColor.Write("", False, InputColor)
                    End If

                    'Prompt for command
                    KernelEventManager.RaiseTextShellInitialized()
                    Dim WrittenCommand As String = Console.ReadLine
                    KernelEventManager.RaiseTextPreExecuteCommand(WrittenCommand)
                    GetLine(WrittenCommand, False, "", ShellType.TextShell)
                    KernelEventManager.RaiseTextPostExecuteCommand(WrittenCommand)
                End SyncLock
            End While

            'Close file
            TextEdit_CloseTextFile()
            TextEdit_AutoSave.Abort()
            TextEdit_AutoSave = New Thread(AddressOf TextEdit_HandleAutoSaveTextFile) With {.Name = "Text Edit Autosave Thread"}

            'Remove handler for text editor shell
            SwitchCancellationHandler(LastShellType)
        End Sub

    End Class
End Namespace