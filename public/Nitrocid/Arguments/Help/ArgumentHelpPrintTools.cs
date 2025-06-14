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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Switches;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Arguments.Help
{
    internal static class ArgumentHelpPrintTools
    {
        internal static Dictionary<string, ArgumentInfo> GetArguments()
        {
            var ArgumentList =
                ArgumentParse.AvailableCMDLineArgs
                .OrderBy((CommandValuePair) => CommandValuePair.Key)
                .ToDictionary((CommandValuePair) => CommandValuePair.Key, (CommandValuePair) => CommandValuePair.Value);
            DebugWriter.WriteDebug(DebugLevel.I, "Arguments: {0} [{1}]", vars: [ArgumentList.Count, string.Join(", ", ArgumentList.Keys)]);
            return ArgumentList;
        }

        internal static void ShowArgumentList()
        {
            var argumentList = GetArguments();
            foreach (string arg in argumentList.Keys)
            {
                string entry = LanguageTools.GetLocalized(argumentList[arg].HelpDefinition);
                string[] usages = [.. argumentList[arg].ArgArgumentInfo.Select((cai) => cai.RenderedUsage).Where((usage) => !string.IsNullOrEmpty(usage))];
                DebugWriter.WriteDebug(DebugLevel.I, "Help entry for {0}: {1}", vars: [arg, entry]);
                TextWriters.Write("  - {0}{1}: ", false, KernelColorType.ListEntry, arg, usages.Length > 0 ? $" {string.Join(" | ", usages)}" : "");
                TextWriters.Write("{0}", true, KernelColorType.ListValue, entry);
            }
        }

        internal static void ShowArgumentListSimple()
        {
            var argumentList = GetArguments();
            DebugWriter.WriteDebug(DebugLevel.I, "Simple help is printing {0} commands...", vars: [argumentList.Count]);
            TextWriters.Write(string.Join(", ", argumentList.Keys), false, KernelColorType.ListEntry);
        }

        internal static void ShowHelpUsage(string argument)
        {
            // Check to see if we have this argument
            var argumentList = GetArguments();
            if (!argumentList.TryGetValue(argument, out ArgumentInfo? argInfo))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "We found no help! {0}", vars: [argument]);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_HELP_NOHELP"), true, KernelColorType.Error, argument);
                return;
            }

            // Now, populate usages for each argument
            string HelpDefinition = LanguageTools.GetLocalized(argInfo.HelpDefinition);
            var argumentInfos = argInfo.ArgArgumentInfo;
            foreach (var argumentInfo in argumentInfos)
            {
                var Arguments = Array.Empty<CommandArgumentPart>();
                var Switches = Array.Empty<SwitchInfo>();

                // Populate help usages
                if (argumentInfo is not null)
                {
                    Arguments = argumentInfo.Arguments;
                    Switches = argumentInfo.Switches;
                    DebugWriter.WriteDebug(DebugLevel.I, "{0} args, {1} switches", vars: [Arguments.Length, Switches.Length]);
                }
                else
                    continue;

                // Print usage information
                if (Arguments.Length != 0 || Switches.Length != 0)
                {
                    // Print the usage information holder
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_HELP_USAGE") + $" {argument}", false, KernelColorType.ListEntry);

                    // Enumerate through the available switches first
                    foreach (var Switch in Switches)
                    {
                        bool required = Switch.IsRequired;
                        string switchName = Switch.SwitchName;
                        string renderedSwitch = required ? $" <-{switchName}[=value]>" : $" [-{switchName}[=value]]";
                        DebugWriter.WriteDebug(DebugLevel.I, "Rendered switch: {0}", vars: [renderedSwitch]);
                        TextWriters.Write(renderedSwitch, false, KernelColorType.ListEntry);
                    }

                    // Enumerate through the available arguments
                    int howManyRequired = argumentInfo.MinimumArguments;
                    int queriedArgs = 1;
                    foreach (var argumentPart in Arguments)
                    {
                        bool required = argumentInfo.ArgumentsRequired && queriedArgs <= howManyRequired;
                        string renderedArgument = required ? $" <{argumentPart.ArgumentExpression}>" : $" [{argumentPart.ArgumentExpression}]";
                        DebugWriter.WriteDebug(DebugLevel.I, "Rendered argument: {0}", vars: [renderedArgument]);
                        TextWriters.Write(renderedArgument, false, KernelColorType.ListEntry);
                    }
                    TextWriterRaw.Write();
                }
                else
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_HELP_USAGE") + $" {argument}", true, KernelColorType.ListEntry);
            }

            // Write the description now
            DebugWriter.WriteDebug(DebugLevel.I, "Definition: {0}", vars: [HelpDefinition]);
            if (string.IsNullOrEmpty(HelpDefinition))
                HelpDefinition = LanguageTools.GetLocalized("NKS_ARGUMENTS_HELP_NOHELPDESC");
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_BASE_HELP_DESCRIPTION") + $" {HelpDefinition}", true, KernelColorType.ListValue);
            argInfo.ArgumentBase.HelpHelper();
        }
    }
}
