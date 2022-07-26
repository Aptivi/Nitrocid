
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

Imports KS.Network.RemoteDebug
Imports KS.Shell.ShellBase.Shells

Namespace Shell.ShellBase.Aliases
    Module AliasExecutor

        ''' <summary>
        ''' Translates alias to actual command, preserving arguments
        ''' </summary>
        ''' <param name="aliascmd">Specifies the alias with arguments</param>
        Sub ExecuteAlias(aliascmd As String, ShellType As ShellType)
            Dim AliasesList As Dictionary(Of String, String) = GetAliasesListFromType(ShellType)
            Dim FirstWordCmd As String = aliascmd.SplitEncloseDoubleQuotes(" ")(0)
            Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, AliasesList(FirstWordCmd))
            Wdbg(DebugLevel.I, "Actual command: {0}", actualCmd)
            Dim Params As New ExecuteCommandThreadParameters(actualCmd, ShellType, Nothing)
            Dim StartCommandThread As KernelThread = ShellStack(ShellStack.Count - 1).ShellCommandThread
            StartCommandThread.Start(Params)
            StartCommandThread.Wait()
            StartCommandThread.Stop()
        End Sub

        ''' <summary>
        ''' Executes the remote debugger alias
        ''' </summary>
        ''' <param name="aliascmd">Aliased command with arguments</param>
        ''' <param name="SocketStream">A socket stream writer</param>
        ''' <param name="Address">IP Address</param>
        Sub ExecuteRDAlias(aliascmd As String, SocketStream As IO.StreamWriter, Address As String)
            Dim FirstWordCmd As String = aliascmd.Split(" "c)(0)
            Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, RemoteDebugAliases(FirstWordCmd))
            ParseCmd(actualCmd, SocketStream, Address)
        End Sub

    End Module
End Namespace
