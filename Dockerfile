FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /NKS

# Initialize the project files
COPY . ./

# Remove this when Terminaux 8.1 gets released by Jan 2026
# RUN --mount=type=secret,id=github_token dotnet nuget add source --username AptiviCEO --password $(cat /run/secrets/github_token) --store-password-in-clear-text --name github "https://nuget.pkg.github.com/Aptivi/index.json"

# Attempt to build Nitrocid KS
RUN dotnet build "Nitrocid.slnx" -p:Configuration=Release

# Run the ASP.NET image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /NKS

# Copy the output files and start Nitrocid KS
COPY --from=build-env /NKS/public/Nitrocid/KSBuild/net10.0 .
ENTRYPOINT ["dotnet", "Nitrocid.dll"]
