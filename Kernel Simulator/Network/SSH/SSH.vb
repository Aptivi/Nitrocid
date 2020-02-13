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

Module SSH

    Sub InitializeSSH(ByVal Address As String, ByVal Port As Integer, ByVal Username As String)
        'Authentication
        Wdbg("I", "Address: {0}:{1}, Username: {2}", Address, Port, Username)
        W(DoTranslation("Enter the password for {0}: ", currentLang), False, ColTypes.Input, Username)
        Dim Pass As String = ReadLineNoInput()

        'Connection
        Dim SSH As New SshClient(Address, Port, Username, Pass)
        SSH.ConnectionInfo.Timeout = TimeSpan.FromSeconds(30)
        Wdbg("I", "Connecting to {0}...", Address)
        SSH.Connect()

        'Shell creation
        Wdbg("I", "Opening shell...")
        Dim SSHS As Renci.SshNet.Shell = SSH.CreateShell(Console.OpenStandardInput, Console.OpenStandardOutput, Console.OpenStandardError)
        SSHS.Start()

        'Wait until disconnection
        While SSH.IsConnected
            If Console.ReadKey(True).Key = ConsoleKey.Escape Then
                SSHS.Stop()
                SSH.Disconnect()
            End If
        End While
        Wdbg("I", "Connected: {0}", SSH.IsConnected)
        W(vbNewLine + DoTranslation("SSH Disconnected.", currentLang), True, ColTypes.Neutral)
    End Sub

End Module
