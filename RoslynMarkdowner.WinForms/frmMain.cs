using JsonSettings.Library;
using RoslynMarkdowner.WinForms.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using WinForms.Library;
using WinForms.Library.Extensions.ComboBoxes;
using WinForms.Library.Models;

namespace RoslynMarkdowner.WinForms
{
    public partial class frmMain : Form
    {
        private Settings _settings;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _settings = SettingsBase.Load<Settings>();
            _settings.Position?.Apply(this);

            cbRepository.Fill(_settings.RepositoryUrls);
            cbLocalSolution.Fill(_settings.LocalSolutions);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _settings.Position = FormPosition.FromForm(this);
            _settings.Save();
        }

        private void cbRepository_Validated(object sender, EventArgs e)
        {
            AddComboBoxItem(cbRepository, _settings.RepositoryUrls);
        }

        private void cbLocalSolution_Validated(object sender, EventArgs e)
        {
            AddComboBoxItem(cbLocalSolution, _settings.LocalSolutions);
        }

        private void AddComboBoxItem(ComboBox comboBox, HashSet<string> collection)
        {
            if (!string.IsNullOrEmpty(comboBox.Text) && collection.Add(comboBox.Text))
            {
                comboBox.Items.Add(comboBox.Text);
            }
            
        }
    }
}
