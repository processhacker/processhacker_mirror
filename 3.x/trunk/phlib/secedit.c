/*
 * Process Hacker -
 *   object security editor
 *
 * Copyright (C) 2010 wj32
 *
 * This file is part of Process Hacker.
 *
 * Process Hacker is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Process Hacker is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Process Hacker.  If not, see <http://www.gnu.org/licenses/>.
 */

#include <phgui.h>
#include <seceditp.h>

static ISecurityInformationVtbl PhSecurityInformation_VTable =
{
    PhSecurityInformation_QueryInterface,
    PhSecurityInformation_AddRef,
    PhSecurityInformation_Release,
    PhSecurityInformation_GetObjectInformation,
    PhSecurityInformation_GetSecurity,
    PhSecurityInformation_SetSecurity,
    PhSecurityInformation_GetAccessRights,
    PhSecurityInformation_MapGeneric,
    PhSecurityInformation_GetInheritTypes,
    PhSecurityInformation_PropertySheetPageCallback
};

static PH_INITONCE SecurityEditorInitOnce = PH_INITONCE_INIT;
static _CreateSecurityPage CreateSecurityPage_I;
static _EditSecurity EditSecurity_I;

FORCEINLINE VOID PhpSecurityEditorInitialization(
    VOID
    )
{
    if (PhBeginInitOnce(&SecurityEditorInitOnce))
    {
        HMODULE aclui;

        aclui = LoadLibrary(L"aclui.dll");
        CreateSecurityPage_I = (PVOID)GetProcAddress(aclui, "CreateSecurityPage");
        EditSecurity_I = (PVOID)GetProcAddress(aclui, "EditSecurity");

        PhEndInitOnce(&SecurityEditorInitOnce);
    }
}

/**
 * Creates a security editor page.
 *
 * \param ObjectName The name of the object.
 * \param GetObjectSecurity A callback function executed to retrieve
 * the security descriptor of the object.
 * \param SetObjectSecurity A callback function executed to modify
 * the security descriptor of the object.
 * \param Context A user-defined value to pass to the callback functions.
 * \param AccessEntries An array of access mask descriptors.
 * \param NumberOfAccessEntries The number of elements in \a AccessEntries.
 */
HPROPSHEETPAGE PhCreateSecurityPage(
    __in PWSTR ObjectName,
    __in PPH_GET_OBJECT_SECURITY GetObjectSecurity,
    __in PPH_SET_OBJECT_SECURITY SetObjectSecurity,
    __in_opt PVOID Context,
    __in PPH_ACCESS_ENTRY AccessEntries,
    __in ULONG NumberOfAccessEntries
    )
{
    ISecurityInformation *info;
    HPROPSHEETPAGE page;

    PhpSecurityEditorInitialization();

    if (!CreateSecurityPage_I)
        return NULL;

    info = PhSecurityInformation_Create(
        ObjectName,
        GetObjectSecurity,
        SetObjectSecurity,
        Context,
        AccessEntries,
        NumberOfAccessEntries
        );

    page = CreateSecurityPage_I(info);

    PhSecurityInformation_Release(info);

    return page;
}

/**
 * Displays a security editor dialog.
 *
 * \param hWnd The parent window of the dialog.
 * \param ObjectName The name of the object.
 * \param GetObjectSecurity A callback function executed to retrieve
 * the security descriptor of the object.
 * \param SetObjectSecurity A callback function executed to modify
 * the security descriptor of the object.
 * \param Context A user-defined value to pass to the callback functions.
 * \param AccessEntries An array of access mask descriptors.
 * \param NumberOfAccessEntries The number of elements in \a AccessEntries.
 */
VOID PhEditSecurity(
    __in HWND hWnd,
    __in PWSTR ObjectName,
    __in PPH_GET_OBJECT_SECURITY GetObjectSecurity,
    __in PPH_SET_OBJECT_SECURITY SetObjectSecurity,
    __in_opt PVOID Context,
    __in PPH_ACCESS_ENTRY AccessEntries,
    __in ULONG NumberOfAccessEntries
    )
{
    ISecurityInformation *info;

    PhpSecurityEditorInitialization();

    if (!EditSecurity_I)
        return;

    info = PhSecurityInformation_Create(
        ObjectName,
        GetObjectSecurity,
        SetObjectSecurity,
        Context,
        AccessEntries,
        NumberOfAccessEntries
        );

    EditSecurity_I(hWnd, info);

    PhSecurityInformation_Release(info);
}

