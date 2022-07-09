# Mod Dependencies

## What is a dependency for a mod?

A depenedency for a mod is a library that allows mods to use features that the library provides. Some mods may make use of such libraries to be able to do a thing that the built-in libraries aren't able to.

### Adding references

In your mod or screensaver project, follow these steps:

1. Open your mod or screensaver project
2. Right-click on References in the Solution Explorer, and press `Manage NuGet packages...`
3. Find your desired library and install it
4. Use these libraries in your code, and when you're finished, build the project. Right-click on the project or solution and select `Build`
5. Move the mod or screensaver file to `KSMods`
6. Create a directory called `Deps`
7. Inside it, create another folder called `<ModFileName>-<ModFileVersion>`. Make sure to use the mod or screensaver file version, not the assembly version.
8. Move all the dependencies from the project output directory to that folder.

If you followed all the steps correctly, you should see this structure:

```
KSMods/
  |
  +-> mod.dll
  +-> Deps/
        |
        +-> mod-1.0.0.0/
              |
              +-> Dep1.dll
              +-> Dep2.dll
```

All the commands or routines in your mod or screensaver should work correctly.

### Does it apply to splashes?

Yes! You can do this in your custom splashes! Follow the above steps for the splashes, but make sure that you maintain this structure:

```
KSSplashes/
  |
  +-> splash.dll
  +-> Deps/
        |
        +-> splash-1.0.0.0/
              |
              +-> Dep1.dll
              +-> Dep2.dll
```
