
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Typo
    /// </summary>
    public static class TypoSettings
    {

        /// <summary>
        /// [Typo] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int TypoDelay
        {
            get
            {
                return Config.SaverConfig.TypoDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                Config.SaverConfig.TypoDelay = value;
            }
        }
        /// <summary>
        /// [Typo] How many milliseconds to wait before writing the text again?
        /// </summary>
        public static int TypoWriteAgainDelay
        {
            get
            {
                return Config.SaverConfig.TypoWriteAgainDelay;
            }
            set
            {
                if (value <= 0)
                    value = 3000;
                Config.SaverConfig.TypoWriteAgainDelay = value;
            }
        }
        /// <summary>
        /// [Typo] Text for Typo. Longer is better.
        /// </summary>
        public static string TypoWrite
        {
            get
            {
                return Config.SaverConfig.TypoWrite;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "Nitrocid KS";
                Config.SaverConfig.TypoWrite = value;
            }
        }
        /// <summary>
        /// [Typo] Minimum writing speed in WPM
        /// </summary>
        public static int TypoWritingSpeedMin
        {
            get
            {
                return Config.SaverConfig.TypoWritingSpeedMin;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                Config.SaverConfig.TypoWritingSpeedMin = value;
            }
        }
        /// <summary>
        /// [Typo] Maximum writing speed in WPM
        /// </summary>
        public static int TypoWritingSpeedMax
        {
            get
            {
                return Config.SaverConfig.TypoWritingSpeedMax;
            }
            set
            {
                if (value <= 0)
                    value = 80;
                Config.SaverConfig.TypoWritingSpeedMax = value;
            }
        }
        /// <summary>
        /// [Typo] Possibility that the writer made a typo in percent
        /// </summary>
        public static int TypoMissStrikePossibility
        {
            get
            {
                return Config.SaverConfig.TypoMissStrikePossibility;
            }
            set
            {
                if (value <= 0)
                    value = 20;
                Config.SaverConfig.TypoMissStrikePossibility = value;
            }
        }
        /// <summary>
        /// [Typo] Possibility that the writer missed a character in percent
        /// </summary>
        public static int TypoMissPossibility
        {
            get
            {
                return Config.SaverConfig.TypoMissPossibility;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                Config.SaverConfig.TypoMissPossibility = value;
            }
        }
        /// <summary>
        /// [Typo] Text color
        /// </summary>
        public static string TypoTextColor
        {
            get
            {
                return Config.SaverConfig.TypoTextColor;
            }
            set
            {
                Config.SaverConfig.TypoTextColor = new Color(value).PlainSequence;
            }
        }

    }

    /// <summary>
    /// Display code for Typo
    /// </summary>
    public class TypoDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Typo";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            KernelColorTools.SetConsoleColor(new Color(TypoSettings.TypoTextColor));
            ConsoleWrapper.Clear();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int CpmSpeedMin = TypoSettings.TypoWritingSpeedMin * 5;
            int CpmSpeedMax = TypoSettings.TypoWritingSpeedMax * 5;
            var Strikes = new List<string>() { "q`12wsa", "r43edfgt5", "u76yhjki8", @"p09ol;'[-=]\", "/';. ", "m,lkjn ", "vbhgfc ", "zxdsa " };
            var CapStrikes = new List<string>() { "Q~!@WSA", "R$#EDFGT%", "U&^YHJKI*", "P)(OL:\"{_+}|", "?\":> ", "M<LKJN ", "VBHGFC ", "ZXDSA " };
            string CapSymbols = "~!@$#%&^*)(:\"{_+}|?><";

            ConsoleWrapper.CursorVisible = false;

            // Prepare display (make a paragraph indentation)
            ConsoleWrapper.WriteLine();
            ConsoleWrapper.Write("    ");
            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Indented in {0}, {1}", ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);

            // Get struck character and write it
            var StrikeCharsIndex = 0;
            foreach (char StruckChar in TypoSettings.TypoWrite)
            {
                char StruckCharAssigned = StruckChar;

                // Check to see if we can go ahead
                if (ConsoleResizeListener.WasResized(false))
                    break;

                // Calculate needed milliseconds from two WPM speeds (minimum and maximum)
                int SelectedCpm = RandomDriver.Random(CpmSpeedMin, CpmSpeedMax);
                int WriteMs = (int)Math.Round(60d / SelectedCpm * 1000d);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Delay for {0} CPM: {1} ms", SelectedCpm, WriteMs);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Struck character: {0}", StruckCharAssigned);

                // See if the typo is guaranteed
                double Probability = (TypoSettings.TypoMissStrikePossibility >= 80 ? 80 : TypoSettings.TypoMissStrikePossibility) / 100d;
                bool TypoGuaranteed = RandomDriver.RandomChance(Probability);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Probability: {0} Guarantee: {1}", Probability, TypoGuaranteed);
                if (TypoGuaranteed)
                {
                    // Sometimes, a typo is generated by missing a character.
                    DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Made a typo!");
                    double MissProbability = (TypoSettings.TypoMissPossibility >= 10 ? 10 : TypoSettings.TypoMissPossibility) / 100d;
                    bool MissGuaranteed = RandomDriver.RandomChance(MissProbability);
                    if (MissGuaranteed)
                    {
                        // Miss is guaranteed. Simulate the missed character
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Missed a character!");
                        StruckCharAssigned = Convert.ToChar("\0");
                    }
                    // Typo is guaranteed. Select a strike string randomly until the struck key is found in between the characters
                    else
                    {
                        bool StruckFound = false;
                        bool CappedStrike = false;
                        string StrikesString = "";
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Bruteforcing...");
                        while (!StruckFound)
                        {
                            StrikeCharsIndex = RandomDriver.RandomIdx(0, Strikes.Count);
                            CappedStrike = char.IsUpper(StruckCharAssigned) | CapSymbols.Contains(Convert.ToString(StruckCharAssigned));
                            StrikesString = CappedStrike ? CapStrikes[StrikeCharsIndex] : Strikes[StrikeCharsIndex];
                            StruckFound = !string.IsNullOrEmpty(StrikesString) && StrikesString.Contains(Convert.ToString(StruckCharAssigned));
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Strike chars index: {0}", StrikeCharsIndex);
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Capped strike: {0}", CappedStrike);
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Strikes pattern: {0}", StrikesString);
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Found? {0}", StruckFound);
                        }
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Found!");

                        // Select a random character that is a typo from the selected strike index
                        int RandomStrikeIndex = RandomDriver.RandomIdx(0, StrikesString.Length);
                        char MistypedChar = StrikesString[RandomStrikeIndex];
                        if (@"`-=\][';/.,".Contains(Convert.ToString(MistypedChar)) & CappedStrike)
                        {
                            // The mistyped character is a symbol and the strike is capped. Select a symbol from CapStrikes.
                            DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Mistyped character is a symbol and the strike is capped.");
                            MistypedChar = CapStrikes[StrikeCharsIndex][RandomStrikeIndex];
                        }
                        StruckCharAssigned = MistypedChar;
                        DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Struck character: {0}", StruckCharAssigned);
                    }
                }

                // Write the final character to the console and wait
                if (!(StruckCharAssigned == Convert.ToChar(0)))
                    ConsoleWrapper.Write(StruckCharAssigned);
                ThreadManager.SleepNoBlock(WriteMs, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Wait until retry
            ConsoleWrapper.WriteLine();
            if (!ConsoleResizeListener.WasResized(false))
                ThreadManager.SleepNoBlock(TypoSettings.TypoWriteAgainDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(TypoSettings.TypoDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
