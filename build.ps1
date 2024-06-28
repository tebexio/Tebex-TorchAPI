# Uses dotnet to compile the release version of the plugin. A distributable zip will be placed in your working dir.
$version = "2.0.0"
$workingDir = Get-Location

# Compile the solution using dotnet and check for success
$buildToolPath = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\amd64\MSBuild.exe"
$buildResult = & $buildToolPath .\Tebex-TorchAPI\Tebex-TorchAPI.sln -p:Configuration=Release

if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet build failed with exit code $LASTEXITCODE"
    Write-Error $buildResult
    exit $LASTEXITCODE
}

# Change to the release directory
Set-Location -Path .\Tebex-TorchAPI\bin\x64\Release

# Rename the DLL file
if (Test-Path -Path "Tebex-TorchAPI-$version.dll") {
    Remove-Item -Path "Tebex-TorchAPI-$version.dll"
}

Rename-Item -Path "TebexTorchAPI.dll" -NewName "Tebex-TorchAPI-$version.dll"

# Remove existing zip file if it exists
$zipPath = "Tebex-TorchAPI-$version.zip"
if (Test-Path -Path $zipPath) {
    Remove-Item -Path $zipPath
}

# Create a new zip file
Compress-Archive -Path "Tebex-TorchAPI-$version.dll" -DestinationPath $zipPath

# Move the zip file to the original working directory
Move-Item -Path $zipPath -Destination $workingDir

# Return to the original working directory
Set-Location -Path $workingDir

# Write a success message
Write-Output "Build and packaging completed successfully. The distributable zip is located at $workingDir\Tebex-TorchAPI-$version.zip"