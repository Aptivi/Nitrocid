@echo off

set ROOTDIR=%~dp0\..

echo Finding DocFX...
if exist "%USERPROFILE%\.dotnet\tools\docfx.exe" goto :build
echo You don't have DocFX installed. Download and install .NET and DocFX.
goto :finished

:build
REM Turn off telemetry and logo
set DOTNET_CLI_TELEMETRY_OPTOUT=1
set DOTNET_NOLOGO=1

echo Building the documentation...
"%USERPROFILE%\.dotnet\tools\docfx.exe" "%ROOTDIR%\DocGen\docfx.json"

:finished
