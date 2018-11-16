@echo off
echo docker build src -f src\settingsService\Dockerfile -t azure-mesh-settings:dev-nanoserver-1709
docker build src -f src\settingsService\Dockerfile -t azure-mesh-settings:dev-nanoserver-1709
