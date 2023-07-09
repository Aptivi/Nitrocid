
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

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Operations;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KS.Shell.ShellBase.Aliases
{
    /// <summary>
    /// Alias management module
    /// </summary>
    public static class AliasManager
    {

        internal static Dictionary<string, string> AliasesToBeRemoved = new();

        /// <summary>
        /// Initializes aliases
        /// </summary>
        public static void InitAliases()
        {
            // Get all aliases from file
            Making.MakeFile(Paths.GetKernelPath(KernelPathType.Aliases), false);
            string AliasJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.Aliases));
            var AliasNameToken = JToken.Parse(!string.IsNullOrEmpty(AliasJsonContent) ? AliasJsonContent : "{}");
            string AliasCmd, ActualCmd;
            string AliasType;

            foreach (JObject AliasObject in AliasNameToken.Cast<JObject>())
            {
                AliasCmd = (string)AliasObject["Alias"];
                ActualCmd = (string)AliasObject["Command"];
                AliasType = (string)AliasObject["Type"];
                DebugWriter.WriteDebug(DebugLevel.I, "Adding \"{0}\" and \"{1}\" from Aliases.json to {2} list...", AliasCmd, ActualCmd, AliasType);
                var TargetAliasList = GetAliasesListFromType(AliasType);
                if (TargetAliasList.ContainsKey(AliasCmd))
                    TargetAliasList.Remove(AliasCmd);
                TargetAliasList.Add(AliasCmd, ActualCmd);
            }
        }

        /// <summary>
        /// Saves aliases
        /// </summary>
        public static void SaveAliases()
        {
            // Save all aliases
            foreach (string Shell in Shell.AvailableShells.Keys)
                SaveAliasesInternal(Shell);
        }

        internal static void SaveAliasesInternal(ShellType ShellType) =>
            SaveAliasesInternal(Shell.GetShellTypeName(ShellType));

        internal static void SaveAliasesInternal(string ShellType)
        {
            // Get all aliases from file
            Making.MakeFile(Paths.GetKernelPath(KernelPathType.Aliases), false);
            string AliasJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.Aliases));
            var AliasNameToken = JArray.Parse(!string.IsNullOrEmpty(AliasJsonContent) ? AliasJsonContent : "[]");

            // Save the alias
            var ShellAliases = GetAliasesListFromType(ShellType);
            for (int i = 0; i <= ShellAliases.Count - 1; i++)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Adding \"{0}\" and \"{1}\" from list to Aliases.json with type {2}...", ShellAliases.Keys.ElementAtOrDefault(i), ShellAliases.Values.ElementAtOrDefault(i), ShellType.ToString());
                var AliasObject = new JObject() { { "Alias", ShellAliases.Keys.ElementAtOrDefault(i) }, { "Command", ShellAliases.Values.ElementAtOrDefault(i) }, { "Type", ShellType.ToString() } };
                if (!DoesAliasExist(ShellAliases.Keys.ElementAtOrDefault(i), ShellType))
                    AliasNameToken.Add(AliasObject);
            }

            // Save changes
            File.WriteAllText(Paths.GetKernelPath(KernelPathType.Aliases), JsonConvert.SerializeObject(AliasNameToken, Formatting.Indented));
        }

        /// <summary>
        /// Manages the alias
        /// </summary>
        /// <param name="mode">Either add or rem</param>
        /// <param name="Type">Alias type (Shell or Remote Debug)</param>
        /// <param name="AliasCmd">A specified alias</param>
        /// <param name="DestCmd">A destination command (target)</param>
        public static void ManageAlias(string mode, ShellType Type, string AliasCmd, string DestCmd = "") =>
            ManageAlias(mode, Shell.GetShellTypeName(Type), AliasCmd, DestCmd);

        /// <summary>
        /// Manages the alias
        /// </summary>
        /// <param name="mode">Either add or rem</param>
        /// <param name="Type">Alias type (Shell or Remote Debug)</param>
        /// <param name="AliasCmd">A specified alias</param>
        /// <param name="DestCmd">A destination command (target)</param>
        public static void ManageAlias(string mode, string Type, string AliasCmd, string DestCmd = "")
        {
            if (Enum.IsDefined(typeof(ShellType), Type))
            {
                if (mode == "add")
                {
                    // User tries to add an alias.
                    try
                    {
                        AddAlias(AliasCmd, DestCmd, Type);
                        TextWriterColor.Write(Translate.DoTranslation("You can now run \"{0}\" as a command: \"{1}\"."), AliasCmd, DestCmd);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to add alias. Stack trace written using WStkTrc(). {0}", ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                        TextWriterColor.Write(ex.Message, true, KernelColorType.Error);
                    }
                }
                else if (mode == "rem")
                {
                    // User tries to remove an alias
                    try
                    {
                        RemoveAlias(AliasCmd, Type);
                        PurgeAliases();
                        TextWriterColor.Write(Translate.DoTranslation("Removed alias {0} successfully."), AliasCmd);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to remove alias. Stack trace written using WStkTrc(). {0}", ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                        TextWriterColor.Write(ex.Message, true, KernelColorType.Error);
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Mode {0} was neither add nor rem.", mode);
                    TextWriterColor.Write(Translate.DoTranslation("Invalid mode {0}."), true, KernelColorType.Error, mode);
                }

                // Save all aliases
                SaveAliases();
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Type {0} not found.", Type);
                TextWriterColor.Write(Translate.DoTranslation("Invalid type {0}."), true, KernelColorType.Error, Type);
            }
        }

        /// <summary>
        /// Adds alias to kernel
        /// </summary>
        /// <param name="SourceAlias">A command to be aliased. It should exist in both shell and remote debug.</param>
        /// <param name="Destination">A one-word command to alias to.</param>
        /// <param name="Type">Alias type, whether it be shell or remote debug.</param>
        /// <returns>True if successful, False if unsuccessful.</returns>
        public static bool AddAlias(string SourceAlias, string Destination, ShellType Type) =>
            AddAlias(SourceAlias, Destination, Type);

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
                    DebugWriter.WriteDebug(DebugLevel.I, "Assertion succeeded: {0} = {1}", SourceAlias, Destination);
                    throw new KernelException(KernelExceptionType.AliasInvalidOperation, Translate.DoTranslation("Alias can't be the same name as a command."));
                }
                else if (!CommandManager.IsCommandFound(Destination))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "{0} not found in all the command lists", Destination);
                    throw new KernelException(KernelExceptionType.AliasNoSuchCommand, Translate.DoTranslation("Command not found to alias to {0}."), Destination);
                }
                else if (DoesAliasExist(SourceAlias, Type))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Alias {0} already found", SourceAlias);
                    throw new KernelException(KernelExceptionType.AliasAlreadyExists, Translate.DoTranslation("Alias already found: {0}"), SourceAlias);
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Aliasing {0} to {1}", SourceAlias, Destination);
                    var TargetAliasList = GetAliasesListFromType(Type);
                    TargetAliasList.Add(SourceAlias, Destination);
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
            RemoveAlias(TargetAlias, Shell.GetShellTypeName(Type));

        /// <summary>
        /// Removes alias from kernel
        /// </summary>
        /// <param name="TargetAlias">An alias that needs to be removed.</param>
        /// <param name="Type">Alias type.</param>
        /// <returns>True if successful, False if unsuccessful.</returns>
        public static bool RemoveAlias(string TargetAlias, string Type)
        {
            // Variables
            var TargetAliasList = GetAliasesListFromType(Type);

            // Do the action!
            if (TargetAliasList.ContainsKey(TargetAlias))
            {
                string Aliased = TargetAliasList[TargetAlias];
                DebugWriter.WriteDebug(DebugLevel.I, "aliases({0}) is found. That makes it {1}", TargetAlias, Aliased);
                TargetAliasList.Remove(TargetAlias);
                AliasesToBeRemoved.Add($"{AliasesToBeRemoved.Count + 1}-{TargetAlias}", Type);
                return true;
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "{0} is not found in the {1} aliases", TargetAlias, Type.ToString());
                throw new KernelException(KernelExceptionType.AliasNoSuchAlias, Translate.DoTranslation("Alias {0} is not found to be removed."), TargetAlias);
            }
        }

        /// <summary>
        /// Purge aliases that are removed from config
        /// </summary>
        public static void PurgeAliases()
        {
            // Get all aliases from file
            Making.MakeFile(Paths.GetKernelPath(KernelPathType.Aliases), false);
            string AliasJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.Aliases));
            var AliasNameToken = JArray.Parse(!string.IsNullOrEmpty(AliasJsonContent) ? AliasJsonContent : "[]");

            // Purge aliases that are to be removed from config
            foreach (string TargetAliasItem in AliasesToBeRemoved.Keys)
            {
                for (int RemovedAliasIndex = AliasNameToken.Count - 1; RemovedAliasIndex >= 0; RemovedAliasIndex -= 1)
                {
                    var TargetAliasType = AliasesToBeRemoved[TargetAliasItem];
                    string TargetAlias = TargetAliasItem[(TargetAliasItem.IndexOf("-") + 1)..];
                    if ((string)AliasNameToken[RemovedAliasIndex]["Alias"] == TargetAlias & (string)AliasNameToken[RemovedAliasIndex]["Type"] == TargetAliasType.ToString())
                        AliasNameToken.RemoveAt(RemovedAliasIndex);
                }
            }

            // Clear the "to be removed" list of aliases
            AliasesToBeRemoved.Clear();

            // Save the changes
            File.WriteAllText(Paths.GetKernelPath(KernelPathType.Aliases), JsonConvert.SerializeObject(AliasNameToken, Formatting.Indented));
        }

        /// <summary>
        /// Checks to see if the specified alias exists.
        /// </summary>
        /// <param name="TargetAlias">The existing alias</param>
        /// <param name="Type">The alias type</param>
        /// <returns>True if it exists; false if it doesn't exist</returns>
        public static bool DoesAliasExist(string TargetAlias, ShellType Type) => 
            DoesAliasExist(TargetAlias, Shell.GetShellTypeName(Type));

        /// <summary>
        /// Checks to see if the specified alias exists.
        /// </summary>
        /// <param name="TargetAlias">The existing alias</param>
        /// <param name="Type">The alias type</param>
        /// <returns>True if it exists; false if it doesn't exist</returns>
        public static bool DoesAliasExist(string TargetAlias, string Type)
        {
            // Get all aliases from file
            Making.MakeFile(Paths.GetKernelPath(KernelPathType.Aliases), false);
            string AliasJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.Aliases));
            var AliasNameToken = JArray.Parse(!string.IsNullOrEmpty(AliasJsonContent) ? AliasJsonContent : "[]");

            // Check to see if the specified alias exists
            foreach (JObject AliasName in AliasNameToken.Cast<JObject>())
            {
                if ((string)AliasName["Alias"] == TargetAlias & (string)AliasName["Type"] == Type.ToString())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the aliases list from the shell type
        /// </summary>
        /// <param name="ShellType">Selected shell type</param>
        public static Dictionary<string, string> GetAliasesListFromType(ShellType ShellType) => 
            GetAliasesListFromType(Shell.GetShellTypeName(ShellType));

        /// <summary>
        /// Gets the aliases list from the shell type
        /// </summary>
        /// <param name="ShellType">Selected shell type</param>
        public static Dictionary<string, string> GetAliasesListFromType(string ShellType) => 
            Shell.GetShellInfo(ShellType).Aliases;

    }
}
