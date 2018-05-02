
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

    Sub initializeMainUsers()

        'Check if the process is done, then do nothing if it is done.
        If (MainUserDone = False) Then

            'Main users will be initialized
            userword.Add("root", "toor")
            Groups.permission("Admin", True, "root", "Add", True)
            Groups.permission("Disabled", False, "root", "Add", True)

            'Print each main user initialized, if quiet mode wasn't passed
            If (Quiet = False) Then
                System.Console.WriteLine("usrmgr: System usernames: {0}", String.Join(", ", userword.Keys.ToArray))
            End If

            'Send signal to kernel that this function is done
            MainUserDone = True
        End If

    End Sub

    Sub initializeUser(ByVal uninitUser As String, Optional ByVal unpassword As String = "")

        'Do not confuse with initializeUsers. It initializes user.
        userword.Add(uninitUser, unpassword)
        Groups.permission("Admin", False, uninitUser, "Add", True)
        Groups.permission("Disabled", False, uninitUser, "Add", True)

    End Sub

    Sub adduser(ByVal newUser As String, Optional ByVal newPassword As String = "")

        'Adds users
        If (Quiet = False) Then
            System.Console.WriteLine("usrmgr: Creating username {0}...", newUser)
        End If
        If (newPassword = Nothing) Then
            initializeUser(newUser)
        Else
            initializeUser(newUser, newPassword)
        End If

    End Sub

    Sub resetUsers()

        'Resets users and permissions
        For Each rmuser As String In userword.Keys.ToArray
            Groups.permission("Admin", False, rmuser, "Remove", True)
            Groups.permission("Disabled", False, rmuser, "Remove", True)
        Next
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
        System.Console.Write("Username to be changed: ")
        System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
        Dim answernuser = System.Console.ReadLine()
        System.Console.ResetColor()
        If InStr(CStr(answernuser), " ") > 0 Then
            System.Console.WriteLine("Spaces are not allowed.")
            changePassword()
        ElseIf (answernuser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
            System.Console.WriteLine("Special characters are not allowed.")
            changePassword()
        ElseIf (answernuser = "q") Then
            System.Console.WriteLine("Username changing has been cancelled.")
        Else
            For Each user As String In userword.Keys.ToArray
                If (user = answernuser) Then
                    DoneFlag = True
                    System.Console.Write("Username to change to: ")
                    System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                    Dim answerNewUserTemp = System.Console.ReadLine()
                    System.Console.ResetColor()
                    If InStr(CStr(answerNewUserTemp), " ") > 0 Then
                        System.Console.WriteLine("Spaces are not allowed.")
                        changePassword()
                    ElseIf (answerNewUserTemp.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                        System.Console.WriteLine("Special characters are not allowed.")
                        changePassword()
                    ElseIf (answerNewUserTemp = "q") Then
                        System.Console.WriteLine("Username changing has been cancelled.")
                    ElseIf (userword.ContainsKey(answernuser) = True) Then
                        If Not (userword.ContainsKey(answerNewUserTemp) = True) Then
                            Dim temporary As String = userword(answernuser)
                            userword.Remove(answernuser)
                            userword.Add(answerNewUserTemp, temporary)
                            Groups.permissionEditForNewUser(answernuser, answerNewUserTemp)
                            System.Console.WriteLine("Username has been changed to {0}!", answerNewUserTemp)
                            If (answernuser = signedinusrnm) Then
                                LoginPrompt()
                            End If
                        Else
                            System.Console.WriteLine("The new name you entered is already found.")
                        End If
                    End If
                End If
            Next
        End If
        If (DoneFlag = False) Then
            System.Console.WriteLine("User {0} not found.", answernuser)
            changePassword()
        End If

    End Sub

    Sub changePassword()

        'Prompts user to enter current password
        On Error Resume Next
        password = userword.Item(CStr(answeruser))

        'Checks if there is a password
        If Not (password = Nothing) Then
            System.Console.Write("Current password: ")
            System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
            answerpass = System.Console.ReadLine()
            System.Console.ResetColor()
            If InStr(CStr(answerpass), " ") > 0 Then
                System.Console.WriteLine("Spaces are not allowed.")
                changePassword()
            ElseIf (answerpass.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                System.Console.WriteLine("Special characters are not allowed.")
                changePassword()
            ElseIf (answerpass = "q") Then
                System.Console.WriteLine("Password changing has been cancelled.")
            Else
                If userword.TryGetValue(CStr(answeruser), password) AndAlso password = answerpass Then
                    changePasswordPrompt(CStr(answeruser))
                Else
                    System.Console.WriteLine(vbNewLine + "Wrong password.")
                    changePassword()
                End If
            End If
        Else
            changePasswordPrompt(CStr(answeruser))
        End If

    End Sub

    Sub changePasswordPrompt(ByVal usernamerequestedChange As String)

        'Prompts user to enter new password
        System.Console.Write("New password: ")
        System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
        Dim answernewpass = System.Console.ReadLine()
        System.Console.ResetColor()
        If InStr(answernewpass, " ") > 0 Then
            System.Console.WriteLine("Spaces are not allowed.")
            changePasswordPrompt(usernamerequestedChange)
        ElseIf (answernewpass.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
            System.Console.WriteLine("Special characters are not allowed.")
            changePasswordPrompt(usernamerequestedChange)
        ElseIf (answernewpass = "q") Then
            System.Console.WriteLine("Password changing has been cancelled.")
        Else
            System.Console.Write("Confirm: ")
            System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
            Dim answernewpassconfirm = System.Console.ReadLine()
            System.Console.ResetColor()
            If InStr(answernewpassconfirm, " ") > 0 Then
                System.Console.WriteLine("Spaces are not allowed.")
                changePasswordPrompt(usernamerequestedChange)
            ElseIf (answernewpassconfirm.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                System.Console.WriteLine("Special characters are not allowed.")
                changePasswordPrompt(usernamerequestedChange)
            ElseIf (answernewpassconfirm = "q") Then
                System.Console.WriteLine("Password changing has been cancelled.")
            ElseIf (answernewpassconfirm = answernewpass) Then
                userword.Item(usernamerequestedChange) = answernewpass
            ElseIf (answernewpassconfirm <> answernewpass) Then
                System.Console.WriteLine("Passwords doesn't match.")
                changePasswordPrompt(usernamerequestedChange)
            End If
        End If

    End Sub

    Sub removeUser()

        'Variables for removeUser()
        Dim DoneFlag As String = "No"
        On Error Resume Next

        'Removes user from the username and password list
        System.Console.Write("Username to be removed: ")
        System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
        Dim answerrmuser = System.Console.ReadLine()
        System.Console.ResetColor()
        If InStr(answerrmuser, " ") > 0 Then
            System.Console.WriteLine("Spaces are not allowed.")
            removeUser()
            answerrmuser = Nothing
        ElseIf (answerrmuser = "q") Then
            DoneFlag = "Cancelled"
            answerrmuser = Nothing
        ElseIf (answerrmuser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
            System.Console.WriteLine("Special characters are not allowed.")
            removeUser()
            answerrmuser = Nothing
        ElseIf (answerrmuser = Nothing) Then
            System.Console.WriteLine("Blank username.")
            removeUser()
            answerrmuser = Nothing
        ElseIf userword.ContainsKey(answerrmuser) = False Then
            System.Console.WriteLine("User {0} not found.", answerrmuser)
            removeUser()
            answerrmuser = Nothing
        Else
            For Each usersRemove As String In userword.Keys.ToArray
                If (usersRemove = answerrmuser And answerrmuser = "root") Then
                    System.Console.WriteLine("User {0} isn't allowed to be removed.", answerrmuser)
                    removeUser()
                    answerrmuser = Nothing
                ElseIf (answerrmuser = usersRemove And usersRemove = signedinusrnm) Then
                    System.Console.WriteLine("User {0} is already logged in. Log-out and log-in as another admin.", answerrmuser)
                    removeUser()
                    answerrmuser = Nothing
                ElseIf (usersRemove = answerrmuser And answerrmuser <> "root") Then
                    Groups.permission("Admin", False, answerrmuser, "Remove")
                    Groups.permission("Disabled", False, answerrmuser, "Remove")
                    userword.Remove(answerrmuser)
                    System.Console.WriteLine("User {0} removed.", answerrmuser)
                    DoneFlag = "Yes"
                    answerrmuser = Nothing
                End If
            Next
        End If


    End Sub

    Sub addUser()

        'Prompt user to write username to be added
        System.Console.Write("Write username: ")
        System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
        answernewuser = System.Console.ReadLine()
        System.Console.ResetColor()
        If InStr(answernewuser, " ") > 0 Then
            System.Console.WriteLine("Spaces are not allowed.")
        ElseIf (answernewuser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
            System.Console.WriteLine("Special characters are not allowed.")
        ElseIf (answernewuser = "q") Then
            System.Console.WriteLine("Username creation has been cancelled.")
        Else
            System.Console.Write("Write password: ")
            System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
            answerpassword = System.Console.ReadLine()
            System.Console.ResetColor()
            If InStr(answerpassword, " ") > 0 Then
                System.Console.WriteLine("Spaces are not allowed.")
            ElseIf (answerpassword.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                System.Console.WriteLine("Special characters are not allowed.")
            ElseIf (answerpassword = "q") Then
                System.Console.WriteLine("Username creation has been cancelled.")
            Else
                System.Console.Write("Confirm: ")
                System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                Dim answerpasswordconfirm As String = System.Console.ReadLine()
                System.Console.ResetColor()
                If InStr(answerpasswordconfirm, " ") > 0 Then
                    System.Console.WriteLine("Spaces are not allowed.")
                ElseIf (answerpasswordconfirm.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                    System.Console.WriteLine("Special characters are not allowed.")
                ElseIf (answerpasswordconfirm = "q") Then
                    System.Console.WriteLine("Username creation has been cancelled.")
                ElseIf (answerpassword = answerpasswordconfirm) Then
                    adduser(answernewuser, answerpassword)
                ElseIf (answerpassword <> answerpasswordconfirm) Then
                    System.Console.WriteLine("Password doesn't match.")
                End If
            End If
        End If

    End Sub

End Module
