docker build . -f web\Dockerfile -t seabreeze/azure-mesh-visualobjects-web:1.1-rc-nanoserver-1709
docker build . -f rcworker\Dockerfile -t seabreeze/azure-mesh-visualobjects-rcworker:1.1-servercore-1803
docker build . -f rcworker\rotate.Dockerfile -t seabreeze/azure-mesh-visualobjects-rcworker:1.1-rotate-servercore-1803
docker push seabreeze/azure-mesh-visualobjects-web:1.1-rc-nanoserver-1709
docker push seabreeze/azure-mesh-visualobjects-rcworker:1.1-servercore-1803
docker push seabreeze/azure-mesh-visualobjects-rcworker:1.1-rotate-servercore-1803
