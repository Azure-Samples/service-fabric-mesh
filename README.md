---
languages:
- csharp
- yaml
products:
- azure
topic: sample
---

![Mesh-01](./media/Mesh_icon.png)

# Service Fabric Mesh Samples

This repository contains samples that showcase what you can do with Service Fabric Mesh Applications.

For more details about Service Fabric Mesh public preview in Azure, refer to this blog post - https://azure.microsoft.com/en-us/blog/azure-service-fabric-mesh-is-now-in-public-preview/

## Structure of this repo

This repository has two main folders:

- **src** contains source code samples. Some of these are built using the [Visual Studio Tools for Mesh](https://docs.microsoft.com/en-us/azure/service-fabric-mesh/service-fabric-mesh-howto-setup-developer-environment-sdk). Use these samples if you want to build the code from source, publish container images and deploy the app to Mesh.

- **templates** contains samples with ARM templates ready to deploy to Azure Mesh. Use these templates if you want to deploy an application using existing container images and deployment templates

Both folders contain instructions on how to work with the samples.

## Useful links

- Documentation - https://docs.microsoft.com/azure/service-fabric-mesh/

- REST API Specification - https://github.com/Azure/azure-rest-api-specs/tree/master/specification/servicefabricmesh/resource-manager

- Azure Service Fabric Mesh lab - https://github.com/MikkelHegn/ContainersSFLab/blob/master/Instructions/ServiceFabricMesh/README.md

## Feedback and issues

We look forward to hearing your feedback about Mesh.

Please use this repo's [Issues](https://github.com/Azure-Samples/service-fabric-mesh/issues) to raise issues with the samples.

For any other issue you encounter with Mesh, please use the [Issues](https://github.com/Azure/seabreeze-preview-pr/issues) in this repo https://github.com/Azure/seabreeze-preview-pr/ to inform us of any bugs you come across, or improvements you would like to request.

---
*This project welcomes contributions and suggestions. Most contributions require you to agree to a Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the Microsoft Open Source Code of Conduct. For more information see the Code of Conduct FAQ or contact opencode@microsoft.com with any additional questions or comments.*
