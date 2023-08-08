#!/bin/bash

#    Nitrocid KS  Copyright (C) 2018-2021  Aptivi
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

# This script builds KS and packs the artifacts. Use when you have MSBuild installed.
ksversion=$(cat version)

# Check for dependencies
rarpath=`which rar`
if [ ! $? == 0 ]; then
	echo rar is not found.
	exit 1
fi

# Pack binary
echo Packing binary...
"$rarpath" a -ep1 -r -m5 /tmp/$ksversion-bin.rar "../public/Nitrocid/KSBuild/net6.0/"
if [ ! $? == 0 ]; then
	echo Packing using rar failed.
	exit 1
fi

# Inform success
mv ~/tmp/$ksversion-bin.rar .
cp "../public/Nitrocid/KSBuild/net6.0/Nitrocid.pdb" ./$ksversion.pdb
echo Build and pack successful.
exit 0
