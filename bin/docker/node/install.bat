docker images -q typescript-compiler | find /c /v "" > nul
if %errorlevel%==1 (
    docker pull node:alpine
    docker build -t typescript-compiler . -f .\bin\docker\node\Dockerfile
)
docker container inspect typescript-compiler-container >nul 2>&1
if %errorlevel% equ 0 (
    docker ps -q --filter "name=typescript-compiler-container" > nul
    if %errorlevel%==1 (
        docker start typescript-compiler-container
    )
) else (
    docker run -v %CD%:/app -d --name typescript-compiler-container typescript-compiler tail -f /dev/null
    docker exec -it typescript-compiler-container npm install
)