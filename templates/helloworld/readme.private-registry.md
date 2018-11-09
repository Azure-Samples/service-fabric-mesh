# Deploy a Service Fabric Mesh app from a private container image registry

This article shows how to deploy a simple Azure Service Fabric Mesh app that uses container images from a public registry.

## Install Docker

### Windows 10

Download and install the latest version of [Docker Community Edition for Windows][download-docker] to support the containerized Service Fabric apps used by Service Fabric Mesh.

During installation, select **Use Windows containers instead of Linux containers** when asked. 

If Hyper-V is not enabled on your machine, Docker's installer will offer to enable it. Click **OK** to do so if prompted.

### Windows Server 2016

If you don't have the Hyper-V role enabled, open PowerShell as an administrator and run the following command to enable Hyper-V, and then restart your computer. For more information, see [Docker Enterprise Edition for Windows Server][download-docker-server].

```powershell
Install-WindowsFeature -Name Hyper-V -IncludeManagementTools
```

Restart your computer.

Open PowerShell as an administrator and run the following commands to install Docker:

```powershell
Install-Module DockerMsftProvider -Force
Install-Package Docker -ProviderName DockerMsftProvider -Force
Install-WindowsFeature Containers
```
## Setup Service Fabric Mesh CLI

You can use the Azure Cloud Shell or a local installation of the Azure CLI to complete this quickstart. Install Azure Service Fabric Mesh CLI extension module by following these [instructions](https://docs.microsoft.com/en-us/azure/service-fabric-mesh/service-fabric-mesh-howto-setup-cli).

## Sign in to Azure
Sign in to Azure and set your subscription.

```azurecli
az login
az account set --subscription "<subscriptionID>"
```

## Create a resource group

An Azure resource group is a logical container into which Azure resources are deployed and managed. Use the following command to create a resource group named *myResourceGroup* in the *eastus* location.

```azurecli
az group create --name myResourceGroup --location eastus
```

## Create a container registry and push an image to it

### Create a container registry

Create an Azure container registry (ACR) instance using the `az acr create` command. The registry name must be unique within Azure and contain 5-50 alphanumeric characters. In the following example, the name *myContainerRegistry007* is used. If you get an error that the registry name is in use, choose a different name. Use that name everywhere `<acrName>` appears in these instructions.

```azurecli
az acr create --resource-group myResourceGroup --name myContainerRegistry007 --sku Basic
```

When the registry is created, you'll see output similar to the following:

```json
{
  "adminUserEnabled": false,
  "creationDate": "2017-09-08T22:32:13.175925+00:00",
  "id": "/subscriptions/00000000-0000-0000-0000-000000000000/resourceGroups/myResourceGroup/providers/Microsoft.ContainerRegistry/registries/myContainerRegistry007",
  "location": "eastus",
  "loginServer": "myContainerRegistry007.azurecr.io",
  "name": "myContainerRegistry007",
  "provisioningState": "Succeeded",
  "resourceGroup": "myResourceGroup",
  "sku": {
    "name": "Basic",
    "tier": "Basic"
  },
  "status": null,
  "storageAccount": null,
  "tags": {},
  "type": "Microsoft.ContainerRegistry/registries"
}
```

### Push the image to Azure Container Registry

To push an image to an Azure container registry (ACR), you must first have a container image. If you don't yet have any local container images, run the following command to pull the an image from Docker Hub (you may need to switch Docker to work with Linux images by right-clicking the docker icon and selecting **Switch to Linux containers**).

If you are deploying Windows application, please use Windows image `seabreeze/azure-mesh-helloworld:1.1-windowsservercore-1709`.

```bash
docker pull seabreeze/azure-mesh-helloworld:1.1-alpine
```

Before you can push an image to your registry, you must tag it with the fully qualified name of your ACR login server.

Run the following command to get the full login server name of your ACR instance.

```azurecli
az acr list --resource-group myResourceGroup --query "[].{acrLoginServer:loginServer}" --output table
```

The full login server name that is returned will be referred to as `<acrLoginServer>` throughout the rest of this article.

Now tag your docker image using the `docker tag` command. In the command below, replace `<acrLoginServer>` with the login server name reported by the command above. The following example tags the seabreeze/azure-mesh-helloworld:1.1-alpine image. If you are using a different image, substitute the image name in the following command.

```bash
docker tag seabreeze/azure-mesh-helloworld:1.1-alpine <acrLoginServer>/seabreeze/azure-mesh-helloworld:1.1-alpine
```

For example: `docker tag seabreeze/azure-mesh-helloworld:1.1-alpine myContainerRegistry007.azurecr.io/seabreeze/azure-mesh-helloworld:1.1-alpine`

Log in to the Azure Container Registry.

```bash
az acr login -n <acrName>
```

For example: `az acr login -n myContainerRegistry007`

Push the image to the azure container registry with the following command:

```bash
docker push <acrLoginServer>/seabreeze/azure-mesh-helloworld:1.1-alpine
```

For example: `docker push myContainerRegistry007.azurecr.io/seabreeze/azure-mesh-helloworld:1.1-alpine`

### List container images

The following example lists the repositories in a registry. The examples that follow assume you are using the azure-mesh-helloworld:1.1-alpine image. If you are using a different image, substitute its name where the azure-mesh-helloworld image is used.

```azurecli
az acr repository list --name <acrName> --output table
```
For example: `az acr repository list --name myContainerRegistry007 --output table`

Output:

```bash
Result
-------------------------------
seabreeze/azure-mesh-helloworld
```

The following example lists the tags on the **azure-mesh-helloworld** repository.

```azurecli
az acr repository show-tags --name <acrName> --repository seabreeze/azure-mesh-helloworld --output table
```

For example: `az acr repository show-tags --name myContainerRegistry007 --repository seabreeze/azure-mesh-helloworld --output table`

Output:

```bash
Result
--------
1.1-alpine
```

The preceding output confirms the presence of `azure-mesh-helloworld:1.1-alpine` in the private container registry. .

## Retrieve credentials for the registry

> [!IMPORTANT]
> Enabling the admin user on an Azure container registry is not recommended for production scenarios. It is done here to keep this demonstration brief. For production scenarios, use a [service principal](https://docs.microsoft.com/azure/container-registry/container-registry-auth-service-principal) for both user and system authentication in production scenarios.

In order to deploy a container instance from the registry that was created, you must provide credentials during the deployment. Enable the admin user on your registry with the following command:

```azurecli-interactive
az acr update --name <acrName> --admin-enabled true
```

For example: `az acr update --name myContainerRegistry007 --admin-enabled true`

Get the registry server name, user name, and password by using the following commands:

```azurecli-interactive
az acr list --resource-group myResourceGroup --query "[].{acrLoginServer:loginServer}" --output table
az acr credential show --name <acrName> --query username
az acr credential show --name <acrName> --query "passwords[0].value"
```

The values provided by preceding commands are referenced as `<acrLoginServer>`, `<acrUserName>`, and `<acrPassword>` below.

## Deploy the application

Create the application and related resources in the resource group using the `az mesh deployment create` command, and provide the credentials from the previous step. The following will deploy the Hello World Linux application using the [helloworld.private_registry.linux.json template](https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/helloworld/helloworld.private_registry.linux.json). If you want to deploy a Windows application instead, use the [helloworld.private_registry.windows.json template](https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/helloworld/helloworld.private_registry.windows.json) instead. Windows container images are larger than Linux container images and may take more time to deploy.

If you're using a Bash console or Windows Command Prompt, run the following:

```azurecli-interactive
az mesh deployment create --resource-group myResourceGroup --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/helloworld/helloworld.private_registry.linux.json --parameters "{\"location\": {\"value\": \"eastus\"}, \"registry-server\": {\"value\": \"<acrLoginServer>\"}, \"registry-username\": {\"value\": \"<acrUserName>\"}, \"registry-password\": {\"value\": \"<acrPassword>\"}}"
```

If you're using a PowerShell console, run the following:

```azurecli-interactive
az mesh deployment create --resource-group myResourceGroup --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/helloworld/helloworld.private_registry.windows.json --parameters "{'location': {'value': 'eastus'}, 'registry-server': {'value': '<acrLoginServer>'}, 'registry-username': {'value': '<acrUserName>'}, 'registry-password': {'value': '<acrPassword>'}}"
```

In a few minutes, if the deployment is successful the command will return the list of created resources and an output property for the public IP address.

## See the app in action 

### Open the application
Once the application successfully deploys, copy the public IP address from the `publicIPAddress` output property. Open the IP address in a web browser. A web page with the Azure Service Fabric Mesh logo displays.

You can also obtain the public IP address from the details of the `helloWorldPrivateRegistryGateway` resource using the following command. The name of the gateway for Windows application is `helloWorldPrivateRegistryGatewayWindows`.

```azurecli
az mesh gateway show -g myResourceGroup -n helloWorldPrivateRegistryGateway -o table
```

### Check the application details
You can check the application's status using the `az mesh app show` command. This command provides useful information that you can follow up on.

The application name for the Linux app is `helloWorldPrivateRegistryApp` and for Windows app is `helloWorldPrivateRegistryAppWindows`, to gather the details on the application execute the following command:

```azurecli
az mesh app show --resource-group myResourceGroup --name helloWorldPrivateRegistryApp
```

## Clean up

When you are ready to delete the application, run the [az group delete][az-group-delete] command to remove the resource group and the application and network resources it contains.

```azurecli
az group delete --name myResourceGroup
```