docker images -q blazor-wasm-app | find /c /v "" > nul
if %errorlevel%==1 (
    docker pull mcr.microsoft.com/dotnet/sdk:9.0
    docker pull mcr.microsoft.com/dotnet/aspnet:9.0
    docker build -t blazor-wasm-app . -f .\bin\docker\blazor\Dockerfile
)
docker container inspect blazor-wasm-app-container >nul 2>&1
if %errorlevel% equ 0 (
    docker ps -q --filter "name=blazor-wasm-app-container" > nul
    if %errorlevel%==1 (
        docker start blazor-wasm-app-container tail -f /dev/null
    )
) else (
    docker run -v %CD%:/app -p 8080:8080 -d --name blazor-wasm-app-container --cpus 8 blazor-wasm-app tail -f /dev/null
)