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

using Newtonsoft.Json;
using Terminaux.Inputs.Styles.Choice;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Textify.General;
using Nitrocid.Base.Files;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Security.Privacy.Consents;

namespace Nitrocid.Base.Security.Privacy
{
    /// <summary>
    /// Privacy consent tools
    /// </summary>
    public static class PrivacyConsentTools
    {
        private static List<ConsentedPermission> consentedPermissions = [];

        /// <summary>
        /// Consents a permission
        /// </summary>
        /// <param name="consentType">Permission type to consent</param>
        public static bool ConsentPermission(ConsentedPermissionType consentType)
        {
            // Get the namespace of the calling method
            var methodInfo = new StackTrace().GetFrame(2)?.GetMethod() ??
                throw new KernelException(KernelExceptionType.PrivacyConsent, LanguageTools.GetLocalized("NKS_SECURITY_PRIVACY_EXCEPTION_LASTTWOFRAMES"));
            var context = methodInfo.ReflectedType?.Namespace ?? "";
            string finalContext = context.Contains('.') ? context[..context.IndexOf('.')] : context;

            // Verify the type and the context
            var type = methodInfo.ReflectedType;
            if (finalContext == "KS" && ReflectionCommon.KernelTypes.Contains(type) ||
                 finalContext == "Nitrocid")
                return true;

            // We're not calling from the built-in methods, so make a consent
            var consent = new ConsentedPermission()
            {
                type = consentType,
                context = finalContext,
                user = UserManagement.CurrentUser.Username,
            };
            return ConsentPermission(consent);
        }

        internal static bool ConsentPermission(ConsentedPermission consent)
        {
            // Check the consent
            if (consent is null)
                throw new KernelException(KernelExceptionType.PrivacyConsent, LanguageTools.GetLocalized("NKS_SECURITY_PERMISSIONS_EXCEPTION_EMPTY"));

            // If already consented, return true.
            if (consentedPermissions.Contains(consent))
                return true;

            // Now, ask for consent, but respect the splash screen display
            if (!SplashReport.KernelBooted)
                SplashManager.BeginSplashOut(SplashManager.CurrentSplashContext);
            string consentAnswer = ChoiceStyle.PromptChoice(
                TextTools.FormatString(
                    LanguageTools.GetLocalized("NKS_SECURITY_PRIVACY_TRYINGACCESS"),
                    consent.Type.ToString(), consent.Context
                ), [("y", "Yes"), ("n", "No")]
            );
            if (!SplashReport.KernelBooted)
                SplashManager.EndSplashOut(SplashManager.CurrentSplashContext);
            if (!consentAnswer.Equals("y", System.StringComparison.OrdinalIgnoreCase))
                return false;

            // Add the consented permission to the list of consents
            consentedPermissions.Add(consent);
            return true;
        }

        internal static void LoadConsents()
        {
            string consentsPath = PathsManagement.ConsentsPath;

            // Check to see if we have the file
            if (!FilesystemTools.FileExists(consentsPath))
                SaveConsents();

            // Now, load all the consents
            string serialized = FilesystemTools.ReadContentsText(consentsPath);
            consentedPermissions = JsonConvert.DeserializeObject<List<ConsentedPermission>>(serialized) ?? [];
        }

        internal static void SaveConsents()
        {
            // Save all the consents to JSON
            string consentsPath = PathsManagement.ConsentsPath;
            string serialized = JsonConvert.SerializeObject(consentedPermissions, Formatting.Indented);
            FilesystemTools.WriteContentsText(consentsPath, serialized);
        }
    }
}
