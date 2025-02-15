@echo off
cd ..\
call .\bin\docker\node\install.bat
echo Compiling TypeScript...
docker exec -it typescript-compiler-container npm run build
echo Done.
cmd /k