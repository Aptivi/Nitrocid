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
using System.Linq;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Scripting.Conditions.Types;
using Textify.General;

namespace Nitrocid.Shell.ShellBase.Scripting.Conditions
{
    /// <summary>
    /// UESH condition manager
    /// </summary>
    public static class UESHConditional
    {

        private readonly static Dictionary<string, BaseCondition> Conditions = new()
        {
            { "eq", new EqualsCondition() },
            { "neq", new NotEqualsCondition() },
            { "les", new LessThanCondition() },
            { "lesoreq", new LessThanOrEqualCondition() },
            { "gre", new GreaterThanCondition() },
            { "greoreq", new GreaterThanOrEqualCondition() },
            { "fileex", new FileExistsCondition() },
            { "filenex", new FileNotExistsCondition() },
            { "direx", new DirectoryExistsCondition() },
            { "dirnex", new DirectoryNotExistsCondition() },
            { "has", new ContainsCondition() },
            { "hasno", new NotContainsCondition() },
            { "ispath", new ValidPathCondition() },
            { "isnotpath", new InvalidPathCondition() },
            { "isfname", new ValidFileNameCondition() },
            { "isnotfname", new InvalidFileNameCondition() },
            { "sane", new HashesMatchCondition() },
            { "insane", new HashesMismatchCondition() },
            { "fsane", new FileHashMatchCondition() },
            { "finsane", new FileHashMismatchCondition() },
            { "is", new IsCondition() },
            { "isnot", new IsNotCondition() },
            { "isplat", new IsPlatCondition() },
            { "isnotplat", new IsNotPlatCondition() },
            { "none", new NoneCondition() }
        };

        private readonly static Dictionary<string, BaseCondition> CustomConditions = [];

        /// <summary>
        /// The available condition names
        /// </summary>
        public static Dictionary<string, BaseCondition> AvailableConditions =>
            Conditions.Union(CustomConditions).ToDictionary((kvp) => kvp.Key, (kvp) => kvp.Value);

        /// <summary>
        /// Checks if the UESH condition was satisfied
        /// </summary>
        /// <param name="ConditionToSatisfy">The UESH condition to satisfy</param>
        public static bool ConditionSatisfied(string ConditionToSatisfy)
        {
            if (!string.IsNullOrWhiteSpace(ConditionToSatisfy))
            {
                bool Satisfied;

                // First, check for the existence of one of the conditional words
                DebugWriter.WriteDebug(DebugLevel.I, "Checking expression {0} for condition", ConditionToSatisfy);
                var EnclosedWords = ConditionToSatisfy.SplitEncloseDoubleQuotes();
                var ConditionFound = false;
                string ConditionType = "none";
                var ConditionBase = AvailableConditions[ConditionType];
                foreach (string Condition in AvailableConditions.Keys)
                {
                    if (EnclosedWords.Contains(Condition))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Condition found in the expression string. It was {0}", Condition);
                        ConditionFound = true;
                        ConditionType = Condition;
                        ConditionBase = AvailableConditions[ConditionType];
                    }
                }
                if (!ConditionFound)
                    throw new KernelException(KernelExceptionType.UESHConditionParse, Translate.DoTranslation("The condition was not found in the expression."));

                // Check the expression for argument numbers and middle condition
                int RequiredArguments = ConditionBase.ConditionRequiredArguments;
                int ConditionPosition = ConditionBase.ConditionPosition;
                if (EnclosedWords.Length < RequiredArguments)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Argument count {0} is less than the required arguments {1}", EnclosedWords.Length, RequiredArguments);
                    throw new KernelException(KernelExceptionType.UESHConditionParse, Translate.DoTranslation("Condition {0} requires {1} arguments. Got {2}."), ConditionType, RequiredArguments, EnclosedWords.Length);
                }
                if (!AvailableConditions.ContainsKey(EnclosedWords[ConditionPosition - 1]))
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Condition should be in position {0}, but {1} is not a condition.", ConditionPosition, EnclosedWords[ConditionPosition - 1]);
                    throw new KernelException(KernelExceptionType.UESHConditionParse, Translate.DoTranslation("The condition needs to be placed in the end."));
                }

