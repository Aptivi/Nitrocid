# Contributing guidelines

First of all, thank you for contributing to our project! This kind of act is the most welcome act to help us keep running our projects the way we want them to run. All your contributions are valuable, but you need to follow these simple rules to get your contribution accepted.

In the pull requests, we might ask you to make a few changes until we can accept them. If there's no reason for us to add your changes to the project, we might reject them altogether.

## Following templates

Your pull requests should follow the template currently outlined here:

```
## Description
<!-- Describe about your pull request. Note that you need to be as more descriptive as you can so we can understand this request. -->


## Change type
<!-- Specify what kind of changes you made. -->
- [ ] Bug fixes
- [ ] Performance improvements
- [ ] Feature changes
- [ ] Behavioral changes
- [ ] Other (specify)

## Tested?
<!-- Have you tested your changes? -->
- [ ] Yes, I have
- [ ] No, I haven't
- [ ] Not sure

## Specify other changes
<!-- Specify the changes that wouldn't fit into the selection. -->

```

You should be descriptive about what your change is trying to do and what's your opinion about how it affects the whole project. Moreover, it's vital to test your changes before trying to start a pull request to ensure that there are no problems in your initial version. **Always** draft your pull requests.

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

Extended Summary

---

Description

---

Type: Type
Breaking: Yes/No
Documentation Required: Yes/No
Part: 1/1
```

For types, you should select exactly one type from the following types:

```
add: for additions
fix: for fixes
rem: for removals
imp: for improvements
ref: for refactors
upd: for library updates
doc: for documentation updates
```

Additionally, attributes are optional and can be specified. Multiple attributes should be separated with the pipe character (`|`). However, there are special cases that you may need to handle when you're committing your changes to your pull request:

* If documentation is required (i.e. your commit requires documentation on GitBook and you've specified `doc`), change the `Documentation Required` part to `Yes`, otherwise, `No`.
* If this commit includes breaking changes (i.e. you've specified `brk`), change the `Breaking` part to `Yes`, otherwise, `No`.
* If this commit is a part of the commit series, specify `prt` and change the `Part` field where it says `1/1` to the current and the total parts in this format: `current/total`. Total parts must be accurate, and the title should stay the same as any former commits in the series.

These are the attributes we officially support:

```
brk: for breaking changes
sec: for security
prf: for performance improvements
reg: for regression fixes
doc: for documentation requirement
ptp: for prototyping
prt: for commit series (PartNum is required)
```

## Engagement with the Community

Thank you for your contribution to our project, but in order for this contribution to be flawless, you must be respectful to all other developers of the projects and the users in general, regardless of whether there is a fight or a heated discussion going on. Try to keep it civil during fights and don't use personal attacks, threats of any kind, derogatory and racist remarks against people or groups of any race, ethnicity, religion, or group, and explicit words (like swearing) to try to solve any disagreement with anyone, including the developers of the project.
