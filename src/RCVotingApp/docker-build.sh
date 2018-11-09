docker build . --network host -f ./VotingWeb/Dockerfile -t votingweb:dev
docker build . --network host -f ./VotingData/Linux.Dockerfile -t votingdata:dev
