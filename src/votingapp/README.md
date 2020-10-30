## Voting App

Effective November 2, 2020, download rate limits apply to anonymous and authenticated requests to Docker Hub from Docker Free plan accounts and are enforced by IP address.

This sample pulls the following public images from Docker Hub. Please note that you may be rate limited.

| Source                      | Image   |
| -------------               |-------------|
| windows/VotingApp/VotingData/Dockerfile       | microsoft/aspnetcore:2.0-nanoserver-1709       |
|| microsoft/aspnetcore-build:2.0|
| windows/VotingApp/VotingWeb/Dockerfile| microsoft/aspnetcore:2.0-nanoserver-1709|
|| microsoft/aspnetcore-build:2.0|
| linux/Dockerfile-data   | microsoft/aspnetcore:2.0|
|| microsoft/aspnetcore-build:2.0|
| linux/Dockerfile-web| microsoft/aspnetcore:2.0|
|| microsoft/aspnetcore-build:2.0|
| linux/VotingData/Dockerfile       | microsoft/aspnetcore:2.0       |
|| microsoft/aspnetcore-build:2.0|

For more details, see [Authenticate with Docker Hub](https://docs.microsoft.com/en-us/azure/container-registry/buffer-gate-public-content#authenticate-with-docker-hub).