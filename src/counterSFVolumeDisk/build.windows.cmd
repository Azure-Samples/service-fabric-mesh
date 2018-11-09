@echo off
echo docker build src -f src\counterService\Dockerfile -t azure-mesh-counter:dev-nanoserver-1709
docker build src -f src\counterService\Dockerfile -t azure-mesh-counter:dev-nanoserver-1709