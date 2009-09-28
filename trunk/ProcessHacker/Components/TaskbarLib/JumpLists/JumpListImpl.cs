/*
 * Process Hacker - 
 *   ProcessHacker Taskbar Extensions
 * 
 * Copyright (C) 2009 dmex
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
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using TaskbarLib.Interop;
using System.Runtime.InteropServices;
using ProcessHacker.Native.Api;

namespace TaskbarLib
{
    /// <summary>
    /// A collection of categorized jump list destinations.
    /// </summary>
    internal sealed class JumpListDestinations
    {
        //TODO: This is highly inefficient, but we want to maintain
        //insertion order when adding the categories because the bottom
        //categories are first to be truncated if screen estate is low.
        private SortedDictionary<int, List<IJumpListDestination>> _categorizedDestinations =
            new SortedDictionary<int, List<IJumpListDestination>>();

        public void AddDestination(IJumpListDestination destination)
        {
            List<IJumpListDestination> destinations =
                _categorizedDestinations.Values.FirstOrDefault(
                    list => list.First().Category == destination.Category);
            if (destinations == null)
            {
                destinations = new List<IJumpListDestination>();
                _categorizedDestinations.Add(
                    _categorizedDestinations.Keys.LastOrDefault() + 1, destinations);
            }

            destinations.Add(destination);
        }

        public void DeleteDestination(IJumpListDestination destination)
        {
            List<IJumpListDestination> destinations =
                _categorizedDestinations.Values.First(
                    list => list.First().Category == destination.Category);
            IJumpListDestination toDelete = destinations.Find(
                d => d.Path == destination.Path && d.Category == destination.Category && d.Title == destination.Title);
            if (toDelete != null)
                destinations.Remove(toDelete);
        }

        internal void RefreshDestinations(ICustomDestinationList destinationList)
        {
            if (_categorizedDestinations.Count == 0)
                return;

            foreach (int key in _categorizedDestinations.Keys)
            {
                IObjectCollection categoryContents =
                    (IObjectCollection)new CEnumerableObjectCollection();
                var destinations = _categorizedDestinations[key];
                foreach (IJumpListDestination destination in destinations)
                {
                    categoryContents.AddObject(destination.GetShellRepresentation());
                }
                destinationList.AppendCategory(
                    destinations.First().Category,
                    (IObjectArray)categoryContents);
            }
        }

        public IEnumerable<string> Categories
        {
            get
            {
                return
                    (from d in _categorizedDestinations.Keys
                     select _categorizedDestinations[d].First().Category);
            }
        }
        public IEnumerable<IJumpListDestination> GetDestinationsByCategory(
            string category)
        {
            return
                (from k in _categorizedDestinations.Keys
                 let d = _categorizedDestinations[k]
                 where d.First().Category == category
                 select d).Single();
        }

        public void Clear()
        {
            _categorizedDestinations.Clear();
        }
    }

    /// <summary>
    /// A collection of jump list tasks.
    /// </summary>
    internal sealed class JumpListTasks
    {
        private List<IJumpListTask> _tasks = new List<IJumpListTask>();

        public void AddTask(IJumpListTask task)
        {
            _tasks.Add(task);
        }

        public void DeleteTask(IJumpListTask task)
        {
            IJumpListTask toDelete = _tasks.Find(t => t.Path == task.Path && t.Arguments == task.Arguments);
            if (toDelete != null)
                _tasks.Remove(toDelete);
        }

        internal void RefreshTasks(ICustomDestinationList destinationList)
        {
            if (_tasks.Count == 0)
                return;

            IObjectCollection taskCollection = (IObjectCollection)new CEnumerableObjectCollection();
            foreach (IJumpListTask task in _tasks)
            {
                taskCollection.AddObject(task.GetShellRepresentation());
            }
            destinationList.AddUserTasks((IObjectArray)taskCollection);
        }

        public IEnumerable<IJumpListTask> Tasks
        {
            get { return _tasks; }
        }

        public void Clear()
        {
            _tasks.Clear();
        }
    }

    /// <summary>
    /// Represents a shell object that can be inserted to an application's
    /// jump list.
    /// </summary>
    public interface IJumpListShellObject
    {
        /// <summary>
        /// Gets or sets the object's title.
        /// </summary>
        string Title { get; }
        /// <summary>
        /// Gets or sets the object's path.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the shell representation of an object, such as
        /// <b>IShellLink</b> or <b>IShellItem</b>.
        /// </summary>
        /// <returns></returns>
        object GetShellRepresentation();
    }

    /// <summary>
    /// Represents a jump list destination.
    /// </summary>
    public interface IJumpListDestination : IJumpListShellObject
    {
        /// <summary>
        /// Gets or sets the destination's category.
        /// </summary>
        string Category { get; }
    }

    /// <summary>
    /// Represents a jump list task.
    /// </summary>
    public interface IJumpListTask : IJumpListShellObject
    {
        /// <summary>
        /// Gets or sets the task's command line arguments.
        /// </summary>
        string Arguments { get; }
    }

    /// <summary>
    /// Flags controlling the appearance of a window.
    /// </summary>
    public enum WindowShowCommand : uint
    {
        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        Hide = 0,
        /// <summary>
        /// Activates and displays the window (including restoring
        /// it to its original size and position).
        /// </summary>
        Normal = 1,
        /// <summary>
        /// Minimizes the window.
        /// </summary>
        Minimized = 2,
        /// <summary>
        /// Maximizes the window.
        /// </summary>
        Maximized = 3,
        /// <summary>
        /// Similar to <see cref="Normal"/>, except that the window
        /// is not activated.
        /// </summary>
        ShowNoActivate = 4,
        /// <summary>
        /// Activates the window and displays it in its current size
        /// and position.
        /// </summary>
        Show = 5,
        /// <summary>
        /// Minimizes the window and activates the next top-level window.
        /// </summary>
        Minimize = 6,
        /// <summary>
        /// Minimizes the window and does not activate it.
        /// </summary>
        ShowMinimizedNoActivate = 7,
        /// <summary>
        /// Similar to <see cref="Normal"/>, except that the window is not
        /// activated.
        /// </summary>
        ShowNA = 8,
        /// <summary>
        /// Activates and displays the window, restoring it to its original
        /// size and position.
        /// </summary>
        Restore = 9,
        /// <summary>
        /// Sets the show state based on the initial value specified when
        /// the process was created.
        /// </summary>
        Default = 10,
        /// <summary>
        /// Minimizes a window, even if the thread owning the window is not
        /// responding.  Use this only to minimize windows from a different
        /// thread.
        /// </summary>
        ForceMinimize = 11
    }

    /// <summary>
    /// Represents a separator in the task area of the jump list.
    /// There is no need to set any properties on this class.
    /// </summary>
    public sealed class Separator : IJumpListTask
    {
        string IJumpListTask.Arguments
        {
            get { throw new NotImplementedException(); }
        }

        string IJumpListShellObject.Title
        {
            get { throw new NotImplementedException(); }
        }

        string IJumpListShellObject.Path
        {
            get { throw new NotImplementedException(); }
        }

        object IJumpListShellObject.GetShellRepresentation()
        {
            ShellLink shellLink = new ShellLink
            {
                IsSeparator = true
            };
            return shellLink.GetShellRepresentation();
        }
    }

    /// <summary>
    /// Represents a shell link (<b>IShellLink</b>) object.
    /// </summary>
    public sealed class ShellLink : IJumpListTask, IJumpListDestination
    {
        /// <summary>
        /// Gets or sets the object's title.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the object's category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the object's path.
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Gets or sets the location of the object's icon.
        /// </summary>
        public string IconLocation { get; set; }
        /// <summary>
        /// Gets or sets the index of the object's icon in the specified
        /// icon's location (<see cref="IconLocation"/>).
        /// </summary>
        public int IconIndex { get; set; }
        /// <summary>
        /// Gets or sets the object's arguments (passed to the command
        /// line).
        /// </summary>
        public string Arguments { get; set; }
        /// <summary>
        /// Gets or sets the object's working directory.
        /// </summary>
        public string WorkingDirectory { get; set; }

        /// <summary>
        /// Gets or sets the show command of the launched application.
        /// </summary>
        public WindowShowCommand ShowCommand { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating that the shell link
        /// is a menu separator.  If this flag is set, all other
        /// properties are ignored.
        /// </summary>
        internal bool IsSeparator { get; set; }

        /// <summary>
        /// Gets the shell <b>IShellLink</b> representation
        /// of the object.
        /// </summary>
        /// <returns>An <b>IShellLink</b> up-cast to <b>object</b>.</returns>
        public object GetShellRepresentation()
        {
            IShellLinkW shellLink = (IShellLinkW)new CShellLink();

            IPropertyStore propertyStore = (IPropertyStore)shellLink;
            PropVariant propVariant = new PropVariant();

            if (IsSeparator)
            {
                propVariant.SetValue(true);
               
                HResult setValueResult = propertyStore.SetValue(ref PropertyKey.PKEY_AppUserModel_IsDestListSeparator, ref propVariant);
                setValueResult.ThrowIf();
              
                propVariant.Clear();
            }
            else
            {
                HResult setPathResult = shellLink.SetPath(Path);
                setPathResult.ThrowIf();

                if (!String.IsNullOrEmpty(IconLocation))
                {
                    shellLink.SetIconLocation(IconLocation, IconIndex);
                }
                if (!String.IsNullOrEmpty(Arguments))
                {
                    shellLink.SetArguments(Arguments);
                }
                if (!String.IsNullOrEmpty(WorkingDirectory))
                {
                    shellLink.SetWorkingDirectory(WorkingDirectory);
                }

                shellLink.SetShowCmd((uint)ShowCommand);

                propVariant.SetValue(Title);
                propertyStore.SetValue(ref PropertyKey.PKEY_Title, ref propVariant);
                propVariant.Clear();
            }

            propertyStore.Commit();

            return shellLink;
        }
    }

    /// <summary>
    /// Represents a <b>IShellItem</b> object.
    /// </summary>
    public sealed class ShellItem : IJumpListDestination
    {
        string IJumpListShellObject.Title { get { return null; } }

        /// <summary>
        /// Gets or sets the object's category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the object's path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets the shell <b>IShellItem</b> representation
        /// of the object.
        /// </summary>
        /// <returns>An <b>IShellItem</b> up-cast to <b>object</b>.</returns>
        public object GetShellRepresentation()
        {
            return GetShellItemFromPath(Path);

            //Note: this will only work if there is a file association
            //for the extension we're trying to register to our program.
        }

        internal static IShellItem GetShellItemFromPath(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException(
                "path", "Shell item cannot be generated from null or empty path.");

            IShellItem resultItem = default(IShellItem);
            Guid shellItemGuid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE");
            uint result = UnsafeNativeMethods.SHCreateItemFromParsingName(
                path,
                IntPtr.Zero,
                ref shellItemGuid,
                out resultItem);
            //// Throw if an error occurred.
            Marshal.ThrowExceptionForHR((int)result);
            return resultItem;
        }
    }
}