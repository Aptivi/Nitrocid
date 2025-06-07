$rootDir = $args[0]
$oldVersion = $args[1]
$newVersion = $args[2]
$oldApiVersion = $args[3]
$newApiVersion = $args[4]

# Modify the NitrocidModAPIVersionMajor and the NitrocidModAPIVersionChangeset properties
$oldApiVersionSplit = $oldApiVersion.Split(".")
$newApiVersionSplit = $newApiVersion.Split(".")
$oldApiVerNoPatch = "$($oldApiVersionSplit[0]).$($oldApiVersionSplit[1]).$($oldApiVersionSplit[2])"
$newApiVerNoPatch = "$($newApiVersionSplit[0]).$($newApiVersionSplit[1]).$($newApiVersionSplit[2])"
$oldApiMajor = "<NitrocidModAPIVersionMajor>$oldApiVerNoPatch"
$newApiMajor = "<NitrocidModAPIVersionMajor>$newApiVerNoPatch"
$oldApiChangeset = "<NitrocidModAPIVersionChangeset>$($oldApiVersionSplit[3])"
$newApiChangeset = "<NitrocidModAPIVersionChangeset>$($newApiVersionSplit[3])"
$directoryBuild = "$rootDir\Directory.Build.props"
$propsContent = [System.IO.File]::ReadAllText($directoryBuild).Replace($oldApiMajor, $newApiMajor).Replace($oldApiChangeset, $newApiChangeset)
[System.IO.File]::WriteAllText($directoryBuild, $propsContent)

# Modify the template mod files, too
$oldApiMajorTmpl = "$($oldApiVersionSplit[0]), $($oldApiVersionSplit[1]), $($oldApiVersionSplit[2]), $($oldApiVersionSplit[3])"
$newApiMajorTmpl = "$($newApiVersionSplit[0]), $($newApiVersionSplit[1]), $($newApiVersionSplit[2]), $($newApiVersionSplit[3])"
$modClassCs = "$rootDir\public\Nitrocid.Templates\templates\KSMod\ModClass.cs"
$modClassVb = "$rootDir\public\Nitrocid.Templates\templates\KSModVB\ModClass.vb"
$modClassCsContent = [System.IO.File]::ReadAllText($modClassCs).Replace($oldApiMajorTmpl, $newApiMajorTmpl)
$modClassVbContent = [System.IO.File]::ReadAllText($modClassVb).Replace($oldApiMajorTmpl, $newApiMajorTmpl)
[System.IO.File]::WriteAllText($modClassCs, $modClassCsContent)
[System.IO.File]::WriteAllText($modClassVb, $modClassVbContent)

# Modify the changes.chg file
$oldChanges = "version $oldVersion (mod API $oldApiVersion)"
$newChanges = "version $newVersion (mod API $newApiVersion)"
$changesFile = "$rootDir\vnd\changes.chg"
$changesContent = [System.IO.File]::ReadAllText($changesFile).Replace($oldChanges, $newChanges)
[System.IO.File]::WriteAllText($changesFile, $changesContent)

# Modify the Package.wxs file
$oldVersionSplit = $oldVersion.Split(".")
$newVersionSplit = $newVersion.Split(".")
$oldVerNoPatch = "$($oldVersionSplit[0]).$($oldVersionSplit[1]).$($oldVersionSplit[2])"
$newVerNoPatch = "$($newVersionSplit[0]).$($newVersionSplit[1]).$($newVersionSplit[2])"
$oldPackageWxs = "Name=`"Nitrocid $oldVerNoPatch`""
$newPackageWxs = "Name=`"Nitrocid $newVerNoPatch`""
$packageWxsFile = "$rootDir\public\Nitrocid.Installers\Nitrocid.Installer\Package.wxs"
$packageWxsContent = [System.IO.File]::ReadAllText($packageWxsFile).Replace($oldPackageWxs, $newPackageWxs)
[System.IO.File]::WriteAllText($packageWxsFile, $packageWxsContent)

# Modify the PKGBUILD VCS files
$pkgBuildVcsFile = "$rootDir\PKGBUILD-VCS"
$pkgBuildVcsLiteFile = "$rootDir\PKGBUILD-VCS-LITE"
$oldPkgNameVcs = "pkgname=nitrocid-$($oldApiVersionSplit[2])"
$newPkgNameVcs = "pkgname=nitrocid-$($newApiVersionSplit[2])"
$oldPkgVerVcs = "pkgver=v$oldVerNoPatch"
$newPkgVerVcs = "pkgver=v$newVerNoPatch"
$oldBranchVcs = "branch=v$oldVerNoPatch"
$newBranchVcs = "branch=v$newVerNoPatch"
$pkgBuildVcsContent = [System.IO.File]::ReadAllText($pkgBuildVcsFile).Replace($oldPkgNameVcs, $newPkgNameVcs).Replace($oldPkgVerVcs, $newPkgVerVcs).Replace($oldBranchVcs, $newBranchVcs)
$pkgBuildVcsLiteContent = [System.IO.File]::ReadAllText($pkgBuildVcsLiteFile).Replace($oldPkgNameVcs, $newPkgNameVcs).Replace($oldPkgVerVcs, $newPkgVerVcs).Replace($oldBranchVcs, $newBranchVcs)
[System.IO.File]::WriteAllText($pkgBuildVcsFile, $pkgBuildVcsContent)
[System.IO.File]::WriteAllText($pkgBuildVcsLiteFile, $pkgBuildVcsLiteContent)
