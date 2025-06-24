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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Base.Languages;
using System;
using System.Text;
using VisualCard;
using VisualCard.Parts.Enums;
using Nitrocid.Base.Files;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Nitrocid.Extras.Contacts.Contacts
{
    internal static class ContactsHandler
    {
        public static void Handle(string path)
        {
            if (!FilesystemTools.FileExists(path))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTFILENOTFOUND_NAMED"), new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                }, path);
                return;
            }
            try
            {
                ContactsManager.InstallContacts(path);
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_IMPORTSUCCESS"), new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Success)
                });
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTFILEINVALID_NAMED") + $" {ex.Message}", new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                }, path);
            }
        }

        public static string InfoHandle(string path)
        {
            if (!FilesystemTools.FileExists(path))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTFILENOTFOUND_NAMED"), new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                }, path);
                return LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTFILENOTFOUND_UNNAMED");
            }
            try
            {
                var builder = new StringBuilder();
                var cards = CardTools.GetCards(path);
                builder.AppendLine($"{cards.Length} {LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTSCOUNT")}");
                foreach (var card in cards)
                    builder.AppendLine($"  - {card.GetString(CardStringsEnum.FullName)[0].Value}");
                return builder.ToString();
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTFILEINVALID_NAMED") + $" {ex.Message}", new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                }, path);
                return LanguageTools.GetLocalized("NKS_CONTACTS_CONTACTFILEINVALID_UNNAMED");
            }
        }
    }
}
