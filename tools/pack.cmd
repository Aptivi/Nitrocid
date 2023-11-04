@echo off

REM    Nitrocid KS  Copyright (C) 2018-2021  Aptivi
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

for /f "tokens=* USEBACKQ" %%f in (`type version`) do set ksversion=%%f

:packbin
echo Packing binary...
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%ksversion%-bin.zip "..\public\Nitrocid\KSBuild\net8.0\*"
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%ksversion%-bin-lite.zip "..\public\Nitrocid\KSBuild\net8.0\*" -xr!Addons
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%ksversion%-addons.zip "..\public\Nitrocid\KSBuild\net8.0\Addons\*"
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%ksversion%-analyzers.zip "..\public\Nitrocid\KSAnalyzer\netstandard2.0\*"
"%ProgramFiles%\7-Zip\7z.exe" a -tzip %temp%/%ksversion%-mod-analyzer.zip "..\public\Nitrocid\KSAnalyzer\net8.0\*"
if %errorlevel% == 0 goto :complete
echo There was an error trying to pack binary (%errorlevel%).
goto :finished

:complete
move %temp%\%ksversion%-bin.zip
move %temp%\%ksversion%-bin-lite.zip
move %temp%\%ksversion%-addons.zip
move %temp%\%ksversion%-analyzers.zip
move %temp%\%ksversion%-mod-analyzer.zip

echo Pack successful.
:finished
