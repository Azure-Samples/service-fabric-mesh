#!/bin/sh
echo docker build linux/main -f linux/main/Dockerfile -t azure-mesh-helloworld:dev-alpine
docker build linux/main -f linux/main/Dockerfile -t azure-mesh-helloworld:dev-alpine
echo docker build linux/sidecar -f linux/sidecar/Dockerfile -t azure-mesh-helloworld-sidecar:dev-alpine
docker build linux/sidecar -f linux/sidecar/Dockerfile -t azure-mesh-helloworld-sidecar:dev-alpine
