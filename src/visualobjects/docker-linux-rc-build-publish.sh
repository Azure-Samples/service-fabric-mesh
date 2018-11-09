docker build . --network host -f web/Dockerfile -t seabreeze/azure-mesh-visualobjects-web:1.1-stretch
docker build . --network host -f rcworker/Linux.Dockerfile -t seabreeze/azure-mesh-visualobjects-rcworker:1.1-stretch
docker build . --network host -f rcworker/rotate.Linux.Dockerfile -t seabreeze/azure-mesh-visualobjects-rcworker:1.1-rotate-stretch
docker push seabreeze/azure-mesh-visualobjects-web:1.1-stretch
docker push seabreeze/azure-mesh-visualobjects-rcworker:1.1-stretch
docker push seabreeze/azure-mesh-visualobjects-rcworker:1.1-rotate-stretch
