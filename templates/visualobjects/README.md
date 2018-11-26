# Deploy the Visual Objects application to Azure Service Fabric Mesh

This dir contains the deployment templates for the Visual Object sample based on pre-built images that we create. The Visual Objects sample app shows you how to deploy, scale, and upgrade a Mesh application. This app consists of two services, a `worker` service which is responsible for tracking an object that moves, and a `web` service that renders the object(s) in a UI.  

The deployments steps will guide you through deploying the `base` version of the app, which will deploy a single "visual object" you will see rendered in the web UI. We will then scale up the worker service to 3 instances, thus creating 3 objects that will show up individually in the web UI. The last step will be to upgrade the application (which changes the way each worker is represented in the UI).  

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

## Deploy the base version of the application

Create the application and related resources in the resource group using the `az mesh deployment create` command. The following will deploy the Visual Objects Linux application using the [mesh_rp.base.linux.json template](https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/visualobjects/mesh_rp.base.linux.json). If you want to deploy a Windows application instead, use the [mesh_rp.base.windows.json template](https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/visualobjects/mesh_rp.base.windows.json) instead. Windows container images are larger than Linux container images and may take more time to deploy.

Deploy Visual Object using the following steps:

```azurecli
az mesh deployment create --resource-group myResourceGroup --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/visualobjects/mesh_rp.base.linux.json --parameters "{\"location\": {\"value\": \"eastus\"}}" 
```

## See it in action 

#### Open the application

Once the application successfully deploys, copy the public IP address from the `publicIPAddress` output property. Open up `<IP address>:8080` in your web browser to see the UI displayed. 

You can also obtain the public IP address from the details of the `visualObjectsGateway` resource using the following command.

```azurecli
az mesh gateway show -g myResourceGroup -n visualObjectsGateway -o table
```

You should see one triangle flying around, representing the single instance worker service that was deployed. Leave this page open as you progress through the scale out and upgrade steps to see the changes!


## Scale the application

The next step here is to scale the worker service up to 3 instances. 3 is the current limit imposed for service instances while Mesh is still in private previw - see  [Mesh FAQ](https://docs.microsoft.com/azure/service-fabric-mesh/service-fabric-mesh-faq) for updated information on resource limits for Mesh. 

For this step, we will be using the [mesh_rp.scaleout.linux.json template](https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/visualobjects/mesh_rp.scaleout.linux.json). If you chose to deploy the sample as a Windows application in the prior step, use the [mesh_rp.scaleout.windows.json template](https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/visualobjects/mesh_rp.scaleout.windows.json) in the following command instead. 

```azurecli
az mesh deployment create --resource-group myResourceGroup --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/visualobjects/mesh_rp.scaleout.linux.json --parameters "{\"location\": {\"value\": \"eastus\"}}" 
```

There is only one difference between the template used in this step and the one prior (scalout vs. base) - where the `worker` service is being described in the JSON, you will see `"replicaCount"` set to 3 instead of 1. 

In a few minutes, your web service should update and be rendering 3 triangles instead! Cool!

## Upgrade the application

We're now going to upgrade the same `worker` service to use a "new" image. Previously, the worker service replicas were using the seabreeze/azure-mesh-visualobjects-worker:1.1-stretch image, but with this deployment, we will be using a template that uses the seabreeze/azure-mesh-visualobjects-worker:1.1-rotate-stretch image. The template being used to upgrade the app is the [mesh_rp.upgrade.linux.json template](https://github.com/Azure-Samples/service-fabric-mesh/blob/2018-09-01-preview/templates/visualobjects/mesh_rp.upgrade.linux.json). If you chose to deploy the sample as a Windows application in the prior steps, use the [mesh_rp.upgrade.windows.json template](https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/2018-09-01-preview/templates/visualobjects/mesh_rp.upgrade.windows.json) in the following command instead. 

```azurecli
az mesh deployment create --resource-group myResourceGroup --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/master/templates/visualobjects/mesh_rp.upgrade.linux.json --parameters "{\"location\": {\"value\": \"eastus\"}}"
```

The difference between this template and the one previously deployed is in the container image being use in the code package for the `worker` service. Another thing to point out here is that though both the scale out and the upgrade were just deployments of an updated template on the same application resource, they did result in two different types of changes - the former is more of a config change since the code packages being deployed are not change and only the request number of replicas changed, whereas the latter results in a full rolling upgrade for the applciation, where the container images are updated for a specific code package in a service.  

Back in the web UI, as the replicas start to get upgraded, their respective triangles will start rotating.

## Clean up

When you are ready to delete the application, run the [az group delete][az-group-delete] command to remove the resource group and the application and network resources it contains.

```azurecli
az group delete --name myResourceGroup
```