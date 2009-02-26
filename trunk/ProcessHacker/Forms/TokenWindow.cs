﻿/*
 * Process Hacker - 
 *   token properties window
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
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ProcessHacker
{
    public partial class TokenWindow : Form
    {
        TokenProperties _tokenProps;

        public TokenWindow(Win32.IWithToken obj)
        {
            InitializeComponent();

            _tokenProps = new TokenProperties(obj);
            _tokenProps.Dock = DockStyle.Fill;

            panelToken.Controls.Add(_tokenProps);
        }

        public TokenProperties TokenProperties
        {
            get { return _tokenProps; }
        }

        private void TokenWindow_Load(object sender, EventArgs e)
        {
            this.Size = Properties.Settings.Default.TokenWindowSize;
        }

        private void TokenWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _tokenProps.SaveSettings();
            Properties.Settings.Default.TokenWindowSize = this.Size;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
