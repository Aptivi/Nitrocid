using KS.ConsoleBase.Colors;
using KS.Misc.Writers.FancyWriters;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// It lets you test the figlet print to print every text, using the font and colors that you need.
    /// </summary>
    class Test_PrintFigletCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            ColorTools.ColTypes Color = (ColorTools.ColTypes)Convert.ToInt32(ListArgsOnly[0]);
            var FigletFont = FigletTools.GetFigletFont(ListArgsOnly[1]);
            string Text = ListArgsOnly[2];
            FigletColor.WriteFiglet(Text, FigletFont, Color);
        }

    }
}
