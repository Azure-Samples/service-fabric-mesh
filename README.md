# service-fabric-mesh-samples

This repository contains samples that showcase various features of Service Fabric Mesh.

## Overview

The table below provides an overview of the samples in this repository.

| Sample Name  | Description  |
|:-----------:|-------------|
| [azurefiles-volume](./azurefiles-volume) | This sample illustrates the use of volumes in Mesh containers. The container requests a volume, that is backed by a specific Azure Files file share, to be mounted to a specific location within the container. The application that runs inside the container writes a text file to this location. |
| [basicservicefabricmeshapp](./basicservicefabricmeshapp) | Create and publish an Azure Service Fabric app to Azure that has an ASP.NET web front end and an ASP.NET Core Web API back-end service. Demonstrates a service-to-service call in a Service Fabric Mesh app. |
| [helloworld](./helloworld) | Demonstrates how to deploy a single container microservices application and connect to the service endpoint. |
| [templates](./templates) | This folder containers ARM templates to deploy container images containing some of the code samples in this repository, as well as showing how to use private registries and volumes. This provides an easy way to try and deploy the applications, without having to build the applications and container images. |
| [visualobjects](./visualobjects) | Shows how to independently scale microservices within an application. |
| [votingapp](./votingapp) | Shows how to deploy a .NET Core application to Service Fabric Mesh using a template. When you're finished, you have a voting application with an ASP.NET Core web front end that saves voting results in a stateful back-end service in the cluster. |