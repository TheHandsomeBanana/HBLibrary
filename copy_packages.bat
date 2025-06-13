@echo off
powershell -NoExit -ExecutionPolicy Bypass -File "%~dp0\copy_packages.ps1"
pause