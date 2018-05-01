
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Module Groups

    'Variables
    Public adminList As New Dictionary(Of String, Boolean)()         'Users that are allowed to have administrative access.
    Public disabledList As New Dictionary(Of String, Boolean)()      'Users that are unable to login

    Sub permission(ByVal type As String, ByVal allowed As Boolean, ByVal username As String, ByVal mode As String, Optional ByVal quiet As Boolean = False)

        'Variables
        Dim DoneFlag As Boolean = False
        On Error GoTo permBug

        'Adds user into permission lists.

        If (mode = "Add") Then
            If (allowed = True Or allowed = False) Then
                For Each availableUsers As String In userword.Keys.ToArray
                    If (username = availableUsers) Then
                        If (type = "Admin") Then
                            DoneFlag = True
                            adminList.Add(username, allowed)
                            If (quiet = False) Then
                                System.Console.WriteLine("The user {0} has been added to the admin list with the allowance being {1}.", username, allowed)
                            End If
                        ElseIf (type = "Disabled") Then
                            DoneFlag = True
                            disabledList.Add(username, allowed)
                            If (quiet = False) Then
                                System.Console.WriteLine("The user {0} has been added to the disabled list with the allowance being {1}.", username, allowed)
                            End If
                        Else
                            If (quiet = False) Then
                                System.Console.WriteLine("Failed to add user into permission lists: invalid type {0}", type)
                            End If
                            Exit Sub
                        End If
                    End If
                Next
            Else
                If (quiet = False) Then
                    System.Console.WriteLine("Failed to add user into permission lists: invalid allowance {0}", allowed)
                End If
                Exit Sub
            End If
            If (DoneFlag = False And quiet = False) Then
                System.Console.WriteLine("Failed to add user into permission lists: invalid user {0}", username)
            End If
        ElseIf (mode = "Remove") Then
            For Each availableUsers As String In userword.Keys.ToArray
                If (username = availableUsers) Then
                    If (type = "Admin") Then
                        DoneFlag = True
                        adminList.Remove(username)
                        If (quiet = False) Then
                            System.Console.WriteLine("The user {0} has been removed from the admin list.", username)
                        End If
                    ElseIf (type = "Disabled") Then
                        DoneFlag = True
                        disabledList.Remove(username)
                        If (quiet = False) Then
                            System.Console.WriteLine("The user {0} has been removed from the disabled list.", username)
                        End If
                    Else
                        If (quiet = False) Then
                            System.Console.WriteLine("Failed to remove user from permission lists: invalid type {0}", type)
                        End If
                        Exit Sub
                    End If
                End If
            Next
            If (DoneFlag = False And quiet = False) Then
                System.Console.WriteLine("Failed to remove user from permission lists: invalid user {0}", username)
            End If
        Else
            If (quiet = False) Then
                System.Console.WriteLine("You have found a bug in the permission system: invalid mode {0}", mode)
            End If
        End If
        Exit Sub
permBug:
        System.Console.WriteLine("You have either found a bug, or the permission you tried to add or remove is already done, or other error." + vbNewLine + _
                                 "Error {0}: {1}", Err.Number, Err.Description)

    End Sub

    Sub permissionEdit(ByVal username As String, ByVal type As String, ByVal allowed As Boolean)

        'Variables
        Dim DoneFlag As Boolean = False
        On Error GoTo permBug

        'Permission that is about to be edited for the user
        If (allowed = True Or allowed = False) Then
            For Each availableUsers As String In userword.Keys.ToArray
                If (username = availableUsers) Then
                    If (type = "Admin") Then
                        DoneFlag = True
                        adminList(username) = allowed
                        System.Console.WriteLine("The user {0} has been edited in the admin list with the allowance being {1}.", username, allowed)
                    ElseIf (type = "Disabled") Then
                        DoneFlag = True
                        disabledList(username) = allowed
                        System.Console.WriteLine("The user {0} has been edited in the disabled list with the allowance being {1}.", username, allowed)
                    Else
                        System.Console.WriteLine("Failed to edit user in the permission lists: invalid type {0}", type)
                        Exit Sub
                    End If
                End If
            Next
        Else
            System.Console.WriteLine("Failed to edit user in the permission lists: invalid allowance {0}", allowed)
            Exit Sub
        End If
        If (DoneFlag = False) Then
            System.Console.WriteLine("Failed to edit user in the permission lists: invalid user {0}", username)
        End If
        Exit Sub
