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

using Terminaux.Inputs.Presentation;
using Terminaux.Inputs.Presentation.Elements;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Users;
using System;
using Textify.General;
using Terminaux.Base;
using Terminaux.Inputs;
using System.Linq;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Modules;

namespace Nitrocid.Kernel
{
    internal static class KernelFirstRun
    {
        internal static void PresentFirstRunIntro()
        {
            try
            {
                // Populate the first run presentations in case language changed during the first start-up
                Slideshow firstRunPres = new(
                    // Presentation name
                    LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_TITLE"),

                    // Presentation list
                    [
                        // First page - introduction
                        new PresentationPage(
                            // Page name
                            LanguageTools.GetLocalized("NKS_MISC_SPLASHES_WELCOME"),

                            // Page elements
                            [
                                new TextElement()
                                {
                                    Arguments =
                                    [
                                        LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE1_TEXT1")
                                    ]
                                },
                                new TextElement()
                                {
                                    Arguments =
                                    [
                                        LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE1_TEXT2")
                                    ]
                                }
                            ],

                            // Page inputs
                            [
                                new PresentationInputInfo(
                                    LanguageTools.GetLocalized("NKS_SETTINGS_KERNEL_GENERAL_CULTUREANDLANGUAGE_LANGUAGE_NAME"), LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_LANGAUGE_PROMPT"),
                                    new ComboBoxModule()
                                    {
                                        Name = LanguageTools.GetLocalized("NKS_SETTINGS_KERNEL_GENERAL_CULTUREANDLANGUAGE_LANGUAGE_NAME"),
                                        Description = LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_LANGPROMPT"),
                                        Choices = [new("Language", [new("Language group", LanguageManager.Languages.Select((kvp) => new InputChoiceInfo(kvp.Key, kvp.Value.FullLanguageName)).ToArray())])]
                                    }, true
                                )
                            ]
                        )
                    ]
                );

                // Present all presentations
                PresentationTools.Present(firstRunPres, true, true);
                DebugWriter.WriteDebug(DebugLevel.I, "Out of introductory run. Going straight to the rest once language configuration has been saved.");

                // Save all the changes
                InfoBoxNonModalColor.WriteInfoBox(LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SAVINGSETTINGS"));
                int selectedLanguageIdx = firstRunPres.Pages[0].Inputs[0].InputMethod.GetValue<int?>() ?? 0;
                string selectedLanguage = LanguageManager.Languages.ElementAt(selectedLanguageIdx).Key;
                DebugWriter.WriteDebug(DebugLevel.I, "Got selectedLanguage {0}.", vars: [selectedLanguage]);
                LanguageManager.SetLang(selectedLanguage);

                // Now, go to the first-run.
                PresentFirstRun();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error in introductory run: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                ConsoleWrapper.Clear();
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_CRASH1") + " {0}", ex.Message);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_CRASH2"));
                Input.ReadKey();
            }
        }

