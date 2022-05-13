@echo off

REM    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

REM This script builds KS documentation and packs the artifacts. Use when you have VS installed.
set ksversion=0.0.21.5

echo Make sure you have the following:
echo   - Visual Studio 2017+
echo   - %ProgramData%\chocolatey\bin\docfx.exe
echo   - %ProgramFiles%\WinRAR\rar.exe
echo.
echo Press any key to start.
pause > nul

echo Finding DocFX...
if exist %ProgramData%\chocolatey\bin\docfx.exe goto :build
echo You don't have DocFX installed. Download and install Chocolatey and DocFX.
goto :finished

:build
echo Building Kernel Simulator Documentation...
%ProgramData%\chocolatey\bin\docfx.exe "DocGen\docfx.json" > %temp%/buildandpack.log 2>&1
if %errorlevel% == 0 goto :pack
echo There was an error trying to build documentation (%errorlevel%).
goto :finished

:pack
echo Packing documentation...
"%ProgramFiles%\WinRAR\rar.exe" a -ep1 -r -m5 %temp%/%ksversion%-doc.rar "docs\" >> %temp%/buildandpack.log 2>&1
if %errorlevel% == 0 goto :finalize
echo There was an error trying to pack documentation (%errorlevel%).
goto :finished

:finalize
rmdir /S /Q "DocGen\api\" >> %temp%/buildandpack.log 2>&1
rmdir /S /Q "DocGen\obj\" >> %temp%/buildandpack.log 2>&1
rmdir /S /Q "docs\" >> %temp%/buildandpack.log 2>&1
move %temp%\%ksversion%-doc.rar >> %temp%/buildandpack.log 2>&1
echo Build and pack successful.
goto :finished

:finished
pause