permBug:
        System.Console.WriteLine("You have either found a bug, or the permission you tried to edit failed to edit." + vbNewLine + _
                                 "Error {0}: {1}", Err.Number, Err.Description)

    End Sub

    Sub permissionEditForNewUser(ByVal oldName As String, ByVal username As String)

        'Error Handler
        On Error Resume Next

        'Edit username (continuation for changeName() sub)
        If (adminList.ContainsKey(oldName) = True And disabledList.ContainsKey(oldName) = True) Then
            Dim temporary1 As Boolean = adminList(oldName)
            Dim temporary2 As Boolean = disabledList(oldName)
            adminList.Remove(oldName)
            disabledList.Remove(oldName)
            adminList.Add(username, temporary1)
            disabledList.Add(username, temporary2)
        End If

    End Sub

    Sub permissionPrompt()

        'Variables
        Dim DoneFlag As Boolean = False
        On Error Resume Next

        'Asks for username, and then permission prompts.
        System.Console.Write("Username to be managed: ")
        System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
        Dim answermuser = System.Console.ReadLine()
        System.Console.ResetColor()
        For Each userPerm As String In userword.Keys.ToArray
            If (userPerm = answermuser And answermuser <> "root") Then
                System.Console.Write("Type (Admin / Disabled): ")
                System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                Dim answermtype = System.Console.ReadLine()
                System.Console.ResetColor()
                If (answermtype = "Admin" Or answermtype = "Disabled") Then
                    System.Console.Write("Is the user allowed? <y/n> ")
                    System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                    Dim answermallow = System.Console.ReadLine()
                    System.Console.ResetColor()
                    If (answermallow = "y" Or answermallow = "n") Then
                        System.Console.Write("Add or remove? <Add/Remove> ")
                        System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                        Dim answermaddremove = System.Console.ReadLine()
                        System.Console.ResetColor()
                        If (answermaddremove = "Add" Or answermaddremove = "Remove") Then
                            If (answermallow = "y") Then
                                permission(answermtype, True, answermuser, answermaddremove)
                                DoneFlag = True
                            ElseIf (answermallow = "n") Then
                                permission(answermtype, False, answermuser, answermaddremove)
                                DoneFlag = True
                            End If
                        Else
                            System.Console.WriteLine("Invalid choice")
                        End If
                    Else
                        System.Console.WriteLine("Invalid choice")
                    End If
                Else
                    System.Console.WriteLine("Type {0} not found.", answermtype)
                End If
            ElseIf (userPerm = answermuser And answermuser = "root") Then
                System.Console.WriteLine("User {0}'s permission cannot be changed.", answermuser)
                Exit Sub
            End If
        Next
        If (DoneFlag = False) Then
            System.Console.WriteLine("User {0} not found.", answermuser)
        End If

    End Sub

    Sub permissionEditingPrompt()

        'Variables
        Dim DoneFlag As Boolean = False
        On Error Resume Next

        'Asks for username, and then permission prompts.
        System.Console.Write("Username to be managed: ")
        System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
        Dim answermuser = System.Console.ReadLine()
        System.Console.ResetColor()
        For Each userPerm As String In userword.Keys.ToArray
            If (userPerm = answermuser And answermuser <> "root") Then
                System.Console.Write("Type (Admin / Disabled): ")
                System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                Dim answermtype = System.Console.ReadLine()
                System.Console.ResetColor()
                If (answermtype = "Admin" Or answermtype = "Disabled") Then
                    System.Console.Write("Is the user allowed? <y/n> ")
                    System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                    Dim answermallow = System.Console.ReadLine()
                    System.Console.ResetColor()
                    If (answermallow = "y") Then
                        permissionEdit(answermuser, answermtype, True)
                        DoneFlag = True
                    ElseIf (answermallow = "n") Then
                        permissionEdit(answermuser, answermtype, False)
                        DoneFlag = True
                    Else
                        System.Console.WriteLine("Invalid choice")
                    End If
                Else
                    System.Console.WriteLine("Type {0} not found.", answermtype)
                End If
            ElseIf (userPerm = answermuser And answermuser = "root") Then
                System.Console.WriteLine("User {0}'s permission cannot be changed.", answermuser)
                Exit Sub
            End If
        Next
        If (DoneFlag = False) Then
            System.Console.WriteLine("User {0} not found.", answermuser)
        End If

    End Sub

End Module
