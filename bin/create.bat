@echo off

cd /d "%~dp0"
cd ..\
set "PROJECT_PATH=%CD%"

docker pull mcr.microsoft.com/dotnet/sdk:9.0
docker run --rm -v "%PROJECT_PATH%:/app" -w /app mcr.microsoft.com/dotnet/sdk:9.0 dotnet new blazorwasm -o Application

cmd /k