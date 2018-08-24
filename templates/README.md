# Service Fabric Mesh Sample Application Model Templates

This folder contains the deployment templates for the example Service Fabric Mesh Applications that shows how to achieve a particular scenario using Service Fabric Resource Model. These templates can be deployed to Azure Service Fabric Mesh service using Azure Mesh CLI. 

|Example Name|Scenario Description|Deploy to Azure|
|------------|--------------------|---------------|
| [Hello World App](./helloworld) | Deploy a single container microservices application from public or private container registry and connect to the service endpoint. | <a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure-Samples%2Fservice-fabric-mesh%2Fmaster%2Ftemplates%2Fhelloworld%2Fmesh_rp.linux.json" target="_blank"><img src="https://azuredeploy.net/deploybutton.png"/></a> |
| [Counter App](./counter) | Store state by mounting Azure Files based volume inside the container. | <a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure-Samples%2Fservice-fabric-mesh%2Fmaster%2Ftemplates%2Fcounter%2Fmesh_rp.linux.json" target="_blank"><img src="https://azuredeploy.net/deploybutton.png"/></a> |
| [Voting App](./voting) | Create an application with a frontend and backend service that uses DNS-based resolution. | <a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure-Samples%2Fservice-fabric-mesh%2Fmaster%2Ftemplates%2Fvoting%2Fmesh_rp.linux.json" target="_blank"><img src="https://azuredeploy.net/deploybutton.png"/></a> |
| [Visual Objects App](./visualobjects) | Scale and upgrade microservices within an application.  | Multiple templates... |
| [Fireworks App](./fireworks) | Independently Scale different types of micro-services within an application. | Not here... |