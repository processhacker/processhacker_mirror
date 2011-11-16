#ifndef SETTINGS_H
#define SETTINGS_H

typedef enum _PH_SETTING_TYPE
{
    StringSettingType,
    IntegerSettingType,
    IntegerPairSettingType
} PH_SETTING_TYPE, PPH_SETTING_TYPE;

typedef struct _PH_SETTING
{
    PH_SETTING_TYPE Type;
    PH_STRINGREF Name;
    PH_STRINGREF DefaultValue;

    union
    {
        PVOID Pointer;
        ULONG Integer;
        PH_INTEGER_PAIR IntegerPair;
    } u;
} PH_SETTING, *PPH_SETTING;

VOID PhSettingsInitialization(
    VOID
    );

VOID PhUpdateCachedSettings(
    VOID
    );

PHAPPAPI
__mayRaise ULONG PhGetIntegerSetting(
    __in PWSTR Name
    );

PHAPPAPI
__mayRaise PH_INTEGER_PAIR PhGetIntegerPairSetting(
    __in PWSTR Name
    );

PHAPPAPI
__mayRaise PPH_STRING PhGetStringSetting(
    __in PWSTR Name
    );

PHAPPAPI
__mayRaise VOID PhSetIntegerSetting(
    __in PWSTR Name,
    __in ULONG Value
    );

PHAPPAPI
__mayRaise VOID PhSetIntegerPairSetting(
    __in PWSTR Name,
    __in PH_INTEGER_PAIR Value
    );

PHAPPAPI
__mayRaise VOID PhSetStringSetting(
    __in PWSTR Name,
    __in PWSTR Value
    );

PHAPPAPI
__mayRaise VOID PhSetStringSetting2(
    __in PWSTR Name,
    __in PPH_STRINGREF Value
    );

VOID PhClearIgnoredSettings(
    VOID
    );

VOID PhConvertIgnoredSettings(
    VOID
    );

NTSTATUS PhLoadSettings(
    __in PWSTR FileName
    );

NTSTATUS PhSaveSettings(
    __in PWSTR FileName
    );

VOID PhResetSettings(
    VOID
    );

#define PHA_GET_STRING_SETTING(Name) ((PPH_STRING)PHA_DEREFERENCE(PhGetStringSetting(Name)))

// High-level settings creation

typedef struct _PH_CREATE_SETTING
{
    PH_SETTING_TYPE Type;
    PWSTR Name;
    PWSTR DefaultValue;
} PH_SETTING_CREATE, *PPH_SETTING_CREATE;

PHAPPAPI
VOID PhAddSettings(
    __in PPH_SETTING_CREATE Settings,
    __in ULONG NumberOfSettings
    );

// Cached settings

#undef EXT

#ifdef PH_SETTINGS_PRIVATE
#define EXT
#else
#define EXT extern
#endif

EXT ULONG PhCsCollapseServicesOnStart;
EXT ULONG PhCsForceNoParent;
EXT ULONG PhCsHighlightingDuration;
EXT ULONG PhCsPropagateCpuUsage;
EXT ULONG PhCsScrollToNewProcesses;
EXT ULONG PhCsShowCpuBelow001;
EXT ULONG PhCsUpdateInterval;

EXT ULONG PhCsColorNew;
EXT ULONG PhCsColorRemoved;
EXT ULONG PhCsUseColorOwnProcesses;
EXT ULONG PhCsColorOwnProcesses;
EXT ULONG PhCsUseColorSystemProcesses;
EXT ULONG PhCsColorSystemProcesses;
EXT ULONG PhCsUseColorServiceProcesses;
EXT ULONG PhCsColorServiceProcesses;
EXT ULONG PhCsUseColorJobProcesses;
EXT ULONG PhCsColorJobProcesses;
EXT ULONG PhCsUseColorWow64Processes;
EXT ULONG PhCsColorWow64Processes;
EXT ULONG PhCsUseColorPosixProcesses;
EXT ULONG PhCsColorPosixProcesses;
EXT ULONG PhCsUseColorDebuggedProcesses;
EXT ULONG PhCsColorDebuggedProcesses;
EXT ULONG PhCsUseColorElevatedProcesses;
EXT ULONG PhCsColorElevatedProcesses;
EXT ULONG PhCsUseColorImmersiveProcesses;
EXT ULONG PhCsColorImmersiveProcesses;
EXT ULONG PhCsUseColorSuspended;
EXT ULONG PhCsColorSuspended;
EXT ULONG PhCsUseColorDotNet;
EXT ULONG PhCsColorDotNet;
EXT ULONG PhCsUseColorPacked;
EXT ULONG PhCsColorPacked;
EXT ULONG PhCsUseColorGuiThreads;
EXT ULONG PhCsColorGuiThreads;
EXT ULONG PhCsUseColorRelocatedModules;
EXT ULONG PhCsColorRelocatedModules;
EXT ULONG PhCsUseColorProtectedHandles;
EXT ULONG PhCsColorProtectedHandles;
EXT ULONG PhCsUseColorInheritHandles;
EXT ULONG PhCsColorInheritHandles;
EXT ULONG PhCsGraphShowText;
EXT ULONG PhCsGraphColorMode;
EXT ULONG PhCsColorCpuKernel;
EXT ULONG PhCsColorCpuUser;
EXT ULONG PhCsColorIoReadOther;
EXT ULONG PhCsColorIoWrite;
EXT ULONG PhCsColorPrivate;
EXT ULONG PhCsColorPhysical;

#define PH_SET_INTEGER_CACHED_SETTING(Name, Value) (PhSetIntegerSetting(L#Name, PhCs##Name = (Value)))

#endif
