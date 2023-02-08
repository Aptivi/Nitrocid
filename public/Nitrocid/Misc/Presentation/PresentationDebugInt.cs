
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using ColorSeq;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Misc.Presentation.Elements;
using KS.Misc.Writers.ConsoleWriters;
using KS.TimeDate;
using System.Collections.Generic;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Presentation
{
    internal static class PresentationDebugInt
    {
        internal static Presentation Debug =>
            new(
                "Debugging the Presentation",
                new List<PresentationPage>()
                {
                    #region First page - Debugging just text elements
                    new PresentationPage("First page - Debugging just text elements",
                        new List<IElement>()
                        {
                            new TextElement() {
                                Arguments = new object[] {
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                }
                            },
                            new TextElement() {
                                Arguments = new object[] {
                                    "Enjoying yet? {0}Color treat!",
                                    new Color(ConsoleColors.Purple4_5f00af).VTSequenceForeground
                                }
                            }
                        }
                    ),
                    #endregion
                        
                    #region Second page - Debugging text and input elements
                    new PresentationPage("Second page - Debugging text and input elements",
                        new List<IElement>()
                        {
                            new TextElement() {
                                Arguments = new object[] {
                                    "{0}Lorem ipsum {1}dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore {2}magna aliqua. {1}Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. {2}Enim blandit {1}volutpat maecenas volutpat " +
                                    "blandit aliquam. {3}Ultricies {1}mi eget mauris pharetra. {3}Vitae {1}elementum curabitur vitae nunc sed " +
                                    "velit dignissim. {3}Tempor {1}orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc {2}pulvinar sapien {1}et ligula ullamcorper malesuada proin.",
                                    new Color(ConsoleColors.Green).VTSequenceForeground,
                                    ColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground,
                                    new Color(ConsoleColors.Yellow).VTSequenceForeground,
                                    new Color(ConsoleColors.Red).VTSequenceForeground
                                }
                            },
                            new TextElement() {
                                Arguments = new object[] {
                                    "Happy {0}hacking!",
                                    new Color(ConsoleColors.Green1).VTSequenceForeground
                                }
                            },
                            new InputElement() {
                                Arguments = new object[] {
                                    "\nDid you enjoy {0}testing? ",
                                    new Color(ConsoleColors.Green1).VTSequenceForeground
                                },
                                InvokeActionInput =
                                    (objs) => TextWriterWhereColor.WriteWhere($"You said \"{objs[0]}\".", PresentationTools.PresentationUpperInnerBorderLeft, ConsoleWrapper.CursorTop)
                            }
                        }
                    ),
                    #endregion
                        
                    #region Third page - Debugging dynamic text
                    new PresentationPage("Third page - Debugging dynamic text",
                        new List<IElement>()
                        {
                            new DynamicTextElement() {
                                Arguments = new object[] {
                                    () => TimeDateRenderers.Render()
                                }
                            }
                        }
                    ),
                    #endregion
                        
                    #region Fourth page - Debugging overflow check
                    new PresentationPage("Fourth page - Debugging overflow check",
                        new List<IElement>()
                        {
                            new TextElement() {
                                Arguments = new object[] {
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                }
                            },
                            new TextElement() {
                                Arguments = new object[] {
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                }
                            },
                            new TextElement() {
                                Arguments = new object[] {
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                }
                            },
                            new TextElement() {
                                Arguments = new object[] {
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                }
                            },
                            new TextElement() {
                                Arguments = new object[] {
                                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt " +
                                    "ut labore et dolore magna aliqua. Risus sed vulputate odio ut enim blandit. Ac tortor vitae " +
                                    "purus faucibus. Quis eleifend quam adipiscing vitae. Enim blandit volutpat maecenas volutpat " +
                                    "blandit aliquam. Ultricies mi eget mauris pharetra. Vitae elementum curabitur vitae nunc sed " +
                                    "velit dignissim. Tempor orci dapibus ultrices in iaculis nunc sed augue lacus. Cras tincidunt " +
                                    "lobortis feugiat vivamus at. Scelerisque fermentum dui faucibus in ornare quam viverra. " +
                                    "Tincidunt nunc pulvinar sapien et ligula ullamcorper malesuada proin."
                                }
                            }
                        }
                    ),
                    #endregion
                }
            );
    }
}
