
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

Namespace Misc.Writers.ConsoleWriters
    Public Module ListWriterColor
#Region "Dictionary"
        ''' <summary>
        ''' Outputs the list entries into the terminal prompt. It wraps output depending on the kernel settings.
        ''' </summary>
        ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        Public Sub WriteList(Of TKey, TValue)(List As Dictionary(Of TKey, TValue))
            WriteList(List, WrapListOutputs)
        End Sub

        ''' <summary>
        ''' Outputs the list entries into the terminal prompt, and wraps output if needed.
        ''' </summary>
        ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        ''' <param name="Wrap">Wraps the output as needed.</param>
        Public Sub WriteList(Of TKey, TValue)(List As Dictionary(Of TKey, TValue), Wrap As Boolean)
            SyncLock WriteLock
                Try
                    'Variables
                    Dim LinesMade As Integer
                    Dim OldTop As Integer

                    'Try to write list to console
                    OldTop = Console.CursorTop
                    For Each ListEntry As TKey In List.Keys
                        Dim Values As New List(Of Object)
                        If TryCast(List(ListEntry), IEnumerable) IsNot Nothing And TryCast(List(ListEntry), String) Is Nothing Then
                            For Each Value In CType(List(ListEntry), IEnumerable)
                                Values.Add(Value)
                            Next
                            Write("- {0}: ", False, ColTypes.ListEntry, ListEntry) : Write("{0}", True, ColTypes.ListValue, String.Join(", ", Values))
                        Else
                            Write("- {0}: ", False, ColTypes.ListEntry, ListEntry) : Write("{0}", True, ColTypes.ListValue, List(ListEntry))
                        End If
                        If Wrap Then
                            LinesMade += Console.CursorTop - OldTop
                            OldTop = Console.CursorTop
                            If LinesMade = Console.WindowHeight - 1 Then
                                If Console.ReadKey(True).Key = ConsoleKey.Escape Then Exit For
                                LinesMade = 0
                            End If
                        Else
                            If Console.KeyAvailable Then
                                If Console.ReadKey(True).Key = ConsoleKey.Escape Then Exit For
                            End If
                        End If
                    Next
                    If BackgroundColor.PlainSequence = New Color(ConsoleColors.Black).PlainSequence Then Console.ResetColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support.
        ''' </summary>
        ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        ''' <param name="ListKeyColor">A key color.</param>
        ''' <param name="ListValueColor">A value color.</param>
        Public Sub WriteList(Of TKey, TValue)(List As Dictionary(Of TKey, TValue), ListKeyColor As ConsoleColor, ListValueColor As ConsoleColor)
            WriteList(List, ListKeyColor, ListValueColor, WrapListOutputs)
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support.
        ''' </summary>
        ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        ''' <param name="ListKeyColor">A key color.</param>
        ''' <param name="ListValueColor">A value color.</param>
        ''' <param name="Wrap">Wraps the output as needed.</param>
        Public Sub WriteList(Of TKey, TValue)(List As Dictionary(Of TKey, TValue), ListKeyColor As ConsoleColor, ListValueColor As ConsoleColor, Wrap As Boolean)
            SyncLock WriteLock
                Try
                    'Variables
                    Dim LinesMade As Integer
                    Dim OldTop As Integer

                    'Try to write list to console
                    OldTop = Console.CursorTop
                    For Each ListEntry As TKey In List.Keys
                        Dim Values As New List(Of Object)
                        If TryCast(List(ListEntry), IEnumerable) IsNot Nothing And TryCast(List(ListEntry), String) Is Nothing Then
                            For Each Value In CType(List(ListEntry), IEnumerable)
                                Values.Add(Value)
                            Next
                            Write("- {0}: ", False, ListKeyColor, ListEntry) : Write("{0}", True, ListValueColor, String.Join(", ", Values))
                        Else
                            Write("- {0}: ", False, ListKeyColor, ListEntry) : Write("{0}", True, ListValueColor, List(ListEntry))
                        End If
                        If Wrap Then
                            LinesMade += Console.CursorTop - OldTop
                            OldTop = Console.CursorTop
                            If LinesMade = Console.WindowHeight - 1 Then
                                If Console.ReadKey(True).Key = ConsoleKey.Escape Then Exit For
                                LinesMade = 0
                            End If
                        Else
                            If Console.KeyAvailable Then
                                If Console.ReadKey(True).Key = ConsoleKey.Escape Then Exit For
                            End If
                        End If
                    Next
                    If BackgroundColor.PlainSequence = New Color(ConsoleColors.Black).PlainSequence Then Console.ResetColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support.
        ''' </summary>
        ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        ''' <param name="ListKeyColor">A key color.</param>
        ''' <param name="ListValueColor">A value color.</param>
        Public Sub WriteList(Of TKey, TValue)(List As Dictionary(Of TKey, TValue), ListKeyColor As Color, ListValueColor As Color)
            WriteList(List, ListKeyColor, ListValueColor, WrapListOutputs)
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support.
        ''' </summary>
        ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        ''' <param name="ListKeyColor">A key color.</param>
        ''' <param name="ListValueColor">A value color.</param>
        ''' <param name="Wrap">Wraps the output as needed.</param>
        Public Sub WriteList(Of TKey, TValue)(List As Dictionary(Of TKey, TValue), ListKeyColor As Color, ListValueColor As Color, Wrap As Boolean)
            SyncLock WriteLock
                Try
                    'Variables
                    Dim LinesMade As Integer
                    Dim OldTop As Integer

                    'Try to write list to console
                    OldTop = Console.CursorTop
                    For Each ListEntry As TKey In List.Keys
                        Dim Values As New List(Of Object)
                        If TryCast(List(ListEntry), IEnumerable) IsNot Nothing And TryCast(List(ListEntry), String) Is Nothing Then
                            For Each Value In CType(List(ListEntry), IEnumerable)
                                Values.Add(Value)
                            Next
                            Write("- {0}: ", False, ListKeyColor, ListEntry) : Write("{0}", True, ListValueColor, String.Join(", ", Values))
                        Else
                            Write("- {0}: ", False, ListKeyColor, ListEntry) : Write("{0}", True, ListValueColor, List(ListEntry))
                        End If
                        If Wrap Then
                            LinesMade += Console.CursorTop - OldTop
                            OldTop = Console.CursorTop
                            If LinesMade = Console.WindowHeight - 1 Then
                                If Console.ReadKey(True).Key = ConsoleKey.Escape Then Exit For
                                LinesMade = 0
                            End If
                        Else
                            If Console.KeyAvailable Then
                                If Console.ReadKey(True).Key = ConsoleKey.Escape Then Exit For
                            End If
                        End If
                    Next
                    If BackgroundColor.PlainSequence = New Color(ConsoleColors.Black).PlainSequence Then Console.ResetColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub
#End Region
#Region "Enumerables"
        ''' <summary>
        ''' Outputs the list entries into the terminal prompt. It wraps output depending on the kernel settings.
        ''' </summary>
        ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        Public Sub WriteList(Of T)(List As IEnumerable(Of T))
            WriteList(List, WrapListOutputs)
        End Sub

        ''' <summary>
        ''' Outputs the list entries into the terminal prompt, and wraps output if needed.
        ''' </summary>
        ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        ''' <param name="Wrap">Wraps the output as needed.</param>
        Public Sub WriteList(Of T)(List As IEnumerable(Of T), Wrap As Boolean)
            SyncLock WriteLock
                Try
                    'Variables
                    Dim LinesMade As Integer
                    Dim OldTop As Integer
                    Dim EntryNumber As Integer = 1

                    'Try to write list to console
                    OldTop = Console.CursorTop
                    For Each ListEntry As T In List
                        Dim Values As New List(Of Object)
                        If TryCast(ListEntry, IEnumerable) IsNot Nothing And TryCast(ListEntry, String) Is Nothing Then
                            For Each Value In CType(ListEntry, IEnumerable)
                                Values.Add(Value)
                            Next
                            Write("- [{0}] ", False, ColTypes.ListEntry, EntryNumber) : Write("{0}", True, ColTypes.ListValue, String.Join(", ", Values))
                        Else
                            Write("- [{0}] ", False, ColTypes.ListEntry, EntryNumber) : Write("{0}", True, ColTypes.ListValue, ListEntry)
                        End If
                        EntryNumber += 1
                        If Wrap Then
                            LinesMade += Console.CursorTop - OldTop
                            OldTop = Console.CursorTop
                            If LinesMade = Console.WindowHeight - 1 Then
                                If Console.ReadKey(True).Key = ConsoleKey.Escape Then Exit For
                                LinesMade = 0
                            End If
                        Else
                            If Console.KeyAvailable Then
                                If Console.ReadKey(True).Key = ConsoleKey.Escape Then Exit For
                            End If
                        End If
                    Next
                    If BackgroundColor.PlainSequence = New Color(ConsoleColors.Black).PlainSequence Then Console.ResetColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support.
        ''' </summary>
        ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        ''' <param name="ListKeyColor">A key color.</param>
        ''' <param name="ListValueColor">A value color.</param>
        Public Sub WriteList(Of T)(List As IEnumerable(Of T), ListKeyColor As ConsoleColor, ListValueColor As ConsoleColor)
            WriteList(List, ListKeyColor, ListValueColor, WrapListOutputs)
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support.
        ''' </summary>
        ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        ''' <param name="ListKeyColor">A key color.</param>
        ''' <param name="ListValueColor">A value color.</param>
        ''' <param name="Wrap">Wraps the output as needed.</param>
        Public Sub WriteList(Of T)(List As IEnumerable(Of T), ListKeyColor As ConsoleColor, ListValueColor As ConsoleColor, Wrap As Boolean)
            SyncLock WriteLock
                Try
                    'Variables
                    Dim LinesMade As Integer
                    Dim OldTop As Integer
                    Dim EntryNumber As Integer = 1

                    'Try to write list to console
                    OldTop = Console.CursorTop
                    For Each ListEntry As T In List
                        Dim Values As New List(Of Object)
                        If TryCast(ListEntry, IEnumerable) IsNot Nothing And TryCast(ListEntry, String) Is Nothing Then
                            For Each Value In CType(ListEntry, IEnumerable)
                                Values.Add(Value)
                            Next
                            Write("- [{0}] ", False, ListKeyColor, EntryNumber) : Write("{0}", True, ListValueColor, String.Join(", ", Values))
                        Else
                            Write("- [{0}] ", False, ListKeyColor, EntryNumber) : Write("{0}", True, ListValueColor, ListEntry)
                        End If
                        EntryNumber += 1
                        If Wrap Then
                            LinesMade += Console.CursorTop - OldTop
                            OldTop = Console.CursorTop
                            If LinesMade = Console.WindowHeight - 1 Then
                                If Console.ReadKey(True).Key = ConsoleKey.Escape Then Exit For
                                LinesMade = 0
                            End If
                        Else
                            If Console.KeyAvailable Then
                                If Console.ReadKey(True).Key = ConsoleKey.Escape Then Exit For
                            End If
                        End If
                    Next
                    If BackgroundColor.PlainSequence = New Color(ConsoleColors.Black).PlainSequence Then Console.ResetColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support.
        ''' </summary>
        ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        ''' <param name="ListKeyColor">A key color.</param>
        ''' <param name="ListValueColor">A value color.</param>
        Public Sub WriteList(Of T)(List As IEnumerable(Of T), ListKeyColor As Color, ListValueColor As Color)
            WriteList(List, ListKeyColor, ListValueColor, WrapListOutputs)
        End Sub

        ''' <summary>
        ''' Outputs the text into the terminal prompt with custom color support.
        ''' </summary>
        ''' <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        ''' <param name="ListKeyColor">A key color.</param>
        ''' <param name="ListValueColor">A value color.</param>
        ''' <param name="Wrap">Wraps the output as needed.</param>
        Public Sub WriteList(Of T)(List As IEnumerable(Of T), ListKeyColor As Color, ListValueColor As Color, Wrap As Boolean)
            SyncLock WriteLock
                Try
                    'Variables
                    Dim LinesMade As Integer
                    Dim OldTop As Integer
                    Dim EntryNumber As Integer = 1

                    'Try to write list to console
                    OldTop = Console.CursorTop
                    For Each ListEntry As T In List
                        Dim Values As New List(Of Object)
                        If TryCast(ListEntry, IEnumerable) IsNot Nothing And TryCast(ListEntry, String) Is Nothing Then
                            For Each Value In CType(ListEntry, IEnumerable)
                                Values.Add(Value)
                            Next
                            Write("- {0}: ", False, ListKeyColor, EntryNumber) : Write("{0}", True, ListValueColor, String.Join(", ", Values))
                        Else
                            Write("- {0}: ", False, ListKeyColor, EntryNumber) : Write("{0}", True, ListValueColor, ListEntry)
                        End If
                        EntryNumber += 1
                        If Wrap Then
                            LinesMade += Console.CursorTop - OldTop
                            OldTop = Console.CursorTop
                            If LinesMade = Console.WindowHeight - 1 Then
                                If Console.ReadKey(True).Key = ConsoleKey.Escape Then Exit For
                                LinesMade = 0
                            End If
                        Else
                            If Console.KeyAvailable Then
                                If Console.ReadKey(True).Key = ConsoleKey.Escape Then Exit For
                            End If
                        End If
                    Next
                    If BackgroundColor.PlainSequence = New Color(ConsoleColors.Black).PlainSequence Then Console.ResetColor()
                Catch ex As Exception When Not ex.GetType.Name = "ThreadInterruptedException"
                    WStkTrc(ex)
                    KernelError(KernelErrorLevel.C, False, 0, DoTranslation("There is a serious error when printing text."), ex)
                End Try
            End SyncLock
        End Sub
#End Region
    End Module
End Namespace