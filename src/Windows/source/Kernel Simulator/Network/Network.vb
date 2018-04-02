
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

Module Network

    Sub CheckNetworkKernel()

        If System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() Then
            System.Console.WriteLine("net: Network available.")
            System.Console.WriteLine("net: Checking for connectivity...")
            System.Console.Write("net: Write address, or URL: ")
            System.Console.ForegroundColor = ConsoleColor.White
            Dim AnswerPing As String = System.Console.ReadLine()
            System.Console.ResetColor()
            PingTargetKernel(AnswerPing)
        Else
            System.Console.WriteLine("net: Network not available.")
        End If

    End Sub

    Sub CheckNetworkCommand()

        System.Console.Write("net: Write address, or URL: ")
        System.Console.ForegroundColor = ConsoleColor.White
        Dim AnswerPing As String = System.Console.ReadLine()
        System.Console.ResetColor()
        PingTarget(AnswerPing)

    End Sub

    Sub PingTargetKernel(ByVal Address As String)

        On Error GoTo PingError
        If My.Computer.Network.Ping(Address) Then
            System.Console.WriteLine("net: Connection is ready.")
        End If
        Exit Sub
PingError:
        System.Console.WriteLine("net: Connection is not ready, server error, or connection problem.")

    End Sub

    Sub PingTarget(ByVal Address As String)

        On Error GoTo PingError1
        Dim s As New Stopwatch
        s.Start()
        If My.Computer.Network.Ping(Address) Then
            System.Console.WriteLine("net: Got response in {0} ms", s.ElapsedMilliseconds.ToString)
        End If
        s.Stop()
        Exit Sub
PingError1:
        System.Console.WriteLine("net: Site was down, isn't reachable, server error or connection problem.")

    End Sub

End Module
