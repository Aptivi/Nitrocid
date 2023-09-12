
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

using KS.Kernel.Debugging;
using KS.Kernel.Extensions;
using KS.Languages;
using KS.Misc.Reflection;
using Newtonsoft.Json.Linq;
using Nitrocid.LanguagePacks.Resources;
using System.Linq;

namespace Nitrocid.LanguagePacks
{
    internal class LanguagePackInit : IAddon
    {
        string IAddon.AddonName => "Extra Languages Pack";

        AddonType IAddon.AddonType => AddonType.Important;

        void IAddon.StartAddon()
        {
            // Add them all!
            string[] languageResNames = GetLanguageResourceNames();
            foreach (string key in languageResNames)
            {
                string keyNormalized = key.Replace("_", "-");
                var languageMetadataToken = JToken.Parse(LanguageResources.LanguageMetadata);
                var languageToken = JToken.Parse(LanguageResources.ResourceManager.GetString(key));
                LanguageManager.AddBaseLanguage(languageMetadataToken[keyNormalized].Parent, true, (JObject)languageToken);
                DebugWriter.WriteDebug(DebugLevel.I, "Added {0}", keyNormalized);
            }
        }

        void IAddon.StopAddon()
        {
            // Remove them all!
            string[] languageResNames = GetLanguageResourceNames();
            foreach (string key in languageResNames)
            {
                bool result = LanguageManager.BaseLanguages.Remove(key);
                DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}: {1}", key, result);
            }
        }

        void IAddon.FinalizeAddon()
        { }

        private string[] GetLanguageResourceNames()
        {
            // Get all the languages provided by this pack
            string[] nonLanguages = new[]
            {
                nameof(LanguageResources.Culture),
                nameof(LanguageResources.ResourceManager),
                nameof(LanguageResources.LanguageMetadata),
            };
            return PropertyManager.GetPropertiesNoEvaluation(typeof(LanguageResources)).Keys.Except(nonLanguages).ToArray();
        }
    }
}
