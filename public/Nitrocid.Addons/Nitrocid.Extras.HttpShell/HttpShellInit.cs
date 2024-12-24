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

using Nitrocid.Extras.HttpShell.HTTP;
using Nitrocid.Extras.HttpShell.Settings;
using Nitrocid.Extras.HttpShell.Tools;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Modifications;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Nitrocid.Extras.HttpShell
{
    internal class HttpShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("http", /* Localizable */ "Starts the HTTP shell",
                [
                    new CommandArgumentInfo()
                ], new HttpCommandExec())
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasHttpShell);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        internal static HttpConfig HttpConfig =>
            (HttpConfig)Config.baseConfigurations[nameof(HttpConfig)];

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => new(new Dictionary<string, Delegate>()
        {
            { nameof(HttpTools.HttpDelete), new Func<string, Task>(HttpTools.HttpDelete) },
            { nameof(HttpTools.HttpGetString), new Func<string, Task<string>>(HttpTools.HttpGetString) },
            { nameof(HttpTools.HttpGet), new Func<string, Task<HttpResponseMessage>>(HttpTools.HttpGet) },
            { nameof(HttpTools.HttpPutString), new Func<string, string, Task<HttpResponseMessage>>(HttpTools.HttpPutString) },
            { nameof(HttpTools.HttpPutFile), new Func<string, string, Task<HttpResponseMessage>>(HttpTools.HttpPutFile) },
            { nameof(HttpTools.HttpPostString), new Func<string, string, Task<HttpResponseMessage>>(HttpTools.HttpPostString) },
            { nameof(HttpTools.HttpPostFile), new Func<string, string, Task<HttpResponseMessage>>(HttpTools.HttpPostFile) },
            { nameof(HttpTools.HttpAddHeader), new Action<string, string>(HttpTools.HttpAddHeader) },
            { nameof(HttpTools.HttpRemoveHeader), new Action<string>(HttpTools.HttpRemoveHeader) },
            { nameof(HttpTools.HttpListHeaders), new Func<(string, string)[]>(HttpTools.HttpListHeaders) },
            { nameof(HttpTools.HttpGetCurrentUserAgent), new Func<string>(HttpTools.HttpGetCurrentUserAgent) },
            { nameof(HttpTools.HttpSetUserAgent), new Action<string>(HttpTools.HttpSetUserAgent) },
            { nameof(HttpTools.NeutralizeUri), new Func<string, string>(HttpTools.NeutralizeUri) },
        });

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        {
            var config = new HttpConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterAddonShell("HTTPShell", new HTTPShellInfo());
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterAddonShell("HTTPShell");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(HttpConfig));
        }
    }
}
