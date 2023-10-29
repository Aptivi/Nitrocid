#!/bin/bash

# This script builds KS and packs the artifacts. Use when you have MSBuild installed.
ksversion=$(cat version)

# Check for dependencies
zippath=`which zip`
if [ ! $? == 0 ]; then
	echo zip is not found.
	exit 1
fi

# Pack documentation
echo Packing documentation...
cd "../docs/" && "$zippath" -r /tmp/$ksversion-doc.zip . && cd -
if [ ! $? == 0 ]; then
	echo Packing failed.
	exit 1
fi

# Inform success
rm -rf "../DocGen/api"
rm -rf "../DocGen/obj"
rm -rf "../docs"
mv /tmp/$ksversion-doc.zip .
echo Pack successful.
exit 0
