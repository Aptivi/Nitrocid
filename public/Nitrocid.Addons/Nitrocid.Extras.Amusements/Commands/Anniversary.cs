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
                Translate.DoTranslation("Commemorating the 5-year anniversary of the kernel"),
                [
                    new PresentationPage(
                        Translate.DoTranslation("First ever release"),
                        [
                            new TextElement()
                            {
                                Arguments =
                                [
                                    Translate.DoTranslation("This kernel was first released on February 22, 2018 to make a minimalistic simulator that actually simulates how the core of the kernel works.") + " " +
                                    Translate.DoTranslation("It used to only host one shell, and a few commands, and it only worked with Windows due to its usage of the Windows Management Instrumentation tool.") + " " +
                                    Translate.DoTranslation("It also didn't have its own documentation.")
                                ]
                            }
                        ]
                    ),
                    new PresentationPage(
                        Translate.DoTranslation("Kernel Refinement"),
                        [
                            new TextElement()
                            {
                                Arguments =
                                [
                                    Translate.DoTranslation("Versions of the kernel were released, such as the second major release to add coloring and basic features, and the fourth major release to add configuration.") + " " +
                                    Translate.DoTranslation("Documentation started on the sixth major release, and various nice things have been slowly added to the kernel.")
                                ]
                            }
                        ]
                    ),
                    new PresentationPage(
                        Translate.DoTranslation("Current version"),
                        [
                            new TextElement()
                            {
                                Arguments =
                                [
                                    Translate.DoTranslation("This version now refines the kernel to the point that it no longer behaves like the old versions.") + " " +
                                    Translate.DoTranslation("Because the new groundbreaking features got released, we decided to name this kernel...") + " \n\n" +
                                    new Color(ConsoleColors.Green3Alt).VTSequenceForeground + "Nitrocid KS 0.1.0!\n\n" + KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground +
                                    "< " + Translate.DoTranslation("Happy 5-year anniversary!") + " >\n\n" +
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
