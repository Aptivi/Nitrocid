
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

using System.Linq;
using static Dictify.Manager.DictionaryManager;
using Dictify.Models;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;

namespace Nitrocid.Extras.Dictionary.Commands
{
    /// <summary>
    /// The English Dictionary
    /// </summary>
    /// <remarks>
    /// If you want to define a specific English word, you can use this command.
    /// </remarks>
    class DictCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            var Words = GetWordInfo(ListArgsOnly[0]);

            // Iterate for each word
            foreach (DictionaryWord Word in Words)
            {
                // First, print the license out
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("License information"), true);
                TextWriterColor.Write("dictionaryapi.dev " + Translate.DoTranslation("API is licensed under") + $" {Word.LicenseInfo.Name}: {Word.LicenseInfo.Url}");

                // Now, we can write the word information
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Word information for") + $" {ListArgsOnly[0]}", true);
                TextWriterColor.Write(Translate.DoTranslation("Word:"), false, KernelColorType.ListEntry);
                TextWriterColor.Write($" {Word.Word}", true, KernelColorType.ListValue);

                // Meanings...
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Word meanings for") + $" {ListArgsOnly[0]}", true);
                foreach (DictionaryWord.Meaning MeaningBase in Word.Meanings)
                {
                    // Base part of speech
                    TextWriterColor.Write(Translate.DoTranslation("Part of Speech:"), false, KernelColorType.ListEntry);
                    TextWriterColor.Write($" {MeaningBase.PartOfSpeech}", true, KernelColorType.ListValue);

                    // Get the definitions
                    foreach (DictionaryWord.DefinitionType DefinitionBase in MeaningBase.Definitions)
                    {
                        // Write definition and, if applicable, example
                        TextWriterColor.Write("  - " + Translate.DoTranslation("Definition:"), false, KernelColorType.ListEntry);
                        TextWriterColor.Write($" {DefinitionBase.Definition}", true, KernelColorType.ListValue);
                        TextWriterColor.Write("  - " + Translate.DoTranslation("Example in Sentence:"), false, KernelColorType.ListEntry);
                        TextWriterColor.Write($" {DefinitionBase.Example}", true, KernelColorType.ListValue);

                        // Now, write the specific synonyms (usually blank)
                        if (DefinitionBase.Synonyms.Any())
                        {
                            TextWriterColor.Write("  - " + Translate.DoTranslation("Synonyms:"), true, KernelColorType.ListEntry);
                            ListWriterColor.WriteList(DefinitionBase.Synonyms);
                        }

                        // ...and the specific antonyms (usually blank)
                        if (DefinitionBase.Antonyms.Any())
                        {
                            TextWriterColor.Write("  - " + Translate.DoTranslation("Antonyms:"), true, KernelColorType.ListEntry);
                            ListWriterColor.WriteList(DefinitionBase.Antonyms);
                        }
                    }

                    // Now, write the base synonyms (usually blank)
                    if (MeaningBase.Synonyms.Any())
                    {
                        TextWriterColor.Write("  - " + Translate.DoTranslation("Synonyms:"), true, KernelColorType.ListEntry);
                        ListWriterColor.WriteList(MeaningBase.Synonyms);
                    }

                    // ...and the base antonyms (usually blank)
                    if (MeaningBase.Antonyms.Any())
                    {
                        TextWriterColor.Write("  - " + Translate.DoTranslation("Antonyms:"), true, KernelColorType.ListEntry);
                        ListWriterColor.WriteList(MeaningBase.Antonyms);
                    }
                }

                // Sources...
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Sources used to define") + $" {ListArgsOnly[0]}", true);
                ListWriterColor.WriteList(Word.SourceUrls);
            }
            return 0;
        }

        public override int ExecuteDumb(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            var Words = GetWordInfo(ListArgsOnly[0]);

            // Iterate for each word
            foreach (DictionaryWord Word in Words)
            {
                // First, print the license out
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("License information"), true);
                TextWriterColor.Write("dictionaryapi.dev " + Translate.DoTranslation("API is licensed under") + $" {Word.LicenseInfo.Name}: {Word.LicenseInfo.Url}");

                // Now, we can write the word information
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Word information for") + $" {ListArgsOnly[0]}", true);
                TextWriterColor.Write(Translate.DoTranslation("Word:"), false, KernelColorType.ListEntry);
                TextWriterColor.Write($" {Word.Word}", true, KernelColorType.ListValue);

                // Meanings...
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Word meanings for") + $" {ListArgsOnly[0]}", true);
                foreach (DictionaryWord.Meaning MeaningBase in Word.Meanings)
                {
                    // Base part of speech
                    TextWriterColor.Write(Translate.DoTranslation("Part of Speech:"), false, KernelColorType.ListEntry);
                    TextWriterColor.Write($" {MeaningBase.PartOfSpeech}", true, KernelColorType.ListValue);

                    // Get the definitions
                    foreach (DictionaryWord.DefinitionType DefinitionBase in MeaningBase.Definitions)
                    {
                        // Write definition and, if applicable, example
                        TextWriterColor.Write("  - " + Translate.DoTranslation("Definition:"), false, KernelColorType.ListEntry);
                        TextWriterColor.Write($" {DefinitionBase.Definition}", true, KernelColorType.ListValue);
                        TextWriterColor.Write("  - " + Translate.DoTranslation("Example in Sentence:"), false, KernelColorType.ListEntry);
                        TextWriterColor.Write($" {DefinitionBase.Example}", true, KernelColorType.ListValue);

                        // Now, write the specific synonyms (usually blank)
                        if (DefinitionBase.Synonyms.Any())
                        {
                            TextWriterColor.Write("  - " + Translate.DoTranslation("Synonyms:"), true, KernelColorType.ListEntry);
                            foreach (string synonym in DefinitionBase.Synonyms)
                                TextWriterColor.Write($"    {synonym}");
                        }

                        // ...and the specific antonyms (usually blank)
                        if (DefinitionBase.Antonyms.Any())
                        {
                            TextWriterColor.Write("  - " + Translate.DoTranslation("Antonyms:"), true, KernelColorType.ListEntry);
                            foreach (string antonym in DefinitionBase.Antonyms)
                                TextWriterColor.Write($"    {antonym}");
                        }
                    }

                    // Now, write the base synonyms (usually blank)
                    if (MeaningBase.Synonyms.Any())
                    {
                        TextWriterColor.Write("  - " + Translate.DoTranslation("Synonyms:"), true, KernelColorType.ListEntry);
                        foreach (string synonym in MeaningBase.Synonyms)
                            TextWriterColor.Write($"    {synonym}");
                    }

                    // ...and the base antonyms (usually blank)
                    if (MeaningBase.Antonyms.Any())
                    {
                        TextWriterColor.Write("  - " + Translate.DoTranslation("Antonyms:"), true, KernelColorType.ListEntry);
                        foreach (string antonym in MeaningBase.Antonyms)
                            TextWriterColor.Write($"    {antonym}");
                    }
                }

                // Sources...
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Sources used to define") + $" {ListArgsOnly[0]}", true);
                foreach (string source in Word.SourceUrls)
                    TextWriterColor.Write(source);
            }
            return 0;
        }

    }
}
