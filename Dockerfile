# escape=`
# ──────────────────────────────────────────────────────────────────────────────
# Tebex-TorchAPI build image
#
# Requires Windows containers (not Linux containers).
# On Docker Desktop: Settings → "Switch to Windows containers..."
# On a Windows host/CI: docker buildx build --platform windows/amd64 .
#
# Build:
#   docker build -t tebex-torch-build .
#
# Extract the DLL after building:
#   docker create --name torch-build tebex-torch-build
#   docker cp torch-build:C:\output\TebexTorchAPI.dll .
#   docker rm torch-build
# ──────────────────────────────────────────────────────────────────────────────

FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2022 AS build

WORKDIR C:\build

# Copy solution and dependencies
COPY Tebex-TorchAPI\ Tebex-TorchAPI\
COPY Lib\ Tebex-TorchAPI\Lib\

# Compile Release|x64 (no NuGet restore needed — all refs are local DLLs)
RUN msbuild Tebex-TorchAPI\Tebex-TorchAPI.sln `
        /p:Configuration=Release `
        /p:Platform=x64 `
        /nologo `
        /verbosity:minimal

# ── Output stage: strip the SDK layer, keep only the compiled DLL ─────────────
FROM mcr.microsoft.com/windows/servercore:ltsc2022

WORKDIR C:\output

COPY --from=build C:\build\Tebex-TorchAPI\bin\x64\Release\TebexTorchAPI.dll .
