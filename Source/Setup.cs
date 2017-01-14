using System;

namespace Artificial_Intelligence
{
    public partial class Setup : DevExpress.XtraEditors.XtraForm
    {
        public Setup()
        {
            InitializeComponent();
        }

        private void donebtn_Click(object sender, EventArgs e)
        {
            Settings.Name = nametxt.Text;
            Settings.NameSet = true;
            Settings.Save();

            this.Hide();
            var Mainform = new MainForm();
            Mainform.Closed += (s, args) => this.Close();
            Mainform.Show();
        }
    }
}