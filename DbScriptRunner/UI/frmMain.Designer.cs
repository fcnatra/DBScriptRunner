namespace DbScriptRunner.UI
{
    partial class frmMain
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnClose = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serversToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenServersConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveServersConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.addServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editSelectedServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenScriptConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveScriptConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoadScript = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUnloadScript = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRunScriptsSequentially = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRunScriptsParallel = new System.Windows.Forms.ToolStripMenuItem();
            this.lvDatabases = new System.Windows.Forms.ListView();
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonMoveUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMoveDown = new System.Windows.Forms.ToolStripButton();
            this.toolbarBtnEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemove = new System.Windows.Forms.ToolStripButton();
            this.lblDatabasesTitle = new System.Windows.Forms.Label();
            this.lblScriptsTitle = new System.Windows.Forms.Label();
            this.lvScripts = new System.Windows.Forms.ListView();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(715, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(89, 21);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 642);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(807, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(79, 17);
            this.toolStripStatusLabel1.Text = "Disconnected";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 619);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(807, 23);
            this.panel1.TabIndex = 3;
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.serversToolStripMenuItem,
            this.scriptsToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip2.Size = new System.Drawing.Size(807, 24);
            this.menuStrip2.TabIndex = 5;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(100, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // serversToolStripMenuItem
            // 
            this.serversToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpenServersConfiguration,
            this.menuSaveServersConfiguration,
            this.addServerToolStripMenuItem,
            this.editSelectedServerToolStripMenuItem});
            this.serversToolStripMenuItem.Name = "serversToolStripMenuItem";
            this.serversToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.serversToolStripMenuItem.Text = "&Databases";
            // 
            // menuOpenServersConfiguration
            // 
            this.menuOpenServersConfiguration.Name = "menuOpenServersConfiguration";
            this.menuOpenServersConfiguration.Size = new System.Drawing.Size(231, 22);
            this.menuOpenServersConfiguration.Text = "&Open Database Configuration";
            this.menuOpenServersConfiguration.Click += new System.EventHandler(this.menuOpenServersConfiguration_Click);
            // 
            // menuSaveServersConfiguration
            // 
            this.menuSaveServersConfiguration.Name = "menuSaveServersConfiguration";
            this.menuSaveServersConfiguration.Size = new System.Drawing.Size(231, 22);
            this.menuSaveServersConfiguration.Text = "&Save Database Configuration";
            this.menuSaveServersConfiguration.Click += new System.EventHandler(this.menuSaveServersConfiguration_Click);
            // 
            // addServerToolStripMenuItem
            // 
            this.addServerToolStripMenuItem.Name = "addServerToolStripMenuItem";
            this.addServerToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.addServerToolStripMenuItem.Text = "&Add Database";
            this.addServerToolStripMenuItem.Click += new System.EventHandler(this.addServerToolStripMenuItem_Click);
            // 
            // editSelectedServerToolStripMenuItem
            // 
            this.editSelectedServerToolStripMenuItem.Name = "editSelectedServerToolStripMenuItem";
            this.editSelectedServerToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.editSelectedServerToolStripMenuItem.Text = "&Edit Selected Database";
            this.editSelectedServerToolStripMenuItem.Click += new System.EventHandler(this.editSelectedServerToolStripMenuItem_Click);
            // 
            // scriptsToolStripMenuItem
            // 
            this.scriptsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpenScriptConfiguration,
            this.menuSaveScriptConfiguration,
            this.menuLoadScript,
            this.menuUnloadScript,
            this.menuRunScriptsSequentially,
            this.menuRunScriptsParallel});
            this.scriptsToolStripMenuItem.Name = "scriptsToolStripMenuItem";
            this.scriptsToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.scriptsToolStripMenuItem.Text = "&Scripts";
            // 
            // menuOpenScriptConfiguration
            // 
            this.menuOpenScriptConfiguration.Name = "menuOpenScriptConfiguration";
            this.menuOpenScriptConfiguration.Size = new System.Drawing.Size(309, 22);
            this.menuOpenScriptConfiguration.Text = "&Open Script Configuration";
            this.menuOpenScriptConfiguration.Click += new System.EventHandler(this.menuOpenScriptConfiguration_Click);
            // 
            // menuSaveScriptConfiguration
            // 
            this.menuSaveScriptConfiguration.Name = "menuSaveScriptConfiguration";
            this.menuSaveScriptConfiguration.Size = new System.Drawing.Size(309, 22);
            this.menuSaveScriptConfiguration.Text = "&Save Script Configuration";
            this.menuSaveScriptConfiguration.Click += new System.EventHandler(this.menuSaveScriptConfiguration_Click);
            // 
            // menuLoadScript
            // 
            this.menuLoadScript.Name = "menuLoadScript";
            this.menuLoadScript.Size = new System.Drawing.Size(309, 22);
            this.menuLoadScript.Text = "&Load scripts into this configuration";
            // 
            // menuUnloadScript
            // 
            this.menuUnloadScript.Name = "menuUnloadScript";
            this.menuUnloadScript.Size = new System.Drawing.Size(309, 22);
            this.menuUnloadScript.Text = "&Unload selected scripts";
            // 
            // menuRunScriptsSequentially
            // 
            this.menuRunScriptsSequentially.Name = "menuRunScriptsSequentially";
            this.menuRunScriptsSequentially.Size = new System.Drawing.Size(309, 22);
            this.menuRunScriptsSequentially.Text = "&Run selected scripts on servers - Sequentially";
            // 
            // menuRunScriptsParallel
            // 
            this.menuRunScriptsParallel.Name = "menuRunScriptsParallel";
            this.menuRunScriptsParallel.Size = new System.Drawing.Size(309, 22);
            this.menuRunScriptsParallel.Text = "Run selected scripts on servers - &PARALLEL";
            // 
            // lvDatabases
            // 
            this.lvDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvDatabases.FullRowSelect = true;
            this.lvDatabases.HideSelection = false;
            this.lvDatabases.Location = new System.Drawing.Point(12, 85);
            this.lvDatabases.Name = "lvDatabases";
            this.lvDatabases.Size = new System.Drawing.Size(318, 529);
            this.lvDatabases.TabIndex = 6;
            this.lvDatabases.UseCompatibleStateImageBehavior = false;
            // 
            // toolbar
            // 
            this.toolbar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonMoveUp,
            this.toolStripButtonMoveDown,
            this.toolbarBtnEdit,
            this.toolStripButtonAdd,
            this.toolStripButtonRemove});
            this.toolbar.Location = new System.Drawing.Point(0, 24);
            this.toolbar.Name = "toolbar";
            this.toolbar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolbar.Size = new System.Drawing.Size(807, 25);
            this.toolbar.TabIndex = 7;
            this.toolbar.Text = "toolStrip1";
            // 
            // toolStripButtonMoveUp
            // 
            this.toolStripButtonMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMoveUp.Image")));
            this.toolStripButtonMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMoveUp.Name = "toolStripButtonMoveUp";
            this.toolStripButtonMoveUp.Size = new System.Drawing.Size(73, 22);
            this.toolStripButtonMoveUp.Text = "Move Up";
            this.toolStripButtonMoveUp.ToolTipText = "Move Up";
            this.toolStripButtonMoveUp.Click += new System.EventHandler(this.toolbarMoveUp_Click);
            // 
            // toolStripButtonMoveDown
            // 
            this.toolStripButtonMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMoveDown.Image")));
            this.toolStripButtonMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMoveDown.Name = "toolStripButtonMoveDown";
            this.toolStripButtonMoveDown.Size = new System.Drawing.Size(89, 22);
            this.toolStripButtonMoveDown.Text = "Move Down";
            this.toolStripButtonMoveDown.ToolTipText = "Move Down";
            this.toolStripButtonMoveDown.Click += new System.EventHandler(this.toolbarMoveDown_Click);
            // 
            // toolbarBtnEdit
            // 
            this.toolbarBtnEdit.Image = ((System.Drawing.Image)(resources.GetObject("toolbarBtnEdit.Image")));
            this.toolbarBtnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbarBtnEdit.Name = "toolbarBtnEdit";
            this.toolbarBtnEdit.Size = new System.Drawing.Size(47, 22);
            this.toolbarBtnEdit.Text = "Edit";
            this.toolbarBtnEdit.Click += new System.EventHandler(this.toolbarEdit_Click);
            // 
            // toolStripButtonAdd
            // 
            this.toolStripButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAdd.Image")));
            this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAdd.Name = "toolStripButtonAdd";
            this.toolStripButtonAdd.Size = new System.Drawing.Size(74, 22);
            this.toolStripButtonAdd.Text = "Add New";
            this.toolStripButtonAdd.Click += new System.EventHandler(this.toolbarAddNewItem_Click);
            // 
            // toolStripButtonRemove
            // 
            this.toolStripButtonRemove.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRemove.Image")));
            this.toolStripButtonRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemove.Name = "toolStripButtonRemove";
            this.toolStripButtonRemove.Size = new System.Drawing.Size(67, 22);
            this.toolStripButtonRemove.Text = "Remove";
            this.toolStripButtonRemove.Click += new System.EventHandler(this.toolbarRemove_Click);
            // 
            // lblDatabasesTitle
            // 
            this.lblDatabasesTitle.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblDatabasesTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDatabasesTitle.Location = new System.Drawing.Point(13, 60);
            this.lblDatabasesTitle.Name = "lblDatabasesTitle";
            this.lblDatabasesTitle.Size = new System.Drawing.Size(317, 22);
            this.lblDatabasesTitle.TabIndex = 8;
            this.lblDatabasesTitle.Text = "DATABASES";
            this.lblDatabasesTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblScriptsTitle
            // 
            this.lblScriptsTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScriptsTitle.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblScriptsTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblScriptsTitle.Location = new System.Drawing.Point(337, 60);
            this.lblScriptsTitle.Name = "lblScriptsTitle";
            this.lblScriptsTitle.Size = new System.Drawing.Size(458, 22);
            this.lblScriptsTitle.TabIndex = 10;
            this.lblScriptsTitle.Text = "SCRIPTS";
            this.lblScriptsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvScripts
            // 
            this.lvScripts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvScripts.FullRowSelect = true;
            this.lvScripts.HideSelection = false;
            this.lvScripts.Location = new System.Drawing.Point(336, 85);
            this.lvScripts.Name = "lvScripts";
            this.lvScripts.Size = new System.Drawing.Size(459, 529);
            this.lvScripts.TabIndex = 9;
            this.lvScripts.UseCompatibleStateImageBehavior = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(807, 664);
            this.Controls.Add(this.lblScriptsTitle);
            this.Controls.Add(this.lvScripts);
            this.Controls.Add(this.lblDatabasesTitle);
            this.Controls.Add(this.lvDatabases);
            this.Controls.Add(this.toolbar);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmMain";
            this.Text = "DB SCRIPT RUNNER";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serversToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuOpenServersConfiguration;
        private System.Windows.Forms.ToolStripMenuItem menuSaveServersConfiguration;
        private System.Windows.Forms.ToolStripMenuItem addServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editSelectedServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scriptsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuLoadScript;
        private System.Windows.Forms.ToolStripMenuItem menuRunScriptsSequentially;
        private System.Windows.Forms.ToolStripMenuItem menuRunScriptsParallel;
        private System.Windows.Forms.ToolStripMenuItem menuUnloadScript;
        private System.Windows.Forms.ToolStripMenuItem menuOpenScriptConfiguration;
        private System.Windows.Forms.ToolStripMenuItem menuSaveScriptConfiguration;
        public System.Windows.Forms.ListView lvDatabases;
        private System.Windows.Forms.ToolStrip toolbar;
        private System.Windows.Forms.ToolStripButton toolStripButtonMoveUp;
        private System.Windows.Forms.ToolStripButton toolStripButtonMoveDown;
        private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemove;
        private System.Windows.Forms.ToolStripButton toolbarBtnEdit;
        private System.Windows.Forms.Label lblDatabasesTitle;
        private System.Windows.Forms.Label lblScriptsTitle;
        public System.Windows.Forms.ListView lvScripts;
    }
}

