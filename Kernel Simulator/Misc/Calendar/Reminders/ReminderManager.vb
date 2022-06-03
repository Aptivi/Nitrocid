
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

Imports System.IO
Imports System.Xml.Serialization
Imports System.Threading
Imports KS.Misc.Notifications

Namespace Misc.Calendar.Reminders
    Public Module ReminderManager

        Public Reminders As New List(Of ReminderInfo)
        Public CurrentReminderImportance As NotifPriority = NotifPriority.Low
        Public ReminderThread As New KernelThread("Reminder Thread", False, AddressOf ReminderListen)
        Friend ReminderManagerLock As New Object

        ''' <summary>
        ''' Listens for reminders and notifies the user
        ''' </summary>
        Private Sub ReminderListen()
            While ReminderThread.IsAlive
                Try
                    Thread.Sleep(100)
                    SyncLock ReminderManagerLock
                        For ReminderIndex As Integer = 0 To Reminders.Count - 1
                            Dim ReminderInstance As ReminderInfo = Reminders(ReminderIndex)
                            Dim CurrentDate As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, Date.Now.Hour, Date.Now.Minute, Date.Now.Second)
                            If Date.Now >= ReminderInstance.ReminderDate Then
                                ReminderInstance.NotifyReminder()
                            End If
                        Next
                    End SyncLock
                Catch ex As ThreadInterruptedException
                    Wdbg(DebugLevel.I, "Aborting reminder listener...")
                    Exit Sub
                End Try
            End While
        End Sub

        ''' <summary>
        ''' Adds the reminder to the list (calendar will mark the day with parentheses)
        ''' </summary>
        ''' <param name="ReminderDate">Reminder date and time</param>
        ''' <param name="ReminderTitle">Reminder title</param>
        Public Sub AddReminder(ReminderDate As Date, ReminderTitle As String)
            AddReminder(ReminderDate, ReminderTitle, CurrentReminderImportance)
        End Sub

        ''' <summary>
        ''' Adds the reminder to the list (calendar will mark the day with parentheses)
        ''' </summary>
        ''' <param name="ReminderDate">Reminder date and time</param>
        ''' <param name="ReminderTitle">Reminder title</param>
        ''' <param name="ReminderImportance">Reminder importance</param>
        Public Sub AddReminder(ReminderDate As Date, ReminderTitle As String, ReminderImportance As NotifPriority)
            If String.IsNullOrWhiteSpace(ReminderTitle) Then ReminderTitle = DoTranslation("Untitled reminder")
            Dim Reminder As New ReminderInfo() With {
                .ReminderTitle = ReminderTitle,
                .ReminderImportance = ReminderImportance,
                .ReminderDate = ReminderDate
            }
            AddReminder(Reminder)
        End Sub

        ''' <summary>
        ''' Adds the reminder to the list (calendar will mark the day with parentheses)
        ''' </summary>
        ''' <param name="Reminder">Reminder info instance</param>
        Friend Sub AddReminder(Reminder As ReminderInfo)
            Reminders.Add(Reminder)
        End Sub

        ''' <summary>
        ''' Removes the reminder from the list
        ''' </summary>
        ''' <param name="ReminderDate">Reminder date and time</param>
        ''' <param name="ReminderId">Reminder ID</param>
        Public Sub RemoveReminder(ReminderDate As Date, ReminderId As Integer)
            Dim ReminderIndex As Integer = ReminderId - 1
            Dim Reminder As ReminderInfo = Reminders(ReminderIndex)
            If Reminder.ReminderDate = ReminderDate Then
                Reminders.Remove(Reminder)
            End If
        End Sub

        ''' <summary>
        ''' List all the reminders
        ''' </summary>
        Public Sub ListReminders()
            For Each Reminder As ReminderInfo In Reminders
                Write("- {0}: ", False, ColTypes.ListEntry, Reminder.ReminderDate)
                Write(Reminder.ReminderTitle, True, ColTypes.ListValue)
            Next
        End Sub

        ''' <summary>
        ''' Loads all the reminders from the KSReminders directory and adds them to the reminder list
        ''' </summary>
        Public Sub LoadReminders()
            MakeDirectory(GetKernelPath(KernelPathType.Reminders), False)
            Dim ReminderFiles As List(Of String) = Directory.EnumerateFileSystemEntries(GetKernelPath(KernelPathType.Reminders), "*", SearchOption.AllDirectories).ToList
            Wdbg(DebugLevel.I, "Got {0} reminders.", ReminderFiles.Count)

            'Load all the reminders
            For Each ReminderFile As String In ReminderFiles
                Dim LoadedReminder As ReminderInfo = LoadReminder(ReminderFile)
                AddReminder(LoadedReminder)
            Next
        End Sub

        ''' <summary>
        ''' Loads an reminder file
        ''' </summary>
        ''' <param name="ReminderFile">Reminder file</param>
        ''' <returns>A converted reminder info instance. null if unsuccessful.</returns>
        Public Function LoadReminder(ReminderFile As String) As ReminderInfo
            SyncLock ReminderManagerLock
                ThrowOnInvalidPath(ReminderFile)
                ReminderFile = NeutralizePath(ReminderFile)
                Wdbg(DebugLevel.I, "Loading reminder {0}...", ReminderFile)

                'If file exists, convert the file to the reminder instance
                If FileExists(ReminderFile) Then
                    Dim Converter As New XmlSerializer(GetType(ReminderInfo))
                    Dim ReminderFileStream As New FileStream(ReminderFile, FileMode.Open)
                    Wdbg(DebugLevel.I, "Opened stream [{0}]. Converting...", ReminderFileStream.Length)
                    Dim ConvertedReminder As ReminderInfo = DirectCast(Converter.Deserialize(ReminderFileStream), ReminderInfo)
                    Wdbg(DebugLevel.I, "Converted!")
                    ReminderFileStream.Close()
                    Return ConvertedReminder
                Else
                    Wdbg(DebugLevel.E, "File doesn't exist!")
                End If
                Return Nothing
            End SyncLock
        End Function

        ''' <summary>
        ''' Saves all the reminders from the reminder list to their individual files
        ''' </summary>
        Public Sub SaveReminders()
            SaveReminders(GetKernelPath(KernelPathType.Reminders), SaveEventsRemindersDestructively)
        End Sub

        ''' <summary>
        ''' Saves all the reminders from the reminder list to their individual files
        ''' </summary>
        Public Sub SaveReminders(Path As String, Destructive As Boolean)
            ThrowOnInvalidPath(Path)
            Path = NeutralizePath(Path)
            Wdbg(DebugLevel.I, "Saving reminders to {0}...", Path)

            'Remove all events from path, if running destructively
            If Destructive Then
                Dim ReminderFiles As String() = Directory.EnumerateFiles(Path, "*", SearchOption.AllDirectories)
                Dim ReminderFolders As String() = Directory.EnumerateDirectories(Path, "*", SearchOption.AllDirectories)

                'First, remove all files
                For Each FilePath As String In ReminderFiles
                    RemoveFile(FilePath)
                Next

                'Then, remove all empty folders
                For Each FolderPath As String In ReminderFolders
                    RemoveDirectory(FolderPath)
                Next
            End If

            'Enumerate through every reminder and save them
            For ReminderIndex As Integer = 0 To Reminders.Count - 1
                Dim ReminderInstance As ReminderInfo = Reminders(ReminderIndex)
                Dim ReminderFileName As String = $"[{ReminderIndex}] {ReminderInstance.ReminderTitle}.ksreminder"
                Wdbg(DebugLevel.I, "Reminder file name: {0}...", ReminderFileName)
                Dim ReminderFilePath As String = NeutralizePath(ReminderFileName, Path)
                Wdbg(DebugLevel.I, "Reminder file path: {0}...", ReminderFilePath)
                SaveReminder(ReminderInstance, ReminderFilePath)
            Next
        End Sub

        ''' <summary>
        ''' Saves an reminder to a file
        ''' </summary>
        Public Sub SaveReminder(ReminderInstance As ReminderInfo)
            SaveReminder(ReminderInstance, GetKernelPath(KernelPathType.Reminders))
        End Sub

        ''' <summary>
        ''' Saves an reminder to a file
        ''' </summary>
        Public Sub SaveReminder(ReminderInstance As ReminderInfo, File As String)
            ThrowOnInvalidPath(File)
            File = NeutralizePath(File)
            Wdbg(DebugLevel.I, "Saving reminder to {0}...", File)
            Dim Converter As New XmlSerializer(GetType(ReminderInfo))
            Dim ReminderFileStream As New FileStream(File, FileMode.OpenOrCreate)
            Wdbg(DebugLevel.I, "Opened stream with length {0}", ReminderFileStream.Length)
            Converter.Serialize(ReminderFileStream, ReminderInstance)
            ReminderFileStream.Close()
        End Sub

    End Module
End Namespace