#!/bin/bash

# Restore packages
echo "- Restoring packages..."
echo "  - HOME=`pwd`/nuget dotnet restore Nitrocid.sln"
HOME=`pwd`/nuget dotnet restore Nitrocid.sln
if [ "$?" -ne 0 ]; then
	exit $?
fi

# Copy dependencies to deps
echo "- Copying dependencies to deps..."
echo "  - mkdir deps"
mkdir deps
echo "  - cp -R ./nuget/.nuget/packages/* ./deps/"
cp -R ./nuget/.nuget/packages/* ./deps/

# Copy NuGet.config for offline use
echo "- Copying NuGet.config..."
echo "  - cp tools/OfflineNuGet.config ./NuGet.config"
cp tools/OfflineNuGet.config ./NuGet.config

echo "- You should be able to build offline!"

