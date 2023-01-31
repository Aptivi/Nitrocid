# Custom splash guide

## What is the custom splash for the kernel?

The custom splash is an assembly file that loads before the splash is displayed. It allows you to create custom splashes from simple progress bars to complex console art.

> [!TIP]
> To get started to our Nitrocid KS API, visit [this page](https://aptivi.github.io/NitrocidKS/).

## Custom splash format

The mods have the file extension of `.dll`, and can support more than one code file for each mod. This will allow you to make bigger mods that can't fit on one source file, or if you want to separate some parts of the big source code to multiple fragments.

## How to start your own custom splash on Visual Studio?

> [!NOTE]
> We recommend following the template repository for making your own mod [here](https://github.com/Aptivi/KSCustomSplashTemplate).

If you're going to make your custom splash, follow these steps:

1. On the Start Page, click on `New Project`
2. Click on `Class Library (.NET Framework)` or `Class Library`, select VB or C#, and name your splash. Select Framework as `.NET Framework 4.8` or `.NET 6.0`. When you're finished, click `Create`.
3. Right-click on References in the Solution Explorer, and press `Manage NuGet packages...`
4. Go to `Browse`, and find `Nitrocid KS` and install it.
5. You will see that your KS executable files are added to the references. In your project file, this will be added:
```xml
    <PackageReference Include="KS">
      <Version>0.0.21.1</Version>
    </PackageReference>
```
6. The code will be ready in your SplashName codefile:
```vb
Public Class SplashName
    'Your code here
End Class
```

Now, follow these steps to create your first mod:

1. Between the Public Class... and the End Class lines, let Visual Studio know that you're going to create your KS mod by writing: `Implements ISplash`
2. Define properties for mod information by putting below the Implements ISplash: (Change the values as appropriate)
```vb
Property SplashClosing As Boolean Implements ISplash.SplashClosing
Property ProgressWritePositionX As Integer Implements ISplash.ProgressWritePositionX
Property ProgressWritePositionY As Integer Implements ISplash.ProgressWritePositionY
Property ProgressReportWritePositionX As Integer Implements ISplash.ProgressReportWritePositionX
Property ProgressReportWritePositionY As Integer Implements ISplash.ProgressReportWritePositionY
Property SplashDisplaysProgress As Boolean Implements ISplash.SplashDisplaysProgress
Property ISplash.SplashName As String Implements ISplash.SplashName
```
3. Make your opening splash sub named Opening() that implements the ISplash.Opening, by writing:
```vb
Sub Opening() Implements ISplash.Opening
    'Your code below
End Sub
```
4. Replace every `'Your code below` comment with your code. Repeat this step on all the interface subs
5. Make your display splash sub named Display() that implements the ISplash.Display, by writing:
```vb
Sub Display() Implements ISplash.Display
    'Your code below
End Sub
```
6. Make your progress report splash sub named Report() that implements the ISplash.Report, by writing:
```vb
Sub Report(Progress As Integer, ProgressReport As String, ProgressWritePositionX As Integer, ProgressWritePositionY As Integer, ProgressReportWritePositionX As Integer, ProgressReportWritePositionY As Integer, ParamArray Vars() As Object) Implements ISplash.Report
    'Your code below
End Sub
```
7. Make your closing splash sub named Closing() that implements the ISplash.Closing, by writing:
```vb
Sub Closing() Implements ISplash.Closing
    'Your code below
End Sub
```
8. Right-click on the solution and press Build.
9. Copy the output `.dll` file to KSSplashes directory in your profile folder (`/home/<user>/` in Linux, and `Users\<user>\` in Windows)
10. Run your Nitrocid KS you've just referenced to in your project, and set it to display your splash using the `settings` command. Be sure to reboot the kernel.

## Optional Stuff

1. You can make your subs anywhere on the class, but if:
   - they're on the different class, make a separate code file for it:
```vb
Public Class AnotherClass
    'Your definitions below, and so your subs, functions, interfaces, etc.
End Class
```
2. The new subs or functions should meet the following conditions:
   - They shouldn't make an infinite loop unless you're making them that exits if specified conditions are met
   - They shouldn't try to cause errors with the kernel.
   - If you're an exploiter and are making the exploit code for the splash implementation, do so on your test environment first then the production environment, then make your CVE report so we get attention and fix that quickly.
3. If you're going to add imports, these rules must be met:
   - Don't import "KS" by itself. KS does that automatically
   - When importing modules/classes like TextWriterColor, it's written like this: `Imports KS.Misc.Writers.TextWriterColor`

## Examples

If you want to check out the examples, feel free to check them out in the [KSModExamples](https://github.com/Aptivi/KSModExamples) respository in GitHub.
