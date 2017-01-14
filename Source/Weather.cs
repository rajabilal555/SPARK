using System;
using System.Collections.Generic;
using APIXULib;

namespace Artificial_Intelligence
{
    public class Weather
    {
        /*
        private OpenWeatherMapClient client;
        private CurrentWeatherResponse currentWeather;
        */
        public bool Weatherdatastatus = false;//Is Weather Data Recieved ?

        public Dictionary<string, string> WeatherData = new Dictionary<string, string>();

        public Weather()
        {
            Initialize();
        }

        public void Initialize()
        {
            try
            {
                /*
                client = new OpenWeatherMapClient("5e807196957598db4577d683162606a1");
                currentWeather = await client.CurrentWeather.GetByName("Karachi");
                */
                GetWeather();
                Weatherdatastatus = true;
            }
            catch
            {
                Weatherdatastatus = false;
            }
        }
        /*
        public CurrentWeatherResponse GetWeatherData()
        {
            if (Weatherdatastatus == true)
            {
                return currentWeather;
            }
            else
            {
                Weatherdatastatus = false;
                return currentWeather;
            }
        }

        public string GetWeatherData(string input)
        {
            switch (input)
            {
                case "temp":
                    return currentWeather.Temperature.Value.ToString();
                case "high":
                    return currentWeather.Temperature.Max.ToString();
                case "low":
                    return currentWeather.Temperature.Min.ToString();
                case "cond":
                    return currentWeather.Weather.Value.ToString();
                case "code":
                    return currentWeather.Weather.Number.ToString();
                case "icon":
                    return currentWeather.Weather.Icon.ToString();

                default:
                    break;
            }

            //Not reachable :P
            return "Error";
        }*/
        private void GetWeather()
        {
            try
            {
                string key = "0095ea3b4785490db6495239170401";
                //IAPIXUWeatherRepository repo = new APIXUWeatherRepository();
                IRepository repo = new Repository();

                WeatherModel result = repo.GetWeatherData(key, GetBy.CityName, Settings.Location , Days.Three);

                WeatherData.Add("TemperatureC", result.current.temp_c.ToString());
                WeatherData.Add("TemperatureF", result.current.temp_f.ToString());
                WeatherData.Add("Temperature", result.current.temp_f.ToString());

                WeatherData.Add("Condition", result.current.condition.text);
                WeatherData.Add("CondtionCode", result.current.condition.code.ToString());
                WeatherData.Add("LastUpdate", result.current.last_updated);

                WeatherData.Add("Icon", result.current.condition.icon);

                WeatherData.Add("Humidity", result.current.humidity.ToString());
                WeatherData.Add("Pressure", result.current.pressure_in.ToString());
                WeatherData.Add("Precipitation", result.current.precip_mm.ToString());
                //WeatherData.Add("Visibility", result.current.);

                WeatherData.Add("City", result.location.name);
                WeatherData.Add("Country", result.location.country);
                WeatherData.Add("Region", result.location.region);

                WeatherData.Add("Wind_Speed", result.current.wind_mph.ToString());
                WeatherData.Add("Wind_Direction", result.current.wind_degree.ToString());

                if (result.current.condition.icon.Contains("day"))
                {
                    WeatherData.Add("Is_Day", "true");
                }
                else
                {
                    WeatherData.Add("Is_Day", "false");
                }
                //WeatherData.Add("Is_Day", result.current.is_day);

                //WeatherData.Add("Temp_High", channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["high"].Value);
                //WeatherData.Add("Temp_Low", channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["low"].Value);

                Weatherdatastatus = true;
            }
            catch (Exception)
            {
                Weatherdatastatus = false;
                throw;
            }
        }

        /*
        private void GetWeather()
        {
            try
            {
                string query = string.Format("https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='" + Properties.Settings.Default.Location + "')&format=xml&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");

                XmlDocument wData = new XmlDocument();
                wData.Load(query);

                XmlNamespaceManager manager = new XmlNamespaceManager(wData.NameTable);
                manager.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

                XmlNode channel = wData.SelectSingleNode("query").SelectSingleNode("results").SelectSingleNode("channel");
                XmlNodeList nodes = wData.SelectNodes("query/results/channel");
                

                //XmlNodeList forecast = channel.SelectSingleNode("item").SelectNodes("yweather:forecast", manager);

                //for (int i = 0; i < forecast.Count; i++)
                //{
                //    next_day[i] = forecast[i].Attributes["day"].Value;
                //    next_cond[i] = forecast[i].Attributes["code"].Value;
                //    next_condt[i] = forecast[i].Attributes["text"].Value;
                //    next_high[i] = forecast[i].Attributes["high"].Value;
                //    next_low[i] = forecast[i].Attributes["low"].Value;
                //}
                
                WeatherData.Add("Temperature", channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["temp"].Value);
                WeatherData.Add("Condition", channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["text"].Value);
                WeatherData.Add("CondtionCode", channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["code"].Value);
                WeatherData.Add("LastUpdate", channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["date"].Value);

                WeatherData.Add("Icon", channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["code"].Value);

                WeatherData.Add("Humidity", channel.SelectSingleNode("yweather:atmosphere", manager).Attributes["humidity"].Value);
                WeatherData.Add("Pressure", channel.SelectSingleNode("yweather:atmosphere", manager).Attributes["pressure"].Value);
                WeatherData.Add("Rising", channel.SelectSingleNode("yweather:atmosphere", manager).Attributes["rising"].Value);
                WeatherData.Add("Visibility", channel.SelectSingleNode("yweather:atmosphere", manager).Attributes["visibility"].Value);

                WeatherData.Add("City", channel.SelectSingleNode("yweather:location", manager).Attributes["city"].Value);
                WeatherData.Add("Country", channel.SelectSingleNode("yweather:location", manager).Attributes["country"].Value);
                WeatherData.Add("Region", channel.SelectSingleNode("yweather:location", manager).Attributes["region"].Value);

                WeatherData.Add("Wind_Speed", channel.SelectSingleNode("yweather:wind", manager).Attributes["speed"].Value);
                WeatherData.Add("Wind_Direction", channel.SelectSingleNode("yweather:wind", manager).Attributes["direction"].Value);
                WeatherData.Add("Wind_Chill", channel.SelectSingleNode("yweather:wind", manager).Attributes["chill"].Value);

                WeatherData.Add("Temp_High", channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["high"].Value);
                WeatherData.Add("Temp_Low", channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["low"].Value);

                Weatherdatastatus = true;
            }
            catch
            {
                Weatherdatastatus = false;
            }
        }*/
    }
}
