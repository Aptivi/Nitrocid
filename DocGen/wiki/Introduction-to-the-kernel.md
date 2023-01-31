# Introduction to the Kernel

## What is the Kernel? How about Nitrocid KS?

The kernel in general is a computer program that is the core of a computer's operating system, with complete control over everything in the system. On most systems, it is one of the first programs loaded on start-up (after the bootloader). It handles the rest of start-up as well as input/output requests from software, translating them into data-processing instructions for the central processing unit. It handles memory and peripherals like keyboards, monitors, printers, and speakers.

However Nitrocid KS is the actual first Simulation, and not Emulation, app that can simulate these kinds of behaviors, except it's limited as we're still in the Alpha stage, and it's also showing you the transition on how is our kernel improved and built over time. Starting from 0.0.5.6, we have implemented support for kernel simulation under Unix systems. The first version is extremely limited, and so we can use it as an example on how the kernel was built. The first version can also be fit on the diskettes as it doesn't exceed 1.44MB floppy disk limits. You can even sell the floppy disk of our simulator to your local computing museum for everyone to see how the kernel was first built.

### The history of Unix and Linux.

#### What is Unix? and what happened in the past?

Unix is a family of multitasking, multiuser computer operating systems that derive from the original AT&T Unix, development starting in the 1970s at the Bell Labs research center by Ken Thompson, Dennis Ritchie, and others.

Initially intended for use inside the Bell System, AT&T licensed Unix to outside parties in the late 1970s, leading to a variety of both academic and commercial Unix variants from vendors including University of California, Berkeley (BSD), Microsoft (Xenix), IBM (AIX), and Sun Microsystems (Solaris). In the early 1990s, AT&T sold its rights in Unix to Novell, which then sold its Unix business to the Santa Cruz Operation (SCO) in 1995. The UNIX trademark passed to The Open Group, a neutral industry consortium, which allows the use of the mark for certified operating systems that comply with the Single UNIX Specification (SUS). As of 2014, the Unix version with the largest installed base is Apple's macOS.

The origins of Unix date back to the mid-1960s when the Massachusetts Institute of Technology, Bell Labs, and General Electric were developing Multics, a time-sharing operating system for the GE-645 mainframe computer. Multics featured several innovations, but also presented severe problems. Frustrated by the size and complexity of Multics, but not by its goals, individual researchers at Bell Labs started withdrawing from the project. The last to leave were Ken Thompson, Dennis Ritchie, Douglas McIlroy, and Joe Ossanna, who decided to reimplement their experiences in a new project of smaller scale. This new operating system was initially without organizational backing, and also without a name.

The new operating system was a single-tasking system. In 1970, the group coined the name Unics for Uniplexed Information and Computing Service (pronounced eunuchs), as a pun on Multics, which stood for Multiplexed Information and Computer Services. Brian Kernighan takes credit for the idea, but adds that no one can remember the origin of the final spelling Unix. Dennis Ritchie, Doug McIlroy, and Peter G. Neumann also credit Kernighan.

The operating system was originally written in assembly language, but in 1973, Version 4 Unix was rewritten in C. Version 4 Unix, however, still had many PDP-11 dependent codes, and is not suitable for porting. The first port to other platform was made five years later (1978) for Interdata 8/32.

Bell Labs produced several versions of Unix that are collectively referred to as Research Unix. In 1975, the first source license for UNIX was sold to Donald B. Gillies at the University of Illinois Department of Computer Science. UIUC graduate student Greg Chesson, who had worked on the UNIX kernel at Bell Labs, was instrumental in negotiating the terms of the license.

During the late 1970s and early 1980s, the influence of Unix in academic circles led to large-scale adoption of Unix (BSD and System V) by commercial startups, including Sequent, HP-UX, Solaris, AIX, and Xenix. In the late 1980s, AT&T Unix System Laboratories and Sun Microsystems developed System V Release 4 (SVR4), which was subsequently adopted by many commercial Unix vendors.

In the 1990s, Unix and Unix-like systems grew in popularity as BSD and Linux distributions were developed through collaboration by a worldwide network of programmers. In 2000, Apple released Darwin, also a Unix system, which became the core of the Mac OS X operating system, which was later renamed macOS.

#### How about Linux?

