//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Nitrocid.Files.Operations;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;

namespace Nitrocid.Shell.ShellBase.Aliases
{
    /// <summary>
    /// Alias management module
    /// </summary>
    public static class AliasManager
    {

        internal static List<AliasInfo> builtinAliases = [
            new AliasInfo("ls", "list", ShellManager.GetShellTypeName(ShellType.Shell))
        ];
        internal static List<AliasInfo> aliases = [];

        /// <summary>
        /// Initializes aliases
        /// </summary>
        public static void InitAliases()
        {
            // Get all aliases from file
            Making.MakeJsonFile(PathsManagement.GetKernelPath(KernelPathType.Aliases), false, true);
            string AliasJsonContent = Reading.ReadContentsText(PathsManagement.GetKernelPath(KernelPathType.Aliases));
            var aliasesArray = JsonConvert.DeserializeObject<AliasInfo[]>(AliasJsonContent) ?? [];
            aliases = [.. aliasesArray];
        }

        /// <summary>
        /// Saves aliases
        /// </summary>
        public static void SaveAliases()
        {
            // Save all aliases
            DebugWriter.WriteDebug(DebugLevel.I, "Saving aliases...");
            string serialized = JsonConvert.SerializeObject(aliases.ToArray(), Formatting.Indented);
            Writing.WriteContentsText(PathsManagement.GetKernelPath(KernelPathType.Aliases), serialized);
        }

        /// <summary>
        /// Adds alias to kernel
        /// </summary>
        /// <param name="SourceAlias">A command to be aliased. It should exist in both shell and remote debug.</param>
        /// <param name="Destination">A one-word command to alias to.</param>
        /// <param name="Type">Alias type, whether it be shell or remote debug.</param>
        /// <returns>True if successful, False if unsuccessful.</returns>
        public static bool AddAlias(string SourceAlias, string Destination, ShellType Type) =>
            AddAlias(SourceAlias, Destination, ShellManager.GetShellTypeName(Type));

