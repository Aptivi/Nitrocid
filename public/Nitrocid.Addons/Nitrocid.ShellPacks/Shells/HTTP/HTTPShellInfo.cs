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
            new CommandInfo("addheader", LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ADDHEADER_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "key", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_KEY_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "value", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_VALUE_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new AddHeaderCommand()),

            new CommandInfo("curragent", LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_CURRAGENT_DESC", "Nitrocid.ShellPacks"), new CurrAgentCommand()),

            new CommandInfo("delete", LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_DELETE_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_REQUEST_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new DeleteCommand()),

            new CommandInfo("detach", LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_COMMAND_DETACH_DESC", "Nitrocid.ShellPacks"), new DetachCommand()),

            new CommandInfo("editheader", LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_EDITHEADER_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "key", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_KEY_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "value", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_VALUE_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new EditHeaderCommand()),

            new CommandInfo("get", LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_GET_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_REQUEST_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new GetCommand(), CommandFlags.Wrappable),

            new CommandInfo("getstring", LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_GETSTRING_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_REQUEST_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new GetStringCommand(), CommandFlags.Wrappable),

            new CommandInfo("lsheader", LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_LSHEADER_DESC", "Nitrocid.ShellPacks"), new LsHeaderCommand(), CommandFlags.Wrappable),

            new CommandInfo("put", LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_PUT_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_REQUEST_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "pathtofile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_FILE_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new PutCommand(), CommandFlags.Wrappable),

            new CommandInfo("putstring", LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_PUTSTRING_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_REQUEST_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "string", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_STRING_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new PutStringCommand(), CommandFlags.Wrappable),

            new CommandInfo("post", LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_POST_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_REQUEST_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "pathtofile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_FILE_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new PostCommand(), CommandFlags.Wrappable),

            new CommandInfo("poststring", LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_POSTSTRING", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "request", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_REQUEST_DESC", "Nitrocid.ShellPacks")
                        }),
                        new CommandArgumentPart(true, "string", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_STRING_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new PostStringCommand(), CommandFlags.Wrappable),

            new CommandInfo("rmheader", LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_RMHEADER_", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "key", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_ARGUMENT_KEY_DESC", "Nitrocid.ShellPacks")
                        })
                    ])
                ], new RmHeaderCommand()),

            new CommandInfo("setagent", LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_SETAGENT_DESC", "Nitrocid.ShellPacks"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "userAgent", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_COMMAND_SETAGENT_ARGUMENT_UA_DESC", "Nitrocid.ShellPacks")
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
