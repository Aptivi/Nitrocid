using Microsoft.VisualBasic;
using System.Collections.Generic;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;

namespace Kernel_Simulator
{
    public static class ModParser
    {

        // Variables
        public interface IScript
        {
            void StartMod();
            void StopMod();
            string Cmd { get; set; }
            string Def { get; set; }
            string Name { get; set; }
            string Version { get; set; }
            void PerformCmd(string args = "");
            void InitEvents(string ev);
        }
        public static Dictionary<string, IScript> scripts = new Dictionary<string, IScript>();
        private readonly static string modPath = KernelTools.paths["Mods"];

        // ------------------------------------------- Generators -------------------------------------------
        private static IScript GenMod(string code) // For Visual Basic mods
        {
            using (VBCodeProvider provider = new VBCodeProvider())
            {
                CompilerParameters prm = new CompilerParameters()
                {
                    GenerateExecutable = false,
                    GenerateInMemory = true
                };

                // Add referenced assemblies
                TextWriterColor.Wdbg("Referenced assemblies will be added.");
                prm.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location); // It should reference itself
                prm.ReferencedAssemblies.Add("System.dll");
                prm.ReferencedAssemblies.Add("System.Core.dll");
                prm.ReferencedAssemblies.Add("System.Data.dll");
                prm.ReferencedAssemblies.Add("System.DirectoryServices.dll");
                prm.ReferencedAssemblies.Add("System.Xml.dll");
                prm.ReferencedAssemblies.Add("System.Xml.Linq.dll");
                TextWriterColor.Wdbg("Referenced assemblies added.");

                // Try to compile
                string namespc = typeof(IScript).Namespace;
                string[] modCode = new string[] { ("Imports " + namespc + Constants.vbNewLine + code) };
                CompilerResults res = provider.CompileAssemblyFromSource(prm, modCode);

                // Check to see if there are compilation errors
                TextWriterColor.Wdbg("Has errors: {0}", res.Errors.HasErrors);
                TextWriterColor.Wdbg("Has warnings: {0}", res.Errors.HasWarnings);
                if (res.Errors.HasErrors & (Flags.Quiet == false))
                {
                    TextWriterColor.Wln(Translate.DoTranslation("Mod can't be loaded because of the following: ", Translate.currentLang), "neutralText");
                    foreach (var errorName in res.Errors)
                    {
                        TextWriterColor.Wln(errorName.ToString(), "neutralText"); TextWriterColor.Wdbg(errorName.ToString());
                    }
                    return;
                }
                foreach (Type t in res.CompiledAssembly.GetTypes())
                {
                    if (t.GetInterface(typeof(IScript).Name) != null)
                        return (IScript)res.CompiledAssembly.CreateInstance(t.Name);
                }
            }

            return default(IScript);
        }
        private static IScript GenModCS(string code) // For C# Mods
        {
            using (CSharpCodeProvider provider = new CSharpCodeProvider())
            {
                CompilerParameters prm = new CompilerParameters()
                {
                    GenerateExecutable = false,
                    GenerateInMemory = true
                };

                // Add referenced assemblies
                TextWriterColor.Wdbg("Referenced assemblies will be added.");
                prm.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location); // It should reference itself
                prm.ReferencedAssemblies.Add("System.dll");
                prm.ReferencedAssemblies.Add("System.Core.dll");
                prm.ReferencedAssemblies.Add("System.Data.dll");
                prm.ReferencedAssemblies.Add("System.DirectoryServices.dll");
                prm.ReferencedAssemblies.Add("System.Xml.dll");
                prm.ReferencedAssemblies.Add("System.Xml.Linq.dll");
                TextWriterColor.Wdbg("Referenced assemblies added.");

                // Try to compile
                string namespc = typeof(IScript).Namespace;
                string[] modCode = new string[] { ("using " + namespc + ";" + Constants.vbNewLine + code) };
                CompilerResults res = provider.CompileAssemblyFromSource(prm, modCode);

                // Check to see if there are compilation errors
                TextWriterColor.Wdbg("Has errors: {0}", res.Errors.HasErrors);
                TextWriterColor.Wdbg("Has warnings: {0}", res.Errors.HasWarnings);
                if (res.Errors.HasErrors & (Flags.Quiet == false))
                {
                    TextWriterColor.Wln(Translate.DoTranslation("Mod can't be loaded because of the following: ", Translate.currentLang), "neutralText");
                    foreach (var errorName in res.Errors)
                    {
                        TextWriterColor.Wln(errorName.ToString(), "neutralText"); TextWriterColor.Wdbg(errorName.ToString());
                    }
                    return;
                }
                foreach (Type t in res.CompiledAssembly.GetTypes())
                {
                    if (t.GetInterface(typeof(IScript).Name) != null)
                        return (IScript)res.CompiledAssembly.CreateInstance(t.Name);
                }
            }

