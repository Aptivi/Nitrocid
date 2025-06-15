//
// Terminaux  Copyright (C) 2023-2025  Aptivi
//
// This file is part of Terminaux
//
// Terminaux is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Terminaux is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using LocaleStation.Tools;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Localized;

namespace Nitrocid.Languages
{
    internal static class LanguageTools
    {
        private const string localType = "Nitrocid";

        internal static string GetLocalized(string id)
        {
            if (LanguageCommon.Language != LanguageManager.CurrentLanguageInfo.ThreeLetterLanguageName)
                LanguageCommon.Language = LanguageManager.CurrentLanguageInfo.ThreeLetterLanguageName;
            foreach (string type in LanguageCommon.Actions)
            {
                var action = LanguageCommon.GetAction(type);
                if (action.Exists.Invoke(id, LanguageCommon.Language))
                    return GetLocalized(id, type, LanguageCommon.Language);
            }
            return GetLocalized(id, localType, LanguageCommon.Language);
        }

        internal static string GetLocalized(string id, string localType, string language)
        {
            AddCustomAction(localType, new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            var type = LanguageCommon.GetAction(localType);
            if (type.Exists.Invoke(id, language))
                return LanguageCommon.Translate(id, localType, language);
            return id;
        }

        internal static void AddCustomAction(string localType, LanguageLocalActions action)
        {
            if (!LanguageCommon.IsCustomActionDefined(localType))
                LanguageCommon.AddCustomAction(localType, action);
        }

        internal static void RemoveCustomAction(string localType)
        {
            if (LanguageCommon.IsCustomActionDefined(localType))
                LanguageCommon.RemoveCustomAction(localType);
        }
    }
}
