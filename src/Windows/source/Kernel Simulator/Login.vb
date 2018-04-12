
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

Module Login

    'TODO: Re-write login system, removing unnecessary variables
    'Variables
    Public usernamelist As New List(Of String)                  'Temporary
    Public passwordlist As New List(Of String)                  'Temporary
    Public userword As New Dictionary(Of String, String)()      'List of usernames and passwords
    Public answeruser                                           'Input of username
    Public answerpass                                           'Input of password
    Public password As String                                   'Password for user we're logging in to
    Public signedinusrnm As String                              'Username that is signed in
    Public LoginFlag As Boolean                                 'Flag for log-in
    Public MainUserDone As Boolean                              'Main users initialization is done

    Sub initializeMainUsers()
        'Check if the process is done, then do nothing if it is done.
        If (MainUserDone = False) Then

            'Main users will be initialized
            usernamelist.Add("root")
            usernamelist.Add("useradd")

            'Main passwords will be initialized
            passwordlist.Add("toor")
            passwordlist.Add("")

            My.Settings.Passwords.Add("toor")
            My.Settings.Passwords.Add("")

            'Print each main user initialized, if quiet mode wasn't passed
            If (Quiet = False) Then
                System.Console.WriteLine("usrmgr: System usernames: {0}", String.Join(", ", usernamelist))
            End If

            'Send signal to kernel that this function is done
            MainUserDone = True
        End If

    End Sub

    Sub makeUserDatabase()

        'Mandatory. Don't remove
        For i As Integer = 0 To usernamelist.Count - 1
            If (userword.TryGetValue(usernamelist(i), passwordlist(i)) = False) Then
                userword.Add(usernamelist(i), My.Settings.Passwords(i))
            End If
        Next

    End Sub
    Sub initializeUsers()

        'Do not confuse with initializeUser. It loads users.
        'Initializing users and prompts to log-in if the login flag is true
        For Each setUsers As String In My.Settings.Usernames
            usernamelist.Add(setUsers)
        Next
        For Each setPassword As String In My.Settings.Passwords
            passwordlist.Add(setPassword)
        Next
        If (LoginFlag = True) Then
            If (Quiet = False) Then
                System.Console.WriteLine("usrmgr: Users {0} has been successfully loaded", String.Join(", ", usernamelist))
            End If
            makeUserDatabase()
            If (LoginFlag = True) Then
                Login.LoginPrompt()
            End If
        End If

    End Sub

    Sub initializeUser(ByVal uninitUser As String, Optional ByVal unpassword As String = "")

        'Do not confuse with initializeUsers. It initializes user.
        If (Quiet = False) Then
            System.Console.WriteLine("usrmgr: Username {0} created.", uninitUser)
        End If
        passwordlist.Add(unpassword)
        usernamelist.Add(uninitUser)
        If (unpassword = "") Then
            My.Settings.Passwords.Add("")
        Else
            My.Settings.Passwords.Add(unpassword)
        End If

    End Sub

    Sub adduser(ByVal newUser As String, Optional ByVal newPassword As String = "")

        'Adds users
        If (Quiet = False) Then
            System.Console.WriteLine("usrmgr: {0} not found. Creating...", newUser)
        End If
        If (newPassword = Nothing) Then
            initializeUser(newUser)
        Else
            initializeUser(newUser, newPassword)
        End If

    End Sub

    Sub LoginPrompt()

        'Prompts user to log-in
        System.Console.Write(vbNewLine + My.Settings.MOTD + vbNewLine + vbNewLine + "Username: ")
        System.Console.ForegroundColor = ConsoleColor.White
        answeruser = System.Console.ReadLine()
        System.Console.ResetColor()
        If InStr(answeruser, " ") > 0 Then
            System.Console.WriteLine("Spaces are not allowed.")
            LoginPrompt()
        ElseIf (answeruser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
            System.Console.WriteLine("Special characters are not allowed.")
            LoginPrompt()
        Else
            showPasswordPrompt(answeruser)
        End If

    End Sub

    Sub changePassword()

        'Prompts user to enter current password
        On Error Resume Next
        password = userword.Item(answeruser)

        'Checks if there is a password
        If Not (password = Nothing) Then
            System.Console.Write("Current password: ")
            System.Console.ForegroundColor = ConsoleColor.White
            answerpass = System.Console.ReadLine()
            System.Console.ResetColor()
            If InStr(answerpass, " ") > 0 Then
                System.Console.WriteLine("Spaces are not allowed.")
                changePassword()
            ElseIf (answerpass.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                System.Console.WriteLine("Special characters are not allowed.")
                changePassword()
            Else
                If userword.TryGetValue(answeruser, password) AndAlso password = answerpass Then
                    changePasswordPrompt(answeruser)
                Else
                    System.Console.WriteLine(vbNewLine + "Wrong password.")
                    changePassword()
                End If
            End If
        Else
            changePasswordPrompt(answeruser)
        End If

    End Sub

    Sub changePasswordPrompt(ByVal usernamerequestedChange As String)

        'Prompts user to enter new password
        System.Console.Write("New password: ")
        System.Console.ForegroundColor = ConsoleColor.White
        Dim answernewpass = System.Console.ReadLine()
        System.Console.ResetColor()
        If InStr(answernewpass, " ") > 0 Then
            System.Console.WriteLine("Spaces are not allowed.")
            changePassword()
        ElseIf (answernewpass.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
            System.Console.WriteLine("Special characters are not allowed.")
            changePassword()
        Else
            'TODO: Implement password confirmation
            userword.Item(answeruser) = answernewpass
        End If

    End Sub

    Sub showPasswordPrompt(ByVal usernamerequested As String)

        'Prompts user to enter a user's password
        Dim DoneFlag As Boolean = False
        On Error Resume Next
        For Each availableUsers As String In usernamelist
            If availableUsers = answeruser Then
                DoneFlag = True
                password = userword.Item(usernamerequested)

                'Check if there's the password
                If Not (password = Nothing) Then
                    System.Console.Write("Password for {0}: ", usernamerequested)
                    System.Console.ForegroundColor = ConsoleColor.White
                    answerpass = System.Console.ReadLine()
                    System.Console.ResetColor()
                    If InStr(answerpass, " ") > 0 Then
                        System.Console.WriteLine("Spaces are not allowed.")
                        LoginPrompt()
                    ElseIf (answerpass.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
                        System.Console.WriteLine("Special characters are not allowed.")
                        LoginPrompt()
                    Else
                        If userword.TryGetValue(usernamerequested, password) AndAlso password = answerpass Then
                            signIn(usernamerequested)
                        Else
                            System.Console.WriteLine(vbNewLine + "Wrong password.")
                            LoginPrompt()
                        End If
                    End If
                Else
                    'Log-in instantly
                    signIn(usernamerequested)
                End If
            End If
        Next
        If DoneFlag = False Then
            System.Console.WriteLine(vbNewLine + "Wrong username.")
            LoginPrompt()
        End If

    End Sub

    Sub signIn(ByVal signedInUser As String)

        'Initialize shell, and sign in to user.
        System.Console.WriteLine(vbNewLine + "Logged in successfully as {0}!", signedInUser)
        signedinusrnm = signedInUser
        Shell.initializeShell()

    End Sub

End Module
