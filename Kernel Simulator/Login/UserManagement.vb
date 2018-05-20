
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

Module UserManagement

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

    Sub initializeUser(ByVal uninitUser As String, Optional ByVal unpassword As String = "")

        'Do not confuse with initializeUsers. It initializes user.
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

    Sub adduser(ByVal newUser As String, Optional ByVal newPassword As String = "")

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

    Sub resetUsers()

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
        answernewuser = Nothing
        answerpass = Nothing
        answerpassword = Nothing
        answeruser = Nothing
        arguser = Nothing
        argword = Nothing

    End Sub

    Sub changeName()

        'Variables
        Dim DoneFlag As Boolean = False
        On Error Resume Next

        'Prompts user to enter a new username
        W("Username to be changed: ", "input")
        Dim answernuser = System.Console.ReadLine()
        If InStr(CStr(answernuser), " ") > 0 Then
            Wln("Spaces are not allowed.", "neutralText")
            changePassword()
        ElseIf (answernuser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
            Wln("Special characters are not allowed.", "neutralText")
            changePassword()
        ElseIf (answernuser = "q") Then
            Wln("Username changing has been cancelled.", "neutralText")
        Else
            For Each user As String In userword.Keys.ToArray
                If (user = answernuser) Then
                    DoneFlag = True
                    W("Username to change to: ", "input")
                    System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                    Dim answerNewUserTemp = System.Console.ReadLine()
                    If InStr(CStr(answerNewUserTemp), " ") > 0 Then
                        Wln("Spaces are not allowed.", "neutralText")
                        changePassword()
                    ElseIf (answerNewUserTemp.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                        Wln("Special characters are not allowed.", "neutralText")
                        changePassword()
                    ElseIf (answerNewUserTemp = "q") Then
                        Wln("Username changing has been cancelled.", "neutralText")
                    ElseIf (userword.ContainsKey(answernuser) = True) Then
                        If Not (userword.ContainsKey(answerNewUserTemp) = True) Then
                            Dim temporary As String = userword(answernuser)
                            Wdbg("userword.ToBeRemoved = {0}", True, String.Join(", ", userword(answernuser).ToArray))
                            userword.Remove(answernuser)
                            userword.Add(answerNewUserTemp, temporary)
                            Wdbg("userword.Added = {0}", True, userword(answerNewUserTemp))
                            Groups.permissionEditForNewUser(answernuser, answerNewUserTemp)
                            Wln("Username has been changed to {0}!", "neutralText", answerNewUserTemp)
                            If (answernuser = signedinusrnm) Then
                                Wdbg("{0}.Logout.Execute(because ASSERT({0} = {1}) = True)", True, answernuser, signedinusrnm)
                                LoginPrompt()
                            End If
                        Else
                            Wdbg("ASSERT(userwordDict.Cont({0})) = True", True, answerNewUserTemp)
                            Wln("The new name you entered is already found.", "neutralText")
                        End If
                    End If
                End If
            Next
        End If
        If (DoneFlag = False) Then
            Wdbg("ASSERT(isFound({0})) = False", True, answernuser)
            Wln("User {0} not found.", "neutralText", answernuser)
            changePassword()
        End If

    End Sub

    Sub changePassword()

        'Prompts user to enter current password
        On Error Resume Next
        password = userword.Item(CStr(answeruser))

        'Checks if there is a password
        If Not (password = Nothing) Then
            W("Current password: ", "input")
            answerpass = System.Console.ReadLine()
            If InStr(CStr(answerpass), " ") > 0 Then
                Wln("Spaces are not allowed.", "neutralText")
                changePassword()
            ElseIf (answerpass.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                Wln("Special characters are not allowed.", "neutralText")
                changePassword()
            ElseIf (answerpass = "q") Then
                Wln("Password changing has been cancelled.", "neutralText")
            Else
                If userword.TryGetValue(CStr(answeruser), password) AndAlso password = answerpass Then
                    changePasswordPrompt(CStr(answeruser))
                Else
                    Wln(vbNewLine + "Wrong password.", "neutralText")
                    changePassword()
                End If
            End If
        Else
            changePasswordPrompt(CStr(answeruser))
        End If

    End Sub

    Sub changePasswordPrompt(ByVal usernamerequestedChange As String)

        'Prompts user to enter new password
        W("New password: ", "input")
        Dim answernewpass = System.Console.ReadLine()
        If InStr(answernewpass, " ") > 0 Then
            Wln("Spaces are not allowed.", "neutralText")
            changePasswordPrompt(usernamerequestedChange)
        ElseIf (answernewpass.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
            Wln("Special characters are not allowed.", "neutralText")
            changePasswordPrompt(usernamerequestedChange)
        ElseIf (answernewpass = "q") Then
            Wln("Password changing has been cancelled.", "neutralText")
        Else
            W("Confirm: ", "input")
            Dim answernewpassconfirm = System.Console.ReadLine()
            If InStr(answernewpassconfirm, " ") > 0 Then
                Wln("Spaces are not allowed.", "neutralText")
                changePasswordPrompt(usernamerequestedChange)
            ElseIf (answernewpassconfirm.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                Wln("Special characters are not allowed.", "neutralText")
                changePasswordPrompt(usernamerequestedChange)
            ElseIf (answernewpassconfirm = "q") Then
                Wln("Password changing has been cancelled.", "neutralText")
            ElseIf (answernewpassconfirm = answernewpass) Then
                userword.Item(usernamerequestedChange) = answernewpass
            ElseIf (answernewpassconfirm <> answernewpass) Then
                Wln("Passwords doesn't match.", "neutralText")
                changePasswordPrompt(usernamerequestedChange)
            End If
        End If

    End Sub

    Sub removeUser()

        'Removes user from the username and password list
        W("Username to be removed: ", "input")
        Dim answerrmuser = System.Console.ReadLine()
        removeUserFromDatabase(answerrmuser)

    End Sub

    'This sub is an accomplice of in-shell command arguments.
    Friend Sub removeUserFromDatabase(ByVal user As String)

        Try
            Dim DoneFlag As String = "No"
            If InStr(user, " ") > 0 Then
                Wln("Spaces are not allowed.", "neutralText")
                If (strcommand = "rmuser") Then
                    removeUser()
                End If
                user = Nothing
            ElseIf (user = "q") Then
                DoneFlag = "Cancelled"
                user = Nothing
            ElseIf (user.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                Wln("Special characters are not allowed.", "neutralText")
                If (strcommand = "rmuser") Then
                    removeUser()
                End If
                user = Nothing
            ElseIf (user = Nothing) Then
                Wln("Blank username.", "neutralText")
                If (strcommand = "rmuser") Then
                    removeUser()
                End If
                user = Nothing
            ElseIf userword.ContainsKey(user) = False Then
                Wdbg("ASSERT(isFound({0})) = False", True, user)
                Wln("User {0} not found.", "neutralText", user)
                If (strcommand = "rmuser") Then
                    removeUser()
                End If
                user = Nothing
            Else
                For Each usersRemove As String In userword.Keys.ToArray
                    If (usersRemove = user And user = "root") Then
                        Wln("User {0} isn't allowed to be removed.", "neutralText", user)
                        If (strcommand = "rmuser") Then
                            removeUser()
                        End If
                        user = Nothing
                    ElseIf (user = usersRemove And usersRemove = signedinusrnm) Then
                        Wln("User {0} is already logged in. Log-out and log-in as another admin.", "neutralText", user)
                        Wdbg("ASSERT({0}.isLoggedIn(ASSERT({0} = {1}) = True)) = True", True, user, signedinusrnm)
                        If (strcommand = "rmuser") Then
                            removeUser()
                        End If
                        user = Nothing
                    ElseIf (usersRemove = user And user <> "root") Then
                        adminList.Remove(user)
                        disabledList.Remove(user)
                        Wdbg("userword.ToBeRemoved = {0}", True, String.Join(", ", userword(user).ToArray))
                        userword.Remove(user)
                        Wln("User {0} removed.", "neutralText", user)
                        DoneFlag = "Yes"
                        user = Nothing
                    End If
                Next
            End If
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

    Sub addUser()

        'Prompt user to write username to be added
        W("Write username: ", "input")
        answernewuser = System.Console.ReadLine()
        If InStr(answernewuser, " ") > 0 Then
            Wln("Spaces are not allowed.", "neutralText")
        ElseIf (answernewuser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
            Wln("Special characters are not allowed.", "neutralText")
        ElseIf (answernewuser = "q") Then
            Wln("Username creation has been cancelled.", "neutralText")
        Else
            newPassword(answernewuser)
        End If

    End Sub

    Sub newPassword(ByVal user As String)

        W("Write password: ", "input")
        answerpassword = System.Console.ReadLine()
        If InStr(answerpassword, " ") > 0 Then
            Wln("Spaces are not allowed.", "neutralText")
        ElseIf (answerpassword.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
            Wln("Special characters are not allowed.", "neutralText")
        ElseIf (answerpassword = "q") Then
            Wln("Username creation has been cancelled.", "neutralText")
        Else
            W("Confirm: ", "input")
            Dim answerpasswordconfirm As String = System.Console.ReadLine()
            If InStr(answerpasswordconfirm, " ") > 0 Then
                Wln("Spaces are not allowed.", "neutralText")
            ElseIf (answerpasswordconfirm.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                Wln("Special characters are not allowed.", "neutralText")
            ElseIf (answerpasswordconfirm = "q") Then
                Wln("Username creation has been cancelled.", "neutralText")
            ElseIf (answerpassword = answerpasswordconfirm) Then
                adduser(user, answerpassword)
            ElseIf (answerpassword <> answerpasswordconfirm) Then
                Wln("Password doesn't match.", "neutralText")
            End If
        End If

    End Sub

End Module
