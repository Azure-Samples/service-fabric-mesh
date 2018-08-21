#!/bin/bash
rgName=$1
location=$2

#az login
#az account set --subscription {your subscriptionID}

az group create --name $rgName --location $location
az mesh deployment create --resource-group $rgName --template-file ./Deployment/quickstart-linux.json