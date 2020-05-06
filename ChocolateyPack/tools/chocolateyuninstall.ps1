$ErrorActionPreference = 'Stop';
$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  zipFileName   = "0.0.9.1-bin.rar"
}
$userProfile = $Env:USERPROFILE

Write-Output "<*>: for assumptions, <+> for progress, <-> for error"
Write-Output "<*> Package name: $packageName"
Write-Output "<*> ZIP Name: $zipFileName"
Write-Output "<+> Removing config files..."
Write-Output "<*> Config files are located in $userProfile. What is removed below:"
Write-Output "<*>  |"
try{
  if (Test-Path $userProfile\aliases.csv) {
    Write-Output "<+>  |-> Aliases........(aliases.csv)"
    Remove-Item $userProfile\aliases.csv
  }
  if (Test-Path $userProfile\kernelConfig.ini) {
    Write-Output "<+>  |-> Configuration..(kernelConfig.ini)"
    Remove-Item $userProfile\kernelConfig.ini
  }
  if (Test-Path $userProfile\kernelDbg.log) {
    Write-Output "<+>  |-> Debugging logs.(kernelDbg.log)"
    Remove-Item $userProfile\kernelDbg.log
  }
  if (Test-Path $userProfile\MOTD.txt) {
    Write-Output "<+>  |-> MOTD Text......(MOTD.txt)"
    Remove-Item $userProfile\MOTD.txt
  }
  if (Test-Path $userProfile\MAL.txt) {
    Write-Output "<+>  |-> MAL Text.......(MAL.txt)"
    Remove-Item $userProfile\MAL.txt
  }
  Write-Output "<*> Mods are in $userProfile\KSMods"
  if (Test-Path -Path $userProfile\KSMods) {
    Remove-Item -Path $userProfile\KSMods -Recurse
  }
}
catch{"<-> Error removing config. Uninstalling anyways... (Preserving some config files)"}
Uninstall-ChocolateyZipPackage @packageArgs