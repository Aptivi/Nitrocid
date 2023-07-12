
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
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using KS.Shell.Shells.UESH.Commands;
using System;
using System.Linq;

namespace KS.Arguments.ArgumentBase
{
    /// <summary>
    /// Argument help system module
    /// </summary>
    public static class ArgumentHelpSystem
    {

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
            var ArgumentList = ArgumentParse.AvailableCMDLineArgs
                               .OrderBy((CommandValuePair) => CommandValuePair.Key)
                               .ToDictionary((CommandValuePair) => CommandValuePair.Key, (CommandValuePair) => CommandValuePair.Value);

            // Check to see if argument exists
            if (!string.IsNullOrWhiteSpace(Argument) & ArgumentList.ContainsKey(Argument))
            {
                string HelpDefinition = ArgumentList[Argument].GetTranslatedHelpEntry();
                var Arguments = Array.Empty<string>();
                var Switches = Array.Empty<SwitchInfo>();
                var argumentInfo = ArgumentList[Argument].ArgArgumentInfo;

                // Populate help usages
                if (argumentInfo is not null)
                {
                    Arguments = argumentInfo.Arguments;
                    Switches = argumentInfo.Switches;
                }

                // Print usage information
                if (Arguments.Length != 0 || Switches.Length != 0)
                {
                    // Print the usage information holder
                    TextWriterColor.Write(Translate.DoTranslation("Usage:") + $" {Argument}", false, KernelColorType.ListEntry);

                    // Enumerate through the available switches first
                    foreach (var Switch in Switches)
                    {
                        bool required = Switch.IsRequired;
                        string switchName = Switch.SwitchName;
                        string renderedSwitch = required ? $" <-{switchName}[=value]>" : $" [-{switchName}[=value]]";
                        TextWriterColor.Write(renderedSwitch, false, KernelColorType.ListEntry);
                    }

                    // Enumerate through the available arguments
                    int howManyRequired = argumentInfo.MinimumArguments;
                    int queriedArgs = 1;
                    foreach (string argument in Arguments)
                    {
                        bool required = argumentInfo.ArgumentsRequired && queriedArgs <= howManyRequired;
                        string renderedArgument = required ? $" <{argument}>" : $" [{argument}]";
                        TextWriterColor.Write(renderedArgument, false, KernelColorType.ListEntry);
                    }
                    TextWriterColor.Write();
                }
                else
                    TextWriterColor.Write(Translate.DoTranslation("Usage:") + $" {Argument}", true, KernelColorType.ListEntry);

                // Write the description now
                if (string.IsNullOrEmpty(HelpDefinition))
                    HelpDefinition = Translate.DoTranslation("Command defined by ") + Argument;
                TextWriterColor.Write(Translate.DoTranslation("Description:") + $" {HelpDefinition}", true, KernelColorType.ListValue);

                // Extra help action for some arguments
                ArgumentList[Argument].AdditionalHelpAction?.DynamicInvoke();
            }
            else if (string.IsNullOrWhiteSpace(Argument))
            {
                // List the available arguments
                if (!Flags.SimHelp)
                {
                    foreach (string cmd in ArgumentList.Keys)
                    {
                        TextWriterColor.Write("- {0}: ", false, KernelColorType.ListEntry, cmd);
                        TextWriterColor.Write("{0}", true, KernelColorType.ListValue, ArgumentList[cmd].GetTranslatedHelpEntry());
                    }
                }
                else
                {
                    foreach (string cmd in ArgumentList.Keys)
                        TextWriterColor.Write("{0}, ", false, KernelColorType.ListEntry, cmd);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("No help for argument \"{0}\"."), true, KernelColorType.Error, Argument);
            }
        }

    }
}
