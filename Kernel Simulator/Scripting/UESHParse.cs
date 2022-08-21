using System;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.IO;
using Extensification.StringExts;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;

namespace KS.Scripting
{
    public static class UESHParse
    {

        /// <summary>
        /// Executes the UESH script
        /// </summary>
        /// <param name="ScriptPath">Full path to script</param>
        /// <param name="ScriptArguments">Script arguments</param>
        public static void Execute(string ScriptPath, string ScriptArguments)
        {
            try
            {
                // Raise event
                Kernel.Kernel.KernelEventManager.RaiseUESHPreExecute(ScriptPath, ScriptArguments);

                // Open the script file for reading
                var FileStream = new StreamReader(ScriptPath);
                int LineNo = 1;
                DebugWriter.Wdbg(DebugLevel.I, "Stream opened. Parsing script");

                // Look for $variables and initialize them
                while (!FileStream.EndOfStream)
                {
                    // Get line
                    string Line = FileStream.ReadLine();
                    DebugWriter.Wdbg(DebugLevel.I, "Line {0}: \"{1}\"", LineNo, Line);

                    // If $variable is found in string, initialize it
                    var SplitWords = Line.Split(' ');
                    for (int i = 0, loopTo = SplitWords.Length - 1; i <= loopTo; i++)
                    {
                        if (!UESHVariables.ShellVariables.ContainsKey(SplitWords[i]) & SplitWords[i].StartsWith("$"))
                        {
                            UESHVariables.InitializeVariable(SplitWords[i]);
                        }
                    }
                }

                // Seek to the beginning
                FileStream.BaseStream.Seek(0L, SeekOrigin.Begin);

                // Get all lines and parse comments, commands, and arguments
                while (!FileStream.EndOfStream)
                {
                    // Get line
                    string Line = FileStream.ReadLine();
                    DebugWriter.Wdbg(DebugLevel.I, "Line {0}: \"{1}\"", LineNo, Line);

                    // See if the line contains variable, and replace every instance of it with its value
                    var SplitWords = Line.SplitEncloseDoubleQuotes(" ");
                    if (SplitWords is not null)
                    {
                        for (int i = 0, loopTo1 = SplitWords.Length - 1; i <= loopTo1; i++)
                        {
                            if (SplitWords[i].StartsWith("$"))
                            {
                                Line = UESHVariables.GetVariableCommand(SplitWords[i], Line);
                            }
                        }
                    }

                    // See if the line contains argument placeholder, and replace every instance of it with its value
                    var SplitArguments = ScriptArguments.SplitEncloseDoubleQuotes(" ");
                    if (SplitArguments is not null)
                    {
                        for (int i = 0, loopTo2 = SplitWords.Length - 1; i <= loopTo2; i++)
                        {
                            for (int j = 0, loopTo3 = SplitArguments.Length - 1; j <= loopTo3; j++)
                            {
                                if ((SplitWords[i] ?? "") == ($"{{{j}}}" ?? ""))
                                {
                                    Line = Line.Replace(SplitWords[i], SplitArguments[j]);
                                }
                            }
                        }
                    }

                    // See if the line is a comment or command
                    if (!Line.StartsWith("#") & !Line.StartsWith(" "))
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Line {0} is not a comment.", Line);
                        Shell.Shell.GetLine(Line);
                    }
                    else // For debugging purposes
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Line {0} is a comment.", Line);
                    }
                }

                // Close the stream
                FileStream.Close();
                Kernel.Kernel.KernelEventManager.RaiseUESHPostExecute(ScriptPath, ScriptArguments);
            }
            catch (Exception ex)
            {
                Kernel.Kernel.KernelEventManager.RaiseUESHError(ScriptPath, ScriptArguments, ex);
                DebugWriter.Wdbg(DebugLevel.E, "Error trying to execute script {0} with arguments {1}: {2}", ScriptPath, ScriptArguments, ex.Message);
                DebugWriter.WStkTrc(ex);
                throw new Kernel.Exceptions.UESHScriptException(Translate.DoTranslation("The script is malformed. Check the script and resolve any errors: {0}"), ex, ex.Message);
            }
        }

    }
}