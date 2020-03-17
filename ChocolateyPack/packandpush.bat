@echo off
REM This batch file is used for fallback purposes. Don't use this file. Only use it on authorized user, which is us. Haha lol!

REM Make a package, then push
choco pack
choco push

REM Remove nupkg
delete /q *.nupkg