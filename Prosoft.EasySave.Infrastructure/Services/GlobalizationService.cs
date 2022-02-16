using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using ProSoft.EasySave.Infrastructure.Interfaces.Services;

namespace ProSoft.EasySave.Infrastructure.Services
{
    public class GlobalizationService : IGlobalizationService
    {
        private readonly ResourceManager _manager;

        public GlobalizationService()
        {
            _manager = new ResourceManager("ProSoft.EasySave.Infrastructure.Globalisation.Lang",
                Assembly.GetExecutingAssembly());
        }

        public string GetString(string key)
        {
            return GetString(key, Thread.CurrentThread.CurrentCulture);
        }

        public string GetString(string key, CultureInfo cultureInfo)
        {
            var value = _manager.GetString(key, cultureInfo);

            if (value is not null)
                return value;

            throw new NotImplementedException();
        }
    }
}