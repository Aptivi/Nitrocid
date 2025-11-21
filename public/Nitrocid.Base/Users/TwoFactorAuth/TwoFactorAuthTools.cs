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

using System.Collections;
using System.Collections.Generic;
using System.Text;
using Nitrocid.Base.Drivers;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Base.Drivers.Encryption.Bases;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Text.Probers.Regexp;
using OtpNet;
using QRCoder;
using Terminaux.Base.TermInfo.Tabsets;

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
            return user.TwoFactorEnabled && !string.IsNullOrEmpty(user.TwoFactorSecret) && RegexpTools.IsMatch(user.TwoFactorSecret, "(?:[A-Z2-7]{8})*(?:[A-Z2-7]{2}={6}|[A-Z2-7]{4}={4}|[A-Z2-7]{5}={3}|[A-Z2-7]{7}=)?");
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
            if (IsUserEnrolled(user))
                // TODO: NKS_USERS_2FA_EXCEPTION_USERALREADYENROLLED -> User has already enrolled in to the 2FA authentication.
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERALREADYENROLLED"));

            // Check the lock
            if (UserManagement.IsLocked(user.Username))
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_EXCEPTION_USERLOCKED"));

            // Check the password
            if (string.IsNullOrEmpty(user.Password) || user.Password == Encryption.GetEmptyHash("SHA256"))
                // TODO: NKS_USERS_2FA_EXCEPTION_USERHASNOPASSWORD_ENROLL -> User has no password to enroll it in to the 2FA authentication.
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERHASNOPASSWORD_ENROLL"));

            // Generate a random key
            var key = KeyGeneration.GenerateRandomKey(20);
            string keyString = Base32Encoding.ToString(key);

            // Process the enrollment
            user.TwoFactorEnabled = true;
            user.TwoFactorSecret = keyString;
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
            if (!IsUserEnrolled(user))
                // TODO: NKS_USERS_2FA_EXCEPTION_USERNOTENROLLED -> User has not enrolled in to the 2FA authentication.
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERNOTENROLLED"));

            // Check the lock
            if (UserManagement.IsLocked(user.Username))
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_EXCEPTION_USERLOCKED"));

            // Check the password
            if (string.IsNullOrEmpty(user.Password) || user.Password == Encryption.GetEmptyHash("SHA256"))
                // TODO: NKS_USERS_2FA_EXCEPTION_USERHASNOPASSWORD_UNENROLL -> User has no password to unenroll it in to the 2FA authentication.
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERHASNOPASSWORD_UNENROLL"));

            // Cancel the enrollment
            user.TwoFactorEnabled = false;
            user.TwoFactorSecret = "";
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
            if (!IsUserEnrolled(user))
                // TODO: NKS_USERS_2FA_EXCEPTION_USERNOTENROLLED -> User has not enrolled in to the 2FA authentication.
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERNOTENROLLED"));

            // Check the password
            if (string.IsNullOrEmpty(user.Password) || user.Password == Encryption.GetEmptyHash("SHA256"))
                // TODO: NKS_USERS_2FA_EXCEPTION_USERHASNOPASSWORD_MISC -> User has no password to perform 2FA authentication operations.
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERHASNOPASSWORD_MISC"));

            // Get the secret bytes
            return Base32Encoding.ToBytes(user.TwoFactorSecret);
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
            if (!IsUserEnrolled(user))
                // TODO: NKS_USERS_2FA_EXCEPTION_USERNOTENROLLED -> User has not enrolled in to the 2FA authentication.
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERNOTENROLLED"));

            // Check the password
            if (string.IsNullOrEmpty(user.Password) || user.Password == Encryption.GetEmptyHash("SHA256"))
                // TODO: NKS_USERS_2FA_EXCEPTION_USERHASNOPASSWORD_MISC -> User has no password to perform 2FA authentication operations.
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERHASNOPASSWORD_MISC"));

            // Generate the OTP auth URL for Google Authenticator
            var otpUrl = new OtpUri(
                OtpType.Totp,
                Base32Encoding.ToBytes(user.TwoFactorSecret),
                user.Username,
                "Nitrocid"
            );
            return otpUrl.ToString();
        }

        internal static QRCodeData GenerateQRCodeData(string user)
        {
            // Get the user info and re-call this function
            var userInfo = UserManagement.GetUser(user) ??
                throw new KernelException(KernelExceptionType.NoSuchUser);
            return GenerateQRCodeData(userInfo);
        }

        internal static QRCodeData GenerateQRCodeData(UserInfo user)
        {
            // Check for user enrollment
            if (!IsUserEnrolled(user))
                // TODO: NKS_USERS_2FA_EXCEPTION_USERNOTENROLLED -> User has not enrolled in to the 2FA authentication.
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERNOTENROLLED"));

            // Check the password
            if (string.IsNullOrEmpty(user.Password) || user.Password == Encryption.GetEmptyHash("SHA256"))
                // TODO: NKS_USERS_2FA_EXCEPTION_USERHASNOPASSWORD_MISC -> User has no password to perform 2FA authentication operations.
                throw new KernelException(KernelExceptionType.UserManagement, LanguageTools.GetLocalized("NKS_USERS_2FA_EXCEPTION_USERHASNOPASSWORD_MISC"));

            // Generate the OTP auth QR code for scanning from phone
            var otpUrl = GenerateOtpAuthUrl(user);
            var otpGenerator = new QRCodeGenerator();
            var otpQRData = otpGenerator.CreateQrCode(otpUrl, QRCodeGenerator.ECCLevel.Q);
            return otpQRData;
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
            // Generate the code matrix
            var codeData = GenerateQRCodeData(user);
            var otpQRMatrix = codeData.ModuleMatrix;

            // Now, we need to use this data to print QR code to the console
            var qrBuilder = new StringBuilder();
            for (int y = 0; y < otpQRMatrix.Count; y += 2)
            {
                for (int x = 0; x < otpQRMatrix[y].Count; x++)
                {
                    // Get the upper and the lower matrix of QR code to determine how to print the code
                    bool upperMatrix = otpQRMatrix[y][x];
                    bool lowerMatrix = y + 1 < otpQRMatrix.Count && otpQRMatrix[y + 1][x];

                    // Now, use the block characters to print the QR code
                    if (upperMatrix && lowerMatrix)
                        qrBuilder.Append('█');
                    else if (upperMatrix && !lowerMatrix)
                        qrBuilder.Append('▀');
                    else if (!upperMatrix && lowerMatrix)
                        qrBuilder.Append('▄');
                    else
                        qrBuilder.Append(' ');
                }
                qrBuilder.AppendLine();
            }
            return qrBuilder.ToString();
        }
    }
}