                // Execute the conditions
                try
                {
                    switch (RequiredArguments)
                    {
                        case 1:
                            {
                                Satisfied = ConditionBase.IsConditionSatisfied("", "");
                                break;
                            }
                        case 2:
                            {
                                string Variable = "";
                                switch (ConditionPosition)
                                {
                                    case 1:
                                        {
                                            // Expression can be "<condition> <variable>". Since there is no middle here, assume first.
                                            Variable = EnclosedWords[1];
                                            break;
                                        }
                                    case 2:
                                        {
                                            // Expression can be "<variable> <condition>"
                                            Variable = EnclosedWords[0];
                                            break;
                                        }
                                }
                                Satisfied = ConditionBase.IsConditionSatisfied(Variable, "");
                                break;
                            }
                        case 3:
                            {
                                string FirstVariable = "";
                                string SecondVariable = "";
                                switch (ConditionPosition)
                                {
                                    case 1:
                                        {
                                            // Expression can be "<condition> <variable> <variable>"
                                            FirstVariable = EnclosedWords[1];
                                            SecondVariable = EnclosedWords[2];
                                            break;
                                        }
                                    case 2:
                                        {
                                            // Expression can be "<variable> <condition> <variable>"
                                            FirstVariable = EnclosedWords[0];
                                            SecondVariable = EnclosedWords[2];
                                            break;
                                        }
                                    case 3:
                                        {
                                            // Expression can be "<variable> <variable> <condition>"
                                            FirstVariable = EnclosedWords[0];
                                            SecondVariable = EnclosedWords[1];
                                            break;
                                        }
                                }
                                Satisfied = ConditionBase.IsConditionSatisfied(FirstVariable, SecondVariable);
                                break;
                            }

                        default:
                            {
                                var Variables = EnclosedWords.SkipWhile(str => str == EnclosedWords[ConditionPosition - 1]).ToArray();
                                Satisfied = ConditionBase.IsConditionSatisfied(Variables);
                                break;
                            }
                    }
                    DebugWriter.WriteDebug(DebugLevel.I, "Satisfied: {0}", Satisfied);
                    return Satisfied;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Syntax error in {0}: {1}", ConditionToSatisfy, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new KernelException(KernelExceptionType.UESHConditionParse, Translate.DoTranslation("Error parsing expression due to syntax error.") + " {0}: {1}", ex, ConditionToSatisfy, ex.Message);
                }
            }
            return false;
        }

        /// <summary>
        /// Registers a custom condition
        /// </summary>
        /// <param name="name">Condition name to register. Must be short and not contain whitespace</param>
        /// <param name="condition">Condition class containing information about the condition</param>
        public static void RegisterCondition(string name, BaseCondition condition)
        {
            if (Conditions.ContainsKey(name) || CustomConditions.ContainsKey(name))
                throw new KernelException(KernelExceptionType.UESHConditional, Translate.DoTranslation("Can't register a condition that already exists."));
            if (condition is null)
                throw new KernelException(KernelExceptionType.UESHConditional, Translate.DoTranslation("Can't register an empty condition."));

            // Add a custom condition
            CustomConditions.Add(name, condition);
        }

        /// <summary>
        /// Unregisters a custom condition
        /// </summary>
        /// <param name="name">Condition name to unregister. Must be short and not contain whitespace</param>
        public static void UnregisterCondition(string name)
        {
            if (!CustomConditions.ContainsKey(name))
                throw new KernelException(KernelExceptionType.UESHConditional, Translate.DoTranslation("Can't unregister a condition that doesn't exist."));

            // Add a custom condition
            CustomConditions.Remove(name);
        }

    }
}
