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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Nitrocid.Base.Files;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Power;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Audio;
using Nitrocid.Base.Misc.Interactives;
using Nitrocid.Base.Users;
using Nitrocid.Base.Users.Login;
using Nitrocid.Base.Misc.Widgets;
using Nitrocid.Base.Misc.Widgets.Canvas;
using Nitrocid.Base.Misc.Widgets.Implementations;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;

namespace Nitrocid.Base.Shell.Homepage
{
    /// <summary>
    /// The Nitrocid Homepage tools
    /// </summary>
    public static class HomepageTools
    {
        internal static bool isHomepageEnabled = true;
        internal static bool isHomepageRssFeedEnabled = true;
        internal static string homepageWidgetName = nameof(AnalogClock);
        private static bool isOnHomepage = false;
        private static readonly Dictionary<string, Action> choiceActionsAddons = [];
        private static readonly Dictionary<string, Action> choiceActionsCustom = [];
        private static readonly Dictionary<string, Action> choiceActionsBuiltin = new()
        {
            { /* Localizable */ "NKS_SHELL_HOMEPAGE_FILEMANAGER", () => FilesystemTools.OpenFileManagerTui() },
            // TODO: NKS_SHELL_HOMEPAGE_FILEMANAGERSINGLE -> "File Manager (single pane)"
            { /* Localizable */ "NKS_SHELL_HOMEPAGE_FILEMANAGERSINGLE", () => FilesystemTools.OpenFileManagerTui(true) },
            { /* Localizable */ "NKS_SHELL_HOMEPAGE_ALARMMANAGER", AlarmCli.OpenAlarmCli },
            { /* Localizable */ "NKS_SHELL_HOMEPAGE_NOTIFICATIONS", NotificationsCli.OpenNotificationsCli },
            { /* Localizable */ "NKS_SHELL_HOMEPAGE_TASKMANAGER", TaskManagerCli.OpenTaskManagerCli },
        };

        private static Keybinding[] Bindings =>
        [
            // Keyboard
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_EXECUTE"), ConsoleKey.Enter),
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_LOGOUT"), ConsoleKey.Escape),
            // TODO: NKS_SHELL_HOMEPAGE_KEYBINDING_SHUTDOWN -> "Shut down"
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_SHUTDOWN"), ConsoleKey.Escape, ConsoleModifiers.Shift),
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_SHELL"), ConsoleKey.S),
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_KEYBINDINGS"), ConsoleKey.K),
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_SWITCH"), ConsoleKey.Tab),
            // TODO: NKS_SHELL_HOMEPAGE_KEYBINDING_NEXTPAGE -> "Next page"
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_NEXTPAGE"), ConsoleKey.RightArrow),
            // TODO: NKS_SHELL_HOMEPAGE_KEYBINDING_PrevPAGE -> "Previous page"
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_PREVPAGE"), ConsoleKey.LeftArrow),
            new("Play...", ConsoleKey.P, true),

            // Mouse
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_EXECUTE"), PointerButton.Left),
        ];

