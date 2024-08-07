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

REM Set locals and banner
setlocal
set ESSENTIALS=X:\Windows\Essentials
title Nitrocid LIVE!
echo.
echo                       ** Welcome to Nitrocid LIVE! **
echo.

REM DEBUG: Remove this ASAP
cmd.exe

REM Silently install dotnet
echo - Installing .NET 8.0...
start /w %ESSENTIALS%\Setup\dotnet-runtime.exe /install /quiet /norestart

REM Start wpeinit and N-KS
echo - Initializing necessary components...
wpeinit
echo - Starting Nitrocid...
dotnet %ESSENTIALS%\Nitrocid\Nitrocid.dll

REM Finished!
cls
echo.
echo                    --==++==-- Nitrocid LIVE! --==++==--
echo.
echo Thank you for using Nitrocid LIVE! To save your progress, download the full
echo         application. Restarting your computer back to your OS now...
echo.
echo           ** Unplug your USB or eject the CD during 60 seconds **
echo.
endlocal
timeout /t 60
shutdown /r /t 00
exit
