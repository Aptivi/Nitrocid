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
    cd "$ROOTDIR/public/Nitrocid/KSBuild/net8.0/" && "$zippath" -r /tmp/$version-bin-lite.zip . -x "./Addons/*" -x "./Addons.Essentials/*" && cd -
    checkerror $? "Failed to pack"
    cd "$ROOTDIR/public/Nitrocid/KSBuild/net8.0/" && "$zippath" -r /tmp/$version-addons.zip Addons Addons.Essentials && cd -
    checkerror $? "Failed to pack"
    cd "$ROOTDIR/public/Nitrocid/KSAnalyzer/netstandard2.0/" && "$zippath" -r /tmp/$version-analyzers.zip . && cd -
    checkerror $? "Failed to pack"
    cd "$ROOTDIR/public/Nitrocid/KSAnalyzer/net8.0/" && "$zippath" -r /tmp/$version-mod-analyzer.zip . && cd -
    checkerror $? "Failed to pack"

    # Inform success
    mv /tmp/$version-bin.zip $ROOTDIR/vnd
    checkerror $? "Failed to move archive from temporary folder"
    mv /tmp/$version-bin-lite.zip $ROOTDIR/vnd
    checkerror $? "Failed to move archive from temporary folder"
    mv /tmp/$version-addons.zip $ROOTDIR/vnd
    checkerror $? "Failed to move archive from temporary folder"
    mv /tmp/$version-analyzers.zip $ROOTDIR/vnd
    checkerror $? "Failed to move archive from temporary folder"
    mv /tmp/$version-mod-analyzer.zip $ROOTDIR/vnd
    checkerror $? "Failed to move archive from temporary folder"
    cp $ROOTDIR/vnd/changes.chg $ROOTDIR/vnd/$version-changes.chg
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
    done < <(find "$ROOTDIR"/public/Nitrocid/KS* -maxdepth 1 -type f -name "*.nupkg")
    for pkg in "${packages[@]}"; do
        echo "$pkg"
        dotnet nuget push "$pkg" --api-key "$NUGET_APIKEY" --source "$nugetsource" --skip-duplicate
        push_result=$?
        if [ $push_result -ne 0 ]; then
            checkvendorerror $push_result
            return $push_result
        fi
    done
    
    checkvendorerror $?
}

