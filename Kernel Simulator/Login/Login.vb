
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

Public Module Login

    'Variables
    ''' <summary>
    ''' Current username
    ''' </summary>
    Public CurrentUser As String
    ''' <summary>
    ''' List of usernames and passwords
    ''' </summary>
    Friend Users As New Dictionary(Of String, String)()

    ''' <summary>
    ''' Prompts user for login information
    ''' </summary>
    Sub LoginPrompt()
        While True
            'Check to see if the reboot is requested
            If RebootRequested = True Then
                Wdbg(DebugLevel.W, "Reboot has been requested. Exiting...")
                RebootRequested = False
                Exit Sub
            End If

            'Fire event PreLogin
            EventManager.RaisePreLogin()

            'Check to see if there are any users
            If Users.Count = 0 Then
                'Extremely rare state reached
                Wdbg(DebugLevel.F, "Shell reached rare state, because userword count is 0.")
                Throw New Exceptions.NullUsersException(DoTranslation("There are no more users remaining in the list."))
            ElseIf Users.Count = 1 And Users.Keys(0) = "root" Then
                'Run a first user trigger
                Wdbg(DebugLevel.W, "Only root is found. Triggering first user setup...")
                FirstUserTrigger()
            End If

            'Clear console if clsOnLogin is set to True (If a user has enabled Clear Screen on Login)
            If ClearOnLogin = True Then
                Wdbg(DebugLevel.I, "Clearing screen...")
                Console.Clear()
            End If

            'Generate user list
            If ShowAvailableUsers Then W(DoTranslation("Available usernames: {0}"), True, ColTypes.Neutral, String.Join(", ", ListAllUsers))

            'Read MOTD and MAL
            ReadMOTD(MessageType.MOTD)
            ReadMOTD(MessageType.MAL)

            'Show MOTD once
            Wdbg(DebugLevel.I, "showMOTDOnceFlag = {0}, showMOTD = {1}", ShowMOTDOnceFlag, ShowMOTD)
            If ShowMOTDOnceFlag = True And ShowMOTD = True Then
                W(vbNewLine + ProbePlaces(MOTDMessage), True, ColTypes.Banner)
            End If
            ShowMOTDOnceFlag = False

            'Prompt user to login
            W(DoTranslation("Username: "), False, ColTypes.Input)
            Dim answeruser As String = Console.ReadLine()

            'Parse input
            If InStr(answeruser, " ") > 0 Then
                Wdbg(DebugLevel.W, "Spaces found in username.")
                W(DoTranslation("Spaces are not allowed."), True, ColTypes.Error)
                EventManager.RaiseLoginError(answeruser, "spaces")
            ElseIf answeruser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
                Wdbg(DebugLevel.W, "Unknown characters found in username.")
                W(DoTranslation("Special characters are not allowed."), True, ColTypes.Error)
                EventManager.RaiseLoginError(answeruser, "specialchars")
            ElseIf Users.ContainsKey(answeruser) Then
                Wdbg(DebugLevel.I, "Username correct. Finding if the user is disabled...")
                If Not HasPermission(answeruser, PermissionType.Disabled) Then
                    Wdbg(DebugLevel.I, "User can log in. (User is not in disabled list)")
                    ShowPasswordPrompt(answeruser)
                Else
                    Wdbg(DebugLevel.W, "User can't log in. (User is in disabled list)")
                    W(DoTranslation("User is disabled."), True, ColTypes.Error)
                    EventManager.RaiseLoginError(answeruser, "disabled")
                End If
            Else
                Wdbg(DebugLevel.E, "Username not found.")
                W(DoTranslation("Wrong username."), True, ColTypes.Error)
                EventManager.RaiseLoginError(answeruser, "notfound")
            End If
        End While
    End Sub

    ''' <summary>
    ''' Prompts user for password
    ''' </summary>
    ''' <param name="usernamerequested">A username that is about to be logged in</param>
    Public Sub ShowPasswordPrompt(usernamerequested As String)
        'Error handler
        On Error Resume Next

        'Prompts user to enter a user's password
        While True
            'Check to see if reboot is requested
            If RebootRequested = True Then
                Wdbg(DebugLevel.W, "Reboot has been requested. Exiting...")
                RebootRequested = False
                Exit Sub
            End If

            'Get the password from dictionary
            Dim UserPassword As String = Users.Item(usernamerequested)

            'Check if there's a password
            If Not UserPassword = GetEmptyHash(Algorithms.SHA256) Then 'No password
                'Wait for input
                Wdbg(DebugLevel.I, "Password not empty")
                W(DoTranslation("{0}'s password: "), False, ColTypes.Input, usernamerequested)

                'Get input
                Dim answerpass As String = ReadLineNoInput("*"c)
                Console.WriteLine()

                'Compute password hash
                Wdbg(DebugLevel.I, "Computing written password hash...")
                answerpass = GetEncryptedString(answerpass, Algorithms.SHA256)

                'Parse password input
                If Users.TryGetValue(usernamerequested, UserPassword) AndAlso UserPassword = answerpass Then
                    'Log-in instantly
                    Wdbg(DebugLevel.I, "Password written correctly. Entering shell...")
                    SignIn(usernamerequested)
                    Exit Sub
                Else
                    Wdbg(DebugLevel.I, "Passowrd written wrong...")
                    W(DoTranslation("Wrong password."), True, ColTypes.Error)
                    EventManager.RaiseLoginError(usernamerequested, "wrongpass")
                    If Not Maintenance Then
                        If Not LockMode Then
                            Exit Sub
                        End If
                    End If
                End If
            Else
                'Log-in instantly
                Wdbg(DebugLevel.I, "Password is empty")
                SignIn(usernamerequested)
                Exit Sub
            End If
        End While

    End Sub

    ''' <summary>
    ''' Signs in to the username
    ''' </summary>
    ''' <param name="signedInUser">A specified username</param>
    Public Sub SignIn(signedInUser As String)

        'Release lock
        If LockMode Then
            Wdbg(DebugLevel.I, "Releasing lock and getting back to shell...")
            LockMode = False
            EventManager.RaisePostUnlock(DefSaverName)
            Exit Sub
        End If

        'Get out of login mode
        LoginFlag = False

        'Notifies the kernel that the user has signed in
        LoggedIn = True

        'Sign in to user.
        CurrentUser = signedInUser
        If LockMode = True Then LockMode = False
        Wdbg(DebugLevel.I, "Lock released.")
        ShowMOTDOnceFlag = True
        W(ProbePlaces(MAL), True, ColTypes.Banner)

        'Fire event PostLogin
        EventManager.RaisePostLogin(CurrentUser)

        'Initialize shell
        Wdbg(DebugLevel.I, "Shell is being initialized...")
        InitializeShell()
    End Sub

End Module
