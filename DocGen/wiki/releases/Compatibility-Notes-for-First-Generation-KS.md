## Compatibility notes for first-gen KS

The first-generation versions of Nitrocid KS are not compatible with the second-generation versions, due to too many structural changes. Here are the breaking changes per generation.

During the lifecycle of Nitrocid KS, new versions tend to appear frequently. This causes some of the functions and APIs that we think are no longer relevant to be deleted. It doesn't list fields and screensaver codes, although they may be part of the API, because they're out of scope for this document.

### How to Read the List

It starts with the function name and where and which version range of KS it is found. It'll then tell you why it's deleted and if it's going to come back (likeliness). If said function or API has came back, it'll tell you which version, which function, and which file has it. The format is as follows:

- Name: DeletedFunction()
- Source file: SomeFile.vb
- Version range: 0.0.x.x - 0.0.y.y, where 0.0.x.x first implemented the function and 0.0.y.y doesn't have it.
- Reason for deletion: Some reason
- Likeliness to come back: Highly unlikely because blah.
- Returning version: 0.0.z.z
- Returning name: RestoredFunction()
- Returning file: SomeOtherFile.vb
- Returning mod example name: SomeMod
- Returning mod example function name: RestoredFunction()

### Breaking API changes (First-generation, Revision 0)

Here are the removed functions:

#### 0.0.4

##### ResetTimeDate()

- Name: ResetTimeDate()
- Source file: TimeDate.vb
- Version range: 0.0.3 - 0.0.4
- Reason for deletion: Unnecessary
- Likeliness to come back: Highly unlikely.

##### permissionEdit()

- Name: permissionEdit()
- Source file: Groups.vb
- Version range: 0.0.3 - 0.0.4
- Reason for deletion: Permission() can already do its job
- Likeliness to come back: Highly unlikely.

##### ProbeBIOS()

- Name: ProbeBIOS()
- Source file: HardwareProbe.vb
- Version range: 0.0.2.3 - 0.0.4
- Reason for deletion: Wrapper for BiosInformation()
- Likeliness to come back: Highly unlikely.

#### 0.0.4.5

##### ShowTimeQuiet()

- Name: ShowTimeQuiet()
- Source file: TimeDate.vb
- Version range: 0.0.2 - 0.0.4.5
- Reason for deletion: Wrapper for ShowTime()
- Likeliness to come back: Highly unlikely.

#### 0.0.5 beta (0.0.4.12)

##### DiscoSystem()

- Name: DiscoSystem()
- Source file: GetCommand.vb
- Version range: 0.0.1 - 0.0.4.12
- Reason for deletion: Disco moved to screensaver
- Likeliness to come back: Highly unlikely.

#### 0.0.5

##### BeepFreq()

- Name: BeepFreq()
- Source file: Beep.vb
- Version range: 0.0.1 - 0.0.5
- Reason for deletion: Prompts removed at that time
- Likeliness to come back: Highly unlikely. 

##### BeepSystem()

- Name: BeepSystem()
- Source file: Beep.vb
- Version range: 0.0.1 - 0.0.5
- Reason for deletion: Prompts removed at that time
- Likeliness to come back: Highly unlikely. 

##### CheckNetworkKernel()

- Name: CheckNetworkKernel()
- Source file: Network.vb
- Version range: 0.0.2 - 0.0.5
- Reason for deletion: chkn=1 kernel argument removed and prompts removed at that time
- Likeliness to come back: Highly unlikely.

##### CheckNetworkCommand()

- Name: CheckNetworkCommand()
- Source file: Network.vb
- Version range: 0.0.2 - 0.0.5
- Reason for deletion: chkn=1 kernel argument removed and prompts removed at that time
- Likeliness to come back: Highly unlikely.

##### PingTargetKernel()

- Name: PingTargetKernel()
- Source file: Network.vb
- Version range: 0.0.2 - 0.0.5
- Reason for deletion: chkn=1 kernel argument removed and prompts removed at that time
- Likeliness to come back: Highly unlikely.

##### panicPrompt()

- Name: panicPrompt()
- Source file: PanicSim.vb
- Version range: 0.0.4 - 0.0.5
- Reason for deletion: Considered as abusive due to being included in normal shell since 0.0.1
- Likeliness to come back: Highly unlikely.

##### changeName()

