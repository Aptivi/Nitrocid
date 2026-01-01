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

using Nitrocid.Drivers.Encryption;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Kernel.Debugging;

namespace Nitrocid.Shell.ShellBase.Scripting
{
    /// <summary>
    /// UESH operator code functions
    /// </summary>
    public static class UESHOperators
    {

        /// <summary>
        /// Checks to see if the two UESH variables are equal
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if the two UESH variables are equal. False if otherwise.</returns>
        public static bool UESHVariableEqual(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} and {1} for equality...", FirstVariable, SecondVariable);
            string FirstVarValue = UESHVariables.GetVariable(FirstVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = UESHVariables.GetVariable(SecondVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            Satisfied = FirstVarValue == SecondVarValue;
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the two UESH variables are different
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if the two UESH variables are different. False if otherwise.</returns>
        public static bool UESHVariableNotEqual(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} and {1} for inequality...", FirstVariable, SecondVariable);
            string FirstVarValue = UESHVariables.GetVariable(FirstVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = UESHVariables.GetVariable(SecondVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            Satisfied = FirstVarValue != SecondVarValue;
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if one of the two UESH variables is less than the other
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if the one of the two UESH variables is less than the other. False if otherwise.</returns>
        public static bool UESHVariableLessThan(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} and {1} for inequality...", FirstVariable, SecondVariable);
            string FirstVarValue = UESHVariables.GetVariable(FirstVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = UESHVariables.GetVariable(SecondVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            long FirstVarInt = long.Parse(FirstVarValue);
            long SecondVarInt = long.Parse(SecondVarValue);
            Satisfied = FirstVarInt < SecondVarInt;
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if one of the two UESH variables is greater than the other
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if the one of the two UESH variables is greater than the other. False if otherwise.</returns>
        public static bool UESHVariableGreaterThan(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} and {1} for inequality...", FirstVariable, SecondVariable);
            string FirstVarValue = UESHVariables.GetVariable(FirstVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = UESHVariables.GetVariable(SecondVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            long FirstVarInt = long.Parse(FirstVarValue);
            long SecondVarInt = long.Parse(SecondVarValue);
            Satisfied = FirstVarInt > SecondVarInt;
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if one of the two UESH variables is less than the other or equal to each other
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if the one of the two UESH variables is less than the other or equal to each other. False if otherwise.</returns>
        public static bool UESHVariableLessThanOrEqual(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} and {1} for inequality...", FirstVariable, SecondVariable);
            string FirstVarValue = UESHVariables.GetVariable(FirstVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = UESHVariables.GetVariable(SecondVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            long FirstVarInt = long.Parse(FirstVarValue);
            long SecondVarInt = long.Parse(SecondVarValue);
            Satisfied = FirstVarInt <= SecondVarInt;
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if one of the two UESH variables is greater than the other or equal to each other
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if the one of the two UESH variables is greater than the other or equal to each other. False if otherwise.</returns>
        public static bool UESHVariableGreaterThanOrEqual(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} and {1} for inequality...", FirstVariable, SecondVariable);
            string FirstVarValue = UESHVariables.GetVariable(FirstVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = UESHVariables.GetVariable(SecondVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            long FirstVarInt = long.Parse(FirstVarValue);
            long SecondVarInt = long.Parse(SecondVarValue);
            Satisfied = FirstVarInt >= SecondVarInt;
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the UESH variable is a file path and exists
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if the file exists. False if otherwise.</returns>
        public static bool UESHVariableFileExists(string Variable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} for file existence...", Variable);
            string VarValue = UESHVariables.GetVariable(Variable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue);
            Satisfied = Checking.FileExists(VarValue, true);
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the UESH variable is a file path and doesn't exist
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if the file doesn't exist. False if otherwise.</returns>
        public static bool UESHVariableFileDoesNotExist(string Variable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} for file existence...", Variable);
            string VarValue = UESHVariables.GetVariable(Variable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue);
            Satisfied = !Checking.FileExists(VarValue, true);
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the UESH variable is a directory path and exists
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if the directory exists. False if otherwise.</returns>
        public static bool UESHVariableDirectoryExists(string Variable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} for directory existence...", Variable);
            string VarValue = UESHVariables.GetVariable(Variable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue);
            Satisfied = Checking.FolderExists(VarValue, true);
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the UESH variable is a directory path and doesn't exist
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if the directory doesn't exist. False if otherwise.</returns>
        public static bool UESHVariableDirectoryDoesNotExist(string Variable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} for directory existence...", Variable);
            string VarValue = UESHVariables.GetVariable(Variable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue);
            Satisfied = !Checking.FolderExists(VarValue, true);
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if a UESH variable contains an expression
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if satisfied. False if otherwise.</returns>
        public static bool UESHVariableContains(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} and {1}...", FirstVariable, SecondVariable);
            string FirstVarValue = UESHVariables.GetVariable(FirstVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = UESHVariables.GetVariable(SecondVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            Satisfied = FirstVarValue.Contains(SecondVarValue);
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if a UESH variable doesn't contain an expression
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if satisfied. False if otherwise.</returns>
        public static bool UESHVariableNotContains(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} and {1}...", FirstVariable, SecondVariable);
            string FirstVarValue = UESHVariables.GetVariable(FirstVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = UESHVariables.GetVariable(SecondVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            Satisfied = !FirstVarValue.Contains(SecondVarValue);
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the UESH variable is a valid file path
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if valid. False if otherwise.</returns>
        public static bool UESHVariableValidPath(string Variable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0}...", Variable);
            string VarValue = UESHVariables.GetVariable(Variable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue);
            Satisfied = Parsing.TryParsePath(VarValue);
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the UESH variable is not a valid file path
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if invalid. False if otherwise.</returns>
        public static bool UESHVariableInvalidPath(string Variable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0}...", Variable);
            string VarValue = UESHVariables.GetVariable(Variable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue);
            Satisfied = !Parsing.TryParsePath(VarValue);
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the UESH variable is a valid file name
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if valid. False if otherwise.</returns>
        public static bool UESHVariableValidFileName(string Variable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0}...", Variable);
            string VarValue = UESHVariables.GetVariable(Variable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue);
            Satisfied = Parsing.TryParseFileName(VarValue);
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if the value of the UESH variable is not a valid file name
        /// </summary>
        /// <param name="Variable">The $variable</param>
        /// <returns>True if invalid. False if otherwise.</returns>
        public static bool UESHVariableInvalidFileName(string Variable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0}...", Variable);
            string VarValue = UESHVariables.GetVariable(Variable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of {0}: {1}...", Variable, VarValue);
            Satisfied = !Parsing.TryParseFileName(VarValue);
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if a UESH variable matches its hash specified on the second
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if satisfied. False if otherwise.</returns>
        public static bool UESHVariableHashMatch(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} and {1}...", FirstVariable, SecondVariable);
            string FirstVarValue = UESHVariables.GetVariable(FirstVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = UESHVariables.GetVariable(SecondVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            Satisfied = Encryption.GetEncryptedString(FirstVarValue, "SHA256") == SecondVarValue;
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if a UESH variable doesn't match its hash specified on the second
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if satisfied. False if otherwise.</returns>
        public static bool UESHVariableHashNoMatch(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} and {1}...", FirstVariable, SecondVariable);
            string FirstVarValue = UESHVariables.GetVariable(FirstVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = UESHVariables.GetVariable(SecondVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            Satisfied = Encryption.GetEncryptedString(FirstVarValue, "SHA256") != SecondVarValue;
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if a file specified from a UESH variable matches its hash specified on the second
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if satisfied. False if otherwise.</returns>
        public static bool UESHVariableFileHashMatch(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} and {1}...", FirstVariable, SecondVariable);
            string FirstVarValue = UESHVariables.GetVariable(FirstVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = UESHVariables.GetVariable(SecondVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            Satisfied = Encryption.GetEncryptedFile(FirstVarValue, "SHA256") == SecondVarValue;
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

        /// <summary>
        /// Checks to see if a file specified from a UESH variable doesn't match its hash specified on the second
        /// </summary>
        /// <param name="FirstVariable">The first $variable</param>
        /// <param name="SecondVariable">The second $variable</param>
        /// <returns>True if satisfied. False if otherwise.</returns>
        public static bool UESHVariableFileHashNoMatch(string FirstVariable, string SecondVariable)
        {
            bool Satisfied;
            DebugWriter.WriteDebug(DebugLevel.I, "Querying {0} and {1}...", FirstVariable, SecondVariable);
            string FirstVarValue = UESHVariables.GetVariable(FirstVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of first var {0}: {1}...", FirstVariable, FirstVarValue);
            string SecondVarValue = UESHVariables.GetVariable(SecondVariable);
            DebugWriter.WriteDebug(DebugLevel.I, "Got value of second var {0}: {1}...", SecondVariable, SecondVarValue);
            Satisfied = Encryption.GetEncryptedFile(FirstVarValue, "SHA256") != SecondVarValue;
            DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
            return Satisfied;
        }

    }
}
