//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Security.Permissions;
using Nitrocid.Base.Users.Groups;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Themes.Colors;
using Textify.General;

namespace Nitrocid.Base.Users.Interactives
{
    internal class UsersTui : BaseInteractiveTui<string>, IInteractiveTui<string>
    {
        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                // TODO: NKS_MISC_INTERACTIVES_USERSCLI_HELP01_TITLE -> Managing users
                // TODO: NKS_MISC_INTERACTIVES_USERSCLI_HELP01_DESC -> Using the users TUI to manage users
                // TODO: NKS_MISC_INTERACTIVES_USERSCLI_HELP01_BODY -> With this textual user interface, you can easily manage users.
                HelpTitle = /* Localizable */ "NKS_MISC_INTERACTIVES_USERSCLI_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_MISC_INTERACTIVES_USERSCLI_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_USERSCLI_HELP01_BODY") + "\n\n" +
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/advanced-and-power-users/inner-workings/inner-essentials/system-notifications",
            }
        ];

        /// <inheritdoc/>
        public override IEnumerable<string> PrimaryDataSource =>
            UserManagement.ListAllUsers(true, true);

        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetInfoFromItem(string item)
        {
            // Get the user
            var user = UserManagement.GetUser(item);

            // Render user information
            StringBuilder builder = new();
            builder.AppendLine(LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_USERNAME_PROMPT") + ": " + user.Username);
            builder.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FULLNAME") + ": " + user.FullName);
            builder.AppendLine(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERINFO_PREFLANG") + ": " + user.PreferredLanguage ?? "");
            builder.AppendLine(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERINFO_PREFCULTURE") + ": " + user.PreferredCulture ?? "");
            builder.AppendLine(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERINFO_FLAGS") + ": " + string.Join(", ", user.Flags) + "\n");

            // Now, the permissions.
            builder.AppendLine(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERINFO_PERMS"));
            foreach (string perm in user.Permissions)
                builder.AppendLine($"  - {perm}");

            // Now, the groups.
            builder.AppendLine(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERINFO_GROUPS"));
            foreach (string group in user.Groups)
                builder.AppendLine($"  - {group}");

            // Render them to the second pane
            return builder.ToString();
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem(string item)
        {
            var user = UserManagement.GetUser(item);
            return $"{item}{(!string.IsNullOrWhiteSpace(user.FullName) ? $" - {user.FullName}" : "")}";
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(string item) =>
            item;

        private void AddUserPrompt()
        {
            try
            {
                string userName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_USERNAME_PROMPT_DESC"));
                UserManagement.AddUser(userName);
                UserManagement.SaveUsers();
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_USERS_EXCEPTION_ADDERROR") + $" {ex.Message}");
            }
        }

        private void Remove(string? user)
        {
            try
            {
                UserManagement.RemoveUser(user ?? "");
                UserManagement.SaveUsers();
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_USERS_EXCEPTION_REMOVEERROR") + $" {ex.Message}");
            }
        }

        private void ChangePassword(string? user)
        {
            try
            {
                var userInstance = UserManagement.GetUser(user ?? "");

                // Prompt for current password if a target user has one
                string currentPassword = "";
                if (userInstance.Password != Encryption.GetEmptyHash("SHA256"))
                    currentPassword = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_USERS_LOGIN_PASSWORDPROMPT").FormatString(user ?? ""), InfoBoxInputType.Password);

                // Prompt for new password and confirmation
                // TODO: NKS_USERS_LOGIN_NEWPASSWORDPROMPT -> {0}'s new password: 
                // TODO: NKS_USERS_LOGIN_NEWCONFIRMPASSWORDPROMPT -> Confirm {0}'s new password: 
                string newPassword = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_USERS_LOGIN_NEWPASSWORDPROMPT").FormatString(user ?? ""), InfoBoxInputType.Password);
                string confirmPassword = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_USERS_LOGIN_NEWCONFIRMPASSWORDPROMPT").FormatString(user ?? ""), InfoBoxInputType.Password);
                if (confirmPassword.Contains(' '))
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHPWD_NOSPACES"), true, ThemeColorType.Error);
                else if (confirmPassword != newPassword)
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_CHPWD_MISMATCH"), true, ThemeColorType.Error);
                else
                {
                    UserManagement.ChangePassword(user ?? "", currentPassword, newPassword);
                    UserManagement.SaveUsers();
                }
            }
            catch (Exception ex)
            {
                // TODO: NKS_USERS_EXCEPTION_CHANGEPASSWORDERROR -> Error when trying to change password of a user.
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_USERS_EXCEPTION_CHANGEPASSWORDERROR") + $" {ex.Message}");
            }
        }

        private void ManagePermissions(string? user)
        {
            try
            {
                var userInstance = UserManagement.GetUser(user ?? "");

                // Get permissions and let the user manage them
                // TODO: NKS_MISC_INTERACTIVES_USERSTUI_SELECTPERMISSIONS -> Select user permissions from the list below.
                var permissions = userInstance.Permissions.Select(Enum.Parse<PermissionTypes>);
                var allPermissions = Enum.GetNames<PermissionTypes>();
                var selectedPermissions = allPermissions.Select((_, idx) => idx).Where((idx) => permissions.Contains(Enum.Parse<PermissionTypes>(allPermissions[idx]))).ToArray();
                var selectedPermissionsNew = InfoBoxSelectionMultipleColor.WriteInfoBoxSelectionMultiple(selectedPermissions, 0, InputChoiceTools.GetInputChoices(allPermissions), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_USERSTUI_SELECTPERMISSIONS"));
                string[] selectedPermissionsString = [.. selectedPermissionsNew.Select((idx) => allPermissions[idx])];
                userInstance.Permissions = selectedPermissionsString;
                UserManagement.SaveUsers();
            }
            catch (Exception ex)
            {
                // TODO: NKS_SECURITY_PERMISSIONS_EXCEPTION_MANAGEMENTERROR -> Managing permissions failed.
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SECURITY_PERMISSIONS_EXCEPTION_MANAGEMENTERROR") + $" {ex.Message}");
            }
        }

        private void ManageFlags(string? user)
        {
            try
            {
                var userInstance = UserManagement.GetUser(user ?? "");

                // Get flags and let the user manage them
                // TODO: NKS_MISC_INTERACTIVES_USERSTUI_SELECTFLAGS -> Select user flags from the list below.
                var flags = userInstance.Flags;
                var flagValues = Enum.GetValues<UserFlags>().Where(uf => uf > 0).ToArray();
                string[] allFlags = [.. flagValues.Select(uf => uf.ToString())];
                var selectedFlags = allFlags.Select((_, idx) => idx).Where((idx) => flags.HasFlag(Enum.Parse<UserFlags>(allFlags[idx]))).ToArray();
                var selectedFlagsIndexes = InfoBoxSelectionMultipleColor.WriteInfoBoxSelectionMultiple(selectedFlags, 0, InputChoiceTools.GetInputChoices(allFlags), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_USERSTUI_SELECTFLAGS"));
                var selectedFlagsNew = selectedFlagsIndexes.Select(idx => flagValues[idx]);
                foreach (var selectedFlag in selectedFlagsNew)
                    userInstance.Flags |= selectedFlag;
                UserManagement.SaveUsers();
            }
            catch (Exception ex)
            {
                // TODO: NKS_USERS_EXCEPTION_FLAGMANAGEMENTERROR -> Managing flags failed.
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_USERS_EXCEPTION_FLAGMANAGEMENTERROR") + $" {ex.Message}");
            }
        }

        private void ManageGroupMemberships(string? user)
        {
            try
            {
                var userInstance = UserManagement.GetUser(user ?? "");

                // Get groups and let the user manage them
                // TODO: NKS_MISC_INTERACTIVES_USERSTUI_SELECTGROUPS -> Select user groups from the list below.
                var allGroups = GroupManagement.AvailableGroups.Select(group => group.GroupName).ToArray();
                var groupIndexes = allGroups.Select((_, idx) => idx).Where(idx => userInstance.Groups.Contains(allGroups[idx])).ToArray();
                var selectedFlagsIndexes = InfoBoxSelectionMultipleColor.WriteInfoBoxSelectionMultiple(groupIndexes, 0, InputChoiceTools.GetInputChoices(allGroups), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_USERSTUI_SELECTGROUPS"));
                var selectedFlagsNew = selectedFlagsIndexes.Select(idx => allGroups[idx]).ToArray();
                userInstance.Groups = selectedFlagsNew;
                UserManagement.SaveUsers();
            }
            catch (Exception ex)
            {
                // TODO: NKS_USERS_EXCEPTION_GROUPMANAGEMENTERROR -> Managing groups failed.
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_USERS_EXCEPTION_GROUPMANAGEMENTERROR") + $" {ex.Message}");
            }
        }

        internal static void OpenUsersTui()
        {
            // TODO: NKS_MISC_INTERACTIVES_USERSTUI_KEYBINDING_CHANGEPASSWORD -> Change password
            // TODO: NKS_MISC_INTERACTIVES_USERSTUI_KEYBINDING_MANAGEFLAGS -> Manage flags
            // TODO: NKS_MISC_INTERACTIVES_USERSTUI_KEYBINDING_MANAGEPERMS -> Manage permissions
            // TODO: NKS_MISC_INTERACTIVES_USERSTUI_KEYBINDING_MANAGEMEMBERSHIPS -> Manage memberships
            var tui = new UsersTui();
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_DELETE"), ConsoleKey.Delete, (user, _, _, _) => tui.Remove(user)));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_KEYBINDING_ADD"), ConsoleKey.F1, (_, _, _, _) => tui.AddUserPrompt()));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_USERSTUI_KEYBINDING_CHANGEPASSWORD"), ConsoleKey.F2, (user, _, _, _) => tui.ChangePassword(user)));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_USERSTUI_KEYBINDING_MANAGEFLAGS"), ConsoleKey.F3, (user, _, _, _) => tui.ManageFlags(user)));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_USERSTUI_KEYBINDING_MANAGEPERMS"), ConsoleKey.F4, (user, _, _, _) => tui.ManagePermissions(user)));
            tui.Bindings.Add(new InteractiveTuiBinding<string>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_USERSTUI_KEYBINDING_MANAGEMEMBERSHIPS"), ConsoleKey.F5, (user, _, _, _) => tui.ManageGroupMemberships(user)));
            InteractiveTuiTools.OpenInteractiveTui(tui);
        }
    }
}
