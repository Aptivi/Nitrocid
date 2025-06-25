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

using Nitrocid.Base.Arguments.CommandLineArguments;
using System.Collections.Generic;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Arguments.Base;

namespace Nitrocid.Base.Arguments
{
    /// <summary>
    /// Argument parser class
    /// </summary>
    public static class KernelArguments
    {

        internal readonly static Dictionary<string, ArgumentInfo> outArgs = new()
        {
            { "help",
                new ArgumentInfo("help", /* Localizable */ "NKS_SHELL_SHELLS_COMMAND_HELP_DESC",
                    [
                        new CommandArgumentInfo()
                    ], new HelpArgument())
            },

            { "version",
                new ArgumentInfo("version", /* Localizable */ "NKS_ARGUMENTS_ARGUMENT_VERSION_DESC",
                    [
                        new CommandArgumentInfo()
                    ], new VersionArgument())
            },

            { "apiversion",
                new ArgumentInfo("apiversion", /* Localizable */ "NKS_ARGUMENTS_ARGUMENT_APIVERSION_DESC",
                    [
                        new CommandArgumentInfo()
                    ], new ApiVersionArgument())
            },

            { "lang",
                new ArgumentInfo("lang", /* Localizable */ "NKS_ARGUMENTS_ARGUMENT_LANG_DESC",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "lang")
                        ])
                    ], new LangArgument())
            },
        };

        private readonly static Dictionary<string, ArgumentInfo> args = new()
        {
            { "quiet",
                new ArgumentInfo("quiet", /* Localizable */ "NKS_ARGUMENTS_ARGUMENT_QUIET_DESC",
                    [
                        new CommandArgumentInfo()
                    ], new QuietArgument())
            },

            { "maintenance",
                new ArgumentInfo("maintenance", /* Localizable */ "NKS_ARGUMENTS_ARGUMENT_MAINTENANCE_DESC",
                    [
                        new CommandArgumentInfo()
                    ], new MaintenanceArgument())
            },

            { "safe",
                new ArgumentInfo("safe", /* Localizable */ "NKS_ARGUMENTS_ARGUMENT_SAFE_DESC",
                    [
                        new CommandArgumentInfo()
                    ], new SafeArgument())
            },

            { "testInteractive",
                new ArgumentInfo("testInteractive", /* Localizable */ "NKS_ARGUMENTS_ARGUMENT_TESTINTERACTIVE_DESC",
                    [
                        new CommandArgumentInfo()
                    ], new TestInteractiveArgument())
            },

            { "debug",
                new ArgumentInfo("debug", /* Localizable */ "NKS_ARGUMENTS_ARGUMENT_DEBUG_DESC",
                    [
                        new CommandArgumentInfo()
                    ], new DebugArgument())
            },

            { "terminaldebug",
                new ArgumentInfo("terminaldebug", /* Localizable */ "NKS_ARGUMENTS_ARGUMENT_TERMINALDEBUG_DESC",
                    [
                        new CommandArgumentInfo()
                    ], new TerminalDebugArgument())
            },

            { "reset",
                new ArgumentInfo("reset", /* Localizable */ "NKS_ARGUMENTS_ARGUMENT_RESET_DESC",
                    [
                        new CommandArgumentInfo()
                    ], new ResetArgument())
            },

            { "noaltbuffer",
                new ArgumentInfo("noaltbuffer", /* Localizable */ "NKS_ARGUMENTS_ARGUMENT_NOALTBUFFER_DESC",
                    [
                        new CommandArgumentInfo()
                    ], new NoAltBufferArgument())
            },

            { "attach",
                new ArgumentInfo("attach", /* Localizable */ "NKS_ARGUMENTS_ARGUMENT_ATTACH_DESC",
                    [
                        new CommandArgumentInfo()
                    ], new AttachArgument())
            },

            { "verbosepreboot",
                new ArgumentInfo("verbosepreboot", /* Localizable */ "NKS_ARGUMENTS_ARGUMENT_VERBOSEPREBOOT_DESC",
                    [
                        new CommandArgumentInfo()
                    ], new VerbosePrebootArgument())
            },

            { "noprebootsplash",
                new ArgumentInfo("noprebootsplash", /* Localizable */ "NKS_ARGUMENTS_ARGUMENT_NOPREBOOTSPLASH_DESC",
                    [
                        new CommandArgumentInfo()
                    ], new NoPrebootSplashArgument())
            },
        };

        /// <summary>
        /// Available command line arguments
        /// </summary>
        public static Dictionary<string, ArgumentInfo> AvailableCMDLineArgs =>
            args;
    }
}
