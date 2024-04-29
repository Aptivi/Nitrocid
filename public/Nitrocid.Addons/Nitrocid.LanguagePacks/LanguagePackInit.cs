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
using Nitrocid.Languages;
using Nitrocid.Languages.Decoy;
using Nitrocid.Misc.Reflection.Internal;
using Nitrocid.Modifications;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Textify.General;

namespace Nitrocid.LanguagePacks
{
    internal class LanguagePackInit : IAddon
    {
        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.AddonLanguagePacks);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Important;

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.StartAddon()
        {
            // Add them all!
            string[] languageResNames = ResourcesManager.GetResourceNames(typeof(LanguagePackInit).Assembly).Except(["Languages.Metadata.json"]).ToArray();
            var languageMetadataToken = JsonConvert.DeserializeObject<LanguageMetadata[]>(ResourcesManager.GetData("Metadata.json", ResourcesType.Languages, typeof(LanguagePackInit).Assembly));
            for (int i = 0; i < languageResNames.Length; i++)
            {
                var metadata = languageMetadataToken[i];
                string key = languageResNames[i].RemovePrefix("Languages.");
                var languageToken = JsonConvert.DeserializeObject<LanguageLocalizations>(ResourcesManager.GetData(key, ResourcesType.Languages, typeof(LanguagePackInit).Assembly));
                LanguageManager.AddBaseLanguage(metadata, true, languageToken.Localizations);
                DebugWriter.WriteDebug(DebugLevel.I, "Added {0}", key);
            }
        }

        void IAddon.StopAddon()
        {
            // Remove them all!
            string[] languageResNames = ResourcesManager.GetResourceNames(typeof(LanguagePackInit).Assembly).Except(["Languages.Metadata.json"]).ToArray();
            foreach (string resource in languageResNames)
            {
                string key = resource.RemovePrefix("Languages.");
                bool result = LanguageManager.BaseLanguages.Remove(key);
                DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}: {1}", key, result);
            }
        }

        void IAddon.FinalizeAddon()
        { }
    }
}
