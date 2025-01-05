OUTPUTS = public/Nitrocid/KSBuild public/*/obj public/*/*/obj private/*/bin private/*/obj debian/nitrocid-27
OUTPUT = public/Nitrocid/KSBuild/net8.0
BINARIES = assets/ks-n assets/ks-jl
MANUALS = assets/*.1
DESKTOPS = assets/ks.desktop
BRANDINGS = public/Nitrocid/OfficialAppIcon-NitrocidKS-512.png

MODAPI = 27
ARCH := $(shell if [ `uname -m` = "x86_64" ]; then echo "linux-x64"; else echo "linux-arm64"; fi)

.PHONY: all debian-install

# General use

all: all-online

all-online:
	$(MAKE) -C tools invoke-build

dbg:
	$(MAKE) -C tools invoke-build ENVIRONMENT=Debug

doc:
	$(MAKE) -C tools invoke-doc-build

clean:
	rm -rf $(OUTPUTS)

# Below targets are for Debian packaging only

debian-all-offline:
	$(MAKE) -C tools invoke-build-offline

debian-init-offline:
	$(MAKE) -C tools debian-invoke-init-offline

debian-install:
	$(MAKE) debian-install-all PACKAGE=nitrocid-$(MODAPI)

debian-install-all:
	mkdir -m 755 -p debian/$(PACKAGE)/usr/bin debian/$(PACKAGE)/usr/lib/ks-$(MODAPI) debian/$(PACKAGE)/usr/share/applications
	install -m 755 -t debian/$(PACKAGE)/usr/bin/ $(BINARIES)
	install -m 755 -t debian/ $(MANUALS)
	find $(OUTPUT) -mindepth 1 -type d -exec sh -c 'mkdir -p -m 755 "debian/$(PACKAGE)/usr/lib/ks-$(MODAPI)/$$(realpath --relative-to $(OUTPUT) "$$0")"' {} \;
	find $(OUTPUT) -mindepth 1 -type f -exec sh -c 'install -m 644 -t "debian/$(PACKAGE)/usr/lib/ks-$(MODAPI)/$$(dirname $$(realpath --relative-to $(OUTPUT) "$$0"))" "$$0"' {} \;
	install -m 755 -t debian/$(PACKAGE)/usr/share/applications/ $(DESKTOPS)
	install -m 755 -t debian/$(PACKAGE)/usr/lib/ks-$(MODAPI)/ $(BRANDINGS)
	mv debian/$(PACKAGE)/usr/bin/ks-n debian/$(PACKAGE)/usr/bin/ks-$(MODAPI)
	mv debian/$(PACKAGE)/usr/bin/ks-jl debian/$(PACKAGE)/usr/bin/ks-jl-$(MODAPI)
	mv debian/$(PACKAGE)/usr/share/applications/ks.desktop debian/$(PACKAGE)/usr/share/applications/ks-$(MODAPI).desktop
	sed -i 's|/usr/lib/ks|/usr/lib/ks-$(MODAPI)|g' debian/$(PACKAGE)/usr/bin/ks-*
	sed -i 's|/usr/lib/ks|/usr/lib/ks-$(MODAPI)|g' debian/$(PACKAGE)/usr/share/applications/ks-$(MODAPI).desktop
	sec -i 's|/usr/bin/ks|/usr/bin/ks-$(MODAPI)|g' debian/$(PACKAGE)/usr/share/applications/ks-$(MODAPI).desktop
	find 'debian/$(PACKAGE)/usr/lib/' -type d -name "runtimes" -exec sh -c 'find $$0 -mindepth 1 -maxdepth 1 -not -name $(ARCH) -type d -exec rm -rf \{\} \;' {} \;

debian-install-lite:
	$(MAKE) debian-install-all PACKAGE=nitrocid-$(MODAPI)-lite
	rm -rf debian/nitrocid-$(MODAPI)-lite/usr/lib/ks-$(MODAPI)/Addons

# This makefile is just a wrapper for tools scripts.
