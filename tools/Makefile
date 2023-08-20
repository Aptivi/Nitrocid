#
#    Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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
#

# Below is a workaround for .NET SDK 7.0 trying to allocate large amounts of memory for GC work:
# https://github.com/dotnet/runtime/issues/85556#issuecomment-1529177092
DOTNET_PAGE_SIZE = $(shell getconf PAGESIZE)
DOTNET_AVPHYS_PAGES = $(shell getconf _AVPHYS_PAGES)
DOTNET_HEAP_LIMIT = $(shell printf '%X\n' $$(($(DOTNET_AVPHYS_PAGES) * $(DOTNET_PAGE_SIZE))))

invoke-build:
	chmod +x ./build.sh
	./build.sh || (echo Retrying with heap limit 0x$(DOTNET_HEAP_LIMIT)... && DOTNET_GCHeapHardLimit=$(DOTNET_HEAP_LIMIT) ./build.sh)

invoke-build-offline:
	chmod +x ./build.sh
	HOME=`pwd`"/debian/homedir" ./build.sh || (echo Retrying with heap limit 0x$(DOTNET_HEAP_LIMIT)... && DOTNET_GCHeapHardLimit=$(DOTNET_HEAP_LIMIT) HOME=`pwd`"/debian/homedir" ./build.sh)

debian-invoke-init-offline:
	chmod +x ./initializeoffline.sh
	cd .. && ./tools/initializeoffline.sh || (echo Retrying with heap limit 0x$(DOTNET_HEAP_LIMIT)... && DOTNET_GCHeapHardLimit=$(DOTNET_HEAP_LIMIT) ./tools/initializeoffline.sh)
