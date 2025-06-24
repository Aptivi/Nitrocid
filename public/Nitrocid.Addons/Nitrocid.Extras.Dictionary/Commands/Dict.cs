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

using Nettify.EnglishDictionary;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Languages;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;

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
                    SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_DICTIONARY_LICENSEINFO"), ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                    TextWriterColor.Write("dictionaryapi.dev " + LanguageTools.GetLocalized("NKS_DICTIONARY_APILICENSE") + $" {Word.LicenseInfo.Name}: {Word.LicenseInfo.Url}");
                }

                // Now, we can write the word information
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_DICTIONARY_WORDINFO") + $" {parameters.ArgumentsList[0]}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DICTIONARY_WORD"), false, ThemeColorType.ListEntry);
                TextWriterColor.Write($" {Word.Word}", true, ThemeColorType.ListValue);

                // Meanings...
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_DICTIONARY_MEAININGS") + $" {parameters.ArgumentsList[0]}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                foreach (DictionaryWord.Meaning MeaningBase in Word.Meanings ?? [])
                {
                    // Base part of speech
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DICTIONARY_PARTOFSPEECH"), false, ThemeColorType.ListEntry);
                    TextWriterColor.Write($" {MeaningBase.PartOfSpeech}", true, ThemeColorType.ListValue);

                    // Get the definitions
                    foreach (DictionaryWord.DefinitionType DefinitionBase in MeaningBase.Definitions ?? [])
                    {
                        // Write definition and, if applicable, example
                        TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_DICTIONARY_DEF"), false, ThemeColorType.ListEntry);
                        TextWriterColor.Write($" {DefinitionBase.Definition}", true, ThemeColorType.ListValue);
                        TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_DICTIONARY_EXAMPLE"), false, ThemeColorType.ListEntry);
                        TextWriterColor.Write($" {DefinitionBase.Example}", true, ThemeColorType.ListValue);

                        // Now, write the specific synonyms (usually blank)
                        if (DefinitionBase.Synonyms is not null && DefinitionBase.Synonyms.Length != 0)
                        {
                            TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_DICTIONARY_SYNONYMS"), true, ThemeColorType.ListEntry);
                            ListWriterColor.WriteList(DefinitionBase.Synonyms);
                        }

                        // ...and the specific antonyms (usually blank)
                        if (DefinitionBase.Antonyms is not null && DefinitionBase.Antonyms.Length != 0)
                        {
                            TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_DICTIONARY_ANTONYMS"), true, ThemeColorType.ListEntry);
                            ListWriterColor.WriteList(DefinitionBase.Antonyms);
                        }
                    }

                    // Now, write the base synonyms (usually blank)
                    if (MeaningBase.Synonyms is not null && MeaningBase.Synonyms.Length != 0)
                    {
                        TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_DICTIONARY_SYNONYMS"), true, ThemeColorType.ListEntry);
                        ListWriterColor.WriteList(MeaningBase.Synonyms);
                    }

                    // ...and the base antonyms (usually blank)
                    if (MeaningBase.Antonyms is not null && MeaningBase.Antonyms.Length != 0)
                    {
                        TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_DICTIONARY_ANTONYMS"), true, ThemeColorType.ListEntry);
                        ListWriterColor.WriteList(MeaningBase.Antonyms);
                    }
                }

                // Sources...
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_DICTIONARY_SOURCES") + $" {parameters.ArgumentsList[0]}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                ListWriterColor.WriteList(Word.SourceUrls ?? []);
            }
            return 0;
        }

    }
}
