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

# Pack documentation
echo Packing documentation...
cd "$ROOTDIR/docs/" && "$zippath" -r /tmp/$ksversion-doc.zip . && cd -
checkerror $? "Failed to pack"

# Inform success
rm -rf "$ROOTDIR/DocGen/api"
checkerror $? "Failed to remove api folder"
rm -rf "$ROOTDIR/DocGen/obj"
checkerror $? "Failed to remove obj folder"
rm -rf "$ROOTDIR/docs"
checkerror $? "Failed to remove docs folder"
mv /tmp/$ksversion-doc.zip $ROOTDIR/tools/
checkerror $? "Failed to move archive from temporary folder"
echo Pack successful.
exit 0
