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

using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Terminaux.Inputs.Interactive;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Debugging;
using System.IO;
using System;
using Nitrocid.Extras.Mods.Modifications;
using Nitrocid.Extras.Mods.Modifications.ManPages;
using Nitrocid.Extras.Mods.Modifications.Interactive;

namespace Nitrocid.Extras.Mods.Commands
{
    /// <summary>
    /// Opens the mod manual
    /// </summary>
    /// <remarks>
    /// If the mod has a manual page which you can refer to, you can use them by this command.
    /// <br></br>
    /// </remarks>
    class ModManualCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string modName = parameters.ArgumentsList[0];
            if (!ModManager.Mods.TryGetValue(modName, out ModInfo? mod))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_MODS_MANPAGE_EXCEPTION_MODNOTFOUND"), true, ThemeColorType.Error, modName);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.NoSuchMod);
            }

            // Check for accompanying manual pages for mods
            string ModManualPath = FilesystemTools.NeutralizePath(mod.ModFilePath + ".manual");
            if (FilesystemTools.FolderExists(ModManualPath))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Found manual page collection in {0}", vars: [ModManualPath]);
                foreach (string ModManualFile in Directory.GetFiles(ModManualPath, "*.man", SearchOption.AllDirectories))
                    PageParser.InitMan(modName, ModManualFile);
            }

            var tuiInstance = new ManualViewerCli()
            {
                modName = modName,
            };
            tuiInstance.Bindings.Add(new InteractiveTuiBinding<Manual>("Info", ConsoleKey.F1, (manual, _, _, _) => tuiInstance.ShowManualInfo(manual)));
            InteractiveTuiTools.OpenInteractiveTui(tuiInstance);
            return 0;
        }

    }
}
