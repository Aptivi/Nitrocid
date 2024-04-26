OUTPUTS = public/Nitrocid/KSBuild public/*/obj public/*/*/obj private/*/bin private/*/obj debian/kernel-simulator
OUTPUT = public/Nitrocid/KSBuild/net8.0
BINARIES = assets/ks-n assets/ks-jl
MANUALS = assets/*.1
DESKTOPS = assets/ks.desktop
BRANDINGS = public/Nitrocid/OfficialAppIcon-KernelSimulator-512.png
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
	mkdir -m 755 -p debian/kernel-simulator/usr/bin debian/kernel-simulator/usr/lib/ks debian/kernel-simulator/usr/share/applications
	install -m 755 -t debian/kernel-simulator/usr/bin/ $(BINARIES)
	install -m 755 -t debian/ $(MANUALS)
	find $(OUTPUT) -mindepth 1 -type d -exec sh -c 'mkdir -p -m 755 "debian/kernel-simulator/usr/lib/ks/$$(realpath --relative-to $(OUTPUT) "$$0")"' {} \;
	find $(OUTPUT) -mindepth 1 -type f -exec sh -c 'install -m 644 -t "debian/kernel-simulator/usr/lib/ks/$$(dirname $$(realpath --relative-to $(OUTPUT) "$$0"))" "$$0"' {} \;
	install -m 755 -t debian/kernel-simulator/usr/share/applications/ $(DESKTOPS)
	install -m 755 -t debian/kernel-simulator/usr/lib/ks/ $(BRANDINGS)
	mv debian/kernel-simulator/usr/bin/ks-n debian/kernel-simulator/usr/bin/ks
	find 'debian/kernel-simulator/usr/lib/' -type d -name "runtimes" -exec sh -c 'find $$0 -mindepth 1 -maxdepth 1 -not -name $(ARCH) -type d -exec rm -rf \{\} \;' {} \;

# This makefile is just a wrapper for tools scripts.
