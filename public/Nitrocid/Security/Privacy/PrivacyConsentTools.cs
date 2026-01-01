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
using Nitrocid.Files.Operations;
using Nitrocid.Files.Operations.Querying;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Nitrocid.Misc.Splash;
using Nitrocid.Security.Privacy.Consents;
using Nitrocid.Users;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Textify.General;

namespace Nitrocid.Security.Privacy
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
                throw new KernelException(KernelExceptionType.PrivacyConsent, Translate.DoTranslation("Can't get method from the last two frames."));
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
                throw new KernelException(KernelExceptionType.PrivacyConsent, Translate.DoTranslation("Can't consent an empty permission."));

            // If already consented, return true.
            if (consentedPermissions.Contains(consent))
                return true;

            // Now, ask for consent, but respect the splash screen display
            if (!SplashReport.KernelBooted)
                SplashManager.BeginSplashOut(SplashManager.CurrentSplashContext);
            string consentAnswer = ChoiceStyle.PromptChoice(
                TextTools.FormatString(
                    Translate.DoTranslation("It looks like that a mod with the root namespace of {0} tries to access your data with the permission of {1}. Do you want to allow this mod to access your data?"),
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
            if (!Checking.FileExists(consentsPath))
                SaveConsents();

            // Now, load all the consents
            string serialized = Reading.ReadContentsText(consentsPath);
            consentedPermissions = JsonConvert.DeserializeObject<List<ConsentedPermission>>(serialized) ?? [];
        }

        internal static void SaveConsents()
        {
            // Save all the consents to JSON
            string consentsPath = PathsManagement.ConsentsPath;
            string serialized = JsonConvert.SerializeObject(consentedPermissions, Formatting.Indented);
            Writing.WriteContentsText(consentsPath, serialized);
        }
    }
}
