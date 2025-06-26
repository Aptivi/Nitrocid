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

using Microsoft.Data.Sqlite;
using Nitrocid.Extras.SqlShell.Settings;
using Nitrocid.Extras.SqlShell.Sql;
using Nitrocid.Extras.SqlShell.Tools;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Modifications;
using Nitrocid.Shell.ShellBase.Shells;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Nitrocid.Extras.SqlShell
{
    internal class SqlShellInit : IAddon
    {
        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasSqlShell);

        internal static SqlConfig SqlConfig =>
            (SqlConfig)Config.baseConfigurations[nameof(SqlConfig)];

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => new(new Dictionary<string, Delegate>()
        {
            { nameof(SqlShellCommon.IsSql), new Func<string, bool>(SqlShellCommon.IsSql) },
            { nameof(SqlEditTools.SqlEdit_OpenSqlFile), new Func<string, bool>(SqlEditTools.SqlEdit_OpenSqlFile) },
            { nameof(SqlEditTools.SqlEdit_CheckSqlFile), new Func<string, bool>(SqlEditTools.SqlEdit_CheckSqlFile) },
            { nameof(SqlEditTools.SqlEdit_SqlCommand), new Func<string, string[], SqliteParameter[], bool>((query, replies, parameters) => SqlEditTools.SqlEdit_SqlCommand(query, ref replies, parameters)) },
        });

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        {
            var config = new SqlConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterAddonShell("SqlShell", new SqlShellInfo());
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.UnregisterAddonShell("SqlShell");
            ConfigTools.UnregisterBaseSetting(nameof(SqlConfig));
        }
    }
}
