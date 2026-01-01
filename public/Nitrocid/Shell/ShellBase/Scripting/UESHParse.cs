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
using System.Linq;
using System.Collections.Generic;
using Nitrocid.Shell.ShellBase.Scripting.Conditions;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Events;
using Textify.General;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.CyclicWriters;
using Nitrocid.Files.Operations;

namespace Nitrocid.Shell.ShellBase.Scripting
{
    /// <summary>
    /// UESH script parser
    /// </summary>
    public static class UESHParse
    {
        /// <summary>
        /// Executes the UESH script
        /// </summary>
        /// <param name="ScriptPath">Full path to script</param>
        /// <param name="ScriptArguments">Script arguments</param>
        /// <param name="justLint">If true, just lints the script and throws exception if there are parsing errors</param>
        public static void Execute(string ScriptPath, string ScriptArguments, bool justLint = false)
        {
            int LineNo = 1;
            try
            {
                // Raise event
                EventsManager.FireEvent(EventType.UESHPreExecute, ScriptPath, ScriptArguments);

                // Open the script file for reading
                var FileLines = Reading.ReadAllLinesNoBlock(ScriptPath);
                DebugWriter.WriteDebug(DebugLevel.I, "Stream opened. Parsing script");

                // Look for $variables and initialize them
                for (int l = 0; l < FileLines.Length; l++)
                {
                    // Get line
                    string Line = FileLines[l];
                    DebugWriter.WriteDebug(DebugLevel.I, "Line {0}: \"{1}\"", vars: [LineNo, Line]);

                    // If $variable is found in string, initialize it
                    var SplitWords = Line.Split(' ');
                    for (int i = 0; i <= SplitWords.Length - 1; i++)
                        if (!UESHVariables.ShellVariables.ContainsKey(SplitWords[i]) & SplitWords[i].StartsWith("$"))
                            UESHVariables.InitializeVariable(SplitWords[i]);
                    LineNo++;
                }

                // Get all lines and parse comments, commands, and arguments
                string[] commandBlocks = ["if", "while", "until"];
                int commandStackNum = 0;
                bool newCommandStackRequired = false;
                bool retryLoopCondition = false;
                bool advance = true;
                List<(int, int)> whilePlaces = [];
                LineNo = 1;
                for (int l = 0; l < FileLines.Length; l++)
                {
                    // Decrement if not advancing
                    if (!advance)
                    {
                        advance = true;
                        l--;
                    }

                    // Get line
                    string Line = FileLines[l];
                    DebugWriter.WriteDebug(DebugLevel.I, "Line {0}: \"{1}\"", vars: [LineNo, Line]);

                    // First, trim the line from the left after checking the stack
                    string stackIndicator = new('|', commandStackNum);
                    if (Line.StartsWith(stackIndicator))
                    {
                        newCommandStackRequired = false;

                        // Get the actual command
                        Line = Line[commandStackNum..];

                        // If it still starts with the new stack indicator, throw an error
                        if (Line.StartsWith('|'))
                            throw new KernelException(KernelExceptionType.UESHScript, Translate.DoTranslation("You can't declare the new block before you place expressions that support the creation, like conditions or loops. The stack number is {0}.") + " {1}:{2}\n{3}", commandStackNum, ScriptPath, LineNo, GetLineHandle(ScriptPath, LineNo, commandStackNum, KernelColorType.Error));
                    }
                    else if (!Line.StartsWith(stackIndicator) && newCommandStackRequired)
                        throw new KernelException(KernelExceptionType.UESHScript, Translate.DoTranslation("When starting a new block, make sure that you've indented the stack correctly. The stack number is {0}.") + " {1}:{2}\n{3}", commandStackNum, ScriptPath, LineNo, GetLineHandle(ScriptPath, LineNo, commandStackNum, KernelColorType.Error));
                    else
                    {
                        if (retryLoopCondition && !justLint)
                        {
                            (int, int) whilePlace = whilePlaces[^1];
                            commandStackNum = whilePlace.Item2;
                            l = whilePlace.Item1;
                            Line = FileLines[l][commandStackNum..];
                        }
                        else
                            commandStackNum = 0;
                    }

                    // See if the line contains variable, and replace every instance of it with its value
                    var SplitWords = Line.SplitEncloseDoubleQuotes();
                    if (SplitWords is not null)
                        // Iterate every word
                        for (int i = 0; i <= SplitWords.Length - 1; i++)
                            // Every word that start with the $ sign means it's a variable that should be replaced with the
                            // value from the UESH variable manager.
                            if (SplitWords[i].StartsWith("$"))
                                Line = UESHVariables.GetVariableCommand(SplitWords[i], Line);

                    // See if the line contains argument placeholder, and replace every instance of it with its value
                    var SplitArguments = ScriptArguments.SplitEncloseDoubleQuotes();
                    if (SplitArguments is not null)
                        // Iterate every script argument
                        for (int j = 0; j <= SplitArguments.Length - 1; j++)
                            // If there is a placeholder variable like so:
                            //     echo Hello, {0}
                            // ...or...
                            //     echo {0}ification
                            // ...then proceed to replace the placeholder that contains an index of argument with the
                            // actual value
                            Line = Line.Replace($"{{{j}}}", SplitArguments[j]);

                    // See if the line is a command that starts with the if statement
                    if (SplitWords is not null)
                    {
                        string Command = SplitWords[0];
                        string Arguments = Line.RemovePrefix($"{Command} ");
                        bool isBlock = commandBlocks.Contains(Command);
                        if (isBlock)
                        {
                            bool satisfied = false;
                            switch (Command)
                            {
                                case "if":
                                case "while":
                                    satisfied = justLint || UESHConditional.ConditionSatisfied(Arguments);
                                    if (Command == "while")
                                    {
                                        if (!whilePlaces.Contains((l, commandStackNum)))
                                            whilePlaces.Add((l, commandStackNum));
                                        retryLoopCondition = true;
                                    }
                                    break;
                                case "until":
                                    satisfied = justLint || !UESHConditional.ConditionSatisfied(Arguments);
                                    if (!whilePlaces.Contains((l, commandStackNum)))
                                        whilePlaces.Add((l, commandStackNum));
                                    retryLoopCondition = true;
                                    break;
                            }
                            if (satisfied)
                            {
                                // New stack required
                                newCommandStackRequired = true;
                                commandStackNum++;
                                continue;
                            }
                            else if (!justLint)
                            {
                                // Skip all the if block until we reach our stack
                                while (true)
                                {
                                    l++;
                                    if (l < FileLines.Length)
                                        Line = FileLines[l];
                                    string blockStackIndicator = new('|', commandStackNum + 1);
                                    if (!Line.StartsWith(blockStackIndicator))
                                    {
                                        int newStackNum = 0;
                                        int charNum = 0;
                                        while (Line[charNum] == '|')
                                        {
                                            newStackNum++;
                                            charNum++;
                                        }
                                        commandStackNum = newStackNum;
                                        break;
                                    }
                                    if (l >= FileLines.Length)
                                        return;
                                }
                                Line = Line[commandStackNum..];
                                retryLoopCondition = false;

                                // Continue, because the script might have the if condition directly after the stack
                                advance = false;
                                continue;
                            }
                        }
                    }

                    // See if the line is a comment or command
                    if (!Line.StartsWith("#") & !Line.StartsWith(" "))
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Line {0} is not a comment.", vars: [Line]);
                        if (!justLint)
                            ShellManager.GetLine(Line);
                    }
                    else
                        // For debugging purposes
                        DebugWriter.WriteDebug(DebugLevel.I, "Line {0} is a comment.", vars: [Line]);

                    // Increment the new line number
                    LineNo++;
                }
                EventsManager.FireEvent(EventType.UESHPostExecute, ScriptPath, ScriptArguments);
            }
            catch (KernelException ex)
            {
                EventsManager.FireEvent(EventType.UESHError, ScriptPath, ScriptArguments, ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to execute script {0} with arguments {1}: {2}", vars: [ScriptPath, ScriptArguments, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.UESHScript, Translate.DoTranslation("The script is malformed. Check the script and resolve any errors.") + "\n{0}", ex, GetLineHandle(ScriptPath, LineNo, 0, KernelColorType.Error));
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.UESHError, ScriptPath, ScriptArguments, ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to execute script {0} with arguments {1}: {2}", vars: [ScriptPath, ScriptArguments, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.UESHScript, Translate.DoTranslation("The script is malformed. Check the script and resolve any errors: {0}") + "\n{1}", ex, ex.Message, GetLineHandle(ScriptPath, LineNo, 0, KernelColorType.Error));
            }
        }

        internal static LineHandle GetLineHandle(string path, int line, int column, KernelColorType colorType)
        {
            var lineHandle = new LineHandle(path)
            {
                Position = line,
                SourcePosition = column,
                Color = KernelColorTools.GetColor(colorType),
            };
            return lineHandle;
        }
    }
}
