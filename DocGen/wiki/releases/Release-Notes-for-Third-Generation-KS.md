# Third-generation KS releases

We have outlined in our old product documentation that `0.1.x` is real beta, so we'll promote it to this version.

> [!WARNING]
> Please note that the API that the third-generation KS has will not be backwards-compatible with the second-generation and the first-generation APIs, so we urge all mod and screensaver developers to update their mod to fully support third-generation and optionally provide the second-generation and the first-generation version, following the [Compatibility Notes for Third Generation KS](Compatibility-Notes-for-Third-Generation-KS.md). Meanwhile, we plan to keep supporting the first generation KS until 2024 and the second generation until 2027.

## KS 0.1.x.y series

### KS 0.1.0 (WIP)

> [!NOTE]
> This version is WIP as of 8/10/2022.

1. Updated all libraries
2. Added new commands
3. Added new screensavers
4. Added new splashes
5. Added new themes
6. Added support for transparent terminals
7. Removed command injection as it isn't secure
8. Removed LOLCAT as it has become inconsistent
9. Removed references to `Microsoft.VisualBasic` completely
10. Removed checks for multi-instance
11. Removed frequency and time from `beep` command as it isn't cross-platform
12. Console printing errors shouldn't cause kernel error anymore as `KernelError` itself calls these printing function
13. General API improvements
14. General improvements and bug fixes
15. And more surprises...
