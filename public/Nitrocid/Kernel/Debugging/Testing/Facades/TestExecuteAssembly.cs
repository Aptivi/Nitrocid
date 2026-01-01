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

using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Files;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using System.Reflection;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class TestExecuteAssembly : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests assembly entry point execution");
        public override void Run()
        {
            string path = "";
            if (string.IsNullOrEmpty(path))
                path = InputTools.ReadLine(Translate.DoTranslation("Write a path to assembly file:") + " ");
            path = FilesystemTools.NeutralizePath(path);
            var entryPoint = Assembly.LoadFrom(path).EntryPoint ??
                throw new KernelException(KernelExceptionType.Reflection, Translate.DoTranslation("Failed to get entry point."));
            entryPoint.Invoke("", []);
        }
    }
}
