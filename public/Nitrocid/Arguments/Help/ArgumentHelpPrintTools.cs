//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Switches;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.Arguments.Help
{
    internal static class ArgumentHelpPrintTools
    {
        internal static Dictionary<string, ArgumentInfo> GetArguments()
        {
            var ArgumentList =
                ArgumentParse.AvailableCMDLineArgs
                .OrderBy((CommandValuePair) => CommandValuePair.Key)
                .ToDictionary((CommandValuePair) => CommandValuePair.Key, (CommandValuePair) => CommandValuePair.Value);
            DebugWriter.WriteDebug(DebugLevel.I, "Arguments: {0} [{1}]", ArgumentList.Count, string.Join(", ", ArgumentList.Keys));
            return ArgumentList;
        }

        internal static void ShowArgumentList()
        {
            var argumentList = GetArguments();
            foreach (string arg in argumentList.Keys)
            {
                string entry = argumentList[arg].GetTranslatedHelpEntry();
                DebugWriter.WriteDebug(DebugLevel.I, "Help entry for {0}: {1}", arg, entry);
                TextWriterColor.WriteKernelColor("- {0}: ", false, KernelColorType.ListEntry, arg);
                TextWriterColor.WriteKernelColor("{0}", true, KernelColorType.ListValue, entry);
            }
        }

        internal static void ShowArgumentListSimple()
        {
            var argumentList = GetArguments();
            DebugWriter.WriteDebug(DebugLevel.I, "Simple help is printing {0} commands...", argumentList.Count);
            TextWriterColor.WriteKernelColor(string.Join(", ", argumentList.Keys), false, KernelColorType.ListEntry);
        }

        internal static void ShowHelpUsage(string argument)
        {
            // Check to see if we have this argument
            var argumentList = GetArguments();
            if (!argumentList.ContainsKey(argument))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "We found no help! {0}", argument);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No help for argument \"{0}\"."), true, KernelColorType.Error, argument);
                return;
            }

            // Now, populate usages for each argument
            string HelpDefinition = argumentList[argument].GetTranslatedHelpEntry();
            var argumentInfos = argumentList[argument].ArgArgumentInfo;
            foreach (var argumentInfo in argumentInfos)
            {
                var Arguments = Array.Empty<CommandArgumentPart>();
                var Switches = Array.Empty<SwitchInfo>();

                // Populate help usages
                if (argumentInfo is not null)
                {
                    Arguments = argumentInfo.Arguments;
                    Switches = argumentInfo.Switches;
                    DebugWriter.WriteDebug(DebugLevel.I, "{0} args, {1} switches", Arguments.Length, Switches.Length);
                }

                // Print usage information
                if (Arguments.Length != 0 || Switches.Length != 0)
                {
                    // Print the usage information holder
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Usage:") + $" {argument}", false, KernelColorType.ListEntry);

                    // Enumerate through the available switches first
                    foreach (var Switch in Switches)
                    {
                        bool required = Switch.IsRequired;
                        string switchName = Switch.SwitchName;
                        string renderedSwitch = required ? $" <-{switchName}[=value]>" : $" [-{switchName}[=value]]";
                        DebugWriter.WriteDebug(DebugLevel.I, "Rendered switch: {0}", renderedSwitch);
                        TextWriterColor.WriteKernelColor(renderedSwitch, false, KernelColorType.ListEntry);
                    }

                    // Enumerate through the available arguments
                    int howManyRequired = argumentInfo.MinimumArguments;
                    int queriedArgs = 1;
                    foreach (var argumentPart in Arguments)
                    {
                        bool required = argumentInfo.ArgumentsRequired && queriedArgs <= howManyRequired;
                        string renderedArgument = required ? $" <{argumentPart.ArgumentExpression}>" : $" [{argumentPart.ArgumentExpression}]";
                        DebugWriter.WriteDebug(DebugLevel.I, "Rendered argument: {0}", renderedArgument);
                        TextWriterColor.WriteKernelColor(renderedArgument, false, KernelColorType.ListEntry);
                    }
                    TextWriterColor.Write();
                }
                else
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Usage:") + $" {argument}", true, KernelColorType.ListEntry);
            }

            // Write the description now
            DebugWriter.WriteDebug(DebugLevel.I, "Definition: {0}", HelpDefinition);
            if (string.IsNullOrEmpty(HelpDefinition))
                HelpDefinition = Translate.DoTranslation("No argument help description");
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Description:") + $" {HelpDefinition}", true, KernelColorType.ListValue);

            // Extra help action for some arguments
            argumentList[argument].ArgumentBase.HelpHelper();
        }
    }
}
