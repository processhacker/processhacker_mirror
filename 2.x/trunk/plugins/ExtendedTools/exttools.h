#ifndef EXTTOOLS_H
#define EXTTOOLS_H

#define PHNT_VERSION PHNT_VISTA
#include <phdk.h>

extern PPH_PLUGIN PluginInstance;
extern LIST_ENTRY EtProcessBlockListHead;
extern LIST_ENTRY EtNetworkBlockListHead;
extern HWND ProcessTreeNewHandle;
extern HWND NetworkTreeNewHandle;

#define SETTING_PREFIX L"ProcessHacker.ExtendedTools."
#define SETTING_NAME_DISK_TREE_LIST_COLUMNS (SETTING_PREFIX L"DiskTreeListColumns")
#define SETTING_NAME_DISK_TREE_LIST_SORT (SETTING_PREFIX L"DiskTreeListSort")
#define SETTING_NAME_ENABLE_ETW_MONITOR (SETTING_PREFIX L"EnableEtwMonitor")
#define SETTING_NAME_ENABLE_GPU_MONITOR (SETTING_PREFIX L"EnableGpuMonitor")
#define SETTING_NAME_GPU_NODE_BITMAP (SETTING_PREFIX L"GpuNodeBitmap")

// Process icon

typedef struct _ET_PROCESS_ICON
{
    LONG RefCount;
    HICON Icon;
} ET_PROCESS_ICON, *PET_PROCESS_ICON;

// Disk item

#define HISTORY_SIZE 60

typedef struct _ET_DISK_ITEM
{
    LIST_ENTRY AgeListEntry;
    ULONG AddTime;
    ULONG FreshTime;

    HANDLE ProcessId;
    PPH_STRING FileName;
    PPH_STRING FileNameWin32;

    PPH_STRING ProcessName;
    PET_PROCESS_ICON ProcessIcon;
    PPH_PROCESS_RECORD ProcessRecord;

    ULONG IoPriority;
    ULONG ResponseTimeCount;
    FLOAT ResponseTimeTotal; // in milliseconds
    FLOAT ResponseTimeAverage;

    ULONG64 ReadTotal;
    ULONG64 WriteTotal;
    ULONG64 ReadDelta;
    ULONG64 WriteDelta;
    ULONG64 ReadAverage;
    ULONG64 WriteAverage;

    ULONG64 ReadHistory[HISTORY_SIZE];
    ULONG64 WriteHistory[HISTORY_SIZE];
    ULONG HistoryCount;
    ULONG HistoryPosition;
} ET_DISK_ITEM, *PET_DISK_ITEM;

// Disk node

#define ETDSTNC_NAME 0
#define ETDSTNC_FILE 1
#define ETDSTNC_READRATEAVERAGE 2
#define ETDSTNC_WRITERATEAVERAGE 3
#define ETDSTNC_TOTALRATEAVERAGE 4
#define ETDSTNC_IOPRIORITY 5
#define ETDSTNC_RESPONSETIME 6
#define ETDSTNC_MAXIMUM 7

typedef struct _ET_DISK_NODE
{
    PH_TREENEW_NODE Node;

    PET_DISK_ITEM DiskItem;

    PH_STRINGREF TextCache[ETDSTNC_MAXIMUM];

    PPH_STRING ProcessNameText;
    PPH_STRING ReadRateAverageText;
    PPH_STRING WriteRateAverageText;
    PPH_STRING TotalRateAverageText;
    PPH_STRING ResponseTimeText;

    PPH_STRING TooltipText;
} ET_DISK_NODE, *PET_DISK_NODE;

// Process tree columns

