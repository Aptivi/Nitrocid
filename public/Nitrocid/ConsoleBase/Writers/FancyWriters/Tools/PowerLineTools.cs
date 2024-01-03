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
using System;
using System.Collections.Generic;
using System.Text;
using Terminaux.Colors;

namespace Nitrocid.ConsoleBase.Writers.FancyWriters.Tools
{
    /// <summary>
    /// PowerLine tools
    /// </summary>
    public static class PowerLineTools
    {
        /// <summary>
        /// Renders the segments
        /// </summary>
        /// <param name="segments">List of segments to render</param>
        public static string RenderSegments(List<PowerLineSegment> segments) =>
            RenderSegments(segments, KernelColorTools.GetColor(KernelColorType.Background));

        /// <summary>
        /// Renders the segments
        /// </summary>
        /// <param name="segments">List of segments to render</param>
        /// <param name="EndingColor">Ending background color for the last segment transition</param>
        public static string RenderSegments(List<PowerLineSegment> segments, Color EndingColor)
        {
            // PowerLine glyphs
            char transitionChar = Convert.ToChar(0xE0B0);

            // Builder
            var SegmentStringBuilder = new StringBuilder();

            for (int segmentIdx = 0; segmentIdx < segments.Count; segmentIdx++)
            {
                // Get the segment
                var segment = segments[segmentIdx];

                // If we're in segment index 1 or higher, this means that we're going to have to make a transition, so we need
                // to get the last segment.
                if (segmentIdx > 0)
                {
                    // Get the last segment
                    var lastsegment = segments[segmentIdx - 1];

                    // Get the colors for the transition
                    var backAsFore = lastsegment.SegmentBackground;
                    var nextBack = segment.SegmentBackground;

                    // Now, put transition to our string
                    SegmentStringBuilder.Append(backAsFore.VTSequenceForeground);
                    SegmentStringBuilder.Append(nextBack.VTSequenceBackground);
                    SegmentStringBuilder.AppendFormat("{0}", segment.SegmentTransitionIcon != default ? segment.SegmentTransitionIcon : transitionChar);
                }

                // Now, try to append the PowerLine segment and its contents
                bool appendIcon = segment.SegmentIcon != default;
                SegmentStringBuilder.Append(segment.SegmentForeground.VTSequenceForeground);
                SegmentStringBuilder.Append(segment.SegmentBackground.VTSequenceBackground);
                if (appendIcon)
                    SegmentStringBuilder.AppendFormat(" {0}", segment.SegmentIcon);
                SegmentStringBuilder.AppendFormat(" {0} ", segment.SegmentText);

                // If the segment is the last one, make the final transition!
                if (segmentIdx == segments.Count - 1)
                {
                    SegmentStringBuilder.Append(segment.SegmentBackground.VTSequenceForeground);
                    SegmentStringBuilder.Append(EndingColor.VTSequenceBackground);
                    SegmentStringBuilder.AppendFormat("{0} ", transitionChar);
                }
            }

            // Return the final string
            return SegmentStringBuilder.ToString();
        }
    }
}
