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

using System.Collections.Generic;
using System.Linq;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Inputs.Styles.Selection;
using Nitrocid.ConsoleBase.Themes;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Files.Folders;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Events;
using Nitrocid.Languages;
using Terminaux.Colors;
using Textify.General;
using Terminaux.Base;
using Nitrocid.ConsoleBase.Inputs;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Interactive;
using System;

namespace Nitrocid.Extras.ThemeStudio.Studio
{
    static class ThemeStudioApp
    {

        /// <summary>
        /// Starts the theme studio
        /// </summary>
        /// <param name="ThemeName">Theme name</param>
        /// <param name="useTui">Whether to use TUI or not</param>
        public static void StartThemeStudio(string ThemeName, bool useTui = false)
        {
            // Inform user that we're on the studio
            EventsManager.FireEvent(EventType.ThemeStudioStarted);
            DebugWriter.WriteDebug(DebugLevel.I, "Starting theme studio with theme name {0}", ThemeName);
            ThemeStudioTools.SelectedThemeName = ThemeName;

            // Maximum options is number of kernel colors plus more options
            int MaximumOptions = ThemeStudioTools.SelectedColors.Count + 9;
            var StudioExiting = false;
            var originalColors = new Dictionary<KernelColorType, Color>(ThemeStudioTools.SelectedColors);

            // Check for TUI
            if (useTui)
            {
                var tui = new ThemeStudioCli()
                {
                    originalColors = originalColors,
                    themeName = ThemeName,
                };
                tui.Bindings.Add(new InteractiveTuiBinding<KernelColorType>(Translate.DoTranslation("Change..."), ConsoleKey.Enter, (line, _, _, _) => tui.Change(line)));
                tui.Bindings.Add(new InteractiveTuiBinding<KernelColorType>(Translate.DoTranslation("Save..."), ConsoleKey.F1, (_, _, _, _) => tui.Save()));
                tui.Bindings.Add(new InteractiveTuiBinding<KernelColorType>(Translate.DoTranslation("Load..."), ConsoleKey.F2, (_, _, _, _) => tui.Load()));
                tui.Bindings.Add(new InteractiveTuiBinding<KernelColorType>(Translate.DoTranslation("Copy..."), ConsoleKey.F3, (line, _, _, _) => tui.Copy(line)));
                InteractiveTuiTools.OpenInteractiveTui(tui);
                return;
            }

            // Main Loop
            while (!StudioExiting)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Studio not exiting yet. Populating {0} options...", MaximumOptions);
                ConsoleWrapper.Clear();

                // Make a list of choices
                List<InputChoiceInfo> choices = [];
                var colors = ThemeStudioTools.SelectedColors;
                for (int key = 0; key < colors.Count; key++)
                {
                    var colorType = colors.Keys.ElementAt(key);
                    var color = colors.Values.ElementAt(key).PlainSequence;
                    bool changed = originalColors.Values.ElementAt(key) != colors.Values.ElementAt(key);
                    choices.Add(new InputChoiceInfo($"{key + 1}{(changed ? "*" : "")}", $"{colorType}: [{color}]"));
                }
                List<InputChoiceInfo> altChoices =
                [
                    new InputChoiceInfo($"{colors.Count + 1}", Translate.DoTranslation("Save Theme to Current Directory")),
                    new InputChoiceInfo($"{colors.Count + 2}", Translate.DoTranslation("Save Theme to Another Directory...")),
                    new InputChoiceInfo($"{colors.Count + 3}", Translate.DoTranslation("Save Theme to Current Directory as...")),
                    new InputChoiceInfo($"{colors.Count + 4}", Translate.DoTranslation("Save Theme to Another Directory as...")),
                    new InputChoiceInfo($"{colors.Count + 5}", Translate.DoTranslation("Load Theme From File...")),
                    new InputChoiceInfo($"{colors.Count + 6}", Translate.DoTranslation("Load Theme From Prebuilt Themes...")),
                    new InputChoiceInfo($"{colors.Count + 7}", Translate.DoTranslation("Load Current Colors")),
                    new InputChoiceInfo($"{colors.Count + 8}", Translate.DoTranslation("Preview...")),
                    new InputChoiceInfo($"{colors.Count + 9}", Translate.DoTranslation("Copy Color To...")),
                    new InputChoiceInfo($"{colors.Count + 10}", Translate.DoTranslation("Exit")),
                ];
                TextWriterColor.Write(Translate.DoTranslation("Making a new theme \"{0}\".") + CharManager.NewLine, ThemeName);

                // Prompt user
                int response = SelectionStyle.PromptSelection(TextTools.FormatString(Translate.DoTranslation("Making a new theme \"{0}\"."), ThemeName), [.. choices], [.. altChoices], true);
                DebugWriter.WriteDebug(DebugLevel.I, "Got response: {0}", response);

                // Check for response integrity
                DebugWriter.WriteDebug(DebugLevel.I, "Numeric response {0} is >= 1 and <= {1}.", response, MaximumOptions);
                Color SelectedColorInstance;
                if (response == colors.Count + 1)
                {
                    // Save theme to current directory
                    ThemeStudioTools.SaveThemeToCurrentDirectory(ThemeName);
                }
                else if (response == colors.Count + 2)
                {
                    // Save theme to another directory...
                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for directory name...");
                    TextWriters.Write(Translate.DoTranslation("Specify directory to save theme to:") + " [{0}] ", false, KernelColorType.Input, CurrentDirectory.CurrentDir);
                    string DirectoryName = InputTools.ReadLine();
                    DirectoryName = string.IsNullOrWhiteSpace(DirectoryName) ? CurrentDirectory.CurrentDir : DirectoryName;
                    DebugWriter.WriteDebug(DebugLevel.I, "Got directory name {0}.", DirectoryName);
                    ThemeStudioTools.SaveThemeToAnotherDirectory(ThemeName, DirectoryName);
                }
                else if (response == colors.Count + 3)
                {
                    // Save theme to current directory as...
                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                    TextWriters.Write(Translate.DoTranslation("Specify theme name:") + " [{0}] ", false, KernelColorType.Input, ThemeName);
                    string AltThemeName = InputTools.ReadLine();
                    AltThemeName = string.IsNullOrWhiteSpace(AltThemeName) ? ThemeName : AltThemeName;
                    DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
                    ThemeStudioTools.SaveThemeToCurrentDirectory(AltThemeName);
                }
                else if (response == colors.Count + 4)
                {
                    // Save theme to another directory as...
                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme and directory name...");
                    TextWriters.Write(Translate.DoTranslation("Specify directory to save theme to:") + " [{0}] ", false, KernelColorType.Input, CurrentDirectory.CurrentDir);
                    string DirectoryName = InputTools.ReadLine();
                    DirectoryName = string.IsNullOrWhiteSpace(DirectoryName) ? CurrentDirectory.CurrentDir : DirectoryName;
                    DebugWriter.WriteDebug(DebugLevel.I, "Got directory name {0}.", DirectoryName);
                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                    TextWriters.Write(Translate.DoTranslation("Specify theme name:") + " [{0}] ", false, KernelColorType.Input, ThemeName);
                    string AltThemeName = InputTools.ReadLine();
                    AltThemeName = string.IsNullOrWhiteSpace(AltThemeName) ? ThemeName : AltThemeName;
                    DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
                    ThemeStudioTools.SaveThemeToAnotherDirectory(AltThemeName, DirectoryName);
                }
                else if (response == colors.Count + 5)
                {
                    // Load Theme From File...
                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                    TextWriters.Write(Translate.DoTranslation("Specify theme file name wihout the .json extension:") + " ", false, KernelColorType.Input);
                    string AltThemeName = InputTools.ReadLine() + ".json";
                    DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
                    ThemeStudioTools.LoadThemeFromFile(AltThemeName);
                }
                else if (response == colors.Count + 6)
                {
                    // Load Theme From Prebuilt Themes...
                    DebugWriter.WriteDebug(DebugLevel.I, "Prompting user for theme name...");
                    TextWriters.Write(Translate.DoTranslation("Specify theme name:") + " ", false, KernelColorType.Input);
                    string AltThemeName = InputTools.ReadLine();
                    DebugWriter.WriteDebug(DebugLevel.I, "Got theme name {0}.", AltThemeName);
                    ThemeStudioTools.LoadThemeFromResource(AltThemeName);
                }
                else if (response == colors.Count + 7)
                {
                    // Load Current Colors
                    DebugWriter.WriteDebug(DebugLevel.I, "Loading current colors...");
                    ThemeStudioTools.LoadThemeFromCurrentColors();
                }
                else if (response == colors.Count + 8)
                {
                    // Preview...
                    DebugWriter.WriteDebug(DebugLevel.I, "Printing text with colors of theme...");
                    ThemePreviewTools.PreviewTheme(colors);
                }
                else if (response == colors.Count + 9)
                {
                    // Copy Color To...
                    DebugWriter.WriteDebug(DebugLevel.I, "Copying color to...");

                    // Specify the source...
                    int sourceColorIdx = InfoBoxSelectionColor.WriteInfoBoxSelection([.. choices], Translate.DoTranslation("Select the source color type to copy the color from."));
                    if (sourceColorIdx < 0)
                        continue;
                    var sourceType = (KernelColorType)sourceColorIdx;
                    var sourceColorType = choices[sourceColorIdx];
                    var sourceColor = colors[sourceType];

                    // Specify the target...
                    int[] targetColors = InfoBoxSelectionMultipleColor.WriteInfoBoxSelectionMultiple([.. choices], Translate.DoTranslation("Select the target color types to copy the source color type, {0}, to.").FormatString(sourceColorType.ChoiceTitle));
                    if (targetColors.Length == 0)
                        continue;

                    // Copying...
                    foreach (int idx in targetColors)
                    {
                        var type = (KernelColorType)idx;
                        colors[type] = sourceColor;
                    }
                }
                else if (response == colors.Count + 10)
                {
                    // Exit
                    DebugWriter.WriteDebug(DebugLevel.I, "Exiting studio...");
                    StudioExiting = true;
                }
                else
                {
                    ColorTools.LoadBackDry(0);
                    SelectedColorInstance = colors[colors.Keys.ElementAt(response - 1)];
                    var finalColor = ColorSelector.OpenColorSelector(SelectedColorInstance);
                    colors[colors.Keys.ElementAt(response - 1)] = finalColor;
                }
            }

            // Raise event
            EventsManager.FireEvent(EventType.ThemeStudioExit);
        }

    }
}
