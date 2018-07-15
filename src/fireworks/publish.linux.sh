#!/bin/sh
docker tag azure-mesh-fireworks-web:dev-alpine vipulmmsft/azure-mesh-fireworks-web:0.1-alpine
docker push vipulmmsft/azure-mesh-fireworks-web:0.1-alpine
docker tag azure-mesh-fireworks-worker-v1:dev-strech vipulmmsft/azure-mesh-fireworks-worker-v1:0.1-strech
docker push vipulmmsft/azure-mesh-fireworks-worker-v1:0.1-strech
docker tag azure-mesh-fireworks-worker-v2:dev-strech vipulmmsft/azure-mesh-fireworks-worker-v2:0.1-strech
docker push vipulmmsft/azure-mesh-fireworks-worker-v2:0.1-strech
