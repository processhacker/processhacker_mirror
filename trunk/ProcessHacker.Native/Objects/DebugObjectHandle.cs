﻿/*
 * Process Hacker - 
 *   debug object handle
 * 
 * Copyright (C) 2009 wj32
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

using System;
using System.Collections.Generic;
using System.Text;
using ProcessHacker.Native.Api;
using ProcessHacker.Native.Security;

namespace ProcessHacker.Native.Objects
{
    public class DebugObjectHandle : NativeHandle<DebugObjectAccess>
    {
        public static DebugObjectHandle Create(DebugObjectAccess access, DebugObjectFlags flags)
        {
            return Create(access, null, flags);
        }

        public static DebugObjectHandle Create(DebugObjectAccess access, string name, DebugObjectFlags flags)
        {
            return Create(access, name, 0, null, flags);
        }

        public static DebugObjectHandle Create(DebugObjectAccess access, string name, ObjectFlags objectFlags, DirectoryHandle rootDirectory, DebugObjectFlags flags)
        {
            NtStatus status;
            ObjectAttributes oa = new ObjectAttributes(name, objectFlags, rootDirectory);
            IntPtr handle;

            try
            {
                if ((status = Win32.NtCreateDebugObject(
                    out handle,
                    access,
                    ref oa,
                    flags
                    )) >= NtStatus.Error)
                    Win32.ThrowLastError(status);
            }
            finally
            {
                oa.Dispose();
            }

            return new DebugObjectHandle(handle, true);
        }

        private DebugObjectHandle(IntPtr handle, bool owned)
            : base(handle, owned)
        { }

        public void Continue(ClientId cid, NtStatus continueStatus)
        {
            NtStatus status;

            if ((status = Win32.NtDebugContinue(
                this,
                ref cid,
                continueStatus
                )) > NtStatus.Error)
                Win32.ThrowLastError(status);
        }

        public void SetFlags(DebugObjectFlags flags)
        {
            unsafe
            {
                NtStatus status;
                int retLength;

                if ((status = Win32.NtSetInformationDebugObject(
                    this,
                    DebugObjectInformationClass.DebugObjectFlags,
                    new IntPtr(&flags),
                    sizeof(DebugObjectFlags),
                    out retLength
                    )) >= NtStatus.Error)
                    Win32.ThrowLastError(status);
            }
        }

        public void WaitForDebugEvent(out DbgUiWaitStateChange waitStateChange, bool alertable, long timeout, bool timeoutRelative)
        {
            NtStatus status;
            long realTimeout = timeoutRelative ? -timeout : timeout;

            if ((status = Win32.NtWaitForDebugEvent(
                this,
                alertable,
                ref realTimeout,
                out waitStateChange
                )) >= NtStatus.Error)
                Win32.ThrowLastError(status);
        }
    }
}
