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

using Newtonsoft.Json;
using Nitrocid.Files.Operations;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;

namespace Nitrocid.Extras.BeepSynth.Tools
{
    internal static class SynthTools
    {
        public static SynthInfo GetSynthInfoFromFile(string path)
        {
            string contents = Reading.ReadContentsText(path);
            return GetSynthInfo(contents);
        }

        public static SynthInfo GetSynthInfo(string synthJson)
        {
            var synthInfo = JsonConvert.DeserializeObject<SynthInfo>(synthJson) ??
                throw new KernelException(KernelExceptionType.Console, Translate.DoTranslation("Can't get synth info from the provided representation."));
            return synthInfo;
        }
    }
}