- Name: changeName()
- Source file: UserManagement.vb
- Version range: 0.0.3 - 0.0.5
- Reason for deletion: Wrapper for change name prompt
- Likeliness to come back: Likely.
- Returning version: 0.0.11
- Returning name: ChangeUsername()
- Returning file: UserManagement.vb

##### changePassword()

- Name: changePassword()
- Source file: UserManagement.vb
- Version range: 0.0.2 - 0.0.5
- Reason for deletion: Wrapper for change password prompt
- Likeliness to come back: Highly unlikely.

##### changePasswordPrompt()

- Name: changePasswordPrompt()
- Source file: UserManagement.vb
- Version range: 0.0.2 - 0.0.5
- Reason for deletion: Accomplice of the above function
- Likeliness to come back: Likely.
- Returning version: 0.0.12
- Returning name: ChangePassword()
- Returning file: UserManagement.vb

##### removeUser()

- Name: removeUser()
- Source file: UserManagement.vb
- Version range: 0.0.3 - 0.0.5
- Reason for deletion: Wrapper for removeUserFromDatabase(), and prompts removed at that time
- Likeliness to come back: Likely.

##### addUser()

- Name: addUser()
- Source file: UserManagement.vb
- Version range: 0.0.3 - 0.0.5
- Reason for deletion: Prompts removed at that time
- Likeliness to come back: Likely.

##### newPassword()

- Name: newPassword()
- Source file: UserManagement.vb
- Version range: 0.0.4 - 0.0.5
- Reason for deletion: Prompts removed at that time
- Likeliness to come back: Highly unlikely.

##### UseDefaults()

- Name: UseDefaults()
- Source file: ColorSet.vb
- Version range: 0.0.3 - 0.0.5
- Reason for deletion: Prompts removed at that time
- Likeliness to come back: Highly unlikely.

##### SetColorSteps()

- Name: SetColorSteps()
- Source file: ColorSet.vb
- Version range: 0.0.3 - 0.0.5
- Reason for deletion: Prompts removed at that time
- Likeliness to come back: Highly unlikely.

##### advanceStep()

- Name: advanceStep()
- Source file: ColorSet.vb
- Version range: 0.0.3 - 0.0.5
- Reason for deletion: Prompts removed at that time
- Likeliness to come back: Highly unlikely.

##### TemplatePrompt()

- Name: TemplatePrompt()
- Source file: TemplateSet.vb
- Version range: 0.0.4 - 0.0.5
- Reason for deletion: Prompts removed at that time
- Likeliness to come back: Highly unlikely.

#### 0.0.5.1

##### permissionPrompt()

- Name: permissionPrompt()
- Source file: Groups.vb
- Version range: 0.0.3 - 0.0.5.1
- Reason for deletion: Prompts removed at that time
- Likeliness to come back: Highly unlikely.

##### permissionEditingPrompt()

- Name: permissionEditingPrompt()
- Source file: Groups.vb
- Version range: 0.0.3 - 0.0.5.1
- Reason for deletion: Prompts removed at that time
- Likeliness to come back: Highly unlikely.

#### 0.0.6 beta (0.0.5.9)

##### initializeMainUsers()

- Name: initializeMainUsers()
- Source file: Login.vb
- Version range: 0.0.1 - 0.0.5.9
- Reason for deletion: No longer needed
- Likeliness to come back: Likely
- Returning version: 0.0.12
- Returning name: InitializeSystemAccount()
- Returning file: UserManagement.vb

#### 0.0.6 beta (0.0.5.11)

##### ReadLineWithNewLine()

- Name: ReadLineWithNewLine()
- Source file: StreamReaderExtensions.vb
- Version range: 0.0.5.9 - 0.0.5.11
- Reason for deletion: Unused.
- Likeliness to come back: Highly unlikely.
- Returning version: v2021.5 (WIP)
- Returning name: ReadLineWithNewLine()
- Returning file: StreamReader.vb (Extensification)

##### ReadyPath_MOD()

- Name: ReadyPath_MOD()
- Source file: ModParser.vb
- Version range: 0.0.5.9 - 0.0.5.11
- Reason for deletion: Unnecessary.
- Likeliness to come back: Highly unlikely.

#### 0.0.6 beta (0.0.5.12)

##### ProbeGPU()

- Name: ProbeGPU()
- Source file: Kernel.vb
- Version range: 0.0.3 - 0.0.5.12
- Reason for deletion: Moved to ProbeHardware()
- Likeliness to come back: Highly unlikely.

