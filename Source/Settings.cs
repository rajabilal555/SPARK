using System;

namespace Artificial_Intelligence
{
    public class Settings
    {
        public static string Location= "";
        public static string Name = "";
        public static bool NameSet = false;
        public static bool Temp_Unit_F = false;
        public static int RefreshInterval = 1000;
        private static bool Initialized = false;

        public static void Initialise()
        {
            var Inisettings = new INI_Reader("Data/settings.ini");

            if (Inisettings.KeyExists("Name", "Settings"))
            {
                NameSet = true;
                Name = Inisettings.Read("Name", "Settings");
            }

            Location = Inisettings.Read("Location", "Settings");

            Temp_Unit_F = Functions.ToBoolean(Inisettings.Read("TempunitF", "Settings"));

            RefreshInterval = Convert.ToInt32(Inisettings.Read("RefreshInterval", "Settings"));
            Initialized = true;
        }

        public Settings()
        {
            Initialise();
        }

        public static void Save()
        {
            INI_Reader Inisettings = new INI_Reader("Data/settings.ini");
            if (Initialized)
            {
                Inisettings.DeleteKey("Location", "Settings");
                Inisettings.Write("Location", Location, "Settings");

                Inisettings.DeleteKey("TempunitF", "Settings");
                Inisettings.Write("TempunitF", Temp_Unit_F.ToString(), "Settings");

                Inisettings.DeleteKey("RefreshInterval", "Settings");
                Inisettings.Write("RefreshInterval", RefreshInterval.ToString(), "Settings");

                if (NameSet)
                {
                    Inisettings.DeleteKey("Name", "Settings");
                    Inisettings.Write("Name", Name, "Settings");
                }
            }
            else
            {
                Console.WriteLine("Error");
            }
        }
    }
}
