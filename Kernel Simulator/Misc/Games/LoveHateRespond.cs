
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Writers.FancyWriters;
using static Namer.NameGenerator;

namespace KS.Misc.Games
{
    public static class LoveHateRespond
    {

        /// <summary>
        /// How many users to add to the love/hate comment room?
        /// </summary>
        public static int LoveOrHateUsersCount = 20;
        private readonly static List<string> LoveComments = new List<string>() { Translate.DoTranslation("Thanks! This is interesting."), Translate.DoTranslation("Everyone will support your video for this."), Translate.DoTranslation("I gave you the special file in your e-mail for your next video."), Translate.DoTranslation("Listen, haters, he is trying to help us, not scam."), Translate.DoTranslation("I don't know how much do I and my friends thank you for this video."), Translate.DoTranslation("I love you for this video."), Translate.DoTranslation("Keep going, don't stop."), Translate.DoTranslation("I will help you reach to 1M subscribers!"), Translate.DoTranslation("My friends got their computer fixed because of you."), Translate.DoTranslation("Awesome prank! I shut down my enemy's PC."), Translate.DoTranslation("To haters: STOP HATING ON HIM"), Translate.DoTranslation("To haters: GET TO WORK"), Translate.DoTranslation("Nobody will notice this now thanks to your object hiding guide") };
        private readonly static List<string> HateComments = new List<string>() { Translate.DoTranslation("I will stop watching your videos. Subscriber lost."), Translate.DoTranslation("What is this? This is unclear."), Translate.DoTranslation("This video is the worst!"), Translate.DoTranslation("Everyone report this video!"), Translate.DoTranslation("My friends are furious with you!"), Translate.DoTranslation("Lovers will now hate you for this."), Translate.DoTranslation("Your friend will hate you for this."), Translate.DoTranslation("This prank made me unsubscribe to you."), Translate.DoTranslation("Mission failed, Respect -, Subscriber -"), Translate.DoTranslation("Stop making this kind of video!!!"), Translate.DoTranslation("Get back to your job, your videos are the worst!"), Translate.DoTranslation("We prejudice on this video.") };
        private readonly static Dictionary<string, List<string>> Comments = new Dictionary<string, List<string>>() { { ((int)CommentType.Love).ToString(), LoveComments }, { ((int)CommentType.Hate).ToString(), HateComments } };
        private readonly static Dictionary<string, CommentType> Users = new Dictionary<string, CommentType>();

        public enum CommentType
        {
            /// <summary>
            /// A love comment
            /// </summary>
            Love,
            /// <summary>
            /// A hate comment
            /// </summary>
            Hate
        }

        /// <summary>
        /// Initializes the game
        /// </summary>
        public static void InitializeLoveHate()
        {
            var RandomDriver = new Random();
            string RandomUser, RandomComment, Response;
            CommentType Type;
            var ExitRequested = default(bool);
            long Score = default, CommentNumber = default;

            // Download the names list
            TextWriterColor.Write(Translate.DoTranslation("Downloading names..."), true, ColorTools.ColTypes.Progress);
            PopulateNames();
            for (int NameNum = 1, loopTo = LoveOrHateUsersCount; NameNum <= loopTo; NameNum++)
            {
                string GeneratedName = GenerateNames()[0];
                Users.Add($"{GeneratedName}", (CommentType)RandomDriver.Next(2));
            }

            // Game logic
            TextWriterColor.Write(Translate.DoTranslation("Press A on hate comments to apologize. Press T on love comments to thank. Press Q to quit the game."), true, ColorTools.ColTypes.Tip);
            while (!ExitRequested)
            {
                // Set necessary variables
                RandomUser = Users.Keys.ElementAt(RandomDriver.Next(Users.Keys.Count));
                Type = Users[RandomUser];
                RandomComment = Comments[((int)Type).ToString()].ElementAt(RandomDriver.Next(Comments[((int)Type).ToString()].Count));
                CommentNumber += 1L;
                DebugWriter.Wdbg(DebugLevel.I, "Comment type: {0}", Type);
                DebugWriter.Wdbg(DebugLevel.I, "Commenter: {0}", RandomUser);
                DebugWriter.Wdbg(DebugLevel.I, "Comment: {0}", RandomComment);

                // Ask the user the question
                SeparatorWriterColor.WriteSeparator("[S: {0} / C: {1}]", true, Score, CommentNumber);
                TextWriterColor.Write(Translate.DoTranslation("If someone made this comment to your video:"), true, ColorTools.ColTypes.Neutral);
                TextWriterColor.Write("- {0}:", false, ColorTools.ColTypes.ListEntry, RandomUser);
                TextWriterColor.Write(" {0}", true, ColorTools.ColTypes.ListValue, RandomComment);
                TextWriterColor.Write(Translate.DoTranslation("How would you respond?") + " <A/T/Q> ", false, ColorTools.ColTypes.Input);
                Response = Convert.ToString(Console.ReadKey().KeyChar);
                Console.WriteLine();
                DebugWriter.Wdbg(DebugLevel.I, "Response: {0}", Response);

                // Parse response
                switch (Response.ToLower() ?? "")
                {
                    case "a": // Apologize
                        {
                            switch (Type)
                            {
                                case CommentType.Love:
                                    {
                                        DebugWriter.Wdbg(DebugLevel.I, "Apologized to love comment");
                                        TextWriterColor.Write("[-1] " + Translate.DoTranslation("Apologized to love comment. Not good enough."), true, ColorTools.ColTypes.Neutral);
                                        Score -= 1L;
                                        break;
                                    }
                                case CommentType.Hate:
                                    {
                                        DebugWriter.Wdbg(DebugLevel.I, "Apologized to hate comment");
                                        TextWriterColor.Write("[+1] " + Translate.DoTranslation("You've apologized to a hate comment! Excellent!"), true, ColorTools.ColTypes.Neutral);
                                        Score += 1L;
                                        break;
                                    }
                            }

                            break;
                        }
                    case "t": // Thank
                        {
                            switch (Type)
                            {
                                case CommentType.Love:
                                    {
                                        DebugWriter.Wdbg(DebugLevel.I, "Thanked love comment");
                                        TextWriterColor.Write("[+1] " + Translate.DoTranslation("Great! {0} will appreciate your thanks."), true, ColorTools.ColTypes.Neutral, RandomUser);
                                        Score += 1L;
                                        break;
                                    }
                                case CommentType.Hate:
                                    {
                                        DebugWriter.Wdbg(DebugLevel.I, "Thanked hate comment");
                                        TextWriterColor.Write("[-1] " + Translate.DoTranslation("You just thanked the hater for the hate comment!"), true, ColorTools.ColTypes.Neutral);
                                        Score -= 1L;
                                        break;
                                    }
                            }

                            break;
                        }
                    case "q": // Quit
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Exit requested");
                            ExitRequested = true;
                            break;
                        }

                    default:
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "No such selection");
                            TextWriterColor.Write(Translate.DoTranslation("Invalid selection. Going to the next comment..."), true, ColorTools.ColTypes.Error);
                            break;
                        }
                }
            }
        }

    }
}
