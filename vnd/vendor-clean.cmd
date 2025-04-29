@echo off

set ROOTDIR=%~dp0..

echo Cleaning up...
for %%f in (bin, obj, docs, KSBuild, KSAnalyzer, KSTest, nitrocid-28, nitrocid-28-lite, tmp) do forfiles /s /m %%f /p %ROOTDIR%\ /C "cmd /c echo @path && rd /s /q @path"
