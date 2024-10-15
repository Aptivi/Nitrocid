# Contribution guidelines

First of all, thank you for contributing to our project! This kind of act is the most welcome act to help us keep running our projects the way we want them to run. All your contributions are valuable, but you need to follow these simple rules to get your contribution accepted.

In the pull requests, we might ask you to make a few changes until we can accept them. If there's no reason for us to add your changes to the project, we might reject them altogether.

## Following templates

Your pull requests should follow the template.

You should be descriptive about what your change is trying to do and what's your opinion about how it affects the whole project. Moreover, it's vital to test your changes before trying to start a pull request to ensure that there are no problems in your initial version. **Always** draft your pull requests.

## Windows compatibility

*This doesn't apply to projects that don't use C# or aren't exclusive to Windows.*

When contributing changes to any part of the code, especially when creating files, your pull requests should follow the below requirements:

  - File names should not contain forbidden characters, such as `/ \ : * ? " < > |`, and control characters, such as a new line, as files with such names don't sit well with Windows.
  - File names should always be treated as case sensitive, even if *nix systems allow you to create files with same name but different casing, as files that fall into this category don't sit well with Windows.
  - When creating shell scripts in Windows, you should give it executable permissions with `git update-index --chmod=+x <SHELLSCRIPT>` prior to committing, as Windows in general has no concept of Unix permissions.
  - In general, make sure that any of your changes don't cause incompatibilities with Windows in any way in both build-time and run-time.

## Code guidelines

When it comes to coding your changes, they should follow the below coding guidelines to retain the style of our projects' code. These are the below code guidelines:

### Sorting of Fields and Properties

Fields must come at the very beginning of each class before the properties. Between the fields and the properties and the functions, there should be an empty line. Moreover, accesibility modifiers should be sorted in the following order:

```
ClassName
  |
  |-> Public Fields
  |-> Public Read-Only Fields
  |-> Internal Fields
  |-> Private Fields
  |-> Internal Read-Only Fields
  |-> Private Read-Only Fields
  |
  |-> Public Properties
  |-> Internal Properties (rare, unless you treat them like C #macros)
  |
(...)
```

### Sorting of Functions

Sorting of functions is only affected by their accessibility modifiers in your code. Public functions must come before internal functions, at which the private functions come last.

```
(...)
  |
  |-> Public Functions
  |-> Internal Functions
  |-> Private Functions
  |
 ---
```

### Arrangement of logic in functions

Inside functions, the arrangement of logic must be in the below order. Moreover, each part of the logic in each function must be preceded by a comment that explains why is your logic is here and a couple of necessary variables before actual logic.

```
ClassName
(...)
  |
  |-> FunctionName(int arg1, string arg2, ...)
  |     |
  |     |-> Comment explaining why (not what) is this logic here
  |     |-> A couple of necessary variables (optional)
  |     |-> Actual function logic
  |     |
  |   (...)
  |
(...)
```

Example:

```
private static void PollForResize()
{
(...)
    // We need to call the WindowHeight and WindowWidth properties on the Terminal console driver, because
    // this polling works for all the terminals. Other drivers that don't use the terminal may not even
    // implement these two properties.
    if (CurrentWindowHeight != termDriver.WindowHeight | CurrentWindowWidth != termDriver.WindowWidth)
    {
        ResizeDetected = true;
(...)
}
```

### Tabs versus Spaces

Here we come to the argument of tabs vs spaces. Our problem with tabs is that there are systems that treat tabs as four spaces, and there are systems that treat tabs as eight spaces. Moreover, there is no universal way to accurately query the operating system for tab lengths, as such queries are up to the application handling tabs.

We recommend that you set your IDE to use **four spaces** for each tab press. Also, don't use literal tab characters for indentation (`\t`); use four spaces.

### Functions that do only one thing

*This is not applicable to languages that don't support this feature*

In C#, you can literally make a function without the opening and closing bracelets (`{ }`) if your function only contains one logic. However, you must append `=>` before the logic. Moreover, the logic should be in its own separate line with four spaces as indentation, like the following:

```
access_modifier [static] type SingleLogicFunctionName(string arg1, ...) =>
    MyLogic(arg1, ...).Modify().(...);
```

For example,

```
public static string[] GetWrappedSentences(string text, int maximumLength) =>
    GetWrappedSentences(text, maximumLength, 0);
```

### If, while, for, foreach statements that do only one thing

*This is not applicable to languages that don't support this feature*

In C#, you can literally make an if, while, for, and foreach statements without the opening and closing bracelets (`{ }`) if your statement only contains one logic. However, you must append `=>` before the logic. Moreover, the logic should be in its own separate line with four spaces as indentation, like the following:

```
if/while/for/foreach (...)
    MyLogicToDo();
```

For example,

```
// Also, compensate the \0 characters
if (text[i] == '\0')
    vtSeqCompensate++;
```

### Naming of Public vs Internal and Private Components

The naming must satisfy the following rules:

