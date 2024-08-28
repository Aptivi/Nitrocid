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
using Nitrocid.Files.Instances;
using Nitrocid.Files.Paths;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Configuration.Settings;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Languages;
using Nitrocid.Misc.Interactives;
using Nitrocid.Misc.Notifications;
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
using Terminaux.Inputs.Styles;
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
                new("5", Translate.DoTranslation("About Nitrocid")),
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
                                $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.TuiKeyBindingOption), false, true)}" +
                                $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.TuiOptionBackground), true)}" +
                                GetBindingKeyShortcut(binding, false) +
                                $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.TuiOptionForeground))}" +
                                $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.TuiBackground), true)}" +
                                $" {binding.BindingName}  "
                            );
                        }
                        else
                        {
                            // We can't render anymore, so just break and write a binding to show more
                            bindingsBuilder.Append(
                                $"{CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth - 2, ConsoleWrapper.WindowHeight)}" +
                                $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.TuiKeyBindingBuiltin), false, true)}" +
                                $"{ColorTools.RenderSetConsoleColor(KernelColorTools.GetColor(KernelColorType.TuiKeyBindingBuiltinBackground), true)}" +
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

                // Helper function
                void DoAction(int choiceIdx)
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
                                tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Open"), ConsoleKey.Enter, (entry1, _, entry2, _) => tui.Open(entry1, entry2)));
                                tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Copy"), ConsoleKey.F1, (entry1, _, entry2, _) => tui.CopyFileOrDir(entry1, entry2)));
                                tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Move"), ConsoleKey.F2, (entry1, _, entry2, _) => tui.MoveFileOrDir(entry1, entry2)));
                                tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Delete"), ConsoleKey.F3, (entry1, _, entry2, _) => tui.RemoveFileOrDir(entry1, entry2)));
                                tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Up"), ConsoleKey.F4, (_, _, _, _) => tui.GoUp()));
                                tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Info"), ConsoleKey.F5, (entry1, _, entry2, _) => tui.PrintFileSystemEntry(entry1, entry2)));
                                tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Go To"), ConsoleKey.F6, (_, _, _, _) => tui.GoTo()));
                                tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Copy To"), ConsoleKey.F1, ConsoleModifiers.Shift, (entry1, _, entry2, _) => tui.CopyTo(entry1, entry2)));
                                tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Move to"), ConsoleKey.F2, ConsoleModifiers.Shift, (entry1, _, entry2, _) => tui.MoveTo(entry1, entry2)));
                                tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Rename"), ConsoleKey.F9, (entry1, _, entry2, _) => tui.Rename(entry1, entry2)));
                                tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("New Folder"), ConsoleKey.F10, (_, _, _, _) => tui.MakeDir()));
                                tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Hash"), ConsoleKey.F11, (entry1, _, entry2, _) => tui.Hash(entry1, entry2)));
                                tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Verify"), ConsoleKey.F12, (entry1, _, entry2, _) => tui.Verify(entry1, entry2)));
                                tui.Bindings.Add(new InteractiveTuiBinding<FileSystemEntry>(Translate.DoTranslation("Preview"), ConsoleKey.P, (entry1, _, entry2, _) => tui.Preview(entry1, entry2)));
                                InteractiveTuiTools.OpenInteractiveTui(tui);
                            }
                            break;
                        case 1:
                            {
                                var tui = new AlarmCli();
                                tui.Bindings.Add(new InteractiveTuiBinding<string>(Translate.DoTranslation("Add"), ConsoleKey.A, (_, _, _, _) => tui.Start(), true));
                                tui.Bindings.Add(new InteractiveTuiBinding<string>(Translate.DoTranslation("Remove"), ConsoleKey.Delete, (alarm, _, _, _) => tui.Stop(alarm)));
                                InteractiveTuiTools.OpenInteractiveTui(tui);
                            }
                            break;
                        case 2:
                            {
                                var tui = new NotificationsCli();
                                tui.Bindings.Add(new InteractiveTuiBinding<Notification>(Translate.DoTranslation("Dismiss"), ConsoleKey.Delete, (notif, _, _, _) => tui.Dismiss(notif)));
                                tui.Bindings.Add(new InteractiveTuiBinding<Notification>(Translate.DoTranslation("Dismiss All"), ConsoleKey.Delete, ConsoleModifiers.Control, (_, _, _, _) => tui.DismissAll()));
                                InteractiveTuiTools.OpenInteractiveTui(tui);
                            }
                            break;
                        case 3:
                            {
                                var tui = new TaskManagerCli();
                                tui.Bindings.Add(new InteractiveTuiBinding<(int, object)>(Translate.DoTranslation("Kill"), ConsoleKey.F1, (thread, _, _, _) => tui.KillThread(thread)));
                                tui.Bindings.Add(new InteractiveTuiBinding<(int, object)>(Translate.DoTranslation("Switch"), ConsoleKey.F2, (_, _, _, _) => tui.SwitchMode()));
                                InteractiveTuiTools.OpenInteractiveTui(tui);
                            }
                            break;
                        case 4:
                            InfoBoxButtonsColor.WriteInfoBoxButtons(
                                Translate.DoTranslation("About Nitrocid"),
                                [new InputChoiceInfo(Translate.DoTranslation("Close"), Translate.DoTranslation("Close"))],
                                Translate.DoTranslation("Nitrocid KS simulates our future kernel, the Nitrocid Kernel.") + "\n\n" +
                                Translate.DoTranslation("Version") + $": {KernelMain.VersionFullStr}" + "\n" +
                                Translate.DoTranslation("Mod API") + $": {KernelMain.ApiVersion}" + "\n\n" +
                                Translate.DoTranslation("Copyright (C) 2018-2024 Aptivi - All rights reserved") + " - https://aptivi.github.io"
                            );
                            break;
                    }
                }

                // Render the thing and wait for a keypress
                bool exiting = false;
                while (!exiting)
                {
                    // Render and wait for input for a second
                    ScreenTools.Render();
                    if (!SpinWait.SpinUntil(() => Input.InputAvailable, 1000))
                        continue;

                    // Read the available input
                    if (Input.MouseInputAvailable)
                    {
                        var context = Input.ReadPointer();

                        // Get the necessary positions
                        int settingsButtonWidth = ConsoleWrapper.WindowWidth / 2 - 5 + ConsoleWrapper.WindowWidth % 2;
                        int settingsButtonHeight = 1;
                        int settingsButtonStartPosX = 2;
                        int settingsButtonStartPosY = ConsoleWrapper.WindowHeight - 5;
                        int settingsButtonEndPosX = settingsButtonStartPosX + settingsButtonWidth + 1;
                        int settingsButtonEndPosY = settingsButtonStartPosY + settingsButtonHeight + 1;
                        int clockTop = 5;
                        int widgetWidth = ConsoleWrapper.WindowWidth / 2 - 4;
                        int widgetHeight = ConsoleWrapper.WindowHeight - 13;
                        int optionsEndX = settingsButtonStartPosX + widgetWidth - 1 + ConsoleWrapper.WindowWidth % 2;
                        int optionsEndY = clockTop + widgetHeight + 1;

                        // Check the ranges
                        bool isWithinSettings = PointerTools.PointerWithinRange(context, (settingsButtonStartPosX, settingsButtonStartPosY), (settingsButtonEndPosX, settingsButtonEndPosY));
                        bool isWithinOptions = PointerTools.PointerWithinRange(context, (settingsButtonStartPosX + 1, clockTop), (optionsEndX, optionsEndY));

                        // If the mouse pointer is within the settings, check for left release
                        if (isWithinSettings)
                        {
                            if (context.ButtonPress == PointerButtonPress.Released && context.Button == PointerButton.Left)
                                SettingsApp.OpenMainPage(Config.MainConfig);
                        }
                        else if (isWithinOptions)
                        {
                            int selectionChoices = widgetHeight + 2;
                            int currentChoices = choices.Length;
                            if ((context.ButtonPress == PointerButtonPress.Released && context.Button == PointerButton.Left) || context.ButtonPress == PointerButtonPress.Moved)
                            {
                                int posY = context.Coordinates.y;
                                int finalPos = posY - clockTop;
                                if (finalPos < currentChoices)
                                {
                                    choiceIdx = finalPos;
                                    if (context.ButtonPress == PointerButtonPress.Released && context.Button == PointerButton.Left)
                                        DoAction(choiceIdx);
                                }
                            }
                            else if (context.ButtonPress == PointerButtonPress.Scrolled)
                            {
                                if (context.Button == PointerButton.WheelUp)
                                {
                                    choiceIdx--;
                                    if (choiceIdx < 0)
                                        choiceIdx++;
                                }
                                else if (context.Button == PointerButton.WheelDown)
                                {
                                    choiceIdx++;
                                    if (choiceIdx >= choices.Length)
                                        choiceIdx--;
                                }
                            }
                        }
                        if (context.ButtonPress == PointerButtonPress.Moved)
                            settingsHighlighted = isWithinSettings;
                    }
                    else if (ConsoleWrapper.KeyAvailable && !Input.PointerActive)
                    {
                        var keypress = Input.ReadKey();
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
                                    DoAction(choiceIdx);
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
                                    $"{(bindingMouseRepresentations.Length > 0 ? string.Join("\n", bindingMouseRepresentations) : "No mouse bindings")}", 
                                    KernelColorTools.GetColor(KernelColorType.TuiBoxForeground),
                                    KernelColorTools.GetColor(KernelColorType.TuiBoxBackground));
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
