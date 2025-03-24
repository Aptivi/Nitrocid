#!/bin/bash

# Repository root
ROOTDIR=$( cd -- "$( dirname -- "$0" )/.." &> /dev/null && pwd )

# Convenience functions
checkerror() {
    if [ $1 != 0 ]
    then
        printf "$2 - Error $1\n" >&2
        exit $1
    fi
}

# Turn off telemetry and logo
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

# Restore packages
echo "- Restoring packages..."
echo "  - HOME=$ROOTDIR/nuget dotnet restore $ROOTDIR/Nitrocid.sln"
HOME=$ROOTDIR/nuget dotnet restore $ROOTDIR/Nitrocid.sln
checkerror $? "  - Failed to restore NuGet packages"

# Copy dependencies to deps
echo "- Copying dependencies to deps..."
echo "  - mkdir $ROOTDIR/deps"
mkdir $ROOTDIR/deps
checkerror $? "  - Failed to mkdir deps"
echo "  - cp $ROOTDIR/nuget/.nuget/packages/*/*/*.nupkg $ROOTDIR/deps/"
cp $ROOTDIR/nuget/.nuget/packages/*/*/*.nupkg $ROOTDIR/deps/
checkerror $? "  - Failed to copy deps"
echo "  - rm -rf $ROOTDIR/nuget"
rm -rf $ROOTDIR/nuget
checkerror $? "  - Failed to remove nuget folder"

# Copy NuGet.config for offline use
echo "- Copying NuGet.config..."
echo "  - cp $ROOTDIR/vnd/OfflineNuGet.config $ROOTDIR/NuGet.config"
cp $ROOTDIR/vnd/OfflineNuGet.config $ROOTDIR/NuGet.config
checkerror $? "  - Failed to copy offline NuGet config"

echo "- You should be able to build offline!"

