# Service Fabric Mesh Sample Application - Voting Application with Reliable Collections

This folder contains the deployment templates for the Voting Application with Reliable Collections sample. This sample shows you how to deploy this mesh application.

You will need the Azure CLI and the Mesh extension to deploy this sample. Follow this link for instructions on how to install the [Azure CLI and Mesh extension](https://docs.microsoft.com/en-us/azure/service-fabric-mesh/service-fabric-mesh-howto-setup-cli)

Deploy Visual Object using the following steps:

1. Login to you Azure account - ``az login``

1. Create a resource group - ``az group create --name <resource group name> --location eastus``

1. Deploy the base template - ``az mesh deployment create --resource-group <resource group name> --template-uri https://raw.githubusercontent.com/Azure-Samples/service-fabric-mesh/master/templates/rcvoting/mesh_rp_rc.linux.json --parameters "{\"location\": {\"value\": \"eastus\"}}"`` Once the deployment is complete, you can navigate to the application IP to see Voting Application Frontend.


[The source code for the sample can be found here](../../src/rcvotingapp)