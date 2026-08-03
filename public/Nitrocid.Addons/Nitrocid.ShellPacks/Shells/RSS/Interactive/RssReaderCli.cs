//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nettify.Rss.Instance;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Interactive;
using Nitrocid.Base.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Textify.General;
using SpecProbe.Software.Platform;
using Terminaux.Base.Extensions;
using System.Threading;
using Nitrocid.ShellPacks.Shells.RSS.Tools;
using Nitrocid.Base.Drivers.Regexp;
using Nitrocid.Base.Kernel.Debugging;
using Terminaux.Inputs;
using Terminaux.Inputs.Modules;

namespace Nitrocid.ShellPacks.Shells.RSS.Interactive
{
    /// <summary>
    /// RSS Reader TUI class
    /// </summary>
    public class RssReaderCli : BaseInteractiveTui<RSSFeed, RSSArticle>, IInteractiveTui<RSSFeed, RSSArticle>
    {
        internal List<RSSFeed> feeds = [];
        internal string filterRegex = "";
        internal RSSFilterType filterType;
        internal Timer? timer;
        internal ManualResetEvent mre = new(false);

        /// <inheritdoc/>
        public override InteractiveTuiHelpPage[] HelpPages =>
        [
            new()
            {
                HelpTitle = /* Localizable */ "NKS_SHELLPACKS_RSS_READERCLI_HELP01_TITLE",
                HelpDescription = /* Localizable */ "NKS_SHELLPACKS_RSS_READERCLI_HELP01_DESC",
                HelpBody =
                    LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_HELP01_BODY") + "\n\n" +
#pragma warning disable NLOC0001
                    LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_COMMON_HELP_MOREINFO") + ": https://aptivi.gitbook.io/aptivi/nitrocid-ks-manual/fundamentals/simulated-kernel-features/extra-features/more-networking/rss-client",
#pragma warning restore NLOC0001
            }
        ];

        /// <inheritdoc/>
        public override bool SecondPaneInteractable =>
            true;

        /// <inheritdoc/>
        public override IEnumerable<RSSFeed> PrimaryDataSource =>
            feeds;

        /// <inheritdoc/>
        public override IEnumerable<RSSArticle> SecondaryDataSource
        {
            get
            {
                IEnumerable<RSSArticle> articles = [];
                if (feeds.Count > 0)
                {
                    articles = feeds[FirstPaneCurrentSelection - 1].FeedArticles;
                    if (!string.IsNullOrEmpty(filterRegex))
                    {
                        try
                        {
                            switch (filterType)
                            {
                                case RSSFilterType.Name:
                                    articles = articles.Where((article) => !RegexpTools.IsMatch(article.ArticleTitle, filterRegex));
                                    break;
                                case RSSFilterType.Desc:
                                    articles = articles.Where((article) => !RegexpTools.IsMatch(article.ArticleDescription, filterRegex));
                                    break;
                                case RSSFilterType.NameDesc:
                                    articles = articles.Where((article) => !RegexpTools.IsMatch(article.ArticleTitle, filterRegex) && !RegexpTools.IsMatch(article.ArticleDescription, filterRegex));
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, $"Failed to filter RSS articles with pattern {filterRegex}");
                            DebugWriter.WriteDebugStackTrace(ex);
                            filterRegex = "";
                        }
                    }
                }
                return articles;
            }
        }

