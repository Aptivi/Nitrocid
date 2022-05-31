
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

Namespace Misc.Calendar.Events
    Public Module EventManager

        Public CalendarEvents As New List(Of EventInfo)
        Public EventThread As New Thread(AddressOf EventListen) With {.Name = "Event Thread"}

        ''' <summary>
        ''' Listens for events and notifies the user if the date is due to the event
        ''' </summary>
        Private Sub EventListen()
            While EventThread.IsAlive
                Try
                    Thread.Sleep(1)
                    For EventIndex As Integer = 0 To CalendarEvents.Count - 1
                        Dim EventInstance As EventInfo = CalendarEvents(EventIndex)
                        If Date.Today = EventInstance.EventDate.Date Then
                            EventInstance.NotifyEvent()
                        End If
                    Next
                Catch ex As ThreadAbortException
                    Wdbg(DebugLevel.I, "Aborting event listener...")
                    Exit Sub
                End Try
            End While
        End Sub

        ''' <summary>
        ''' Adds the event to the list (calendar will mark the day with color)
        ''' </summary>
        ''' <param name="EventDate">Event date and time</param>
        ''' <param name="EventTitle">Event title</param>
        Public Sub AddEvent(EventDate As Date, EventTitle As String)
            If String.IsNullOrWhiteSpace(EventTitle) Then EventTitle = DoTranslation("Untitled event")
            Dim EventInstance As New EventInfo() With {
                .EventTitle = EventTitle,
                .EventDate = EventDate
            }
            AddEvent(EventInstance)
        End Sub

        ''' <summary>
        ''' Adds the event to the list (calendar will mark the day with color)
        ''' </summary>
        ''' <param name="EventInstance">Event info instance</param>
        Friend Sub AddEvent(EventInstance As EventInfo)
            CalendarEvents.Add(EventInstance)
        End Sub

        ''' <summary>
        ''' Removes the event from the list
        ''' </summary>
        ''' <param name="EventDate">Event date and time</param>
        ''' <param name="EventId">Event ID</param>
        Public Sub RemoveEvent(EventDate As Date, EventId As Integer)
            Dim EventIndex As Integer = EventId - 1
            Dim EventInstance As EventInfo = CalendarEvents(EventIndex)
            If EventInstance.EventDate = EventDate Then
                CalendarEvents.Remove(EventInstance)
            End If
        End Sub

        ''' <summary>
        ''' List all the events
        ''' </summary>
        Public Sub ListEvents()
            For Each EventInstance As EventInfo In CalendarEvents
                Write("- {0}: ", False, ColTypes.ListEntry, EventInstance.EventDate)
                Write(EventInstance.EventTitle, True, ColTypes.ListValue)
            Next
        End Sub

        ''' <summary>
        ''' Loads all the events from the KSEvents directory and adds them to the event list
        ''' </summary>
        Public Sub LoadEvents()
            MakeDirectory(GetKernelPath(KernelPathType.Events), False)
            Dim EventFiles As List(Of String) = Directory.EnumerateFileSystemEntries(GetKernelPath(KernelPathType.Events), "*", SearchOption.AllDirectories).ToList
            Wdbg(DebugLevel.I, "Got {0} events.", EventFiles.Count)

            'Load all the events
            For Each EventFile As String In EventFiles
                Dim LoadedEvent As EventInfo = LoadEvent(EventFile)
                If LoadedEvent IsNot Nothing Then AddEvent(LoadedEvent)
            Next
        End Sub

        ''' <summary>
        ''' Loads an event file
        ''' </summary>
        ''' <param name="EventFile">Event file</param>
        ''' <returns>A converted event info instance. null if unsuccessful.</returns>
        Public Function LoadEvent(EventFile As String) As EventInfo
            ThrowOnInvalidPath(EventFile)
            EventFile = NeutralizePath(EventFile)
            Wdbg(DebugLevel.I, "Loading event {0}...", EventFile)

            'If file exists, convert the file to the event instance
            If FileExists(EventFile) Then
                Dim Converter As New XmlSerializer(GetType(EventInfo))
                Dim EventFileStream As New FileStream(EventFile, FileMode.Open)
                Wdbg(DebugLevel.I, "Opened stream [{0}]. Converting...", EventFileStream.Length)
                Dim ConvertedEvent As EventInfo = DirectCast(Converter.Deserialize(EventFileStream), EventInfo)
                Wdbg(DebugLevel.I, "Converted!")
                Return ConvertedEvent
            Else
                Wdbg(DebugLevel.E, "File doesn't exist!")
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Saves all the events from the event list to their individual files
        ''' </summary>
        Public Sub SaveEvents()
            SaveEvents(GetKernelPath(KernelPathType.Events), SaveEventsRemindersDestructively)
        End Sub

        ''' <summary>
        ''' Saves all the events from the event list to their individual files
        ''' </summary>
        Public Sub SaveEvents(Path As String, Destructive As Boolean)
            ThrowOnInvalidPath(Path)
            Path = NeutralizePath(Path)
            Wdbg(DebugLevel.I, "Saving events to {0}...", Path)

            'Remove all events from path, if running destructively
            If Destructive Then
                Dim EventFiles As String() = Directory.EnumerateFiles(Path, "*", SearchOption.AllDirectories).ToArray
                Dim EventFolders As String() = Directory.EnumerateDirectories(Path, "*", SearchOption.AllDirectories).ToArray

                'First, remove all files
                For Each FilePath As String In EventFiles
                    RemoveFile(FilePath)
                Next

                'Then, remove all empty folders
                For Each FolderPath As String In EventFolders
                    RemoveDirectory(FolderPath)
                Next
            End If

            'Enumerate through every event and save them
            For EventIndex As Integer = 0 To CalendarEvents.Count - 1
                Dim EventInstance As EventInfo = CalendarEvents(EventIndex)
                Dim EventFileName As String = $"[{EventIndex}] {EventInstance.EventTitle}.ksevent"
                Wdbg(DebugLevel.I, "Event file name: {0}...", EventFileName)
                Dim EventFilePath As String = NeutralizePath(EventFileName, Path)
                Wdbg(DebugLevel.I, "Event file path: {0}...", EventFilePath)
                SaveEvent(EventInstance, EventFilePath)
            Next
        End Sub

        ''' <summary>
        ''' Saves an event to a file
        ''' </summary>
        Public Sub SaveEvent(EventInstance As EventInfo)
            SaveEvent(EventInstance, GetKernelPath(KernelPathType.Events))
        End Sub

        ''' <summary>
        ''' Saves an event to a file
        ''' </summary>
        Public Sub SaveEvent(EventInstance As EventInfo, File As String)
            ThrowOnInvalidPath(File)
            File = NeutralizePath(File)
            Wdbg(DebugLevel.I, "Saving event to {0}...", File)
            Dim Converter As New XmlSerializer(GetType(EventInfo))
            Dim EventFileStream As New FileStream(File, FileMode.OpenOrCreate)
            Wdbg(DebugLevel.I, "Opened stream with length {0}", EventFileStream.Length)
            Converter.Serialize(EventFileStream, EventInstance)
        End Sub

    End Module
End Namespace