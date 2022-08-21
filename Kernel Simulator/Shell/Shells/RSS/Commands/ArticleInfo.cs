using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.ShellBase.Commands;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Shell.Shells.RSS.Commands
{
    /// <summary>
    /// Gets article information
    /// </summary>
    /// <remarks>
    /// If you want to know more about the article, you can use this command to get extensive information about the article. Some feeds provide extra arguments to the article to make getting information even more vital to those who need it.
    /// <br></br>
    /// It shows you the article title, the link, the description, and the extra arguments and their values, if available.
    /// </remarks>
    class RSS_ArticleInfoCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            int ArticleIndex = (int)Math.Round(Conversions.ToDouble(ListArgsOnly[0]) - 1d);
            if (ArticleIndex > RSSShellCommon.RSSFeedInstance.FeedArticles.Count - 1)
            {
                TextWriterColor.Write(Translate.DoTranslation("Article number couldn't be bigger than the available articles."), true, ColorTools.ColTypes.Error);
                DebugWriter.Wdbg(DebugLevel.E, "Tried to access article number {0}, but count is {1}.", ArticleIndex, RSSShellCommon.RSSFeedInstance.FeedArticles.Count - 1);
            }
            else
            {
                var Article = RSSShellCommon.RSSFeedInstance.FeedArticles[ArticleIndex];
                TextWriterColor.Write("- " + Translate.DoTranslation("Title:") + " ", false, ColorTools.ColTypes.ListEntry);
                TextWriterColor.Write(Article.ArticleTitle, true, ColorTools.ColTypes.ListValue);
                TextWriterColor.Write("- " + Translate.DoTranslation("Link:") + " ", false, ColorTools.ColTypes.ListEntry);
                TextWriterColor.Write(Article.ArticleLink, true, ColorTools.ColTypes.ListValue);
                foreach (string Variable in Article.ArticleVariables.Keys)
                {
                    if (!(Variable == "title") & !(Variable == "link") & !(Variable == "summary") & !(Variable == "description") & !(Variable == "content"))
                    {
                        TextWriterColor.Write("- {0}: ", false, ColorTools.ColTypes.ListEntry, Variable);
                        TextWriterColor.Write(Article.ArticleVariables[Variable].InnerText, true, ColorTools.ColTypes.ListValue);
                    }
                }
                TextWriterColor.Write(Kernel.Kernel.NewLine + Article.ArticleDescription, true, ColorTools.ColTypes.Neutral);
            }
        }

    }
}