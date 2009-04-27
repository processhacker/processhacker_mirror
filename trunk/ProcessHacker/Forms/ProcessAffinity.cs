﻿/*
 * Process Hacker - 
 *   process affinity editor
 * 
 * Copyright (C) 2008 wj32
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
using System.Windows.Forms;
using ProcessHacker.Native.Api;
using ProcessHacker.Native.Objects;
using ProcessHacker.Native.Security;

namespace ProcessHacker
{
    public partial class ProcessAffinity : Form
    {
        private int _pid;

        public ProcessAffinity(int PID)
        {
            InitializeComponent();
            this.AddEscapeToClose();

            _pid = PID;

            try
            {
                using (ProcessHandle phandle = new ProcessHandle(PID, ProcessAccess.QueryInformation))
                {
                    uint systemMask = 0;
                    uint processMask = 0;

                    if (!Win32.GetProcessAffinityMask(phandle, out processMask, out systemMask))
                        Win32.ThrowLastError();

                    for (int i = 0; (systemMask & (1 << i)) != 0; i++)
                    {
                        CheckBox c = new CheckBox();

                        c.Name = "cpu" + i.ToString();
                        c.Text = "CPU " + i.ToString();
                        c.Tag = i;

                        c.FlatStyle = FlatStyle.System;
                        c.Checked = (processMask & (1 << i)) != 0;

                        flowPanel.Controls.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Process Hacker", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();
                return;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            uint newMask = 0;

            for (int i = 0; i < flowPanel.Controls.Count; i++)
            {
                CheckBox c = (CheckBox)flowPanel.Controls["cpu" + i.ToString()];

                newMask |= ((uint)(c.Checked ? 1 : 0) << i);
            }

            try
            {
                using (ProcessHandle phandle = new ProcessHandle(_pid, ProcessAccess.SetInformation))
                {
                    if (!Win32.SetProcessAffinityMask(phandle, newMask))
                        Win32.ThrowLastError();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Process Hacker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
