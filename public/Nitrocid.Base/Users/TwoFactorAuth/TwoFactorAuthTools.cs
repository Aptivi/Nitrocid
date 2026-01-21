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

using System;
using System.Text;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Drivers.Encoding;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Text.Probers.Regexp;
using OtpNet;
using QRCoder;
using Textify.General;

namespace Nitrocid.Base.Users.TwoFactorAuth
{
    internal static class TwoFactorAuthTools
    {
        internal static bool IsUserEnrolled(string user)
        {
            // Get the user info and re-call this function
            var userInfo = UserManagement.GetUser(user) ??
                throw new KernelException(KernelExceptionType.NoSuchUser);
            return IsUserEnrolled(userInfo);
        }

        internal static bool IsUserEnrolled(UserInfo user)
        {
            // Check to see if there is a valid secret and the enrollment is enabled
            try
            {
                if (!user.TwoFactorEnabled || string.IsNullOrEmpty(user.TwoFactorSecret))
                    return false;
                var secretBytes = SecretToBytesInternal(user);
                string secretString = Base32Encoding.ToString(secretBytes);
                return IsValidBase32(secretString);
            }
            catch
            {
                return false;
            }
        }

        internal static bool IsUserEnrolledLegacy(string user)
        {
            // Get the user info and re-call this function
            var userInfo = UserManagement.GetUser(user) ??
                throw new KernelException(KernelExceptionType.NoSuchUser);
            return IsUserEnrolledLegacy(userInfo);
        }

        internal static bool IsUserEnrolledLegacy(UserInfo user)
        {
            // Check to see if there is a valid secret and the enrollment is enabled
            return user.TwoFactorEnabled && !string.IsNullOrEmpty(user.TwoFactorSecret) && IsValidBase32(user.TwoFactorSecret);
        }

        internal static void EnrollUser(string user)
        {
            // Get the user info and re-call this function
            var userInfo = UserManagement.GetUser(user) ??
                throw new KernelException(KernelExceptionType.NoSuchUser);
            EnrollUser(userInfo);
        }

        internal static void EnrollUser(UserInfo user)
        {
            // Check for user enrollment
            if (IsUserEnrolled(user) || IsUserEnrolledLegacy(user))
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERALREADYENROLLED"));

            // Check the lock
            if (UserManagement.IsLocked(user.Username))
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_EXCEPTION_USERLOCKED"));

            // Check the password
            if (string.IsNullOrEmpty(user.Password) || user.Password == Encryption.GetEmptyHash("SHA256"))
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERHASNOPASSWORD_ENROLL"));

            // Generate a random key
            var key = KeyGeneration.GenerateRandomKey(20);
            string keyString = Base32Encoding.ToString(key);
            EncodeKey(keyString, out string keyAesEncoded, out string keyIvEncoded, out string keyEncoded);

            // Process the enrollment
            user.TwoFactorEnabled = true;
            user.TwoFactorSecret = keyEncoded;
            user.TwoFactorKey = keyAesEncoded;
            user.TwoFactorIv = keyIvEncoded;
            UserManagement.SaveUsers();
        }

        internal static void UnenrollUser(string user)
        {
            // Get the user info and re-call this function
            var userInfo = UserManagement.GetUser(user) ??
                throw new KernelException(KernelExceptionType.NoSuchUser);
            UnenrollUser(userInfo);
        }

        internal static void UnenrollUser(UserInfo user)
        {
            // Check for user enrollment
            if (!IsUserEnrolled(user) && !IsUserEnrolledLegacy(user))
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERNOTENROLLED"));

            // Check the lock
            if (UserManagement.IsLocked(user.Username))
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_EXCEPTION_USERLOCKED"));

            // Check the password
            if (string.IsNullOrEmpty(user.Password) || user.Password == Encryption.GetEmptyHash("SHA256"))
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERHASNOPASSWORD_UNENROLL"));

            // Cancel the enrollment
            user.TwoFactorEnabled = false;
            user.TwoFactorSecret = "";
            user.TwoFactorKey = "";
            user.TwoFactorIv = "";
            UserManagement.SaveUsers();
        }

