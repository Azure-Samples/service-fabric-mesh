#!/bin/sh
echo docker build src -f src/stocktickerService/linux.Dockerfile -t azure-mesh-stockticker:dev-alpine
docker build src -f src/stocktickerService/linux.Dockerfile -t azure-mesh-stockticker:dev-alpine