increment() {
    # Check the versions
    OLDVER=$1
    NEWVER=$2
    OLDAPIVER=$3
    NEWAPIVER=$4
    if [ -z $OLDVER ]; then
        printf "Old version must be specified.\n"
        exit 1
    fi
    if [ -z $NEWVER ]; then
        printf "New version must be specified to replace old version $OLDVER.\n"
        exit 1
    fi
    if [ -z $OLDAPIVER ]; then
        printf "Old API version must be specified.\n"
        exit 1
    fi
    if [ -z $NEWAPIVER ]; then
        printf "New API version must be specified to replace old API version $NEWAPIVER.\n"
        exit 1
    fi

    # Populate some of the files needed to replace the old version with the new version
    FILES=(
        "$ROOTDIR/PKGBUILD-REL"
        "$ROOTDIR/PKGBUILD-REL-LITE"
        "$ROOTDIR/.github/workflows/build-ppa-package-with-lintian.yml"
        "$ROOTDIR/.github/workflows/build-ppa-package.yml"
        "$ROOTDIR/.github/workflows/pushamend.yml"
        "$ROOTDIR/.github/workflows/pushppa.yml"
        "$ROOTDIR/.gitlab/workflows/release.yml"
        "$ROOTDIR/public/Nitrocid.Installers/Nitrocid.Installer/Package.wxs"
        "$ROOTDIR/public/Nitrocid.Installers/Nitrocid.InstallerBundle/Bundle.wxs"
        "$ROOTDIR/public/Nitrocid.Templates/templates/KSMod/KSMod.csproj"
        "$ROOTDIR/public/Nitrocid.Templates/templates/KSModVB/KSModVB.vbproj"
        "$ROOTDIR/Directory.Build.props"
        "$ROOTDIR/CHANGES.TITLE"
    )
    IFS='.' read -ra NKSMODAPIVERSPLITOLD <<< "$OLDAPIVER"
    IFS='.' read -ra NKSMODAPIVERSPLITNEW <<< "$NEWAPIVER"
    for FILE in "${FILES[@]}"; do
        printf "Processing $FILE...\n"
        sed -b -i "s/$OLDVER/$NEWVER/g" "$FILE"
        sed -b -i "s/$OLDAPIVER/$NEWAPIVER/g" "$FILE"
        sed -b -i "s/nitrocid-${NKSMODAPIVERSPLITOLD[2]}/nitrocid-${NKSMODAPIVERSPLITNEW[2]}/g" "$FILE"
        result=$?
        if [ $result -ne 0 ]; then
            checkvendorerror $result
            return $result
        fi
    done

    # Modify the NitrocidModAPIVersionMajor and the NitrocidModAPIVersionChangeset properties
    OLDAPIMAJOR="${NKSMODAPIVERSPLITOLD[0]}.${NKSMODAPIVERSPLITOLD[1]}.${NKSMODAPIVERSPLITOLD[2]}"
    NEWAPIMAJOR="${NKSMODAPIVERSPLITNEW[0]}.${NKSMODAPIVERSPLITNEW[1]}.${NKSMODAPIVERSPLITNEW[2]}"
    sed -b -i "s/<NitrocidModAPIVersionMajor>${OLDAPIMAJOR}/<NitrocidModAPIVersionMajor>${NEWAPIMAJOR}/g" "$ROOTDIR/Directory.Build.props"
    sed -b -i "s/<NitrocidModAPIVersionChangeset>${NKSMODAPIVERSPLITOLD[3]}/<NitrocidModAPIVersionChangeset>${NKSMODAPIVERSPLITNEW[3]}/g" "$ROOTDIR/Directory.Build.props"

    # Modify the template mod files, too
    OLDAPIMAJORTMPL="${NKSMODAPIVERSPLITOLD[0]}, ${NKSMODAPIVERSPLITOLD[1]}, ${NKSMODAPIVERSPLITOLD[2]}, ${NKSMODAPIVERSPLITOLD[3]}"
    NEWAPIMAJORTMPL="${NKSMODAPIVERSPLITNEW[0]}, ${NKSMODAPIVERSPLITNEW[1]}, ${NKSMODAPIVERSPLITNEW[2]}, ${NKSMODAPIVERSPLITNEW[3]}"
    sed -b -i "s/new(${OLDAPIMAJORTMPL})/new(${NEWAPIMAJORTMPL})/g" "$ROOTDIR/public/Nitrocid.Templates/templates/KSMod/ModClass.cs"
    sed -b -i "s/New Version(${OLDAPIMAJORTMPL})/New Version(${NEWAPIMAJORTMPL})/g" "$ROOTDIR/public/Nitrocid.Templates/templates/KSModVB/ModClass.vb"

    # Modify the changes.chg file
    sed -b -i "s/version $OLDVER (mod API $OLDAPIVER)/version $NEWVER (mod API $NEWAPIVER)/g" "$ROOTDIR/vnd/changes.chg"

    # Modify the Package.wxs file
    IFS='.' read -ra NKSVERSPLITOLD <<< "$OLDVER"
    IFS='.' read -ra NKSVERSPLITNEW <<< "$NEWVER"
    OLDMAJOR="${NKSVERSPLITOLD[0]}.${NKSVERSPLITOLD[1]}.${NKSVERSPLITOLD[2]}"
    NEWMAJOR="${NKSVERSPLITNEW[0]}.${NKSVERSPLITNEW[1]}.${NKSVERSPLITNEW[2]}"
    sed -b -i "s/Name=\"Nitrocid $OLDMAJOR\"/Name=\"Nitrocid $NEWMAJOR\"/g" "$ROOTDIR/public/Nitrocid.Installers/Nitrocid.Installer/Package.wxs"

    # Modify the PKGBUILD VCS files
    sed -b -i "s/pkgname=nitrocid-${NKSMODAPIVERSPLITOLD[2]}/pkgname=nitrocid-${NKSMODAPIVERSPLITNEW[2]}/g" "$ROOTDIR"/PKGBUILD-VCS*
    sed -b -i "s/pkgver=v$OLDMAJOR/pkgver=v$NEWMAJOR/g" "$ROOTDIR"/PKGBUILD-VCS*
    sed -b -i "s/branch=v$OLDMAJOR/branch=v$NEWMAJOR/g" "$ROOTDIR"/PKGBUILD-VCS*

    # Add a Debian changelog entry
    printf "Changing Debian changelogs info...\n"
    DEBIAN_CHANGES_FILE="$ROOTDIR/debian/changelog"
    DEBIAN_CHANGES_DATE=$(date "+%a, %d %b %Y %H:%M:%S %z")
    DEBIAN_CHANGES_ENTRY=$(cat <<EOF
nitrocid-${NKSMODAPIVERSPLITNEW[2]} ($NEWAPIVER-$NEWVER-1) noble; urgency=medium

  * Please populate changelogs here

 -- Aptivi CEO <ceo@aptivi.anonaddy.com>  $DEBIAN_CHANGES_DATE
EOF
    )
    DEBIAN_CHANGES_CONTENT=$(printf "$DEBIAN_CHANGES_ENTRY\n\n$(cat "$DEBIAN_CHANGES_FILE")")
    printf "$DEBIAN_CHANGES_CONTENT\n" > $DEBIAN_CHANGES_FILE
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
