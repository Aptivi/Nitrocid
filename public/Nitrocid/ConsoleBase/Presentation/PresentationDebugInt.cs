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
using Nitrocid.ConsoleBase.Presentation.Elements;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Time.Renderers;
using System.Linq;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace Nitrocid.ConsoleBase.Presentation
{
    internal static class PresentationDebugInt
    {
        internal static string[] data1 =
        [
            "Alex",
            "Zhao",
            "Agustin",
            "Jim",
            "Sarah"
        ];

        internal static Slideshow Debug =>
            new(
                "Debugging the Presentation",
                [
                    #region First page - Debugging just text elements
                    new PresentationPage("First page - Debugging just text elements",
                        [
                            new TextElement()
                            {
                                Arguments = [
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                ]
                            },
                            new TextElement()
                            {
                                Arguments = [
                                    "Enjoying yet? {0}Color treat!",
                                    new Color(ConsoleColors.Purple4_5f00af).VTSequenceForeground
                                ]
                            }
                        ]
                    ),
                    #endregion

                    #region Second page - Debugging text and input elements
                    new PresentationPage("Second page - Debugging text and input elements",
                        [
                            new TextElement()
                            {
                                Arguments = [
                                    "{0}Lorem ipsum {1}dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore {2}magna aliqua. {1}Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. {2}Enim blandit {1}volutpat maecenas volutpat " +
                                    "blandit aliquam. {3}Ultricies {1}mi eget mauris pharetra. {3}Vitae {1}elementum curabitur vitae nunc sed " +
                                    "velit dignissim. {3}Tempor {1}orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc {2}pulvinar sapien {1}et ligula ullamcorper malesuada proin.",
                                    new Color(ConsoleColors.Green).VTSequenceForeground,
                                    KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground,
                                    new Color(ConsoleColors.Yellow).VTSequenceForeground,
                                    new Color(ConsoleColors.Red).VTSequenceForeground
                                ]
                            },
                            new TextElement()
                            {
                                Arguments = [
                                    "Happy {0}hacking!",
                                    new Color(ConsoleColors.Green1).VTSequenceForeground
                                ]
                            },
                            new InputElement()
                            {
                                Arguments = [
                                    "\nDid you enjoy {0}testing? ",
                                    new Color(ConsoleColors.Green1).VTSequenceForeground
                                ],
                                InvokeActionInput =
                                    (objs) => TextWriterWhereColor.WriteWhere($"You said \"{objs[0]}\".", PresentationTools.PresentationUpperInnerBorderLeft, ConsoleWrapper.CursorTop)
                            }
                        ]
                    ),
                    #endregion

                    #region Third page - Debugging dynamic text
                    new PresentationPage("Third page - Debugging dynamic text",
                        [
                            new DynamicTextElement()
                            {
                                Arguments = [
                                    () => TimeDateRenderers.Render()
                                ]
                            }
                        ]
                    ),
                    #endregion

                    #region Fourth page - Debugging overflow check
                    new PresentationPage("Fourth page - Debugging overflow check",
                        [
                            new TextElement()
                            {
                                Arguments = [
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                ]
                            },
                            new TextElement()
                            {
                                Arguments = [
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                ]
                            },
                            new TextElement()
                            {
                                Arguments = [
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                ]
                            },
                            new TextElement()
                            {
                                Arguments = [
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                ]
                            },
                            new TextElement()
                            {
                                Arguments = [
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                ]
                            }
                        ]
                    ),
                    #endregion

                    #region Fifth page - Debugging choice input
                    new PresentationPage("Fifth page - Debugging choice input",
                        [
                            new TextElement()
                            {
                                Arguments = [
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                ]
                            },
                            new ChoiceInputElement()
                            {
                                Arguments =
                                [
                                    "Ultricies mi eget mauris pharetra:",
                                    data1
                                ],
                                InvokeActionInput =
                                    (objs) => TextWriterWhereColor.WriteWhere($"You chose \"{objs[0]}\", a {((string)objs[0] == "Sarah" ? "girl" : "boy")}.", PresentationTools.PresentationUpperInnerBorderLeft, ConsoleWrapper.CursorTop)
                            },
                            new ChoiceInputElement()
                            {
                                Arguments =
                                [
                                    "Ultricies mi eget mauris pharetra sapien et ligula:",
                                    data1,
                                    "Akshay",
                                    "Aladdin",
                                    "Bella",
                                    "Billy",
                                    "Blake",
                                    "Bobby",
                                    "Chandran",
                                    "Colin",
                                    "Connor",
                                    "Debbie",
                                    "Eduard",
                                    "David",
                                    "Paul",
                                    "Ella",
                                    "Elizabeth",
                                    "Fitz",
                                    "Gary",
                                    "Hendrick",
                                    "Henry",
                                    "Jared",
                                    "Jasmine",
                                    "Johnny",
                                    "Sofia",
                                    "Thalia",
                                    "Vincent"
                                ],
                                InvokeActionInput =
                                    (objs) =>
                                    TextWriterWhereColor.WriteWhere($"You chose \"{objs[0]}\", a " +
                                        $"{((string)objs[0] is "Sarah" or "Bella" or "Debbie" or "Ella" or "Elizabeth" or "Jasmine" or "Sofia" or "Thalia"
                                    ? "girl" : "boy")}.",
                                    PresentationTools.PresentationUpperInnerBorderLeft, ConsoleWrapper.CursorTop)
                            }
                        ]
                    ),
                    #endregion

                    #region Sixth page - Debugging multiple choice input
                    new PresentationPage("Sixth page - Debugging choice input",
                        [
                            new TextElement()
                            {
                                Arguments = [
                                    "Tempor orci dapibus ultrices in iaculis nunc sed augue lacus."
                                ]
                            },
                            new MultipleChoiceInputElement()
                            {
                                Arguments =
                                [
                                    "Ultricies mi eget mauris pharetra:",
                                    data1
                                ],
                                InvokeActionInput =
                                    (objs) =>
                                    {
                                        string[] names = objs[0].ToString().Split(';');
                                        TextWriterWhereColor.WriteWhere($"You chose {names.Length} persons, {names.Count((name) => name == "Sarah")} of which are girls.", PresentationTools.PresentationUpperInnerBorderLeft, ConsoleWrapper.CursorTop);
                                    }
                            },
                            new MultipleChoiceInputElement()
                            {
                                Arguments =
                                [
                                    "Ultricies mi eget mauris pharetra sapien et ligula:",
                                    data1,
                                    "Akshay",
                                    "Aladdin",
                                    "Bella",
                                    "Billy",
                                    "Blake",
                                    "Bobby",
                                    "Chandran",
                                    "Colin",
                                    "Connor",
                                    "Debbie",
                                    "Eduard",
                                    "David",
                                    "Paul",
                                    "Ella",
                                    "Elizabeth",
                                    "Fitz",
                                    "Gary",
                                    "Hendrick",
                                    "Henry",
                                    "Jared",
                                    "Jasmine",
                                    "Johnny",
                                    "Sofia",
                                    "Thalia",
                                    "Vincent"
                                ],
                                InvokeActionInput =
                                    (objs) =>
                                    {
                                        string[] names = objs[0].ToString().Split(';');
                                        TextWriterWhereColor.WriteWhere($"You chose {names.Length} persons, " +
                                            $"{names.Count((name) =>
                                               name is "Sarah" or "Bella" or "Debbie" or "Ella" or "Elizabeth" or "Jasmine" or "Sofia" or "Thalia")}" +
                                            $" of which are girls.", PresentationTools.PresentationUpperInnerBorderLeft, ConsoleWrapper.CursorTop);
                                    }
                            }
                        ]
                    ),
                    #endregion
                ]
            );
    }
}
