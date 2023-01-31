# Modding guide

## What is the mod for the kernel?

The mod is the kernel extension file that loads on boot and adds extensions to the kernel, the shell, and everything. It can also respond to events.

This is useful if you want to add your own extensions to the kernel, like event handlers for the kernel.

> [!TIP]
> To get started to our Nitrocid KS API, visit [this page](https://aptivi.github.io/NitrocidKS/).

## Mod format

The mods have the file extension of `.dll`, and can support more than one code file for each mod. This will allow you to make bigger mods that can't fit on one source file, or if you want to separate some parts of the big source code to multiple fragments.

## How to start your own mod on Visual Studio?

> [!NOTE]
> We recommend following the template repository for making your own mod [here](https://github.com/Aptivi/KSModTemplate).

If you're going to make your mod, follow these steps:

1. On the Start Page, click on `New Project`
2. Click on `Class Library (.NET Framework)` or `Class Library`, select VB or C#, and name your mod or modpack. Select Framework as `.NET Framework 4.8` or `.NET 6.0`. When you're finished, click `Create`.
3. Right-click on References in the Solution Explorer, and press `Manage NuGet packages...`
4. Go to `Browse`, and find `Nitrocid KS` and install it.
5. You will see that your KS executable files are added to the references. In your project file, this will be added:
```xml
    <PackageReference Include="KS">
      <Version>0.0.23.0</Version>
    </PackageReference>
```
6. The code will be ready in your ModName codefile:
```vb
Public Class ModName
    'Your code here
End Class
```

Now, follow these steps to create your first mod:

1. Between the Public Class... and the End Class lines, let Visual Studio know that you're going to create your KS mod by writing: `Implements IScript`
2. Define properties for mod information by putting below the Implements IScript:
```vb
Property Commands As Dictionary(Of String, CommandInfo) Implements IScript.Commands
Property ModPart As String Implements IScript.ModPart
Property Def As String Implements IScript.Def
Property Name As String Implements IScript.Name
Property Version As String Implements IScript.Version
```
3. Make your start mod sub named StartMod() that implements the IScript.StartMod, by writing:
```vb
Sub StartMod() Implements IScript.StartMod
    'If you're going to add commands, write your commands here. Explain what are your commands and what they're going to do.
    Commands = New Dictionary(Of String, CommandInfo) From {{"command", New CommandInfo("command", ShellType.Shell, "", {"<Required> [Optional]"}, True, 1, New MyModCommand)}, ...}

    'Replace ModName with your mod name whatever you like, but it SHOULD reflect the mod purpose. You can also use verbs, adjectives, space galaxy names, and so on. This field is required.
    Name = "ModName"

    'You can specify your mod version, but it should follow the versioning guidelines and you can find it on the Internet. This field is required.
    Version = "1.0"

    'Specify the shell command type
    CmdType = ShellCommandType.Shell
    
    'The name of the mod part
    ModPart = "Main"

    'Your code below
End Sub
```
4. Replace every `'Your code below` comment with your code. Repeat this step on all the interface subs
5. Make your mod stop sub named StopMod() that implements the IScript.StopMod, by writing: (You can leave it blank)
```vb
Sub StopMod() Implements IScript.StopMod
    'Your code below
End Sub
```
6. Make a command handler, which can be one of the following forms:
   i) If you're making your commands in your mod, make a class that implements both CommandExecutor and ICommand for each command and include your statements for each:
```vb
Class MyModCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
        'Your code below for your command
    End Sub

End Class
```
   ii) If you're making your commands which handle text arguments, write the response code below:
```vb
Class MyModCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
        If StringArgs = "Something" Then
            'Write your code
        End If
    End Sub

End Class
```
   iii) If you're making your commands which handle subsequent arguments, write the response code below:
```vb
Class MyModCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
        If ListArgsOnly(0) = "Arg1" Then
            'Write your code for the first argument
        End If
    End Sub

End Class
```
7. Make an event handler code for your mod, which can be one of the following:
   i) If you're making your event handler which handles what happened in the kernel, write the handle code below:
```vb
Sub InitEvents(ByVal ev As String) Implements IScript.InitEvents
    'Replace EventName with events that are found on "Events for Mod Developers"
    If ev = "EventName" Then
        'Your code below
    End If
End Sub
```
   ii) If you're handling multiple events, write the handle code below:
```vb
Sub InitEvents(ByVal ev As String) Implements IScript.InitEvents
    'Replace EventName with events that are found on "Events for Mod Developers"
    If ev = "EventName" Then
        'Your code below
    ElseIf ev = "AnotherEvent" Then
        'Your code below
    End If
End Sub
```
   iii) IMPORTANT NOTICE! Never try to use infinite loops in handlers unless you're making infinite loops that exits if the specified condition is met, or in the test environment. That will lock the kernel up.

