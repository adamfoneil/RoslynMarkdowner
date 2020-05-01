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
            this.btnAnalyzeSolution = new System.Windows.Forms.Button();
            this.pbMain = new System.Windows.Forms.ProgressBar();
            this.llSelectLocalRepo = new System.Windows.Forms.LinkLabel();
            this.cbMSBuildInstance = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cbAssembly = new System.Windows.Forms.ToolStripComboBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnCopy = new System.Windows.Forms.ToolStripButton();
            this.dgvRepos = new System.Windows.Forms.DataGridView();
            this.colRepoUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocalSolution = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.cbRepo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.llManageRepos = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRepos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 170);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvObjects);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer1.Size = new System.Drawing.Size(549, 208);
            this.splitContainer1.SplitterDistance = 183;
            this.splitContainer1.TabIndex = 5;
            // 
            // tvObjects
            // 
            this.tvObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvObjects.Location = new System.Drawing.Point(0, 25);
            this.tvObjects.Name = "tvObjects";
            this.tvObjects.Size = new System.Drawing.Size(183, 183);
            this.tvObjects.TabIndex = 0;
            // 
            // btnAnalyzeSolution
            // 
            this.btnAnalyzeSolution.Location = new System.Drawing.Point(132, 72);
            this.btnAnalyzeSolution.Name = "btnAnalyzeSolution";
            this.btnAnalyzeSolution.Size = new System.Drawing.Size(133, 23);
            this.btnAnalyzeSolution.TabIndex = 4;
            this.btnAnalyzeSolution.Text = "Analyze Solution";
            this.btnAnalyzeSolution.UseVisualStyleBackColor = true;
            this.btnAnalyzeSolution.Click += new System.EventHandler(this.btnAnalyzeSolution_Click);
            // 
            // pbMain
            // 
            this.pbMain.Location = new System.Drawing.Point(385, 72);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(102, 11);
            this.pbMain.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbMain.TabIndex = 5;
            this.pbMain.Visible = false;
            // 
            // llSelectLocalRepo
            // 
            this.llSelectLocalRepo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llSelectLocalRepo.AutoSize = true;
            this.llSelectLocalRepo.Location = new System.Drawing.Point(44, 77);
            this.llSelectLocalRepo.Name = "llSelectLocalRepo";
            this.llSelectLocalRepo.Size = new System.Drawing.Size(61, 13);
            this.llSelectLocalRepo.TabIndex = 6;
            this.llSelectLocalRepo.TabStop = true;
            this.llSelectLocalRepo.Text = "Browse...";
            this.llSelectLocalRepo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSelectLocalRepo_LinkClicked);
            // 
            // cbMSBuildInstance
            // 
            this.cbMSBuildInstance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMSBuildInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMSBuildInstance.FormattingEnabled = true;
            this.cbMSBuildInstance.Location = new System.Drawing.Point(132, 13);
            this.cbMSBuildInstance.Name = "cbMSBuildInstance";
            this.cbMSBuildInstance.Size = new System.Drawing.Size(405, 21);
            this.cbMSBuildInstance.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "MS Build Instance:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbAssembly});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(183, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cbAssembly
            // 
            this.cbAssembly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAssembly.Name = "cbAssembly";
            this.cbAssembly.Size = new System.Drawing.Size(150, 25);
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCopy});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(362, 25);
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
            // 
            // dgvRepos
            // 
            this.dgvRepos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRepos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRepoUrl,
            this.colLocalSolution});
            this.dgvRepos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRepos.Location = new System.Drawing.Point(0, 0);
            this.dgvRepos.Name = "dgvRepos";
            this.dgvRepos.Size = new System.Drawing.Size(549, 61);
            this.dgvRepos.TabIndex = 9;
            // 
            // colRepoUrl
            // 
            this.colRepoUrl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRepoUrl.DataPropertyName = "PublicUrl";
            this.colRepoUrl.HeaderText = "Repo Url";
            this.colRepoUrl.Name = "colRepoUrl";
            // 
            // colLocalSolution
            // 
            this.colLocalSolution.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLocalSolution.DataPropertyName = "LocalSolution";
            this.colLocalSolution.HeaderText = "Local Solution";
            this.colLocalSolution.Name = "colLocalSolution";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.llManageRepos);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.cbRepo);
            this.splitContainer2.Panel1.Controls.Add(this.cbMSBuildInstance);
            this.splitContainer2.Panel1.Controls.Add(this.pbMain);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.btnAnalyzeSolution);
            this.splitContainer2.Panel1.Controls.Add(this.llSelectLocalRepo);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dgvRepos);
            this.splitContainer2.Size = new System.Drawing.Size(549, 170);
            this.splitContainer2.SplitterDistance = 105;
            this.splitContainer2.TabIndex = 10;
            // 
            // cbRepo
            // 
            this.cbRepo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRepo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRepo.FormattingEnabled = true;
            this.cbRepo.Location = new System.Drawing.Point(132, 40);
            this.cbRepo.Name = "cbRepo";
            this.cbRepo.Size = new System.Drawing.Size(348, 21);
            this.cbRepo.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Repository:";
            // 
            // llManageRepos
            // 
            this.llManageRepos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llManageRepos.AutoSize = true;
            this.llManageRepos.Location = new System.Drawing.Point(486, 43);
            this.llManageRepos.Name = "llManageRepos";
            this.llManageRepos.Size = new System.Drawing.Size(51, 13);
            this.llManageRepos.TabIndex = 11;
            this.llManageRepos.TabStop = true;
            this.llManageRepos.Text = "Manage";
            this.llManageRepos.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llManageRepos_LinkClicked);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 378);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splitContainer2);
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
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRepos)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvObjects;
        private System.Windows.Forms.ProgressBar pbMain;
        private System.Windows.Forms.Button btnAnalyzeSolution;
        private System.Windows.Forms.LinkLabel llSelectLocalRepo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbMSBuildInstance;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox cbAssembly;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnCopy;
        private System.Windows.Forms.DataGridView dgvRepos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRepoUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocalSolution;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.LinkLabel llManageRepos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbRepo;
    }
}

