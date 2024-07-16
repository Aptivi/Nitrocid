//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Nitrocid.Files.Paths;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Languages;
using Nitrocid.Misc.Interactives;
using Nitrocid.Network.Types.RSS;
using Nitrocid.Users;
using Nitrocid.Users.Login;
using Nitrocid.Users.Login.Widgets;
using Nitrocid.Users.Login.Widgets.Implementations;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Terminaux.Inputs;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Pointer;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Selection;
using Terminaux.Reader;
using Terminaux.Sequences;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Writer.FancyWriters;

namespace Nitrocid.Shell.Homepage
{
    /// <summary>
    /// The Nitrocid Homepage tools
    /// </summary>
    public static class HomepageTools
    {
        internal static bool isHomepageEnabled = true;
        private static bool isOnHomepage = false;
        private static readonly HomepageBinding[] bindings =
        [
            // Keyboard
            new(/* Localizable */ "Execute", ConsoleKey.Enter),
            new(/* Localizable */ "Logout", ConsoleKey.Escape),
            new(/* Localizable */ "Shell", ConsoleKey.S),
            new(/* Localizable */ "Keybindings", ConsoleKey.K),
            new(/* Localizable */ "Switch", ConsoleKey.Tab),

            // Mouse
            new(/* Localizable */ "Execute", PointerButton.Left),
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
            bool settingsHighlighted = false;
            InputChoiceInfo[] choices =
            [
                new("1", Translate.DoTranslation("File Manager")),
                new("2", Translate.DoTranslation("Alarm Manager")),
                new("3", Translate.DoTranslation("Notifications")),
                new("4", Translate.DoTranslation("Task Manager")),
            ];

            try
            {
                // Create a screen for the homepage
                var homeScreenBuffer = new ScreenPart();
                string rssSequence = "";
                ScreenTools.SetCurrent(homeScreen);
                ColorTools.LoadBack();

                // Now, render the homepage
                homeScreenBuffer.AddDynamicText(() =>
                {
                    var builder = new StringBuilder();

                    // Make a master border
                    builder.Append(BorderColor.RenderBorder(0, 3, ConsoleWrapper.WindowWidth - 2, ConsoleWrapper.WindowHeight - 6, KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator)));

                    // Show username at the top
                    builder.Append(CenteredTextColor.RenderCenteredOneLine(1, Translate.DoTranslation("Hi, {0}! Welcome to Nitrocid!"), Vars: [string.IsNullOrWhiteSpace(UserManagement.CurrentUser.FullName) ? UserManagement.CurrentUser.Username : UserManagement.CurrentUser.FullName]));

                    // Show bindings
                    var bindingsBuilder = new StringBuilder(CsiSequences.GenerateCsiCursorPosition(1, ConsoleWrapper.WindowHeight));
                    foreach (HomepageBinding binding in bindings)
                    {
                        // Check the binding mode
                        if (binding.BindingUsesMouse)
                            continue;

                        // First, check to see if the rendered binding info is going to exceed the console window width
                        string renderedBinding = $"{GetBindingKeyShortcut(binding, false)} {binding.BindingName}  ";
                        int bindingLength = ConsoleChar.EstimateCellWidth(renderedBinding);
                        int actualLength = ConsoleChar.EstimateCellWidth(VtSequenceTools.FilterVTSequences(bindingsBuilder.ToString()));
                        bool canDraw = bindingLength + actualLength < ConsoleWrapper.WindowWidth - 3;
                        if (canDraw)
                        {
                            bindingsBuilder.Append(
                                $"{ColorTools.RenderSetConsoleColor(InteractiveTuiStatus.KeyBindingOptionColor, false, true)}" +
                                $"{ColorTools.RenderSetConsoleColor(InteractiveTuiStatus.OptionBackgroundColor, true)}" +
                                GetBindingKeyShortcut(binding, false) +
                                $"{ColorTools.RenderSetConsoleColor(InteractiveTuiStatus.OptionForegroundColor)}" +
                                $"{ColorTools.RenderSetConsoleColor(InteractiveTuiStatus.BackgroundColor, true)}" +
                                $" {binding.BindingName}  "
                            );
                        }
                        else
                        {
                            // We can't render anymore, so just break and write a binding to show more
                            bindingsBuilder.Append(
                                $"{CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth - 2, ConsoleWrapper.WindowHeight)}" +
                                $"{ColorTools.RenderSetConsoleColor(InteractiveTuiStatus.KeyBindingBuiltinColor, false, true)}" +
                                $"{ColorTools.RenderSetConsoleColor(InteractiveTuiStatus.KeyBindingBuiltinBackgroundColor, true)}" +
                                " K "
                            );
                            break;
                        }
                    }
                    builder.Append(bindingsBuilder);

                    // Make a border for an analog clock widget and the first three RSS feeds (if the addon is installed)
                    int widgetLeft = ConsoleWrapper.WindowWidth / 2 + ConsoleWrapper.WindowWidth % 2;
                    int widgetWidth = ConsoleWrapper.WindowWidth / 2 - 4;
                    int widgetHeight = ConsoleWrapper.WindowHeight - 13;
                    int clockTop = 4;
                    int rssTop = clockTop + widgetHeight + 2;
                    int rssHeight = 3;
                    builder.Append(BorderColor.RenderBorder(widgetLeft, clockTop, widgetWidth, widgetHeight, KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator)));
                    builder.Append(BorderColor.RenderBorder(widgetLeft, rssTop, widgetWidth, rssHeight, KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator)));

                    // Render the clock widget
                    var widget = WidgetTools.GetWidget(nameof(AnalogClock));
                    string widgetInit = widget.Initialize(widgetLeft + 1, clockTop + 1, widgetWidth, widgetHeight);
                    string widgetSeq = widget.Render(widgetLeft + 1, clockTop + 1, widgetWidth, widgetHeight);
                    builder.Append(widgetInit + widgetSeq);

                    // Render the first three RSS feeds
                    if (string.IsNullOrEmpty(rssSequence))
                    {
                        var rssSequenceBuilder = new StringBuilder();
                        try
                        {
                            if (!Config.MainConfig.ShowHeadlineOnLogin)
                                rssSequenceBuilder.Append(Translate.DoTranslation("Enable headlines on login to show RSS feeds").Truncate(widgetWidth - 2));
                            else
                            {
                                var feedsObject = InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasRssShell, "GetArticles", Config.MainConfig.RssHeadlineUrl);
                                bool found = false;
                                if (feedsObject is (string feedTitle, string articleTitle)[] feeds)
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        if (i >= feeds.Length)
                                            break;
                                        (string _, string articleTitle) = feeds[i];
                                        rssSequenceBuilder.Append(CsiSequences.GenerateCsiCursorPosition(widgetLeft + 2, rssTop + i + 2));
                                        rssSequenceBuilder.Append(articleTitle.Truncate(widgetWidth - 2));
                                        found = true;
                                    }
                                }
                                if (!found)
                                    rssSequenceBuilder.Append(Translate.DoTranslation("No feed.").Truncate(widgetWidth - 2));
                            }
                        }
                        catch (KernelException ex) when (ex.ExceptionType == KernelExceptionType.AddonManagement)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", ex.Message);
                            DebugWriter.WriteDebugStackTrace(ex);
                            rssSequenceBuilder.Append(Translate.DoTranslation("Install the RSS Shell Extras addon!").Truncate(widgetWidth - 2));
                        }
                        catch (Exception ex)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", ex.Message);
                            DebugWriter.WriteDebugStackTrace(ex);
                            rssSequenceBuilder.Append(Translate.DoTranslation("Failed to get the latest news.").Truncate(widgetWidth - 2));
                        }
                        rssSequence = rssSequenceBuilder.ToString();
                    }
                    builder.Append(CsiSequences.GenerateCsiCursorPosition(widgetLeft + 2, rssTop + 2));
                    builder.Append(rssSequence);

                    // Populate the settings button
                    int settingsButtonPosX = 2;
                    int settingsButtonPosY = ConsoleWrapper.WindowHeight - 5;
                    int settingsButtonWidth = ConsoleWrapper.WindowWidth / 2 - 5 + ConsoleWrapper.WindowWidth % 2;
                    int settingsButtonHeight = 1;
                    builder.Append(BorderColor.RenderBorder(settingsButtonPosX, settingsButtonPosY, settingsButtonWidth, settingsButtonHeight, settingsHighlighted ? new Color(ConsoleColors.Black) : KernelColorTools.GetColor(KernelColorType.TuiPaneSeparator), settingsHighlighted ? KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator) : ColorTools.CurrentBackgroundColor));
                    builder.Append(CenteredTextColor.RenderCenteredOneLine(settingsButtonPosY + 1, Translate.DoTranslation("Settings"), settingsHighlighted ? new Color(ConsoleColors.Black) : KernelColorTools.GetColor(KernelColorType.NeutralText), settingsHighlighted ? KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator) : ColorTools.CurrentBackgroundColor, settingsButtonPosX + 1, settingsButtonWidth + settingsButtonPosX + 5 - ConsoleWrapper.WindowWidth % 2));

                    // Populate the available options
                    builder.Append(BorderColor.RenderBorder(settingsButtonPosX, clockTop, widgetWidth - 1 + ConsoleWrapper.WindowWidth % 2, widgetHeight + 2, KernelColorTools.GetColor(!settingsHighlighted ? KernelColorType.TuiPaneSelectedSeparator : KernelColorType.TuiPaneSeparator)));
                    builder.Append(SelectionInputTools.RenderSelections(choices, settingsButtonPosX + 1, clockTop + 1, choiceIdx, widgetHeight + 2, widgetWidth - 1 + ConsoleWrapper.WindowWidth % 2, foregroundColor: KernelColorTools.GetColor(KernelColorType.NeutralText), selectedForegroundColor: KernelColorTools.GetColor(KernelColorType.TuiPaneSelectedSeparator)));

                    // Return the resulting homepage
                    return builder.ToString();
                });
                homeScreen.AddBufferedPart("The Nitrocid Homepage", homeScreenBuffer);

                // Render the thing and wait for a keypress
                bool exiting = false;
                while (!exiting)
                {
                    // Render and wait for input for a second
                    ScreenTools.Render();
                    if (!SpinWait.SpinUntil(() => PointerListener.InputAvailable, 1000))
                        continue;

                    // Read the available input
                    var key = TermReader.ReadPointerOrKey();
                    if (key.Item1 is PointerEventContext context)
                    {

                    }
                    else
                    {
                        var keypress = key.Item2;
                        switch (keypress.Key)
                        {
                            case ConsoleKey.DownArrow:
                                if (settingsHighlighted)
                                    break;
                                choiceIdx++;
                                if (choiceIdx >= choices.Length)
                                    choiceIdx--;
                                break;
                            case ConsoleKey.UpArrow:
                                if (settingsHighlighted)
                                    break;
                                choiceIdx--;
                                if (choiceIdx < 0)
                                    choiceIdx++;
                                break;
                            case ConsoleKey.Tab:
                                settingsHighlighted = !settingsHighlighted;
                                break;
                            case ConsoleKey.Enter:
                                if (settingsHighlighted)
                                    SettingsApp.OpenMainPage(Config.MainConfig);
                                else
                                {
                                    switch (choiceIdx)
                                    {
                                        case 0:
                                            {
                                                var tui = new FileManagerCli
                                                {
                                                    firstPanePath = PathsManagement.HomePath,
                                                    secondPanePath = PathsManagement.HomePath
                                                };
                                                InteractiveTuiTools.OpenInteractiveTui(tui);
                                            }
                                            break;
                                        case 1:
                                            {
                                                var tui = new AlarmCli();
                                                InteractiveTuiTools.OpenInteractiveTui(tui);
                                            }
                                            break;
                                        case 2:
                                            {
                                                var tui = new NotificationsCli();
                                                InteractiveTuiTools.OpenInteractiveTui(tui);
                                            }
                                            break;
                                        case 3:
                                            {
                                                var tui = new TaskManagerCli();
                                                InteractiveTuiTools.OpenInteractiveTui(tui);
                                            }
                                            break;
                                    }
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
                                // User needs an infobox that shows all available keys
                                var binds = bindings.Where((binding) => !binding.BindingUsesMouse);
                                var mouseBindings = bindings.Where((binding) => binding.BindingUsesMouse);
                                int maxBindingLength = binds
                                    .Max((itb) => ConsoleChar.EstimateCellWidth(GetBindingKeyShortcut(itb)));
                                string[] bindingRepresentations = binds
                                    .Select((itb) => $"{GetBindingKeyShortcut(itb) + new string(' ', maxBindingLength - ConsoleChar.EstimateCellWidth(GetBindingKeyShortcut(itb))) + $" | {itb.BindingName}"}")
                                    .ToArray();
                                string[] bindingMouseRepresentations = [];
                                if (mouseBindings is not null && mouseBindings.Any())
                                {
                                    int maxMouseBindingLength = mouseBindings
                                        .Max((itb) => ConsoleChar.EstimateCellWidth(GetBindingMouseShortcut(itb)));
                                    bindingMouseRepresentations = mouseBindings
                                        .Select((itb) => $"{GetBindingMouseShortcut(itb) + new string(' ', maxMouseBindingLength - ConsoleChar.EstimateCellWidth(GetBindingMouseShortcut(itb))) + $" | {itb.BindingName}"}")
                                        .ToArray();
                                }
                                InfoBoxColor.WriteInfoBoxColorBack(
                                    "Available keys",
                                    $"{string.Join("\n", bindingRepresentations)}" +
                                    "\n\nMouse bindings:\n\n" +
                                    $"{(bindingMouseRepresentations.Length > 0 ? string.Join("\n", bindingMouseRepresentations) : "No mouse bindings")}"
                                , InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ColorTools.LoadBack();
                InfoBoxColor.WriteInfoBoxColor(Translate.DoTranslation("The Nitrocid Homepage has crashed and needs to revert back to the shell.") + $": {ex.Message}", true, KernelColorTools.GetColor(KernelColorType.Error));
            }
            finally
            {
                isOnHomepage = false;
                ScreenTools.UnsetCurrent(homeScreen);
                ColorTools.LoadBack();
            }
        }

        private static string GetBindingKeyShortcut(HomepageBinding bind, bool mark = true)
        {
            if (bind.BindingUsesMouse)
                return "";
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(bind.BindingKeyModifiers != 0 ? $"{bind.BindingKeyModifiers} + " : "")}{bind.BindingKeyName}{markEnd}";
        }

        private static string GetBindingMouseShortcut(HomepageBinding bind, bool mark = true)
        {
            if (!bind.BindingUsesMouse)
                return "";
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(bind.BindingPointerModifiers != 0 ? $"{bind.BindingPointerModifiers} + " : "")}{bind.BindingPointerButton}{(bind.BindingPointerButtonPress != 0 ? $" {bind.BindingPointerButtonPress}" : "")}{markEnd}";
        }
    }
}
