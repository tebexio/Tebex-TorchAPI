#!/usr/bin/env bash

# Uses msbuild to compile the release version of the plugin. A distributable zip will be placed in your working dir.
VERSION="2.0.1"
workingDir=$(pwd)
msbuild ./Tebex-TorchAPI/Tebex-TorchAPI.sln -p:Configuration=Release
cd ./Tebex-TorchAPI/bin/x64/Release
mv TebexTorchAPI.dll Tebex-TorchAPI-${VERSION}.dll
zip "Tebex-TorchAPI-${VERSION}.zip" Tebex-TorchAPI-${VERSION}.dll
mv "Tebex-TorchAPI-${VERSION}.zip" "${workingDir}"
cd $workingDir