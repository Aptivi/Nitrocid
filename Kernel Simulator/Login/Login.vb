
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Imports System.Security.Cryptography
Imports System.Text

Public Module Login

    'Variables
    Public userword As New Dictionary(Of String, String)()      'List of usernames and passwords
    Public answeruser As String                                 'Input of username
    Public answerpass As String                                 'Input of password
    Public password As String                                   'Password for user we're logging in to
    Public signedinusrnm As String                              'Username that is signed in
    Private showMOTDOnceFlag As Boolean = True                  'Show MOTD every LoginPrompt() session

    Sub LoginPrompt()
        While True
            'Check to see if the reboot is requested
            If RebootRequested = True Then
                Wdbg("W", "Reboot has been requested. Exiting...")
                RebootRequested = False
                Exit Sub
            End If

            'Fire event PreLogin
            EventManager.RaisePreLogin()

            'Extremely rare under normal conditions except if modded: Check to see if there are any users
            If userword.Count = 0 Then 'Check if user amount is zero
                Wdbg("F", "Shell reached rare state, because userword count is 0.")
                Throw New EventsAndExceptions.NullUsersException(DoTranslation("There is no more users remaining in the list.", currentLang))
            End If

            'Clear console if clsOnLogin is set to True (If a user has enabled Clear Screen on Login)
            If clsOnLogin = True Then
                Wdbg("I", "Clearing screen...")
                Console.Clear()
            End If

            'Generate user list
            If ShowAvailableUsers Then W(DoTranslation("Available usernames: {0}", currentLang), True, ColTypes.Neutral, String.Join(", ", userword.Keys))

            'Read MOTD and MAL
            ReadMOTDFromFile(MessageType.MOTD)
            ReadMOTDFromFile(MessageType.MAL)

            'Show MOTD once
            Wdbg("I", "showMOTDOnceFlag = {0}, showMOTD = {1}", showMOTDOnceFlag, showMOTD)
            If showMOTDOnceFlag = True And showMOTD = True Then
                W(vbNewLine + ProbePlaces(MOTDMessage), True, ColTypes.Neutral)
            End If
            showMOTDOnceFlag = False

            'Prompt user to login
            W(DoTranslation("Username: ", currentLang), False, ColTypes.Input)
            answeruser = Console.ReadLine()

            'Parse input
            If InStr(answeruser, " ") > 0 Then
                Wdbg("W", "Spaces found in username.")
                W(DoTranslation("Spaces are not allowed.", currentLang), True, ColTypes.Neutral)
            ElseIf answeruser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
                Wdbg("W", "Unknown characters found in username.")
                W(DoTranslation("Special characters are not allowed.", currentLang), True, ColTypes.Neutral)
            ElseIf userword.ContainsKey(answeruser) Then
                Wdbg("I", "Username correct. Finding if the user is disabled...")
                If disabledList(answeruser) = False Then
                    Wdbg("I", "User can log in. (User is not in disabled list)")
                    ShowPasswordPrompt(answeruser)
                Else
                    Wdbg("W", "User can't log in. (User is in disabled list)")
                    W(DoTranslation("User is disabled.", currentLang), True, ColTypes.Neutral)
                End If
            Else
                Wdbg("E", "Username not found.")
                W(DoTranslation("Wrong username.", currentLang), True, ColTypes.Neutral)
            End If
        End While
    End Sub

    Sub ShowPasswordPrompt(ByVal usernamerequested As String)
        'Error handler
        On Error Resume Next

        'Prompts user to enter a user's password
        While True
            'Check to see if reboot is requested
            answerpass = ""
            If RebootRequested = True Then
                Wdbg("W", "Reboot has been requested. Exiting...")
                RebootRequested = False
                Exit Sub
            End If

            'Get the password from dictionary
            password = userword.Item(usernamerequested)

            'Check if there's a password
            If Not password = "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855" Then 'No password
                'Wait for input
                Wdbg("I", "Password not empty")
                W(DoTranslation("{0}'s password: ", currentLang), False, ColTypes.Input, usernamerequested)

                'Get input
                answerpass = ReadLineNoInput("*"c)
                Console.WriteLine()

                'Compute password hash
                Wdbg("I", "Computing written password hash...")
                answerpass = GetEncryptedString(answerpass, Algorithms.SHA256)

                'Parse password input
                If userword.TryGetValue(usernamerequested, password) AndAlso password = answerpass Then
                    'Log-in instantly
                    Wdbg("I", "Password written correctly. Entering shell...")
                    SignIn(usernamerequested)
                    Exit Sub
                Else
                    Wdbg("I", "Passowrd written wrong...")
                    W(DoTranslation("Wrong password.", currentLang), True, ColTypes.Neutral)
                    If Not maintenance Then
                        If Not LockMode Then
                            Exit Sub
                        End If
                    End If
                End If
            Else
                'Log-in instantly
                Wdbg("I", "Password is empty")
                SignIn(usernamerequested)
                Exit Sub
            End If
        End While

    End Sub

    Public Sub SignIn(ByVal signedInUser As String)

        'Release lock
        If LockMode Then
            Wdbg("I", "Releasing lock and getting back to shell...")
            LockMode = False
            EventManager.RaisePostUnlock()
            Exit Sub
        End If

        'Resets inputs
        answerpass = Nothing
        answeruser = Nothing

        'Resets outputs
        password = Nothing
        LoginFlag = False
        signedinusrnm = Nothing

        'Notifies the kernel that the user has signed in
        LoggedIn = True

        'Sign in to user.
        signedinusrnm = signedInUser
        If LockMode = True Then LockMode = False
        Wdbg("I", "Lock released.")
        showMOTDOnceFlag = True
        W(ProbePlaces(MAL), True, ColTypes.Neutral)

        'Fire event PostLogin
        EventManager.RaisePostLogin()

        'Initialize shell
        Wdbg("I", "Shell is being initialized...")
        InitializeShell()
    End Sub

End Module
