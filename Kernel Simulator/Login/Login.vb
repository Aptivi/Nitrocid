
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

Imports KS.Misc.Encryption
Imports KS.Misc.Screensaver
Imports KS.Network.RSS

Namespace Login
    Public Module Login

        'Variables
        ''' <summary>
        ''' Username prompt
        ''' </summary>
        Public UsernamePrompt As String
        ''' <summary>
        ''' Password prompt
        ''' </summary>
        Public PasswordPrompt As String
        ''' <summary>
        ''' List of usernames and passwords
        ''' </summary>
        Friend Users As New Dictionary(Of String, String)()
        ''' <summary>
        ''' Current username
        ''' </summary>
        Private CurrentUserInfo As UserInfo

        ''' <summary>
        ''' Current username
        ''' </summary>
        Public ReadOnly Property CurrentUser As UserInfo
            Get
                Return CurrentUserInfo
            End Get
        End Property

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
                KernelEventManager.RaisePreLogin()

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

                'Clear console if ClearOnLogin is set to True (If a user has enabled Clear Screen on Login)
                If ClearOnLogin = True Then
                    Wdbg(DebugLevel.I, "Clearing screen...")
                    Console.Clear()
                End If

                'Read MOTD and MAL
                ReadMOTD(MessageType.MOTD)
                ReadMOTD(MessageType.MAL)

                'Show MOTD once
                Wdbg(DebugLevel.I, "showMOTDOnceFlag = {0}, showMOTD = {1}", ShowMOTDOnceFlag, ShowMOTD)
                If ShowMOTDOnceFlag = True And ShowMOTD = True Then
                    Write(NewLine + ProbePlaces(MOTDMessage), True, ColTypes.Banner)
                End If
                ShowMOTDOnceFlag = False

                'How do we prompt user to login?
                Dim UsersList As List(Of String) = ListAllUsers()
                If ChooseUser Then
                    'Generate user list
                    WriteList(UsersList)
                    Dim AnswerUserInt As Integer = 0

                    'Prompt user to choose a user
                    Do Until AnswerUserInt <> 0
                        Write(">> ", False, ColTypes.Input)
                        Dim AnswerUserString As String = ReadLine()

                        'Parse input
                        If Not String.IsNullOrWhiteSpace(AnswerUserString) Then
                            If Integer.TryParse(AnswerUserString, AnswerUserInt) Then
                                Dim SelectedUser As String = SelectUser(AnswerUserInt)
                                Wdbg(DebugLevel.I, "Username correct. Finding if the user is disabled...")
                                If Not HasPermission(SelectedUser, PermissionType.Disabled) Then
                                    Wdbg(DebugLevel.I, "User can log in. (User is not in disabled list)")
                                    ShowPasswordPrompt(SelectedUser)
                                Else
                                    Wdbg(DebugLevel.W, "User can't log in. (User is in disabled list)")
                                    Write(DoTranslation("User is disabled."), True, ColTypes.Error)
                                    KernelEventManager.RaiseLoginError(SelectedUser, LoginErrorReasons.Disabled)
                                End If
                            Else
                                Write(DoTranslation("The answer must be numeric."), True, ColTypes.Error)
                            End If
                        ElseIf ReadLineReboot.ReadLine.ReadRanToCompletion Then
                            Write(DoTranslation("Please enter a user number."), True, ColTypes.Error)
                        End If
                    Loop
                Else
                    'Generate user list
                    If ShowAvailableUsers Then Write(DoTranslation("Available usernames: {0}"), True, ColTypes.Neutral, String.Join(", ", UsersList))

                    'Prompt user to login
                    If Not String.IsNullOrWhiteSpace(UsernamePrompt) Then
                        Write(ProbePlaces(UsernamePrompt), False, ColTypes.Input)
                    Else
                        Write(DoTranslation("Username: "), False, ColTypes.Input)
                    End If
                    Dim answeruser As String = ReadLine()

                    'Parse input
                    If answeruser.Contains(" ") Then
                        Wdbg(DebugLevel.W, "Spaces found in username.")
                        Write(DoTranslation("Spaces are not allowed."), True, ColTypes.Error)
                        KernelEventManager.RaiseLoginError(answeruser, LoginErrorReasons.Spaces)
                    ElseIf answeruser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
                        Wdbg(DebugLevel.W, "Unknown characters found in username.")
                        Write(DoTranslation("Special characters are not allowed."), True, ColTypes.Error)
                        KernelEventManager.RaiseLoginError(answeruser, LoginErrorReasons.SpecialCharacters)
                    ElseIf Users.ContainsKey(answeruser) Then
                        Wdbg(DebugLevel.I, "Username correct. Finding if the user is disabled...")
                        If Not HasPermission(answeruser, PermissionType.Disabled) Then
                            Wdbg(DebugLevel.I, "User can log in. (User is not in disabled list)")
                            ShowPasswordPrompt(answeruser)
                        Else
                            Wdbg(DebugLevel.W, "User can't log in. (User is in disabled list)")
                            Write(DoTranslation("User is disabled."), True, ColTypes.Error)
                            KernelEventManager.RaiseLoginError(answeruser, LoginErrorReasons.Disabled)
                        End If
                    ElseIf ReadLineReboot.ReadLine.ReadRanToCompletion Then
                        Wdbg(DebugLevel.E, "Username not found.")
                        Write(DoTranslation("Wrong username."), True, ColTypes.Error)
                        KernelEventManager.RaiseLoginError(answeruser, LoginErrorReasons.NotFound)
                    End If
                End If
            End While
        End Sub

        ''' <summary>
        ''' Prompts user for password
        ''' </summary>
        ''' <param name="usernamerequested">A username that is about to be logged in</param>
        Public Sub ShowPasswordPrompt(usernamerequested As String)
            'Prompts user to enter a user's password
            While True
                'Check to see if reboot is requested
                If RebootRequested = True Then
                    Wdbg(DebugLevel.W, "Reboot has been requested. Exiting...")
                    RebootRequested = False
                    Exit Sub
                End If

                'Get the password from dictionary
                Dim UserPassword As String = Users(usernamerequested)

                'Check if there's a password
                If Not UserPassword = GetEmptyHash(Algorithms.SHA256) Then 'No password
                    'Wait for input
                    Wdbg(DebugLevel.I, "Password not empty")
                    If Not String.IsNullOrWhiteSpace(PasswordPrompt) Then
                        Write(ProbePlaces(PasswordPrompt), False, ColTypes.Input)
                    Else
                        Write(DoTranslation("{0}'s password: "), False, ColTypes.Input, usernamerequested)
                    End If

                    'Get input
                    Dim answerpass As String = ReadLineNoInput()

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
                        Write(DoTranslation("Wrong password."), True, ColTypes.Error)
                        KernelEventManager.RaiseLoginError(usernamerequested, LoginErrorReasons.WrongPassword)
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
        Friend Sub SignIn(signedInUser As String)
            'Release lock
            If LockMode Then
                Wdbg(DebugLevel.I, "Releasing lock and getting back to shell...")
                LockMode = False
                KernelEventManager.RaisePostUnlock(DefSaverName)
                Exit Sub
            End If

            'Notifies the kernel that the user has signed in
            LoggedIn = True

            'Sign in to user.
            CurrentUserInfo = New UserInfo(signedInUser)
            If LockMode = True Then LockMode = False
            Wdbg(DebugLevel.I, "Lock released.")
            ShowMOTDOnceFlag = True
            If ShowMAL Then Write(ProbePlaces(MAL), True, ColTypes.Banner)
            ShowHeadlineLogin()

            'Fire event PostLogin
            KernelEventManager.RaisePostLogin(CurrentUser.Username)

            'Initialize shell
            Wdbg(DebugLevel.I, "Shell is being initialized...")
            StartShellForced(ShellType.Shell)
            PurgeShells()
        End Sub

    End Module
End Namespace