#!/bin/sh
echo docker build src -f src/counterService/linux.Dockerfile -t azure-mesh-counter:dev-alpine
docker build src -f src/counterService/linux.Dockerfile -t azure-mesh-counter:dev-alpine
