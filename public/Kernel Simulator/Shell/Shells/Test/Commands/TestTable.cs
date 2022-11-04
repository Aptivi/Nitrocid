
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
using KS.Misc.Writers.FancyWriters;
using KS.Shell.ShellBase.Commands;
using KS.TimeDate;

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// Tests the table drawing
    /// </summary>
    class Test_TestTableCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var Headers = new string[] { "Ubuntu Version", "Release Date", "Support End", "ESM Support End" };
            var Rows = new string[,] { { "12.04 (Precise Pangolin)", TimeDateRenderers.Render(new DateTime(2012, 4, 26)), TimeDateRenderers.Render(new DateTime(2017, 4, 28)), TimeDateRenderers.Render(new DateTime(2019, 4, 28)) }, { "14.04 (Trusty Tahr)", TimeDateRenderers.Render(new DateTime(2014, 4, 17)), TimeDateRenderers.Render(new DateTime(2019, 4, 25)), TimeDateRenderers.Render(new DateTime(2024, 4, 25)) }, { "16.04 (Xenial Xerus)", TimeDateRenderers.Render(new DateTime(2016, 4, 21)), TimeDateRenderers.Render(new DateTime(2021, 4, 30)), TimeDateRenderers.Render(new DateTime(2026, 4, 30)) }, { "18.04 (Bionic Beaver)", TimeDateRenderers.Render(new DateTime(2018, 4, 26)), TimeDateRenderers.Render(new DateTime(2023, 4, 30)), TimeDateRenderers.Render(new DateTime(2028, 4, 30)) }, { "20.04 (Focal Fossa)", TimeDateRenderers.Render(new DateTime(2020, 4, 23)), TimeDateRenderers.Render(new DateTime(2025, 4, 25)), TimeDateRenderers.Render(new DateTime(2030, 4, 25)) }, { "22.04 (Jammy Jellyfish)", TimeDateRenderers.Render(new DateTime(2022, 4, 26)), TimeDateRenderers.Render(new DateTime(2027, 4, 25)), TimeDateRenderers.Render(new DateTime(2032, 4, 25)) } };
            int Margin = Convert.ToInt32(ListArgsOnly.Length > 0 ? ListArgsOnly[0] : 2);
            TableColor.WriteTable(Headers, Rows, Margin);
        }

    }
}