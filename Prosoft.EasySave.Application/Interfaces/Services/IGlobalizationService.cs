using System.Globalization;

namespace ProSoft.EasySave.Application.Interfaces.Services;

public interface IGlobalizationService
{
    /// <summary>
    /// Method allowing to recuperate the OS language to decide which language the program will be using.
    /// </summary>
    /// <param name="key">The string key.</param>
    /// <returns>Returns the string according to the user culture info.</returns>
    /// <seealso cref="GetString"/>
    string GetString(string key);

    string GetString(string key, CultureInfo cultureInfo);
}