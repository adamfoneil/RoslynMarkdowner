namespace RoslynMarkdowner.WinForms
{
    partial class frmMain
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvObjects = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cbAssembly = new System.Windows.Forms.ToolStripComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tbMarkdown = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnCopy = new System.Windows.Forms.ToolStripButton();
            this.btnAnalyzeSolution = new System.Windows.Forms.Button();
            this.cbMSBuildInstance = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvRepos = new System.Windows.Forms.DataGridView();
            this.colRepoUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBranchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocalSolution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbRepo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.llManageRepos = new System.Windows.Forms.LinkLabel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCachedInfo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pbMain = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRepos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvObjects);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer1.Size = new System.Drawing.Size(612, 169);
            this.splitContainer1.SplitterDistance = 204;
            this.splitContainer1.TabIndex = 5;
            // 
            // tvObjects
            // 
            this.tvObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvObjects.Location = new System.Drawing.Point(0, 25);
            this.tvObjects.Name = "tvObjects";
            this.tvObjects.Size = new System.Drawing.Size(204, 144);
            this.tvObjects.TabIndex = 0;
            this.tvObjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvObjects_AfterSelect);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbAssembly});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(204, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cbAssembly
            // 
            this.cbAssembly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAssembly.Name = "cbAssembly";
            this.cbAssembly.Size = new System.Drawing.Size(150, 25);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(404, 144);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbMarkdown);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(396, 118);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Markdown";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tbMarkdown
            // 
            this.tbMarkdown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMarkdown.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbMarkdown.Location = new System.Drawing.Point(3, 3);
            this.tbMarkdown.Multiline = true;
            this.tbMarkdown.Name = "tbMarkdown";
            this.tbMarkdown.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMarkdown.Size = new System.Drawing.Size(390, 112);
            this.tbMarkdown.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.webBrowser1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(396, 118);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Html";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(3, 3);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(390, 112);
            this.webBrowser1.TabIndex = 0;
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCopy});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(404, 25);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // btnCopy
            // 
            this.btnCopy.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
            this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(55, 22);
            this.btnCopy.Text = "Copy";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnAnalyzeSolution
            // 
            this.btnAnalyzeSolution.Location = new System.Drawing.Point(132, 92);
            this.btnAnalyzeSolution.Name = "btnAnalyzeSolution";
            this.btnAnalyzeSolution.Size = new System.Drawing.Size(133, 23);
            this.btnAnalyzeSolution.TabIndex = 4;
            this.btnAnalyzeSolution.Text = "Analyze Solution";
            this.btnAnalyzeSolution.UseVisualStyleBackColor = true;
            this.btnAnalyzeSolution.Click += new System.EventHandler(this.btnAnalyzeSolution_Click);
            // 
            // cbMSBuildInstance
            // 
            this.cbMSBuildInstance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMSBuildInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMSBuildInstance.FormattingEnabled = true;
            this.cbMSBuildInstance.Location = new System.Drawing.Point(132, 12);
            this.cbMSBuildInstance.Name = "cbMSBuildInstance";
            this.cbMSBuildInstance.Size = new System.Drawing.Size(468, 21);
            this.cbMSBuildInstance.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "MS Build Instance:";
            // 
            // dgvRepos
            // 
            this.dgvRepos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRepos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRepoUrl,
            this.colBranchName,
            this.colLocalSolution});
            this.dgvRepos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRepos.Location = new System.Drawing.Point(0, 0);
            this.dgvRepos.Name = "dgvRepos";
            this.dgvRepos.Size = new System.Drawing.Size(612, 80);
            this.dgvRepos.TabIndex = 9;
            // 
            // colRepoUrl
            // 
            this.colRepoUrl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRepoUrl.DataPropertyName = "PublicUrl";
            this.colRepoUrl.HeaderText = "Repo Url";
            this.colRepoUrl.Name = "colRepoUrl";
            // 
            // colBranchName
            // 
            this.colBranchName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colBranchName.DataPropertyName = "BranchName";
            this.colBranchName.HeaderText = "Branch";
            this.colBranchName.Name = "colBranchName";
            this.colBranchName.Width = 72;
            // 
            // colLocalSolution
            // 
            this.colLocalSolution.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLocalSolution.DataPropertyName = "LocalSolution";
            this.colLocalSolution.HeaderText = "Local Solution";
            this.colLocalSolution.Name = "colLocalSolution";
            // 
            // cbRepo
            // 
            this.cbRepo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRepo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRepo.FormattingEnabled = true;
            this.cbRepo.Location = new System.Drawing.Point(132, 39);
            this.cbRepo.Name = "cbRepo";
            this.cbRepo.Size = new System.Drawing.Size(411, 21);
            this.cbRepo.TabIndex = 9;
            this.cbRepo.SelectedIndexChanged += new System.EventHandler(this.cbRepo_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Repository:";
            // 
            // llManageRepos
            // 
            this.llManageRepos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llManageRepos.AutoSize = true;
            this.llManageRepos.Location = new System.Drawing.Point(549, 42);
            this.llManageRepos.Name = "llManageRepos";
            this.llManageRepos.Size = new System.Drawing.Size(51, 13);
            this.llManageRepos.TabIndex = 11;
            this.llManageRepos.TabStop = true;
            this.llManageRepos.Text = "Manage";
            this.llManageRepos.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llManageRepos_LinkClicked);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 125);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.dgvRepos);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer3.Size = new System.Drawing.Size(612, 253);
            this.splitContainer3.SplitterDistance = 80;
            this.splitContainer3.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblCachedInfo);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.pbMain);
            this.panel1.Controls.Add(this.llManageRepos);
            this.panel1.Controls.Add(this.cbMSBuildInstance);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnAnalyzeSolution);
            this.panel1.Controls.Add(this.cbRepo);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(612, 125);
            this.panel1.TabIndex = 12;
            // 
            // lblCachedInfo
            // 
            this.lblCachedInfo.AutoSize = true;
            this.lblCachedInfo.Location = new System.Drawing.Point(131, 69);
            this.lblCachedInfo.Name = "lblCachedInfo";
            this.lblCachedInfo.Size = new System.Drawing.Size(41, 13);
            this.lblCachedInfo.TabIndex = 14;
            this.lblCachedInfo.Text = "label4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Last Analyzed:";
            // 
            // pbMain
            // 
            this.pbMain.Location = new System.Drawing.Point(275, 99);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(100, 10);
            this.pbMain.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbMain.TabIndex = 12;
            this.pbMain.Visible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 378);
            this.Controls.Add(this.splitContainer3);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Roslyn Markdowner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRepos)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvObjects;
        private System.Windows.Forms.Button btnAnalyzeSolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbMSBuildInstance;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox cbAssembly;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnCopy;
        private System.Windows.Forms.DataGridView dgvRepos;
        private System.Windows.Forms.LinkLabel llManageRepos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbRepo;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar pbMain;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRepoUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBranchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocalSolution;
        private System.Windows.Forms.Label lblCachedInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbMarkdown;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}

