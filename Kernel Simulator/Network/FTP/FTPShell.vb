
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

Imports System
Imports System.IO
Imports System.Reflection

Public Module FTPShell

    Public ftpstream As FtpWebRequest
    Public availftpcmds As String() = {"binary", "bin", "connect", "changelocaldir", "cdl", "changeremotedir", "cdr", "currlocaldir", "pwdl", _
                                       "currremotedir", "pwdr", "delete", "del", "disconnect", "download", "get", "exit", "help", _
                                       "listlocal", "lsl", "listremote", "lsr", "passive", "ssl", "text", "txt", "upload", "put"}
    Public connected As Boolean = False
    Private initialized As Boolean = False
    Public ftpsite As String
    Public currDirect As String 'Current Local Directory
    Public currentremoteDir As String 'Current Remote Directory
    Public user As String
    Public pass As String
    Private strcmd As String
    Public ftpexit As Boolean = False
    Private cwdinitializedandset As Boolean = False
    Public Passive As Boolean = False
    Public SSL As Boolean = False
    Public Binary As Boolean = False

    'The FTPWebRequest does not issue CWD (Change Working Directory) in .NET Framework 4+, so we have to use below:
    Private Sub InitiateCWD()

        'Send a message
        Wln("Injecting CWD...", "neutralText")
        Wdbg(".NET Framework 4+ detected! Adding CWD...", True)

        'Get the type of FtpWebRequest
        Dim requestType As Type = GetType(FtpWebRequest)
        Wdbg("FtpWebRequest = {0}", True, requestType)

        'Get all method information and known methods
        Dim methodInfoField As FieldInfo = requestType.GetField("m_MethodInfo", BindingFlags.NonPublic Or BindingFlags.Instance)
        Dim methodInfoType As Type = methodInfoField.FieldType
        Dim knownMethodsField As FieldInfo = methodInfoType.GetField("KnownMethodInfo", BindingFlags.Static Or BindingFlags.NonPublic)
        Dim knownMethodsArray As Array = CType(knownMethodsField.GetValue(Nothing), Array)

        'Get all flags
        Dim flagsField As FieldInfo = methodInfoType.GetField("Flags", BindingFlags.NonPublic Or BindingFlags.Instance)

        'Create a flag that executes CWD
        Dim MustChangeWorkingDirectoryToPath As Integer = &H100
        Wdbg(MustChangeWorkingDirectoryToPath, True)

        'Set all known methods to have the newly-created flag
        For Each KnownMethod As Object In knownMethodsArray
            Wdbg("{0} is about to be set", True, KnownMethod)
            Dim flags As Integer = CInt(Math.Truncate(flagsField.GetValue(KnownMethod)))
            flags = flags Or MustChangeWorkingDirectoryToPath
            flagsField.SetValue(KnownMethod, flags)
        Next KnownMethod

        'CWD is now injected to all methods and every single request will execute CWD
        Wdbg("CWD injected", True)
        cwdinitializedandset = True

    End Sub

    Public Sub InitiateShell()

        'Inject CWD into all methods
        If (cwdinitializedandset = False) Then InitiateCWD()

        'Make a new request everytime a request is done to prevent InvalidOperationException
        If (connected = True) Then
            ftpstream = WebRequest.Create(ftpsite + currentremoteDir)
            ftpstream.Credentials = New NetworkCredential(user, pass)
            ftpstream.Timeout = 60000
            If Binary = True Then ftpstream.UseBinary = True Else ftpstream.UseBinary = False
            If Passive = True Then ftpstream.UsePassive = True Else ftpstream.UsePassive = False
        End If

        'Complete initialization
        If (initialized = False) Then
            If (EnvironmentOSType.Contains("Unix")) Then
                currDirect = Environ("HOME")
            Else
                currDirect = Environ("USERPROFILE")
            End If
            Binary = True : Passive = True
            initialized = True
        End If

        'Check if the shell is going to exit
        If (ftpexit = True) Then
            connected = False
            ftpsite = ""
            currDirect = ""
            currentremoteDir = ""
            user = ""
            pass = ""
            strcmd = ""
            ftpexit = False
            initialized = False
            Exit Sub
        End If

        'Prompt for command
        If (connected = True And Passive) Then
            W("[", "def") : W("{0}", "userName", user) : W("@", "def") : W("{0}", "hostName", ftpsite) : W("(PASSIVE)", "neutralText") : W("]{0} ", "def", currentremoteDir)
        ElseIf (connected = True And Passive = False) Then
            W("[", "def") : W("{0}", "userName", user) : W("@", "def") : W("{0}", "hostName", ftpsite) : W("(ACTIVE)", "neutralText") : W("]{0} ", "def", currentremoteDir)
        Else
            W("{0}> ", "def", currDirect)
        End If
        DisposeExit.DisposeAll()
        If (ColoredShell = True) Then System.Console.ForegroundColor = CType(inputColor, ConsoleColor)
        strcmd = System.Console.ReadLine()
        GetLine()

    End Sub

    Public Sub GetLine()

        Dim words As String() = strcmd.Split({" "c})
        If (availftpcmds.Contains(words(0))) Then
            FTPGetCommand.ExecuteCommand(strcmd)
        ElseIf (words(0) <> "") Then
            Wln("FTP message: The requested command {0} is not found. See 'help' for a list of available commands specified on FTP shell.", "neutralText", strcmd)
        End If
        FTPShell.InitiateShell()

    End Sub

End Module
