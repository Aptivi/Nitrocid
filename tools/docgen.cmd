@echo off

REM    Nitrocid KS  Copyright (C) 2018-2022  Aptivi
REM
REM    This file is part of Nitrocid KS
REM
REM    Nitrocid KS is free software: you can redistribute it and/or modify
REM    it under the terms of the GNU General Public License as published by
REM    the Free Software Foundation, either version 3 of the License, or
REM    (at your option) any later version.
REM
REM    Nitrocid KS is distributed in the hope that it will be useful,
REM    but WITHOUT ANY WARRANTY; without even the implied warranty of
REM    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
REM    GNU General Public License for more details.
REM
REM    You should have received a copy of the GNU General Public License
REM    along with this program.  If not, see <https://www.gnu.org/licenses/>.

REM This script builds the documentation and packs the artifacts. Use when you have VS installed.
for /f "tokens=* USEBACKQ" %%f in (`type version`) do set ksversion=%%f

echo Finding DocFX...
if exist %USERPROFILE%\.dotnet\tools\docfx.exe goto :build
echo You don't have DocFX installed. Download and install .NET and DocFX.
goto :finished

:build
echo Building the documentation...
%USERPROFILE%\.dotnet\tools\docfx.exe "..\DocGen\docfx.json"
if %errorlevel% == 0 goto :success
echo There was an error trying to build documentation (%errorlevel%).
goto :finished

:success
echo Build and pack successful.
:finished
