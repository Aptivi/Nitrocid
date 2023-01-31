## Test shell commands

The types of the colors, which are used to display messages colorfully, are located in the ColTypes enumeration found in ColorTools.

Refer to [Events for Mod Developers](../development/Events-for-Mod-Developers.md) for the events that can be used in the `<Event>` argument.

`<ErrorType>` options:
```
S = Severe
F = Fatal
U = Unrecoverable
D = Double panic
C = Continuable
```
`<RebootTime>` is measured in seconds. It is used if `<Reboot>` is True or 1.

You can find strings inside the text files in the [Resources](https://github.com/Aptivi/NitrocidKS/tree/master/Kernel%20Simulator/Resources) folder.

Refer to [this document](../misc/Placeholders.md) for the list of placeholders.

The testing shell provides these commands:

| Command                | Description | Usage
|:-----------------------|:------------|:------
| print                  | It lets you test the `W()` call to print every text, using the lines and colors that you need. | `print <Color> <Line> <Message>`
| printf                 | It lets you test the `W()` call to print every text, using the lines and colors that you need. It has an additional feature of variables, but are not parsed yet, so they are parsed as text, but will be fixed in the upcoming release. | `printf <Color> <Line> <Variable1;Variable2;Variable3;...> <Message>`
| printd                 | It lets you send any message to the debugger, using `Wdbg()` call. It doesn't provide support for variables unlike `printdf`. It only works if you have enabled the debugger which you can enable by `debug 1`. | `printd <Message>`
| printdf                | It lets you send any message to the debugger, using `Wdbg()` call. It provides support for variables, but are not parsed yet. It only works if you have enabled the debugger which you can enable by `debug 1`. | `printdf <Variable1;Variable2;Variable3;...> <Message>`
| printsep               | It lets you print the separator using any text you want. | `printsep <Message>`
| printsepf              | It lets you print the separator using any text you want with format support. | `printsepf <Variable1;Variable2;Variable3;...> <Message>`
| printsepcolor          | It lets you print the separator using any text you want with color support. | `printsepcolor <color> <Message>`
| printsepcolorf         | It lets you print the separator using any text you want with color and format support. | `printsepcolorf <color> <Variable1;Variable2;Variable3;...> <Message>`
| testevent              | It lets you raise any event. If you have loaded mods, you can use this command for testing event raises. | `testevent <Event>`
| probehw                | It lets you probe hardware in the testing session. | `probehw`
| panic                  | It force crashes the kernel using custom exception types, messages, reboot times, etc. It does not provide support for variables. | `panic <ErrorType> <Reboot> <RebootTime> <Description>`
| panicf                 | It force crashes the kernel using custom exception types, messages, reboot times, etc. It provides support for variables, but are not parsed yet. | `panic <ErrorType> <Reboot> <RebootTime> <Variable1;Variable2;Variable3;...> <Description>`
| translate              | It lets you translate strings that are found in the current language file in the source code from the current source language to the target language, and prints it in the console. | `translate <Lang> <Message>`
| places                 | It lets you parse placeholders in the text without the option of changing color and the newline. | `places <Message>`
| loadmods               | It lets you load mods in the testing shell. | `loadmods`
| stopmods               | It lets you stop mods in the testing shell.  | `stopmods`
| reloadmods             | It lets you reload mods in the testing shell.  | `reloadmods`
| blacklistmod           | It lets you blacklist a mod in the testing shell.  | `blacklistmod <mod>`
| unblacklistmod         | It lets you remove a mod from the blacklist in the testing shell.  | `unblacklistmod <mod>`
| debug                  | It lets you enable and disable debugging mode. It only enables the local debugging which will write to your home directory. This allows `printd` and `printdf` to function. | `debug <Enable>`
| rdebug                 | It lets you enable remote debugger inside the debugging core. This allows users who need to see what's going on in another computer running KS to see its debugging logs. It uses port number `3014` and can be changed. | `rdebug <Enable>`
| testmd5                | It lets you estimate the time taken to encode a specified string on milliseconds using MD5 algorithm. | `testmd5 <message>`
| testsha1               | It lets you estimate the time taken to encode a specified string on milliseconds using SHA1 algorithm. | `testsha1 <message>`
| testsha256             | It lets you estimate the time taken to encode a specified string on milliseconds using SHA256 algorithm. | `testsha256 <message>`
| testsha384             | It lets you estimate the time taken to encode a specified string on milliseconds using SHA384 algorithm. | `testsha384 <message>`
| testsha512             | It lets you estimate the time taken to encode a specified string on milliseconds using SHA512 algorithm. | `testsha512 <message>`
| testcrc32              | It lets you estimate the time taken to encode a specified string on milliseconds using CRC32 algorithm. | `testcrc32 <message>`
| testregexp             | It lets you test the regular expression pattern on a specific string. It prints all matches. | `testregexp <pattern> <string>`
| colortest              | It lets you test the 255 color compatibility. | `colortest <index>`
| colortruetest          | It lets you test the 24-bit color compatibility, assuming that R, G, and B aren't less than 0 or greater than 255. | `colortruetest <R;G;B>`
| colorwheel             | Opens the colorwheel facility to test choosing the color based on the type of color whether it's a 255-color or a true color. | `colorwheel`
| sendnot                | It lets you test the notification system by sending the notification with the specified title and description on a specific priority. | `sendnot <Priority> <title> <desc>`
| sendnotprog            | It lets you test the notification system by sending the notification with the specified title and description on a specific priority with progress support to test the incrementation. It can be set to fail at a specific percentage (0-100). | `sendnot <Priority> <title> <desc> <failat>`
| dcalend                | It lets you render date using different calendar types (one of Gregorian, Hijri, Persian, Saudi-Hijri, Thai-Buddhist) | `dcalend <CalendType>`
| listcodepages          | It lets you list all the available codepages installed on the system. | `listcodepages`
| lscompilervars         | It lets you list all the compiler variables used to build Nitrocid KS. | `lscompilervars`
| testlistwriterst       | It lets you test the list writer using the String type. | `testlistwriterstr`
| testlistwriterint      | It lets you test the list writer using the Integer type. | `testlistwriterint`
| testlistwriterchar     | It lets you test the list writer using the Char type. | `testlistwriterchar`
| testdictwriterstr      | It lets you test the dictionary writer using the String type. | `testdictwriterstr`
| testdictwriterint      | It lets you test the dictionary writer using the Integer type. | `testdictwriterint`
| testdictwriterchar     | It lets you test the dictionary writer using the Char type. | `testdictwriterchar`
| lscultures             | It lets you test the available cultures installed on the system. | `lscultures [search]`
| getcustomsaversetting  | It lets you get a setting from a custom saver. Load all the mods and screensavers first before using this command. | `getcustomsaversetting <saver> <setting>`
| setcustomsaversetting  | It lets you set a setting from a custom saver. Load all the mods and screensavers first before using this command. | `setcustomsaversetting <saver> <setting> <value>`
| showtime               | Shows the current time | `showtime`
| showdate               | Shows the current date | `showdate`
| showtd                 | Shows the current time and date | `showtd`
| showtimeutc            | Shows the current time (UTC) | `showtimeutc`
| showdateutc            | Shows the current date (UTC) | `showdateutc`
| showtdutc              | Shows the current time and date (UTC) | `showtdutc`
| testtable              | Tests the table drawing | `testtable [margin]`
| checkstring            | Checks the specified string if it exists in the localization files found in the resources of KS (found in the language JSON file) | `checkstring <string>`
| checksettingsentryvars | Checks the settings entry variables to see if it can be accessible using the settings program. | `checksettingsentryvars`
| checklocallines        | Checks the localization files to see if the line numbers in them are all equal. | `checklocallines`
| checkstrings           | Checks the specified strings in a separate text file if they exist in the localization files found in the resources of KS (found in the language JSON file) | `checkstrings <stringsfile>`
| sleeptook              | Checks how many milliseconds (or ticks if started with the `-t` switch) did it really take to sleep. | `sleeptook [-t] <milliseconds>`
| getlinestyle           | Gets the line ending style from text file. | `getlinestyle <textfile>`
| printfiglet            | It lets you test the figlet print to print every text, using the font and colors that you need. | `printfiglet <Color> <FigletFont> <Message>`
| printfigletf           | It lets you test the figlet print to print every text, using the font and colors that you need. It has an additional feature of variables. | `printfigletf <Color> <FigletFont> <Variable1;Variable2;Variable3;...> <Message>`
| powerlinetest          | It lets you test the powerline glyphs
| testexecuteasm         | It lets you test the assembly execution by reflection
| help                   | It lists all the commands and its usages. | `help [command]`
| exit                   | It exits the test shell, and starts up the kernel. Exits normally if started up with `testshell`. | `exit`
| shutdown               | It shuts down the system. | `shutdown`
