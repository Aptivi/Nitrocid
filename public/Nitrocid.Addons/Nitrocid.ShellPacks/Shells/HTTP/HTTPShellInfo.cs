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
using Nitrocid.ShellPacks.Shells.HTTP.Presets;
using Nitrocid.ShellPacks.Shells.HTTP.Commands;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.Prompts;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Languages;

namespace Nitrocid.ShellPacks.Shells.HTTP
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
            new CommandInfo("addheader", "NKS_SHELLPACKS_HTTP_COMMAND_ADDHEADER_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "key", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_KEY_DESC"
                        }),
                        new CommandArgumentPart(true, "value", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_VALUE_DESC"
                        })
                    ])
                ], new AddHeaderCommand()),

            new CommandInfo("curragent", "NKS_SHELLPACKS_HTTP_COMMAND_CURRAGENT_DESC", new CurrAgentCommand()),

            new CommandInfo("delete", "NKS_SHELLPACKS_HTTP_COMMAND_DELETE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_REQUEST_DESC"
                        })
                    ])
                ], new DeleteCommand()),

            new CommandInfo("detach", "NKS_SHELLPACKS_FTPSFTP_COMMAND_DETACH_DESC", new DetachCommand()),

            new CommandInfo("editheader", "NKS_SHELLPACKS_HTTP_COMMAND_EDITHEADER_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "key", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_KEY_DESC"
                        }),
                        new CommandArgumentPart(true, "value", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_VALUE_DESC"
                        })
                    ])
                ], new EditHeaderCommand()),

            new CommandInfo("get", "NKS_SHELLPACKS_HTTP_COMMAND_GET_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_REQUEST_DESC"
                        })
                    ])
                ], new GetCommand(), CommandFlags.Wrappable),

            new CommandInfo("getstring", "NKS_SHELLPACKS_HTTP_COMMAND_GETSTRING_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_REQUEST_DESC"
                        })
                    ])
                ], new GetStringCommand(), CommandFlags.Wrappable),

            new CommandInfo("lsheader", "NKS_SHELLPACKS_HTTP_COMMAND_LSHEADER_DESC", new LsHeaderCommand(), CommandFlags.Wrappable),

            new CommandInfo("put", "NKS_SHELLPACKS_HTTP_COMMAND_PUT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_REQUEST_DESC"
                        }),
                        new CommandArgumentPart(true, "pathtofile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_FILE_DESC"
                        })
                    ])
                ], new PutCommand(), CommandFlags.Wrappable),

            new CommandInfo("putstring", "NKS_SHELLPACKS_HTTP_COMMAND_PUTSTRING_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_REQUEST_DESC"
                        }),
                        new CommandArgumentPart(true, "string", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_STRING_DESC"
                        })
                    ])
                ], new PutStringCommand(), CommandFlags.Wrappable),

            new CommandInfo("post", "NKS_SHELLPACKS_HTTP_COMMAND_POST_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_REQUEST_DESC"
                        }),
                        new CommandArgumentPart(true, "pathtofile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_FILE_DESC"
                        })
                    ])
                ], new PostCommand(), CommandFlags.Wrappable),

            new CommandInfo("poststring", "NKS_SHELLPACKS_HTTP_COMMAND_POSTSTRING",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_REQUEST_DESC"
                        }),
                        new CommandArgumentPart(true, "string", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_STRING_DESC"
                        })
                    ])
                ], new PostStringCommand(), CommandFlags.Wrappable),

            new CommandInfo("rmheader", "NKS_SHELLPACKS_HTTP_COMMAND_RMHEADER_",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "key", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_KEY_DESC"
                        })
                    ])
                ], new RmHeaderCommand()),

            new CommandInfo("setagent", "NKS_SHELLPACKS_HTTP_COMMAND_SETAGENT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "userAgent", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_SHELLPACKS_HTTP_COMMAND_SETAGENT_ARGUMENT_UA_DESC"
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
