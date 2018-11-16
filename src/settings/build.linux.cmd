#!/bin/sh
echo docker build src -f src/settingsService/linux.Dockerfile -t azure-mesh-settings:dev-alpine
docker build src -f src/settingsService/linux.Dockerfile -t azure-mesh-settings:dev-alpine
