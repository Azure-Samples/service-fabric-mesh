# Deploy mesh sample application with an ingress gateway to Azure Service Fabric Mesh

This article shows how to deploy an ingress gateway to route requests to multiple backend services.
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

Create the application and related resources in the resource group using the `az mesh deployment create` command. The following will deploy the mesh sample application using the [meshingress.linux.json template](https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/ingress/meshingress.linux.json). If you want to deploy a Windows application instead, use the [meshingress.windows.json template](https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/ingress/meshingress.linux.json) instead. Windows container images are larger than Linux container images and may take more time to deploy.

If you're using a Bash console or Windows Command Prompt, run the following:


```azurecli
az mesh deployment create --resource-group myResourceGroup --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/ingress/meshingress.linux.json --parameters "{\"location\": {\"value\": \"eastus\"}}" 
```

If you're using a PowerShell console, run the following:

```PowerShell
az mesh deployment create --resource-group myResourceGroup --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/ingress/meshingress.linux.json --parameters "{'location': {'value': 'eastus'}}"
```
In a few minutes, if the deployment is successful the command will return the list of created resources and an output property for the public IP address.

## See the app in action 

This application has two services, helloWorldService and counterService. The ingress gateway is configured to perform path based routing to these services:
1. Incoming requests to PublicIPAddress:Port/hello will be routed to helloWorldService.
2. Incoming requests to PublicIPAddress:Port/counter will be routed to counterService.

### Open the application
Once the application successfully deploys, copy the public IP address from the `publicIPAddress` output property. 
Open the PublicIPAddress/hello in a web browser. A web page with the Azure Service Fabric Mesh logo displays.
Open the PublicIPAddress/counter in a web browser. It will display a web page with the counter value being updated every second.

You can also obtain the public IP address from the details of the `ingressGatewayLinux` resource using the following command. The name of the gateway for Windows application is `ingressGatewayWindows`.

```azurecli
az mesh gateway show -g myResourceGroup -n ingressGatewayLinux -o table
```

### Check the application details
You can check the application's status using the `az mesh app show` command. This command provides useful information that you can follow up on.

The application name for the Linux app is `meshAppLinux` and for Windows app is `meshAppWindows`, to gather the details on the application execute the following command:

```azurecli
az mesh app show --resource-group myResourceGroup --name meshAppLinux
```

##Samples to demonstrate different ways to route requests to endpoints
The previous sample demonstrated how to route a request to different services using information in the URI of the requests. Following samples demonstrate routing requests using other configuration settings.
While each sample uses a specific config setting to demonstrate the capabilities of the gateway, these configs can be combined to create more complex routing rules. 

### Ingress listening on different ports:
This sample demonstrates how to route requests coming on different ports to different services. 
The meshingress.ports.windows.json and meshingress.ports.linux.json configures two different http rules for the gateway.

```azurecli
az mesh deployment create --resource-group myResourceGroup --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/master/templates/ingress/meshingress.ports.linux.json --parameters "{\"location\": {\"value\": \"eastus\"}}" 
```

* http://PublicIPAddress/ forwards the request to gateway on port 80. This is routed to the helloWorldService.
* http://PublicIPAddress:8080/ forwards the request to gateway on port 8080. This is routed to the counterService.

### Ingress routing based on headers:
This sample demonstrates routing messages to different services based on the value of a specific header in the request.

```azurecli
az mesh deployment create --resource-group myResourceGroup --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/master/templates/ingress/meshingress.headers.linux.json --parameters "{\"location\": {\"value\": \"eastus\"}}" 
```

* http://PublicIPAddress/ forwards the request to helloWorldService if the header serviceNameHeader=helloWorld is present in the incoming request.
* http://PublicIPAddress/ forwards the request to counterService based on the presence of header serviceNameHeader=counter.

## Clean up

When you are ready to delete the application, run the [az group delete][az-group-delete] command to remove the resource group and the application and network resources it contains.

```azurecli
az group delete --name myResourceGroup
```