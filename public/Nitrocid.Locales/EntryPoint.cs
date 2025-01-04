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

using Nitrocid.Locales.Actions.Arguments;
using System.Linq;
using System.Reflection;
using Terminaux.Base.Checks;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Arguments.Base;
using Terminaux.Shell.Switches;

namespace Nitrocid.Locales
{
    internal static class EntryPoint
    {
        internal static readonly ArgumentInfo[] argumentsMain =
        [
            new("check", "Checks the localizations",
                [
                    new CommandArgumentInfo()
                ], new CheckerArgument()),

            new("clean", "Cleans unused localizations",
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("dry", "Performs this operation dryly", new()
                        {
                            AcceptsValues = false,
                        }),
                    ])
                ], new CleanerArgument()),

            new("generate", "Generates localization info and metadata",
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("dry", "Performs this operation dryly", new()
                        {
                            AcceptsValues = false,
                        }),
                        new SwitchInfo("custom", "Generates localization info from custom language files", new()
                        {
                            AcceptsValues = false,
                        }),
                        new SwitchInfo("addon", "Generates localization info from language files found in the addon translations folder", new()
                        {
                            AcceptsValues = false,
                        }),
                        new SwitchInfo("normal", "Generates localization info from language files found in the base translations folder", new()
                        {
                            AcceptsValues = false,
                        }),
                        new SwitchInfo("all", "Generates localization info from all language files", new()
                        {
                            AcceptsValues = false,
                        }),
                        new SwitchInfo("resources", "Copies the generated files to the resources", new()
                        {
                            AcceptsValues = false,
                        }),
                        new SwitchInfo("singular", "Allows you to generate a single language", new()),
                    ])
                ], new GeneratorArgument()),

            new("trim", "Trims redundant languages",
                [
                    new CommandArgumentInfo([
                        new SwitchInfo("dry", "Performs this operation dryly", new()
                        {
                            AcceptsValues = false,
                        }),
                    ])
                ], new TrimmerArgument()),

            new("help", "Shows this help page",
                [
                    new CommandArgumentInfo([
                        new CommandArgumentPart(false, "argument", new()),
                    ])
                ], new HelpArgument()),
        ];
        internal static readonly Assembly thisAssembly = typeof(EntryPoint).Assembly;

        /// <summary>
        /// Entry point
        /// </summary>
        public static void Main(string[] args)
        {
            // Check console
            ConsoleChecker.CheckConsole();

            // Check the arguments
            var arguments = argumentsMain.ToDictionary((ai) => ai.Argument, (ai) => ai);
            ArgumentParse.ParseArguments([string.Join(" ", args)], arguments);
        }
    }
}
