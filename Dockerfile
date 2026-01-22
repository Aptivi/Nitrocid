FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /NKS

# Initialize the project files
COPY . ./

# Attempt to build Nitrocid KS
RUN dotnet build "Nitrocid.slnx" -p:Configuration=Release

# Run the ASP.NET image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /NKS

# Copy the output files and start Nitrocid KS
COPY --from=build-env /NKS/public/Nitrocid/KSBuild/net10.0 .
ENTRYPOINT ["dotnet", "Nitrocid.dll"]
