//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using BassBoom.Basolia.File;
using BassBoom.Basolia.Playback;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files;
using KS.Files.Operations.Querying;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using System;
using System.Threading;

namespace Nitrocid.Extras.BassBoom.Commands
{
    /// <summary>
    /// Plays a sound file
    /// </summary>
    /// <remarks>
    /// This command allows you to play a sound file.
    /// </remarks>
    class PlaySoundCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string path = parameters.ArgumentsList[0];
            path = FilesystemTools.NeutralizePath(path);
            if (!Checking.FileExists(path))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Can't play sound because the file is not found."), KernelColorType.Error);
                return 29;
            }
            try
            {
                FileTools.OpenFile(path);
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Opened music file successfully."), KernelColorType.Success);
            }
            catch (Exception ex)
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Can't open music file.") + $" {ex.Message}", KernelColorType.Error);
                return ex.HResult;
            }
            if (FileTools.IsOpened)
            {
                try
                {
                    PlaybackTools.PlayAsync();
                    if (!SpinWait.SpinUntil(() => PlaybackTools.Playing, 15000))
                    {
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Can't play sound because of timeout."), KernelColorType.Error);
                        return 30;
                    }
                    while (PlaybackTools.Playing)
                    {
                        if (ConsoleWrapper.KeyAvailable)
                        {
                            var key = Input.DetectKeypress();
                            if (key.Key == ConsoleKey.Q)
                                PlaybackTools.Stop();
                        }
                    }
                }
                catch (Exception ex)
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Can't play sound.") + $" {ex.Message}", KernelColorType.Error);
                    return ex.HResult;
                }
                finally
                {
                    FileTools.CloseFile();
                }
            }
            return 0;
        }

    }
}
