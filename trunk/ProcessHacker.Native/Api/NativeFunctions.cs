﻿/*
 * Process Hacker - 
 *   native API functions
 *                       
 * Copyright (C) 2009 Flavio Erlich
 * Copyright (C) 2008-2009 wj32
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

#pragma warning disable 1573

using System;
using System.Runtime.InteropServices;
using ProcessHacker.Native.Security;

namespace ProcessHacker.Native.Api
{
    public static partial class Win32
    {
        // IMPORTANT: All timeouts, etc. are in 100ns units except when stated otherwise.

        #region System Calls

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtAcceptConnectPort(
            [Out] out IntPtr PortHandle,
            [In] [Optional] IntPtr PortContext,
            [In] IntPtr ConnectionRequest,
            [In] bool AcceptConnection,
            [Optional] ref PortView ServerView,
            [Out] [Optional] out RemotePortView ClientView
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtAccessCheck(
            [In] IntPtr SecurityDescriptor,
            [In] IntPtr ClientToken,
            [In] int DesiredAccess,
            [In] ref GenericMapping GenericMapping,
            [In] [Optional] IntPtr PrivilegeSet, // out PrivilegeSet*
            ref int PrivilegeSetLength,
            [Out] out int GrantedAccess,
            [Out] out NtStatus AccessStatus
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtAdjustGroupsToken(
            [In] IntPtr TokenHandle,
            [In] bool ResetToDefault,
            [In] ref TokenGroups NewState,
            [In] [Optional] int BufferLength,
            [In] [Optional] IntPtr PreviousState, // out TokenGroups*
            [In] [Optional] IntPtr ReturnLength // out int*
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtAdjustPrivilegesToken(
            [In] IntPtr TokenHandle,
            [In] bool DisableAllPrivileges,
            [In] [Optional] ref TokenPrivileges NewState,
            [In] [Optional] int BufferLength,
            [In] [Optional] IntPtr PreviousState, // out TokenPrivileges*
            [In] [Optional] IntPtr ReturnLength // out int*
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtAlertThread(
            [In] IntPtr ThreadHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtAlertResumeThread(
            [In] IntPtr ThreadHandle,
            [Out] [Optional] out int PreviousSuspendCount
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtAllocateLocallyUniqueId(
            [Out] out Luid Luid
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtAllocateVirtualMemory(
            [In] IntPtr ProcessHandle,
            ref IntPtr BaseAddress,
            [In] IntPtr ZeroBits,
            ref IntPtr RegionSize,
            [In] MemoryFlags AllocationType,
            [In] MemoryProtection Protect
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtAreMappedFilesTheSame(
            [In] IntPtr File1MappedAsAnImage,
            [In] IntPtr File2MappedAsFile
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtAssignProcessToJobObject(
            [In] IntPtr JobHandle,
            [In] IntPtr ProcessHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCancelTimer(
            [In] IntPtr TimerHandle,
            [Out] [Optional] out bool CurrentState
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtClearEvent(
            [In] IntPtr EventHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtClose(
            [In] IntPtr Handle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCompareTokens(
            [In] IntPtr FirstTokenHandle,
            [In] IntPtr SecondTokenHandle,
            [MarshalAs(UnmanagedType.I1)]
            [Out] out bool Equal
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCompleteConnectPort(
            [In] IntPtr PortHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtConnectPort(
            [Out] out IntPtr PortHandle,
            [In] ref UnicodeString PortName,
            [In] ref SecurityQualityOfService SecurityQos,
            [Optional] ref PortView ClientView,
            [Optional] ref RemotePortView ServerView,
            [Out] [Optional] out int MaxMessageLength,
            [Optional] IntPtr ConnectionInformation,
            [Optional] ref int ConnectionInformationLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtConnectPort(
            [Out] out IntPtr PortHandle,
            [In] ref UnicodeString PortName,
            [In] ref SecurityQualityOfService SecurityQos,
            [Optional] IntPtr ClientView,
            [Optional] IntPtr ServerView,
            [Out] [Optional] out int MaxMessageLength,
            [Optional] IntPtr ConnectionInformation,
            [Optional] ref int ConnectionInformationLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtConnectPort(
            [Out] out IntPtr PortHandle,
            [In] ref UnicodeString PortName,
            [In] ref SecurityQualityOfService SecurityQos,
            [Optional] ref PortView ClientView,
            [Optional] ref RemotePortView ServerView,
            [Out] [Optional] IntPtr MaxMessageLength,
            [Optional] IntPtr ConnectionInformation,
            [Optional] IntPtr ConnectionInformationLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtConnectPort(
            [Out] out IntPtr PortHandle,
            [In] ref UnicodeString PortName,
            [In] ref SecurityQualityOfService SecurityQos,
            [Optional] IntPtr ClientView,
            [Optional] IntPtr ServerView,
            [Out] [Optional] IntPtr MaxMessageLength,
            [Optional] IntPtr ConnectionInformation,
            [Optional] IntPtr ConnectionInformationLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateDebugObject(
            [Out] out IntPtr DebugObjectHandle,
            [In] DebugObjectAccess DesiredAccess,
            [In] [Optional] ref ObjectAttributes ObjectAttributes,
            [In] DebugObjectFlags Flags
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateDebugObject(
            [Out] out IntPtr DebugObjectHandle,
            [In] DebugObjectAccess DesiredAccess,
            [In] [Optional] IntPtr ObjectAttributes,
            [In] DebugObjectFlags Flags
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateDirectoryObject(
            [Out] out IntPtr DirectoryHandle,
            [In] DirectoryAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateEvent(
            [Out] out IntPtr EventHandle,
            [In] EventAccess DesiredAccess,
            [In] [Optional] ref ObjectAttributes ObjectAttributes,
            [In] EventType EventType,
            [In] bool InitialState
            ); 

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateEvent(
            [Out] out IntPtr EventHandle,
            [In] EventAccess DesiredAccess,
            [In] [Optional] IntPtr ObjectAttributes,
            [In] EventType EventType,
            [In] bool InitialState
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateEventPair(
            [Out] out IntPtr EventPairHandle,
            [In] EventPairAccess DesiredAccess,
            [In] [Optional] ref ObjectAttributes ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateEventPair(
            [Out] out IntPtr EventPairHandle,
            [In] EventPairAccess DesiredAccess,
            [In] [Optional] IntPtr ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateIoCompletion(
            [Out] out IntPtr IoCompletionHandle,
            [In] IoCompletionAccess DesiredAccess,
            [In] [Optional] ref ObjectAttributes ObjectAttributes,
            [In] [Optional] int Count
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateIoCompletion(
            [Out] out IntPtr IoCompletionHandle,
            [In] IoCompletionAccess DesiredAccess,
            [In] [Optional] IntPtr ObjectAttributes,
            [In] [Optional] int Count
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateJobObject(
            [Out] out IntPtr JobHandle,
            [In] JobObjectAccess DesiredAccess,
            [In] [Optional] ref ObjectAttributes ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateJobObject(
            [Out] out IntPtr JobHandle,
            [In] JobObjectAccess DesiredAccess,
            [In] [Optional] IntPtr ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateJobSet(
            [In] int NumJob,
            JobSetArray[] UserJobSet,
            [In] int Flags
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateKeyedEvent(
            [Out] out IntPtr KeyedEventHandle,
            [In] KeyedEventAccess DesiredAccess,
            [In] [Optional] ref ObjectAttributes ObjectAttributes,
            [In] int Flags
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateKeyedEvent(
            [Out] out IntPtr KeyedEventHandle,
            [In] KeyedEventAccess DesiredAccess,
            [In] [Optional] IntPtr ObjectAttributes,
            [In] int Flags
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateMutant(
            [Out] out IntPtr MutantHandle,
            [In] MutantAccess DesiredAccess,
            [In] [Optional] ref ObjectAttributes ObjectAttributes,
            [In] bool InitialOwner
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateMutant(
            [Out] out IntPtr MutantHandle,
            [In] MutantAccess DesiredAccess,
            [In] [Optional] IntPtr ObjectAttributes,
            [In] bool InitialOwner
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreatePort(
            [Out] out IntPtr PortHandle,
            [In] ref ObjectAttributes ObjectAttributes,
            [In] int MaxConnectionInfoLength,
            [In] int MaxMessageLength,
            [In] [Optional] int MaxPoolUsage
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateProcess(
            [Out] out IntPtr ProcessHandle,
            [In] ProcessAccess DesiredAccess,
            [In] [Optional] ref ObjectAttributes ObjectAttributes,
            [In] IntPtr ParentProcess,
            [In] bool InheritHandleTable,
            [In] [Optional] IntPtr SectionHandle,
            [In] [Optional] IntPtr DebugPort,
            [In] [Optional] IntPtr ExceptionPort
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateProcess(
            [Out] out IntPtr ProcessHandle,
            [In] ProcessAccess DesiredAccess,
            [In] [Optional] IntPtr ObjectAttributes,
            [In] IntPtr ParentProcess,
            [In] bool InheritHandleTable,
            [In] [Optional] IntPtr SectionHandle,
            [In] [Optional] IntPtr DebugPort,
            [In] [Optional] IntPtr ExceptionPort
            );

        /// <summary>
        /// Creates a profile object.
        /// </summary>
        /// <param name="ProfileHandle">A handle to the profile object.</param>
        /// <param name="ProcessHandle">
        /// A handle to the process to profile. If NULL, all address spaces are profiled.
        /// </param>
        /// <param name="ProfileBase">
        /// The first address at which to collect profiling information.
        /// </param>
        /// <param name="ProfileSize">
        /// The size of the range to profile. ProfileBase &lt;= address &lt; 
        /// ProfileBase + ProfileSize will generate a hit.
        /// </param>
        /// <param name="BucketSize">
        /// A log2 value of each address bucket. Acceptable values are from 2 to 30.
        /// </param>
        /// <param name="Buffer">An array of int hit counters.</param>
        /// <param name="BufferSize">The size of the buffer, in bytes.</param>
        /// <param name="ProfileSource">The profiling source.</param>
        /// <param name="Affinity">The processors to profile.</param>
        /// <returns>A NTSTATUS value.</returns>
        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateProfile(
            [Out] out IntPtr ProfileHandle,
            [In] [Optional] IntPtr ProcessHandle,
            [In] IntPtr ProfileBase,
            [In] IntPtr ProfileSize,
            [In] int BucketSize,
            [In] IntPtr Buffer,
            [In] int BufferSize,
            [In] KProfileSource ProfileSource,
            [In] IntPtr Affinity
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateSection(
            [Out] out IntPtr SectionHandle,
            [In] SectionAccess DesiredAccess,
            [In] [Optional] ref ObjectAttributes ObjectAttributes,
            [In] [Optional] ref long MaximumSize,
            [In] MemoryProtection PageAttributes,
            [In] SectionAttributes SectionAttributes,
            [In] [Optional] IntPtr FileHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateSection(
            [Out] out IntPtr SectionHandle,
            [In] SectionAccess DesiredAccess,
            [In] [Optional] ref ObjectAttributes ObjectAttributes,
            [In] [Optional] IntPtr MaximumSize,
            [In] MemoryProtection PageAttributes,
            [In] SectionAttributes SectionAttributes,
            [In] [Optional] IntPtr FileHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateSection(
            [Out] out IntPtr SectionHandle,
            [In] SectionAccess DesiredAccess,
            [In] [Optional] IntPtr ObjectAttributes,
            [In] [Optional] ref long MaximumSize,
            [In] int PageAttributes,
            [In] int SectionAttributes,
            [In] [Optional] IntPtr FileHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateSemaphore(
            [Out] out IntPtr SemaphoreHandle,
            [In] SemaphoreAccess DesiredAccess,
            [In] [Optional] ref ObjectAttributes ObjectAttributes,
            [In] int InitialCount,
            [In] int MaximumCount
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateSemaphore(
            [Out] out IntPtr SemaphoreHandle,
            [In] SemaphoreAccess DesiredAccess,
            [In] [Optional] IntPtr ObjectAttributes,
            [In] int InitialCount,
            [In] int MaximumCount
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateSymbolicLinkObject(
            [Out] out IntPtr LinkHandle,
            [In] SymbolicLinkAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes,
            [In] ref UnicodeString LinkTarget
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateThread(
            [Out] out IntPtr ThreadHandle,
            [In] ThreadAccess DesiredAccess,
            [In] [Optional] ref ObjectAttributes ObjectAttributes,
            [In] IntPtr ProcessHandle,
            [Out] out ClientId ClientId,
            [In] ref Context ThreadContext,
            [In] ref InitialTeb InitialTeb,
            [In] bool CreateSuspended
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateTimer(
            [Out] out IntPtr TimerHandle,
            [In] TimerAccess DesiredAccess,
            [In] [Optional] ref ObjectAttributes ObjectAttributes,
            [In] TimerType TimerType
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateTimer(
            [Out] out IntPtr TimerHandle,
            [In] TimerAccess DesiredAccess,
            [In] [Optional] IntPtr ObjectAttributes,
            [In] TimerType TimerType
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateToken(
            [Out] out IntPtr TokenHandle,
            [In] TokenAccess DesiredAccess,
            [In] [Optional] ref ObjectAttributes ObjectAttributes,
            [In] TokenType TokenType,
            [In] ref Luid AuthenticationId,
            [In] ref long ExpirationTime,
            [In] ref TokenUser User,
            [In] ref TokenGroups Groups,
            [In] ref TokenPrivileges Privileges,
            [In] [Optional] ref TokenOwner Owner,
            [In] ref TokenPrimaryGroup PrimaryGroup,
            [In] [Optional] ref TokenDefaultDacl DefaultDacl,
            [In] ref TokenSource TokenSource
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtCreateWaitablePort(
            [Out] out IntPtr PortHandle,
            [In] ref ObjectAttributes ObjectAttributes,
            [In] int MaxConnectionInfoLength,
            [In] int MaxMessageLength,
            [In] [Optional] int MaxPoolUsage
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtDebugActiveProcess(
            [In] IntPtr ProcessHandle,
            [In] IntPtr DebugObjectHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtDebugContinue(
            [In] IntPtr DebugObjectHandle,
            [In] ref ClientId ClientId,
            [In] NtStatus ContinueStatus
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtDelayExecution(
            [In] bool Alertable,
            [In] ref long DelayInterval
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtDeviceIoControlFile(
            [In] IntPtr FileHandle,
            [In] IntPtr Event,
            [In] IntPtr ApcRoutine,
            [In] IntPtr ApcContext,
            [Out] out IoStatusBlock IoStatusBlock,
            [In] int IoControlCode,
            [In] IntPtr InputBuffer,
            [In] int InputBufferLength,
            [In] IntPtr OutputBuffer,
            [In] int OutputBufferLength
            );

        [DllImport("ntdll.dll")]
        public unsafe static extern NtStatus NtDeviceIoControlFile(
            [In] IntPtr FileHandle,
            [In] IntPtr Event,
            [In] IntPtr ApcRoutine,
            [In] IntPtr ApcContext,
            [Out] out IoStatusBlock IoStatusBlock,
            [In] int IoControlCode,
            [In] void* InputBuffer,
            [In] int InputBufferLength,
            [In] void* OutputBuffer,
            [In] int OutputBufferLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtDuplicateObject(
            [In] IntPtr SourceProcessHandle,
            [In] IntPtr SourceHandle,
            [In] IntPtr TargetProcessHandle,
            [Out] out IntPtr TargetHandle,
            [In] int DesiredAccess,
            [In] HandleFlags Attributes,
            [In] DuplicateOptions Options
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtDuplicateToken(
            [In] IntPtr ExistingTokenHandle,
            [In] TokenAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes,
            [In] bool EffectiveOnly,
            [In] TokenType TokenType,
            [Out] out IntPtr NewTokenHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtExtendSection(
            [In] IntPtr SectionHandle,
            ref long NewSectionSize
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtFilterToken(
            [In] IntPtr ExistingTokenHandle,
            [In] int Flags,
            [In] [Optional] ref TokenGroups SidsToDisable,
            [In] [Optional] ref TokenPrivileges PrivilegesToDelete,
            [In] [Optional] ref TokenGroups RestrictedSids,
            [Out] out IntPtr NewTokenHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtFlushVirtualMemory(
            [In] IntPtr ProcessHandle,
            ref IntPtr BaseAddress,
            ref IntPtr RegionSize,
            [Out] out IoStatusBlock IoStatus
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtFreeVirtualMemory(
            [In] IntPtr ProcessHandle,
            ref IntPtr BaseAddress,
            ref IntPtr RegionSize,
            [In] MemoryFlags FreeType
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtGetContextThread(
            [In] IntPtr ThreadHandle,
            ref Context ThreadContext
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtGetCurrentProcessorNumber();

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtGetNextProcess(
            [In] [Optional] IntPtr ProcessHandle,
            [In] ProcessAccess DesiredAccess,
            [In] HandleFlags HandleAttributes,
            [In] int Flags,
            [Out] out IntPtr NewProcessHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtGetNextThread(
            [In] [Optional] IntPtr ProcessHandle,
            [In] [Optional] IntPtr ThreadHandle,
            [In] ThreadAccess DesiredAccess,
            [In] HandleFlags HandleAttributes,
            [In] int Flags,
            [Out] out IntPtr NewThreadHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtImpersonateAnonymousToken(
            [In] IntPtr ThreadHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtImpersonateClientOfPort(
            [In] IntPtr PortHandle,
            [In] IntPtr Message
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtImpersonateThread(
            [In] IntPtr ServerThreadHandle,
            [In] IntPtr ClientThreadHandle,
            [In] ref SecurityQualityOfService SecurityQos
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtIsProcessInJob(
            [In] IntPtr ProcessHandle,
            [In] [Optional] IntPtr JobHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtListenPort(
            [In] IntPtr PortHandle,
            [In] IntPtr ConnectionRequest
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtLoadDriver(
            [In] ref UnicodeString DriverPath
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtLockVirtualMemory(
            [In] IntPtr ProcessHandle,
            ref IntPtr BaseAddress,
            ref IntPtr RegionSize,
            [In] MemoryFlags MapType
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtMakePermanentObject(
            [In] IntPtr Handle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtMakeTemporaryObject(
            [In] IntPtr Handle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtMapViewOfSection(
            [In] IntPtr SectionHandle,
            [In] IntPtr ProcessHandle,
            ref IntPtr BaseAddress,
            [In] IntPtr ZeroBits,
            [In] IntPtr CommitSize,
            [Optional] ref long SectionOffset,
            ref IntPtr ViewSize,
            [In] SectionInherit InheritDisposition,
            [In] MemoryFlags AllocationType,
            [In] MemoryProtection Win32Protect
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenDirectoryObject(
            [Out] out IntPtr DirectoryHandle,
            [In] DirectoryAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenEvent(
            [Out] out IntPtr EventHandle,
            [In] EventAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenEventPair(
            [Out] out IntPtr EventPairHandle,
            [In] EventPairAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenFile(
            [Out] out IntPtr FileHandle,
            [In] FileAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes,
            [Out] out IoStatusBlock IoStatusBlock,
            [In] FileShareMode ShareAccess,
            [In] FileCreationDisposition OpenOptions
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenIoCompletion(
            [Out] out IntPtr IoCompletionHandle,
            [In] IoCompletionAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenJobObject(
            [Out] out IntPtr JobHandle,
            [In] JobObjectAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenKeyedEvent(
            [Out] out IntPtr KeyedEventHandle,
            [In] KeyedEventAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenMutant(
            [Out] out IntPtr MutantHandle,
            [In] MutantAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenProcess(
            [Out] out IntPtr ProcessHandle,
            [In] ProcessAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes,
            [In] [Optional] ref ClientId ClientId
            );      

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenProcess(
            [Out] out IntPtr ProcessHandle,
            [In] ProcessAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes,
            [In] [Optional] IntPtr ClientId
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenProcessToken(
            [In] IntPtr ProcessHandle,
            [In] TokenAccess DesiredAccess,
            [Out] out IntPtr TokenHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenProcessTokenEx(
            [In] IntPtr ProcessHandle,
            [In] TokenAccess DesiredAccess,
            [In] int HandleAttributes,
            [Out] out IntPtr TokenHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenSection(
            [Out] out IntPtr SectionHandle,
            [In] SectionAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenSemaphore(
            [Out] out IntPtr SemaphoreHandle,
            [In] SemaphoreAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenSymbolicLinkObject(
            [Out] out IntPtr LinkHandle,
            [In] SymbolicLinkAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenThread(
            [Out] out IntPtr ThreadHandle,
            [In] ThreadAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes,
            [In] [Optional] ref ClientId ClientId
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenThread(
            [Out] out IntPtr ThreadHandle,
            [In] ThreadAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes,
            [In] [Optional] IntPtr ClientId
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenThreadToken(
            [In] IntPtr ThreadHandle,
            [In] TokenAccess DesiredAccess,
            [In] bool OpenAsSelf,
            [Out] out IntPtr TokenHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenThreadTokenEx(
            [In] IntPtr ThreadHandle,
            [In] TokenAccess DesiredAccess,
            [In] bool OpenAsSelf,
            [In] int HandleAttributes,
            [Out] out IntPtr TokenHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtOpenTimer(
            [Out] out IntPtr TimerHandle,
            [In] TimerAccess DesiredAccess,
            [In] ref ObjectAttributes ObjectAttributes
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtPrivilegeCheck(
            [In] IntPtr ClientToken,
            [In] IntPtr RequiredPrivileges, // PrivilegeSet*
            [MarshalAs(UnmanagedType.U1)]
            [Out] out bool Result
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtProtectVirtualMemory(
            [In] IntPtr ProcessHandle,
            ref IntPtr BaseAddress,
            ref IntPtr RegionSize,
            [In] MemoryProtection NewProtect,
            [Out] out MemoryProtection OldProtect
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtPulseEvent(
            [In] IntPtr EventHandle,
            [Out] [Optional] out int PreviousState
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryDirectoryObject(
            [In] IntPtr DirectoryHandle,
            [In] IntPtr Buffer,
            [In] int Length,
            [In] bool ReturnSingleEntry,
            [In] bool RestartScan,
            ref int Context,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryEvent(
            [In] IntPtr EventHandle,
            [In] EventInformationClass EventInformationClass,
            [Out] out EventBasicInformation EventInformation,
            [In] int EventInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationFile(
            [In] IntPtr FileHandle,
            [Out] out IoStatusBlock IoStatusBlock,
            [In] IntPtr FileInformation,
            [In] int FileInformationLength,
            [In] FileInformationClass FileInformationClass
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationJobObject(
            [In] [Optional] IntPtr JobHandle,
            [In] JobObjectInformationClass JobObjectInformationClass,
            [In] IntPtr JobObjectInformation,
            [In] int JobObjectInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationProcess(
            [In] IntPtr ProcessHandle,
            [In] ProcessInformationClass ProcessInformationClass,
            IntPtr ProcessInformation,
            [In] int ProcessInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationProcess(
            [In] IntPtr ProcessHandle,
            [In] ProcessInformationClass ProcessInformationClass,
            [Out] out int ProcessInformation,
            [In] int ProcessInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationProcess(
            [In] IntPtr ProcessHandle,
            [In] ProcessInformationClass ProcessInformationClass,
            [Out] out IntPtr ProcessInformation,
            [In] int ProcessInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationProcess(
            [In] IntPtr ProcessHandle,
            [In] ProcessInformationClass ProcessInformationClass,
            [Out] out PooledUsageAndLimits ProcessInformation,
            [In] int ProcessInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationProcess(
            [In] IntPtr ProcessHandle,
            [In] ProcessInformationClass ProcessInformationClass,
            [Out] out QuotaLimits ProcessInformation,
            [In] int ProcessInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationProcess(
            [In] IntPtr ProcessHandle,
            [In] ProcessInformationClass ProcessInformationClass,
            [Out] out MemExecuteOptions ProcessInformation,
            [In] int ProcessInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationProcess(
            [In] IntPtr ProcessHandle,
            [In] ProcessInformationClass ProcessInformationClass,
            [Out] out ProcessBasicInformation ProcessInformation,
            [In] int ProcessInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationProcess(
            [In] IntPtr ProcessHandle,
            [In] ProcessInformationClass ProcessInformationClass,
            [Out] out UnicodeString ProcessInformation,
            [In] int ProcessInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationThread(
            [In] IntPtr ThreadHandle,
            [In] ThreadInformationClass ThreadInformationClass,
            ref ThreadBasicInformation ThreadInformation,
            [In] int ThreadInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationThread(
            [In] IntPtr ThreadHandle,
            [In] ThreadInformationClass ThreadInformationClass,
            [Out] out int ThreadInformation,
            [In] int ThreadInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationThread(
            [In] IntPtr ThreadHandle,
            [In] ThreadInformationClass ThreadInformationClass,
            [Out] out IntPtr ThreadInformation,
            [In] int ThreadInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationThread(
            [In] IntPtr ThreadHandle,
            [In] ThreadInformationClass ThreadInformationClass,
            IntPtr ThreadInformation,
            [In] int ThreadInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public unsafe static extern NtStatus NtQueryInformationThread(
            [In] IntPtr ThreadHandle,
            [In] ThreadInformationClass ThreadInformationClass,
            void* ThreadInformation,
            [In] int ThreadInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryInformationToken(
            [In] IntPtr TokenHandle,
            [In] TokenInformationClass TokenInformationClass,
            [In] IntPtr TokenInformation,
            [In] int TokenInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryIntervalProfile(
            [In] KProfileSource Source,
            [Out] out int Interval 
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryIoCompletion(
            [In] IntPtr IoCompletionHandle,
            [In] IoCompletionInformationClass IoCompletionInformationClass,
            [Out] out IoCompletionBasicInformation IoCompletionInformation,
            [In] int IoCompletionInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryMutant(
            [In] IntPtr MutantHandle,
            [In] MutantInformationClass MutantInformationClass,
            [Out] out MutantBasicInformation MutantInformation,
            [In] int MutantInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryObject(
            [In] IntPtr Handle,
            [In] ObjectInformationClass ObjectInformationClass,
            [Out] IntPtr ObjectInformation,
            [In] int ObjectInformationLength,
            [Out] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryPortInformationProcess();

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQuerySection(
            [In] IntPtr SectionHandle,
            [In] SectionInformationClass SectionInformationClass,
            [Out] out SectionBasicInformation SectionInformation,
            [In] IntPtr SectionInformationLength,
            [Out] [Optional] out IntPtr ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQuerySection(
            [In] IntPtr SectionHandle,
            [In] SectionInformationClass SectionInformationClass,
            [Out] out SectionImageInformation SectionInformation,
            [In] IntPtr SectionInformationLength,
            [Out] [Optional] out IntPtr ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQuerySecurityObject(
            [In] IntPtr Handle,
            [In] SecurityInformation SecurityInformation,
            [In] IntPtr SecurityDescriptor,
            [In] int SecurityDescriptorLength,
            [Out] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQuerySemaphore(
            [In] IntPtr SemaphoreHandle,
            [In] SemaphoreInformationClass SemaphoreInformationClass,
            [Out] out SemaphoreBasicInformation SemaphoreInformation,
            [In] int SemaphoreInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQuerySymbolicLinkObject(
            [In] IntPtr LinkHandle,
            ref UnicodeString LinkName,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQuerySystemInformation(
            [In] SystemInformationClass SystemInformationClass,
            [Out] out SystemBasicInformation SystemInformation,
            [In] int SystemInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQuerySystemInformation(
            [In] SystemInformationClass SystemInformationClass,
            IntPtr SystemInformation,
            [In] int SystemInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQuerySystemInformation(
            [In] SystemInformationClass SystemInformationClass,
            [MarshalAs(UnmanagedType.LPArray)] SystemProcessorPerformanceInformation[] SystemInformation,
            [In] int SystemInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQuerySystemInformation(
            [In] SystemInformationClass SystemInformationClass,
            [Out] out SystemPerformanceInformation SystemInformation,
            [In] int SystemInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQuerySystemInformation(
            [In] SystemInformationClass SystemInformationClass,
            [Out] out SystemCacheInformation SystemInformation,
            [In] int SystemInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryTimer(
            [In] IntPtr TimerHandle,
            [In] TimerInformationClass TimerInformationClass,
            [Out] out TimerBasicInformation TimerInformation,
            [In] int TimerInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryVirtualMemory(
            [In] IntPtr ProcessHandle,
            [In] IntPtr BaseAddress,
            [In] MemoryInformationClass MemoryInformationClass,
            [In] IntPtr Buffer,
            [In] IntPtr MemoryInformationLength,
            [Out] [Optional] out IntPtr ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryVirtualMemory(
            [In] IntPtr ProcessHandle,
            [In] IntPtr BaseAddress,
            [In] MemoryInformationClass MemoryInformationClass,
            [Out] out MemoryBasicInformation Buffer,
            [In] IntPtr MemoryInformationLength,
            [Out] [Optional] out IntPtr ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueryVolumeInformationFile(
            [In] IntPtr FileHandle,
            [Out] out IoStatusBlock IoStatusBlock,
            [In] IntPtr FsInformation,
            [In] int FsInformationLength,
            [In] FsInformationClass FsInformationClass
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtQueueApcThread(
            [In] IntPtr ThreadHandle,
            [In] IntPtr ApcRoutine,
            [In] [Optional] IntPtr ApcArgument1,
            [In] [Optional] IntPtr ApcArgument2,
            [In] [Optional] IntPtr ApcArgument3
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtReadRequestData(
            [In] IntPtr PortHandle,
            [In] IntPtr Message,
            [In] int DataEntryIndex,
            [In] IntPtr Buffer,
            [In] IntPtr BufferSize,
            [Out] [Optional] out IntPtr ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtReadVirtualMemory(
            [In] IntPtr ProcessHandle,
            [In] [Optional] IntPtr BaseAddress,
            [In] IntPtr Buffer,
            [In] IntPtr BufferSize,
            [Out] [Optional] out IntPtr ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtRegisterThreadTerminatePort(
            [In] IntPtr PortHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtReleaseKeyedEvent(
            [In] IntPtr KeyedEventHandle,
            [In] IntPtr KeyValue,
            [In] bool Alertable,
            [In] [Optional] ref long Timeout
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtReleaseMutant(
            [In] IntPtr MutantHandle,
            [Out] [Optional] out int PreviousCount
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtReleaseSemaphore(
            [In] IntPtr SemaphoreHandle,
            [In] int ReleaseCount,
            [Out] [Optional] out int PreviousCount
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtRemoveIoCompletion(
            [In] IntPtr IoCompletionHandle,
            [Out] out IntPtr KeyContext,
            [Out] out IntPtr ApcContext,
            [Out] out IoStatusBlock IoStatusBlock,
            [In] [Optional] ref long Timeout
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtRemoveProcessDebug(
            [In] IntPtr ProcessHandle,
            [In] IntPtr DebugObjectHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtReplyPort(
            [In] IntPtr PortHandle,
            [In] IntPtr ReplyMessage
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtReplyWaitReceivePort(
            [In] IntPtr PortHandle,
            [Out] [Optional] out IntPtr PortContext,
            [In] [Optional] IntPtr ReplyMessage,
            [In] IntPtr ReceiveMessage
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtReplyWaitReceivePortEx(
            [In] IntPtr PortHandle,
            [Out] [Optional] out IntPtr PortContext,
            [In] [Optional] IntPtr ReplyMessage,
            [In] IntPtr ReceiveMessage,
            [In] [Optional] ref long Timeout
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtReplyWaitReplyPort(
            [In] IntPtr PortHandle,
            [In] IntPtr ReplyMessage
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtRequestPort(
            [In] IntPtr PortHandle,
            [In] IntPtr RequestMessage
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtRequestWaitReplyPort(
            [In] IntPtr PortHandle,
            [In] IntPtr RequestMessage,
            [In] IntPtr ReplyMessage
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtResetEvent(
            [In] IntPtr EventHandle,
            [Out] [Optional] out int PreviousState
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtResumeProcess(
            [In] IntPtr ProcessHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtResumeThread(
            [In] IntPtr ThreadHandle,
            [Out] [Optional] out int PreviousSuspendCount
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetContextThread(
            [In] IntPtr ThreadHandle,
            [In] ref Context ThreadContext
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetEvent(
            [In] IntPtr EventHandle,
            [Out] [Optional] out int PreviousState
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetEventBoostPriority(
            [In] IntPtr EventHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetHighEventPair(
            [In] IntPtr EventPairHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetHighWaitLowEventPair(
            [In] IntPtr EventPairHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetInformationDebugObject(
            [In] IntPtr DebugObjectHandle,
            [In] DebugObjectInformationClass DebugObjectInformationClass,
            [In] IntPtr DebugObjectInformation,
            [In] int DebugObjectInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetInformationJobObject(
            [In] IntPtr JobHandle,
            [In] JobObjectInformationClass JobObjectInformationClass,
            [In] IntPtr JobObjectInformation,
            [In] int JobObjectInformationLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetInformationObject(
            [In] IntPtr Handle,
            [In] ObjectInformationClass ObjectInformationClass,
            [In] IntPtr ObjectInformation,
            [In] int ObjectInformationLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetInformationProcess(
            [In] IntPtr ProcessHandle,
            [In] ProcessInformationClass ProcessInformationClass,
            [In] IntPtr ProcessInformation,
            [In] int ProcessInformationLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetInformationProcess(
            [In] IntPtr ProcessHandle,
            [In] ProcessInformationClass ProcessInformationClass,
            [In] ref int ProcessInformation,
            [In] int ProcessInformationLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetInformationProcess(
            [In] IntPtr ProcessHandle,
            [In] ProcessInformationClass ProcessInformationClass,
            [In] ref ProcessHandleTracingEnable ProcessInformation,
            [In] int ProcessInformationLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetInformationThread(
            [In] IntPtr ThreadHandle,
            [In] ThreadInformationClass ThreadInformationClass,
            [In] IntPtr ThreadInformation,
            [In] int ThreadInformationLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetInformationThread(
            [In] IntPtr ThreadHandle,
            [In] ThreadInformationClass ThreadInformationClass,
            [In] ref int ThreadInformation,
            [In] int ThreadInformationLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetInformationToken(
            [In] IntPtr TokenHandle,
            [In] TokenInformationClass TokenInformationClass,
            [In] IntPtr TokenInformation,
            [In] int TokenInformationLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetIntervalProfile(
            [In] int Interval,
            [In] KProfileSource Source
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetIoCompletion(
            [In] IntPtr IoCompletionHandle,
            [In] IntPtr KeyContext,
            [In] [Optional] IntPtr ApcContext,
            [In] NtStatus IoStatus,
            [In] IntPtr IoStatusInformation
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetLowEventPair(
            [In] IntPtr EventPairHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetLowWaitHighEventPair(
            [In] IntPtr EventPairHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetSecurityObject(
            [In] IntPtr Handle,
            [In] SecurityInformation SecurityInformation,
            [In] IntPtr SecurityDescriptor
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetSystemInformation(
            [In] SystemInformationClass SystemInformationClass,
            [In] ref SystemLoadAndCallImage SystemInformation,
            [In] int SystemInformationLength
            );

        /// <param name="Period">Period, in milliseconds.</param>
        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSetTimer(
            [In] IntPtr TimerHandle,
            [In] ref long DueTime,
            [In] [Optional] TimerApcRoutine TimerApcRoutine,
            [In] [Optional] IntPtr TimerContext,
            [In] bool ResumeTimer,
            [In] [Optional] int Period,
            [Out] [Optional] out bool PreviousState
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSignalAndWaitForSingleObject(
            [In] IntPtr SignalHandle,
            [In] IntPtr WaitHandle,
            [In] bool Alertable,
            [In] [Optional] ref long Timeout
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtStartProfile(
            [In] IntPtr ProfileHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtStopProfile(
            [In] IntPtr ProfileHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSuspendProcess(
            [In] IntPtr ProcessHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtSuspendThread(
            [In] IntPtr ThreadHandle,
            [Out] [Optional] out int PreviousSuspendCount
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtTerminateJobObject(
            [In] IntPtr JobHandle,
            [In] NtStatus ExitStatus
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtTerminateProcess(
            [In] [Optional] IntPtr ProcessHandle,
            [In] NtStatus ExitStatus
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtTerminateThread(
            [In] [Optional] IntPtr ThreadHandle,
            [In] NtStatus ExitStatus
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtTestAlert();

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtUnloadDriver(
            [In] ref UnicodeString DriverPath
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtUnlockVirtualMemory(
            [In] IntPtr ProcessHandle,
            ref IntPtr BaseAddress,
            ref IntPtr RegionSize,
            [In] MemoryFlags MapType
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtUnmapViewOfSection(
            [In] IntPtr ProcessHandle,
            [In] IntPtr BaseAddress
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtWaitForDebugEvent(
            [In] IntPtr DebugObjectHandle,
            [In] bool Alertable,
            [In] [Optional] ref long Timeout,
            [Out] out DbgUiWaitStateChange WaitStateChange
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtWaitForKeyedEvent(
            [In] IntPtr KeyedEventHandle,
            [In] IntPtr KeyValue,
            [In] bool Alertable,
            [In] [Optional] ref long Timeout
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtWaitForMultipleObjects(
            [In] int Count,
            [In] IntPtr[] Handles,
            [In] WaitType WaitType,
            [In] bool Alertable,
            [In] [Optional] ref long Timeout
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtWaitForMultipleObjects32(
            [In] int Count,
            [In] int[] Handles,
            [In] WaitType WaitType,
            [In] bool Alertable,
            [In] [Optional] ref long Timeout
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtWaitForSingleObject(
            [In] IntPtr Handle,
            [In] bool Alertable,
            [In] [Optional] ref long Timeout
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtWaitHighEventPair(
            [In] IntPtr EventPairHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtWaitLowEventPair(
            [In] IntPtr EventPairHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtWriteRequestData(
            [In] IntPtr PortHandle,
            [In] IntPtr Message,
            [In] int DataEntryIndex,
            [In] IntPtr Buffer,
            [In] IntPtr BufferSize,
            [Out] [Optional] out IntPtr ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtWriteVirtualMemory(
            [In] IntPtr ProcessHandle,
            [In] [Optional] IntPtr BaseAddress,
            [In] IntPtr Buffer,
            [In] IntPtr BufferSize,
            [Out] [Optional] out IntPtr ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus NtYieldExecution();

        #endregion

        #region Loader

        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        public static extern NtStatus LdrGetDllHandle(
            [In] [Optional] string DllPath,
            [In] [Optional] ref int DllCharacteristics,
            [In] ref UnicodeString DllName,
            [Out] out IntPtr DllHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus LdrGetProcedureAddress(
            [In] IntPtr DllHandle,
            [In] [Optional] ref AnsiString ProcedureName,
            [In] [Optional] int ProcedureNumber,
            [Out] out IntPtr ProcedureAddress
            );

        [DllImport("ntdll.dll", CharSet = CharSet.Unicode)]
        public static extern NtStatus LdrLoadDll(
            [In] [Optional] string DllPath,
            [In] [Optional] ref int DllCharacteristics,
            [In] ref UnicodeString DllName,
            [Out] out IntPtr DllHandle
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus LdrQueryProcessModuleInformation(
            [In] IntPtr ModuleInformation, // RtlProcessModules*
            [In] int ModuleInformationLength,
            [Out] [Optional] out int ReturnLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus LdrUnloadDll(
            [In] IntPtr DllHandle
            );

        #endregion

        #region Run-Time Library

        #region Access Control

        #region Access Control Entries

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlAddAccessAllowedAce(
            [In] IntPtr Acl,
            [In] int AceRevision,
            [In] int AccessMask,
            [In] IntPtr Sid // Sid*
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlAddAccessAllowedAceEx(
            [In] IntPtr Acl,
            [In] int AceRevision,
            [In] AceFlags AceFlags,
            [In] int AccessMask,
            [In] IntPtr Sid // Sid*
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlAddAccessDeniedAce(
            [In] IntPtr Acl,
            [In] int AceRevision,
            [In] int AccessMask,
            [In] IntPtr Sid // Sid*
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlAddAccessDeniedAceEx(
            [In] IntPtr Acl,
            [In] int AceRevision,
            [In] AceFlags AceFlags,
            [In] int AccessMask,
            [In] IntPtr Sid // Sid*
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlAddAce(
            [In] IntPtr Acl,
            [In] int AceRevision,
            [In] int StartingAceIndex,
            [In] IntPtr AceList, // Ace**
            [In] int AceListLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlAddAuditAccessAce(
            [In] IntPtr Acl,
            [In] int AceRevision,
            [In] int AccessMask,
            [In] IntPtr Sid, // Sid*
            [In] bool AuditSuccess,
            [In] bool AuditFailure
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlAddAuditAccessAceEx(
            [In] IntPtr Acl,
            [In] int AceRevision,
            [In] AceFlags AceFlags,
            [In] int AccessMask,
            [In] IntPtr Sid, // Sid*
            [In] bool AuditSuccess,
            [In] bool AuditFailure
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlAddCompoundAce(
            [In] IntPtr Acl,
            [In] int AceRevision,
            [In] AceType AceType,
            [In] int AccessMask,
            [In] IntPtr ServerSid, // Sid*
            [In] IntPtr ClientSid
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlDeleteAce(
            [In] IntPtr Acl,
            [In] int AceIndex
            );

        [DllImport("ntdll.dll")]
        public static extern bool RtlFirstFreeAce(
            [In] IntPtr Acl,
            [Out] out IntPtr FirstFree
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlGetAce(
            [In] IntPtr Acl,
            [In] int AceIndex,
            [Out] out IntPtr Ace // Ace**
            );

        #endregion

        #region Access Control Lists

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlCreateAcl(
            [In] IntPtr Acl, // Acl*
            [In] int AclLength,
            [In] int AclRevision
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlQueryInformationAcl(
            [In] IntPtr Acl,
            [In] IntPtr AclInformation,
            [In] int AclInformationLength,
            [In] AclInformationClass AclInformationClass
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlQueryInformationAcl(
            [In] IntPtr Acl,
            [Out] out AclRevisionInformation AclInformation,
            [In] int AclInformationLength,
            [In] AclInformationClass AclInformationClass
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlQueryInformationAcl(
            [In] IntPtr Acl,
            [Out] out AclSizeInformation AclInformation,
            [In] int AclInformationLength,
            [In] AclInformationClass AclInformationClass
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlSetInformationAcl(
            [In] IntPtr Acl,
            [In] IntPtr AclInformation,
            [In] int AclInformationLength,
            [In] AclInformationClass AclInformationClass
            );

        [DllImport("ntdll.dll")]
        public static extern bool RtlValidAcl(
            [In] IntPtr Acl
            );

        #endregion

        #region Access Masks

        [DllImport("ntdll.dll")]
        public static extern bool RtlAreAllAccessesGranted(
            [In] int GrantedAccess,
            [In] int DesiredAccess
            );

        [DllImport("ntdll.dll")]
        public static extern bool RtlAreAnyAccessesGranted(
            [In] int GrantedAccess,
            [In] int DesiredAccess
            );

        [DllImport("ntdll.dll")]
        public static extern void RtlMapGenericMask(
            ref int AccessMask,
            [In] ref GenericMapping GenericMapping
            );

        #endregion

        #region Security Descriptors

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlAbsoluteToSelfRelativeSD(
            [In] IntPtr AbsoluteSecurityDescriptor,
            [In] IntPtr SelfRelativeSecurityDescriptor,
            ref int BufferLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlCreateSecurityDescriptor(
            [In] IntPtr SecurityDescriptor, // SecurityDescriptor*
            [In] int Revision
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlCreateSecurityDescriptorRelative(
            [In] IntPtr SecurityDescriptor, // SecurityDescriptorRelative*
            [In] int Revision
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlGetControlSecurityDescriptor(
            [In] IntPtr SecurityDescriptor,
            [Out] out SecurityDescriptorControlFlags Control,
            [Out] out int Revision
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlGetDaclSecurityDescriptor(
            [In] IntPtr SecurityDescriptor,
            [MarshalAs(UnmanagedType.U1)]
            [Out] out bool DaclPresent,
            [Out] out IntPtr Dacl, // Acl**
            [MarshalAs(UnmanagedType.U1)]
            [Out] out bool DaclDefaulted
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlGetGroupSecurityDescriptor(
            [In] IntPtr SecurityDescriptor,
            [Out] out IntPtr Group, // Sid**
            [MarshalAs(UnmanagedType.U1)]
            [Out] out bool GroupDefaulted
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlGetOwnerSecurityDescriptor(
            [In] IntPtr SecurityDescriptor,
            [Out] out IntPtr Owner, // Sid**
            [MarshalAs(UnmanagedType.U1)]
            [Out] out bool OwnerDefaulted
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlGetSaclSecurityDescriptor(
            [In] IntPtr SecurityDescriptor,
            [MarshalAs(UnmanagedType.U1)]
            [Out] out bool SaclPresent,
            [Out] out IntPtr Sacl, // Acl**
            [MarshalAs(UnmanagedType.U1)]
            [Out] out bool SaclDefaulted
            );

        [DllImport("ntdll.dll")]
        public static extern bool RtlGetSecurityDescriptorRMControl(
            [In] IntPtr SecurityDescriptor,
            [Out] out byte RMControl
            );

        [DllImport("ntdll.dll")]
        public static extern int RtlLengthSecurityDescriptor(
            [In] IntPtr SecurityDescriptor
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlMakeSelfRelativeSD(
            [In] IntPtr AbsoluteSecurityDescriptor,
            [In] IntPtr SelfRelativeSecurityDescriptor,
            ref int BufferLength
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlSelfRelativeToAbsoluteSD(
            [In] IntPtr SelfRelativeSecurityDescriptor,
            [In] IntPtr AbsoluteSecurityDescriptor,
            ref int AbsoluteSecurityDescriptorSize,
            [In] IntPtr Dacl, // Acl*
            ref int DaclSize,
            [In] IntPtr Sacl, // Acl*
            ref int SaclSize,
            [In] IntPtr Owner, // Sid*
            ref int OwnerSize,
            [In] IntPtr PrimaryGroup, // Sid*
            ref int PrimaryGroupSize
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlSelfRelativeToAbsoluteSD2(
            [In] IntPtr SelfRelativeSecurityDescriptor,
            ref int BufferSize
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlSetAttributesSecurityDescriptor(
            [In] IntPtr SecurityDescriptor,
            [In] SecurityDescriptorControlFlags Control,
            ref int Revision
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlSetControlSecurityDescriptor(
            [In] IntPtr SecurityDescriptor,
            [In] SecurityDescriptorControlFlags ControlBitsOfInterest,
            [In] SecurityDescriptorControlFlags ControlBitsToSet
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlSetDaclSecurityDescriptor(
            [In] IntPtr SecurityDescriptor,
            [In] bool DaclPresent,
            [In] IntPtr Dacl, // Acl*
            [In] bool DaclDefaulted
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlSetGroupSecurityDescriptor(
            [In] IntPtr SecurityDescriptor,
            [In] IntPtr Group, // Sid*
            [In] bool GroupDefaulted
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlSetOwnerSecurityDescriptor(
            [In] IntPtr SecurityDescriptor,
            [In] IntPtr Owner, // Sid*
            [In] bool OwnerDefaulted
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlSetSaclSecurityDescriptor(
            [In] IntPtr SecurityDescriptor,
            [In] bool SaclPresent,
            [In] IntPtr Sacl, // Acl*
            [In] bool SaclDefaulted
            );

        [DllImport("ntdll.dll")]
        public static extern void RtlSetSecurityDescriptorRMControl(
            [In] IntPtr SecurityDescriptor,
            [In] [Optional] ref byte RMControl
            );

        [DllImport("ntdll.dll")]
        public static extern bool RtlValidRelativeSecurityDescriptor(
            [In] IntPtr SecurityDescriptorInput,
            [In] int SecurityDescriptorLength,
            [In] SecurityInformation RequiredInformation
            );

        [DllImport("ntdll.dll")]
        public static extern bool RtlValidSecurityDescriptor(
            [In] IntPtr SecurityDescriptor
            );

        #endregion

        #endregion

        #region Debugging

        [DllImport("ntdll.dll")]
        // return: RtlDebugInformation*
        public static extern IntPtr RtlCreateQueryDebugBuffer(
            [In] [Optional] int MaximumCommit,
            [In] bool UseEventPair
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlDestroyQueryDebugBuffer(
            [In] IntPtr Buffer // RtlDebugInformation*
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlQueryProcessBackTraceInformation(
            [In] IntPtr Buffer // RtlDebugInformation*
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlQueryProcessDebugInformation(
            [In] IntPtr UniqueProcessId,
            [In] RtlQueryProcessDebugFlags Flags,
            [In] IntPtr Buffer // RtlDebugInformation*
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlQueryProcessHeapInformation(
            [In] IntPtr Buffer // RtlDebugInformation*
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlQueryProcessLockInformation(
            [In] IntPtr Buffer // RtlDebugInformation*
            );

        // Not exported.
        //[DllImport("ntdll.dll")]
        //public static extern NtStatus RtlQueryProcessModuleInformation(
        //    [In] [Optional] IntPtr ProcessHandle,
        //    [In] RtlQueryProcessDebugFlags Flags,
        //    [In] IntPtr Buffer // RtlDebugInformation*
        //    );

        #endregion

        #region Handle Tables

        [DllImport("ntdll.dll")]
        public static extern IntPtr RtlAllocateHandle(
            [In] ref RtlHandleTable HandleTable,
            [Out] [Optional] out int HandleIndex
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlDestroyHandleTable(
            ref RtlHandleTable HandleTable
            );

        [DllImport("ntdll.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool RtlFreeHandle(
            [In] ref RtlHandleTable HandleTable,
            [In] IntPtr Handle
            );

        [DllImport("ntdll.dll")]
        public static extern void RtlInitializeHandleTable(
            [In] int MaximumNumberOfHandles,
            [In] int SizeOfHandleTableEntry,
            [Out] out RtlHandleTable HandleTable
            );

        [DllImport("ntdll.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool RtlIsValidHandle(
            [In] ref RtlHandleTable HandleTable,
            [In] IntPtr Handle
            );

        [DllImport("ntdll.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool RtlIsValidIndexHandle(
            [In] ref RtlHandleTable HandleTable,
            [In] int HandleIndex,
            [Out] out IntPtr Handle
            );

        #endregion

        #region Memory

        [DllImport("ntdll.dll")]
        public static extern void RtlMoveMemory(
            [In] IntPtr Destination,
            [In] IntPtr Source,
            [In] IntPtr Length
            );

        #endregion

        #region Privileges

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlAcquirePrivilege(
            [In] uint[] Privilege,
            [In] int NumPriv,
            [In] RtlAcquirePrivilegeFlags Flags,
            [Out] out IntPtr ReturnedState
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlAdjustPrivilege(
            [In] uint Privilege,
            [In] bool Enable,
            [In] bool Client,
            [MarshalAs(UnmanagedType.I1)]
            [Out] bool WasEnabled
            );

        [DllImport("ntdll.dll")]
        public static extern void RtlReleasePrivilege(
            [In] IntPtr StatePointer
            );

        #endregion

        #region Processes and Threads

        [DllImport("ntdll.dll")]
        public static extern void RtlAcquirePebLock();

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlAllocateFromPeb(
            [In] int Size,
            [Out] out IntPtr Block
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlCreateEnvironment(
            [In] bool CloneCurrentEnvironment,
            [Out] out IntPtr Environment
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlCreateProcessParameters(
            [Out] out IntPtr ProcessParameters,
            [In] ref UnicodeString ImagePathName,
            [In] ref UnicodeString DllPath,
            [In] ref UnicodeString CurrentDirectory,
            [In] ref UnicodeString CommandLine,
            [In] IntPtr Environment,
            [In] ref UnicodeString WindowTitle,
            [In] ref UnicodeString DesktopInfo,
            [In] ref UnicodeString ShellInfo,
            [In] ref UnicodeString RuntimeData
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlCreateUserProcess(
            [In] ref UnicodeString NtImagePathName,
            [In] ObjectFlags Attributes,
            [In] ref RtlUserProcessParameters ProcessParameters,
            [In] IntPtr ProcessSecurityDescriptor,
            [In] IntPtr ThreadSecurityDescriptor,
            [In] IntPtr ParentProcess,
            [In] bool InheritHandles,
            [In] IntPtr DebugPort,
            [In] IntPtr ExceptionPort,
            [Out] out RtlUserProcessInformation ProcessInformation
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlCreateUserThread(
            [In] IntPtr Process,
            [In] IntPtr ThreadSecurityDescriptor,
            [In] bool CreateSuspended,
            [In] int StackZeroBits,
            [In] [Optional] IntPtr MaximumStackSize,
            [In] [Optional] IntPtr InitialStackSize,
            [In] IntPtr StartAddress,
            [In] IntPtr Parameter,
            [Out] out IntPtr Thread,
            [Out] out ClientId ClientId
            );

        [DllImport("ntdll.dll")]
        public static extern IntPtr RtlDeNormalizeProcessParameters(
            [In] IntPtr ProcessParameters
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlDestroyEnvironment(
            [In] IntPtr Environment
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlDestroyProcessParameters(
            [In] IntPtr ProcessParameters
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlExitUserProcess(
            [In] NtStatus ExitStatus
            );

        [DllImport("ntdll.dll")]
        public static extern void RtlExitUserThread(
            [In] NtStatus ExitStatus
            );

        [DllImport("ntdll.dll")]
        public static extern void RtlFreeUserThreadStack(
            [In] IntPtr Process,
            [In] IntPtr Thread
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlFreeToPeb(
            [In] IntPtr Block,
            [In] int Size
            );

        [DllImport("ntdll.dll")]
        public static extern void RtlInitializeContext(
            [In] IntPtr Process,
            ref Context Context,
            [In] IntPtr Parameter,
            [In] IntPtr InitialPc,
            [In] IntPtr InitialSp
            );

        [DllImport("ntdll.dll")]
        public static extern IntPtr RtlNormalizeProcessParameters(
            [In] IntPtr ProcessParameters
            );

        [DllImport("ntdll.dll")]
        public static extern int RtlNtStatusToDosError(
            [In] NtStatus Status
            );

        [DllImport("ntdll.dll")]
        public static extern void RtlReleasePebLock();

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlRemoteCall(
            [In] IntPtr Process,
            [In] IntPtr Thread,
            [In] IntPtr CallSite,
            [In] int ArgumentCount,
            [In] IntPtr[] Arguments,
            [In] bool PassContext,
            [In] bool AlreadySuspended
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlSetCurrentEnvironment(
            [In] IntPtr Environment,
            [Out] out IntPtr PreviousEnvironment
            );

        #endregion

        #region Security IDs

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlAllocateAndInitializeSid(
            [In] ref SidIdentifierAuthority IdentifierAuthority,
            [In] int SubAuthorityCount,
            [In] int SubAuthority0,
            [In] int SubAuthority1,
            [In] int SubAuthority2,
            [In] int SubAuthority3,
            [In] int SubAuthority4,
            [In] int SubAuthority5,
            [In] int SubAuthority6,
            [In] int SubAuthority7,
            [Out] out IntPtr Sid
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlConvertSidToUnicodeString(
            ref UnicodeString UnicodeString,
            [In] IntPtr Sid,
            [In] bool AllocateDestinationString
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlCopySid(
            [In] int DestinationSidLength,
            [In] IntPtr DestinationSid,
            [In] IntPtr SourceSid
            );

        [DllImport("ntdll.dll")]  
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool RtlEqualSid(
            [In] IntPtr Sid1,
            [In] IntPtr Sid2
            );

        [DllImport("ntdll.dll")]  
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool RtlEqualPrefixSid(
            [In] IntPtr Sid1,
            [In] IntPtr Sid2
            );

        [DllImport("ntdll.dll")]
        public static extern IntPtr RtlFreeSid(
            [In] IntPtr Sid
            );

        [DllImport("ntdll.dll")]
        public unsafe static extern SidIdentifierAuthority* RtlIdentifierAuthoritySid(
            [In] IntPtr Sid
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlInitializeSid(
            [In] IntPtr Sid,
            [In] ref SidIdentifierAuthority IdentifierAuthority,
            [In] int SubAuthorityCount
            );

        [DllImport("ntdll.dll")]
        public static extern int RtlLengthRequiredSid(
            [In] int SubAuthorityCount
            );

        [DllImport("ntdll.dll")]
        public static extern int RtlLengthSid(
            [In] IntPtr Sid
            );

        [DllImport("ntdll.dll")]
        public unsafe static extern int* RtlSubAuthoritySid(
            [In] IntPtr Sid,
            [In] int SubAuthority
            );

        [DllImport("ntdll.dll")]
        public unsafe static extern byte* RtlSubAuthorityCountSid(
            [In] IntPtr Sid
            );

        [DllImport("ntdll.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool RtlValidSid(
            [In] IntPtr Sid
            );

        #endregion

        #region Strings

        #region ANSI

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlAnsiStringToUnicodeString(
            ref UnicodeString DestinationString,
            [In] ref AnsiString SourceString,
            [In] bool AllocateDestinationString
            );

        [DllImport("ntdll.dll")]
        public static extern void RtlFreeAnsiString(
            [In] ref AnsiString AnsiString
            );

        #endregion

        #region Unicode

        [DllImport("ntdll.dll")]
        public static extern int RtlCompareUnicodeString(
            [In] ref UnicodeString String1,
            [In] ref UnicodeString String2,
            [In] bool CaseInSensitive
            );

        [DllImport("ntdll.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool RtlCreateUnicodeString(
            [Out] out UnicodeString DestinationString,
            [MarshalAs(UnmanagedType.LPWStr)]
            [In] string SourceString
            );

        [DllImport("ntdll.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool RtlCreateUnicodeStringFromAsciiz(
            [Out] out UnicodeString DestinationString,
            [MarshalAs(UnmanagedType.LPStr)]
            [In] string SourceString
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlDuplicateUnicodeString(
            [In] RtlDuplicateUnicodeStringFlags Flags,
            [In] ref UnicodeString StringIn,
            [Out] out UnicodeString StringOut
            );

        [DllImport("ntdll.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool RtlEqualUnicodeString(
            [In] ref UnicodeString String1,
            [In] ref UnicodeString String2,
            [In] bool CaseInSensitive
            );

        [DllImport("ntdll.dll")]
        public static extern void RtlFreeUnicodeString(
            [In] ref UnicodeString UnicodeString
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlHashUnicodeString(
            [In] ref UnicodeString String,
            [In] bool CaseInSensitive,
            [In] HashStringAlgorithm HashAlgorithm,
            [Out] out int HashValue
            );

        [DllImport("ntdll.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool RtlPrefixUnicodeString(
            [In] ref UnicodeString String1,
            [In] ref UnicodeString String2,
            [In] bool CaseInSensitive
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlUnicodeStringToAnsiString(
            ref AnsiString DestinationString,
            [In] ref UnicodeString SourceString,
            [In] bool AllocateDestinationString
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlUpcaseUnicodeStringToAnsiString(
            ref AnsiString DestinationString,
            [In] ref UnicodeString SourceString,
            [In] bool AllocateDestinationString
            );

        [DllImport("ntdll.dll")]
        public static extern NtStatus RtlValidateUnicodeString(
            [In] int Flags,
            [In] ref UnicodeString String
            );

        #endregion

        #endregion

        #endregion
    }
}
