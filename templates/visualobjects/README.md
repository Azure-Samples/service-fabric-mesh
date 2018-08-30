# Service Fabric Mesh Sample Application - Visual Objects

This folder contains the deployment templates for the Visual Object sample. This sample shows you how to deploy, upgrade and scale out a mesh application.

You will need the Azure CLI and the Mesh extension to deploy this sample. Follow this link for instructions on how to install the [Azure CLI and Mesh extension](https://docs.microsoft.com/en-us/azure/service-fabric-mesh/service-fabric-mesh-howto-setup-cli)

Deploy Visual Object using the following steps:

1. Login to you Azure account - ``az login``

1. Create a resource group - ``az group create --name <resource group name> --location eastus``

1. Deploy the base template - ``az mesh deployment create --resource-group <resource group name> --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/master/templates/visualobjects/mesh_rp.base.linux.json --parameters "{\"location\": {\"value\": \"eastus\"}}"`` Once the deployment is complete, you can navigate to the application IP to see flying triangles.

1. Scale out the application - ``az mesh deployment create --resource-group <resource group name> --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/master/templates/visualobjects/mesh_rp.scaleout.linux.json --parameters "{\"location\": {\"value\": \"eastus"}}"`` Once the deployment is completed, you will now see three flying triangles, each representing an replica of the backend service.

1. Upgrade the application - ``az mesh deployment create --resource-group <resource group name> --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/master/templates/visualobjects/mesh_rp.upgrade.linux.json --parameters "{\"location\": {\"value\": \"eastus\"}}"`` As the replicas get upgrade, the triangles will start rotating.

[The source code for the sample can be found here](../../src/visualobjects)