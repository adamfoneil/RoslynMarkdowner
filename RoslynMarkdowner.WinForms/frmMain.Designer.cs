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
            this.cbRepository = new System.Windows.Forms.ComboBox();
            this.rbRemoteRepo = new System.Windows.Forms.RadioButton();
            this.rbLocalRepo = new System.Windows.Forms.RadioButton();
            this.cbLocalSolution = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvObjects = new System.Windows.Forms.TreeView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbRepository
            // 
            this.cbRepository.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRepository.FormattingEnabled = true;
            this.cbRepository.Location = new System.Drawing.Point(129, 12);
            this.cbRepository.Name = "cbRepository";
            this.cbRepository.Size = new System.Drawing.Size(408, 21);
            this.cbRepository.TabIndex = 0;
            this.cbRepository.Validated += new System.EventHandler(this.cbRepository_Validated);
            // 
            // rbRemoteRepo
            // 
            this.rbRemoteRepo.AutoSize = true;
            this.rbRemoteRepo.Location = new System.Drawing.Point(12, 12);
            this.rbRemoteRepo.Name = "rbRemoteRepo";
            this.rbRemoteRepo.Size = new System.Drawing.Size(111, 17);
            this.rbRemoteRepo.TabIndex = 1;
            this.rbRemoteRepo.TabStop = true;
            this.rbRemoteRepo.Text = "Repository Url:";
            this.rbRemoteRepo.UseVisualStyleBackColor = true;
            // 
            // rbLocalRepo
            // 
            this.rbLocalRepo.AutoSize = true;
            this.rbLocalRepo.Location = new System.Drawing.Point(12, 40);
            this.rbLocalRepo.Name = "rbLocalRepo";
            this.rbLocalRepo.Size = new System.Drawing.Size(109, 17);
            this.rbLocalRepo.TabIndex = 2;
            this.rbLocalRepo.TabStop = true;
            this.rbLocalRepo.Text = "Local Solution:";
            this.rbLocalRepo.UseVisualStyleBackColor = true;
            // 
            // cbLocalSolution
            // 
            this.cbLocalSolution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLocalSolution.FormattingEnabled = true;
            this.cbLocalSolution.Location = new System.Drawing.Point(129, 39);
            this.cbLocalSolution.Name = "cbLocalSolution";
            this.cbLocalSolution.Size = new System.Drawing.Size(408, 21);
            this.cbLocalSolution.TabIndex = 3;
            this.cbLocalSolution.Validated += new System.EventHandler(this.cbLocalSolution_Validated);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbRepository);
            this.panel1.Controls.Add(this.cbLocalSolution);
            this.panel1.Controls.Add(this.rbRemoteRepo);
            this.panel1.Controls.Add(this.rbLocalRepo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(549, 72);
            this.panel1.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 72);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvObjects);
            this.splitContainer1.Size = new System.Drawing.Size(549, 306);
            this.splitContainer1.SplitterDistance = 183;
            this.splitContainer1.TabIndex = 5;
            // 
            // tvObjects
            // 
            this.tvObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvObjects.Location = new System.Drawing.Point(0, 0);
            this.tvObjects.Name = "tvObjects";
            this.tvObjects.Size = new System.Drawing.Size(183, 306);
            this.tvObjects.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 378);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Roslyn Markdowner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbRepository;
        private System.Windows.Forms.RadioButton rbRemoteRepo;
        private System.Windows.Forms.RadioButton rbLocalRepo;
        private System.Windows.Forms.ComboBox cbLocalSolution;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvObjects;
    }
}

