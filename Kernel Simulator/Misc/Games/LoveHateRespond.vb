
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

    'TODO: Comments need localization
    Dim LoveComments As New List(Of String) From {"Thanks! This is interesting.",
                                                  "Everyone will support your video for this.",
                                                  "I gave you the special file in your e-mail for your next video.",
                                                  "Listen, haters, he is trying to help us, not scam.",
                                                  "I don't know how much do I and my friends thank you for this video.",
                                                  "I love you for this video.",
                                                  "Keep going, don't stop.",
                                                  "I will help you reach to 1M subscribers!",
                                                  "My friends got their computer fixed because of you.",
                                                  "Awesome prank! I shut down my enemy's PC.",
                                                  "To haters: STOP HATING ON HIM",
                                                  "To haters: GET TO WORK",
                                                  "Nobody will notice this now thanks to your object hiding guide"}
    Dim HateComments As New List(Of String) From {"I will stop watching your videos. Subscriber lost.",
                                                  "What is this? This is unclear.",
                                                  "This video is the worst!",
                                                  "Everyone report this video!",
                                                  "My friends are furious with you!",
                                                  "Lovers will now hate you for this.",
                                                  "Your friend will hate you for this.",
                                                  "This prank made me unsubscribe to you.",
                                                  "Mission failed, Respect -, Subscriber -",
                                                  "Stop making this kind of video!!!",
                                                  "Get back to your job, your videos are the worst!",
                                                  "We prejudice on this video."}
    Dim Comments As New Dictionary(Of String, List(Of String)) From {{"Love", LoveComments}, {"Hate", HateComments}}
    Dim Users As New List(Of String) From {"TheTrickster", "The Eagle", "ErrorStopper", "Losting - Computers, fixes, and more", "Windows", "CyberBully #2095", "Snap",
                                           "ExtFS", "Elaine Stretch", "NFSMW2005", "WhatsUp", "BSearch - Beta hunting for GTA V, and more", "Wellington Marks", "Carbon",
                                           "LinuxUser348", "Linux is Awesome", "The Matrix"}
    Sub InitializeLoteresp()
        Dim RandomDriver As New Random()
        Dim RandomUser, RandomComment, Type, Response As String
        While True
            Type = Comments.ElementAt(RandomDriver.Next(Comments.Keys.Count)).Key
            RandomUser = Users.ElementAt(RandomDriver.Next(Users.Count))
            RandomComment = Comments(Type).ElementAt(RandomDriver.Next(Comments(Type).Count))
            W(DoTranslation("If someone made this comment to your video:", currentLang), True, ColTypes.Neutral)
            W("{0}: {1}", True, ColTypes.Neutral, RandomUser, RandomComment)
            W(DoTranslation("How would you respond?", currentLang) + " <R/T/Q> ", False, ColTypes.Input)
            Response = Console.ReadKey.KeyChar
            Console.WriteLine()
            If Response = "R" Or Response = "r" Then
                If Type = "Hate" Then
                    W(DoTranslation("Great! {0}'s comment will be removed for that.", currentLang), True, ColTypes.Neutral, RandomUser)
                Else
                    W(DoTranslation("You just reported the love comment!", currentLang), True, ColTypes.Neutral)
                End If
            ElseIf Response = "T" Or Response = "t" Then
                If Type = "Love" Then
                    W(DoTranslation("Great! {0} will appreciate your thanks.", currentLang), True, ColTypes.Neutral, RandomUser)
                Else
                    W(DoTranslation("You just replied to the hate comment!", currentLang), True, ColTypes.Neutral)
                End If
            ElseIf Response = "Q" Or Response = "q" Then
                Exit Sub
            Else
                W(DoTranslation("Invalid selection. Going to the next comment...", currentLang), True, ColTypes.Neutral)
            End If
        End While
    End Sub

End Module