#define ETPRTNC_DISKREADS 1
#define ETPRTNC_DISKWRITES 2
#define ETPRTNC_DISKREADBYTES 3
#define ETPRTNC_DISKWRITEBYTES 4
#define ETPRTNC_DISKTOTALBYTES 5
#define ETPRTNC_DISKREADSDELTA 6
#define ETPRTNC_DISKWRITESDELTA 7
#define ETPRTNC_DISKREADBYTESDELTA 8
#define ETPRTNC_DISKWRITEBYTESDELTA 9
#define ETPRTNC_DISKTOTALBYTESDELTA 10
#define ETPRTNC_NETWORKRECEIVES 11
#define ETPRTNC_NETWORKSENDS 12
#define ETPRTNC_NETWORKRECEIVEBYTES 13
#define ETPRTNC_NETWORKSENDBYTES 14
#define ETPRTNC_NETWORKTOTALBYTES 15
#define ETPRTNC_NETWORKRECEIVESDELTA 16
#define ETPRTNC_NETWORKSENDSDELTA 17
#define ETPRTNC_NETWORKRECEIVEBYTESDELTA 18
#define ETPRTNC_NETWORKSENDBYTESDELTA 19
#define ETPRTNC_NETWORKTOTALBYTESDELTA 20
#define ETPRTNC_HARDFAULTS 21
#define ETPRTNC_HARDFAULTSDELTA 22
#define ETPRTNC_PEAKTHREADS 23
#define ETPRTNC_GPU 24
#define ETPRTNC_GPUDEDICATEDBYTES 25
#define ETPRTNC_GPUSHAREDBYTES 26
#define ETPRTNC_DISKREADRATE 27
#define ETPRTNC_DISKWRITERATE 28
#define ETPRTNC_DISKTOTALRATE 29
#define ETPRTNC_NETWORKRECEIVERATE 30
#define ETPRTNC_NETWORKSENDRATE 31
#define ETPRTNC_NETWORKTOTALRATE 32
#define ETPRTNC_MAXIMUM 32

// Network list columns

#define ETNETNC_RECEIVES 1
#define ETNETNC_SENDS 2
#define ETNETNC_RECEIVEBYTES 3
#define ETNETNC_SENDBYTES 4
#define ETNETNC_TOTALBYTES 5
#define ETNETNC_RECEIVESDELTA 6
#define ETNETNC_SENDSDELTA 7
#define ETNETNC_RECEIVEBYTESDELTA 8
#define ETNETNC_SENDBYTESDELTA 9
#define ETNETNC_TOTALBYTESDELTA 10
#define ETNETNC_FIREWALLSTATUS 11
#define ETNETNC_RECEIVERATE 12
#define ETNETNC_SENDRATE 13
#define ETNETNC_TOTALRATE 14
#define ETNETNC_MAXIMUM 14

// Firewall status

typedef enum _ET_FIREWALL_STATUS
{
    FirewallUnknownStatus,
    FirewallAllowedNotRestricted,
    FirewallAllowedRestricted,
    FirewallNotAllowedNotRestricted,
    FirewallNotAllowedRestricted,
    FirewallMaximumStatus
} ET_FIREWALL_STATUS;

// Object extensions

typedef struct _ET_PROCESS_BLOCK
{
    LIST_ENTRY ListEntry;
    PPH_PROCESS_ITEM ProcessItem;

    ULONG64 DiskReadCount;
    ULONG64 DiskWriteCount;
    ULONG64 NetworkReceiveCount;
    ULONG64 NetworkSendCount;

    ULONG64 DiskReadRaw;
    ULONG64 DiskWriteRaw;
    ULONG64 NetworkReceiveRaw;
    ULONG64 NetworkSendRaw;

    PH_UINT64_DELTA DiskReadDelta;
    PH_UINT64_DELTA DiskReadRawDelta;
    PH_UINT64_DELTA DiskWriteDelta;
    PH_UINT64_DELTA DiskWriteRawDelta;
    PH_UINT64_DELTA NetworkReceiveDelta;
    PH_UINT64_DELTA NetworkReceiveRawDelta;
    PH_UINT64_DELTA NetworkSendDelta;
    PH_UINT64_DELTA NetworkSendRawDelta;

    PH_UINT64_DELTA GpuRunningTimeDelta;
    FLOAT GpuNodeUsage;
    ULONG64 GpuDedicatedUsage;
    ULONG64 GpuSharedUsage;

    PH_UINT32_DELTA HardFaultsDelta;

    PH_QUEUED_LOCK TextCacheLock;
    PPH_STRING TextCache[ETPRTNC_MAXIMUM + 1];
    BOOLEAN TextCacheValid[ETPRTNC_MAXIMUM + 1];

    PET_PROCESS_ICON SmallProcessIcon;
} ET_PROCESS_BLOCK, *PET_PROCESS_BLOCK;

typedef struct _ET_NETWORK_BLOCK
{
    LIST_ENTRY ListEntry;
    PPH_NETWORK_ITEM NetworkItem;

    ULONG64 ReceiveCount;
    ULONG64 SendCount;
    ULONG64 ReceiveRaw;
    ULONG64 SendRaw;

    union
    {
        struct
        {
            PH_UINT64_DELTA ReceiveDelta;
            PH_UINT64_DELTA ReceiveRawDelta;
            PH_UINT64_DELTA SendDelta;
            PH_UINT64_DELTA SendRawDelta;
        };
        PH_UINT64_DELTA Deltas[4];
    };

    ET_FIREWALL_STATUS FirewallStatus;
    BOOLEAN FirewallStatusValid;

    PH_QUEUED_LOCK TextCacheLock;
    PPH_STRING TextCache[ETNETNC_MAXIMUM + 1];
    BOOLEAN TextCacheValid[ETNETNC_MAXIMUM + 1];
} ET_NETWORK_BLOCK, *PET_NETWORK_BLOCK;

