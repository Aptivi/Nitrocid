
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

Imports KS.Files.Operations
Imports System.IO
Imports Newtonsoft.Json.Linq

Namespace Shell.ShellBase.Aliases
    Public Module AliasManager

        Friend Aliases As New Dictionary(Of String, String)
        Friend RemoteDebugAliases As New Dictionary(Of String, String)
        Friend FTPShellAliases As New Dictionary(Of String, String)
        Friend MailShellAliases As New Dictionary(Of String, String)
        Friend SFTPShellAliases As New Dictionary(Of String, String)
        Friend TextShellAliases As New Dictionary(Of String, String)
        Friend TestShellAliases As New Dictionary(Of String, String)
        Friend ZIPShellAliases As New Dictionary(Of String, String)
        Friend RSSShellAliases As New Dictionary(Of String, String)
        Friend JsonShellAliases As New Dictionary(Of String, String)
        Friend HTTPShellAliases As New Dictionary(Of String, String)
        Friend HexShellAliases As New Dictionary(Of String, String)
        Friend RARShellAliases As New Dictionary(Of String, String)
        Friend AliasesToBeRemoved As New Dictionary(Of String, ShellType)

        ''' <summary>
        ''' Initializes aliases
        ''' </summary>
        Public Sub InitAliases()
            'Get all aliases from file
            MakeFile(GetKernelPath(KernelPathType.Aliases), False)
            Dim AliasJsonContent As String = File.ReadAllText(GetKernelPath(KernelPathType.Aliases))
            Dim AliasNameToken As JToken = JToken.Parse(If(Not String.IsNullOrEmpty(AliasJsonContent), AliasJsonContent, "{}"))
            Dim AliasCmd, ActualCmd As String
            Dim AliasType As ShellType

            For Each AliasObject As JObject In AliasNameToken
                AliasCmd = AliasObject("Alias")
                ActualCmd = AliasObject("Command")
                AliasType = AliasObject("Type").ToObject(GetType(ShellType))
                Wdbg(DebugLevel.I, "Adding ""{0}"" and ""{1}"" from Aliases.json to {2} list...", AliasCmd, ActualCmd, AliasType.ToString)
                Dim TargetAliasList = GetAliasesListFromType(AliasType)
                TargetAliasList.AddOrModify(AliasCmd, ActualCmd)
            Next
        End Sub

        ''' <summary>
        ''' Saves aliases
        ''' </summary>
        Public Sub SaveAliases()
            'Save all aliases
            For Each Shell As ShellType In [Enum].GetValues(GetType(ShellType))
                SaveAliasesInternal(Shell)
            Next
        End Sub

        Friend Sub SaveAliasesInternal(ShellType As ShellType)
            'Get all aliases from file
            MakeFile(GetKernelPath(KernelPathType.Aliases), False)
            Dim AliasJsonContent As String = File.ReadAllText(GetKernelPath(KernelPathType.Aliases))
            Dim AliasNameToken As JArray = JArray.Parse(If(Not String.IsNullOrEmpty(AliasJsonContent), AliasJsonContent, "[]"))

            'Save the alias
            Dim ShellAliases = GetAliasesListFromType(ShellType)
            For i As Integer = 0 To ShellAliases.Count - 1
                Wdbg(DebugLevel.I, "Adding ""{0}"" and ""{1}"" from list to Aliases.json with type {2}...", ShellAliases.Keys(i), ShellAliases.Values(i), ShellType.ToString)
                Dim AliasObject As New JObject From {
                    {"Alias", ShellAliases.Keys(i)},
                    {"Command", ShellAliases.Values(i)},
                    {"Type", ShellType.ToString}
                }
                If Not DoesAliasExist(ShellAliases.Keys(i), ShellType) Then AliasNameToken.Add(AliasObject)
            Next

            'Save changes
            File.WriteAllText(GetKernelPath(KernelPathType.Aliases), JsonConvert.SerializeObject(AliasNameToken, Formatting.Indented))
        End Sub

        ''' <summary>
        ''' Manages the alias
        ''' </summary>
        ''' <param name="mode">Either add or rem</param>
        ''' <param name="Type">Alias type (Shell or Remote Debug)</param>
        ''' <param name="AliasCmd">A specified alias</param>
        ''' <param name="DestCmd">A destination command (target)</param>
        Public Sub ManageAlias(mode As String, Type As ShellType, AliasCmd As String, Optional DestCmd As String = "")
            If [Enum].IsDefined(GetType(ShellType), Type) Then
                If mode = "add" Then
                    'User tries to add an alias.
                    Try
                        AddAlias(AliasCmd, DestCmd, Type)
                        Write(DoTranslation("You can now run ""{0}"" as a command: ""{1}""."), True, ColTypes.Neutral, AliasCmd, DestCmd)
                    Catch ex As Exception
                        Wdbg(DebugLevel.E, "Failed to add alias. Stack trace written using WStkTrc(). {0}", ex.Message)
                        WStkTrc(ex)
                        Write(ex.Message, True, ColTypes.Error)
                    End Try
                ElseIf mode = "rem" Then
                    'User tries to remove an alias
                    Try
                        RemoveAlias(AliasCmd, Type)
                        PurgeAliases()
                        Write(DoTranslation("Removed alias {0} successfully."), True, ColTypes.Neutral, AliasCmd)
                    Catch ex As Exception
                        Wdbg(DebugLevel.E, "Failed to remove alias. Stack trace written using WStkTrc(). {0}", ex.Message)
                        WStkTrc(ex)
                        Write(ex.Message, True, ColTypes.Error)
                    End Try
                Else
                    Wdbg(DebugLevel.E, "Mode {0} was neither add nor rem.", mode)
                    Write(DoTranslation("Invalid mode {0}."), True, ColTypes.Error, mode)
                End If

                'Save all aliases
                SaveAliases()
            Else
                Wdbg(DebugLevel.E, "Type {0} not found.", Type)
                Write(DoTranslation("Invalid type {0}."), True, ColTypes.Error, Type)
            End If
        End Sub

        ''' <summary>
        ''' Adds alias to kernel
        ''' </summary>
        ''' <param name="SourceAlias">A command to be aliased. It should exist in both shell and remote debug.</param>
        ''' <param name="Destination">A one-word command to alias to.</param>
        ''' <param name="Type">Alias type, whether it be shell or remote debug.</param>
        ''' <returns>True if successful, False if unsuccessful.</returns>
        ''' <exception cref="Exceptions.AliasInvalidOperationException"></exception>
        ''' <exception cref="Exceptions.AliasNoSuchCommandException"></exception>
        ''' <exception cref="Exceptions.AliasAlreadyExistsException"></exception>
        ''' <exception cref="Exceptions.AliasNoSuchTypeException"></exception>
        Public Function AddAlias(SourceAlias As String, Destination As String, Type As ShellType) As Boolean
            If [Enum].IsDefined(GetType(ShellType), Type) Then
                If SourceAlias = Destination Then
                    Wdbg(DebugLevel.I, "Assertion succeeded: {0} = {1}", SourceAlias, Destination)
                    Throw New Exceptions.AliasInvalidOperationException(DoTranslation("Alias can't be the same name as a command."))
                ElseIf Not IsCommandFound(Destination) Then
                    Wdbg(DebugLevel.W, "{0} not found in all the command lists", Destination)
                    Throw New Exceptions.AliasNoSuchCommandException(DoTranslation("Command not found to alias to {0}."), Destination)
                ElseIf DoesAliasExist(SourceAlias, Type) Then
                    Wdbg(DebugLevel.W, "Alias {0} already found", SourceAlias)
                    Throw New Exceptions.AliasAlreadyExistsException(DoTranslation("Alias already found: {0}"), SourceAlias)
                Else
                    Wdbg(DebugLevel.W, "Aliasing {0} to {1}", SourceAlias, Destination)
                    Dim TargetAliasList = GetAliasesListFromType(Type)
                    TargetAliasList.Add(SourceAlias, Destination)
                    Return True
                End If
            Else
                Wdbg(DebugLevel.E, "Type {0} not found.", Type)
                Throw New Exceptions.AliasNoSuchTypeException(DoTranslation("Invalid type {0}."), Type)
            End If
            Return False
        End Function

        ''' <summary>
        ''' Removes alias from kernel
        ''' </summary>
        ''' <param name="TargetAlias">An alias that needs to be removed.</param>
        ''' <param name="Type">Alias type, whether it be shell or remote debug.</param>
        ''' <returns>True if successful, False if unsuccessful.</returns>
        ''' <exception cref="Exceptions.AliasNoSuchAliasException"></exception>
        ''' <exception cref="Exceptions.AliasNoSuchTypeException"></exception>
        Public Function RemoveAlias(TargetAlias As String, Type As ShellType) As Boolean
            'Variables
            Dim TargetAliasList = GetAliasesListFromType(Type)

            'Do the action!
            If TargetAliasList.ContainsKey(TargetAlias) Then
                Dim Aliased As String = TargetAliasList(TargetAlias)
                Wdbg(DebugLevel.I, "aliases({0}) is found. That makes it {1}", TargetAlias, Aliased)
                TargetAliasList.Remove(TargetAlias)
                AliasesToBeRemoved.Add($"{AliasesToBeRemoved.Count + 1}-{TargetAlias}", Type)
                Return True
            Else
                Wdbg(DebugLevel.W, "{0} is not found in the {1} aliases", TargetAlias, Type.ToString)
                Throw New Exceptions.AliasNoSuchAliasException(DoTranslation("Alias {0} is not found to be removed."), TargetAlias)
            End If
            Return False
        End Function

        ''' <summary>
        ''' Purge aliases that are removed from config
        ''' </summary>
        Public Sub PurgeAliases()
            'Get all aliases from file
            MakeFile(GetKernelPath(KernelPathType.Aliases), False)
            Dim AliasJsonContent As String = File.ReadAllText(GetKernelPath(KernelPathType.Aliases))
            Dim AliasNameToken As JArray = JArray.Parse(If(Not String.IsNullOrEmpty(AliasJsonContent), AliasJsonContent, "[]"))

            'Purge aliases that are to be removed from config
            For Each TargetAliasItem As String In AliasesToBeRemoved.Keys
                For RemovedAliasIndex As Integer = AliasNameToken.Count - 1 To 0 Step -1
                    Dim TargetAliasType As ShellType = AliasesToBeRemoved(TargetAliasItem)
                    Dim TargetAlias As String = TargetAliasItem.Substring(TargetAliasItem.IndexOf("-") + 1)
                    If AliasNameToken(RemovedAliasIndex)("Alias") = TargetAlias And AliasNameToken(RemovedAliasIndex)("Type") = TargetAliasType.ToString Then AliasNameToken.RemoveAt(RemovedAliasIndex)
                Next
            Next

            'Clear the "to be removed" list of aliases
            AliasesToBeRemoved.Clear()

            'Save the changes
            File.WriteAllText(GetKernelPath(KernelPathType.Aliases), JsonConvert.SerializeObject(AliasNameToken, Formatting.Indented))
        End Sub

        ''' <summary>
        ''' Checks to see if the specified alias exists.
        ''' </summary>
        ''' <param name="TargetAlias">The existing alias</param>
        ''' <param name="Type">The alias type</param>
        ''' <returns>True if it exists; false if it doesn't exist</returns>
        Public Function DoesAliasExist(TargetAlias As String, Type As ShellType) As Boolean
            'Get all aliases from file
            MakeFile(GetKernelPath(KernelPathType.Aliases), False)
            Dim AliasJsonContent As String = File.ReadAllText(GetKernelPath(KernelPathType.Aliases))
            Dim AliasNameToken As JArray = JArray.Parse(If(Not String.IsNullOrEmpty(AliasJsonContent), AliasJsonContent, "[]"))

            'Check to see if the specified alias exists
            For Each AliasName As JObject In AliasNameToken
                If AliasName("Alias") = TargetAlias And AliasName("Type") = Type.ToString Then Return True
            Next
            Return False
        End Function

        ''' <summary>
        ''' Gets the aliases list from the shell type
        ''' </summary>
        ''' <param name="ShellType">Selected shell type</param>
        Public Function GetAliasesListFromType(ShellType As ShellType) As Dictionary(Of String, String)
            Select Case ShellType
                Case ShellType.Shell
                    Return Aliases
                Case ShellType.RemoteDebugShell
                    Return RemoteDebugAliases
                Case ShellType.FTPShell
                    Return FTPShellAliases
                Case ShellType.SFTPShell
                    Return SFTPShellAliases
                Case ShellType.MailShell
                    Return MailShellAliases
                Case ShellType.TextShell
                    Return TextShellAliases
                Case ShellType.TestShell
                    Return TestShellAliases
                Case ShellType.ZIPShell
                    Return ZIPShellAliases
                Case ShellType.RSSShell
                    Return RSSShellAliases
                Case ShellType.JsonShell
                    Return JsonShellAliases
                Case ShellType.HTTPShell
                    Return HTTPShellAliases
                Case ShellType.HexShell
                    Return HexShellAliases
                Case ShellType.RARShell
                    Return RARShellAliases
                Case Else
                    Wdbg(DebugLevel.E, "Type {0} not found.", ShellType)
                    Throw New Exceptions.AliasNoSuchTypeException(DoTranslation("Invalid type {0}."), ShellType)
            End Select
        End Function

    End Module
End Namespace
