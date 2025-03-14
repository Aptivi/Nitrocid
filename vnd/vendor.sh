#!/bin/bash

build() {
    # Check for dependencies
    dotnetpath=`which dotnet`
    checkerror $? "dotnet is not found"

    # Turn off telemetry and logo
    export DOTNET_CLI_TELEMETRY_OPTOUT=1
    export DOTNET_NOLOGO=1
    
    # Determine the release configuration
    releaseconf=$1
    if [ -z $releaseconf ]; then
	    releaseconf=Release
    fi

    # Now, build.
    echo Building with configuration $releaseconf...
    "$dotnetpath" build "$ROOTDIR/Nitrocid.sln" -p:Configuration=$releaseconf ${@:2}
    checkvendorerror $?
}

docpack() {
    # Get the project version
    version=$(grep "<Version>" $ROOTDIR/Directory.Build.props | cut -d "<" -f 2 | cut -d ">" -f 2)
    checkerror $? "Failed to get version. Check to make sure that the version is specified correctly in D.B.props"

    # Check for dependencies
    zippath=`which zip`
    checkerror $? "zip is not found"

    # Pack documentation
    echo Packing documentation...
    cd "$ROOTDIR/docs/" && "$zippath" -r /tmp/$version-doc.zip . && cd -
    checkvendorerror $?

    # Clean things up
    rm -rf "$ROOTDIR/DocGen/api"
    checkvendorerror $?
    rm -rf "$ROOTDIR/DocGen/obj"
    checkvendorerror $?
    rm -rf "$ROOTDIR/docs"
    checkvendorerror $?
    mv /tmp/$version-doc.zip "$ROOTDIR/tools"
    checkvendorerror $?
}

docgenerate() {
    # Check for dependencies
    docfxpath=`which docfx`
    checkerror $? "docfx is not found"

    # Turn off telemetry and logo
    export DOTNET_CLI_TELEMETRY_OPTOUT=1
    export DOTNET_NOLOGO=1

    # Build docs
    echo Building documentation...
    "$docfxpath" $ROOTDIR/DocGen/docfx.json
    checkvendorerror $?
}

packall() {
    # Get the project version
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
    mv /tmp/$ksversion-bin.zip $ROOTDIR/vnd
    checkerror $? "Failed to move archive from temporary folder"
    mv /tmp/$ksversion-bin-lite.zip $ROOTDIR/vnd
    checkerror $? "Failed to move archive from temporary folder"
    mv /tmp/$ksversion-addons.zip $ROOTDIR/vnd
    checkerror $? "Failed to move archive from temporary folder"
    mv /tmp/$ksversion-analyzers.zip $ROOTDIR/vnd
    checkerror $? "Failed to move archive from temporary folder"
    mv /tmp/$ksversion-mod-analyzer.zip $ROOTDIR/vnd
    checkerror $? "Failed to move archive from temporary folder"
    cp $ROOTDIR/vnd/changes.chg $ROOTDIR/vnd/$ksversion-changes.chg
    checkerror $? "Failed to copy changelogs"
}

pushall() {
    # This script pushes.
    releaseconf=$1
    if [ -z $releaseconf ]; then
	    releaseconf=Release
    fi
    nugetsource=$2
    if [ -z $nugetsource ]; then
	    nugetsource=nuget.org
    fi
    dotnetpath=`which dotnet`
    checkerror $? "dotnet is not found"

    # Push packages
    echo Pushing packages with configuration $releaseconf to $nugetsource...
    find $ROOTDIR/public/Nitrocid/KS*/ -maxdepth 1 -type f -name "*.nupkg" -exec sh -c "echo {} ; dotnet nuget push {} --api-key $NUGET_APIKEY --source \"$nugetsource\"" \;
    checkvendorerror $?
}
