//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using System.Collections.Generic;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.ShellBase.Arguments;
using Nitrocid.Extras.HttpShell.HTTP.Presets;
using Nitrocid.Extras.HttpShell.HTTP.Commands;

namespace Nitrocid.Extras.HttpShell.HTTP
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
                new CommandInfo("addheader", /* Localizable */ "Adds a header with the key and the value to all the upcoming requests",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "key"),
                            new CommandArgumentPart(true, "value")
                        })
                    ], new AddHeaderCommand())
            },

            { "curragent",
                new CommandInfo("curragent", /* Localizable */ "Gets current user agent",
                    [
                        new CommandArgumentInfo()
                    ], new CurrAgentCommand())
            },

            { "delete",
                new CommandInfo("delete", /* Localizable */ "Deletes content from HTTP server",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "request")
                        })
                    ], new DeleteCommand())
            },

            { "detach",
                new CommandInfo("detach", /* Localizable */ "Exits the shell without disconnecting",
                    [
                        new CommandArgumentInfo()
                    ], new DetachCommand())
            },

            { "editheader",
                new CommandInfo("editheader", /* Localizable */ "Edits a key on the header to all the upcoming requests",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "key"),
                            new CommandArgumentPart(true, "value")
                        })
                    ], new EditHeaderCommand())
            },

            { "get",
                new CommandInfo("get", /* Localizable */ "Gets the response from the HTTP server using the specified request",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "request")
                        })
                    ], new GetCommand(), CommandFlags.Wrappable)
            },

            { "getstring",
                new CommandInfo("getstring", /* Localizable */ "Gets the string from the HTTP server using the specified request",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "request")
                        })
                    ], new GetStringCommand(), CommandFlags.Wrappable)
            },

            { "lsheader",
                new CommandInfo("lsheader", /* Localizable */ "Lists the request headers",
                    [
                        new CommandArgumentInfo()
                    ], new LsHeaderCommand(), CommandFlags.Wrappable)
            },

            { "put",
                new CommandInfo("put", /* Localizable */ "Puts the file to the HTTP server using the specified request",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "request"),
                            new CommandArgumentPart(true, "pathtofile")
                        })
                    ], new PutCommand(), CommandFlags.Wrappable)
            },

            { "putstring",
                new CommandInfo("putstring", /* Localizable */ "Puts the string to the HTTP server using the specified request",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "request"),
                            new CommandArgumentPart(true, "string")
                        })
                    ], new PutStringCommand(), CommandFlags.Wrappable)
            },

            { "post",
                new CommandInfo("post", /* Localizable */ "Posts the file to the HTTP server using the specified request",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "request"),
                            new CommandArgumentPart(true, "pathtofile")
                        })
                    ], new PostCommand(), CommandFlags.Wrappable)
            },

            { "poststring",
                new CommandInfo("poststring", /* Localizable */ "Posts the string to the HTTP server using the specified request",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "request"),
                            new CommandArgumentPart(true, "string")
                        })
                    ], new PostStringCommand(), CommandFlags.Wrappable)
            },

            { "rmheader",
                new CommandInfo("rmheader", /* Localizable */ "Removes a key on the header to all the upcoming requests",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "key")
                        })
                    ], new RmHeaderCommand())
            },

            { "setagent",
                new CommandInfo("setagent", /* Localizable */ "Sets a user agent",
                    [
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "userAgent")
                        })
                    ], new SetAgentCommand())
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

        public override PromptPresetBase CurrentPreset =>
            PromptPresetManager.GetAllPresetsFromShell(ShellType)[PromptPresetManager.CurrentPresets[ShellType]];

        public override bool AcceptsNetworkConnection => true;

        public override string NetworkConnectionType => "HTTP";

    }
}
