using System;
using System.Diagnostics;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace Artificial_Intelligence
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        public Weather weather;

        private System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

        private SpeechSynthesizer s = new SpeechSynthesizer();
        //Variables
        bool status = true;
        bool sayon = false;
        //---------------

        Choices list = new Choices();

        SpeechRecognitionEngine rec = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadWeather();

            list.Add(new String[] { "hello", "hey", "how are you", "what time is it", "open internet",
                "wake up", "get up", "sleep", "whats the weather",
                "whats the temperature", "open calculator" });

            foreach (var item in File.ReadAllLines("Data/dictionary.txt"))
            {
                list.Add(new string[] { item });
            }

            Grammar gr = new Grammar(new GrammarBuilder(list));

            switch (Settings.Location)
            {
                case "Karachi, Pakistan":
                    locationcombo.SelectedIndex = 0;
                    break;
                case "Lahore, Pakistan":
                    locationcombo.SelectedIndex = 1;
                    break;
                case "Islamabad, Pakistan":
                    locationcombo.SelectedIndex = 2;
                    break;
                default:
                    break;
            }
            nametxt.Text = Settings.Name;

            try
            {
                rec.RequestRecognizerUpdate();
                rec.LoadGrammar(gr);
                rec.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(rec_SpeechRecognized);
                rec.SetInputToDefaultAudioDevice();
                rec.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(recognizer_AudioLevelUpdated);
                //rec.SpeechDetected += rec_SpeechDetected;
                rec.SpeechRecognitionRejected += rec_SpeechRecognitionRejected;
            }
            catch (Exception)
            {

                throw;
            }

            s.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.Child);
            s.SetOutputToDefaultAudioDevice();
        }

        private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            //Properties.Settings.Default.Save();
            Settings.Save();
        }

        private void rec_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            voiceprogress.EditValue = 0;
            sayon = false;
            stoplistenbtn.Enabled = false;
            startlistenbtn.Enabled = true;
            pauselistenbtn.Enabled = true;
            resumelistenbtn.Enabled = false;
            statuslblitem.Caption = "Status: Idle";
            say3("Sorry, I didn't catch that.", "Sorry, I didn't catch that.", "Sorry, I didn't catch that.");
        }
        private void rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string r = e.Result.Text;

            voiceprogress.EditValue = 0;

            inputtxt.Text = r;

            getjawab(r);

            sayon = false;
            if (status != false)
            {
                startlistenbtn.Enabled = true;
            }

            statuslblitem.Caption = "Status: Idle";
            stoplistenbtn.Enabled = false;
            resumelistenbtn.Enabled = false;
            pauselistenbtn.Enabled = true;
        }
        void recognizer_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            voiceprogress.EditValue = e.AudioLevel;
        }
        private void refreshweatherbtn_Click(object sender, EventArgs e)
        {
            refreshweatherbtn.Enabled = false;
            LoadWeather();
        }

        private void LoadWeather()
        {
            try
            {
                weather = new Weather();
                //radialMenu1.ShowPopup(new Point(20, 20));
                //weatherstatpic.LoadAsync("http://5.196.67.58/weathericons/w/tick/" + "50n" + ".png");
                weatherstatuspic.LoadAsync("http:" + weather.WeatherData["Icon"]);
                /*
                WebClient wc = new WebClient();
                byte[] bytes = wc.DownloadData("http:" + weather.WeatherData["Icon"]);
                MemoryStream ms = new MemoryStream(bytes);
                Image img = Image.FromStream(ms);

                double zoom = 200 / 100.0;
                Bitmap bmp = new Bitmap(img, Convert.ToInt32(64.0 * zoom), Convert.ToInt32(64.0 * zoom));
                Graphics g = Graphics.FromImage(bmp);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                weatherstatpic.Image = bmp;*/

                conditionlbl.Text = "Condition: " + weather.WeatherData["Condition"];
                citylbl.Text = "City: " + weather.WeatherData["City"];
                if (Settings.Temp_Unit_F)
                {
                    //weathertemplbl.Text = "Temperature: " + Functions.KelvinToFahrenheit(Convert.ToInt32(weather.WeatherData["Temperature"])).ToString() + "°F Hi: " + Functions.KelvinToFahrenheit(Convert.ToDouble(weather.WeatherData["Temp_High"])) + " Lo: " + Functions.KelvinToFahrenheit(Convert.ToDouble(weather.WeatherData["Temp_Low"]));
                    weathertemplbl.Text = "Temperature: " + weather.WeatherData["TemperatureF"] + " °F";
                }
                else
                {
                    //weathertemplbl.Text = "Temperature: " + Functions.KelvinToCelcius(Convert.ToInt32(weather.WeatherData["Temperature"])).ToString() + "°F Hi: " + Functions.KelvinToCelcius(Convert.ToDouble(weather.WeatherData["Temp_High"])) + " Lo: " + Functions.KelvinToCelcius(Convert.ToDouble(weather.WeatherData["Temp_Low"]));
                    weathertemplbl.Text = "Temperature: " + weather.WeatherData["TemperatureC"] + " °C";
                }
                windlbl.Text = "Wind: " + weather.WeatherData["Wind_Speed"] + " mph";

                if (weather.WeatherData["Precipitation"] == "0")
                {
                    precipitationlbl.Text = "Precipitation: No Precipitation";
                }
                else
                {
                    precipitationlbl.Text = "Precipitation: " + weather.WeatherData["Precipitation"] + " In";
                }
                humiditylbl.Text = "Humidity: " + weather.WeatherData["Humidity"] + "%";
                Lastupdatelbl.Text = "Last Server Update: " + weather.WeatherData["LastUpdate"];
            }
            catch (Exception)
            {
                statuslblitem.Caption = "Status: Internet Connection Down...";
                alertControl1.Show(this, "Error!", "Your Internet Connection might be down...", ((System.Drawing.Image)(resources.GetObject("aboutpage.Image"))));
            }

            if (refreshweatherbtn.Enabled == false)
            {
                refreshweatherbtn.Enabled = true;
            }
        }

        private void answerbtn_Click(object sender, EventArgs e)
        {
            var input = inputtxt.Text;
            var output = outputtxt.Text;

            outputtxt.Text = GetCmdanswer(input);
        }

        #region Say
        public void say2(string h, string e)
        {
            Random rnd = new Random();
            int num = rnd.Next(1, 3);

            switch (num)
            {
                case 1:
                    s.SpeakAsync(h); outputtxt.Text = h;
                    break;
                case 2:
                    s.SpeakAsync(e); outputtxt.Text = e;
                    break;
                default:
                    break;
            }
        }

        public void say3(string h, string e, string f)
        {
            Random rnd = new Random();
            int num = rnd.Next(1, 4);

            switch (num)
            {
                case 1:
                    s.SpeakAsync(h); outputtxt.Text = h;
                    break;
                case 2:
                    s.SpeakAsync(e); outputtxt.Text = e;
                    break;
                case 3:
                    s.SpeakAsync(f); outputtxt.Text = f;
                    break;
                default:
                    break;
            }
        }

        public void say(string h)
        {
            outputtxt.Text = h;
            s.SpeakAsync(h);
        }

        #endregion

        #region GetAnswer

        public string GetCmdanswer(string input)
        {
            var MyIni = new INI_Reader("Data/commands.ini");

            var answer = MyIni.Read(input, "Commands");

            if (answer.Contains("%time%"))
            {
                answer = answer.Replace("%time%", DateTime.Now.ToString("h:mm tt"));
            }
            if (answer.Contains("%date%"))
            {
                answer = answer.Replace("%date%", DateTime.Now.ToString("dd/MM/yyyy"));
            }
            if (answer.Contains("%weather%"))
            {
                try
                {
                    answer = answer.Replace("%weather%", weather.WeatherData["Condition"]);
                }
                catch (Exception)
                {
                    answer = "Connection Problem";
                }
            }
            if (answer.Contains("%temperature%"))
            {
                if (Settings.Temp_Unit_F)
                {
                    try
                    {
                        answer = answer.Replace("%temperature%", weather.WeatherData["TemperatureF"]);
                    }
                    catch (Exception)
                    {
                        answer = "Connection Problem";
                    }
                }
                else
                {
                    try
                    {
                        answer = answer.Replace("%temperature%", weather.WeatherData["TemperatureC"]);
                    }
                    catch (Exception)
                    {
                        answer = "Connection Problem";
                    }
                }
            }
            if (answer.Contains("%temperatureF%"))
            {
                try
                {
                    answer = answer.Replace("%temperatureF%", weather.WeatherData["TemperatureF"]);
                }
                catch (Exception)
                {
                    answer = "Connection Problem";
                }
            }
            if (answer.Contains("%temperatureC%"))
            {
                try
                {
                    answer = answer.Replace("%temperatureC%", weather.WeatherData["TemperatureC"]);
                }
                catch (Exception)
                {
                    answer = "Connection Problem";
                }
            }
            if (answer.Contains("%location%"))
            {
                answer = answer.Replace("%location%", Settings.Location);
            }
            if (answer.Contains("%name%") && Settings.NameSet)
            {
                answer = answer.Replace("%name%", Settings.Name);
            }
            else
            {
                answer = "Error: Name not Set";
            }
            return answer;
        }

        public bool GetCmdExists(string input)
        {
            var MyIni = new INI_Reader("Data/commands.ini");

            var answer = MyIni.KeyExists(input, "Commands");

            return answer;
        }

        public string GetCmdquestion(string output)
        {
            var MyIni = new INI_Reader("Data/commands.ini");

            var answer = MyIni.Read(output, "Commands2");

            return answer;
        }


        public void getjawab(string r)
        {
            switch (r)
            {
                case "wake up":
                    status = true;
                    statuslbl.Text = "Awake";
                    stoplistenbtn.Enabled = true;
                    startlistenbtn.Enabled = true;
                    resumelistenbtn.Enabled = false;
                    pauselistenbtn.Enabled = true;
                    inputtxt.Text = "";
                    outputtxt.Text = "";
                    say3("Okay, I'm Here", "Okay, I'm up!", "Yo! I'm Here");
                    break;
                case "get up":
                    status = true;
                    statuslbl.Text = "Awake";
                    stoplistenbtn.Enabled = true;
                    startlistenbtn.Enabled = true;
                    resumelistenbtn.Enabled = false;
                    pauselistenbtn.Enabled = true;
                    inputtxt.Text = "";
                    outputtxt.Text = "";
                    say3("Okay, I'm Here", "Okay, I'm up!", "Yo! I'm Here");
                    break;
                case "sleep":
                    status = false;
                    statuslbl.Text = "Sleep";
                    stoplistenbtn.Enabled = false;
                    resumelistenbtn.Enabled = true;
                    pauselistenbtn.Enabled = false;
                    startlistenbtn.Enabled = false;
                    inputtxt.Text = "";
                    outputtxt.Text = "";
                    say("Fine, Bye bye!");
                    break;
            }
            if (status == true && sayon == true)
            {
                //Commands
                /*
                var commands = new INI_Reader("Data/commands.ini");
                var statement = commands.Read(r, "Commands");//nothing
                var statement2 = commands.Read(statement, "Commands2");//what

                if (statement.Contains("%time%"))
                {
                    statement = DateTime.Now.ToString("h:mm tt");
                }
                if (statement.Contains("%date%"))
                {
                    statement = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (statement.Contains("%weather%"))
                {
                    if (weather.Weatherdatastatus == false)
                    {
                        statement = "Error Reciving data";
                    }
                    else
                    {
                        statement = ("It's " + weather.WeatherData["Condition"] + " outside");
                    }
                }
                if (statement.Contains("%temperature%"))
                {
                    if (Properties.Settings.Default.Temp_Unit_F) statement = ("It's " + weather.WeatherData["TemperatureF"] + " degrees in " + Properties.Settings.Default.Location);
                    else
                    {
                        try
                        {
                            statement = ("It's " + weather.WeatherData["TemperatureC"] + " degrees in " + Properties.Settings.Default.Location);
                        }
                        catch (Exception)
                        {
                            statement = "Connection Problem";
                        }
                    }
                }*/
                //Commands Workaround...

                //humor
                var humor = new INI_Reader("Data/humor.ini");
                var statement1 = humor.Read(r, "Humor");//nothing
                var statement12 = humor.Read(statement1, "Humor2");//what

                //if r-what = what...
                if (GetCmdExists(r))
                {
                    say(GetCmdanswer(r));//say nothing
                }
                else
                {
                    if (r == statement12)
                    {
                        say(statement1);//say nothing
                    }
                    else
                    {
                        switch (r)
                        {
                            case "hello":
                                say("Hey, i'm Spark");
                                break;
                            case "what time is it":
                                say(DateTime.Now.ToString("h:mm tt"));
                                break;
                            case "how are you":
                                say("Fine, what about you?");
                                break;
                            case "open internet":
                                Process.Start("https://www.google.com");
                                say2("Fine", "Okay");
                                break;
                            case "open calculator":
                                Process.Start("calc.exe");
                                say2("Fine", "Okay");
                                break;
                        }
                    }
                }
            }
        }
        #endregion


        private void startlisten_Click(object sender, EventArgs e)
        {
            rec.RecognizeAsync(RecognizeMode.Single);
            sayon = true;
            startlistenbtn.Enabled = false;
            stoplistenbtn.Enabled = true;
            pauselistenbtn.Enabled = false;
            resumelistenbtn.Enabled = false;
            statuslblitem.Caption = "Status: Listening...";
            inputtxt.Text = "";
            outputtxt.Text = "";
        }

        private void stoplisten_Click(object sender, EventArgs e)
        {
            rec.RecognizeAsyncStop();
            sayon = false;
            stoplistenbtn.Enabled = false;
            startlistenbtn.Enabled = true;
            pauselistenbtn.Enabled = true;
            statuslblitem.Caption = "Status: Idle";
            resumelistenbtn.Enabled = true;
        }

        private void pauselisten_Click(object sender, EventArgs e)
        {
            status = false;
            statuslbl.Text = "Sleep";
            startlistenbtn.Enabled = false;
            stoplistenbtn.Enabled = false;
            resumelistenbtn.Enabled = true;
            pauselistenbtn.Enabled = false;
            inputtxt.Text = "";
            outputtxt.Text = "";
            say3("Fine, Bye Bye", "K!, Bye", "Okay, Bye");

        }

        private void resumelisten_Click(object sender, EventArgs e)
        {
            status = true;
            statuslbl.Text = "Awake";
            startlistenbtn.Enabled = true;
            stoplistenbtn.Enabled = false;
            resumelistenbtn.Enabled = false;
            pauselistenbtn.Enabled = true;
            inputtxt.Text = "";
            outputtxt.Text = "";
            say3("Okay, I'm Here", "Okay, I'm up!", "Yo! I'm Here");
        }

        private void locationcombo_EditValueChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(locationcombo.SelectedText))
            {
                Settings.Location = locationcombo.SelectedText;
                LoadWeather();
                Settings.Save();
            }
        }

        private void tempunitradiogroup_EditValueChanged(object sender, EventArgs e)
        {
            Settings.Temp_Unit_F = (bool)tempunitradiogroup.EditValue;
            LoadWeather();
            Settings.Save();
        }

        private void addcommandbtn_Click(object sender, EventArgs e)
        {
            if (!Functions.Settingsopened)
            {
                new AddVoiceCommands().Show();
                Functions.Settingsopened = true;
            }
        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            Settings.Name = nametxt.Text;
            Settings.Save();
        }
    }
}