
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
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
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

        /// <summary>
        /// [PersonLookup] How many milliseconds to wait before getting the new name?
        /// </summary>
        public static int PersonLookupDelay
        {
            get
            {
                return Config.SaverConfig.PersonLookupDelay;
            }
            set
            {
                if (value <= 0)
                    value = 75;
                Config.SaverConfig.PersonLookupDelay = value;
            }
        }
        /// <summary>
        /// [PersonLookup] How many milliseconds to show the looked up name?
        /// </summary>
        public static int PersonLookupLookedUpDelay
        {
            get
            {
                return Config.SaverConfig.PersonLookupLookedUpDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                Config.SaverConfig.PersonLookupLookedUpDelay = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Minimum names count
        /// </summary>
        public static int PersonLookupMinimumNames
        {
            get
            {
                return Config.SaverConfig.PersonLookupMinimumNames;
            }
            set
            {
                if (value <= 10)
                    value = 10;
                if (value > 1000)
                    value = 1000;
                Config.SaverConfig.PersonLookupMinimumNames = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Maximum names count
        /// </summary>
        public static int PersonLookupMaximumNames
        {
            get
            {
                return Config.SaverConfig.PersonLookupMaximumNames;
            }
            set
            {
                if (value <= Config.SaverConfig.PersonLookupMinimumNames)
                    value = Config.SaverConfig.PersonLookupMinimumNames;
                if (value > 1000)
                    value = 1000;
                Config.SaverConfig.PersonLookupMaximumNames = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Minimum age years
        /// </summary>
        public static int PersonLookupMinimumAgeYears
        {
            get
            {
                return Config.SaverConfig.PersonLookupMinimumAgeYears;
            }
            set
            {
                if (value <= 18)
                    value = 18;
                if (value > 100)
                    value = 100;
                Config.SaverConfig.PersonLookupMinimumAgeYears = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Maximum age years
        /// </summary>
        public static int PersonLookupMaximumAgeYears
        {
            get
            {
                return Config.SaverConfig.PersonLookupMaximumAgeYears;
            }
            set
            {
                if (value <= Config.SaverConfig.PersonLookupMinimumAgeYears)
                    value = Config.SaverConfig.PersonLookupMinimumAgeYears;
                if (value > 100)
                    value = 100;
                Config.SaverConfig.PersonLookupMaximumAgeYears = value;
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
