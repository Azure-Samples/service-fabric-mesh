@echo off
setlocal
set BUILD_CMD=docker build windows -f windows\Dockerfile -t azure-mesh-helloworld:nanoserver
echo %BUILD_CMD%
%BUILD_CMD%
