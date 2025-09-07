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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Nitrocid.Files;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Textify.Tools.Placeholder;

namespace Nitrocid.Extras.Mods.Modifications.ManPages
{
    static class PageParser
    {

        /// <summary>
        /// Initializes a manual page
        /// </summary>
        /// <param name="modName">Kernel modification name</param>
        /// <param name="ManualFile">A manual file path (neutralized)</param>
        public static void InitMan(string modName, string ManualFile)
        {
            ManualFile = FilesystemTools.NeutralizePath(ManualFile);
            if (!ModManager.Mods.ContainsKey(modName))
                throw new KernelException(KernelExceptionType.ModManual, Translate.DoTranslation("Tried to initialize the manual file {0} for nonexistent mod {1}."), ManualFile, modName);
            if (!FilesystemTools.FileExists(ManualFile))
                throw new KernelException(KernelExceptionType.ModManual, Translate.DoTranslation("Tried to initialize the manual file {0} which doesn't exist for mod {1}."), ManualFile, modName);

            // File found, but we need to verify that we're actually dealing with the manual page. If not, ignore it.
            if (Path.GetExtension(ManualFile) != ".man")
                return;

            // We found the manual, but we need to check its contents.
            DebugWriter.WriteDebug(DebugLevel.I, "Found manual page {0}. Parsing manpage...", vars: [ManualFile]);
            var ManualInstance = new Manual(modName, ManualFile);
            PageManager.AddManualPage(modName, ManualInstance.Title, ManualInstance);
        }

        /// <summary>
        /// Checks to see if the manual page is valid
        /// </summary>
        /// <param name="ManualFile">A manual file path (neutralized)</param>
        /// <param name="ManTitle">Manual title to install to the new manual class instance</param>
        /// <param name="ManRevision">Manual revision to install to the new manual class instance</param>
        /// <param name="Body">Body to install to the new manual class instance</param>
        /// <param name="Todos">Todo list to install to the new manual class instance</param>
        internal static bool CheckManual(string ManualFile, ref string ManTitle, ref string ManRevision, ref StringBuilder Body, ref List<string> Todos)
        {
            bool Success = true;
            try
            {
                bool InternalParseDone = false;
                ManualFile = FilesystemTools.NeutralizePath(ManualFile);
                DebugWriter.WriteDebug(DebugLevel.I, "Current manual file: {0}", vars: [ManualFile]);

                // First, get all lines in the file
                var ManLines = FilesystemTools.ReadContents(ManualFile);
                var BodyParsing = false;
                foreach (string ManLine in ManLines)
                {
                    // Check for the rest if the manpage has MAN START section
                    if (InternalParseDone)
                    {
                        // Check for the TODOs
                        string TodoConstant = "TODO";
                        if (ManLine.StartsWith("~~-") & ManLine.Contains(TodoConstant))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "TODO found on this line: {0}", vars: [ManLine]);
                            Todos.Add(ManLine);
                        }

                        // Check the manual metadata
                        string RevisionConstant = "-REVISION:";
                        string TitleConstant = "-TITLE:";
                        string BodyStartConstant = "-BODY START-";
                        string BodyEndConstant = "-BODY END-";

                        // Check the body or manual metadata
                        if (!ManLine.StartsWith("~~-"))
                        {
                            if (BodyParsing)
                            {
                                // If we're not at the end of the body
                                if (ManLine != BodyEndConstant)
                                {
                                    if (!string.IsNullOrWhiteSpace(ManLine))
                                        DebugWriter.WriteDebug(DebugLevel.I, "Appending {0} to builder", vars: [ManLine]);
                                    Body.AppendLine(PlaceParse.ProbePlaces(ManLine));
                                }
                                else
                                {
                                    // Stop parsing the body
                                    BodyParsing = false;
                                }
                            }
                            // Check for constants
                            else if (ManLine.StartsWith(RevisionConstant))
                            {
                                // Found the revision constant
                                DebugWriter.WriteDebug(DebugLevel.I, "Revision found on this line: {0}", vars: [ManLine]);
                                string Rev = ManLine[RevisionConstant.Length..];
                                if (string.IsNullOrWhiteSpace(Rev))
                                {
                                    DebugWriter.WriteDebug(DebugLevel.W, "Revision not defined. Assuming v1...");
                                    Rev = "1";
                                }
                                ManRevision = Rev;
                            }
                            else if (ManLine.StartsWith(TitleConstant))
                            {
                                // Found the title constant
                                DebugWriter.WriteDebug(DebugLevel.I, "Title found on this line: {0}", vars: [ManLine]);
                                string Title = ManLine[TitleConstant.Length..];
                                if (string.IsNullOrWhiteSpace(Title))
                                {
                                    DebugWriter.WriteDebug(DebugLevel.W, "Title not defined.");
                                    Title = $"Untitled ({PageManager.Pages.Count})";
                                }
                                ManTitle = Title;
                            }
                            else if (ManLine == BodyStartConstant)
                            {
                                BodyParsing = true;
                            }
                        }
                    }

                    // Check to see if the manual starts with (*MAN START*) header
                    if (ManLine == "(*MAN START*)")
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Successfully found (*MAN START*) in manpage {0}.", vars: [ManualFile]);
                        InternalParseDone = true;
                    }
                }

                // Check for body
                if (InternalParseDone)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Valid manual page! ({0})", vars: [ManualFile]);
                    if (string.IsNullOrWhiteSpace(Body.ToString()))
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Body for \"{0}\" does not contain anything.", vars: [ManualFile]);
                        Body.AppendLine(Translate.DoTranslation("Consider filling this manual page."));
                    }
                }
                else
                {
                    throw new KernelException(KernelExceptionType.InvalidManpage, Translate.DoTranslation("The manual page {0} is invalid."), ManualFile);
                }
            }
            catch (Exception ex)
            {
                Success = false;
                DebugWriter.WriteDebug(DebugLevel.E, "The manual page {0} is invalid. {1}", vars: [ManTitle, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return Success;
        }

    }
}