##### Hddinfo()

- Name: Hddinfo()
- Source file: Kernel.vb
- Version range: 0.0.1 - 0.0.5.12
- Reason for deletion: Moved to ProbeHardware()
- Likeliness to come back: Highly unlikely.

##### Cpuinfo()

- Name: Cpuinfo()
- Source file: Kernel.vb
- Version range: 0.0.1 - 0.0.5.12
- Reason for deletion: Moved to ProbeHardware()
- Likeliness to come back: Highly unlikely.

##### SysMemory()

- Name: SysMemory()
- Source file: Kernel.vb
- Version range: 0.0.1 - 0.0.5.12
- Reason for deletion: Moved to ProbeHardware()
- Likeliness to come back: Highly unlikely.

##### BiosInformation()

- Name: BiosInformation()
- Source file: Kernel.vb
- Version range: 0.0.1 - 0.0.5.12
- Reason for deletion: Moved to ProbeHardware()
- Likeliness to come back: Highly unlikely.

#### 0.0.6

##### RespondPreWriteToDebugger()

- Name: RespondPreWriteToDebugger()
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.6
- Reason for deletion: For writing events, the "Wdbg" instruction has been removed because it's spamming the debug log and makes the kernel even slower.
- Likeliness to come back: Highly unlikely.

##### RespondPostWriteToDebugger()

- Name: RespondPostWriteToDebugger()
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.6
- Reason for deletion: For writing events, the "Wdbg" instruction has been removed because it's spamming the debug log and makes the kernel even slower.
- Likeliness to come back: Highly unlikely.

##### RespondPreWriteToConsole()

- Name: RespondPreWriteToConsole()
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.6
- Reason for deletion: For writing events, the "Wdbg" instruction has been removed because it's spamming the debug log and makes the kernel even slower.
- Likeliness to come back: Highly unlikely.

##### RespondPostWriteToConsole()

- Name: RespondPostWriteToConsole()
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.6
- Reason for deletion: For writing events, the "Wdbg" instruction has been removed because it's spamming the debug log and makes the kernel even slower.
- Likeliness to come back: Highly unlikely.

##### RaisePreWriteToDebugger()

- Name: RaisePreWriteToDebugger()
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.6
- Reason for deletion: For writing events, the "Wdbg" instruction has been removed because it's spamming the debug log and makes the kernel even slower.
- Likeliness to come back: Highly unlikely.

##### RaisePostWriteToDebugger()

- Name: RaisePostWriteToDebugger()
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.6
- Reason for deletion: For writing events, the "Wdbg" instruction has been removed because it's spamming the debug log and makes the kernel even slower.
- Likeliness to come back: Highly unlikely.

##### RaisePreWriteToConsole()

- Name: RaisePreWriteToConsole()
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.6
- Reason for deletion: For writing events, the "Wdbg" instruction has been removed because it's spamming the debug log and makes the kernel even slower.
- Likeliness to come back: Highly unlikely.

##### RaisePostWriteToConsole()

- Name: RaisePostWriteToConsole()
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.6
- Reason for deletion: For writing events, the "Wdbg" instruction has been removed because it's spamming the debug log and makes the kernel even slower.
- Likeliness to come back: Highly unlikely.

##### ResetUsers()

- Name: ResetUsers()
- Source file: UserManagement.vb
- Version range: 0.0.5.13 - 0.0.6
- Reason for deletion: Merged to ResetEverything()
- Likeliness to come back: Highly unlikely.

##### GetAllCurrencies()

- Name: GetAllCurrencies()
- Source file: UnitConv.vb
- Version range: 0.0.5.9 - 0.0.6
- Reason for deletion: Changes in the free.currencyconverterapi.com to paid API
- Likeliness to come back: Unlikely.

#### 0.0.6.6

##### CurrencyConvert()

- Name: CurrencyConvert()
- Source file: UnitConv.vb
- Version range: 0.0.5.9 - 0.0.6.6
- Reason for deletion: Changes in the free.currencyconverterapi.com to paid API
- Likeliness to come back: Unlikely.

#### 0.0.7 beta (0.0.6.9)

##### ExpressionCalculate()

- Name: ExpressionCalculate()
- Source file: SciCalc.vb and StdCalc.vb
- Version range: 0.0.5.13 - 0.0.6.9
- Reason for deletion: Limited expressions support
- Likeliness to come back: Likely.
- Returning version: 0.0.12
- Returning name: DoCalc()
- Returning file: Calc.vb

