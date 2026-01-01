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

using Nitrocid.ConsoleBase.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Languages;
using System;
using System.Text;
using VisualCard;
using VisualCard.Parts.Enums;

namespace Nitrocid.Extras.Contacts.Contacts
{
    internal static class ContactsHandler
    {
        public static void Handle(string path)
        {
            if (!Checking.FileExists(path))
            {
                InfoBoxModalColor.WriteInfoBoxModalColor(Translate.DoTranslation("Can't open file '{0}' because it's not found."), KernelColorTools.GetColor(KernelColorType.Error), path);
                return;
            }
            try
            {
                ContactsManager.InstallContacts(path);
                InfoBoxModalColor.WriteInfoBoxModalColor(Translate.DoTranslation("Imported contacts successfully."), KernelColorTools.GetColor(KernelColorType.Success));
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModalColor(Translate.DoTranslation("Can't open file '{0}' because the card file is invalid.") + $" {ex.Message}", KernelColorTools.GetColor(KernelColorType.Error), path);
            }
        }

        public static string InfoHandle(string path)
        {
            if (!Checking.FileExists(path))
            {
                InfoBoxModalColor.WriteInfoBoxModalColor(Translate.DoTranslation("Can't open file '{0}' because it's not found."), KernelColorTools.GetColor(KernelColorType.Error), path);
                return Translate.DoTranslation("Contact file doesn't exist.");
            }
            try
            {
                var builder = new StringBuilder();
                var cards = CardTools.GetCards(path);
                builder.AppendLine($"{cards.Length} {Translate.DoTranslation("contacts found")}");
                foreach (var card in cards)
                    builder.AppendLine($"  - {card.GetString(CardStringsEnum.FullName)[0].Value}");
                return builder.ToString();
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModalColor(Translate.DoTranslation("Can't open file '{0}' because the card file is invalid.") + $" {ex.Message}", KernelColorTools.GetColor(KernelColorType.Error), path);
                return Translate.DoTranslation("Contact file is invalid.");
            }
        }
    }
}
