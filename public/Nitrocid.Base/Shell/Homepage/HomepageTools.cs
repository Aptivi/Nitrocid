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

using Terminaux.Colors.Themes.Colors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Textify.General;
using Nitrocid.Base.Files;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Configuration.Settings;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;
using Nitrocid.Base.Misc.Interactives;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Misc.Audio;
using Nitrocid.Base.Users.Login.Widgets;
using Nitrocid.Base.Users.Login.Widgets.Implementations;
using Nitrocid.Base.Users.Login;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Power;


#if NKS_EXTENSIONS
using Nitrocid.Base.Kernel.Extensions;
#endif

namespace Nitrocid.Base.Shell.Homepage
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
        private static bool isThemeMusicPlaying = false;
        private static readonly KernelThread themeMusicThread = new("Theme Music Thread", true, HandleThemeMusic);
        private static readonly Dictionary<string, Action> choiceActionsAddons = [];
        private static readonly Dictionary<string, Action> choiceActionsCustom = [];
        private static readonly Dictionary<string, Action> choiceActionsBuiltin = new()
        {
            { /* Localizable */ "NKS_SHELL_HOMEPAGE_FILEMANAGER", FilesystemTools.OpenFileManagerTui },
            { /* Localizable */ "NKS_SHELL_HOMEPAGE_ALARMMANAGER", AlarmCli.OpenAlarmCli },
            { /* Localizable */ "NKS_SHELL_HOMEPAGE_NOTIFICATIONS", NotificationsCli.OpenNotificationsCli },
            { /* Localizable */ "NKS_SHELL_HOMEPAGE_TASKMANAGER", TaskManagerCli.OpenTaskManagerCli },
        };

        private static Keybinding[] Bindings =>
        [
            // Keyboard
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_EXECUTE"), ConsoleKey.Enter),
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_LOGOUT"), ConsoleKey.Escape),
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_SHUTDOWN"), ConsoleKey.Escape, ConsoleModifiers.Shift),
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_SHELL"), ConsoleKey.S),
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_KEYBINDINGS"), ConsoleKey.K),
            new(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_KEYBINDING_SWITCH"), ConsoleKey.Tab),
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

            try
            {
                // Create a screen for the homepage
                var homeScreenBuffer = new ScreenPart();
                string rssSequence = "";
                ScreenTools.SetCurrent(homeScreen);
                ThemeColorsTools.LoadBackground();

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
                        Color = ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator),
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
                        KeybindingList = Bindings,
                        BuiltinColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltin),
                        BuiltinForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltinForeground),
                        BuiltinBackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingBuiltinBackground),
                        OptionColor = ThemeColorsTools.GetColor(ThemeColorType.TuiKeyBindingOption),
                        OptionForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiOptionForeground),
                        OptionBackgroundColor = ThemeColorsTools.GetColor(ThemeColorType.TuiOptionBackground),
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
                        Color = ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator),
                    };
                    var rssBorder = new Border()
                    {
                        Left = widgetLeft,
                        Top = rssTop,
                        Width = widgetWidth,
                        Height = rssHeight,
                        Color = ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator),
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
                        if (string.IsNullOrEmpty(rssSequence))
                        {
                            var rssSequenceBuilder = new StringBuilder();
                            try
                            {
                                if (!Config.MainConfig.ShowHeadlineOnLogin)
                                    rssSequenceBuilder.Append(LanguageTools.GetLocalized("NKS_SHELL_HOMEPAGE_NEEDSHEADLINES"));
                                else
                                {
#if NKS_EXTENSIONS
                                    var addonType = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasRssShell, "Nitrocid.Extras.RssShell.Tools.RSSShellTools");
                                    var feedsObject = InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasRssShell, "GetArticles", addonType, Config.MainConfig.RssHeadlineUrl);
                                    bool found = false;
                                    if (feedsObject is (string feedTitle, string articleTitle)[] feeds)
                                    {
                                        for (int i = 0; i < 3; i++)
                                        {
                                            if (i >= feeds.Length)
                                                break;
                                            (string _, string articleTitle) = feeds[i];
                                            rssSequenceBuilder.AppendLine(articleTitle);
                                            found = true;
                                        }
                                    }
                                    if (!found)
                                        rssSequenceBuilder.Append(LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_RSSFEED_NOFEED"));
#else
                                    throw new KernelException(KernelExceptionType.AddonManagement, LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RSS_LATESTNEWS_NEEDSADDON"));
#endif
                                }
                            }
                            catch (KernelException ex) when (ex.ExceptionType == KernelExceptionType.AddonManagement)
                            {
                                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", vars: [ex.Message]);
                                DebugWriter.WriteDebugStackTrace(ex);
                                rssSequenceBuilder.Append(LanguageTools.GetLocalized("NKS_USERS_LOGIN_MODERNLOGON_RSSFEED_NEEDSADDON"));
                            }
                            catch (Exception ex)
                            {
                                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", vars: [ex.Message]);
                                DebugWriter.WriteDebugStackTrace(ex);
                                rssSequenceBuilder.Append(LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RSS_FETCHFAILED"));
                            }
                            rssSequence = rssSequenceBuilder.ToString();
                        }

                        // Render the RSS feed sequence
                        var sequences = rssSequence.GetWrappedSentencesByWords(widgetWidth);
                        for (int i = 0; i < 3; i++)
                        {
                            if (i >= sequences.Length)
                                break;
                            string sequence = sequences[i];
                            builder.Append(CsiSequences.GenerateCsiCursorPosition(widgetLeft + 2, rssTop + 2 + i));
                            builder.Append(sequence);
                        }
                    }

                    // Populate the button positions
                    int buttonPanelPosY = ConsoleWrapper.WindowHeight - 5;
                    int buttonPanelWidth = widgetLeft - 4;
                    int buttonWidth = buttonPanelWidth / 2 - 2;
                    int buttonHeight = 1;
                    int settingsButtonPosX = 2;
                    int aboutButtonPosX = settingsButtonPosX + buttonWidth + 3;

                    // Populate the settings button
                    var foregroundSettings = buttonHighlight == 1 ? new Color(ConsoleColors.Black) : ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSeparator);
                    var backgroundSettings = buttonHighlight == 1 ? ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator) : ColorTools.CurrentBackgroundColor;
                    var foregroundSettingsText = buttonHighlight == 1 ? new Color(ConsoleColors.Black) : ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
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
                    var foregroundAbout = buttonHighlight == 2 ? new Color(ConsoleColors.Black) : ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSeparator);
                    var backgroundAbout = buttonHighlight == 2 ? ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator) : ColorTools.CurrentBackgroundColor;
                    var foregroundAboutText = buttonHighlight == 2 ? new Color(ConsoleColors.Black) : ThemeColorsTools.GetColor(ThemeColorType.NeutralText);
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
                        Color = ThemeColorsTools.GetColor(buttonHighlight == 0 ? ThemeColorType.TuiPaneSelectedSeparator : ThemeColorType.TuiPaneSeparator),
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
                            OptionColor = ThemeColorsTools.GetColor(ThemeColorType.NeutralText),
                            SelectedOptionColor = ThemeColorsTools.GetColor(ThemeColorType.TuiPaneSelectedSeparator),
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
                        int widgetHeight = ConsoleWrapper.WindowHeight - 13;
                        int optionsEndX = settingsButtonStartPosX + widgetWidth - 1 + ConsoleWrapper.WindowWidth % 2;
                        int optionsEndY = widgetTop + widgetHeight + 1;

                        // Check the ranges
                        bool isWithinSettings = PointerTools.PointerWithinRange(context, (settingsButtonStartPosX, settingsButtonStartPosY), (settingsButtonEndPosX, settingsButtonEndPosY));
                        bool isWithinAbout = PointerTools.PointerWithinRange(context, (aboutButtonStartPosX, aboutButtonStartPosY), (aboutButtonEndPosX, aboutButtonEndPosY));
                        bool isWithinOptions = PointerTools.PointerWithinRange(context, (settingsButtonStartPosX + 1, widgetTop), (optionsEndX, optionsEndY));

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
                            int selectionChoices = widgetHeight + 2;
                            int currentChoices = choices.Length;
                            if (context.ButtonPress == PointerButtonPress.Released && context.Button == PointerButton.Left || context.ButtonPress == PointerButtonPress.Moved)
                            {
                                int posY = context.Coordinates.y;
                                int finalPos = posY - widgetTop;
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
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx++;
                                if (choiceIdx >= choices.Length)
                                    choiceIdx--;
                                break;
                            case ConsoleKey.UpArrow:
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx--;
                                if (choiceIdx < 0)
                                    choiceIdx++;
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
                                choiceIdx = startIndex;
                                break;
                            case ConsoleKey.PageDown:
                                if (buttonHighlight > 0)
                                    break;
                                choiceIdx = endIndex > choices.Length - 1 ? choices.Length - 1 : endIndex + 1;
                                choiceIdx = endIndex == choices.Length - 1 ? endIndex : choiceIdx;
                                break;
                            case ConsoleKey.Tab:
                                buttonHighlight++;
                                if (buttonHighlight > 2)
                                    buttonHighlight = 0;
                                break;
                            case ConsoleKey.Enter:
                                if (buttonHighlight == 1)
                                    SettingsApp.OpenMainPage(Config.MainConfig);
                                else if (buttonHighlight == 2)
                                    OpenAboutBox();
                                else
                                    DoAction(choiceIdx);
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
                                    exiting = answer != 2;
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
                                if (!isThemeMusicPlaying)
                                {
                                    if (themeMusicThread.BaseThread.ThreadState == ThreadState.Stopped)
                                        themeMusicThread.Regen();
                                    themeMusicThread.Start();
                                }
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

        private static void HandleThemeMusic()
        {
            try
            {
                if (isThemeMusicPlaying)
                    return;
                isThemeMusicPlaying = true;
                AudioCuesTools.PlayAudioCue(AudioCueType.Full, false);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Error trying to play theme music: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
            }
            finally
            {
                isThemeMusicPlaying = false;
            }
        }
    }
}