##### Converter()

- Name: Converter()
- Source file: UnitConv.vb
- Version range: 0.0.4.1 - 0.0.6.9
- Reason for deletion: Hard to maintain
- Likeliness to come back: Likely.

##### Wln()

- Name: Wln()
- Source file: TextWriterColor.vb
- Version range: 0.0.4 - 0.0.6.9
- Reason for deletion: Merged to W()
- Likeliness to come back: Highly unlikely.

##### ReadImportantConfig()

- Name: ReadImportantConfig()
- Source file: Config.vb
- Version range: 0.0.5.13 - 0.0.6.9
- Reason for deletion: ReadConfig() already does its job
- Likeliness to come back: Highly unlikely.

##### GenModCS()

- Name: GenModCS()
- Source file: ModParser.vb
- Version range: 0.0.6.2 - 0.0.6.9
- Reason for deletion: Merged to GenMod()
- Likeliness to come back: Highly unlikely.

#### 0.0.7

##### Class Manual

- Name: Manual
- Source file: Manual.vb
- Version range: 0.0.5.9 - 0.0.7
- Reason for deletion: Hard to maintain
- Likeliness to come back: Likely.

##### InitMan()

- Name: InitMan()
- Source file: PageParser.vb
- Version range: 0.0.6.5 - 0.0.7
- Reason for deletion: Hard to maintain
- Likeliness to come back: Likely.

##### CheckManual()

- Name: CheckManual()
- Source file: PageParser.vb
- Version range: 0.0.5.9 - 0.0.7
- Reason for deletion: Hard to maintain
- Likeliness to come back: Likely.

##### CheckTODO()

- Name: CheckTODO()
- Source file: PageParser.vb
- Version range: 0.0.5.9 - 0.0.7
- Reason for deletion: Hard to maintain
- Likeliness to come back: Likely.

##### ParseMan_INTERNAL()

- Name: ParseMan_INTERNAL()
- Source file: PageParser.vb
- Version range: 0.0.5.9 - 0.0.7
- Reason for deletion: Hard to maintain
- Likeliness to come back: Highly unlikely.

##### ParseMan_EXTERNAL()

- Name: ParseMan_EXTERNAL()
- Source file: PageParser.vb
- Version range: 0.0.5.9 - 0.0.7
- Reason for deletion: Hard to maintain
- Likeliness to come back: Likely.

##### ParseBody()

- Name: ParseBody()
- Source file: PageParser.vb
- Version range: 0.0.5.9 - 0.0.7
- Reason for deletion: Hard to maintain
- Likeliness to come back: Likely.

##### ParseColor()

- Name: ParseColor()
- Source file: PageParser.vb
- Version range: 0.0.5.9 - 0.0.7
- Reason for deletion: Hard to maintain
- Likeliness to come back: Likely.

##### ParseSection()

- Name: ParseSection()
- Source file: PageParser.vb
- Version range: 0.0.5.9 - 0.0.7
- Reason for deletion: Hard to maintain
- Likeliness to come back: Likely.

##### Sanity_INTERNAL()

- Name: Sanity_INTERNAL()
- Source file: PageParser.vb
- Version range: 0.0.5.9 - 0.0.7
- Reason for deletion: Hard to maintain
- Likeliness to come back: Highly unlikely.

##### Sanity_EXTERNAL()

- Name: Sanity_EXTERNAL()
- Source file: PageParser.vb
- Version range: 0.0.5.9 - 0.0.7
- Reason for deletion: Hard to maintain
- Likeliness to come back: Likely.

##### ViewPage()

- Name: ViewPage()
- Source file: PageViewer.vb
- Version range: 0.0.5.9 - 0.0.7
- Reason for deletion: Hard to maintain
- Likeliness to come back: Likely.

##### WriteInfo()

- Name: WriteInfo()
- Source file: PageViewer.vb
- Version range: 0.0.5.9 - 0.0.7
- Reason for deletion: Hard to maintain
- Likeliness to come back: Likely.

##### ListLocal()

- Name: ListLocal()
- Source file: FTPTools.vb
- Version range: 0.0.5.5 - 0.0.7
- Reason for deletion: We already have List() who can do this job
- Likeliness to come back: Highly unlikely.