8. Make the same event handler code, but this time, with arguments provided:
```vb
Sub InitEvents(ByVal ev As String, ParamArray Args As Object()) Implements IScript.InitEvents
    'Replace EventName with events that are found on "Events for Mod Developers"
    If ev = "EventName" Then
        'Your code below
    ElseIf ev = "AnotherEvent" Then
        'Your code below
    End If
End Sub
```
9. Right-click on the solution and press Build.
10. Copy the output `.dll` file to KSMods directory in your profile folder (`/home/<user>/` in Linux, and `Users\<user>\` in Windows)
11. Run your Nitrocid KS you've just referenced to in your project.

## Optional Stuff

1. You can make your subs anywhere on the class, but if:
   - they're on the different class, make a separate code file for it:
```vb
Public Class AnotherClass
    'Your definitions below, and so your subs, functions, interfaces, etc.
End Class
```
   - they're trying to re-initialize the mod by re-calling StartMod(), Try so on your test environment first, then the production environment if that worked properly.
2. The new subs or functions should meet the following conditions:
   - They shouldn't make an infinite loop unless you're making them that exits if specified conditions are met
   - They shouldn't try to cause errors with the kernel.
   - If you're an exploiter and are making the exploit code for the kernel, do so on your test environment first then the production environment, then make your CVE report so we get attention and fix that quickly.
   - If your mod is going to extend the kernel, place your extension codes on separate subs
   - Put your sub call on one of the four subs that implements the IScript interface. Ex. If you're going to make a sub that's going to be called on mod start, place your sub call on the StartMod() sub, and then your code on your sub.
3. If you're going to add imports, these rules must be met:
   - Don't import "KS" by itself. KS does that automatically
   - When importing modules/classes like TextWriterColor, it's written like this: `Imports KS.Misc.Writers.TextWriterColor`
4. You can add dependencies for your mods.
5. You can also add manual page files to your mods. Consult the [Mod Manual Page](Mod-Manual-Page.md) for more info.

## Example

Here is the simplest example of how to make a mod:

### Hello World on Mod Start, and Goodbye World on Mod Stop

Hello World is the popular starting example for all of the programmers. These examples usually start with printing the "Hello World" string to the console output or command prompt. To make your first Hello World mod, follow the project creation steps, then follow these steps:

1. Write below the (assuming that your mod name is HelloGuys) Public Class HelloGuys: `Implements IScript`
```vb
Public Class HelloGuys
    Implements IScript
End Class
```
2. Write above the Public Class HelloGuys:
```vb
    Imports KS.ConsoleBase.ColorTools
    Imports KS.Misc.Writers.ConsoleWriters
    Imports KS.Modifications
    Imports KS.Shell.ShellBase
```
3. Make your start mod event handler by writing:
```vb
    Property Commands As Dictionary(Of String, CommandInfo) Implements IScript.Commands
    Property ModPart As String Implements IScript.ModPart
    Property Name As String Implements IScript.Name
    Property Version As String Implements IScript.Version
    Sub StartMod() Implements IScript.StartMod
        Name = "HelloGuys"
        Version = "1.0"
        ModPart = "Main"
        W("Hello World", True, ColTypes.Neutral)
    End Sub
```
4. Make your stop event handler by writing:
```vb
    Sub StopMod() Implements IScript.StopMod
        W("Goodbye World", True, ColTypes.Neutral)
    End Sub
```
5. Since we're not implementing commands nor event handlers, we're going to leave these blank:
```vb
    Sub InitEvents(ByVal ev As String) Implements IScript.InitEvents
    End Sub
    Sub InitEvents(ByVal ev As String, ParamArray Args As Object()) Implements IScript.InitEvents
    End Sub
```
6. The code should look like this in VB:
```vb
    Imports KS.ConsoleBase.ColorTools
    Imports KS.Misc.Writers.ConsoleWriters
    Imports KS.Modifications
    Imports KS.Shell.ShellBase

    Public Class HelloGuys
        Implements IScript
        Property Commands As Dictionary(Of String, CommandInfo) Implements IScript.Commands
        Property ModPart As String Implements IScript.ModPart
        Property Name As String Implements IScript.Name
        Property Version As String Implements IScript.Version
        Sub StartMod() Implements IScript.StartMod
            Name = "HelloGuys"
            Version = "1.0"
            ModPart = "Main"
            Write("Hello World", True, ColTypes.Neutral)
        End Sub
        Sub StopMod() Implements IScript.StopMod
            Write("Goodbye World", True, ColTypes.Neutral)
        End Sub
        Sub InitEvents(ev As String) Implements IScript.InitEvents
        End Sub
        Sub InitEvents(ev As String, ParamArray Args As Object()) Implements IScript.InitEvents
        End Sub
    End Class
```
...Or in C#:
```cs
    using System.Collections.Generic;
    using KS.ConsoleBase;
    using KS.Modifications;
    using KS.Shell.ShellBase;
    using KS.Misc.Writers.ConsoleWriters;

    public class HelloGuys : IScript
    {
        public Dictionary<string, CommandInfo> Commands { get; set; }
        public string ModPart { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public void StartMod()
        {
            Name = "HelloGuys";
            Version = "1.0";
            ModPart = "Main";
            TextWriterColor.Write("Hello World", true, ColorTools.ColTypes.Neutral);
        }
        public void StopMod()
        {
            TextWriterColor.Write("Goodbye World", true, ColorTools.ColTypes.Neutral);
        }
        public void InitEvents(string ev)
        {
        }
        public void InitEvents(string ev, params object[] Args)
        {
        }
    }
```
7. Right-click on the solution and press Build.
8. Run the target KS once you copied the generated `.dll` file.

## More Examples

If you want to check out more examples, feel free to check them out in the [KSModExamples](https://github.com/Aptivi/KSModExamples) respository in GitHub.
