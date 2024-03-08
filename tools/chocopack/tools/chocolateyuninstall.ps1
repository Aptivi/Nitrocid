$ErrorActionPreference = 'Stop';
$packageArgs = @{
  packageName   = $env:ChocolateyPackageName
  zipFileName   = "0.1.0.1-bin.zip"
}
$userProfile = $Env:USERPROFILE

Write-Output "<*>: for assumptions, <+> for progress, <-> for error"
Write-Output "<*> Package name: $packageName"
Write-Output "<*> ZIP Name: $zipFileName"

Write-Output "<+> Uninstalling..."
Uninstall-ChocolateyZipPackage @packageArgs
