using System;
using System.Collections.Generic;
using ColorSeq;
using KS.ConsoleBase.Colors;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;
using Microsoft.VisualBasic.CompilerServices;

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

namespace KS.Misc.Screensaver.Displays
{
    public static class TypoSettings
    {

        private static int _typoDelay = 50;
        private static int _typoWriteAgainDelay = 3000;
        private static string _typoWrite = "Kernel Simulator";
        private static int _typoWritingSpeedMin = 50;
        private static int _typoWritingSpeedMax = 80;
        private static int _typoMissStrikePossibility = 20;
        private static int _typoMissPossibility = 10;
        private static string _typoTextColor = new Color((int)ConsoleColor.White).PlainSequence;

        /// <summary>
        /// [Typo] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int TypoDelay
        {
            get
            {
                return _typoDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _typoDelay = value;
            }
        }
        /// <summary>
        /// [Typo] How many milliseconds to wait before writing the text again?
        /// </summary>
        public static int TypoWriteAgainDelay
        {
            get
            {
                return _typoWriteAgainDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                _typoWriteAgainDelay = value;
            }
        }
        /// <summary>
        /// [Typo] Text for Typo. Longer is better.
        /// </summary>
        public static string TypoWrite
        {
            get
            {
                return _typoWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Kernel Simulator";
                _typoWrite = value;
            }
        }
        /// <summary>
        /// [Typo] Minimum writing speed in WPM
        /// </summary>
        public static int TypoWritingSpeedMin
        {
            get
            {
                return _typoWritingSpeedMin;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                _typoWritingSpeedMin = value;
            }
        }
        /// <summary>
        /// [Typo] Maximum writing speed in WPM
        /// </summary>
        public static int TypoWritingSpeedMax
        {
            get
            {
                return _typoWritingSpeedMax;
            }
            set
            {
                if (value <= 0)
                    value = 80;
                _typoWritingSpeedMax = value;
            }
        }
        /// <summary>
        /// [Typo] Possibility that the writer made a typo in percent
        /// </summary>
        public static int TypoMissStrikePossibility
        {
            get
            {
                return _typoMissStrikePossibility;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                _typoMissStrikePossibility = value;
            }
        }
        /// <summary>
        /// [Typo] Possibility that the writer missed a character in percent
        /// </summary>
        public static int TypoMissPossibility
        {
            get
            {
                return _typoMissPossibility;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                _typoMissPossibility = value;
            }
        }
        /// <summary>
        /// [Typo] Text color
        /// </summary>
        public static string TypoTextColor
        {
            get
            {
                return _typoTextColor;
            }
            set
            {
                _typoTextColor = new Color(value).PlainSequence;
            }
        }

    }
    public class TypoDisplay : BaseScreensaver, IScreensaver
    {

        private Random RandomDriver;
        private int CurrentWindowWidth;
        private int CurrentWindowHeight;
        private bool ResizeSyncing;

        public override string ScreensaverName { get; set; } = "Typo";

        public override Dictionary<string, object> ScreensaverSettings { get; set; }

        public override void ScreensaverPreparation()
        {
            // Variable preparations
            RandomDriver = new Random();
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;
            ColorTools.SetConsoleColor(new Color(TypoSettings.TypoTextColor));
            Console.Clear();
        }