        /// <summary>
        /// Opens The Nitrocid Homepage
        /// </summary>
        public static void OpenHomepage()
        {
            if (isOnHomepage || !isHomepageEnabled)
                return;
            isOnHomepage = true;
            var homeScreen = new Screen();
            int choiceIdx = 0;
            int buttonHighlight = 0;
            var choices = PopulateChoices();

            // Handle paging
            List<WidgetRenderInfo[]> canvases = GetHomepagePages();
            int maxScreens = canvases.Count + 1;
            int pageNumber = 1;

            try
            {
                // Create a screen for the homepage
                var homeScreenBuffer = new ScreenPart();
                ScreenTools.SetCurrent(homeScreen);
                ThemeColorsTools.LoadBackground();

                // Now, render the homepage
                homeScreenBuffer.AddDynamicText(() => RenderHomepagePage(homeScreen.RefreshWasDone, pageNumber, canvases, choices, choiceIdx, buttonHighlight));
                homeScreen.AddBufferedPart("The Nitrocid Homepage", homeScreenBuffer);

                // Helper function
                void DoAction(int choiceIdx)
                {
                    if (choiceIdx < 0 || choiceIdx >= choices.Length)
                        return;

                    // Now, do the action!
                    var action = choices[choiceIdx].Item2;
                    action.Invoke();
                }

                // Render the thing and wait for a keypress
                bool exiting = false;
                bool render = true;
                while (!exiting)
                {
                    // Render and wait for input for a second
                    if (render)
                    {
                        ScreenTools.Render();
                        render = false;
                    }
                    InputEventInfo? data = null;
                    bool idle = !SpinWait.SpinUntil(() =>
                    {
                        data = Input.ReadPointerOrKeyNoBlock();
                        return data.EventType == InputEventType.Keyboard || data.EventType == InputEventType.Mouse;
                    }, 1000);
                    if (idle)
                    {
                        render = true;
                        continue;
                    }

                    // Read the available input
                    if (data?.PointerEventContext is PointerEventContext context)
                    {
                        // Unavailable in pages other than the first page
                        if (pageNumber != 1)
                            continue;

                        // Prepare the common variables
                        int height = ConsoleWrapper.WindowHeight - 4;
                        int width = ConsoleWrapper.WindowWidth - 8;
                        int posX = ConsoleWrapper.WindowWidth / 2 - width / 2 - 1;
                        int posY = ConsoleWrapper.WindowHeight / 2 - height / 2 - 1;

                        // Prepare the common widget variables
                        int widgetLeft = ConsoleWrapper.WindowWidth / 2 + (ConsoleWrapper.WindowWidth % 2) + 1;
                        int widgetWidth = width / 2 - 4;
                        int widgetHeight = height - 7;
                        int widgetTop = posY + 1;

                        // Get the necessary positions
                        int buttonPanelPosY = ConsoleWrapper.WindowHeight - 5;
                        int buttonPanelWidth = widgetWidth + 1 + ConsoleWrapper.WindowWidth % 2;
                        int buttonWidth = buttonPanelWidth / 2 - 1;
                        int buttonHeight = 1;

                        // Settings button
                        int settingsButtonStartPosX = posX + 2;
                        int settingsButtonStartPosY = buttonPanelPosY;
                        int settingsButtonEndPosX = settingsButtonStartPosX + buttonWidth + 1;
                        int settingsButtonEndPosY = settingsButtonStartPosY + buttonHeight + 1;

                        // About button
                        int aboutButtonStartPosX = settingsButtonStartPosX + buttonWidth + 3;
                        int aboutButtonStartPosY = buttonPanelPosY;
                        int aboutButtonEndPosX = aboutButtonStartPosX + buttonWidth + 1;
                        int aboutButtonEndPosY = aboutButtonStartPosY + buttonHeight + 1;

                        // Options
                        int optionsEndX = settingsButtonStartPosX + widgetWidth + 1 + ConsoleWrapper.WindowWidth % 2;
                        int optionsEndY = widgetTop + widgetHeight - 1;

                        // Notifications
                        int notificationsEndY = widgetTop + widgetHeight + 2;

                        // Check the ranges
                        bool isWithinSettings = PointerTools.PointerWithinRange(context, (settingsButtonStartPosX, settingsButtonStartPosY), (settingsButtonEndPosX, settingsButtonEndPosY));
                        bool isWithinAbout = PointerTools.PointerWithinRange(context, (aboutButtonStartPosX, aboutButtonStartPosY), (aboutButtonEndPosX, aboutButtonEndPosY));
                        bool isWithinOptions = PointerTools.PointerWithinRange(context, (settingsButtonStartPosX + 1, widgetTop + 1), (optionsEndX, optionsEndY));
                        bool isWithinNotifications = PointerTools.PointerWithinRange(context, (settingsButtonStartPosX + 1, notificationsEndY), (optionsEndX, notificationsEndY));

                        // If the mouse pointer is within the settings, check for left release
                        if (isWithinSettings)
                        {
                            if (context.ButtonPress == PointerButtonPress.Released && context.Button == PointerButton.Left)
                                SettingsApp.OpenMainPage(Config.MainConfig);
                            render = true;
                        }
                        else if (isWithinAbout)
                        {
                            if (context.ButtonPress == PointerButtonPress.Released && context.Button == PointerButton.Left)
                                OpenAboutBox();
                            render = true;
                        }
                        else if (isWithinOptions)
                        {
                            int currentChoices = choices.Length;
                            if (context.ButtonPress == PointerButtonPress.Released && context.Button == PointerButton.Left || context.ButtonPress == PointerButtonPress.Moved)
                            {
                                int contextPosY = context.Coordinates.y;
                                int finalPos = contextPosY - widgetTop - 1;
                                if (finalPos < currentChoices)
                                {
                                    choiceIdx = finalPos;
                                    if (context.ButtonPress == PointerButtonPress.Released && context.Button == PointerButton.Left)
                                        DoAction(choiceIdx);
                                }
                                render = true;
                            }
                            else if (context.ButtonPress == PointerButtonPress.Scrolled)
                            {
                                if (context.Button == PointerButton.WheelUp)
                                {
                                    choiceIdx--;
                                    if (choiceIdx < 0)
                                        choiceIdx++;
                                    render = true;
                                }
                                else if (context.Button == PointerButton.WheelDown)
                                {
                                    choiceIdx++;
                                    if (choiceIdx >= choices.Length)
                                        choiceIdx--;
                                    render = true;
                                }
                            }
                        }
                        else if (isWithinNotifications)
                        {
                            if (context.ButtonPress == PointerButtonPress.Released && context.Button == PointerButton.Left)
                                NotificationsCli.OpenNotificationsCli();
                            render = true;
                        }
                        if (context.ButtonPress == PointerButtonPress.Moved)
                        {
                            if (isWithinSettings)
                                buttonHighlight = 1;
                            else if (isWithinAbout)
                                buttonHighlight = 2;
                            else
                                buttonHighlight = 0;
                            render = true;
                        }
                    }
                    else if (data?.ConsoleKeyInfo is ConsoleKeyInfo keypress)
                    {
                        render = true;
                        int widgetHeight = ConsoleWrapper.WindowHeight - 10;
                        int currentPage = (choiceIdx - 1) / widgetHeight;
                        int startIndex = widgetHeight * currentPage;
                        int endIndex = widgetHeight * (currentPage + 1);
                        switch (keypress.Key)
                        {
                            case ConsoleKey.DownArrow:
                                if (pageNumber != 1)
                                    break;
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx++;
                                if (choiceIdx >= choices.Length)
                                    choiceIdx--;
                                break;
                            case ConsoleKey.UpArrow:
                                if (pageNumber != 1)
                                    break;
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx--;
                                if (choiceIdx < 0)
                                    choiceIdx++;
                                break;
                            case ConsoleKey.Home:
                                if (pageNumber != 1)
                                    break;
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx = 0;
                                break;
                            case ConsoleKey.End:
                                if (pageNumber != 1)
                                    break;
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx = choices.Length - 1;
                                break;
                            case ConsoleKey.PageUp:
                                if (pageNumber != 1)
                                    break;
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx = startIndex;
                                break;
                            case ConsoleKey.PageDown:
                                if (pageNumber != 1)
                                    break;
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx = endIndex > choices.Length - 1 ? choices.Length - 1 : endIndex + 1;
                                choiceIdx = endIndex == choices.Length - 1 ? endIndex : choiceIdx;
                                break;
                            case ConsoleKey.Tab:
                                if (pageNumber != 1)
                                    break;
                                buttonHighlight++;
                                if (buttonHighlight > 3)
                                    buttonHighlight = 0;
                                break;
                            case ConsoleKey.Enter:
                                if (pageNumber != 1)
                                    break;
                                if (buttonHighlight == 1)
                                    SettingsApp.OpenMainPage(Config.MainConfig);
                                else if (buttonHighlight == 2)
                                    OpenAboutBox();
                                else if (buttonHighlight == 3)
                                    NotificationsCli.OpenNotificationsCli();
                                else
                                    DoAction(choiceIdx);
                                break;
                            case ConsoleKey.LeftArrow:
                                pageNumber--;
                                if (pageNumber <= 0)
                                    pageNumber = 1;
                                break;
                            case ConsoleKey.RightArrow:
                                pageNumber++;
                                if (pageNumber >= maxScreens + 1)
                                    pageNumber = maxScreens;
                                break;
                            case ConsoleKey.Escape:
                                if (keypress.Modifiers == ConsoleModifiers.Shift)
                                {
                                    int answer = InfoBoxButtonsColor.WriteInfoBoxButtons([
                                        new InputChoiceInfo("shutdown", LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_SHUTDOWN")),
                                        new InputChoiceInfo("reboot", LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_RESTART")),
                                        // TODO: NKS_USERS_LOGIN_MODERNLOGON_CLOSE -> "Close"
                                        new InputChoiceInfo("exit", LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_CLOSE")),
                                    ], LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_POWERACTION"));
                                    if (answer == 0)
                                        PowerManager.PowerManage(PowerMode.Shutdown);
                                    else if (answer == 1)
                                        PowerManager.PowerManage(PowerMode.Reboot);
                                    exiting = answer != 2 && answer != -1;
                                }
                                else
                                {
                                    exiting = true;
                                    Login.LogoutRequested = true;
                                }
                                break;
                            case ConsoleKey.S:
                                exiting = true;
                                break;
                            case ConsoleKey.K:
                                InfoBoxModalColor.WriteInfoBoxModal(
                                    KeybindingTools.RenderKeybindingHelpText(Bindings), new InfoBoxSettings()
                                    {
                                        Title = "Available keys",
                                        ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiBoxForeground),
                                        BackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiBoxBackground),
                                    });
                                break;
                            case ConsoleKey.P:
                                AudioCuesTools.PlayThemeMusic();
                                break;
                            default:
                                render = false;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ThemeColorsTools.LoadBackground();
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_CRASH") + $": {ex.Message}", new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error),
                });
            }
            finally
            {
                isOnHomepage = false;
                ScreenTools.UnsetCurrent(homeScreen);
                ThemeColorsTools.LoadBackground();
            }
        }

        private static (InputChoiceInfo, Action)[] PopulateChoices()
        {
            // Variables
            var choices = new List<(InputChoiceInfo, Action)>();

            // Helper function
            void AddChoices(Dictionary<string, Action> choiceActions)
            {
                foreach (var choiceAction in choiceActions)
                {
                    // Sanity checks
                    string key = choiceAction.Key;
                    var action = choiceAction.Value;
                    if (action is null)
                        continue;
                    if (string.IsNullOrEmpty(key))
                        continue;

                    // Add this action
                    var inputChoiceInfo = new InputChoiceInfo($"{choices.Count + 1}", LanguageTools.GetLocalized(key));
                    choices.Add((inputChoiceInfo, action));
                }
            }

            // First, deal with the builtin choices that are added by the core kernel
            AddChoices(choiceActionsBuiltin);

            // Then, deal with the choices that are added by the addons
            AddChoices(choiceActionsAddons);

            // Now, deal with the custom choices that are added by the mods
            AddChoices(choiceActionsCustom);

            // Finally, return the result for the homepage to recognize them
            return [.. choices];
        }

        /// <summary>
        /// Checks to see if the homepage action is registered or not
        /// </summary>
        /// <param name="actionName">Action name to search (case sensitive)</param>
        /// <returns>True if it exists; false otherwise</returns>
        public static bool IsHomepageActionRegistered(string actionName)
        {
            if (string.IsNullOrEmpty(actionName))
                return false;
            if (IsHomepageActionBuiltin(actionName))
                return true;
            var actions = choiceActionsAddons.Union(choiceActionsCustom).Select((kvp) => kvp.Key).ToArray();
            return actions.Contains(actionName);
        }

        /// <summary>
        /// Checks to see if the homepage action is bulitin or not
        /// </summary>
        /// <param name="actionName">Action name to search (case sensitive)</param>
        /// <returns>True if it exists; false otherwise</returns>
        public static bool IsHomepageActionBuiltin(string actionName)
        {
            if (string.IsNullOrEmpty(actionName))
                return false;
            var actions = choiceActionsBuiltin.Select((kvp) => kvp.Key).ToArray();
            return actions.Contains(actionName);
        }

        /// <summary>
        /// Registers a custom action
        /// </summary>
        /// <param name="actionName">Action name (case sensitive)</param>
        /// <param name="action">Action to delegate a specific function to</param>
        /// <exception cref="KernelException"></exception>
        public static void RegisterAction(string actionName, Action? action)
        {
            if (string.IsNullOrEmpty(actionName))
                throw new KernelException(KernelExceptionType.Homepage, LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_EXCEPTION_NEEDSACTIONNAME"));
            if (action is null)
                throw new KernelException(KernelExceptionType.Homepage, LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_EXCEPTION_NEEDSACTION"));
            if (IsHomepageActionRegistered(actionName))
                throw new KernelException(KernelExceptionType.Homepage, LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_EXCEPTION_ACTIONFOUND"));
            choiceActionsCustom.Add(actionName, action);
        }

        /// <summary>
        /// Unregisters a custom action
        /// </summary>
        /// <param name="actionName">Action name to delete (case sensitive)</param>
        /// <exception cref="KernelException"></exception>
        public static void UnregisterAction(string actionName)
        {
            if (string.IsNullOrEmpty(actionName))
                throw new KernelException(KernelExceptionType.Homepage, LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_EXCEPTION_NEEDSACTIONNAME"));
            if (!IsHomepageActionRegistered(actionName))
                throw new KernelException(KernelExceptionType.Homepage, LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_EXCEPTION_ACTIONNOTFOUND"));
            if (IsHomepageActionBuiltin(actionName))
                throw new KernelException(KernelExceptionType.Homepage, LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_EXCEPTION_ACTIONUNREMOVABLE"));
            choiceActionsCustom.Remove(actionName);
        }

        internal static void RegisterBuiltinAction(string actionName, Action? action)
        {
            if (string.IsNullOrEmpty(actionName))
                throw new KernelException(KernelExceptionType.Homepage, LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_EXCEPTION_NEEDSACTIONNAME"));
            if (action is null)
                throw new KernelException(KernelExceptionType.Homepage, LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_EXCEPTION_NEEDSACTION"));
            if (IsHomepageActionRegistered(actionName))
                throw new KernelException(KernelExceptionType.Homepage, LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_EXCEPTION_ACTIONFOUND"));
            choiceActionsAddons.Add(actionName, action);
        }

        internal static void UnregisterBuiltinAction(string actionName)
        {
            if (string.IsNullOrEmpty(actionName))
                throw new KernelException(KernelExceptionType.Homepage, LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_EXCEPTION_NEEDSACTIONNAME"));
            if (!IsHomepageActionRegistered(actionName))
                throw new KernelException(KernelExceptionType.Homepage, LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_EXCEPTION_ACTIONNOTFOUND"));
            if (IsHomepageActionBuiltin(actionName))
                throw new KernelException(KernelExceptionType.Homepage, LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_EXCEPTION_ACTIONUNREMOVABLE"));
            choiceActionsAddons.Remove(actionName);
        }

        private static void OpenAboutBox()
        {
            InfoBoxModalColor.WriteInfoBoxModal(
                LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_ABOUT_DESC") + "\n\n" +
                LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_VERSION") + $": {KernelReleaseInfo.VersionFullStr}" + "\n" +
                LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_ABOUT_MODAPI") + $": {KernelReleaseInfo.ApiVersion}" + "\n\n" +
                LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_ABOUT_COPYRIGHT") + " - https://aptivi.github.io", new InfoBoxSettings()
                {
                    Title = LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_ABOUTNITROCID"),
                }
            );
        }

        internal static List<WidgetRenderInfo[]> GetHomepagePages()
        {
            // File path to the homepage pages
            string pathToPages = PathsManagement.HomepagePagesPath;
            if (!FilesystemTools.FolderExists(pathToPages))
                FilesystemTools.MakeDirectory(pathToPages);

            // Enumerate the homepage pages
            string[] homepagePageFiles = Directory.GetFiles(pathToPages, "*.json");
            List<WidgetRenderInfo[]> homepagePages = [];
            foreach (var homepagePageFile in homepagePageFiles)
            {
                // Parse the file and convert it to an array of render info instances
                WidgetRenderInfo[] renderInfos = WidgetCanvasTools.GetRenderInfosFromFile(homepagePageFile);

                // Add the page
                homepagePages.Add(renderInfos);
            }

            // Return the result
            return homepagePages;
        }

        private static string RenderHomepagePage(bool refreshWasDone, int pageNumber, List<WidgetRenderInfo[]> canvases, (InputChoiceInfo, Action)[] choices, int choiceIdx, int buttonHighlight)
        {
            int actualScreenNum = pageNumber - 2;
            var builder = new StringBuilder();

            // Clear the console
            builder.Append(ConsoleClearing.GetClearWholeScreenSequence());

            if (actualScreenNum < 0)
            {
                // Prepare the common variables
                int height = ConsoleWrapper.WindowHeight - 4;
                int width = ConsoleWrapper.WindowWidth - 8;
                int posX = ConsoleWrapper.WindowWidth / 2 - width / 2 - 1;
                int posY = ConsoleWrapper.WindowHeight / 2 - height / 2 - 1;

                // Prepare the common widget variables
                int widgetLeft = ConsoleWrapper.WindowWidth / 2 + (ConsoleWrapper.WindowWidth % 2) + 1;
                int widgetWidth = width / 2 - 4;
                int widgetHeight = height - 7;
                int widgetTop = posY + 1;

                // Prepare the widget
                var widget =
                    WidgetTools.CheckWidget(Config.MainConfig.HomepageWidget) ?
                    WidgetTools.GetWidget(Config.MainConfig.HomepageWidget) :
                    WidgetTools.GetWidget(nameof(AnalogClock));
                if (refreshWasDone)
                {
                    string widgetInit = widget.Initialize(widgetLeft + 1, widgetTop + 1, widgetWidth, widgetHeight);
                    builder.Append(widgetInit);
                }

                // Make a master border
                string displayName =
                    string.IsNullOrWhiteSpace(UserManagement.CurrentUser.FullName) ?
                    UserManagement.CurrentUser.Username :
                    UserManagement.CurrentUser.FullName;
                var masterBorder = new BoxFrame()
                {
                    Left = posX,
                    Top = posY,
                    Width = width,
                    Height = height,
                    Text = LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_HEADER").FormatString(displayName),
                };
                builder.Append(masterBorder.Render());

                // Show bindings
                var keybindingSequenceBuilder = new StringBuilder();
                keybindingSequenceBuilder.Append(masterBorder.Settings.BorderRightHorizontalIntersectionEnabled ? $"{ColorTools.RenderSetConsoleColor(masterBorder.FrameColor)}{masterBorder.Settings.BorderRightHorizontalIntersectionChar} " : "");
                keybindingSequenceBuilder.Append($"{ColorTools.RenderSetConsoleColor(masterBorder.TitleColor)}K: {LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_KEYBINDINGS")}");
                keybindingSequenceBuilder.Append(masterBorder.Settings.BorderLeftHorizontalIntersectionEnabled ? $"{ColorTools.RenderSetConsoleColor(masterBorder.FrameColor)} {masterBorder.Settings.BorderLeftHorizontalIntersectionChar}" : "");
                string keybindingSequence = keybindingSequenceBuilder.ToString();
                int keybindingSequenceWidth = ConsoleChar.EstimateCellWidth(keybindingSequence);
                int keybindingSequencePanelWidth = width - 2;
                if (keybindingSequenceWidth <= keybindingSequencePanelWidth)
                {
                    int sequenceLeft = posX + width - keybindingSequenceWidth;
                    builder.Append(TextWriterWhereColor.RenderWherePlain(keybindingSequence, sequenceLeft, posY + height + 1));
                }

                // Make a border for a widget and the first three RSS feeds (if the addon is installed)
                int rssTop = widgetTop + widgetHeight + 2;
                int rssHeight = 3;
                var widgetBorder = new Border()
                {
                    Left = widgetLeft,
                    Top = widgetTop,
                    Width = widgetWidth,
                    Height = widgetHeight,
                };
                var rssBorder = new Border()
                {
                    Left = widgetLeft,
                    Top = rssTop,
                    Width = widgetWidth,
                    Height = rssHeight,
                };
                builder.Append(
                    widgetBorder.Render() +
                    rssBorder.Render()
                );

                // Render the widget
                string widgetSeq = widget.Render(widgetLeft + 1, widgetTop + 1, widgetWidth, widgetHeight);
                builder.Append(widgetSeq);

                // Render the first three RSS feeds
                if (Config.MainConfig.EnableHomepageRssFeed)
                {
                    int rssFeedLeft = widgetLeft + 1;
                    int rssFeedTop = rssTop + 1;
                    string rssSequence = "";
                    bool needsWrapping = true;
                    try
                    {
                        if (!Config.MainConfig.ShowHeadlineOnLogin)
                            rssSequence = LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_NEEDSHEADLINES");
                        else if (!WidgetTools.IsWidgetBuiltin("RssFeeds"))
                            rssSequence = LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_RSSFEED_NEEDSADDON");
                        else
                        {
                            var feedWidget = WidgetTools.GetWidget("RssFeeds");
                            needsWrapping = false;
                            rssSequence = feedWidget.Render(rssFeedLeft, rssFeedTop, widgetWidth, 3);
                        }
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", vars: [ex.Message]);
                        DebugWriter.WriteDebugStackTrace(ex);
                        rssSequence = LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RSS_FETCHFAILED");
                    }

                    // Render the RSS feed sequence or an error message
                    if (needsWrapping)
                    {
                        rssSequence = new BoundedText()
                        {
                            Left = rssFeedLeft,
                            Top = rssFeedTop,
                            Width = widgetWidth,
                            Height = 3,
                            Text = rssSequence,
                        }.Render();
                    }
                    builder.Append(rssSequence);
                }

                // Populate the button positions
                int buttonPanelPosY = ConsoleWrapper.WindowHeight - 5;
                int buttonPanelWidth = widgetWidth + 1 + ConsoleWrapper.WindowWidth % 2;
                int buttonWidth = buttonPanelWidth / 2 - 1;
                int buttonHeight = 1;
                int settingsButtonPosX = posX + 2;
                int aboutButtonPosX = settingsButtonPosX + buttonWidth + 3;

                // Populate the settings button
                var foregroundSettings = buttonHighlight == 2 ? ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator) : ThemeColorsTools.GetColor(ThemeColorType.Separator);
                var foregroundSettingsText = buttonHighlight == 2 ? ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator) : ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
                var settingsBorder = new Border()
                {
                    Left = settingsButtonPosX,
                    Top = buttonPanelPosY,
                    Width = buttonWidth,
                    Height = buttonHeight,
                    Color = foregroundSettings,
                };
                var settingsText = new AlignedText()
                {
                    Left = settingsButtonPosX + 1,
                    Top = buttonPanelPosY + 1,
                    Text = LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_SETTINGS"),
                    ForegroundColor = foregroundSettingsText,
                    Width = buttonWidth,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    },
                };
                builder.Append(
                    settingsBorder.Render() +
                    settingsText.Render()
                );

                // Populate the about button
                var foregroundAbout = buttonHighlight == 3 ? ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator) : ThemeColorsTools.GetColor(ThemeColorType.Separator);
                var foregroundAboutText = buttonHighlight == 3 ? ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator) : ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
                var aboutBorder = new Border()
                {
                    Left = aboutButtonPosX,
                    Top = buttonPanelPosY,
                    Width = buttonWidth * 2 + 2 == buttonPanelWidth ? buttonWidth - 1 : buttonWidth,
                    Height = buttonHeight,
                    Color = foregroundAbout,
                };
                var aboutText = new AlignedText()
                {
                    Left = aboutButtonPosX + 1,
                    Top = buttonPanelPosY + 1,
                    Text = LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_ABOUT"),
                    ForegroundColor = foregroundAboutText,
                    Width = buttonWidth,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    },
                };
                builder.Append(
                    aboutBorder.Render() +
                    aboutText.Render()
                );

                // Populate the available options
                var availableChoices = choices.Select((tuple) => tuple.Item1).ToArray();
                var choicesBorder = new Border()
                {
                    Left = settingsButtonPosX,
                    Top = widgetTop,
                    Width = buttonPanelWidth,
                    Height = widgetHeight - 1,
                    Color = ThemeColorsTools.GetColor(buttonHighlight == 0 ? ThemeColorType.TuiPaneSelectedSeparator : ThemeColorType.Separator),
                };
                var choicesSelection = new Selection(availableChoices)
                {
                    Left = settingsButtonPosX + 1,
                    Top = widgetTop + 1,
                    CurrentSelection = choiceIdx,
                    AltChoicePos = availableChoices.Length,
                    Width = buttonPanelWidth,
                    Height = widgetHeight - 1,
                    Settings = new()
                    {
                        OptionColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText),
                        SelectedOptionColor = ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator),
                    },
                };
                builder.Append(
                    choicesBorder.Render() +
                    choicesSelection.Render()
                );

                // Render the notification icons widget
                int notificationsY = widgetTop + widgetHeight + 1;
                var notificationsWidget = WidgetTools.GetWidget(nameof(NotificationIcons));
                var notificationsBorder = new BoxFrame()
                {
                    Left = settingsButtonPosX,
                    Top = notificationsY,
                    Width = buttonPanelWidth,
                    Height = buttonHeight,
                    FrameColor = ThemeColorsTools.GetColor(buttonHighlight == 1 ? ThemeColorType.TuiPaneSelectedSeparator : ThemeColorType.Separator),
                    Text = LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_NOTIFICATIONS")
                };
                notificationsWidget.Options["alignment"] = TextAlignment.Left;
                builder.Append(
                    notificationsBorder.Render() +
                    notificationsWidget.Render(settingsButtonPosX + 1, notificationsY + 1, buttonPanelWidth - 2, buttonHeight)
                );

                // Return the resulting homepage
                return builder.ToString();
            }
            else
            {
                var canvas = canvases[actualScreenNum];
                var renderedCanvas = WidgetCanvasTools.RenderFromInfos(canvas);
                builder.Append(renderedCanvas);
            }
            return builder.ToString();
        }
    }
}