        /// <summary>
        /// Adds alias to kernel
        /// </summary>
        /// <param name="SourceAlias">A command to be aliased. It should exist in both shell and remote debug.</param>
        /// <param name="Destination">A one-word command to alias to.</param>
        /// <param name="Type">Alias type, whether it be shell or remote debug.</param>
        /// <returns>True if successful, False if unsuccessful.</returns>
        public static bool AddAlias(string SourceAlias, string Destination, string Type)
        {
            if (Enum.IsDefined(typeof(ShellType), Type))
            {
                if (SourceAlias == Destination)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Assertion succeeded: {0} = {1}", SourceAlias, Destination);
                    throw new KernelException(KernelExceptionType.AliasInvalidOperation, Translate.DoTranslation("Alias can't be the same name as a command."));
                }
                else if (!CommandManager.IsCommandFound(SourceAlias, Type))
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} not found in all the command lists", Destination);
                    throw new KernelException(KernelExceptionType.AliasNoSuchCommand, Translate.DoTranslation("Command not found to alias to {0}."), Destination);
                }
                else if (DoesAliasExist(Destination, Type))
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Alias {0} already found", SourceAlias);
                    throw new KernelException(KernelExceptionType.AliasAlreadyExists, Translate.DoTranslation("Alias already found: {0}"), SourceAlias);
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Aliasing {0} to {1}", SourceAlias, Destination);
                    var aliasInstance = new AliasInfo()
                    {
                        alias = Destination,
                        command = SourceAlias,
                        type = Type,
                    };
                    aliases.Add(aliasInstance);
                    return true;
                }
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Type {0} not found.", Type);
                throw new KernelException(KernelExceptionType.AliasNoSuchType, Translate.DoTranslation("Invalid type {0}."), Type);
            }
        }

        /// <summary>
        /// Removes alias from kernel
        /// </summary>
        /// <param name="TargetAlias">An alias that needs to be removed.</param>
        /// <param name="Type">Alias type.</param>
        /// <returns>True if successful, False if unsuccessful.</returns>
        public static bool RemoveAlias(string TargetAlias, ShellType Type) =>
            RemoveAlias(TargetAlias, ShellManager.GetShellTypeName(Type));

        /// <summary>
        /// Removes alias from kernel
        /// </summary>
        /// <param name="TargetAlias">An alias that needs to be removed.</param>
        /// <param name="Type">Alias type.</param>
        /// <returns>True if successful, False if unsuccessful.</returns>
        public static bool RemoveAlias(string TargetAlias, string Type)
        {
            // Do the action!
            if (DoesAliasExist(TargetAlias, Type))
            {
                var AliasInfo = GetAlias(TargetAlias, Type);
                DebugWriter.WriteDebug(DebugLevel.I, "Target alias {0} is found under type {1}, so removing...", TargetAlias, Type);
                return aliases.Remove(AliasInfo);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "{0} is not found in the {1} aliases", TargetAlias, Type.ToString());
                throw new KernelException(KernelExceptionType.AliasNoSuchAlias, Translate.DoTranslation("Alias {0} is not found to be removed."), TargetAlias);
            }
        }

        /// <summary>
        /// Checks to see if the specified alias exists.
        /// </summary>
        /// <param name="TargetAlias">The existing alias</param>
        /// <param name="Type">The alias type</param>
        /// <returns>True if it exists; false if it doesn't exist</returns>
        public static bool DoesAliasExist(string TargetAlias, ShellType Type) =>
            DoesAliasExist(TargetAlias, ShellManager.GetShellTypeName(Type));

        /// <summary>
        /// Checks to see if the specified alias exists.
        /// </summary>
        /// <param name="TargetAlias">The existing alias</param>
        /// <param name="Type">The alias type</param>
        /// <returns>True if it exists; false if it doesn't exist</returns>
        public static bool DoesAliasExist(string TargetAlias, string Type) =>
            GetEntireAliasListFromType(Type).Any((info) => info.Alias == TargetAlias && info.Type == Type);

        /// <summary>
        /// Gets the aliases list from the shell type
        /// </summary>
        /// <param name="ShellType">Selected shell type</param>
        public static List<AliasInfo> GetAliasesListFromType(ShellType ShellType) =>
            GetAliasesListFromType(ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the aliases list from the shell type
        /// </summary>
        /// <param name="ShellType">Selected shell type</param>
        public static List<AliasInfo> GetAliasesListFromType(string ShellType) =>
            aliases.Where((info) => info.Type == ShellType).ToList();

        /// <summary>
        /// Gets the aliases list from the shell type
        /// </summary>
        /// <param name="ShellType">Selected shell type</param>
        public static List<AliasInfo> GetEntireAliasListFromType(ShellType ShellType) =>
            GetEntireAliasListFromType(ShellManager.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the aliases list from the shell type
        /// </summary>
        /// <param name="ShellType">Selected shell type</param>
        public static List<AliasInfo> GetEntireAliasListFromType(string ShellType) =>
            aliases
                .Union(builtinAliases)
                .Where((info) => info.Type == ShellType).ToList();

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <param name="TargetAlias">The existing alias</param>
        /// <param name="Type">The alias type</param>
        /// <returns>Alias info if it exists. Throws if it doesn't exist.</returns>
        public static AliasInfo GetAlias(string TargetAlias, ShellType Type) =>
            GetAlias(TargetAlias, ShellManager.GetShellTypeName(Type));

        /// <summary>
        /// Gets the alias.
        /// </summary>
        /// <param name="TargetAlias">The existing alias</param>
        /// <param name="Type">The alias type</param>
        /// <returns>Alias info if it exists. Throws if it doesn't exist.</returns>
        public static AliasInfo GetAlias(string TargetAlias, string Type)
        {
            if (!DoesAliasExist(TargetAlias, Type))
                throw new KernelException(KernelExceptionType.AliasNoSuchAlias, Translate.DoTranslation("Alias {0} is not found to be queried."), TargetAlias);

            // Get the list of available aliases and get an alias matching the target alias
            var aliases = GetEntireAliasListFromType(Type);
            return aliases.Single((info) => info.Alias == TargetAlias);
        }
    }
}
