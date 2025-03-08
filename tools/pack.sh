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

# This script packs the artifacts.
version=$(grep "<Version>" $ROOTDIR/Directory.Build.props | cut -d "<" -f 2 | cut -d ">" -f 2)
checkerror $? "Failed to get version. Check to make sure that the version is specified correctly in D.B.props"

# Check for dependencies
zippath=`which zip`
checkerror $? "zip is not found"
shapath=`which sha256sum`
checkerror $? "sha256sum is not found"

# Pack binary
echo Packing binary...
cd "$ROOTDIR/public/Nitrocid/KSBuild/net8.0/" && "$zippath" -r /tmp/$version-bin.zip . && cd -
checkerror $? "Failed to pack"
cd "$ROOTDIR/public/Nitrocid/KSBuild/net8.0/" && "$zippath" -r /tmp/$version-bin-lite.zip . -x "./Addons/*" && cd -
checkerror $? "Failed to pack"
cd "$ROOTDIR/public/Nitrocid/KSBuild/net8.0/Addons/" && "$zippath" -r /tmp/$version-addons.zip . && cd -
checkerror $? "Failed to pack"
cd "$ROOTDIR/public/Nitrocid/KSAnalyzer/netstandard2.0/" && "$zippath" -r /tmp/$version-analyzers.zip . && cd -
checkerror $? "Failed to pack"
cd "$ROOTDIR/public/Nitrocid/KSAnalyzer/net8.0/" && "$zippath" -r /tmp/$version-mod-analyzer.zip . && cd -
checkerror $? "Failed to pack"

# Inform success
mv /tmp/$ksversion-bin.zip $ROOTDIR/tools
checkerror $? "Failed to move archive from temporary folder"
mv /tmp/$ksversion-bin-lite.zip $ROOTDIR/tools
checkerror $? "Failed to move archive from temporary folder"
mv /tmp/$ksversion-addons.zip $ROOTDIR/tools
checkerror $? "Failed to move archive from temporary folder"
mv /tmp/$ksversion-analyzers.zip $ROOTDIR/tools
checkerror $? "Failed to move archive from temporary folder"
mv /tmp/$ksversion-mod-analyzer.zip $ROOTDIR/tools
checkerror $? "Failed to move archive from temporary folder"
cp $ROOTDIR/tools/changes.chg $ROOTDIR/tools/$ksversion-changes.chg
checkerror $? "Failed to copy changelogs"

echo Pack successful.
exit 0
