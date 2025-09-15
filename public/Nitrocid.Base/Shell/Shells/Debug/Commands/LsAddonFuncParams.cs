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

#if NKS_EXTENSIONS
using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using System.Reflection;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Simple;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Extensions;

namespace Nitrocid.Base.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can list all the available function parameters from a function
    /// </summary>
    /// <remarks>
    /// This command lets you list all the function parameters from a function.
    /// </remarks>
    class LsAddonFuncParamsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSADDONFUNCPARAMS_LISTING") + $" {parameters.ArgumentsList[1]}, {parameters.ArgumentsList[0]} -> {parameters.ArgumentsList[2]}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));

            // List all the available addons
            var list = InterAddonTools.GetFunctionParameters(parameters.ArgumentsList[0], parameters.ArgumentsList[2], parameters.ArgumentsList[1]) ?? [];
            var listing = new Listing()
            {
                Objects = list,
                Stringifier = (para) => $"[{((ParameterInfo)para).ParameterType.FullName ?? LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSADDONFUNCPARAMS_UNKNOWNTYPE")}] {((ParameterInfo)para).Name ?? LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSADDONFUNCPARAMS_UNKNOWNPARAM")}",
                KeyColor = ThemeColorsTools.GetColor(ThemeColorType.ListEntry),
                ValueColor = ThemeColorsTools.GetColor(ThemeColorType.ListValue),
            };
            TextWriterRaw.WriteRaw(listing.Render());
            return 0;
        }

        public override int ExecuteDumb(CommandParameters parameters, ref string variableValue)
        {
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSADDONFUNCPARAMS_LISTING") + $" {parameters.ArgumentsList[1]}, {parameters.ArgumentsList[0]} -> {parameters.ArgumentsList[2]}");

            // List all the available addons
            var list = InterAddonTools.GetFunctionParameters(parameters.ArgumentsList[0], parameters.ArgumentsList[2], parameters.ArgumentsList[1]) ?? [];
            foreach (var parameter in list)
                TextWriterColor.Write($"  - [{parameter.ParameterType.FullName ?? LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSADDONFUNCPARAMS_UNKNOWNTYPE")}] {parameter.Name ?? LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_LSADDONFUNCPARAMS_UNKNOWNPARAM")}");
            return 0;
        }

    }
}
#endif
