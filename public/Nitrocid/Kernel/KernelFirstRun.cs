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
                    Translate.DoTranslation("Kernel first-run"),

                    // Presentation list
                    [
                        // First page - introduction
                        new PresentationPage(
                            // Page name
                            Translate.DoTranslation("Welcome!"),

                            // Page elements
                            [
                                new TextElement()
                                {
                                    Arguments =
                                    [
                                        Translate.DoTranslation("Welcome to Nitrocid Kernel! Thank you for trying it out!")
                                    ]
                                },
                                new TextElement()
                                {
                                    Arguments =
                                    [
                                        Translate.DoTranslation("To get started, press ENTER.")
                                    ]
                                }
                            ],

                            // Page inputs
                            [
                                new PresentationInputInfo(
                                    Translate.DoTranslation("Language"), Translate.DoTranslation("Select your language."),
                                    new ComboBoxModule()
                                    {
                                        Name = Translate.DoTranslation("Language"),
                                        Description = Translate.DoTranslation("Select your language. By default, the kernel uses the English language, but you can select any other language here."),
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
                InfoBoxNonModalColor.WriteInfoBox(Translate.DoTranslation("Saving settings..."));
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
                TextWriterColor.Write(Translate.DoTranslation("We apologize for your inconvenience, but the out-of-box experience has crashed. If you're sure that this is a defect in the experience, please report the crash to us with debugging logs.") + " {0}", ex.Message);
                TextWriterColor.Write(Translate.DoTranslation("Press any key to start the shell anyways, but please note that you may have to create your new user manually."));
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
                    Translate.DoTranslation("Kernel first-run"),

                    // Presentation list
                    [
                        // Second page - username creation
                        new PresentationPage(
                            // Page name
                            Translate.DoTranslation("Create your first user"),

                            // Page elements
                            [
                                new TextElement()
                                {
                                    Arguments =
                                    [
                                        Translate.DoTranslation("We'll help you create your own username. Select any name you want. This could be your nickname or your short name, as long as your username doesn't contain spaces and special characters and that it doesn't already exist. The following usernames are registered:")
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
                                    Translate.DoTranslation("Username"), Translate.DoTranslation("Enter the username"),
                                    new TextBoxModule()
                                    {
                                        Name = Translate.DoTranslation("Username"),
                                        Description = Translate.DoTranslation("Enter your new username. You should enter a new username that doesn't already exist."),
                                    }, true
                                ),
                                new PresentationInputInfo(
                                    Translate.DoTranslation("Password"), Translate.DoTranslation("Enter the password"),
                                    new TextBoxModule()
                                    {
                                        Name = Translate.DoTranslation("Password"),
                                        Description = Translate.DoTranslation("Enter your user password. You should choose a strong password for increased security."),
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
                        userStepFailureReason = Translate.DoTranslation("Failed to create username. Please ensure that your username doesn't contain spaces and special characters.");
                    }
                }

                Slideshow firstRunPresUpdates = new(
                    // Presentation name
                    Translate.DoTranslation("Kernel first-run"),

                    // Presentation list
                    [
                        // Fifth page - Automatic updates
                        new PresentationPage(
                            // Page name
                            Translate.DoTranslation("Automatic updates"),

                            // Page elements
                            [
                                new TextElement()
                                {
                                    Arguments =
                                    [
                                        Translate.DoTranslation("Nitrocid KS currently updates itself to get the most recent version that includes general improvements and bug fixes. New major versions usually include breaking changes and new exciting features.") + " " +
                                        Translate.DoTranslation("In addition to automatically checking for updates, Nitrocid KS can also download the update file automatically.") + " " +
                                        Translate.DoTranslation("You can always check for kernel updates using the \"update\" command.")
                                    ]
                                }
                            ],

                            // Page inputs
                            [
                                new PresentationInputInfo(
                                    Translate.DoTranslation("Automatic Update Check"), Translate.DoTranslation("Automatic Update Check"),
                                    new ComboBoxModule()
                                    {
                                        Name = Translate.DoTranslation("Automatic Update Check"),
                                        Description = Translate.DoTranslation("Do you want Nitrocid KS to automatically check for updates?"),
                                        Choices = [new("Choices", [new("Choices", [
                                            new InputChoiceInfo("y", Translate.DoTranslation("Yes, I do!")),
                                            new InputChoiceInfo("n", Translate.DoTranslation("No, thanks.")),
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
                    Translate.DoTranslation("Kernel first-run"),

                    // Presentation list
                    [
                        // Third page - get started
                        new PresentationPage(
                            // Page name
                            Translate.DoTranslation("Get Started!"),

                            // Page elements
                            [
                                new DynamicTextElement()
                                {
                                    Arguments =
                                    [
                                        () => TextTools.FormatString(Translate.DoTranslation("Congratulations! You now have a user account, {0}!"), user)
                                    ]
                                },
                                new TextElement()
                                {
                                    Arguments =
                                    [
                                        Translate.DoTranslation("Press the ENTER key to get started using the kernel and log-in to your new account. Good luck!")
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
                TextWriterColor.Write(Translate.DoTranslation("We apologize for your inconvenience, but the out-of-box experience has crashed. If you're sure that this is a defect in the experience, please report the crash to us with debugging logs.") + " {0}", ex.Message);
                TextWriterColor.Write(Translate.DoTranslation("Press any key to start the shell anyways, but please note that you may have to create your new user manually."));
                Input.ReadKey();
            }
        }
    }
}
