//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nettify.EnglishDictionary;
using Nitrocid.Shell.ShellBase.Commands;
using Terminaux.Writer.FancyWriters;
using Nitrocid.Languages;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.ConsoleBase.Colors;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var Words = DictionaryManager.GetWordInfo(parameters.ArgumentsList[0]);

            // Iterate for each word
            foreach (DictionaryWord Word in Words)
            {
                // First, print the license out
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("License information"), true);
                TextWriterColor.Write("dictionaryapi.dev " + Translate.DoTranslation("API is licensed under") + $" {Word.LicenseInfo.Name}: {Word.LicenseInfo.Url}");

                // Now, we can write the word information
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Word information for") + $" {parameters.ArgumentsList[0]}", true);
                TextWriters.Write(Translate.DoTranslation("Word:"), false, KernelColorType.ListEntry);
                TextWriters.Write($" {Word.Word}", true, KernelColorType.ListValue);

                // Meanings...
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Word meanings for") + $" {parameters.ArgumentsList[0]}", true);
                foreach (DictionaryWord.Meaning MeaningBase in Word.Meanings)
                {
                    // Base part of speech
                    TextWriters.Write(Translate.DoTranslation("Part of Speech:"), false, KernelColorType.ListEntry);
                    TextWriters.Write($" {MeaningBase.PartOfSpeech}", true, KernelColorType.ListValue);

                    // Get the definitions
                    foreach (DictionaryWord.DefinitionType DefinitionBase in MeaningBase.Definitions)
                    {
                        // Write definition and, if applicable, example
                        TextWriters.Write("  - " + Translate.DoTranslation("Definition:"), false, KernelColorType.ListEntry);
                        TextWriters.Write($" {DefinitionBase.Definition}", true, KernelColorType.ListValue);
                        TextWriters.Write("  - " + Translate.DoTranslation("Example in Sentence:"), false, KernelColorType.ListEntry);
                        TextWriters.Write($" {DefinitionBase.Example}", true, KernelColorType.ListValue);

                        // Now, write the specific synonyms (usually blank)
                        if (DefinitionBase.Synonyms.Length != 0)
                        {
                            TextWriters.Write("  - " + Translate.DoTranslation("Synonyms:"), true, KernelColorType.ListEntry);
                            ListWriterColor.WriteList(DefinitionBase.Synonyms);
                        }

                        // ...and the specific antonyms (usually blank)
                        if (DefinitionBase.Antonyms.Length != 0)
                        {
                            TextWriters.Write("  - " + Translate.DoTranslation("Antonyms:"), true, KernelColorType.ListEntry);
                            ListWriterColor.WriteList(DefinitionBase.Antonyms);
                        }
                    }

                    // Now, write the base synonyms (usually blank)
                    if (MeaningBase.Synonyms.Length != 0)
                    {
                        TextWriters.Write("  - " + Translate.DoTranslation("Synonyms:"), true, KernelColorType.ListEntry);
                        ListWriterColor.WriteList(MeaningBase.Synonyms);
                    }

                    // ...and the base antonyms (usually blank)
                    if (MeaningBase.Antonyms.Length != 0)
                    {
                        TextWriters.Write("  - " + Translate.DoTranslation("Antonyms:"), true, KernelColorType.ListEntry);
                        ListWriterColor.WriteList(MeaningBase.Antonyms);
                    }
                }

                // Sources...
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Sources used to define") + $" {parameters.ArgumentsList[0]}", true);
                ListWriterColor.WriteList(Word.SourceUrls);
            }
            return 0;
        }

        public override int ExecuteDumb(CommandParameters parameters, ref string variableValue)
        {
            var Words = DictionaryManager.GetWordInfo(parameters.ArgumentsList[0]);

            // Iterate for each word
            foreach (DictionaryWord Word in Words)
            {
                // First, print the license out
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("License information"), true);
                TextWriterColor.Write("dictionaryapi.dev " + Translate.DoTranslation("API is licensed under") + $" {Word.LicenseInfo.Name}: {Word.LicenseInfo.Url}");

                // Now, we can write the word information
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Word information for") + $" {parameters.ArgumentsList[0]}", true);
                TextWriters.Write(Translate.DoTranslation("Word:"), false, KernelColorType.ListEntry);
                TextWriters.Write($" {Word.Word}", true, KernelColorType.ListValue);

                // Meanings...
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Word meanings for") + $" {parameters.ArgumentsList[0]}", true);
                foreach (DictionaryWord.Meaning MeaningBase in Word.Meanings)
                {
                    // Base part of speech
                    TextWriters.Write(Translate.DoTranslation("Part of Speech:"), false, KernelColorType.ListEntry);
                    TextWriters.Write($" {MeaningBase.PartOfSpeech}", true, KernelColorType.ListValue);

                    // Get the definitions
                    foreach (DictionaryWord.DefinitionType DefinitionBase in MeaningBase.Definitions)
                    {
                        // Write definition and, if applicable, example
                        TextWriters.Write("  - " + Translate.DoTranslation("Definition:"), false, KernelColorType.ListEntry);
                        TextWriters.Write($" {DefinitionBase.Definition}", true, KernelColorType.ListValue);
                        TextWriters.Write("  - " + Translate.DoTranslation("Example in Sentence:"), false, KernelColorType.ListEntry);
                        TextWriters.Write($" {DefinitionBase.Example}", true, KernelColorType.ListValue);

                        // Now, write the specific synonyms (usually blank)
                        if (DefinitionBase.Synonyms.Length != 0)
                        {
                            TextWriters.Write("  - " + Translate.DoTranslation("Synonyms:"), true, KernelColorType.ListEntry);
                            foreach (string synonym in DefinitionBase.Synonyms)
                                TextWriterColor.Write($"    {synonym}");
                        }

                        // ...and the specific antonyms (usually blank)
                        if (DefinitionBase.Antonyms.Length != 0)
                        {
                            TextWriters.Write("  - " + Translate.DoTranslation("Antonyms:"), true, KernelColorType.ListEntry);
                            foreach (string antonym in DefinitionBase.Antonyms)
                                TextWriterColor.Write($"    {antonym}");
                        }
                    }

                    // Now, write the base synonyms (usually blank)
                    if (MeaningBase.Synonyms.Length != 0)
                    {
                        TextWriters.Write("  - " + Translate.DoTranslation("Synonyms:"), true, KernelColorType.ListEntry);
                        foreach (string synonym in MeaningBase.Synonyms)
                            TextWriterColor.Write($"    {synonym}");
                    }

                    // ...and the base antonyms (usually blank)
                    if (MeaningBase.Antonyms.Length != 0)
                    {
                        TextWriters.Write("  - " + Translate.DoTranslation("Antonyms:"), true, KernelColorType.ListEntry);
                        foreach (string antonym in MeaningBase.Antonyms)
                            TextWriterColor.Write($"    {antonym}");
                    }
                }

                // Sources...
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Sources used to define") + $" {parameters.ArgumentsList[0]}", true);
                foreach (string source in Word.SourceUrls)
                    TextWriterColor.Write(source);
            }
            return 0;
        }

    }
}
