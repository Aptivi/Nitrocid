@echo off

REM This script builds and packs the artifacts.
set releaseconfig=%1
if "%releaseconfig%" == "" set releaseconfig=Release

set buildoptions=%*
call set buildoptions=%%buildoptions:*%1=%%
if "%buildoptions%" == "*=" set buildoptions=

REM Turn off telemetry and logo
set DOTNET_CLI_TELEMETRY_OPTOUT=1
set DOTNET_NOLOGO=1

set ROOTDIR=%~dp0\..

echo Building with configuration %releaseconfig%...
"%ProgramFiles%\dotnet\dotnet.exe" build "%ROOTDIR%\Nitrocid.sln" -p:Configuration=%releaseconfig% %buildoptions%