ISecurityInformation *PhSecurityInformation_Create(
    __in PWSTR ObjectName,
    __in PPH_GET_OBJECT_SECURITY GetObjectSecurity,
    __in PPH_SET_OBJECT_SECURITY SetObjectSecurity,
    __in_opt PVOID Context,
    __in PPH_ACCESS_ENTRY AccessEntries,
    __in ULONG NumberOfAccessEntries
    )
{
    PhSecurityInformation *info;
    ULONG i;

    info = PhAllocate(sizeof(PhSecurityInformation));
    info->VTable = &PhSecurityInformation_VTable;
    info->RefCount = 1;

    info->ObjectName = PhCreateString(ObjectName);
    info->GetObjectSecurity = GetObjectSecurity;
    info->SetObjectSecurity = SetObjectSecurity;
    info->Context = Context;
    info->AccessEntries = PhAllocate(sizeof(SI_ACCESS) * NumberOfAccessEntries);
    info->NumberOfAccessEntries = NumberOfAccessEntries;

    for (i = 0; i < NumberOfAccessEntries; i++)
    {
        memset(&info->AccessEntries[i], 0, sizeof(SI_ACCESS));
        info->AccessEntries[i].pszName = AccessEntries[i].Name;
        info->AccessEntries[i].mask = AccessEntries[i].Access;

        if (AccessEntries[i].General)
            info->AccessEntries[i].dwFlags |= SI_ACCESS_GENERAL;
        if (AccessEntries[i].Specific)
            info->AccessEntries[i].dwFlags |= SI_ACCESS_SPECIFIC;
    }

    return (ISecurityInformation *)info;
}

HRESULT STDMETHODCALLTYPE PhSecurityInformation_QueryInterface(
    __in ISecurityInformation *This,
    __in REFIID Riid,
    __out PVOID *Object
    )
{
    if (
        IsEqualIID(Riid, &IID_IUnknown) ||
        IsEqualIID(Riid, &IID_ISecurityInformation)
        )
    {
        PhSecurityInformation_AddRef(This);
        *Object = This;
        return S_OK;
    }

    *Object = NULL;
    return E_NOINTERFACE;
}

ULONG STDMETHODCALLTYPE PhSecurityInformation_AddRef(
    __in ISecurityInformation *This
    )
{
    PhSecurityInformation *this = (PhSecurityInformation *)This;

    this->RefCount++;

    return this->RefCount;
}

ULONG STDMETHODCALLTYPE PhSecurityInformation_Release(
    __in ISecurityInformation *This
    )
{
    PhSecurityInformation *this = (PhSecurityInformation *)This;

    this->RefCount--;

    if (this->RefCount == 0)
    {
        if (this->ObjectName) PhDereferenceObject(this->ObjectName);
        PhFree(this->AccessEntries);

        PhFree(this);

        return 0;
    }

    return this->RefCount;
}

HRESULT STDMETHODCALLTYPE PhSecurityInformation_GetObjectInformation(
    __in ISecurityInformation *This,
    __out PSI_OBJECT_INFO ObjectInfo
    )
{
    PhSecurityInformation *this = (PhSecurityInformation *)This;

    memset(ObjectInfo, 0, sizeof(SI_OBJECT_INFO));
    ObjectInfo->dwFlags =
        SI_EDIT_AUDITS |
        SI_EDIT_OWNER |
        SI_EDIT_PERMS |
        SI_ADVANCED |
        SI_NO_ACL_PROTECT |
        SI_NO_TREE_APPLY;
    ObjectInfo->hInstance = NULL;
    ObjectInfo->pszObjectName = this->ObjectName->Buffer;

    return S_OK;
}

HRESULT STDMETHODCALLTYPE PhSecurityInformation_GetSecurity(
    __in ISecurityInformation *This,
    __in SECURITY_INFORMATION RequestedInformation,
    __out PSECURITY_DESCRIPTOR *SecurityDescriptor,
    __in BOOL Default
    )
{
    PhSecurityInformation *this = (PhSecurityInformation *)This;
    NTSTATUS status;
    PSECURITY_DESCRIPTOR securityDescriptor;
    ULONG sdLength;
    PSECURITY_DESCRIPTOR newSd;

    status = this->GetObjectSecurity(
        &securityDescriptor,
        RequestedInformation,
        this->Context
        );

    if (!NT_SUCCESS(status))
        return HRESULT_FROM_WIN32(PhNtStatusToDosError(status));

    sdLength = RtlLengthSecurityDescriptor(securityDescriptor);
    newSd = LocalAlloc(0, sdLength);
    memcpy(newSd, securityDescriptor, sdLength);
    PhFree(securityDescriptor);

    *SecurityDescriptor = newSd;

    return S_OK;
}

