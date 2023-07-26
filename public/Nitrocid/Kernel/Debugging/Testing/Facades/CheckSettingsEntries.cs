
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Configuration;
using KS.Languages;
using System.Collections.Generic;
using System.Linq;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class CheckSettingsEntries : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Checks all the KS settings to see if the variables are written correctly");
        public override bool TestInteractive => false;
        public override object TestExpectedValue => false;
        public override void Run()
        {
            var Results = ConfigTools.CheckConfigVariables();
            var NotFound = new List<string>();

            // Go through each and every result
            foreach (string Variable in Results.Keys)
            {
                bool IsFound = Results[Variable];
                if (!IsFound)
                {
                    NotFound.Add(Variable);
                }
            }

            // Warn if not found
            if (NotFound.Count > 0)
            {
                TextWriterColor.Write(Translate.DoTranslation("These configuration entries have invalid variables or enumerations and need to be fixed:"), true, KernelColorType.Warning);
                ListWriterColor.WriteList(NotFound);
            }

            TestActualValue = NotFound.Any();
        }
    }
}
