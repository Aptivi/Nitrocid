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

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json.Linq;
using Nitrocid.Base.Kernel.Debugging;
using Textify.Versioning;

namespace Nitrocid.Base.Kernel.Updates
{
    /// <summary>
    /// Kernel update class instance
    /// </summary>
    public class KernelUpdate
    {

        /// <summary>
        /// Updated kernel version
        /// </summary>
        public SemVer? UpdateVersion { get; private set; }

        /// <summary>
        /// Update file URL
        /// </summary>
        public Uri UpdateURL { get; private set; }

        /// <summary>
        /// Kind of update
        /// </summary>
        public UpdateKind UpdateKind { get; private set; }

        /// <summary>
        /// Is the kernel up to date?
        /// </summary>
        public bool Updated { get; private set; }

        /// <summary>
        /// Installs a new instance of class KernelUpdate
        /// </summary>
        /// <param name="UpdateToken">The kernel update token</param>
        /// <param name="kind">The kernel update kind</param>
        protected internal KernelUpdate(JToken UpdateToken, UpdateKind kind)
        {
            // Sort the versions (We sometimes release servicing versions of earlier series, like 0.0.8.x, and the GitHub API sorts the releases based
            // on the date of the release, so we retry sorting them, this time, by version, so we get the list in the below format.)
            // 
            // Before:
            // [ 0.0.21.5, 0.0.19.5, 0.0.18.5, 0.0.17.7, 0.0.16.13, 0.0.12.8, 0.0.8.12, 0.0.21.4, 0.0.21.3, 0.0.20.6, 0.0.21.2, ... ]
            // After:
            // [ 0.0.21.5, 0.0.21.4, 0.0.21.3, 0.0.21.2, 0.0.21.1,  0.0.21.0, 0.0.20.6, 0.0.20.5, 0.0.20.4, 0.0.20.3, 0.0.20.2, ... ]
            // 
            // After we do this, Nitrocid KS should recognize newer servicing versions based on the current series (i.e. KS 0.0.21.3 didn't notify
            // the user that 0.0.21.4 was available due to 0.0.8.12 and versions that came after coming as first according to the API until 0.0.21.5
            // arrived)
            List<(SemVer? UpdateVersion, Uri UpdateURL)> SortedVersions = [];
            string specifier = kind == UpdateKind.BinaryLite ? "bin-lite" : "bin";
            foreach (JToken KernelUpdate in UpdateToken)
            {
                if (KernelUpdate is null)
                    continue;

                // We usually prefix versions with vx.x.x.x-xxx on Nitrocid KS releases.
                string tagName = KernelUpdate.SelectToken("tag_name")?.ToString() ?? "";
                tagName = tagName.StartsWith('v') ? tagName[1..] : tagName;
                SemVer? KernelUpdateVer = default;
                if (tagName.Split('.').Length > 3)
                    KernelUpdateVer = SemVer.ParseWithRev(tagName);
                else
                    KernelUpdateVer = SemVer.Parse(tagName);

                // Now, get the appropriate -bin URLs
                string KernelUpdateURL = "";
                var assets = KernelUpdate.SelectToken("assets");
                if (assets is null)
                    continue;
                foreach (var asset in assets)
                {
                    string url = (string?)asset["browser_download_url"] ?? "";
                    if (url.EndsWith($"-{specifier}.zip") ||
                        url.EndsWith($"-{specifier}.rar"))
                    {
                        KernelUpdateURL = url;
                        break;
                    }
                }
                DebugWriter.WriteDebug(DebugLevel.I, "Update information: {0}, {1}.", vars: [KernelUpdateVer?.ToString(), KernelUpdateURL]);
                if (!string.IsNullOrEmpty(KernelUpdateURL))
                    SortedVersions.Add((KernelUpdateVer, new Uri(KernelUpdateURL)));
            }
            SortedVersions =
            [
                .. SortedVersions.OrderByDescending((x) =>
                    {
                        var updateVersion = x.UpdateVersion;
                        if (updateVersion is null)
                            return new Version();
                        return new Version(updateVersion.MajorVersion, updateVersion.MinorVersion, updateVersion.PatchVersion, updateVersion.RevisionVersion);
                    }
                ),
            ];
            DebugWriter.WriteDebug(DebugLevel.I, "Found {0} kernel updates.", vars: [SortedVersions.Count]);

            // Get the latest version found
            var CurrentVer = KernelReleaseInfo.VersionFull;
            var UpdateVer = SortedVersions[0].UpdateVersion;
            var UpdateURI =
                kind == UpdateKind.Addons && UpdateVer is not null && CurrentVer is not null ?
                new Uri(SortedVersions[0].UpdateURL.ToString().Replace(UpdateVer.ToString(), CurrentVer.ToString()).Replace($"-{specifier}", "-addons")) :
                SortedVersions[0].UpdateURL;
            DebugWriter.WriteDebug(DebugLevel.I, "Update version: {0}", vars: [UpdateVer?.ToString()]);
            DebugWriter.WriteDebug(DebugLevel.I, "Update URL: {0}", vars: [UpdateURI.ToString()]);

            // Install the values
            UpdateVersion = UpdateVer;
            UpdateURL = UpdateURI;
            UpdateKind = kind;

            // If the updated version is lower or equal to the current version, consider the kernel up-to-date.
            if (UpdateVersion is not null && CurrentVer is not null)
                Updated = UpdateVersion <= CurrentVer;
            else
                Updated = true;
            DebugWriter.WriteDebug(DebugLevel.I, "Is this kernel up-to-date? {0}", vars: [Updated]);
        }

    }
}
