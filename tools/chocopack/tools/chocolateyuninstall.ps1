$ErrorActionPreference = 'Stop';
$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  zipFileName   = "0.1.0-bin.zip"
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
      if (Test-Path $userProfile\Aliases.json) {
        Write-Output "<+>  |-> Aliases.........(Aliases.json)"
        Remove-Item $userProfile\Aliases.json
      }
      if (Test-Path $userProfile\DebugDeviceNames.json) {
        Write-Output "<+>  |-> Debug devices...(DebugDeviceNames.json)"
        Remove-Item $userProfile\DebugDeviceNames.json
      }
      if (Test-Path $userProfile\KernelConfig.json) {
        Write-Output "<+>  |-> Configuration...(KernelConfig.json)"
        Remove-Item $userProfile\KernelConfig.json
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
      if (Test-Path $userProfile\Users.json) {
        Write-Output "<+>  |-> Users...........(Users.json)"
        Remove-Item $userProfile\Users.json
      }
      if (Test-Path $userProfile\FTP_SpeedDial.json) {
        Write-Output "<+>  |-> FTP speed dial..(FTP_SpeedDial.json)"
        Remove-Item $userProfile\FTP_SpeedDial.json
      }
      if (Test-Path $userProfile\SFTP_SpeedDial.json) {
        Write-Output "<+>  |-> SFTP speed dial.(SFTP_SpeedDial.json)"
        Remove-Item $userProfile\SFTP_SpeedDial.json
      }
      if (Test-Path $userProfile\CustomSaverSettings.json) {
        Write-Output "<+>  |-> SFTP speed dial.(CustomSaverSettings.json)"
        Remove-Item $userProfile\CustomSaverSettings.json
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
