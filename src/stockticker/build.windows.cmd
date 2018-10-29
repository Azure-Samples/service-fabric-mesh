@echo off
echo docker build src -f src\stocktickerService\Dockerfile -t azure-mesh-stockticker:dev-nanoserver-1709
docker build src -f src\stocktickerService\Dockerfile -t azure-mesh-stockticker:dev-nanoserver-1709
