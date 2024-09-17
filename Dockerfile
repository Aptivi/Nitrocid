FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /NKS

# Initialize the project files
COPY . ./

# Attempt to build Nitrocid KS
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Run the ASP.NET image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /NKS

# Copy the output files and start Nitrocid KS
COPY --from=build-env /NKS/out .
ENTRYPOINT ["dotnet", "Nitrocid.dll"]
