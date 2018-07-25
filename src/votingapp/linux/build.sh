#!/bin/bash
docker build . -f Dockerfile-web -t votingweb:1.0-stretch
docker build . -f Dockerfile-data -t votingdata:1.0-stretch
docker rmi -f $(docker images -f "dangling=true" -q)