HRESULT STDMETHODCALLTYPE PhSecurityInformation_SetSecurity(
    __in ISecurityInformation *This,
    __in SECURITY_INFORMATION SecurityInformation,
    __in PSECURITY_DESCRIPTOR SecurityDescriptor
    )
{
    PhSecurityInformation *this = (PhSecurityInformation *)This;
    NTSTATUS status;

    status = this->SetObjectSecurity(
        SecurityDescriptor,
        SecurityInformation,
        this->Context
        );

    if (!NT_SUCCESS(status))
        return HRESULT_FROM_WIN32(PhNtStatusToDosError(status));

    return S_OK;
}

HRESULT STDMETHODCALLTYPE PhSecurityInformation_GetAccessRights(
    __in ISecurityInformation *This,
    __in const GUID *ObjectType,
    __in ULONG Flags,
    __out PSI_ACCESS *Access,
    __out PULONG Accesses,
    __out PULONG DefaultAccess
    )
{
    PhSecurityInformation *this = (PhSecurityInformation *)This;

    *Access = this->AccessEntries;
    *Accesses = this->NumberOfAccessEntries;
    *DefaultAccess = 0;

    return S_OK;
}

HRESULT STDMETHODCALLTYPE PhSecurityInformation_MapGeneric(
    __in ISecurityInformation *This,
    __in const GUID *ObjectType,
    __in PUCHAR AceFlags,
    __inout PACCESS_MASK Mask
    )
{
    return S_OK;
}

HRESULT STDMETHODCALLTYPE PhSecurityInformation_GetInheritTypes(
    __in ISecurityInformation *This,
    __out PSI_INHERIT_TYPE *InheritTypes,
    __out PULONG InheritTypesCount
    )
{
    return E_NOTIMPL;
}

HRESULT STDMETHODCALLTYPE PhSecurityInformation_PropertySheetPageCallback(
    __in ISecurityInformation *This,
    __in HWND hwnd,
    __in UINT uMsg,
    __in SI_PAGE_TYPE uPage
    )
{
    return E_NOTIMPL;
}

/**
 * Retrieves the security descriptor of an object.
 *
 * \param SecurityDescriptor A variable which receives a pointer to
 * the security descriptor of the object. The security descriptor
 * must be freed using PhFree() when no longer needed.
 * \param SecurityInformation The security information to retrieve.
 * \param Context A pointer to a PH_STD_OBJECT_SECURITY structure
 * describing the object.
 *
 * \remarks This function may be used for the \a GetObjectSecurity
 * callback in PhCreateSecurityPage() or PhEditSecurity().
 */
__callback NTSTATUS PhStdGetObjectSecurity(
    __out PSECURITY_DESCRIPTOR *SecurityDescriptor,
    __in SECURITY_INFORMATION SecurityInformation,
    __in_opt PVOID Context
    )
{
    NTSTATUS status;
    PPH_STD_OBJECT_SECURITY stdObjectSecurity;
    HANDLE handle;

    stdObjectSecurity = (PPH_STD_OBJECT_SECURITY)Context;

    status = stdObjectSecurity->OpenObject(
        &handle,
        PhGetAccessForGetSecurity(SecurityInformation),
        stdObjectSecurity->Context
        );

    if (!NT_SUCCESS(status))
        return status;

    if (WSTR_IEQUAL(stdObjectSecurity->ObjectType, L"Service"))
    {
        status = PhGetSeObjectSecurity(handle, SE_SERVICE, SecurityInformation, SecurityDescriptor);
        CloseServiceHandle(handle);
    }
    else
    {
        status = PhGetObjectSecurity(handle, SecurityInformation, SecurityDescriptor);
        NtClose(handle);
    }

    return status;
}

