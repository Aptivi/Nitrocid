//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nettify.EnglishDictionary;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

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
        public override string Command =>
            "dict";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_DICTIONARY_DICTIONARY");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "word", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_DICTIONARY_WORDTODEFINE"
                    }),
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
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
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DICTIONARY_WORD"), Word.Word, needsIndent: false);

                // Meanings...
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_DICTIONARY_MEAININGS") + $" {parameters.ArgumentsList[0]}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                foreach (DictionaryWord.Meaning MeaningBase in Word.Meanings ?? [])
                {
                    // Base part of speech
                    ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DICTIONARY_PARTOFSPEECH"), MeaningBase.PartOfSpeech);

                    // Get the definitions
                    foreach (DictionaryWord.DefinitionType DefinitionBase in MeaningBase.Definitions ?? [])
                    {
                        // Write definition and, if applicable, example
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DICTIONARY_DEF"), DefinitionBase.Definition, indent: 1);
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DICTIONARY_EXAMPLE"), DefinitionBase.Example, indent: 1);

                        // Now, write the specific synonyms (usually blank)
                        if (DefinitionBase.Synonyms is not null && DefinitionBase.Synonyms.Length != 0)
                            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DICTIONARY_SYNONYMS"), string.Join(", ", DefinitionBase.Synonyms), indent: 2);

                        // ...and the specific antonyms (usually blank)
                        if (DefinitionBase.Antonyms is not null && DefinitionBase.Antonyms.Length != 0)
                            ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DICTIONARY_ANTONYMS"), string.Join(", ", DefinitionBase.Antonyms), indent: 2);
                    }

                    // Now, write the base synonyms (usually blank)
                    if (MeaningBase.Synonyms is not null && MeaningBase.Synonyms.Length != 0)
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DICTIONARY_SYNONYMS"), string.Join(", ", MeaningBase.Synonyms), indent: 1);

                    // ...and the base antonyms (usually blank)
                    if (MeaningBase.Antonyms is not null && MeaningBase.Antonyms.Length != 0)
                        ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_DICTIONARY_SYNONYMS"), string.Join(", ", MeaningBase.Antonyms), indent: 1);
                }

                // Sources...
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_DICTIONARY_SOURCES") + $" {parameters.ArgumentsList[0]}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                ListWriterColor.WriteList(Word.SourceUrls ?? []);
            }
            return 0;
        }

    }
}
