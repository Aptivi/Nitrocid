
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

Imports SharpCompress.Archives.Zip
Imports KS.Misc.ZipFile

Namespace Shell.Shells.Zip.Commands
    ''' <summary>
    ''' Lists ZIP file entries
    ''' </summary>
    ''' <remarks>
    ''' If you want to know what this ZIP file contains, you can use this command to list all the files and folders included in the archive.
    ''' </remarks>
    Class ZipShell_ListCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Entries As List(Of ZipArchiveEntry)
            If ListArgs?.Length > 0 Then
                Wdbg(DebugLevel.I, "Listing entries with {0} as target directory", ListArgs(0))
                Entries = ListZipEntries(ListArgs(0))
            Else
                Wdbg(DebugLevel.I, "Listing entries with current directory as target directory")
                Entries = ListZipEntries(ZipShell_CurrentArchiveDirectory)
            End If
            For Each Entry As ZipArchiveEntry In Entries
                Write("- {0}: ", False, ColTypes.ListEntry, Entry.Key)
                If Not Entry.IsDirectory Then 'Entry is a file
                    Write("{0} ({1})", True, ColTypes.ListValue, Entry.CompressedSize.FileSizeToString, Entry.Size.FileSizeToString)
                Else
                    Console.WriteLine()
                End If
            Next
        End Sub

    End Class
End Namespace
