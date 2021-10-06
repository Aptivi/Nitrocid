
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
    ReadOnly Comments As New Dictionary(Of String, List(Of String)) From {{"Love", LoveComments}, {"Hate", HateComments}}
    ReadOnly Users As New List(Of String) From {"GS4L", "The Eagle", "Vercity", "Losting - Computers, fixes, and more", "WillStrike", "CyberBully #2095", "ProGamer453",
                                                "ExtFS", "Elaine Stretch", "NFSMW2005", "WhatsUp", "BSearch", "Wellington Marks", "CD-OS", "LinuxUser348", "Speculate Ltd.",
                                                "The Matrix"}

    ''' <summary>
    ''' Initializes the game
    ''' </summary>
    Sub InitializeLoteresp()
        Dim RandomDriver As New Random()
        Dim RandomUser, RandomComment, Type, Response As String
        While True
            Type = Comments.ElementAt(RandomDriver.Next(Comments.Keys.Count)).Key
            RandomUser = Users.ElementAt(RandomDriver.Next(Users.Count))
            RandomComment = Comments(Type).ElementAt(RandomDriver.Next(Comments(Type).Count))
            Wdbg(DebugLevel.I, "Comment type: {0}", Type)
            Wdbg(DebugLevel.I, "Commenter: {0}", RandomUser)
            Wdbg(DebugLevel.I, "Comment: {0}", RandomComment)
            W(DoTranslation("If someone made this comment to your video:"), True, ColTypes.Neutral)
            W("{0}: {1}", True, ColTypes.Neutral, RandomUser, RandomComment)
            W(DoTranslation("How would you respond?") + " <R/T/Q> ", False, ColTypes.Input)
            Response = Console.ReadKey.KeyChar
            Console.WriteLine()
            Wdbg(DebugLevel.I, "Response: {0}", Response)
            If Response = "R" Or Response = "r" Then
                If Type = "Hate" Then
                    Wdbg(DebugLevel.I, "Reported hate comment")
                    W(DoTranslation("Great! {0}'s comment will be removed for that."), True, ColTypes.Neutral, RandomUser)
                Else
                    Wdbg(DebugLevel.I, "Reported love comment")
                    W(DoTranslation("You just reported the love comment!"), True, ColTypes.Neutral)
                End If
            ElseIf Response = "T" Or Response = "t" Then
                If Type = "Love" Then
                    Wdbg(DebugLevel.I, "Thanked love comment")
                    W(DoTranslation("Great! {0} will appreciate your thanks."), True, ColTypes.Neutral, RandomUser)
                Else
                    Wdbg(DebugLevel.I, "Thanked hate comment")
                    W(DoTranslation("You just replied to the hate comment!"), True, ColTypes.Neutral)
                End If
            ElseIf Response = "Q" Or Response = "q" Then
                Wdbg(DebugLevel.I, "Exit requested")
                Exit Sub
            Else
                Wdbg(DebugLevel.I, "No such selection")
                W(DoTranslation("Invalid selection. Going to the next comment..."), True, ColTypes.Error)
            End If
        End While
    End Sub

End Module
