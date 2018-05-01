
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

    'Variables
    Public userword As New Dictionary(Of String, String)()      'List of usernames and passwords
    Public answeruser As String                                 'Input of username
    Public answerpass As String                                 'Input of password
    Public password As String                                   'Password for user we're logging in to
    Public signedinusrnm As String                              'Username that is signed in
    Public LoginFlag As Boolean                                 'Flag for log-in
    Public MainUserDone As Boolean                              'Main users initialization is done

    Sub LoginPrompt()

        'Prompts user to log-in
        System.Console.Write(vbNewLine + My.Settings.MOTD + vbNewLine + vbNewLine + "Username: ")
        System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
        answeruser = System.Console.ReadLine()
        System.Console.ResetColor()
        If InStr(CStr(answeruser), " ") > 0 Then
            System.Console.WriteLine("Spaces are not allowed.")
            LoginPrompt()
        ElseIf (answeruser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1) Then
            System.Console.WriteLine("Special characters are not allowed.")
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
                DoneFlag = True
                password = userword.Item(usernamerequested)
                'Check if there's the password
                If Not (password = Nothing) Then
                    System.Console.Write("{0}'s password: ", usernamerequested)
                    System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
                    answerpass = System.Console.ReadLine()
                    System.Console.ResetColor()
                    If InStr(CStr(answerpass), " ") > 0 Then
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
            ElseIf (availableUsers = answeruser And disabledList(availableUsers) = True) Then
                System.Console.WriteLine("User is disabled.")
                LoginPrompt()
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
