$rootDir = $args[0]
$oldApiVersion = $args[1]
$newApiVersion = $args[2]

$oldApiVersionSplit = $oldApiVersion.Split(".")
$newApiVersionSplit = $newApiVersion.Split(".")

# Modify the NitrocidModAPIVersionMajor and the NitrocidModAPIVersionChangeset properties
$oldApiMajor = "<NitrocidModAPIVersionMajor>$($oldApiVersionSplit[0]).$($oldApiVersionSplit[1]).$($oldApiVersionSplit[2])"
$newApiMajor = "<NitrocidModAPIVersionMajor>$($newApiVersionSplit[0]).$($newApiVersionSplit[1]).$($newApiVersionSplit[2])"
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
