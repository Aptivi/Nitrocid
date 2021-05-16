Public Module RSSHelpSystem

    'This dictionary is the definitions for commands.
    Public RSSDefinitions As Dictionary(Of String, String)
    Public RSSModDefs As New Dictionary(Of String, String)

    ''' <summary>
    ''' Updates the help definition so it reflects the available commands
    ''' </summary>
    Public Sub InitRSSHelp()
        RSSDefinitions = New Dictionary(Of String, String) From {{"chfeed", DoTranslation("Changes the feed link")},
                                                                 {"list", DoTranslation("Lists all feeds")},
                                                                 {"read", DoTranslation("Reads a feed in a web browser")},
                                                                 {"exit", DoTranslation("Exits RSS shell and returns to kernel")},
                                                                 {"help", DoTranslation("Shows help screen")}}
    End Sub

    ''' <summary>
    ''' Shows the list of commands.
    ''' </summary>
    ''' <param name="command">Specified command</param>
    Public Sub RSSShowHelp(Optional ByVal command As String = "")

        If command = "" Then
            If simHelp = False Then
                For Each cmd As String In RSSDefinitions.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, RSSDefinitions(cmd))
                Next
                For Each cmd As String In RSSModDefs.Keys
                    W("- {0}: ", False, ColTypes.ListEntry, cmd) : W("{0}", True, ColTypes.ListValue, RSSModDefs(cmd))
                Next
            Else
                W(String.Join(", ", RSSCommands.Keys), True, ColTypes.Neutral)
            End If
        ElseIf command = "chfeed" Then
            W(DoTranslation("Usage:") + " chfeed <feedurl>", True, ColTypes.Neutral)
        ElseIf command = "exit" Then
            W(DoTranslation("Usage:") + " exit", True, ColTypes.Neutral)
        ElseIf command = "list" Then
            W(DoTranslation("Usage:") + " list", True, ColTypes.Neutral)
        ElseIf command = "read" Then
            W(DoTranslation("Usage:") + " read <feednum>", True, ColTypes.Neutral)
        End If

    End Sub

End Module
