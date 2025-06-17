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

using Terminaux.Shell.Arguments.Base;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using System.Diagnostics;

namespace Nitrocid.Arguments.CommandLineArguments
{
    class AttachArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(ArgumentParameters parameters)
        {
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_DEBUG_WAITING"));
            if (!Debugger.Launch())
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_DEBUG_FAILED"));
        }

        public override void HelpHelper() =>
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_DEBUG_HELPER"));

    }
}
