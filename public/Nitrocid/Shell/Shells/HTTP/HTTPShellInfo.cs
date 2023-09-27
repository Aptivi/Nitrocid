
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

using System.Collections.Generic;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.HTTP.Commands;
using System;
using KS.Shell.ShellBase.Switches;
using KS.Shell.ShellBase.Arguments;
using KS.Shell.Shells.HTTP.Presets;

namespace KS.Shell.Shells.HTTP
{
    /// <summary>
    /// Common HTTP shell class
    /// </summary>
    internal class HTTPShellInfo : BaseShellInfo, IShellInfo
    {

        /// <summary>
        /// HTTP commands
        /// </summary>
        public override Dictionary<string, CommandInfo> Commands => new()
        {
            { "addheader",
                new CommandInfo("addheader", ShellType, /* Localizable */ "Adds a header with the key and the value to all the upcoming requests",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "key"),
                            new CommandArgumentPart(true, "value")
                        }, Array.Empty<SwitchInfo>())
                    }, new HTTP_AddHeaderCommand())
            },

            { "curragent",
                new CommandInfo("curragent", ShellType, /* Localizable */ "Gets current user agent",
                    new[] {
                        new CommandArgumentInfo()
                    }, new HTTP_CurrAgentCommand())
            },

            { "delete",
                new CommandInfo("delete", ShellType, /* Localizable */ "Deletes content from HTTP server",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "request")
                        }, Array.Empty<SwitchInfo>())
                    }, new HTTP_DeleteCommand())
            },

            { "detach",
                new CommandInfo("detach", ShellType, /* Localizable */ "Exits the shell without disconnecting",
                    new[] {
                        new CommandArgumentInfo()
                    }, new HTTP_DetachCommand())
            },

            { "editheader",
                new CommandInfo("editheader", ShellType, /* Localizable */ "Edits a key on the header to all the upcoming requests",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "key"),
                            new CommandArgumentPart(true, "value")
                        }, Array.Empty<SwitchInfo>())
                    }, new HTTP_EditHeaderCommand())
            },

            { "get",
                new CommandInfo("get", ShellType, /* Localizable */ "Gets the response from the HTTP server using the specified request",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "request")
                        }, Array.Empty<SwitchInfo>())
                    }, new HTTP_GetCommand(), CommandFlags.Wrappable)
            },

            { "getstring",
                new CommandInfo("getstring", ShellType, /* Localizable */ "Gets the string from the HTTP server using the specified request",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "request")
                        }, Array.Empty<SwitchInfo>())
                    }, new HTTP_GetStringCommand(), CommandFlags.Wrappable)
            },

            { "lsheader",
                new CommandInfo("lsheader", ShellType, /* Localizable */ "Lists the request headers",
                    new[] {
                        new CommandArgumentInfo()
                    }, new HTTP_LsHeaderCommand(), CommandFlags.Wrappable)
            },

            { "put",
                new CommandInfo("put", ShellType, /* Localizable */ "Puts the file to the HTTP server using the specified request",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "request"),
                            new CommandArgumentPart(true, "pathtofile")
                        }, Array.Empty<SwitchInfo>())
                    }, new HTTP_PutCommand(), CommandFlags.Wrappable)
            },

            { "putstring",
                new CommandInfo("putstring", ShellType, /* Localizable */ "Puts the string to the HTTP server using the specified request",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "request"),
                            new CommandArgumentPart(true, "string")
                        }, Array.Empty<SwitchInfo>())
                    }, new HTTP_PutStringCommand(), CommandFlags.Wrappable)
            },

            { "post",
                new CommandInfo("post", ShellType, /* Localizable */ "Posts the file to the HTTP server using the specified request",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "request"),
                            new CommandArgumentPart(true, "pathtofile")
                        }, Array.Empty<SwitchInfo>())
                    }, new HTTP_PostCommand(), CommandFlags.Wrappable)
            },

            { "poststring",
                new CommandInfo("poststring", ShellType, /* Localizable */ "Posts the string to the HTTP server using the specified request",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "request"),
                            new CommandArgumentPart(true, "string")
                        }, Array.Empty<SwitchInfo>())
                    }, new HTTP_PostStringCommand(), CommandFlags.Wrappable)
            },

            { "rmheader",
                new CommandInfo("rmheader", ShellType, /* Localizable */ "Removes a key on the header to all the upcoming requests",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "key")
                        }, Array.Empty<SwitchInfo>())
                    }, new HTTP_RmHeaderCommand())
            },

            { "setagent",
                new CommandInfo("setagent", ShellType, /* Localizable */ "Sets a user agent",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "userAgent")
                        }, Array.Empty<SwitchInfo>())
                    }, new HTTP_SetAgentCommand())
            },
        };

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new HTTPDefaultPreset() },
            { "PowerLine1", new HTTPPowerLine1Preset() },
            { "PowerLine2", new HTTPPowerLine2Preset() },
            { "PowerLine3", new HTTPPowerLine3Preset() },
            { "PowerLineBG1", new HTTPPowerLineBG1Preset() },
            { "PowerLineBG2", new HTTPPowerLineBG2Preset() },
            { "PowerLineBG3", new HTTPPowerLineBG3Preset() }
        };

        public override BaseShell ShellBase => new HTTPShell();

        public override PromptPresetBase CurrentPreset => PromptPresetManager.CurrentPresets["HTTPShell"];

        public override bool AcceptsNetworkConnection => true;

        public override string NetworkConnectionType => Network.Base.Connections.NetworkConnectionType.HTTP.ToString();

    }
}