        internal static byte[] SecretToBytes(string user)
        {
            // Get the user info and re-call this function
            var userInfo = UserManagement.GetUser(user) ??
                throw new KernelException(KernelExceptionType.NoSuchUser);
            return SecretToBytes(userInfo);
        }

        internal static byte[] SecretToBytes(UserInfo user)
        {
            // Check for user enrollment
            if (!IsUserEnrolled(user) && !IsUserEnrolledLegacy(user))
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERNOTENROLLED"));

            return SecretToBytesInternal(user);
        }

        internal static byte[] SecretToBytesInternal(UserInfo user)
        {
            // Check the password
            if (string.IsNullOrEmpty(user.Password) || user.Password == Encryption.GetEmptyHash("SHA256"))
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERHASNOPASSWORD_MISC"));

            // Upgrade the key if needed
            UpgradeLegacyKey(user);

            // Get the secret bytes
            var rsaDriver = DriverHandler.GetDriver<IEncodingDriver>("Default");
            rsaDriver.Initialize();
            byte[] keyAesEncoded = Convert.FromBase64String(user.TwoFactorKey);
            byte[] keyIvEncoded = Convert.FromBase64String(user.TwoFactorIv);
            string keyEncoded = rsaDriver.GetDecodedString(Convert.FromBase64String(user.TwoFactorSecret), keyAesEncoded, keyIvEncoded);
            return Base32Encoding.ToBytes(keyEncoded);
        }

        internal static void EncodeKey(string keyString, out string keyAesEncoded, out string keyIvEncoded, out string keyEncoded)
        {
            var rsaDriver = DriverHandler.GetDriver<IEncodingDriver>("Default");
            rsaDriver.Initialize();
            keyAesEncoded = Convert.ToBase64String(rsaDriver.Key);
            keyIvEncoded = Convert.ToBase64String(rsaDriver.Iv);
            keyEncoded = Convert.ToBase64String(rsaDriver.GetEncodedString(keyString));
        }

        internal static void UpgradeLegacyKey(UserInfo user)
        {
            if (!IsUserEnrolledLegacy(user))
                return;
            EncodeKey(user.TwoFactorSecret, out string keyAesEncoded, out string keyIvEncoded, out string keyEncoded);
            user.TwoFactorEnabled = true;
            user.TwoFactorSecret = keyEncoded;
            user.TwoFactorKey = keyAesEncoded;
            user.TwoFactorIv = keyIvEncoded;
            UserManagement.SaveUsers();
        }

        internal static string GenerateOtpAuthUrl(string user)
        {
            // Get the user info and re-call this function
            var userInfo = UserManagement.GetUser(user) ??
                throw new KernelException(KernelExceptionType.NoSuchUser);
            return GenerateOtpAuthUrl(userInfo);
        }

        internal static string GenerateOtpAuthUrl(UserInfo user)
        {
            // Check for user enrollment
            if (!IsUserEnrolled(user) && !IsUserEnrolledLegacy(user))
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERNOTENROLLED"));

            // Check the password
            if (string.IsNullOrEmpty(user.Password) || user.Password == Encryption.GetEmptyHash("SHA256"))
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERHASNOPASSWORD_MISC"));

            // Generate the OTP auth URL for Google Authenticator
            var otpUrl = new OtpUri(
                OtpType.Totp,
                SecretToBytes(user),
                user.Username,
                "Nitrocid"
            );
            return otpUrl.ToString();
        }

        internal static string RenderQRCodeMatrix(string user)
        {
            // Get the user info and re-call this function
            var userInfo = UserManagement.GetUser(user) ??
                throw new KernelException(KernelExceptionType.NoSuchUser);
            return RenderQRCodeMatrix(userInfo);
        }

        internal static string RenderQRCodeMatrix(UserInfo user)
        {
            var otpUrl = GenerateOtpAuthUrl(user);
            var otpQRRenderer = new QrCode()
            {
                Text = otpUrl,
            };
            return otpQRRenderer.Render();
        }

        private static bool IsValidBase32(string secret)
        {
            for (int i = 0; i < secret.Length; i++)
            {
                char c = secret[i];
                if ((c >= '[' || c <= '@') && (c >= '8' || c <= '1') && (c >= '{' || c <= '`'))
                    return false;
            }
            return true;
        }
    }
}
