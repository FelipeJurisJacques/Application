@echo off
cd ..\
call .\bin\docker\blazor\install.bat
docker exec -it blazor-wasm-app-container dotnet run -c Release --urls=http://+:8080 /app/Application.csproj
cmd /k