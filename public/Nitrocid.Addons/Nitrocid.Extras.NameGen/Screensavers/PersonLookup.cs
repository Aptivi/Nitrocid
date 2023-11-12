//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using KS.ConsoleBase;
using KS.Drivers.RNG;
using KS.Kernel.Threading;
using KS.Kernel.Time.Renderers;
using KS.Misc.Screensaver;
using Terminaux.Colors;
using Namer;
using KS.ConsoleBase.Inputs.Styles;

namespace Nitrocid.Extras.NameGen.Screensavers
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
                return NameGenInit.SaversConfig.PersonLookupDelay;
            }
            set
            {
                if (value <= 0)
                    value = 75;
                NameGenInit.SaversConfig.PersonLookupDelay = value;
            }
        }
        /// <summary>
        /// [PersonLookup] How many milliseconds to show the looked up name?
        /// </summary>
        public static int PersonLookupLookedUpDelay
        {
            get
            {
                return NameGenInit.SaversConfig.PersonLookupLookedUpDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10000;
                NameGenInit.SaversConfig.PersonLookupLookedUpDelay = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Minimum names count
        /// </summary>
        public static int PersonLookupMinimumNames
        {
            get
            {
                return NameGenInit.SaversConfig.PersonLookupMinimumNames;
            }
            set
            {
                if (value <= 10)
                    value = 10;
                if (value > 1000)
                    value = 1000;
                NameGenInit.SaversConfig.PersonLookupMinimumNames = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Maximum names count
        /// </summary>
        public static int PersonLookupMaximumNames
        {
            get
            {
                return NameGenInit.SaversConfig.PersonLookupMaximumNames;
            }
            set
            {
                if (value <= NameGenInit.SaversConfig.PersonLookupMinimumNames)
                    value = NameGenInit.SaversConfig.PersonLookupMinimumNames;
                if (value > 1000)
                    value = 1000;
                NameGenInit.SaversConfig.PersonLookupMaximumNames = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Minimum age years
        /// </summary>
        public static int PersonLookupMinimumAgeYears
        {
            get
            {
                return NameGenInit.SaversConfig.PersonLookupMinimumAgeYears;
            }
            set
            {
                if (value <= 18)
                    value = 18;
                if (value > 100)
                    value = 100;
                NameGenInit.SaversConfig.PersonLookupMinimumAgeYears = value;
            }
        }
        /// <summary>
        /// [PersonLookup] Maximum age years
        /// </summary>
        public static int PersonLookupMaximumAgeYears
        {
            get
            {
                return NameGenInit.SaversConfig.PersonLookupMaximumAgeYears;
            }
            set
            {
                if (value <= NameGenInit.SaversConfig.PersonLookupMinimumAgeYears)
                    value = NameGenInit.SaversConfig.PersonLookupMinimumAgeYears;
                if (value > 100)
                    value = 100;
                NameGenInit.SaversConfig.PersonLookupMaximumAgeYears = value;
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
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();

            // Populate the names
            InfoBoxColor.WriteInfoBoxColor("Welcome to the database! Fetching identities...", false, ConsoleColors.Green);
            NameGenerator.PopulateNames();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;

            // Generate names
            int NumberOfPeople = RandomDriver.Random(PersonLookupSettings.PersonLookupMinimumNames, PersonLookupSettings.PersonLookupMaximumNames);
            var NamesToLookup = NameGenerator.GenerateNames(NumberOfPeople);

            // Loop through names
            foreach (string GeneratedName in NamesToLookup)
            {
                // Get random age (initial) and its month and day components
                int Age = RandomDriver.Random(PersonLookupSettings.PersonLookupMinimumAgeYears, PersonLookupSettings.PersonLookupMaximumAgeYears);
                int AgeMonth = RandomDriver.Random(-12, 12);
                int AgeDay = RandomDriver.Random(-31, 31);

                // Get random birth time
                int birthHour = RandomDriver.Random(0, 23);
                int birthMinute = RandomDriver.Random(0, 59);
                int birthSecond = RandomDriver.Random(0, 59);

                // Form the final birthdate and the age
                var Birthdate = DateTime.Now
                    .AddYears(-Age)
                    .AddMonths(AgeMonth)
                    .AddDays(AgeDay)
                    .AddHours(birthHour)
                    .AddMinutes(birthMinute)
                    .AddSeconds(birthSecond);
                int FinalAge = new DateTime((DateTime.Now - Birthdate).Ticks).Year - 1;

                // Get the first and the last name
                string FirstName = GeneratedName[..GeneratedName.IndexOf(" ")];
                string LastName = GeneratedName[(GeneratedName.IndexOf(" ") + 1)..];

                // Print all information
                ConsoleWrapper.Clear();
                InfoBoxColor.WriteInfoBoxColor(
                    "- Name:                  {0}\n" +
                   $"{new string('=', $"- Name:                  {GeneratedName}".Length)}\n" +
                    "\n" +
                    "  - First Name:          {1}\n" +
                    "  - Last Name / Surname: {2}\n" +
                    "  - Age:                 {3} years old\n" +
                    "  - Birth date:          {4}\n",
                    
                    // We don't want to wait for input as we're on the screensaver environment.
                    false,
                    ConsoleColors.Green,

                    // Necessary variables to print
                    new object[] { GeneratedName, FirstName, LastName, FinalAge, TimeDateRenderers.Render(Birthdate) }
                );

                // Lookup delay
                ThreadManager.SleepNoBlock(PersonLookupSettings.PersonLookupDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Wait until we run the lookup again
            ThreadManager.SleepNoBlock(PersonLookupSettings.PersonLookupLookedUpDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
