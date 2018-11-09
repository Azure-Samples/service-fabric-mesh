docker build . -f ./VotingWeb/Dockerfile -t seabreeze/azure-mesh-rcvoting-web:1.0-windows
docker build . -f ./VotingData/Dockerfile -t seabreeze/azure-mesh-rcvoting-data:1.0-windows
docker push seabreeze/azure-mesh-rcvoting-web:1.0-windows
docker push seabreeze/azure-mesh-rcvoting-data:1.0-windows
