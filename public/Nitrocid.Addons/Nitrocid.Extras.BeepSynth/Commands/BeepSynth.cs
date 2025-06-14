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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Extras.BeepSynth.Tools;
using Nitrocid.Files;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Kernel.Threading;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Terminaux.Base;

namespace Nitrocid.Extras.BeepSynth.Commands
{
    /// <summary>
    /// Plays a beep synth
    /// </summary>
    /// <remarks>
    /// This command allows you to read a beep synth file represented with the help of JSON.
    /// </remarks>
    class BeepSynthCommand : BaseCommand, ICommand
    {
        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string path = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            if (!FilesystemTools.FileExists(path))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_BEEPSYNTH_FILENOTFOUND", "Nitrocid.Extras.BeepSynth"), KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Console);
            }    
            var synthInfo = SynthTools.GetSynthInfoFromFile(path);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_BEEPSYNTH_NOWPLAYING", "Nitrocid.Extras.BeepSynth") + ": ", false, KernelColorType.ListEntry);
            TextWriters.Write(synthInfo.Name, KernelColorType.ListValue);
            for (int i = 0; i < synthInfo.Chapters.Length; i++)
            {
                SynthInfo.Chapter chapter = synthInfo.Chapters[i];
                TextWriters.Write($"- [{i + 1}/{synthInfo.Chapters.Length}] ", false, KernelColorType.NeutralText);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_BEEPSYNTH_CHAPTERNAME", "Nitrocid.Extras.BeepSynth") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write(chapter.Name, KernelColorType.ListValue);
                for (int j = 0; j < chapter.Synths.Length; j++)
                {
                    string synth = chapter.Synths[j];
                    var split = synth.Split(' ');
                    if (split.Length != 2)
                    {
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_BEEPSYNTH_INVALIDSYNTH", "Nitrocid.Extras.BeepSynth") + $" [{i + 1}.{j + 1}]", KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.Console);
                    }
                    if (!int.TryParse(split[0], out int freq))
                    {
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_BEEPSYNTH_INVALIDFREQ", "Nitrocid.Extras.BeepSynth") + $" [{i + 1}.{j + 1}]", KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.Console);
                    }
                    if (!int.TryParse(split[1], out int ms))
                    {
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_BEEPSYNTH_INVALIDDURATION", "Nitrocid.Extras.BeepSynth") + $" [{i + 1}.{j + 1}]", KernelColorType.Error);
                        return KernelExceptionTools.GetErrorCode(KernelExceptionType.Console);
                    }
                    if (freq == 0)
                        ThreadManager.SleepNoBlock(ms);
                    else
                        ConsoleWrapper.Beep(freq, ms);
                }
            }
            return 0;
        }

    }
}
