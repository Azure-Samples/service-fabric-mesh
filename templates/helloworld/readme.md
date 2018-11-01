# Deploy Hello World application to Azure Service Fabric Mesh

This article shows how to deploy a simple application to Azure Service Fabric Mesh that uses the container images from a public registry, to deploy application from a private registry, see **[Deploying from a private registry](readme.private-registry.md)**.

## Set up Service Fabric Mesh CLI

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

## Deploy the application

Create the application and related resources in the resource group using the `az mesh deployment create` command. The following will deploy the Hello World Linux application using the [helloworld.linux.json template](https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/helloworld/helloworld.linux.json). If you want to deploy a Windows application instead, use the [helloworld.windows.json template](https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/helloworld/helloworld.linux.json) instead. Windows container images are larger than Linux container images and may take more time to deploy.

*Note: both these templates reference a container image in a public registry. If you would like to learn how to deploy an application that references container images from a private registry, see [Deploying from a private registry](readme.private-registry.md)*

If you're using a Bash console or Windows Command Prompt, run the following:


```azurecli
az mesh deployment create --resource-group myResourceGroup --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/helloworld/helloworld.linux.json --parameters "{\"location\": {\"value\": \"eastus\"}}" 
```

If you're using a PowerShell console, run the following:

```azurecli
az mesh deployment create --resource-group myResourceGroup --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/helloworld/helloworld.linux.json --parameters "{'location': {'value': 'eastus'}}"
```
In a few minutes, if the deployment is successful the command will return the list of created resources and an output property for the public IP address.

## See the app in action 

### Open the application
Once the application successfully deploys, copy the public IP address from the `publicIPAddress` output property. Open the IP address in a web browser. A web page with the Azure Service Fabric Mesh logo displays.

You can also obtain the public IP address from the details of the `helloWorldGateway` resource using the following command. The name of the gateway for Windows application is `helloWorldGatewayWindows`.

```azurecli
az mesh gateway show -g myResourceGroup -n helloWorldGateway -o table
```

### Check the application details
You can check the application's status using the `az mesh app show` command. This command provides useful information that you can follow up on.

The application name for the Linux app is `helloWorldApp` and for Windows app is `helloWorldAppWindows`, to gather the details on the application execute the following command:

```azurecli
az mesh app show --resource-group myResourceGroup --name helloWorldApp
```

## Clean up

When you are ready to delete the application, run the [az group delete][az-group-delete] command to remove the resource group and the application and network resources it contains.

```azurecli
az group delete --name myResourceGroup
```