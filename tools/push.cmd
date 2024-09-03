@echo off
set apikey=%1

REM This script pushes. Use when you have VS installed.
echo Pushing...
cmd /C "forfiles /s /m *.nupkg /p ..\ /C "cmd /c dotnet nuget push @path --api-key %apikey% --source nuget.org""
if %errorlevel% == 0 goto :success
echo There was an error trying to push (%errorlevel%).
goto :finished

:success
echo Push successful.
:finished
