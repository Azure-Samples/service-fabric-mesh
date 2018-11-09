docker build . --network host -f ./VotingWeb/Dockerfile -t seabreeze/azure-mesh-rcvoting-web:1.0-linux
docker build . --network host -f ./VotingData/Linux.Dockerfile -t seabreeze/azure-mesh-rcvoting-data:1.0-linux
docker push seabreeze/azure-mesh-rcvoting-web:1.0-linux
docker push seabreeze/azure-mesh-rcvoting-data:1.0-linux
