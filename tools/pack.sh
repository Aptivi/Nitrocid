#!/bin/bash

#    Nitrocid KS  Copyright (C) 2018-2025  Aptivi
#
#    This file is part of Nitrocid KS
#
#    Nitrocid KS is free software: you can redistribute it and/or modify
#    it under the terms of the GNU General Public License as published by
#    the Free Software Foundation, either version 3 of the License, or
#    (at your option) any later version.
#
#    Nitrocid KS is distributed in the hope that it will be useful,
#    but WITHOUT ANY WARRANTY; without even the implied warranty of
#    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#    GNU General Public License for more details.
#
#    You should have received a copy of the GNU General Public License
#    along with this program.  If not, see <https://www.gnu.org/licenses/>.

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

# This script builds KS and packs the artifacts. Use when you have MSBuild installed.
ksversion=$(grep "<Version>" $ROOTDIR/Directory.Build.props | cut -d "<" -f 2 | cut -d ">" -f 2)
checkerror $? "Failed to get version. Check to make sure that the version is specified correctly in D.B.props"

# Check for dependencies
zippath=`which zip`
checkerror $? "zip is not found"
shapath=`which sha256sum`
checkerror $? "sha256sum is not found"

# Pack binary
echo Packing binary...
cd "$ROOTDIR/public/Nitrocid/KSBuild/net8.0/" && "$zippath" -r /tmp/$ksversion-bin.zip . && cd -
checkerror $? "Failed to pack"
cd "$ROOTDIR/public/Nitrocid/KSBuild/net8.0/" && "$zippath" -r /tmp/$ksversion-bin-lite.zip . -x "./Addons/*" && cd -
checkerror $? "Failed to pack"
cd "$ROOTDIR/public/Nitrocid/KSBuild/net8.0/Addons/" && "$zippath" -r /tmp/$ksversion-addons.zip . && cd -
checkerror $? "Failed to pack"
cd "$ROOTDIR/public/Nitrocid/KSAnalyzer/netstandard2.0/" && "$zippath" -r /tmp/$ksversion-analyzers.zip . && cd -
checkerror $? "Failed to pack"
cd "$ROOTDIR/public/Nitrocid/KSAnalyzer/net8.0/" && "$zippath" -r /tmp/$ksversion-mod-analyzer.zip . && cd -
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
cp changes.chg $ksversion-changes.chg
checkerror $? "Failed to copy changelogs"

echo Pack successful.
exit 0
