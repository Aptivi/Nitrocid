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

using Nitrocid.Arguments.CommandLineArguments;
using System.Collections.Generic;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Arguments.Base;

namespace Nitrocid.Arguments
{
    /// <summary>
    /// Argument parser class
    /// </summary>
    public static class KernelArguments
    {

        internal readonly static Dictionary<string, ArgumentInfo> outArgs = new()
        {
            { "help",
                new ArgumentInfo("help", /* Localizable */ "Help page",
                    [
                        new CommandArgumentInfo()
                    ], new HelpArgument())
            },

            { "version",
                new ArgumentInfo("version", /* Localizable */ "Prints the kernel version",
                    [
                        new CommandArgumentInfo()
                    ], new VersionArgument())
            },

            { "apiversion",
                new ArgumentInfo("apiversion", /* Localizable */ "Prints the API version",
                    [
                        new CommandArgumentInfo()
                    ], new ApiVersionArgument())
            },
        };

        private readonly static Dictionary<string, ArgumentInfo> args = new()
        {
            { "quiet",
                new ArgumentInfo("quiet", /* Localizable */ "Starts the kernel quietly",
                    [
                        new CommandArgumentInfo()
                    ], new QuietArgument())
            },

            { "maintenance",
                new ArgumentInfo("maintenance", /* Localizable */ "Like safe mode, but also disables multi-user and some customization",
                    [
                        new CommandArgumentInfo()
                    ], new MaintenanceArgument())
            },

            { "safe",
                new ArgumentInfo("safe", /* Localizable */ "Starts the kernel in safe mode, disabling all mods",
                    [
                        new CommandArgumentInfo()
                    ], new SafeArgument())
            },

            { "testInteractive",
                new ArgumentInfo("testInteractive", /* Localizable */ "Opens a test shell",
                    [
                        new CommandArgumentInfo()
                    ], new TestInteractiveArgument())
            },

            { "debug",
                new ArgumentInfo("debug", /* Localizable */ "Enables debug mode",
                    [
                        new CommandArgumentInfo()
                    ], new DebugArgument())
            },

            { "terminaldebug",
                new ArgumentInfo("terminaldebug", /* Localizable */ "Enables terminal debug mode",
                    [
                        new CommandArgumentInfo()
                    ], new TerminalDebugArgument())
            },

            { "reset",
                new ArgumentInfo("reset", /* Localizable */ "Resets the kernel to the factory settings",
                    [
                        new CommandArgumentInfo()
                    ], new ResetArgument())
            },

            { "noaltbuffer",
                new ArgumentInfo("noaltbuffer", /* Localizable */ "Prevents the kernel from using the alternative buffer",
                    [
                        new CommandArgumentInfo()
                    ], new NoAltBufferArgument())
            },

            { "lang",
                new ArgumentInfo("lang", /* Localizable */ "Sets the initial pre-boot environment language",
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "lang")
                        ])
                    ], new LangArgument())
            },

            { "attach",
                new ArgumentInfo("attach", /* Localizable */ "Attaches the Visual Studio debugger to this instance of Nitrocid",
                    [
                        new CommandArgumentInfo()
                    ], new AttachArgument())
            },

            { "verbosepreboot",
                new ArgumentInfo("verbosepreboot", /* Localizable */ "Turns on verbose messages for pre-boot environment",
                    [
                        new CommandArgumentInfo()
                    ], new VerbosePrebootArgument())
            },

            { "noprebootsplash",
                new ArgumentInfo("noprebootsplash", /* Localizable */ "Hides the pre-boot splash before configuration is loaded",
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
