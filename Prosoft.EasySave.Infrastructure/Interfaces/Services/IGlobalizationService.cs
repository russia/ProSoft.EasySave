using System.Globalization;

namespace ProSoft.EasySave.Infrastructure.Interfaces.Services
{
    public interface IGlobalizationService
    {
        string GetString(string key);

        string GetString(string key, CultureInfo cultureInfo);
    }
}