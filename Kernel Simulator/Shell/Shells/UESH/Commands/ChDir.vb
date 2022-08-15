
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
Imports KS.Files.Folders

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' You can change your current working directory to another directory
    ''' </summary>
    ''' <remarks>
    ''' You can change your working directory to another directory to execute listing commands, removing files, or creating directories on another directory.
    ''' </remarks>
    Class ChDirCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Try
                SetCurrDir(ListArgs(0))
            Catch sex As Security.SecurityException
                Wdbg(DebugLevel.E, "Security error: {0} ({1})", sex.Message, sex.PermissionType)
                Write(DoTranslation("You are unauthorized to set current directory to {0}: {1}"), True, ColTypes.Error, ListArgs(0), sex.Message)
                WStkTrc(sex)
            Catch ptlex As PathTooLongException
                Wdbg(DebugLevel.I, "Directory length: {0}", NeutralizePath(ListArgs(0)).Length)
                Write(DoTranslation("The path you've specified is too long."), True, ColTypes.Error)
                WStkTrc(ptlex)
            Catch ex As Exception
                Write(DoTranslation("Changing directory has failed: {0}"), True, ColTypes.Error, ex.Message)
                WStkTrc(ex)
            End Try
        End Sub

    End Class
End Namespace
