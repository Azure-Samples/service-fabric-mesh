* Change the container image. Default image is `seabreeze/azure-mesh-counter:0.5-alpine`.
* Deploy using :
```
cd "App Resources"

az group create --name testrg1 --location eastus && az mesh deployment create --resource-
group testrg1 --input-yaml-files .\app.yaml,.\network.yaml,.\gateway.yaml  --parameters "{'location' : {'value' : 'eastus'}}"
```