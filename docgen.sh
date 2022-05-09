#!/bin/bash

#    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
#
#    This file is part of Kernel Simulator
#
#    Kernel Simulator is free software: you can redistribute it and/or modify
#    it under the terms of the GNU General Public License as published by
#    the Free Software Foundation, either version 3 of the License, or
#    (at your option) any later version.
#
#    Kernel Simulator is distributed in the hope that it will be useful,
#    but WITHOUT ANY WARRANTY; without even the implied warranty of
#    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
#    GNU General Public License for more details.
#
#    You should have received a copy of the GNU General Public License
#    along with this program.  If not, see <https://www.gnu.org/licenses/>.

# This script builds KS and packs the artifacts. Use when you have MSBuild installed.
ksversion=0.0.21.4

mkdir ~/tmp
echo Make sure you have the following:
echo "  - docfx in your PATH"
echo 
echo Press any key to start.
read -n 1

# Check for dependencies
msbuildpath=`which docfx`
if [ ! $? == 0 ]; then
	echo DocFX is not found.
	exit 1
fi
rarpath=`which rar`
if [ ! $? == 0 ]; then
	echo rar is not found.
	exit 1
fi

# Build KS
echo Building KS documentation...
docfx DocGen/docfx.json >> ~/tmp/buildandpack.log
if [ ! $? == 0 ]; then
	echo Build failed.
	exit 1
fi

# Pack documentation
echo Packing documentation...
"$rarpath" a -ep1 -r -m5 ~/tmp/$ksversion-doc.rar "docs/" > ~/tmp/buildandpack.log
if [ ! $? == 0 ]; then
	echo Packing using rar failed.
	exit 1
fi

# Inform success
rm -rf "DocGen/api" >> ~/tmp/buildandpack.log
rm -rf "DocGen/obj" >> ~/tmp/buildandpack.log
rm -rf "docs" >> ~/tmp/buildandpack.log
mv ~/tmp/$ksversion-doc.rar .
echo Build and pack successful.
exit 0
