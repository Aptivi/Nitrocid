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

using Terminaux.Inputs.Presentation;
using Terminaux.Inputs.Presentation.Elements;
using Terminaux.Writer.ConsoleWriters;
using System;
using Textify.General;
using Terminaux.Base;
using Terminaux.Inputs;
using System.Linq;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Modules;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;

namespace Nitrocid.Base.Kernel
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
                                        Choices = [new("Language", [new("Language group", LanguageManager.Languages.Select((kvp) => new InputChoiceInfo(kvp.Key, kvp.Value.EnglishName)).ToArray())])]
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
                                    new MaskedTextBoxModule()
                                    {
                                        Name = LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PASSWORD_PROMPT"),
                                        Description = LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PASSWORD_PROMPT_DESC"),
                                        Mask = !string.IsNullOrEmpty(Config.MainConfig.CurrentMask) ? Config.MainConfig.CurrentMask[0] : '\0',
                                    }
                                ),

                                // TODO: NKS_KERNEL_FIRSTRUN_FLAGS_PROMPT -> Attributes
                                // TODO: NKS_KERNEL_FIRSTRUN_PRESENTATION_FLAGSPROMPT -> Choose the attributes
                                // TODO: NKS_KERNEL_FIRSTRUN_FLAGS_PROMPT_DESC -> Choose the attributes of this user
                                // TODO: NKS_USERS_FLAGS_ADMIN -> Administrator
                                // TODO: NKS_USERS_FLAGS_ADMIN_DESC -> This user can execute privileged operations
                                // TODO: NKS_USERS_FLAGS_ANONYMOUS -> Anonymous
                                // TODO: NKS_USERS_FLAGS_ANONYMOUS_DESC -> This user won't show up in the list of usernames
                                // TODO: NKS_USERS_FLAGS_DISABLED -> Disabled
                                // TODO: NKS_USERS_FLAGS_DISABLED_DESC -> This user is disabled and can't be used to sign in
                                new PresentationInputInfo(
                                    LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_FLAGS_PROMPT"), LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_FLAGSPROMPT"),
                                    new MultiComboBoxModule()
                                    {
                                        Name = LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_FLAGS_PROMPT"),
                                        Description = LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_FLAGS_PROMPT_DESC"),
                                        Choices =
                                        [
                                            new("",
                                            [
                                                new("",
                                                [
                                                    new(LanguageTools.GetLocalized("NKS_USERS_FLAGS_ADMIN"), LanguageTools.GetLocalized("NKS_USERS_FLAGS_ADMIN_DESC")),
                                                    new(LanguageTools.GetLocalized("NKS_USERS_FLAGS_ANONYMOUS"), LanguageTools.GetLocalized("NKS_USERS_FLAGS_ANONYMOUS_DESC")),
                                                    new(LanguageTools.GetLocalized("NKS_USERS_FLAGS_DISABLED"), LanguageTools.GetLocalized("NKS_USERS_FLAGS_DISABLED_DESC")),
                                                ])
                                            ])
                                        ],
                                    }
                                ),

                                // TODO: NKS_KERNEL_FIRSTRUN_EXTRAUSER_PROMPT -> Attributes
                                // TODO: NKS_KERNEL_FIRSTRUN_PRESENTATION_EXTRAUSERPROMPT -> Choose the attributes
                                // TODO: NKS_KERNEL_FIRSTRUN_EXTRAUSER_PROMPT_DESC -> Choose the attributes of this user
                                // TODO: NKS_KERNEL_FIRSTRUN_EXTRAUSER_PROMPT_YES_DESC -> Add an extra user
                                // TODO: NKS_KERNEL_FIRSTRUN_EXTRAUSER_PROMPT_NO_DESC -> Don't add an extra user
                                new PresentationInputInfo(
                                    LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_EXTRAUSER_PROMPT"), LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_EXTRAUSERPROMPT"),
                                    new ComboBoxModule()
                                    {
                                        Name = LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_EXTRAUSER_PROMPT"),
                                        Description = LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_EXTRAUSER_PROMPT_DESC"),
                                        Choices =
                                        [
                                            new("",
                                            [
                                                new("",
                                                [
                                                    new(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_THEMESET_YES"), LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_EXTRAUSER_PROMPT_YES_DESC")),
                                                    new(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_THEMESET_NO"), LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_EXTRAUSER_PROMPT_NO_DESC"), "", true, true),
                                                ])
                                            ])
                                        ],
                                    }, true
                                ),
                            ]
                        )
                    ]
                );
                while (!moveOn)
                {
                    PresentationTools.Present(firstRunPresUser, true, true);
                    string inputUser = firstRunPresUser.Pages[0].Inputs[0].InputMethod.GetValue<string?>() ?? "";
                    inputUser = string.IsNullOrEmpty(inputUser) ? "owner" : inputUser;
                    string pass = firstRunPresUser.Pages[0].Inputs[1].InputMethod.GetValue<string?>() ?? "";
                    int[] flagIndexes = firstRunPresUser.Pages[0].Inputs[2].InputMethod.GetValue<int[]?>() ?? [];
                    int addAnotherFlag = firstRunPresUser.Pages[0].Inputs[3].InputMethod.GetValue<int>();
                    UserFlags[] allFlags = [.. Enum.GetValues<UserFlags>().Where(uf => uf > 0)];
                    var selectedFlags = flagIndexes.Select(idx => allFlags[idx]);
                    try
                    {
                        UserManagement.AddUser(inputUser, pass);
                        foreach (var selectedFlag in selectedFlags)
                        {
                            var user = UserManagement.GetUser(inputUser);
                            user.Flags |= selectedFlag;
                        }
                        DebugWriter.WriteDebug(DebugLevel.I, "We shall move on.");
                        userStepFailureReason = "";
                        moveOn = addAnotherFlag == 1;
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
                                            new InputChoiceInfo(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_THEMESET_YES"), LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_AUTOUPDATECHECK_AGREE")),
                                            new InputChoiceInfo(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_THEMESET_NO"), LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_AUTOUPDATECHECK_DISAGREE")),
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
                                        // TODO: NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE4_TEXT1_NEW -> You're now ready to use the Nitrocid operating system!
                                        () => TextTools.FormatString(LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE4_TEXT1_NEW"))
                                    ]
                                },
                                new TextElement()
                                {
                                    Arguments =
                                    [
                                        LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE4_TEXT2") + "\n\n" +
                                        LanguageTools.GetLocalized("NKS_KERNEL_FIRSTRUN_PRESENTATION_PAGE4_TEXT3")
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
