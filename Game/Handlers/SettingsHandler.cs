using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;
using Viper.Game.Screens;

namespace Viper.Game.Services
{
    public static class SettingsHandler
    {
        private static string _appPath = AppDomain.CurrentDomain.BaseDirectory;

        private static string _settingsIniPath;

        private static FileIniDataParser _parser = new();

        private static IniData _data = new();

        public static void CheckIntegrity()
        {
            _settingsIniPath = Path.Combine(_appPath, "preferences.ini");

            if (File.Exists(_settingsIniPath))
            {
                try
                {
                    _data = _parser.ReadFile(_settingsIniPath);
                }
                catch // File is corrupt.
                {
                    File.Delete(_settingsIniPath);
                    CreateData();
                }
            }
            else
            {
                CreateData();
            }

            void CreateData()
            {
                _data = new();

                if (!_data.Sections.ContainsSection("Global"))
                {
                    _data["Global"]["GameplayScale"] = GameplayScreen.DEFAULT_GM_SCALE.ToString();
                }

                SaveChanges();
            }
        }
        private static void SaveChanges()
        {
            _parser.WriteFile(_settingsIniPath, _data);
        }

        public static void SaveGameplayScale(double newScale)
        {
            CheckIntegrity();

            _data["Global"]["GameplayScale"] = newScale.ToString();

            SaveChanges();
        }

        public static double GetGameplayScale()
        {
            CheckIntegrity();

            string scaleString = GetSetting("Global", "GameplayScale");

            if (double.TryParse(scaleString, out double scale) && scale >= 0)
            {
                return scale;
            }
            else
            {
                _data["Global"]["GameplayScale"] = GameplayScreen.DEFAULT_GM_SCALE.ToString();

                SaveChanges();

                return GameplayScreen.DEFAULT_GM_SCALE;
            }
        }

        static string GetSetting(string section, string key)
        {
            CheckIntegrity();

            return _data[section][key];
        }
    }
}
