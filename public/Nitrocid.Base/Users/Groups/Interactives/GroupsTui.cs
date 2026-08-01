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
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Security.Permissions;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;

namespace Nitrocid.Base.Users.Groups.Interactives
{
    internal class GroupsTui : BaseInteractiveTui<GroupInfo>, IInteractiveTui<GroupInfo>
    {
        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                // TODO: NKS_MISC_INTERACTIVES_GROUPSCLI_HELP01_TITLE -> Managing groups
                // TODO: NKS_MISC_INTERACTIVES_GROUPSCLI_HELP01_DESC -> Using the groups TUI to manage groups
                // TODO: NKS_MISC_INTERACTIVES_GROUPSCLI_HELP01_BODY -> With this textual user interface, you can easily manage groups.
                HelpTitle = /* Localizable */ "NKS_MISC_INTERACTIVES_GROUPSCLI_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_MISC_INTERACTIVES_GROUPSCLI_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_GROUPSCLI_HELP01_BODY") + "\n\n" +
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/advanced-and-power-groups/inner-workings/inner-essentials/system-notifications",
            }
        ];

        /// <inheritdoc/>
        public override IEnumerable<GroupInfo> PrimaryDataSource =>
            GroupManagement.AvailableGroups;

        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetInfoFromItem(GroupInfo item)
        {
            // Render group information
            // TODO: NKS_MISC_INTERACTIVES_GROUPSTUI_GROUPNAME -> Group name
            StringBuilder builder = new();
            builder.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_GROUPSTUI_GROUPNAME") + ": " + item.GroupName);

            // Now, the permissions.
            builder.AppendLine(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_USERINFO_PERMS"));
            foreach (string perm in item.Permissions)
                builder.AppendLine($"  - {perm}");

            // Render them to the second pane
            return builder.ToString();
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem(GroupInfo item) =>
            item.GroupName;

        /// <inheritdoc/>
        public override string GetEntryFromItem(GroupInfo item) =>
            item.GroupName;

        private void AddGroupPrompt()
        {
            try
            {
                // TODO: NKS_MISC_INTERACTIVES_GROUPSTUI_GROUPNAME_PROMPT -> Enter the group name.
                string groupName = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_GROUPSTUI_GROUPNAME_PROMPT"));
                GroupManagement.AddGroup(groupName);
                GroupManagement.SaveGroups();
            }
            catch (Exception ex)
            {
                // TODO: NKS_USERS_GROUPS_EXCEPTION_GROUPADDFAILED -> Failed to add group.
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_USERS_GROUPS_EXCEPTION_GROUPADDFAILED") + $" {ex.Message}");
            }
        }

        private void Remove(GroupInfo? group)
        {
            try
            {
                GroupManagement.RemoveGroup(group?.GroupName ?? "");
                GroupManagement.SaveGroups();
            }
            catch (Exception ex)
            {
                // TODO: NKS_USERS_GROUPS_EXCEPTION_GROUPREMOVEFAILED -> Failed to remove group.
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_USERS_GROUPS_EXCEPTION_GROUPREMOVEFAILED") + $" {ex.Message}");
            }
        }

        private void ManagePermissions(GroupInfo? group)
        {
            try
            {
                if (group is null)
                    throw new KernelException(KernelExceptionType.GroupManagement, LanguageTools.GetLocalized("NKS_USERS_GROUPS_EXCEPTION_GROUPINFO"));

                // Get permissions and let the group manage them
                // TODO: NKS_MISC_INTERACTIVES_GROUPSTUI_SELECTPERMISSIONS -> Select group permissions from the list below.
                var permissions = group.Permissions.Select(Enum.Parse<PermissionTypes>);
                var allPermissions = Enum.GetNames<PermissionTypes>();
                var selectedPermissions = allPermissions.Select((_, idx) => idx).Where((idx) => permissions.Contains(Enum.Parse<PermissionTypes>(allPermissions[idx]))).ToArray();
                var selectedPermissionsNew = InfoBoxSelectionMultipleColor.WriteInfoBoxSelectionMultiple(selectedPermissions, 0, InputChoiceTools.GetInputChoices(allPermissions), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_GROUPSTUI_SELECTPERMISSIONS"));
                string[] selectedPermissionsString = [.. selectedPermissionsNew.Select((idx) => allPermissions[idx])];
                group.Permissions = selectedPermissionsString;
                GroupManagement.SaveGroups();
            }
            catch (Exception ex)
            {
                // TODO: NKS_SECURITY_PERMISSIONS_EXCEPTION_MANAGEMENTERROR -> Managing permissions failed.
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SECURITY_PERMISSIONS_EXCEPTION_MANAGEMENTERROR") + $" {ex.Message}");
            }
        }

        internal static void OpenGroupsTui()
        {
            // TODO: NKS_MISC_INTERACTIVES_USERSTUI_KEYBINDING_MANAGEPERMS -> Manage permissions
            var tui = new GroupsTui();
            tui.Bindings.Add(new InteractiveTuiBinding<GroupInfo>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_DELETE"), ConsoleKey.Delete, (group, _, _, _) => tui.Remove(group)));
            tui.Bindings.Add(new InteractiveTuiBinding<GroupInfo>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_ALARMTUI_KEYBINDING_ADD"), ConsoleKey.F1, (_, _, _, _) => tui.AddGroupPrompt(), true));
            tui.Bindings.Add(new InteractiveTuiBinding<GroupInfo>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_USERSTUI_KEYBINDING_MANAGEPERMS"), ConsoleKey.F2, (group, _, _, _) => tui.ManagePermissions(group)));
            InteractiveTuiTools.OpenInteractiveTui(tui);
        }
    }
}