##### PingTarget()

- Name: PingTarget()
- Source file: NetworkTools.vb
- Version range: 0.0.2 - 0.0.7
- Reason for deletion: Using the My.Computer API
- Likeliness to come back: Very likely.
- Returning version: 0.0.12
- Returning name: PingAddress()
- Returning file: NetworkTools.vb

##### ListOnlineAndOfflineHosts()

- Name: ListOnlineAndOfflineHosts()
- Source file: NetworkTools.vb
- Version range: 0.0.4.10 - 0.0.7
- Reason for deletion: Incompatible with Unix and is error-prone
- Likeliness to come back: Highly unlikely.

##### ListHostsInNetwork()

- Name: ListHostsInNetwork()
- Source file: NetworkTools.vb
- Version range: 0.0.2 - 0.0.7
- Reason for deletion: Incompatible with Unix and is error-prone
- Likeliness to come back: Highly unlikely.

##### GetNetworkComputers()

- Name: GetNetworkComputers()
- Source file: NetworkTools.vb
- Version range: 0.0.2 - 0.0.7
- Reason for deletion: Incompatible with Unix and is error-prone
- Likeliness to come back: Highly unlikely.

##### ListHostsInTree()

- Name: ListHostsInTree()
- Source file: NetworkTools.vb
- Version range: 0.0.2 - 0.0.7
- Reason for deletion: Incompatible with Unix and is error-prone
- Likeliness to come back: Highly unlikely.

### Breaking API changes (First-generation, Revision 1)

Here are the removed functions:

#### 0.0.8

##### CheckSSEs()

- Name: CheckSSEs()
- Source file: CPUFeatures.vb
- Version range: 0.0.6.9 - 0.0.8
- Reason for deletion: Removal of sses command
- Likeliness to come back: Highly unlikely.

#### 0.0.8.5

##### InitStructure()

- Name: InitStructure()
- Source file: Filesystem.vb
- Version range: 0.0.6 - 0.0.8.5
- Reason for deletion: Abusive
- Likeliness to come back: Highly unlikely.

##### UACNoticeShow()

- Name: UACNoticeShow()
- Source file: Filesystem.vb
- Verison range: 0.0.8 - 0.0.8.5
- Reason for deletion: Part of abusive API, InitStructure()
- Likeliness to come back: Highly unlikely.

#### 0.0.10

##### DisconnectDbgDevCmd()

- Name: DisconnectDbgDevCmd()
- Source file: RemoteDebugger.vb
- Version range: 0.0.7.1 - 0.0.10
- Reason for deletion: Wrapper to DisconnectDbgDev() with extra prints
- Likeliness to come back: Highly unlikely.

#### 0.0.11

##### InitFS()

- Name: InitFS()
- Source file: Filesystem.vb
- Version range: 0.0.6.9 - 0.0.11
- Reason for deletion: Wrapper for a one-liner
- Likeliness to come back: Highly unlikely.

##### Class FTPNotEnoughArgumentsException

- Name: FTPNotEnoughArgumentsException
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.11
- Reason for deletion: Unused since implementation
- Likeliness to come back: Likely.

##### Class JsonNullException

- Name: JsonNullException
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.11
- Reason for deletion: Ununsed since Converter() deletion
- Likeliness to come back: Likely.

##### Class TruncatedManpageException

- Name: TruncatedManpageException
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.11
- Reason for deletion: Unused since 0.0.7
- Likeliness to come back: Highly unlikely.

##### Speak()

- Name: Speak()
- Source file: VoiceManagement.vb
- Version range: 0.0.8 - 0.0.11
- Reason for deletion: See "Truth about Sound Libraries" for more details
- Likeliness to come back: Highly unlikely.

### Breaking API changes (First-generation, Revision 2)

Here are the removed functions:

#### 0.0.12

##### ProbeHW()

- Name: ProbeHW()
- Source file: HardwareProbe.vb
- Version range: 0.0.2.3 - 0.0.12
- Reason for deletion: Entry-point wrapper to StartProbing()
- Likeliness to come back: Highly unlikely.

##### RespondPreFetchNetworks()

- Name: RespondPreFetchNetworks()
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.12
- Reason for deletion: Event handler to non-existent APIs ListOnlineAndOfflineHosts, ListHostsInNetwork, GetNetworkComputers, and ListHostsInTree
- Likeliness to come back: Highly unlikely.

