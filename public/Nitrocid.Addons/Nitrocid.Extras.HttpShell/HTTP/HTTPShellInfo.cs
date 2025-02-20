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

using System.Collections.Generic;
using Nitrocid.Extras.HttpShell.HTTP.Presets;
using Nitrocid.Extras.HttpShell.HTTP.Commands;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;
using Nitrocid.Shell.ShellBase.Arguments;

namespace Nitrocid.Extras.HttpShell.HTTP
{
    /// <summary>
    /// Common HTTP shell class
    /// </summary>
    internal class HTTPShellInfo : BaseShellInfo<HTTPShell>, IShellInfo
    {
        /// <summary>
        /// HTTP commands
        /// </summary>
        public override List<CommandInfo> Commands =>
        [
            new CommandInfo("addheader", /* Localizable */ "Adds a header with the key and the value to all the upcoming requests",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "key", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Header key"
                        }),
                        new CommandArgumentPart(true, "value", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Header value"
                        })
                    ])
                ], new AddHeaderCommand()),

            new CommandInfo("curragent", /* Localizable */ "Gets current user agent", new CurrAgentCommand()),

            new CommandInfo("delete", /* Localizable */ "Deletes content from HTTP server",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "HTTP request"
                        })
                    ])
                ], new DeleteCommand()),

            new CommandInfo("detach", /* Localizable */ "Exits the shell without disconnecting", new DetachCommand()),

            new CommandInfo("editheader", /* Localizable */ "Edits a key on the header to all the upcoming requests",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "key", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Header key"
                        }),
                        new CommandArgumentPart(true, "value", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Header value"
                        })
                    ])
                ], new EditHeaderCommand()),

            new CommandInfo("get", /* Localizable */ "Gets the response from the HTTP server using the specified request",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "HTTP request"
                        })
                    ])
                ], new GetCommand(), CommandFlags.Wrappable),

            new CommandInfo("getstring", /* Localizable */ "Gets the string from the HTTP server using the specified request",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "HTTP request"
                        })
                    ])
                ], new GetStringCommand(), CommandFlags.Wrappable),

            new CommandInfo("lsheader", /* Localizable */ "Lists the request headers", new LsHeaderCommand(), CommandFlags.Wrappable),

            new CommandInfo("put", /* Localizable */ "Puts the file to the HTTP server using the specified request",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "HTTP request"
                        }),
                        new CommandArgumentPart(true, "pathtofile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "File to upload"
                        })
                    ])
                ], new PutCommand(), CommandFlags.Wrappable),

            new CommandInfo("putstring", /* Localizable */ "Puts the string to the HTTP server using the specified request",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "HTTP request"
                        }),
                        new CommandArgumentPart(true, "string", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "String to upload"
                        })
                    ])
                ], new PutStringCommand(), CommandFlags.Wrappable),

            new CommandInfo("post", /* Localizable */ "Posts the file to the HTTP server using the specified request",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "HTTP request"
                        }),
                        new CommandArgumentPart(true, "pathtofile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "File to upload"
                        })
                    ])
                ], new PostCommand(), CommandFlags.Wrappable),

            new CommandInfo("poststring", /* Localizable */ "Posts the string to the HTTP server using the specified request",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "HTTP request"
                        }),
                        new CommandArgumentPart(true, "string", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "String to upload"
                        })
                    ])
                ], new PostStringCommand(), CommandFlags.Wrappable),

            new CommandInfo("rmheader", /* Localizable */ "Removes a key on the header to all the upcoming requests",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "key", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Header key"
                        })
                    ])
                ], new RmHeaderCommand()),

            new CommandInfo("setagent", /* Localizable */ "Sets a user agent",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "userAgent", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "User agent string"
                        })
                    ])
                ], new SetAgentCommand()),
        ];

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

        public override bool AcceptsNetworkConnection => true;

        public override string NetworkConnectionType => "HTTP";
    }
}
