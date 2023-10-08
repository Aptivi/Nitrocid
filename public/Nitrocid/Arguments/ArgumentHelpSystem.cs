
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
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.ShellBase.Switches;
using System;
using System.Linq;

namespace KS.Arguments
{
    /// <summary>
    /// Argument help system module
    /// </summary>
    public static class ArgumentHelpSystem
    {
        internal static bool acknowledged = false;

        /// <summary>
        /// Shows the help of an argument, or argument list if nothing is specified
        /// </summary>
        public static void ShowArgsHelp() =>
            ShowArgsHelp("");

        /// <summary>
        /// Shows the help of an argument, or argument list if nothing is specified
        /// </summary>
        /// <param name="Argument">A specified argument</param>
        public static void ShowArgsHelp(string Argument)
        {
            acknowledged = true;
            var ArgumentList = ArgumentParse.AvailableCMDLineArgs
                               .OrderBy((CommandValuePair) => CommandValuePair.Key)
                               .ToDictionary((CommandValuePair) => CommandValuePair.Key, (CommandValuePair) => CommandValuePair.Value);
            DebugWriter.WriteDebug(DebugLevel.I, "Arguments: {0} [{1}]", ArgumentList.Count, string.Join(", ", ArgumentList));

            // Check to see if argument exists
            if (!string.IsNullOrWhiteSpace(Argument) & ArgumentList.ContainsKey(Argument))
            {
                string HelpDefinition = ArgumentList[Argument].GetTranslatedHelpEntry();
                var argumentInfos = ArgumentList[Argument].ArgArgumentInfo;
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
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Usage:") + $" {Argument}", false, KernelColorType.ListEntry);

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
                        foreach (var argument in Arguments)
                        {
                            bool required = argumentInfo.ArgumentsRequired && queriedArgs <= howManyRequired;
                            string renderedArgument = required ? $" <{argument.ArgumentExpression}>" : $" [{argument.ArgumentExpression}]";
                            DebugWriter.WriteDebug(DebugLevel.I, "Rendered argument: {0}", renderedArgument);
                            TextWriterColor.WriteKernelColor(renderedArgument, false, KernelColorType.ListEntry);
                        }
                        TextWriterColor.Write();
                    }
                    else
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Usage:") + $" {Argument}", true, KernelColorType.ListEntry);
                }

                // Write the description now
                DebugWriter.WriteDebug(DebugLevel.I, "Definition: {0}", HelpDefinition);
                if (string.IsNullOrEmpty(HelpDefinition))
                    HelpDefinition = Translate.DoTranslation("Command defined by ") + Argument;
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Description:") + $" {HelpDefinition}", true, KernelColorType.ListValue);

                // Extra help action for some arguments
                ArgumentList[Argument].AdditionalHelpAction?.DynamicInvoke();
            }
            else if (string.IsNullOrWhiteSpace(Argument))
            {
                // List the available arguments
                if (!KernelFlags.SimHelp)
                {
                    foreach (string cmd in ArgumentList.Keys)
                    {
                        string entry = ArgumentList[cmd].GetTranslatedHelpEntry();
                        DebugWriter.WriteDebug(DebugLevel.I, "Help entry for {0}: {1}", cmd, entry);
                        TextWriterColor.WriteKernelColor("- {0}: ", false, KernelColorType.ListEntry, cmd);
                        TextWriterColor.WriteKernelColor("{0}", true, KernelColorType.ListValue, entry);
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Simple help is printing {0} commands...", ArgumentList.Count);
                    foreach (string cmd in ArgumentList.Keys)
                        TextWriterColor.WriteKernelColor("{0}, ", false, KernelColorType.ListEntry, cmd);
                }
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "We found no help! {0}", Argument);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("No help for argument \"{0}\"."), true, KernelColorType.Error, Argument);
            }
        }

    }
}
