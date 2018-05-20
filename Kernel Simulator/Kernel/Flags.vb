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
