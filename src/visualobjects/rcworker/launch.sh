#!/bin/bash
export LD_LIBRARY_PATH=$LD_LIBRARY_PATH:.:$FabricCodePath
dotnet ./rcworker.dll 
