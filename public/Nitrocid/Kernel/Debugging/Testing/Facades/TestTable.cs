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

using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using System;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class TestTable : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the table functionality");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override void Run(params string[] args)
        {
            var Rows = new string[,]
            {
                { "Ubuntu Version", "Release Date", "Support End", "ESM Support End" },
                { "12.04 (Precise Pangolin)", TimeDateRenderers.Render(new DateTime(2012, 4, 26)), TimeDateRenderers.Render(new DateTime(2017, 4, 28)), TimeDateRenderers.Render(new DateTime(2019, 4, 28)) },
                { "14.04 (Trusty Tahr)", TimeDateRenderers.Render(new DateTime(2014, 4, 17)), TimeDateRenderers.Render(new DateTime(2019, 4, 25)), TimeDateRenderers.Render(new DateTime(2024, 4, 25)) },
                { "16.04 (Xenial Xerus)", TimeDateRenderers.Render(new DateTime(2016, 4, 21)), TimeDateRenderers.Render(new DateTime(2021, 4, 30)), TimeDateRenderers.Render(new DateTime(2026, 4, 30)) },
                { "18.04 (Bionic Beaver)", TimeDateRenderers.Render(new DateTime(2018, 4, 26)), TimeDateRenderers.Render(new DateTime(2023, 4, 30)), TimeDateRenderers.Render(new DateTime(2028, 4, 30)) },
                { "20.04 (Focal Fossa)", TimeDateRenderers.Render(new DateTime(2020, 4, 23)), TimeDateRenderers.Render(new DateTime(2025, 4, 25)), TimeDateRenderers.Render(new DateTime(2030, 4, 25)) },
                { "22.04 (Jammy Jellyfish)", TimeDateRenderers.Render(new DateTime(2022, 4, 21)), TimeDateRenderers.Render(new DateTime(2027, 4, 21)), TimeDateRenderers.Render(new DateTime(2032, 4, 21)) },
                { "24.04 (Noble Numbat)", TimeDateRenderers.Render(new DateTime(2024, 4, 18)), TimeDateRenderers.Render(new DateTime(2029, 4, 18)), TimeDateRenderers.Render(new DateTime(2034, 4, 18)) },
            };
            var table = new Table()
            {
                Rows = Rows,
                Left = 4,
                Top = 2,
                InteriorWidth = ConsoleWrapper.WindowWidth - 7,
                InteriorHeight = ConsoleWrapper.WindowHeight - 5,
                Header = true,
                Settings =
                [
                    new CellOptions(2, 2)
                    {
                        CellColor = ConsoleColors.Red,
                        CellBackgroundColor = ConsoleColors.DarkRed,
                        ColoredCell = true
                    }
                ],
            };
            TextWriterRaw.WriteRaw(table.Render());
        }
    }
}
