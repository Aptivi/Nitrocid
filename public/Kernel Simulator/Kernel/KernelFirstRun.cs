
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase;
using KS.ConsoleBase.Inputs;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Presentation;
using KS.Misc.Presentation.Elements;
using KS.Misc.Writers.ConsoleWriters;
using KS.Users;
using System;
using System.Collections.Generic;

namespace KS.Kernel
{
    internal static class KernelFirstRun
    {
        internal static void PresentFirstRun()
        {
            try
            {
                // Some variables
                string user = "owner";
                string stepFailureReason = "";
                bool moveOn = false;

                // Populate the first run presentations in case language changed during the first start-up
                Presentation firstRunPresIntro = new(
                    // Presentation name
                    Translate.DoTranslation("Kernel first-run"),

                    // Presentation list
                    new List<PresentationPage>()
                    {
                        // First page - introduction
                        new PresentationPage(
                            // Page name
                            Translate.DoTranslation("Welcome!"),
                        
                            // Page elements
                            new List<IElement>()
                            {
                                new TextElement()
                                { 
                                    Arguments = new object[] 
                                    { 
                                        Translate.DoTranslation("Welcome to Nitrocid Kernel! Thank you for trying it out!") + "\n"
                                    }
                                },
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("To get started, press ENTER.")
                                    }
                                }
                            }
                        )
                    }
                );

                Presentation firstRunPresStep1 = new(
                    // Presentation name
                    Translate.DoTranslation("Kernel first-run"),

                    // Presentation list
                    new List<PresentationPage>()
                    {
                        // Second page - username creation
                        new PresentationPage(
                            // Page name
                            Translate.DoTranslation("Create your first user"),
                        
                            // Page elements
                            new List<IElement>()
                            {
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("We'll help you create your own username. Select any name you want. This could be your nickname or your short name, as long as your username doesn't contain spaces and special characters.") + "\n" +
                                        (string.IsNullOrWhiteSpace(stepFailureReason) ? (stepFailureReason + "\n") : "")
                                    }
                                },
                                new InputElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Enter the username") + ": "
                                    },
                                    InvokeActionInput = 
                                        (args) => 
                                            user = string.IsNullOrWhiteSpace((string)args[0]) ? "owner" : (string)args[0]
                                },
                                new MaskedInputElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Enter the password") + ": "
                                    },
                                    InvokeActionInput = 
                                        (args) => {
                                            try
                                            {
                                                UserManagement.AddUser(user, (string)args[0]);
                                                DebugWriter.WriteDebug(DebugLevel.I, "We shall move on.");
                                                moveOn = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                DebugWriter.WriteDebug(DebugLevel.I, "We shouldn't move on. Failed to create username. {0}", ex.Message);
                                                DebugWriter.WriteDebugStackTrace(ex);
                                                stepFailureReason = Translate.DoTranslation("Failed to create username. Please ensure that your username doesn't contain spaces and special characters.");
                                            }
                                        }
                                },
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Press the ENTER key to continue.") + "\n"
                                    }
                                }
                            }
                        )
                    }
                );

                Presentation firstRunPresOutro = new(
                    // Presentation name
                    Translate.DoTranslation("Kernel first-run"),

                    // Presentation list
                    new List<PresentationPage>()
                    {
                        // Third page - get started
                        new PresentationPage(
                            // Page name
                            Translate.DoTranslation("Get Started!"),
                        
                            // Page elements
                            new List<IElement>()
                            {
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Congratulations! You now have a user account, {0}!") + "\n",
                                        user
                                    }
                                },
                                new TextElement()
                                {
                                    Arguments = new object[]
                                    {
                                        Translate.DoTranslation("Press the ENTER key to get started using the kernel and log-in to your new account. Good luck!") + "\n"
                                    }
                                }
                            }
                        )
                    }
                );

                // Present all presentations
                Presentation[] firstRuns = { firstRunPresIntro, firstRunPresStep1, firstRunPresOutro };
                for (int step = 0; step < firstRuns.Length; step++)
                {
                    // Put in loop if the presentation contains input
                    var firstRun = firstRuns[step];
                    DebugWriter.WriteDebug(DebugLevel.I, "First run: step {0}", step);
                    if (PresentationTools.PresentationContainsInput(firstRun))
                    {
                        // Contains input.
                        DebugWriter.WriteDebug(DebugLevel.I, "Presentation contains input.");
                        while (!moveOn)
                            PresentationTools.Present(firstRun, true);
                        moveOn = false;
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Presentation doesn't contain input.");
                        PresentationTools.Present(firstRun, true);
                    }
                }
                DebugWriter.WriteDebug(DebugLevel.I, "Out of first run");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error in first run: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                ConsoleWrapper.Clear();
                TextWriterColor.Write(Translate.DoTranslation("We apologize for your inconvenience, but the out-of-box experience has crashed. If you're sure that this is a defect in the experience, please report the crash to us with debugging logs.") + " {0}", ex.Message);
                TextWriterColor.Write(Translate.DoTranslation("Press any key to start the shell anyways, but please note that you may have to create your new user manually."));
                Input.DetectKeypress();
            }
        }
    }
}
