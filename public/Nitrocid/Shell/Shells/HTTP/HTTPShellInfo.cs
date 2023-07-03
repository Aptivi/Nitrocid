
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
using KS.Shell.Prompts.Presets.HTTP;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.HTTP.Commands;

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
            { "delete", new CommandInfo("delete", ShellType, /* Localizable */ "Deletes content from HTTP server", new CommandArgumentInfo(new[] { "<request>" }, true, 1), new HTTP_DeleteCommand()) },
            { "get", new CommandInfo("get", ShellType, /* Localizable */ "Gets the response from the HTTP server using the specified request", new CommandArgumentInfo(new[] { "<request>" }, true, 1), new HTTP_GetCommand()) },
            { "getstring", new CommandInfo("getstring", ShellType, /* Localizable */ "Gets the string from the HTTP server using the specified request", new CommandArgumentInfo(new[] { "<request>" }, true, 1), new HTTP_GetStringCommand()) },
            { "put", new CommandInfo("put", ShellType, /* Localizable */ "Puts the file to the HTTP server using the specified request", new CommandArgumentInfo(new[] { "<request> <pathtofile>" }, true, 2), new HTTP_PutCommand()) },
            { "putstring", new CommandInfo("putstring", ShellType, /* Localizable */ "Puts the string to the HTTP server using the specified request", new CommandArgumentInfo(new[] { "<request> <string>" }, true, 2), new HTTP_PutStringCommand()) },
            { "post", new CommandInfo("post", ShellType, /* Localizable */ "Posts the file to the HTTP server using the specified request", new CommandArgumentInfo(new[] { "<request> <pathtofile>" }, true, 2), new HTTP_PostCommand()) },
            { "poststring", new CommandInfo("poststring", ShellType, /* Localizable */ "Posts the string to the HTTP server using the specified request", new CommandArgumentInfo(new[] { "<request> <string>" }, true, 2), new HTTP_PostStringCommand()) },
            { "setsite", new CommandInfo("setsite", ShellType, /* Localizable */ "Sets the HTTP site. Must be a valid URI.", new CommandArgumentInfo(new[] { "<uri>" }, true, 1), new HTTP_SetSiteCommand()) }
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

    }
}
