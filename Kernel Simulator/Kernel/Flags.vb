
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Module Flags

    'Variables
    Public ProbeFlag As Boolean = True                          'Check to see if the hardware can be probed
    Public GPUProbeFlag As Boolean = False                      'No GPU probe (Probe GPU = 'gpuprobe' kernel argument)
    Public Quiet As Boolean = False                             'Quiet mode
    Public TimeDateIsSet As Boolean = False                     'To fix a bug after reboot
    Public StopPanicAndGoToDoublePanic As Boolean               'Double panic mode in kernel error
    Public DebugMode As Boolean = False                         'Toggle Debugging mode
    Public LoginFlag As Boolean                                 'Flag for log-in
    Public MainUserDone As Boolean                              'Main users initialization is done
    Public CommandFlag As Boolean = False                       'A signal for command kernel argument
    Public templateSetExitFlag As Boolean = False               'A signal for checking if the template was set
    Public CruserFlag As Boolean = False                        'A signal to the kernel where user has to be created
    Public argsFlag As Boolean                                  'A flag for checking for an argument later
    Public argsInjected As Boolean                              'A flag for checking for an argument on reboot
    Public customColor As Boolean = False                       'Enable custom colors
    Public enableDemo As Boolean = True                         'Enable Demo Account
    Public setRootPasswd As Boolean = False                     'Set Root Password
    Public RootPasswd As String = ""                            'Set Root Password
    Public maintenance As Boolean = False                       'Maintenance Mode
    Public argsOnBoot As Boolean = False                        'Arguments On Boot
    Public clsOnLogin As Boolean = False                        'Clear Screen On Log-in
    Public showMOTD As Boolean = True                           'Show MOTD on log-in
    Public simHelp As Boolean = False                           'Simplified Help Command
    Public slotProbe As Boolean = True                          'Probe slots
    Public quietProbe As Boolean = False                        'Probe quietly

End Module
