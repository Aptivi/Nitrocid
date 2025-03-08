#!/bin/bash

# Repository root
ROOTDIR=$( cd -- "$( dirname -- "$0" )/.." &> /dev/null && pwd )

# Check for dependencies
msbuildpath=`which docfx`
if [ ! $? == 0 ]; then
	echo DocFX is not found.
	exit 1
fi

# Turn off telemetry and logo
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

# Build docs
echo Building documentation...
docfx $ROOTDIR/DocGen/docfx.json
if [ ! $? == 0 ]; then
	echo Build failed.
	exit 1
fi

# Inform success
echo Build successful.
exit 0
