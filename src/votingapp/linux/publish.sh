#!/bin/bash
registry=$1

docker login $registry

docker tag votingweb:1.0-stretch $registry/votingweb:1.0-stretch
docker tag votingdata:1.0-stretch $registry/votingdata:1.0-stretch

docker push $registry/votingweb:1.0-stretch
docker push $registry/votingdata:1.0-stretch