
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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
                RebootRequested = False
                Exit Sub
            End If

            'Fire event PreLogin
            EventManager.RaisePreLogin()

            'Extremely rare under normal conditions except if modded: Check to see if there are any users
            If userword.Count = 0 Then 'Check if user amount is zero
                Throw New EventsAndExceptions.NullUsersException(DoTranslation("There is no more users remaining in the list.", currentLang))
            End If

            'Prompts user to log-in
            If clsOnLogin = True Then
                Console.Clear()
            End If

            'Generate user list
            W(DoTranslation("Available usernames: {0}", currentLang), True, ColTypes.Neutral, String.Join(", ", userword.Keys))

            'Login process
            ReadMOTDFromFile(MessageType.MOTD)
            ReadMOTDFromFile(MessageType.MAL)
            If showMOTDOnceFlag = True And showMOTD = True Then
                W(vbNewLine + ProbePlaces(MOTDMessage), False, ColTypes.Input)
            End If
            W(vbNewLine + DoTranslation("Username: ", currentLang), False, ColTypes.Input)
            showMOTDOnceFlag = False
            answeruser = Console.ReadLine()

            'Parse input
            If InStr(answeruser, " ") > 0 Then
                W(DoTranslation("Spaces are not allowed.", currentLang), True, ColTypes.Neutral)
            ElseIf answeruser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
                W(DoTranslation("Special characters are not allowed.", currentLang), True, ColTypes.Neutral)
            ElseIf userword.ContainsKey(answeruser) Then
                If disabledList(answeruser) = False Then
                    ShowPasswordPrompt(answeruser)
                Else
                    W(DoTranslation("User is disabled.", currentLang), True, ColTypes.Neutral)
                End If
            Else
                W(DoTranslation("Wrong username.", currentLang), True, ColTypes.Neutral)
            End If
        End While
    End Sub

    Sub ShowPasswordPrompt(ByVal usernamerequested As String)
        'Error handler
        On Error Resume Next

        'Prompts user to enter a user's password
        While True
            If RebootRequested = True Then
                answerpass = ""
                RebootRequested = False
                Exit Sub
            End If
            answerpass = ""
            Wdbg("User {0} is not disabled", usernamerequested)

            'Get the password from dictionary
            password = userword.Item(usernamerequested)

            'Check if there's the password
            If Not password = Nothing Then
                'Wait for input
                W(DoTranslation("{0}'s password: ", currentLang), False, ColTypes.Input, usernamerequested)

                'Get input
                While True
                    Dim character As Char = Console.ReadKey(True).KeyChar
                    If character = vbCr Or character = vbLf Then
                        Console.WriteLine()
                        Exit While
                    Else
                        answerpass += character
                    End If
                End While
                Dim hashbyte As Byte() = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(answerpass))
                answerpass = GetArraySHA256(hashbyte)

                'Parse password input
                If InStr(answerpass, " ") > 0 Then
                    W(DoTranslation("Spaces are not allowed.", currentLang), True, ColTypes.Neutral)
                    If Not maintenance Then
                        If Not LockMode Then
                            Exit Sub
                        End If
                    End If
                Else
                    If userword.TryGetValue(usernamerequested, password) AndAlso password = answerpass Then
                        'Log-in instantly
                        SignIn(usernamerequested)
                        Exit Sub
                    Else
                        W(DoTranslation("Wrong password.", currentLang), True, ColTypes.Neutral)
                        If Not maintenance Then
                            If Not LockMode Then
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            Else
                'Log-in instantly
                SignIn(usernamerequested)
                Exit Sub
            End If
        End While

    End Sub

    Public Sub SignIn(ByVal signedInUser As String)

        'Release lock
        If LockMode Then
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
        CruserFlag = False
        signedinusrnm = Nothing

        'Sign in to user.
        signedinusrnm = signedInUser
        If LockMode = True Then LockMode = False
        showMOTDOnceFlag = True
        W(ProbePlaces(MAL), True, ColTypes.Neutral)

        'Fire event PostLogin
        EventManager.RaisePostLogin()

        'Initialize shell
        InitializeShell()

    End Sub

End Module
