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

# This script builds the documentation and packs the artifacts.
version=$(grep "<Version>" $ROOTDIR/Directory.Build.props | cut -d "<" -f 2 | cut -d ">" -f 2)
checkerror $? "Failed to get version. Check to make sure that the version is specified correctly in D.B.props"

# Check for dependencies
zippath=`which zip`
checkerror $? "zip is not found"

# Pack documentation
echo Packing documentation...
cd "$ROOTDIR/docs/" && "$zippath" -r /tmp/$version-doc.zip . && cd -
checkerror $? "Failed to pack"

# Inform success
rm -rf "$ROOTDIR/DocGen/api"
checkerror $? "Failed to remove api folder"
rm -rf "$ROOTDIR/DocGen/obj"
checkerror $? "Failed to remove obj folder"
rm -rf "$ROOTDIR/docs"
checkerror $? "Failed to remove docs folder"
mv /tmp/$version-doc.zip "$ROOTDIR/tools"
checkerror $? "Failed to move archive from temporary folder"
echo Pack successful.
exit 0
