using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.Misc.Writers.FancyWriters;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// It lets you test the figlet print to print every text, using the font and colors that you need. It has an additional feature of variables.
    /// </summary>
    class Test_PrintFigletFCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var Parts = new List<string>(ListArgsOnly);
            ColorTools.ColTypes Color = (ColorTools.ColTypes)Convert.ToInt32(ListArgsOnly[0]);
            var FigletFont = FigletTools.GetFigletFont(ListArgsOnly[1]);
            object[] Vars = ListArgsOnly[2].Split(';');
            string Text = ListArgsOnly[3];
            FigletColor.WriteFiglet(Text, FigletFont, Color, Vars);
        }

    }
}
