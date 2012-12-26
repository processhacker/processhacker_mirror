#ifndef TOOLSTATUS_H
#define TOOLSTATUS_H

#include <phdk.h>

#include "resource.h"
#include "btncontrol.h"

#include <phapppub.h>
#include <phplug.h>
#include <phappresource.h>
#include <windowsx.h>

#define TIDC_REFRESH 0
#define TIDC_OPTIONS 1
#define TIDC_FINDOBJ 2
#define TIDC_SYSINFO 3
#define TIDC_FINDWINDOW 4
#define TIDC_FINDWINDOWTHREAD 5
#define TIDC_FINDWINDOWKILL 6

#define STATUS_COUNT 10
#define STATUS_MINIMUM 0x1
#define STATUS_CPUUSAGE 0x1
#define STATUS_COMMIT 0x2
#define STATUS_PHYSICAL 0x4
#define STATUS_NUMBEROFPROCESSES 0x8
#define STATUS_NUMBEROFTHREADS 0x10
#define STATUS_NUMBEROFHANDLES 0x20
#define STATUS_IOREADOTHER 0x40
#define STATUS_IOWRITE 0x80
#define STATUS_MAXCPUPROCESS 0x100
#define STATUS_MAXIOPROCESS 0x200
#define STATUS_MAXIMUM 0x400

typedef enum _TOOLBAR_DISPLAY_STYLE
{
    ImageOnly = 0,
    SelectiveText = 1,
    AllText = 2
} TOOLBAR_DISPLAY_STYLE;

BOOLEAN EnableToolBar;
BOOLEAN EnableStatusBar;
TOOLBAR_DISPLAY_STYLE DisplayStyle;

PPH_PLUGIN PluginInstance;
PH_CALLBACK_REGISTRATION PluginLoadCallbackRegistration;
PH_CALLBACK_REGISTRATION PluginShowOptionsCallbackRegistration;
PH_CALLBACK_REGISTRATION MainWindowShowingCallbackRegistration;
PH_CALLBACK_REGISTRATION ProcessesUpdatedCallbackRegistration;
PH_CALLBACK_REGISTRATION LayoutPaddingCallbackRegistration;
PH_CALLBACK_REGISTRATION TabPageCallbackRegistration;

INT_PTR CALLBACK OptionsDlgProc(
    __in HWND hwndDlg,
    __in UINT uMsg,
    __in WPARAM wParam,
    __in LPARAM lParam
    );

#endif
