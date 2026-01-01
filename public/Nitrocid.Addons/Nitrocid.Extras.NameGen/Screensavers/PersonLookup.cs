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
using Textify.Data.NameGen;
using Nitrocid.Misc.Screensaver;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Threading;
using Nitrocid.Kernel.Time.Renderers;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace Nitrocid.Extras.NameGen.Screensavers
{
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
            InfoBoxNonModalColor.WriteInfoBoxColor("Welcome to the database! Fetching identities...", ConsoleColors.Green);
            NameGenerator.PopulateNames();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;

            // Generate names
            int NumberOfPeople = RandomDriver.Random(NameGenInit.SaversConfig.PersonLookupMinimumNames, NameGenInit.SaversConfig.PersonLookupMaximumNames);
            var NamesToLookup = NameGenerator.GenerateNames(NumberOfPeople);

            // Loop through names
            foreach (string GeneratedName in NamesToLookup)
            {
                // Get random age (initial) and its month and day components
                int Age = RandomDriver.Random(NameGenInit.SaversConfig.PersonLookupMinimumAgeYears, NameGenInit.SaversConfig.PersonLookupMaximumAgeYears);
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
                InfoBoxNonModalColor.WriteInfoBoxColor(
                    "- Name:                  {0}\n" +
                   $"{new string('=', $"- Name:                  {GeneratedName}".Length)}\n" +
                    "\n" +
                    "  - First Name:          {1}\n" +
                    "  - Last Name / Surname: {2}\n" +
                    "  - Age:                 {3} years old\n" +
                    "  - Birth date:          {4}\n",

                    // We don't want to wait for input as we're on the screensaver environment.
                    ConsoleColors.Green,

                    // Necessary variables to print
                    [GeneratedName, FirstName, LastName, FinalAge, TimeDateRenderers.Render(Birthdate)]
                );

                // Lookup delay
                ThreadManager.SleepNoBlock(NameGenInit.SaversConfig.PersonLookupDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            }

            // Wait until we run the lookup again
            ThreadManager.SleepNoBlock(NameGenInit.SaversConfig.PersonLookupLookedUpDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}
