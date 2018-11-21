# Service Fabric Mesh Application Model - Source Samples

This folder contains the source code for the example Service Fabric Mesh Applications that shows how to achieve a particular scenario using Service Fabric Resource Model.

|Sample|Scenario Description|Developer Tools|
|------------|--------------------|-----------------------|
| [Hello World App](./helloworld) | Static webpage hosted in a container. For Linux it uses nginx, for Windows IIS | No requirements |
| [Counter App](./counter) | A Store state by mounting Azure Files based volume inside the container. <br><br> **Note:** This template requires an Azure Files file share to already be provisioned [Instructions](https://docs.microsoft.com/en-us/azure/storage/files/storage-how-to-create-file-share) | Visual Studio Mesh Tooling |
| [TodoListApp](./todolistapp) | Create an application with a frontend and backend service that uses DNS-based resolution. Used as a tutorial [here](https://docs.microsoft.com/en-us/azure/service-fabric-mesh/service-fabric-mesh-tutorial-create-dotnetcore) | Visual Studio Mesh Tooling |
| [Visual Objects App](./visualobjects) | Scale and upgrade microservices within an application.  | Visual Studio Mesh Tooling |
| [Voting App](./votingapp) | Create an application with a frontend and backend service that uses DNS-based resolution | Visual Studio Mesh Tooling for the Windows version, VS Code / dotnet cli can be used for the Linux version |
| [Voting App with Reliable Collections](./RCVotingApp) | Create an application with a frontend and backend service that uses DNS-based resolution and reliable collections as backed store| Visual Studio Mesh Tooling for the Windows version, VS Code / dotnet cli can be used for the Linux version |