@echo off
call .\bin\docker\blazor\install.bat
docker exec -it blazor-wasm-app-container dotnet build /app/Application.csproj
docker exec -it blazor-wasm-app-container dotnet serve /app/ -c Release
cmd /k