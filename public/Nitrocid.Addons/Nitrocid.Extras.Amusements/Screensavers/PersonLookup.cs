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
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors.Data;
using Terminaux.Themes.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.ConsoleWriters;
using Textify.Data.NameGen;
using Textify.General;

namespace Nitrocid.Extras.Amusements.Screensavers
{
    /// <summary>
    /// Display code for PersonLookup
    /// </summary>
    public class PersonLookupDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "PersonLookup";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();

            // Populate the names
            var welcomeInfoBox = new InfoBox()
            {
                Text = LanguageTools.GetLocalized("NKS_AMUSEMENTS_PERSONLOOKUP_WELCOME"),
                Settings = new()
                {
                    ForegroundColor = ConsoleColors.Lime
                }
            };
            TextWriterRaw.WriteRaw(welcomeInfoBox.Render());
            NameGenerator.PopulateNames();

            // Prepare again
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Generate names
            int NumberOfPeople = RandomDriver.Random(AmusementsInit.SaversConfig.PersonLookupMinimumNames, AmusementsInit.SaversConfig.PersonLookupMaximumNames);
            var NamesToLookup = NameGenerator.GenerateNames(NumberOfPeople);

            // Loop through names
            foreach (string GeneratedName in NamesToLookup)
            {
                // Get random age (initial) and its month and day components
                int Age = RandomDriver.Random(AmusementsInit.SaversConfig.PersonLookupMinimumAgeYears, AmusementsInit.SaversConfig.PersonLookupMaximumAgeYears);
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
                ThemeColorsTools.LoadBackground();
                string header = ListEntryWriterColor.RenderListEntry(LanguageTools.GetLocalized("NKS_AMUSEMENTS_PERSONLOOKUP_NAME"), GeneratedName);
                var infoBox = new InfoBox()
                {
                    Text =
                        header + "\n" +
                        new string('=', ConsoleChar.EstimateCellWidth(header)) + "\n\n" +
                        ListEntryWriterColor.RenderListEntry(LanguageTools.GetLocalized("NKS_AMUSEMENTS_PERSONLOOKUP_FIRSTNAME"), FirstName) + "\n" +
                        ListEntryWriterColor.RenderListEntry(LanguageTools.GetLocalized("NKS_AMUSEMENTS_PERSONLOOKUP_LASTNAME"), LastName) + "\n" +
                        ListEntryWriterColor.RenderListEntry(LanguageTools.GetLocalized("NKS_AMUSEMENTS_PERSONLOOKUP_AGE"), LanguageTools.GetLocalized("NKS_AMUSEMENTS_PERSONLOOKUP_AGE_YEARSOLD").FormatString(FinalAge)) + "\n" +
                        ListEntryWriterColor.RenderListEntry(LanguageTools.GetLocalized("NKS_AMUSEMENTS_PERSONLOOKUP_BIRTHDATE"), TimeDateRenderers.Render(Birthdate)),
                    Settings = new InfoBoxSettings()
                    {
                        ForegroundColor = ConsoleColors.Lime
                    }
                };
                TextWriterRaw.WriteRaw(infoBox.Render());

                // Lookup delay
                ScreensaverManager.Delay(AmusementsInit.SaversConfig.PersonLookupDelay);
            }

            // Wait until we run the lookup again
            ScreensaverManager.Delay(AmusementsInit.SaversConfig.PersonLookupLookedUpDelay);
        }

    }
}
