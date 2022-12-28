#!/bin/sh

#    Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

unrarpath=`which unrar`
unrarexistent=$?
isppa=0

# Run the entry point
if [ -e "/usr/lib/ks/Kernel Simulator.exe" ]; then
	isppa=1
	mono "/usr/lib/ks/Kernel Simulator.exe" $@
elif [ -e "./Kernel Simulator.exe" ]; then
	mono "./Kernel Simulator.exe" $@
else
	echo "Unable to find the entry point."
	exit 1
fi

# Check to see if we're on the PPA
if [ isppa == 0 ]; then
	# Check to see if we have unrar and update.rar
	if [ ! $unrarexistent == 0 ]; then
		echo unrar is not found.
		exit 1
	fi
	if [ -e "./update.rar" ]; then
		echo Update found. Installing...
		"$unrarpath" x -ep1 -r "./update.rar"
	fi
fi
