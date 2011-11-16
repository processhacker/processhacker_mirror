#ifndef _PH_EMENU_H
#define _PH_EMENU_H

#define PH_EMENU_DISABLED 0x1
#define PH_EMENU_CHECKED 0x2
#define PH_EMENU_HIGHLIGHT 0x4
#define PH_EMENU_MENUBARBREAK 0x8
#define PH_EMENU_MENUBREAK 0x10
#define PH_EMENU_DEFAULT 0x20
#define PH_EMENU_MOUSESELECT 0x40
#define PH_EMENU_RADIOCHECK 0x80

#define PH_EMENU_STRING 0x100000
#define PH_EMENU_SEPARATOR 0x200000

#define PH_EMENU_TEXT_OWNED 0x80000000
#define PH_EMENU_BITMAP_OWNED 0x40000000

struct _PH_EMENU_ITEM;

typedef VOID (NTAPI *PPH_EMENU_ITEM_DELETE_FUNCTION)(
    __in struct _PH_EMENU_ITEM *Item
    );

typedef struct _PH_EMENU_ITEM
{
    ULONG Flags;
    ULONG Id;
    PWSTR Text;
    HBITMAP Bitmap;

    PVOID Parameter;
    PVOID Context;
    PPH_EMENU_ITEM_DELETE_FUNCTION DeleteFunction;
    PVOID Reserved;

    struct _PH_EMENU_ITEM *Parent;
    PPH_LIST Items;
} PH_EMENU_ITEM, *PPH_EMENU_ITEM;

typedef struct _PH_EMENU_ITEM PH_EMENU, *PPH_EMENU;

PHLIBAPI
PPH_EMENU_ITEM PhCreateEMenuItem(
    __in ULONG Flags,
    __in ULONG Id,
    __in PWSTR Text,
    __in_opt PWSTR Bitmap,
    __in_opt PVOID Context
    );

PHLIBAPI
VOID PhDestroyEMenuItem(
    __in PPH_EMENU_ITEM Item
    );

#define PH_EMENU_FIND_DESCEND 0x1
#define PH_EMENU_FIND_STARTSWITH 0x2
#define PH_EMENU_FIND_LITERAL 0x4

PHLIBAPI
PPH_EMENU_ITEM PhFindEMenuItem(
    __in PPH_EMENU_ITEM Item,
    __in ULONG Flags,
    __in_opt PWSTR Text,
    __in_opt ULONG Id
    );

PPH_EMENU_ITEM PhFindEMenuItemEx(
    __in PPH_EMENU_ITEM Item,
    __in ULONG Flags,
    __in_opt PWSTR Text,
    __in_opt ULONG Id,
    __out_opt PPH_EMENU_ITEM *FoundParent,
    __out_opt PULONG FoundIndex
    );

PHLIBAPI
ULONG PhIndexOfEMenuItem(
    __in PPH_EMENU_ITEM Parent,
    __in PPH_EMENU_ITEM Item
    );

PHLIBAPI
VOID PhInsertEMenuItem(
    __inout PPH_EMENU_ITEM Parent,
    __inout PPH_EMENU_ITEM Item,
    __in ULONG Index
    );

PHLIBAPI
BOOLEAN PhRemoveEMenuItem(
    __inout_opt PPH_EMENU_ITEM Parent,
    __in_opt PPH_EMENU_ITEM Item,
    __in_opt ULONG Index
    );

PHLIBAPI
VOID PhRemoveAllEMenuItems(
    __inout PPH_EMENU_ITEM Parent
    );

PHLIBAPI
PPH_EMENU PhCreateEMenu(
    VOID
    );

PHLIBAPI
VOID PhDestroyEMenu(
    __in PPH_EMENU Menu
    );

#define PH_EMENU_CONVERT_ID 0x1

typedef struct _PH_EMENU_DATA
{
    PPH_LIST IdToItem;
} PH_EMENU_DATA, *PPH_EMENU_DATA;

VOID PhInitializeEMenuData(
    __out PPH_EMENU_DATA Data
    );

VOID PhDeleteEMenuData(
    __inout PPH_EMENU_DATA Data
    );

HMENU PhEMenuToHMenu(
    __in PPH_EMENU_ITEM Menu,
    __in ULONG Flags,
    __inout_opt PPH_EMENU_DATA Data
    );

VOID PhEMenuToHMenu2(
    __in HMENU MenuHandle,
    __in PPH_EMENU_ITEM Menu,
    __in ULONG Flags,
    __inout_opt PPH_EMENU_DATA Data
    );

VOID PhHMenuToEMenuItem(
    __inout PPH_EMENU_ITEM MenuItem,
    __in HMENU MenuHandle
    );

PHLIBAPI
VOID PhLoadResourceEMenuItem(
    __inout PPH_EMENU_ITEM MenuItem,
    __in HINSTANCE InstanceHandle,
    __in PWSTR Resource,
    __in ULONG SubMenuIndex
    );

#define PH_EMENU_SHOW_NONOTIFY 0x1
#define PH_EMENU_SHOW_LEFTRIGHT 0x2

PHLIBAPI
PPH_EMENU_ITEM PhShowEMenu(
    __in PPH_EMENU Menu,
    __in HWND WindowHandle,
    __in ULONG Flags,
    __in ULONG Align,
    __in ULONG X,
    __in ULONG Y
    );

// Convenience functions

PHLIBAPI
BOOLEAN PhSetFlagsEMenuItem(
    __in PPH_EMENU_ITEM Item,
    __in ULONG Id,
    __in ULONG Mask,
    __in ULONG Value
    );

FORCEINLINE BOOLEAN PhEnableEMenuItem(
    __in PPH_EMENU_ITEM Item,
    __in ULONG Id,
    __in BOOLEAN Enable
    )
{
    return PhSetFlagsEMenuItem(Item, Id, PH_EMENU_DISABLED, Enable ? 0 : PH_EMENU_DISABLED);
}

PHLIBAPI
VOID PhSetFlagsAllEMenuItems(
    __in PPH_EMENU_ITEM Item,
    __in ULONG Mask,
    __in ULONG Value
    );

#endif
