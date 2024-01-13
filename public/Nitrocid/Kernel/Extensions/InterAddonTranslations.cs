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

using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using System.Collections.Generic;

namespace Nitrocid.Kernel.Extensions
{
    /// <summary>
    /// Translations from enumerations to names for all known addons
    /// </summary>
    public static class InterAddonTranslations
    {
        private static readonly Dictionary<KnownAddons, string> knownAddons = new()
        {
            // Note: The names are not to be localized for usage with GetAddonName(), because they are constant addon names.
            { KnownAddons.AddonLanguagePacks,           /* Localizable */ "Extra Languages Pack" },
            { KnownAddons.AddonScreensaverPacks,        /* Localizable */ "Extra Screensavers Pack" },
            { KnownAddons.AddonSplashPacks,             /* Localizable */ "Extra Splashes Pack" },
            { KnownAddons.AddonThemePacks,              /* Localizable */ "Extra Themes Pack" },
            { KnownAddons.ExtrasAmusements,             /* Localizable */ "Extras - Amusements" },
            { KnownAddons.ExtrasArchiveShell,           /* Localizable */ "Extras - Archive Shell" },
            { KnownAddons.ExtrasBassBoom,               /* Localizable */ "Extras - BassBoom" },
            { KnownAddons.ExtrasCaffeine,               /* Localizable */ "Extras - Caffeine" },
            { KnownAddons.ExtrasCalculators,            /* Localizable */ "Extras - Calculators" },
            { KnownAddons.ExtrasCalendar,               /* Localizable */ "Extras - Calendar" },
            { KnownAddons.ExtrasColorConvert,           /* Localizable */ "Extras - Color Converter" },
            { KnownAddons.ExtrasContacts,               /* Localizable */ "Extras - Contacts" },
            { KnownAddons.ExtrasCrc32,                  /* Localizable */ "Extras - CRC32" },
            { KnownAddons.ExtrasDiagnostics,            /* Localizable */ "Extras - Diagnostics" },
            { KnownAddons.ExtrasDictionary,             /* Localizable */ "Extras - Dictionary" },
            { KnownAddons.ExtrasDocking,                /* Localizable */ "Extras - Docking" },
            { KnownAddons.ExtrasForecast,               /* Localizable */ "Extras - Forecast" },
            { KnownAddons.ExtrasFtpShell,               /* Localizable */ "Extras - FTP Shell" },
            { KnownAddons.ExtrasGitShell,               /* Localizable */ "Extras - Git Shell" },
            { KnownAddons.ExtrasHttpShell,              /* Localizable */ "Extras - HTTP Shell" },
            { KnownAddons.ExtrasInternetRadioInfo,      /* Localizable */ "Extras - Internet Radio Information" },
            { KnownAddons.ExtrasJsonShell,              /* Localizable */ "Extras - JSON Shell" },
            { KnownAddons.ExtrasLanguageStudio,         /* Localizable */ "Extras - Language Studio" },
            { KnownAddons.ExtrasMailShell,              /* Localizable */ "Extras - Mail Shell" },
            { KnownAddons.ExtrasNameGen,                /* Localizable */ "Extras - NameGen" },
            { KnownAddons.ExtrasNotes,                  /* Localizable */ "Extras - Notes" },
            { KnownAddons.ExtrasRssShell,               /* Localizable */ "Extras - RSS Shell" },
            { KnownAddons.ExtrasSftpShell,              /* Localizable */ "Extras - SFTP Shell" },
            { KnownAddons.ExtrasSqlShell,               /* Localizable */ "Extras - SQL Shell" },
            { KnownAddons.ExtrasThemeStudio,            /* Localizable */ "Extras - Theme Studio" },
            { KnownAddons.ExtrasTimeInfo,               /* Localizable */ "Extras - Time Info" },
            { KnownAddons.ExtrasTimers,                 /* Localizable */ "Extras - Timers" },
            { KnownAddons.ExtrasTips,                   /* Localizable */ "Extras - Kernel Tips" },
            { KnownAddons.ExtrasToDoList,               /* Localizable */ "Extras - To-do List" },
            { KnownAddons.ExtrasUnitConv,               /* Localizable */ "Extras - Unit Converter" },
            { KnownAddons.LegacyHddUncleaner,           /* Localizable */ "Legacy - HDD Uncleaner 2015" },
        };

        /// <summary>
        /// Gets the addon name from the type
        /// </summary>
        /// <param name="addon">Addon type</param>
        /// <returns>The known addon name</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetAddonName(KnownAddons addon)
        {
            if (knownAddons.TryGetValue(addon, out string name))
                return name;
            throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("No such addon type '{0}'"), addon.ToString());
        }

        /// <summary>
        /// Gets the localized addon name from the type
        /// </summary>
        /// <param name="addon">Addon type</param>
        /// <returns>The known addon name</returns>
        /// <exception cref="KernelException"></exception>
        public static string GetLocalizedAddonName(KnownAddons addon)
        {
            if (knownAddons.TryGetValue(addon, out string name))
                return Translate.DoTranslation(name);
            throw new KernelException(KernelExceptionType.AddonManagement, Translate.DoTranslation("No such addon type '{0}'"), addon.ToString());
        }
    }
}
