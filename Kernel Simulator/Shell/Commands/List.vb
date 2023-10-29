﻿
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

Namespace Shell.Commands
    Class ListCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim ShowFileDetails As Boolean = ListSwitchesOnly.Contains("-showdetails") OrElse ShowFileDetailsList
            Dim SuppressUnauthorizedMessage As Boolean = ListSwitchesOnly.Contains("-suppressmessages") OrElse SuppressUnauthorizedMessages
            If ListArgsOnly?.Length = 0 Or ListArgsOnly Is Nothing Then
                List(CurrDir, ShowFileDetails, SuppressUnauthorizedMessage)
            Else
                For Each Directory As String In ListArgsOnly
                    Dim direct As String = NeutralizePath(Directory)
                    List(direct, ShowFileDetails, SuppressUnauthorizedMessage)
                Next
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            TextWriterColor.Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            TextWriterColor.Write("  -showdetails: ", False, ColTypes.ListEntry) : TextWriterColor.Write(DoTranslation("Shows the file details in the list"), True, ColTypes.ListValue)
            TextWriterColor.Write("  -suppressmessages: ", False, ColTypes.ListEntry) : TextWriterColor.Write(DoTranslation("Suppresses the annoying ""permission denied"" messages"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace