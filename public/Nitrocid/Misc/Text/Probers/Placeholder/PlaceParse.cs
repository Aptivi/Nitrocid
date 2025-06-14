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
using System.IO;
using System.Collections.Generic;
using Terminaux.Colors;
using System.Linq;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Time;
using Nitrocid.Shell.ShellBase.Scripting;
using Nitrocid.Users;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Events;
using Nitrocid.Kernel.Power;
using Nitrocid.Kernel.Time.Timezones;
using Textify.General;
using Nitrocid.Misc.Text.Probers.Regexp;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Files;

namespace Nitrocid.Misc.Text.Probers.Placeholder
{
    /// <summary>
    /// Placeholder parsing module
    /// </summary>
    public static class PlaceParse
    {
        private readonly static List<PlaceInfo> customPlaceholders = [];
        private readonly static List<PlaceInfo> placeholders =
        [
            new PlaceInfo("user", (_) => UserManagement.CurrentUser.Username),
            new PlaceInfo("host", (_) => Config.MainConfig.HostName),
            new PlaceInfo("currentdirectory", (_) => FilesystemTools.CurrentDir),
            new PlaceInfo("currentdirectoryname", (_) => !string.IsNullOrEmpty(FilesystemTools.CurrentDir) ? new DirectoryInfo(FilesystemTools.CurrentDir).Name : ""),
            new PlaceInfo("shortdate", (_) => TimeDateRenderers.RenderDate(FormatType.Short)),
            new PlaceInfo("longdate", (_) => TimeDateRenderers.RenderDate(FormatType.Long)),
            new PlaceInfo("shorttime", (_) => TimeDateRenderers.RenderTime(FormatType.Short)),
            new PlaceInfo("longtime", (_) => TimeDateRenderers.RenderTime(FormatType.Long)),
            new PlaceInfo("date", (_) => TimeDateRenderers.RenderDate()),
            new PlaceInfo("time", (_) => TimeDateRenderers.RenderTime()),
            new PlaceInfo("timezone", (_) => TimeZones.GetCurrentZoneInfo().StandardName),
            new PlaceInfo("summertimezone", (_) => TimeZones.GetCurrentZoneInfo().DaylightName),
            new PlaceInfo("system", (_) => Environment.OSVersion.ToString()),
            new PlaceInfo("newline", (_) => CharManager.NewLine),
            new PlaceInfo("dollar", (_) => UserManagement.GetUserDollarSign()),
            new PlaceInfo("randomfile", (_) => FilesystemTools.GetRandomFileName()),
            new PlaceInfo("randomfolder", (_) => FilesystemTools.GetRandomFolderName()),
            new PlaceInfo("rid", (_) => KernelPlatform.GetCurrentRid()),
            new PlaceInfo("ridgeneric", (_) => KernelPlatform.GetCurrentGenericRid()),
            new PlaceInfo("termemu", (_) => KernelPlatform.GetTerminalEmulator()),
            new PlaceInfo("termtype", (_) => KernelPlatform.GetTerminalType()),
            new PlaceInfo("f", (c) => new Color(c).VTSequenceForeground),
            new PlaceInfo("b", (c) => new Color(c).VTSequenceBackground),
            new PlaceInfo("fgreset", (_) => KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground),
            new PlaceInfo("bgreset", (_) => KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground),
            new PlaceInfo("uptime", (_) => PowerManager.KernelUptime),
            new PlaceInfo("$", UESHVariables.GetVariable),
        ];

        /// <summary>
        /// Probes the placeholders found in string
        /// </summary>
        /// <param name="text">Specified string</param>
        /// <param name="ThrowIfFailure">Throw if the placeholder parsing fails</param>
        /// <returns>A string that has the parsed placeholders</returns>
        public static string ProbePlaces(string text, bool ThrowIfFailure = false)
        {
            EventsManager.FireEvent(EventType.PlaceholderParsing, text);
            try
            {
                // Parse the text for the following placeholders:
                DebugWriter.WriteDebug(DebugLevel.I, "Parsing text for placeholders...");
                var placeMatches = RegexpTools.Matches(text, /* lang=regex */ @"\<.*\>").ToArray();

                // Get all the placeholder matches and replace them as appropriate
                foreach (var placeMatch in placeMatches)
                {
                    // Get the argument (if any)
                    string place = placeMatch.Value;
                    string arg = "";
                    if (place.Contains(':'))
                        arg = place[(place.IndexOf(':') + 1)..(place.Length - 1)];
                    string placeNoArg = place.Replace($":{arg}>", ">");

                    // Fetch a placeholder
                    DebugWriter.WriteDebug(DebugLevel.I, "{0} placeholder found.", vars: [place]);
                    try
                    {
                        // Execute the action
                        var action = GetPlaceholderAction(placeNoArg);
                        string result = action(arg);
                        DebugWriter.WriteDebug(DebugLevel.I, "Result: {0}", vars: [result]);
                        text = text.Replace(place, result);
                    }
                    catch (Exception ex)
                    {
                        // Leave the text and the placeholder alone in this case.
                        DebugWriter.WriteDebug(DebugLevel.E, "Leaving placeholder alone because of failure: {0}", vars: [ex.Message]);
                        DebugWriter.WriteDebugStackTrace(ex);
                    }
                }

                // If successful, raise the parsed event
                EventsManager.FireEvent(EventType.PlaceholderParsed, text);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to parse placeholder {0}: {1}", vars: [text, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                EventsManager.FireEvent(EventType.PlaceholderParseError, text, ex);
                if (ThrowIfFailure)
                    throw new KernelException(KernelExceptionType.InvalidPlaceholder, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PARSEERROR"), ex.Message);
            }
            return text;
        }

        /// <summary>
        /// Registers a custom placeholder
        /// </summary>
        /// <param name="placeholder">Custom placeholder to register (may be without the &lt; and the &gt; marks)</param>
        /// <param name="placeholderAction">Action associated with the placeholder</param>
        public static void RegisterCustomPlaceholder(string placeholder, Func<string, string> placeholderAction)
        {
            // Sanity checks
            if (string.IsNullOrEmpty(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERNULL"));
            if (placeholderAction is null)
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERACTIONNULL"));

            // Try to register
            if (!IsPlaceholderRegistered($"<{placeholder}>"))
            {
                var place = new PlaceInfo(placeholder, placeholderAction);
                customPlaceholders.Add(place);
            }
        }

        /// <summary>
        /// Unregisters a custom placeholder
        /// </summary>
        /// <param name="placeholder">Custom placeholder to unregister (should be with the &lt; and the &gt; marks)</param>
        public static void UnregisterCustomPlaceholder(string placeholder)
        {
            // Sanity checks
            if (string.IsNullOrEmpty(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERNULL"));
            if (!IsPlaceholderRegistered(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERNOTREGGED"));
            if (!placeholder.StartsWith('<') || !placeholder.EndsWith('>'))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERFORMAT") + ": <place>");

            // Try to unregister
            var place = GetPlaceholder(placeholder);
            customPlaceholders.Remove(place);
        }

        /// <summary>
        /// Checks to see whether the placeholder is built in
        /// </summary>
        /// <param name="placeholder">Placeholder to query (should be with the &lt; and the &gt; marks)</param>
        /// <returns>True if the placeholder is in the list of built-in placeholders</returns>
        /// <exception cref="KernelException"></exception>
        public static bool IsPlaceholderBuiltin(string placeholder)
        {
            // Sanity checks
            if (string.IsNullOrEmpty(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERNULL"));
            if (!placeholder.StartsWith('<') || !placeholder.EndsWith('>'))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERFORMAT") + ": <place>");

            // Check to see if we have this placeholder
            string placeNoArg = StripPlaceholderArgs(placeholder);
            return placeholders.Any((pi) => $"<{pi.Placeholder}>" == placeNoArg);
        }

        /// <summary>
        /// Checks to see whether the placeholder is registered
        /// </summary>
        /// <param name="placeholder">Placeholder to query (should be with the &lt; and the &gt; marks)</param>
        /// <returns>True if the placeholder is in the list of placeholders</returns>
        /// <exception cref="KernelException"></exception>
        public static bool IsPlaceholderRegistered(string placeholder)
        {
            // Sanity checks
            if (string.IsNullOrEmpty(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERNULL"));
            if (!placeholder.StartsWith('<') || !placeholder.EndsWith('>'))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERFORMAT") + ": <place>");

            // Check to see if we have this placeholder
            string placeNoArg = StripPlaceholderArgs(placeholder);
            return
                IsPlaceholderBuiltin(placeholder) ||
                customPlaceholders.Any((pi) => $"<{pi.Placeholder}>" == placeNoArg);
        }

        /// <summary>
        /// Gets a placeholder from the placeholder name
        /// </summary>
        /// <param name="placeholder">Placeholder to query (should be with the &lt; and the &gt; marks)</param>
        public static PlaceInfo GetPlaceholder(string placeholder)
        {
            // Sanity checks
            if (string.IsNullOrEmpty(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERNULL"));
            if (!IsPlaceholderRegistered(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERNOTREGGED"));
            if (!placeholder.StartsWith('<') || !placeholder.EndsWith('>'))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERFORMAT") + ": <place>");

            // Try to get a placeholder
            string placeNoArg = StripPlaceholderArgs(placeholder);
            return
                IsPlaceholderBuiltin(placeholder) ?
                placeholders.First((pi) => $"<{pi.Placeholder}>" == placeNoArg) :
                customPlaceholders.First((pi) => $"<{pi.Placeholder}>" == placeNoArg);
        }

        /// <summary>
        /// Gets a placeholder action from the placeholder name
        /// </summary>
        /// <param name="placeholder">Placeholder to query (should be with the &lt; and the &gt; marks)</param>
        public static Func<string, string> GetPlaceholderAction(string placeholder)
        {
            // Sanity checks
            if (string.IsNullOrEmpty(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERNULL"));
            if (!IsPlaceholderRegistered(placeholder))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERNOTREGGED"));
            if (!placeholder.StartsWith('<') || !placeholder.EndsWith('>'))
                throw new KernelException(KernelExceptionType.InvalidPlaceholderAction, LanguageTools.GetLocalized("NKS_MISC_TEXT_PROBERS_PLACEHOLDER_EXCEPTION_PLACEHOLDERFORMAT") + ": <place>");

            // Try to get a placeholder action
            string placeNoArg = StripPlaceholderArgs(placeholder);
            return GetPlaceholder(placeNoArg).PlaceholderAction;
        }

        /// <summary>
        /// Strips the placeholder with arguments
        /// </summary>
        /// <param name="placeholder">Placeholder with arguments (should be with the &lt; and the &gt; marks)</param>
        /// <returns>Stripped placeholder</returns>
        public static string StripPlaceholderArgs(string placeholder)
        {
            string place = placeholder;
            string arg = "";
            if (place.Contains(':'))
                arg = place[(place.IndexOf(':') + 1)..(place.Length - 1)];
            string placeNoArg = place.Replace($":{arg}>", ">");
            return placeNoArg;
        }
    }
}