* All public functions, properties, and fields must follow the Pascal Case (FunctionName) naming scheme, regardless of their purpose.
* For argument names in public, internal, and private functions, they must be named in the scheme of Camel Case (argumentName).
* Private and internal functions and properties must also follow the Pascal Case naming scheme.
* Private and internal fields must use the Camel Case naming style.
* Pascal Case should be used in class names at all times.
* You may never use the snake_case naming (this isn't Rust) or the kebab-case naming scheme (this isn't HTML) in all the components.

To learn more about Pascal Case and Camel Case, visit [this site](https://www.freecodecamp.org/news/snake-case-vs-camel-case-vs-pascal-case-vs-kebab-case-whats-the-difference/).

## Git commits

We follow this conventional Git commit scheme:

```
Type - Attributes - Summary

---

Description

---

Type: Type
Breaking: Yes/No
Doc Required: Yes/No
Backport Required: Yes/No
Part: 1/1
```

For types, you should select exactly one type from the following types:

* `add`: for additions
* `fix`: for fixes
* `rem`: for removals
* `imp`: for improvements
* `ref`: for refactors
* `upd`: for library updates
* `doc`: for documentation updates
* `dev`: for development checkpoints (version bump, etc.)
* `fin`: for development finish points
* `chg`: for minor changes to previous commits
* `und`: for other changes that don't apply

Additionally, attributes are optional and can be specified. Multiple attributes should be separated with the pipe character (`|`). However, there are special cases that you may need to handle when you're committing your changes to your pull request:

* If documentation is required (i.e. your commit requires documentation on GitBook and you've specified `doc`), change the `Doc Required` part to `Yes`, otherwise, `No`.
* If backport is required (i.e. your commit introduces a fix that needs to be backported to previous version series that don't have a fix), change the `Backport Required` part to `Yes`, otherwise, `No`.
* If this commit includes breaking changes (i.e. you've specified `brk`), change the `Breaking` part to `Yes`, otherwise, `No`.
* If this commit is a part of the commit series, specify `prt` and change the `Part` field where it says `1/1` to the current and the total parts in this format: `current/total`. Total parts must be accurate, the title should stay the same as any former commits in the series, and the current part must not be larger than the total part number.

These are the attributes we officially support:

* `brk`: for breaking changes
* `sec`: for security
* `prf`: for performance improvements
* `reg`: for regression fixes
* `doc`: for documentation requirement
* `ptp`: for prototyping
* `prt`: for commit series (PartNum is required)
* `bkp`: for backports

Take note that if you've set a commit that is a backport, make sure that the type is not one of the following types:

* Incompatible types with `bkp`
  * `rem`: for removals
  * `dev`: for development checkpoints (version bump, etc.)
  * `fin`: for development finish points
* Incompatible attributes with `bkp`
  * `ptp`: for prototyping
  * `brk`: for breaking changes

## Releases

When planning for a new version, always start the development of the next version by changing the version found in all the relevant files (usually `Directory.Build.props` for C# projects) before starting to push commits that add new features and everything else. When development finishes, before the release tag can be pushed, the `CHANGES` file should be changed to reflect the new release, as long as it follows this convention:

```
Long description of the release

### Changes

This release contains a variety of changes, including, but not limited to:

- `[+]` Added X
- `[*]` Improved Y
- `[-]` Removed Z

Review the commit history if you want to get a deep insight about the changes.

### Feedback?

If you have issues with this version, report to us by [making a new issue ticket](https://github.com/Aptivi/PROJECT/issues/new).

### Sum hashes

To verify integrity of your download, compare the SHA256 sum of your downloaded file with the following information:


```

Two new lines are applied intentionally so that the hash list gets rendered in a way that you'd expect, because the hash sum list gets populated automatically. Also, the `CHANGES.TITLE` file should be changed to match the version whose development finished but tag not pushed, as long as it follows this format:

```
[servicing] PROJECT v1.0.0: Release Name
```

The type at the beginning is necessary as it can tell us and the users in what stage this release belongs to. Currently, this list of releases should be used:

  * `alpha`: Indicates that this release is an alpha version
  * `beta`: Indicates that this release is a beta version
  * `release`: Indicates that this release is a major release (i.e. changes the major part and/or the minor part)
  * `servicing`: Indicates that this release is a minor release (i.e. changes the build part and/or the patch part)

## Assistance of AI

Although artifical intelligence (AI) is a next-gen technology that every company is leaning to, which we are proud of, but when contributing code or other things to this project, we rely on human work to ensure maximum quality. This means that you are not allowed to use any kind of AI assistance to generate code and non-code contributions, such as ChatGPT and others, in fear of licensing issues, potential security issues, and current hallucination issues. Any usage of such tools when contributing will be immediately rejected.

## Engagement with the Community

Thank you for your contribution to our project, but in order for this contribution to be flawless, you must be respectful to all other developers of the projects and the users in general, regardless of whether there is a fight or a heated discussion going on. Try to keep it civil during fights and don't use personal attacks, threats of any kind, derogatory and racist remarks against people or groups of any race, ethnicity, religion, or group, and explicit words (like swearing) to try to solve any disagreement with anyone, including the developers of the project.
