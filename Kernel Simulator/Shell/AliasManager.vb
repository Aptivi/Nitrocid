
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

Public Module AliasManager

    Public Aliases As New Dictionary(Of String, String)
    Public RemoteDebugAliases As New Dictionary(Of String, String)
    Public Forbidden As String() = {"alias"}
    Private AliasStreamReader As New IO.StreamReader(paths("Aliases"))

    ''' <summary>
    ''' Aliases type
    ''' </summary>
    Enum AliasType
        Shell = 1
        RDebug
    End Enum

    'Alias format: {Alias Type}, {Alias Command}, {Actual Command}
    'Example:      Shell, h, help
    'Example 2:    Remote, e, exit
    ''' <summary>
    ''' Initializes aliases
    ''' </summary>
    Public Sub InitAliases()
        'Get all aliases from file
        While Not AliasStreamReader.EndOfStream
            'Read line
            Dim line As String = AliasStreamReader.ReadLine
            Dim AliasCmd, ActualCmd As String
            If line.StartsWith("Shell, ") Then
                line = line.Replace("Shell, ", "")

                'Add alias to list from file
                AliasCmd = line.Remove(line.IndexOf(","c))
                ActualCmd = line.Substring(line.IndexOf(" "c) + 1)
                If Not Aliases.ContainsKey(AliasCmd) Then
                    Wdbg("I", "Adding ""{0}, {1}"" from aliases.csv to list...", AliasCmd, ActualCmd)
                    Aliases.Add(AliasCmd, ActualCmd)
                End If
            ElseIf line.StartsWith("Remote, ") Then
                line = line.Replace("Remote, ", "")

                'Add alias to list from file
                AliasCmd = line.Remove(line.IndexOf(","c))
                ActualCmd = line.Substring(line.IndexOf(" "c) + 1)
                If Not RemoteDebugAliases.ContainsKey(AliasCmd) Then
                    Wdbg("I", "Adding ""{0}, {1}"" from aliases.csv to list...", AliasCmd, ActualCmd)
                    RemoteDebugAliases.Add(AliasCmd, ActualCmd)
                End If
            Else
                'Invalid type spotted. (General case)
                'If you have aliases.csv which is generated on older versions of KS, it might not be up-to-date, which makes you have to
                'prefix all the entries with the type and the comma. For example, if your aliases.csv looks like:
                '  h, help
                'you should change it so it looks like:
                '  Shell, h, help
                Wdbg("E", "Invalid type {0}", line.Remove(line.IndexOf(","c)))
            End If
        End While
        AliasStreamReader.BaseStream.Seek(0, IO.SeekOrigin.Begin)
    End Sub

    ''' <summary>
    ''' Saves aliases
    ''' </summary>
    Public Sub SaveAliases()
        'Variables
        Dim aliast As New List(Of String)

        'Shell aliases
        For i As Integer = 0 To Aliases.Count - 1
            Wdbg("I", "Adding ""Shell, {0}, {1}"" from list to aliases.csv...", Aliases.Keys(i), Aliases.Values(i))
            aliast.Add($"Shell, {Aliases.Keys(i)}, {Aliases.Values(i)}")
        Next

        'Remote Debug aliases
        For i As Integer = 0 To RemoteDebugAliases.Count - 1
            Wdbg("I", "Adding ""Remote, {0}, {1}"" from list to aliases.csv...", RemoteDebugAliases.Keys(i), RemoteDebugAliases.Values(i))
            aliast.Add($"Remote, {RemoteDebugAliases.Keys(i)}, {RemoteDebugAliases.Values(i)}")
        Next

        'Close the stream reader, write all the lines, then open the stream reader again
        AliasStreamReader.Close()
        IO.File.WriteAllLines(paths("Aliases"), aliast)
        AliasStreamReader = New IO.StreamReader(paths("Aliases"))
    End Sub

    ''' <summary>
    ''' Manages the alias
    ''' </summary>
    ''' <param name="mode">Either add or rem</param>
    ''' <param name="Type">Alias type (Shell or Remote Debug)</param>
    ''' <param name="AliasCmd">A specified alias</param>
    ''' <param name="DestCmd">A destination command (target)</param>
    Public Sub ManageAlias(ByVal mode As String, ByVal Type As AliasType, ByVal AliasCmd As String, Optional ByVal DestCmd As String = "")
        If Type = AliasType.Shell Or Type = AliasType.RDebug Then
            If mode = "add" Then
                'User tries to add an alias.
                If AliasCmd = DestCmd Then
                    W(DoTranslation("Alias can't be the same name as a command.", currentLang), True, ColTypes.Err)
                    Wdbg("I", "Assertion succeeded: {0} = {1}", AliasCmd, DestCmd)
                ElseIf Not availableCommands.Contains(DestCmd) And Not DebugCmds.Contains(DestCmd) Then
                    W(DoTranslation("Command not found to alias to {0}.", currentLang), True, ColTypes.Err, AliasCmd)
                    Wdbg("W", "{0} not found in either list of availableCmds or DebugCmds", DestCmd)
                ElseIf Forbidden.Contains(DestCmd) Then
                    W(DoTranslation("Aliasing {0} to {1} is forbidden.", currentLang), True, ColTypes.Err, DestCmd, AliasCmd)
                    Wdbg("W", "forbid.Contains({0}) = true | No aliasing", DestCmd)
                ElseIf Not Aliases.ContainsKey(AliasCmd) Or Not RemoteDebugAliases.ContainsKey(AliasCmd) Then
                    Wdbg("W", "Assertion failed: {0} = {1}", AliasCmd, DestCmd)
                    If Type = AliasType.Shell Then
                        Aliases.Add(AliasCmd, DestCmd)
                    ElseIf Type = AliasType.RDebug Then
                        RemoteDebugAliases.Add(AliasCmd, DestCmd)
                    End If
                    W(DoTranslation("You can now run ""{0}"" as a command: ""{1}"".", currentLang), True, ColTypes.Neutral, AliasCmd, DestCmd)
                Else
                    Wdbg("W", "Alias {0} already found", AliasCmd)
                    W(DoTranslation("Alias already found: {0}", currentLang), True, ColTypes.Err, AliasCmd)
                End If
            ElseIf mode = "rem" Then
                'user tries to remove an alias
                If Aliases.ContainsKey(AliasCmd) Then
                    DestCmd = Aliases(AliasCmd)
                    Wdbg("I", "aliases({0}) is found. That makes it {1}", AliasCmd, DestCmd)
                    Aliases.Remove(AliasCmd)
                    W(DoTranslation("You can no longer use ""{0}"" as a command ""{1}"".", currentLang), True, ColTypes.Err, AliasCmd, DestCmd)
                Else
                    Wdbg("W", "aliases({0}) is not found", AliasCmd)
                    W(DoTranslation("Alias {0} is not found to be removed.", currentLang), True, ColTypes.Err, AliasCmd)
                End If
            Else
                Wdbg("E", "Mode {0} was neither add nor rem.", mode)
                W(DoTranslation("Invalid mode {0}.", currentLang), True, ColTypes.Err, mode)
            End If

            'Save all aliases
            SaveAliases()
        Else
            Wdbg("E", "Type {0} not found.", Type)
            W(DoTranslation("Invalid type {0}.", currentLang), True, ColTypes.Err, Type)
        End If
    End Sub

    ''' <summary>
    ''' Executes the remote debugger alias
    ''' </summary>
    ''' <param name="aliascmd">Aliased command with arguments</param>
    ''' <param name="SocketStream">A socket stream writer</param>
    ''' <param name="Address">IP Address</param>
    Sub ExecuteRDAlias(ByVal aliascmd As String, ByVal SocketStream As IO.StreamWriter, ByVal Address As String)
        'If this sub worked properly, consider putting it to RemoteDebugShell directly
        Dim FirstWordCmd As String = aliascmd.Split(" "c)(0)
        Dim actualCmd As String = aliascmd.Replace(FirstWordCmd, RemoteDebugAliases(FirstWordCmd))
        ParseCmd(actualCmd, SocketStream, Address)
    End Sub

End Module
