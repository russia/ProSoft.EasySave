using System.Globalization;

namespace ProSoft.EasySave.Application.Interfaces.Services;

public interface IGlobalizationService
{
    string GetString(string key);

    string GetString(string key, CultureInfo cultureInfo);
}