#!/bin/bash

#    Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

# Convenience functions
checkerror() {
    if [ $1 != 0 ]
    then
        printf "$2 - Error $1\n" >&2
        exit $1
    fi
}

# Restore packages
echo "- Restoring packages..."
echo "  - HOME=`pwd`/nuget dotnet restore Nitrocid.sln"
HOME=`pwd`/nuget dotnet restore Nitrocid.sln
checkerror $? "  - Failed to restore NuGet packages"

# Copy dependencies to deps
echo "- Copying dependencies to deps..."
echo "  - mkdir deps"
mkdir deps
checkerror $? "  - Failed to mkdir deps"
echo "  - cp nuget/.nuget/packages/*/*/*.nupkg ./deps/"
cp nuget/.nuget/packages/*/*/*.nupkg ./deps/
checkerror $? "  - Failed to copy deps"
echo "  - rm -rf nuget"
rm -rf nuget
checkerror $? "  - Failed to remove nuget folder"

# Copy NuGet.config for offline use
echo "- Copying NuGet.config..."
echo "  - cp tools/OfflineNuGet.config ./NuGet.config"
cp tools/OfflineNuGet.config ./NuGet.config
checkerror $? "  - Failed to copy offline NuGet config"

echo "- You should be able to build offline!"