        /// <inheritdoc/>
        public override string GetInfoFromItemSecondary(RSSArticle item)
        {
            // Get some info from the article
            RSSArticle selectedArticle = item;
            bool hasTitle = !string.IsNullOrEmpty(selectedArticle.ArticleTitle);
            bool hasDescription = !string.IsNullOrEmpty(selectedArticle.ArticleDescription);

            // Generate the rendered text
            string finalRenderedArticleTitle =
                hasTitle ?
                $"{selectedArticle.ArticleTitle}" :
                LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_UNKNOWNTITLE") + $" -> {selectedArticle.ArticleLink}";
            string finalRenderedArticleBody =
                hasDescription ?
                selectedArticle.ArticleDescription :
                LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_NOCONTENTS") + $" {selectedArticle.ArticleLink}.";

            // Render them to the second pane
            return
                ConsoleFormatting.GetFormattingSequences(ConsoleFormattingType.Intense) +
                finalRenderedArticleTitle + CharManager.NewLine + CharManager.NewLine +
                ConsoleFormatting.GetFormattingSequences(ConsoleFormattingType.Default) +
                finalRenderedArticleBody;
            ;
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem(RSSFeed item)
        {
            var statusBuilder = new StringBuilder();
            statusBuilder.Append($"{(!string.IsNullOrEmpty(filterRegex) ? "[*] " : "")}");
            statusBuilder.Append($"{item.FeedUrl} - ");
            statusBuilder.Append($"{item.FeedTitle} - ");
            statusBuilder.Append(item.FeedDescription);
            return statusBuilder.ToString();
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(RSSFeed item) =>
            item.FeedTitle;

        /// <inheritdoc/>
        public override string GetStatusFromItemSecondary(RSSArticle item)
        {
            var statusBuilder = new StringBuilder();
            statusBuilder.Append($"{(!string.IsNullOrEmpty(filterRegex) ? "[*] " : "")}");
            statusBuilder.Append($"{item.ArticleLink} - ");
            statusBuilder.Append(item.ArticleTitle);
            return statusBuilder.ToString();
        }

        /// <inheritdoc/>
        public override string GetEntryFromItemSecondary(RSSArticle item) =>
            item.ArticleTitle;

        internal void ShowArticleInfo(RSSArticle? item)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            if (item is null)
                return;
            bool hasTitle = !string.IsNullOrEmpty(item.ArticleTitle);
            bool hasDescription = !string.IsNullOrEmpty(item.ArticleDescription);
            bool hasVars = item.ArticleVariables.Count > 0;

            string finalRenderedArticleTitle =
                hasTitle ?
                $"{item.ArticleTitle}" :
                LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_UNKNOWNTITLE") + $" -> {item.ArticleLink}";
            string finalRenderedArticleBody =
                hasDescription ?
                item.ArticleDescription :
                LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_NOCONTENTS") + $" {item.ArticleLink}.";
            string finalRenderedArticleVars =
                hasVars ?
                $"  - {string.Join("\n  - ", item.ArticleVariables.Select((kvp) => $"{kvp.Key} [{kvp.Value.InnerText}]"))}" :
                LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_ARTICLEINFO_NOREV");
            finalInfoRendered.AppendLine(finalRenderedArticleTitle);
            finalInfoRendered.AppendLine(finalRenderedArticleBody);
            finalInfoRendered.AppendLine(finalRenderedArticleVars);

            // Now, render the info box
            InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
        }

        internal void OpenArticleLink(RSSArticle? item)
        {
            // Check to see if we have a link
            if (item is null)
                return;
            bool hasLink = !string.IsNullOrEmpty(item.ArticleLink);
            if (!hasLink)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_NOLINK"), Settings.InfoBoxSettings);
                return;
            }

            // Now, open the host browser
            try
            {
                PlatformHelper.PlatformOpen(item.ArticleLink);
            }
            catch (Exception e)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_HOSTBROWSEROPENFAILED") + $" {e.Message}", Settings.InfoBoxSettings);
            }
        }

        internal void OpenFeedLink(RSSFeed? item)
        {
            // Check to see if we have a link
            if (item is null)
                return;
            bool hasLink = !string.IsNullOrEmpty(item.FeedUrl);
            if (!hasLink)
            {
                // TODO: NKS_SHELLPACKS_RSS_READERCLI_NOFEEDLINK -> This feed doesn't have a link.
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_NOFEEDLINK"), Settings.InfoBoxSettings);
                return;
            }

            // Now, open the host browser
            try
            {
                PlatformHelper.PlatformOpen(item.FeedUrl);
            }
            catch (Exception e)
            {
                // TODO: NKS_SHELLPACKS_RSS_READERCLI_HOSTBROWSEROPENFEEDFAILED -> Can't open the host browser to the article link.
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_HOSTBROWSEROPENFEEDFAILED") + $" {e.Message}", Settings.InfoBoxSettings);
            }
        }

        internal void RefreshFeed(RSSFeed? feed) =>
            feed?.Refresh();