        internal static void PresentFirstRun()
        {
            try
            {
                // Some variables
                string userStepFailureReason = "";
                bool moveOn = false;

                Slideshow firstRunPresUser = new(
                    // Presentation name
                    LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_TITLE"),

                    // Presentation list
                    [
                        // Second page - username creation
                        new PresentationPage(
                            // Page name
                            LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE2_TITLE"),

                            // Page elements
                            [
                                new TextElement()
                                {
                                    Arguments =
                                    [
                                        LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE2_TEXT1")
                                    ]
                                },
                                new DynamicTextElement()
                                {
                                    Arguments =
                                    [
                                        () =>
                                        {
                                            var userList = UserManagement.ListAllUsers();
                                            string list = string.Join(", ", userList);
                                            if (string.IsNullOrEmpty(userStepFailureReason))
                                                return $"{list}\n";
                                            return $"{list}\n{userStepFailureReason}";
                                        }
                                    ]
                                }
                            ],

                            // Page inputs
                            [
                                new PresentationInputInfo(
                                    LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_USERNAME_PROMPT"), LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_USERNAMEPROMPT"),
                                    new TextBoxModule()
                                    {
                                        Name = LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_USERNAME_PROMPT"),
                                        Description = LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_USERNAME_PROMPT_DESC"),
                                    }, true
                                ),
                                new PresentationInputInfo(
                                    LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PASSWORD_PROMPT"), LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PASSWORDPROMPT"),
                                    new TextBoxModule()
                                    {
                                        Name = LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PASSWORD_PROMPT"),
                                        Description = LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PASSWORD_PROMPT_DESC"),
                                    }
                                )
                            ]
                        )
                    ]
                );
                string user = "owner";
                while (!moveOn)
                {
                    PresentationTools.Present(firstRunPresUser, true, true);
                    string inputUser = firstRunPresUser.Pages[0].Inputs[0].InputMethod.GetValue<string?>() ?? user;
                    user = string.IsNullOrEmpty(inputUser) ? user : inputUser;
                    string pass = firstRunPresUser.Pages[0].Inputs[1].InputMethod.GetValue<string?>() ?? "";
                    try
                    {
                        UserManagement.AddUser(user, pass);
                        DebugWriter.WriteDebug(DebugLevel.I, "We shall move on.");
                        userStepFailureReason = "";
                        moveOn = true;
                        DebugWriter.WriteDebug(DebugLevel.I, "Let's move on!");
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "We shouldn't move on. Failed to create username. {0}", vars: [ex.Message]);
                        DebugWriter.WriteDebugStackTrace(ex);
                        userStepFailureReason = LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_USERCREATIONFAILED");
                    }
                }

                Slideshow firstRunPresUpdates = new(
                    // Presentation name
                    LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_TITLE"),

                    // Presentation list
                    [
                        // Fifth page - Automatic updates
                        new PresentationPage(
                            // Page name
                            LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE3_TITLE"),

                            // Page elements
                            [
                                new TextElement()
                                {
                                    Arguments =
                                    [
                                        LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE3_TEXT1") + " " +
                                        LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE3_TEXT2") + " " +
                                        LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE3_TEXT3")
                                    ]
                                }
                            ],

                            // Page inputs
                            [
                                new PresentationInputInfo(
                                    LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_AUTOUPDATECHECK_PROMPT"), LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_AUTOUPDATECHECK_PROMPT"),
                                    new ComboBoxModule()
                                    {
                                        Name = LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_AUTOUPDATECHECK_PROMPT"),
                                        Description = LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_CHECKUPDATESPROMPT"),
                                        Choices = [new("Choices", [new("Choices", [
                                            new InputChoiceInfo("y", LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_AUTOUPDATECHECK_AGREE")),
                                            new InputChoiceInfo("n", LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_AUTOUPDATECHECK_DISAGREE")),
                                        ])])]
                                    }, true
                                ),
                            ]
                        )
                    ]
                );
                PresentationTools.Present(firstRunPresUpdates, true, true);
                bool needsAutoCheck = firstRunPresUpdates.Pages[0].Inputs[0].InputMethod.GetValue<int?>() == 0;
                Config.MainConfig.CheckUpdateStart = needsAutoCheck;

                Slideshow firstRunPresOutro = new(
                    // Presentation name
                    LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_TITLE"),

                    // Presentation list
                    [
                        // Third page - get started
                        new PresentationPage(
                            // Page name
                            LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE4_TITLE"),

                            // Page elements
                            [
                                new DynamicTextElement()
                                {
                                    Arguments =
                                    [
                                        () => TextTools.FormatString(LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE4_TEXT1"), user)
                                    ]
                                },
                                new TextElement()
                                {
                                    Arguments =
                                    [
                                        LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE4_TEXT2")
                                    ]
                                }
                            ]
                        )
                    ]
                );
                PresentationTools.Present(firstRunPresOutro, true, true);
                DebugWriter.WriteDebug(DebugLevel.I, "Out of first run");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error in first run: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                ConsoleWrapper.Clear();
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_CRASH1") + " {0}", ex.Message);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_CRASH2"));
                Input.ReadKey();
            }
        }
    }
}
