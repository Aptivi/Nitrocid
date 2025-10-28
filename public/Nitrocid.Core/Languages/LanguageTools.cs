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

using ResourceLab.Management;
using System.Globalization;
using System.Resources;

namespace Nitrocid.Core.Languages
{
    internal static class LanguageTools
    {
        private const string LocalName = "Nitrocid.Core";

        internal static string GetLocalized(string id) =>
            GetLocalized(id, CultureInfo.CurrentUICulture);

        internal static string GetLocalized(string id, CultureInfo culture)
        {
            // Add local resource
            if (!ResourcesManager.ResourceManagerExists(LocalName))
                ResourcesManager.AddResourceManager(LocalName, new($"{LocalName}.Resources.Languages.Output.Localizations", typeof(LanguageTools).Assembly));

            // Loop through all resource managers
            foreach (var resourceManager in ResourcesManager.ResourceManagers.Values)
            {
                try
                {
                    string resourceLocalization = resourceManager.GetString(id, culture) ?? "";
                    if (!string.IsNullOrEmpty(resourceLocalization))
                        return resourceLocalization;
                }
                catch
                {
                    return id;
                }
            }
            return id;
        }

        internal static void AddCustomAction(string localType, ResourceManager resourceManager)
        {
            // Add resource
            if (!ResourcesManager.ResourceManagerExists(localType))
                ResourcesManager.AddResourceManager(localType, resourceManager);
        }

        internal static void RemoveCustomAction(string localType)
        {
            // Remove resource
            if (ResourcesManager.ResourceManagerExists(localType))
                ResourcesManager.RemoveResourceManager(localType);
        }
    }
}
