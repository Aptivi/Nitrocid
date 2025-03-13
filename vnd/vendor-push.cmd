@echo off

set apikey=%1
set source=%2
if "%source%" == "" set source=nuget.org

set ROOTDIR=%~dp0..

REM This script pushes.
echo Pushing...
forfiles /s /m *.nupkg /p %ROOTDIR%\ /C "cmd /c echo @path && dotnet nuget push @path --api-key %apikey% --source %source%"
if %errorlevel% == 0 goto :success
echo There was an error trying to push (%errorlevel%).
goto :finished

:success
echo Push successful.

:finished
