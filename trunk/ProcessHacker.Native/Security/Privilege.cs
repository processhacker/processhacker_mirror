﻿/*
 * Process Hacker - 
 *   privilege
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
using System.Text;
using ProcessHacker.Common.Objects;
using ProcessHacker.Native.Api;
using ProcessHacker.Native.Objects;

namespace ProcessHacker.Native.Security
{
    /// <summary>
    /// Represents a Windows security privilege.
    /// </summary>
    public sealed class Privilege : BaseObject
    {
        private TokenHandle _tokenHandle;
        private Luid _luid;
        private SePrivilegeAttributes _attributes;
        private string _name;
        private string _displayName;

        public Privilege(string name)
            : this(null, name)
        { }

        public Privilege(TokenHandle tokenHandle, string name)
            : this(tokenHandle, name, 0)
        { }

        public Privilege(TokenHandle tokenHandle, string name, SePrivilegeAttributes attributes)
            : this(tokenHandle, name, null, attributes)
        { }

        public Privilege(Luid luid)
            : this(null, luid)
        { }

        public Privilege(LuidAndAttributes laa)
            : this(null, laa.Luid, laa.Attributes)
        { }

        public Privilege(TokenHandle tokenHandle, Luid luid)
            : this(tokenHandle, luid, 0)
        { }

        public Privilege(Luid luid, SePrivilegeAttributes attributes)
            : this(null, luid, attributes)
        { }

        public Privilege(TokenHandle tokenHandle, Luid luid, SePrivilegeAttributes attributes)
            : this(tokenHandle, null, luid, attributes)
        { }

        private Privilege(TokenHandle tokenHandle, string name, Luid? luid, SePrivilegeAttributes attributes)
            : base(tokenHandle != null)
        {
            _tokenHandle = tokenHandle;

            if (_tokenHandle != null)
                _tokenHandle.Reference();

            _name = name;
            _attributes = attributes;

            if (luid == null)
            {
                if (_name == null)
                    throw new Exception("You must specify either a LUID or a name.");

                if (!Win32.LookupPrivilegeValue(null, _name, out _luid))
                    throw new Exception("Invalid privilege name '" + _name + "'.");
            }
            else
            {
                _luid = luid.Value;
            }
        }

        protected override void DisposeObject(bool disposing)
        {
            if (_tokenHandle != null)
                _tokenHandle.Dereference(disposing);
        }

        public SePrivilegeAttributes Attributes
        {
            get { return _attributes; }
        }

        public bool Disabled
        {
            get
            {
                return (_attributes & SePrivilegeAttributes.Disabled)
                    != SePrivilegeAttributes.Disabled;
            }
            set
            {
                _attributes = SePrivilegeAttributes.Disabled;
            }
        }

        public string DisplayName
        {
            get
            {
                if (_displayName == null)
                {
                    StringBuilder sb = new StringBuilder(0x100);
                    int size = sb.Capacity;
                    int languageId = 0;

                    if (!Win32.LookupPrivilegeDisplayName(null, this.Name, sb, ref size, out languageId))
                    {
                        sb.EnsureCapacity(size);

                        if (!Win32.LookupPrivilegeDisplayName(null, this.Name, sb, ref size, out languageId))
                            Win32.ThrowLastError();
                    }

                    _displayName = sb.ToString();
                }

                return _displayName;
            }
        }

        public bool Enabled
        {
            get
            {
                return ((_attributes & SePrivilegeAttributes.Enabled) 
                    == SePrivilegeAttributes.Enabled) || this.EnabledByDefault && !this.Disabled;
            }
            set
            {
                _attributes = SePrivilegeAttributes.Enabled;
            }
        }

        public bool EnabledByDefault
        {
            get
            {
                return ((_attributes & SePrivilegeAttributes.EnabledByDefault) ==
                    SePrivilegeAttributes.EnabledByDefault) && !this.Disabled;
            }
            set
            {
                _attributes = SePrivilegeAttributes.EnabledByDefault;
            }
        }

        public Luid Luid
        {
            get { return _luid; }
        }

        public string Name
        {
            get
            {
                if (_name == null)
                {
                    StringBuilder sb = new StringBuilder(0x100);
                    int size = sb.Capacity;

                    if (!Win32.LookupPrivilegeName(null, ref _luid, sb, ref size))
                    {
                        sb.EnsureCapacity(size);

                        if (!Win32.LookupPrivilegeName(null, ref _luid, sb, ref size))
                            Win32.ThrowLastError();
                    }

                    _name = sb.ToString();
                }

                return _name;
            }
        }

        public bool Removed
        {
            get
            {
                return (_attributes & SePrivilegeAttributes.Removed) ==
                    SePrivilegeAttributes.Removed;
            }
            set
            {
                _attributes = SePrivilegeAttributes.Removed;
            }
        }

        public bool UsedForAccess
        {
            get
            {
                return (_attributes & SePrivilegeAttributes.UsedForAccess)
                    == SePrivilegeAttributes.UsedForAccess;
            }
            set
            {
                if (value)
                    _attributes |= SePrivilegeAttributes.UsedForAccess;
                else
                    _attributes &= ~SePrivilegeAttributes.UsedForAccess;
            }
        }

        public void Disable()
        {
            if (_tokenHandle == null)
                throw new InvalidOperationException(
                    "Cannot disable the privilege because there is no token associated with the instance.");
            this.Disable(_tokenHandle);
        }

        public void Disable(TokenHandle tokenHandle)
        {
            this.SetState(tokenHandle, SePrivilegeAttributes.Disabled);
        }

        public void Enable()
        {
            if (_tokenHandle == null)
                throw new InvalidOperationException(
                    "Cannot enable the privilege because there is no token associated with the instance.");
            this.Enable(_tokenHandle);
        }

        public void Enable(TokenHandle tokenHandle)
        {
            this.SetState(tokenHandle, SePrivilegeAttributes.Enabled);
        }

        public void Remove()
        {
            if (_tokenHandle == null)
                throw new InvalidOperationException(
                    "Cannot remove the privilege because there is no token associated with the instance.");
            this.Remove(_tokenHandle);
        }

        public void Remove(TokenHandle tokenHandle)
        {
            this.SetState(tokenHandle, SePrivilegeAttributes.Removed);
        }

        private void SetState(TokenHandle tokenHandle, SePrivilegeAttributes attributes)
        {
            _attributes = attributes;
            _tokenHandle.SetPrivilege(_luid, _attributes);
        }

        public LuidAndAttributes ToLuidAndAttributes()
        {
            return new LuidAndAttributes()
            {
                Attributes = _attributes,
                Luid = _luid
            };
        }
    }
}
