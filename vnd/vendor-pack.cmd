@echo off

set ROOTDIR=%~dp0\..

for /f "tokens=*" %%g in ('findstr "<Version>" "%ROOTDIR%\Directory.Build.props"') do (set MIDVER=%%g)
for /f "tokens=1 delims=<" %%a in ("%MIDVER:~9%") do (set version=%%a)
set releaseconfig=%1
if "%releaseconfig%" == "" set releaseconfig=Release

:packbin
echo Packing binary...
"%ProgramFiles%\7-Zip\7z.exe" a -tzip "%temp%/%version%-bin.zip" "%ROOTDIR%\public\Nitrocid\KSBuild\net8.0\*"
"%ProgramFiles%\7-Zip\7z.exe" a -tzip "%temp%/%version%-bin-lite.zip" "%ROOTDIR%\public\Nitrocid\KSBuild\net8.0\*" -xr^^!Addons -xr^^!Addons.Essentials
"%ProgramFiles%\7-Zip\7z.exe" a -tzip "%temp%/%version%-addons.zip" "%ROOTDIR%\public\Nitrocid\KSBuild\net8.0\Addons*"
"%ProgramFiles%\7-Zip\7z.exe" a -tzip "%temp%/%version%-analyzers.zip" "%ROOTDIR%\public\Nitrocid\KSAnalyzer\netstandard2.0\*"
"%ProgramFiles%\7-Zip\7z.exe" a -tzip "%temp%/%version%-mod-analyzer.zip" "%ROOTDIR%\public\Nitrocid\KSAnalyzer\net8.0\*"
if %errorlevel% == 0 goto :complete
echo There was an error trying to pack binary (%errorlevel%).
goto :finished

:complete
move "%temp%\%version%-bin.zip" "%ROOTDIR%\vnd\"
move "%temp%\%version%-bin-lite.zip" "%ROOTDIR%\vnd\"
move "%temp%\%version%-addons.zip" "%ROOTDIR%\vnd\"
move "%temp%\%version%-analyzers.zip" "%ROOTDIR%\vnd\"
move "%temp%\%version%-mod-analyzer.zip" "%ROOTDIR%\vnd\"
copy "%ROOTDIR%\vnd\changes.chg" "%ROOTDIR%\vnd\%version%-changes.chg"

:finished
