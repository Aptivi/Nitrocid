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
using System.Text;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Extras.Amusements.Screensavers.Utilities;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.Data.Figlet;
using Textify.Data.NameGen;
using Textify.General;

namespace Nitrocid.Extras.Amusements.Screensavers
{
    /// <summary>
    /// Display code for BdayCard
    /// </summary>
    public class BdayCardDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_SETTINGS_DESC -> Shows a “birthday card” on the screen
        private string randomName = "";

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "BdayCard";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            switch (AmusementsInit.SaversConfig.BdayCardNameType)
            {
                case BdayCardNameType.Random:
                    randomName = NameGenerator.GenerateFirstNames(1)[0];
                    break;
            }
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Change color
            var bannerColor = ChangeBdayCardColor();
            var builder = new StringBuilder();

            // Write an infobox border
            int height = ConsoleWrapper.WindowHeight - 4;
            int width = ConsoleWrapper.WindowWidth - 8;
            int posX = ConsoleWrapper.WindowWidth / 2 - width / 2 - 1;
            int posY = ConsoleWrapper.WindowHeight / 2 - height / 2 - 1;
            var border = new BoxFrame()
            {
                Left = posX,
                Top = posY,
                Width = width,
                Height = height,
                UseColors = true,
                FrameColor = bannerColor,
            };
            builder.Append(border.Render());

            // Select an appropriate string based on the selection
            string birthdayWish = GetBirthdayWishMessage();

            // Write banner and wish
            // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_BDAY -> Happy birthday!
            string word = LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_BDAY");

            // TODO: Remove this line once localization is ready
            word = "Happy birthday!";
            // TODO: Remove this line once localization is ready

            var figFont = FigletTools.GetFigletFont("script");
            int figHeight = FigletTools.GetFigletHeight(word, figFont) / 2;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            int hashY = ConsoleWrapper.WindowHeight / 2 + figHeight + 2;
            ConsoleWrapper.Clear();
            var wordText = new AlignedFigletText(figFont)
            {
                Top = consoleY,
                Text = word,
                ForegroundColor = bannerColor,
                Settings = new()
                {
                    Alignment = TextAlignment.Middle,
                }
            };
            builder.Append(wordText.Render());
            builder.Append(TextWriterWhereColor.RenderWhereColor(birthdayWish, (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - ConsoleChar.EstimateCellWidth(birthdayWish) / 2d), hashY, ConsoleColors.White));
            TextWriterRaw.WriteRaw(builder.ToString());

