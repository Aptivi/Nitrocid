## General Contributions

We currently have branches for each major series to separate between the master branch, which is where we're developing the major versions of kernel, and the servicing branch, which is where we're making minor versions of the kernel.

Branches that start with version series are branches that exist for specific major version series, like `v0.0.12.x-servicing` for 0.0.12.x servicing versions. All of the branches get their own continuous integration builds. However, consult the supported version table in the wiki homepage to see if the version you're going to contribute is still officially supported.

#### If you want to contribute to Nitrocid KS, the conditions below must be followed

1. If you want to modify/recode the core kernel, make sure that you write the changes correctly and carefully, making sure that you don't damage the core kernel features.
2. If you want to add/modify/remove/recode some desired features everywhere, write the changes carefully.
3. Like every other developer that wants to contribute to Nitrocid KS, the changes must be tested. We don't want the results, only test, and say that it is tested. If you want to provide the results, we still accept your pull request.
4. If you haven't tested it, and you sent us pull request, we will test the code and if we find that...

   - ...your code is working, we will accept your pull request
   - ...your code is not working, we will close your pull request

5. If you haven't tested it, and you are ready to send us, test the code first.
6. If you want to modify the documentation, or other parts other than coding, or add parts of/new documentation, feel free to. You can notify us.

#### Porting to programming languages that is other than Assembly (ASM, NASM, ...) or C/C++ language

You are free to port KS to any programming language, as long as it isn't ASM/C/C++.

The porting to ASM/C/C++ is up to us, the owner. Any ports to them before our official Kernel ASM/C/C++ bootable release will not be supported by us, and any issues regarding them will be immediately declined.

#### If you want to port Nitrocid KS to macOS (untested) on Visual Basic

1. Install Mono and Visual Studio for Mac, or Rider. You can find out how at their official website.
2. Download the KS source code
3. Open the solution using the above IDEs, and run the program.
4. Report any issues on Issues section in our KS repo.

#### If you want to port Nitrocid KS to Sun OS, BSDs, Solaris, etc. (untested)

This section is under construction. If you have instructions, please contribute.

Note: Porting Kernel inside the simulator to Android/iOS is OK. You can make appropriate modifications.

## Other contributions, issues, etc.

#### Reporting issues

Follow the bug reporting design pattern, and explain as much as you can. Less-specific explanation and wrong design pattern following might close your issue. Screenshots are optional.