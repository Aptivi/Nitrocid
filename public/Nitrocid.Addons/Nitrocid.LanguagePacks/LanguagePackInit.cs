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

using Newtonsoft.Json;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Extensions;
using Nitrocid.LanguagePacks.Resources;
using Nitrocid.Languages;
using Nitrocid.Languages.Decoy;
using Nitrocid.Misc.Reflection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Nitrocid.LanguagePacks
{
    internal class LanguagePackInit : IAddon
    {
        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.AddonLanguagePacks);

        AddonType IAddon.AddonType => AddonType.Important;

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.StartAddon()
        {
            // Add them all!
            string[] languageResNames = GetLanguageResourceNames();
            var languageMetadataToken = JsonConvert.DeserializeObject<LanguageMetadata[]>(LanguageResources.LanguageMetadata);
            for (int i = 0; i < languageResNames.Length; i++)
            {
                var metadata = languageMetadataToken[i];
                string key = languageResNames[i].Replace("_", "-");
                var languageToken = JsonConvert.DeserializeObject<LanguageLocalizations>(LanguageResources.ResourceManager.GetString(languageResNames[i]));
                LanguageManager.AddBaseLanguage(metadata, true, languageToken.Localizations);
                DebugWriter.WriteDebug(DebugLevel.I, "Added {0}", key);
            }
        }

        void IAddon.StopAddon()
        {
            // Remove them all!
            string[] languageResNames = GetLanguageResourceNames();
            foreach (string key in languageResNames)
            {
                string keyNormalized = key.Replace("_", "-");
                bool result = LanguageManager.BaseLanguages.Remove(keyNormalized);
                DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}: {1}", keyNormalized, result);
            }
        }

        void IAddon.FinalizeAddon()
        { }

        private string[] GetLanguageResourceNames()
        {
            // Get all the languages provided by this pack
            string[] nonLanguages =
            [
                nameof(LanguageResources.Culture),
                nameof(LanguageResources.ResourceManager),
                nameof(LanguageResources.LanguageMetadata),
            ];
            return PropertyManager.GetPropertiesNoEvaluation(typeof(LanguageResources)).Keys.Except(nonLanguages).ToArray();
        }
    }
}
