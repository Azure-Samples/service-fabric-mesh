# Hello World App

This sample covers deploying a simple single container application as a Windows or Linux based app. The app can be deployed using a container image that is from a public or a private container registry: 
* Public container registry - dockerhub
* Private container registry - Azure Container Registry

## Set up

These steps assume that you have an Azure account set up, for which you will open up the Portal and use Cloud Shell with the Service Fabric Mesh extension installed. If you don't have this set up:
* [Create a free Azure account before you begin](https://azure.microsoft.com/free/)
* [Set up the Service Fabric Mesh CLI](https://docs.microsoft.com/azure/service-fabric-mesh/service-fabric-mesh-quickstart-deploy-container)

### Open up Cloud Shell
Log into Azure Portal and open up a Cloud Shell instance. You can choose to use either Bash or PowerShell.

### Sign in to Azure
Sign in to Azure and set your subscription.

```azurecli
az login
az account set --subscription "<subscriptionID>"
```

### Create resource group
Create a resource group to deploy the application to. You can use an existing resource group and skip this step. 

```azurecli
az group create --name myResourceGroup --location eastus 
```

## Deploy the application

### Deploy from public registry 

Create your application in the resource group using the `az mesh deployment create` command. The following will deploy the Hello World App as a Linux application using the [mesh_rp.linux.json template](https://sfmeshsamples.blob.core.windows.net/templates/helloworld/mesh_rp.linux.json). If you want to deploy a Windows application instead, use the [mesh_rp.windows.json template](https://sfmeshsamples.blob.core.windows.net/templates/helloworld/mesh_rp.windows.json) instead. Windows container images are larger than Linux container images and may take more time to deploy.

If you're using a Bash console in Cloud Shell, run the following:

```azurecli
az mesh deployment create --resource-group myResourceGroup --template-uri https://sfmeshsamples.blob.core.windows.net/templates/helloworld/mesh_rp.linux.json --parameters "{\"location\": {\"value\": \"eastus\"}}" 
```

If you're using a PowerShell console, run the following:

```azurecli
az mesh deployment create --resource-group myResourceGroup --template-uri https://sfmeshsamples.blob.core.windows.net/templates/helloworld/mesh_rp.linux.json --parameters "{'location': {'value': 'eastus'}}"
```

The preceding command deploys a Linux application using 

*Note: both these templates reference a container image in a public registry. If you would like to see a sample deployment from a private registry, see **Deploying from a private registry** below.*

In a few minutes, the command returns:

`helloWorldApp has been deployed successfully on helloWorldNetwork with public ip address <IP Address>` 

### Deploy from private registry

TODO: Why use a private registry, and main changes in the template. 

Create your application in the resource group using the `az mesh deployment create` command. The following will deploy the Hello World App as a Linux application using the [mesh_rp.linux.json template](https://sfmeshsamples.blob.core.windows.net/templates/helloworld/mesh_rp.linux.json). If you want to deploy a Windows application instead, use the [mesh_rp.windows.json template](https://sfmeshsamples.blob.core.windows.net/templates/helloworld/mesh_rp.windows.json) instead. Windows container images are larger than Linux container images and may take more time to deploy.

If you're using a Bash console in Cloud Shell, run the following:

```azurecli

```

If you're using a PowerShell console, run the following:

```azurecli

```

The preceding command deploys a Linux application using the [mesh_rp.linux.json template](https://sfmeshsamples.blob.core.windows.net/templates/helloworld/mesh_rp.linux.json). If you want to deploy a Windows application, use [mesh_rp.windows.json template](https://sfmeshsamples.blob.core.windows.net/templates/helloworld/mesh_rp.windows.json) instead. Windows container images are larger than Linux container images and may take more time to deploy.

In a few minutes, the command returns:

`helloWorldApp has been deployed successfully on helloWorldNetwork with public ip address <IP Address>` 

## See the app in action 

### Open the application
Once the application successfully deploys, copy the public IP address for the service endpoint from the CLI output. Open the IP address in a web browser. A web page with the Azure Service Fabric Mesh logo displays.

### Check the application details
You can check the application's status using the `az mesh app show` command. This command provides useful information that you can follow up on.

The application name for this quickstart is `helloWorldApp`, to gather the details on the application execute the following command:

```azurecli-interactive
az mesh app show --resource-group myResourceGroup --name helloWorldApp
```

## Clean up

When you are ready to delete the application, run the [az group delete][az-group-delete] command to remove the resource group and the application and network resources it contains.

```azurecli-interactive
az group delete --name myResourceGroup
```