// main

PET_PROCESS_BLOCK EtGetProcessBlock(
    __in PPH_PROCESS_ITEM ProcessItem
    );

PET_NETWORK_BLOCK EtGetNetworkBlock(
    __in PPH_NETWORK_ITEM NetworkItem
    );

// utils

VOID EtFormatRate(
    __in ULONG64 ValuePerPeriod,
    __inout PPH_STRING *Buffer,
    __out_opt PPH_STRINGREF String
    );

// etwmon

extern BOOLEAN EtEtwEnabled;

// etwstat

extern ULONG EtDiskReadCount;
extern ULONG EtDiskWriteCount;
extern ULONG EtNetworkReceiveCount;
extern ULONG EtNetworkSendCount;

extern PH_UINT32_DELTA EtDiskReadDelta;
extern PH_UINT32_DELTA EtDiskWriteDelta;
extern PH_UINT32_DELTA EtNetworkReceiveDelta;
extern PH_UINT32_DELTA EtNetworkSendDelta;

extern PH_UINT32_DELTA EtDiskReadCountDelta;
extern PH_UINT32_DELTA EtDiskWriteCountDelta;
extern PH_UINT32_DELTA EtNetworkReceiveCountDelta;
extern PH_UINT32_DELTA EtNetworkSendCountDelta;

extern PH_CIRCULAR_BUFFER_ULONG EtDiskReadHistory;
extern PH_CIRCULAR_BUFFER_ULONG EtDiskWriteHistory;
extern PH_CIRCULAR_BUFFER_ULONG EtNetworkReceiveHistory;
extern PH_CIRCULAR_BUFFER_ULONG EtNetworkSendHistory;
extern PH_CIRCULAR_BUFFER_ULONG EtMaxDiskHistory;
extern PH_CIRCULAR_BUFFER_ULONG EtMaxNetworkHistory;

VOID EtEtwStatisticsInitialization(
    VOID
    );

VOID EtEtwStatisticsUninitialization(
    VOID
    );

// etwdisk

extern BOOLEAN EtDiskEnabled;

extern PPH_OBJECT_TYPE EtDiskItemType;
extern PH_CALLBACK EtDiskItemAddedEvent;
extern PH_CALLBACK EtDiskItemModifiedEvent;
extern PH_CALLBACK EtDiskItemRemovedEvent;
extern PH_CALLBACK EtDiskItemsUpdatedEvent;

VOID EtInitializeDiskInformation(
    VOID
    );

PET_DISK_ITEM EtCreateDiskItem(
    VOID
    );

PET_DISK_ITEM EtReferenceDiskItem(
    __in HANDLE ProcessId,
    __in PPH_STRING FileName
    );

PPH_STRING EtFileObjectToFileName(
    __in PVOID FileObject
    );

// procicon

PET_PROCESS_ICON EtProcIconCreateProcessIcon(
    __in HICON Icon
    );

VOID EtProcIconReferenceProcessIcon(
    __inout PET_PROCESS_ICON ProcessIcon
    );

VOID EtProcIconDereferenceProcessIcon(
    __inout PET_PROCESS_ICON ProcessIcon
    );

PET_PROCESS_ICON EtProcIconReferenceSmallProcessIcon(
    __inout PET_PROCESS_BLOCK Block
    );

VOID EtProcIconNotifyProcessDelete(
    __inout PET_PROCESS_BLOCK Block
    );

// etwprprp

VOID EtProcessEtwPropertiesInitializing(
    __in PVOID Parameter
    );

// disktab

VOID EtInitializeDiskTab(
    VOID
    );

VOID EtLoadSettingsDiskTreeList(
    VOID
    );

VOID EtSaveSettingsDiskTreeList(
    VOID
    );

// gpumon

extern BOOLEAN EtGpuEnabled;

extern ULONG EtGpuTotalNodeCount;
extern ULONG EtGpuTotalSegmentCount;
extern ULONG64 EtGpuDedicatedLimit;
extern ULONG64 EtGpuSharedLimit;
extern RTL_BITMAP EtGpuNodeBitMap;

