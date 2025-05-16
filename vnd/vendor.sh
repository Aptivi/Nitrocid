#!/bin/bash

localize() {
    # Check for dependencies
    dotnetpath=`which dotnet`
    checkerror $? "dotnet is not found"

    # Turn off telemetry and logo
    export DOTNET_CLI_TELEMETRY_OPTOUT=1
    export DOTNET_NOLOGO=1

    # Restore the packages
    echo "Restoring NuGet packages..."
    "$dotnetpath" restore "$ROOTDIR/Nitrocid.sln" --packages "$ROOTDIR/nuget"
    checkerror $? "Failed to restore NuGet packages"

    # Copy dependencies to the "deps" folder underneath the root directory
    mkdir -p "$ROOTDIR/deps"
    checkerror $? "Failed to initialize the dependencies folder"
    cp "$HOME/.nuget/packages"/*/*/*.nupkg "$ROOTDIR/deps/"
    cp "$ROOTDIR/nuget"/*/*/*.nupkg "$ROOTDIR/deps/"
    checkerror $? "Failed to vendor dependencies"
    rm -rf "$ROOTDIR/nuget"
    checkerror $? "Failed to wipe cache"

    # Initialize the NuGet configuration
    cat > "$ROOTDIR/NuGet.config" << EOF
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="./deps" />
  </packageSources>
</configuration>
EOF
    checkerror $? "Failed to generate offline NuGet config"
}

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
    packages=()
    while IFS= read -r pkg; do
        packages+=("$pkg")
    done < <(find "$ROOTDIR/public/Nitrocid/KS*/" -maxdepth 1 -type f -name "*.nupkg")
    for pkg in "${packages[@]}"; do
        echo "$pkg"
        dotnet nuget push "$pkg" --api-key "$NUGET_APIKEY" --source "$nugetsource"
        push_result=$?
        if [ $push_result -ne 0 ]; then
            checkvendorerror $push_result
            return $push_result
        fi
    done
    
    checkvendorerror $?
}

clean() {
    OUTPUTS=(
        '-name "bin" -or'
        '-name "obj" -or'
        '-name "KSBuild" -or'
        '-name "KSAnalyzer" -or'
        '-name "KSTest" -or'
        '-name "nitrocid-27" -or'
        '-name "nitrocid-27-lite" -or'
        '-name "tmp" -or'
        '-name "docs"'
    )
    find "$ROOTDIR" -type d \( ${OUTPUTS[@]} \) -print -exec rm -rf "{}" +
}
