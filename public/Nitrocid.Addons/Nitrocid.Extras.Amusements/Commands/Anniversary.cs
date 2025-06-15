//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.ConsoleBase.Colors;
using Terminaux.Inputs.Presentation;
using Terminaux.Inputs.Presentation.Elements;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Terminaux.Colors;
using Terminaux.Colors.Data;

namespace Nitrocid.Extras.Amusements.Commands
{
    class AnniversaryCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var annivPres = new Slideshow(
                LanguageTools.GetLocalized("NKS_AMUSEMENTS_2018_ANNIVERSARY_TITLE"),
                [
                    new PresentationPage(
                        LanguageTools.GetLocalized("NKS_AMUSEMENTS_2018_ANNIVERSARY_PAGE1_TITLE"),
                        [
                            new TextElement()
                            {
                                Arguments =
                                [
                                    LanguageTools.GetLocalized("NKS_AMUSEMENTS_2018_ANNIVERSARY_PAGE1_LINE1") + " " +
                                    LanguageTools.GetLocalized("NKS_AMUSEMENTS_2018_ANNIVERSARY_PAGE1_LINE2") + " " +
                                    LanguageTools.GetLocalized("NKS_AMUSEMENTS_2018_ANNIVERSARY_PAGE1_LINE3")
                                ]
                            }
                        ]
                    ),
                    new PresentationPage(
                        LanguageTools.GetLocalized("NKS_AMUSEMENTS_2018_ANNIVERSARY_PAGE2_TITLE"),
                        [
                            new TextElement()
                            {
                                Arguments =
                                [
                                    LanguageTools.GetLocalized("NKS_AMUSEMENTS_2018_ANNIVERSARY_PAGE2_LINE1") + " " +
                                    LanguageTools.GetLocalized("NKS_AMUSEMENTS_2018_ANNIVERSARY_PAGE2_LINE2")
                                ]
                            }
                        ]
                    ),
                    new PresentationPage(
                        LanguageTools.GetLocalized("NKS_AMUSEMENTS_2018_ANNIVERSARY_PAGE3_TITLE"),
                        [
                            new TextElement()
                            {
                                Arguments =
                                [
                                    LanguageTools.GetLocalized("NKS_AMUSEMENTS_2018_ANNIVERSARY_PAGE3_LINE1") + " " +
                                    LanguageTools.GetLocalized("NKS_AMUSEMENTS_2018_ANNIVERSARY_PAGE3_LINE2") + " \n\n" +
                                    new Color(ConsoleColors.Green3Alt).VTSequenceForeground + "Nitrocid KS 0.1.0!\n\n" + KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground +
                                    "< " + LanguageTools.GetLocalized("NKS_AMUSEMENTS_2018_ANNIVERSARY_PAGE3_LINE3") + " >\n\n" +
                                    "-- Aptivi"
                                ]
                            }
                        ]
                    )
                ]
            );

            PresentationTools.Present(annivPres, true, false);
            return 0;
        }

    }
}