extern PH_UINT64_DELTA EtClockTotalRunningTimeDelta;
extern LARGE_INTEGER EtClockTotalRunningTimeFrequency;
extern PH_UINT64_DELTA EtGpuTotalRunningTimeDelta;
extern PH_UINT64_DELTA EtGpuSystemRunningTimeDelta;
extern FLOAT EtGpuNodeUsage;
extern PH_CIRCULAR_BUFFER_FLOAT EtGpuNodeHistory;
extern PH_CIRCULAR_BUFFER_ULONG EtMaxGpuNodeHistory; // ID of max. GPU usage process
extern PH_CIRCULAR_BUFFER_FLOAT EtMaxGpuNodeUsageHistory;

extern PPH_UINT64_DELTA EtGpuNodesTotalRunningTimeDelta;
extern PPH_CIRCULAR_BUFFER_FLOAT EtGpuNodesHistory;

extern ULONG64 EtGpuDedicatedUsage;
extern ULONG64 EtGpuSharedUsage;
extern PH_CIRCULAR_BUFFER_ULONG EtGpuDedicatedHistory;
extern PH_CIRCULAR_BUFFER_ULONG EtGpuSharedHistory;

VOID EtGpuMonitorInitialization(
    VOID
    );

typedef struct _ET_PROCESS_GPU_STATISTICS
{
    ULONG SegmentCount;
    ULONG NodeCount;

    ULONG64 DedicatedCommitted;
    ULONG64 SharedCommitted;

    ULONG64 BytesAllocated;
    ULONG64 BytesReserved;
    ULONG64 WriteCombinedBytesAllocated;
    ULONG64 WriteCombinedBytesReserved;
    ULONG64 CachedBytesAllocated;
    ULONG64 CachedBytesReserved;
    ULONG64 SectionBytesAllocated;
    ULONG64 SectionBytesReserved;

    ULONG64 RunningTime;
    ULONG64 ContextSwitches;
} ET_PROCESS_GPU_STATISTICS, *PET_PROCESS_GPU_STATISTICS;

VOID EtSaveGpuMonitorSettings(
    VOID
    );

ULONG EtGetGpuAdapterCount(
    VOID
    );

ULONG EtGetGpuAdapterIndexFromNodeIndex(
    __in ULONG NodeIndex
    );

PPH_STRING EtGetGpuAdapterDescription(
    __in ULONG Index
    );

VOID EtAllocateGpuNodeBitMap(
    __out PRTL_BITMAP BitMap
    );

VOID EtUpdateGpuNodeBitMap(
    __in PRTL_BITMAP NewBitMap
    );

VOID EtQueryProcessGpuStatistics(
    __in HANDLE ProcessHandle,
    __out PET_PROCESS_GPU_STATISTICS Statistics
    );

// gpuprprp

VOID EtProcessGpuPropertiesInitializing(
    __in PVOID Parameter
    );

// treeext

VOID EtProcessTreeNewInitializing(
    __in PVOID Parameter
    );

VOID EtProcessTreeNewMessage(
    __in PVOID Parameter
    );

VOID EtNetworkTreeNewInitializing(
    __in PVOID Parameter
    );

VOID EtNetworkTreeNewMessage(
    __in PVOID Parameter
    );

ET_FIREWALL_STATUS EtQueryFirewallStatus(
    __in PPH_NETWORK_ITEM NetworkItem
    );

// etwsys

VOID EtEtwSystemInformationInitializing(
    __in PPH_PLUGIN_SYSINFO_POINTERS Pointers
    );

// gpunodes

VOID EtShowGpuNodesDialog(
    __in HWND ParentWindowHandle,
    __in PPH_SYSINFO_PARAMETERS Parameters
    );

// gpusys

VOID EtGpuSystemInformationInitializing(
    __in PPH_PLUGIN_SYSINFO_POINTERS Pointers
    );

// iconext

VOID EtRegisterNotifyIcons(
    VOID
    );

// modsrv

VOID EtShowModuleServicesDialog(
    __in HWND ParentWindowHandle,
    __in HANDLE ProcessId,
    __in PWSTR ModuleName
    );

// objprp

VOID EtHandlePropertiesInitializing(
    __in PVOID Parameter
    );

// options

VOID EtShowOptionsDialog(
    __in HWND ParentWindowHandle
    );

// thrdact

BOOLEAN EtUiCancelIoThread(
    __in HWND hWnd,
    __in PPH_THREAD_ITEM Thread
    );

// unldll

VOID EtShowUnloadedDllsDialog(
    __in HWND ParentWindowHandle,
    __in PPH_PROCESS_ITEM ProcessItem
    );

// wswatch

VOID EtShowWsWatchDialog(
    __in HWND ParentWindowHandle,
    __in PPH_PROCESS_ITEM ProcessItem
    );

#endif
