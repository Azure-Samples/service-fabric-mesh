## Counter

Effective November 2, 2020, download rate limits apply to anonymous and authenticated requests to Docker Hub from Docker Free plan accounts and are enforced by IP address.

This sample pulls the following public images from Docker Hub. Please note that you may be rate limited.

| Source                      | Image   |
| -------------               |-------------|
| src/counterService/Dockerfile       | microsoft/aspnetcore:2.0-nanoserver-1709       |
|| microsoft/aspnetcore-build:2.0|
| src/counterService/linux.Dockerfile | microsoft/aspnetcore:2.0|
|| microsoft/aspnetcore-build:2.0|
| sfvolume/counter/linux/App Resources/app.yaml    | seabreeze/azure-mesh-counter:0.5-alpine|

For more details, see [Authenticate with Docker Hub](https://docs.microsoft.com/en-us/azure/container-registry/buffer-gate-public-content#authenticate-with-docker-hub).