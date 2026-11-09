@echo off
setlocal
cd /d "%~dp0"
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0GenerarBackup_StoredProcedures.ps1"
pause

