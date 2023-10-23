@echo off
REM This batch file is used for fallback purposes. Don't use this file. Only use it on authorized user, which is us.

REM Make a package, then push
choco pack
choco push --source https://push.chocolatey.org/

REM Remove nupkg
delete /q *.nupkg
