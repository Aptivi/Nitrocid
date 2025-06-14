MODAPI = 28
ROOT_DIR := $(shell dirname "$(realpath $(lastword $(MAKEFILE_LIST)))")

OUTPUT = "$(ROOT_DIR)/public/Nitrocid/KSBuild/net8.0"
BINARIES = "$(ROOT_DIR)/assets/ks"
MANUALS = "$(ROOT_DIR)/assets/ks.1"
DESKTOPS = "$(ROOT_DIR)/assets/ks.desktop"
BRANDINGS = "$(ROOT_DIR)/public/Nitrocid/OfficialAppIcon-NitrocidKS-512.png"

ARCH := $(shell if [ `uname -m` = "x86_64" ]; then echo "linux-x64"; else echo "linux-arm64"; fi)

ifndef DESTDIR
FDESTDIR := /usr/local
else
FDESTDIR := $(DESTDIR)/usr
endif

.PHONY: all install lite

# General use

all:
	$(MAKE) all-online BUILDARGS="$(BUILDARGS)"

all-online:
	$(MAKE) invoke-build ENVIRONMENT=Release BUILDARGS="$(BUILDARGS)"

dbg:
	$(MAKE) invoke-build ENVIRONMENT=Debug BUILDARGS="$(BUILDARGS)"

dbg-ci:
	$(MAKE) invoke-build ENVIRONMENT=Debug BUILDARGS="-p:ContinuousIntegrationBuild=true $(BUILDARGS)"

rel-ci:
	$(MAKE) invoke-build ENVIRONMENT=Release BUILDARGS="-p:ContinuousIntegrationBuild=true $(BUILDARGS)"

doc: invoke-doc-build

clean:
	bash tools/clean.sh

all-offline:
	$(MAKE) invoke-build-offline BUILDARGS="-p:NitrocidFlags=PACKAGEMANAGERBUILD -p:ContinuousIntegrationBuild=true $(BUILDARGS)"

init-offline:
	$(MAKE) invoke-init-offline

install:
	mkdir -m 755 -p $(FDESTDIR)/bin $(FDESTDIR)/lib/ks-$(MODAPI) $(FDESTDIR)/share/applications $(FDESTDIR)/share/man/man1/
	install -m 755 -t $(FDESTDIR)/bin/ $(BINARIES)
	install -m 644 -t $(FDESTDIR)/share/man/man1/ $(MANUALS)
	find "$(OUTPUT)" -mindepth 1 -type d -exec sh -c 'mkdir -p -m 755 "$(FDESTDIR)/lib/ks-$(MODAPI)/$$(realpath --relative-to "$(OUTPUT)" "$$0")"' {} \;
	find "$(OUTPUT)" -mindepth 1 -type f -exec sh -c 'install -m 644 -t "$(FDESTDIR)/lib/ks-$(MODAPI)/$$(dirname $$(realpath --relative-to "$(OUTPUT)" "$$0"))" "$$0"' {} \;
	install -m 755 -t $(FDESTDIR)/share/applications/ $(DESKTOPS)
	install -m 755 -t $(FDESTDIR)/lib/ks-$(MODAPI)/ $(BRANDINGS)
	mv $(FDESTDIR)/bin/ks $(FDESTDIR)/bin/ks-$(MODAPI)
	mv $(FDESTDIR)/share/man/man1/ks.1 $(FDESTDIR)/share/man/man1/ks-$(MODAPI).1
	mv $(FDESTDIR)/share/applications/ks.desktop $(FDESTDIR)/share/applications/ks-$(MODAPI).desktop
	sed -i 's|/usr/lib/ks|/usr/lib/ks-$(MODAPI)|g' $(FDESTDIR)/bin/ks-*
	sed -i 's|/usr/lib/ks|/usr/lib/ks-$(MODAPI)|g' $(FDESTDIR)/share/applications/ks-$(MODAPI).desktop
	sed -i 's|/usr/bin/ks|/usr/bin/ks-$(MODAPI)|g' $(FDESTDIR)/share/applications/ks-$(MODAPI).desktop
	find '$(FDESTDIR)/lib/' -type d -name "runtimes" -exec sh -c 'find $$0 -mindepth 1 -maxdepth 1 -not -name $(ARCH) -type d -exec rm -rf \{\} \;' {} \;

# Below targets specify functions for full build

invoke-build:
	bash tools/build.sh "$(ENVIRONMENT)" $(BUILDARGS)
    
invoke-doc-build:
	bash tools/docgen.sh

invoke-build-offline:
	HOME=`pwd`"/debian/homedir" bash tools/build.sh Release $(BUILDARGS)

invoke-init-offline:
	bash tools/localize.sh
