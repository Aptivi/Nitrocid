$targetFile = $args[0]
$newVersion = $args[1]
$newApiVersion = $args[2]

$date = [System.DateTime]::Now.ToString("ddd, dd MMM yyyy")
$time = [System.DateTime]::Now.ToString("HH:mm:ss")
$offset = [System.TimeZoneInfo]::Local.GetUtcOffset([System.DateTime]::Now)
$tzoff = if ($offset -lt [TimeSpan]::Zero) {"\-"} else {"\+"}
$tz = $offset.ToString("${tzoff}hhmm")

$contents = $(Write-Output "nitrocid-25 (${newApiVersion}-${newVersion}-1) noble; urgency=medium`n")
$contents += $(Write-Output "`n")
$contents += $(Write-Output "  * General improvements and bug fixes`n")
$contents += $(Write-Output "`n")
$contents += $(Write-Output " -- Aptivi CEO <ceo@aptivi.anonaddy.com>  $date $time $tz`n`n")

$contents += [System.IO.File]::ReadAllText($targetFile)

[System.IO.File]::WriteAllText($targetFile, $contents)
