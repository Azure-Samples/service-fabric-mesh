# Service Fabric Mesh Sample Application Model Templates

This folder contains the deployment templates for the example Service Fabric Mesh Applications that shows how to achieve a particular scenario using Service Fabric Resource Model. These templates can be deployed to Azure Service Fabric Mesh service using Azure Mesh CLI. 

|Example Name|Scenario Description|
|------------|--------------------|
| [Hello World App](./helloworld) | Deploy a single container microservices application from public or private container registry and connect to the service endpoint. |
| [Counter App](./counter) | Store state by mounting Azure Files based volume inside the container. |
| [Voting App](./voting) | Create an application with a frontend and backend service that uses DNS-based resolution. |
| [Visual Objects App](./visualobjects) | Scale and upgrade microservices within an application.  |
| [Fireworks App](./fireworks) | Independently Scale different types of micro-services within an application. |