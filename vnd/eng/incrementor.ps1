$targetFile = $args[0]
$oldVersion = $args[1]
$newVersion = $args[2]
$oldApiVersion = $args[3]
$newApiVersion = $args[4]

$oldApiVersionSplit = $oldApiVersion.Split(".")
$newApiVersionSplit = $newApiVersion.Split(".")

$contents = [System.IO.File]::ReadAllText($targetFile).Replace($oldVersion, $newVersion).Replace($oldApiVersion, $newApiVersion).Replace("nitrocid-$($oldApiVersionSplit[2])", "nitrocid-$($newApiVersionSplit[2])")
[System.IO.File]::WriteAllText($targetFile, $contents)
