
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using KS.Drivers.RNG;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.TimeDate;
using static Namer.NameGenerator;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for PersonLookup
    /// </summary>
    public static class PersonLookupSettings
    {

        private static int _Delay = 75;
        private static int _LookedUpDelay = 10000;
        private static int _MinimumNames = 10;
        private static int _MaximumNames = 100;
        private static int _MinimumAgeYears = 18;
        private static int _MaximumAgeYears = 100;

        /// <summary>
        /// [PersonLookup] How many milliseconds to wait before getting the new name?
        /// </summary>
        public static int PersonLookupDelay
        {
            get
            {
                return _Delay;
            }
            set
            {
                if (value <= 0)
                    value = 75;
                _Delay = value;
            }
        }
        /// <summary>
        /// [PersonLookup] How many milliseconds to show the looked up name?
        /// </summary>
        public static int PersonLookupLookedUpDelay
        {
            get
            {
                return _LookedUpDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                _LookedUpDelay = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Minimum names count
        /// </summary>
        public static int PersonLookupMinimumNames
        {
            get
            {
                return _MinimumNames;
            }
            set
            {
                if (value <= 10)
                    value = 10;
                if (value > 1000)
                    value = 1000;
                _MinimumNames = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Maximum names count
        /// </summary>
        public static int PersonLookupMaximumNames
        {
            get
            {
                return _MaximumNames;
            }
            set
            {
                if (value <= _MinimumNames)
                    value = _MinimumNames;
                if (value > 1000)
                    value = 1000;
                _MaximumNames = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Minimum age years
        /// </summary>
        public static int PersonLookupMinimumAgeYears
        {
            get
            {
                return _MinimumAgeYears;
            }
            set
            {
                if (value <= 18)
                    value = 18;
                if (value > 100)
                    value = 100;
                _MinimumAgeYears = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Maximum age years
        /// </summary>
        public static int PersonLookupMaximumAgeYears
        {
            get
            {
                return _MaximumAgeYears;
            }
            set
            {
                if (value <= _MinimumAgeYears)
                    value = _MinimumAgeYears;
                if (value > 100)
                    value = 100;
                _MaximumAgeYears = value;
            }
        }

    }

    /// <summary>
    /// Display code for PersonLookup
    /// </summary>
    public class PersonLookupDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "PersonLookup";

        /// <inheritdoc/>
        public override void ScreensaverPreparation() => PopulateNames();

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleBase.ConsoleWrapper.BackgroundColor = ConsoleColor.Black;
            ConsoleBase.ConsoleWrapper.ForegroundColor = ConsoleColor.Green;
            ConsoleBase.ConsoleWrapper.Clear();
            ConsoleBase.ConsoleWrapper.CursorVisible = false;

            // Generate names
            int NumberOfPeople = RandomDriver.Random(PersonLookupSettings.PersonLookupMinimumNames, PersonLookupSettings.PersonLookupMaximumNames);
            var NamesToLookup = GenerateNames(NumberOfPeople);

            // Loop through names
            foreach (string GeneratedName in NamesToLookup)
            {
                int Age = RandomDriver.Random(PersonLookupSettings.PersonLookupMinimumAgeYears, PersonLookupSettings.PersonLookupMaximumAgeYears);
                int AgeMonth = RandomDriver.Random(-12, 12);
                int AgeDay = RandomDriver.Random(-31, 31);
                var Birthdate = DateTime.Now.AddYears(-Age).AddMonths(AgeMonth).AddDays(AgeDay);
                int FinalAge = new DateTime((DateTime.Now - Birthdate).Ticks).Year;
                string FirstName = GeneratedName.Substring(0, GeneratedName.IndexOf(" "));
                string LastName = GeneratedName.Substring(GeneratedName.IndexOf(" ") + 1);

                // Print all information
                ConsoleBase.ConsoleWrapper.Clear();
                TextWriterWhereColor.WriteWhere("  - Name:                {0}", 0, 1, false, GeneratedName);
                TextWriterWhereColor.WriteWhere("  - First Name:          {0}", 0, 2, false, FirstName);
                TextWriterWhereColor.WriteWhere("  - Last Name / Surname: {0}", 0, 3, false, LastName);
                TextWriterWhereColor.WriteWhere("  - Age:                 {0} years old", 0, 4, false, 0, FinalAge);
                TextWriterWhereColor.WriteWhere("  - Birth date:          {0}", 0, 5, false, TimeDateRenderers.Render(Birthdate));

                // Lookup delay
                ThreadManager.SleepNoBlock(PersonLookupSettings.PersonLookupDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Wait until we run the lookup again
            ThreadManager.SleepNoBlock(PersonLookupSettings.PersonLookupLookedUpDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