            // Delay
            ScreensaverManager.Delay(AmusementsInit.SaversConfig.BdayCardDelay);
        }

        private string GetBirthdayWishMessage()
        {
            switch (AmusementsInit.SaversConfig.BdayCardTextType)
            {
                case BdayCardTextType.Simple:
                    switch (AmusementsInit.SaversConfig.BdayCardNameType)
                    {
                        case BdayCardNameType.Random:
                            // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_NAMED_SIMPLE_BDAYWISH -> Happy birthday, {0}!
                            return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_NAMED_SIMPLE_BDAYWISH").FormatString(randomName);
                        case BdayCardNameType.User:
                            // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_NAMED_SIMPLE_BDAYWISH -> Happy birthday, {0}!
                            return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_NAMED_SIMPLE_BDAYWISH").FormatString(AmusementsInit.SaversConfig.BdayCardPersonName);
                        case BdayCardNameType.Implicit:
                            switch (AmusementsInit.SaversConfig.BdayCardGender)
                            {
                                case BdayCardGender.Male:
                                    // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SIMPLE_MALE_BDAYWISH -> Happy birthday to him!
                                    return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SIMPLE_MALE_BDAYWISH");
                                case BdayCardGender.Female:
                                    // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SIMPLE_FEMALE_BDAYWISH -> Happy birthday to her!
                                    return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SIMPLE_FEMALE_BDAYWISH");
                                case BdayCardGender.Unspecific:
                                    // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SIMPLE_UNSPECIFIC_BDAYWISH -> Happy birthday to the most loyal one!
                                    return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SIMPLE_UNSPECIFIC_BDAYWISH");
                            }
                            break;
                        case BdayCardNameType.ImplicitFirstPerson:
                            // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SIMPLE_FIRSTPERSON_BDAYWISH -> Happy birthday to me!
                            return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SIMPLE_FIRSTPERSON_BDAYWISH");
                        case BdayCardNameType.ImplicitSecondPerson:
                            // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SIMPLE_SECONDPERSON_BDAYWISH -> Happy birthday to you!
                            return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SIMPLE_SECONDPERSON_BDAYWISH");
                    }
                    break;
                case BdayCardTextType.Superlative:
                    switch (AmusementsInit.SaversConfig.BdayCardNameType)
                    {
                        case BdayCardNameType.Random:
                            // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_NAMED_SUPER_BDAYWISH -> Happiest birthday, {0}!
                            return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_NAMED_SUPER_BDAYWISH").FormatString(randomName);
                        case BdayCardNameType.User:
                            // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_NAMED_SUPER_BDAYWISH -> Happiest birthday, {0}!
                            return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_NAMED_SUPER_BDAYWISH").FormatString(AmusementsInit.SaversConfig.BdayCardPersonName);
                        case BdayCardNameType.Implicit:
                            switch (AmusementsInit.SaversConfig.BdayCardGender)
                            {
                                case BdayCardGender.Male:
                                    // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPER_MALE_BDAYWISH -> Happiest birthday to him!
                                    return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPER_MALE_BDAYWISH");
                                case BdayCardGender.Female:
                                    // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPER_FEMALE_BDAYWISH -> Happiest birthday to her!
                                    return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPER_FEMALE_BDAYWISH");
                                case BdayCardGender.Unspecific:
                                    // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPER_UNSPECIFIC_BDAYWISH -> Happiest birthday to the most loyal one!
                                    return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPER_UNSPECIFIC_BDAYWISH");
                            }
                            break;
                        case BdayCardNameType.ImplicitFirstPerson:
                            // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPER_FIRSTPERSON_BDAYWISH -> Happiest birthday to me!
                            return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPER_FIRSTPERSON_BDAYWISH");
                        case BdayCardNameType.ImplicitSecondPerson:
                            // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPER_SECONDPERSON_BDAYWISH -> Happiest birthday to you!
                            return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPER_SECONDPERSON_BDAYWISH");
                    }
                    break;
                case BdayCardTextType.SuperAlt:
                    switch (AmusementsInit.SaversConfig.BdayCardNameType)
                    {
                        case BdayCardNameType.Random:
                            // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_NAMED_SUPERALT_BDAYWISH -> Happiest one, {0}!
                            return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_NAMED_SUPERALT_BDAYWISH").FormatString(randomName);
                        case BdayCardNameType.User:
                            // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_NAMED_SUPERALT_BDAYWISH -> Happiest one, {0}!
                            return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_NAMED_SUPERALT_BDAYWISH").FormatString(AmusementsInit.SaversConfig.BdayCardPersonName);
                        case BdayCardNameType.Implicit:
                            switch (AmusementsInit.SaversConfig.BdayCardGender)
                            {
                                case BdayCardGender.Male:
                                    // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPERALT_MALE_BDAYWISH -> Happiest one to him!
                                    return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPERALT_MALE_BDAYWISH");
                                case BdayCardGender.Female:
                                    // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPERALT_FEMALE_BDAYWISH -> Happiest one to her!
                                    return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPERALT_FEMALE_BDAYWISH");
                                case BdayCardGender.Unspecific:
                                    // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPERALT_UNSPECIFIC_BDAYWISH -> Happiest one to the most loyal one!
                                    return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPERALT_UNSPECIFIC_BDAYWISH");
                            }
                            break;
                        case BdayCardNameType.ImplicitFirstPerson:
                            // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPERALT_FIRSTPERSON_BDAYWISH -> Happiest one to me!
                            return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPERALT_FIRSTPERSON_BDAYWISH");
                        case BdayCardNameType.ImplicitSecondPerson:
                            // TODO: NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPERALT_SECONDPERSON_BDAYWISH -> Happiest one to you!
                            return LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_BDAYCARD_UNNAMED_SUPERALT_SECONDPERSON_BDAYWISH");
                    }
                    break;
            }
            return "";
        }

        /// <summary>
        /// Changes the color of word and its hash
        /// </summary>
        private Color ChangeBdayCardColor()
        {
            Color ColorInstance;
            if (AmusementsInit.SaversConfig.BdayCardTrueColor)
            {
                int RedColorNum = RandomDriver.Random(AmusementsInit.SaversConfig.BdayCardMinimumRedColorLevel, AmusementsInit.SaversConfig.BdayCardMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(AmusementsInit.SaversConfig.BdayCardMinimumGreenColorLevel, AmusementsInit.SaversConfig.BdayCardMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(AmusementsInit.SaversConfig.BdayCardMinimumBlueColorLevel, AmusementsInit.SaversConfig.BdayCardMaximumBlueColorLevel);
                ColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(AmusementsInit.SaversConfig.BdayCardMinimumColorLevel, AmusementsInit.SaversConfig.BdayCardMaximumColorLevel);
                ColorInstance = new Color(ColorNum);
            }
            return ColorInstance;
        }

    }
}
