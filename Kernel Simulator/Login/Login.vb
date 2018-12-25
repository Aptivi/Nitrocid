
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

Public Module Login

    'Variables
    Public userword As New Dictionary(Of String, String)()      'List of usernames and passwords
    Public answeruser As String                                 'Input of username
    Public answerpass As String                                 'Input of password
    Public password As String                                   'Password for user we're logging in to
    Public signedinusrnm As String                              'Username that is signed in
    Private showMOTDOnceFlag As Boolean = True                  'Show MOTD every LoginPrompt() session

    'TODO: Re-write in the final release of 0.0.6 (delayed)

    Sub LoginPrompt()

        'Fire event PreLogin
        EventManager.RaisePreLogin()

        'Prompts user to log-in
        If (clsOnLogin = True) Then
            System.Console.Clear()
        End If
        MOTDMessage = PlaceParse.ProbePlaces(MOTDMessage)

        'Generate user list
        Wln(vbNewLine + DoTranslation("Available usernames: {0}", currentLang), "neutralText", String.Join(", ", userword.Keys))

        'Login process
        If (showMOTD = False) Or (showMOTDOnceFlag = False) Then
            W(vbNewLine + DoTranslation("Username: ", currentLang), "input")
        ElseIf (showMOTDOnceFlag = True And showMOTD = True) Then
            W(vbNewLine + MOTDMessage + vbNewLine + vbNewLine + DoTranslation("Username: ", currentLang), "input")
        End If
        showMOTDOnceFlag = False
        answeruser = System.Console.ReadLine()
        If InStr(CStr(answeruser), " ") > 0 Then
            Wln(DoTranslation("Spaces are not allowed.", currentLang), "neutralText")
            LoginPrompt()
        ElseIf (answeruser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
            Wln(DoTranslation("Special characters are not allowed.", currentLang), "neutralText")
            LoginPrompt()
        Else
            showPasswordPrompt(CStr(answeruser))
        End If

    End Sub

    Sub showPasswordPrompt(ByVal usernamerequested As String)

        'Variables and error handler
        Dim DoneFlag As Boolean = False
        On Error Resume Next

        'Prompts user to enter a user's password
        For Each availableUsers As String In userword.Keys.ToArray
            If availableUsers = answeruser And disabledList(availableUsers) = False Then
                Wdbg("ASSERT({0} = {1}, {2} = False) = True, True", availableUsers, answeruser, disabledList(availableUsers))
                DoneFlag = True
                password = userword.Item(usernamerequested)
                'Check if there's the password
                If Not (password = Nothing) Then
                    W(DoTranslation("{0}'s password: ", currentLang), "input", usernamerequested)
                    answerpass = System.Console.ReadLine()
                    If InStr(CStr(answerpass), " ") > 0 Then
                        Wln(DoTranslation("Spaces are not allowed.", currentLang), "neutralText")
                        If (maintenance = False) Then
                            If (LockMode = True) Then
                                showPasswordPrompt(usernamerequested)
                            Else
                                LoginPrompt()
                            End If
                        Else
                            showPasswordPrompt(usernamerequested)
                        End If
                    Else
                        If userword.TryGetValue(usernamerequested, password) AndAlso password = answerpass Then
                            Wdbg("ASSERT(Parse({0}, {1})) = True | ASSERT({1} = {2}) = True", usernamerequested, password, answerpass)
                            If (LockMode = True) Then
                                LockMode = False
                                EventManager.RaisePostUnlock()
                            End If
                            signIn(usernamerequested)
                        Else
                            Wln(DoTranslation("Wrong password.", currentLang), "neutralText")
                            If (maintenance = False) Then
                                If (LockMode = True) Then
                                    showPasswordPrompt(usernamerequested)
                                Else
                                    LoginPrompt()
                                End If
                            Else
                                showPasswordPrompt(usernamerequested)
                            End If
                        End If
                    End If
                Else
                    'Log-in instantly
                    If (LockMode = True) Then
                        LockMode = False
                        EventManager.RaisePostUnlock()
                        Exit Sub
                    End If
                    signIn(usernamerequested)
                End If
            ElseIf (availableUsers = answeruser And disabledList(availableUsers) = True) Then
                Wln(DoTranslation("User is disabled.", currentLang), "neutralText")
                LoginPrompt()
            End If
        Next
        If DoneFlag = False Then
            Wln(DoTranslation("Wrong username.", currentLang), "neutralText")
            LoginPrompt()
        End If

    End Sub

    Public Sub signIn(ByVal signedInUser As String)

        'Initialize shell, and sign in to user.
        signedinusrnm = signedInUser
        MAL = PlaceParse.ProbePlaces(MAL)
        If LockMode = True Then LockMode = False
        showMOTDOnceFlag = True
        Wln(MAL, "neutralText")
        EventManager.RaisePostLogin()
        Shell.initializeShell()

    End Sub

End Module
