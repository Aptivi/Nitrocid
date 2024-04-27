@echo off

REM This script builds KS documentation and packs the artifacts. Use when you have VS installed.
for /f "tokens=*" %%g in ('findstr "<Version>" ..\Directory.Build.props') do (set MIDVER=%%g)
for /f "tokens=1 delims=<" %%a in ("%MIDVER:~9%") do (set ksversion=%%a)

:pack
echo Packing documentation...
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%ksversion%-doc.zip "..\docs\*"
if %errorlevel% == 0 goto :finalize
echo There was an error trying to pack documentation (%errorlevel%).
goto :finished

:finalize
rmdir /S /Q "..\DocGen\api\"
rmdir /S /Q "..\DocGen\obj\"
rmdir /S /Q "..\docs\"
move %temp%\%ksversion%-doc.zip
echo Build and pack successful.
goto :finished

:finished
