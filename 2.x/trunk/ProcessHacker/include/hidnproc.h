#ifndef PH_HIDNPROC_H
#define PH_HIDNPROC_H

typedef enum _PH_HIDDEN_PROCESS_METHOD
{
    BruteForceScanMethod,
    CsrHandlesScanMethod
} PH_HIDDEN_PROCESS_METHOD;

typedef enum _PH_HIDDEN_PROCESS_TYPE
{
    UnknownProcess,
    NormalProcess,
    HiddenProcess,
    TerminatedProcess
} PH_HIDDEN_PROCESS_TYPE;

typedef struct _PH_HIDDEN_PROCESS_ENTRY
{
    HANDLE ProcessId;
    PPH_STRING FileName;
    PH_HIDDEN_PROCESS_TYPE Type;
} PH_HIDDEN_PROCESS_ENTRY, *PPH_HIDDEN_PROCESS_ENTRY;

typedef struct _PH_CSR_HANDLE_INFO
{
    HANDLE CsrProcessHandle;
    HANDLE Handle;
    BOOLEAN IsThreadHandle;

    HANDLE ProcessId;
} PH_CSR_HANDLE_INFO, *PPH_CSR_HANDLE_INFO;

typedef BOOLEAN (NTAPI *PPH_ENUM_HIDDEN_PROCESSES_CALLBACK)(
    __in PPH_HIDDEN_PROCESS_ENTRY Process,
    __in_opt PVOID Context
    );

PHAPPAPI
NTSTATUS
NTAPI
PhEnumHiddenProcesses(
    __in PH_HIDDEN_PROCESS_METHOD Method,
    __in PPH_ENUM_HIDDEN_PROCESSES_CALLBACK Callback,
    __in_opt PVOID Context
    );

typedef BOOLEAN (NTAPI *PPH_ENUM_CSR_PROCESS_HANDLES_CALLBACK)(
    __in PPH_CSR_HANDLE_INFO Handle,
    __in_opt PVOID Context
    );

PHAPPAPI
NTSTATUS
NTAPI
PhEnumCsrProcessHandles(
    __in PPH_ENUM_CSR_PROCESS_HANDLES_CALLBACK Callback,
    __in_opt PVOID Context
    );

PHAPPAPI
NTSTATUS
NTAPI
PhOpenProcessByCsrHandle(
    __out PHANDLE ProcessHandle,
    __in ACCESS_MASK DesiredAccess,
    __in PPH_CSR_HANDLE_INFO Handle
    );

PHAPPAPI
NTSTATUS
NTAPI
PhOpenProcessByCsrHandles(
    __out PHANDLE ProcessHandle,
    __in ACCESS_MASK DesiredAccess,
    __in HANDLE ProcessId
    );

#endif