##### RespondPostFetchNetworks()

- Name: RespondPostFetchNetworks()
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.12
- Reason for deletion: Event handler to non-existent APIs ListOnlineAndOfflineHosts, ListHostsInNetwork, GetNetworkComputers, and ListHostsInTree
- Likeliness to come back: Highly unlikely.

##### RaisePreFetchNetworks()

- Name: RaisePreFetchNetworks()
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.12
- Reason for deletion: Event handler to non-existent APIs ListOnlineAndOfflineHosts, ListHostsInNetwork, GetNetworkComputers, and ListHostsInTree
- Likeliness to come back: Highly unlikely.

##### RaisePostFetchNetworks()

- Name: RaisePostFetchNetworks()
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.12
- Reason for deletion: Event handler to non-existent APIs ListOnlineAndOfflineHosts, ListHostsInNetwork, GetNetworkComputers, and ListHostsInTree
- Likeliness to come back: Highly unlikely.

##### ReplaceLastOccurrence()

- Name: ReplaceLastOccurrence()
- Source file: StringExtensions.vb
- Version range: 0.0.8.0 - 0.0.12
- Reason for deletion: Merged to Extensification
- Likeliness to come back: Very likely.
- Returning version: v2020.0
- Returning name: ReplaceLastOccurrence()
- Returning file: String.vb (Extensification)

##### AllIndexesOf()

- Name: AllIndexesOf()
- Source file: StringExtensions.vb
- Version range: 0.0.8.0 - 0.0.12
- Reason for deletion: Merged to Extensification
- Likeliness to come back: Very likely.
- Returning version: v2020.0
- Returning name: AllIndexesOf()
- Returning file: String.vb (Extensification)

##### Truncate()

- Name: Truncate()
- Source file: StringExtensions.vb
- Version range: 0.0.8.0 - 0.0.12
- Reason for deletion: Merged to Extensification
- Likeliness to come back: Very likely.
- Returning version: v2020.0
- Returning name: Truncate()
- Returning file: String.vb (Extensification)

#### 0.0.12.3

##### ListenRPC()

- Name: ListenRPC()
- Source file: RemoteProcedure.vb
- Version range: 0.0.8 - 0.0.12.3
- Reason for deletion: Migrated to StartRPC()
- Likeliness to come back: Highly unlikely.

#### 0.0.14

##### All classes in HardwareVars.vb

- Version range: 0.0.5.6 - 0.0.14
- Reason for deletion: Migrated to Inxi.NET
- Likeliness to come back: Very likely.
- Returning version: v2020.0
- Returning name: See Inxi.NET repo
- Returning file: See Inxi.NET repo

##### ProbeHardware()

- Name: ProbeHardware()
- Source file: HardwareProbe.vb
- Version range: 0.0.5.12 - 0.0.14
- Reason for deletion: Migrated to Inxi.NET
- Likeliness to come back: Very likely.
- Returning version: v2020.0
- Returning name: See Inxi.NET repo
- Returning file: See Inxi.NET repo

##### ProbeHardwareLinux()

- Name: ProbeHardwareLinux()
- Source file: HardwareProbe.vb
- Version range: 0.0.5.13 - 0.0.14
- Reason for deletion: Migrated to Inxi.NET
- Likeliness to come back: Very likely.
- Returning version: v2020.0
- Returning name: See Inxi.NET repo
- Returning file: See Inxi.NET repo

##### ListDrivers_Linux()

- Name: ListDrivers_Linux()
- Source file: HardwareProbe.vb
- Version range: 0.0.5.13 - 0.0.14
- Reason for deletion: Migrated to Inxi.NET
- Likeliness to come back: Highly unlikely.

##### PrintDrives()

- Name: PrintDrives()
- Source file: PrintHDDInfo.vb
- Version range: 0.0.12 - 0.0.14
- Reason for deletion: Migrated to Inxi.NET
- Likeliness to come back: Highly unlikely.

##### PrintPartitions()

- Name: PrintPartitions
- Source file: PrintHDDInfo.vb
- Version range: 0.0.12 - 0.0.14
- Reason for deletion: Migrated to Inxi.NET
- Likeliness to come back: Highly unlikely.

##### Class CPUFeatures_Win

- Name: CPUFeatures_Win
- Source file: CPUFeatures.vb
- Version range: 0.0.8 - 0.0.14
- Reason for deletion: Migrated to Inxi.NET
- Likeliness to come back: Highly unlikely.

