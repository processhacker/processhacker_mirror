﻿/*
 * Process Hacker - 
 *   memory allocation wrapper
 * 
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

#if DEBUG
#define ENABLE_STATISTICS
#endif

using System;
using ProcessHacker.Native.Api;

namespace ProcessHacker.Native
{
    /// <summary>
    /// Represents an unmanaged memory allocation from the heap.
    /// </summary>
    public class MemoryAlloc : MemoryRegion
    {
        private static int _allocatedCount;
        private static int _freedCount;
        private static int _reallocatedCount;

        // A private heap just for the client.
        private static Heap _privateHeap = new Heap(HeapFlags.Class1 | HeapFlags.Growable);
        //private static Heap _processHeap = Heap.GetDefault();

        public static int AllocatedCount
        {
            get { return _allocatedCount; }
        }

        public static new int FreedCount
        {
            get { return _freedCount; }
        }

        public static Heap PrivateHeap
        {
            get { return _privateHeap; }
        }

        public static int ReallocatedCount
        {
            get { return _reallocatedCount; }
        }

        /// <summary>
        /// Creates a new, invalid memory allocation. 
        /// You must set the pointer using the Memory property.
        /// </summary>
        protected MemoryAlloc()
        { }

        public MemoryAlloc(IntPtr memory)
            : this(memory, true)
        { }

        public MemoryAlloc(IntPtr memory, bool owned)
            : this(memory, 0, owned)
        { }

        public MemoryAlloc(IntPtr memory, int size, bool owned)
            : base(memory, size, owned)
        { }

        /// <summary>
        /// Creates a new memory allocation with the specified size.
        /// </summary>
        /// <param name="size">The amount of memory, in bytes, to allocate.</param>
        public MemoryAlloc(int size)
            : this(size, 0)
        { }

        /// <summary>
        /// Creates a new memory allocation with the specified size.
        /// </summary>
        /// <param name="size">The amount of memory, in bytes, to allocate.</param>
        /// <param name="flags">Any flags to use.</param>
        public MemoryAlloc(int size, HeapFlags flags)
        {
            this.Memory = _privateHeap.Allocate(size);
            this.Size = size;

#if ENABLE_STATISTICS
            System.Threading.Interlocked.Increment(ref _allocatedCount);
#endif

            GC.AddMemoryPressure(this.Size);
        }

        protected override void Free()
        {
            _privateHeap.Free(this);

#if ENABLE_STATISTICS
            System.Threading.Interlocked.Increment(ref _freedCount);
#endif
            GC.RemoveMemoryPressure(this.Size);
        }

        /// <summary>
        /// Resizes the memory allocation.
        /// </summary>
        /// <param name="newSize">The new size of the allocation.</param>
        public virtual void Resize(int newSize)
        {
            GC.RemoveMemoryPressure(this.Size);

            this.Memory = _privateHeap.Reallocate(this.Memory, newSize);
            this.Size = newSize;

#if ENABLE_STATISTICS
            System.Threading.Interlocked.Increment(ref _reallocatedCount);
#endif
            GC.AddMemoryPressure(this.Size);
        }

        /// <summary>
        /// Resizes the memory allocation without retaining the contents 
        /// of the allocated memory.
        /// </summary>
        /// <param name="newSize">The new size of the allocation.</param>
        public virtual void ResizeNew(int newSize)
        {
            GC.RemoveMemoryPressure(this.Size);
            _privateHeap.Free(this.Memory);

            this.Memory = _privateHeap.Allocate(newSize);
            this.Size = newSize;
            GC.AddMemoryPressure(this.Size);
        }
    }
}