        internal void AddFeedPrompt()
        {
            // Prompt for new feed
            // TODO: NKS_SHELLPACKS_RSS_READERCLI_NEWFEEDPROMPT -> Write an RSS feed link for your news site.
            string feedLink = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_NEWFEEDPROMPT"), Settings.InfoBoxSettings);
            try
            {
                var feed = new RSSFeed(feedLink, RSSFeedType.Infer);
                feed.Refresh();
                feeds.Add(feed);
            }
            catch (Exception e)
            {
                // TODO: NKS_SHELLPACKS_RSS_READERCLI_NEWFEEDFAILED -> Adding new feed has failed.
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_NEWFEEDFAILED") + $" {e.Message}", Settings.InfoBoxSettings);
            }
        }

        internal void RefreshAllFeeds()
        {
            mre.Reset();
            foreach (var feed in feeds)
                feed.Refresh();
            mre.Set();
        }

        internal void FilterArticlesPrompt()
        {
            // Prompt for regex and filter type
            try
            {
                // TODO: NKS_SHELLPACKS_RSS_READERCLI_FILTERCRITERIA_NAME -> Filter criteria
                // TODO: NKS_SHELLPACKS_RSS_READERCLI_FILTERCRITERIA_DESC -> Write a regular expression for RSS feed article filtering. Articles that meet your criteria will be hidden from view. For example, (deal|offer|sale) will hide deals, offers, and sales.
                // TODO: NKS_SHELLPACKS_RSS_READERCLI_FILTERTYPE_NAME -> Filter type
                // TODO: NKS_SHELLPACKS_RSS_READERCLI_FILTERTYPE_DESC -> Specify the filter target, whether you want to filter articles by name, by description, or by both.
                // TODO: NKS_SHELLPACKS_RSS_READERCLI_FILTERTYPE_NAME_NAME -> Filter by name
                // TODO: NKS_SHELLPACKS_RSS_READERCLI_FILTERTYPE_DESC_NAME -> Filter by description
                // TODO: NKS_SHELLPACKS_RSS_READERCLI_FILTERTYPE_NAMEDESC_NAME -> Filter by name and description
                // TODO: NKS_SHELLPACKS_RSS_READERCLI_FILTERPROMPT -> Specify how you want to filter articles.
                InputModule[] modules = [
                    new TextBoxModule()
                    {
                        Name = LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_FILTERCRITERIA_NAME"),
                        Description = LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_FILTERCRITERIA_DESC"),
                        Value = filterRegex
                    },
                    new ComboBoxModule()
                    {
                        Name = LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_FILTERTYPE_NAME"),
                        Description = LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_FILTERTYPE_DESC"),
                        Choices = [new("", [new("",
                        [
                            new(nameof(RSSFilterType.Name), LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_FILTERTYPE_NAME_NAME"), "", true, true),
                            new(nameof(RSSFilterType.Desc), LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_FILTERTYPE_DESC_NAME")),
                            new(nameof(RSSFilterType.NameDesc), LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_FILTERTYPE_NAMEDESC_NAME")),
                        ])])],
                        Value = 0,
                    }
                ];
                bool provided = InfoBoxMultiInputColor.WriteInfoBoxMultiInput(modules, LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_FILTERPROMPT"), Settings.InfoBoxSettings);
                if (!provided)
                    return;

                // Validate regex before setting one
                // NKS_SHELLPACKS_RSS_READERCLI_INVALIDFILTER -> Invalid filter criteria.
                string finalRegex = (string?)modules[0].Value ?? "";
                if (!RegexpTools.IsValidRegex(finalRegex))
                {
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_INVALIDFILTER"), Settings.InfoBoxSettings);
                    return;
                }

                // Set the regex filter
                RSSFilterType regexFilterType = (RSSFilterType)((int?)modules[1].Value ?? 0);
                filterRegex = finalRegex;
                filterType = regexFilterType;
            }
            catch (Exception e)
            {
                // TODO: NKS_SHELLPACKS_RSS_READERCLI_FILTERFAILED -> Filtering articles has failed.
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_READERCLI_FILTERFAILED") + $" {e.Message}", Settings.InfoBoxSettings);
            }
        }

        internal void ResetFilter()
        {
            filterRegex = "";
            filterType = RSSFilterType.Name;
        }
    }
}
