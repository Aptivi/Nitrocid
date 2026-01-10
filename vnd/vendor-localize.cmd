@echo off

set ROOTDIR=%~dp0\..
set ROOTDIRFIND=%~dp0..

echo Restoring NuGet packages...
"%ProgramFiles%\dotnet\dotnet.exe" restore "%ROOTDIR%\Nitrocid.slnx" --packages "%ROOTDIR%\nuget"
if %errorlevel% == 0 goto :copy
echo There was an error trying to restore packages (%errorlevel%).
goto :finished

:copy
echo Copying dependencies...
md "%ROOTDIR%\deps"
forfiles /s /m *.nupkg /p "%ROOTDIRFIND%\nuget\" /C "cmd /c copy @path ""%ROOTDIR%\deps"""
forfiles /s /m *.nupkg /p "%USERPROFILE%\.nuget\packages\" /C "cmd /c copy @path ""%ROOTDIR%\deps"""
if %errorlevel% == 0 goto :initconfig
echo There was an error trying to copy dependencies (%errorlevel%).
goto :finished

:initconfig
echo "<?xml version=""1.0"" encoding=""utf-8""?>" > "%ROOTDIR%\NuGet.config"
echo "<configuration>" >> "%ROOTDIR%\NuGet.config"
echo "  <packageSources>" >> "%ROOTDIR%\NuGet.config"
echo "    <clear />" >> "%ROOTDIR%\NuGet.config"
echo "    <add key=""nuget.org"" value=""./deps"" />" >> "%ROOTDIR%\NuGet.config"
echo "  </packageSources>" >> "%ROOTDIR%\NuGet.config"
echo "</configuration>" >> "%ROOTDIR%\NuGet.config"
rd /s /q "%ROOTDIR%\nuget"
if %errorlevel% == 0 goto :success
echo There was an error trying to delete cache (%errorlevel%).
goto :finished

:success
echo Localization successful.

:finished
