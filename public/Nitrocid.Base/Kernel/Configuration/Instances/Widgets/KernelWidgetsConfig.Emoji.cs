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

#if NKS_EXTENSIONS
using Nitrocid.Base.Kernel.Extensions;
#endif

namespace Nitrocid.Base.Kernel.Configuration.Instances
{
    /// <summary>
    /// Widgets kernel configuration instance
    /// </summary>
    public partial class KernelWidgetsConfig : BaseKernelConfig
    {
        private string emojiWidgetCurrentEmoticon = "gem-stone";

        /// <summary>
        /// Whether to cycle between emoticons
        /// </summary>
        public bool EmojiWidgetCycleEmoticons { get; set; }
        /// <summary>
        /// Emoticon name to show
        /// </summary>
        public string EmojiWidgetEmoticonName
        {
            get => emojiWidgetCurrentEmoticon;
            set
            {
#if NKS_EXTENSIONS
                if (AddonTools.GetAddon(InterAddonTranslations.GetAddonName(KnownAddons.ExtrasImagesIcons)) is null)
                {
#endif
                    emojiWidgetCurrentEmoticon = value;
                    return;
#if NKS_EXTENSIONS
                }
                var type = InterAddonTools.GetTypeFromAddon(KnownAddons.ExtrasImagesIcons, "Nitrocid.Extras.Images.Icons.Tools.IconsTools");
                var hasIcon = (bool?)InterAddonTools.ExecuteCustomAddonFunction(KnownAddons.ExtrasImagesIcons, "HasIcon", type, value) ?? false;
                emojiWidgetCurrentEmoticon = hasIcon ? value : emojiWidgetCurrentEmoticon;
#endif
            }
        }
    }
}
