#ifndef PH_PHPLUG_H
#define PH_PHPLUG_H

#include <extmgr.h>

#ifdef __cplusplus
extern "C" {
#endif

// Callbacks

typedef enum _PH_GENERAL_CALLBACK
{
    GeneralCallbackMainWindowShowing = 0, // INT ShowCommand [main thread]
    GeneralCallbackProcessesUpdated = 1, // [main thread]
    GeneralCallbackGetProcessHighlightingColor = 2, // PPH_PLUGIN_GET_HIGHLIGHTING_COLOR Data [main thread]
    GeneralCallbackGetProcessTooltipText = 3, // PPH_PLUGIN_GET_TOOLTIP_TEXT Data [main thread]
    GeneralCallbackProcessPropertiesInitializing = 4, // PPH_PLUGIN_PROCESS_PROPCONTEXT Data [properties thread]
    GeneralCallbackMainMenuInitializing = 5, // PPH_PLUGIN_MENU_INFORMATION Data [main thread]
    GeneralCallbackNotifyEvent = 6, // PPH_PLUGIN_NOTIFY_EVENT Data [main thread]
    GeneralCallbackServicePropertiesInitializing = 7, // PPH_PLUGIN_OBJECT_PROPERTIES Data [properties thread]
    GeneralCallbackHandlePropertiesInitializing = 8, // PPH_PLUGIN_OBJECT_PROPERTIES Data [properties thread]
    GeneralCallbackProcessMenuInitializing = 9, // PPH_PLUGIN_MENU_INFORMATION Data [main thread]
    GeneralCallbackServiceMenuInitializing = 10, // PPH_PLUGIN_MENU_INFORMATION Data [main thread]
    GeneralCallbackNetworkMenuInitializing = 11, // PPH_PLUGIN_MENU_INFORMATION Data [main thread]
    GeneralCallbackIconMenuInitializing = 12, // PPH_PLUGIN_MENU_INFORMATION Data [main thread]
    GeneralCallbackThreadMenuInitializing = 13, // PPH_PLUGIN_MENU_INFORMATION Data [properties thread]
    GeneralCallbackModuleMenuInitializing = 14, // PPH_PLUGIN_MENU_INFORMATION Data [properties thread]
    GeneralCallbackMemoryMenuInitializing = 15, // PPH_PLUGIN_MENU_INFORMATION Data [properties thread]
    GeneralCallbackHandleMenuInitializing = 16, // PPH_PLUGIN_MENU_INFORMATION Data [properties thread]
    GeneralCallbackProcessTreeNewInitializing = 17, // PPH_PLUGIN_TREENEW_INFORMATION Data [main thread]
    GeneralCallbackServiceTreeNewInitializing = 18, // PPH_PLUGIN_TREENEW_INFORMATION Data [main thread]
    GeneralCallbackNetworkTreeNewInitializing = 19, // PPH_PLUGIN_TREENEW_INFORMATION Data [main thread]
    GeneralCallbackModuleTreeNewInitializing = 20, // PPH_PLUGIN_TREENEW_INFORMATION Data [properties thread]
    GeneralCallbackModuleTreeNewUninitializing = 21, // PPH_PLUGIN_TREENEW_INFORMATION Data [properties thread]
    GeneralCallbackThreadTreeNewInitializing = 22, // PPH_PLUGIN_TREENEW_INFORMATION Data [properties thread]
    GeneralCallbackThreadTreeNewUninitializing = 23, // PPH_PLUGIN_TREENEW_INFORMATION Data [properties thread]
    GeneralCallbackHandleTreeNewInitializing = 24, // PPH_PLUGIN_TREENEW_INFORMATION Data [properties thread]
    GeneralCallbackHandleTreeNewUninitializing = 25, // PPH_PLUGIN_TREENEW_INFORMATION Data [properties thread]
    GeneralCallbackThreadStackControl = 26, // PPH_PLUGIN_THREAD_STACK_CONTROL Data [properties thread]
    GeneralCallbackSystemInformationInitializing = 27, // PPH_PLUGIN_SYSINFO_POINTERS Data [system information thread]

    GeneralCallbackMaximum
} PH_GENERAL_CALLBACK, *PPH_GENERAL_CALLBACK;

typedef struct _PH_PLUGIN_GET_HIGHLIGHTING_COLOR
{
    // Parameter is:
    // PPH_PROCESS_ITEM for GeneralCallbackGetProcessHighlightingColor

    PVOID Parameter;
    COLORREF BackColor;
    BOOLEAN Handled;
    BOOLEAN Cache;
} PH_PLUGIN_GET_HIGHLIGHTING_COLOR, *PPH_PLUGIN_GET_HIGHLIGHTING_COLOR;

typedef struct _PH_PLUGIN_GET_TOOLTIP_TEXT
{
    // Parameter is:
    // PPH_PROCESS_ITEM for GeneralCallbackGetProcessTooltipText

    PVOID Parameter;
    PPH_STRING_BUILDER StringBuilder;
} PH_PLUGIN_GET_TOOLTIP_TEXT, *PPH_PLUGIN_GET_TOOLTIP_TEXT;

typedef struct _PH_PLUGIN_PROCESS_PROPCONTEXT
{
    PPH_PROCESS_PROPCONTEXT PropContext;
    PPH_PROCESS_ITEM ProcessItem;
} PH_PLUGIN_PROCESS_PROPCONTEXT, *PPH_PLUGIN_PROCESS_PROPCONTEXT;

typedef struct _PH_PLUGIN_NOTIFY_EVENT
{
    // Parameter is:
    // PPH_PROCESS_ITEM for Type = PH_NOTIFY_PROCESS_*
    // PPH_SERVICE_ITEM for Type = PH_NOTIFY_SERVICE_*

    ULONG Type;
    BOOLEAN Handled;
    PVOID Parameter;
} PH_PLUGIN_NOTIFY_EVENT, *PPH_PLUGIN_NOTIFY_EVENT;

typedef struct _PH_PLUGIN_OBJECT_PROPERTIES
{
    // Parameter is:
    // PPH_SERVICE_ITEM for GeneralCallbackServicePropertiesInitializing
    // PPH_PLUGIN_HANDLE_PROPERTIES_CONTEXT for GeneralCallbackHandlePropertiesInitializing

    PVOID Parameter;
    ULONG NumberOfPages;
    ULONG MaximumNumberOfPages;
    HPROPSHEETPAGE *Pages;
} PH_PLUGIN_OBJECT_PROPERTIES, *PPH_PLUGIN_OBJECT_PROPERTIES;

typedef struct _PH_PLUGIN_HANDLE_PROPERTIES_CONTEXT
{
    HANDLE ProcessId;
    PPH_HANDLE_ITEM HandleItem;
} PH_PLUGIN_HANDLE_PROPERTIES_CONTEXT, *PPH_PLUGIN_HANDLE_PROPERTIES_CONTEXT;

typedef struct _PH_EMENU_ITEM *PPH_EMENU_ITEM, *PPH_EMENU;

typedef struct _PH_PLUGIN_MENU_INFORMATION
{
    PPH_EMENU Menu;
    HWND OwnerWindow;

    union
    {
        struct
        {
            ULONG SubMenuIndex;
        } MainMenu;
        struct
        {
            PPH_PROCESS_ITEM *Processes;
            ULONG NumberOfProcesses;
        } Process;
        struct
        {
            PPH_SERVICE_ITEM *Services;
            ULONG NumberOfServices;
        } Service;
        struct
        {
            PPH_NETWORK_ITEM *NetworkItems;
            ULONG NumberOfNetworkItems;
        } Network;
        struct
        {
            HANDLE ProcessId;
            PPH_THREAD_ITEM *Threads;
            ULONG NumberOfThreads;
        } Thread;
        struct
        {
            HANDLE ProcessId;
            PPH_MODULE_ITEM *Modules;
            ULONG NumberOfModules;
        } Module;
        struct
        {
            HANDLE ProcessId;
            PPH_MEMORY_ITEM *MemoryItems;
            ULONG NumberOfMemoryItems;
        } Memory;
        struct
        {
            HANDLE ProcessId;
            PPH_HANDLE_ITEM *Handles;
            ULONG NumberOfHandles;
        } Handle;
    } u;
} PH_PLUGIN_MENU_INFORMATION, *PPH_PLUGIN_MENU_INFORMATION;

typedef struct _PH_PLUGIN_TREENEW_INFORMATION
{
    HWND TreeNewHandle;
    PVOID CmData;
    PVOID SystemContext;
} PH_PLUGIN_TREENEW_INFORMATION, *PPH_PLUGIN_TREENEW_INFORMATION;

typedef enum _PH_PLUGIN_THREAD_STACK_CONTROL_TYPE
{
    PluginThreadStackInitializing,
    PluginThreadStackUninitializing,
    PluginThreadStackResolveSymbol,
    PluginThreadStackGetTooltip,
    PluginThreadStackMaximum
} PH_PLUGIN_THREAD_STACK_CONTROL_TYPE;

typedef struct _PH_PLUGIN_THREAD_STACK_CONTROL
{
    PH_PLUGIN_THREAD_STACK_CONTROL_TYPE Type;
    PVOID UniqueKey;

    union
    {
        struct
        {
            HANDLE ProcessId;
            HANDLE ThreadId;
            HANDLE ThreadHandle;
            PPH_SYMBOL_PROVIDER SymbolProvider;
        } Initializing;
        struct
        {
            PPH_THREAD_STACK_FRAME StackFrame;
            PPH_STRING Symbol;
        } ResolveSymbol;
        struct
        {
            PPH_THREAD_STACK_FRAME StackFrame;
            PPH_STRING_BUILDER StringBuilder;
        } GetTooltip;
    } u;
} PH_PLUGIN_THREAD_STACK_CONTROL, *PPH_PLUGIN_THREAD_STACK_CONTROL;

typedef PPH_SYSINFO_SECTION (NTAPI *PPH_SYSINFO_CREATE_SECTION)(
    __in PPH_SYSINFO_SECTION Template
    );

typedef PPH_SYSINFO_SECTION (NTAPI *PPH_SYSINFO_FIND_SECTION)(
    __in PPH_STRINGREF Name
    );

typedef VOID (NTAPI *PPH_SYSINFO_ENTER_SECTION_VIEW)(
    __in PPH_SYSINFO_SECTION NewSection
    );

typedef VOID (NTAPI *PPH_SYSINFO_RESTORE_SUMMARY_VIEW)(
    VOID
    );

typedef struct _PH_PLUGIN_SYSINFO_POINTERS
{
    HWND WindowHandle;
    PPH_SYSINFO_CREATE_SECTION CreateSection;
    PPH_SYSINFO_FIND_SECTION FindSection;
    PPH_SYSINFO_ENTER_SECTION_VIEW EnterSectionView;
    PPH_SYSINFO_RESTORE_SUMMARY_VIEW RestoreSummaryView;
} PH_PLUGIN_SYSINFO_POINTERS, *PPH_PLUGIN_SYSINFO_POINTERS;

typedef struct _PH_PLUGIN_TREENEW_MESSAGE
{
    HWND TreeNewHandle;
    PH_TREENEW_MESSAGE Message;
    PVOID Parameter1;
    PVOID Parameter2;
    ULONG SubId;
    PVOID Context;
} PH_PLUGIN_TREENEW_MESSAGE, *PPH_PLUGIN_TREENEW_MESSAGE;

typedef LONG (NTAPI *PPH_PLUGIN_TREENEW_SORT_FUNCTION)(
    __in PVOID Node1,
    __in PVOID Node2,
    __in ULONG SubId,
    __in PVOID Context
    );

typedef enum _PH_PLUGIN_CALLBACK
{
    PluginCallbackLoad = 0, // [main thread]
    PluginCallbackUnload = 1, // [main thread]
    PluginCallbackShowOptions = 2, // HWND ParentWindowHandle [main thread]
    PluginCallbackMenuItem = 3, // PPH_PLUGIN_MENU_ITEM MenuItem [main/properties thread]
    PluginCallbackTreeNewMessage = 4, // PPH_PLUGIN_TREENEW_MESSAGE Message [main/properties thread]

    PluginCallbackMaximum
} PH_PLUGIN_CALLBACK, *PPH_PLUGIN_CALLBACK;

typedef struct _PH_PLUGIN_INFORMATION
{
    PWSTR DisplayName;
    PWSTR Author;
    PWSTR Description;
    PWSTR Url;
    BOOLEAN HasOptions;
    BOOLEAN Reserved1[3];
} PH_PLUGIN_INFORMATION, *PPH_PLUGIN_INFORMATION;

#define PH_PLUGIN_FLAG_IS_CLR 0x1 // plugin is .NET

typedef struct _PH_PLUGIN
{
    PH_AVL_LINKS Links;

    PWSTR Name;
    PVOID DllBase;
    PPH_STRING FileName;
    ULONG Flags;

    PH_PLUGIN_INFORMATION Information;

    PH_CALLBACK Callbacks[PluginCallbackMaximum];
    PH_EM_APP_CONTEXT AppContext;
} PH_PLUGIN, *PPH_PLUGIN;

PHAPPAPI
PPH_PLUGIN
NTAPI
PhRegisterPlugin(
    __in PWSTR Name,
    __in PVOID DllBase,
    __out_opt PPH_PLUGIN_INFORMATION *Information
    );

PHAPPAPI
PPH_PLUGIN
NTAPI
PhFindPlugin(
    __in PWSTR Name
    );

PHAPPAPI
PPH_CALLBACK
NTAPI
PhGetPluginCallback(
    __in PPH_PLUGIN Plugin,
    __in PH_PLUGIN_CALLBACK Callback
    );

PHAPPAPI
PPH_CALLBACK
NTAPI
PhGetGeneralCallback(
    __in PH_GENERAL_CALLBACK Callback
    );

PHAPPAPI
ULONG
NTAPI
PhPluginReserveIds(
    __in ULONG Count
    );

typedef VOID (NTAPI *PPH_PLUGIN_MENU_ITEM_DELETE_FUNCTION)(
    __in struct _PH_PLUGIN_MENU_ITEM *MenuItem
    );

typedef struct _PH_PLUGIN_MENU_ITEM
{
    PPH_PLUGIN Plugin;
    ULONG Id;
    ULONG Reserved1;
    PVOID Context;

    HWND OwnerWindow; // valid only when the menu item is chosen
    PVOID Reserved2;
    PVOID Reserved3;
    PPH_PLUGIN_MENU_ITEM_DELETE_FUNCTION DeleteFunction; // valid only for EMENU-based menu items
} PH_PLUGIN_MENU_ITEM, *PPH_PLUGIN_MENU_ITEM;

// Location
#define PH_MENU_ITEM_LOCATION_VIEW 1
#define PH_MENU_ITEM_LOCATION_TOOLS 2

// Id flags (non-functional)
#define PH_MENU_ITEM_SUB_MENU 0x80000000
#define PH_MENU_ITEM_RETURN_MENU 0x40000000
#define PH_MENU_ITEM_VALID_FLAGS 0xc0000000

PHAPPAPI
ULONG_PTR
NTAPI
PhPluginAddMenuItem(
    __in PPH_PLUGIN Plugin,
    __in ULONG_PTR Location,
    __in_opt PWSTR InsertAfter,
    __in ULONG Id,
    __in PWSTR Text,
    __in_opt PVOID Context
    );

typedef struct _PH_PLUGIN_SYSTEM_STATISTICS
{
    PSYSTEM_PERFORMANCE_INFORMATION Performance;

    ULONG NumberOfProcesses;
    ULONG NumberOfThreads;
    ULONG NumberOfHandles;

    FLOAT CpuKernelUsage;
    FLOAT CpuUserUsage;

    PH_UINT64_DELTA IoReadDelta;
    PH_UINT64_DELTA IoWriteDelta;
    PH_UINT64_DELTA IoOtherDelta;

    ULONG CommitPages;
    ULONG PhysicalPages;

    HANDLE MaxCpuProcessId;
    HANDLE MaxIoProcessId;

    PPH_CIRCULAR_BUFFER_FLOAT CpuKernelHistory;
    PPH_CIRCULAR_BUFFER_FLOAT CpuUserHistory;
    PPH_CIRCULAR_BUFFER_FLOAT *CpusKernelHistory;
    PPH_CIRCULAR_BUFFER_FLOAT *CpusUserHistory;
    PPH_CIRCULAR_BUFFER_ULONG64 IoReadHistory;
    PPH_CIRCULAR_BUFFER_ULONG64 IoWriteHistory;
    PPH_CIRCULAR_BUFFER_ULONG64 IoOtherHistory;
    PPH_CIRCULAR_BUFFER_ULONG CommitHistory;
    PPH_CIRCULAR_BUFFER_ULONG PhysicalHistory;
    PPH_CIRCULAR_BUFFER_ULONG MaxCpuHistory; // ID of max. CPU process
    PPH_CIRCULAR_BUFFER_ULONG MaxIoHistory; // ID of max. I/O process
    PPH_CIRCULAR_BUFFER_FLOAT MaxCpuUsageHistory;
    PPH_CIRCULAR_BUFFER_ULONG64 MaxIoReadOtherHistory;
    PPH_CIRCULAR_BUFFER_ULONG64 MaxIoWriteHistory;
} PH_PLUGIN_SYSTEM_STATISTICS, *PPH_PLUGIN_SYSTEM_STATISTICS;

PHAPPAPI
VOID
NTAPI
PhPluginGetSystemStatistics(
    __out PPH_PLUGIN_SYSTEM_STATISTICS Statistics
    );

PHAPPAPI
PPH_EMENU_ITEM
NTAPI
PhPluginCreateEMenuItem(
    __in PPH_PLUGIN Plugin,
    __in ULONG Flags,
    __in ULONG Id,
    __in PWSTR Text,
    __in_opt PVOID Context
    );

PHAPPAPI
BOOLEAN
NTAPI
PhPluginTriggerEMenuItem(
    __in HWND OwnerWindow,
    __in PPH_EMENU_ITEM Item
    );

PHAPPAPI
BOOLEAN
NTAPI
PhPluginAddTreeNewColumn(
    __in PPH_PLUGIN Plugin,
    __in PVOID CmData,
    __in PPH_TREENEW_COLUMN Column,
    __in ULONG SubId,
    __in_opt PVOID Context,
    __in_opt PPH_PLUGIN_TREENEW_SORT_FUNCTION SortFunction
    );

PHAPPAPI
VOID
NTAPI
PhPluginSetObjectExtension(
    __in PPH_PLUGIN Plugin,
    __in PH_EM_OBJECT_TYPE ObjectType,
    __in ULONG ExtensionSize,
    __in_opt PPH_EM_OBJECT_CALLBACK CreateCallback,
    __in_opt PPH_EM_OBJECT_CALLBACK DeleteCallback
    );

PHAPPAPI
PVOID
NTAPI
PhPluginGetObjectExtension(
    __in PPH_PLUGIN Plugin,
    __in PVOID Object,
    __in PH_EM_OBJECT_TYPE ObjectType
    );

PHAPPAPI
struct _PH_NF_ICON *
NTAPI
PhPluginRegisterIcon(
    __in PPH_PLUGIN Plugin,
    __in ULONG SubId,
    __in_opt PVOID Context,
    __in PWSTR Text,
    __in ULONG Flags,
    __in struct _PH_NF_ICON_REGISTRATION_DATA *RegistrationData
    );

#ifdef __cplusplus
}
#endif

#endif
