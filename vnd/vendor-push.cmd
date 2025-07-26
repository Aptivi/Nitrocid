@echo off

set apikey=%1
set source=%2
if "%source%" == "" set source=nuget.org

set ROOTDIR=%~dp0..

REM This script pushes.
echo Pushing...

REM Find nupkg files
set "found_packages=0"
for /r "%ROOTDIR%" %%f in (*.nupkg) do (
    set /a found_packages+=1
    set "package_path[!found_packages!]=%%f"
)

REM Try to push packages one by one
set error=0
for /l %%i in (1,1,%found_packages%) do (
    echo !package_path[%%i]!
    dotnet nuget push "!package_path[%%i]!" --api-key %apikey% --source %source% --skip-duplicate
    if !errorlevel! neq 0 (
        set error=!errorlevel!
        echo !package_path[%%i]! (!error!^)
        exit /b !error!
    )
)
