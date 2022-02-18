﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Prism.Regions;
using ProSoft.EasySave.Presentation.ViewModels;
using ProSoft.EasySave.Presentation.Views;

namespace ProSoft.EasySave.Presentation.Views.PartialViews
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class _HomeView : UserControl
    {
        private readonly IRegionManager _regionManager;

        public _HomeView(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            InitializeComponent();
        }

        
    }
}