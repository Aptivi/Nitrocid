
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
Imports KS.Files.Querying
Imports KS.Misc.Configuration.Config
Imports KS.Misc.Configuration
Imports Newtonsoft.Json.Linq

Namespace Files.Folders
    Public Module CurrentDirectory

        Private _CurrentDirectory As String = HomePath

        ''' <summary>
        ''' The current directory
        ''' </summary>
        Public Property CurrentDir As String
            Get
                Return _CurrentDirectory
            End Get
            Set
                ThrowOnInvalidPath(Value)
                Value = NeutralizePath(Value)
                If FolderExists(Value) Then
                    _CurrentDirectory = Value
                Else
                    Throw New DirectoryNotFoundException(DoTranslation("Directory {0} not found").FormatString(Value))
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets the current working directory
        ''' </summary>
        ''' <param name="dir">A directory</param>
        ''' <exception cref="DirectoryNotFoundException"></exception>
        Public Sub SetCurrDir(dir As String)
            CurrentDir = dir

            'Raise event
            KernelEventManager.RaiseCurrentDirectoryChanged()
        End Sub

        ''' <summary>
        ''' Tries to set the current working directory
        ''' </summary>
        ''' <param name="dir">A directory</param>
        ''' <returns>True if successful; otherwise, false.</returns>
        ''' <exception cref="DirectoryNotFoundException"></exception>
        Public Function TrySetCurrDir(dir As String) As Boolean
            Try
                SetCurrDir(dir)
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to set current directory: {0}", ex.Message)
                WStkTrc(ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Saves the current directory to configuration
        ''' </summary>
        Public Sub SaveCurrDir()
            Dim Token As JToken = GetConfigCategory(ConfigCategory.Shell)
            SetConfigValue(ConfigCategory.Shell, Token, "Current Directory", CurrentDir)
        End Sub

        ''' <summary>
        ''' Tries to set the current directory to configuration
        ''' </summary>
        ''' <returns>True if successful; otherwise, false.</returns>
        Public Function TrySaveCurrDir() As Boolean
            Try
                SaveCurrDir()
                Return True
            Catch ex As Exception
                WStkTrc(ex)
                Wdbg(DebugLevel.E, "Failed to save current directory: {0}", ex.Message)
                Throw New Exceptions.FilesystemException(DoTranslation("Failed to save current directory: {0}"), ex, ex.Message)
            End Try
            Return False
        End Function

    End Module
End Namespace
