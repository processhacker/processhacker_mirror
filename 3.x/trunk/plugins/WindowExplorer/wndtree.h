#ifndef WNDTREE_H
#define WNDTREE_H

#define WEWNTLC_CLASS 0
#define WEWNTLC_HANDLE 1
#define WEWNTLC_TEXT 2
#define WEWNTLC_THREAD 3
#define WEWNTLC_MAXIMUM 4

typedef struct _WE_WINDOW_NODE
{
    PH_TREENEW_NODE Node;

    struct _WE_WINDOW_NODE *Parent;
    PPH_LIST Children;
    BOOLEAN HasChildren;
    BOOLEAN Opened;

    PH_STRINGREF TextCache[WEWNTLC_MAXIMUM];

    HWND WindowHandle;
    WCHAR WindowClass[64];
    PPH_STRING WindowText;
    CLIENT_ID ClientId;
    BOOLEAN WindowVisible;

    WCHAR WindowHandleString[PH_PTR_STR_LEN_1];
    PPH_STRING ThreadString;
} WE_WINDOW_NODE, *PWE_WINDOW_NODE;

typedef struct _WE_WINDOW_TREE_CONTEXT
{
    HWND ParentWindowHandle;
    HWND TreeNewHandle;
    ULONG TreeNewSortColumn;
    PH_SORT_ORDER TreeNewSortOrder;

    PPH_HASHTABLE NodeHashtable;
    PPH_LIST NodeList;
    PPH_LIST NodeRootList;
} WE_WINDOW_TREE_CONTEXT, *PWE_WINDOW_TREE_CONTEXT;

VOID WeInitializeWindowTree(
    __in HWND ParentWindowHandle,
    __in HWND TreeNewHandle,
    __out PWE_WINDOW_TREE_CONTEXT Context
    );

VOID WeDeleteWindowTree(
    __in PWE_WINDOW_TREE_CONTEXT Context
    );

PWE_WINDOW_NODE WeAddWindowNode(
    __inout PWE_WINDOW_TREE_CONTEXT Context
    );

PWE_WINDOW_NODE WeFindWindowNode(
    __in PWE_WINDOW_TREE_CONTEXT Context,
    __in HWND WindowHandle
    );

VOID WeRemoveWindowNode(
    __in PWE_WINDOW_TREE_CONTEXT Context,
    __in PWE_WINDOW_NODE WindowNode
    );

VOID WeClearWindowTree(
    __in PWE_WINDOW_TREE_CONTEXT Context
    );

PWE_WINDOW_NODE WeGetSelectedWindowNode(
    __in PWE_WINDOW_TREE_CONTEXT Context
    );

VOID WeGetSelectedWindowNodes(
    __in PWE_WINDOW_TREE_CONTEXT Context,
    __out PWE_WINDOW_NODE **Windows,
    __out PULONG NumberOfWindows
    );

#endif
