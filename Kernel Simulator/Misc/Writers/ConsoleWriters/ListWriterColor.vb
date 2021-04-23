
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Imports System.Console

Public Module ListWriterColor

    ''' <summary>
    ''' Outputs the list entries into the terminal prompt. It wraps output depending on the kernel settings.
    ''' </summary>
    ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
    Public Sub WriteList(Of TKey, TValue)(ByVal List As Dictionary(Of TKey, TValue))
        WriteList(List, WrapListOutputs)
    End Sub

    ''' <summary>
    ''' Outputs the list entries into the terminal prompt, and wraps output if needed.
    ''' </summary>
    ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
    ''' <param name="Wrap">Wraps the output as needed.</param>
    Public Sub WriteList(Of TKey, TValue)(ByVal List As Dictionary(Of TKey, TValue), ByVal Wrap As Boolean)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Try
                'Variables
                Dim LinesMade As Integer
                Dim OldTop As Integer

                'Try to write list to console
                OldTop = CursorTop
                For Each ListEntry As TKey In List.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, ListEntry) : W("{0}", True, ColTypes.ListValue, List(ListEntry))
                    If Wrap Then
                        LinesMade += CursorTop - OldTop
                        OldTop = CursorTop
                        If LinesMade = WindowHeight - 1 Then
                            ReadKey(True)
                            LinesMade = 0
                        End If
                    End If
                Next
                If backgroundColor = ConsoleColors.Black Then ResetColor()
            Catch ex As Exception
                WStkTrc(ex)
                KernelError("C", False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with custom color support.
    ''' </summary>
    ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
    ''' <param name="ListKeyColor">A key color.</param>
    ''' <param name="ListValueColor">A value color.</param>
    Public Sub WriteList(Of TKey, TValue)(ByVal List As Dictionary(Of TKey, TValue), ByVal ListKeyColor As ConsoleColor, ByVal ListValueColor As ConsoleColor)
        WriteList(List, ListKeyColor, ListValueColor, WrapListOutputs)
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with custom color support.
    ''' </summary>
    ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
    ''' <param name="ListKeyColor">A key color.</param>
    ''' <param name="ListValueColor">A value color.</param>
    ''' <param name="Wrap">Wraps the output as needed.</param>
    Public Sub WriteList(Of TKey, TValue)(ByVal List As Dictionary(Of TKey, TValue), ByVal ListKeyColor As ConsoleColor, ByVal ListValueColor As ConsoleColor, ByVal Wrap As Boolean)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Dim esc As Char = GetEsc()
            Try
                'Variables
                Dim LinesMade As Integer
                Dim OldTop As Integer

                'Try to write list to console
                OldTop = CursorTop
                For Each ListEntry As TKey In List.Keys
                    WriteC16("- {0}: ", False, ListKeyColor, ListEntry) : WriteC16("{0}", True, ListValueColor, List(ListEntry))
                    If Wrap Then
                        LinesMade += CursorTop - OldTop
                        OldTop = CursorTop
                        If LinesMade = WindowHeight - 1 Then
                            ReadKey(True)
                            LinesMade = 0
                        End If
                    End If
                Next
                If backgroundColor = ConsoleColors.Black Then ResetColor()
            Catch ex As Exception
                WStkTrc(ex)
                KernelError("C", False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with custom color support.
    ''' </summary>
    ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
    ''' <param name="ListKeyColor">A key color.</param>
    ''' <param name="ListValueColor">A value color.</param>
    Public Sub WriteList(Of TKey, TValue)(ByVal List As Dictionary(Of TKey, TValue), ByVal ListKeyColor As Color, ByVal ListValueColor As Color)
        WriteList(List, ListKeyColor, ListValueColor, WrapListOutputs)
    End Sub

    ''' <summary>
    ''' Outputs the text into the terminal prompt with custom color support.
    ''' </summary>
    ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
    ''' <param name="ListKeyColor">A key color.</param>
    ''' <param name="ListValueColor">A value color.</param>
    ''' <param name="Wrap">Wraps the output as needed.</param>
    Public Sub WriteList(Of TKey, TValue)(ByVal List As Dictionary(Of TKey, TValue), ByVal ListKeyColor As Color, ByVal ListValueColor As Color, ByVal Wrap As Boolean)
#If Not NOWRITELOCK Then
        SyncLock WriteLock
#End If
            Dim esc As Char = GetEsc()
            Try
                'Variables
                Dim LinesMade As Integer
                Dim OldTop As Integer

                'Try to write list to console
                OldTop = CursorTop
                For Each ListEntry As TKey In List.Keys
                    WriteC("- {0}: ", False, ListKeyColor, ListEntry) : WriteC("{0}", True, ListValueColor, List(ListEntry))
                    If Wrap Then
                        LinesMade += CursorTop - OldTop
                        OldTop = CursorTop
                        If LinesMade = WindowHeight - 1 Then
                            ReadKey(True)
                            LinesMade = 0
                        End If
                    End If
                Next
                If BackgroundColor = ConsoleColors.Black Then ResetColor()
            Catch ex As Exception
                WStkTrc(ex)
                KernelError("C", False, 0, DoTranslation("There is a serious error when printing text."), ex)
            End Try
#If Not NOWRITELOCK Then
        End SyncLock
#End If
    End Sub

End Module
