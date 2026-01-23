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

using Nettify.EnglishDictionary;
using Terminaux.Shell.Commands;
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
                if (Word.LicenseInfo is not null)
                {
                    SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("License information"), KernelColorTools.GetColor(KernelColorType.ListTitle));
                    TextWriterColor.Write("dictionaryapi.dev " + Translate.DoTranslation("API is licensed under") + $" {Word.LicenseInfo.Name}: {Word.LicenseInfo.Url}");
                }

                // Now, we can write the word information
                SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Word information for") + $" {parameters.ArgumentsList[0]}", KernelColorTools.GetColor(KernelColorType.ListTitle));
                TextWriters.Write(Translate.DoTranslation("Word:"), false, KernelColorType.ListEntry);
                TextWriters.Write($" {Word.Word}", true, KernelColorType.ListValue);

                // Meanings...
                SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Word meanings for") + $" {parameters.ArgumentsList[0]}", KernelColorTools.GetColor(KernelColorType.ListTitle));
                foreach (DictionaryWord.Meaning MeaningBase in Word.Meanings ?? [])
                {
                    // Base part of speech
                    TextWriters.Write(Translate.DoTranslation("Part of Speech:"), false, KernelColorType.ListEntry);
                    TextWriters.Write($" {MeaningBase.PartOfSpeech}", true, KernelColorType.ListValue);

                    // Get the definitions
                    foreach (DictionaryWord.DefinitionType DefinitionBase in MeaningBase.Definitions ?? [])
                    {
                        // Write definition and, if applicable, example
                        TextWriters.Write("  - " + Translate.DoTranslation("Definition:"), false, KernelColorType.ListEntry);
                        TextWriters.Write($" {DefinitionBase.Definition}", true, KernelColorType.ListValue);
                        TextWriters.Write("  - " + Translate.DoTranslation("Example in Sentence:"), false, KernelColorType.ListEntry);
                        TextWriters.Write($" {DefinitionBase.Example}", true, KernelColorType.ListValue);

                        // Now, write the specific synonyms (usually blank)
                        if (DefinitionBase.Synonyms is not null && DefinitionBase.Synonyms.Length != 0)
                        {
                            TextWriters.Write("  - " + Translate.DoTranslation("Synonyms:"), true, KernelColorType.ListEntry);
                            TextWriters.WriteList(DefinitionBase.Synonyms);
                        }

                        // ...and the specific antonyms (usually blank)
                        if (DefinitionBase.Antonyms is not null && DefinitionBase.Antonyms.Length != 0)
                        {
                            TextWriters.Write("  - " + Translate.DoTranslation("Antonyms:"), true, KernelColorType.ListEntry);
                            TextWriters.WriteList(DefinitionBase.Antonyms);
                        }
                    }

                    // Now, write the base synonyms (usually blank)
                    if (MeaningBase.Synonyms is not null && MeaningBase.Synonyms.Length != 0)
                    {
                        TextWriters.Write("  - " + Translate.DoTranslation("Synonyms:"), true, KernelColorType.ListEntry);
                        TextWriters.WriteList(MeaningBase.Synonyms);
                    }

                    // ...and the base antonyms (usually blank)
                    if (MeaningBase.Antonyms is not null && MeaningBase.Antonyms.Length != 0)
                    {
                        TextWriters.Write("  - " + Translate.DoTranslation("Antonyms:"), true, KernelColorType.ListEntry);
                        TextWriters.WriteList(MeaningBase.Antonyms);
                    }
                }

                // Sources...
                SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Sources used to define") + $" {parameters.ArgumentsList[0]}", KernelColorTools.GetColor(KernelColorType.ListTitle));
                TextWriters.WriteList(Word.SourceUrls ?? []);
            }
            return 0;
        }

    }
}