/**
 * Modifies the security descriptor of an object.
 *
 * \param SecurityDescriptor A security descriptor containing
 * security information to set.
 * \param SecurityInformation The security information to retrieve.
 * \param Context A pointer to a PH_STD_OBJECT_SECURITY structure
 * describing the object.
 *
 * \remarks This function may be used for the \a SetObjectSecurity
 * callback in PhCreateSecurityPage() or PhEditSecurity().
 */
__callback NTSTATUS PhStdSetObjectSecurity(
    __in PSECURITY_DESCRIPTOR SecurityDescriptor,
    __in SECURITY_INFORMATION SecurityInformation,
    __in_opt PVOID Context
    )
{
    NTSTATUS status;
    PPH_STD_OBJECT_SECURITY stdObjectSecurity;
    HANDLE handle;

    stdObjectSecurity = (PPH_STD_OBJECT_SECURITY)Context;

    status = stdObjectSecurity->OpenObject(
        &handle,
        PhGetAccessForSetSecurity(SecurityInformation),
        stdObjectSecurity->Context
        );

    if (!NT_SUCCESS(status))
        return status;

    if (WSTR_IEQUAL(stdObjectSecurity->ObjectType, L"Service"))
    {
        status = PhSetSeObjectSecurity(handle, SE_SERVICE, SecurityInformation, SecurityDescriptor);
        CloseServiceHandle(handle);
    }
    else
    {
        status = PhSetObjectSecurity(handle, SecurityInformation, SecurityDescriptor);
        NtClose(handle);
    }

    return status;
}

NTSTATUS PhGetSeObjectSecurity(
    __in HANDLE Handle,
    __in ULONG ObjectType,
    __in SECURITY_INFORMATION SecurityInformation,
    __out PSECURITY_DESCRIPTOR *SecurityDescriptor
    )
{
    ULONG win32Result;
    PSECURITY_DESCRIPTOR securityDescriptor;

    win32Result = GetSecurityInfo(
        Handle,
        ObjectType,
        SecurityInformation,
        NULL,
        NULL,
        NULL,
        NULL,
        &securityDescriptor
        );

    if (win32Result != ERROR_SUCCESS)
        return NTSTATUS_FROM_WIN32(win32Result);

    *SecurityDescriptor = PhAllocateCopy(
        securityDescriptor,
        RtlLengthSecurityDescriptor(securityDescriptor)
        );
    LocalFree(securityDescriptor);

    return STATUS_SUCCESS;
}

NTSTATUS PhSetSeObjectSecurity(
    __in HANDLE Handle,
    __in ULONG ObjectType,
    __in SECURITY_INFORMATION SecurityInformation,
    __in PSECURITY_DESCRIPTOR SecurityDescriptor
    )
{
    ULONG win32Result;
    SECURITY_INFORMATION securityInformation = 0;
    BOOLEAN present;
    BOOLEAN defaulted;
    PSID owner = NULL;
    PSID group = NULL;
    PACL dacl = NULL;
    PACL sacl = NULL;

    if (SecurityInformation & OWNER_SECURITY_INFORMATION)
    {
        if (NT_SUCCESS(RtlGetOwnerSecurityDescriptor(SecurityDescriptor, &owner, &defaulted)))
            securityInformation |= OWNER_SECURITY_INFORMATION;
    }

    if (SecurityInformation & GROUP_SECURITY_INFORMATION)
    {
        if (NT_SUCCESS(RtlGetGroupSecurityDescriptor(SecurityDescriptor, &group, &defaulted)))
            securityInformation |= GROUP_SECURITY_INFORMATION;
    }

    if (SecurityInformation & DACL_SECURITY_INFORMATION)
    {
        if (NT_SUCCESS(RtlGetDaclSecurityDescriptor(SecurityDescriptor, &present, &dacl, &defaulted)) && present)
            securityInformation |= DACL_SECURITY_INFORMATION;
    }

    if (SecurityInformation & SACL_SECURITY_INFORMATION)
    {
        if (NT_SUCCESS(RtlGetSaclSecurityDescriptor(SecurityDescriptor, &present, &sacl, &defaulted)) && present)
            securityInformation |= SACL_SECURITY_INFORMATION;
    }

    win32Result = SetSecurityInfo(
        Handle,
        ObjectType,
        SecurityInformation,
        owner,
        group,
        dacl,
        sacl
        );

    if (win32Result != ERROR_SUCCESS)
        return NTSTATUS_FROM_WIN32(win32Result);

    return STATUS_SUCCESS;
}
