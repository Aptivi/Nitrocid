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

# This script builds.
releaseconf=$1
if [ -z $releaseconf ]; then
	releaseconf=Release
fi

# Check for dependencies
dotnetpath=`which dotnet`
checkerror $? "dotnet is not found"

# Turn off telemetry and logo
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

# Download packages
echo Downloading packages...
"$dotnetpath" restore "$ROOTDIR/Nitrocid.sln" -p:Configuration=$releaseconf ${@:2}
checkerror $? "Failed to download packages"

# Build
echo Building...
"$dotnetpath" build "$ROOTDIR/Nitrocid.sln" -p:Configuration=$releaseconf ${@:2}
checkerror $? "Failed to build"

# Inform success
echo Build successful.
