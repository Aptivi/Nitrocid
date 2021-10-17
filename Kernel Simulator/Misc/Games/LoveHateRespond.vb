
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Module LoveHateRespond

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
    ReadOnly Users As New Dictionary(Of String, CommentType) From {{"John Williams", Rnd()}, {"The Eagle", Rnd()},
                                                                   {"Jim Walker", Rnd()}, {"Alex", Rnd()},
                                                                   {"Will Philips", Rnd()}, {"Elaine McCann", Rnd()},
                                                                   {"ProGamer453", Rnd()}, {"Rajput Singh", Rnd()},
                                                                   {"Abihshek Chaudhary", Rnd()}, {"Johnny Alfonso", Rnd()},
                                                                   {"Bruce Fitzgerald", Rnd()}, {"Mark Adams", Rnd()},
                                                                   {"Wellington Marks", Rnd()}, {"CD-OS", Rnd()},
                                                                   {"LinuxUser348", Rnd()}, {"Suspicion Ltd.", Rnd()}}

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
        W(DoTranslation("Press A on hate comments to apologize. Press T on love comments to thank. Press Q to quit the game."), True, ColTypes.Tip)

        'Game logic
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
            W(DoTranslation("If someone made this comment to your video:"), True, ColTypes.Neutral)
            W("- {0}:", False, ColTypes.ListEntry, RandomUser)
            W(" {0}", True, ColTypes.ListValue, RandomComment)
            W(DoTranslation("How would you respond?") + " <A/T/Q> ", False, ColTypes.Input)
            Response = Console.ReadKey.KeyChar
            Console.WriteLine()
            Wdbg(DebugLevel.I, "Response: {0}", Response)

            'Parse response
            Select Case Response
                Case "A", "a" 'Apologize
                    Select Case Type
                        Case CommentType.Love
                            Wdbg(DebugLevel.I, "Apologized to love comment")
                            W(DoTranslation("Apologized to love comment. Not good enough."), True, ColTypes.Neutral)
                            Score -= 1
                        Case CommentType.Hate
                            Wdbg(DebugLevel.I, "Apologized to hate comment")
                            W(DoTranslation("You've apologized to a hate comment! Excellent!"), True, ColTypes.Neutral)
                            Score += 1
                    End Select
                Case "T", "t" 'Thank
                    Select Case Type
                        Case CommentType.Love
                            Wdbg(DebugLevel.I, "Thanked love comment")
                            W(DoTranslation("Great! {0} will appreciate your thanks."), True, ColTypes.Neutral, RandomUser)
                            Score += 1
                        Case CommentType.Hate
                            Wdbg(DebugLevel.I, "Thanked hate comment")
                            W(DoTranslation("You just thanked the hater for the hate comment!"), True, ColTypes.Neutral)
                            Score -= 1
                    End Select
                Case "Q", "q" 'Quit
                    Wdbg(DebugLevel.I, "Exit requested")
                    ExitRequested = True
                Case Else
                    Wdbg(DebugLevel.I, "No such selection")
                    W(DoTranslation("Invalid selection. Going to the next comment..."), True, ColTypes.Error)
            End Select
        End While
    End Sub

End Module
