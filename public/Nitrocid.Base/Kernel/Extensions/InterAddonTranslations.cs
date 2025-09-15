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

#if NKS_EXTENSIONS
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Kernel.Extensions
{
    /// <summary>
    /// Translations from enumerations to names for all known addons
    /// </summary>
    public static partial class InterAddonTranslations
    {
        /// <summary>
        /// Gets the addon name from the type
        /// </summary>
        /// <param name="addon">Addon type</param>
        /// <returns>The known addon name</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetAddonName(KnownAddons addon)
        {
            if (knownAddons.TryGetValue(addon, out (string, string) name))
                return name.Item1;
            throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_INTERADDON_EXCEPTION_NOADDONTYPE"), addon.ToString());
        }

        /// <summary>
        /// Gets the localized addon name from the type
        /// </summary>
        /// <param name="addon">Addon type</param>
        /// <returns>The known addon name</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetLocalizedAddonName(KnownAddons addon)
        {
            if (knownAddons.TryGetValue(addon, out (string, string) name))
                return LanguageTools.GetLocalized(name.Item2);
            throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_KERNEL_EXTENSIONS_INTERADDON_EXCEPTION_NOADDONTYPE"), addon.ToString());
        }
    }
}
#endif
