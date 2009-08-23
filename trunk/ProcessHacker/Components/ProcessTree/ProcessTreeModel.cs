﻿/*
 * Process Hacker - 
 *   ITreeModel implementation for the process tree
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

// The event 'event' is never used
#pragma warning disable 0067

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aga.Controls.Tree;
using ProcessHacker.Common;
using ProcessHacker.Native;

namespace ProcessHacker
{
    /// <summary>
    /// The process tree model. None of the methods are thread-safe.
    /// </summary>
    public class ProcessTreeModel : ITreeModel
    {
        private ProcessTree _tree;
        private Dictionary<int, ProcessNode> _processes = new Dictionary<int, ProcessNode>();
        private List<ProcessNode> _roots = new List<ProcessNode>();

        public ProcessTreeModel(ProcessTree tree)
        {
            _tree = tree;
        }

        public void Add(ProcessItem item)
        {
            ProcessNode itemNode = new ProcessNode(item);

            // Find the process' parent and add the process to it if we found it.
            if (item.HasParent && _processes.ContainsKey(item.ParentPid))
            {
                _processes[item.ParentPid].Children.Add(itemNode);
            }
            else
            {
                // The process doesn't have a parent, so add it to the root nodes.
                _roots.Add(itemNode);
            }

            // Add the process to the list of all processes.
            _processes.Add(item.Pid, itemNode);

            // Find this process' children and add them.  
            ProcessNode[] roots = _roots.ToArray();

            foreach (ProcessNode node in roots)
            {
                if (node.ProcessItem.HasParent && node.PPid == item.Pid)
                {
                    _roots.Remove(node);
                    itemNode.Children.Add(node);
                }
            }

            this.StructureChanged(this, new TreePathEventArgs(new TreePath()));
        }

        public void Modify(ProcessItem oldItem, ProcessItem newItem)
        {
            ProcessNode node = _processes[newItem.Pid];

            node.ProcessItem = newItem;

            //if (node.ProcessItem.HasParent && node.PPID != -1)
            //    this.NodesChanged(this, new TreeModelEventArgs(this.GetPath(
            //        _processes.ContainsKey(node.PPID) ? _processes[node.PPID] : null),
            //        new object[] { node }));
        }

        public void Remove(ProcessItem item)
        {
            ProcessNode targetNode = _processes[item.Pid];
            ProcessNode[] targetChildren = null;

            // Dispose of the process node we're removing.
            targetNode.Dispose();

            // Check if the process is a root.
            ProcessNode rootNode = _roots.Find(node => node.Pid == item.Pid);

            if (rootNode != null)
            {
                // Remove the process from the roots and make its children root nodes.
                _roots.Remove(rootNode);
                this.MoveChildrenToRoot(rootNode);
            }
            else
            {
                // The process isn't a root, so we have to search for the process' parent.
                if (targetNode.ProcessItem.HasParent && _processes.ContainsKey(targetNode.PPid))
                {
                    ProcessNode parentNode = _processes[targetNode.PPid];

                    if (parentNode != null)
                    {
                        // Remove the node from its parent and make its children root nodes.
                        parentNode.Children.Remove(targetNode);
                        targetChildren = targetNode.Children.ToArray();
                        this.MoveChildrenToRoot(targetNode);
                    }
                }
            }

            // Remove the process from the process dictionary.
            _processes.Remove(item.Pid);
            this.StructureChanged(this, new TreePathEventArgs(new TreePath()));

            // Expand the children because TreeViewAdv collapses them by default.
            if (targetChildren != null)
            {
                foreach (ProcessNode n in targetChildren)
                {
                    try
                    {
                        _tree.FindTreeNode(n).ExpandAll();
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(ex);
                    }
                }
            }

            _tree.Invalidate();
        }

        public TreePath GetPath(ProcessNode node)
        {
            if (this.GetSortColumn() != "")
                return new TreePath(node);

            if (node == null)
                return TreePath.Empty;
            else
            {
                ProcessNode currentNode = node;
                Stack<ProcessNode> stack = new Stack<ProcessNode>();

                while (true)
                {
                    stack.Push(currentNode);

                    if (currentNode.ProcessItem.HasParent && _processes.ContainsKey(currentNode.PPid))
                    {
                        ProcessNode newNode = _processes[currentNode.PPid];

                        if (newNode == currentNode)
                            break;

                        currentNode = newNode;
                    }
                    else
                    {
                        break;
                    }
                }

                return new TreePath(stack.ToArray());
            }
        }

        public void MoveChildrenToRoot(ProcessNode node)
        {
            ProcessNode[] children = node.Children.ToArray();

            _roots.AddRange(children);
        }

        public Dictionary<int, ProcessNode> Nodes
        {
            get { return _processes; }
        }

        public ProcessNode[] Roots
        {
            get { return _roots.ToArray(); }
        }

        public string GetSortColumn()
        {
            foreach (TreeColumn column in _tree.Tree.Columns)
                if (column.SortOrder != SortOrder.None)
                    return column.Header.ToLower();

            return "";
        }

        public SortOrder GetSortOrder()
        {
            foreach (TreeColumn column in _tree.Tree.Columns)
                if (column.SortOrder != SortOrder.None)
                    return column.SortOrder;

            return SortOrder.None;
        }

        public int ModifySort(int sortResult, SortOrder order)
        {
            if (order == SortOrder.Ascending)
                return sortResult * -1;
            else if (order == SortOrder.Descending)
                return sortResult;
            else
                return 0;
        }

        public System.Collections.IEnumerable GetChildren(TreePath treePath)
        {
            if (this.GetSortColumn() != "")
            {
                List<ProcessNode> nodes = new List<ProcessNode>();
                string sortC = this.GetSortColumn();
                SortOrder sortO = this.GetSortOrder();

                nodes.AddRange(_processes.Values);

                nodes.Sort(new Comparison<ProcessNode>(delegate(ProcessNode n1, ProcessNode n2)
                    {
                        switch (sortC)
                        {
                            case "name":
                                return ModifySort(n1.Name.CompareTo(n2.Name), sortO);
                            case "pid":
                                return ModifySort(n1.Pid.CompareTo(n2.Pid), sortO);
                            case "pvt. memory":
                                return ModifySort(n1.ProcessItem.Process.VirtualMemoryCounters.PrivatePageCount.CompareTo(
                                    n2.ProcessItem.Process.VirtualMemoryCounters.PrivatePageCount), sortO);
                            case "working set":
                                return ModifySort(n1.ProcessItem.Process.VirtualMemoryCounters.WorkingSetSize.CompareTo(
                                    n2.ProcessItem.Process.VirtualMemoryCounters.WorkingSetSize), sortO);
                            case "peak working set":
                                return ModifySort(n1.ProcessItem.Process.VirtualMemoryCounters.PeakWorkingSetSize.CompareTo(
                                    n2.ProcessItem.Process.VirtualMemoryCounters.PeakWorkingSetSize), sortO);
                            case "private ws":
                                return ModifySort(n1.PrivateWorkingSetNumber.CompareTo(n2.PrivateWorkingSetNumber), sortO);
                            case "shared ws":
                                return ModifySort(n1.SharedWorkingSetNumber.CompareTo(n2.SharedWorkingSetNumber), sortO);
                            case "shareable ws":
                                return ModifySort(n1.ShareableWorkingSetNumber.CompareTo(n2.ShareableWorkingSetNumber), sortO);
                            case "virtual size":
                                return ModifySort(n1.ProcessItem.Process.VirtualMemoryCounters.VirtualSize.CompareTo(
                                    n2.ProcessItem.Process.VirtualMemoryCounters.VirtualSize), sortO);
                            case "peak virtual size":
                                return ModifySort(n1.ProcessItem.Process.VirtualMemoryCounters.PeakVirtualSize.CompareTo(
                                    n2.ProcessItem.Process.VirtualMemoryCounters.PeakVirtualSize), sortO);
                            case "pagefile usage":
                                return ModifySort(n1.ProcessItem.Process.VirtualMemoryCounters.PagefileUsage.CompareTo(
                                    n2.ProcessItem.Process.VirtualMemoryCounters.PagefileUsage), sortO);
                            case "peak pagefile usage":
                                return ModifySort(n1.ProcessItem.Process.VirtualMemoryCounters.PeakPagefileUsage.CompareTo(
                                    n2.ProcessItem.Process.VirtualMemoryCounters.PeakPagefileUsage), sortO);
                            case "page faults":
                                return ModifySort(n1.ProcessItem.Process.VirtualMemoryCounters.PageFaultCount.CompareTo(
                                    n2.ProcessItem.Process.VirtualMemoryCounters.PageFaultCount), sortO);
                            case "cpu":
                                return ModifySort(n1.ProcessItem.CpuUsage.CompareTo(n2.ProcessItem.CpuUsage), sortO);
                            case "username":
                                return ModifySort(n1.Username.CompareTo(n2.Username), sortO);
                            case "session id":
                                return ModifySort(n1.ProcessItem.SessionId.CompareTo(n2.ProcessItem.SessionId), sortO);
                            case "priority class":
                            case "base priority":
                                return ModifySort(n1.ProcessItem.Process.BasePriority.CompareTo(
                                    n2.ProcessItem.Process.BasePriority), sortO);
                            case "description":
                                return ModifySort(n1.Description.CompareTo(n2.Description), sortO);
                            case "company":
                                return ModifySort(n1.Company.CompareTo(n2.Company), sortO);
                            case "file name":
                                return ModifySort(n1.FileName.CompareTo(n2.FileName), sortO);
                            case "command line":
                                return ModifySort(n1.CommandLine.CompareTo(n2.CommandLine), sortO);
                            case "threads":
                                return ModifySort(n1.ProcessItem.Process.NumberOfThreads.CompareTo(
                                    n2.ProcessItem.Process.NumberOfThreads), sortO);
                            case "handles":
                                return ModifySort(n1.ProcessItem.Process.HandleCount.CompareTo(
                                    n2.ProcessItem.Process.HandleCount), sortO);
                            case "gdi handles":
                                return ModifySort(n1.GdiHandlesNumber.CompareTo(n2.GdiHandlesNumber), sortO);
                            case "user handles":
                                return ModifySort(n1.UserHandlesNumber.CompareTo(n2.UserHandlesNumber), sortO);
                            case "i/o total":
                                return ModifySort(n1.IoTotalNumber.CompareTo(n2.IoTotalNumber), sortO);
                            case "i/o ro":
                                return ModifySort(n1.IoReadOtherNumber.CompareTo(n2.IoReadOtherNumber), sortO);
                            case "i/o w":
                                return ModifySort(n1.IoWriteNumber.CompareTo(n2.IoWriteNumber), sortO);
                            case "integrity":
                                return ModifySort(n1.IntegrityLevel.CompareTo(n2.IntegrityLevel), sortO);
                            case "i/o priority":
                                return ModifySort(n1.IoPriority.CompareTo(n2.IoPriority), sortO);
                            case "page priority":
                                return ModifySort(n1.PagePriority.CompareTo(n2.PagePriority), sortO);
                            case "start time":
                            case "start time (relative)":
                                return ModifySort(n1.ProcessItem.CreateTime.CompareTo(n2.ProcessItem.CreateTime), sortO);
                            case "total cpu time":
                                return ModifySort((n1.ProcessItem.Process.KernelTime + n1.ProcessItem.Process.UserTime).
                                    CompareTo(n2.ProcessItem.Process.KernelTime + n2.ProcessItem.Process.UserTime), sortO);
                            case "kernel cpu time":
                                return ModifySort(n1.ProcessItem.Process.KernelTime.CompareTo(
                                    n2.ProcessItem.Process.KernelTime), sortO);
                            case "user cpu time":
                                return ModifySort(n1.ProcessItem.Process.UserTime.CompareTo(
                                    n2.ProcessItem.Process.UserTime), sortO);
                            case "verification status":
                                return ModifySort(n1.VerificationStatus.CompareTo(n2.VerificationStatus), sortO);
                            default:
                                return 0;
                        }
                    }));

                return nodes;
            }

            if (treePath.IsEmpty())
                return _roots;
            else
                return (treePath.LastNode as ProcessNode).Children;
        }

        public bool IsLeaf(TreePath treePath)
        {
            // When we're sorting the whole tree is a flat list, so there are no children.
            if (this.GetSortColumn() != "")
                return true;

            if (treePath.IsEmpty())
                return false;
            else
                return (treePath.LastNode as ProcessNode).Children.Count == 0;
        }

        public event EventHandler<TreeModelEventArgs> NodesChanged;

        public event EventHandler<TreeModelEventArgs> NodesInserted;

        public event EventHandler<TreeModelEventArgs> NodesRemoved;

        public event EventHandler<TreePathEventArgs> StructureChanged;

        public void CallStructureChanged(TreePathEventArgs args)
        {
            this.StructureChanged(this, args);
        }
    }
}
