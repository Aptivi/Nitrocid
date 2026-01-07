//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

namespace Nitrocid.ScreensaverPacks.Screensavers.Utilities
{
    internal static class HackingLogList
    {
        internal static string[] logSnippets =
        [
            """
            [    0.000000] Linux version 6.18.2-arch2-1 (linux@archlinux) (gcc (GCC) 15.2.1 20251112, GNU ld (GNU Binutils) 2.45.1) #1 SMP PREEMPT_DYNAMIC Thu, 18 Dec 2025 18:00:18 +0000
            [    0.000000] Command line: BOOT_IMAGE=/vmlinuz-linux root=UUID=e7ab2972-2e54-4f38-a039-bc23f5f91fca rw rootflags=subvol=@ zswap.enabled=0 rootfstype=btrfs loglevel=3 quiet
            [    0.000000] BIOS-provided physical RAM map:
            [    0.000000] BIOS-e820: [mem 0x0000000000000000-0x000000000009e7ff] usable
            [    0.000000] BIOS-e820: [mem 0x000000000009e800-0x000000000009ffff] reserved
            [    0.000000] BIOS-e820: [mem 0x00000000000dc000-0x00000000000fffff] reserved
            [    0.000000] BIOS-e820: [mem 0x0000000000100000-0x00000000bfecffff] usable
            [    0.000000] BIOS-e820: [mem 0x00000000bfed0000-0x00000000bfefefff] ACPI data
            [    0.000000] BIOS-e820: [mem 0x00000000bfeff000-0x00000000bfefffff] ACPI NVS
            [    0.000000] BIOS-e820: [mem 0x00000000bff00000-0x00000000bfffffff] usable
            [    0.000000] BIOS-e820: [mem 0x00000000f0000000-0x00000000f7ffffff] reserved
            [    0.000000] BIOS-e820: [mem 0x00000000fec00000-0x00000000fec0ffff] reserved
            [    0.000000] BIOS-e820: [mem 0x00000000fee00000-0x00000000fee00fff] reserved
            [    0.000000] BIOS-e820: [mem 0x00000000fffe0000-0x00000000ffffffff] reserved
            [    0.000000] BIOS-e820: [mem 0x0000000100000000-0x000000023fffffff] usable
            [    0.000000] NX (Execute Disable) protection: active
            [    0.000000] APIC: Static calls initialized
            [    0.000000] SMBIOS 2.7 present.
            [    0.000000] DMI: VMware, Inc. VMware Virtual Platform/440BX Desktop Reference Platform, BIOS 6.00 03/24/2025
            [    0.000000] DMI: Memory slots populated: 1/128
            [    0.000000] vmware: hypercall mode: 0x02
            [    0.000000] Hypervisor detected: VMware
            [    0.000000] vmware: TSC freq read from hypervisor : 3072.001 MHz
            [    0.000000] vmware: Host bus clock speed read from hypervisor : 66000000 Hz
            [    0.000000] vmware: using clock offset of 5730168161 ns
            [    0.000012] tsc: Detected 3072.001 MHz processor
            [    0.001580] e820: update [mem 0x00000000-0x00000fff] usable ==> reserved
            [    0.001583] e820: remove [mem 0x000a0000-0x000fffff] usable
            [    0.001587] last_pfn = 0x240000 max_arch_pfn = 0x400000000
            [    0.001609] total RAM covered: 261120M
            [    0.001707] Found optimal setting for mtrr clean up
            [    0.001708]  gran_size: 64K 	chunk_size: 64K 	num_reg: 8  	lose cover RAM: 0G
            [    0.001710] MTRR map: 7 entries (5 fixed + 2 variable; max 21), built from 8 variable MTRRs
            [    0.001712] x86/PAT: Configuration [0-7]: WB  WC  UC- UC  WB  WP  UC- WT  
            [    0.001747] e820: update [mem 0xc0000000-0xffffffff] usable ==> reserved
            [    0.001750] last_pfn = 0xc0000 max_arch_pfn = 0x400000000
            [    0.008953] found SMP MP-table at [mem 0x000f6a70-0x000f6a7f]
            [    0.008965] Using GB pages for direct mapping
            [    0.009099] RAMDISK: [mem 0x36b1f000-0x37586fff]
            [    0.009114] ACPI: Early table checksum verification disabled
            [    0.009117] ACPI: RSDP 0x00000000000F6A00 000024 (v02 PTLTD )
            [    0.009121] ACPI: XSDT 0x00000000BFEDB633 00005C (v01 INTEL  440BX    06040000 VMW  01324272)
            [    0.009126] ACPI: FACP 0x00000000BFEFEE73 0000F4 (v04 INTEL  440BX    06040000 PTL  000F4240)
            [    0.009148] ACPI: DSDT 0x00000000BFEDD001 021E72 (v01 PTLTD  Custom   06040000 MSFT 03000001)
            [    0.009160] ACPI: FACS 0x00000000BFEFFFC0 000040
            [    0.009163] ACPI: FACS 0x00000000BFEFFFC0 000040
            [    0.009165] ACPI: BOOT 0x00000000BFEDCFB4 000028 (v01 PTLTD  $SBFTBL$ 06040000  LTP 00000001)
            [    0.009168] ACPI: APIC 0x00000000BFEDC872 000742 (v01 PTLTD  ? APIC   06040000  LTP 00000000)
            [    0.009171] ACPI: MCFG 0x00000000BFEDC836 00003C (v01 PTLTD  $PCITBL$ 06040000  LTP 00000001)
            [    0.009173] ACPI: SRAT 0x00000000BFEDB72F 0008D0 (v02 VMWARE MEMPLUG  06040000 VMW  00000001)
            [    0.009176] ACPI: HPET 0x00000000BFEDB6F7 000038 (v01 VMWARE VMW HPET 06040000 VMW  00000001)
            [    0.009178] ACPI: WAET 0x00000000BFEDB6CF 000028 (v01 VMWARE VMW WAET 06040000 VMW  00000001)
            [    0.009180] ACPI: Reserving FACP table memory at [mem 0xbfefee73-0xbfefef66]
            [    0.009181] ACPI: Reserving DSDT table memory at [mem 0xbfedd001-0xbfefee72]
            [    0.009182] ACPI: Reserving FACS table memory at [mem 0xbfefffc0-0xbfefffff]
            [    0.009182] ACPI: Reserving FACS table memory at [mem 0xbfefffc0-0xbfefffff]
            [    0.009182] ACPI: Reserving BOOT table memory at [mem 0xbfedcfb4-0xbfedcfdb]
            [    0.009183] ACPI: Reserving APIC table memory at [mem 0xbfedc872-0xbfedcfb3]
            [    0.009183] ACPI: Reserving MCFG table memory at [mem 0xbfedc836-0xbfedc871]
            [    0.009184] ACPI: Reserving SRAT table memory at [mem 0xbfedb72f-0xbfedbffe]
            [    0.009184] ACPI: Reserving HPET table memory at [mem 0xbfedb6f7-0xbfedb72e]
            [    0.009185] ACPI: Reserving WAET table memory at [mem 0xbfedb6cf-0xbfedb6f6]
            [    0.009233] ACPI: SRAT: Node 0 PXM 0 [mem 0x00000000-0x0009ffff]
            [    0.009234] ACPI: SRAT: Node 0 PXM 0 [mem 0x00100000-0xbfffffff]
            [    0.009235] ACPI: SRAT: Node 0 PXM 0 [mem 0x100000000-0x23fffffff]
            [    0.009235] ACPI: SRAT: Node 0 PXM 0 [mem 0x240000000-0x203fffffff] hotplug
            [    0.009238] NUMA: Node 0 [mem 0x00001000-0x0009ffff] + [mem 0x00100000-0xbfffffff] -> [mem 0x00001000-0xbfffffff]
            [    0.009239] NUMA: Node 0 [mem 0x00001000-0xbfffffff] + [mem 0x100000000-0x23fffffff] -> [mem 0x00001000-0x23fffffff]
            [    0.009244] NODE_DATA(0) allocated [mem 0x23ffd5280-0x23fffffff]
            [    0.009986] Zone ranges:
            [    0.009987]   DMA      [mem 0x0000000000001000-0x0000000000ffffff]
            [    0.009988]   DMA32    [mem 0x0000000001000000-0x00000000ffffffff]
            [    0.009989]   Normal   [mem 0x0000000100000000-0x000000023fffffff]
            [    0.009989]   Device   empty
            [    0.009990] Movable zone start for each node
            [    0.009991] Early memory node ranges
            [    0.009992]   node   0: [mem 0x0000000000001000-0x000000000009dfff]
            [    0.009992]   node   0: [mem 0x0000000000100000-0x00000000bfecffff]
            [    0.009993]   node   0: [mem 0x00000000bff00000-0x00000000bfffffff]
            [    0.009994]   node   0: [mem 0x0000000100000000-0x000000023fffffff]
            [    0.009995] Initmem setup node 0 [mem 0x0000000000001000-0x000000023fffffff]
            [    0.010040] On node 0, zone DMA: 1 pages in unavailable ranges
            [    0.010660] On node 0, zone DMA: 98 pages in unavailable ranges
            [    0.134855] On node 0, zone DMA32: 48 pages in unavailable ranges
            """,

            """
            [    0.590458] mem auto-init: stack:all(zero), heap alloc:on, heap free:off
            [    0.602949] SLUB: HWalign=64, Order=0-3, MinObjects=0, CPUs=128, Nodes=1
            [    0.605599] Kernel/User page tables isolation: enabled
            [    0.616107] ftrace: allocating 57002 entries in 224 pages
            [    0.616109] ftrace: allocated 224 pages with 3 groups
            [    0.617196] Dynamic Preempt: full
            [    0.619642] rcu: Preemptible hierarchical RCU implementation.
            [    0.619643] rcu: 	RCU restricting CPUs from NR_CPUS=8192 to nr_cpu_ids=128.
            [    0.619644] rcu: 	RCU priority boosting: priority 1 delay 500 ms.
            [    0.619645] 	Trampoline variant of Tasks RCU enabled.
            [    0.619646] 	Rude variant of Tasks RCU enabled.
            [    0.619646] 	Tracing variant of Tasks RCU enabled.
            [    0.619647] rcu: RCU calculated value of scheduler-enlistment delay is 100 jiffies.
            [    0.619647] rcu: Adjusting geometry for rcu_fanout_leaf=16, nr_cpu_ids=128
            [    0.620076] RCU Tasks: Setting shift to 7 and lim to 1 rcu_task_cb_adjust=1 rcu_task_cpu_ids=128.
            [    0.620082] RCU Tasks Rude: Setting shift to 7 and lim to 1 rcu_task_cb_adjust=1 rcu_task_cpu_ids=128.
            [    0.620088] RCU Tasks Trace: Setting shift to 7 and lim to 1 rcu_task_cb_adjust=1 rcu_task_cpu_ids=128.
            [    0.625121] NR_IRQS: 524544, nr_irqs: 1448, preallocated irqs: 16
            [    0.625727] rcu: srcu_init: Setting srcu_struct sizes to big.
            [    0.626404] kfence: initialized - using 2097152 bytes for 255 objects at 0x(____ptrval____)-0x(____ptrval____)
            [    0.627389] Console: colour dummy device 80x25
            [    0.627392] printk: legacy console [tty0] enabled
            [    0.627519] ACPI: Core revision 20250807
            [    0.628325] clocksource: hpet: mask: 0xffffffff max_cycles: 0xffffffff, max_idle_ns: 133484882848 ns
            [    0.628459] APIC: Switch to symmetric I/O mode setup
            [    0.628901] x2apic enabled
            [    0.629214] APIC: Switched APIC routing to: physical x2apic
            [    0.631082] ..TIMER: vector=0x30 apic1=0 pin1=2 apic2=-1 pin2=-1
            [    0.631131] clocksource: tsc-early: mask: 0xffffffffffffffff max_cycles: 0x2c47f5e31cd, max_idle_ns: 440795287094 ns
            [    0.631135] Calibrating delay loop (skipped) preset value.. 6144.00 BogoMIPS (lpj=3072001)
            [    0.632464] x86/cpu: User Mode Instruction Prevention (UMIP) activated
            [    0.632691] Last level iTLB entries: 4KB 0, 2MB 0, 4MB 0
            [    0.632693] Last level dTLB entries: 4KB 0, 2MB 0, 4MB 0, 1GB 0
            [    0.632707] mitigations: Enabled attack vectors: user_kernel, user_user, guest_host, guest_guest, SMT mitigations: auto
            [    0.632710] Speculative Store Bypass: Mitigation: Speculative Store Bypass disabled via prctl
            [    0.632711] Spectre V2 : Mitigation: IBRS
            [    0.632712] RETBleed: Mitigation: IBRS
            [    0.632712] ITS: Mitigation: Aligned branch/return thunks
            [    0.632713] MDS: Mitigation: Clear CPU buffers
            [    0.632713] Spectre V1 : Mitigation: usercopy/swapgs barriers and __user pointer sanitization
            [    0.632714] Spectre V2 : Spectre v2 / SpectreRSB: Filling RSB on context switch and VMEXIT
            [    0.632715] Spectre V2 : mitigation: Enabling conditional Indirect Branch Prediction Barrier
            [    0.632716] active return thunk: its_return_thunk
            [    0.632720] Spectre V2 : Spectre BHI mitigation: SW BHB clearing on syscall and VM exit
            [    0.632766] x86/fpu: Supporting XSAVE feature 0x001: 'x87 floating point registers'
            [    0.632768] x86/fpu: Supporting XSAVE feature 0x002: 'SSE registers'
            [    0.632768] x86/fpu: Supporting XSAVE feature 0x004: 'AVX registers'
            [    0.632769] x86/fpu: xstate_offset[2]:  576, xstate_sizes[2]:  256
            [    0.632770] x86/fpu: Enabled xstate features 0x7, context size is 832 bytes, using 'compacted' format.
            [    0.655985] Freeing SMP alternatives memory: 56K
            [    0.655991] pid_max: default: 131072 minimum: 1024
            [    0.656294] LSM: initializing lsm=capability,landlock,lockdown,yama,bpf
            [    0.656377] landlock: Up and running.
            [    0.656379] Yama: becoming mindful.
            [    0.656510] LSM support for eBPF active
            [    0.656895] Mount-cache hash table entries: 16384 (order: 5, 131072 bytes, linear)
            [    0.657213] Mountpoint-cache hash table entries: 16384 (order: 5, 131072 bytes, linear)
            [    0.659728] smpboot: CPU0: Intel(R) Core(TM) Ultra 9 275HX (family: 0x6, model: 0xc6, stepping: 0x2)
            [    0.661660] Performance Events: Lunarlake Hybrid events, core PMU driver.
            """,

            """
            [    1.638002] usbcore: registered new interface driver usbserial_generic
            [    1.638005] usbserial: USB Serial support registered for generic
            [    1.638031] i8042: PNP: PS/2 Controller [PNP0303:KBC,PNP0f13:MOUS] at 0x60,0x64 irq 1,12
            [    1.640366] serio: i8042 KBD port at 0x60,0x64 irq 1
            [    1.640371] serio: i8042 AUX port at 0x60,0x64 irq 12
            [    1.641534] rtc_cmos 00:01: registered as rtc0
            [    1.641676] rtc_cmos 00:01: setting system clock to 2026-01-07T17:20:00 UTC (1767806400)
            [    1.641739] rtc_cmos 00:01: alarms up to one month, y3k, 114 bytes nvram
            [    1.641763] intel_pstate: CPU model not supported
            [    1.644354] simple-framebuffer simple-framebuffer.0: [drm] Registered 1 planes with drm panic
            [    1.644356] [drm] Initialized simpledrm 1.0.0 for simple-framebuffer.0 on minor 0
            [    1.645200] input: AT Translated Set 2 keyboard as /devices/platform/i8042/serio0/input/input1
            [    1.647545] ehci-pci 0000:02:03.0: EHCI Host Controller
            [    1.647550] ehci-pci 0000:02:03.0: new USB bus registered, assigned bus number 2
            [    1.647772] ehci-pci 0000:02:03.0: irq 17, io mem 0xfd5ef000
            [    1.648741] fbcon: Deferring console take-over
            [    1.648742] simple-framebuffer simple-framebuffer.0: [drm] fb0: simpledrmdrmfb frame buffer device
            [    1.648828] hid: raw HID events driver (C) Jiri Kosina
            [    1.648853] usbcore: registered new interface driver usbhid
            [    1.648854] usbhid: USB HID core driver
            [    1.648884] rust_binder: Loaded Rust Binder.
            [    1.649088] drop_monitor: Initializing network drop monitor service
            [    1.649260] NET: Registered PF_INET6 protocol family
            [    1.651465] Segment Routing with IPv6
            [    1.651466] RPL Segment Routing with IPv6
            [    1.651470] In-situ OAM (IOAM) with IPv6
            [    1.651500] NET: Registered PF_PACKET protocol family
            [    1.652185] IPI shorthand broadcast: enabled
            [    1.653222] ehci-pci 0000:02:03.0: USB 2.0 started, EHCI 1.00
            [    1.653253] usb usb2: New USB device found, idVendor=1d6b, idProduct=0002, bcdDevice= 6.18
            [    1.653255] usb usb2: New USB device strings: Mfr=3, Product=2, SerialNumber=1
            [    1.653256] usb usb2: Product: EHCI Host Controller
            [    1.653256] usb usb2: Manufacturer: Linux 6.18.2-arch2-1 ehci_hcd
            [    1.653257] usb usb2: SerialNumber: 0000:02:03.0
            [    1.654194] hub 2-0:1.0: USB hub found
            [    1.654199] hub 2-0:1.0: 6 ports detected
            [    1.655160] sched_clock: Marking stable (1649002848, 5168270)->(1655769259, -1598141)
            [    1.656376] registered taskstats version 1
            [    1.661150] Loading compiled-in X.509 certificates
            [    1.667619] Loaded X.509 cert 'Build time autogenerated kernel key: a5b00b95218882d944c89b2378e440be4d948982'
            [    1.670425] Demotion targets for Node 0: null
            [    1.671054] Key type .fscrypt registered
            [    1.671056] Key type fscrypt-provisioning registered
            [    1.671684] Btrfs loaded, zoned=yes, fsverity=yes
            [    1.671723] Key type big_key registered
            [    1.672086] PM:   Magic number: 10:568:339
            [    1.672126] pciehp 0000:00:17.0:pcie004: hash matches
            [    1.673024] RAS: Correctable Errors collector initialized.
            [    1.676012] clk: Disabling unused clocks
            [    1.676014] PM: genpd: Disabling unused power domains
            """,

            """
            systemd[1]: Starting Update is Completed...
            systemd[1]: Finished Update is Completed.
            systemd[1]: Reached target System Initialization.
            systemd[1]: Started Refresh existing PGP keys of archlinux-keyring regularly.
            systemd[1]: Started Daily verification of password and group files.
            systemd[1]: Started Daily Cleanup of Temporary Directories.
            systemd[1]: Reached target Timer Units.
            systemd[1]: Listening on D-Bus System Message Bus Socket.
            systemd[1]: Listening on GnuPG network certificate management daemon for /etc/pacman.d/gnupg.
            systemd[1]: Listening on GnuPG cryptographic agent and passphrase cache (access for web browsers) for /etc/pacman.d/gnupg.
            systemd[1]: Listening on GnuPG cryptographic agent and passphrase cache (restricted) for /etc/pacman.d/gnupg.
            systemd[1]: Listening on GnuPG cryptographic agent (ssh-agent emulation) for /etc/pacman.d/gnupg.
            systemd[1]: Listening on GnuPG cryptographic agent and passphrase cache for /etc/pacman.d/gnupg.
            systemd[1]: Listening on GnuPG public key management service for /etc/pacman.d/gnupg.
            systemd[1]: Listening on Hostname Service Socket.
            systemd[1]: Listening on User Login Management Varlink Socket.
            systemd[1]: Listening on Virtual Machine and Container Registration Service Socket.
            systemd[1]: Reached target Socket Units.
            systemd[1]: Starting D-Bus System Message Bus...
            systemd[1]: TPM PCR Barrier (Initialization) was skipped because of an unmet condition check (ConditionSecurity=measured-uki).
            systemd[1]: Started D-Bus System Message Bus.
            dbus-broker-launch[66]: Ready
            systemd[1]: Reached target Basic System.
            systemd[1]: Starting User Login Management...
            systemd[1]: TPM PCR Barrier (User) was skipped because of an unmet condition check (ConditionSecurity=measured-uki).
            systemd[1]: Starting Permit User Sessions...
            systemd[1]: Finished Permit User Sessions.
            systemd[1]: Started Console Getty.
            systemd[1]: Getty on tty1 was skipped because of an unmet condition check (ConditionPathExists=/dev/tty0).
            systemd[1]: Reached target Login Prompts.
            systemd-logind[68]: New seat seat0.
            systemd[1]: Started User Login Management.
            systemd[1]: Reached target Multi-User System.
            systemd[1]: Reached target Graphical Interface.
            systemd[1]: Startup finished in 1.913s.
            systemd[1]: Started [systemd-run] localectl set-keymap "".
            systemd[1]: Starting Locale Service...
            (-localed)[82]: systemd-localed.service: PrivateNetwork=yes is configured, but the kernel does not support or we lack privileges for network namespace, proceeding without.
            systemd[1]: Started Locale Service.
            systemd-localed[82]: No conversion found for virtual console keymap "".
            systemd[1]: run-p18708-i18709.service: Deactivated successfully.
            systemd[1]: Started [systemd-run] /usr/bin/localectl set-keymap us.
            systemd-localed[82]: The virtual console keymap 'us' is converted to X11 keyboard layout 'us' model 'pc105+inet' variant '' options 'terminate:ctrl_alt_bksp'
            systemd-localed[82]: Changed virtual console keymap to 'us' toggle ''
            systemd[1]: Virtual Console Setup was skipped because of an unmet condition check (ConditionPathExists=/dev/tty0).
            systemd[1]: run-p18718-i18719.service: Deactivated successfully.
            systemd[1]: Started [systemd-run] shutdown now.
            systemd-logind[68]: The system will power off now!
            systemd-logind[68]: System is powering down.
            systemd[1]: run-p18727-i18728.service: Deactivated successfully.
            systemd[1]: Stopped [systemd-run] shutdown now.
            systemd[1]: Removed slice Slice /system/getty.
            systemd[1]: Removed slice Slice /system/modprobe.
            systemd[1]: Stopped target Graphical Interface.
            systemd[1]: Stopped target Multi-User System.
            systemd[1]: Stopped target Login Prompts.
            systemd[1]: Stopped target Timer Units.
            systemd[1]: archlinux-keyring-wkd-sync.timer: Deactivated successfully.
            systemd[1]: Stopped Refresh existing PGP keys of archlinux-keyring regularly.
            systemd[1]: shadow.timer: Deactivated successfully.
            systemd[1]: Stopped Daily verification of password and group files.
            systemd[1]: systemd-tmpfiles-clean.timer: Deactivated successfully.
            systemd[1]: Stopped Daily Cleanup of Temporary Directories.
            systemd[1]: systemd-coredump.socket: Deactivated successfully.
            systemd[1]: Closed Process Core Dump Socket.
            systemd[1]: Stopping Console Getty...
            systemd[1]: Starting Generate shutdown-ramfs...
            """,

            """
            kwin_wayland_wrapper[848]: MESA-LOADER: failed to open dri: /usr/lib/libc.so.6: version `GLIBC_ABI_GNU2_TLS' not found (required by /usr/lib/libgallium-25.3.1-arch1.2.so) (search paths /usr/lib/gbm, suffix _gbm)
            kwin_wayland[848]: Failed to create gbm device for "/dev/dri/card0"
            kwin_wayland[848]: No suitable DRM devices have been found
            kwin_wayland[848]: QThreadStorage: entry 7 destroyed before end of thread 0x563d560fa810
            kwin_wayland[848]: QThreadStorage: entry 1 destroyed before end of thread 0x563d560fa810
            kwin_wayland[848]: QThreadStorage: entry 0 destroyed before end of thread 0x563d560fa810
            kwin_wayland[852]: No backend specified, automatically choosing drm
            kwin_wayland_wrapper[852]: MESA-LOADER: failed to open dri: /usr/lib/libc.so.6: version `GLIBC_ABI_GNU2_TLS' not found (required by /usr/lib/libgallium-25.3.1-arch1.2.so) (search paths /usr/lib/gbm, suffix _gbm)
            kwin_wayland[852]: Failed to create gbm device for "/dev/dri/card0"
            kwin_wayland[852]: No suitable DRM devices have been found
            kwin_wayland[852]: QThreadStorage: entry 7 destroyed before end of thread 0x5622607b5810
            kwin_wayland[852]: QThreadStorage: entry 1 destroyed before end of thread 0x5622607b5810
            kwin_wayland[852]: QThreadStorage: entry 0 destroyed before end of thread 0x5622607b5810
            kwin_wayland[855]: No backend specified, automatically choosing drm
            kwin_wayland_wrapper[855]: MESA-LOADER: failed to open dri: /usr/lib/libc.so.6: version `GLIBC_ABI_GNU2_TLS' not found (required by /usr/lib/libgallium-25.3.1-arch1.2.so) (search paths /usr/lib/gbm, suffix _gbm)
            kwin_wayland[855]: Failed to create gbm device for "/dev/dri/card0"
            kwin_wayland[855]: No suitable DRM devices have been found
            kwin_wayland[855]: QThreadStorage: entry 7 destroyed before end of thread 0x55e7eaec5810
            kwin_wayland[855]: QThreadStorage: entry 1 destroyed before end of thread 0x55e7eaec5810
            kwin_wayland[855]: QThreadStorage: entry 0 destroyed before end of thread 0x55e7eaec5810
            kwin_wayland[860]: No backend specified, automatically choosing drm
            kwin_wayland_wrapper[860]: MESA-LOADER: failed to open dri: /usr/lib/libc.so.6: version `GLIBC_ABI_GNU2_TLS' not found (required by /usr/lib/libgallium-25.3.1-arch1.2.so) (search paths /usr/lib/gbm, suffix _gbm)
            kwin_wayland[860]: Failed to create gbm device for "/dev/dri/card0"
            kwin_wayland[860]: No suitable DRM devices have been found
            kwin_wayland[860]: QThreadStorage: entry 7 destroyed before end of thread 0x5631068d7810
            kwin_wayland[860]: QThreadStorage: entry 1 destroyed before end of thread 0x5631068d7810
            kwin_wayland[860]: QThreadStorage: entry 0 destroyed before end of thread 0x5631068d7810
            systemd[717]: Requested transaction contradicts existing jobs: Transaction for plasma-workspace.target/stop is destructive (plasma-workspace.target has 'start' job queued, but 'stop' is included in transaction).
            kcminit_startup[789]: "wl-shell" is a deprecated shell extension, prefer using "xdg-shell" if supported by the compositor by setting the environment variable QT_WAYLAND_SHELL_INTEGRATION
            kcminit_startup[789]: No shell integration named "ivi-shell" found
            kcminit_startup[789]: No shell integration named "qt-shell" found
            kcminit_startup[789]: Loading shell integration failed.
            kcminit_startup[789]: Attempted to load the following shells QList("xdg-shell", "wl-shell", "ivi-shell", "qt-shell")
            kcminit_startup[789]: Could not load the Qt platform plugin "wayland" in "" even though it was found.
            ksplashqml[788]: "wl-shell" is a deprecated shell extension, prefer using "xdg-shell" if supported by the compositor by setting the environment variable QT_WAYLAND_SHELL_INTEGRATION
            ksplashqml[788]: No shell integration named "ivi-shell" found
            ksplashqml[788]: No shell integration named "qt-shell" found
            ksplashqml[788]: Loading shell integration failed.
            ksplashqml[788]: Attempted to load the following shells QList("xdg-shell", "wl-shell", "ivi-shell", "qt-shell")
            lightdm[690]: pam_unix(lightdm:session): session closed for user aptivi
            lightdm[690]: pam_kwallet5(lightdm:session): pam_kwallet5: pam_sm_close_session
            lightdm[690]: pam_kwallet5(lightdm:setcred): pam_kwallet5: pam_sm_setcred
            kcminit_startup[789]: could not connect to display :0
            kcminit_startup[789]: From 6.5.0, xcb-cursor0 or libxcb-cursor0 is needed to load the Qt xcb platform plugin.
            kcminit_startup[789]: Could not load the Qt platform plugin "xcb" in "" even though it was found.
            kcminit_startup[789]: This application failed to start because no Qt platform plugin could be initialized. Reinstalling the application may fix this problem.

            Available platform plugins are: eglfs, linuxfb, minimal, minimalegl, offscreen, vkkhrdisplay, vnc, wayland-brcm, wayland-egl, wayland, xcb.
            systemd-logind[556]: Session 2 logged out. Waiting for processes to exit.
            ksplashqml[788]: Could not load the Qt platform plugin "wayland" in "" even though it was found.
            systemd-coredump[865]: Process 789 (kcminit_startup) of user 1000 terminated abnormally with signal 6/ABRT, processing...
            systemd[1]: Created slice Slice /system/drkonqi-coredump-processor.
            systemd[1]: Created slice Slice /system/systemd-coredump.
            systemd[1]: Started Process Core Dump (PID 865/UID 0).
            systemd[1]: Started Pass systemd-coredump journal entries to relevant user for potential DrKonqi handling.
            systemd-coredump[870]: Process 789 (kcminit_startup) of user 1000 dumped core.

             Stack trace of thread 789:
             #0  0x00007fba87a9894c n/a (libc.so.6 + 0x9894c)
             #1  0x00007fba87a3e410 raise (libc.so.6 + 0x3e410)
             #2  0x00007fba87a2557a abort (libc.so.6 + 0x2557a)
             #3  0x00007fba882934a4 n/a (libQt6Core.so.6 + 0x934a4)
             #4  0x00007fba88294268 _ZNK14QMessageLogger5fatalEPKcz (libQt6Core.so.6 + 0x94268)
             #5  0x00007fba88ae5fef n/a (libQt6Gui.so.6 + 0xe5fef)
             #6  0x00007fba88b9f1b8 _ZN22QGuiApplicationPrivate21createEventDispatcherEv (libQt6Gui.so.6 + 0x19f1b8)
             #7  0x00007fba8836ec85 _ZN23QCoreApplicationPrivate4initEv (libQt6Core.so.6 + 0x16ec85)
             #8  0x00007fba88b9f24e _ZN22QGuiApplicationPrivate4initEv (libQt6Gui.so.6 + 0x19f24e)
             #9  0x00007fba88ba040e _ZN15QGuiApplicationC2ERiPPci (libQt6Gui.so.6 + 0x1a040e)
             #10 0x000055f59969728f n/a (/usr/bin/kcminit + 0x328f)
             #11 0x00007fba87a27675 n/a (libc.so.6 + 0x27675)
             #12 0x00007fba87a27729 __libc_start_main (libc.so.6 + 0x27729)
             #13 0x000055f5996986a5 n/a (/usr/bin/kcminit + 0x46a5)

             Stack trace of thread 790:
             #0  0x00007fba87a9f042 n/a (libc.so.6 + 0x9f042)
             #1  0x00007fba87a931ac n/a (libc.so.6 + 0x931ac)
             #2  0x00007fba87a931f4 n/a (libc.so.6 + 0x931f4)
             #3  0x00007fba87b0da36 ppoll (libc.so.6 + 0x10da36)
             #4  0x00007fba87908784 n/a (libglib-2.0.so.0 + 0x60784)
             #5  0x00007fba87908865 g_main_context_iteration (libglib-2.0.so.0 + 0x60865)
             #6  0x00007fba88648152 _ZN20QEventDispatcherGlib13processEventsE6QFlagsIN10QEventLoop17ProcessEventsFlagEE (libQt6Core.so.6 + 0x448152)
             #7  0x00007fba88375786 _ZN10QEventLoop4execE6QFlagsINS_17ProcessEventsFlagEE (libQt6Core.so.6 + 0x175786)
             #8  0x00007fba8849041e _ZN7QThread4execEv (libQt6Core.so.6 + 0x29041e)
             #9  0x00007fba8936103e n/a (libQt6DBus.so.6 + 0x3703e)
             #10 0x00007fba8852f899 n/a (libQt6Core.so.6 + 0x32f899)
             #11 0x00007fba87a969cb n/a (libc.so.6 + 0x969cb)
             #12 0x00007fba87b1aa0c n/a (libc.so.6 + 0x11aa0c)
             ELF object binary architecture: AMD x86-64
            systemd[1]: systemd-coredump@0-1-865_866-0.service: Deactivated successfully.
            systemd[717]: Started KDE Config Module Initialization.
            systemd[717]: Starting KDE Session Management Server...
            systemd[717]: Started Unlock kwallet from pam credentials.
            systemd[717]: Starting KDE Daemon 6...
            kded6[885]: could not connect to display 
            kded6[885]: From 6.5.0, xcb-cursor0 or libxcb-cursor0 is needed to load the Qt xcb platform plugin.
            kded6[885]: Could not load the Qt platform plugin "xcb" in "" even though it was found.
            kded6[885]: This application failed to start because no Qt platform plugin could be initialized. Reinstalling the application may fix this problem.

                        Available platform plugins are: eglfs, linuxfb, minimal, minimalegl, offscreen, vkkhrdisplay, vnc, wayland-brcm, wayland-egl, wayland, xcb.
            systemd-coredump[887]: Process 885 (kded6) of user 1000 terminated abnormally with signal 6/ABRT, processing...
            systemd[1]: Started Process Core Dump (PID 887/UID 0).
            systemd[1]: Started Pass systemd-coredump journal entries to relevant user for potential DrKonqi handling.
            ksmserver[883]: could not connect to display 
            ksmserver[883]: From 6.5.0, xcb-cursor0 or libxcb-cursor0 is needed to load the Qt xcb platform plugin.
            ksmserver[883]: Could not load the Qt platform plugin "xcb" in "" even though it was found.
            ksmserver[883]: This application failed to start because no Qt platform plugin could be initialized. Reinstalling the application may fix this problem.

                            Available platform plugins are: eglfs, linuxfb, minimal, minimalegl, offscreen, vkkhrdisplay, vnc, wayland-brcm, wayland-egl, wayland, xcb.
            systemd-coredump[892]: Process 883 (ksmserver) of user 1000 terminated abnormally with signal 6/ABRT, processing...
            systemd[1]: Started Process Core Dump (PID 892/UID 0).
            systemd[1]: Started Pass systemd-coredump journal entries to relevant user for potential DrKonqi handling.
            drkonqi-coredump-processor[871]: "/usr/bin/kcminit" 789 "/var/lib/systemd/coredump/core.kcminit_startup.1000.6e1c833e1c5244239f851813cff8d072.789.1765886952000000.zst"
            systemd[717]: Started Launch DrKonqi for a systemd-coredump crash (PID 871/UID 0).
            drkonqi-coredump-launcher[898]: Unable to find file for pid 789 expected at "kcrash-metadata/kcminit.6e1c833e1c5244239f851813cff8d072.789.ini"
            drkonqi-coredump-launcher[898]: Nothing handled the dump :O
            """
        ];
    }
}
