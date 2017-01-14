using DevExpress.XtraEditors;
using System;
using System.IO;
using System.Windows.Forms;

namespace Artificial_Intelligence
{
    public partial class AddVoiceCommands : XtraForm
    {
        public AddVoiceCommands()
        {
            InitializeComponent();
        }

        private void AddVoiceCommands_FormClosing(object sender, FormClosingEventArgs e)
        {
            Functions.Settingsopened = false;
        }

        private void addstatementbtn_Click(object sender, EventArgs e)
        {
            string inputtext = inputtxt.Text.ToLower();
            string outputtext = outputtxt.Text;


            if (commandtypecombo.SelectedIndex == 1)
            {
                var MyIni = new INI_Reader("Data/humor.ini");
                if (!MyIni.KeyExists(inputtext, "Humor"))
                {
                    MyIni.Write(inputtext+ " ", " " + outputtext, "Humor");
                    MyIni.Write(outputtext + " ", " " + inputtext, "Humor2");

                    File.AppendAllText("Data/dictionary.txt", Environment.NewLine + inputtext);
                }
            }
            else if (commandtypecombo.SelectedIndex == 0)
            {
                var MyIni = new INI_Reader("Data/commands.ini");
                if (!MyIni.KeyExists(inputtext, "Commands"))
                {
                    MyIni.Write(inputtext + " ", " " + outputtext, "Commands");
                    MyIni.Write(outputtext + " ", " " + inputtext, "Commands2");

                    File.AppendAllText("Data/dictionary.txt", Environment.NewLine + inputtext);
                }
            }
        }
    }
}