Linux is a family of free and open-source software operating systems built around the Linux kernel. Typically, Linux is packaged in a form known as a Linux distribution (or distro for short) for both desktop and server use. The defining component of a Linux distribution is the Linux kernel, an operating system kernel first released on September 17, 1991, by Linus Torvalds.

In 1991, while attending the University of Helsinki, Torvalds became curious about operating systems. Frustrated by the licensing of MINIX, which at the time limited it to educational use only, he began to work on his own operating system kernel, which eventually became the Linux kernel.

Torvalds began the development of the Linux kernel on MINIX and applications written for MINIX were also used on Linux. Later, Linux matured and further Linux kernel development took place on Linux systems. GNU applications also replaced all MINIX components, because it was advantageous to use the freely available code from the GNU Project with the fledgling operating system; code licensed under the GNU GPL can be reused in other computer programs as long as they also are released under the same or a compatible license. Torvalds initiated a switch from his original license, which prohibited commercial redistribution, to the GNU GPL. Developers worked to integrate GNU components with the Linux kernel, making a fully functional and free operating system.

Linus Torvalds had wanted to call his invention Freax, a portmanteau of free, freak, and x (as an allusion to Unix). During the start of his work on the system, some of the project's makefiles included the name Freax for about half a year. Torvalds had already considered the name Linux, but initially dismissed it as too egotistical.

In order to facilitate development, the files were uploaded to the FTP server (ftp.funet.fi) of FUNET in September 1991. Ari Lemmke, Torvalds' coworker at the Helsinki University of Technology (HUT), who was one of the volunteer administrators for the FTP server at the time, did not think that Freax was a good name. So, he named the project Linux on the server without consulting Torvalds. Later, however, Torvalds consented to Linux.

Adoption of Linux in production environments, rather than being used only by hobbyists, started to take off first in the mid-1990s in the supercomputing community, where organizations such as NASA started to replace their increasingly expensive machines with clusters of inexpensive commodity computers running Linux. Commercial use began when Dell and IBM, followed by Hewlett-Packard, started offering Linux support to escape Microsoft's monopoly in the desktop operating system market.

Today, Linux systems are used throughout computing, from embedded systems to virtually all supercomputers, and have secured a place in server installations such as the popular LAMP application stack. Use of Linux distributions in home and enterprise desktops has been growing. Linux distributions have also become popular in the netbook market, with many devices shipping with customized Linux distributions installed, and Google releasing their own Chrome OS designed for netbooks.

Linux's greatest success in the consumer market is perhaps the mobile device market, with Android being one of the most dominant operating systems on smartphones and very popular on tablets and, more recently, on wearables. Linux gaming is also on the rise with Valve showing its support for Linux and rolling out its own gaming oriented Linux distribution. Linux distributions have also gained popularity with various local and national governments, such as the federal government of Brazil.

Greg Kroah-Hartman is the lead maintainer for the Linux kernel and guides its development. Stallman heads the Free Software Foundation, which in turn supports the GNU components. Finally, individuals and corporations develop third-party non-GNU components. These third-party components comprise a vast body of work and may include both kernel modules and user applications and libraries.

Linux vendors and communities combine and distribute the kernel, GNU components, and non-GNU components, with additional package management software in the form of Linux distributions.

#### Brief description for our Kernel aka Aptivi Kernel

The kernel is built from scratch, and the first public Simulator version was built at 23 February, 2018. Currently, the Simulator grabs hardware information via Windows Management Instrumentation (WMI) or files in Linux which provides every information for your hardware, including the processor, RAM, and hard drive. It provides a built-in shell named Unified Eofla SHell (UESH) that will later be a kernel initialization program as known as InitiaOE which runs UESH if run on multiuser and uesh-maint if run on maintenance mode.

Currently, it has a debugging feature that debugs the kernel on what's going on and saves it to a file. The debugger is useful to track issues with the kernel.

## Sources

1. [https://en.wikipedia.org/wiki/Kernel_(operating_system)](https://en.wikipedia.org/wiki/Kernel_(operating_system))
2. [https://en.wikipedia.org/wiki/Unix](https://en.wikipedia.org/wiki/Unix)
3. [https://en.wikipedia.org/wiki/Linux](https://en.wikipedia.org/wiki/Linux)
