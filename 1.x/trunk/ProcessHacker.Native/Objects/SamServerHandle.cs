﻿/*
 * Process Hacker - 
 *   SAM server handle
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
using ProcessHacker.Common;
using ProcessHacker.Native.Api;
using ProcessHacker.Native.Security;

namespace ProcessHacker.Native.Objects
{
    /// <summary>
    /// Represents a handle to a SAM server.
    /// </summary>
    public sealed class SamServerHandle : SamHandle<SamServerAccess>
    {
        private static WeakReference<SamServerHandle> _connectServerHandle;
        private static int _connectServerHandleMisses = 0;

        public static SamServerHandle ConnectServerHandle
        {
            get
            {
                WeakReference<SamServerHandle> weakRef = _connectServerHandle;
                SamServerHandle connectHandle = null;

                if (weakRef != null)
                {
                    connectHandle = weakRef.Target;
                }

                if (connectHandle == null)
                {
                    System.Threading.Interlocked.Increment(ref _connectServerHandleMisses);

                    connectHandle = new SamServerHandle(SamServerAccess.GenericRead | SamServerAccess.GenericExecute);

                    if (connectHandle != IntPtr.Zero)
                        _connectServerHandle = new WeakReference<SamServerHandle>(connectHandle);
                }

                return connectHandle;
            }
        }

        public static int ConnectServerHandleMisses
        {
            get { return _connectServerHandleMisses; }
        }

        public delegate bool EnumDomainsDelegate(string name);

        /// <summary>
        /// Opens the local SAM server.
        /// </summary>
        /// <param name="access">The desired access to the server.</param>
        public SamServerHandle(SamServerAccess access)
            : this(null, access)
        { }

        /// <summary>
        /// Opens a SAM server.
        /// </summary>
        /// <param name="serverName">The name of the server.</param>
        /// <param name="access">The desired access to the server.</param>
        public SamServerHandle(string serverName, SamServerAccess access)
        {
            ObjectAttributes oa = new ObjectAttributes();
            IntPtr handle;

            UnicodeString serverNameStr = new UnicodeString(serverName);

            try
            {
                Win32.SamConnect(serverNameStr, out handle, access, oa).ThrowIf();
            }
            finally
            {
                serverNameStr.Dispose();
            }

            this.Handle = handle;
        }

        public void EnumDomains(EnumDomainsDelegate callback)
        {
            NtStatus status;
            int enumerationContext = 0;
            IntPtr buffer;
            int count;

            while (true)
            {
                status = Win32.SamEnumerateDomainsInSamServer(
                    this,
                    ref enumerationContext,
                    out buffer,
                    0x100,
                    out count
                    );

                if (status >= NtStatus.Error)
                    Win32.Throw(status);

                if (count == 0)
                    break;

                using (var bufferAlloc = new SamMemoryAlloc(buffer))
                {
                    for (int i = 0; i < count; i++)
                    {
                        var data = bufferAlloc.ReadStruct<SamSidEnumeration>(i);

                        if (!callback(data.Name.Read()))
                            return;
                    }
                }
            }
        }

        public string[] GetDomains()
        {
            List<string> domains = new List<string>();

            this.EnumDomains(name =>
            {
                domains.Add(name);
                return true;
            });

            return domains.ToArray();
        }

        public Sid LookupDomain(string name)
        {
            IntPtr domainId;

            UnicodeString nameStr = new UnicodeString(name);

            try
            {
                Win32.SamLookupDomainInSamServer(this, nameStr, out domainId).ThrowIf();
            }
            finally
            {
                nameStr.Dispose();
            }

            using (var domainIdAlloc = new SamMemoryAlloc(domainId))
            {
                return new Sid(domainIdAlloc);
            }
        }

        public void Shutdown()
        {
            Win32.SamShutdownSamServer(this).ThrowIf();
        }
    }
}
