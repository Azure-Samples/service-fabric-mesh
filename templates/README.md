# Service Fabric Mesh Application Model - Template Samples

This folder contains the deployment templates for the example Service Fabric Mesh Applications that shows how to achieve a particular scenario using Service Fabric Resource Model. These templates can be deployed to Azure Service Fabric Mesh service using Azure Mesh CLI, or using the Deploy to Azure button below.

Follow this link for instructions on how to install the [Azure CLI and Mesh extension](https://docs.microsoft.com/en-us/azure/service-fabric-mesh/service-fabric-mesh-howto-setup-cli)

**Note**:
If you've deployed your application using the Azure deploy button and the Azure Portal, use the following Azure CLI commands to get the public IPs for the application:

``az mesh network list``

This command will list all Mesh networks deployed in the current subscription. Look for the ``"publicIpAddress"`` and ``"publicPort"`` elements in the relevant network resource description.

``az mesh network show --resource-group <resource group name> --name <network name>``

This command will output all information about one network resource. To get the network name, look at the template file.

|Sample|Scenario Description|Deploy to Azure - Linux|Deploy to Azure - Windows|
|------------|--------------------|-----------------------|-------------------------|
| [Hello World App](./helloworld) | Deploy a single container microservices application from public or private container registry and connect to the service endpoint. | <a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure-Samples%2Fservice-fabric-mesh%2Fmaster%2Ftemplates%2Fhelloworld%2Fmesh_rp.linux.json" target="_blank"><img src="https://azuredeploy.net/deploybutton.png"/></a> | <a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure-Samples%2Fservice-fabric-mesh%2Fmaster%2Ftemplates%2Fhelloworld%2Fmesh_rp.windows.json" target="_blank"><img src="https://azuredeploy.net/deploybutton.png"/></a> |
| [Counter App](./counter) | Store state by mounting Azure Files based volume inside the container. <br><br> **Note:** This template requires an Azure Files file share to already be provisioned [Instructions](https://docs.microsoft.com/en-us/azure/storage/files/storage-how-to-create-file-share) | <a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure-Samples%2Fservice-fabric-mesh%2Fmaster%2Ftemplates%2Fcounter%2Fmesh_rp.linux.json" target="_blank"><img src="https://azuredeploy.net/deploybutton.png"/></a> | <a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure-Samples%2Fservice-fabric-mesh%2Fmaster%2Ftemplates%2Fcounter%2Fmesh_rp.windows.json" target="_blank"><img src="https://azuredeploy.net/deploybutton.png"/></a> |
| [Counter App (Service Fabric VolumeDisk)](./counter) | Store state on Service Fabric Volume Disk based volume inside the container. |  | <a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure-Samples%2Fservice-fabric-mesh%2Fmaster%2Ftemplates%2FcounterSFVolumeDisk%2Fsfvd_mesh_rp.windows.json" target="_blank"><img src="https://azuredeploy.net/deploybutton.png"/></a> |
| [Voting App](./voting) | Create an application with a frontend and backend service that uses DNS-based resolution. | <a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure-Samples%2Fservice-fabric-mesh%2Fmaster%2Ftemplates%2Fvoting%2Fmesh_rp.linux.json" target="_blank"><img src="https://azuredeploy.net/deploybutton.png"/></a> | <a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure-Samples%2Fservice-fabric-mesh%2Fmaster%2Ftemplates%2Fvoting%2Fmesh_rp.windows.json" target="_blank"><img src="https://azuredeploy.net/deploybutton.png"/></a> |
| [Visual Objects App](./visualobjects) | Scale and upgrade microservices within an application.  | [Look here](./visualobjects/)  | [Look here](./visualobjects/) |