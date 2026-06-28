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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Languages;
using Nitrocid.Misc.Interactives;
using Nitrocid.Users;
using Nitrocid.Users.Login;
using Nitrocid.Users.Login.Widgets;
using Nitrocid.Users.Login.Widgets.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Colorimetry;
using Colorimetry.Data;
using Terminaux.Inputs;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;
using Terminaux.Writer.CyclicWriters.Graphical;
using Nitrocid.Files;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Base.Extensions;

namespace Nitrocid.Shell.Homepage
{
    /// <summary>
    /// The Nitrocid Homepage tools
    /// </summary>
    public static class HomepageTools
    {
        internal static bool isHomepageEnabled = true;
        internal static bool isHomepageWidgetEnabled = true;
        internal static bool isHomepageRssFeedEnabled = true;
        internal static string homepageWidgetName = nameof(AnalogClock);
        private static bool isOnHomepage = false;
        private static readonly Dictionary<string, Action> choiceActionsAddons = [];
        private static readonly Dictionary<string, Action> choiceActionsCustom = [];
        private static readonly Dictionary<string, Action> choiceActionsBuiltin = new()
        {
            { LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_FILEMANAGER"), FilesystemTools.OpenFileManagerTui },
            { LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_ALARMMANAGER"), AlarmCli.OpenAlarmCli },
            { LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_NOTIFICATIONS"), NotificationsCli.OpenNotificationsCli },
            { LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_TASKMANAGER"), TaskManagerCli.OpenTaskManagerCli },
        };
        private static readonly Keybinding[] bindings =
        [
            // Keyboard
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_EXECUTE"), ConsoleKey.Enter),
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_LOGOUT"), ConsoleKey.Escape),
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_SHELL"), ConsoleKey.S),
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_KEYBINDINGS"), ConsoleKey.K),
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_SWITCH"), ConsoleKey.Tab),

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
            int choiceIdx = 0;
            int buttonHighlight = 0;
            var choices = PopulateChoices();
            var homeScreen = new Screen
            {
                CycleFrequency = 1000
            };

            try
            {
                // Create a screen for the homepage
                var homeScreenBuffer = new ScreenPart();
                ScreenTools.SetCurrent(homeScreen);
                ScreenTools.SetCurrentCyclic(homeScreen);
                KernelColorTools.LoadBackground();

                // Prepare the widget
                var widget =
                    WidgetTools.CheckWidget(Config.MainConfig.HomepageWidget) ?
                    WidgetTools.GetWidget(Config.MainConfig.HomepageWidget) :
                    WidgetTools.GetWidget(nameof(AnalogClock));
                if (Config.MainConfig.EnableHomepageWidgets)
                {
                    int widgetLeft = ConsoleWrapper.WindowWidth / 2 + ConsoleWrapper.WindowWidth % 2;
                    int widgetWidth = ConsoleWrapper.WindowWidth / 2 - 4;
                    int widgetHeight = ConsoleWrapper.WindowHeight - 11;
                    int widgetTop = 2;
                    string widgetInit = widget.Initialize(widgetLeft + 1, widgetTop + 1, widgetWidth, widgetHeight);
                    TextWriterRaw.WriteRaw(widgetInit);
                }

                // Check to see if we have the feed addon installed
                bool feedAddonInstalled = AddonTools.GetAddon(InterAddonTranslations.GetAddonName(KnownAddons.ExtrasRssShell)) is not null;
                string feedErrorMessage = "";
                (string feedTitle, string articleTitle)[]? articles = null;

                // Now, render the homepage
                homeScreenBuffer.AddDynamicText(() =>
                {
                    var builder = new StringBuilder();

                    // Make a master border
                    var masterBorder = new Border()
                    {
                        Left = 0,
                        Top = 1,
                        Width = ConsoleWrapper.WindowWidth - 2,
                        Height = ConsoleWrapper.WindowHeight - 4,
                        Color = KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator),
                    };
                    builder.Append(masterBorder.Render());

                    // Show username at the top
                    string displayName =
                        string.IsNullOrWhiteSpace(UserManagement.CurrentUser.FullName) ?
                        UserManagement.CurrentUser.Username :
                        UserManagement.CurrentUser.FullName;
                    var greeting = new AlignedText()
                    {
                        Top = 0,
                        Text = LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_HEADER").FormatString(displayName),
                        Settings = new()
                        {
                            Alignment = TextAlignment.Middle
                        }
                    };
                    builder.Append(greeting.Render());

                    // Show bindings
                    var keybindings = new Keybindings()
                    {
                        KeybindingList = bindings,
                        BuiltinColor = KernelColorTools.GetColor(KernelColorType.TuiKeyBindingBuiltin),
                        BuiltinForegroundColor = KernelColorTools.GetColor(KernelColorType.TuiKeyBindingBuiltinForeground),
                        BuiltinBackgroundColor = KernelColorTools.GetColor(KernelColorType.TuiKeyBindingBuiltinBackground),
                        OptionColor = KernelColorTools.GetColor(KernelColorType.TuiKeyBindingOption),
                        OptionForegroundColor = KernelColorTools.GetColor(KernelColorType.TuiOptionForeground),
                        OptionBackgroundColor = KernelColorTools.GetColor(KernelColorType.TuiOptionBackground),
                        Width = ConsoleWrapper.WindowWidth - 1,
                    };
                    builder.Append(RendererTools.RenderRenderable(keybindings, new(0, ConsoleWrapper.WindowHeight - 1)));

                    // Make a border for a widget and the first three RSS feeds (if the addon is installed)
                    int widgetLeft = ConsoleWrapper.WindowWidth / 2 + ConsoleWrapper.WindowWidth % 2;
                    int widgetWidth = ConsoleWrapper.WindowWidth / 2 - 4;
                    int widgetHeight = ConsoleWrapper.WindowHeight - 11;
                    int widgetTop = 2;
                    int rssTop = widgetTop + widgetHeight + 2;
                    int rssHeight = 3;
                    var widgetBorder = new Border()
                    {
                        Left = widgetLeft,
                        Top = widgetTop,
                        Width = widgetWidth,
                        Height = widgetHeight,
                        Color = KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator),
                    };
                    var rssBorder = new Border()
                    {
                        Left = widgetLeft,
                        Top = rssTop,
                        Width = widgetWidth,
                        Height = rssHeight,
                        Color = KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator),
                    };
                    builder.Append(
                        widgetBorder.Render() +
                        rssBorder.Render()
                    );

                    // Render the widget
                    if (Config.MainConfig.EnableHomepageWidgets)
                    {
                        string widgetSeq = widget.Render(widgetLeft + 1, widgetTop + 1, widgetWidth, widgetHeight);
                        builder.Append(widgetSeq);
                    }

                    // Render the first three RSS feeds
                    if (Config.MainConfig.EnableHomepageRssFeed)
                    {
                        int rssFeedLeft = widgetLeft + 1;
                        int rssFeedTop = rssTop + 1;
                        string rssSequence = "";
                        try
                        {
                            if (!Config.MainConfig.ShowHeadlineOnLogin)
                                rssSequence = LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_NEEDSHEADLINES");
                            else if (!feedAddonInstalled)
                                rssSequence = LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_RSSFEED_NEEDSADDON");
                            else
                            {
                                articles ??= ((string feedTitle, string articleTitle)[]?)InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasRssShell, "GetArticles", "Nitrocid.Extras.RssShell.Tools.RSSShellTools", Config.MainConfig.RssHeadlineUrl) ?? [];
                                if (articles is not null && articles.Length > 0)
                                {
                                    var headlines = new StringBuilder();
                                    foreach (var (feedTitle, articleTitle) in articles)
                                    {
                                        string finalFeedEntry = $"{articleTitle}";
                                        finalFeedEntry = finalFeedEntry.Truncate(widgetWidth);
                                        headlines.AppendLine(finalFeedEntry);
                                    }
                                    rssSequence = headlines.ToString();
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(feedErrorMessage))
                                        feedErrorMessage = LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_NOARTICLES");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", vars: [ex.Message]);
                            DebugWriter.WriteDebugStackTrace(ex);
                            feedErrorMessage = LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RSS_FETCHFAILED");
                        }
                        if (!string.IsNullOrEmpty(feedErrorMessage))
                        {
                            rssSequence = feedErrorMessage;
                            articles = [];
                        }

                        // Render the RSS feed sequence or an error message
                        rssSequence = new BoundedText()
                        {
                            Left = rssFeedLeft,
                            Top = rssFeedTop,
                            Width = widgetWidth,
                            Height = 3,
                            Text = rssSequence,
                        }.Render();
                        builder.Append(rssSequence);
                    }

                    // Populate the button positions
                    int buttonPanelPosY = ConsoleWrapper.WindowHeight - 5;
                    int buttonPanelWidth = widgetLeft - 4;
                    int buttonWidth = buttonPanelWidth / 2 - 2;
                    int buttonHeight = 1;
                    int settingsButtonPosX = 2;
                    int aboutButtonPosX = settingsButtonPosX + buttonWidth + 3;

                    // Populate the settings button
                    var foregroundSettings = buttonHighlight == 1 ? new Color(ConsoleColors.Black) : KernelColorTools.GetColor(KernelColorType.TuiPaneSeparator);
                    var backgroundSettings = buttonHighlight == 1 ? KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator) : ConsoleColoring.CurrentBackgroundColor;
                    var foregroundSettingsText = buttonHighlight == 1 ? new Color(ConsoleColors.Black) : KernelColorTools.GetColor(KernelColorType.NeutralText);
                    var settingsBorder = new Border()
                    {
                        Left = settingsButtonPosX,
                        Top = buttonPanelPosY,
                        Width = buttonWidth,
                        Height = buttonHeight,
                        Color = foregroundSettings,
                        BackgroundColor = backgroundSettings,
                    };
                    var settingsText = new AlignedText()
                    {
                        Left = settingsButtonPosX + 1,
                        Top = buttonPanelPosY + 1,
                        Text = LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_SETTINGS"),
                        ForegroundColor = foregroundSettingsText,
                        BackgroundColor = backgroundSettings,
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
                    var foregroundAbout = buttonHighlight == 2 ? new Color(ConsoleColors.Black) : KernelColorTools.GetColor(KernelColorType.TuiPaneSeparator);
                    var backgroundAbout = buttonHighlight == 2 ? KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator) : ConsoleColoring.CurrentBackgroundColor;
                    var foregroundAboutText = buttonHighlight == 2 ? new Color(ConsoleColors.Black) : KernelColorTools.GetColor(KernelColorType.NeutralText);
                    var aboutBorder = new Border()
                    {
                        Left = aboutButtonPosX,
                        Top = buttonPanelPosY,
                        Width = aboutButtonPosX + buttonWidth == buttonPanelWidth ? buttonWidth + 1 : buttonWidth,
                        Height = buttonHeight,
                        Color = foregroundAbout,
                        BackgroundColor = backgroundAbout,
                    };
                    var aboutText = new AlignedText()
                    {
                        Left = aboutButtonPosX + 1,
                        Top = buttonPanelPosY + 1,
                        Text = LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_ABOUT"),
                        ForegroundColor = foregroundAboutText,
                        BackgroundColor = backgroundAbout,
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
                        Width = widgetWidth - 1 + ConsoleWrapper.WindowWidth % 2,
                        Height = widgetHeight + 2,
                        Color = KernelColorTools.GetColor(buttonHighlight == 0 ? KernelColorType.TuiPaneSelectedSeparator : KernelColorType.TuiPaneSeparator),
                    };
                    var choicesSelection = new Selection(availableChoices)
                    {
                        Left = settingsButtonPosX + 1,
                        Top = widgetTop + 1,
                        CurrentSelection = choiceIdx,
                        AltChoicePos = availableChoices.Length,
                        Height = widgetHeight + 2,
                        Width = widgetWidth - 1 + ConsoleWrapper.WindowWidth % 2,
                        Settings = new()
                        {
                            OptionColor = KernelColorTools.GetColor(KernelColorType.NeutralText),
                            SelectedOptionColor = KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator),
                        },
                    };
                    builder.Append(
                        choicesBorder.Render() +
                        choicesSelection.Render()
                    );

                    // Return the resulting homepage
                    return builder.ToString();
                });
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
                // Main loop
                ScreenTools.StartCyclicScreen();
                bool exiting = false;
                bool hold = false;
                while (!exiting)
                {
                    if (hold)
                    {
                        ScreenTools.SetCurrent(homeScreen);
                        ScreenTools.SetCurrentCyclic(homeScreen);
                        ScreenTools.StartCyclicScreen();
                    }
                    ScreenTools.Render();

                    // Render and wait for input for a second
                    var data = Input.ReadPointerOrKey();

                    // Read the available input
                    if (data?.PointerEventContext is PointerEventContext context)
                    {
                        // Get the necessary positions
                        int buttonPanelPosY = ConsoleWrapper.WindowHeight - 5;
                        int buttonPanelWidth = ConsoleWrapper.WindowWidth / 2 - 5 + ConsoleWrapper.WindowWidth % 2;
                        int buttonWidth = buttonPanelWidth / 2 - 2;
                        int buttonHeight = 1;
                        int settingsButtonStartPosX = 2;
                        int settingsButtonStartPosY = ConsoleWrapper.WindowHeight - 5;
                        int settingsButtonEndPosX = settingsButtonStartPosX + buttonWidth + 1;
                        int settingsButtonEndPosY = settingsButtonStartPosY + buttonHeight + 1;
                        int aboutButtonStartPosX = buttonWidth + 2;
                        int aboutButtonStartPosY = ConsoleWrapper.WindowHeight - 5;
                        int aboutButtonEndPosX = aboutButtonStartPosX + buttonWidth + 1;
                        int aboutButtonEndPosY = aboutButtonStartPosY + buttonHeight + 1;
                        int widgetTop = 3;
                        int widgetWidth = ConsoleWrapper.WindowWidth / 2 - 4;
                        int widgetHeight = ConsoleWrapper.WindowHeight - 9;
                        int optionsEndX = settingsButtonStartPosX + widgetWidth - 1 + ConsoleWrapper.WindowWidth % 2;
                        int optionsEndY = widgetTop + widgetHeight;

                        // Check the ranges
                        bool isWithinSettings = PointerTools.PointerWithinRange(context, (settingsButtonStartPosX, settingsButtonStartPosY), (settingsButtonEndPosX, settingsButtonEndPosY));
                        bool isWithinAbout = PointerTools.PointerWithinRange(context, (aboutButtonStartPosX, aboutButtonStartPosY), (aboutButtonEndPosX, aboutButtonEndPosY));
                        bool isWithinOptions = PointerTools.PointerWithinRange(context, (settingsButtonStartPosX + 1, widgetTop), (optionsEndX, optionsEndY));

                        // If the mouse pointer is within the settings, check for left release
                        if (isWithinSettings)
                        {
                            if (context.ButtonPress == PointerButtonPress.Released && context.Button == PointerButton.Left)
                            {
                                ScreenTools.StopCyclicScreen();
                                hold = true;
                                SettingsApp.OpenMainPage(Config.MainConfig);
                                homeScreen.RequireRefresh();
                            }
                        }
                        else if (isWithinAbout)
                        {
                            if (context.ButtonPress == PointerButtonPress.Released && context.Button == PointerButton.Left)
                                OpenAboutBox();
                        }
                        else if (isWithinOptions)
                        {
                            int currentPage = choiceIdx / widgetHeight;
                            int startIndex = widgetHeight * currentPage;
                            if ((context.ButtonPress == PointerButtonPress.Released && context.Button == PointerButton.Left) || context.ButtonPress == PointerButtonPress.Moved)
                            {
                                int contextPosY = context.Coordinates.y;
                                int finalChoiceIdx = startIndex + contextPosY - widgetTop;
                                if (finalChoiceIdx < choices.Length)
                                {
                                    choiceIdx = finalChoiceIdx;
                                    if (context.ButtonPress == PointerButtonPress.Released && context.Button == PointerButton.Left)
                                    {
                                        ScreenTools.StopCyclicScreen();
                                        hold = true;
                                        DoAction(choiceIdx);
                                        homeScreen.RequireRefresh();
                                    }
                                }
                            }
                            else if (context.ButtonPress == PointerButtonPress.Scrolled)
                            {
                                if (context.Button == PointerButton.WheelUp)
                                {
                                    choiceIdx -= 3;
                                    if (choiceIdx < 0)
                                        choiceIdx = 0;
                                }
                                else if (context.Button == PointerButton.WheelDown)
                                {
                                    choiceIdx += 3;
                                    if (choiceIdx >= choices.Length)
                                        choiceIdx = choices.Length - 1;
                                }
                            }
                        }
                        if (context.ButtonPress == PointerButtonPress.Moved)
                        {
                            if (isWithinSettings)
                                buttonHighlight = 1;
                            else if (isWithinAbout)
                                buttonHighlight = 2;
                            else
                                buttonHighlight = 0;
                        }
                    }
                    else if (data?.ConsoleKeyInfo is ConsoleKeyInfo keypress)
                    {
                        int widgetHeight = ConsoleWrapper.WindowHeight - 9;
                        switch (keypress.Key)
                        {
                            case ConsoleKey.DownArrow:
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx++;
                                if (choiceIdx >= choices.Length)
                                    choiceIdx = 0;
                                break;
                            case ConsoleKey.UpArrow:
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx--;
                                if (choiceIdx < 0)
                                    choiceIdx = choices.Length - 1;
                                break;
                            case ConsoleKey.Home:
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx = 0;
                                break;
                            case ConsoleKey.End:
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx = choices.Length - 1;
                                break;
                            case ConsoleKey.PageUp:
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx -= widgetHeight;
                                if (choiceIdx < 0)
                                    choiceIdx = 0;
                                break;
                            case ConsoleKey.PageDown:
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx += widgetHeight;
                                if (choiceIdx > choices.Length - 1)
                                    choiceIdx = choices.Length - 1;
                                break;
                            case ConsoleKey.Tab:
                                buttonHighlight++;
                                if (buttonHighlight > 2)
                                    buttonHighlight = 0;
                                break;
                            case ConsoleKey.Enter:
                                if (buttonHighlight == 1)
                                {
                                    ScreenTools.StopCyclicScreen();
                                    hold = true;
                                    SettingsApp.OpenMainPage(Config.MainConfig);
                                    homeScreen.RequireRefresh();
                                }
                                else if (buttonHighlight == 2)
                                    OpenAboutBox();
                                else
                                {
                                    ScreenTools.StopCyclicScreen();
                                    hold = true;
                                    DoAction(choiceIdx);
                                    homeScreen.RequireRefresh();
                                }
                                break;
                            case ConsoleKey.Escape:
                                exiting = true;
                                Login.LogoutRequested = true;
                                break;
                            case ConsoleKey.S:
                                exiting = true;
                                break;
                            case ConsoleKey.K:
                                InfoBoxModalColor.WriteInfoBoxModal(
                                    KeybindingTools.RenderKeybindingHelpText(bindings), new InfoBoxSettings()
                                    {
                                        ForegroundColor = KernelColorTools.GetColor(KernelColorType.TuiBoxForeground),
                                        BackgroundColor = KernelColorTools.GetColor(KernelColorType.TuiBoxBackground),
                                        Title = LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_AVAILABLEKEYS"),
                                    });
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KernelColorTools.LoadBackground();
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_CRASH") + $": {ex.Message}", new InfoBoxSettings()
                {
                    ForegroundColor = KernelColorTools.GetColor(KernelColorType.Error),
                });
            }
            finally
            {
                isOnHomepage = false;
                ScreenTools.StopCyclicScreen();
                ScreenTools.UnsetCurrentCyclic();
                ScreenTools.UnsetCurrent(homeScreen);
                KernelColorTools.LoadBackground();
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
                LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_VERSION") + $": {KernelMain.VersionFullStr}" + "\n" +
                LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_ABOUT_MODAPI") + $": {KernelMain.ApiVersion}" + "\n\n" +
                LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_ABOUT_COPYRIGHT") + " - https://aptivi.github.io", new InfoBoxSettings()
                {
                    Title = LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_ABOUTNITROCID"),
                }
            );
        }
    }
}
