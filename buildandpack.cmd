@echo off

REM    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
REM
REM    This file is part of Kernel Simulator
REM
REM    Kernel Simulator is free software: you can redistribute it and/or modify
REM    it under the terms of the GNU General Public License as published by
REM    the Free Software Foundation, either version 3 of the License, or
REM    (at your option) any later version.
REM
REM    Kernel Simulator is distributed in the hope that it will be useful,
REM    but WITHOUT ANY WARRANTY; without even the implied warranty of
REM    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
REM    GNU General Public License for more details.
REM
REM    You should have received a copy of the GNU General Public License
REM    along with this program.  If not, see <https://www.gnu.org/licenses/>.

REM This script builds KS and packs the artifacts. Use when you have VS installed.
set ksversion=0.0.22.1

echo Make sure you have the following:
echo   - %ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe
echo   - %ProgramFiles%\WinRAR\rar.exe
echo   - %ProgramFiles%\7-Zip\7z.exe
echo   - %ProgramFiles(x86)%\GnuWin32\bin\gzip.exe
echo.
echo Press any key to start.
pause > nul

echo Finding MSBuild...
for /f "delims=" %%i in ('"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe') do set msbuildpath=%%i
if %errorlevel% == 0 goto :download
echo There was an error trying to find MSBuild (%errorlevel%).
goto :finished

:download
echo MSBuild found in %msbuildpath%
echo Downloading packages...
"%msbuildpath%" "Kernel Simulator.sln" -t:restore > %temp%/buildandpack.log 2>&1
if %errorlevel% == 0 goto :build
echo There was an error trying to download packages (%errorlevel%).
goto :finished

:build
echo Building Kernel Simulator...
"%msbuildpath%" "Kernel Simulator.sln" -p:Configuration=Release >> %temp%/buildandpack.log 2>&1
if %errorlevel% == 0 goto :packbin
echo There was an error trying to build (%errorlevel%).
goto :finished

:packbin
echo Packing binary...
del "Kernel Simulator\KSBuild\*.nupkg" >> %temp%/buildandpack.log 2>&1
"%ProgramFiles%\WinRAR\rar.exe" a -ep1 -r -m5 %temp%/%ksversion%-bin.rar "Kernel Simulator\KSBuild\" >> %temp%/buildandpack.log 2>&1
if %errorlevel% == 0 goto :packsrc
echo There was an error trying to pack binary (%errorlevel%).
goto :finished

:packsrc
echo Binary artifact found in root directory.
echo Packing source...
rmdir /S /Q "Kernel Simulator\KSBuild\" >> %temp%/buildandpack.log 2>&1
rmdir /S /Q "Kernel Simulator\obj\" >> %temp%/buildandpack.log 2>&1
rmdir /S /Q "KSTests\KSTest" >> %temp%/buildandpack.log 2>&1
rmdir /S /Q "KSTests\obj\" >> %temp%/buildandpack.log 2>&1
rmdir /S /Q "KSJsonifyLocales\obj\" >> %temp%/buildandpack.log 2>&1
rmdir /S /Q "KSConverter\obj\" >> %temp%/buildandpack.log 2>&1
rmdir /S /Q "DocGen\api\" >> %temp%/buildandpack.log 2>&1
rmdir /S /Q "DocGen\obj\" >> %temp%/buildandpack.log 2>&1
rmdir /S /Q "docs\" >> %temp%/buildandpack.log 2>&1
echo Packing source using rar...
"%ProgramFiles%\WinRAR\rar.exe" a -ep1 -r -m5 -x.git -x.vs %temp%/%ksversion%-src.rar >> %temp%/buildandpack.log 2>&1
if %errorlevel% == 0 goto :packsrctar
echo There was an error trying to pack source using rar (%errorlevel%).
goto :finished

:packsrctar
echo Packing source using tar...
"%ProgramFiles%\7-Zip\7z.exe" a -ttar %temp%/%ksversion%-src.tar -xr!.git -xr!.vs >> %temp%/buildandpack.log 2>&1
if %errorlevel% == 0 goto :compresstar
echo There was an error trying to pack source using tar (%errorlevel%).
goto :finished

:compresstar
echo Compressing tar using gzip...
"%ProgramFiles(x86)%\GnuWin32\bin\gzip.exe" -9 %temp%/%ksversion%-src.tar >> %temp%/buildandpack.log 2>&1
if %errorlevel% == 0 goto :complete
echo There was an error trying to compress tar (%errorlevel%).
goto :finished

:complete
move %temp%\%ksversion%-bin.rar >> %temp%/buildandpack.log 2>&1
move %temp%\%ksversion%-src.rar >> %temp%/buildandpack.log 2>&1
move %temp%\%ksversion%-src.tar.gz >> %temp%/buildandpack.log 2>&1
echo Build and pack successful.
goto :finished

:finished
pause