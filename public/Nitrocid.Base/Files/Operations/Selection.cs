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

using Terminaux.Inputs.Interactive;
using Nitrocid.Base.Misc.Interactives;
using System;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Files.Instances;
using Nitrocid.Base.Files.Paths;

namespace Nitrocid.Base.Files
{
    /// <summary>
    /// File and folder selection class
    /// </summary>
    public static partial class FilesystemTools
    {
        /// <summary>
        /// Opens the file selector
        /// </summary>
        /// <returns>A selected file, or an empty string if not selected</returns>
        public static string SelectFile() =>
            SelectFile(PathsManagement.HomePath);

        /// <summary>
        /// Opens the file selector
        /// </summary>
        /// <param name="path">Target path to open</param>
        /// <returns>A selected file, or an empty string if not selected</returns>
        public static string SelectFile(string path)
        {
            var selector = new FileSelectorCli()
            {
                firstPanePath =
                    string.IsNullOrEmpty(path) ?
                    PathsManagement.HomePath :
                    FilesystemTools.NeutralizePath(path),
            };
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_SELECT"), ConsoleKey.Enter, (entry1, _, _, _) => selector.SelectOrGoTo(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_COPY"), ConsoleKey.F1, (entry1, _, _, _) => selector.CopyTo(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_MOVE"), ConsoleKey.F2, (entry1, _, _, _) => selector.MoveTo(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_DELETE"), ConsoleKey.F3, (entry1, _, _, _) => selector.RemoveFileOrDir(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_UP"), ConsoleKey.F4, (_, _, _, _) => selector.GoUp(), true));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_INFO"), ConsoleKey.F5, (entry1, _, _, _) => selector.PrintFileSystemEntry(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_GOTO"), ConsoleKey.F6, (_, _, _, _) => selector.GoTo(), true));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_RENAME"), ConsoleKey.F7, (entry1, _, _, _) => selector.Rename(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_NEWFOLDER"), ConsoleKey.F8, (_, _, _, _) => selector.MakeDir(), true));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_HASH"), ConsoleKey.F9, (entry1, _, _, _) => selector.Hash(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_VERIFY"), ConsoleKey.F10, (entry1, _, _, _) => selector.Verify(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_PREVIEW"), ConsoleKey.P, (entry1, _, _, _) => selector.Preview(entry1)));
            InteractiveTuiTools.OpenInteractiveTui(selector);
            string selected = selector.selectedFile;
            return selected;
        }

        /// <summary>
        /// Opens the file selector
        /// </summary>
        /// <returns>A list of selected files, or an empty list if not selected</returns>
        public static string[] SelectFiles() =>
            SelectFiles(PathsManagement.HomePath);

        /// <summary>
        /// Opens the file selector
        /// </summary>
        /// <param name="path">Target path to open</param>
        /// <returns>A list of selected files, or an empty list if not selected</returns>
        public static string[] SelectFiles(string path)
        {
            var selector = new FilesSelectorCli()
            {
                firstPanePath =
                    string.IsNullOrEmpty(path) ?
                    PathsManagement.HomePath :
                    FilesystemTools.NeutralizePath(path),
            };
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_SELECT"), ConsoleKey.Enter, (entry1, _, _, _) => selector.SelectOrGoTo(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_COPY"), ConsoleKey.F1, (entry1, _, _, _) => selector.CopyTo(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_MOVE"), ConsoleKey.F2, (entry1, _, _, _) => selector.MoveTo(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_DELETE"), ConsoleKey.F3, (entry1, _, _, _) => selector.RemoveFileOrDir(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_UP"), ConsoleKey.F4, (_, _, _, _) => selector.GoUp(), true));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_INFO"), ConsoleKey.F5, (entry1, _, _, _) => selector.PrintFileSystemEntry(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_GOTO"), ConsoleKey.F6, (_, _, _, _) => selector.GoTo(), true));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_RENAME"), ConsoleKey.F7, (entry1, _, _, _) => selector.Rename(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_NEWFOLDER"), ConsoleKey.F8, (_, _, _, _) => selector.MakeDir(), true));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_HASH"), ConsoleKey.F9, (entry1, _, _, _) => selector.Hash(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_VERIFY"), ConsoleKey.F10, (entry1, _, _, _) => selector.Verify(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_PREVIEWSELECTIONS"), ConsoleKey.F11, (_, _, _, _) => selector.PreviewSelected()));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_PREVIEW"), ConsoleKey.P, (entry1, _, _, _) => selector.Preview(entry1)));
            InteractiveTuiTools.OpenInteractiveTui(selector);
            string[] selected = [.. selector.selectedFiles];
            return selected;
        }

        /// <summary>
        /// Opens the folder selector
        /// </summary>
        /// <returns>A selected folder, or an empty string if not selected</returns>
        public static string SelectFolder() =>
            SelectFolder(PathsManagement.HomePath);

        /// <summary>
        /// Opens the folder selector
        /// </summary>
        /// <param name="path">Target path to open</param>
        /// <returns>A selected folder, or an empty string if not selected</returns>
        public static string SelectFolder(string path)
        {
            var selector = new FolderSelectorCli()
            {
                firstPanePath =
                    string.IsNullOrEmpty(path) ?
                    PathsManagement.HomePath :
                    FilesystemTools.NeutralizePath(path),
            };
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_OPEN"), ConsoleKey.Enter, (entry1, _, _, _) => selector.Open(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_SELECT"), ConsoleKey.Spacebar, (entry1, _, _, _) => selector.Select(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_COPY"), ConsoleKey.F1, (entry1, _, _, _) => selector.CopyTo(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_MOVE"), ConsoleKey.F2, (entry1, _, _, _) => selector.MoveTo(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_DELETE"), ConsoleKey.F3, (entry1, _, _, _) => selector.RemoveFileOrDir(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_UP"), ConsoleKey.F4, (_, _, _, _) => selector.GoUp(), true));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_INFO"), ConsoleKey.F5, (entry1, _, _, _) => selector.PrintFileSystemEntry(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_GOTO"), ConsoleKey.F6, (_, _, _, _) => selector.GoTo(), true));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_RENAME"), ConsoleKey.F7, (entry1, _, _, _) => selector.Rename(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_NEWFOLDER"), ConsoleKey.F8, (_, _, _, _) => selector.MakeDir(), true));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_HASH"), ConsoleKey.F9, (entry1, _, _, _) => selector.Hash(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_VERIFY"), ConsoleKey.F10, (entry1, _, _, _) => selector.Verify(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_PREVIEW"), ConsoleKey.P, (entry1, _, _, _) => selector.Preview(entry1)));
            InteractiveTuiTools.OpenInteractiveTui(selector);
            string selected = selector.selectedFolder;
            return selected;
        }

        /// <summary>
        /// Opens the folder selector
        /// </summary>
        /// <returns>A list of selected folders, or an empty list if not selected</returns>
        public static string[] SelectFolders() =>
            SelectFolders(PathsManagement.HomePath);

        /// <summary>
        /// Opens the folder selector
        /// </summary>
        /// <param name="path">Target path to open</param>
        /// <returns>A list of selected folders, or an empty list if not selected</returns>
        public static string[] SelectFolders(string path)
        {
            var selector = new FoldersSelectorCli()
            {
                firstPanePath =
                    string.IsNullOrEmpty(path) ?
                    PathsManagement.HomePath :
                    FilesystemTools.NeutralizePath(path),
            };
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_OPEN"), ConsoleKey.Enter, (entry1, _, _, _) => selector.Open(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_SELECT"), ConsoleKey.Spacebar, (entry1, _, _, _) => selector.Select(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_COPY"), ConsoleKey.F1, (entry1, _, _, _) => selector.CopyTo(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_MOVE"), ConsoleKey.F2, (entry1, _, _, _) => selector.MoveTo(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_DELETE"), ConsoleKey.F3, (entry1, _, _, _) => selector.RemoveFileOrDir(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_UP"), ConsoleKey.F4, (_, _, _, _) => selector.GoUp(), true));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_INFO"), ConsoleKey.F5, (entry1, _, _, _) => selector.PrintFileSystemEntry(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_GOTO"), ConsoleKey.F6, (_, _, _, _) => selector.GoTo(), true));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_RENAME"), ConsoleKey.F7, (entry1, _, _, _) => selector.Rename(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_NEWFOLDER"), ConsoleKey.F8, (_, _, _, _) => selector.MakeDir(), true));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_HASH"), ConsoleKey.F9, (entry1, _, _, _) => selector.Hash(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_VERIFY"), ConsoleKey.F10, (entry1, _, _, _) => selector.Verify(entry1)));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_PREVIEWSELECTIONS"), ConsoleKey.F11, (_, _, _, _) => selector.PreviewSelected()));
            selector.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_KEYBINDING_PREVIEW"), ConsoleKey.P, (entry1, _, _, _) => selector.Preview(entry1)));
            InteractiveTuiTools.OpenInteractiveTui(selector);
            string[] selected = [.. selector.selectedFolders];
            return selected;
        }
    }
}