##### CheckSSE()

- Name: CheckSSE()
- Source file: CPUFeatures.vb
- Version range: 0.0.8 - 0.0.14
- Reason for deletion: Migrated to Inxi.NET
- Likeliness to come back: Highly unlikely.

##### DoCalc()

- Name: DoCalc()
- Source file: Calc.vb
- Version range: 0.0.8 - 0.0.14
- Reason for deletion: In favor of string evaluator
- Likeliness to come back: Highly unlikely.

#### 0.0.15

##### PrintLog()

- Name: PrintLog()
- Source file: DebugLogPrint.vb
- Version range: 0.0.8 - 0.0.15
- Reason for deletion: Unnecessary
- Likeliness to come back: Highly unlikely.

### Breaking API changes (First-generation, Revision 3)

Here are the removed functions:

#### 0.0.16

##### TemplateSet()

- Name: TemplateSet()
- Source file: Color.vb
- Version range: 0.0.4 - 0.0.16
- Reason for deletion: Dynamic themes
- Likeliness to come back: Very likely.
- Returning version: 0.0.16
- Returning name: ApplyThemeFromResources()
- Returning file: Color.vb

##### ParseCurrentTheme()

- Name: ParseCurrentTheme()
- Source file: Color.vb
- Version range: 0.0.5.7 - 0.0.16
- Reason for deletion: Dynamic themes
- Likeliness to come back: Unlikely.

##### ListDrivers()

- Name: ListDrivers()
- Source file: HardwareProbe.vb
- Version range: 0.0.3 - 0.0.16
- Reason for deletion: Duplicate of ListHardware() since 0.0.16
- Likeliness to come back: Highly unlikely.

##### Class NotEnoughArgumentsException

- Name: NotEnoughArgumentsException
- Source file: EventsAndExceptions.vb
- Version range: 0.0.5.9 - 0.0.16
- Reason for deletion: Harmful to throw exception on not enough arguments supplied
- Likeliness to come back: Highly unlikely.

##### Class InvalidSynthException

- Name: InvalidSynthException
- Source file: EventsAndExceptions.vb
- Version range: 0.0.12 - 0.0.16
- Reason for deletion: Beep synth removed
- Likeliness to come back: Highly unlikely.

##### GetCultureFromLang()

- Name: GetCultureFromLang()
- Source file: Translate.vb
- Version range: 0.0.12 - 0.0.16
- Reason for deletion: Constant maintenance for new languages needed
- Likeliness to come back: Highly unlikely.

##### GetUserEncryptedPassword()

- Name: GetUserEncryptedPassword()
- Source file: UserManagement.vb
- Version range: 0.0.12 - 0.0.16
- Reason for deletion: Deleted as part of user config jsonification
- Likeliness to come back: Highly unlikely.

##### CheckForUpgrade()

- Name: CheckForUpgrade()
- Source file: Config.vb
- Version range: 0.0.4.1 - 0.0.16
- Reason for deletion: Deleted as part of kernel config jsonification
- Likeliness to come back: Highly unlikely.

##### UpdateConfig()

- Name: UpdateConfig()
- Source file: Config.vb
- Version range: 0.0.4.9 - 0.0.16
- Reason for deletion: Deleted as part of kernel config jsonification
- Likeliness to come back: Highly unlikely.

##### UpgradeConfig()

- Name: UpgradeConfig()
- Source file: OldConfigUp.vb
- Version range: 0.0.6.13N - 0.0.16
- Reason for deletion: Moved to KSConverter and deleted as part of kernel config jsonification
- Likeliness to come back: Highly unlikely.

##### ProbeSynth()

- Name: ProbeSynth()
- Source file: BeepSynth.vb
- Version range: 0.0.8 - 0.0.16
- Reason for deletion: Under investigation
- Likeliness to come back: Unknown
- Returning mod example name: BeepSynth
- Returning mod example function name: TryParseSynth()

##### WriteTrueColor()

- Name: WriteTrueColor()
- Source file: TextWriterColor.vb
- Version range: 0.0.12 - 0.0.16
- Reason for deletion: Merged to the Color class
- Likeliness to come back: Highly unlikely.

##### WriteSlowlyTrueColor()

