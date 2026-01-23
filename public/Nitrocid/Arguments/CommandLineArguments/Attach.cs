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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Arguments.Base;
using Nitrocid.Languages;
using System.Diagnostics;

namespace Nitrocid.Arguments.CommandLineArguments
{
    class AttachArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(ArgumentParameters parameters)
        {
            TextWriterColor.Write(Translate.DoTranslation("Kernel is waiting for the debugger..."));
            if (!Debugger.Launch())
                TextWriterColor.Write(Translate.DoTranslation("Debugger failed to attach. Starting anyways..."));
        }

        public override void HelpHelper() =>
            TextWriterColor.Write(Translate.DoTranslation("Make sure that you have an appropriate debugger set up in your system before being able to attach Nitrocid to it."));

    }
}
