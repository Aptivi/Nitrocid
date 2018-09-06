
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

'We have separated below subs from Login.vb for easy management of user tools.

Public Module UserManagement

    'TODO: Merge initializeMainUsers to adduser
    Sub initializeMainUsers()

        'Check if the process is done, then do nothing if it is done.
        If (MainUserDone = False) Then

            'Main users will be initialized
            If (setRootPasswd = True) Then
                userword.Add("root", RootPasswd)
            Else
                userword.Add("root", "toor")
            End If
            Wdbg("Dictionary {0}.userword has been added, result: userword = {1}", True, userword, String.Join(", ", userword.ToArray))
            adminList.Add("root", True)
            disabledList.Add("root", False)

            'Print each main user initialized, if quiet mode wasn't passed
            If (Quiet = False) Then
                Wln("usrmgr: System usernames: {0}", "neutralText", String.Join(", ", userword.Keys.ToArray))
            End If

            'Send signal to kernel that this function is done
            MainUserDone = True
        End If

    End Sub

    Public Sub initializeUser(ByVal uninitUser As String, Optional ByVal unpassword As String = "")

        Try
            userword.Add(uninitUser, unpassword)
            Wdbg("userword = {1}", True, userword, String.Join(", ", userword.ToArray))
            adminList.Add(uninitUser, False)
            disabledList.Add(uninitUser, False)
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln("Error trying to add username." + vbNewLine + "Error {0}: {1}" + vbNewLine + "{2}", "neutralText", _
                    Err.Number, Err.Description, ex.StackTrace)
                Wdbg(ex.StackTrace, True)
            Else
                Wln("Error trying to add username." + vbNewLine + "Error {0}: {1}", "neutralText", Err.Number, Err.Description)
            End If
        End Try

    End Sub

    Public Sub adduser(ByVal newUser As String, Optional ByVal newPassword As String = "")

        'Adds users
        If (Quiet = False) Then
            Wln("usrmgr: Creating username {0}...", "neutralText", newUser)
        End If
        If (newPassword = Nothing) Then
            initializeUser(newUser)
        Else
            initializeUser(newUser, newPassword)
        End If

    End Sub

    Public Sub resetUsers()

        'Resets users and permissions
        adminList.Clear()
        disabledList.Clear()
        userword.Clear()

        'Resets outputs
        password = Nothing
        LoginFlag = False
        CruserFlag = False
        MainUserDone = False
        signedinusrnm = Nothing

        'Resets inputs
        answerpass = Nothing
        answeruser = Nothing
        arguser = Nothing
        argword = Nothing

    End Sub

    'This sub is an accomplice of in-shell command arguments.
    Public Sub removeUserFromDatabase(ByVal user As String)

        Try
            Dim DoneFlag As String = "No"
            If InStr(user, " ") > 0 Then
                Wln("Spaces are not allowed.", "neutralText")
            ElseIf (user = "q") Then
                DoneFlag = "Cancelled"
            ElseIf (user.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                Wln("Special characters are not allowed.", "neutralText")
            ElseIf (user = Nothing) Then
                Wln("Blank username.", "neutralText")
            ElseIf userword.ContainsKey(user) = False Then
                Wdbg("ASSERT(isFound({0})) = False", True, user)
                Wln("User {0} not found.", "neutralText", user)
            Else
                For Each usersRemove As String In userword.Keys.ToArray
                    If (usersRemove = user And user = "root") Then
                        Wln("User {0} isn't allowed to be removed.", "neutralText", user)
                    ElseIf (user = usersRemove And usersRemove = signedinusrnm) Then
                        Wln("User {0} is already logged in. Log-out and log-in as another admin.", "neutralText", user)
                        Wdbg("ASSERT({0}.isLoggedIn(ASSERT({0} = {1}) = True)) = True", True, user, signedinusrnm)
                    ElseIf (usersRemove = user And user <> "root") Then
                        adminList.Remove(user)
                        disabledList.Remove(user)
                        Wdbg("userword.ToBeRemoved = {0}", True, String.Join(", ", userword(user).ToArray))
                        userword.Remove(user)
                        Wln("User {0} removed.", "neutralText", user)
                        DoneFlag = "Yes"
                    End If
                Next
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln("Error trying to remove username." + vbNewLine + "Error {0}: {1}" + vbNewLine + "{2}", "neutralText", _
                    Err.Number, Err.Description, ex.StackTrace)
                Wdbg(ex.StackTrace, True)
            Else
                Wln("Error trying to remove username." + vbNewLine + "Error {0}: {1}", "neutralText", Err.Number, Err.Description)
            End If
        End Try
        user = Nothing

    End Sub

End Module
