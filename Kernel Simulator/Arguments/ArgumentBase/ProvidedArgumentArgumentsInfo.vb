
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Namespace Arguments.ArgumentBase
    Public Class ProvidedArgumentArgumentsInfo

        ''' <summary>
        ''' Target kernel argument that the user executed in shell
        ''' </summary>
        Public ReadOnly Property Argument As String
        ''' <summary>
        ''' Text version of the provided arguments and switches
        ''' </summary>
        Public ReadOnly Property ArgumentsText As String
        ''' <summary>
        ''' List version of the provided arguments and switches
        ''' </summary>
        Public ReadOnly Property FullArgumentsList As String()
        ''' <summary>
        ''' List version of the provided arguments
        ''' </summary>
        Public ReadOnly Property ArgumentsList As String()
        ''' <summary>
        ''' List version of the provided switches
        ''' </summary>
        Public ReadOnly Property SwitchesList As String()
        ''' <summary>
        ''' Checks to see if the required arguments are provided
        ''' </summary>
        Public ReadOnly Property RequiredArgumentsProvided As Boolean

        ''' <summary>
        ''' Makes a new instance of the kernel argument argument info with the user-provided command text
        ''' </summary>
        ''' <param name="ArgumentText">Kernel argument text that the user provided</param>
        ''' <param name="ArgumentType">Kernel argument type. Consult the <see cref="ArgumentType"/> enum for information about supported shells.</param>
        Friend Sub New(ArgumentText As String, ArgumentType As ArgumentType)
            Dim Argument As String
            Dim RequiredArgumentsProvided As Boolean = True
            Dim KernelArguments As Dictionary(Of String, ArgumentInfo) = AvailableArgs

            'Change the available commands list according to command type
            Select Case ArgumentType
                Case ArgumentType.KernelArgs
                    KernelArguments = AvailableArgs
                Case ArgumentType.CommandLineArgs
                    KernelArguments = AvailableCMDLineArgs
                Case ArgumentType.PreBootCommandLineArgs
                    KernelArguments = AvailablePreBootCMDLineArgs
            End Select

            'Get the index of the first space (Used for step 3)
            Dim index As Integer = ArgumentText.IndexOf(" ")
            If index = -1 Then index = ArgumentText.Length
            Wdbg(DebugLevel.I, "Index: {0}", index)

            'Split the requested command string into words
            Dim words() As String = ArgumentText.Split({" "c})
            For i As Integer = 0 To words.Length - 1
                Wdbg(DebugLevel.I, "Word {0}: {1}", i + 1, words(i))
            Next
            Argument = words(0)

            'Get the string of arguments
            Dim strArgs As String = ArgumentText.Substring(index)
            Wdbg(DebugLevel.I, "Prototype strArgs: {0}", strArgs)
            If Not index = ArgumentText.Length Then strArgs = strArgs.Substring(1)
            Wdbg(DebugLevel.I, "Finished strArgs: {0}", strArgs)

            'Split the arguments with enclosed quotes and set the required boolean variable
            Dim EnclosedArgs As List(Of String) = strArgs.SplitEncloseDoubleQuotes(" ")?.ToList
            If EnclosedArgs IsNot Nothing Then
                RequiredArgumentsProvided = EnclosedArgs?.Count >= KernelArguments(Argument).MinimumArguments
            ElseIf KernelArguments(Argument).ArgumentsRequired And EnclosedArgs Is Nothing Then
                RequiredArgumentsProvided = False
            End If
            If EnclosedArgs IsNot Nothing Then Wdbg(DebugLevel.I, "Arguments parsed: " + String.Join(", ", EnclosedArgs))

            'Separate the arguments from the switches
            Dim FinalArgs As New List(Of String)
            Dim FinalSwitches As New List(Of String)
            If EnclosedArgs IsNot Nothing Then
                For Each EnclosedArg As String In EnclosedArgs
                    If EnclosedArg.StartsWith("-") Then
                        FinalSwitches.Add(EnclosedArg)
                    Else
                        FinalArgs.Add(EnclosedArg)
                    End If
                Next
            End If

            'Install the parsed values to the new class instance
            FullArgumentsList = EnclosedArgs?.ToArray
            ArgumentsList = FinalArgs.ToArray
            SwitchesList = FinalSwitches.ToArray
            ArgumentsText = strArgs
            Me.Argument = Argument
            Me.RequiredArgumentsProvided = RequiredArgumentsProvided
        End Sub

    End Class
End Namespace