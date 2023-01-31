# Command line arguments for KS

## How do I run command-line arguments?

You can run these arguments to make the simulator behave differently, by running Nitrocid KS like below examples:
```cmd
ks.cmd debug
ks.cmd args
```
```sh
ks debug
ks args
```

> [!NOTE]
> These command line arguments are handled by the kernel after the pre-boot stage has been done. For pre-boot command line arguments that get executed before the kernel really starts, take a look at the [pre-boot arguments](Preboot-Command-line-arguments-for-KS.md).

### Useful arguments

| Argument        | Description
|:----------------|:------------
| testInteractive | Opens the test interactive shell.
| debug           | Alternative way to turn on kernel debugging.
| args            | Makes the kernel prompt the user to write arguments on boot.
| help            | Opens help page for command-line arguments