            return default(IScript);
        }

        // ------------------------------------------- Misc -------------------------------------------
        public static void ParseMods(bool StartStop)
        {
            // StartStop: If true, the mods start, otherwise, the mod stops.
            if (!Microsoft.VisualBasic.FileIO.FileSystem.DirectoryExists(modPath))
                Microsoft.VisualBasic.FileIO.FileSystem.CreateDirectory(modPath);
            int count = Microsoft.VisualBasic.FileIO.FileSystem.GetFiles(modPath).Count;
            if ((Flags.Quiet == false) & (count != 0) & (StartStop == true))
            {
                TextWriterColor.Wln(Translate.DoTranslation("mod: Loading mods...", Translate.currentLang), "neutralText");
                TextWriterColor.Wdbg("Mods are being loaded. Total mods with screensavers = {0}", count);
            }
            else if ((Flags.Quiet == false) & (count != 0) & (StartStop == false))
            {
                TextWriterColor.Wln(Translate.DoTranslation("mod: Stopping mods...", Translate.currentLang), "neutralText");
                TextWriterColor.Wdbg("Mods are being stopped. Total mods with screensavers = {0}", count);
            }
            if (StartStop == false)
            {
                foreach (IScript script in scripts.Values)
                {
                    script.StopMod();
                    TextWriterColor.Wdbg("script.StopMod() initialized. Mod name: {0} | Version: {0}", script.Name, script.Version);
                    if (!string.IsNullOrEmpty(script.Name) & !string.IsNullOrEmpty(script.Version))
                        TextWriterColor.Wln("{0} v{1} stopped", "neutralText", script.Name, script.Version);
                }
            }
            else
                foreach (string modFile in Microsoft.VisualBasic.FileIO.FileSystem.GetFiles(modPath))
                    StartParse(modFile, StartStop);
        }
        public static void StartParse(string modFile, bool StartStop = true)
        {
            modFile = modFile.Replace(modPath, "");
            if (!modFile.EndsWith(".m"))
                // Ignore all mods who doesn't end with .m
                TextWriterColor.Wdbg("Unsupported file type for mod file {0}.", modFile);
            else if (modFile.EndsWith("SS.m"))
                // Ignore all mods who ends with SS.m
                TextWriterColor.Wdbg("Mod file {0} is a screensaver and is ignored.", modFile);
            else if (modFile.EndsWith("CS.m"))
            {
                // Mod has a language of C#
                IScript script = GenModCS(System.IO.File.ReadAllText(modPath + modFile));
                FinalizeMods(script, modFile, StartStop);
            }
            else
            {
                IScript script = GenMod(System.IO.File.ReadAllText(modPath + modFile));
                FinalizeMods(script, modFile, StartStop);
            }
        }
        public static void FinalizeMods(IScript script, string modFile, bool StartStop = true)
        {
            if (!Information.IsNothing(script))
            {
                script.StartMod();
                TextWriterColor.Wdbg("script.StartMod() initialized. Mod name: {0} | Version: {0}", script.Name, script.Version);
                if (string.IsNullOrEmpty(script.Name))
                {
                    TextWriterColor.Wdbg("No name for {0}", modFile);
                    TextWriterColor.Wln(Translate.DoTranslation("Mod {0} does not have the name. Review the source code.", Translate.currentLang), "neutralText", modFile);
                    scripts.Add(script.Cmd, script);
                }
                else
                {
                    TextWriterColor.Wdbg("There is a name for {0}", modFile);
                    scripts.Add(script.Name, script);
                }
                if (string.IsNullOrEmpty(script.Version) & !string.IsNullOrEmpty(script.Name))
                {
                    TextWriterColor.Wdbg("{0}.Version = \"\" | {0}.Name = {1}", modFile, script.Name);
                    TextWriterColor.Wln(Translate.DoTranslation("Mod {0} does not have the version.", Translate.currentLang), "neutralText", script.Name);
                }
                else if (!string.IsNullOrEmpty(script.Name) & !string.IsNullOrEmpty(script.Version))
                {
                    TextWriterColor.Wdbg("{0}.Version = {2} | {0}.Name = {1}", modFile, script.Name, script.Version);
                    TextWriterColor.Wln(Translate.DoTranslation("{0} v{1} started", Translate.currentLang), "neutralText", script.Name, script.Version);
                }
                if (!string.IsNullOrEmpty(script.Cmd) & (StartStop == true))
                {
                    Shell.modcmnds.Add(script.Cmd);
                    if (string.IsNullOrEmpty(script.Def))
                    {
                        TextWriterColor.Wln(Translate.DoTranslation("No definition for command {0}.", Translate.currentLang), "neutralText", script.Cmd);
                        TextWriterColor.Wdbg("{0}.Def = Nothing, {0}.Def = \"Command defined by {1}\"", script.Cmd, script.Name);
                        script.Def = Translate.DoTranslation("Command defined by ", Translate.currentLang) + script.Name;
                    }
                    HelpSystem.moddefs.Add(script.Cmd, script.Def);
                }
            }
        }
    }

    class _failedMemberConversionMarker1
    {
    }
