# Below is a workaround for .NET SDK 7.0 trying to allocate large amounts of memory for GC work:
# https://github.com/dotnet/runtime/issues/85556#issuecomment-1529177092
DOTNET_HEAP_LIMIT_INT = $(shell sysctl -n hw.memsize 2>/dev/null || grep MemAvailable /proc/meminfo | awk '{print $$2 * 1024}')
DOTNET_HEAP_LIMIT = $(shell printf '%X\n' $(DOTNET_HEAP_LIMIT_INT))

MODAPI = 25
OUTPUTS = public/Nitrocid/KSBuild public/*/obj public/*/*/obj private/*/bin private/*/obj debian/nitrocid-$(MODAPI) debian/nitrocid-$(MODAPI)-lite debian/tmp
OUTPUT = public/Nitrocid/KSBuild/net8.0
BINARIES = assets/ks assets/ks-jl
MANUALS = assets/ks.1 assets/ks-jl.1
DESKTOPS = assets/ks.desktop
BRANDINGS = public/Nitrocid/OfficialAppIcon-NitrocidKS-512.png

ARCH := $(shell if [ `uname -m` = "x86_64" ]; then echo "linux-x64"; else echo "linux-arm64"; fi)

ifndef DESTDIR
FDESTDIR := /usr/local
else
FDESTDIR := $(DESTDIR)/usr
endif

.PHONY: all install lite

# General use

all: all-online

all-online: invoke-build

dbg:
	$(MAKE) invoke-build ENVIRONMENT=Debug

dbg-ci:
	$(MAKE) invoke-build-ci ENVIRONMENT=Debug

rel-ci:
	$(MAKE) invoke-build-ci ENVIRONMENT=Release

doc: invoke-doc-build

clean:
	rm -rf $(OUTPUTS)

all-offline:
	$(MAKE) invoke-build-offline

init-offline:
	$(MAKE) invoke-init-offline

install:
	mkdir -m 755 -p $(FDESTDIR)/bin $(FDESTDIR)/lib/ks-$(MODAPI) $(FDESTDIR)/share/applications $(FDESTDIR)/share/man/man1/
	install -m 755 -t $(FDESTDIR)/bin/ $(BINARIES)
	install -m 644 -t $(FDESTDIR)/share/man/man1/ $(MANUALS)
	find $(OUTPUT) -mindepth 1 -type d -exec sh -c 'mkdir -p -m 755 "$(FDESTDIR)/lib/ks-$(MODAPI)/$$(realpath --relative-to $(OUTPUT) "$$0")"' {} \;
	find $(OUTPUT) -mindepth 1 -type f -exec sh -c 'install -m 644 -t "$(FDESTDIR)/lib/ks-$(MODAPI)/$$(dirname $$(realpath --relative-to $(OUTPUT) "$$0"))" "$$0"' {} \;
	install -m 755 -t $(FDESTDIR)/share/applications/ $(DESKTOPS)
	install -m 755 -t $(FDESTDIR)/lib/ks-$(MODAPI)/ $(BRANDINGS)
	mv $(FDESTDIR)/bin/ks $(FDESTDIR)/bin/ks-$(MODAPI)
	mv $(FDESTDIR)/bin/ks-jl $(FDESTDIR)/bin/ks-jl-$(MODAPI)
	mv $(FDESTDIR)/share/man/man1/ks.1 $(FDESTDIR)/share/man/man1/ks-$(MODAPI).1
	mv $(FDESTDIR)/share/man/man1/ks-jl.1 $(FDESTDIR)/share/man/man1/ks-jl-$(MODAPI).1
	mv $(FDESTDIR)/share/applications/ks.desktop $(FDESTDIR)/share/applications/ks-$(MODAPI).desktop
	sed -i 's|/usr/lib/ks|/usr/lib/ks-$(MODAPI)|g' $(FDESTDIR)/bin/ks-*
	sed -i 's|/usr/lib/ks|/usr/lib/ks-$(MODAPI)|g' $(FDESTDIR)/share/applications/ks-$(MODAPI).desktop
	sed -i 's|/usr/bin/ks|/usr/bin/ks-$(MODAPI)|g' $(FDESTDIR)/share/applications/ks-$(MODAPI).desktop
	find '$(FDESTDIR)/lib/' -type d -name "runtimes" -exec sh -c 'find $$0 -mindepth 1 -maxdepth 1 -not -name $(ARCH) -type d -exec rm -rf \{\} \;' {} \;

# Below targets specify functions for full build

invoke-build:
	./tools/build.sh $(ENVIRONMENT) || (echo Retrying with heap limit 0x$(DOTNET_HEAP_LIMIT)... && DOTNET_GCHeapHardLimit=$(DOTNET_HEAP_LIMIT) ./tools/build.sh $(ENVIRONMENT))

invoke-build-ci:
	./tools/build.sh $(ENVIRONMENT) -p:ContinuousIntegrationBuild=true || (echo Retrying with heap limit 0x$(DOTNET_HEAP_LIMIT)... && DOTNET_GCHeapHardLimit=$(DOTNET_HEAP_LIMIT) ./tools/build.sh $(ENVIRONMENT) -p:ContinuousIntegrationBuild=true)
    
invoke-doc-build:
	./tools/docgen.sh || (echo Retrying with heap limit 0x$(DOTNET_HEAP_LIMIT)... && DOTNET_GCHeapHardLimit=$(DOTNET_HEAP_LIMIT) ./tools/docgen.sh)

invoke-build-offline:
	HOME=`pwd`"/debian/homedir" ./tools/build.sh Release -p:NitrocidFlags=PACKAGEMANAGERBUILD -p:ContinuousIntegrationBuild=true || (echo Retrying with heap limit 0x$(DOTNET_HEAP_LIMIT)... && DOTNET_GCHeapHardLimit=$(DOTNET_HEAP_LIMIT) HOME=`pwd`"/debian/homedir" ./tools/build.sh Release -p:NitrocidFlags=PACKAGEMANAGERBUILD -p:ContinuousIntegrationBuild=true)

invoke-init-offline:
	./vnd/initializeoffline.sh || (echo Retrying with heap limit 0x$(DOTNET_HEAP_LIMIT)... && DOTNET_GCHeapHardLimit=$(DOTNET_HEAP_LIMIT) ./vnd/initializeoffline.sh)
