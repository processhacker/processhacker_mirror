﻿using System;

namespace ProcessHacker.Common.Objects
{
    public interface IReferenceCountedObject : IDisposable
    {
        /// <summary>
        /// Decrements the reference count of the object.
        /// </summary>
        /// <returns>The new reference count.</returns>
        int Dereference();

        /// <summary>
        /// Decrements the reference count of the object.
        /// </summary>
        /// <param name="managed">Whether to dispose managed resources.</param>
        /// <returns>The new reference count.</returns>
        int Dereference(bool managed);

        /// <summary>
        /// Decreases the reference count of the object.
        /// </summary>
        /// <param name="count">The number of times to dereference the object.</param>
        /// <returns>The new reference count.</returns>
        int Dereference(int count);

        /// <summary>
        /// Decreases the reference count of the object.
        /// </summary>
        /// <param name="count">The number of times to dereference the object.</param>
        /// <param name="managed">Whether to dispose managed resources.</param>
        /// <returns>The new reference count.</returns>
        int Dereference(int count, bool managed);

        /// <summary>
        /// Ensures that the reference counting system has exclusive control 
        /// over the lifetime of the object.
        /// </summary>
        /// <param name="managed">Whether to dispose managed resources.</param>
        void Dispose(bool managed);

        /// <summary>
        /// Increments the reference count of the object.
        /// </summary>
        /// <returns>The new reference count.</returns>
        int Reference();

        /// <summary>
        /// Increases the reference count of the object.
        /// </summary>
        /// <param name="count">The number of times to reference the object.</param>
        /// <returns>The new reference count.</returns>
        int Reference(int count);
    }
}
