
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

Imports KS.Network.RSS.Instance
Imports KS.Network.RSS
Imports System.Threading

Namespace Shell.Shells
    Public Class RSSShell
        Inherits ShellExecutor
        Implements IShell

        Public Overrides ReadOnly Property ShellType As ShellType Implements IShell.ShellType
            Get
                Return ShellType.RSSShell
            End Get
        End Property

        Public Overrides Property Bail As Boolean Implements IShell.Bail

        Public Overrides Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
            'Handle the RSS feed link provided by user
            Dim BailFromEnter As Boolean = False
            Dim OldRSSFeedLink As String = ""
            Dim FeedUrl As String = ""
            If ShellArgs.Length > 0 Then
                FeedUrl = ShellArgs(0)
            End If
            RSSFeedLink = FeedUrl

            Do Until BailFromEnter
                If String.IsNullOrWhiteSpace(RSSFeedLink) Then
                    Do While String.IsNullOrWhiteSpace(RSSFeedLink)
                        Try
                            If Not String.IsNullOrWhiteSpace(RSSFeedUrlPromptStyle) Then
                                Write(ProbePlaces(RSSFeedUrlPromptStyle), False, ColTypes.Input)
                            Else
                                Write(DoTranslation("Enter an RSS feed URL:") + " ", False, ColTypes.Input)
                            End If
                            RSSFeedLink = ReadLine()
                            If ReadLineReboot.ReadLine.ReadRanToCompletion Then
                                'The user entered the feed URL
                                RSSFeedInstance = New RSSFeed(RSSFeedLink, RSSFeedType.Infer)
                                RSSFeedLink = RSSFeedInstance.FeedUrl
                                OldRSSFeedLink = RSSFeedLink
                            Else
                                'We're aborting.
                                Bail = True
                            End If
                            BailFromEnter = True
                        Catch taex As ThreadInterruptedException
                            CancelRequested = False
                            BailFromEnter = True
                            Bail = True
                        Catch ex As Exception
                            Wdbg(DebugLevel.E, "Failed to parse RSS feed URL {0}: {1}", FeedUrl, ex.Message)
                            WStkTrc(ex)
                            Write(DoTranslation("Failed to parse feed URL:") + " {0}", True, ColTypes.Error, ex.Message)
                            RSSFeedLink = ""
                        End Try
                    Loop
                Else
                    'Make a new RSS feed instance
                    Try
                        If OldRSSFeedLink <> RSSFeedLink Then
                            If RSSFeedLink = "select" Then
                                OpenFeedSelector()
                            End If
                            RSSFeedInstance = New RSSFeed(RSSFeedLink, RSSFeedType.Infer)
                            RSSFeedLink = RSSFeedInstance.FeedUrl
                        End If
                        OldRSSFeedLink = RSSFeedLink
                        BailFromEnter = True
                    Catch taex As ThreadInterruptedException
                        CancelRequested = False
                        BailFromEnter = True
                        Bail = True
                    Catch ex As Exception
                        Wdbg(DebugLevel.E, "Failed to parse RSS feed URL {0}: {1}", RSSFeedLink, ex.Message)
                        WStkTrc(ex)
                        Write(DoTranslation("Failed to parse feed URL:") + " {0}", True, ColTypes.Error, ex.Message)
                        RSSFeedLink = ""
                    End Try
                End If
            Loop

            While Not Bail
                Try
                    'Send ping to keep the connection alive
                    If Not RSSKeepAlive And Not RSSRefresher.IsAlive And RSSRefreshFeeds Then RSSRefresher.Start()
                    Wdbg(DebugLevel.I, "Made new thread about RefreshFeeds()")

                    'See UESHShell.vb for more info
                    SyncLock GetCancelSyncLock(ShellType)
                        'Prepare for prompt
                        If DefConsoleOut IsNot Nothing Then
                            Console.SetOut(DefConsoleOut)
                        End If
                        Wdbg(DebugLevel.I, "RSSShellPromptStyle = {0}", RSSShellPromptStyle)
                        If RSSShellPromptStyle = "" Then
                            Write("[", False, ColTypes.Gray) : Write("{0}", False, ColTypes.UserName, New Uri(RSSFeedLink).Host) : Write("] > ", False, ColTypes.Gray)
                        Else
                            Dim ParsedPromptStyle As String = ProbePlaces(RSSShellPromptStyle)
                            ParsedPromptStyle.ConvertVTSequences
                            Write(ParsedPromptStyle, False, ColTypes.Gray)
                        End If
                        SetInputColor()

                        'Raise the event
                        KernelEventManager.RaiseRSSShellInitialized(RSSFeedLink)
                    End SyncLock

                    'Prompt for command
                    Dim WrittenCommand As String = ReadLine()
                    If Not (WrittenCommand = Nothing Or WrittenCommand?.StartsWithAnyOf({" ", "#"})) Then
                        KernelEventManager.RaiseRSSPreExecuteCommand(RSSFeedLink, WrittenCommand)
                        GetLine(WrittenCommand, False, "", ShellType.RSSShell)
                        KernelEventManager.RaiseRSSPostExecuteCommand(RSSFeedLink, WrittenCommand)
                    End If
                Catch taex As ThreadInterruptedException
                    CancelRequested = False
                    Bail = True
                Catch ex As Exception
                    WStkTrc(ex)
                    Write(DoTranslation("There was an error in the shell.") + NewLine + "Error {0}: {1}", True, ColTypes.Error, ex.GetType.FullName, ex.Message)
                    Continue While
                End Try
            End While

            'Disconnect the session
            If RSSKeepAlive Then
                Wdbg(DebugLevel.W, "Exit requested, but not disconnecting.")
            Else
                Wdbg(DebugLevel.W, "Exit requested. Disconnecting host...")
                If RSSRefreshFeeds Then RSSRefresher.Stop()
                RSSFeedLink = ""
                RSSFeedInstance = Nothing
            End If
        End Sub

    End Class
End Namespace
