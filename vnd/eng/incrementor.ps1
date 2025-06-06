$targetFile = $args[0]
$oldVersion = $args[1]
$newVersion = $args[2]
$oldApiVersion = $args[3]
$newApiVersion = $args[4]

$contents = [System.IO.File]::ReadAllText($targetFile).Replace($oldVersion, $newVersion).Replace($oldApiVersion, $newApiVersion)
[System.IO.File]::WriteAllText($targetFile, $contents)
