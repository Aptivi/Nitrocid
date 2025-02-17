﻿//
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
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Languages;
using System.Collections.Generic;
using Terminaux.Writer.CyclicWriters;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class CheckSettingsEntries : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Checks all the KS settings to see if the variables are written correctly");
        public override TestSection TestSection => TestSection.Kernel;
        public override bool TestInteractive => false;
        public override object TestExpectedValue => false;
        public override void Run(params string[] args)
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
                TextWriters.Write(Translate.DoTranslation("These configuration entries have invalid variables or enumerations and need to be fixed:"), true, KernelColorType.Warning);
                var invalidSettings = new Listing()
                {
                    Objects = NotFound,
                };
                TextWriterRaw.WriteRaw(invalidSettings.Render());
            }

            TestActualValue = NotFound.Count != 0;
        }
    }
}
