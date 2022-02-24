using System;
using System.IO;
using System.Configuration;

namespace ProSoft.EasySave.Infrastructure.Helpers
{
    public class ConfigHelpers
    {
        public ConfigHelpers()
        {

        }


        public void UpdateSetting(string key, string value)
        {
            string configPath = Path.Combine(Environment.CurrentDirectory, "Prosoft.EasySave.Presentation.exe");
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }

        public string ReadSetting(string key)
        {
            string configPath = Path.Combine(Environment.CurrentDirectory, "Prosoft.EasySave.Presentation.exe");
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection appSettings = (AppSettingsSection)configuration.GetSection("appSettings");
            return appSettings.Settings[key].Value;
        }

    }
}