        public override void ScreensaverLogic()
        {
            int CpmSpeedMin = TypoSettings.TypoWritingSpeedMin * 5;
            int CpmSpeedMax = TypoSettings.TypoWritingSpeedMax * 5;
            var Strikes = new List<string>() { "q`12wsa", "r43edfgt5", "u76yhjki8", @"p09ol;'[-=]\", "/';. ", "m,lkjn ", "vbhgfc ", "zxdsa " };
            var CapStrikes = new List<string>() { "Q~!@WSA", "R$#EDFGT%", "U&^YHJKI*", "P)(OL:\"{_+}|", "?\":> ", "M<LKJN ", "VBHGFC ", "ZXDSA " };
            string CapSymbols = "~!@$#%&^*)(:\"{_+}|?><";

            Console.CursorVisible = false;

            // Prepare display (make a paragraph indentation)
            Console.WriteLine();
            Console.Write("    ");
            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", Console.CursorLeft, Console.CursorTop);

            // Get struck character and write it
            var StrikeCharsIndex = default(int);
            foreach (char StruckChar in TypoSettings.TypoWrite)
            {
                if (CurrentWindowHeight != Console.WindowHeight | CurrentWindowWidth != Console.WindowWidth)
                    ResizeSyncing = true;
                if (ResizeSyncing)
                    break;

                // Calculate needed milliseconds from two WPM speeds (minimum and maximum)
                int SelectedCpm = RandomDriver.Next(CpmSpeedMin, CpmSpeedMax);
                int WriteMs = (int)Math.Round(60d / SelectedCpm * 1000d);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Delay for {0} CPM: {1} ms", SelectedCpm, WriteMs);
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Struck character: {0}", StruckChar);

                // See if the typo is guaranteed
                double Probability = (TypoSettings.TypoMissStrikePossibility >= 80 ? 80 : TypoSettings.TypoMissStrikePossibility) / 100d;
                bool TypoGuaranteed = RandomDriver.NextDouble() < Probability;
                DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Probability: {0} Guarantee: {1}", Probability, TypoGuaranteed);
                if (TypoGuaranteed)
                {
                    // Sometimes, a typo is generated by missing a character.
                    DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Made a typo!");
                    double MissProbability = (TypoSettings.TypoMissPossibility >= 10 ? 10 : TypoSettings.TypoMissPossibility) / 100d;
                    bool MissGuaranteed = RandomDriver.NextDouble() < MissProbability;
                    if (MissGuaranteed)
                    {
                        // Miss is guaranteed. Simulate the missed character
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Missed a character!");
                        StruckChar = Conversions.ToChar("");
                    }
                    // Typo is guaranteed. Select a strike string randomly until the struck key is found in between the characters
                    else
                    {
                        bool StruckFound = false;
                        bool CappedStrike = false;
                        string StrikesString = "";
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Bruteforcing...");
                        while (!StruckFound)
                        {
                            ph37273d295ba64e6caaaff99e99013814 = RandomDriver.Next(0, Strikes.Count - 1);
                            CappedStrike = char.IsUpper(StruckChar) | CapSymbols.Contains(Conversions.ToString(StruckChar));
                            StrikesString = CappedStrike ? CapStrikes[ph37273d295ba64e6caaaff99e99013814] : Strikes[ph37273d295ba64e6caaaff99e99013814];
                            StruckFound = !string.IsNullOrEmpty(StrikesString) && StrikesString.Contains(Conversions.ToString(StruckChar));
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Strike chars index: {0}", (object)ph37273d295ba64e6caaaff99e99013814);
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Capped strike: {0}", CappedStrike);
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Strikes pattern: {0}", StrikesString);
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Found? {0}", StruckFound);
                        }
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Found!");

                        // Select a random character that is a typo from the selected strike index
                        int RandomStrikeIndex = RandomDriver.Next(0, StrikesString.Length - 1);
                        char MistypedChar = StrikesString[RandomStrikeIndex];
                        if (@"`-=\][';/.,".Contains(Conversions.ToString(MistypedChar)) & CappedStrike)
                        {
                            // The mistyped character is a symbol and the strike is capped. Select a symbol from CapStrikes.
                            DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Mistyped character is a symbol and the strike is capped.");
                            MistypedChar = CapStrikes[ph37273d295ba64e6caaaff99e99013814][RandomStrikeIndex];
                        }
                        StruckChar = MistypedChar;
                        DebugWriter.WdbgConditional(ref Screensaver.ScreensaverDebug, DebugLevel.I, "Struck character: {0}", StruckChar);
                    }
                }

                // Write the final character to the console and wait
                if (!(Conversions.ToString(StruckChar) == Microsoft.VisualBasic.Constants.vbNullChar))
                    Console.Write(StruckChar);
                ThreadManager.SleepNoBlock(WriteMs, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Wait until retry
            Console.WriteLine();
            if (!ResizeSyncing)
                ThreadManager.SleepNoBlock(TypoSettings.TypoWriteAgainDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ResizeSyncing = false;
            CurrentWindowWidth = Console.WindowWidth;
            CurrentWindowHeight = Console.WindowHeight;
            ThreadManager.SleepNoBlock(TypoSettings.TypoDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}