#error Cannot convert ImportsStatementSyntax - see comment for details
    /* Cannot convert ImportsStatementSyntax, CONVERSION ERROR: Conversion for ImportsStatement not implemented, please report this issue in 'Imports Kernel_Simulator.Mo...' at character 9810
       at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.DefaultVisit(SyntaxNode node)
       at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.VisitImportsStatement(ImportsStatementSyntax node)
       at Microsoft.CodeAnalysis.VisualBasic.Syntax.ImportsStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
       at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
       at ICSharpCode.CodeConverter.CSharp.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
       at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.VisitImportsStatement(ImportsStatementSyntax node)
       at Microsoft.CodeAnalysis.VisualBasic.Syntax.ImportsStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
       at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.ConvertMember(StatementSyntax member)

    Input: 
    Imports Kernel_Simulator.ModParser

     */
    class _failedMemberConversionMarker2
    {
    }
#error Cannot convert ImportsStatementSyntax - see comment for details
    /* Cannot convert ImportsStatementSyntax, CONVERSION ERROR: Conversion for ImportsStatement not implemented, please report this issue in 'Imports Kernel_Simulator.Te...' at character 9846
       at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.DefaultVisit(SyntaxNode node)
       at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.VisitImportsStatement(ImportsStatementSyntax node)
       at Microsoft.CodeAnalysis.VisualBasic.Syntax.ImportsStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
       at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.Visit(SyntaxNode node)
       at ICSharpCode.CodeConverter.CSharp.CommentConvertingNodesVisitor.DefaultVisit(SyntaxNode node)
       at Microsoft.CodeAnalysis.VisualBasic.VisualBasicSyntaxVisitor`1.VisitImportsStatement(ImportsStatementSyntax node)
       at Microsoft.CodeAnalysis.VisualBasic.Syntax.ImportsStatementSyntax.Accept[TResult](VisualBasicSyntaxVisitor`1 visitor)
       at ICSharpCode.CodeConverter.CSharp.VisualBasicConverter.NodesVisitor.ConvertMember(StatementSyntax member)

    Input: 
    Imports Kernel_Simulator.TextWriterColor

     */
    public class HelloArgs : ModParser.IScript
    {
        public string Cmd { get; set; }
        public string Def { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public void StartMod()
        {
            Cmd = "WhatDoISay";
            Def = "";
            Name = "HelloArgs";
            Version = "1.0";
        }
        public void StopMod()
        {
        }
        public void PerformCmd(string args = "")
        {
            string[] splitArgs = args.Split(new[] { ' ' });
            int ArgCount = 1;
            if (splitArgs.Length > 1)
            {
                if ((((splitArgs[0] ?? "") == "Say") & ((splitArgs[1] ?? "") == "Hello")))
                    TextWriterColor.Wln("Hello World", "neutralText");
                else
                    TextWriterColor.Wln("Make me say Hello by running \"WhatDoISay Say Hello.\"", "neutralText");
            }
            else
                TextWriterColor.Wln("Make me say Hello by running \"WhatDoISay Say Hello.\"", "neutralText");
            foreach (string arg in splitArgs)
            {
                TextWriterColor.Wln("Argument {0}: {1}", "neutralText", ArgCount, splitArgs[ArgCount - 1]);
                ArgCount += 1;
            }
        }
        public void InitEvents(string ev)
        {
        }
    }
}