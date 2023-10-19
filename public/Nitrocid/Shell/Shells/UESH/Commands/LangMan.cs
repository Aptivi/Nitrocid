
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Files;
using KS.Files.Operations.Querying;
using KS.Kernel;
using KS.Kernel.Configuration;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Help;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Manages your custom languages
    /// </summary>
    /// <remarks>
    /// You can manage all your custom languages installed in Nitrocid KS by this command.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class LangManCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (!KernelEntry.SafeMode)
            {
                string CommandMode = parameters.ArgumentsList[0].ToLower();
                string TargetLanguage = "";
                string TargetLanguagePath = "";
                string LanguageListTerm = "";

                // These command modes require two arguments to be passed, so re-check here and there. Optional arguments also lie there.
                switch (CommandMode)
                {
                    case "reload":
                    case "load":
                    case "unload":
                        {
                            if (parameters.ArgumentsList.Length > 1)
                            {
                                TargetLanguage = parameters.ArgumentsList[1];
                                TargetLanguagePath = Filesystem.NeutralizePath(TargetLanguage + ".json", Paths.GetKernelPath(KernelPathType.CustomLanguages));
                                if (!(Parsing.TryParsePath(TargetLanguagePath) && Checking.FileExists(TargetLanguagePath)) & !LanguageManager.Languages.ContainsKey(TargetLanguage))
                                {
                                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Language not found or file has invalid characters."), true, KernelColorType.Error);
                                    return 10000 + (int)KernelExceptionType.NoSuchLanguage;
                                }
                            }
                            else
                            {
                                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Language is not specified."), true, KernelColorType.Error);
                                return 10000 + (int)KernelExceptionType.NoSuchLanguage;
                            }

                            break;
                        }
                    case "list":
                        {
                            if (parameters.ArgumentsList.Length > 1)
                            {
                                LanguageListTerm = parameters.ArgumentsList[1];
                            }

                            break;
                        }
                }

                // Now, the actual logic
                switch (CommandMode)
                {
                    case "reload":
                        {
                            LanguageManager.UninstallCustomLanguage(TargetLanguagePath);
                            LanguageManager.InstallCustomLanguage(TargetLanguagePath);
                            break;
                        }
                    case "load":
                        {
                            LanguageManager.InstallCustomLanguage(TargetLanguage);
                            break;
                        }
                    case "unload":
                        {
                            LanguageManager.UninstallCustomLanguage(TargetLanguage);
                            break;
                        }
                    case "list":
                        {
                            foreach (string Language in LanguageManager.ListLanguages(LanguageListTerm).Keys)
                            {
                                SeparatorWriterColor.WriteSeparator(Language, true);
                                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Language short name:") + " ", false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor(LanguageManager.Languages[Language].ThreeLetterLanguageName, true, KernelColorType.ListValue);
                                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Language full name:") + " ", false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor(LanguageManager.Languages[Language].FullLanguageName, true, KernelColorType.ListValue);
                                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Language transliterable:") + " ", false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor($"{LanguageManager.Languages[Language].Transliterable}", true, KernelColorType.ListValue);
                                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Custom language:") + " ", false, KernelColorType.ListEntry);
                                TextWriterColor.WriteKernelColor($"{LanguageManager.Languages[Language].Custom}", true, KernelColorType.ListValue);
                            }

                            break;
                        }
                    case "reloadall":
                        {
                            LanguageManager.UninstallCustomLanguages();
                            LanguageManager.InstallCustomLanguages();
                            break;
                        }
                    default:
                        {
                            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Invalid command {0}. Check the usage below:"), true, KernelColorType.Error, CommandMode);
                            HelpPrint.ShowHelp("langman");
                            return 10000 + (int)KernelExceptionType.LanguageManagement;
                        }
                }
            }
            else
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Language management is disabled in safe mode."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.LanguageManagement;
            }
            return 0;
        }

    }
}
