
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

Imports System.IO
Imports System.Reflection

Namespace Misc.Reflection
    Public Module AssemblyLookup

        Private ReadOnly AssemblyLookupPaths As New List(Of String)

        ''' <summary>
        ''' Adds the path pointing to the dependencies to the assembly search path
        ''' </summary>
        ''' <param name="Path">Path to the dependencies</param>
        Public Sub AddPathToAssemblySearchPath(Path As String)
            Path = NeutralizePath(Path)

            'Add the path to the search path
            If Not AssemblyLookupPaths.Contains(Path) Then
                Wdbg(DebugLevel.I, "Adding path {0} to lookup paths...", Path)
                AssemblyLookupPaths.Add(Path)
            End If
        End Sub

        ''' <summary>
        ''' Loads assembly from the search paths
        ''' </summary>
        ''' <returns>If successful, returns the assembly instance. Otherwise, null.</returns>
        Friend Function LoadFromAssemblySearchPaths(sender As Object, args As ResolveEventArgs) As Assembly
            Dim FinalAssembly As Assembly = Nothing
            Dim DepAssemblyName As String = New AssemblyName(args.Name).Name
            Wdbg(DebugLevel.I, "Requested to load {0}.", args.Name)

            'Try to load assembly from lookup path
            For Each LookupPath As String In AssemblyLookupPaths
                Dim DepAssemblyFilePath As String = Path.Combine(LookupPath, DepAssemblyName + ".dll")
                Try
                    'Try loading
                    Wdbg(DebugLevel.I, "Loading from {0}...", DepAssemblyFilePath)
                    FinalAssembly = Assembly.LoadFrom(DepAssemblyFilePath)
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Failed to load {0} from {1}: {2}", args.Name, DepAssemblyFilePath, ex.Message)
                    WStkTrc(ex)
                    Wdbg(DebugLevel.E, "Trying another path...")
                End Try
            Next

            'Get the final assembly
            Return FinalAssembly
        End Function

    End Module
End Namespace
