#!/bin/bash
# This script pushes. Use when you have dotnet installed.
dotnetpath=`which dotnet`
if [ ! $? == 0 ]; then
	echo dotnet is not found.
	exit 1
fi

# Push packages
echo Pushing packages...
dotnet nuget push "public/Nitrocid/KSBuild/*.nupkg" --api-key $NUGET_APIKEY --source "nuget.org"
if [ ! $? == 0 ]; then
	echo Push failed.
	exit 1
fi

# Inform success
echo Push successful.
exit 0
