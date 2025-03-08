@echo off

set ROOTDIR=%~dp0..

for /f "tokens=*" %%g in ('findstr "<Version>" %ROOTDIR%\Directory.Build.props') do (set MIDVER=%%g)
for /f "tokens=1 delims=<" %%a in ("%MIDVER:~9%") do (set version=%%a)

:packbin
echo Packing binary...
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%version%-bin.zip "%ROOTDIR%\public\Nitrocid\KSBuild\net8.0\*"
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%version%-bin-lite.zip "%ROOTDIR%\public\Nitrocid\KSBuild\net8.0\*" -xr!Addons
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%version%-addons.zip "%ROOTDIR%\public\Nitrocid\KSBuild\net8.0\Addons\*"
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%version%-analyzers.zip "%ROOTDIR%\public\Nitrocid\KSAnalyzer\netstandard2.0\*"
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%version%-mod-analyzer.zip "%ROOTDIR%\public\Nitrocid\KSAnalyzer\net8.0\*"
if %errorlevel% == 0 goto :complete
echo There was an error trying to pack binary (%errorlevel%).
goto :finished

:complete
move %temp%\%version%-bin.zip %ROOTDIR%\tools\
move %temp%\%version%-bin-lite.zip %ROOTDIR%\tools\
move %temp%\%version%-addons.zip %ROOTDIR%\tools\
move %temp%\%version%-analyzers.zip %ROOTDIR%\tools\
move %temp%\%version%-mod-analyzer.zip %ROOTDIR%\tools\
copy %ROOTDIR%\tools\changes.chg %ROOTDIR%\tools\%version%-changes.chg

echo Pack successful.
:finished
