## Visual Objects

Effective November 2, 2020, download rate limits apply to anonymous and authenticated requests to Docker Hub from Docker Free plan accounts and are enforced by IP address.

This sample pulls the following public images from Docker Hub. Please note that you may be rate limited.

| Source                      | Image   |
| -------------               |-------------|
| web/Dockerfile       | microsoft/aspnetcore:2.0       |
|| microsoft/aspnetcore-build:2.0|
| worker/Dockerfile| microsoft/dotnet:2.0-runtime|
|| microsoft/dotnet:2.0-sdk|
| worker/rotate.Dockerfile| microsoft/dotnet:2.0-runtime|
|| microsoft/dotnet:2.0-sdk|

For more details, see [Authenticate with Docker Hub](https://docs.microsoft.com/en-us/azure/container-registry/buffer-gate-public-content#authenticate-with-docker-hub).