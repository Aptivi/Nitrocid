
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

Imports KS.Misc

Namespace Misc.Games
    Public Module LoveHateRespond

        ''' <summary>
        ''' How many users to add to the love/hate comment room?
        ''' </summary>
        Public LoveOrHateUsersCount As Integer = 20
        ReadOnly LoveComments As New List(Of String) From {DoTranslation("Thanks! This is interesting."),
                                                           DoTranslation("Everyone will support your video for this."),
                                                           DoTranslation("I gave you the special file in your e-mail for your next video."),
                                                           DoTranslation("Listen, haters, he is trying to help us, not scam."),
                                                           DoTranslation("I don't know how much do I and my friends thank you for this video."),
                                                           DoTranslation("I love you for this video."),
                                                           DoTranslation("Keep going, don't stop."),
                                                           DoTranslation("I will help you reach to 1M subscribers!"),
                                                           DoTranslation("My friends got their computer fixed because of you."),
                                                           DoTranslation("Awesome prank! I shut down my enemy's PC."),
                                                           DoTranslation("To haters: STOP HATING ON HIM"),
                                                           DoTranslation("To haters: GET TO WORK"),
                                                           DoTranslation("Nobody will notice this now thanks to your object hiding guide")}
        ReadOnly HateComments As New List(Of String) From {DoTranslation("I will stop watching your videos. Subscriber lost."),
                                                           DoTranslation("What is this? This is unclear."),
                                                           DoTranslation("This video is the worst!"),
                                                           DoTranslation("Everyone report this video!"),
                                                           DoTranslation("My friends are furious with you!"),
                                                           DoTranslation("Lovers will now hate you for this."),
                                                           DoTranslation("Your friend will hate you for this."),
                                                           DoTranslation("This prank made me unsubscribe to you."),
                                                           DoTranslation("Mission failed, Respect -, Subscriber -"),
                                                           DoTranslation("Stop making this kind of video!!!"),
                                                           DoTranslation("Get back to your job, your videos are the worst!"),
                                                           DoTranslation("We prejudice on this video.")}
        ReadOnly Comments As New Dictionary(Of String, List(Of String)) From {{CommentType.Love, LoveComments}, {CommentType.Hate, HateComments}}
        ReadOnly Users As New Dictionary(Of String, CommentType)
        Friend Names() As String = {}
        Friend Surnames() As String = {}

        Enum CommentType
            ''' <summary>
            ''' A love comment
            ''' </summary>
            Love
            ''' <summary>
            ''' A hate comment
            ''' </summary>
            Hate
        End Enum

        ''' <summary>
        ''' Initializes the game
        ''' </summary>
        Sub InitializeLoveHate()
            Dim RandomDriver As New Random()
            Dim RandomUser, RandomComment, Response As String
            Dim Type As CommentType
            Dim ExitRequested As Boolean
            Dim Score, CommentNumber As Long

            'Download the names list
            Write(DoTranslation("Downloading names..."), True, ColTypes.Progress)
            PopulateNames()
            For NameNum As Integer = 1 To LoveOrHateUsersCount
                Dim GeneratedName As String = Names(RandomDriver.Next(Names.Length))
                Dim GeneratedSurname As String = Surnames(RandomDriver.Next(Surnames.Length))
                Users.Add($"{GeneratedName} {GeneratedSurname}", RandomDriver.Next(2))
            Next

            'Game logic
            Write(DoTranslation("Press A on hate comments to apologize. Press T on love comments to thank. Press Q to quit the game."), True, ColTypes.Tip)
            While Not ExitRequested
                'Set necessary variables
                RandomUser = Users.Keys.ElementAt(RandomDriver.Next(Users.Keys.Count))
                Type = Users(RandomUser)
                RandomComment = Comments(Type).ElementAt(RandomDriver.Next(Comments(Type).Count))
                CommentNumber += 1
                Wdbg(DebugLevel.I, "Comment type: {0}", Type)
                Wdbg(DebugLevel.I, "Commenter: {0}", RandomUser)
                Wdbg(DebugLevel.I, "Comment: {0}", RandomComment)

                'Ask the user the question
                WriteSeparator("[{0}/{1}]", True, Score, CommentNumber)
                Write(DoTranslation("If someone made this comment to your video:"), True, ColTypes.Neutral)
                Write("- {0}:", False, ColTypes.ListEntry, RandomUser)
                Write(" {0}", True, ColTypes.ListValue, RandomComment)
                Write(DoTranslation("How would you respond?") + " <A/T/Q> ", False, ColTypes.Input)
                Response = Console.ReadKey.KeyChar
                Console.WriteLine()
                Wdbg(DebugLevel.I, "Response: {0}", Response)

                'Parse response
                Select Case Response.ToLower
                    Case "a" 'Apologize
                        Select Case Type
                            Case CommentType.Love
                                Wdbg(DebugLevel.I, "Apologized to love comment")
                                Write("[-1] " + DoTranslation("Apologized to love comment. Not good enough."), True, ColTypes.Neutral)
                                Score -= 1
                            Case CommentType.Hate
                                Wdbg(DebugLevel.I, "Apologized to hate comment")
                                Write("[+1] " + DoTranslation("You've apologized to a hate comment! Excellent!"), True, ColTypes.Neutral)
                                Score += 1
                        End Select
                    Case "t" 'Thank
                        Select Case Type
                            Case CommentType.Love
                                Wdbg(DebugLevel.I, "Thanked love comment")
                                Write("[+1] " + DoTranslation("Great! {0} will appreciate your thanks."), True, ColTypes.Neutral, RandomUser)
                                Score += 1
                            Case CommentType.Hate
                                Wdbg(DebugLevel.I, "Thanked hate comment")
                                Write("[-1] " + DoTranslation("You just thanked the hater for the hate comment!"), True, ColTypes.Neutral)
                                Score -= 1
                        End Select
                    Case "q" 'Quit
                        Wdbg(DebugLevel.I, "Exit requested")
                        ExitRequested = True
                    Case Else
                        Wdbg(DebugLevel.I, "No such selection")
                        Write(DoTranslation("Invalid selection. Going to the next comment..."), True, ColTypes.Error)
                End Select
            End While
        End Sub

    End Module
End Namespace