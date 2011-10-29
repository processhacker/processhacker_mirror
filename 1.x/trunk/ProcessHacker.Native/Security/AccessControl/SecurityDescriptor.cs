﻿/*
 * Process Hacker - 
 *   security descriptor
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
using ProcessHacker.Common.Objects;
using ProcessHacker.Native.Api;
using ProcessHacker.Native.Objects;

namespace ProcessHacker.Native.Security.AccessControl
{
    /// <summary>
    /// Represents a security descriptor.
    /// </summary>
    public sealed class SecurityDescriptor : BaseObject
    {
        /// <summary>
        /// Gets the security descriptor of a kernel object.
        /// </summary>
        /// <param name="handle">A handle to a kernel object.</param>
        /// <param name="securityInformation">The information to retrieve.</param>
        /// <returns>A security descriptor.</returns>
        public static SecurityDescriptor GetSecurity(IntPtr handle, SecurityInformation securityInformation)
        {
            using (MemoryAlloc data = new MemoryAlloc(0x100))
            {
                int retLength;

                NtStatus status = Win32.NtQuerySecurityObject(
                    handle,
                    securityInformation,
                    data,
                    data.Size,
                    out retLength
                    );

                if (status == NtStatus.BufferTooSmall)
                {
                    data.ResizeNew(retLength);

                    Win32.NtQuerySecurityObject(
                        handle,
                        securityInformation,
                        data,
                        data.Size,
                        out retLength
                        ).ThrowIf();
                }

                status.ThrowIf();

                return new SecurityDescriptor(data);
            }
        }

        /// <summary>
        /// Gets the security descriptor of an object.
        /// </summary>
        /// <param name="handle">A handle to an object.</param>
        /// <param name="objectType">The type of the object.</param>
        /// <param name="securityInformation">The information to retrieve.</param>
        /// <returns>A security descriptor.</returns>
        public static SecurityDescriptor GetSecurity(IntPtr handle, SeObjectType objectType, SecurityInformation securityInformation)
        {
            Win32Error result;
            IntPtr dummy, securityDescriptor;

            if ((result = Win32.GetSecurityInfo(
                handle,
                objectType,
                securityInformation,
                out dummy, out dummy, out dummy, out dummy,
                out securityDescriptor
                )) != 0)
                Win32.Throw(result);

            return new SecurityDescriptor(new LocalMemoryAlloc(securityDescriptor));
        }

        /// <summary>
        /// Sets the security descriptor of a kernel object.
        /// </summary>
        /// <param name="handle">A handle to a kernel object.</param>
        /// <param name="securityInformation">The information to modify.</param>
        /// <param name="securityDescriptor">The security descriptor.</param>
        public static void SetSecurity(IntPtr handle, SecurityInformation securityInformation, SecurityDescriptor securityDescriptor)
        {
            Win32.NtSetSecurityObject(
                handle,
                securityInformation,
                securityDescriptor
                ).ThrowIf();
        }

        /// <summary>
        /// Sets the security descriptor of an object.
        /// </summary>
        /// <param name="handle">A handle to an object.</param>
        /// <param name="objectType">The type of the object.</param>
        /// <param name="securityInformation">The information to modify.</param>
        /// <param name="securityDescriptor">The security descriptor.</param>
        public static void SetSecurity(IntPtr handle, SeObjectType objectType, SecurityInformation securityInformation, SecurityDescriptor securityDescriptor)
        {
            Win32Error result;
            IntPtr dacl = IntPtr.Zero;
            IntPtr group = IntPtr.Zero;
            IntPtr owner = IntPtr.Zero;
            IntPtr sacl = IntPtr.Zero;

            if (securityInformation.HasFlag(SecurityInformation.Dacl))
                dacl = securityDescriptor.Dacl ?? IntPtr.Zero;
            if (securityInformation.HasFlag(SecurityInformation.Group))
                group = securityDescriptor.Group;
            if (securityInformation.HasFlag(SecurityInformation.Owner))
                owner = securityDescriptor.Owner;
            if (securityInformation.HasFlag(SecurityInformation.Sacl))
                sacl = securityDescriptor.Sacl ?? IntPtr.Zero;

            if ((result = Win32.SetSecurityInfo(
                handle,
                objectType,
                securityInformation,
                owner,
                group,
                dacl,
                sacl
                )) != 0)
                Win32.Throw(result);
        }

        public static implicit operator IntPtr(SecurityDescriptor securityDescriptor)
        {
            return securityDescriptor.Memory;
        }

        private readonly MemoryRegion _memory;
        private Acl _dacl;
        private Acl _sacl;
        private Sid _owner;
        private Sid _group;

        /// <summary>
        /// Creates an empty security descriptor.
        /// </summary>
        public SecurityDescriptor()
        {
            NtStatus status;

            _memory = new MemoryAlloc(Win32.SecurityDescriptorMinLength);

            if ((status = Win32.RtlCreateSecurityDescriptor(
                _memory,
                Win32.SecurityDescriptorRevision
                )).IsError())
            {
                _memory.Dispose();
                _memory = null;
                this.DisableOwnership(false);
                Win32.Throw(status);
            }

            _memory.Reference();
            _memory.Dispose();
        }

        /// <summary>
        /// Creates a security descriptor with the specified components.
        /// </summary>
        /// <param name="owner">A SID representing an owner.</param>
        /// <param name="group">A SID representing a group.</param>
        /// <param name="dacl">The discretionary access control list.</param>
        /// <param name="sacl">The system access control list.</param>
        public SecurityDescriptor(Sid owner, Sid group, Acl dacl, Acl sacl)
            : this()
        {
            this.Owner = owner;
            this.Group = group;
            this.Dacl = dacl;
            this.Sacl = sacl;
        }

        /// <summary>
        /// Creates a security descriptor from memory.
        /// </summary>
        /// <param name="memory">The memory region to use. This object will be referenced.</param>
        public SecurityDescriptor(MemoryRegion memory)
        {
            _memory = memory;
            _memory.Reference();
            this.Read();
        }

        protected override void DisposeObject(bool disposing)
        {
            if (_dacl != null)
                _dacl.Dereference(disposing);
            if (_sacl != null)
                _sacl.Dereference(disposing);
            if (_owner != null)
                _owner.Dereference(disposing);
            if (_group != null)
                _group.Dereference(disposing);
            if (_memory != null)
                _memory.Dereference(disposing);
        }

        /// <summary>
        /// Gets or sets the control flags.
        /// </summary>
        public SecurityDescriptorControlFlags ControlFlags
        {
            get
            {
                SecurityDescriptorControlFlags control;
                int revision;

                Win32.RtlGetControlSecurityDescriptor(
                    this,
                    out control,
                    out revision
                    ).ThrowIf();

                return control;
            }
            set
            {
                Win32.RtlSetControlSecurityDescriptor(
                    this,
                    value,
                    value
                    ).ThrowIf();
            }
        }

        /// <summary>
        /// Gets or sets the DACL.
        /// </summary>
        public Acl Dacl
        {
            get { return _dacl; }
            set
            {
                Win32.RtlSetDaclSecurityDescriptor(
                    this,
                    value != null,
                    value ?? IntPtr.Zero,
                    false
                    ).ThrowIf();

                this.SwapDacl(value);
            }
        }

        /// <summary>
        /// Gets or sets whether the DACL has been defaulted.
        /// </summary>
        public bool DaclDefaulted
        {
            get { return this.ControlFlags.HasFlag(SecurityDescriptorControlFlags.DaclDefaulted); }
            set
            {
                if (value)
                    this.ControlFlags |= SecurityDescriptorControlFlags.DaclDefaulted;
                else
                    this.ControlFlags &= ~SecurityDescriptorControlFlags.DaclDefaulted;
            }
        }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        public Sid Group
        {
            get { return _group; }
            set
            {
                Win32.RtlSetGroupSecurityDescriptor(
                    this,
                    value,
                    false
                    ).ThrowIf();

                this.SwapGroup(value);
            }
        }

        /// <summary>
        /// Gets or sets whether the group has been defaulted.
        /// </summary>
        public bool GroupDefaulted
        {
            get { return this.ControlFlags.HasFlag(SecurityDescriptorControlFlags.GroupDefaulted); }
            set
            {
                if (value)
                    this.ControlFlags |= SecurityDescriptorControlFlags.GroupDefaulted;
                else
                    this.ControlFlags &= ~SecurityDescriptorControlFlags.GroupDefaulted;
            }
        }

        /// <summary>
        /// Gets the size of the security descriptor, in bytes.
        /// </summary>
        public int Length
        {
            get { return Win32.RtlLengthSecurityDescriptor(this); }
        }

        /// <summary>
        /// Gets a pointer to the associated memory of the security descriptor.
        /// </summary>
        public IntPtr Memory
        {
            get { return _memory; }
        }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        public Sid Owner
        {
            get { return _owner; }
            set
            {
                Win32.RtlSetOwnerSecurityDescriptor(
                    this,
                    value,
                    false
                    ).ThrowIf();

                this.SwapOwner(value);
            }
        }

        /// <summary>
        /// Gets or sets whether the owner has been defaulted.
        /// </summary>
        public bool OwnerDefaulted
        {
            get
            {
                return (this.ControlFlags & SecurityDescriptorControlFlags.OwnerDefaulted) ==
                    SecurityDescriptorControlFlags.OwnerDefaulted;
            }
            set
            {
                if (value)
                    this.ControlFlags |= SecurityDescriptorControlFlags.OwnerDefaulted;
                else
                    this.ControlFlags &= ~SecurityDescriptorControlFlags.OwnerDefaulted;
            }
        }

        /// <summary>
        /// Gets or sets the SACL.
        /// </summary>
        public Acl Sacl
        {
            get { return _sacl; }
            set
            {
                Win32.RtlSetSaclSecurityDescriptor(
                    this,
                    value != null,
                    value ?? IntPtr.Zero,
                    false
                    ).ThrowIf();

                this.SwapSacl(value);
            }
        }

        /// <summary>
        /// Gets or sets whether the SACL has been defaulted.
        /// </summary>
        public bool SaclDefaulted
        {
            get { return this.ControlFlags.HasFlag(SecurityDescriptorControlFlags.SaclDefaulted); }
            set
            {
                if (value)
                    this.ControlFlags |= SecurityDescriptorControlFlags.SaclDefaulted;
                else
                    this.ControlFlags &= ~SecurityDescriptorControlFlags.SaclDefaulted;
            }
        }

        /// <summary>
        /// Gets whether the security descriptor is in self-relative form.
        /// </summary>
        public bool SelfRelative
        {
            get { return this.ControlFlags.HasFlag(SecurityDescriptorControlFlags.SelfRelative); }
        }

        /// <summary>
        /// Checks whether the security descriptor grants a set of access rights to a client.
        /// </summary>
        /// <param name="tokenHandle">A handle to a token which represents the client.</param>
        /// <param name="desiredAccess">The access rights requested by the client.</param>
        /// <param name="genericMapping">A structure which defines how generic access rights are to be mapped.</param>
        /// <param name="grantedAccess">A variable which receives the granted access rights.</param>
        /// <returns>Success if access was granted, otherwise another NT status value.</returns>
        public NtStatus CheckAccess(TokenHandle tokenHandle, int desiredAccess, GenericMapping genericMapping, out int grantedAccess)
        {
            NtStatus accessStatus;
            int privilegeSetLength = 0;

            Win32.NtAccessCheck(
                this,
                tokenHandle,
                desiredAccess,
                ref genericMapping,
                IntPtr.Zero,
                ref privilegeSetLength,
                out grantedAccess,
                out accessStatus
                ).ThrowIf();

            return accessStatus;
        }

        /// <summary>
        /// Checks whether the security descriptor is valid.
        /// </summary>
        /// <returns>True if the security descriptor is valid, otherwise false.</returns>
        public bool IsValid
        {
            get { return Win32.RtlValidSecurityDescriptor(this); }
        }

        private void Read()
        {
            bool present, defaulted;
            IntPtr dacl, group, owner, sacl;

            // Read the DACL.
            Win32.RtlGetDaclSecurityDescriptor(
                this,
                out present,
                out dacl,
                out defaulted
                ).ThrowIf();

            if (present && dacl != IntPtr.Zero)
                this.SwapDacl(new Acl(Acl.FromPointer(dacl)));
            else
                this.SwapDacl(null);

            // Read the SACL.
            Win32.RtlGetSaclSecurityDescriptor(
                this,
                out present,
                out sacl,
                out defaulted
                ).ThrowIf();

            if (present && sacl != IntPtr.Zero)
                this.SwapSacl(new Acl(Acl.FromPointer(sacl)));
            else
                this.SwapSacl(null);

            // Read the group.
            Win32.RtlGetGroupSecurityDescriptor(
                this,
                out group,
                out defaulted
                ).ThrowIf();

            if (group != IntPtr.Zero)
                this.SwapGroup(new Sid(group));
            else
                this.SwapGroup(null);

            // Read the owner.
            Win32.RtlGetOwnerSecurityDescriptor(
                this,
                out owner,
                out defaulted
                ).ThrowIf();

            if (owner != IntPtr.Zero)
                this.SwapOwner(new Sid(owner));
            else
                this.SwapOwner(null);
        }

        private void SwapDacl(Acl dacl)
        {
            BaseObject.SwapRef(ref _dacl, dacl);
        }

        private void SwapGroup(Sid group)
        {
            BaseObject.SwapRef(ref _group, group);
        }

        private void SwapOwner(Sid owner)
        {
            BaseObject.SwapRef(ref _owner, owner);
        }

        private void SwapSacl(Acl sacl)
        {
            BaseObject.SwapRef(ref _sacl, sacl);
        }

        /// <summary>
        /// Creates a copy of the security descriptor in self-relative form.
        /// </summary>
        /// <returns>A new self-relative security descriptor.</returns>
        public SecurityDescriptor ToSelfRelative()
        {
            using (MemoryAlloc data = new MemoryAlloc(Win32.SecurityDescriptorMinLength))
            {
                int retLength = data.Size;

                NtStatus status = Win32.RtlMakeSelfRelativeSD(this, data, ref retLength);

                if (status == NtStatus.BufferTooSmall)
                {
                    data.ResizeNew(retLength);
                    status = Win32.RtlMakeSelfRelativeSD(this, data, ref retLength);
                }

                status.ThrowIf();

                return new SecurityDescriptor(data);
            }
        }
    }
}
