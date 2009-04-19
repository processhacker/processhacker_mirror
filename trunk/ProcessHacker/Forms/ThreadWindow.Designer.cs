﻿namespace ProcessHacker
{
    partial class ThreadWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (_phandle != null)
                _phandle.Dispose();
            if (_thandle != null)
                _thandle.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThreadWindow));
            this.listViewCallStack = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.groupBoxCallStack = new System.Windows.Forms.GroupBox();
            this.fileModule = new ProcessHacker.Components.FileNameBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonWalk = new System.Windows.Forms.Button();
            this.labelThreadUser = new System.Windows.Forms.Label();
            this.vistaMenu = new wyDay.Controls.VistaMenu(this.components);
            this.buttonToken = new System.Windows.Forms.Button();
            this.groupBoxCallStack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vistaMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewCallStack
            // 
            this.listViewCallStack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCallStack.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.listViewCallStack.FullRowSelect = true;
            this.listViewCallStack.HideSelection = false;
            this.listViewCallStack.Location = new System.Drawing.Point(6, 19);
            this.listViewCallStack.Name = "listViewCallStack";
            this.listViewCallStack.ShowItemToolTips = true;
            this.listViewCallStack.Size = new System.Drawing.Size(363, 353);
            this.listViewCallStack.TabIndex = 0;
            this.listViewCallStack.UseCompatibleStateImageBehavior = false;
            this.listViewCallStack.View = System.Windows.Forms.View.Details;
            this.listViewCallStack.SelectedIndexChanged += new System.EventHandler(this.listViewCallStack_SelectedIndexChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Address";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Symbol Name";
            this.columnHeader4.Width = 220;
            // 
            // groupBoxCallStack
            // 
            this.groupBoxCallStack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCallStack.Controls.Add(this.fileModule);
            this.groupBoxCallStack.Controls.Add(this.label1);
            this.groupBoxCallStack.Controls.Add(this.buttonWalk);
            this.groupBoxCallStack.Controls.Add(this.listViewCallStack);
            this.groupBoxCallStack.Location = new System.Drawing.Point(12, 41);
            this.groupBoxCallStack.Name = "groupBoxCallStack";
            this.groupBoxCallStack.Size = new System.Drawing.Size(375, 407);
            this.groupBoxCallStack.TabIndex = 1;
            this.groupBoxCallStack.TabStop = false;
            this.groupBoxCallStack.Text = "Call Stack";
            // 
            // fileModule
            // 
            this.fileModule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fileModule.Location = new System.Drawing.Point(57, 377);
            this.fileModule.Name = "fileModule";
            this.fileModule.ReadOnly = false;
            this.fileModule.Size = new System.Drawing.Size(231, 24);
            this.fileModule.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 383);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Module:";
            // 
            // buttonWalk
            // 
            this.buttonWalk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonWalk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonWalk.Location = new System.Drawing.Point(294, 378);
            this.buttonWalk.Name = "buttonWalk";
            this.buttonWalk.Size = new System.Drawing.Size(75, 23);
            this.buttonWalk.TabIndex = 3;
            this.buttonWalk.Text = "&Refresh";
            this.buttonWalk.UseVisualStyleBackColor = true;
            this.buttonWalk.Click += new System.EventHandler(this.buttonWalk_Click);
            // 
            // labelThreadUser
            // 
            this.labelThreadUser.AutoSize = true;
            this.labelThreadUser.Location = new System.Drawing.Point(93, 17);
            this.labelThreadUser.Name = "labelThreadUser";
            this.labelThreadUser.Size = new System.Drawing.Size(107, 13);
            this.labelThreadUser.TabIndex = 3;
            this.labelThreadUser.Text = "Username: Unknown";
            // 
            // vistaMenu
            // 
            this.vistaMenu.ContainerControl = this;
            // 
            // buttonToken
            // 
            this.buttonToken.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonToken.Location = new System.Drawing.Point(12, 12);
            this.buttonToken.Name = "buttonToken";
            this.buttonToken.Size = new System.Drawing.Size(75, 23);
            this.buttonToken.TabIndex = 4;
            this.buttonToken.Text = "Token...";
            this.buttonToken.UseVisualStyleBackColor = true;
            this.buttonToken.Click += new System.EventHandler(this.buttonToken_Click);
            // 
            // ThreadWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 460);
            this.Controls.Add(this.buttonToken);
            this.Controls.Add(this.labelThreadUser);
            this.Controls.Add(this.groupBoxCallStack);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ThreadWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thread";
            this.Load += new System.EventHandler(this.ThreadWindow_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ThreadWindow_FormClosing);
            this.groupBoxCallStack.ResumeLayout(false);
            this.groupBoxCallStack.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vistaMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private wyDay.Controls.VistaMenu vistaMenu;
        private System.Windows.Forms.GroupBox groupBoxCallStack;
        private System.Windows.Forms.ListView listViewCallStack;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button buttonWalk;
        private System.Windows.Forms.Label labelThreadUser;
        private ProcessHacker.Components.FileNameBox fileModule;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonToken;
    }
}