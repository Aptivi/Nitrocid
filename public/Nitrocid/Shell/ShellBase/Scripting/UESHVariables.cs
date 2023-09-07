
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
using System.Linq;
using KS.Drivers;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Commands.ArgumentsParsers;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.ShellBase.Scripting
{
    /// <summary>
    /// UESH shell variable manager
    /// </summary>
    public static class UESHVariables
    {

        internal static Dictionary<string, string> ShellVariables = new() {
            { "$IsRunningFromGrilo", Convert.ToString(KernelPlatform.IsRunningFromGrilo()) },
            { "$FrameworkRid", KernelPlatform.GetCurrentRid() },
        };

        /// <summary>
        /// Checks to see if the variable name starts with the correct format
        /// </summary>
        /// <param name="var">A $variable name</param>
        /// <returns>Sanitized variable name</returns>
        public static string SanitizeVariableName(string var)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Sanitizing variable {0}...", var);
            if (!var.StartsWith("$"))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Unsanitized variable found. Prepending $...");
                var = $"${var}";
            }
            return var;
        }

        /// <summary>
        /// Initializes a $variable
        /// </summary>
        /// <param name="var">A $variable</param>
        public static void InitializeVariable(string var)
        {
            var = SanitizeVariableName(var);
            if (!ShellVariables.ContainsKey(var))
            {
                ShellVariables.Add(var, "");
                DebugWriter.WriteDebug(DebugLevel.I, "Initialized variable {0}", var);
            }
        }

        /// <summary>
        /// Gets a value of a $variable on command line
        /// </summary>
        /// <param name="var">A $variable</param>
        /// <param name="cmd">A command line in script</param>
        /// <returns>A command line in script that has a value of $variable</returns>
        public static string GetVariableCommand(string var, string cmd)
        {
            var CommandArgumentsInfo = ArgumentsParser.ParseShellCommandArguments(cmd, ShellType.Shell);
            string NewCommand = $"{CommandArgumentsInfo.Command} ";
            if (!CommandManager.IsCommandFound(CommandArgumentsInfo.Command) ||
                !ShellManager.GetShellInfo(ShellType.Shell).Commands[CommandArgumentsInfo.Command].Flags.HasFlag(CommandFlags.SettingVariable))
            {
                foreach (string Word in CommandArgumentsInfo.ArgumentsList)
                {
                    string finalWord = Word;
                    if (finalWord.Contains(var) & finalWord.StartsWith("$"))
                    {
                        finalWord = ShellVariables[var];
                    }
                    NewCommand += $"{finalWord} ";
                }
                DebugWriter.WriteDebug(DebugLevel.I, "Replaced variable {0} with their values. Result: {1}", var, NewCommand);
                return NewCommand.TrimEnd(' ');
            }
            return cmd;
        }

        /// <summary>
        /// Gets a value of a $variable
        /// </summary>
        /// <param name="var">A $variable</param>
        /// <returns>A value of $variable, or a variable name if not found</returns>
        public static string GetVariable(string var)
        {
            try
            {
                string FinalVar = SanitizeVariableName(var);
                if (ShellVariables.ContainsKey(FinalVar))
                    return ShellVariables[FinalVar];
                return var;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error getting variable {0}: {1}", var, ex.Message);
            }
            return var;
        }

        /// <summary>
        /// Gets the variables and returns the available variables as a dictionary
        /// </summary>
        public static Dictionary<string, string> Variables =>
            ShellVariables;

        /// <summary>
        /// Sets a $variable
        /// </summary>
        /// <param name="var">A $variable</param>
        /// <param name="value">A value to set to $variable</param>
        public static bool SetVariable(string var, string value)
        {
            try
            {
                var = SanitizeVariableName(var);
                if (!ShellVariables.ContainsKey(var))
                    InitializeVariable(var);
                ShellVariables[var] = value;
                DebugWriter.WriteDebug(DebugLevel.I, "Set variable {0} to {1}", var, value);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error setting variable {0}: {1}", var, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Makes an array of a $variable with the chosen number of values (e.g. $variable[0] = first value, $variable[1] = second value, ...)
        /// </summary>
        /// <param name="var">A $variable array name</param>
        /// <param name="values">A set of values to set</param>
        public static bool SetVariables(string var, string[] values)
        {
            try
            {
                var = SanitizeVariableName(var);
                for (int ValueIndex = 0; ValueIndex <= values.Length - 1; ValueIndex++)
                {
                    string VarName = $"{var}[{ValueIndex}]";
                    string VarValue = values[ValueIndex];
                    if (!ShellVariables.ContainsKey(VarName))
                        InitializeVariable(VarName);
                    ShellVariables[VarName] = VarValue;
                    DebugWriter.WriteDebug(DebugLevel.I, "Set variable {0} to {1}", VarName, VarValue);
                }
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error creating variable array {0}: {1}", var, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Removes a variable from the shell variables dictionary
        /// </summary>
        /// <param name="var">Target variable</param>
        public static bool RemoveVariable(string var)
        {
            try
            {
                var = SanitizeVariableName(var);
                return ShellVariables.Remove(var);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error removing variable {0}: {1}", var, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Parses the system environment variables and converts them to the UESH shell variables
        /// </summary>
        public static void ConvertSystemEnvironmentVariables()
        {
            var EnvVars = Environment.GetEnvironmentVariables();
            foreach (string EnvVar in EnvVars.Keys)
                SetVariable(EnvVar, Convert.ToString(EnvVars[EnvVar]));
        }

        /// <summary>
        /// Initializes the UESH variables from the expression
        /// </summary>
        /// <param name="expression">UESH variable expressions in the form of $var=value</param>
        public static void InitializeVariablesFrom(string expression)
        {
            // Get the variable keys and values
            expression = string.IsNullOrEmpty(expression) ? "" : expression;
            var varTuple = GetVariablesFrom(expression);
            var varStoreKeys = varTuple.varStoreKeys;
            var varStoreValues = varTuple.varStoreValues;

            // Now, initialize the variables one by one
            for (int i = 0; i < varStoreKeys.Length; i++)
            {
                // Get the key and the value
                string varStoreKey = varStoreKeys[i];
                string varStoreValue = varStoreValues[i].ReleaseDoubleQuotes();
                DebugWriter.WriteDebug(DebugLevel.I, "Adding {0} to {1}...", varStoreValue, varStoreKey);

                // Initialize each variable and set them
                InitializeVariable(varStoreKey);
                bool result = SetVariable(varStoreKey, varStoreValue);
                DebugWriter.WriteDebug(DebugLevel.I, "Added {0} to {1}: {2}", varStoreValue, varStoreKey, result);
            }
        }

        /// <summary>
        /// Gets the UESH variables from the expression
        /// </summary>
        /// <param name="expression">UESH variable expressions in the form of $var=value</param>
        public static (string[] varStoreKeys, string[] varStoreValues) GetVariablesFrom(string expression)
        {
            // Get the variable keys and values
            expression = string.IsNullOrEmpty(expression) ? "" : expression;
            string localVarStoreValuesMatch = /* lang=regex */
                @"((?'key'\$\S+)=(?'value'(""(.+?)(?<![^\\]\\)"")|('(.+?)(?<![^\\]\\)')|(`(.+?)(?<![^\\]\\)`)|(?:[^\\\s\)]|\\.)+))+";
            var varStoreMatches = DriverHandler.CurrentRegexpDriverLocal.Matches(expression, localVarStoreValuesMatch);
            var varStoreKeys = varStoreMatches
                .Select((match) => match.Groups["key"].Value).ToArray();
            var varStoreValues = varStoreMatches
                .Select((match) => match.Groups["value"].Value).ToArray();
            DebugWriter.WriteDebug(DebugLevel.I, "Keys: {0} [{1}], Values: {2} [{3}]", varStoreKeys.Length, string.Join(", ", varStoreKeys), varStoreValues.Length, string.Join(", ", varStoreValues));
            return (varStoreKeys, varStoreValues);
        }

    }
}
