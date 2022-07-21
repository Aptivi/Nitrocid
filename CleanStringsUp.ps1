# Remove this script in API Milestone 1.
echo "We advise not to use this script to clean unused strings; this script is very slow. Use KSCleanUnusedLocales instead."

$PSDefaultParameterValues['*:Encoding'] = "default"

Get-Content KSJsonifyLocales\Translations\eng.txt | ForEach-Object { 
    $Sel = Get-ChildItem -Path "Kernel Simulator" -Include *.vb -Recurse;
    $SelJson = Get-ChildItem -Path "Kernel Simulator/Resources/Data" -Include *Entries.json -Recurse;
    $Sel2 = Select-String -Path $Sel -Pattern ("DoTranslation(""" + ($_ -replace """", """""") + """") -SimpleMatch;
    $Sel3 = Select-String -Path $SelJson -Pattern ("                ""Description"": """ + ($_ -replace """", "\""") + """") -SimpleMatch;
    $Sel4 = Select-String -Path $SelJson -Pattern ("                ""Name"": """ + ($_ -replace """", "\""") + """,") -SimpleMatch;
    if ($Sel2 -ne $null) {
      echo $_
    } else {
      $Sel2 = Select-String -Path $Sel -Pattern ("Shell, """ + $_ + """, ") -SimpleMatch;
      if ($Sel2 -ne $null) { 
        echo $_
      } else {
        $Sel2 = Select-String -Path $Sel -Pattern ("Args, """ + $_ + """, ") -SimpleMatch;
        if ($Sel2 -ne $null) { 
          echo $_
        }
      }
    }
    if ($Sel3 -ne $null) {
      echo $_
    }
    if ($Sel4 -ne $null) {
      echo $_
    }
} > CleanEnglish.txt

$LineIndex = 1
Get-Content KSJsonifyLocales\Translations\eng.txt | ForEach-Object {
    $Sel = Get-ChildItem -Path "Kernel Simulator" -Include *.vb -Recurse;
    $SelJson = Get-ChildItem -Path "Kernel Simulator/Resources/Data" -Include *Entries.json -Recurse;
    $Sel2 = Select-String -Path $Sel -Pattern ("DoTranslation(""" + ($_ -replace """", """""") + """") -SimpleMatch;
    $Sel3 = Select-String -Path $Sel -Pattern ("Shell, """ + $_ + """, ") -SimpleMatch;
    $Sel4 = Select-String -Path $SelJson -Pattern ("                ""Description"": """ + ($_ -replace """", "\""") + """") -SimpleMatch;
    $Sel5 = Select-String -Path $SelJson -Pattern ("                ""Name"": """ + ($_ -replace """", "\""") + """,") -SimpleMatch;
    $Sel6 = Select-String -Path $Sel -Pattern ("Args, """ + $_ + """, ") -SimpleMatch;
    if($Sel2 -eq $null -and $Sel3 -eq $null -and $Sel4 -eq $null -and $Sel5 -eq $null -and $Sel6 -eq $null) {
      $LineIndex
    }
    $LineIndex += 1
} > DirtyIndexes.txt

$Indexes = -split $(Get-Content DirtyIndexes.txt)
Get-Content "KSJsonifyLocales/Translations/eng.txt" | Where-Object ReadCount -notin $Indexes | Out-File -encoding default "KSJsonifyLocales/Translations/f_eng.txt"
$(Get-ChildItem -Path "KSJsonifyLocales/Translations" -Exclude "*eng.txt" -Recurse).Name | ForEach-Object {
    Get-Content "KSJsonifyLocales/Translations/$_" | Where-Object ReadCount -notin $Indexes | Out-File -encoding default "KSJsonifyLocales/Translations/f_$_"
}
$(Get-ChildItem -Path "KSJsonifyLocales/Translations" -Exclude "f_*.txt" -Recurse).Name | ForEach-Object {
    Move-Item -Force -Path "KSJsonifyLocales/Translations/f_$_" "KSJsonifyLocales/Translations/$_"
}

Remove-Item DirtyIndexes.txt
Remove-Item CleanEnglish.txt