- Name: WriteSlowlyTrueColor()
- Source file: TextWriterSlowColor.vb
- Version range: 0.0.15 - 0.0.16
- Reason for deletion: Merged to the Color class
- Likeliness to come back: Highly unlikely.

##### WriteWhereTrueColor()

- Name: WriteWhereTrueColor()
- Source file: TextWriterWhereColor.vb
- Version range: 0.0.12 - 0.0.16
- Reason for deletion: Merged to the Color class
- Likeliness to come back: Highly unlikely.

##### ParseMods()

- Name: ParseMods()
- Source file: ModParser.vb
- Version range: 0.0.4.10 - 0.0.16
- Reason for deletion: Split to StartMods() and StopMods()
- Likeliness to come back: Highly unlikely

##### SFTPPromptForPassword()

- Name: SFTPPromptForPassword()
- Source file: SFTPTools.vb
- Version range: 0.0.13 - 0.0.16
- Reason for deletion: Migrated to GetConnectionInfo()
- Likeliness to come back: Highly unlikely

##### CloseAliasesFile()

- Name: CloseAliasesFile()
- Source file: AliasManager.vb
- Version range: 0.0.12 - 0.0.16
- Reason for deletion: Deleted as part of alias config jsonification
- Likeliness to come back: Highly unlikely

#### 0.0.17

##### InitTimesInZones()

- Name: InitTimesInZones()
- Source file: TimeZones.vb
- Version range: 0.0.4.9 - 0.0.17
- Reason for deletion: High redundancy (unneeded public declarations of variables)
- Likeliness to come back: Likely
- Returning version: 0.0.17
- Returning name: GetTimeZones()
- Returning file: TimeZones.vb

##### ShowTimesInZones()

- Name: ShowTimesInZones()
- Source file: TimeZones.vb
- Version range: 0.0.4.9 - 0.0.17
- Reason for deletion: More than one behavior in one sub
- Likeliness to come back: Likely
- Returning version: 0.0.17
- Returning name: ShowTimeZone(), ShowTimeZones(), ShowAllTimeZones()
- Returning file: TimeZones.vb

#### 0.0.18

##### InitHelp()

- Name: InitHelp()
- Source file: HelpSystem.vb
- Version range: 0.0.6 - 0.0.18
- Reason for deletion: Avoiding duplicates
- Likeliness to come back: Highly unlikely

##### InitTestHelp()

- Name: InitTestHelp()
- Source file: TestHelpSystem.vb
- Version range: 0.0.16 - 0.0.18
- Reason for deletion: Avoiding duplicates
- Likeliness to come back: Highly unlikely

##### InitSFTPHelp()

- Name: InitSFTPHelp()
- Source file: SFTPHelpSystem.vb
- Version range: 0.0.13 - 0.0.18
- Reason for deletion: Avoiding duplicates
- Likeliness to come back: Highly unlikely

##### InitRSSHelp()

- Name: InitRSSHelp()
- Source file: RSSHelpSystem.vb
- Version range: 0.0.16 - 0.0.18
- Reason for deletion: Avoiding duplicates
- Likeliness to come back: Highly unlikely

##### InitRDebugHelp()

- Name: InitRDebugHelp()
- Source file: RemoteDebugHelpSystem.vb
- Version range: 0.0.16 - 0.0.18
- Reason for deletion: Avoiding duplicates
- Likeliness to come back: Highly unlikely

##### IMAPInitHelp()

- Name: IMAPInitHelp()
- Source file: MailHelpSystem.vb
- Version range: 0.0.11 - 0.0.18
- Reason for deletion: Avoiding duplicates
- Likeliness to come back: Highly unlikely

##### InitFTPHelp()

- Name: InitFTPHelp()
- Source file: FTPHelpSystem.vb
- Version range: 0.0.6.9 - 0.0.18
- Reason for deletion: Avoiding duplicates
- Likeliness to come back: Highly unlikely

##### ZipShell_UpdateHelp()

- Name: ZipShell_UpdateHelp()
- Source file: ZipHelpSystem.vb
- Version range: 0.0.16 - 0.0.18
- Reason for deletion: Avoiding duplicates
- Likeliness to come back: Highly unlikely

##### TextEdit_UpdateHelp()

- Name: TextEdit_UpdateHelp()
- Source file: TextEditHelpSystem.vb
- Version range: 0.0.11 - 0.0.18
- Reason for deletion: Avoiding duplicates
- Likeliness to come back: Highly unlikely
