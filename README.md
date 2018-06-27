# service-fabric-mesh-demos

This repository contains samples that showcase various features of Service Fabric Mesh.

## Overview

The table below provides an overview of the samples in this repository.

| Sample Name  | Description  |
|:-----------:|-------------|
| azurefiles-volume | This sample illustrates the use of volumes in Mesh containers. The container requests a volume, that is backed by a specific Azure Files file share, to be mounted to a specific location within the container. The application that runs inside the container writes a text file to this location. |
| BasicServiceFabricMeshApp | Create and publish an Azure Service Fabric app to Azure that has an ASP.NET web front end and an ASP.NET Core Web API back-end service. Demonstrates a service-to-service call in a Service Fabric Mesh app. |
| helloworld | Service Fabric Mesh makes it easy to create and manage Docker containers in Azure, without having to provision virtual machines. This sample creates a container in Azure and exposes it to the internet. |
| templates | This folder containers ARM templates to deploy container images containing some of the code samples in this repository, as well as showing how to use private registries and volumes. This provides an easy way to try and deploy the applications, without having to build the applications and container images. |
| visualobjects | Visual Object sample that show scaleout and upgrade |
| votingapp | Shows how to deploy a .NET Core application to Service Fabric Mesh using a template. When you're finished, you have a voting application with an ASP.NET Core web front end that saves voting results in a stateful back-end service in the cluster. |