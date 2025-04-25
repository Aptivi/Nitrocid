FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /NKS

# Initialize the project files
COPY . ./

# Remove this when Terminaux 7.0 prep stage is done on August 10th
RUN --mount=type=secret,id=github_token,env=GITHUB_TOKEN dotnet nuget add source --username AptiviCEO --password $(cat /run/secrets/github_token) --store-password-in-clear-text --name github "https://nuget.pkg.github.com/Aptivi/index.json"

# Attempt to build Nitrocid KS
RUN dotnet build "Nitrocid.sln" -p:Configuration=Release

# Run the ASP.NET image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /NKS

# Copy the output files and start Nitrocid KS
COPY --from=build-env /NKS/public/Nitrocid/KSBuild/net8.0 .
ENTRYPOINT ["dotnet", "Nitrocid.dll"]
