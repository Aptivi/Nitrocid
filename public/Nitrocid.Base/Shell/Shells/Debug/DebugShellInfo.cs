//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Prompts;
using Nitrocid.Base.Shell.Shells.Debug.Commands;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Nitrocid.Base.Shell.Shells.Debug.Presets;
using System.Linq;
using Nitrocid.Base.Misc.Reflection;

#if NKS_EXTENSIONS
using Nitrocid.Base.Kernel.Extensions;
#endif

namespace Nitrocid.Base.Shell.Shells.Debug
{
    /// <summary>
    /// Common debug shell class
    /// </summary>
    internal class DebugShellInfo : BaseShellInfo<DebugShell>, IShellInfo
    {

        /// <summary>
        /// Debug commands
        /// </summary>
        public override BaseCommand[] Commands =>
        [
            new CurrentBtCommand(),
            new DebugLogCommand(),
            new ExcInfoCommand(),
            new GetFieldValueCommand(),
            new GetPropertyValueCommand(),
            new KeyInfoCommand(),

#if NKS_EXTENSIONS
            new LsAddonsCommand(),
            new LsAddonFieldsCommand(),
            new LsAddonFuncsCommand(),
            new LsAddonFuncParamsCommand(),
            new LsAddonPropsCommand(),
            new LsAddonTypesCommand(),
            new LsBaseAddonsCommand(),
#endif

            new LsFieldsCommand(),
            new LsPropertiesCommand(),
            new LsShellsCommand(),
            new PreviewSplashCommand(),
            new ShowMainBufferCommand(),
            new SendNotificationCommand(),
        ];

        public override Dictionary<string, PromptPresetBase> ShellPresets => new()
        {
            { "Default", new DebugDefaultPreset() },
            { "PowerLine1", new DebugPowerLine1Preset() },
            { "PowerLine2", new DebugPowerLine2Preset() },
            { "PowerLine3", new DebugPowerLine3Preset() },
            { "PowerLineBG1", new DebugPowerLineBG1Preset() },
            { "PowerLineBG2", new DebugPowerLineBG2Preset() },
            { "PowerLineBG3", new DebugPowerLineBG3Preset() }
        };
    }
}
