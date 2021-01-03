
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

Imports System.IO

Public Module PermissionManagement

    ''' <summary>
    ''' This enumeration lists all permission types.
    ''' </summary>
    Public Enum PermissionType As Integer
        Administrator = 1
        Disabled
    End Enum

    ''' <summary>
    ''' It specifies whether or not to allow permission
    ''' </summary>
    Public Enum PermissionManagementMode As Integer
        Allow = 1
        Disallow
    End Enum

    ''' <summary>
    ''' Manages permissions
    ''' </summary>
    ''' <param name="PermType">A type of permission</param>
    ''' <param name="Username">A specified username</param>
    ''' <param name="PermissionMode">Whether to allow or disallow a specified type for a user</param>
    Sub Permission(ByVal PermType As PermissionType, ByVal Username As String, ByVal PermissionMode As PermissionManagementMode)

        'Adds user into permission lists.
        Try
            Wdbg("I", "Mode: {0}", PermissionMode)
            If PermissionMode = PermissionManagementMode.Allow Then
                AddPermission(PermType, Username)
                If PermType = PermissionType.Administrator Then
                    W(DoTranslation("The user {0} has been added to the admin list.", currentLang), True, ColTypes.Neutral, Username)
                ElseIf PermType = PermissionType.Disabled Then
                    W(DoTranslation("The user {0} has been added to the disabled list.", currentLang), True, ColTypes.Neutral, Username)
                End If
            ElseIf PermissionMode = PermissionManagementMode.Disallow Then
                RemovePermission(PermType, Username)
                If PermType = PermissionType.Administrator Then
                    W(DoTranslation("The user {0} has been removed from the admin list.", currentLang), True, ColTypes.Neutral, Username)
                ElseIf PermType = PermissionType.Disabled Then
                    W(DoTranslation("The user {0} has been removed from the disabled list.", currentLang), True, ColTypes.Neutral, Username)
                End If
            Else
                Wdbg("W", "Mode is invalid")
                W(DoTranslation("You have found a bug in the permission system: invalid mode {0}", currentLang), True, ColTypes.Err, PermissionMode)
            End If
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("You have either found a bug, or the permission you tried to add or remove is already done, or other error.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Err, Err.Number, ex.Message, ex.StackTrace)
                WStkTrc(ex)
            Else
                W(DoTranslation("You have either found a bug, or the permission you tried to add or remove is already done, or other error.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Err, Err.Number, ex.Message)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Adds user to one of permission types
    ''' </summary>
    ''' <param name="PermType">Whether it be Admin or Disabled</param>
    ''' <param name="Username">A username to be managed</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="EventsAndExceptions.PermissionManagementException"></exception>
    Public Function AddPermission(ByVal PermType As PermissionType, ByVal Username As String)
        'Open users.csv file
        Dim UsersLines As List(Of String) = File.ReadAllLines(paths("Users")).ToList
        Dim UserLine As String() = {}
        For i As Integer = 0 To UsersLines.Count - 1
            If UsersLines(i).StartsWith($"{Username},") Then
                UserLine = UsersLines(i).Split(",")
                Exit For
            End If
        Next

        'Adds user into permission lists.
        If userword.Keys.ToArray.Contains(Username) Then
            Wdbg("I", "Type is {0}", PermType)
            If PermType = PermissionType.Administrator Then
                adminList(Username) = True
                Wdbg("I", "User {0} allowed (Admin): {1}", Username, adminList(Username))
            ElseIf PermType = PermissionType.Disabled Then
                disabledList(Username) = True
                Wdbg("I", "User {0} allowed (Disabled): {1}", Username, disabledList(Username))
            Else
                Wdbg("W", "Type is invalid")
                Throw New EventsAndExceptions.PermissionManagementException(DoTranslation("Failed to add user into permission lists: invalid type {0}", currentLang).FormatString(PermType))
                Return False
            End If
        Else
            Wdbg("W", "User {0} not found on list", Username)
            Throw New EventsAndExceptions.PermissionManagementException(DoTranslation("Failed to add user into permission lists: invalid user {0}", currentLang).FormatString(Username))
            Return False
        End If

        'Save changes
        For i As Integer = 0 To UsersLines.Count - 1
            If UsersLines(i).StartsWith($"{Username},") Then
                UserLine(2) = adminList(Username)
                UserLine(3) = disabledList(Username)
                UsersLines(i) = UserLine.Join(",")
                Exit For
            End If
        Next
        File.WriteAllLines(paths("Users"), UsersLines)
        Return True
    End Function

    ''' <summary>
    ''' Removes user from one of permission types
    ''' </summary>
    ''' <param name="PermType">Whether it be Admin or Disabled</param>
    ''' <param name="Username">A username to be managed</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="EventsAndExceptions.PermissionManagementException"></exception>
    Public Function RemovePermission(ByVal PermType As PermissionType, ByVal Username As String)
        'Open users.csv file
        Dim UsersLines As List(Of String) = File.ReadAllLines(paths("Users")).ToList
        Dim UserLine As String() = {}
        For i As Integer = 0 To UsersLines.Count - 1
            If UsersLines(i).StartsWith($"{Username},") Then
                UserLine = UsersLines(i).Split(",")
                Exit For
            End If
        Next

        'Adds user into permission lists.
        If userword.Keys.ToArray.Contains(Username) And Username <> signedinusrnm Then
            Wdbg("I", "Type is {0}", PermType)
            If PermType = PermissionType.Administrator Then
                Wdbg("I", "User {0} allowed (Admin): {1}", Username, adminList(Username))
                adminList(Username) = False
            ElseIf PermType = PermissionType.Disabled Then
                Wdbg("I", "User {0} allowed (Disabled): {1}", Username, disabledList(Username))
                disabledList(Username) = False
            Else
                Wdbg("W", "Type is invalid")
                Throw New EventsAndExceptions.PermissionManagementException(DoTranslation("Failed to remove user from permission lists: invalid type {0}", currentLang).FormatString(PermType))
                Return False
            End If
        ElseIf Username = signedinusrnm Then
            Throw New EventsAndExceptions.PermissionManagementException(DoTranslation("You are already logged in.", currentLang))
            Return False
        Else
            Wdbg("W", "User {0} not found on list", Username)
            Throw New EventsAndExceptions.PermissionManagementException(DoTranslation("Failed to remove user from permission lists: invalid user {0}", currentLang).FormatString(Username))
            Return False
        End If

        'Save changes
        For i As Integer = 0 To UsersLines.Count - 1
            If UsersLines(i).StartsWith($"{Username},") Then
                UserLine(2) = adminList(Username)
                UserLine(3) = disabledList(Username)
                UsersLines(i) = UserLine.Join(",")
                Exit For
            End If
        Next
        File.WriteAllLines(paths("Users"), UsersLines)
        Return True
    End Function

    ''' <summary>
    ''' Edits the permission database for new user name
    ''' </summary>
    ''' <param name="OldName">Old username</param>
    ''' <param name="Username">New username</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="EventsAndExceptions.PermissionManagementException"></exception>
    Public Function PermissionEditForNewUser(ByVal OldName As String, ByVal Username As String) As Boolean
        'Edit username
        If adminList.ContainsKey(OldName) = True And disabledList.ContainsKey(OldName) = True Then
            Try
                'Store permissions
                Dim Temporary1 As Boolean = adminList(OldName)
                Dim Temporary2 As Boolean = disabledList(OldName)

                'Remove old user entry
                Wdbg("I", "Removing {0} from Admin List", OldName)
                adminList.Remove(OldName)
                Wdbg("I", "Removing {0} from Disabled List", OldName)
                disabledList.Remove(OldName)

                'Add new user entry
                adminList.Add(Username, Temporary1)
                Wdbg("I", "Added {0} to Admin List with value of {1}", Username, Temporary1)
                disabledList.Add(Username, Temporary2)
                Wdbg("I", "Added {0} to Disabled List with value of {1}", Username, Temporary2)
                Return True
            Catch ex As Exception
                WStkTrc(ex)
                Throw New EventsAndExceptions.PermissionManagementException(DoTranslation("You have either found a bug, or the permission you tried to edit for a new user has failed.", currentLang) + vbNewLine +
                                                                            DoTranslation("Error {0}: {1}", currentLang).FormatString(Err.Number, ex.Message))
            End Try
        Else
            Throw New EventsAndExceptions.PermissionManagementException(DoTranslation("One of the permission lists doesn't contain username {0}.", currentLang))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Initializes permissions for a new user with default settings
    ''' </summary>
    ''' <param name="NewUser">A new user name</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="EventsAndExceptions.PermissionManagementException"></exception>
    Public Function InitPermissionsForNewUser(ByVal NewUser As String) As Boolean
        Try
            'Initialize permissions locally
            If Not adminList.ContainsKey(NewUser) Then adminList.Add(NewUser, False)
            If Not disabledList.ContainsKey(NewUser) Then disabledList.Add(NewUser, False)

            'Initialize permissions globally
            Dim UsersLines As List(Of String) = File.ReadAllLines(paths("Users")).ToList
            For i As Integer = 0 To UsersLines.Count - 1
                If UsersLines(i).StartsWith($"{NewUser},") And UsersLines(i).AllIndexesOf(",").Count = 1 Then
                    UsersLines(i) = UsersLines(i) + ",False,False"
                    Exit For
                End If
            Next
            File.WriteAllLines(paths("Users"), UsersLines)
            Return True
        Catch ex As Exception
            WStkTrc(ex)
            Throw New EventsAndExceptions.PermissionManagementException(DoTranslation("Failed to initialize permissions for user {0}: {1}", currentLang).FormatString(NewUser, ex.Message))
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Loads permissions for all users
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="EventsAndExceptions.PermissionManagementException"></exception>
    Public Function LoadPermissions() As Boolean
        Try
            Dim UsersLines As List(Of String) = File.ReadAllLines(paths("Users")).ToList
            Dim UserLine() As String
            For i As Integer = 0 To UsersLines.Count - 1
                UserLine = UsersLines(i).Split(",")
                Dim UserName As String = UserLine(0)
                Dim AdminEnabled As String = UserLine(2)
                Dim UserDisabled As String = UserLine(3)
                adminList(UserName) = CType(AdminEnabled, Boolean)
                disabledList(UserName) = CType(UserDisabled, Boolean)
            Next
            File.WriteAllLines(paths("Users"), UsersLines)
            Return True
        Catch ex As Exception
            WStkTrc(ex)
            Throw New EventsAndExceptions.PermissionManagementException(DoTranslation("Failed to load permissions from file: {0}", currentLang).FormatString(ex.Message))
        End Try
        Return False
    End Function

End Module
