$ErrorActionPreference = 'Stop';
$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  zipFileName   = "0.0.15.3-bin.rar"
}
$userProfile = $Env:USERPROFILE

Write-Output "<*>: for assumptions, <+> for progress, <-> for error"
Write-Output "<*> Package name: $packageName"
Write-Output "<*> ZIP Name: $zipFileName"

$RemoveConfig =  New-Object System.Management.Automation.Host.ChoiceDescription "&Yes", "Removes config files, including mods, aliases, and more..."
$KeepConfig =    New-Object System.Management.Automation.Host.ChoiceDescription "&No",  "Keeps config files, including mods, aliases, and more..."
$ConfigChoices = [System.Management.Automation.Host.ChoiceDescription[]]($RemoveConfig, $KeepConfig)
$ConfigChoice =  $host.ui.PromptForChoice("<+> Kernel Configuration", "<+> Are you sure that you want to remove configuration files?", $ConfigChoices, 0)

if ($ConfigChoice = 0) {
    Write-Output "<+> Removing config files..."
    Write-Output "<*> Config files are located in $userProfile. What is removed below:"
    Write-Output "<*>  |"
    try{
      if (Test-Path $userProfile\aliases.csv) {
        Write-Output "<+>  |-> Aliases.........(aliases.csv)"
        Remove-Item $userProfile\aliases.csv
      }
      if (Test-Path $userProfile\blocked_devices.csv) {
        Write-Output "<+>  |-> Blocked devices.(blocked_devices.csv)"
        Remove-Item $userProfile\MAL.txt
      }
      if (Test-Path $userProfile\kernelConfig.ini) {
        Write-Output "<+>  |-> Configuration...(kernelConfig.ini)"
        Remove-Item $userProfile\kernelConfig.ini
      }
      if (Test-Path $userProfile\kernelDbg.log) {
        Write-Output "<+>  |-> Debugging logs..(kernelDbg.log)"
        Remove-Item $userProfile\kernelDbg.log
      }
      if (Test-Path $userProfile\MOTD.txt) {
        Write-Output "<+>  |-> MOTD Text.......(MOTD.txt)"
        Remove-Item $userProfile\MOTD.txt
      }
      if (Test-Path $userProfile\MAL.txt) {
        Write-Output "<+>  |-> MAL Text........(MAL.txt)"
        Remove-Item $userProfile\MAL.txt
      }
      if (Test-Path $userProfile\users.csv) {
        Write-Output "<+>  |-> Users...........(users.csv)"
        Remove-Item $userProfile\MAL.txt
      }
      Write-Output "<*> Mods are in $userProfile\KSMods"
      if (Test-Path -Path $userProfile\KSMods) {
        Remove-Item -Path $userProfile\KSMods -Recurse
      }
    }
    catch{"<-> Error removing config. Uninstalling anyways... (Preserving some config files)"}
}
Write-Output "<+> Uninstalling..."
Uninstall-ChocolateyZipPackage @packageArgs