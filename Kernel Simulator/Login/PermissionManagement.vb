
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
Imports Newtonsoft.Json.Linq

Namespace Login
    Public Module PermissionManagement

        Friend UserPermissions As New Dictionary(Of String, PermissionType)

        ''' <summary>
        ''' This enumeration lists all permission types.
        ''' </summary>
        Public Enum PermissionType As Integer
            ''' <summary>
            ''' User has no permissions
            ''' </summary>
            None = 0
            ''' <summary>
            ''' This user is an administrator
            ''' </summary>
            Administrator = 1
            ''' <summary>
            ''' This user is disabled
            ''' </summary>
            Disabled = 2
            ''' <summary>
            ''' This user doesn't show in the available users list
            ''' </summary>
            Anonymous = 4
        End Enum

        ''' <summary>
        ''' It specifies whether or not to allow permission
        ''' </summary>
        Public Enum PermissionManagementMode As Integer
            ''' <summary>
            ''' Adds the permission to the user properties
            ''' </summary>
            Allow = 1
            ''' <summary>
            ''' Removes the permission from the user properties
            ''' </summary>
            Disallow
        End Enum

        ''' <summary>
        ''' Manages permissions
        ''' </summary>
        ''' <param name="PermType">A type of permission</param>
        ''' <param name="Username">A specified username</param>
        ''' <param name="PermissionMode">Whether to allow or disallow a specified type for a user</param>
        Sub Permission(PermType As PermissionType, Username As String, PermissionMode As PermissionManagementMode)

            'Adds user into permission lists.
            Try
                Wdbg(DebugLevel.I, "Mode: {0}", PermissionMode)
                If PermissionMode = PermissionManagementMode.Allow Then
                    AddPermission(PermType, Username)
                    Write(DoTranslation("The user {0} has been added to the ""{1}"" list."), True, ColTypes.Neutral, Username, PermType.ToString)
                ElseIf PermissionMode = PermissionManagementMode.Disallow Then
                    RemovePermission(PermType, Username)
                    Write(DoTranslation("The user {0} has been removed from the ""{1}"" list."), True, ColTypes.Neutral, Username, PermType.ToString)
                Else
                    Wdbg(DebugLevel.W, "Mode is invalid")
                    Write(DoTranslation("Invalid mode {0}"), True, ColTypes.Error, PermissionMode)
                End If
            Catch ex As Exception
                Write(DoTranslation("You have either found a bug, or the permission you tried to add or remove is already done, or other error.") + NewLine +
              DoTranslation("Error {0}: {1}"), True, ColTypes.Error, ex.GetType.FullName, ex.Message)
                WStkTrc(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Adds user to one of permission types
        ''' </summary>
        ''' <param name="PermType">Whether it be Admin or Disabled</param>
        ''' <param name="Username">A username to be managed</param>
        ''' <exception cref="Exceptions.PermissionManagementException"></exception>
        Public Sub AddPermission(PermType As PermissionType, Username As String)
            'Sets the required permissions to false.
            If Users.Keys.ToArray.Contains(Username) Then
                Wdbg(DebugLevel.I, "Type is {0}", PermType)
                Select Case PermType
                    Case PermissionType.Administrator
                        UserPermissions(Username) += PermissionType.Administrator
                    Case PermissionType.Disabled
                        UserPermissions(Username) += PermissionType.Disabled
                    Case PermissionType.Anonymous
                        UserPermissions(Username) += PermissionType.Anonymous
                    Case Else
                        Wdbg(DebugLevel.W, "Type is invalid")
                        Throw New Exceptions.PermissionManagementException(DoTranslation("Failed to add user into permission lists: invalid type {0}"), PermType)
                End Select
                Wdbg(DebugLevel.I, "User {0} permission added; value is now: {1}", Username, UserPermissions(Username))
            Else
                Wdbg(DebugLevel.W, "User {0} not found on list", Username)
                Throw New Exceptions.PermissionManagementException(DoTranslation("Failed to add user into permission lists: invalid user {0}"), Username)
            End If

            'Save changes
            For Each UserToken As JObject In UsersToken
                If UserToken("username").ToString = Username Then
                    If Not CType(UserToken("permissions"), JArray).ToObject(GetType(List(Of String))).Contains(PermType.ToString) Then
                        CType(UserToken("permissions"), JArray).Add(PermType.ToString)
                    End If
                End If
            Next
            File.WriteAllText(GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UsersToken, Formatting.Indented))
        End Sub

        ''' <summary>
        ''' Adds user to one of permission types
        ''' </summary>
        ''' <param name="PermType">Whether it be Admin or Disabled</param>
        ''' <param name="Username">A username to be managed</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="Exceptions.PermissionManagementException"></exception>
        Public Function TryAddPermission(PermType As PermissionType, Username As String) As Boolean
            Try
                AddPermission(PermType, Username)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Removes user from one of permission types
        ''' </summary>
        ''' <param name="PermType">Whether it be Admin or Disabled</param>
        ''' <param name="Username">A username to be managed</param>
        ''' <exception cref="Exceptions.PermissionManagementException"></exception>
        Public Sub RemovePermission(PermType As PermissionType, Username As String)
            'Sets the required permissions to false.
            If Users.Keys.ToArray.Contains(Username) And Username <> CurrentUser?.Username Then
                Wdbg(DebugLevel.I, "Type is {0}", PermType)
                Select Case PermType
                    Case PermissionType.Administrator
                        UserPermissions(Username) -= PermissionType.Administrator
                    Case PermissionType.Disabled
                        UserPermissions(Username) -= PermissionType.Disabled
                    Case PermissionType.Anonymous
                        UserPermissions(Username) -= PermissionType.Anonymous
                    Case Else
                        Wdbg(DebugLevel.W, "Type is invalid")
                        Throw New Exceptions.PermissionManagementException(DoTranslation("Failed to remove user from permission lists: invalid type {0}"), PermType)
                End Select
                Wdbg(DebugLevel.I, "User {0} permission removed; value is now: {1}", Username, UserPermissions(Username))
            ElseIf Username = CurrentUser.Username Then
                Throw New Exceptions.PermissionManagementException(DoTranslation("You are already logged in."))
            Else
                Wdbg(DebugLevel.W, "User {0} not found on list", Username)
                Throw New Exceptions.PermissionManagementException(DoTranslation("Failed to remove user from permission lists: invalid user {0}"), Username)
            End If

            'Save changes
            For Each UserToken As JObject In UsersToken
                If UserToken("username").ToString = Username Then
                    Dim PermissionArray As List(Of String) = UserToken("permissions").ToObject(GetType(List(Of String)))
                    PermissionArray.Remove(PermType.ToString)
                    UserToken("permissions") = JArray.FromObject(PermissionArray)
                End If
            Next
            File.WriteAllText(GetKernelPath(KernelPathType.Users), JsonConvert.SerializeObject(UsersToken, Formatting.Indented))
        End Sub

        ''' <summary>
        ''' Removes user from one of permission types
        ''' </summary>
        ''' <param name="PermType">Whether it be Admin or Disabled</param>
        ''' <param name="Username">A username to be managed</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="Exceptions.PermissionManagementException"></exception>
        Public Function TryRemovePermission(PermType As PermissionType, Username As String) As Boolean
            Try
                RemovePermission(PermType, Username)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Edits the permission database for new user name
        ''' </summary>
        ''' <param name="OldName">Old username</param>
        ''' <param name="Username">New username</param>
        ''' <exception cref="Exceptions.PermissionManagementException"></exception>
        Public Sub PermissionEditForNewUser(OldName As String, Username As String)
            'Edit username
            If UserPermissions.ContainsKey(OldName) Then
                Try
                    'Store permissions
                    Dim UserOldPermissions As PermissionType = UserPermissions(OldName)

                    'Remove old user entry
                    Wdbg(DebugLevel.I, "Removing {0} from permissions list...", OldName)
                    UserPermissions.Remove(OldName)

                    'Add new user entry
                    UserPermissions.Add(Username, UserOldPermissions)
                    Wdbg(DebugLevel.I, "Added {0} to permissions list with value of {1}", Username, UserPermissions(Username))
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New Exceptions.PermissionManagementException(DoTranslation("You have either found a bug, or the permission you tried to edit for a new user has failed.") + NewLine +
                                                                   DoTranslation("Error {0}: {1}"), ex, ex.GetType.FullName, ex.Message)
                End Try
            Else
                Throw New Exceptions.PermissionManagementException(DoTranslation("One of the permission lists doesn't contain username {0}."), OldName)
            End If
        End Sub

        ''' <summary>
        ''' Edits the permission database for new user name
        ''' </summary>
        ''' <param name="OldName">Old username</param>
        ''' <param name="Username">New username</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="Exceptions.PermissionManagementException"></exception>
        Public Function TryPermissionEditForNewUser(OldName As String, Username As String) As Boolean
            Try
                PermissionEditForNewUser(OldName, Username)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Initializes permissions for a new user with default settings
        ''' </summary>
        ''' <param name="NewUser">A new user name</param>
        ''' <exception cref="Exceptions.PermissionManagementException"></exception>
        Public Sub InitPermissionsForNewUser(NewUser As String)
            'Initialize permissions locally
            If Not UserPermissions.ContainsKey(NewUser) Then UserPermissions.Add(NewUser, PermissionType.None)
        End Sub

        ''' <summary>
        ''' Initializes permissions for a new user with default settings
        ''' </summary>
        ''' <param name="NewUser">A new user name</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="Exceptions.PermissionManagementException"></exception>
        Public Function TryInitPermissionsForNewUser(NewUser As String) As Boolean
            Try
                InitPermissionsForNewUser(NewUser)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Loads permissions for all users
        ''' </summary>
        ''' <exception cref="Exceptions.PermissionManagementException"></exception>
        Public Sub LoadPermissions()
            For Each UserToken As JObject In UsersToken
                Dim User As String = UserToken("username")
                UserPermissions(User) = PermissionType.None
                For Each Perm As String In CType(UserToken("permissions"), JArray)
                    Select Case Perm
                        Case "Administrator"
                            UserPermissions(User) += PermissionType.Administrator
                        Case "Disabled"
                            UserPermissions(User) += PermissionType.Disabled
                        Case "Anonymous"
                            UserPermissions(User) += PermissionType.Anonymous
                    End Select
                Next
            Next
        End Sub

        ''' <summary>
        ''' Loads permissions for all users
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <exception cref="Exceptions.PermissionManagementException"></exception>
        Public Function TryLoadPermissions() As Boolean
            Try
                LoadPermissions()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Gets the permissions for the user
        ''' </summary>
        ''' <param name="Username">Target username</param>
        ''' <returns>Permission type enumeration for the current user, or none if the user isn't found or has no permissions</returns>
        Public Function GetPermissions(Username As String) As PermissionType
            If Username = Nothing Then Username = ""
            Return If(UserPermissions.ContainsKey(Username), UserPermissions(Username), PermissionType.None)
        End Function

        ''' <summary>
        ''' Checks to see if the user has a specific permission
        ''' </summary>
        ''' <param name="Username">Target username</param>
        ''' <param name="SpecificPermission">Specific permission type</param>
        ''' <returns>True if the user has permission; False otherwise</returns>
        Public Function HasPermission(Username As String, SpecificPermission As PermissionType) As Boolean
            Dim SpecificPermissions As PermissionType = GetPermissions(Username)
            Return SpecificPermissions.HasFlag(SpecificPermission)
        End Function

    End Module
End Namespace