using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSoft.EasySave.Presentation.ViewModels
{

    internal class ConfigViewModel
    {

        private readonly IRegionManager _regionManager;

        public ConfigViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

    }


}
