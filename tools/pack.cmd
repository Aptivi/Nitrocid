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
"%ProgramFiles%\WinRAR\rar.exe" a -ep1 -r -m5 %temp%/%ksversion%-bin.rar "..\public\Nitrocid\KSBuild\net6.0\"
"%ProgramFiles%\WinRAR\rar.exe" a -ep1 -r -m5 %temp%/%ksversion%-analyzers.rar "..\public\Nitrocid\KSBuild\netstandard2.0\"
if %errorlevel% == 0 goto :complete
echo There was an error trying to pack binary (%errorlevel%).
goto :finished

:complete
move %temp%\%ksversion%-bin.rar
move %temp%\%ksversion%-analyzers.rar
copy "..\public\Nitrocid\KSBuild\net6.0\Nitrocid.pdb" .\%ksversion%.pdb

echo Pack successful.
:finished
