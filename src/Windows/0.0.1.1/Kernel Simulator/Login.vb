
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

    Public usernamelist As New List(Of String)
    Public passwordlist As New List(Of String)
    Public userword As New Dictionary(Of String, String)()
    Public answeruser
    Public answerpass
    Public password As String
    Public signedinusrnm As String

    Sub initializeMainUsers()

        usernamelist.Add("root")
        usernamelist.Add("useradd")

        passwordlist.Add("toor")
        passwordlist.Add("")

        My.Settings.Passwords.Add("toor")
        My.Settings.Passwords.Add("")

        For Each users As String In usernamelist
            System.Console.Write(users + " ")
        Next

    End Sub

    Public LoginFlag As Boolean

    Sub makeUserDatabase()

        For i As Integer = 0 To usernamelist.Count - 1
            If (userword.TryGetValue(usernamelist(i), passwordlist(i)) = False) Then
                userword.Add(usernamelist(i), My.Settings.Passwords(i))
            End If
        Next

    End Sub
    Sub initializeUsers()                                                                               'Do not confuse with initializeUser. It loads users.

        For Each setUsers As String In My.Settings.Usernames
            usernamelist.Add(setUsers)
        Next
        For Each setPassword As String In My.Settings.Passwords
            passwordlist.Add(setPassword)
        Next
        For Each initUsers As String In usernamelist
            System.Console.Write("usrmgr: User " + initUsers + " has been successfully loaded." + vbNewLine)
        Next
        makeUserDatabase()
        If (LoginFlag = True) Then
            Login.LoginPrompt()
        End If

    End Sub

    Sub initializeUser(ByVal uninitUser As String, Optional ByVal unpassword As String = "")            'Do not confuse with initializeUsers. It initializes user.

        System.Console.Write(uninitUser)
        passwordlist.Add(unpassword)
        usernamelist.Add(uninitUser)
        My.Settings.Usernames.Add(uninitUser)
        If (unpassword = "") Then
            My.Settings.Passwords.Add("")
        Else
            My.Settings.Passwords.Add(unpassword)
        End If
        System.Console.Write(" created." + vbNewLine)

    End Sub

    Sub adduser(ByVal newUser As String, Optional ByVal newPassword As String = "")

        System.Console.Write("usrmgr: " + newUser + " not found. Creating..." + vbNewLine)
        System.Console.Write("usrmgr: Username ")
        If (newPassword = Nothing) Then
            initializeUser(newUser)
        Else
            initializeUser(newUser, newPassword)
        End If

    End Sub

    Sub LoginPrompt()

        System.Console.Write(vbNewLine + vbNewLine + My.Settings.MOTD + vbNewLine + vbNewLine + "login: ")
        answeruser = System.Console.ReadLine()
        showPasswordPrompt(answeruser)

    End Sub

    Sub showPasswordPrompt(ByVal usernamerequested As String)

        On Error Resume Next
        System.Console.Write(vbNewLine + "password for " + usernamerequested + ": ")
        answerpass = System.Console.ReadLine()
        password = userword.Item(usernamerequested)
        If userword.TryGetValue(usernamerequested, password) AndAlso password = answerpass Then
            signIn(usernamerequested)
        Else
            System.Console.Write(vbNewLine + "Wrong username or password." + vbNewLine)
            LoginPrompt()
        End If

    End Sub

    Sub signIn(ByVal signedInUser As String)

        System.Console.Write(vbNewLine + "Logged in successfully as " + signedInUser + "!" + vbNewLine)
        signedinusrnm = signedInUser
        Shell.initializeShell()

    End Sub

End Module
