//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Textify.General;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Files;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Etaoin fill type
    /// </summary>
    public enum FillType
    {
        /// <summary>
        /// Prints the known pattern of etaoin characters, such as: etaoin shrdlu
        /// </summary>
        EtaoinPattern,
        /// <summary>
        /// Prints the complete pattern of etaoin characters, such as: etaoin shrdlu cmfwyp vbgkqj xz
        /// </summary>
        EtaoinComplete,
        /// <summary>
        /// Prints the random set of characters to rapidly fill in the line
        /// </summary>
        RandomChars
    }

    /// <summary>
    /// Display code for Linotypo
    /// </summary>
    internal class LinotypoDisplay : BaseScreensaver, IScreensaver
    {

        private int CurrentColumn = 1;
        private int CurrentColumnRowConsole = 0;

        public override string ScreensaverName =>
            "Linotypo";

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.SetConsoleColor(new Color(ScreensaverPackInit.SaversConfig.LinotypoTextColor));
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;
            CurrentColumn = 1;
            CurrentColumnRowConsole = 0;
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
        }

        public override void ScreensaverLogic()
        {
            int CpmSpeedMin = ScreensaverPackInit.SaversConfig.LinotypoWritingSpeedMin * 5;
            int CpmSpeedMax = ScreensaverPackInit.SaversConfig.LinotypoWritingSpeedMax * 5;
            int MaxCharacters = (int)Math.Round((ConsoleWrapper.WindowWidth - 2) / (double)ScreensaverPackInit.SaversConfig.LinotypoTextColumns - 3d);
            int ColumnRowConsoleThreshold = (int)Math.Round(ConsoleWrapper.WindowWidth / (double)ScreensaverPackInit.SaversConfig.LinotypoTextColumns);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Minimum speed from {0} WPM: {1} CPM", vars: [ScreensaverPackInit.SaversConfig.LinotypoWritingSpeedMin, CpmSpeedMin]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Maximum speed from {0} WPM: {1} CPM", vars: [ScreensaverPackInit.SaversConfig.LinotypoWritingSpeedMax, CpmSpeedMax]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Maximum characters: {0} (satisfying {1} columns)", vars: [MaxCharacters, ScreensaverPackInit.SaversConfig.LinotypoTextColumns]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Width threshold: {0}", vars: [ColumnRowConsoleThreshold]);

            // Strikes
            var Strikes = new List<string>() { "q`12wsa", "r43edfgt5", "u76yhjki8", @"p09ol;'[-=]\", "/';. ", "m,lkjn ", "vbhgfc ", "zxdsa " };
            var CapStrikes = new List<string>() { "Q~!@WSA", "R$#EDFGT%", "U&^YHJKI*", "P)(OL:\"{_+}|", "?\":> ", "M<LKJN ", "VBHGFC ", "ZXDSA " };
            string CapSymbols = "~!@$#%&^*)(:\"{_+}|?><";
            var LinotypeLayout = new string[,]
            {
                { "e", "t", "a", "o", "i", "n", " " },
                { "s", "h", "r", "d", "l", "u", " " },
                { "c", "m", "f", "w", "y", "p", " " },
                { "v", "b", "g", "k", "q", "j", " " },
                { "x", "z", " ", " ", " ", " ", " " },
                { " ", " ", " ", " ", " ", " ", " " }
            };

            // Other variables
            var CountingCharacters = false;
            var CharacterCounter = 0;
            var EtaoinMode = false;
            var CappedEtaoin = false;
            string LinotypeWrite = ScreensaverPackInit.SaversConfig.LinotypoWrite;

            // Linotypo can also deal with files written on the field that is used for storing text, so check to see if the path exists.
            DebugWriter.WriteDebug(DebugLevel.I, "Checking \"{0}\" to see if it's a file path", vars: [ScreensaverPackInit.SaversConfig.LinotypoWrite]);
            if (FilesystemTools.TryParsePath(ScreensaverPackInit.SaversConfig.LinotypoWrite) && FilesystemTools.FileExists(ScreensaverPackInit.SaversConfig.LinotypoWrite))
            {
                // File found! Now, write the contents of it to the local variable that stores the actual written text.
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Opening file {0} to write...", vars: [ScreensaverPackInit.SaversConfig.LinotypoWrite]);
                LinotypeWrite = FilesystemTools.ReadContentsText(ScreensaverPackInit.SaversConfig.LinotypoWrite);
            }

            // For each line, write four spaces, and extra two spaces if paragraph starts.
            var StrikeCharsIndex1 = 0;
            foreach (string Paragraph in LinotypeWrite.SplitNewLines())
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "New paragraph: {0}", vars: [Paragraph]);

                // Sometimes, a paragraph could consist of nothing, but prints its new line, so honor this by checking to see if we need to
                // clear screen or advance to the next column so that we don't mess up the display by them
                HandleNextColumn(ref CurrentColumn, ref CurrentColumnRowConsole, ColumnRowConsoleThreshold);

                // We need to make sure that we indent spaces for each new paragraph.
                if (CurrentColumn == 1)
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Column 1. Printing newline...");
                    ConsoleWrapper.WriteLine();
                }
                else
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Column {0}. Setting left to {1}...", vars: [CurrentColumn, CurrentColumnRowConsole]);
                    ConsoleWrapper.SetCursorPosition(CurrentColumnRowConsole, ConsoleWrapper.CursorTop + 1);
                }
                ConsoleWrapper.Write("    ");
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", vars: [ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop]);
                bool NewLineDone = true;

                // Split the paragraph into sentences that have the length of maximum characters that can be printed for each column
                // in various terminal sizes. This enables us to easily tell if we're going to re-write the line after a typo and the
                // line completion that consists of "Etaoin shrdlu" and other nonsense written sometimes on newspapers or ads back in
                // the early 20th century.
                var IncompleteSentences = TextTools.GetWrappedSentences(Paragraph, MaxCharacters, 4);

                // Get struck character and write it
                for (int IncompleteSentenceIndex = 0; IncompleteSentenceIndex <= IncompleteSentences.Length - 1; IncompleteSentenceIndex++)
                {
                    string IncompleteSentence = IncompleteSentences[IncompleteSentenceIndex];
                    if (ConsoleResizeHandler.WasResized(false))
                        break;

                    // Check if we need to indent a sentence
                    if (!NewLineDone)
                    {
                        if (CurrentColumn == 1)
                        {
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Column 1. Printing newline...");
                            ConsoleWrapper.WriteLine();
                        }
                        else
                        {
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Column {0}. Setting left to {1}...", vars: [CurrentColumn, CurrentColumnRowConsole]);
                            ConsoleWrapper.SetCursorPosition(CurrentColumnRowConsole, ConsoleWrapper.CursorTop + 1);
                        }
                    }
                    ConsoleWrapper.Write("  ");
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", vars: [ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop]);

                    // We need to store which column and which key from the linotype keyboard layout is taken.
                    int LinotypeColumnIndex = 0;
                    int LinotypeKeyIndex = 0;
                    int LinotypeMaxColumnIndex = 5;

                    // Process the incomplete sentences
                    for (int StruckCharIndex = 0; StruckCharIndex <= IncompleteSentence.Length - 1; StruckCharIndex++)
                    {
                        if (ConsoleResizeHandler.WasResized(false))
                            break;

                        // Sometimes, typing error can be made in the last line and the line is repeated on the first line in the different
                        // column, but it ruins the overall beautiful look of the paragraphs, considering how it is split in columns. We
                        // need to re-indent the sentence.
                        if (ConsoleWrapper.CursorTop == 0)
                        {
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Line repeat in first line in new column. Indenting...");
                            if (CurrentColumn == 1)
                            {
                                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Column 1. Printing newline...");
                                ConsoleWrapper.WriteLine();
                            }
                            else
                            {
                                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Column {0}. Setting left to {1}...", vars: [CurrentColumn, CurrentColumnRowConsole]);
                                ConsoleWrapper.SetCursorPosition(CurrentColumnRowConsole, ConsoleWrapper.CursorTop + 1);
                            }
                            ConsoleWrapper.Write("  ");
                            if (IncompleteSentenceIndex == 0)
                                ConsoleWrapper.Write("    ");
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", vars: [ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop]);
                        }

                        // Select a character
                        char StruckChar = IncompleteSentence[StruckCharIndex];
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Struck character: {0}", vars: [StruckChar]);

                        // Calculate needed milliseconds from two WPM speeds (minimum and maximum)
                        int SelectedCpm = RandomDriver.Random(CpmSpeedMin, CpmSpeedMax);
                        int WriteMs = (int)Math.Round(60d / SelectedCpm * 1000d);
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Delay for {0} CPM: {1} ms", vars: [SelectedCpm, WriteMs]);

                        // Choose a character depending on the current mode
                        if (EtaoinMode)
                        {
                            // Doing this in linotype machines after spotting an error usually triggers a speed boost, because the authors
                            // that used this machine back then considered it as a quick way to fill the faulty line.
                            WriteMs = (int)Math.Round(WriteMs / (1d + RandomDriver.RandomDouble()));
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Etaoin mode on. Delaying {0} ms...", vars: [WriteMs]);

                            // Get the character
                            StruckChar = Convert.ToChar(LinotypeLayout[LinotypeColumnIndex, LinotypeKeyIndex]);
                            if (CappedEtaoin)
                            {
                                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Capped Etaoin.");
                                StruckChar = char.ToUpper(StruckChar);
                            }

                            // Advance the indexes of column and key, depending on their values, and get the character
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Etaoin type: {0}", vars: [ScreensaverPackInit.SaversConfig.LinotypoEtaoinType]);
                            switch (ScreensaverPackInit.SaversConfig.LinotypoEtaoinType)
                            {
                                case FillType.EtaoinComplete:
                                case FillType.EtaoinPattern:
                                    {
                                        if (ScreensaverPackInit.SaversConfig.LinotypoEtaoinType == (int)FillType.EtaoinPattern)
                                            LinotypeMaxColumnIndex = 1;

                                        // Increment the key (and optionally column) index. If both exceed the max limit, reset both to zero.
                                        LinotypeKeyIndex += 1;
                                        if (LinotypeKeyIndex == 7)
                                        {
                                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Key index exceeded 7. Advancing to next column...");
                                            LinotypeKeyIndex = 0;
                                            LinotypeColumnIndex += 1;
                                        }
                                        if (LinotypeColumnIndex == LinotypeMaxColumnIndex + 1)
                                        {
                                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Column indexes exceeded. Resetting...");
                                            LinotypeColumnIndex = 0;
                                            LinotypeKeyIndex = 0;
                                        }

                                        break;
                                    }
                                case FillType.RandomChars:
                                    {
                                        // Randomly select the linotype indexes
                                        LinotypeColumnIndex = RandomDriver.Random(0, 5);
                                        LinotypeKeyIndex = RandomDriver.Random(0, 6);
                                        break;
                                    }
                            }
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Key index: {0} | Column index: {1}", vars: [LinotypeKeyIndex, LinotypeColumnIndex]);
                        }
                        else
                        {
                            // See if the typo is guaranteed
                            double Probability = (ScreensaverPackInit.SaversConfig.LinotypoMissStrikePossibility >= 5 ? 5 : ScreensaverPackInit.SaversConfig.LinotypoMissStrikePossibility) / 100d;
                            bool LinotypoGuaranteed = RandomDriver.RandomChance(Probability);
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Probability: {0} Guarantee: {1}", vars: [Probability, LinotypoGuaranteed]);
                            if (LinotypoGuaranteed)
                            {
                                // Sometimes, a typo is generated by missing a character.
                                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Made a typo!");
                                double MissProbability = (ScreensaverPackInit.SaversConfig.LinotypoMissPossibility >= 10 ? 10 : ScreensaverPackInit.SaversConfig.LinotypoMissPossibility) / 100d;
                                bool MissGuaranteed = RandomDriver.RandomChance(MissProbability);
                                if (MissGuaranteed)
                                {
                                    // Miss is guaranteed. Simulate the missed character
                                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Missed a character!");
                                    StruckChar = Convert.ToChar("\0");
                                }
                                // Typo is guaranteed. Select a strike string randomly until the struck key is found in between the characters
                                else
                                {
                                    bool StruckFound = false;
                                    bool CappedStrike = false;
                                    string StrikesString = "";
                                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Bruteforcing...");
                                    while (!StruckFound)
                                    {
                                        StrikeCharsIndex1 = RandomDriver.RandomIdx(0, Strikes.Count);
                                        CappedStrike = char.IsUpper(StruckChar) | CapSymbols.Contains(StruckChar);
                                        StrikesString = CappedStrike ? CapStrikes[StrikeCharsIndex1] : Strikes[StrikeCharsIndex1];
                                        StruckFound = !string.IsNullOrEmpty(StrikesString) && StrikesString.Contains(StruckChar);
                                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Strike chars index: {0}", vars: [StrikeCharsIndex1]);
                                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Capped strike: {0}", vars: [CappedStrike]);
                                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Strikes pattern: {0}", vars: [StrikesString]);
                                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Found? {0}", vars: [StruckFound]);
                                    }
                                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Found!");

                                    // Select a random character that is a typo from the selected strike index
                                    int RandomStrikeIndex = RandomDriver.RandomIdx(0, StrikesString.Length);
                                    char MistypedChar = StrikesString[RandomStrikeIndex];
                                    if (@"`-=\][';/.,".Contains(MistypedChar) & CappedStrike)
                                    {
                                        // The mistyped character is a symbol and the strike is capped. Select a symbol from CapStrikes.
                                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Mistyped character is a symbol and the strike is capped.");
                                        MistypedChar = CapStrikes[StrikeCharsIndex1][RandomStrikeIndex];
                                    }
                                    StruckChar = MistypedChar;
                                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Struck character: {0}", vars: [StruckChar]);
                                }

                                // Randomly select whether or not to turn on the capped Etaoin
                                double CappingProbability = (ScreensaverPackInit.SaversConfig.LinotypoEtaoinCappingPossibility >= 10 ? 10 : ScreensaverPackInit.SaversConfig.LinotypoEtaoinCappingPossibility) / 100d;
                                CappedEtaoin = RandomDriver.RandomChance(CappingProbability);
                                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Capped Etaoin: {0}", vars: [CappedEtaoin]);

                                // Trigger character counter mode
                                CountingCharacters = true;
                                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Counting...");
                            }
                        }

                        // Write the final character to the console and wait
                        if (StruckChar != Convert.ToChar(0))
                            ConsoleWrapper.Write(StruckChar);
                        ScreensaverManager.Delay(WriteMs);

                        // If we're on the character counter mode, increment this for every character until the "line fill" mode starts
                        if (CountingCharacters)
                        {
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Counter increased. {0}", vars: [CharacterCounter]);
                            CharacterCounter += 1;
                            if (CharacterCounter > ScreensaverPackInit.SaversConfig.LinotypoEtaoinThreshold)
                            {
                                // We've reached the Etaoin threshold. Turn on that mode and stop counting characters.
                                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Etaoin mode on because threshold reached.");
                                EtaoinMode = true;
                                CountingCharacters = false;
                                CharacterCounter = 0;
                            }
                        }

                        // If we're on the Etaoin mode and we've reached the end of incomplete sentence, reset the index to 0 and do the
                        // necessary changes.
                        if (EtaoinMode & (StruckCharIndex == MaxCharacters - 1 | StruckCharIndex == IncompleteSentence.Length - 1))
                        {
                            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Etaoin mode off because end of sentence.");
                            StruckCharIndex = -1;
                            EtaoinMode = false;
                            if (ConsoleWrapper.CursorTop >= ConsoleWrapper.WindowHeight - 2)
                            {
                                HandleNextColumn(ref CurrentColumn, ref CurrentColumnRowConsole, ColumnRowConsoleThreshold);
                            }
                            else
                            {
                                if (CurrentColumn == 1)
                                {
                                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Column 1. Printing newline...");
                                    ConsoleWrapper.WriteLine();
                                }
                                else
                                {
                                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Column {0}. Setting left to {1}...", vars: [CurrentColumn, CurrentColumnRowConsole]);
                                    ConsoleWrapper.SetCursorPosition(CurrentColumnRowConsole, ConsoleWrapper.CursorTop + 1);
                                }
                                ConsoleWrapper.Write("  ");
                                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", vars: [ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop]);
                            }
                        }
                    }

                    // Let the next sentence generate a new line
                    NewLineDone = false;

                    // It's possible that the writer might have made an error on writing a line on the very end of it where the threshold is
                    // lower than the partial sentence being written, so don't do the Etaoin pattern in this case, but re-write the text as
                    // if the error is being made.
                    if (CountingCharacters)
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Sentence ended before Etaoin mode is activated. Resetting counter...");
                        CountingCharacters = false;
                        CharacterCounter = 0;
                        IncompleteSentenceIndex -= 1;
                    }

                    // See if the current cursor is on the bottom so we can make the second column, if we have more than a column assigned
                    HandleNextColumn(ref CurrentColumn, ref CurrentColumnRowConsole, ColumnRowConsoleThreshold);
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.LinotypoDelay);
        }

        /// <summary>
        /// Instructs the Linotypo screensaver to go to the next column
        /// </summary>
        private void HandleNextColumn(ref int CurrentColumn, ref int CurrentColumnRowConsole, int ColumnRowConsoleThreshold)
        {
            if (ScreensaverPackInit.SaversConfig.LinotypoTextColumns > 1)
            {
                if (ConsoleWrapper.CursorTop >= ConsoleWrapper.WindowHeight - 2)
                {
                    // We're on the bottom, so...
                    if (CurrentColumn >= ScreensaverPackInit.SaversConfig.LinotypoTextColumns)
                    {
                        // ...wait until retry
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Drawn all columns. Waiting {0} ms...", vars: [ScreensaverPackInit.SaversConfig.LinotypoNewScreenDelay]);
                        ConsoleWrapper.WriteLine();
                        ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.LinotypoNewScreenDelay);

                        // ...and make a new screen
                        ConsoleWrapper.Clear();
                        CurrentColumn = 1;
                        CurrentColumnRowConsole = ConsoleWrapper.CursorLeft;
                    }
                    else
                    {
                        // ...we're moving to the next column
                        CurrentColumn += 1;
                        CurrentColumnRowConsole += ColumnRowConsoleThreshold;
                        ConsoleWrapper.SetCursorPosition(CurrentColumnRowConsole, 0);
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "New column. Moving to {0}...", vars: [CurrentColumnRowConsole]);
                    }
                }
            }
            else if (ScreensaverPackInit.SaversConfig.LinotypoTextColumns == 1 & ConsoleWrapper.CursorTop >= ConsoleWrapper.WindowHeight - 2)
            {
                // We're on the bottom, so wait until retry...
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Drawn all text. Waiting {0} ms...", vars: [ScreensaverPackInit.SaversConfig.LinotypoNewScreenDelay]);
                ConsoleWrapper.WriteLine();
                ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.LinotypoNewScreenDelay);

                // ...and make a new screen
                ConsoleWrapper.Clear();
                CurrentColumn = 1;
                CurrentColumnRowConsole = ConsoleWrapper.CursorLeft;
            }
        }

    }
}
